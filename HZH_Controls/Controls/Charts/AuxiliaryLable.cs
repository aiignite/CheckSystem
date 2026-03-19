using System.Drawing;

namespace HZH_Controls.Controls.Charts
{
	public class AuxiliaryLable
	{
		public string Text
		{
			get;
			set;
		}

		public Brush TextBrush
		{
			get;
			set;
		}

		public Brush TextBack
		{
			get;
			set;
		}

		public float LocationX
		{
			get;
			set;
		}

		public AuxiliaryLable()
		{
			TextBrush = Brushes.Black;
			TextBack = Brushes.Transparent;
			LocationX = 0.5f;
		}
	}
}
