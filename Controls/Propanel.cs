using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Yata's <c>Propanel</c>.
	/// </summary>
	/// <remarks>Each tabbed <c><see cref="YataGrid"/></c> gets its own.</remarks>
	sealed class Propanel
		: Control
	{
		#region Enums
		internal enum DockState
		{ TR, BR, BL, TL }
		#endregion Enums


		#region Fields (static)
		/// <summary>
		/// The height of a row is the same for all <c>Propanels</c>.
		/// </summary>
		static int _heightr = -1;

		/// <summary>
		/// horizontal text padding
		/// </summary>
		const int _padHori = 5;

		/// <summary>
		/// vertical text padding
		/// </summary>
		const int _padVert = 2;

		const int TABFASTEDIT_Bypass = -1;
		#endregion Fields (static)


		#region Fields
		readonly YataGrid _grid;

		internal readonly ScrollBar   _scroll = new VScrollBar();
		internal readonly YataEditbox _editor = new YataEditbox();

		/// <summary>
		/// The height of the entire panel (incl/ non-displayed top and bot).
		/// </summary>
		int HeightProps;

		/// <summary>
		/// The left col in this <c>Propanel</c>.
		/// </summary>
		int _widthVars;

		/// <summary>
		/// The right col in this <c>Propanel</c>.
		/// </summary>
		int _widthVals;

		/// <summary>
		/// The <c>Rectangle</c> on which the <c><see cref="_editor"/></c> shall
		/// be displayed.
		/// </summary>
		Rectangle _editRect;

		/// <summary>
		/// The currently selected row or row of the currently selected cell in
		/// the table.
		/// </summary>
		int _r;

		/// <summary>
		/// The col of the currently selected cell in the table - aka the
		/// corresponding row in this <c>Propanel</c>.
		/// </summary>
		int _c;

		/// <summary>
		/// Overrides <c><see cref="_c"/></c> for TabFastedit while allowing
		/// <c>[Shift]</c> in
		/// <c><see cref="OnMouseClick()">OnMouseClick()</see></c>.
		/// </summary>
		int _tabOverride = TABFASTEDIT_Bypass;

		/// <summary>
		/// Set this <c>true</c> if you want to explicitly accept or reject the
		/// text in the <c><see cref="_editor"/></c>.
		/// </summary>
		internal bool _bypassleaveditor;
		#endregion Fields


		#region Properties
		DockState _dockstate = DockState.TR;
		internal DockState Dockstate
		{
			get { return _dockstate; }
			set
			{
				switch (_dockstate = value)
				{
					case DockState.TR:
						Left = _grid.Width  - Width  - (_grid._visVert ? _grid._scrollVert.Width  : 0);
						Top  = 0;
						break;

					case DockState.BR:
						Left = _grid.Width  - Width  - (_grid._visVert ? _grid._scrollVert.Width  : 0);
						Top  = _grid.Height - Height - (_grid._visHori ? _grid._scrollHori.Height : 0);
						break;

					case DockState.BL:
						Left = YataGrid.WidthRowhead - 1;
						Top  = _grid.Height - Height - (_grid._visHori ? _grid._scrollHori.Height : 0);
						break;

					case DockState.TL:
						Left = YataGrid.WidthRowhead - 1;
						Top  = 0;
						break;
				}
			}
		}
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <remarks>Each tabbed <c><see cref="YataGrid"/></c> gets its own
		/// <c>Propanel</c>.</remarks>
		internal Propanel(YataGrid grid)
		{
			DrawRegulator.SetDoubleBuffered(this);

			_grid = grid;

//			BackColor = ColorOptions._propanel;

			Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			TabStop = false; // <- the Propanel is not currently coded to cope w/ keyboard-input.

			if (Options._font3 != null)
			{
//				Font.Dispose(); // be wary. Be very wary. -> Do NOT Dispose()
				// debug builds don't throw
				// but release builds CTD when invoking the OptionsEditor after
				// the Propanel has been opened ... eg.

				Font = Options._font3;
			}
			else
				Font = new Font("Verdana", 7.25F, FontStyle.Regular, GraphicsUnit.Point, (byte)0);

			if (_heightr == -1)
				_heightr = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, Font) + _padVert * 2;

			_editRect.Height = _heightr - 1; // cf YataGrid.EditCell()

			_scroll.Dock = DockStyle.Right;
			_scroll.LargeChange = _heightr;
			_scroll.ValueChanged += scroll_valuechanged;

			tele();

			Controls.Add(_scroll);
			_grid.Controls.Add(this);

			_editor.Leave += editor_leave;

			Controls.Add(_editor);
		}
		#endregion cTor


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		internal void tele()
		{
			widthVarcol();
			widthValcol();
			telemetric();
		}

		/// <summary>
		/// Determines required width for the variable-fields col.
		/// </summary>
		internal void widthVarcol()
		{
			_widthVars = 0;

			int wT;
			for (int c = 0; c != _grid.ColCount; ++c)
			{
				wT = YataGraphics.MeasureWidth(_grid.Cols[c].text, Font);
				if (wT > _widthVars)
					_widthVars = wT;
			}
			_widthVars += _padHori * 2 + 1;
		}

		/// <summary>
		/// Determines required width for the value-fields col.
		/// </summary>
		internal void widthValcol()
		{
			_widthVals = 0;

//			for (int r = 0; r != _grid.RowCount; ++r) // find the widest field in the entire table ->
//			for (int c = 0; c != _grid.ColCount; ++c)
//			{
//				wT = YataGraphics.MeasureWidth(_grid[r,c].text, Font);
//				if (wT > _widthVals)
//					_widthVals = wT;
//			}
//			_widthVals += _padHori * 2;

			int width, rwidest = 0, cwidest = 0;
			for (int r = 0; r != _grid.RowCount; ++r) // assume that the widest text in the table-font
			for (int c = 0; c != _grid.ColCount; ++c) // will be widest text in the propanel font. Much faster ->
			{
				if ((width = _grid[r,c]._widthtext) > _widthVals)
				{
					_widthVals = width;
					rwidest = r;
					cwidest = c;
				}
			}

			_widthVals =
			_editRect.Width = YataGraphics.MeasureWidth(_grid[rwidest, cwidest].text, Font) + _padHori * 2;
			_editor  .Width = _widthVals - 6; // cf YataGrid.Celledit()
		}

		/// <summary>
		/// Sets the <c>Width</c>/<c>Left</c>/<c>Height</c> values of this
		/// <c>Propanel</c>.
		/// </summary>
		internal void telemetric()
		{
			HeightProps = _grid.ColCount * _heightr;

			Width = _widthVars + _widthVals;

			int h = _grid.ColCount * _heightr;
			int hGrid = _grid.Height - (_grid._visHori ? _grid._scrollHori.Height : 0);
			if (h > hGrid)
				Height = hGrid;
			else
				Height = h;

			InitScroll();

			Dockstate = Dockstate;
		}

		/// <summary>
		/// Initializes scrollbar values.
		/// </summary>
		void InitScroll()
		{
			if (Height < HeightProps)
			{
				_scroll.Visible = true;

				Left  -= _scroll.Width;
				Width += _scroll.Width;

				int vert = HeightProps - Height + (_scroll.LargeChange - 1);
				if (vert < _scroll.LargeChange) vert = 0;

				_scroll.Maximum = vert;	// NOTE: Do not set this until after deciding whether
										// or not max < 0. 'Cause it fucks everything up. bingo.

				// handle .NET OnResize anomaly ->
				// keep the bottom of the table snuggled against the bottom of
				// the visible area when resize enlarges the area
				if (HeightProps < Height + _scroll.Value)
					_scroll.Value = HeightProps - Height;
			}
			else
			{
				_scroll.Value = 0;
				_scroll.Visible = false;
			}
		}

		/// <summary>
		/// Scrolls this <c>Propanel</c> (vertical only).
		/// </summary>
		/// <param name="e"></param>
		internal void Scroll(MouseEventArgs e)
		{
			if (!_editor.Visible)
			{
				if (e.Delta > 0)
				{
					if (_scroll.Value - _scroll.LargeChange < 0)
						_scroll.Value = 0;
					else
						_scroll.Value -= _scroll.LargeChange;
				}
				else if (e.Delta < 0)
				{
					if (_scroll.Value + _scroll.LargeChange + (_scroll.LargeChange - 1) > _scroll.Maximum)
						_scroll.Value = _scroll.Maximum - (_scroll.LargeChange - 1);
					else
						_scroll.Value += _scroll.LargeChange;
				}
			}
		}

		/// <summary>
		/// Ensures that a selected field is displayed within this
		/// <c>Propanel's</c> visible height.
		/// <param name="c">the col-id in the table, the row in the panel</param>
		/// </summary>
		internal void EnsureDisplayed(int c)
		{
			if (_scroll.Visible)
			{
				int y = c * _heightr;
				if (y - _scroll.Value < 0)
				{
					_scroll.Value = y;
				}
				else if ((y += _heightr) - _scroll.Value > Height)
				{
					_scroll.Value = y - Height;
				}
			}
		}

		/// <summary>
		/// Gets the next <c><see cref="DockState"/></c> in the cycle.
		/// </summary>
		/// <param name="reverse"><c>true</c> to cycle counter-clockwise</param>
		/// <returns></returns>
		internal DockState getNextDockstate(bool reverse)
		{
			if (_scroll.Visible)
			{
				switch (Dockstate) // left/right only
				{
					case DockState.TR: return DockState.TL;
					case DockState.BR: return DockState.BL;
					case DockState.BL: return DockState.BR;
				}
			}
			else if (reverse)
			{
				switch (Dockstate) // counterclockwise (corners)
				{
					case DockState.TR: return DockState.TL;
					case DockState.TL: return DockState.BL;
					case DockState.BL: return DockState.BR;
				}
			}
			else
			{
				switch (Dockstate) // clockwise (corners)
				{
					case DockState.TR: return DockState.BR;
					case DockState.BR: return DockState.BL;
					case DockState.BL: return DockState.TL;
				}
			}
			return DockState.TR;
		}
		#endregion Methods


		#region Handlers (scroll)
		/// <summary>
		/// Hides the <c><see cref="_editor"/></c> when this <c>Propanel</c> is
		/// scrolled.
		/// </summary>
		/// <param name="sender"><c><see cref="_scroll"/></c></param>
		/// <param name="e"></param>
		void scroll_valuechanged(object sender, EventArgs e)
		{
			if (_editor.Visible)
			{
				_editor.Visible = false;
				_grid.Select();
			}
			Invalidate(); // not sure why this is suddenly needed now ...
		}
		#endregion Handlers (scroll)


		#region Handlers (editor)
		/// <summary>
		/// Handles the <c>Leave</c> event for the <c><see cref="_editor"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="_editor"/></c></param>
		/// <param name="e"></param>
		/// <remarks>It's better to set (<c>_editor.Visible=false</c>) before
		/// the <c>Leave</c> event fires - note that the <c>Leave</c> event will
		/// still consider the editor <c>Visible</c> - otherwise .net fires the
		/// <c>Leave</c> event twice.</remarks>
		/// <seealso cref="YataGrid"><c>YataGrid.editor_leave()</c></seealso>
		void editor_leave(object sender, EventArgs e)
		{
			//logfile.Log("Propanel.editor_leave() _editor.Visible= " + _editor.Visible + " _bypassleaveditor= " + _bypassleaveditor);

			if (!_bypassleaveditor)
			{
				if (Options._acceptedit)
				{
					//logfile.Log(". Options._acceptedit");
					editresultaccept(); // do NOT focus the table here. Do it in the calling funct if req'd.
				}
				else
				{
					//logfile.Log(". Options._acceptedit FALSE");
					editresultcancel(); // do NOT focus the table.
				}
			}
			else
				_bypassleaveditor = false;
		}

		/// <summary>
		/// Applies a text-edit via <c><see cref="_editor"/></c>.
		/// </summary>
		/// <param name="select"><c>true</c> to focus the <c>YataGrid</c></param>
		/// <seealso cref="YataGrid.editresultaccept()"><c>YataGrid.editresultaccept()</c></seealso>
		internal void editresultaccept(bool @select = false)
		{
			//logfile.Log("Propanel.editresultaccept()");

			bool sanitized = false;

			Cell cell = _grid[_r,_c];
			if (_editor.Text != cell.text)
			{
				sanitized = _grid.ChangeCellText(cell, _editor); // does a text-check
				_grid.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
			}
			else
			{
				if (cell.replaced)
					_grid.ClearReplaced(cell);

				if (cell.loadchanged)
					_grid.ClearLoadchanged(cell);
			}

			editresultcancel(@select);

			if (sanitized)
			{
				using (var ib = new Infobox(Infobox.Title_warni,
											"The text that was submitted has been altered.",
											null,
											InfoboxType.Warn))
				{
					ib.ShowDialog(_grid._f);
				}
			}
		}

		/// <summary>
		/// Hides the <c><see cref="_editor"/></c>.
		/// </summary>
		/// <param name="select"><c>true</c> to focus the <c>YataGrid</c> - if
		/// <c>false</c> the calling funct shall focus a <c>Control</c> itself</param>
		/// <seealso cref="YataGrid.editresultcancel()"><c>YataGrid.editresultcancel()</c></seealso>
		internal void editresultcancel(bool @select = false)
		{
			//logfile.Log("Propanel.editresultcancel() _editor.Visible= " + _editor.Visible);

			_editor.Visible = false;
			_grid.Invalidator(YataGrid.INVALID_PROP);

			if (@select) _grid.Select();
		}
		#endregion Handlers (editor)


		#region Handlers (override)
