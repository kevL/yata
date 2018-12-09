using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A panel for the col heads.
	/// </summary>
	sealed class YataPanelCols
		:
			Panel
	{
		readonly YataGrid _grid;

		internal YataPanelCols(YataGrid grid)
		{
			DrawingControl.SetDoubleBuffered(this);

			_grid = grid;

			Dock      = DockStyle.Top;
			BackColor = Colors.ColheadPanel;

			Height = YataGrid.HeightColhead;

			MouseClick += _grid.click_ColPanel;
		}

		/// <summary>
		/// Handles the paint event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!YataGrid._init)
			{
				YataGrid.graphics = e.Graphics;
				YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				var FillBrush = new LinearGradientBrush(new Point(0,0), new Point(0, Height),
														Color.Lavender, Color.MediumOrchid); //.DarkOrchid
				var FillRect  = new Rectangle(0, 0, Width, Height);
				YataGrid.graphics.FillRectangle(FillBrush, FillRect);

				YataGrid.graphics.DrawLine(Pens.DarkLine,
										   0,     Height,
										   Width, Height);
				YataGrid.graphics.DrawLine(Pens.DarkLine,
										   Width, 0,
										   Width, Height);
				_grid.LabelColheads();
			}

//			base.OnPaint(e);
		}


		internal bool _grab;
		internal int  _grabCol;
		internal int  _grabStart;

		/// <summary>
		/// Changes cursor to a vertical splitter near the right edge of each
		/// unfrozen colhead.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!_grab)
			{
				int labels = YataGrid.WidthRowhead; // TODO: This value should be cached instead.
				for (int f = 0; f != _grid.FrozenCount; ++f)
					labels += _grid.Cols[f].width();

				if (e.X > labels)
				{
					int x = YataGrid.WidthRowhead - _grid.offsetHori;
					for (int c = 0; c != _grid.ColCount; ++c)
					{
						x += _grid.Cols[c].width();
						if (e.X > x - 5 && e.X < x) // && c >= _grid.FrozenCount
						{
							Cursor = Cursors.VSplit;
							_grabCol = c;
							_grabStart = e.X;
							return;
						}
					}
				}
				Cursor = Cursors.Default;
			}

//			base.OnMouseMove(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if ((_grab = (Cursor == Cursors.VSplit))
				&& e.Button == MouseButtons.Left)
			{
				_grid._editor.Visible = false;
				_grid.Refresh();
			}

//			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (_grab)
			{
				if (e.Button == MouseButtons.Left)
				{
					_grab = false;
					Cursor = Cursors.Default;

					var col = _grid.Cols[_grabCol];
					col.UserSized = true;

					int w = col.width() + e.X - _grabStart;
					if (w < 17) w = 17;

					col.width(w, true);
					_grid.InitScrollers();
					_grid.Refresh();
				}
				else if (e.Button == MouseButtons.Right)
				{
					_grab = false;
					Cursor = Cursors.Default;

					var col = _grid.Cols[_grabCol];
					if (col.UserSized)
					{
						col.UserSized = false;
						_grid.colRewidth(_grabCol);
					}
				}
			}

//			base.OnMouseUp(e);
		}
	}

	/// <summary>
	/// A panel for the row heads.
	/// </summary>
	sealed class YataPanelRows
		:
			Panel
	{
		readonly YataGrid _grid;

		internal YataPanelRows(YataGrid grid)
		{
			DrawingControl.SetDoubleBuffered(this);

			_grid = grid;

			Dock      = DockStyle.Left;
			BackColor = Colors.RowheadPanel;

			MouseClick += _grid.click_RowPanel;
		}

		/// <summary>
		/// Handles the paint event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!YataGrid._init)
			{
				YataGrid.graphics = e.Graphics;
				YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
				YataGrid.graphics.DrawLine(Pens.DarkLine,
										   Width, 0,
										   Width, Height);
				YataGrid.graphics.DrawLine(Pens.DarkLine,
										   Width - 1, 0,
										   Width - 1, Height);
				_grid.LabelRowheads();
			}

//			base.OnPaint(e);
		}
	}

	/// <summary>
	/// A panel for the id-col and any frozen cols that follow it.
	/// </summary>
	sealed class YataPanelFrozen
		:
			Panel
	{
		readonly YataGrid _grid;

		internal YataPanelFrozen(YataGrid grid, int w)
		{
			DrawingControl.SetDoubleBuffered(this);

			_grid = grid;

			Dock      = DockStyle.Left;
			BackColor = Colors.FrozenPanel;

			Width = w;
		}

		/// <summary>
		/// Handles the paint event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!YataGrid._init)
			{
				YataGrid.graphics = e.Graphics;
				YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
				_grid.PaintFrozenPanel();
			}

//			base.OnPaint(e);
		}

		/// <summary>
		/// Handles a mouseclick on the frozen-panel.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (_grid._editor.Visible)
				_grid._editor.Focus();
			else
				_grid.Select();

//			base.OnMouseClick(e);
		}
	}
}
