using MiniExcelLibs;
using Sunny.UI;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.TPS92994
{
    public partial class FrmMatrixConfig : UIForm
    {
        public FrmMatrixConfig()
        {
            InitializeComponent();
            Load += FrmMatrixConfig_Load;
        }

        private Dictionary<string, MatrixCfg> _matrixCfgs = new Dictionary<string, MatrixCfg>();

        private void FrmMatrixConfig_Load(object sender, System.EventArgs e)
        {
            var path = @"D:\Projects\CheckSystem\CheckSystem\bin\Debug\TpsConfig\MatrixCfg.xlsx";
            foreach (IDictionary<string, object> row in MiniExcel.QueryRange(path, startCell: "B"))
            {
                var columns = row.Keys.ToList();
                var bBreak = false;

                for (int i = 0; i < columns.Count; i++)
                {
                    var key = columns[i];
                    var value = row[key];

                    if (!_matrixCfgs.ContainsKey(key))
                    {
                        _matrixCfgs.Add(key, new MatrixCfg { Name = key });

                        if (value is null)
                        {
                            bBreak = true;
                            break;
                        }

                        _matrixCfgs[key].Ts = double.Parse(value.ToString());
                    }
                    else
                    {
                        _matrixCfgs[key].OnOff.Add(!(value is null || string.IsNullOrEmpty(value.ToString()) || string.Equals(value.ToString(), "0")));
                    }
                }
            }
        }

        internal class MatrixCfg
        {
            public string Name { get; set; }
            public double Ts { get; set; }
            public List<bool> OnOff { get; set; }
            public MatrixCfg() => OnOff = new List<bool>();
        }
    }
}
