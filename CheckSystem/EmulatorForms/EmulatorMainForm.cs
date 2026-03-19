using CommonUtility;
using Controller;
using HZH_Controls.Controls.Btn;
using StateMachine;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility.FileOperator;
using HZH_Controls.IconFont;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.EmulatorForms
{
    public partial class EmulatorMainForm : UIForm
    {
        private float _x;//当前窗体的宽度
        private float _y;//当前窗体的高度

        /// <summary>
        /// 将控件的宽，高，左边距，顶边距和字体大小暂存到tag属性中
        /// </summary>
        /// <param name="cons">递归控件中的控件</param>
        private static void SetTag(System.Windows.Forms.Control cons)
        {
            foreach (System.Windows.Forms.Control con in cons.Controls)//循环窗体中的控件
            {
                if (con.GetType() != typeof(UIButton))
                {
                    con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                    if (con.Controls.Count > 0)
                        SetTag(con);
                }
            }
        }

        /// <summary>
        /// 根据窗体大小调整控件大小
        /// </summary>
        /// <param name="newx">窗体宽度缩放比例</param>
        /// <param name="newy">窗体高度缩放比例</param>
        /// <param name="cons">随窗体改变控件大小</param>
        private static void SetControls(float newx, float newy, System.Windows.Forms.Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (System.Windows.Forms.Control con in cons.Controls)
            {
                var mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                var a = System.Convert.ToSingle(mytag[0]) * newx;//根据窗体缩放比例确定控件的值，宽度
                con.Width = (int)a;//宽度
                a = System.Convert.ToSingle(mytag[1]) * newy;//高度
                con.Height = (int)(a);
                a = System.Convert.ToSingle(mytag[2]) * newx;//左边距离
                con.Left = (int)(a);
                a = System.Convert.ToSingle(mytag[3]) * newy;//上边缘距离
                con.Top = (int)(a);
                var currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                    SetControls(newx, newy, con);
            }
        }

        //private readonly string _configFile = string.Format(@"{0}\EmulatorConfig\AS33_PES.xml", Directory.GetCurrentDirectory());
        private EmulatorConfig EmulatorConfig { get; set; }
        protected static List<object> ControllersList { get; set; }
        private Thread RefreshFieldValueTh { get; set; }
        private bool _isInvokeMethod;

        public EmulatorMainForm(string xmlFile)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Closed += EmulatorMainForm_Closed;
            Icon = FontImages.GetIcon(FontIcons.A_fa_simplybuilt, 32,
                Color.DodgerBlue);
            EmulatorConfig = XmlHelper.Deserialize<EmulatorConfig>(xmlFile);
            Text = string.Format("仿真器（{0})", EmulatorConfig.ShowTitle);

            btnTitle.Text = EmulatorConfig.ShowTitle;
            btnTitle.BackColor = Color.DodgerBlue;
            btnTitle.Font = new Font("黑体", 22, FontStyle.Bold);

            for (var i = 0; i < EmulatorConfig.Buttons.Length; i++)
            {
                var btn = EmulatorConfig.Buttons[i];
                var button = new UCBtnExt
                {
                    BtnText = btn.Text,
                    Name = i.ToString(),
                    Margin = new Padding(5, 5, 5, 5),
                    Width = (this.Width / 2) / 4,
                    Font = new Font("微软雅黑", 10 * (System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width / 1920f), FontStyle.Regular)
                };
                button.BtnClick += button_BtnClick;
                btnsPanel.Controls.Add(button);
            }
            btnsPanel.AutoScroll = true;

            //dgvInputFields.label.Height = 30;
            //dgvInputFields.label.Text = @"字段列表-R";
            dgvInputFields.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段名称" });
            //dgvInputFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段备注" });
            //dgvInputFields.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段类型" });
            dgvInputFields.Columns.Add(new DataGridViewTextBoxColumn { Name = "当前值" });
            dgvInputFields.ReadOnly = true;
            dgvInputFields.RowHeadersVisible = false;
            dgvInputFields.AllowUserToAddRows = false;
            dgvInputFields.AllowUserToDeleteRows = false;
            dgvInputFields.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInputFields.AllowUserToResizeColumns = true;
            dgvInputFields.AllowUserToResizeRows = false;
            for (var i = 0; i < dgvInputFields.Columns.Count; i++)
                dgvInputFields.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            ControllersList = new List<object>();
            var controllers = EmulatorConfig.ControllerConfig;
            var asmb = Assembly.LoadFrom("Controller.dll");

            foreach (var controller in from item in controllers
                                       let typeName = asmb.GetType("Controller." + item.ControllerType)
                                       where typeName != null
                                       select Activator.CreateInstance(typeName, item.ControllerName))
                ControllersList.Add(controller);

            try
            {
                if (EmulatorConfig.GridDatas != null)
                {
                    foreach (var t in EmulatorConfig.GridDatas)
                    {
                        if (t != null)
                        {
                            var name = t.Name;
                            var field = t.Type;

                            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(field))
                            {
                                var controllerName = field.Split('.')[0];
                                var fieldName = field.Split('.')[2];

                                foreach (var c in ControllersList)
                                {
                                    var findController = c as ControllerBase;
                                    if (findController != null && findController.Name.Equals(controllerName))
                                    {
                                        var findField = findController.GetType().GetField(fieldName);
                                        if (findField != null)
                                        {
                                            var value = findField.GetValue(findController);
                                            var fieldValue = value != null ? value.ToString() : string.Empty;

                                            var rowAdd = dgvInputFields.Rows.Add();

                                            dgvInputFields.Rows[rowAdd].Cells[0].Value = name;
                                            //dgvInputFields.Rows[rowAdd].Cells[1].Value = findField.FieldType;
                                            dgvInputFields.Rows[rowAdd].Cells[1].Value = fieldValue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            ControllersList.Add(new CheckApp("检测程序"));
            ExecuteMethod(EmulatorConfig.Init.EnterFunction);

            if (RefreshFieldValueTh != null)
            {
                RefreshFieldValueTh.Abort();
                RefreshFieldValueTh.Join();
            }

            RefreshFieldValueTh = new Thread(RefreshFieldValue) { IsBackground = true };
            RefreshFieldValueTh.Start();
            Load += EmulatorMainForm_Load;
        }

        private void EmulatorMainForm_Load(object sender, EventArgs e)
        {
            _x = this.Width;//获取窗体的宽度
            _y = this.Height;//获取窗体的高度
            SetTag(this);//调用方法

            Resize += EmulatorMainForm_Resize;
        }

        private void EmulatorMainForm_Resize(object sender, EventArgs e)
        {
            var newX = (this.Width) / _x; //窗体宽度缩放比例
            var newY = (this.Height) / _y;//窗体高度缩放比例
            SetControls(newX, newY, this);//随窗体改变控件大小
        }

        public void EmulatorMainForm_Closed(object sender, EventArgs e)
        {
            if (RefreshFieldValueTh != null)
            {
                RefreshFieldValueTh.Abort();
                RefreshFieldValueTh.Join();
            }

            if (ControllersList == null)
                return;
            foreach (var cb in ControllersList.OfType<ControllerBase>())
                cb.Dispose();
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private async void button_BtnClick(object sender, EventArgs e)
        {
            if (_isInvokeMethod)
                return;

            _isInvokeMethod = true;
            btnsPanel.BackColor = Color.LightSlateGray;
            //btnsPanel.Enabled = false;

            await Task.Run(() =>
            {
                try
                {
                    var btn = sender as UCBtnExt;
                    if (btn != null)
                    {
                        var index = int.Parse(btn.Name);

                        var btnConfig = EmulatorConfig.Buttons[index];
                        ExecuteMethod(btnConfig.Method);

                        //var str = string.Format("Method:{0};Text:{1};Para:{2}", btnConfig.Method, btnConfig.Text, btnConfig.Para);
                        //MessageBox.Show(str);

                        //if (Interface != null)
                        //{
                        //    var method = Interface.GetType().GetMethod(btnConfig.Method);
                        //    if (method != null)
                        //    {
                        //        method.Invoke(Interface, null);
                        //    }
                        //}
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            });

            //btnsPanel.Enabled = true;
            btnsPanel.BackColor = Color.LightGoldenrodYellow;
            _isInvokeMethod = false;
        }

        private static void ExecuteMethod(string code)
        {
            var codeLines = code.StrTrim().TrimEnd(';').Split(';');
            foreach (var codeStr in codeLines)
            {
                var left = codeStr.GetStrFromLeftSingleEqualitySign();
                var right = codeStr.GetStrFromRightSingleEqualitySign();

                if (codeStr.Contains(".Method."))
                {
                    var leftSplitByDot =
                        codeStr.GetStrsSplitByValue(".Method.");
                    var leftController =
                        codeStr.GetStrsSplitByValue(".Method.")[0].GetControllerByName(ControllersList);

                    var methodName = leftSplitByDot[1].Substring(0, leftSplitByDot[1].IndexOf('('));
                    var strParas = leftSplitByDot[1].Substring(leftSplitByDot[1].IndexOf('(') + 1,
                        leftSplitByDot[1].IndexOf(')') - leftSplitByDot[1].IndexOf('(') - 1);

                    var lstParaObjs = new List<object>();

                    // 把参数列表分开]
                    if (leftController == null)
                        return;

                    var getAllMethods = leftController.GetType().GetMethods();
                    var lstParas = strParas.Split(',').ToList();
                    var paraCount = lstParas.Count;

                    if (paraCount == 1 && lstParas[0].Equals(""))
                        paraCount = 0;
                    else
                    {
                        foreach (
                            var methodPara in
                                from m in getAllMethods
                                where m.Name == methodName && m.GetParameters().Length == paraCount
                                select m.GetParameters())
                            lstParaObjs.AddRange(
                                methodPara.Select((t, i) => Convert.ChangeType(lstParas[i], t.ParameterType)));
                    }

                    var firstOrDefault = getAllMethods.Where(m => m.Name == methodName && m.GetParameters().Length == paraCount)
                        .Select(m1 => (Action)(() => m1.Invoke(leftController, lstParaObjs.ToArray())))
                        .FirstOrDefault();

                    if (firstOrDefault != null) firstOrDefault.Invoke();
                    continue;
                }
                if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right))
                    continue;
                {
                    var leftController = new object();
                    var leftFieldName = string.Empty;

                    if (left.Contains(".Field."))
                    {
                        leftController = left.GetStrsSplitByValue(".Field.")[0].GetControllerByName(ControllersList);
                        leftFieldName = left.GetStrsSplitByValue(".Field.")[1];
                    }

                    if (left.Contains(".Field.") && right.Contains(".Field."))
                    {
                        var rightController = right.GetStrsSplitByValue(".Field.")[0].GetControllerByName(ControllersList);

                        if (rightController == null)
                            return;

                        var rightField = rightController.GetType().GetField(right.GetStrsSplitByValue(".Field.")[1]);

                        leftController.GetType()
                            .GetField(leftFieldName)
                            .SetValue(leftController, rightField.GetValue(rightController));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(leftFieldName) && !string.IsNullOrEmpty(right) && leftController != null)
                        {
                            var leftFieldType = leftController.GetType().GetField(leftFieldName).FieldType;
                            var rightValue = right;
                            Func<object> leftValue = null;

                            if (leftFieldType == typeof(bool))
                                leftValue = () => rightValue == "1";
                            else
                            {
                                if (leftFieldType == typeof(ushort))
                                {
                                    if (codeStr.Contains("=") && !codeStr.Contains("+=") && !codeStr.Contains("-=") && !codeStr.Contains("*=") && !codeStr.Contains("/="))
                                    {
                                        leftValue = () => Convert.ToUInt16(rightValue);
                                    }
                                    else if (codeStr.Contains("+="))
                                    {
                                        leftValue = () => (ushort)(Convert.ToUInt16(rightValue) +
                                                              Convert.ToUInt16(
                                                                  leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                    }
                                    else if (codeStr.Contains("-="))
                                    {
                                        leftValue = () => (ushort)(Convert.ToUInt16(rightValue) -
                                                              Convert.ToUInt16(
                                                                  leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                    }
                                    else if (codeStr.Contains("*="))
                                    {
                                        leftValue = () => (ushort)(Convert.ToUInt16(rightValue) *
                                                             Convert.ToUInt16(
                                                                 leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                    }
                                    else if (codeStr.Contains("+="))
                                    {
                                        leftValue = () => (ushort)(Convert.ToUInt16(rightValue) /
                                                             Convert.ToUInt16(
                                                                 leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                    }

                                }
                                else if (leftFieldType == typeof(float))
                                {
                                    //leftValue = Convert.ToSingle(rightValue);

                                    if (codeStr.Contains("=") && !codeStr.Contains("+=") && !codeStr.Contains("-=") && !codeStr.Contains("*=") && !codeStr.Contains("/="))
                                    {
                                        leftValue = () => Convert.ToSingle(rightValue);
                                    }
                                    else if (codeStr.Contains("+="))
                                    {
                                        leftValue = () => Convert.ToSingle(rightValue) +
                                                    Convert.ToSingle(
                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                    }
                                    else if (codeStr.Contains("-="))
                                    {
                                        leftValue = () => Convert.ToSingle(rightValue) -
                                                    Convert.ToSingle(
                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                    }
                                    else if (codeStr.Contains("*="))
                                    {
                                        leftValue = () => Convert.ToSingle(rightValue) *
                                                    Convert.ToSingle(
                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                    }
                                    else if (codeStr.Contains("/="))
                                    {
                                        leftValue = () => Convert.ToSingle(rightValue) /
                                                    Convert.ToSingle(
                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                    }
                                }
                                else if (leftFieldType == typeof(double))
                                {
                                    //leftValue = Convert.ToDouble(rightValue);

                                    if (codeStr.Contains("=") && !codeStr.Contains("+=") && !codeStr.Contains("-=") && !codeStr.Contains("*=") && !codeStr.Contains("/="))
                                    {
                                        leftValue = () => Convert.ToDouble(rightValue);
                                    }
                                    else if (codeStr.Contains("+="))
                                    {
                                        leftValue = () => Convert.ToDouble(rightValue) +
                                                    Convert.ToDouble(
                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                    }
                                    else if (codeStr.Contains("-="))
                                    {
                                        leftValue = () => Convert.ToDouble(rightValue) -
                                                    Convert.ToDouble(
                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                    }
                                    else if (codeStr.Contains("*="))
                                    {
                                        leftValue = () => Convert.ToDouble(rightValue) *
                                                    Convert.ToDouble(
                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                    }
                                    else if (codeStr.Contains("/="))
                                    {
                                        leftValue = () => Convert.ToDouble(rightValue) /
                                                    Convert.ToDouble(
                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                    }
                                }
                                else if (leftFieldType == typeof(int))
                                {
                                    //leftValue = Convert.ToInt32(rightValue);

                                    if (codeStr.Contains("=") && !codeStr.Contains("+=") && !codeStr.Contains("-=") && !codeStr.Contains("*=") && !codeStr.Contains("/="))
                                    {
                                        leftValue = () => Convert.ToInt32(rightValue);
                                    }
                                    else if (codeStr.Contains("+="))
                                    {
                                        leftValue = () => Convert.ToInt32(rightValue) +
                                                    Convert.ToInt32(
                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                    }
                                    else if (codeStr.Contains("-="))
                                    {
                                        leftValue = () => Convert.ToInt32(rightValue) -
                                                    Convert.ToInt32(
                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                    }
                                    else if (codeStr.Contains("*="))
                                    {
                                        leftValue = () => Convert.ToInt32(rightValue) *
                                                    Convert.ToInt32(
                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                    }
                                    else if (codeStr.Contains("/="))
                                    {
                                        leftValue = () => Convert.ToInt32(rightValue) /
                                                    Convert.ToInt32(
                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                    }
                                }
                                else if (leftFieldType == typeof(byte))
                                {
                                    //leftValue = Convert.ToByte(rightValue);

                                    if (codeStr.Contains("=") && !codeStr.Contains("+=") && !codeStr.Contains("-=") && !codeStr.Contains("*=") && !codeStr.Contains("/="))
                                    {
                                        leftValue = () => Convert.ToByte(rightValue);
                                    }
                                    else if (codeStr.Contains("+="))
                                    {
                                        leftValue = () => (byte)(Convert.ToByte(rightValue) +
                                                            Convert.ToByte(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                    }
                                    else if (codeStr.Contains("-="))
                                    {
                                        leftValue = () => (byte)(Convert.ToByte(rightValue) -
                                                            Convert.ToByte(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                    }
                                    else if (codeStr.Contains("*="))
                                    {
                                        leftValue = () => (byte)(Convert.ToByte(rightValue) *
                                                            Convert.ToByte(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                    }
                                    else if (codeStr.Contains("/="))
                                    {
                                        leftValue = () => (byte)(Convert.ToByte(rightValue) /
                                                            Convert.ToByte(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                    }
                                }
                                else if (leftFieldType == typeof(string))
                                {
                                    //leftValue = rightValue;

                                    if (codeStr.Contains("=") && !codeStr.Contains("+=") && !codeStr.Contains("-=") && !codeStr.Contains("*=") && !codeStr.Contains("/="))
                                    {
                                        leftValue = () => rightValue;
                                    }
                                    else if (codeStr.Contains("+="))
                                    {
                                        leftValue = () => string.Format("{0}{1}",
                                            leftController.GetType().GetField(leftFieldName).GetValue(leftController) ?? string.Empty,
                                            rightValue);
                                    }
                                }
                            }

                            if (leftValue != null)
                                leftController.GetType()
                                    .GetField(leftFieldName)
                                    .SetValue(leftController, Convert.ChangeType(leftValue.Invoke(), leftFieldType));
                        }
                    }
                }
            }
        }

        private void RefreshFieldValue()
        {
            while (RefreshFieldValueTh.IsAlive)
            {
                if (!RefreshFieldValueTh.IsAlive)
                    break;

                Thread.Sleep(50);

                try
                {
                    if (EmulatorConfig.GridDatas != null)
                    {
                        foreach (var t in EmulatorConfig.GridDatas)
                        {
                            if (t != null)
                            {
                                var name = t.Name;
                                var field = t.Type;

                                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(field))
                                {
                                    var controllerName = field.Split('.')[0];
                                    var fieldName = field.Split('.')[2];

                                    foreach (var c in ControllersList)
                                    {
                                        var findController = c as ControllerBase;
                                        if (findController != null && findController.Name.Equals(controllerName))
                                        {
                                            var findField = findController.GetType().GetField(fieldName);
                                            if (findField != null)
                                            {
                                                var value = findField.GetValue(findController);
                                                var fieldValue = value != null ? value.ToString() : string.Empty;

                                                for (var i = 0; i < dgvInputFields.RowCount; i++)
                                                {
                                                    var row = dgvInputFields.Rows[i];

                                                    if (row.Cells[0].Value.ToString() == name)
                                                    {
                                                        //dgvInputFields.Rows[i].Cells[1].Value = findField.FieldType;
                                                        dgvInputFields.Rows[i].Cells[1].Value = fieldValue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
