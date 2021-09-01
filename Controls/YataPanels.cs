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
		: Panel
	{
		#region Fields
		readonly YataGrid _grid;

		internal bool Grab;
		int _grabCol;
		int _grabStart;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="grid"></param>
		internal YataPanelCols(YataGrid grid)
		{
			DrawingControl.SetDoubleBuffered(this);

			_grid = grid;

			Dock      = DockStyle.Top;
			BackColor = Colors.ColheadPanel;

			Height = YataGrid.HeightColhead;

			MouseClick += _grid.click_ColheadPanel;
		}
		#endregion cTor


		#region Events (override)
		/// <summary>
		/// Overrides the <c>Resize</c> handler on this <c>YataPanelCols</c>.
		/// </summary>
		/// <param name="eventargs"></param>
		protected override void OnResize(EventArgs eventargs)
		{
			if (Gradients.ColheadPanel != null)
				Gradients.ColheadPanel.Dispose();

			Gradients.ColheadPanel = new LinearGradientBrush(new Point(0, 0),
															 new Point(0, Height),
															 Color.Lavender, Color.MediumOrchid);
		}

		/// <summary>
		/// Overrides the <c>Paint</c> handler on this <c>YataPanelCols</c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!YataGrid._init)
			{
				YataGrid.graphics = e.Graphics;
				YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				if (Settings._gradient)
				{
					var rect = new Rectangle(0,0, Width, Height);
					YataGrid.graphics.FillRectangle(Gradients.ColheadPanel, rect);
				}

				YataGrid.graphics.DrawLine(Pencils.DarkLine,
										   0,     Height,
										   Width, Height);
				YataGrid.graphics.DrawLine(Pencils.DarkLine,
										   Width, 0,
										   Width, Height);
				_grid.LabelColheads();
			}
		}


		/// <summary>
		/// Overrides the <c>MouseMove</c> handler on this <c>YataPanelCols</c>.
		/// Changes cursor to a vertical splitter near the right edge of each
		/// unfrozen colhead.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!Grab)
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
		}

		/// <summary>
		/// Overrides the <c>MouseDown</c> handler on this <c>YataPanelCols</c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			_grid._editor.Visible = false;
			int invalid = YataGrid.INVALID_GRID;

			if (_grid.Propanel != null && _grid.Propanel._editor.Visible)
			{
				_grid.Propanel._editor.Visible = false;
				invalid |= YataGrid.INVALID_PROP;
			}
			_grid.Invalidator(invalid);

			Grab = (Cursor == Cursors.VSplit);
		}

		/// <summary>
		/// Overrides the <c>MouseUp</c> handler on this <c>YataPanelCols</c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (Grab)
			{
				Grab = false;
				Cursor = Cursors.Default;

				Col col;

				switch (e.Button)
				{
					case MouseButtons.Left:
						if (e.X != _grabStart)
						{
							col = _grid.Cols[_grabCol];
							col.UserSized = true;

							int w = col.width() + e.X - _grabStart;
							if (w < YataGrid._wId) w = YataGrid._wId;

							col.width(w, true);
							_grid.InitScroll();
							_grid.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_COLS);
						}
						break;

					case MouseButtons.Right:
						if ((col = _grid.Cols[_grabCol]).UserSized)
						{
							col.UserSized = false;
							_grid.Colwidth(_grabCol);
							_grid.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_COLS);
						}
						break;
				}
			}
			_grid.Select();
		}
		#endregion Events (override)
	}


	/// <summary>
	/// A panel for the row heads.
	/// </summary>
	sealed class YataPanelRows
		: Panel
	{
		#region Fields
		readonly YataGrid _grid;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="grid"></param>
		internal YataPanelRows(YataGrid grid)
		{
			DrawingControl.SetDoubleBuffered(this);

			_grid = grid;

			Dock      = DockStyle.Left;
			BackColor = Colors.RowheadPanel;

			MouseClick += _grid.click_RowheadPanel;
		}
		#endregion cTor


		#region Events (override)
		/// <summary>
		/// Overrides the <c>Paint</c> handler on this <c>YataPanelRows</c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!YataGrid._init)
			{
				YataGrid.graphics = e.Graphics;
				YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
				YataGrid.graphics.DrawLine(Pencils.DarkLine,
										   Width, 0,
										   Width, Height);
				YataGrid.graphics.DrawLine(Pencils.DarkLine,
										   Width - 1, 0,
										   Width - 1, Height);
				_grid.LabelRowheads();
			}
		}
		#endregion Events (override)
	}


	/// <summary>
	/// A panel for the id-col and any frozen cols that follow it.
	/// </summary>
	sealed class YataPanelFrozen
		: Panel
	{
		#region Fields
		readonly YataGrid _grid;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="grid"></param>
		/// <param name="w"></param>
		internal YataPanelFrozen(YataGrid grid, int w)
		{
			DrawingControl.SetDoubleBuffered(this);

			_grid = grid;

			Dock      = DockStyle.Left;
			BackColor = Colors.FrozenPanel;

			Width = w;
		}
		#endregion cTor


		#region Events (override)
		/// <summary>
		/// Overrides the <c>Paint</c> handler on this <c>YataPanelFrozen</c>.
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
		}

		/// <summary>
		/// Overrides the <c>MouseClick</c> handler on this
		/// <c>YataPanelFrozen</c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (_grid._editor.Visible)
			{
				_grid._editor.Visible = false;
				_grid.Invalidator(YataGrid.INVALID_GRID);
			}
			_grid.Select();
		}
		#endregion Events (override)
	}
}
