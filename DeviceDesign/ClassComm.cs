using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using StateMachine;

namespace DeviceDesign
{
    class ClassComm
    {

        //用于各个模块调用用户信息
        //      public static Model.commUserInfo staticUserLogin;

        public static string FilePathDeviceConfig;

        #region 控制器变更事件通知

        /// <summary>
        /// 控制器名称变更事件参数
        /// </summary>
        public class ControllerNameChangedEventArgs : EventArgs
        {
            public string OldControllerName { get; set; }
            public string NewControllerName { get; set; }
            public string ControllerType { get; set; }

            public ControllerNameChangedEventArgs(string oldName, string newName, string controllerType)
            {
                OldControllerName = oldName;
                NewControllerName = newName;
                ControllerType = controllerType;
            }
        }

        /// <summary>
        /// 控制器名称变更事件通知器
        /// </summary>
        public static event EventHandler<ControllerNameChangedEventArgs> ControllerNameChanged;

        /// <summary>
        /// 触发控制器名称变更事件
        /// </summary>
        public static void OnControllerNameChanged(string oldName, string newName, string controllerType)
        {
            ControllerNameChanged?.Invoke(null, new ControllerNameChangedEventArgs(oldName, newName, controllerType));
        }

        /// <summary>
        /// 更新部件映射表中引用指定控制器的所有记录
        /// </summary>
        public static void UpdatePartsControllerName(string oldControllerName, string newControllerName)
        {
            if (DeviceConfig?.Parts == null || string.IsNullOrEmpty(oldControllerName) || string.IsNullOrEmpty(newControllerName))
                return;

            var partsList = DeviceConfig.Parts.ToList();
            bool hasChanges = false;

            foreach (var part in partsList)
            {
                if (part.ControllerName == oldControllerName)
                {
                    // 更新控制器名称
                    part.ControllerName = newControllerName;

                    // 更新 ControllerField（字段全称格式：控制器名.Field.字段名）
                    if (!string.IsNullOrEmpty(part.ControllerField) && part.ControllerField.StartsWith(oldControllerName + ".Field."))
                    {
                        part.ControllerField = newControllerName + ".Field." +
                            part.ControllerField.Substring((oldControllerName + ".Field.").Length);
                    }

                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                DeviceConfig.Parts = partsList.ToArray();
            }
        }

        /// <summary>
        /// 更新工序参数表中引用指定控制器的所有记录
        /// </summary>
        public static void UpdateParasControllerName(string oldControllerName, string newControllerName)
        {
            if (DeviceConfig?.Paras == null || string.IsNullOrEmpty(oldControllerName) || string.IsNullOrEmpty(newControllerName))
                return;

            var parasList = DeviceConfig.Paras.ToList();
            bool hasChanges = false;

            foreach (var para in parasList)
            {
                if (!string.IsNullOrEmpty(para.ControllerField) && para.ControllerField.StartsWith(oldControllerName + ".Field."))
                {
                    para.ControllerField = newControllerName + ".Field." +
                        para.ControllerField.Substring((oldControllerName + ".Field.").Length);
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                DeviceConfig.Paras = parasList.ToArray();
            }
        }

        #endregion
        public static FieldDisplayName AnalysisDisplayName(string displayname)
        {
            var fieldDisplayName = new FieldDisplayName();

            // [DisplayName("@controlType:LTextBox,@labelString:发布时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
            var listFieldPara = new List<string>(displayname.Split(','));
            const string strcontroltype = "@controlType:";
            const string strlabelstring = "@labelString:";
            const string strsearchstring = "@searchString:";
            const string strdisplayinpanel = "@inPanel:";
            const string strtagstring = "@tipString:";
            const string strindatagrid = "@InDataGrid:";
            const string strindex = "@Index:";

            fieldDisplayName.ControlType = listFieldPara.Find(e => e.StartsWith(strcontroltype)) == null
                ? string.Empty
                : listFieldPara.Find(e => e.StartsWith(strcontroltype)).Substring(strcontroltype.Length);
            fieldDisplayName.SearchString = listFieldPara.Find(e => e.StartsWith(strsearchstring)) == null
                ? string.Empty
                : listFieldPara.Find(e => e.StartsWith(strsearchstring)).Substring(strsearchstring.Length);
            fieldDisplayName.InPanel = listFieldPara.Find(e => e.StartsWith(strdisplayinpanel)) == null
                ? string.Empty
                : listFieldPara.Find(e => e.StartsWith(strdisplayinpanel)).Substring(strdisplayinpanel.Length);
            fieldDisplayName.LabelString = listFieldPara.Find(e => e.StartsWith(strlabelstring)) == null
                ? string.Empty
                : listFieldPara.Find(e => e.StartsWith(strlabelstring)).Substring(strlabelstring.Length);
            fieldDisplayName.TipString = listFieldPara.Find(e => e.StartsWith(strtagstring)) == null
                ? string.Empty
                : listFieldPara.Find(e => e.StartsWith(strtagstring)).Substring(strtagstring.Length);
            fieldDisplayName.InDataGrid = listFieldPara.Find(e => e.StartsWith(strindatagrid)) == null
                ? string.Empty
                : listFieldPara.Find(e => e.StartsWith(strindatagrid)).Substring(strindatagrid.Length);
            fieldDisplayName.Index = listFieldPara.Find(e => e.StartsWith(strindex)) == null
                ? string.Empty
                : listFieldPara.Find(e => e.StartsWith(strindex)).Substring(strindex.Length);

            return fieldDisplayName;
        }

        public static readonly List<string> LstDataTypes = new List<string> { "string", "int", "double", "int[]" };

        #region ControllerDll

        public class ControllerStruct
        {
            public string Name;
            public readonly List<string> LstMethods = new List<string>();
            public readonly List<string> LstFields = new List<string>();
            public readonly List<string> LstProperties = new List<string>();
        }

        public static readonly Assembly DllAsmb = Assembly.LoadFrom("Controller.dll");
        public static readonly List<ControllerStruct> LstControllers = new List<ControllerStruct>();

        public static void GetLstControllers(string controllerPath)
        {
            var asmb = Assembly.LoadFrom(controllerPath);

            var types = asmb.GetTypes().ToList().FindAll(t => t.BaseType != null && t.BaseType.Name == "ControllerBase");

            foreach (var type in types)
            {
                var con = new ControllerStruct { Name = type.Name };
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var field in fields)
                    con.LstFields.Add(
                        field.ToString().Substring(field.ToString().IndexOf(" ", StringComparison.Ordinal) + 1));
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    var para = method.GetParameters().ToList();
                    var strPara = string.Join(",", para);
                    con.LstMethods.Add(method.Name + "(" + strPara + ")");
                }
                var properties =
                    type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var property in properties)
                    con.LstProperties.Add(property.Name);
                LstControllers.Add(con);
            }
        }