#if Keys
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			if ((e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("Propanel.OnPreviewKeyDown() e.KeyData= " + e.KeyData + " e.IsInputKey= " + e.IsInputKey);

			base.OnPreviewKeyDown(e);
		}
#endif

		/// <summary>
		/// Processes key-input for this <c>Propanel</c>.
		/// <list type="bullet">
		/// <item><c>[Enter]</c> - accept edit</item>
		/// <item><c>[Escape]</c> - cancel edit</item>
		/// </list>
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("Propanel.ProcessCmdKey() keyData= " + keyData);
#endif
			switch (keyData)
			{
				case Keys.Enter:
					// NOTE: [Enter] on the grid is processed by YataGrid.ProcessCmdKey()
					if (_editor.Visible)
					{
						_bypassleaveditor = true;
						editresultaccept(true);
#if Keys
						logfile.Log(". Propanel.ProcessCmdKey force TRUE (accept propanel-edit)");
#endif
						return true;
					}
					break;

				case Keys.Escape:
					// NOTE: [Escape] on the grid is processed by YataGrid.ProcessCmdKey()
					if (_editor.Visible)
					{
						_bypassleaveditor = true;
						editresultcancel(true);
#if Keys
						logfile.Log(". Propanel.ProcessCmdKey force TRUE (cancel propanel-edit)");
#endif
						return true;
					}
					break;
			}

			bool ret = base.ProcessCmdKey(ref msg, keyData);
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". Propanel.ProcessCmdKey ret= " + ret);
#endif
			return ret;
		}

