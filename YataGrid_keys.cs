using System;
using System.Windows.Forms;


namespace yata
{
	// Various routines for cell/row/col selection and subselection.
	sealed partial class YataGrid
	{
		#region events and processes (override)
		/// <summary>
		/// Disables textbox navigation etc. keys to allow table scrolling on
		/// certain key-events (happens iff the editbox is not focused).
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
#if Keys
			if ((e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("YataGrid.OnPreviewKeyDown() e.KeyCode= " + e.KeyCode + " e.IsInputKey= " + e.IsInputKey);
#endif
			// perhaps this just optimizes away key-processing and key-bubbling
			// etc. - not req'd but here it is.
			//
			// cf. YataTabs.OnPreviewKeyDown()

			switch (e.KeyCode) // <- note KeyCode not KeyData
			{
				case Keys.Up:
				case Keys.Down:
				case Keys.PageUp:
				case Keys.PageDown:
				case Keys.Left:
				case Keys.Right:
				case Keys.Escape:
#if Keys
					logfile.Log(". YataGrid.OnPreviewKeyDown force e.IsInputKey TRUE");
#endif
					e.IsInputKey = true; // bypass processing and go straight to KeyDown events.
					break;
			}
			base.OnPreviewKeyDown(e);
		}

		/// <summary>
		/// Can be used to process any keystroke - as long as
		/// <c><see cref="OnPreviewKeyDown()">OnPreviewKeyDown()</see></c> has
		/// not flagged <c>e.IsInputKey</c> <c>true</c> - when this
		/// <c>YataGrid</c> or <c><see cref="_editor"/></c> has focus.
		/// <list type="bullet">
		/// <item><c>[Enter]</c> - starts or accepts celledit</item>
		/// <item><c>[Escape]</c> - cancels celledit</item>
		/// </list>
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataGrid.ProcessCmdKey() keyData= " + keyData);
#endif
			switch (keyData)
			{
				case Keys.Enter:
					// NOTE: [Enter] on the propanel is processed by Propanel.ProcessCmdKey()
					if (_editor.Visible)
					{
						_bypassleaveditor = true;
						editresultaccept(true);
#if Keys
						logfile.Log(". YataGrid.ProcessCmdKey force TRUE (accept grid-edit)");
#endif
						return true;
					}

					if (!Readonly
						&& (_editcell = getSelectedCell()) != null
						&&  _editcell.x >= FrozenCount)
					{
						Celledit();
#if Keys
						logfile.Log(". YataGrid.ProcessCmdKey force TRUE (start grid-edit)");
#endif
						return true;
					}
					break;

				case Keys.Escape:
					// NOTE: [Escape] on the propanel is processed by Propanel.ProcessCmdKey()
					if (_editor.Visible)
					{
						_bypassleaveditor = true;
						editresultcancel(INVALID_GRID, true);
#if Keys
						logfile.Log(". YataGrid.ProcessCmdKey force TRUE (cancel grid-edit)");
#endif
						return true;
					}
					break;
			}

			bool ret = base.ProcessCmdKey(ref msg, keyData);
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataGrid.ProcessCmdKey ret= " + ret);
#endif
			return ret;
		}

#if Keys
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataGrid.IsInputKey() keyData= " + keyData);

			bool ret = base.ProcessDialogKey(keyData);
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataGrid.IsInputKey ret= " + ret);

			return ret;
		}
#endif

