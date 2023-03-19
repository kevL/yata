﻿using System;
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
		internal ColorOptionsF(Yata f)
		{
			_f = f;

			InitializeComponent();
			Initialize(METRIC_FUL);

			init();

			bu_Save.Select();
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


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>FormClosing</c> eventhandler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			(_f as Yata).CloseColorOptions();
			base.OnFormClosing(e);
		}

		/// <summary>
		/// Saves current text to the Colors.Cfg file when <c>[Ctrl+s]</c> is
		/// pressed.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Requires <c>KeyPreview</c> <c>true</c>.</remarks>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyData == (Keys.Control | Keys.S))
			{
				e.SuppressKeyPress = true;
				click_Save(null, EventArgs.Empty);
			}
			else
				base.OnKeyDown(e);
		}
		#endregion Handlers (override)


		#region handlers (color panel)
		/// <summary>
		/// Instantiates <c><see cref="ColorSelectorD"/></c>.
		/// </summary>
		/// <param name="sender"><c>pa_*</c></param>
		/// <param name="e"></param>
		void click_Colorpanel(object sender, EventArgs e)
		{
			string title;

			if      (sender == pa_01) title = la_01.Text;
			else if (sender == pa_02) title = la_02.Text;
			else if (sender == pa_03) title = la_03.Text;
			else if (sender == pa_04) title = la_04.Text;
			else if (sender == pa_05) title = la_05.Text;
			else if (sender == pa_06) title = la_06.Text;
			else if (sender == pa_07) title = la_07.Text;
			else if (sender == pa_08) title = la_08.Text;
			else if (sender == pa_09) title = la_09.Text;
			else if (sender == pa_10) title = la_10.Text;
			else if (sender == pa_11) title = la_11.Text;
			else if (sender == pa_12) title = la_12.Text;
			else if (sender == pa_13) title = la_13.Text;
			else                      title = la_14.Text; // sender == pa_14

			var f = new ColorSelectorD(this, sender as Panel, " yata - " + title);
			f.ShowDialog(this);
		}

		/// <summary>
		/// Updates a ColorOption.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void backcolorchanged_ColorPanel(object sender, EventArgs e)
		{
			if      (sender == pa_01)  ColorOptions._tabletext                         = pa_01.BackColor;
			else if (sender == pa_02) (ColorOptions._rowa         as SolidBrush).Color = pa_02.BackColor;
			else if (sender == pa_03) (ColorOptions._rowb         as SolidBrush).Color = pa_03.BackColor;
			else if (sender == pa_04) (ColorOptions._rowdisableda as SolidBrush).Color = pa_04.BackColor;
			else if (sender == pa_05) (ColorOptions._rowdisabledb as SolidBrush).Color = pa_05.BackColor;
			else if (sender == pa_06)  ColorOptions._frozentext                        = pa_06.BackColor;
			else if (sender == pa_07)  ColorOptions._frozen                            = pa_07.BackColor;
			else if (sender == pa_08)  ColorOptions._frozenhead                        = pa_08.BackColor;
			else if (sender == pa_09)  ColorOptions._colhead                           = pa_09.BackColor;
			else if (sender == pa_10)  ColorOptions._rowpaneltext                      = pa_10.BackColor;
			else if (sender == pa_11)  ColorOptions._rowpanel                          = pa_11.BackColor;
			else if (sender == pa_12)  ColorOptions._propaneltext                      = pa_12.BackColor;
			else if (sender == pa_13)  ColorOptions._propanel                          = pa_13.BackColor;
			else                      (ColorOptions._statusbar    as SolidBrush).Color = pa_14.BackColor; // sender == pa_14

			Yata.that.Refresh();
		}
		#endregion handlers (color panel)


		#region handlers (buttons)
		/// <summary>
		/// Saves current colors to the "Colors.Cfg" file.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Save"/></c></param>
		/// <param name="e"></param>
		void click_Save(object sender, EventArgs e)
		{
			try
			{
				string pfeT = Path.Combine(Application.StartupPath, ColorOptions.FE) + ".t";

				File.WriteAllText(pfeT, BuildFile());

				if (File.Exists(pfeT))
				{
					string pfe = Path.Combine(Application.StartupPath, ColorOptions.FE);

					File.Delete(pfe);
					File.Copy(pfeT, pfe);

					if (File.Exists(pfe))
					{
						File.Delete(pfeT);

						if (sender == null) // inform user only if [Ctrl+s] was pressed ->
						{
							using (var ib = new Infobox(Infobox.Title_infor,
														"Colors.Cfg file was written to the application directory.",
														null,
														InfoboxType.Warn))
							{
								ib.ShowDialog(this);
							}
						}
					}
				}
			}
			catch (Exception ex) // handle locked etc. or file is flagged Readonly
			{
				using (var ib = new Infobox(Infobox.Title_excep,
											"Colors.Cfg file could not be written to the application directory.",
											ex.ToString(),
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
			finally
			{
				if (sender != null) Close();
			}
		}

		/// <summary>
		/// Builds text for the "Colors.Cfg" file.
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


		/// <summary>
		/// Restores all colors to defaults.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Defaults"/></c></param>
		/// <param name="e"></param>
		void click_Defaults(object sender, EventArgs e)
		{
			 pa_01.BackColor                                 =
			 ColorOptions._tabletext                         = SystemColors.ControlText;
			 pa_02.BackColor                                 =
			(ColorOptions._rowa         as SolidBrush).Color = Color.AliceBlue;
			 pa_03.BackColor                                 =
			(ColorOptions._rowb         as SolidBrush).Color = Color.BlanchedAlmond;
			 pa_04.BackColor                                 =
			(ColorOptions._rowdisableda as SolidBrush).Color = Color.LavenderBlush;
			 pa_05.BackColor                                 =
			(ColorOptions._rowdisabledb as SolidBrush).Color = Color.MistyRose;
			 pa_06.BackColor                                 =
			 ColorOptions._frozentext                        = SystemColors.ControlText;
			 pa_07.BackColor                                 =
			 ColorOptions._frozen                            = Color.OldLace;
			 pa_08.BackColor                                 =
			 ColorOptions._frozenhead                        = Color.Moccasin;
			 pa_09.BackColor                                 =
			 ColorOptions._colhead                           = Color.Thistle;
			 pa_10.BackColor                                 =
			 ColorOptions._rowpaneltext                      = SystemColors.ControlText;
			 pa_11.BackColor                                 =
			 ColorOptions._rowpanel                          = Color.Azure;
			 pa_12.BackColor                                 =
			 ColorOptions._propaneltext                      = SystemColors.ControlText;
			 pa_13.BackColor                                 =
			 ColorOptions._propanel                          = Color.LightSteelBlue;
			 pa_14.BackColor                                 =
			(ColorOptions._statusbar    as SolidBrush).Color = Color.MintCream;

			Yata.that.Refresh();
		}

		/// <summary>
		/// Deletes the "Colors.Cfg" file from the application folder.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Delete"/></c></param>
		/// <param name="e"></param>
		void click_Delete(object sender, EventArgs e)
		{
			string pfe = Path.Combine(Application.StartupPath, ColorOptions.FE);
			if (File.Exists(pfe))
			{
				try
				{
					File.Delete(pfe);
				}
				catch (Exception ex)
				{
					using (var ib = new Infobox(Infobox.Title_excep,
												"Colors.Cfg file could not be deleted from the application directory.",
												ex.ToString(),
												InfoboxType.Error))
					{
						ib.ShowDialog(this);
					}
				}
			}
		}
		#endregion handlers (buttons)
	}
}
