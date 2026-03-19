using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckSystem.Yfas.Utility;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.Prodcut
{
    public partial class YfasProductParaEditForm : UIForm
    {
        private readonly List<Data> _datas = new List<Data>();
        private Utility._3TierModel.YfasProductInfo _productModel = new Utility._3TierModel.YfasProductInfo();
        private readonly List<Utility._3TierModel.YfasProductParas> _yfasProductParas = new List<Utility._3TierModel.YfasProductParas>();
        private readonly List<Utility._3TierModel.YfasDetectionBase> _yfasDetectionBaseModels = new List<Utility._3TierModel.YfasDetectionBase>();
        private readonly Utility._3TierBll.YfasProductParas _yfasProductParasBll = new Utility._3TierBll.YfasProductParas();
        private readonly Utility._3TierBll.YfasDetectionBase _yfasDetectionBaseBll = new Utility._3TierBll.YfasDetectionBase();

        internal class Data
        {
            public string Id { get; set; }
            public string DetectionId { get; set; }
            public string DetectionName { get; set; }
            public string DateType { get; set; }
            public string OkFormat { get; set; }
            public string Value { get; set; }
            public string Min { get; set; }
            public string Max { get; set; }
            public string Uint { get; set; }
            public string BingControllerId { get; set; }
            public string BingControllerName { get; set; }
            public string BindControllerFieldName { get; set; }
            public string Offset { get; set; }
        }

        public YfasProductParaEditForm(int productId)
        {
            InitializeComponent();
            //Style = UIStyle.Orange;
            WindowState = FormWindowState.Maximized;

            uiDataGridView1.AllowUserToAddRows = false;
            uiDataGridView1.AllowUserToDeleteRows = false;
            uiDataGridView1.AllowUserToResizeColumns = true;
            uiDataGridView1.AllowUserToResizeRows = true;
            uiDataGridView1.RowHeadersVisible = true;
            uiDataGridView1.AllowUserToOrderColumns = false;
            uiDataGridView1.ReadOnly = false;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            uiDataGridView1.CellMouseUp += uiDataGridView1_CellMouseUp;
            uiDataGridView1.CellValueChanged += uiDataGridView1_CellValueChanged;
            InitParas(productId);
        }

        private void uiDataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                var value = uiDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                var str = string.Empty;
                if (value != null)
                    str = value.ToString();

                var data = _datas[e.RowIndex];
                var propertyInfo = data.GetType().GetProperty(uiDataGridView1.Columns[e.ColumnIndex].HeaderText);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(data, str);
                }
            }
        }

        private void uiDataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                uiDataGridView1.EndEdit();

                if (e.RowIndex >= 0 && e.ColumnIndex == -1)
                {
                    uiDataGridView1.ClearSelection();
                    //uiDataGridView1.CurrentRow = uiDataGridView1.Rows[e.RowIndex];
                    uiDataGridView1.Rows[e.RowIndex].Selected = true;
                    uiDataGridView1.CurrentCell = uiDataGridView1.Rows[e.RowIndex].Cells[0];
                    btnAdd.Visible = false;
                    btnDelete.Visible = true;
                    btnSave.Visible = false;
                    uiContextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
                else
                {
                    uiDataGridView1.ClearSelection();
                    btnAdd.Visible = true;
                    btnDelete.Visible = false;
                    btnSave.Visible = true;
                    uiContextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void InitParas(int productId)
        {
            _productModel = new Utility._3TierBll.YfasProductInfo().GetModel(productId);

            _datas.Clear();
            _yfasProductParas.Clear();
            _yfasProductParas.AddRange(
                _yfasProductParasBll.GetModelList(string.Format("ProductId = '{0}'", _productModel.id)));

            foreach (var t in _yfasProductParas)
            {
                Debug.Assert(t.DetectionId != null, "t.DetectionId != null");
                var detect = _yfasDetectionBaseBll.GetModel((int)t.DetectionId);
                var data = new Data
                {
                    Id = t.id.ToString(),
                    DetectionId = detect.id.ToString(),
                    DetectionName = detect.DetectionName,
                    DateType = t.DateType,
                    OkFormat = t.OkForamt,
                    Value = t.Value,
                    Min = t.Min,
                    Max = t.Max,
                    Uint = t.Uint
                };

                Debug.Assert(t.BindControllerId != null, "t.BindControllerId != null");
                data.BingControllerId = t.BindControllerId.ToString();
                data.BingControllerName = YfasDeviceBase.GetController((int)t.BindControllerId).Name;
                data.BindControllerFieldName = t.BindControllerFieldName;
                data.Offset = t.Offset;

                _datas.Add(data);
            }

            if (_datas.Count > 0)
            {
                uiDataGridView1.ClearRows();
                uiDataGridView1.DataSource = _datas;
                uiDataGridView1.AutoResizeRows();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var notCheckedDetections = _yfasDetectionBaseBll.GetModelList(string.Format("Row = '{0}'", _productModel.RowIndex));
            foreach (var find in _datas.Select(d => notCheckedDetections.Find(f => f.id.ToString() == d.DetectionId)).Where(find => find != null))
                notCheckedDetections.Remove(find);

            var dataType = new[] { "double", "string" };

            var sgm458Fields = YfasDeviceBase.ProductController.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var bindingController = (from f in sgm458Fields where f.FieldType == typeof(int) || f.FieldType == typeof(double) || f.FieldType == typeof(float) || f.FieldType == typeof(string) || f.FieldType == typeof(bool) select string.Format("{0}.{1}", YfasDeviceBase.ProductController.Name, f.Name)).ToList();
            foreach (var t in YfasDeviceBase.YfasIoControllers)
            {
                if (!t.IsUse) continue;
                var fs =
                    t.SyController.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                bindingController.AddRange(from f in fs where f.FieldType == typeof(int) || f.FieldType == typeof(double) || f.FieldType == typeof(float) || f.FieldType == typeof(string) || f.FieldType == typeof(bool) select string.Format("{0}.{1}", t.Name, f.Name));
            }

            var option = new UIEditOption
            {
                AutoLabelWidth = true,
                Text = "添加"
            };
            option.AddCombobox("Detection", "Detection", notCheckedDetections, "DetectionName", "id", notCheckedDetections[0].id);
            option.AddCombobox("DataType", "DataType", dataType, 0, true, true);
            option.AddText("OkFormat", "OkFormat", string.Empty, false);
            option.AddText("Value", "Value", string.Empty, false);
            option.AddText("Min", "Min", string.Empty, false);
            option.AddText("Max", "Max", string.Empty, false);
            option.AddText("Uint", "Uint", string.Empty, false);
            option.AddCombobox("Binding", "Binding", bindingController.ToArray(), 0);
            option.AddText("Offset", "Offset", string.Empty, false);

            var frm = new UIEditForm(option);
            frm.ShowDialog();

            if (frm.IsOK)
            {
                var data = new Data
                {
                    Id = (-1).ToString(),
                    DetectionId = ((int)frm["Detection"]).ToString(),
                    DetectionName = notCheckedDetections.Find(f => f.id == (int)frm["Detection"]).DetectionName,
                    DateType = dataType[(int)frm["DataType"]],
                    OkFormat = frm["OkFormat"].ToString(),
                    Value = frm["Value"].ToString(),
                    Min = frm["Min"].ToString(),
                    Max = frm["Max"].ToString(),
                    Uint = frm["Uint"].ToString(),
                    Offset = frm["Offset"].ToString(),
                };

                var binding = bindingController[(int)frm["Binding"]];
                var controllerName = binding.Split('.')[0];
                var controllerId = 0;
                var find56Pin = YfasDeviceBase.YfasIoControllers.Find(f => f.Name == controllerName);
                if (find56Pin != null)
                    controllerId = find56Pin.Id;
                var controllerField = binding.Split('.')[1];
                data.BingControllerId = controllerId.ToString();
                data.BingControllerName = controllerName;
                data.BindControllerFieldName = controllerField;

                _datas.Add(data);

                if (_datas != null && _datas.Count > 0)
                {
                    uiDataGridView1.ClearRows();
                    uiDataGridView1.DataSource = _datas;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (uiDataGridView1.CurrentRow == null) return;
            var rowIndex = uiDataGridView1.CurrentRow.Index;

            if (rowIndex < 0) return;
            var id = uiDataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            this.ShowErrorDialog("将删除：" + id);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_datas.Any())
            {
                if (this.ShowAskDialog("是否需要保存？"))
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            var newModelList = _datas.Select(d => new Utility._3TierModel.YfasProductParas
                            {
                                id = int.Parse(d.Id),
                                ProductId = _productModel.id,
                                DetectionId = int.Parse(d.DetectionId),
                                DateType = d.DateType,
                                OkForamt = d.OkFormat,
                                Value = d.Value,
                                Min = d.Min,
                                Max = d.Max,
                                Uint = d.Uint,
                                BindControllerId = int.Parse(d.BingControllerId),
                                BindControllerFieldName = d.BindControllerFieldName,
                                Offset = d.Offset,
                                IsDelete = 0
                            }).ToList();

                            //_yfasProductParasBll.Delete()

                            foreach (var model in newModelList)
                            {
                                if (model.id == -1)
                                {
                                    _yfasProductParasBll.Add(model);
                                }
                                else
                                {
                                    _yfasProductParasBll.Update(model);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ShowErrorDialog(ex.Message);
                        }
                    });
                    
                    InitParas(_productModel.id);
                }
            }
        }

        private void btbCopy_Click(object sender, EventArgs e)
        {
            if (_datas.Count > 0)
            {
                this.ShowErrorDialog("请将现有数据全部删除！");
                return;
            }

            var bll = new Utility._3TierBll.YfasProductInfo();
            var models = bll.GetModelList("id != '" + _productModel.id + "'");

            if (models.Any())
            {
                var option = new UIEditOption
                {
                    AutoLabelWidth = true,
                    Text = "添加"
                };
                option.AddCombobox("Product", "Product", models, "Name", "id", models[0].id);

                var frm = new UIEditForm(option);
                frm.ShowDialog();

                if (frm.IsOK)
                {
                    var pid = (int)frm["Product"];
                    _datas.Clear();
                    _yfasProductParas.Clear();
                    _yfasProductParas.AddRange(
                        _yfasProductParasBll.GetModelList(string.Format("ProductId = '{0}'", pid)));

                    foreach (var t in _yfasProductParas)
                    {
                        Debug.Assert(t.DetectionId != null, "t.DetectionId != null");
                        var detect = _yfasDetectionBaseBll.GetModel((int)t.DetectionId);
                        var data = new Data
                        {
                            Id = "-1",
                            DetectionId = detect.id.ToString(),
                            DetectionName = detect.DetectionName,
                            DateType = t.DateType,
                            OkFormat = t.OkForamt,
                            Value = t.Value,
                            Min = t.Min,
                            Max = t.Max,
                            Uint = t.Uint
                        };

                        Debug.Assert(t.BindControllerId != null, "t.BindControllerId != null");
                        data.BingControllerId = t.BindControllerId.ToString();
                        data.BingControllerName = YfasDeviceBase.GetController((int)t.BindControllerId).Name;
                        data.BindControllerFieldName = t.BindControllerFieldName;
                        data.Offset = t.Offset;

                        _datas.Add(data);
                    }

                    if (_datas.Count > 0)
                    {
                        uiDataGridView1.ClearRows();
                        uiDataGridView1.DataSource = _datas;
                        uiDataGridView1.AutoResizeRows();
                    }
                }
            }
        }
    }
}
