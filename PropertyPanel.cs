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

		int HeightProps; // height of the entire panel (incl/ non-displayed top & bot)

		int _widthVars;
		int _widthVals;

		static int _heightr;

		const int _padHori = 5; // horizontal text padding
		const int _padVert = 2; // vertical text padding


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
		}

		internal void setLeftHeight()
		{
			Width = _widthVars + _widthVals;

			Left   = _grid.Left   - (_grid._visVert ? _grid._scrollVert.Width  : 0) + _grid.Width - Width;
			Height = _grid.Height - (_grid._visHori ? _grid._scrollHori.Height : 0);
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
			Refresh();
		}

		internal void Scroll(MouseEventArgs e)
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


		Rectangle? getValueRectangle(Point pt)
		{
			Rectangle? rect = null;

//			if ()
			{
				rect = new Rectangle();

			}


			return rect;
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			logfile.Log("e.X= " + e.X + " e.Y= " + e.Y);

			if (e.X > _widthVals && e.Y < HeightProps - _scroll.Value)
			{
				logfile.Log(". in Val");
			}


//			base.OnMouseClick(e);
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			var graphics = e.Graphics;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			int offset = _scroll.Value;
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

			// draw text ->
			var rect = new Rectangle(_padHori, 0,
									 _widthVars, _heightr);

			int r;
			Cell cell = _grid.GetSelectedCell();
			if (cell != null)
				r = cell.y;
			else
				r = _grid.getSelectedRow();

			for (c = 0; c != _grid.ColCount; ++c)
			{
				rect.Y = _heightr * c - offset;// + 1;
				TextRenderer.DrawText(graphics, _grid.Cols[c].text, Font, rect, Colors.Text, YataGraphics.flags);

				if (r != -1)
				{
					rect.X    += _widthVars;
					rect.Width = _widthVals;
					TextRenderer.DrawText(graphics, _grid[r,c].text, Font, rect, Colors.Text, YataGraphics.flags);
					rect.X    -= _widthVars;
					rect.Width = _widthVars;
				}
			}

//			base.OnPaint(e);
		}
	}
}
