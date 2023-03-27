using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ColorOptionsDialog
		: YataDialog
	{
		#region Fields
		bool _init = true;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"></param>
		internal ColorOptionsDialog(Yata f)
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
			// Table ->
//			la_01.Text = "Table text";
//			pa_01.BackColor = ColorOptions._tabletext;

			la_15.Text = "Table lines";    pa_15  .BackColor =  ColorOptions._tablelines                 .Color;
			la_02.Text = "Row a";          pa_02  .BackColor = (ColorOptions._rowa         as SolidBrush).Color;
										   pa_02_t.BackColor =  ColorOptions._rowa_t;
			la_03.Text = "Row b";          pa_03  .BackColor = (ColorOptions._rowb         as SolidBrush).Color;
										   pa_03_t.BackColor =  ColorOptions._rowb_t;
			la_04.Text = "Row disabled a"; pa_04  .BackColor = (ColorOptions._rowdisableda as SolidBrush).Color;
										   pa_04_t.BackColor =  ColorOptions._rowdisableda_t;
			la_05.Text = "Row disabled b"; pa_05  .BackColor = (ColorOptions._rowdisabledb as SolidBrush).Color;
										   pa_05_t.BackColor =  ColorOptions._rowdisabledb_t;
			la_37.Text = "Row created";    pa_37  .BackColor = (ColorOptions._rowcreated   as SolidBrush).Color;
										   pa_37_t.BackColor =  ColorOptions._rowcreated_t;

			// Frozen ->
			la_16.Text = "Frozen panel lines";            pa_16  .BackColor = ColorOptions._frozenlines.Color;
			la_07.Text = "Frozen panel";                  pa_07  .BackColor = ColorOptions._frozen;
														  pa_07_t.BackColor = ColorOptions._frozen_t;
			la_17.Text = "Frozen header lines";           pa_17  .BackColor = ColorOptions._frozenheadlines.Color;
			la_08.Text = "Frozen header";                 pa_08  .BackColor = ColorOptions._frozenhead;
														  pa_08_t.BackColor = ColorOptions._frozenhead_t;
			la_29.Text = "Frozen id unsorted";            pa_29  .BackColor = ColorOptions._frozenidunsort;
			la_42.Text = "Frozen header gradient a";      pa_42  .BackColor = ColorOptions._frozenheadgrada;
			la_43.Text = "Frozen header gradient b";      pa_43  .BackColor = ColorOptions._frozenheadgradb;
			la_44.Text = "Frozen id unsorted gradient a"; pa_44  .BackColor = ColorOptions._frozenidgrada;
			la_45.Text = "Frozen id unsorted gradient b"; pa_45  .BackColor = ColorOptions._frozenidgradb;

			// Colhead ->
			la_21.Text = "Col header lines";           pa_21  .BackColor = ColorOptions._colheadlines.Color;
			la_09.Text = "Col header";                 pa_09  .BackColor = ColorOptions._colhead;
													   pa_09_t.BackColor = ColorOptions._colhead_t;
			la_30.Text = "Col header selected text";   pa_30  .BackColor = ColorOptions._colheadsel_t;
			la_31.Text = "Col header resized text";    pa_31  .BackColor = ColorOptions._colheadsize_t;
			la_32.Text = "Header sorted ascend text";  pa_32  .BackColor = ColorOptions._headsortasc_t;
			la_33.Text = "Header sorted descend text"; pa_33  .BackColor = ColorOptions._headsortdes_t;
			la_40.Text = "Col header gradient a";      pa_40  .BackColor = ColorOptions._colheadgrada;
			la_41.Text = "Col header gradient b";      pa_41  .BackColor = ColorOptions._colheadgradb;

			// Rowpanel ->
			la_19.Text = "Row panel lines";       pa_19  .BackColor =  ColorOptions._rowpanellines           .Color;
			la_11.Text = "Row panel";             pa_11  .BackColor =  ColorOptions._rowpanel;
												  pa_11_t.BackColor =  ColorOptions._rowpanel_t;
			la_35.Text = "Row panel selected";    pa_35  .BackColor = (ColorOptions._rowsel    as SolidBrush).Color;
			la_36.Text = "Row panel subselected"; pa_36  .BackColor = (ColorOptions._rowsubsel as SolidBrush).Color;

			// Propanel ->
			la_22.Text = "Propanel lines";         pa_22  .BackColor =  ColorOptions._propanellines                .Color;
			la_23.Text = "Propanel border";        pa_23  .BackColor =  ColorOptions._propanelborder               .Color;
			la_13.Text = "Propanel";               pa_13  .BackColor =  ColorOptions._propanel;
												   pa_13_t.BackColor =  ColorOptions._propanel_t;
			la_34.Text = "Propanel frozen col";    pa_34  .BackColor = (ColorOptions._propanelfrozen as SolidBrush).Color;
			la_38.Text = "Propanel selected cell"; pa_38  .BackColor = (ColorOptions._propanelsel    as SolidBrush).Color;

			// Statusbar ->
			la_14.Text = "Statusbar"; pa_14  .BackColor = (ColorOptions._statusbar as SolidBrush).Color;
									  pa_14_t.BackColor =  ColorOptions._statusbar_t;

			// Cells ->
			la_24.Text = "Cell selected";    pa_24.BackColor = (ColorOptions._cellselected    as SolidBrush).Color;
			la_25.Text = "Cell loadchanged"; pa_25.BackColor = (ColorOptions._cellloadchanged as SolidBrush).Color;
			la_26.Text = "Cell diffed";      pa_26.BackColor = (ColorOptions._celldiffed      as SolidBrush).Color;
			la_27.Text = "Cell replaced";    pa_27.BackColor = (ColorOptions._cellreplaced    as SolidBrush).Color;
			la_28.Text = "Cell edit";        pa_28.BackColor = (ColorOptions._celledit        as SolidBrush).Color;

			_init = false;
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

		// kL_note: I don't like this. This dialog is nonmodal so if [Ctrl+s] is
		// pressed when user accidentally has Yataform focused the table gets
		// saved ... not good. -> check for [Ctrl] on the Save-button instead
		// (to keep the dialog open)
/*		/// <summary>
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
		} */
		#endregion Handlers (override)


		#region handlers (color panel)
		/// <summary>
		/// Instantiates <c><see cref="ColorSelectorDialog"/></c> on leftclick.
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

					// Table ->
//					if      (sender == pa_01) title = la_01.Text;

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
					else if (sender == pa_42)   title = la_42.Text;
					else if (sender == pa_43)   title = la_43.Text;
					else if (sender == pa_44)   title = la_44.Text;
					else if (sender == pa_45)   title = la_45.Text;

					// Colhead ->
					else if (sender == pa_21)   title = la_21.Text;
					else if (sender == pa_09)   title = la_09.Text;
					else if (sender == pa_09_t) title = la_09.Text + " text";
					else if (sender == pa_30)   title = la_30.Text;
					else if (sender == pa_31)   title = la_31.Text;
					else if (sender == pa_32)   title = la_32.Text;
					else if (sender == pa_33)   title = la_33.Text;
					else if (sender == pa_40)   title = la_40.Text;
					else if (sender == pa_41)   title = la_41.Text;

					// Rowpanel ->
					else if (sender == pa_19)   title = la_19.Text;
					else if (sender == pa_11)   title = la_11.Text;
					else if (sender == pa_11_t) title = la_11.Text + " text";
					else if (sender == pa_35)   title = la_35.Text;
					else if (sender == pa_36)   title = la_36.Text;

					// Propanel ->
					else if (sender == pa_22)   title = la_22.Text;
					else if (sender == pa_23)   title = la_23.Text;
					else if (sender == pa_13)   title = la_13.Text;
					else if (sender == pa_13_t) title = la_13.Text + " text";
					else if (sender == pa_34)   title = la_34.Text;
					else if (sender == pa_38)   title = la_38.Text;

					// Statusbar ->
					else if (sender == pa_14)   title = la_14.Text;
					else if (sender == pa_14_t) title = la_14.Text + " text";

					// Cells ->
					else if (sender == pa_24)   title = la_24.Text;
					else if (sender == pa_25)   title = la_25.Text;
					else if (sender == pa_26)   title = la_26.Text;
					else if (sender == pa_27)   title = la_27.Text;
					else                        title = la_28.Text; // sender == pa_28

					var f = new ColorSelectorDialog(this, sender as Panel, " yata - " + title);
					f.ShowDialog(this);

					Yata.that.Refresh();
					break;
				}

				case MouseButtons.Right:
					RestoreDefaults(sender);
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
			if (!_init)
			{
				// Table ->
//				if      (sender == pa_01)   ColorOptions._tabletext                            = pa_01  .BackColor;

				if      (sender == pa_15)   ColorOptions._tablelines                    .Color = pa_15  .BackColor;
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
				else if (sender == pa_42 || sender == pa_43)
				{
					if (sender == pa_42) ColorOptions._frozenheadgrada = pa_42.BackColor;
					else                 ColorOptions._frozenheadgradb = pa_43.BackColor; // sender == pa_43

					if (Options._gradient && Yata.Table != null)
					{
						Yata.Table.UpdateFrozenLabelGradientBrush(); // recreate 'Gradients.FrozenLabel'
					}
					else return;
				}
				else if (sender == pa_44 || sender == pa_45)
				{
					if (sender == pa_44) ColorOptions._frozenidgrada = pa_44.BackColor;
					else                 ColorOptions._frozenidgradb = pa_45.BackColor; // sender == pa_45

					if (Options._gradient && Yata.Table != null)
					{
						Yata.Table.UpdateDisorderedGradientBrush(); // recreate 'Gradients.Disordered'
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
				else if (sender == pa_36)  (ColorOptions._rowsubsel       as SolidBrush).Color = pa_36  .BackColor;

				// Propanel ->
				else if (sender == pa_22)   ColorOptions._propanellines                 .Color = pa_22  .BackColor;
				else if (sender == pa_23)   ColorOptions._propanelborder                .Color = pa_23  .BackColor;
				else if (sender == pa_13)   ColorOptions._propanel                             = pa_13  .BackColor;
				else if (sender == pa_13_t) ColorOptions._propanel_t                         = pa_13_t.BackColor;
				else if (sender == pa_34)  (ColorOptions._propanelfrozen  as SolidBrush).Color = pa_34  .BackColor;
				else if (sender == pa_38)  (ColorOptions._propanelsel     as SolidBrush).Color = pa_38  .BackColor;

				// Statusbar ->
				else if (sender == pa_14)  (ColorOptions._statusbar       as SolidBrush).Color = pa_14  .BackColor;
				else if (sender == pa_14_t)
				{
					ColorOptions._statusbar_t = pa_14_t.BackColor;
					Yata.that.UpdateStatusbarTextColor(); // update all ToolStripStatusLabels
				}

				// Cells ->
				else if (sender == pa_24)  (ColorOptions._cellselected    as SolidBrush).Color = pa_24  .BackColor;
				else if (sender == pa_25)  (ColorOptions._cellloadchanged as SolidBrush).Color = pa_25  .BackColor;
				else if (sender == pa_26)  (ColorOptions._celldiffed      as SolidBrush).Color = pa_26  .BackColor;
				else if (sender == pa_27)  (ColorOptions._cellreplaced    as SolidBrush).Color = pa_27  .BackColor;
				else if (sender == pa_28)
				{
					(ColorOptions._celledit as SolidBrush).Color = pa_28.BackColor;
					Yata.that.UpdateEditorColor(pa_28.BackColor); // iterate through all YataGrids
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
														"Colors.Cfg saved.", null,
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
//			sb.Append(ColorOptions.CFG_tabletext);
//			sb.AppendLine(pa_01.BackColor.R + "," + pa_01.BackColor.G + "," + pa_01.BackColor.B);
			sb.Append(ColorOptions.CFG_tablelines);
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
			sb.Append(ColorOptions.CFG_rowsubsel);
			sb.AppendLine(pa_36.BackColor.R + "," + pa_36.BackColor.G + "," + pa_36.BackColor.B);

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
			sb.Append(ColorOptions.CFG_propanelsel);
			sb.AppendLine(pa_38.BackColor.R + "," + pa_38.BackColor.G + "," + pa_38.BackColor.B);

			// Statusbar ->
			sb.Append(ColorOptions.CFG_statusbar);
			sb.AppendLine(pa_14.BackColor.R + "," + pa_14.BackColor.G + "," + pa_14.BackColor.B);
			sb.Append(ColorOptions.CFG_statusbar_t);
			sb.AppendLine(pa_14_t.BackColor.R + "," + pa_14_t.BackColor.G + "," + pa_14_t.BackColor.B);

			// Cells ->
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
		void click_RestoreDefaults(object sender, EventArgs e)
		{
			RestoreDefaults(null);
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
					using (var ib = new Infobox(Infobox.Title_infor, "Colors.Cfg deleted."))
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
			else
			{
				using (var ib = new Infobox(Infobox.Title_warni,
											"Colors.Cfg does not exist.",
											null,
											InfoboxType.Warn))
				{
					ib.ShowDialog(this);
				}
			}
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
//				pa_01  .BackColor = ColorOptions.Def_tabletext;

				pa_15  .BackColor = ColorOptions.Def_tablelines;
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
				pa_36  .BackColor = ColorOptions.Def_rowsubsel;

				// Propanel ->
				pa_22  .BackColor = ColorOptions.Def_propanellines;
				pa_23  .BackColor = ColorOptions.Def_propanelborder;
				pa_13  .BackColor = ColorOptions.Def_propanel;
				pa_13_t.BackColor = ColorOptions.Def_propanel_t;
				pa_34  .BackColor = ColorOptions.Def_propanelfrozen;
				pa_38  .BackColor = ColorOptions.Def_propanelsel;

				// Statusbar ->
				pa_14  .BackColor = ColorOptions.Def_statusbar;
				pa_14_t.BackColor = ColorOptions.Def_statusbar_t;

				// Cells ->
				pa_24  .BackColor = ColorOptions.Def_cellselected;
				pa_25  .BackColor = ColorOptions.Def_cellloadchanged;
				pa_26  .BackColor = ColorOptions.Def_celldiffed;
				pa_27  .BackColor = ColorOptions.Def_cellreplaced;
				pa_28  .BackColor = ColorOptions.Def_celledit;


				DrawRegulator.ResumeDrawing(gb_Colors);
				DrawRegulator.ResumeDrawing(Yata.that);
			}
			else // restore option default ->
			{
				// Table ->
//				if (panel == pa_01) pa_01.BackColor = ColorOptions.Def_tabletext;

				if      (panel == pa_15)   pa_15  .BackColor = ColorOptions.Def_tablelines;
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
				else if (panel == pa_36)   pa_36  .BackColor = ColorOptions.Def_rowsubsel;

				// Propanel ->
				else if (panel == pa_22)   pa_22  .BackColor = ColorOptions.Def_propanellines;
				else if (panel == pa_23)   pa_23  .BackColor = ColorOptions.Def_propanelborder;
				else if (panel == pa_13)   pa_13  .BackColor = ColorOptions.Def_propanel;
				else if (panel == pa_13_t) pa_13_t.BackColor = ColorOptions.Def_propanel_t;
				else if (panel == pa_34)   pa_34  .BackColor = ColorOptions.Def_propanelfrozen;
				else if (panel == pa_38)   pa_38  .BackColor = ColorOptions.Def_propanelsel;

				// Statusbar ->
				else if (panel == pa_14)   pa_14  .BackColor = ColorOptions.Def_statusbar;
				else if (panel == pa_14_t) pa_14_t.BackColor = ColorOptions.Def_statusbar_t;

				// Cells ->
				else if (panel == pa_24)   pa_24  .BackColor = ColorOptions.Def_cellselected;
				else if (panel == pa_25)   pa_25  .BackColor = ColorOptions.Def_cellloadchanged;
				else if (panel == pa_26)   pa_26  .BackColor = ColorOptions.Def_celldiffed;
				else if (panel == pa_27)   pa_27  .BackColor = ColorOptions.Def_cellreplaced;
				else if (panel == pa_28)   pa_28  .BackColor = ColorOptions.Def_celledit;


				Yata.that.Refresh();
			}
		}
		#endregion Methods
	}
}
