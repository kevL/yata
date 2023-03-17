using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;


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
		/// <param name="sender"><c>pa_*</c></param>
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
			try
			{
				string pfeT = ColorOptions.FE;
					   pfeT = Path.Combine(Application.StartupPath, pfeT) + ".t";

				File.WriteAllText(pfeT, BuildFile());

				if (File.Exists(pfeT))
				{
					string pfe = ColorOptions.FE;
						   pfe = Path.Combine(Application.StartupPath, pfe);

					File.Delete(pfe);
					File.Copy(pfeT, pfe);

					if (File.Exists(pfe))
					{
						File.Delete(pfeT);

						using (var ib = new Infobox(Infobox.Title_infor,
													"Yata must be restarted for any changes to take effect.",
													null,
													InfoboxType.Warn))
						{
							ib.ShowDialog(this);
						}
					}
				}
			}
			catch (Exception ex)
			{
				using (var ib = new Infobox(Infobox.Title_excep,
											"The config file could not be written to the application directory.",
											ex.ToString(),
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
			finally
			{
				Close();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		string BuildFile()
		{
			var sb = new StringBuilder();

			sb.Append("tabletext=");
			sb.AppendLine(pa_01.BackColor.R + "," + pa_01.BackColor.G + "," + pa_01.BackColor.B);
			sb.Append("rowa=");
			sb.AppendLine(pa_02.BackColor.R + "," + pa_02.BackColor.G + "," + pa_02.BackColor.B);
			sb.Append("rowb=");
			sb.AppendLine(pa_03.BackColor.R + "," + pa_03.BackColor.G + "," + pa_03.BackColor.B);
			sb.Append("rowdisableda=");
			sb.AppendLine(pa_04.BackColor.R + "," + pa_04.BackColor.G + "," + pa_04.BackColor.B);
			sb.Append("rowdisabledb=");
			sb.AppendLine(pa_05.BackColor.R + "," + pa_05.BackColor.G + "," + pa_05.BackColor.B);
			sb.Append("frozentext=");
			sb.AppendLine(pa_06.BackColor.R + "," + pa_06.BackColor.G + "," + pa_06.BackColor.B);
			sb.Append("frozen=");
			sb.AppendLine(pa_07.BackColor.R + "," + pa_07.BackColor.G + "," + pa_07.BackColor.B);
			sb.Append("frozenhead=");
			sb.AppendLine(pa_08.BackColor.R + "," + pa_08.BackColor.G + "," + pa_08.BackColor.B);
			sb.Append("colhead=");
			sb.AppendLine(pa_09.BackColor.R + "," + pa_09.BackColor.G + "," + pa_09.BackColor.B);
			sb.Append("rowpaneltext=");
			sb.AppendLine(pa_10.BackColor.R + "," + pa_10.BackColor.G + "," + pa_10.BackColor.B);
			sb.Append("rowpanel=");
			sb.AppendLine(pa_11.BackColor.R + "," + pa_11.BackColor.G + "," + pa_11.BackColor.B);
			sb.Append("propaneltext=");
			sb.AppendLine(pa_12.BackColor.R + "," + pa_12.BackColor.G + "," + pa_12.BackColor.B);
			sb.Append("propanel=");
			sb.AppendLine(pa_13.BackColor.R + "," + pa_13.BackColor.G + "," + pa_13.BackColor.B);
			sb.Append("statusbar=");
			sb.AppendLine(pa_14.BackColor.R + "," + pa_14.BackColor.G + "," + pa_14.BackColor.B);

			return sb.ToString();
		}
		#endregion handlers
	}
}
