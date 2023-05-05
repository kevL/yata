using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ColorOptionsEditor
		: YataDialog
	{
		#region Fields (static)
		/// <summary>
		/// Cache for the user to store a color.
		/// </summary>
		internal static Color Stored = Color.Empty;

		/// <summary>
		/// The longest help-string in <c><see cref="la_Help"/></c>.
		/// </summary>
		/// <remarks>Used to set the width of <c>la_Help</c>.</remarks>
		const string HELP = "[Shift]+RMB - restore default color";
		#endregion Fields (static)


		#region Fields
		bool _init = true;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"></param>
		internal ColorOptionsEditor(Yata f)
		{
			_f = f;

			InitializeComponent();
			Initialize(METRIC_LOC);

			InitializeColorLabels();
			InitializeColorPanels();

			pa_text1.Width = YataGraphics.MeasureWidth(la_44.Text, Font) + 17;
			pa_text2.Width = YataGraphics.MeasureWidth(la_38.Text, Font) + 17;

			pa_text1 .Left = 5;
			pa_color1.Left = pa_text1 .Right;
			pa_text2 .Left = pa_color1.Right + 14;
			pa_color2.Left = pa_text2 .Right;

			ClientSize = new Size(pa_color2.Right + 7, ClientSize.Height);
			_init = false;

			la_Help.Width = YataGraphics.MeasureWidth(HELP, Font) + 20;
			la_Help.Left  = gb_Colors.Width - la_Help.Width - 7;

			bu_Save.Select();
			Show(_f); // Yata is owner.
		}

		/// <summary>
		/// Initializes the textual <c>Labels</c> that describe various elements
		/// for which the color can be customized.
		/// </summary>
		void InitializeColorLabels()
		{
			// Table ->
			la_15.Text = "Row lines";
			la_02.Text = "Row a";
			la_03.Text = "Row b";
			la_04.Text = "Row disabled a";
			la_05.Text = "Row disabled b";
			la_37.Text = "Row created";

			// Frozen ->
			la_16.Text = "Frozen panel lines";
			la_07.Text = "Frozen panel";
			la_17.Text = "Frozen header lines";
			la_08.Text = "Frozen header";
			la_29.Text = "Frozen id unsorted";
			la_42.Text = "Frozen header gradient a";
			la_43.Text = "Frozen header gradient b";
			la_44.Text = "Frozen id unsorted gradient a";
			la_45.Text = "Frozen id unsorted gradient b";

			// Colhead ->
			la_21.Text = "Col header lines";
			la_09.Text = "Col header";
			la_30.Text = "Col header selected";
			la_31.Text = "Col header resized";
			la_32.Text = "Header sorted ascending";
			la_33.Text = "Header sorted descending";
			la_40.Text = "Col header gradient a";
			la_41.Text = "Col header gradient b";

			// Rowpanel ->
			la_19.Text = "Row panel lines";
			la_11.Text = "Row panel";
			la_35.Text = "Row panel selected";
			la_36.Text = "Row panel subselected";

			// Propanel ->
			la_22.Text = "Propanel lines";
			la_23.Text = "Propanel border";
			la_13.Text = "Propanel";
			la_34.Text = "Propanel frozen cell";
			la_38.Text = "Propanel selected cell";

			// Cells ->
			la_24.Text = "Cell selected";
			la_25.Text = "Cell loadchanged";
			la_26.Text = "Cell diffed";
			la_27.Text = "Cell replaced";
			la_28.Text = "Cell edit";

			// Statusbar ->
			la_14.Text = "Statusbar";
		}

		/// <summary>
		/// Initializes the color <c>Panels</c> with the current color of
		/// their corresponding elements in <c><see cref="ColorOptions"/></c>.
		/// </summary>
		void InitializeColorPanels()
		{
			// Table ->
			pa_15  .BackColor =  ColorOptions._rowlines                      .Color;
			pa_02  .BackColor = (ColorOptions._rowa            as SolidBrush).Color;
			pa_02_t.BackColor =  ColorOptions._rowa_t;
			pa_03  .BackColor = (ColorOptions._rowb            as SolidBrush).Color;
			pa_03_t.BackColor =  ColorOptions._rowb_t;
			pa_04  .BackColor = (ColorOptions._rowdisableda    as SolidBrush).Color;
			pa_04_t.BackColor =  ColorOptions._rowdisableda_t;
			pa_05  .BackColor = (ColorOptions._rowdisabledb    as SolidBrush).Color;
			pa_05_t.BackColor =  ColorOptions._rowdisabledb_t;
			pa_37  .BackColor = (ColorOptions._rowcreated      as SolidBrush).Color;
			pa_37_t.BackColor =  ColorOptions._rowcreated_t;

			// Frozen ->
			pa_16  .BackColor =  ColorOptions._frozenlines                   .Color;
			pa_07  .BackColor =  ColorOptions._frozen;
			pa_07_t.BackColor =  ColorOptions._frozen_t;
			pa_17  .BackColor =  ColorOptions._frozenheadlines               .Color;
			pa_08  .BackColor =  ColorOptions._frozenhead;
			pa_08_t.BackColor =  ColorOptions._frozenhead_t;
			pa_29  .BackColor =  ColorOptions._frozenidunsort;
			pa_29_t.BackColor =  ColorOptions._frozenidunsort_t;
			pa_42  .BackColor =  ColorOptions._frozenheadgrada;
			pa_43  .BackColor =  ColorOptions._frozenheadgradb;
			pa_44  .BackColor =  ColorOptions._frozenidgrada;
			pa_45  .BackColor =  ColorOptions._frozenidgradb;

			// Colhead ->
			pa_21  .BackColor =  ColorOptions._colheadlines                  .Color;
			pa_09  .BackColor =  ColorOptions._colhead;
			pa_09_t.BackColor =  ColorOptions._colhead_t;
			pa_30  .BackColor =  ColorOptions._colheadsel_t;
			pa_31  .BackColor =  ColorOptions._colheadsize_t;
			pa_32  .BackColor =  ColorOptions._headsortasc_t;
			pa_33  .BackColor =  ColorOptions._headsortdes_t;
			pa_40  .BackColor =  ColorOptions._colheadgrada;
			pa_41  .BackColor =  ColorOptions._colheadgradb;

			// Rowpanel ->
			pa_19  .BackColor =  ColorOptions._rowpanellines                 .Color;
			pa_11  .BackColor =  ColorOptions._rowpanel;
			pa_11_t.BackColor =  ColorOptions._rowpanel_t;
			pa_35  .BackColor = (ColorOptions._rowsel          as SolidBrush).Color;
			pa_35_t.BackColor =  ColorOptions._rowsel_t;
			pa_36  .BackColor = (ColorOptions._rowsubsel       as SolidBrush).Color;
			pa_36_t.BackColor =  ColorOptions._rowsubsel_t;

			// Propanel ->
			pa_22  .BackColor =  ColorOptions._propanellines                 .Color;
			pa_23  .BackColor =  ColorOptions._propanelborder                .Color;
			pa_13  .BackColor =  ColorOptions._propanel;
			pa_13_t.BackColor =  ColorOptions._propanel_t;
			pa_34  .BackColor = (ColorOptions._propanelfrozen  as SolidBrush).Color;
			pa_34_t.BackColor =  ColorOptions._propanelfrozen_t;
			pa_38  .BackColor = (ColorOptions._propanelsel     as SolidBrush).Color;
			pa_38_t.BackColor =  ColorOptions._propanelsel_t;

			// Cells ->
			pa_24  .BackColor = (ColorOptions._cellselected    as SolidBrush).Color;
			pa_24_t.BackColor =  ColorOptions._cellselected_t;
			pa_25  .BackColor = (ColorOptions._cellloadchanged as SolidBrush).Color;
			pa_25_t.BackColor =  ColorOptions._cellloadchanged_t;
			pa_26  .BackColor = (ColorOptions._celldiffed      as SolidBrush).Color;
			pa_26_t.BackColor =  ColorOptions._celldiffed_t;
			pa_27  .BackColor = (ColorOptions._cellreplaced    as SolidBrush).Color;
			pa_27_t.BackColor =  ColorOptions._cellreplaced_t;
			pa_28  .BackColor = (ColorOptions._celledit        as SolidBrush).Color;
			pa_28_t.BackColor =  ColorOptions._celledit_t;

			// Statusbar ->
			pa_14  .BackColor = (ColorOptions._statusbar       as SolidBrush).Color;
			pa_14_t.BackColor =  ColorOptions._statusbar_t;
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
		/// Overrides the <c>Resize</c> eventhandler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (!_init) // that appears to be needed else the help-label is !Visible
			{
				la_Help.Visible = la_Help.Top  > pa_color2.Bottom
							   && la_Help.Left > pa_color1.Right;
			}
		}
		#endregion Handlers (override)


		#region handlers (color panel)
		/// <summary>
		/// Instantiates <c><see cref="ColorSelectorDialog"/></c> on <c>LMB</c>
		/// or <c>RMB</c>. Restores default color on <c>[Shift]+RMB</c>. Copies
		/// color on <c>[Ctrl]+RMB</c> and pastes copied color on
		/// <c>[Ctrl]+LMB</c>.
		/// </summary>
		/// <param name="sender"><c>pa_*</c></param>
		/// <param name="e"></param>
		void mouseclick_Colorpanel(object sender, MouseEventArgs e)
		{
			if ((ModifierKeys & Keys.Alt) == Keys.None)
			{
				if (ModifierKeys == Keys.Control) // copy or paste color
				{
					switch (e.Button)
					{
						case MouseButtons.Right: // copy color
							Stored = (sender as Panel).BackColor;
							using (var ib = new Infobox(Infobox.Title_infor, "Color copied"))
								ib.ShowDialog(this);
							break;

						case MouseButtons.Left: // paste color
							if (Stored == Color.Empty)
							{
								using (var ib = new Infobox(Infobox.Title_error,
															"A color has not been copied yet.",
															null,
															InfoboxType.Error))
								{
									ib.ShowDialog(this);
								}
							}
							else
								(sender as Panel).BackColor = Stored;
							break;
					}
				}
				else if (ModifierKeys == Keys.Shift)
				{
					if (e.Button == MouseButtons.Right) // restore color
					{
						RestoreDefaults(sender);
					}
				}
				else // open the ColorSelector dialog
				{
					switch (e.Button)
					{
						case MouseButtons.Left:
						case MouseButtons.Right:
						{
							string title;

							// Table ->
							if      (sender == pa_15)   title = la_15.Text;
							else if (sender == pa_02)   title = la_02.Text;
							else if (sender == pa_02_t) title = la_02.Text + " text";
							else if (sender == pa_03)   title = la_03.Text;
							else if (sender == pa_03_t) title = la_03.Text + " text";
							else if (sender == pa_04)   title = la_04.Text;
							else if (sender == pa_04_t) title = la_04.Text + " text";
							else if (sender == pa_05)   title = la_05.Text;
							else if (sender == pa_05_t) title = la_05.Text + " text";
							else if (sender == pa_37)   title = la_37.Text;
							else if (sender == pa_37_t) title = la_37.Text + " text";

							// Frozen ->
							else if (sender == pa_16)   title = la_16.Text;
							else if (sender == pa_07)   title = la_07.Text;
							else if (sender == pa_07_t) title = la_07.Text + " text";
							else if (sender == pa_17)   title = la_17.Text;
							else if (sender == pa_08)   title = la_08.Text;
							else if (sender == pa_08_t) title = la_08.Text + " text";
							else if (sender == pa_29)   title = la_29.Text;
							else if (sender == pa_29_t) title = la_29.Text + " text";
							else if (sender == pa_42)   title = la_42.Text;
							else if (sender == pa_43)   title = la_43.Text;
							else if (sender == pa_44)   title = la_44.Text;
							else if (sender == pa_45)   title = la_45.Text;

							// Colhead ->
							else if (sender == pa_21)   title = la_21.Text;
							else if (sender == pa_09)   title = la_09.Text;
							else if (sender == pa_09_t) title = la_09.Text + " text";
							else if (sender == pa_30)   title = la_30.Text + " text";
							else if (sender == pa_31)   title = la_31.Text + " text";
							else if (sender == pa_32)   title = la_32.Text + " text";
							else if (sender == pa_33)   title = la_33.Text + " text";
							else if (sender == pa_40)   title = la_40.Text;
							else if (sender == pa_41)   title = la_41.Text;

							// Rowpanel ->
							else if (sender == pa_19)   title = la_19.Text;
							else if (sender == pa_11)   title = la_11.Text;
							else if (sender == pa_11_t) title = la_11.Text + " text";
							else if (sender == pa_35)   title = la_35.Text;
							else if (sender == pa_35_t) title = la_35.Text + " text";
							else if (sender == pa_36)   title = la_36.Text;
							else if (sender == pa_36_t) title = la_36.Text + " text";

							// Propanel ->
							else if (sender == pa_22)   title = la_22.Text;
							else if (sender == pa_23)   title = la_23.Text;
							else if (sender == pa_13)   title = la_13.Text;
							else if (sender == pa_13_t) title = la_13.Text + " text";
							else if (sender == pa_34)   title = la_34.Text;
							else if (sender == pa_34_t) title = la_34.Text + " text";
							else if (sender == pa_38)   title = la_38.Text;
							else if (sender == pa_38_t) title = la_38.Text + " text";

							// Cells ->
							else if (sender == pa_24)   title = la_24.Text;
							else if (sender == pa_24_t) title = la_24.Text + " text";
							else if (sender == pa_25)   title = la_25.Text;
							else if (sender == pa_25_t) title = la_25.Text + " text";
							else if (sender == pa_26)   title = la_26.Text;
							else if (sender == pa_26_t) title = la_26.Text + " text";
							else if (sender == pa_27)   title = la_27.Text;
							else if (sender == pa_27_t) title = la_27.Text + " text";
							else if (sender == pa_28)   title = la_28.Text;
							else if (sender == pa_28_t) title = la_28.Text + " text";

							// Statusbar ->
							else if (sender == pa_14)   title = la_14.Text;
							else                        title = la_14.Text + " text"; // sender == pa_14_t

							var f = new ColorSelectorDialog(this, sender as Panel, " yata - " + title);
							f.ShowDialog(this);

							Yata.that.Refresh();
							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Updates a ColorOption.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void backcolorchanged_ColorPanel(object sender, EventArgs e)
		{
			if (!_init)
			{
				// Table ->
				if      (sender == pa_15)   ColorOptions._rowlines                      .Color = pa_15  .BackColor;
				else if (sender == pa_02)  (ColorOptions._rowa            as SolidBrush).Color = pa_02  .BackColor;
				else if (sender == pa_02_t) ColorOptions._rowa_t                               = pa_02_t.BackColor;
				else if (sender == pa_03)  (ColorOptions._rowb            as SolidBrush).Color = pa_03  .BackColor;
				else if (sender == pa_03_t) ColorOptions._rowb_t                               = pa_03_t.BackColor;
				else if (sender == pa_04)  (ColorOptions._rowdisableda    as SolidBrush).Color = pa_04  .BackColor;
				else if (sender == pa_04_t) ColorOptions._rowdisableda_t                       = pa_04_t.BackColor;
				else if (sender == pa_05)  (ColorOptions._rowdisabledb    as SolidBrush).Color = pa_05  .BackColor;
				else if (sender == pa_05_t) ColorOptions._rowdisabledb_t                       = pa_05_t.BackColor;
				else if (sender == pa_37)  (ColorOptions._rowcreated      as SolidBrush).Color = pa_37  .BackColor;
				else if (sender == pa_37_t) ColorOptions._rowcreated_t                         = pa_37_t.BackColor;

				// Frozen ->
				else if (sender == pa_16)   ColorOptions._frozenlines                   .Color = pa_16  .BackColor;
				else if (sender == pa_07)   ColorOptions._frozen                               = pa_07  .BackColor;
				else if (sender == pa_07_t) ColorOptions._frozen_t                             = pa_07_t.BackColor;
				else if (sender == pa_17)   ColorOptions._frozenheadlines               .Color = pa_17  .BackColor;
				else if (sender == pa_08)   ColorOptions._frozenhead                           = pa_08  .BackColor;
				else if (sender == pa_08_t) ColorOptions._frozenhead_t                         = pa_08_t.BackColor;
				else if (sender == pa_29)   ColorOptions._frozenidunsort                       = pa_29  .BackColor;
				else if (sender == pa_29_t) ColorOptions._frozenidunsort_t                     = pa_29_t.BackColor;
				else if (sender == pa_42 || sender == pa_43)
				{
					if (sender == pa_42) ColorOptions._frozenheadgrada = pa_42.BackColor;
					else                 ColorOptions._frozenheadgradb = pa_43.BackColor; // sender == pa_43

					if (Options._gradient && Yata.Table != null)
					{
						YataGrid.UpdateFrozenLabelGradientBrush(); // recreate 'Gradients.FrozenLabel'
					}
					else return;
				}
				else if (sender == pa_44 || sender == pa_45)
				{
					if (sender == pa_44) ColorOptions._frozenidgrada = pa_44.BackColor;
					else                 ColorOptions._frozenidgradb = pa_45.BackColor; // sender == pa_45

					if (Options._gradient && Yata.Table != null)
					{
						YataGrid.UpdateDisorderedGradientBrush(); // recreate 'Gradients.Disordered'
					}
					else return;
				}

				// Colhead ->
				else if (sender == pa_21)   ColorOptions._colheadlines                  .Color = pa_21  .BackColor;
				else if (sender == pa_09)   ColorOptions._colhead                              = pa_09  .BackColor;
				else if (sender == pa_09_t) ColorOptions._colhead_t                            = pa_09_t.BackColor;
				else if (sender == pa_30)   ColorOptions._colheadsel_t                         = pa_30  .BackColor;
				else if (sender == pa_31)   ColorOptions._colheadsize_t                        = pa_31  .BackColor;
				else if (sender == pa_32)   ColorOptions._headsortasc_t                        = pa_32  .BackColor;
				else if (sender == pa_33)   ColorOptions._headsortdes_t                        = pa_33  .BackColor;
				else if (sender == pa_40 || sender == pa_41)
				{
					if (sender == pa_40) ColorOptions._colheadgrada = pa_40.BackColor;
					else                 ColorOptions._colheadgradb = pa_41.BackColor; // sender == pa_41

					if (Options._gradient && Yata.Table != null)
					{
						Yata.Table._panelCols.UpdateColheadGradientBrush(); // recreate 'Gradients.ColheadPanel'
					}
					else return;
				}

				// Rowpanel ->
				else if (sender == pa_19)   ColorOptions._rowpanellines                 .Color = pa_19  .BackColor;
				else if (sender == pa_11)   ColorOptions._rowpanel                             = pa_11  .BackColor;
				else if (sender == pa_11_t) ColorOptions._rowpanel_t                           = pa_11_t.BackColor;
				else if (sender == pa_35)  (ColorOptions._rowsel          as SolidBrush).Color = pa_35  .BackColor;
				else if (sender == pa_35_t) ColorOptions._rowsel_t                             = pa_35_t.BackColor;
				else if (sender == pa_36)  (ColorOptions._rowsubsel       as SolidBrush).Color = pa_36  .BackColor;
				else if (sender == pa_36_t) ColorOptions._rowsubsel_t                          = pa_36_t.BackColor;

				// Propanel ->
				else if (sender == pa_22)   ColorOptions._propanellines                 .Color = pa_22  .BackColor;
				else if (sender == pa_23)   ColorOptions._propanelborder                .Color = pa_23  .BackColor;
				else if (sender == pa_13)   ColorOptions._propanel                             = pa_13  .BackColor;
				else if (sender == pa_13_t) ColorOptions._propanel_t                           = pa_13_t.BackColor;
				else if (sender == pa_34)  (ColorOptions._propanelfrozen  as SolidBrush).Color = pa_34  .BackColor;
				else if (sender == pa_34_t) ColorOptions._propanelfrozen_t                     = pa_34_t.BackColor;
				else if (sender == pa_38)  (ColorOptions._propanelsel     as SolidBrush).Color = pa_38  .BackColor;
				else if (sender == pa_38_t) ColorOptions._propanelsel_t                        = pa_38_t.BackColor;

				// Cells ->
				else if (sender == pa_24)   (ColorOptions._cellselected    as SolidBrush).Color = pa_24  .BackColor;
				else if (sender == pa_24_t)  ColorOptions._cellselected_t                       = pa_24_t.BackColor;
				else if (sender == pa_25)   (ColorOptions._cellloadchanged as SolidBrush).Color = pa_25  .BackColor;
				else if (sender == pa_25_t)  ColorOptions._cellloadchanged_t                    = pa_25_t.BackColor;
				else if (sender == pa_26)   (ColorOptions._celldiffed      as SolidBrush).Color = pa_26  .BackColor;
				else if (sender == pa_26_t)  ColorOptions._celldiffed_t                         = pa_26_t.BackColor;
				else if (sender == pa_27)   (ColorOptions._cellreplaced    as SolidBrush).Color = pa_27  .BackColor;
				else if (sender == pa_27_t)  ColorOptions._cellreplaced_t                       = pa_27_t.BackColor;
				else if (sender == pa_28)
				{
					(ColorOptions._celledit as SolidBrush).Color = pa_28.BackColor;
					Yata.that.UpdateEditorColor(pa_28.BackColor); // iterate through all YataGrids
				}
				else if (sender == pa_28_t)
				{
					ColorOptions._celledit_t = pa_28_t.BackColor;
					Yata.that.UpdateEditorTextColor(); // iterate through all YataGrids
				}

				// Statusbar ->
				else if (sender == pa_14)   (ColorOptions._statusbar       as SolidBrush).Color = pa_14  .BackColor;
				else // sender == pa_14_t
				{
					ColorOptions._statusbar_t = pa_14_t.BackColor;
					Yata.that.UpdateStatusbarTextColor(); // update all ToolStripStatusLabels
				}

				Yata.that.Refresh();
			}
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
			bool ctrl = (ModifierKeys & Keys.Control) == Keys.Control;

			try
			{
				string pfeT = Path.Combine(Application.StartupPath, ColorOptions.FE) + ".t";

				File.WriteAllText(pfeT, BuildColorsText());

				if (File.Exists(pfeT))
				{
					string pfe = Path.Combine(Application.StartupPath, ColorOptions.FE);

					File.Delete(pfe);
					File.Copy(pfeT, pfe);

					if (File.Exists(pfe))
					{
						File.Delete(pfeT);

						if (ctrl)
						{
							using (var ib = new Infobox(Infobox.Title_succf,
														"Colors.Cfg saved",
														null,
														InfoboxType.Success))
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
				if (!ctrl) Close();
			}
		}

		/// <summary>
		/// Builds text for the "Colors.Cfg" file.
		/// </summary>
		/// <returns></returns>
		string BuildColorsText()
		{
			var sb = new StringBuilder();

			// Table ->
			sb.Append(ColorOptions.CFG_rowlines);
			sb.AppendLine(pa_15.BackColor.R + "," + pa_15.BackColor.G + "," + pa_15.BackColor.B);
			sb.Append(ColorOptions.CFG_rowa);
			sb.AppendLine(pa_02.BackColor.R + "," + pa_02.BackColor.G + "," + pa_02.BackColor.B);
			sb.Append(ColorOptions.CFG_rowa_t);
			sb.AppendLine(pa_02_t.BackColor.R + "," + pa_02_t.BackColor.G + "," + pa_02_t.BackColor.B);
			sb.Append(ColorOptions.CFG_rowb);
			sb.AppendLine(pa_03.BackColor.R + "," + pa_03.BackColor.G + "," + pa_03.BackColor.B);
			sb.Append(ColorOptions.CFG_rowb_t);
			sb.AppendLine(pa_03_t.BackColor.R + "," + pa_03_t.BackColor.G + "," + pa_03_t.BackColor.B);
			sb.Append(ColorOptions.CFG_rowdisableda);
			sb.AppendLine(pa_04.BackColor.R + "," + pa_04.BackColor.G + "," + pa_04.BackColor.B);
			sb.Append(ColorOptions.CFG_rowdisableda_t);
			sb.AppendLine(pa_04_t.BackColor.R + "," + pa_04_t.BackColor.G + "," + pa_04_t.BackColor.B);
			sb.Append(ColorOptions.CFG_rowdisabledb);
			sb.AppendLine(pa_05.BackColor.R + "," + pa_05.BackColor.G + "," + pa_05.BackColor.B);
			sb.Append(ColorOptions.CFG_rowdisabledb_t);
			sb.AppendLine(pa_05_t.BackColor.R + "," + pa_05_t.BackColor.G + "," + pa_05_t.BackColor.B);
			sb.Append(ColorOptions.CFG_rowcreated);
			sb.AppendLine(pa_37.BackColor.R + "," + pa_37.BackColor.G + "," + pa_37.BackColor.B);
			sb.Append(ColorOptions.CFG_rowcreated_t);
			sb.AppendLine(pa_37_t.BackColor.R + "," + pa_37_t.BackColor.G + "," + pa_37_t.BackColor.B);

			// Frozen ->
			sb.Append(ColorOptions.CFG_frozenlines);
			sb.AppendLine(pa_16.BackColor.R + "," + pa_16.BackColor.G + "," + pa_16.BackColor.B);
			sb.Append(ColorOptions.CFG_frozen);
			sb.AppendLine(pa_07.BackColor.R + "," + pa_07.BackColor.G + "," + pa_07.BackColor.B);
			sb.Append(ColorOptions.CFG_frozen_t);
			sb.AppendLine(pa_07_t.BackColor.R + "," + pa_07_t.BackColor.G + "," + pa_07_t.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenheadlines);
			sb.AppendLine(pa_17.BackColor.R + "," + pa_17.BackColor.G + "," + pa_17.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenhead);
			sb.AppendLine(pa_08.BackColor.R + "," + pa_08.BackColor.G + "," + pa_08.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenhead_t);
			sb.AppendLine(pa_08_t.BackColor.R + "," + pa_08_t.BackColor.G + "," + pa_08_t.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenidunsort);
			sb.AppendLine(pa_29.BackColor.R + "," + pa_29.BackColor.G + "," + pa_29.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenidunsort_t);
			sb.AppendLine(pa_29_t.BackColor.R + "," + pa_29_t.BackColor.G + "," + pa_29_t.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenheadgrada);
			sb.AppendLine(pa_42.BackColor.R + "," + pa_42.BackColor.G + "," + pa_42.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenheadgradb);
			sb.AppendLine(pa_43.BackColor.R + "," + pa_43.BackColor.G + "," + pa_43.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenidgrada);
			sb.AppendLine(pa_44.BackColor.R + "," + pa_44.BackColor.G + "," + pa_44.BackColor.B);
			sb.Append(ColorOptions.CFG_frozenidgradb);
			sb.AppendLine(pa_45.BackColor.R + "," + pa_45.BackColor.G + "," + pa_45.BackColor.B);

			// Colhead ->
			sb.Append(ColorOptions.CFG_colheadlines);
			sb.AppendLine(pa_21.BackColor.R + "," + pa_21.BackColor.G + "," + pa_21.BackColor.B);
			sb.Append(ColorOptions.CFG_colhead);
			sb.AppendLine(pa_09.BackColor.R + "," + pa_09.BackColor.G + "," + pa_09.BackColor.B);
			sb.Append(ColorOptions.CFG_colhead_t);
			sb.AppendLine(pa_09_t.BackColor.R + "," + pa_09_t.BackColor.G + "," + pa_09_t.BackColor.B);
			sb.Append(ColorOptions.CFG_colheadsel_t);
			sb.AppendLine(pa_30.BackColor.R + "," + pa_30.BackColor.G + "," + pa_30.BackColor.B);
			sb.Append(ColorOptions.CFG_colheadsize_t);
			sb.AppendLine(pa_31.BackColor.R + "," + pa_31.BackColor.G + "," + pa_31.BackColor.B);
			sb.Append(ColorOptions.CFG_headsortasc_t);
			sb.AppendLine(pa_32.BackColor.R + "," + pa_32.BackColor.G + "," + pa_32.BackColor.B);
			sb.Append(ColorOptions.CFG_headsortdes_t);
			sb.AppendLine(pa_33.BackColor.R + "," + pa_33.BackColor.G + "," + pa_33.BackColor.B);
			sb.Append(ColorOptions.CFG_colheadgrada);
			sb.AppendLine(pa_40.BackColor.R + "," + pa_40.BackColor.G + "," + pa_40.BackColor.B);
			sb.Append(ColorOptions.CFG_colheadgradb);
			sb.AppendLine(pa_41.BackColor.R + "," + pa_41.BackColor.G + "," + pa_41.BackColor.B);

			// Rowpanel ->
			sb.Append(ColorOptions.CFG_rowpanellines);
			sb.AppendLine(pa_19.BackColor.R + "," + pa_19.BackColor.G + "," + pa_19.BackColor.B);
			sb.Append(ColorOptions.CFG_rowpanel);
			sb.AppendLine(pa_11.BackColor.R + "," + pa_11.BackColor.G + "," + pa_11.BackColor.B);
			sb.Append(ColorOptions.CFG_rowpanel_t);
			sb.AppendLine(pa_11_t.BackColor.R + "," + pa_11_t.BackColor.G + "," + pa_11_t.BackColor.B);
			sb.Append(ColorOptions.CFG_rowsel);
			sb.AppendLine(pa_35.BackColor.R + "," + pa_35.BackColor.G + "," + pa_35.BackColor.B);
			sb.Append(ColorOptions.CFG_rowsel_t);
			sb.AppendLine(pa_35_t.BackColor.R + "," + pa_35_t.BackColor.G + "," + pa_35_t.BackColor.B);
			sb.Append(ColorOptions.CFG_rowsubsel);
			sb.AppendLine(pa_36.BackColor.R + "," + pa_36.BackColor.G + "," + pa_36.BackColor.B);
			sb.Append(ColorOptions.CFG_rowsubsel_t);
			sb.AppendLine(pa_36_t.BackColor.R + "," + pa_36_t.BackColor.G + "," + pa_36_t.BackColor.B);

			// Propanel ->
			sb.Append(ColorOptions.CFG_propanellines);
			sb.AppendLine(pa_22.BackColor.R + "," + pa_22.BackColor.G + "," + pa_22.BackColor.B);
			sb.Append(ColorOptions.CFG_propanelborder);
			sb.AppendLine(pa_23.BackColor.R + "," + pa_23.BackColor.G + "," + pa_23.BackColor.B);
			sb.Append(ColorOptions.CFG_propanel);
			sb.AppendLine(pa_13.BackColor.R + "," + pa_13.BackColor.G + "," + pa_13.BackColor.B);
			sb.Append(ColorOptions.CFG_propanel_t);
			sb.AppendLine(pa_13_t.BackColor.R + "," + pa_13_t.BackColor.G + "," + pa_13_t.BackColor.B);
			sb.Append(ColorOptions.CFG_propanelfrozen);
			sb.AppendLine(pa_34.BackColor.R + "," + pa_34.BackColor.G + "," + pa_34.BackColor.B);
			sb.Append(ColorOptions.CFG_propanelfrozen_t);
			sb.AppendLine(pa_34_t.BackColor.R + "," + pa_34_t.BackColor.G + "," + pa_34_t.BackColor.B);
			sb.Append(ColorOptions.CFG_propanelsel);
			sb.AppendLine(pa_38.BackColor.R + "," + pa_38.BackColor.G + "," + pa_38.BackColor.B);
			sb.Append(ColorOptions.CFG_propanelsel_t);
			sb.AppendLine(pa_38_t.BackColor.R + "," + pa_38_t.BackColor.G + "," + pa_38_t.BackColor.B);

			// Cells ->
			sb.Append(ColorOptions.CFG_cellselected);
			sb.AppendLine(pa_24.BackColor.R + "," + pa_24.BackColor.G + "," + pa_24.BackColor.B);
			sb.Append(ColorOptions.CFG_cellselected_t);
			sb.AppendLine(pa_24_t.BackColor.R + "," + pa_24_t.BackColor.G + "," + pa_24_t.BackColor.B);
			sb.Append(ColorOptions.CFG_cellloadchanged);
			sb.AppendLine(pa_25.BackColor.R + "," + pa_25.BackColor.G + "," + pa_25.BackColor.B);
			sb.Append(ColorOptions.CFG_cellloadchanged_t);
			sb.AppendLine(pa_25_t.BackColor.R + "," + pa_25_t.BackColor.G + "," + pa_25_t.BackColor.B);
			sb.Append(ColorOptions.CFG_celldiffed);
			sb.AppendLine(pa_26.BackColor.R + "," + pa_26.BackColor.G + "," + pa_26.BackColor.B);
			sb.Append(ColorOptions.CFG_celldiffed_t);
			sb.AppendLine(pa_26_t.BackColor.R + "," + pa_26_t.BackColor.G + "," + pa_26_t.BackColor.B);
			sb.Append(ColorOptions.CFG_cellreplaced);
			sb.AppendLine(pa_27.BackColor.R + "," + pa_27.BackColor.G + "," + pa_27.BackColor.B);
			sb.Append(ColorOptions.CFG_cellreplaced_t);
			sb.AppendLine(pa_27_t.BackColor.R + "," + pa_27_t.BackColor.G + "," + pa_27_t.BackColor.B);
			sb.Append(ColorOptions.CFG_celledit);
			sb.AppendLine(pa_28.BackColor.R + "," + pa_28.BackColor.G + "," + pa_28.BackColor.B);
			sb.Append(ColorOptions.CFG_celledit_t);
			sb.AppendLine(pa_28_t.BackColor.R + "," + pa_28_t.BackColor.G + "," + pa_28_t.BackColor.B);

			// Statusbar ->
			sb.Append(ColorOptions.CFG_statusbar);
			sb.AppendLine(pa_14.BackColor.R + "," + pa_14.BackColor.G + "," + pa_14.BackColor.B);
			sb.Append(ColorOptions.CFG_statusbar_t);
			sb.AppendLine(pa_14_t.BackColor.R + "," + pa_14_t.BackColor.G + "," + pa_14_t.BackColor.B);

			return sb.ToString();
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
				using (var ib0 = new Infobox(Infobox.Title_alert,
											 "Are you sure you want to delete the Colors.Cfg file ...",
											 null,
											 InfoboxType.Warn,
											 InfoboxButtons.CancelYes))
				{
					if (ib0.ShowDialog(this) == DialogResult.OK)
					{
						try
						{
							File.Delete(pfe);
							using (var ib = new Infobox(Infobox.Title_infor, "Colors.Cfg deleted"))
								ib.ShowDialog(this);
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
			}
			else
			{
				using (var ib = new Infobox(Infobox.Title_warni,
											"Colors.Cfg could not be found in the application directory.",
											null,
											InfoboxType.Warn))
				{
					ib.ShowDialog(this);
				}
			}
		}

		/// <summary>
		/// Reloads the "Colors.Cfg" file from the application folder.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Reload"/></c></param>
		/// <param name="e"></param>
		void click_Reload(object sender, EventArgs e)
		{
			string pfe = Path.Combine(Application.StartupPath, ColorOptions.FE);
			if (File.Exists(pfe))
			{
				try
				{
					DrawRegulator.SuspendDrawing(gb_Colors);
					DrawRegulator.SuspendDrawing(Yata.that);

					ColorOptions.ParseColorsFile(pfe);
					InitializeColorPanels();

					DrawRegulator.ResumeDrawing(gb_Colors);
					DrawRegulator.ResumeDrawing(Yata.that);

					using (var ib = new Infobox(Infobox.Title_succf,
												"Colors.Cfg reloaded",
												null,
												InfoboxType.Success))
					{
						ib.ShowDialog(this);
					}
				}
				catch (Exception ex)
				{
					using (var ib = new Infobox(Infobox.Title_excep,
												"Colors.Cfg file could not be read in the application directory.",
												ex.ToString(),
												InfoboxType.Error))
					{
						ib.ShowDialog(this);
					}
				}
			}
			else
			{
				using (var ib = new Infobox(Infobox.Title_error,
											"Colors.Cfg could not be found in the application directory.",
											null,
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
		}


		/// <summary>
		/// Restores all colors to defaults.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Restore"/></c></param>
		/// <param name="e"></param>
		void click_RestoreDefaults(object sender, EventArgs e)
		{
			RestoreDefaults(null);
		}
		#endregion handlers (buttons)


		#region Methods
		/// <summary>
		/// Restores default color(s).
		/// </summary>
		/// <param name="panel">the <c>Panel</c> right-clicked or <c>null</c> to
		/// restore all default colors</param>
		/// <remarks>Setting a <c>Panel's</c> <c>BackColor</c> fires
		/// <c><see cref="backcolorchanged_ColorPanel()">backcolorchanged_ColorPanel()</see></c></remarks>
		/// which is what sets the user's
		/// <c><see cref="ColorOptions">ColorOption</see></c>.
		void RestoreDefaults(object panel)
		{
			if (panel == null) // restore all defaults ->
			{
				DrawRegulator.SuspendDrawing(gb_Colors);
				DrawRegulator.SuspendDrawing(Yata.that);

				// Table ->
				pa_15  .BackColor = ColorOptions.Def_rowlines;
				pa_02  .BackColor = ColorOptions.Def_rowa;
				pa_02_t.BackColor = ColorOptions.Def_rowa_t;
				pa_03  .BackColor = ColorOptions.Def_rowb;
				pa_03_t.BackColor = ColorOptions.Def_rowb_t;
				pa_04  .BackColor = ColorOptions.Def_rowdisableda;
				pa_04_t.BackColor = ColorOptions.Def_rowdisableda_t;
				pa_05  .BackColor = ColorOptions.Def_rowdisabledb;
				pa_05_t.BackColor = ColorOptions.Def_rowdisabledb_t;
				pa_37  .BackColor = ColorOptions.Def_rowcreated;
				pa_37_t.BackColor = ColorOptions.Def_rowcreated_t;

				// Frozen ->
				pa_16  .BackColor = ColorOptions.Def_frozenlines;
				pa_07  .BackColor = ColorOptions.Def_frozen;
				pa_07_t.BackColor = ColorOptions.Def_frozen_t;
				pa_17  .BackColor = ColorOptions.Def_frozenheadlines;
				pa_08  .BackColor = ColorOptions.Def_frozenhead;
				pa_08_t.BackColor = ColorOptions.Def_frozenhead_t;
				pa_29  .BackColor = ColorOptions.Def_frozenidunsort;
				pa_29_t.BackColor = ColorOptions.Def_frozenidunsort_t;
				pa_42  .BackColor = ColorOptions.Def_frozenheadgrada;
				pa_43  .BackColor = ColorOptions.Def_frozenheadgradb;
				pa_44  .BackColor = ColorOptions.Def_frozenidgrada;
				pa_45  .BackColor = ColorOptions.Def_frozenidgradb;

				// Colhead ->
				pa_21  .BackColor = ColorOptions.Def_colheadlines;
				pa_09  .BackColor = ColorOptions.Def_colhead;
				pa_09_t.BackColor = ColorOptions.Def_colhead_t;
				pa_30  .BackColor = ColorOptions.Def_colheadsel_t;
				pa_31  .BackColor = ColorOptions.Def_colheadsize_t;
				pa_32  .BackColor = ColorOptions.Def_headsortasc_t;
				pa_33  .BackColor = ColorOptions.Def_headsortdes_t;
				pa_40  .BackColor = ColorOptions.Def_colheadgrada;
				pa_41  .BackColor = ColorOptions.Def_colheadgradb;

				// Rowpanel ->
				pa_19  .BackColor = ColorOptions.Def_rowpanellines;
				pa_11  .BackColor = ColorOptions.Def_rowpanel;
				pa_11_t.BackColor = ColorOptions.Def_rowpanel_t;
				pa_35  .BackColor = ColorOptions.Def_rowsel;
				pa_35_t.BackColor = ColorOptions.Def_rowsel_t;
				pa_36  .BackColor = ColorOptions.Def_rowsubsel;
				pa_36_t.BackColor = ColorOptions.Def_rowsubsel_t;

				// Propanel ->
				pa_22  .BackColor = ColorOptions.Def_propanellines;
				pa_23  .BackColor = ColorOptions.Def_propanelborder;
				pa_13  .BackColor = ColorOptions.Def_propanel;
				pa_13_t.BackColor = ColorOptions.Def_propanel_t;
				pa_34  .BackColor = ColorOptions.Def_propanelfrozen;
				pa_34_t.BackColor = ColorOptions.Def_propanelfrozen_t;
				pa_38  .BackColor = ColorOptions.Def_propanelsel;
				pa_38_t.BackColor = ColorOptions.Def_propanelsel_t;

				// Cells ->
				pa_24  .BackColor = ColorOptions.Def_cellselected;
				pa_24_t.BackColor = ColorOptions.Def_cellselected_t;
				pa_25  .BackColor = ColorOptions.Def_cellloadchanged;
				pa_25_t.BackColor = ColorOptions.Def_cellloadchanged_t;
				pa_26  .BackColor = ColorOptions.Def_celldiffed;
				pa_26_t.BackColor = ColorOptions.Def_celldiffed_t;
				pa_27  .BackColor = ColorOptions.Def_cellreplaced;
				pa_27_t.BackColor = ColorOptions.Def_cellreplaced_t;
				pa_28  .BackColor = ColorOptions.Def_celledit;
				pa_28_t.BackColor = ColorOptions.Def_celledit_t;

				// Statusbar ->
				pa_14  .BackColor = ColorOptions.Def_statusbar;
				pa_14_t.BackColor = ColorOptions.Def_statusbar_t;


				DrawRegulator.ResumeDrawing(gb_Colors);
				DrawRegulator.ResumeDrawing(Yata.that);
			}
			else // restore option default ->
			{
				// Table ->
				if      (panel == pa_15)   pa_15  .BackColor = ColorOptions.Def_rowlines;
				else if (panel == pa_02)   pa_02  .BackColor = ColorOptions.Def_rowa;
				else if (panel == pa_02_t) pa_02_t.BackColor = ColorOptions.Def_rowa_t;
				else if (panel == pa_03)   pa_03  .BackColor = ColorOptions.Def_rowb;
				else if (panel == pa_03_t) pa_03_t.BackColor = ColorOptions.Def_rowb_t;
				else if (panel == pa_04)   pa_04  .BackColor = ColorOptions.Def_rowdisableda;
				else if (panel == pa_04_t) pa_04_t.BackColor = ColorOptions.Def_rowdisableda_t;
				else if (panel == pa_05)   pa_05  .BackColor = ColorOptions.Def_rowdisabledb;
				else if (panel == pa_05_t) pa_05_t.BackColor = ColorOptions.Def_rowdisabledb_t;
				else if (panel == pa_37)   pa_37  .BackColor = ColorOptions.Def_rowcreated;
				else if (panel == pa_37_t) pa_37_t.BackColor = ColorOptions.Def_rowcreated_t;

				// Frozen ->
				else if (panel == pa_16)   pa_16  .BackColor = ColorOptions.Def_frozenlines;
				else if (panel == pa_07)   pa_07  .BackColor = ColorOptions.Def_frozen;
				else if (panel == pa_07_t) pa_07_t.BackColor = ColorOptions.Def_frozen_t;
				else if (panel == pa_17)   pa_17  .BackColor = ColorOptions.Def_frozenheadlines;
				else if (panel == pa_08)   pa_08  .BackColor = ColorOptions.Def_frozenhead;
				else if (panel == pa_08_t) pa_08_t.BackColor = ColorOptions.Def_frozenhead_t;
				else if (panel == pa_29)   pa_29  .BackColor = ColorOptions.Def_frozenidunsort;
				else if (panel == pa_29_t) pa_29_t.BackColor = ColorOptions.Def_frozenidunsort_t;
				else if (panel == pa_42)   pa_42  .BackColor = ColorOptions.Def_frozenheadgrada;
				else if (panel == pa_43)   pa_43  .BackColor = ColorOptions.Def_frozenheadgradb;
				else if (panel == pa_44)   pa_44  .BackColor = ColorOptions.Def_frozenidgrada;
				else if (panel == pa_45)   pa_45  .BackColor = ColorOptions.Def_frozenidgradb;

				// Colhead ->
				else if (panel == pa_21)   pa_21  .BackColor = ColorOptions.Def_colheadlines;
				else if (panel == pa_09)   pa_09  .BackColor = ColorOptions.Def_colhead;
				else if (panel == pa_09_t) pa_09_t.BackColor = ColorOptions.Def_colhead_t;
				else if (panel == pa_30)   pa_30  .BackColor = ColorOptions.Def_colheadsel_t;
				else if (panel == pa_31)   pa_31  .BackColor = ColorOptions.Def_colheadsize_t;
				else if (panel == pa_32)   pa_32  .BackColor = ColorOptions.Def_headsortasc_t;
				else if (panel == pa_33)   pa_33  .BackColor = ColorOptions.Def_headsortdes_t;
				else if (panel == pa_40)   pa_40  .BackColor = ColorOptions.Def_colheadgrada;
				else if (panel == pa_41)   pa_41  .BackColor = ColorOptions.Def_colheadgradb;

				// Rowpanel ->
				else if (panel == pa_19)   pa_19  .BackColor = ColorOptions.Def_rowpanellines;
				else if (panel == pa_11)   pa_11  .BackColor = ColorOptions.Def_rowpanel;
				else if (panel == pa_11_t) pa_11_t.BackColor = ColorOptions.Def_rowpanel_t;
				else if (panel == pa_35)   pa_35  .BackColor = ColorOptions.Def_rowsel;
				else if (panel == pa_35_t) pa_35_t.BackColor = ColorOptions.Def_rowsel_t;
				else if (panel == pa_36)   pa_36  .BackColor = ColorOptions.Def_rowsubsel;
				else if (panel == pa_36_t) pa_36_t.BackColor = ColorOptions.Def_rowsubsel_t;

				// Propanel ->
				else if (panel == pa_22)   pa_22  .BackColor = ColorOptions.Def_propanellines;
				else if (panel == pa_23)   pa_23  .BackColor = ColorOptions.Def_propanelborder;
				else if (panel == pa_13)   pa_13  .BackColor = ColorOptions.Def_propanel;
				else if (panel == pa_13_t) pa_13_t.BackColor = ColorOptions.Def_propanel_t;
				else if (panel == pa_34)   pa_34  .BackColor = ColorOptions.Def_propanelfrozen;
				else if (panel == pa_34_t) pa_34_t.BackColor = ColorOptions.Def_propanelfrozen_t;
				else if (panel == pa_38)   pa_38  .BackColor = ColorOptions.Def_propanelsel;
				else if (panel == pa_38_t) pa_38_t.BackColor = ColorOptions.Def_propanelsel_t;

				// Cells ->
				else if (panel == pa_24)   pa_24  .BackColor = ColorOptions.Def_cellselected;
				else if (panel == pa_24_t) pa_24_t.BackColor = ColorOptions.Def_cellselected_t;
				else if (panel == pa_25)   pa_25  .BackColor = ColorOptions.Def_cellloadchanged;
				else if (panel == pa_25_t) pa_25_t.BackColor = ColorOptions.Def_cellloadchanged_t;
				else if (panel == pa_26)   pa_26  .BackColor = ColorOptions.Def_celldiffed;
				else if (panel == pa_26_t) pa_26_t.BackColor = ColorOptions.Def_celldiffed_t;
				else if (panel == pa_27)   pa_27  .BackColor = ColorOptions.Def_cellreplaced;
				else if (panel == pa_27_t) pa_27_t.BackColor = ColorOptions.Def_cellreplaced_t;
				else if (panel == pa_28)   pa_28  .BackColor = ColorOptions.Def_celledit;
				else if (panel == pa_28_t) pa_28_t.BackColor = ColorOptions.Def_celledit_t;

				// Statusbar ->
				else if (panel == pa_14)   pa_14  .BackColor = ColorOptions.Def_statusbar;
				else                       pa_14_t.BackColor = ColorOptions.Def_statusbar_t; // panel == pa_14_t


				Yata.that.Refresh();
			}
		}
		#endregion Methods
	}
}
