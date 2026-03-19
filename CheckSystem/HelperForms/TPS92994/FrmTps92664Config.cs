using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.TPS92994
{
    public partial class FrmTps92664Config : UIForm
    {
        private Tps92664ConfigData _configData;
        private readonly string _configFilePath;

        public FrmTps92664Config(string filePath)
        {
            InitializeComponent();

            _configFilePath = filePath;

            // 先初始化界面（设置列），再加载配置（绑定数据）
            InitializeDataGrids();
            LoadConfigFromFile(true); // 显示提示
            dgvHighBeam.CellValidating += Dgv_CellValidating;

            // 绑定事件
            btnSaveConfig.Click += BtnSaveConfig_Click;
            btnClose.Click += BtnClose_Click;
        }

        /// <summary>
        /// 初始化DataGridView
        /// </summary>
        private void InitializeDataGrids()
        {
            // 设置DataGridView列
            SetupDataGridView(dgvHighBeam);
        }

        /// <summary>
        /// 设置DataGridView的列
        /// </summary>
        private void SetupDataGridView(DataGridView dgv)
        {
            // 手动创建列
            dgv.Columns.Clear();
            dgv.AutoGenerateColumns = false;

            // 通道号列
            var colChannel = new DataGridViewTextBoxColumn
            {
                Name = "Channel",
                HeaderText = "通道号",
                DataPropertyName = "Channel",
                Width = 80,
                ReadOnly = true
            };
            dgv.Columns.Add(colChannel);

            // 通道名称列
            var colChannelName = new DataGridViewTextBoxColumn
            {
                Name = "ChannelName",
                HeaderText = "通道名称",
                DataPropertyName = "ChannelName",
                Width = 120,
                ReadOnly = true
            };
            dgv.Columns.Add(colChannelName);

            // Phase列
            var colPhase = new DataGridViewTextBoxColumn
            {
                Name = "Phase",
                HeaderText = "Phase (0-1023)",
                DataPropertyName = "Phase",
                Width = 120
            };
            dgv.Columns.Add(colPhase);

            // PWM列
            var colPwm = new DataGridViewTextBoxColumn
            {
                Name = "PWM",
                HeaderText = "PWM (0-1023)",
                DataPropertyName = "PWM",
                Width = 120
            };
            dgv.Columns.Add(colPwm);

            // TS1列
            var colTs1 = new DataGridViewTextBoxColumn
            {
                Name = "TS1",
                HeaderText = "第一次点亮间隔",
                DataPropertyName = "Ts1",
                Width = 120
            };
            dgv.Columns.Add(colTs1);

            // TS2列
            var colTs2 = new DataGridViewTextBoxColumn
            {
                Name = "TS2",
                HeaderText = "第二次点亮间隔",
                DataPropertyName = "Ts2",
                Width = 120,
            };
            dgv.Columns.Add(colTs2);

            // 基本设置
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = false;
        }

        private void Dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv is null)
                return;

            var cell = dgv[e.ColumnIndex, e.RowIndex];

            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                ushort tpValue;
                try
                {
                    if (ushort.TryParse(e.FormattedValue.ToString(), out tpValue) && tpValue <= 1023)
                    {
                        return;
                    }
                    else
                    {
                        UIMessageTip.ShowError("请输入0-1023之间的数字");
                        e.Cancel = true;
                        dgv.CancelEdit();
                        return;
                    }
                }
                catch (Exception)
                {
                    UIMessageTip.ShowError("请输入0-1023之间的数字");
                    e.Cancel = true;
                    return;
                }
            }
            else if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
            {
                ushort tpValue;
                try
                {
                    if (ushort.TryParse(e.FormattedValue.ToString(), out tpValue) && tpValue >= 0)
                    {
                        return;
                    }
                    else
                    {
                        UIMessageTip.ShowError("请输入大于0的数字");
                        e.Cancel = true;
                        dgv.CancelEdit();
                        return;
                    }
                }
                catch (Exception)
                {
                    UIMessageTip.ShowError("请输入大于0的数字");
                    e.Cancel = true;
                    return;
                }
            }
        }

        /// <summary>
        /// 更新DataGridView显示
        /// </summary>
        private void UpdateDataGridView(DataGridView dgv, BeamConfig beamConfig, int onTs1, int onTs2, int offTs1)
        {
            if (beamConfig?.Channels != null)
            {
                dgv.DataSource = beamConfig.Channels;
                numKeepOnTs1.Value = onTs1;
                numKeepOffTs1.Value = offTs1;
                numKeepOnTs2.Value = onTs2;
            }
        }

        /// <summary>
        /// 获取当前选择的PWM百分比
        /// </summary>
        private int GetPwmPercentage()
        {
            //if (cmbPwmPercentage.SelectedItem == null)
            //    return 100;

            //string text = cmbPwmPercentage.SelectedItem.ToString();
            //text = text.Replace("%", "");
            //if (int.TryParse(text, out int percentage))
            //    return percentage;
            return 100;
        }

        /// <summary>
        /// 保存配置到文件
        /// </summary>
        private void BtnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                // 保存DataGridView的编辑
                dgvHighBeam.EndEdit();

                _configData.KEEPONTS1 = (int)numKeepOnTs1.Value;
                _configData.KEEPONTS2 = (int)numKeepOnTs2.Value;
                _configData.KEEPOFFTS1 = (int)numKeepOffTs1.Value;

                var json = JsonConvert.SerializeObject(_configData, Formatting.Indented);
                File.WriteAllText(_configFilePath, json);
                UIMessageBox.ShowSuccess($"配置已保存到: {_configFilePath}");
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError($"保存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从文件加载配置
        /// </summary>
        /// <param name="showMessage">是否显示加载成功消息</param>
        private void LoadConfigFromFile(bool showMessage = true)
        {
            try
            {
                if (File.Exists(_configFilePath))
                {
                    var json = File.ReadAllText(_configFilePath);
                    _configData = JsonConvert.DeserializeObject<Tps92664ConfigData>(json);
                    if (_configData == null)
                        _configData = Tps92664ConfigData.CreateDefault();

                    // 确保有HighBeam配置
                    if (_configData.HighBeam == null)
                        _configData.HighBeam = BeamConfig.CreateDefault();

                    // 确保有16个通道
                    if (_configData.HighBeam.Channels == null || _configData.HighBeam.Channels.Count != 16)
                        _configData.HighBeam = BeamConfig.CreateDefault();

                    // 更新DataGridView
                    UpdateDataGridView(dgvHighBeam, _configData.HighBeam, _configData.KEEPONTS1, _configData.KEEPONTS2, _configData.KEEPOFFTS1);

                    if (showMessage)
                        UIMessageBox.ShowSuccess("配置已加载");
                }
                else
                {
                    _configData = Tps92664ConfigData.CreateDefault();
                    UpdateDataGridView(dgvHighBeam, _configData.HighBeam, _configData.KEEPONTS1, _configData.KEEPONTS2, _configData.KEEPOFFTS1);
                }
            }
            catch (Exception ex)
            {
                if (showMessage)
                    UIMessageBox.ShowError($"加载失败: {ex.Message}");

                _configData = Tps92664ConfigData.CreateDefault();
                UpdateDataGridView(dgvHighBeam, _configData.HighBeam, _configData.KEEPONTS1, _configData.KEEPONTS2, _configData.KEEPOFFTS1);
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

