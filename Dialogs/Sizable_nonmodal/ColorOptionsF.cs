using System;
using System.Windows.Forms;
using System.Text;


namespace yata
{
	sealed partial class ColorOptionsF
		: YataDialog
	{
		#region cTor
		internal ColorOptionsF(Yata f, string[] lines)
		{
			_f = f;

			InitializeComponent();
			Initialize(METRIC_FUL);

			if (lines.Length != 0)
			{
				var sb = new StringBuilder();
				for (int i = 0; i != lines.Length; ++i)
					sb.AppendLine(lines[i].Trim());

//				rt_Settings.Text = sb.ToString();
			}

//			rt_Settings.Select();
			Show(_f); // Yata is owner.
		}
		#endregion cTor


		#region handlers
		void click_colorpanel(object sender, EventArgs e)
		{
			var d = new ColorSelectorD(this);
			d.ShowDialog(this);
		}
		#endregion handlers
	}
}
