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
		/// Initializes panel-colors and label-texts.
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

			la_15.Text = "Table lines";
			pa_15.BackColor = ColorOptions._tablelines.Color;
			la_16.Text = "Frozen panel lines";
			pa_16.BackColor = ColorOptions._frozenlines.Color;
			la_17.Text = "Frozen header lines";
			pa_17.BackColor = ColorOptions._frozenheadlines.Color;
			la_18.Text = "Frozen header text";
			pa_18.BackColor = ColorOptions._frozenheadtext;
			la_19.Text = "Row panel lines";
			pa_19.BackColor = ColorOptions._rowpanellines.Color;
			la_20.Text = "Col header text";
			pa_20.BackColor = ColorOptions._colheadtext;
			la_21.Text = "Col header lines";
			pa_21.BackColor = ColorOptions._colheadlines.Color;
			la_22.Text = "Propanel lines";
			pa_22.BackColor = ColorOptions._propanellines.Color;
			la_23.Text = "Propanel border";
			pa_23.BackColor = ColorOptions._propanelborder.Color;

			la_24.Text = "Cell selected";
			pa_24.BackColor = (ColorOptions._cellselected as SolidBrush).Color;
			la_25.Text = "Cell loadchanged";
			pa_25.BackColor = (ColorOptions._cellloadchanged as SolidBrush).Color;
			la_26.Text = "Cell diffed";
			pa_26.BackColor = (ColorOptions._celldiffed as SolidBrush).Color;
			la_27.Text = "Cell replaced";
			pa_27.BackColor = (ColorOptions._cellreplaced as SolidBrush).Color;
			la_28.Text = "Cell edit";
			pa_28.BackColor = (ColorOptions._celledit as SolidBrush).Color;
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
		/// Instantiates <c><see cref="ColorSelectorD"/></c> on leftclick.
		/// Restores default color on right click.
		/// </summary>
		/// <param name="sender"><c>pa_*</c></param>
		/// <param name="e"></param>
		void mouseclick_Colorpanel(object sender, MouseEventArgs e)
		{
			switch (e.Button)
			{
				case MouseButtons.Left:
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
					else if (sender == pa_14) title = la_14.Text;

					else if (sender == pa_15) title = la_15.Text;
					else if (sender == pa_16) title = la_16.Text;
					else if (sender == pa_17) title = la_17.Text;
					else if (sender == pa_18) title = la_18.Text;
					else if (sender == pa_19) title = la_19.Text;
					else if (sender == pa_20) title = la_20.Text;
					else if (sender == pa_21) title = la_21.Text;
					else if (sender == pa_22) title = la_22.Text;
					else if (sender == pa_23) title = la_23.Text;
					else if (sender == pa_24) title = la_24.Text;
					else if (sender == pa_25) title = la_25.Text;
					else if (sender == pa_26) title = la_26.Text;
					else if (sender == pa_27) title = la_27.Text;
					else if (sender == pa_28) title = la_28.Text;
					else title = ""; // t

					var f = new ColorSelectorD(this, sender as Panel, " yata - " + title);
					f.ShowDialog(this);

					Yata.that.Refresh();
					break;
				}

				case MouseButtons.Right:
					if (sender == pa_01)
					{
						pa_01.BackColor =
						ColorOptions._tabletext = SystemColors.ControlText;
					}
					else if (sender == pa_02)
					{
						pa_02.BackColor =
						(ColorOptions._rowa as SolidBrush).Color = Color.AliceBlue;
					}
					else if (sender == pa_03)
					{
						pa_03.BackColor =
						(ColorOptions._rowb as SolidBrush).Color = Color.BlanchedAlmond;
					}
					else if (sender == pa_04)
					{
						pa_04.BackColor =
						(ColorOptions._rowdisableda as SolidBrush).Color = Color.LavenderBlush;
					}
					else if (sender == pa_05)
					{
						pa_05.BackColor =
						(ColorOptions._rowdisabledb as SolidBrush).Color = Color.MistyRose;
					}
					else if (sender == pa_06)
					{
						pa_06.BackColor =
						ColorOptions._frozentext = SystemColors.ControlText;
					}
					else if (sender == pa_07)
					{
						pa_07.BackColor =
						ColorOptions._frozen = Color.OldLace;
					}
					else if (sender == pa_08)
					{
						pa_08.BackColor =
						ColorOptions._frozenhead = Color.Moccasin;
					}
					else if (sender == pa_09)
					{
						pa_09.BackColor =
						ColorOptions._colhead = Color.Thistle;
					}
					else if (sender == pa_10)
					{
						pa_10.BackColor =
						ColorOptions._rowpaneltext = SystemColors.ControlText;
					}
					else if (sender == pa_11)
					{
						pa_11.BackColor =
						ColorOptions._rowpanel = Color.Azure;
					}
					else if (sender == pa_12)
					{
						pa_12.BackColor =
						ColorOptions._propaneltext = SystemColors.ControlText;
					}
					else if (sender == pa_13)
					{
						pa_13.BackColor =
						ColorOptions._propanel = Color.LightSteelBlue;
					}
					else if (sender == pa_14)
					{
						pa_14.BackColor =
						(ColorOptions._statusbar as SolidBrush).Color = Color.MintCream;
					}

					else if (sender == pa_15)
					{
						pa_15.BackColor =
						ColorOptions._tablelines.Color = SystemColors.ControlDark;
					}
					else if (sender == pa_16)
					{
						pa_16.BackColor =
						ColorOptions._frozenlines.Color = SystemColors.ControlDark;
					}
					else if (sender == pa_17)
					{
						pa_17.BackColor =
						ColorOptions._frozenheadlines.Color = SystemColors.ControlDark;
					}
					else if (sender == pa_18)
					{
						pa_18.BackColor =
						ColorOptions._frozenheadtext = SystemColors.ControlText;
					}
					else if (sender == pa_19)
					{
						pa_19.BackColor =
						ColorOptions._rowpanellines.Color = SystemColors.ControlDark;
					}
					else if (sender == pa_20)
					{
						pa_20.BackColor =
						ColorOptions._colheadtext = SystemColors.ControlText;
					}
					else if (sender == pa_21)
					{
						pa_21.BackColor =
						ColorOptions._colheadlines.Color = SystemColors.ControlDark;
					}
					else if (sender == pa_22)
					{
						pa_22.BackColor =
						ColorOptions._propanellines.Color = SystemColors.ControlDark;
					}
					else if (sender == pa_23)
					{
						pa_23.BackColor =
						ColorOptions._propanelborder.Color = SystemColors.ControlDarkDark;
					}

					else if (sender == pa_24)
					{
						pa_24.BackColor =
						(ColorOptions._cellselected as SolidBrush).Color = Color.PaleGreen;
					}
					else if (sender == pa_25)
					{
						pa_25.BackColor =
						(ColorOptions._cellloadchanged as SolidBrush).Color = Color.Pink;
					}
					else if (sender == pa_26)
					{
						pa_26.BackColor =
						(ColorOptions._celldiffed as SolidBrush).Color = Color.Turquoise;
					}
					else if (sender == pa_27)
					{
						pa_27.BackColor =
						(ColorOptions._cellreplaced as SolidBrush).Color = Color.Goldenrod;
					}
					else if (sender == pa_28)
					{
						pa_28.BackColor =
						(ColorOptions._celledit as SolidBrush).Color = Color.Crimson;
					}

					Yata.that.Refresh();
					break;
			}
		}

		/// <summary>
		/// Updates a ColorOption.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void backcolorchanged_ColorPanel(object sender, EventArgs e)
		{
			if      (sender == pa_01)  ColorOptions._tabletext                            = pa_01.BackColor;
			else if (sender == pa_02) (ColorOptions._rowa            as SolidBrush).Color = pa_02.BackColor;
			else if (sender == pa_03) (ColorOptions._rowb            as SolidBrush).Color = pa_03.BackColor;
			else if (sender == pa_04) (ColorOptions._rowdisableda    as SolidBrush).Color = pa_04.BackColor;
			else if (sender == pa_05) (ColorOptions._rowdisabledb    as SolidBrush).Color = pa_05.BackColor;
			else if (sender == pa_06)  ColorOptions._frozentext                           = pa_06.BackColor;
			else if (sender == pa_07)  ColorOptions._frozen                               = pa_07.BackColor;
			else if (sender == pa_08)  ColorOptions._frozenhead                           = pa_08.BackColor;
			else if (sender == pa_09)  ColorOptions._colhead                              = pa_09.BackColor;
			else if (sender == pa_10)  ColorOptions._rowpaneltext                         = pa_10.BackColor;
			else if (sender == pa_11)  ColorOptions._rowpanel                             = pa_11.BackColor;
			else if (sender == pa_12)  ColorOptions._propaneltext                         = pa_12.BackColor;
			else if (sender == pa_13)  ColorOptions._propanel                             = pa_13.BackColor;
			else if (sender == pa_14) (ColorOptions._statusbar       as SolidBrush).Color = pa_14.BackColor;

			else if (sender == pa_15)  ColorOptions._tablelines                    .Color = pa_15.BackColor;
			else if (sender == pa_16)  ColorOptions._frozenlines                   .Color = pa_16.BackColor;
			else if (sender == pa_17)  ColorOptions._frozenheadlines               .Color = pa_17.BackColor;
			else if (sender == pa_18)  ColorOptions._frozenheadtext                       = pa_18.BackColor;
			else if (sender == pa_19)  ColorOptions._rowpanellines                 .Color = pa_19.BackColor;
			else if (sender == pa_20)  ColorOptions._colheadtext                          = pa_20.BackColor;
			else if (sender == pa_21)  ColorOptions._colheadlines                  .Color = pa_21.BackColor;
			else if (sender == pa_22)  ColorOptions._propanellines                 .Color = pa_22.BackColor;
			else if (sender == pa_23)  ColorOptions._propanelborder                .Color = pa_23.BackColor;

			else if (sender == pa_24) (ColorOptions._cellselected    as SolidBrush).Color = pa_24.BackColor;
			else if (sender == pa_25) (ColorOptions._cellloadchanged as SolidBrush).Color = pa_25.BackColor;
			else if (sender == pa_26) (ColorOptions._celldiffed      as SolidBrush).Color = pa_26.BackColor;
			else if (sender == pa_27) (ColorOptions._cellreplaced    as SolidBrush).Color = pa_27.BackColor;
			else if (sender == pa_28)
			{
				(ColorOptions._celledit        as SolidBrush).Color = pa_28.BackColor;
				Yata.that.UpdateEditorColor(pa_28.BackColor); // iterate through all YataGrids
			}

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
														"Colors.Cfg success."))
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

			sb.Append(ColorOptions.CFG_tabletext);
			sb.AppendLine(pa_01.BackColor.R + "," + pa_01.BackColor.G + "," + pa_01.BackColor.B);
			sb.Append(ColorOptions.CFG_rowa);
			sb.AppendLine(pa_02.BackColor.R + "," + pa_02.BackColor.G + "," + pa_02.BackColor.B);
			sb.Append(ColorOptions.CFG_rowb);
			sb.AppendLine(pa_03.BackColor.R + "," + pa_03.BackColor.G + "," + pa_03.BackColor.B);
			sb.Append(ColorOptions.CFG_rowdisableda);
			sb.AppendLine(pa_04.BackColor.R + "," + pa_04.BackColor.G + "," + pa_04.BackColor.B);
			sb.Append(ColorOptions.CFG_rowdisabledb);
			sb.AppendLine(pa_05.BackColor.R + "," + pa_05.BackColor.G + "," + pa_05.BackColor.B);
			sb.Append(ColorOptions.CFG_frozentext);
			sb.AppendLine(pa_06.BackColor.R + "," + pa_06.BackColor.G + "," + pa_06.BackColor.B);
			sb.Append(ColorOptions.CFG_frozen);
			sb.AppendLine(pa_07.BackColor.R + "," + pa_07.BackColor.G + "," + pa_07.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenhead);
			sb.AppendLine(pa_08.BackColor.R + "," + pa_08.BackColor.G + "," + pa_08.BackColor.B);
			sb.Append(ColorOptions.CFG_colhead);
			sb.AppendLine(pa_09.BackColor.R + "," + pa_09.BackColor.G + "," + pa_09.BackColor.B);
			sb.Append(ColorOptions.CFG_rowpaneltext);
			sb.AppendLine(pa_10.BackColor.R + "," + pa_10.BackColor.G + "," + pa_10.BackColor.B);
			sb.Append(ColorOptions.CFG_rowpanel);
			sb.AppendLine(pa_11.BackColor.R + "," + pa_11.BackColor.G + "," + pa_11.BackColor.B);
			sb.Append(ColorOptions.CFG_propaneltext);
			sb.AppendLine(pa_12.BackColor.R + "," + pa_12.BackColor.G + "," + pa_12.BackColor.B);
			sb.Append(ColorOptions.CFG_propanel);
			sb.AppendLine(pa_13.BackColor.R + "," + pa_13.BackColor.G + "," + pa_13.BackColor.B);
			sb.Append(ColorOptions.CFG_statusbar);
			sb.AppendLine(pa_14.BackColor.R + "," + pa_14.BackColor.G + "," + pa_14.BackColor.B);

			sb.Append(ColorOptions.CFG_tablelines);
			sb.AppendLine(pa_15.BackColor.R + "," + pa_15.BackColor.G + "," + pa_15.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenlines);
			sb.AppendLine(pa_16.BackColor.R + "," + pa_16.BackColor.G + "," + pa_16.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenheadlines);
			sb.AppendLine(pa_17.BackColor.R + "," + pa_17.BackColor.G + "," + pa_17.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenheadtext);
			sb.AppendLine(pa_18.BackColor.R + "," + pa_18.BackColor.G + "," + pa_18.BackColor.B);
			sb.Append(ColorOptions.CFG_rowpanellines);
			sb.AppendLine(pa_19.BackColor.R + "," + pa_19.BackColor.G + "," + pa_19.BackColor.B);
			sb.Append(ColorOptions.CFG_colheadtext);
			sb.AppendLine(pa_20.BackColor.R + "," + pa_20.BackColor.G + "," + pa_20.BackColor.B);
			sb.Append(ColorOptions.CFG_colheadlines);
			sb.AppendLine(pa_21.BackColor.R + "," + pa_21.BackColor.G + "," + pa_21.BackColor.B);
			sb.Append(ColorOptions.CFG_propanellines);
			sb.AppendLine(pa_22.BackColor.R + "," + pa_22.BackColor.G + "," + pa_22.BackColor.B);
			sb.Append(ColorOptions.CFG_propanelborder);
			sb.AppendLine(pa_23.BackColor.R + "," + pa_23.BackColor.G + "," + pa_23.BackColor.B);

			sb.Append(ColorOptions.CFG_cellselected);
			sb.AppendLine(pa_24.BackColor.R + "," + pa_24.BackColor.G + "," + pa_24.BackColor.B);
			sb.Append(ColorOptions.CFG_cellloadchanged);
			sb.AppendLine(pa_25.BackColor.R + "," + pa_25.BackColor.G + "," + pa_25.BackColor.B);
			sb.Append(ColorOptions.CFG_celldiffed);
			sb.AppendLine(pa_26.BackColor.R + "," + pa_26.BackColor.G + "," + pa_26.BackColor.B);
			sb.Append(ColorOptions.CFG_cellreplaced);
			sb.AppendLine(pa_27.BackColor.R + "," + pa_27.BackColor.G + "," + pa_27.BackColor.B);
			sb.Append(ColorOptions.CFG_celledit);
			sb.AppendLine(pa_28.BackColor.R + "," + pa_28.BackColor.G + "," + pa_28.BackColor.B);

			return sb.ToString();
		}


		/// <summary>
		/// Restores all colors to defaults.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Defaults"/></c></param>
		/// <param name="e"></param>
		void click_Defaults(object sender, EventArgs e)
		{
			 pa_01.BackColor                                    =
			 ColorOptions._tabletext                            = SystemColors.ControlText;
			 pa_02.BackColor                                    =
			(ColorOptions._rowa            as SolidBrush).Color = Color.AliceBlue;
			 pa_03.BackColor                                    =
			(ColorOptions._rowb            as SolidBrush).Color = Color.BlanchedAlmond;
			 pa_04.BackColor                                    =
			(ColorOptions._rowdisableda    as SolidBrush).Color = Color.LavenderBlush;
			 pa_05.BackColor                                    =
			(ColorOptions._rowdisabledb    as SolidBrush).Color = Color.MistyRose;
			 pa_06.BackColor                                    =
			 ColorOptions._frozentext                           = SystemColors.ControlText;
			 pa_07.BackColor                                    =
			 ColorOptions._frozen                               = Color.OldLace;
			 pa_08.BackColor                                    =
			 ColorOptions._frozenhead                           = Color.Moccasin;
			 pa_09.BackColor                                    =
			 ColorOptions._colhead                              = Color.Thistle;
			 pa_10.BackColor                                    =
			 ColorOptions._rowpaneltext                         = SystemColors.ControlText;
			 pa_11.BackColor                                    =
			 ColorOptions._rowpanel                             = Color.Azure;
			 pa_12.BackColor                                    =
			 ColorOptions._propaneltext                         = SystemColors.ControlText;
			 pa_13.BackColor                                    =
			 ColorOptions._propanel                             = Color.LightSteelBlue;
			 pa_14.BackColor                                    =
			(ColorOptions._statusbar       as SolidBrush).Color = Color.MintCream;

			 pa_15.BackColor                                    =
			 ColorOptions._tablelines                    .Color = SystemColors.ControlDark;
			 pa_16.BackColor                                    =
			 ColorOptions._frozenlines                   .Color = SystemColors.ControlDark;
			 pa_17.BackColor                                    =
			 ColorOptions._frozenheadlines               .Color = SystemColors.ControlDark;
			 pa_18.BackColor                                    =
			 ColorOptions._frozenheadtext                       = SystemColors.ControlText;
			 pa_19.BackColor                                    =
			 ColorOptions._rowpanellines                 .Color = SystemColors.ControlDark;
			 pa_20.BackColor                                    =
			 ColorOptions._colheadtext                          = SystemColors.ControlText;
			 pa_21.BackColor                                    =
			 ColorOptions._colheadlines                  .Color = SystemColors.ControlDark;
			 pa_22.BackColor                                    =
			 ColorOptions._propanellines                 .Color = SystemColors.ControlDark;
			 pa_23.BackColor                                    =
			 ColorOptions._propanelborder                .Color = SystemColors.ControlDarkDark;

			 pa_24.BackColor                                    =
			(ColorOptions._cellselected    as SolidBrush).Color = Color.PaleGreen;
			 pa_25.BackColor                                    =
			(ColorOptions._cellloadchanged as SolidBrush).Color = Color.Pink;
			 pa_26.BackColor                                    =
			(ColorOptions._celldiffed      as SolidBrush).Color = Color.Turquoise;
			 pa_27.BackColor                                    =
			(ColorOptions._cellreplaced    as SolidBrush).Color = Color.Goldenrod;
			 pa_28.BackColor                                    =
			(ColorOptions._celledit        as SolidBrush).Color = Color.Crimson;

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
