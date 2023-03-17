using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text;


namespace yata
{
	sealed partial class ColorOptionsF
		: YataDialog
	{
		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="lines"></param>
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

			init();

			bu_Cancel.Select();
			Show(_f); // Yata is owner.
		}

		/// <summary>
		/// 
		/// </summary>
		void init()
		{
			la_01.Text = "Table text";
			pa_01.BackColor = ColorOptions._tabletext;
			la_02.Text = "Row a";
			pa_02.BackColor = (ColorOptions._rowa as SolidBrush).Color;
			la_03.Text = "Row b";
			pa_03.BackColor = (ColorOptions._rowb as SolidBrush).Color;
			la_04.Text = "Row disabled a";
			pa_04.BackColor = (ColorOptions._rowdisableda as SolidBrush).Color;
			la_05.Text = "Row disabled b";
			pa_05.BackColor = (ColorOptions._rowdisabledb as SolidBrush).Color;
			la_06.Text = "Frozen panel text";
			pa_06.BackColor = ColorOptions._frozentext;
			la_07.Text = "Frozen panel";
			pa_07.BackColor = ColorOptions._frozen;
			la_08.Text = "Frozen header";
			pa_08.BackColor = ColorOptions._frozenhead;
			la_09.Text = "Col header";
			pa_09.BackColor = ColorOptions._colhead;
			la_10.Text = "Row panel text";
			pa_10.BackColor = ColorOptions._rowpaneltext;
			la_11.Text = "Row panel";
			pa_11.BackColor = ColorOptions._rowpanel;
			la_12.Text = "Propanel text";
			pa_12.BackColor = ColorOptions._propaneltext;
			la_13.Text = "Propanel";
			pa_13.BackColor = ColorOptions._propanel;
			la_14.Text = "Statusbar";
			pa_14.BackColor = (ColorOptions._statusbar as SolidBrush).Color;
		}
		#endregion cTor


		#region handlers
		/// <summary>
		/// Instantiates <c><see cref="ColorSelectorD"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_1"/></c></param>
		/// <param name="e"></param>
		void click_Colorpanel(object sender, EventArgs e)
		{
			var f = new ColorSelectorD(this, sender as Panel);
			f.ShowDialog(this);
		}

		/// <summary>
		/// Writes current colors to "Colors.Cfg" file.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Commit"/></c></param>
		/// <param name="e"></param>
		void click_Commit(object sender, EventArgs e)
		{

			Close();
		}
		#endregion handlers
	}
}