#if Keys
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("Propanel.IsInputKey() keyData= " + keyData);

			bool ret = base.IsInputKey(keyData);
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". Propanel.IsInputKey ret= " + ret);

			return ret;
		}
#endif

		/// <summary>
		/// Processes a so-called dialog-key. Use this for TabFastedit
		/// keystrokes only - process keystrokes for other operations in
		/// <c><see cref="ProcessCmdKey()">ProcessCmdKey()</see></c>.
		/// <list type="bullet">
		/// <item><c>[Tab]</c> or <c>[Down]</c> - fastedit down</item>
		/// <item><c>[Shift+Tab]</c> or <c>[Up]</c> - fastedit up</item>
		/// </list></summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		/// <remarks>Certain keystrokes need a return of <c>false</c> in
		/// <c><see cref="YataEditbox"/>.IsInputKey()</c>. Cf
		/// <c><see cref="YataGrid"></see>.ProcessDialogKey()</c>.</remarks>
		/// <seealso cref="YataGrid.IsTabfasteditKey()"><c>YataGrid.IsTabfasteditKey()</c></seealso>
		/// <remarks>These very likely could and should be put in
		/// <c><see cref="ProcessCmdKey()">ProcessCmdKey()</see></c> but I'm
		/// going to keep them here just to differentiate the TabFastedit keys.</remarks>
		protected override bool ProcessDialogKey(Keys keyData)
		{
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("Propanel.ProcessDialogKey() keyData= " + keyData);
#endif
			if (_editor.Visible)
			{
				switch (keyData)
				{
					// TabFastedit ->
					case Keys.Tab:
					case Keys.Down:
						_bypassleaveditor = true;
						editresultaccept();

						if (_c != _grid.ColCount - 1)
						{
							_tabOverride = _c + 1;
							OnMouseClick(new MouseEventArgs(MouseButtons.Left,
															1,
															_widthVars + 1, 0, // fake it ...
															0));
							_tabOverride = TABFASTEDIT_Bypass;
						}
						else
							_grid.Select();
#if Keys
						logfile.Log(". Propanel.ProcessDialogKey force TRUE (is TabFastedit)");
#endif
						return true;

					case Keys.Shift | Keys.Tab:
					case Keys.Up:
						_bypassleaveditor = true;
						editresultaccept();

						if (_c != 0)
						{
							_tabOverride = _c - 1;
							OnMouseClick(new MouseEventArgs(MouseButtons.Left,
															1,
															_widthVars + 1, 0, // fake it ...
															0));
							_tabOverride = TABFASTEDIT_Bypass;
						}
						else
							_grid.Select();
#if Keys
						logfile.Log(". Propanel.ProcessDialogKey force TRUE (is TabFastedit)");
#endif
						return true;
				}
			}

			bool ret = base.ProcessDialogKey(keyData);
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". Propanel.ProcessDialogKey ret= " + ret);
#endif
			return ret;
		}

