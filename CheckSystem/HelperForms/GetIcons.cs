using System.Drawing;
using System.Windows.Forms;
using CommonUtility;
using HZH_Controls.IconFont;

namespace CheckSystem.HelperForms
{
    public partial class GetIcons : Form
    {
        public GetIcons()
        {
            InitializeComponent();

            //FontImages.GetIcon(HZH_Controls.FontIcons.A_fa_magnet, 32,
            //    Color.DodgerBlue);

            var icons = EnumOperater.GetEnumValueList<FontIcons>();
            for (var i = 0; i < icons.Count; i++)
            {
                var icon = icons[i];

                imageList1.Images.Add(FontImages.GetImage(icon, 44, Color.DodgerBlue));

                listView1.Items.Add(icon.ToString());
                listView1.Items[i].ImageIndex = i;
            }
        }
    }
}
