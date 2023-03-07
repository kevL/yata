using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Used by ToolStrips/StatusStrips to get rid of white borders and draw a
	/// 3d-ish border.
	/// </summary>
	public class StripRenderer
		: ToolStripProfessionalRenderer
	{
		/// <summary>
		/// Overrides the standard toolstrip background renderer.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks><c>Yata.statbar_Icon.BackColor</c> needs to be consistent
		/// with the color here - or <c>Color.Transparent</c>.</remarks>
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			e.Graphics.FillRectangle(Settings._statcolor, e.AffectedBounds);
		}

		/// <summary>
		/// Overrides the standard toolstrip border renderer.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if (Yata.Table == null)
				e.Graphics.DrawLine(Pens.Black, 0,0, e.ToolStrip.Width, 0);

			e.Graphics.DrawLine(Pens.Black, 0,0, 0, e.ToolStrip.Height);
		}
	}
}
