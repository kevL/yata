using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A progress bar.
	/// </summary>
	class ProgBar
		:
			Form
	{
		readonly Graphics _graphics;
		Rectangle _rect;

		public int ValCur
		{ get; set; }

		public int ValTop
		{ get; set; }


		/// <summary>
		/// cTor.
		/// </summary>
		internal ProgBar(YataForm f)
		{
			Size            = new Size(300, 30);
			ControlBox      = false;
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			MaximizeBox     = false;
			MinimizeBox     = false;
			ShowIcon        = false;
			ShowInTaskbar   = false;
			SizeGripStyle   = SizeGripStyle.Hide;
			StartPosition   = FormStartPosition.Manual;
			TopMost         = true;

			Left = f.Left + (f.Width  - Width)  / 2;
			Top  = f.Top  + (f.Height - Height) / 2 - 15;

			_graphics = CreateGraphics();
			_rect = new Rectangle(0,0, 0, Height);
		}


		internal void Step()
		{
			if (++ValCur != ValTop)
			{
				_rect.Width = Width * ValCur / ValTop;
				_graphics.FillRectangle(Brushes.CellSel, _rect);

//				Refresh();
			}
			else
				Hide();
		}
	}
}
