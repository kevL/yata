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

		int HeightProps;

		int _widthVars;
		int _widthVals;

		int _height;

		const int _padHori = 5; // horizontal text padding


		/// <summary>
		/// cTor.
		/// </summary>
		internal PropertyPanel(YataGrid grid)
		{
			_grid = grid;

			DoubleBuffered = true;

			BackColor = Color.LightBlue;
			ForeColor = SystemColors.ControlText;

			Font = new System.Drawing.Font("Verdana", 8.0F, FontStyle.Regular, GraphicsUnit.Point, (byte)0);

			_height = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, Font) + 1;

			Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);

			int wT;
			for (int c = 0; c != _grid.ColCount; ++c)
			{
				wT = YataGraphics.MeasureWidth(_grid.Cols[c].text, Font);
				if (wT > _widthVars)
					_widthVars = wT;
			}
			_widthVars += _padHori * 2 + 1;
			_widthVals = _widthVars;

			Left   = _grid.Left   - (_grid._visVert ? _grid._scrollVert.Width : 0) + _grid.Width - _widthVals - _widthVars;
			Top    = _grid.Top;
			Width  = _widthVars + _widthVals;
			Height = _grid.Height - (_grid._visHori ? _grid._scrollHori.Height : 0);

			_scroll.Dock = DockStyle.Right;
			_scroll.ValueChanged += OnScrollValueChanged;

			Controls.Add(_scroll);
			InitScroll(true);

			_grid.Controls.Add(this);
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			var graphics = e.Graphics;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			int offset = _scroll.Value;
			int r;

			// draw lines ->
			graphics.DrawLine(Pens.Black,			// vertical left line
							  1, 0,
							  1, HeightProps - offset - 1);
			graphics.DrawLine(Pens.DarkLine,		// vertical center line
							  _widthVals, 1,
							  _widthVals, HeightProps - offset - 1);
			graphics.DrawLine(Pens.Black,			// horizontal top line
							  1,     1,
							  Width, 1);
			graphics.DrawLine(Pens.Black,			// horizontal bot line
							  1,     _height * _grid.ColCount - offset,
							  Width, _height * _grid.ColCount - offset);

			for (r = 1; r != _grid.ColCount; ++r)	// horizontal row lines
			{
				graphics.DrawLine(Pens.DarkLine,
								  0,     _height * r - offset + 1,
								  Width, _height * r - offset + 1);
			}

			// draw text ->
			var rect = new Rectangle(_padHori, 0,
									 _widthVals, _height);

			for (r = 0; r != _grid.ColCount; ++r)
			{
				rect.Y = _height * r - offset + 1;
				TextRenderer.DrawText(graphics, _grid.Cols[r].text, Font, rect, Colors.Text, YataGraphics.flags);
			}

//			base.OnPaint(e);
		}

		internal void InitScroll(bool init = false)
		{
			if (init)
			{
				HeightProps = _grid.ColCount * _height + 1;
			}
			else // resize event - see YataGrid.OnResize()
			{
				Left   = _grid.Left   - (_grid._visVert ? _grid._scrollVert.Width  : 0) + _grid.Width - _widthVals - _widthVars;
				Height = _grid.Height - (_grid._visHori ? _grid._scrollHori.Height : 0);
			}

			if (Height < HeightProps)
			{
				_scroll.Visible = true;

				int vert = HeightProps
						 + _scroll.LargeChange
						 - Height
						 - 1;

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
	}
}
