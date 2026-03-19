using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Controller
{
    public abstract class ControllerBase : IDisposable
    {
        public delegate void PushControllerMsgEventHandle(
            string controllerName, string msgContent);
        public static event PushControllerMsgEventHandle PushControllerMsg;

        public string Name;
        public object[] Paras;
        private bool _isDisposed;

        private readonly MyTaskScheduler _taskScheduler = new MyTaskScheduler();

        protected ControllerBase(string name)
        {
            Name = name;
        }

        ~ControllerBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposing)
                return;

            // ...
            _isDisposed = true;

            _taskScheduler.SchedulerCancel();

            foreach (var fi in GetType().GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .Where(
                    fi =>
                        fi.FieldType == typeof(MyUdpClient) || fi.FieldType == typeof(Thread) ||
                        fi.FieldType == typeof(VectorDbcEmulator)))
                Dispose(fi.GetValue(this));

            foreach (var p in GetType().GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(MyUdpClient) ||
                            p.PropertyType == typeof(Thread) ||
                            p.PropertyType == typeof(VectorDbcEmulator)))
                Dispose(p.GetValue(this));
        }

        private static void Dispose(object obj)
        {
            if (obj == null)
                return;

            if (obj is MyUdpClient)
            {
                ((MyUdpClient)obj).Dispose();
            }
            else if (obj is Thread)
            {
                if (((Thread)obj).IsAlive)
                {
                    ((Thread)obj).Abort();
                    ((Thread)obj).Join();
                }               
            }
            else if (obj is VectorDbcEmulator)
                ((VectorDbcEmulator)obj).Dispose();
        }

        public object InvokeFuncByName(string methodName, object[] arrObjs)
        {
            var methodInfo = GetType().GetMethod(methodName);
            return methodInfo != null ? methodInfo.Invoke(this, arrObjs) : null;
        }

        protected void OnPushControllerMsg(string msgContent)
        {
            if (PushControllerMsg != null)
                PushControllerMsg(Name, msgContent);
        }

        /// <summary>
        /// 从数据库获取生产日期及生产序列号
        /// </summary>
        /// <param name="isRemoteServer">是否是远程服务器</param>
        /// <param name="productNo">产品编号</param>
        /// <param name="date">返回的生产日期</param>
        /// <param name="serialNo">返回的生产序列号</param>
        /// <returns>是否获取成功</returns>
        protected bool GetDateAndSerialNumber(
            bool isRemoteServer, string productNo, out string date, out string serialNo)
        {
            var sql = isRemoteServer ?
                ConfigurationManager.AppSettings["RemoteConnectionString"] :
                ConfigurationManager.AppSettings["LocalConnectionString"];

            using (var conn = new SqlConnection(sql))
            {
                try
                {
                    var cmd = new SqlCommand("GetCheckNo", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@Productno", productNo);  //给输入参数赋值
                    var parOutputDate = cmd.Parameters.Add("@date", SqlDbType.DateTime);  //定义输出参数 
                    parOutputDate.Direction = ParameterDirection.Output;  //参数类型为Output 
                    var returnSerialNo = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnSerialNo.Direction = ParameterDirection.ReturnValue;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    serialNo = returnSerialNo.Value.ToString();
                    date = parOutputDate.Value.ToString();

                    return true;
                }
                catch (Exception)
                {
                    date = string.Empty;
                    serialNo = string.Empty;

                    return false;
                }
            }
        }

        /// <summary>
        /// 初始化任务
        /// </summary>
        /// <param name="taskInfo"></param>
        protected void SetTimer(MyTaskScheduler.TaskInfo taskInfo)
        {
            _taskScheduler.SetTimer(taskInfo);
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        protected void SchedulerAsync()
        {
            _taskScheduler.SchedulerAsync();
        }

        protected abstract class FieldAttribute : Attribute
        {
            public bool IsReadOnly;
            public string Name;

            protected FieldAttribute(string name, bool isReadOnly)
            {
                Name = name;
                IsReadOnly = isReadOnly;
            }
        }

        protected class ReadOnlyField : FieldAttribute
        {
            public ReadOnlyField(string name) : base(name, true) { }
        }

        protected class ReadWriteField : FieldAttribute
        {
            public ReadWriteField(string name) : base(name, false) { }
        }

        protected class FilePathField : FieldAttribute
        {
            public FilePathField(string name) : base(name, true) { }
        }
    }
}
