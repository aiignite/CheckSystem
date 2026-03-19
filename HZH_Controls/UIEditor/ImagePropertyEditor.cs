using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace HZH_Controls.UIEditor
{
    public class ImagePropertyEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            //指定为模式窗体属性编辑器类型
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            //打开属性编辑器修改数据
            var frm = new FrmSelectImage();
            if (value != null && !(value is Image))
                throw new Exception("这不是一个FontIcons类型的属性");
            if (value != null)
                frm.SelectImage = (Image)value;
            return
                frm.ShowDialog() == System.Windows.Forms.DialogResult.OK ? frm.SelectImage : value;
        }
    }
}
