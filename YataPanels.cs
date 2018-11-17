﻿using System;
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
//			DrawingControl.SuspendDrawing(this);

			YataGrid.graphics = e.Graphics;
			YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			YataGrid.graphics.DrawLine(Pens.DarkLine, 0, Height, Width, Height);
			_grid.LabelColheads();

//			DrawingControl.ResumeDrawing(this);
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
//			DrawingControl.SuspendDrawing(this);

			YataGrid.graphics = e.Graphics;
			YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			YataGrid.graphics.DrawLine(Pens.DarkLine, Width,     0, Width,     Height);
			YataGrid.graphics.DrawLine(Pens.DarkLine, Width - 1, 0, Width - 1, Height);
			_grid.LabelRowheads();

//			DrawingControl.ResumeDrawing(this);
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
			MouseClick += (sender, e) => grid.Select();

			Width = w;
		}

		/// <summary>
		/// Handles the paint event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
//			DrawingControl.SuspendDrawing(this);

			YataGrid.graphics = e.Graphics;
			YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			_grid.PaintFrozenPanel();

//			DrawingControl.ResumeDrawing(this);
		}
	}
}
