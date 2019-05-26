using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace yata
{
	sealed class PropertyPanelButton
		:
			Button
	{
		#region Fields (static)
		internal static int HEIGHT = 20;
		#endregion Fields (static)


		#region Fields
		readonly Rectangle _rectBg;
		readonly Rectangle _rectGr;
		#endregion Fields


		#region Properties
		internal bool Depressed
		{ get; set; }
		#endregion Properties


		#region cTor
		internal PropertyPanelButton()
		{
			DrawingControl.SetDoubleBuffered(this);

			// NOTE: .NET is using the default vals for button's Width/Height
			// here. So set it explicitly.
			Width  =
			Height = HEIGHT;

			_rectBg = new Rectangle(0,0, Width, Height);
			_rectGr = new Rectangle(3,3, Width - 6, Height - 6);
		}
		#endregion cTor


		#region Events (override)
		/// <summary>
		/// Since right-click on a button does not visually depress it do this.
		/// </summary>
		/// <param name="pevent"></param>
		protected override void OnPaint(PaintEventArgs pevent)
		{
			YataGrid.graphics = pevent.Graphics;
			YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			// and you know it don't come easy - Ringo
			YataGrid.graphics.FillRectangle(Brushes.PropanelButton, _rectBg);

			if (Depressed)
			{
				YataGrid.graphics.FillRectangle(Gradients.PropanelButton, _rectGr);

				var pen1 = Pens.Black;
				var pen2 = Pencils.DarkLine;

				YataGrid.graphics.DrawLine(pen1, 2, 2,          Width - 2, 2);			// hori top
				YataGrid.graphics.DrawLine(pen2, 2, 3,          Width - 2, 3);

				YataGrid.graphics.DrawLine(pen1, 2, Height - 1, Width - 2, Height - 1);	// hori bot
				YataGrid.graphics.DrawLine(pen2, 2, Height - 2, Width - 3, Height - 2);

				YataGrid.graphics.DrawLine(pen1, 2, 2,          2, Height - 2);			// vert left
				YataGrid.graphics.DrawLine(pen2, 3, 2,          3, Height - 2);

				YataGrid.graphics.DrawLine(pen1, Width - 1, 2,  Width - 1, Height - 2);	// vert right
				YataGrid.graphics.DrawLine(pen2, Width - 2, 2,  Width - 2, Height - 2);
			}
			else
				base.OnPaint(pevent);
		}
		#endregion Events (override)
	}


	sealed class PropertyPanel
		:
			Control
	{
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
		bool _dockBot;
		internal bool DockBot
		{
			get { return _dockBot; }
			set
			{
				if (_dockBot = value)
					Top = _grid.Height - (_grid._visHori ? _grid._scrollHori.Height : 0) - Height;
				else
					Top = 0;
			}
		}
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal PropertyPanel(YataGrid grid)
		{
			DrawingControl.SetDoubleBuffered(this);

			_grid = grid;

			if (Settings._gradient)
				BackColor = Color.SkyBlue;
			else
				BackColor = Color.LightBlue;

			ForeColor = Colors.Text;

			Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);

			if (Settings._font3 != null)
			{
//				Font.Dispose(); // NOTE: Don't dispose that; it will be needed when another PropertyPanel instantiates.
				Font = Settings._font3;
			}
			else
				Font = new System.Drawing.Font("Verdana", 7.5F, FontStyle.Regular, GraphicsUnit.Point, (byte)0);

			if (_heightr == -1)
				_heightr = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, Font) + _padVert * 2;

			HeightProps = _grid.ColCount * _heightr;

			int wT;
			for (int c = 0; c != _grid.ColCount; ++c)
			{
				wT = YataGraphics.MeasureWidth(_grid.Cols[c].text, Font);
				if (wT > _widthVars)
					_widthVars = wT;
			}
			_widthVars += _padHori * 2 + 1;

			deterValfieldWidth();
			deterTelemetric();

			_scroll.Dock = DockStyle.Right;
			_scroll.ValueChanged += OnScrollValueChanged;

			InitScroll();
			Controls.Add(_scroll);

			_grid.Controls.Add(this);

			_editor.Visible = false;
			_editor.BackColor = Colors.Editor;
			_editor.BorderStyle = BorderStyle.None;
			_editor.Height = _heightr;
			_editor.LostFocus += lostfocus_Editor;
			_editor.KeyDown   += keydown_Editor;
			_editor.Leave     += leave_Editor;

			Controls.Add(_editor);
		}
		#endregion cTor


		#region Methods
		internal void deterValfieldWidth()
		{
//			for (int r = 0; r != _grid.RowCount; ++r)
//			{
//				for (int c = 0; c != _grid.ColCount; ++c)
//				{
//					wT = YataGraphics.MeasureWidth(_grid[r,c].text, Font);
//					if (wT > _widthVals)
//						_widthVals = wT;
//				}
//			}
//			_widthVals += _padHori * 2;

			_widthVals = 0;

			int wT, rT = 0, cT = 0;
			for (int r = 0; r != _grid.RowCount; ++r)
			{
				for (int c = 0; c != _grid.ColCount; ++c)
				{
					wT = _grid[r,c]._widthtext;	// ASSUME: That the widest text in the table-font
					if (wT > _widthVals)		// will be widest text in the propanel font. Much faster.
					{
						_widthVals = wT;
						rT = r;
						cT = c;
					}
				}
			}
			_widthVals = YataGraphics.MeasureWidth(_grid[rT,cT].text, Font) + _padHori * 2;

			_editor.Width = _widthVals;
		}

		/// <summary>
		/// Sets the 'Width', 'Left', and 'Height' values of this property panel.
		/// </summary>
		internal void deterTelemetric()
		{
			Width = _widthVars + _widthVals;

			Left = _grid.Left - (_grid._visVert ? _grid._scrollVert.Width : 0) + _grid.Width - Width;

			int h = _grid.ColCount * _heightr;
			int hGrid = _grid.Height - (_grid._visHori ? _grid._scrollHori.Height : 0);
			if (h > hGrid)
				Height = hGrid;
			else
				Height = h;
		}

		/// <summary>
		/// Calculates scrollbar values.
		/// </summary>
		internal void InitScroll()
		{
			if (Height < HeightProps)
			{
				_scroll.Visible = true;

				Left  -= _scroll.Width;
				Width += _scroll.Width;

				int vert = HeightProps
						 + _scroll.LargeChange - 1
						 - Height;

				if (vert < _scroll.LargeChange) vert = 0;

				_scroll.Maximum = vert;	// NOTE: Do not set this until after deciding
										// whether or not max < 0. 'Cause it fucks everything up. bingo.

				// handle .NET OnResize anomaly ->
				// keep the bottom of the table snuggled against the bottom of the visible area
				// when resize enlarges the area
				if (HeightProps < Height + _scroll.Value)
				{
					_scroll.Value = HeightProps - Height;
				}
			}
			else
			{
				_scroll.Value = 0;
				_scroll.Visible = false;
			}
		}

		/// <summary>
		/// Scrolls this property panel (vertical only).
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
		/// Applies a text-edit via the editbox.
		/// </summary>
		void ApplyTextEdit()
		{
			Cell cell = _grid[_r,_c];
			string text = _editor.Text;
			if (text != cell.text)
				_grid.ChangeCellText(cell, _editor);
		}
		#endregion Methods


		#region Events (scroll)
		void OnScrollValueChanged(object sender, EventArgs e)
		{
			lostfocus_Editor(null, EventArgs.Empty);
		}
		#endregion Events (scroll)


		#region Events (editor)
		/// <summary>
		/// Handles keydown events in the cell-editor.
		/// @note Works around dweeby .NET behavior if Alt is pressed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void keydown_Editor(object sender, KeyEventArgs e)
		{
			if (e.Alt)
				lostfocus_Editor(null, EventArgs.Empty);
		}

		/// <summary>
		/// Handles the editor losing focus.
		/// @note This funct is a partial catchall for other places where the
		/// editor needs to hide.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void lostfocus_Editor(object sender, EventArgs e)
		{
			_editor.Visible = false;
			Refresh();
		}

		/// <summary>
		/// Handles leave event in the cell-editor.
		/// @note Works around dweeby .NET behavior if Ctrl+PageUp/Down is
		/// pressed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void leave_Editor(object sender, EventArgs e)
		{
			if ((ModifierKeys & Keys.Control) == Keys.Control
				&& _editor.Visible)
			{
				_editor.Focus(); // ie. don't leave editor.
			}
		}
		#endregion Events (editor)


		#region Events (override)
		/// <summary>
		/// Handles ending editing a cell by pressing Enter or Escape/Tab - this
		/// fires during edit or so.
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
						ApplyTextEdit();
						goto case Keys.Escape;

					case Keys.Escape:
					case Keys.Tab:
						_editor.Visible = false;
						_grid.Refresh();
						_grid.Select();
						return true;
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			if (!_editor.ContainsFocus)
				lostfocus_Editor(null, EventArgs.Empty);