#if Keys
		/// <summary>
		/// This should never fire I believe since (<c>TabStop=false</c>) and so
		/// this <c>Propanel</c> can never have focus.
		/// 
		/// 
		/// Yeah right ... their wonkey .net bubbling pattern can do anything it
		/// likes.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if ((e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("Propanel.OnKeyDown() ke.KeyData= " + e.KeyData);

			base.OnKeyDown(e);
		}
#endif

		/// <summary>
		/// Clears cords on the statusbar when the mouse enters the control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseEnter(EventArgs e)
		{
			_grid._f.PrintInfo(); // clear
		}


		/// <summary>
		/// Overrides the <c>MouseClick</c> event. Selects the corresponding
		/// <c><see cref="Cell"/></c> on the <c><see cref="_grid"/></c> that
		/// gets clicked in this <c>Propanel</c>, or if a
		/// <c><see cref="Row"/></c> is currently selected on the <c>_grid</c>
		/// then it will be ensured-displayed; starts or applies/cancels an edit
		/// if applicable. <c>OnMouseClick</c> also handles TabFastedit with
		/// faked mouseclicks.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
#if Clicks
			logfile.Log("Propanel.OnMouseClick() " + e.Button + " _tabOverride= " + _tabOverride);
#endif
			if ((ModifierKeys & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				if (!_editor.Visible)
				{
					switch (e.Button)
					{
						case MouseButtons.Left:
							if (_grid.RangeSelect == 0
								&& ((ModifierKeys & Keys.Shift) == Keys.None
									|| _tabOverride != TABFASTEDIT_Bypass))
							{
								if ((_r = _grid.getSelectedRowOrCells(true)) != -1)
								{
									_grid._editor.Visible = false; // edit result default on YataGrid's editor (if visible).

									if (_tabOverride == TABFASTEDIT_Bypass)
									{
										_c = col(e.Y);
									}
									else
										_c = _tabOverride;

									if (_grid.getSelectedRow() == -1)
									{
										for (int c = 0; c != _grid.ColCount; ++c)
											_grid[_r,c].selected = false;

										_grid.SelectCell(_grid[_r,_c]);
									}
									else
									{
										_grid.EnsureDisplayedRow(_r);
										_grid.EnsureDisplayedCol(_c);
									}

									EnsureDisplayed(_c);


									if (!_grid.Readonly && e.X > _widthVars)
									{
#if Clicks
										logfile.Log(". . start edit");
#endif
										_editRect.X = _widthVars;
										_editRect.Y = _c * _heightr + 1;

										_editor.Left = _editRect.X + 5; // cf YataGrid.EditCell() ->
										_editor.Top  = _editRect.Y + 1 - _scroll.Value;
										_editor.Text = _grid[_r,_c].text;

										_editor.SelectionStart = 0;
										_editor.SelectionLength = _editor.Text.Length;

										_editor.Visible = true;
										_editor.Focus();
									}
									else
									{
#if Clicks
										logfile.Log(". . select grid");
#endif
										_grid.Select();
									}

									_grid.Invalidator(YataGrid.INVALID_GRID
													| YataGrid.INVALID_FROZ
													| YataGrid.INVALID_PROP);
								}
								else
								{
#if Clicks
									logfile.Log(". select grid");
#endif
									_grid.EnsureDisplayedCol(col(e.Y));
									_grid.Select();
								}
							}
							break;

						case MouseButtons.Right:
							_grid.Propanel.Dockstate = _grid.Propanel.getNextDockstate((ModifierKeys & Keys.Shift) == Keys.Shift);
							break;
					}
				}
				else if ((ModifierKeys & Keys.Shift) == Keys.None) // _editor.Visible
				{
					switch (e.Button)
					{
						case MouseButtons.Left: // accept edit
#if Clicks
							logfile.Log(". accept edit");
#endif
							_bypassleaveditor = true;
							editresultaccept(true);
							break;

						case MouseButtons.Right: // cancel edit
#if Clicks
							logfile.Log(". cancel edit");
#endif
							_bypassleaveditor = true;
							editresultcancel(true);
							break;

						default:
#if Clicks
							logfile.Log(". edit result default");
#endif
							_editor.Visible = false;
							_grid.Select();
							break;
					}
				}
			}
		}

		/// <summary>
		/// Gets the col-id that corresponds to pixel <paramref name="y"/> down
		/// the y-axis.
		/// </summary>
		/// <param name="y"><c>MouseEventArgs.Y</c></param>
		/// <returns>the colid</returns>
		/// <remarks>Helper for
		/// <c><see cref="OnMouseClick()">OnMouseClick()</see></c>.</remarks>
		int col(int y)
		{
			return (y + _scroll.Value) / _heightr;
		}


		/// <summary>
		/// Paints this <c>Propanel</c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			BackColor = ColorOptions._propanel;

			Graphics graphics = e.Graphics;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			int offset = _scroll.Value;

			Rectangle rect;

			// fill the frozen-field(s) var/val-rect ->
			int c;
			for (c = 0; c != _grid.FrozenCount; ++c)
			{
				rect = new Rectangle(0, c * _heightr - offset, Width, _heightr);
				graphics.FillRectangle(ColorOptions._propanelfrozen, rect);
			}

			// fill any selected cell's var/val-rect ->
			Cell sel = _grid.getSelectedCell();
			if (sel != null)
			{
				rect = new Rectangle(0, sel.x * _heightr - offset, Width, _heightr);
				graphics.FillRectangle(ColorOptions._propanelsel, rect);
			}

			// fill the editor's val-rect if visible ->
			if (_editor.Visible)
			{
				rect = _editRect;
				rect.Y -= offset;

				graphics.FillRectangle(ColorOptions._celledit, rect);
			}

			// draw border and center lines ->
			graphics.DrawLine(ColorOptions._propanelborder,						// vertical left line
							  1, 0,
							  1, HeightProps - offset);
			graphics.DrawLine(ColorOptions._propanellines,						// vertical center line
							  _widthVars, 1,
							  _widthVars, HeightProps - offset - 1);
			graphics.DrawLine(ColorOptions._propanelborder,						// vertical right line
							  _widthVars + _widthVals, 1,
							  _widthVars + _widthVals, HeightProps - offset - 1);
			graphics.DrawLine(ColorOptions._propanelborder,						// horizontal top line
							  1,     1,
							  Width, 1);
			graphics.DrawLine(ColorOptions._propanelborder,						// horizontal bot line
							  1,     _heightr * _grid.ColCount - offset,
							  Width, _heightr * _grid.ColCount - offset);

			// draw horizontal lines ->
			int y;
			for (c = 1; c != _grid.ColCount; ++c)
			{
				y = _heightr * c - offset + 1;
				graphics.DrawLine(ColorOptions._propanellines,
								  0,     y,
								  Width, y);
			}

			// draw var-texts ->
			rect = new Rectangle(_padHori, 0, _widthVars, _heightr);

			for (c = 0; c != _grid.FrozenCount; ++c)
			{
				rect.Y = _heightr * c - offset;
				TextRenderer.DrawText(graphics,
									  _grid.Cols[c].text,
									  Font,
									  rect,
									  ColorOptions._propanelfrozen_t,
									  YataGraphics.flags);
			}

			for (; c != _grid.ColCount; ++c)
			{
				rect.Y = _heightr * c - offset;
				TextRenderer.DrawText(graphics,
									  _grid.Cols[c].text,
									  Font,
									  rect,
									  (sel != null && c == sel.x) ? ColorOptions._propanelsel_t
																  : ColorOptions._propanel_t,
									  YataGraphics.flags);
			}

			// draw val-texts ->
			if (_grid.RangeSelect == 0)
			{
				int r = _grid.getSelectedRowOrCells(true);
				if (r != -1)
				{
					rect.X    += _widthVars;
					rect.Width = _widthVals;

					for (c = 0; c != _grid.FrozenCount; ++c)
					{
						rect.Y = _heightr * c - offset;
						TextRenderer.DrawText(graphics,
											  _grid[r,c].text,
											  Font,
											  rect,
											  ColorOptions._propanelfrozen_t,
											  YataGraphics.flags);
					}

					for (; c != _grid.ColCount; ++c)
					{
						rect.Y = _heightr * c - offset;
						TextRenderer.DrawText(graphics,
											  _grid[r,c].text,
											  Font,
											  rect,
											  (sel != null && c == sel.x) ? ColorOptions._propanelsel_t
																		  : ColorOptions._propanel_t,
											  YataGraphics.flags);
					}
				}
			}
		}
		#endregion Handlers (override)
	}
}