        #endregion

        #region UserControlsDll

        public static List<string> LstUserControls = new List<string>();

        public static void GetLstUserControls(string userControlsPath)
        {
            var types =
                Assembly.LoadFrom(userControlsPath)
                    .GetTypes()
                    .ToList()
                    .FindAll(t => t.BaseType != null && t.BaseType.Name == "UserControl");

            LstUserControls = types.Select(t => t.Name).ToList();
        }

        #endregion

        #region StateMachine.DeviceConfig

        public static DeviceConfig DeviceConfig;

        public static void GetDeviceConfigFromFile(string filepath)
        {
            DeviceConfig = Deserialize<DeviceConfig>(filepath);
            FilePathDeviceConfig = filepath;

            var workstations = DeviceConfig.WorkStations;

            foreach (var w in workstations)
            {
                var stateUnits = DeviceConfig.StatusUnits.ToList().FindAll(f => f.WorkStationName.Equals(w.Name));

                var indexSu = 3;
                var w1 = w;
                foreach (var newState in from object item in Enum.GetValues(typeof(StateMachineHelper.EDefaultStateUnits))
                                         let state = stateUnits.Find(t => string.Equals(t.Name.ToUpper(), item.ToString().ToUpper(), StringComparison.CurrentCultureIgnoreCase))
                                         where state == null
                                         select new DeviceConfigStatusUnit
                                         {
                                             WorkStationName = w1.Name,
                                             Name = item.ToString().ToUpper(),
                                             DuringFunction = string.Empty,
                                             EnterFunction = string.Empty,
                                             PositionSize = 300 * indexSu++ + ",340,180,90"
                                         })
                    stateUnits.Add(newState);

                var tempList = DeviceConfig.StatusUnits.ToList();
                tempList.RemoveAll(f => f.WorkStationName.Equals(w.Name));
                tempList.AddRange(stateUnits);

                DeviceConfig.StatusUnits = tempList.ToArray();
            }

            DeviceConfig.StatusUnits = DeviceConfig.StatusUnits.ToList().OrderBy(f => f.WorkStationName).ToArray();
            DeviceConfig.Conditions = DeviceConfig.Conditions.ToList().OrderBy(f => f.WorkStationName).ToArray();
            SaveDeviceConfigToFile(DeviceConfig, filepath, Encoding.UTF8);
        }

