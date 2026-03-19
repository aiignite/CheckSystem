using StateMachine;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CheckSystem
{
    public partial class FormGroupSelect : Form
    {
        private State St { get; set; }
        private readonly Dictionary<string, List<string>> _groupDictionary = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, List<string>> _barcodeDictionary = new Dictionary<string, List<string>>();

        public FormGroupSelect(State st)
        {
            InitializeComponent();
            St = st;

            foreach (var c in groupBox1.Controls)
            {
                if (c.GetType() == typeof(TableLayoutPanel))
                {
                    var tb = (TableLayoutPanel)c;
                    tb.Visible = false;
                    tb.BackColor = Color.AntiqueWhite;
                }
            }


            foreach (var c in groupBox2.Controls)
            {
                if (c.GetType() == typeof(TableLayoutPanel))
                {
                    var tb = (TableLayoutPanel)c;
                    tb.Visible = false;
                    tb.BackColor = Color.AntiqueWhite;
                }
            }

            if (st.LedGroupList.Count > 0)
            {
                foreach (var t in st.LedGroupList.Keys)
                {
                    // 先提取出所有档位并归类
                    var name = t.ToString();

                    var strTemp = string.Empty;
                    var gpTemp = st.LedGroupList[t].OrderBy(f => f.ToUpper()).ToList();
                    strTemp = gpTemp.Aggregate(strTemp, (current, kk) => current + (kk + "$")).TrimEnd('$');

                    if (!_groupDictionary.ContainsKey(strTemp))
                        _groupDictionary.Add(strTemp, new List<string>());
                    _groupDictionary[strTemp].Add(t);
                }

                for (var i = 0; i < _groupDictionary.Count; i++)
                {
                    var controlName = string.Format("panelLedGroup{0}", i);
                    var findControl = groupBox1.Controls.Find(controlName, false);
                    if (findControl.Any() && findControl[0].GetType() == typeof(TableLayoutPanel))
                    {
                        if (findControl[0] is TableLayoutPanel table)
                        {
                            table.Tag = _groupDictionary.Keys.ToList()[i];
                            table.Visible = true;

                            var gps = table.Tag.ToString().Split('$');

                            var cmbName = table.GetControlFromPosition(0, 0) as ComboBox;
                            var cmbGp = table.GetControlFromPosition(1, 0) as ComboBox;

                            if (cmbName != null && cmbGp != null)
                            {
                                foreach (var t in _groupDictionary[table.Tag.ToString()])
                                    cmbName.Items.Add(t);
                                cmbName.SelectedIndex = 0;

                                foreach (var t in gps)
                                    cmbGp.Items.Add(t);
                                cmbGp.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }

            if (st.BarcodeGroupList.Count > 0)
            {
                foreach (var t in st.BarcodeGroupList.Keys)
                {
                    // 先提取出所有档位并归类
                    var name = t.ToString();

                    var strTemp = string.Empty;
                    var gpTemp = st.BarcodeGroupList[t].OrderBy(f => f.ToUpper()).ToList();
                    strTemp = gpTemp.Aggregate(strTemp, (current, kk) => current + (kk + "$")).TrimEnd('$');

                    if (!_barcodeDictionary.ContainsKey(strTemp))
                        _barcodeDictionary.Add(strTemp, new List<string>());
                    _barcodeDictionary[strTemp].Add(t);
                }

                for (var i = 0; i < _barcodeDictionary.Count; i++)
                {
                    var controlName = string.Format("panelBarcodeGroup{0}", i);
                    var findControl = groupBox2.Controls.Find(controlName, false);
                    if (findControl.Any() && findControl[0].GetType() == typeof(TableLayoutPanel))
                    {
                        if (findControl[0] is TableLayoutPanel table)
                        {
                            table.Tag = _barcodeDictionary.Keys.ToList()[i];
                            table.Visible = true;

                            var gps = table.Tag.ToString().Split('$');

                            var cmbName = table.GetControlFromPosition(0, 0) as ComboBox;
                            var cmbGp = table.GetControlFromPosition(1, 0) as ComboBox;

                            if (cmbName != null && cmbGp != null)
                            {
                                foreach (var t in _barcodeDictionary[table.Tag.ToString()])
                                    cmbName.Items.Add(t);
                                cmbName.SelectedIndex = 0;

                                foreach (var t in gps)
                                    cmbGp.Items.Add(t);
                                cmbGp.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            for (var i = 0; i < 24; i++)
            {
                var controlName = string.Format("panelLedGroup{0}", i);
                var findControl = groupBox1.Controls.Find(controlName, false);
                if (findControl.Any() && findControl[0].GetType() == typeof(TableLayoutPanel))
                {
                    if (findControl[0] is TableLayoutPanel table && table.Visible == true)
                    {
                        var gpKey = table.Tag.ToString();
                        var gps = gpKey.Split('$');
                        var cmbName = table.GetControlFromPosition(0, 0) as ComboBox;
                        var cmbGp = table.GetControlFromPosition(1, 0) as ComboBox;

                        if (cmbName != null && cmbGp != null)
                        {
                            foreach (var name in cmbName.Items)
                            {
                                var paras = _groupDictionary[gpKey];
                                var gear = cmbGp.Items[cmbGp.SelectedIndex].ToString();

                                var toFind = St.LedGroupList.Keys.ToList()
                                    .Find(f => f.ToUpper() == name.ToString().ToUpper());
                                if (toFind != null)
                                {
                                    var toFindGp = St.LedGroupList[toFind].Find(f => f.ToUpper() == gear.ToUpper());
                                    if (toFindGp != null)
                                    {
                                        var temp = St.DeviceConfig.Paras.ToList();
                                        var para = temp.Find(f => f.Name.ToUpper().Equals(toFind.ToUpper()));
                                        para.OkFormat = string.Format("[{0}]", toFindGp);
                                        St.DeviceConfig.Paras = temp.ToArray();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for (var i = 0; i < 16; i++)
            {
                var controlName = string.Format("panelBarcodeGroup{0}", i);
                var findControl = groupBox2.Controls.Find(controlName, false);
                if (findControl.Any() && findControl[0].GetType() == typeof(TableLayoutPanel))
                {
                    if (findControl[0] is TableLayoutPanel table && table.Visible == true)
                    {
                        var gpKey = table.Tag.ToString();
                        var gps = gpKey.Split('$');
                        var cmbName = table.GetControlFromPosition(0, 0) as ComboBox;
                        var cmbGp = table.GetControlFromPosition(1, 0) as ComboBox;

                        if (cmbName != null && cmbGp != null)
                        {
                            foreach (var name in cmbName.Items)
                            {
                                var paras = _barcodeDictionary[gpKey];
                                var gear = cmbGp.Items[cmbGp.SelectedIndex].ToString();

                                var toFind = St.BarcodeGroupList.Keys.ToList()
                                    .Find(f => f.ToUpper() == name.ToString().ToUpper());
                                if (toFind != null)
                                {
                                    var toFindGp = St.BarcodeGroupList[toFind].Find(f => f.ToUpper() == gear.ToUpper());
                                    if (toFindGp != null)
                                    {
                                        var temp = St.DeviceConfig.Paras.ToList();
                                        var para = temp.Find(f => f.Name.ToUpper().Equals(toFind.ToUpper()));
                                        para.Value = string.Format("{0}", toFindGp);
                                        St.DeviceConfig.Paras = temp.ToArray();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