//			base.OnLostFocus(e);
		}

		/// <summary>
		/// Clears cords on the statusbar when the mouse enters the control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseEnter(EventArgs e)
		{
			_grid._f.PrintInfo(); // clear

//			base.OnMouseEnter(e);
		}


		int _r; // -> the row in the table.
		int _c; // -> the col in the table, the row in the panel.

		/// <summary>
		/// Handles a mouse-click event. Selects the row clicked, starts or
		/// applies/cancels edit.
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
					Focus(); // snap

					_c = (e.Y + _scroll.Value) / _heightr;

					if (sel != null && _c >= _grid.FrozenCount)
					{
						sel.selected = false;
						_grid[_r,_c].selected = true;
					}

					_grid.EnsureDisplayedCellOrRow();
					_grid.Invalidate();

					if (!_grid.Readonly && e.X > _widthVars)
					{
						_editRect.X      = _widthVars;
						_editRect.Y      = _c * _heightr + 1;
						_editRect.Width  = _widthVals;
						_editRect.Height = _heightr - 1;

						_editor.Left = _editRect.X + 5;
						_editor.Top  = _editRect.Y + 1 - _scroll.Value;
						_editor.Text = _grid[_r,_c].text;

						_editor.Visible = true;
						_editor.Focus();

						_editor.SelectionStart  = 0; // because .NET
						_editor.SelectionLength = _editor.Text.Length;
//						if (_editor.Text == Constants.Stars)
//							_editor.SelectionLength = _editor.Text.Length;
//						else
//							_editor.SelectionStart = _editor.Text.Length;
					}

					Invalidate();
				}
			}
			else
			{
				if (e.Button == MouseButtons.Left) // accept edit (else cancel edit)
					ApplyTextEdit();

				_editor.Visible = false;
				_grid.Refresh();
				_grid.Select();
			}

//			base.OnMouseClick(e);
		}

		/// <summary>
		/// Paints this property panel.
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

//			base.OnPaint(e);
		}
		#endregion Events (override)
	}
}
