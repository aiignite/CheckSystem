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
    public partial class LabelDateTimePicker : UserControl
    {

        public override string Text
        {
            get
            {
                return dateTimePicker.Text;
            }

            set
            {
                try
                {
                    dateTimePicker.Value = Convert.ToDateTime(value);
                }
                catch
                {
                }
            }
        }

        //public virtual string TEXT
        //{
        //    get { return dateTimePicker.Text; }
        //    set { dateTimePicker.Value = Convert.ToDateTime(value); }
        //}
        public LabelDateTimePicker()
        {
            InitializeComponent();
        }
        public void SetText(string strText)
        {
            dateTimePicker.Text = strText;
        }
    }
}