		/// <summary>
		/// Checks if a keystroke is used by a TabFastedit routine.
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		/// <remarks>Keep these in sync with
		/// <c><see cref="ProcessDialogKey()">ProcessDialogKey()</see></c>.</remarks>
		internal static bool IsTabfasteditKey(Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Tab:								// right
				case Keys.Shift | Keys.Tab:					// left
				case Keys.Control | Keys.Tab:				// down
				case Keys.Down:
				case Keys.Shift | Keys.Control | Keys.Tab:	// up
				case Keys.Up:
				case Keys.PageDown:							// pagedown
				case Keys.PageUp:							// pageup
					return true;
			}
			return false;
		}

		/// <summary>
		/// Processes a so-called dialog-key. Use this for TabFastedit
		/// keystrokes only - process keystrokes for other operations in
		/// <c><see cref="ProcessCmdKey()">ProcessCmdKey()</see></c>.
		/// <list type="bullet">
		/// <item><c>[Tab]</c> - fastedit right</item>
		/// <item><c>[Shift+Tab]</c> - fastedit left</item>
		/// <item><c>[Ctrl+Tab]</c> or <c>[Down]</c> - fastedit down</item>
		/// <item><c>[Shift+Ctrl+Tab]</c> or <c>[Up]</c> - fastedit up</item>
		/// <item><c>[PageDown]</c> - fastedit pagedown</item>
		/// <item><c>[PageUp]</c> - fastedit pageup</item>
		/// </list></summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		/// <remarks>Certain keystrokes need a return of <c>false</c> in
		/// <c><see cref="YataEditbox"/>.IsInputKey()</c>. Cf
		/// <c><see cref="Propanel"></see>.ProcessDialogKey()</c>.</remarks>
		/// <seealso cref="IsTabfasteditKey()"><c>IsTabfasteditKey()</c></seealso>
		/// <remarks>These very likely could and should be put in
		/// <c><see cref="ProcessCmdKey()">ProcessCmdKey()</see></c> but I'm
		/// going to keep them here just to differentiate the TabFastedit keys.</remarks>
		/// <seealso cref="Propanel"><c>Propanel.ProcessDialogKey()</c></seealso>
		protected override bool ProcessDialogKey(Keys keyData)
		{
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataGrid.ProcessDialogKey() keyData= " + keyData);
#endif
			// TabFastedit ->
			if (_editor.Visible)
			{
				switch (keyData)
				{
					case Keys.Tab:								// right
					case Keys.Shift | Keys.Tab:					// left
					case Keys.Control | Keys.Tab:				// down - stop the tabcontrol from responding to [Ctrl+Tab]
					case Keys.Down:
					case Keys.Shift | Keys.Control | Keys.Tab:	// up - stop the tabcontrol from responding to [Shift+Ctrl+Tab]
					case Keys.Up:
					case Keys.PageDown:							// pagedown
					case Keys.PageUp:							// pageup
						_bypassleaveditor = true;
						editresultaccept(true);

						process(keyData);
#if Keys
						logfile.Log(". YataGrid.ProcessDialogKey force TRUE (TabFastedit)");
#endif
						return true;
				}
			}

			bool ret = base.ProcessDialogKey(keyData);
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataGrid.ProcessDialogKey ret= " + ret);
#endif
			return ret;
		}

		/// <summary>
		/// helper for
		/// <c><see cref="ProcessDialogKey()">ProcessDialogKey()</see></c>
		/// </summary>
		/// <param name="keyData"></param>
		void process(Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Tab:								// right
					if (_editcell.x != ColCount - 1)
						startTabedit(+1,0);
					break;

				case Keys.Shift | Keys.Tab:					// left
					if (_editcell.x != FrozenCount)
						startTabedit(-1,0);
					break;

				case Keys.Control | Keys.Tab:				// down
				case Keys.Down:
					if (_editcell.y != RowCount - 1)
						startTabedit(0,+1);
					break;

				case Keys.Shift | Keys.Control | Keys.Tab:	// up
				case Keys.Up:
					if (_editcell.y != 0)
						startTabedit(0,-1);
					break;

				case Keys.PageDown:							// pagedown
					if (_editcell.y != RowCount - 1)
					{
						int shift = GetShiftVert();
						if (_editcell.y + shift > RowCount - 1) shift = RowCount - 1 - _editcell.y;

						startTabedit(0, +shift);
					}
					break;

				case Keys.PageUp:							// pageup
					if (_editcell.y != 0)
					{
						int shift = GetShiftVert();
						if (_editcell.y - shift < 0) shift = _editcell.y;

						startTabedit(0, -shift);
					}
					break;
			}
		}

		/// <summary>
		/// Moves cell selection and calls
		/// <c><see cref="Celledit()">Celledit()</see></c> for the next cell in
		/// a TabFastedit sequence.
		/// </summary>
		/// <param name="dir_hori"></param>
		/// <param name="dir_vert"></param>
		void startTabedit(int dir_hori, int dir_vert)
		{
			this[_editcell.y,
				 _editcell.x].selected = false;

			(_editcell = this[_editcell.y + dir_vert,
							  _editcell.x + dir_hori]).selected = true;

			Celledit();

			SelectSyncCell(_anchorcell = _editcell);
		}


		/// <summary>
		/// Handles navigation by keyboard around this <c>YataGrid</c>. Also
		/// handles the <c>[Esc]</c> key to clear all selections.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
