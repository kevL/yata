using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Yata's <c>PropertyPanel</c>.
	/// </summary>
	/// <remarks> Each tabbed table gets its own.</remarks>
	sealed class PropertyPanel
		: Control
	{
		#region Enums
		internal enum DockState
		{ TR, BR, BL, TL }
		#endregion Enums


		#region Fields (static)
		static int _heightr = -1; // height of a row is the same for all propanels

		const int _padHori = 5; // horizontal text padding
		const int _padVert = 2; // vertical text padding
		#endregion Fields (static)


		#region Fields
		readonly YataGrid _grid;

		internal readonly ScrollBar _scroll = new VScrollBar();
		internal readonly TextBox   _editor = new TextBox();

		readonly int HeightProps; // height of the entire panel (incl/ non-displayed top & bot)

		readonly int _widthVars;	// left col
		int _widthVals;				// right col

		Rectangle _editRect;
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
		/// cTor. Each tabbed table gets its own Propanel.
		/// </summary>
		internal PropertyPanel(YataGrid grid)
		{
			DrawingControl.SetDoubleBuffered(this);

			_grid = grid;

			if (Settings._gradient)
				BackColor = Color.SkyBlue;
			else
				BackColor = Color.LightBlue;

			Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

			if (Settings._font3 != null)
			{
				Font.Dispose(); // be wary. Be very wary.
				Font = Settings._font3;
			}
			else
				Font = new Font("Verdana", 7.25F, FontStyle.Regular, GraphicsUnit.Point, (byte)0);

			if (_heightr == -1)
				_heightr = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, Font) + _padVert * 2;

			_editRect.Height = _heightr - 1; // cf YataGrid.EditCell()

			HeightProps = _grid.ColCount * _heightr;

			int wT;
			for (int c = 0; c != _grid.ColCount; ++c)
			{
				wT = YataGraphics.MeasureWidth(_grid.Cols[c].text, Font);
				if (wT > _widthVars)
					_widthVars = wT;
			}
			_widthVars += _padHori * 2 + 1;

			_scroll.Dock = DockStyle.Right;
			_scroll.LargeChange = _heightr;
			_scroll.ValueChanged += OnScrollValueChanged;

			widthValcol();

			Controls.Add(_scroll);

			_grid.Controls.Add(this);

			_editor.Visible     = false;
			_editor.BackColor   = Colors.Editor;
			_editor.BorderStyle = BorderStyle.None;
			_editor.WordWrap    = false;
			_editor.Margin      = new Padding(0);
			_editor.LostFocus  += lostfocus_Editor;
			_editor.KeyDown    += keydown_Editor;
			_editor.Leave      += leave_Editor;

			Controls.Add(_editor);
		}
		#endregion cTor


		#region Methods
		/// <summary>
		/// Determines required width for the value-fields col.
		/// </summary>
		internal void widthValcol()
		{
//			for (int r = 0; r != _grid.RowCount; ++r)
//			for (int c = 0; c != _grid.ColCount; ++c)
//			{
//				wT = YataGraphics.MeasureWidth(_grid[r,c].text, Font);
//				if (wT > _widthVals)
//					_widthVals = wT;
//			}
//			_widthVals += _padHori * 2;

			_widthVals = 0;

			int wT, rT = 0, cT = 0;
			for (int r = 0; r != _grid.RowCount; ++r)
			for (int c = 0; c != _grid.ColCount; ++c)
			{
				if ((wT = _grid[r,c]._widthtext) > _widthVals)	// NOTE: Assume that the widest text in the table-font
				{												// will be widest text in the propanel font. Much faster.
					_widthVals = wT;
					rT = r;
					cT = c;
				}
			}

			_widthVals =
			_editRect.Width = YataGraphics.MeasureWidth(_grid[rT,cT].text, Font) + _padHori * 2;
			_editor  .Width = _widthVals - 6; // cf YataGrid.EditCell()

			telemetric();
		}

		/// <summary>
		/// Sets the <c>Width</c>/<c>Left</c>/<c>Height</c> values of this
		/// <c>PropertyPanel</c>.
		/// </summary>
		internal void telemetric()
		{
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
		/// Scrolls this <c>PropertyPanel</c> (vertical only).
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
		/// <c>PropertyPanel's</c> visible height.
		/// <param name="c">the col in the table, the row in the panel</param>
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
		/// Applies a text-edit via <c><see cref="_editor"/></c>.
		/// </summary>
		void ApplyCellEdit()
		{
			Cell cell = _grid[_r,_c];
			if (_editor.Text != cell.text)
			{
				_grid.ChangeCellText(cell, _editor); // does a text-check
				_grid.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
			}
			else if (cell.loadchanged)
				_grid.ClearLoadchanged(cell);
		}

		/// <summary>
		/// Hides the <c><see cref="_editor"/></c>.
		/// </summary>
		void hideEditor()
		{
			_editor.Visible = false;
			_grid.Invalidator(YataGrid.INVALID_PROP);
		}

		/// <summary>
		/// Gets the next <c><see cref="DockState"/></c> in the cycle.
		/// </summary>
		/// <returns></returns>
		internal DockState getNextDockstate()
		{
			if (_scroll.Visible)
			{
				switch (Dockstate) // left/right
				{
					case DockState.TR: return DockState.TL;
					case DockState.BR: return DockState.BL;
					case DockState.BL: return DockState.BR;
				}
			}
			else if ((ModifierKeys & Keys.Shift) == 0)
			{
				switch (Dockstate) // clockwise
				{
					case DockState.TR: return DockState.BR;
					case DockState.BR: return DockState.BL;
					case DockState.BL: return DockState.TL;
				}
			}
			else // [Shift] reverse cycle direction
			{
				switch (Dockstate) // counterclockwise
				{
					case DockState.TR: return DockState.TL;
					case DockState.TL: return DockState.BL;
					case DockState.BL: return DockState.BR;
				}
			}
			return DockState.TR;
		}
		#endregion Methods


		#region Events (scroll)
		/// <summary>
		/// Hides the editor when this <c>PropertyPanel</c> is scrolled.
		/// </summary>
		/// <param name="sender"><c><see cref="_scroll"/></c></param>
		/// <param name="e"></param>
		void OnScrollValueChanged(object sender, EventArgs e)
		{
			hideEditor();
		}
		#endregion Events (scroll)


		#region Events (editor)
		/// <summary>
		/// Handles the cell-editor's <c>KeyDown</c> event.
		/// </summary>
		/// <param name="sender"><c><see cref="_editor"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Works around dweeby .NET behavior if <c>[Alt]</c> is
		/// pressed.</remarks>
		void keydown_Editor(object sender, KeyEventArgs e)
		{
			if (e.Alt) hideEditor();
		}

		/// <summary>
		/// Handles the cell-editor's <c>Leave</c> event.
		/// </summary>
		/// <param name="sender"><c><see cref="_editor"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Works around dweeby .NET behavior if <c>[Ctrl+PageUp]</c>
		/// or <c>[Ctrl+PageDown]</c> is pressed.</remarks>
		void leave_Editor(object sender, EventArgs e)
		{
			if (_editor.Visible
				&& (ModifierKeys & Keys.Control) == Keys.Control)
			{
				_editor.Focus(); // ie. don't leave editor.
			}
		}

		/// <summary>
		/// Handles the cell-editor's <c>LostFocus</c> event.
		/// </summary>
		/// <param name="sender"><c><see cref="_editor"/></c></param>
		/// <param name="e"></param>
		void lostfocus_Editor(object sender, EventArgs e)
		{
			hideEditor();
		}
		#endregion Events (editor)


		#region Events (override)
		/// <summary>
		/// Handles ending editing a cell by pressing <c>[Enter]</c> or
		/// <c>[Escape]</c>/<c>[Tab]</c> - this fires during edit or so.
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (_editor.Visible)
			{
				switch (keyData)
				{
					case Keys.Enter:
						ApplyCellEdit();
						goto case Keys.Escape;

					case Keys.Escape:
					case Keys.Tab:
						hideEditor();
						_grid.Select();
						return true;
				}
			}
			return base.ProcessDialogKey(keyData);
		}


		/// <summary>
		/// Clears cords on the statusbar when the mouse enters the control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseEnter(EventArgs e)
		{
			_grid._f.PrintInfo(); // clear
		}


		int _r; // -> the row in the table.
		int _c; // -> the col in the table, the row in the panel.

		/// <summary>
		/// Overrides the <c>MouseClick</c> event. Selects the row clicked,
		/// starts or applies/cancels edit.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (!_editor.Visible)
			{
				Cell sel = _grid.getSelectedCell();
				if (sel != null)
					_r = sel.y;
				else
					_r = _grid.getSelectedRow();

				if (_r != -1)
				{
					_grid._editor.Visible = false;

					_c = (e.Y + _scroll.Value) / _heightr;

					if (sel != null)
					{
						sel.selected = false;

						(_grid._anchorcell = _grid[_r,_c]).selected = true;

//						_grid._f.EnableCelleditOperations(); // should be no need to re-deter cell-operations here.
					}

					if (_c >= _grid.FrozenCount)
						_grid.EnsureDisplayed();
					else
						_grid.EnsureDisplayedRow(_r);

					EnsureDisplayed(_c);

					if (!_grid.Readonly && e.X > _widthVars)
					{
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
						_grid.Select();

					_grid.Invalidator(YataGrid.INVALID_GRID
									| YataGrid.INVALID_FROZ
									| YataGrid.INVALID_PROP);
				}
				else
					_grid.Select();
			}
			else // is edit
			{
				if (e.Button == MouseButtons.Left) // accept edit (else cancel edit)
					ApplyCellEdit();

				hideEditor();
				_grid.Select();
			}
		}


		/// <summary>
		/// Paints this <c>PropertyPanel</c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			YataGrid.graphics = e.Graphics;
			YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			int offset = _scroll.Value;

			Rectangle rect;

			// fill the frozen-field(s) var/val-rect ->
			int c;
			for (c = 0; c != _grid.FrozenCount; ++c)
			{
				rect = new Rectangle(0, c * _heightr - offset, Width, _heightr);
				YataGrid.graphics.FillRectangle(Brushes.PropanelFrozen, rect);
			}

			// fill any selected cell's var/val-rect ->
			Cell sel = _grid.getSelectedCell();
			if (sel != null)
			{
				rect = new Rectangle(0, sel.x * _heightr - offset, Width, _heightr);
				YataGrid.graphics.FillRectangle(Brushes.Selected, rect);
			}

			// fill the editor's val-rect if visible ->
			if (_editor.Visible)
			{
				rect = new Rectangle();
				rect = _editRect;
				rect.Y -= offset;

				YataGrid.graphics.FillRectangle(Brushes.Editor, rect);
			}

			// draw vertical lines ->
			YataGrid.graphics.DrawLine(Pens.Black,									// vertical left line
									   1, 0,
									   1, HeightProps - offset);
			YataGrid.graphics.DrawLine(Pencils.DarkLine,							// vertical center line
									   _widthVars, 1,
									   _widthVars, HeightProps - offset - 1);
			YataGrid.graphics.DrawLine(Pencils.DarkLine,							// vertical right line
									   _widthVars + _widthVals, 1,
									   _widthVars + _widthVals, HeightProps - offset - 1);
			YataGrid.graphics.DrawLine(Pens.Black,									// horizontal top line
									   1,     1,
									   Width, 1);
			YataGrid.graphics.DrawLine(Pens.Black,									// horizontal bot line
									   1,     _heightr * _grid.ColCount - offset,
									   Width, _heightr * _grid.ColCount - offset);

			// draw horizontal lines ->
			int y;
			for (c = 1; c != _grid.ColCount; ++c)
			{
				y = _heightr * c - offset + 1;
				YataGrid.graphics.DrawLine(Pencils.DarkLine,
										   0,     y,
										   Width, y);
			}

			// draw var-texts ->
			rect = new Rectangle(_padHori, 0, _widthVars, _heightr);

			for (c = 0; c != _grid.ColCount; ++c)
			{
				rect.Y = _heightr * c - offset;
				TextRenderer.DrawText(YataGrid.graphics,
									  _grid.Cols[c].text,
									  Font,
									  rect,
									  Colors.Text,
									  YataGraphics.flags);
			}

			// draw val-texts ->
			int r;
			if (sel != null)
				r = sel.y;
			else
				r = _grid.getSelectedRow();

			if (r != -1)
			{
				rect.X    += _widthVars;
				rect.Width = _widthVals;

				for (c = 0; c != _grid.ColCount; ++c)
				{
					rect.Y = _heightr * c - offset;
					TextRenderer.DrawText(YataGrid.graphics,
										  _grid[r,c].text,
										  Font,
										  rect,
										  Colors.Text,
										  YataGraphics.flags);
				}
			}
		}
		#endregion Events (override)
	}
}
