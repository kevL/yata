using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace yata
{
	sealed class PropertyPanel
		:
			Control
	{
		readonly YataGrid _grid;

		internal readonly ScrollBar _scroll = new VScrollBar();

		readonly int HeightProps; // height of the entire panel (incl/ non-displayed top & bot)

		readonly int _widthVars;	// left col
		int _widthVals;				// right col

		static int _heightr; // height of a row is the same for all propanels

		const int _padHori = 5; // horizontal text padding
		const int _padVert = 2; // vertical text padding

		readonly TextBox _editor = new TextBox();
		Rectangle _editRect;


		/// <summary>
		/// cTor.
		/// </summary>
		internal PropertyPanel(YataGrid grid)
		{
			_grid = grid;

			DoubleBuffered = true;

			BackColor = Color.LightBlue;
			ForeColor = SystemColors.ControlText;

			Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);

			if (Settings._font3 != null)
			{
//				Font.Dispose(); // NOTE: Don't dispose that; it will be needed when another PropertyPanel instantiates.
				Font = Settings._font3;
			}
			else
				Font = new System.Drawing.Font("Verdana", 7.5F, FontStyle.Regular, GraphicsUnit.Point, (byte)0);

			if (_heightr == 0)
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

			calcValueWidth();
			setLeftHeight();

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


		internal void calcValueWidth()
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

		internal void setLeftHeight()
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

		void OnScrollValueChanged(object sender, EventArgs e)
		{
			_editor.Visible = false;
			Refresh();
		}

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


		int _r;
		int _c; // -> the row in the panel, the col in the table.

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (!_grid.Readonly)
			{
				if (!_editor.Visible)
				{
					if (e.X > _widthVars)
					{
						int y = e.Y + _scroll.Value;
						if (e.Y < HeightProps - _scroll.Value)
						{
							Cell cell = _grid.GetSelectedCell();
							if (cell != null)
								_r = cell.y;
							else
								_r = _grid.getSelectedRow();
	
							if (_r != -1)
							{
								Focus(); // snap
	
								_c = y / _heightr;
	
								_editRect.X      = _widthVars;
								_editRect.Y      = _c * _heightr + 1;
								_editRect.Width  = _widthVals;
								_editRect.Height = _heightr - 1;
	
								_editor.Left = _editRect.X + 5;
								_editor.Top  = _editRect.Y + 1 - _scroll.Value;
								_editor.Text = _grid[_r,_c].text;
	
								_editor.Visible = true;
								_editor.Focus();
	
								_editor.SelectionStart = 0; // because .NET
								if (_editor.Text == Constants.Stars)
								{
									_editor.SelectionLength = _editor.Text.Length;
								}
								else
									_editor.SelectionStart = _editor.Text.Length;
	
								Refresh();
							}
						}
					}
				}
				else if (e.Button == MouseButtons.Left) // accept edit
				{
					ApplyTextEdit();

					_editor.Visible = false;
					_grid.Select();
					_grid.Refresh();
				}
				else if (e.Button == MouseButtons.Right) // cancel edit
				{
					_editor.Visible = false;
					_grid.Select();
					_grid.Refresh();
				}
			}

//			base.OnMouseClick(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			if (!_editor.ContainsFocus)
			{
				_editor.Visible = false;
				Refresh();
			}

//			base.OnLostFocus(e);
		}

		void lostfocus_Editor(object sender, EventArgs e)
		{
			_editor.Visible = false;
			Refresh();
		}

		/// <summary>
		/// Handles keydown events in the cell-editor.
		/// @note Works around dweeby .NET behavior if Alt is pressed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void keydown_Editor(object sender, KeyEventArgs e)
		{
			if (e.Alt)
			{
				_editor.Visible = false;
				Refresh();
			}
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
			if ((ModifierKeys & Keys.Control) == Keys.Control)
			{
				if (_editor.Visible)
					_editor.Focus(); // ie. don't leave editor.
			}
		}

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
						_grid.Select();
						_grid.Refresh();
						return true;
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		/// <summary>
		/// Sets an edited cell's text and recalculates col-width.
		/// Cf YataGrid.ApplyTextEdit().
		/// </summary>
		void ApplyTextEdit()
		{
			if (_editor.Text != _grid[_r,_c].text)
			{
				_grid.Changed = true;

				_grid[_r,_c].loadchanged = false;

				if (YataGrid.CheckTextEdit(_editor))
					MessageBox.Show("The text that was submitted has been altered.",
									"burp",
									MessageBoxButtons.OK,
									MessageBoxIcon.Exclamation,
									MessageBoxDefaultButton.Button1);

				_grid[_r,_c].text = _editor.Text;

				_grid.colRewidth(_c, _r);
				_grid.UpdateFrozenControls(_c);
			}
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			var graphics = e.Graphics;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			int offset = _scroll.Value;

			Rectangle rect;

			// fill selected val-rect ->
			if (_editor.Visible)
			{
				rect = new Rectangle();
				rect = _editRect;
				rect.Y -= offset;

				graphics.FillRectangle(Brushes.Editor, rect);
			}


			int c;

			// draw lines ->
			graphics.DrawLine(Pens.Black,			// vertical left line
							  1, 0,
							  1, HeightProps - offset);
			graphics.DrawLine(Pens.DarkLine,		// vertical center line
							  _widthVars, 1,
							  _widthVars, HeightProps - offset - 1);
			graphics.DrawLine(Pens.Black,			// horizontal top line
							  1,     1,
							  Width, 1);
			graphics.DrawLine(Pens.Black,			// horizontal bot line
							  1,     _heightr * _grid.ColCount - offset,
							  Width, _heightr * _grid.ColCount - offset);

			for (c = 1; c != _grid.ColCount; ++c)	// horizontal row lines
			{
				graphics.DrawLine(Pens.DarkLine,
								  0,     _heightr * c - offset + 1,
								  Width, _heightr * c - offset + 1);
			}


			rect = new Rectangle(_padHori, 0,
								 _widthVars, _heightr);

			// draw var-texts ->
			for (c = 0; c != _grid.ColCount; ++c)
			{
				rect.Y = _heightr * c - offset;
				TextRenderer.DrawText(graphics, _grid.Cols[c].text, Font, rect, Colors.Text, YataGraphics.flags);
			}

			// draw val-texts ->
			int r;
			Cell cell = _grid.GetSelectedCell();
			if (cell != null)
				r = cell.y;
			else
				r = _grid.getSelectedRow();

			if (r != -1)
			{
				rect.X    += _widthVars;
				rect.Width = _widthVals;

				for (c = 0; c != _grid.ColCount; ++c)
				{
					rect.Y = _heightr * c - offset;
					TextRenderer.DrawText(graphics, _grid[r,c].text, Font, rect, Colors.Text, YataGraphics.flags);
				}
			}
//			base.OnPaint(e);
		}
	}
}