#if Keys
			if ((e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("YataGrid.OnKeyDown() e.KeyData= " + e.KeyData);
#endif
			if ((e.Modifiers & Keys.Alt) == Keys.None)
			{
				bool ctr = (e.Modifiers & Keys.Control) == Keys.Control,
					 sft = (e.Modifiers & Keys.Shift)   == Keys.Shift;

				int invalid  = INVALID_NONE;
				bool display = false;
				bool anchor  = false;

				Cell sel = getSelectedCell();
				int selr = getSelectedRow();

				// TODO: change selected col perhaps

				YataGrid table; // sync-table

				switch (e.KeyCode)
				{
					case Keys.Escape: // NOTE: needs to bypass KeyPreview
#if Keys
						logfile.Log(". Keys.Escape (clear all selects)");
#endif
						if (!ctr && !sft)
						{
							ClearSelects();
							_f.ClearSyncSelects();

							selr = -1;
							invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
						}
						break;


					case Keys.Home:
#if Keys
						logfile.Log(". Keys.Home (navigate grid)");
#endif
						if (selr != -1)
						{
							if (!sft)
							{
								if (ctr)
								{
									selr = row_SelectRow(selr, 0);
									invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
								}
								else if (_visHori) _scrollHori.Value = 0;
							}
							else if (!ctr)
							{
								RangeSelect = -selr;
								row_SelectRangeCells(0, selr);

								if ((table = getSynctable()) != null)
								{
									if (selr < table.RowCount)
									{
										table.RangeSelect = -selr;
										table.row_SelectRangeCells(0, selr);
									}
									else
										table.RangeSelect = 0;
								}
								selr = 0;
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
						}
						else if (sft)
						{
							if (allowContiguous() && areSelectedCellsContiguous())
							{
								if (ctr)
								{
									ClearCellSelects();
									SelectCellBlock(0,           _anchorcell.y,
													FrozenCount, _anchorcell.x);

									sel = this[0, FrozenCount];
								}
								else
								{
									int strt_r, stop_r;
									int sel_r = asStartStop_row(out strt_r, out stop_r);

									ClearCellSelects();
									SelectCellBlock(strt_r,      stop_r,
													FrozenCount, _anchorcell.x);

									sel = this[sel_r, FrozenCount];
								}
								anchor = true;
							}
						}
						else if (ctr)
						{
							if (sel != null)
							{
								if (sel.x != FrozenCount || sel.y != 0)
								{
									sel.selected = false;
									(sel = this[0, FrozenCount]).selected = true;
								}
								display = true;
							}
							else if (_visVert) _scrollVert.Value = 0;
						}
						else if (sel != null)
						{
							if (sel.x != FrozenCount)
							{
								sel.selected = false;
								(sel = this[sel.y, FrozenCount]).selected = true;
							}
							display = true;
						}
						else if (_visHori) _scrollHori.Value = 0;
						break;

					case Keys.End:
#if Keys
						logfile.Log(". Keys.End (navigate grid)");
#endif
						if (selr != -1)
						{
							if (!sft)
							{
								if (ctr)
								{
									selr = row_SelectRow(selr, RowCount - 1);
									invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
								}
								else if (_visHori) _scrollHori.Value = MaxHori;
							}
							else if (!ctr)
							{
								RangeSelect = RowCount - selr - 1;
								row_SelectRangeCells(selr, RowCount - 1);

								if ((table = getSynctable()) != null)
								{
									if (selr < table.RowCount)
									{
										table.setRangeSelect(selr, RangeSelect);
										table.row_SelectRangeCells(selr, selr + table.RangeSelect);
									}
									else
										table.RangeSelect = 0;
								}
								selr = RowCount - 1;
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
						}
						else if (sft)
						{
							if (allowContiguous() && areSelectedCellsContiguous())
							{
								if (ctr)
								{
									ClearCellSelects();
									SelectCellBlock(_anchorcell.y, RowCount - 1,
													_anchorcell.x, ColCount - 1);

									sel = this[RowCount - 1, ColCount - 1];
								}
								else
								{
									int strt_r, stop_r;
									int sel_r = asStartStop_row(out strt_r, out stop_r);

									ClearCellSelects();
									SelectCellBlock(strt_r,        stop_r,
													_anchorcell.x, ColCount - 1);

									sel = this[sel_r, ColCount - 1];
								}
								anchor = true;
							}
						}
						else if (ctr)
						{
							if (sel != null)
							{
								if (sel.x != FrozenCount || sel.y != RowCount - 1)
								{
									sel.selected = false;
									(sel = this[RowCount - 1, FrozenCount]).selected = true;
								}
								display = true;
							}
							else if (_visVert) _scrollVert.Value = MaxVert;
						}
						else if (sel != null)
						{
							if (sel.x != ColCount - 1)
							{
								sel.selected = false;
								(sel = this[sel.y, ColCount - 1]).selected = true;
							}
							display = true;
						}
						else if (_visHori) _scrollHori.Value = MaxHori;
						break;

					case Keys.PageUp:
#if Keys
						logfile.Log(". Keys.PageUp (navigate grid)");
#endif
						if (selr != -1)
						{
							if (!ctr)
							{
								if (!sft)
								{
									int r = selr;
									if (r != 0)
									{
										int shift = GetShiftVert();
										if ((r -= shift) < 0) r = 0;
									}
									selr = row_SelectRow(selr, r);
								}
								else
								{
									int range = -GetShiftVert();
										range += RangeSelect;

									if (selr + range < 0) RangeSelect = 0 - selr;
									else                  RangeSelect = range;

									int strt_r, stop_r;
									asStartStop_range(selr, out strt_r, out stop_r);
									row_SelectRangeCells(strt_r, stop_r);

									if ((table = getSynctable()) != null)
									{
										if (selr < table.RowCount)
										{
											table.setRangeSelect(selr, RangeSelect);

											table.asStartStop_range(selr, out strt_r, out stop_r);
											table.row_SelectRangeCells(strt_r, stop_r);
										}
										else
											table.RangeSelect = 0;
									}
									selr += RangeSelect;
								}
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
						}
						else if (sft)
						{
							if (allowContiguous() && areSelectedCellsContiguous())
							{
								if (ctr)
								{
									int strt_c, stop_c;
									int sel_c = asStartStop_col(out strt_c, out stop_c);

									ClearCellSelects();
									SelectCellBlock(0,      _anchorcell.y,
													strt_c, stop_c);

									sel = this[0, sel_c];
									anchor = true;
								}
								else
								{
									int sel_r = getAnchorRangedRowid();
									if (sel_r != 0)
									{
										int shift = GetShiftVert();
										sel_r = Math.Max(0, sel_r - shift);

										int strt_r = Math.Min(sel_r, _anchorcell.y);
										int stop_r = Math.Max(sel_r, _anchorcell.y);

										int strt_c, stop_c;
										int sel_c = asStartStop_col(out strt_c, out stop_c);

										ClearCellSelects();
										SelectCellBlock(strt_r, stop_r,
														strt_c, stop_c);

										sel = this[sel_r, sel_c];
										anchor = true;
									}
								}
							}
						}
//						else if (ctr)
//						{
							// don't use [Ctrl+PageUp] since it is used/consumed by the tabcontrol
							//
							// Note that can be bypassed in YataTabs.OnKeyDown() -
							// which is how [Ctrl+Shift+PageUp/Down] work for selecting contiguous blocks.
							// but I'd prefer to keep [Ctrl+PageUp/Down] for actually changing tabpages
//						}
						else if (!ctr)
						{
							if (sel != null)
							{
								if (sel.y != 0 && sel.x >= FrozenCount)
								{
									sel.selected = false;

									int shift = GetShiftVert();
									if ((selr = sel.y - shift) < 0) selr = 0;

									(sel = this[selr, sel.x]).selected = true;
								}
								display = true;
							}
							else if (_visVert)
							{
								int h = Height - HeightColhead - (_visHori ? _scrollHori.Height : 0);
								if (_scrollVert.Value - h < 0)
									_scrollVert.Value = 0;
								else
									_scrollVert.Value -= h;
							}
						}
						break;

					case Keys.PageDown:
#if Keys
						logfile.Log(". Keys.PageDown (navigate grid)");
#endif
						if (selr != -1)
						{
							if (!ctr)
							{
								if (!sft)
								{
									int r = selr;
									if (r != RowCount - 1)
									{
										int shift = GetShiftVert();
										if ((r += shift) > RowCount - 1) r = RowCount - 1;
									}
									selr = row_SelectRow(selr, r);
								}
								else
								{
									int range = GetShiftVert();
										range += RangeSelect;

									if (selr + range >= RowCount) RangeSelect = RowCount - selr - 1;
									else                          RangeSelect = range;

									int strt_r, stop_r;
									asStartStop_range(selr, out strt_r, out stop_r);
									row_SelectRangeCells(strt_r, stop_r);

									if ((table = getSynctable()) != null)
									{
										if (selr < table.RowCount)
										{
											table.setRangeSelect(selr, RangeSelect);

											table.asStartStop_range(selr, out strt_r, out stop_r);
											table.row_SelectRangeCells(strt_r, stop_r);
										}
										else
											table.RangeSelect = 0;
									}
									selr += RangeSelect;
								}
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
						}
						else if (sft)
						{
							if (allowContiguous() && areSelectedCellsContiguous())
							{
								if (ctr)
								{
									int strt_c, stop_c;
									int sel_c = asStartStop_col(out strt_c, out stop_c);

									ClearCellSelects();
									SelectCellBlock(_anchorcell.y, RowCount - 1,
													strt_c,        stop_c);

									sel = this[RowCount - 1, sel_c];
									anchor = true;
								}
								else
								{
									int sel_r = getAnchorRangedRowid();
									if (sel_r != RowCount - 1)
									{
										int shift = GetShiftVert();
										sel_r = Math.Min(RowCount - 1, sel_r + shift);

										int strt_r = Math.Min(sel_r, _anchorcell.y);
										int stop_r = Math.Max(sel_r, _anchorcell.y);

										int strt_c, stop_c;
										int sel_c = asStartStop_col(out strt_c, out stop_c);

										ClearCellSelects();
										SelectCellBlock(strt_r, stop_r,
														strt_c, stop_c);

										sel = this[sel_r, sel_c];
										anchor = true;
									}
								}
							}
						}
//						else if (ctr)
//						{
							// don't use [Ctrl+PageDown] since it is used/consumed by the tabcontrol
							//
							// Note that can be bypassed in YataTabs.OnKeyDown() -
							// which is how [Ctrl+Shift+PageUp/Down] work for selecting contiguous blocks.
							// but I'd prefer to keep [Ctrl+PageUp/Down] for actually changing tabpages
//						}
						else if (!ctr)
						{
							if (sel != null)
							{
								if (sel.y != RowCount - 1 && sel.x >= FrozenCount)
								{
									sel.selected = false;

									int shift = GetShiftVert();
									if ((selr = sel.y + shift) > RowCount - 1) selr = RowCount - 1;

									(sel = this[selr, sel.x]).selected = true;
								}
								display = true;
							}
							else if (_visVert)
							{
								int h = Height - HeightColhead - (_visHori ? _scrollHori.Height : 0);
								if (_scrollVert.Value + h > MaxVert)
									_scrollVert.Value = MaxVert;
								else
									_scrollVert.Value += h;
							}
						}
						break;

					case Keys.Up: // NOTE: needs to bypass KeyPreview
#if Keys
						logfile.Log(". Keys.Up (navigate grid)");
#endif
						if (selr != -1)
						{
							if (!ctr)
							{
								if (!sft)
								{
									selr = row_SelectRow(selr, Math.Max(0, selr - 1));
								}
								else
								{
									if (selr + RangeSelect != 0) --RangeSelect;

									int strt_r, stop_r;
									asStartStop_range(selr, out strt_r, out stop_r);
									row_SelectRangeCells(strt_r, stop_r);

									if ((table = getSynctable()) != null)
									{
										if (selr < table.RowCount)
										{
											table.setRangeSelect(selr, RangeSelect);

											table.asStartStop_range(selr, out strt_r, out stop_r);
											table.row_SelectRangeCells(strt_r, stop_r);
										}
										else
											table.RangeSelect = 0;
									}
									selr += RangeSelect;
								}
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
						}
						else if (sft)
						{
							if (!ctr && allowContiguous() && areSelectedCellsContiguous())
							{
								int sel_r = getAnchorRangedRowid() - 1;
								if (sel_r >= 0)
								{
									int strt_r = Math.Min(sel_r, _anchorcell.y);
									int stop_r = Math.Max(sel_r, _anchorcell.y);

									int strt_c, stop_c;
									int sel_c = asStartStop_col(out strt_c, out stop_c);

									ClearCellSelects();
									SelectCellBlock(strt_r, stop_r,
													strt_c, stop_c);

									sel = this[sel_r, sel_c];
									anchor = true;
								}
							}
						}
						else if (ctr) // selection to abovest cell
						{
							if (sel != null)
							{
								if (sel.y != 0 && sel.x >= FrozenCount)
								{
									sel.selected = false;
									(sel = this[0, sel.x]).selected = true;
								}
								display = true;
							}
						}
						else if (sel != null) // selection to the cell above
						{
							if (sel.y != 0 && sel.x >= FrozenCount)
							{
								sel.selected = false;
								(sel = this[sel.y - 1, sel.x]).selected = true;
							}
							display = true;
						}
						else if (_visVert)
						{
							if (_scrollVert.Value - HeightRow < 0)
								_scrollVert.Value = 0;
							else
								_scrollVert.Value -= HeightRow;
						}
						break;

					case Keys.Down: // NOTE: needs to bypass KeyPreview
#if Keys
						logfile.Log(". Keys.Down (navigate grid)");
#endif
						if (selr != -1)
						{
							if (!ctr)
							{
								if (!sft)
								{
									selr = row_SelectRow(selr, Math.Min(RowCount - 1, selr + 1));
								}
								else
								{
									if (selr + RangeSelect != RowCount - 1) ++RangeSelect;

									int strt_r, stop_r;
									asStartStop_range(selr, out strt_r, out stop_r);
									row_SelectRangeCells(strt_r, stop_r);

									if ((table = getSynctable()) != null)
									{
										if (selr < table.RowCount)
										{
											table.setRangeSelect(selr, RangeSelect);

											table.asStartStop_range(selr, out strt_r, out stop_r);
											table.row_SelectRangeCells(strt_r, stop_r);
										}
										else
											table.RangeSelect = 0;
									}
									selr += RangeSelect;
								}
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
						}
						else if (sft)
						{
							if (!ctr && allowContiguous() && areSelectedCellsContiguous())
							{
								int sel_r = getAnchorRangedRowid() + 1;
								if (sel_r < RowCount)
								{
									int strt_r = Math.Min(sel_r, _anchorcell.y);
									int stop_r = Math.Max(sel_r, _anchorcell.y);

									int strt_c, stop_c;
									int sel_c = asStartStop_col(out strt_c, out stop_c);

									ClearCellSelects();
									SelectCellBlock(strt_r, stop_r,
													strt_c, stop_c);

									sel = this[sel_r, sel_c];
									anchor = true;
								}
							}
						}
						else if (ctr) // selection to belowest cell
						{
							if (sel != null)
							{
								if (sel.y != RowCount - 1 && sel.x >= FrozenCount)
								{
									sel.selected = false;
									(sel = this[RowCount - 1, sel.x]).selected = true;
								}
								display = true;
							}
						}
						else if (sel != null) // selection to the cell below
						{
							if (sel.y != RowCount - 1 && sel.x >= FrozenCount)
							{
								sel.selected = false;
								(sel = this[sel.y + 1, sel.x]).selected = true;
							}
							display = true;
						}
						else if (_visVert)
						{
							if (_scrollVert.Value + HeightRow > MaxVert)
								_scrollVert.Value = MaxVert;
							else
								_scrollVert.Value += HeightRow;
						}
						break;

					case Keys.Left: // NOTE: needs to bypass KeyPreview
#if Keys
						logfile.Log(". Keys.Left (navigate grid)");
#endif
						if (!ctr)
						{
							if (sft)
							{
//								if (sel != null)
//								{
//									if (sel.x > FrozenCount)
//									{
//										sel.selected = false;
//
//										int w = Width - getLeft() - (_visVert ? _scrollVert.Width : 0);
//										var pt = getColBounds(sel.x);
//										pt.X += OffsetHori - w;
//
//										int c = -1, tally = 0;
//										while ((tally += Cols[++c].width()) < pt.X)
//										{}
//
//										if (++c >= sel.x)
//											c = sel.x - 1;
//
//										if (c < FrozenCount)
//											c = FrozenCount;
//
//										(sel = this[sel.y, c]).selected = true;
//									}
//									display = true;
//								}

								if (selr == -1 && areSelectedCellsContiguous())
								{
									if (allowContiguous())
									{
										int sel_c = getAnchorRangedColid() - 1;
										if (sel_c >= FrozenCount)
										{
											int strt_c = Math.Min(sel_c, _anchorcell.x);
											int stop_c = Math.Max(sel_c, _anchorcell.x);

											int strt_r, stop_r;
											int sel_r = asStartStop_row(out strt_r, out stop_r);

											ClearCellSelects();
											SelectCellBlock(strt_r, stop_r,
															strt_c, stop_c);

											sel = this[sel_r, sel_c];
											anchor = true;
										}
									}
								}
								else if (_visHori) // shift grid 1 page left
								{
									int w = Width - getLeft() - (_visVert ? _scrollVert.Width : 0);
									if (_scrollHori.Value - w < 0)
										_scrollHori.Value = 0;
									else
										_scrollHori.Value -= w;
								}
							}
							else if (sel != null) // selection to the cell left
							{
								if (sel.x > FrozenCount)
								{
									sel.selected = false;
									(sel = this[sel.y, sel.x - 1]).selected = true;
								}
								display = true;
							}
							else if (_visHori)
							{
								if (_scrollHori.Value - HeightRow < 0)
									_scrollHori.Value = 0;
								else
									_scrollHori.Value -= HeightRow;
							}
						}
						break;

					case Keys.Right: // NOTE: needs to bypass KeyPreview
#if Keys
						logfile.Log(". Keys.Right (navigate grid)");
#endif
						if (!ctr)
						{
							if (sft)
							{
//								if (sel != null)
//								{
//									if (sel.x != ColCount - 1)
//									{
//										sel.selected = false;
//
//										int w = Width - getLeft() - (_visVert ? _scrollVert.Width : 0);
//										var pt = getColBounds(sel.x);
//										pt.X += OffsetHori + w;
//
//										int c = -1, tally = 0;
//										while (++c != ColCount && (tally += Cols[c].width()) < pt.X)
//										{}
//
//										if (--c <= sel.x)
//											c = sel.x + 1;
//
//										if (c > ColCount - 1)
//											c = ColCount - 1;
//
//										(sel = this[sel.y, c]).selected = true;
//									}
//									display = true;
//								}

								if (selr == -1 && areSelectedCellsContiguous())
								{
									if (allowContiguous())
									{
										int sel_c = getAnchorRangedColid() + 1;
										if (sel_c < ColCount)
										{
											int strt_c = Math.Min(sel_c, _anchorcell.x);
											int stop_c = Math.Max(sel_c, _anchorcell.x);

											int strt_r, stop_r;
											int sel_r = asStartStop_row(out strt_r, out stop_r);

											ClearCellSelects();
											SelectCellBlock(strt_r, stop_r,
															strt_c, stop_c);

											sel = this[sel_r, sel_c];
											anchor = true;
										}
									}
								}
								else if (_visHori) // shift grid 1 page right
								{
									int w = Width - getLeft() - (_visVert ? _scrollVert.Width : 0);
									if (_scrollHori.Value + w > MaxHori)
										_scrollHori.Value = MaxHori;
									else
										_scrollHori.Value += w;
								}
							}
							else if (sel != null) // selection to the cell right
							{
								if (sel.x != ColCount - 1)
								{
									sel.selected = false;
									(sel = this[sel.y, sel.x + 1]).selected = true;
								}
								display = true;
							}
							else if (_visHori)
							{
								if (_scrollHori.Value + HeightRow > MaxHori)
									_scrollHori.Value = MaxHori;
								else
									_scrollHori.Value += HeightRow;
							}
						}
						break;
				}

				if (invalid != INVALID_NONE)	// -> is a Row operation or [Esc]
				{
					e.SuppressKeyPress = true;

					if (selr != -1) EnsureDisplayedRow(selr);

					if (Propanel != null && Propanel.Visible)
						invalid |= INVALID_PROP;

					Invalidator(invalid);
				}
				else if (display)				// -> is a Cell operation
				{
					e.SuppressKeyPress = true;

					_anchorcell = sel;

					SelectSyncCell(sel);
					_f.EnableCelleditOperations();

					Invalidator(INVALID_GRID
							  | INVALID_FROZ
							  | EnsureDisplayed(sel));
				}
				else if (anchor)				// -> is a block-select operation
				{
					e.SuppressKeyPress = true;

					_f.EnableCelleditOperations();

					Invalidator(INVALID_GRID
							  | EnsureDisplayed(sel));
				}
			}
		}


		/// <summary>
		/// Gets the count of <c><see cref="Row">Rows</see></c> that are
		/// currently visible in the table.
		/// </summary>
		/// <returns></returns>
		int GetShiftVert()
		{
			return (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;
		}
		#endregion events and processes (override)


		#region row selection
		/// <summary>
		/// Selects a <c><see cref="Row"/></c>.
		/// </summary>
		/// <param name="selr">the currently selected row-id</param>
		/// <param name="r">the row-id to select</param>
		/// <returns>the row-id that gets selected</returns>
		int row_SelectRow(int selr, int r)
		{
			Rows[selr].selected = false;

			ClearCellSelects(true);
			SelectRow(r);

			if (FrozenCount < ColCount)
				_anchorcell = this[r, FrozenCount];

			return r;
		}
		#endregion row selection


		#region row subselection
		/// <summary>
		/// Assigns row-id start/stop values.
		/// </summary>
		/// <param name="selr">the currently selected row-id</param>
		/// <param name="strt_r">ref to start row-id</param>
		/// <param name="stop_r">ref to stop row-id</param>
		/// <remarks>Ensure that <c><see cref="RangeSelect"/></c> has been set
		/// properly before call.</remarks>
		void asStartStop_range(int selr, out int strt_r, out int stop_r)
		{
			if (RangeSelect < 0)
			{
				strt_r = selr + RangeSelect;
				stop_r = selr;
			}
			else
			{
				strt_r = selr;
				stop_r = selr + RangeSelect;
			}
		}

		/// <summary>
		/// Subselects a range of <c><see cref="Row">Rows</see></c>.
		/// </summary>
		/// <param name="strt_r">start row-id</param>
		/// <param name="stop_r">stop row-id</param>
		void row_SelectRangeCells(int strt_r, int stop_r)
		{
			ClearCellSelects(true);

			for (int r = strt_r; r <= stop_r;   ++r)
			for (int c = 0;      c != ColCount; ++c)
			{
				this[r,c].selected = true;
			}
		}

		/// <summary>
		/// Gets the sync-table of this <c>YataGrid</c>.
		/// </summary>
		/// <returns><c>null</c> if there is no valid sync-table</returns>
		YataGrid getSynctable()
		{
			if (this == _f._diff1) return _f._diff2;
			if (this == _f._diff2) return _f._diff1;

			return null;
		}

		/// <summary>
		/// Sets this <c>YataGrid's</c> <c><see cref="RangeSelect"/></c> and
		/// ensures that it does not exceed <c><see cref="RowCount"/></c>.
		/// </summary>
		/// <param name="selr">the currently selected row-id</param>
		/// <param name="range">the range of row-ids to select (+/-)</param>
		/// <remarks>Used for a sync-table.</remarks>
		void setRangeSelect(int selr, int range)
		{
			if (selr + (RangeSelect = range) >= RowCount)
				RangeSelect = RowCount - selr - 1;
		}
		#endregion row subselection


		#region contiguous cell selection
		/// <summary>
		/// Checks if a contiguous operation is allowable. But does not check if
		/// the currently selected cells actually are contiguous - call
		/// <c><see cref="areSelectedCellsContiguous()">areSelectedCellsContiguous()</see></c>
		/// also before allowing a contiguous operation.
		/// </summary>
		/// <returns></returns>
		/// <remarks>Don't allow multi-cell select if sync'd.</remarks>
		bool allowContiguous()
		{
			return this != _f._diff1 && this != _f._diff2 // disallow multi-cell select if sync'd
				&& (    _anchorcell != null
					|| (_anchorcell = getFirstSelectedCell(FrozenCount)) != null)
				&& _anchorcell.x >= FrozenCount;
		}

		/// <summary>
		/// Assigns the start and stop row-ids that shall be used as a range of
		/// <c><see cref="Row">Rows</see></c> that need to be selected during a
		/// horizontal contiguous block selection.
		/// </summary>
		/// <param name="strt_r">ref that holds the start row-id</param>
		/// <param name="stop_r">ref that holds the stop row-id</param>
		/// <returns>the row-id of a <c><see cref="Cell"/></c> that shall be
		/// displayed</returns>
		/// <remarks>Helper for <c><see cref="OnKeyDown()">OnKeyDown()</see></c>
		/// contiguous block selection (horizontal).</remarks>
		int asStartStop_row(out int strt_r, out int stop_r)
		{
			if (_f._copyvert == 1
				|| _anchorcell.y == 0
				|| !this[_anchorcell.y - 1, _anchorcell.x].selected)
			{
				strt_r = _anchorcell.y;
				stop_r = _anchorcell.y + _f._copyvert - 1;

				return stop_r;
			}

			strt_r = _anchorcell.y - _f._copyvert + 1;
			stop_r = _anchorcell.y;

			return strt_r;
		}

		/// <summary>
		/// Assigns the start and stop col-ids that shall be used as a range of
		/// <c><see cref="Col">Cols</see></c> that need to be selected during a
		/// vertical contiguous block selection.
		/// </summary>
		/// <param name="strt_c">ref that holds the start col-id</param>
		/// <param name="stop_c">ref that holds the stop col-id</param>
		/// <returns>the col-id of a <c><see cref="Cell"/></c> that shall be
		/// displayed</returns>
		/// <remarks>Helper for <c><see cref="OnKeyDown()">OnKeyDown()</see></c>
		/// contiguous block selection (vertical).</remarks>
		int asStartStop_col(out int strt_c, out int stop_c)
		{
			if (_f._copyhori == 1
				|| _anchorcell.x == FrozenCount
				|| !this[_anchorcell.y, _anchorcell.x - 1].selected)
			{
				strt_c = _anchorcell.x;
				stop_c = _anchorcell.x + _f._copyhori - 1;

				return stop_c;
			}

			strt_c = _anchorcell.x - _f._copyhori + 1;
			stop_c = _anchorcell.x;

			return strt_c;
		}

		/// <summary>
		/// Gets the col-id with selected cell that is furthest away from the
		/// current <c><see cref="_anchorcell">_anchorcell's</see></c>
		/// <c><see cref="Col"/></c>. Can return the col-id of the
		/// <c>_anchorcell</c> itself.
		/// </summary>
		/// <returns>col-id</returns>
		int getAnchorRangedColid()
		{
			int colid = _anchorcell.x;

			for (int c = ColCount - 1; c >= _anchorcell.x; --c)
			if (this[_anchorcell.y, c].selected)
			{
				colid = c;
				break;
			}

			if (colid == _anchorcell.x)
			{
				for (int c = FrozenCount; c <= _anchorcell.x; ++c)
				if (this[_anchorcell.y, c].selected)
				{
					colid = c;
					break;
				}
			}

			return colid;
		}

		/// <summary>
		/// Gets the row-id with selected cell that is furthest away from the
		/// current <c><see cref="_anchorcell">_anchorcell's</see></c>
		/// <c><see cref="Row"/></c>. Can return the row-id of the
		/// <c>_anchorcell</c> itself.
		/// </summary>
		/// <returns>row-id</returns>
		int getAnchorRangedRowid()
		{
			int rowid = _anchorcell.y;

			for (int r = RowCount - 1; r >= _anchorcell.y; --r)
			if (this[r, _anchorcell.x].selected)
			{
				rowid = r;
				break;
			}

			if (rowid == _anchorcell.y)
			{
				for (int r = 0; r <= _anchorcell.y; ++r)
				if (this[r, _anchorcell.x].selected)
				{
					rowid = r;
					break;
				}
			}

			return rowid;
		}


		/// <summary>
		/// Selects <c><see cref="Cell">Cells</see> in a contiguous block.</c>
		/// </summary>
		/// <param name="strt_r">the start row-id</param>
		/// <param name="stop_r">the stop row-id</param>
		/// <param name="strt_c">the start col-id</param>
		/// <param name="stop_c">the stop col-id</param>
		void SelectCellBlock(int strt_r, int stop_r, int strt_c, int stop_c)
		{
			for (int r = strt_r; r <= stop_r; ++r)
			for (int c = strt_c; c <= stop_c; ++c)
			{
				this[r,c].selected = true;
			}
		}
		#endregion contiguous cell selection
	}
}
