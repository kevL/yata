using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	// Fahfrd and the Gray Mouser and related events.
	sealed partial class YataGrid
	{
		#region Fields
		/// <summary>
		/// The clicked cell.
		/// </summary>
		/// <remarks>Set by
		/// <c><see cref="OnMouseDown()">OnMouseDown()</see></c>.
		/// 
		/// 
		/// Used by
		/// <list type="bullet">
		/// <item><c><see cref="OnMouseClick()">OnMouseClick()</see></c></item>
		/// <item><c><see cref="OnMouseDoubleClick()">OnMouseDoubleClick()</see></c></item>
		/// </list></remarks>
		Cell _cell;

		/// <summary>
		/// <c>true</c> allows
		/// <c><see cref="OnMouseDoubleClick()">OnMouseDoubleClick()</see></c>.
		/// </summary>
		bool _double;

		/// <summary>
		/// <c>true</c> to bypass
		/// <c><see cref="OnMouseClick()">OnMouseClick()</see></c>.
		/// </summary>
		/// <remarks><c><see cref="OnMouseDoubleClick()">OnMouseDoubleClick()</see></c>
		/// can still fire if <c><see cref="_double"/></c> is set <c>true</c> in
		/// <c><see cref="OnMouseDown()">OnMouseDown()</see></c>.</remarks>
		bool _bypassclickhandler;


		/// <summary>
		/// Handles a situation where user attempts a double-click on a cell
		/// that is already selected.
		/// </summary>
		/// <remarks><c><see cref="_doubletclick"/></c> is set <c>true</c> for a
		/// duration of <c>SystemInformation.DoubleClickTime</c> and if
		/// <c><see cref="YataEditbox"/></c> receives a <c>MouseClick</c> event
		/// within that duration it selects all text instead of deselecting the
		/// text and positioning the caret.</remarks>
		static Timer _t1;

		/// <summary>
		/// See <c><see cref="_t1"/></c>.
		/// </summary>
		internal static bool _doubletclick;
		#endregion Fields


		#region eventhandlers (override)
		/// <summary>
		/// Overrides the <c>MouseDown</c> eventhandler. Ensures that the editor
		/// closes properly.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Since .net fires <c>RMB</c> <c>MouseClick</c> after other
		/// stuff (but note that <c>LMB</c> <c>MouseClick</c> happens before
		/// other stuff) use <c>MouseDown</c> instead for <c>RMB</c> operations
		/// that need to happen before other stuff.
		/// 
		/// 
		/// <c>MouseDown</c> <c>MouseClick</c> and <c>MouseDoubleClick</c> need
		/// to work as a single routine.</remarks>
		protected override void OnMouseDown(MouseEventArgs e)
		{
#if Clicks
			logfile.Log("YataGrid.OnMouseDown() " + e.Button + " _editor.Visible= " + _editor.Visible);
#endif
			if ((ModifierKeys & Keys.Alt) == Keys.None) // do this in OnMouseClick() also
			{
				bool @select = true; // set to false if an editor gets focus

				_bypassclickhandler = false;

				// (e.X >= WidthTable || e.Y >= HeightTable)
				if ((_cell = getCell(e.X, e.Y)) != null) // click to the right or below the table-area
				{
#if Clicks
					logfile.Log(". clicked cell VALID");
#endif
					if (_editor.Visible)
					{
						if (_cell != _editcell)
						{
							switch (e.Button)
							{
								case MouseButtons.Left:
									// disallow [Ctrl] or [Shift] since OnMouseClick() will be bypassed

									if ((ModifierKeys & (Keys.Control | Keys.Shift)) == Keys.None)
									{
#if Clicks
										logfile.Log(". . grid accept edit");
#endif
										_bypassleaveditor = true;
										editresultaccept();

										_bypassclickhandler = true;

										_double = true;
									}
									else goto default;
									break;

								case MouseButtons.Right:
									// disallow [Ctrl] or [Shift] w/ RMB in OnMouseClick() also

									if ((ModifierKeys & (Keys.Control | Keys.Shift)) == Keys.None)
									{
#if Clicks
										logfile.Log(". . grid default edit result");
#endif
										_editor.Visible = false;
									}
									else goto default;
									break;

								default:
#if Clicks
									logfile.Log(". . grid focus editor");
#endif
									refocuseditor(_editor, ref @select);
									break;
							}
						}
						else // _cell == _editcell
						{
#if Clicks
							logfile.Log(". . grid (_cell == _editcell) focus editor");
#endif
							refocuseditor(_editor, ref @select);
						}
					}
					else if (Propanel != null && Propanel._editor.Visible)
					{
						switch (e.Button)
						{
							case MouseButtons.Left:
								// disallow [Ctrl] or [Shift] since OnMouseClick() will be bypassed

								if ((ModifierKeys & (Keys.Control | Keys.Shift)) == Keys.None)
								{
#if Clicks
									logfile.Log(". . propanel accept edit");
#endif
									Propanel._bypassleaveditor = true;
									Propanel.editresultaccept();

									_bypassclickhandler = true;

									_double = true;
								}
								else goto default;
								break;

							case MouseButtons.Right:
								// disallow [Ctrl] or [Shift] w/ RMB in OnMouseClick() also

								if ((ModifierKeys & (Keys.Control | Keys.Shift)) == Keys.None)
								{
#if Clicks
									logfile.Log(". . propanel default edit result");
#endif
									Propanel._editor.Visible = false;
								}
								else goto default;
								break;

							default:
#if Clicks
								logfile.Log(". . propanel focus editor");
#endif
								refocuseditor(Propanel._editor, ref @select);
								break;
						}
					}
				}
				else if (_editor.Visible) // _cell == null
				{
#if Clicks
					logfile.Log(". (_cell == null && _editor.Visible)");
#endif
					switch (e.Button)
					{
						case MouseButtons.Left:
							if ((ModifierKeys & (Keys.Control | Keys.Shift)) == Keys.None)
							{
#if Clicks
								logfile.Log(". . grid accept edit");
#endif
								_bypassleaveditor = true;
								editresultaccept();

								_bypassclickhandler = true;
							}
							else goto default;
							break;

						case MouseButtons.Right:
							if ((ModifierKeys & (Keys.Control | Keys.Shift)) == Keys.None)
							{
#if Clicks
								logfile.Log(". . grid cancel edit");
#endif
								_bypassleaveditor = true;
								editresultcancel(INVALID_GRID);

								_bypassclickhandler = true;
							}
							else goto default;
							break;

						default:
#if Clicks
							logfile.Log(". . grid focus editor");
#endif
							refocuseditor(_editor, ref @select);
							break;
					}
				}
				else if (Propanel != null && Propanel._editor.Visible)
				{
#if Clicks
					logfile.Log(". (_cell == null && Propanel._editor.Visible)");
#endif
					switch (e.Button)
					{
						case MouseButtons.Left:
							if ((ModifierKeys & (Keys.Control | Keys.Shift)) == Keys.None)
							{
#if Clicks
								logfile.Log(". . propanel accept edit");
#endif
								Propanel._bypassleaveditor = true;
								Propanel.editresultaccept();

								_bypassclickhandler = true;
							}
							else goto default;
							break;

						case MouseButtons.Right:
							if ((ModifierKeys & (Keys.Control | Keys.Shift)) == Keys.None)
							{
#if Clicks
								logfile.Log(". . propanel cancel edit");
#endif
								Propanel._bypassleaveditor = true;
								Propanel.editresultcancel();

								_bypassclickhandler = true;
							}
							else goto default;
							break;

						default:
#if Clicks
							logfile.Log(". . propanel focus editor");
#endif
							refocuseditor(Propanel._editor, ref @select);
							break;
					}
				}
				else
				{
#if Clicks
					logfile.Log(". (_cell == null");
#endif
					_bypassclickhandler = true;

					if (e.Button == MouseButtons.Right)
					{
						if (anySelected())
						{
#if Clicks
							logfile.Log(". . deselect all");
#endif
							_f.editclick_Deselect(this, EventArgs.Empty);
						}
					}
				}

				if (@select) Select();
			}
		}

		/// <summary>
		/// Gets the <c><see cref="Cell"/></c> at x/y.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		Cell getCell(int x, int y)
		{
			if (   (y += OffsetVert) > HeightColhead && y < HeightTable
				&& (x += OffsetHori) < WidthTable)
			{
				int left = getLeft();
				if (x > left)
				{
					Point cords = getCords(x,y, left);
					return this[cords.Y, cords.X];
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the col/row coordinates of a <c><see cref="Cell"/></c> based on
		/// the current position of the cursor.
		/// </summary>
		/// <param name="x">mouse x-pos within the table</param>
		/// <param name="y">mouse y-pos within the table</param>
		/// <param name="left">the x-pos of the right edge of the frozen-panel;
		/// ie. the left edge of the visible area of this <c>YataGrid</c></param>
		/// <returns>a <c>Point</c> where <c>X</c> is col-id and <c>Y</c> is
		/// row-id</returns>
		Point getCords(int x, int y, int left)
		{
			int l = left;	// NOTE: That's only to get rid of the erroneous "Parameter
							// is assigned but its value is never used" warning. Notice,
							// however, that now 'l' is never used but ... no warning.
							// Thank god these guys didn't write the code that got to the Moon.
							//
							// ps. My theory is that Stanley Kubrick wanted people to believe
							// that he faked the landings; that's right, he ** faked fake **
							// Moon landings!

			var cords = new Point();

			cords.X = FrozenCount;
			while ((l += Cols[cords.X].Width) < x)
				++cords.X;

			int top = HeightColhead;

			cords.Y = 0;
			while ((top += HeightRow) < y)
				++cords.Y;

			return cords;
		}

		/// <summary>
		/// Focuses either <c><see cref="_editor"/></c> or
		/// <c><see cref="Propanel._editor">Propanel._editor</see></c>.
		/// </summary>
		/// <param name="tb">a <c><see cref="YataEditbox"/></c> to focus</param>
		/// <param name="select">ref to set <c>false</c></param>
		/// <remarks>helper for
		/// <c><see cref="OnMouseDown()">OnMouseDown()</see></c></remarks>
		void refocuseditor(Control tb, ref bool @select)
		{
			tb.Focus();
			@select = false;
			_bypassclickhandler = true;
		}

		/// <summary>
		/// Overrides the <c>MouseClick</c> eventhandler and deals with clicks
		/// inside the table-area only. LMB selects cell(s) and RMB opens a
		/// cell's context-menu.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks><c>YataGrid.MouseClick</c> does not fire on any of the top
		/// or left panels.
		/// 
		/// 
		/// <c>MouseDown</c> <c>MouseClick</c> and <c>MouseDoubleClick</c> need
		/// to work as a single routine.</remarks>
		protected override void OnMouseClick(MouseEventArgs e)
		{
#if Clicks
			logfile.Log("YataGrid.OnMouseClick() " + e.Button + " _editor.Visible= " + _editor.Visible + " _bypassclickhandler= " + _bypassclickhandler);
#endif
			if (!_bypassclickhandler)
			{
				_double = false;	// DoubleClick needs to be allowed to fire even if OnMouseClick() gets bypassed.
									// so keep that reset inside (!_bypassclickhandler)

				if ((ModifierKeys & Keys.Alt) == Keys.None	// <- do this in OnMouseDown() also
					&& _cell != null						// <- (e.X < WidthTable && e.Y < HeightTable) click inside the table-area
					&& _cell == getCell(e.X, e.Y))			// <- cancel operation if cursor is no longer on '_cell'
				{
					bool detercellops = false;

					switch (e.Button)
					{
						case MouseButtons.Left:
							if ((ModifierKeys & Keys.Control) == Keys.Control) // select/deselect single cell ->
							{
								if ((ModifierKeys & Keys.Shift) == Keys.None)
								{
#if Clicks
									logfile.Log(". Ctrl");
#endif
									Cell sel = getSelectedCell();
									if (sel == null || sel.x >= FrozenCount) // disallow multi-cell select if a frozen cell is currently selected
									{
										if (_cell.selected = !_cell.selected)
										{
#if Clicks
											logfile.Log(". . select cell");
#endif
											if (SelectSyncCell(_cell)) // disallow multi-cell select if sync'd
											{
												ClearSelects(true);
												_cell.selected = true;
											}
											EnsureDisplayed(_cell, sel == null);	// <- bypass Propanel.EnsureDisplayed() if
																					// selectedcell is not the only selected cell
											_anchorcell = _cell;
										}
										else
										{
#if Clicks
											logfile.Log(". . deselect cell");
#endif
											_f.ClearSyncSelects();
										}

										detercellops = true;

										int invalid = INVALID_GRID;
										if (Propanel != null && Propanel.Visible)
											invalid |= INVALID_PROP;
	
										Invalidator(invalid);
									}
								}
							}
							else if ((ModifierKeys & Keys.Shift) == Keys.Shift) // do block selection ->
							{
#if Clicks
								logfile.Log(". Shift");
#endif
								if (_cell != getSelectedCell()								// else do nothing if clicked cell is the only selected cell
									&& allowContiguous() && areSelectedCellsContiguous())	// else do nothing if no cells are selected or selected cells are not in a contiguous block
								{
#if Clicks
									logfile.Log(". . do block selection");
#endif
									ClearSelects(true);

									int strt_r = Math.Min(_anchorcell.y, _cell.y);
									int stop_r = Math.Max(_anchorcell.y, _cell.y);
									int strt_c = Math.Min(_anchorcell.x, _cell.x);
									int stop_c = Math.Max(_anchorcell.x, _cell.x);

									for (int r = strt_r; r <= stop_r; ++r)
									for (int c = strt_c; c <= stop_c; ++c)
										this[r,c].selected = true;

									EnsureDisplayed(_cell, true);

									detercellops = true;

									int invalid = INVALID_GRID;
									if (Propanel != null && Propanel.Visible)
										invalid |= INVALID_PROP;

									Invalidator(invalid);
								}
							}
							else if (!_cell.selected || getSelectedCell() == null) // select cell if it's not selected or if it's not the only selected cell ->
							{
#if Clicks
								logfile.Log(". select cell");
#endif
								_double = true;

								ClearSelects(true);

								(_anchorcell = _cell).selected = true;
								SelectSyncCell(_cell);

								detercellops = true;

								Invalidator(INVALID_GRID
										  | INVALID_FROZ
										  | INVALID_ROWS
										  | EnsureDisplayed(_cell));
							}
							else if (!Readonly) // cell is already selected
							{
#if Clicks
								logfile.Log(". edit cell");
#endif
								startCelledit(_cell);

								_doubletclick = true;
								_t1.Start();
							}
							break;

						case MouseButtons.Right:
							if ((ModifierKeys & (Keys.Control | Keys.Shift)) == Keys.None)
							{
#if Clicks
								logfile.Log(". select cell and show context");
#endif
								ClearSelects(true);

								(_anchorcell = _cell).selected = true;
								SelectSyncCell(_cell);

								detercellops = true;

								Invalidator(INVALID_GRID
										  | INVALID_FROZ
										  | INVALID_ROWS
										  | EnsureDisplayed(_cell));

								_f.ShowCellContext();
							}
							break;
					}

					if (detercellops)
						_f.EnableCelleditOperations();
				}
			}
#if Clicks
			else logfile.Log(". MouseClick bypassed");
#endif
			_bypassclickhandler = false;
		}

		/// <summary>
		/// Because sometimes I'm a stupid and double-click to start textedit.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>If a cell is currently being edited any changes to that
		/// cell will be accepted and the cell that is double-clicked if any
		/// shall (sic) enter its edit-state.
		/// 
		/// 
		/// <c>MouseDown</c> <c>MouseClick</c> and <c>MouseDoubleClick</c> need
		/// to work as a single routine.</remarks>
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
#if Clicks
			logfile.Log("YataGrid.OnMouseDoubleClick() " + e.Button + " _editor.Visible= " + _editor.Visible);
#endif
			if (_double)
			{
				if (!_cell.selected)
				{
#if Clicks
					logfile.Log(". select cell");
#endif
					ClearSelects(true);

					(_anchorcell = _cell).selected = true;
					SelectSyncCell(_cell);

					Invalidator(INVALID_GRID // not so sure all that is needed ->
							  | INVALID_FROZ
							  | INVALID_ROWS
							  | EnsureDisplayed(_cell));

					_f.EnableCelleditOperations();
				}

				if (!Readonly)
				{
#if Clicks
					logfile.Log(". edit cell");
#endif
					startCelledit(_cell);
				}
			}
#if Clicks
			else logfile.Log(". MouseDoubleClick bypassed");
#endif
		}


		/// <summary>
		/// See <c><see cref="_t1"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="_t1"/></c></param>
		/// <param name="e"></param>
		void _t1_Tick(object sender, EventArgs e)
		{
			_t1.Stop();
			_doubletclick = false;
		}


		/// <summary>
		/// Handles mouse-movement over the grid and its background area -
		/// prints coordinates and path-info to the statusbar.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!_init)
			{
				if (   e.X < Width  - (_visVert ? _scrollVert.Width  : 0)
					&& e.Y < Height - (_visHori ? _scrollHori.Height : 0))
				{
					int y = e.Y + OffsetVert;
					if (y > HeightColhead && y < HeightTable)
					{
						int x = e.X + OffsetHori;
						if (x < WidthTable)
						{
							int left = getLeft();
							if (x > left)
							{
								_f.PrintInfo(getCords(x,y, left));
								return;
							}
						}
					}
				}
				_f.PrintInfo(); // clear
			}
		}

		/// <summary>
		/// Clears statusbar coordinates and path-info when the mouse-cursor
		/// leaves the grid.
		/// </summary>
		/// <remarks>Called by <c><see cref="Yata"></see>.t1_Tick()</c>. The
		/// <c>MouseLeave</c> event is unreliable.</remarks>
		internal void MouseLeaveTicker()
		{
			int left = getLeft();
			var rect = new Rectangle(left,
									 HeightColhead,
									 Width  - left          - (_visVert ? _scrollVert.Width  : 0),
									 Height - HeightColhead - (_visHori ? _scrollHori.Height : 0));

			if (!rect.Contains(PointToClient(Cursor.Position)))
				_f.PrintInfo(); // clear
		}
		#endregion eventhandlers (override)


		#region DragDrop file(s)
		/// <summary>
		/// Handles dragging a file onto the grid.
		/// </summary>
		/// <param name="drgevent"></param>
		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			_f.yata_DragEnter(null, drgevent);
		}

		/// <summary>
		/// Handles dropping a file onto the grid.
		/// </summary>
		/// <param name="drgevent"></param>
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			_f.yata_DragDrop(null, drgevent);
		}
		#endregion DragDrop file(s)


		#region helpers
		/// <summary>
		/// Handles a <c>MouseDown</c> event on
		/// <c><see cref="YataPanelRows"/></c>. Selects or deselects
		/// <c><see cref="Row">Row(s)</see></c>.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>.net fucks with <c>RMB</c> and <c>[Alt]</c> differently
		/// than <c>LMB</c> and <c>[Ctrl]</c>/<c>[Shift]</c>.</remarks>
		internal void click_RowheadPanel(MouseEventArgs e)
		{
			if ((ModifierKeys & Keys.Alt) == Keys.None)
			{
				int click_r = (e.Y + OffsetVert) / HeightRow;
				if (click_r < RowCount)
				{
					switch (e.Button)
					{
						case MouseButtons.Left:
						{
							// get the sync-table if one exists
							YataGrid table;
							if      (this == _f._diff1) table = _f._diff2;
							else if (this == _f._diff2) table = _f._diff1;
							else                        table = null;

							int selcsync = (table != null) ? table.getSelectedCol() : -1;


							int selc = getSelectedCol(); // do not clear cell-selects in a selected col

							// if cells in another row are currently selected and a
							// row that is already selected is clicked just clear those
							// extraneous cells (instead of deselecting the clicked row)
							bool celldeselected = false;

							if ((ModifierKeys & Keys.Control) == Keys.None) // clear all other row's cells ->
							{
								Cell cell;
								for (int r = 0; r != RowCount; ++r)
								for (int c = 0; c != ColCount; ++c)
								{
									if (r != click_r && c != selc
										&& (cell = this[r,c]).selected)
									{
										cell.selected = false;
										celldeselected = true;
									}
								}

								if (table != null)
								{
									for (int r = 0; r != table.RowCount; ++r)
									for (int c = 0; c != table.ColCount; ++c)
									{
										if (r != click_r && c != selcsync)
											table[r,c].selected = false;
									}
								}
							}


							Row.BypassEnableRowedit = true; // call _f.EnableRoweditOperations() later ...

							bool display = false; // deter col for ensure displayed col

							int selr = getSelectedRow();

							if ((ModifierKeys & Keys.Shift) == Keys.None) // select only the clicked row ->
							{
								if (selr != -1 && selr != click_r) // clear any other selected row ->
									Rows[selr].selected = false;

								if (table != null)
								{
									int selrsync = table.getSelectedRow();
									if (selrsync != -1 && selrsync != click_r)
										table.Rows[selrsync].selected = false;
								}


								bool allcellsselected = true;
								foreach (var cell in Rows[click_r]._cells)
								if (!cell.selected)
								{
									allcellsselected = false;
									break;
								}

								// select the clicked row if
								// (a) it is not currently selected
								// (b) or not all cells in the clicked row are currently selected
								// (c) or cells not in the clicked row got deselected above
								bool @select = !Rows[click_r].selected || !allcellsselected || celldeselected;

								if ((Rows[click_r].selected = @select) && FrozenCount < ColCount)
									_anchorcell = this[click_r, FrozenCount];

								for (int c = 0; c != ColCount; ++c) // select or deselect cells in the clicked row ->
								{
									if (@select || c != selc)
										this[click_r,c].selected = @select;
								}

								if (table != null && click_r < table.RowCount) // select or deselect cells in a sync-table's row ->
								{
									table.Rows[click_r].selected = @select;

									for (int c = 0; c != table.ColCount; ++c)
									{
										if (@select || c != selcsync)
											table[click_r,c].selected = @select;
									}
								}

								display = @select;
							}
							else if (selr != -1) // subselect a range of rows ->
							{
								RangeSelect = click_r - selr;
								if (RangeSelect != 0)
								{
									display = true;

									int start, stop;
									if (RangeSelect > 0) { start = selr; stop = click_r; }
									else                 { start = click_r; stop = selr; }

									for (int r = start; r <= stop; ++r) // select subselected rows' cells ->
									for (int c = 0; c != ColCount; ++c)
									{
										this[r,c].selected = true;
									}

									if (table != null && start < table.RowCount) // select subselected rows' cells in a sync-table ->
									{
										for (int r = start; r <= stop && r != table.RowCount; ++r)
										for (int c = 0; c != table.ColCount; ++c)
										{
											table[r,c].selected = true;
										}
									}
								}
							}

							Row.BypassEnableRowedit = false;

							if (display) EnsureDisplayedRow(click_r);

							int invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							if (Propanel != null && Propanel.Visible)
								invalid |= INVALID_PROP;

							Invalidator(invalid);

							_f.EnableCelleditOperations();	// TODO: tighten that to only if necessary.
							_f.EnableRoweditOperations();	// TODO: tighten that to only if necessary.
							break;
						}

						case MouseButtons.Right:
							if (ModifierKeys == Keys.None)
							{
								ClearSelects();

								SelectRow(click_r);
								EnsureDisplayedRow(click_r);

								int invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
								if (Propanel != null && Propanel.Visible)
									invalid |= INVALID_PROP;

								Invalidator(invalid);

								_f.ShowRowContext(click_r);
							}
							break;
					}
				}
				else if (ModifierKeys == Keys.None) // click below the last entry ->
				{
					ClearSelects();
					_f.ClearSyncSelects();

					int invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
					if (Propanel != null && Propanel.Visible)
						invalid |= INVALID_PROP;

					Invalidator(invalid);
				}
			}
		}


		/// <summary>
		/// Handles a <c>MouseDown</c> event on
		/// <c><see cref="YataPanelCols"/></c>. Selects or deselects
		/// <c><see cref="Col">Col(s)</see></c>.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>.net fucks with <c>RMB</c> and <c>[Alt]</c> differently
		/// than <c>LMB</c> and <c>[Ctrl]</c>/<c>[Shift]</c>.</remarks>
		internal void click_ColheadPanel(MouseEventArgs e)
		{
			if (!_panelCols.IsGrab && (ModifierKeys & Keys.Alt) == Keys.None)
			{
				Select();

				int click_c;
				switch (e.Button)
				{
					case MouseButtons.Left:
						if ((click_c = getClickedCol(e.X)) != -1)
						{
							// get the sync-table if one exists
							YataGrid table;
							if      (this == _f._diff1) table = _f._diff2;
							else if (this == _f._diff2) table = _f._diff1;
							else                        table = null;

							int selrsync = (table != null) ? table.getSelectedRow() : -1;


							int selr = getSelectedRow(); // do not clear cell-selects in a selected row

							// if cells in another col are currently selected and a
							// col that is already selected is clicked just clear those
							// extraneous cells (instead of deselecting the clicked col)
							bool celldeselected = false;

							if ((ModifierKeys & Keys.Control) == Keys.None) // clear all other col's cells ->
							{
								for (int r = 0; r != RowCount; ++r)
								for (int c = 0; c != ColCount; ++c)
								{
									if (c != click_c
										&& (   (r > selr && r > selr + RangeSelect)
											|| (r < selr && r < selr + RangeSelect))
										&& this[r,c].selected)
									{
										this[r,c].selected = false;
										celldeselected = true;
									}
								}

								if (table != null)
								{
									for (int r = 0; r != table.RowCount; ++r)
									for (int c = 0; c != table.ColCount; ++c)
									{
										if (c != click_c
											&& (   (r > selrsync && r > selrsync + table.RangeSelect)
												|| (r < selrsync && r < selrsync + table.RangeSelect)))
										{
											table[r,c].selected = false;
										}
									}
								}
							}


							bool display = false; // deter col for ensure displayed col

							int selc = getSelectedCol();

							if ((ModifierKeys & Keys.Shift) == Keys.None)
							{
								if (selc != -1 && selc != click_c) // clear any other selected col ->
									Cols[selc].selected = false;

								if (table != null)
								{
									int selcsync = table.getSelectedCol();
									if (selcsync != -1 && selcsync != click_c)
										table.Cols[selcsync].selected = false;
								}


								bool allcellsselected = true;
								foreach (var row in Rows)
								if (!row[click_c].selected)
								{
									allcellsselected = false;
									break;
								}

								// select the clicked col if
								// (a) it is not currently selected
								// (b) or not all cells in the clicked col are currently selected
								// (c) or cells not in the clicked col got deselected above
								bool @select = !Cols[click_c].selected || !allcellsselected || celldeselected;

								if (Cols[click_c].selected = @select)
									_anchorcell = this[0, click_c];

								for (int r = 0; r != RowCount; ++r) // select or deselect cells in the clicked col ->
								{
									if (@select
										|| (r > selr && r > selr + RangeSelect)
										|| (r < selr && r < selr + RangeSelect))
									{
										this[r,click_c].selected = @select;
									}
								}

								if (table != null && click_c < table.ColCount) // select or deselect cells in a sync-table's col ->
								{
									table.Cols[click_c].selected = @select;

									for (int r = 0; r != table.RowCount; ++r)
									{
										if (@select
											|| (r > selrsync && r > selrsync + table.RangeSelect)
											|| (r < selrsync && r < selrsync + table.RangeSelect))
										{
											table[r,click_c].selected = @select;
										}
									}
								}

								display = @select;
							}
							else if (selc != -1) // subselect a range of cols ->
							{
								int delta = click_c - selc;
								if (delta != 0)
								{
									display = true;

									int start, stop;
									if (delta > 0)
									{
										start = selc; stop = click_c;
									}
									else
									{
										start = click_c; stop = selc;
									}

									for (int r = 0; r != RowCount; ++r) // select range-selected cols' cells ->
									for (int c = start; c <= stop; ++c)
									{
										this[r,c].selected = true;
									}

									if (table != null && start < table.ColCount) // select range-selected cols' cells in a sync-table ->
									{
										for (int r = 0; r != table.RowCount; ++r)
										for (int c = start; c <= stop && c != table.ColCount; ++c)
										{
											table[r,c].selected = true;
										}
									}
								}
							}


							if (display) EnsureDisplayedCol(click_c);

//							invalid |= INVALID_FROZ;					// <- doesn't seem to be needed.
//							if (Propanel != null && Propanel.Visible)
//								invalid |= INVALID_PROP;				// <- doesn't seem to be needed.

							Invalidator(INVALID_COLS | INVALID_GRID);

							_f.EnableCelleditOperations(); // TODO: tighten that to only if necessary.
						}
						break;

					case MouseButtons.Right:
						if (ModifierKeys == Keys.Shift) // sort by col ->
						{
							if ((click_c = getClickedCol(e.X)) != -1)
//								&& _panelCols.GetSplitterCol(e.X) == -1	// this is no longer needed since [Shift] is disallowed for
							{											// resetting the col-width in YataPanelCols.OnMouseDown()
								ColSort(click_c);

								EnsureDisplayedCol(click_c);
								Invalidator(INVALID_GRID
										  | INVALID_FROZ
										  | INVALID_COLS
										  | INVALID_LBLS);
							}
						}
//						else // popup colhead context
//						{
//							// change colhead text
//							// insert col
//							// fill col-cells w/ text (req. inputbox)
//						}
						break;
				}
			}
		}

		/// <summary>
		/// Gets a selected <c><see cref="Col"/></c> based on x-position of a
		/// mouseclick.
		/// </summary>
		/// <param name="x"></param>
		/// <returns>col-id or <c>-1</c> if out of bounds</returns>
		int getClickedCol(int x)
		{
			x += OffsetHori;

			int left = getLeft();
			int c = FrozenCount - 1;
			do
			{
				if (++c == ColCount)
					return -1;
			}
			while ((left += Cols[c].Width) < x);

			return c;
		}
		#endregion helpers


		#region Sort
		/// <summary>
		/// <c>[Shift]+RMB</c> = sort by id-col
		/// </summary>
		/// <param name="sender"><c><see cref="_labelid"/></c></param>
		/// <param name="e"></param>
		void labelid_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				labelSort(0);
		}

		/// <summary>
		/// <c>[Shift]+RMB</c> = sort by 1st frozen col
		/// </summary>
		/// <param name="sender"><c><see cref="_labelfirst"/></c></param>
		/// <param name="e"></param>
		void labelfirst_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				labelSort(1);
		}

		/// <summary>
		/// <c>[Shift]+RMB</c> = sort by 2nd frozen col
		/// </summary>
		/// <param name="sender"><c><see cref="_labelsecond"/></c></param>
		/// <param name="e"></param>
		void labelsecond_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				labelSort(2);
		}

		/// <summary>
		/// helper for sorting by labels
		/// </summary>
		/// <param name="col"></param>
		void labelSort(int col)
		{
			if (ModifierKeys == Keys.Shift)
			{
				editresultdefault();
				Select();

				ColSort(col);
				EnsureDisplayed();

				Invalidator(INVALID_GRID
						  | INVALID_FROZ
						  | INVALID_COLS
						  | INVALID_LBLS);
			}
		}


		/// <summary>
		/// Hint when table isn't sorted by ID-asc.
		/// </summary>
		static ToolTip TooltipSort = new ToolTip();

		/// <summary>
		/// Sorts rows by a col either ascending or descending.
		/// </summary>
		/// <param name="col">the col-id to sort by</param>
		/// <remarks>Performs a mergesort.</remarks>
		void ColSort(int col)
		{
			DrawRegulator.SuspendDrawing(_f);

			_f.tabclick_DiffReset(null, EventArgs.Empty);

			Changed = true; // TODO: do Changed only if rows are swapped/order is *actually* changed.
			_ur.ResetSaved(true);

			RangeSelect = 0;

			if (_sortdir != SORT_ASC || _sortcol != col)
				_sortdir = SORT_ASC;
			else
				_sortdir = SORT_DES;

			_sortcol = col;


			if (!Settings._strict) // ASSUME people who use strict settings know what they're doing.
			{
				if (_sortcol == 0 && _sortdir == SORT_ASC)
					TooltipSort.SetToolTip(_labelid, "");
				else
					TooltipSort.SetToolTip(_labelid, "warn : Table is not sorted by ascending ID");
			}

			Sorter.TopDownMergeSort(Rows, _sortcol, _sortdir);


			Row row; Cell cell; int presort;
			for (int r = 0; r != RowCount; ++r) // straighten out row._id and cell.y ->
			{
				row = Rows[r];
				presort = row._id;

				row._id_presort = presort;
				row._id = r;

				for (int c = 0; c != ColCount; ++c)
				{
					cell = row[c];

					cell.y_presort = presort;
					cell.y = r;
				}
			}
			_ur.ResetY(); // straighten out row._id and cell.y in UndoRedo's Restorables

			DrawRegulator.ResumeDrawing(_f);
		}
		#endregion Sort
	}
}
