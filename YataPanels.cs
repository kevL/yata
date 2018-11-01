using System;
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

		internal YataPanelCols(YataGrid grid, int h)
		{
			DoubleBuffered = true;

			_grid = grid;

			Dock      = DockStyle.Top;
			BackColor = Colors.ColheadPanel;

			Height = h;
		}

		/// <summary>
		/// Handles the paint event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
//			DrawingControl.SuspendDrawing(this);

			e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			e.Graphics.DrawLine(Pens.DarkLine, 0, Height, Width, Height);

			_grid.LabelColheads(e.Graphics);

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

		internal YataPanelRows(YataGrid grid, int w)
		{
			DoubleBuffered = true;

			_grid = grid;

			Dock      = DockStyle.Left;
			BackColor = Colors.RowheadPanel;

			Width = w;
		}

		/// <summary>
		/// Handles the paint event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
//			DrawingControl.SuspendDrawing(this);

			e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			e.Graphics.DrawLine(Pens.DarkLine, Width, 0, Width, Height);

			_grid.LabelRowheads(e.Graphics);

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
			DoubleBuffered = true;

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
//			DrawingControl.SuspendDrawing(this);

			e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			_grid.LabelFrozen(e.Graphics);

//			DrawingControl.ResumeDrawing(this);
		}
	}
}