        public static void SaveDeviceConfigToFile(object config, string filepath, Encoding encoding)
        {
            SerializeToFile(config, filepath, Encoding.UTF8);
        }

        public static T Deserialize<T>(string filePath)
        {
            var type = typeof(T);

            object targetObj;

            var serializer = new XmlSerializer(type);

            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    targetObj =
                        (T) serializer.Deserialize(reader);
                }
            }

            return (T) targetObj;
        }

        private static void SerializeToFile(object o, string path, Encoding encoding)
        {
            //if (string.IsNullOrEmpty(path))
            //    throw new ArgumentNullException(nameof(path));

            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
                XmlSerializeInternal(file, o, encoding);
        }

        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            //if (o == null)
            //    throw new ArgumentNullException(nameof(o));

            //if (encoding == null)
            //    throw new ArgumentNullException(nameof(encoding));

            var serializer = new XmlSerializer(o.GetType());

            var settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineChars = "\r\n",
                Encoding = encoding,
                IndentChars = "    "
            };

            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }

        #endregion

        #region 记录日志 WriteLog(string strLog)

        /// <summary>
        /// WriteLog
        /// 把字符串写到日志表中
        /// </summary>
        /// <param name="strLog"></param>
        public static void WriteLog(string strLog)
        {
            //DAL.commLogsData dalLog = new DAL.commLogsData();
            //Model.commLogsData modelLog = new Model.commLogsData();

            //modelLog.note = strLog;
            //modelLog.creater = staticUserLogin.userNo + " " + staticUserLogin.userName;
            //modelLog.createTime = DateTime.Now;
            //dalLog.Add(modelLog);
        }

        #endregion

        #region  CRC校验 CrcCalc()

        /// <summary>
        /// CRC校验，低位在前，高位在后
        /// </summary>
        /// <param name="data">校验数据</param>
        /// <returns>高低8位</returns>
        public static byte[] CrcCalc(IEnumerable<byte> data)
        {
            var crcbuf = data.ToArray();

            // 计算并填写CRC校验码
            var crc = 0xffff;
            var len = crcbuf.Length;
            for (var n = 0; n < len; n++)
            {
                byte i;
                crc = crc ^ crcbuf[n];
                for (i = 0; i < 8; i++)
                {
                    var tt = crc & 1;
                    crc = crc >> 1;
                    crc = crc & 0x7fff;
                    if (tt == 1)
                        crc = crc ^ 0xa001;
                    crc = crc & 0xffff;
                }
            }

            var result = new byte[2];
            result[1] = (byte) ((crc >> 8) & 0xff);
            result[0] = (byte) (crc & 0xff);

            return result;
        }

        #endregion

        #region 字符串转数组

        /// <summary>
        /// 字符串转数组
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static byte[] StringToHexArray(string strText)
        {
            var replace = strText.Replace(" ", "");
            var byteBuff = new byte[strText.Length / 2];

            for (var i = 0; i < strText.Length / 2; i++)
                byteBuff[i] = Convert.ToByte(strText.Substring(i * 2, 2), 16);
            return byteBuff;
        }

        #endregion

        #region Xml反序列化

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object XmlDeserialize(Type type, string xml)
        {
            try
            {
                using (var sr = new StringReader(xml))
                {
                    var xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static object XmlDeserialize(Type type, Stream stream)
        {
            var xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }

        #endregion

        #region Xml序列化

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string XmlSerializer(Type type, object obj)
        {
            var stream = new MemoryStream();
            var xml = new XmlSerializer(type);
            // 序列化对象
            xml.Serialize(stream, obj);
            stream.Position = 0;
            var sr = new StreamReader(stream);
            var str = sr.ReadToEnd();

            sr.Dispose();
            stream.Dispose();

            return str;
        }

        #endregion

        #region 类型转换

        public static object MyChanageType(object value, Type convertsionType)
        {
            // 判断convertsionType类型是否为泛型，因为nullable是泛型类,//判断convertsionType是否为nullable泛型类
            if (!convertsionType.IsGenericType || convertsionType.GetGenericTypeDefinition() != typeof(Nullable<>))
                return Convert.ChangeType(value, convertsionType);
            if (value == null || value.ToString().Length == 0)
                return null;
            // 如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
            var nullableConverter = new NullableConverter(convertsionType);
            // 将convertsionType转换为nullable对的基础基元类型
            convertsionType = nullableConverter.UnderlyingType;
            return Convert.ChangeType(value, convertsionType);
        }

        #endregion

        #region 读取DXP文件中的实体

        public static List<LineData> ReadLineFromDxp(string filepath)
        {
            var lstLineDatas = new List<LineData>();

            var lines = File.ReadAllLines(filepath, Encoding.Default);

            for (var i = 1; i < lines.Length; i++)
            {
                if (lines[i].Trim() != "AcDbLine")
                    continue;

                var lineData = new LineData();

                if (i < lines.Length - 12)
                {
                    lineData.X1 = float.Parse(lines[i + 2]);
                    lineData.Y1 = float.Parse(lines[i + 4]);
                    lineData.Z1 = float.Parse(lines[i + 6]);
                    lineData.X2 = float.Parse(lines[i + 8]);
                    lineData.Y2 = float.Parse(lines[i + 10]);
                    lineData.Z2 = float.Parse(lines[i + 12]);
                    i += 12;
                }

                lstLineDatas.Add(lineData);
            }

            return lstLineDatas;
        }

        public class LineData
        {
            public float X1, Y1, Z1;
            public float X2, Y2, Z2;
        }

        public class CircleData
        {
            public float X;
            public float Y;
            public float Z;
            public float Radius;
        }

        #endregion

        /// <summary>
        /// 将object对象转换为实体对象
        /// </summary>
        /// <typeparam name="T">实体对象类名</typeparam>
        /// <param name="asObject">object对象</param>
        /// <returns></returns>
        public static T ConvertObject<T>(object asObject) where T : new()
        {
            // 创建实体对象实例
            var t = Activator.CreateInstance<T>();
            if (asObject == null)
                return t;

            var type = asObject.GetType();
            // 遍历实体对象属性
            foreach (var info in typeof(T).GetProperties())
            {
                // 取得object对象中此属性的值
                object val = null;
                var propertyInfo = type.GetProperty(info.Name);
                if (propertyInfo != null)
                    val = propertyInfo.GetValue(asObject);

                if (val == null)
                    continue;

                // 非泛型
                object obj;
                if (!info.PropertyType.IsGenericType)
                    obj = Convert.ChangeType(val, info.PropertyType);
                else // 泛型Nullable<>
                {
                    var genericTypeDefinition = info.PropertyType.GetGenericTypeDefinition();
                    obj = Convert.ChangeType(val,
                        genericTypeDefinition == typeof(Nullable<>)
                            ? Nullable.GetUnderlyingType(info.PropertyType)
                            : info.PropertyType);
                }
                info.SetValue(t, obj, null);
            }
            return t;
        }
    }

    public class FieldDisplayName
    {
        public string ControlType;
        public string LabelString;
        public string SearchString;
        public string TipString;
        public string InDataGrid;
        public string InPanel;
        public string Index;

        public FieldDisplayName()
        {

        }
    }
}
