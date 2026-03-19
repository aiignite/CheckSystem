using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserControls
{
    public partial class UserToolStrip : UserControl
    {
        public UserToolStrip()
        {
            InitializeComponent();

            ADD = toolStripButtonAdd.Text;
            EDIT = toolStripButtonEdit.Text;
            DELETE = toolStripButtonDel.Text;
            SAVE = toolStripButtonSave.Text;
            CANCEL = toolStripButtonCancel.Text;
            SEARCH = toolStripButtonSearch.Text;
            REFRESH = toolStripButtonRefresh.Text;
            EXPORTEXCEL = toolStripButtonExportExcel.Text;
            IMPORTEXCEL = toolStripButtonImportExcel.Text;

            listToolStripButtonName.Add(ADD);
            listToolStripButtonName.Add(EDIT);
            listToolStripButtonName.Add(DELETE);
            listToolStripButtonName.Add(SAVE);
            listToolStripButtonName.Add(CANCEL);
            listToolStripButtonName.Add(SEARCH);
            listToolStripButtonName.Add(REFRESH);
            listToolStripButtonName.Add(EXPORTEXCEL);
            listToolStripButtonName.Add(IMPORTEXCEL);

            listButton.Add(toolStripButtonAdd);
            listButton.Add(toolStripButtonCancel);
            listButton.Add(toolStripButtonDel);
            listButton.Add(toolStripButtonEdit);
            listButton.Add(toolStripButtonExportExcel);
            listButton.Add(toolStripButtonImportExcel);
            listButton.Add(toolStripButtonRefresh);
            listButton.Add(toolStripButtonSave);
            listButton.Add(toolStripButtonSearch);

        }

        /// <summary>
        /// 按钮状态管理常量和变量
        /// </summary>        
        public string strButtonName = "";
        public static string ADD = "ADD";
        public static string EDIT = "EDIT";
        public static string DELETE = "DELETE";
        public static string SAVE = "SAVE";
        public static string CANCEL = "CANCEL";
        public static string SEARCH = "SEARCH";
        public static string REFRESH = "REFRESH";
        public static string EXPORTEXCEL = "EXPORTEXCEL";
        public static string IMPORTEXCEL = "IMPORTEXCEL";

        public List<string> listToolStripButtonName = new List<string>();
        public List<ToolStripButton> listButton = new List<ToolStripButton>();

        public bool bAdd = false;

        public event EventHandler OnToolStripChanged;


        /// <summary>
        /// 控件使能控制
        /// </summary>
        /// <param name="iButtonIndex"></param>
        private void ToolStripButtonStatus(string strButtonNameNow)
        {
            if (strButtonNameNow == ADD)
            {
                bAdd = true;
                toolStripButtonAdd.Enabled = true;
                toolStripButtonSave.Enabled = true;
                toolStripButtonCancel.Enabled = true;
                toolStripButtonEdit.Enabled = false;
                toolStripButtonDel.Enabled = false;
            }
            else if (strButtonNameNow == EDIT)
            {
                bAdd = false;
                toolStripButtonAdd.Enabled = false;
                toolStripButtonSave.Enabled = true;
                toolStripButtonCancel.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonDel.Enabled = false;
            }
            else if (strButtonNameNow == DELETE)
            {
                toolStripButtonAdd.Enabled = true;
                toolStripButtonSave.Enabled = true;
                toolStripButtonCancel.Enabled = false;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonDel.Enabled = true;

            }
            else if (strButtonNameNow == SAVE)
            {
                toolStripButtonAdd.Enabled = true;
                toolStripButtonSave.Enabled = true;
                toolStripButtonCancel.Enabled = false;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonDel.Enabled = true;
            }
            else if (strButtonNameNow == CANCEL)
            {
                toolStripButtonAdd.Enabled = true;
                toolStripButtonSave.Enabled = true;
                toolStripButtonCancel.Enabled = false;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonDel.Enabled = true;
            }
            else
            { }
        }
        public void SetButtonVisible(List<string> listButtonName)
        {
            foreach (var item in listButton)
            {
                if (listButtonName.Find(e => e == item.Text) != null)
                {
                    item.Visible = true;
                }
                else
                {
                    item.Visible = false;
                }
            }
        }

        public void SetComboBoxColValue(List<string> listComboBoxValue)
        {
            toolStripComboBoxCol.Items.Clear();
            // toolStripComboBoxCol.Items.AddRange(listComboBoxValue);
            foreach (string strText in listComboBoxValue)
            {
                toolStripComboBoxCol.Items.Add(strText);
            }
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            strButtonName = ADD;
            ToolStripButtonStatus(strButtonName);
            OnToolStripChanged(this, null);
        }

        private void toolStripButtonDel_Click(object sender, EventArgs e)
        {
            strButtonName = DELETE;
            ToolStripButtonStatus(strButtonName);
            OnToolStripChanged(this, null);
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            strButtonName = EDIT;
            ToolStripButtonStatus(strButtonName);
            OnToolStripChanged(this, null);
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            strButtonName = SAVE;
            ToolStripButtonStatus(strButtonName);
            OnToolStripChanged(this, null);
        }

        private void toolStripButtonCancel_Click(object sender, EventArgs e)
        {
            strButtonName = CANCEL;
            ToolStripButtonStatus(strButtonName);
            OnToolStripChanged(this, null);
        }

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            strButtonName = SEARCH;
            OnToolStripChanged(this, null);
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            strButtonName = REFRESH;
            OnToolStripChanged(this, null);
        }

        private void toolStripButtonExportExcel_Click(object sender, EventArgs e)
        {
            strButtonName = EXPORTEXCEL;
            OnToolStripChanged(this, null);
        }

        private void toolStripButtonImportExcel_Click(object sender, EventArgs e)
        {
            strButtonName = IMPORTEXCEL;
            OnToolStripChanged(this, null);
        }


    }
}
