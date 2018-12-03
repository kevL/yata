using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	sealed class PropertyPanel
		:
			Panel
	{
		/// <summary>
		/// cTor.
		/// </summary>
		internal PropertyPanel(YataGrid grid)
		{
			DoubleBuffered = true;

			BackColor = SystemColors.ControlDark;
			ForeColor = SystemColors.ControlText;

			BorderStyle = BorderStyle.FixedSingle;
			Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);

			Left   = grid.Left + 200;
			Top    = grid.Top;
			Width  = grid.Width - 200 - (grid._visVert ? grid._scrollVert.Width  : 0);
			Height = grid.Height      - (grid._visHori ? grid._scrollHori.Height : 0);

			grid.Controls.Add(this);
		}
	}
}
