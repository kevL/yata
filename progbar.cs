using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	// TODO: Do I have to dispose this.

	/// <summary>
	/// A progress bar.
	/// </summary>
	class ProgBar
		:
			Form
	{
		int _valCur; // inits to 0

		public int ValTop
		{ get; set; }

		/// <summary>
		/// cTor.
		/// </summary>
		internal ProgBar(YataForm f)
		{
			Size            = new Size(300, 40);
			ControlBox      = false;
			FormBorderStyle = FormBorderStyle.SizableToolWindow;
			MaximizeBox     = false;
			MinimizeBox     = false;
			ShowIcon        = false;
			ShowInTaskbar   = false;
			SizeGripStyle   = SizeGripStyle.Hide;
			StartPosition   = FormStartPosition.Manual;
			TopMost         = true;

			Left = f.Left + (f.Width  - Width)  / 2;
			Top  = f.Top  + (f.Height - Height) / 2 - 15;
		}


		const int margin = 1;

		internal void Step()
		{
			if (++_valCur != ValTop)
			{
				using (var graphics = CreateGraphics())
				{
					var rect = new Rectangle(ClientRectangle.X + margin,
											 ClientRectangle.Y + margin,
											 ClientRectangle.Width * _valCur / ValTop - margin * 2,
											 ClientRectangle.Height - margin * 2);

					ProgressBarRenderer.DrawHorizontalChunks(graphics, rect);
				}
			}
			else
				Close();
		}
	}
}
