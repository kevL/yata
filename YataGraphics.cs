using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	static class YataGraphics
	{
		/// <summary>
		/// An IDeviceContext used for measuring texts.
		/// @note Is defined in the YataForm cTor.
		/// </summary>
		internal static Graphics graphics;

		/// <summary>
		/// Flags used when measuring texts.
		/// </summary>
		internal const TextFormatFlags flags = TextFormatFlags.NoClipping
											 | TextFormatFlags.NoPrefix
											 | TextFormatFlags.NoPadding
											 | TextFormatFlags.Left
											 | TextFormatFlags.VerticalCenter
											 | TextFormatFlags.SingleLine;

		/// <summary>
		/// Max.
		/// </summary>
		internal static Size size = new Size(int.MaxValue, int.MaxValue);


		/// <summary>
		/// Measures the size of text in pixels using GDI graphics.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <returns></returns>
		internal static Size MeasureSize(string text, Font font)
		{
			return TextRenderer.MeasureText(graphics,
											text,
											font,
											YataGraphics.size,
											YataGraphics.flags);
		}

		/// <summary>
		/// Measures the width of text in pixels using GDI graphics.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <returns></returns>
		internal static int MeasureWidth(string text, Font font)
		{
			return TextRenderer.MeasureText(graphics,
											text,
											font,
											YataGraphics.size,
											YataGraphics.flags).Width;
		}

		/// <summary>
		/// Measures the height of text in pixels using GDI graphics.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <returns></returns>
		internal static int MeasureHeight(string text, Font font)
		{
			return TextRenderer.MeasureText(graphics,
											text,
											font,
											YataGraphics.size,
											YataGraphics.flags).Height;
		}
	}

	static class Pens
	{
		internal static readonly Pen DarkLine = new Pen(SystemColors.ControlDark);
	}

	static class Brushes
	{
		internal static readonly Brush Alice   = new SolidBrush(Color.AliceBlue);
		internal static readonly Brush Blanche = new SolidBrush(Color.BlanchedAlmond);
		internal static readonly Brush Created = new SolidBrush(SystemColors.ControlLight);

		internal static readonly Brush CellSel = new SolidBrush(Color.PaleGreen);
		internal static readonly Brush Editor  = new SolidBrush(Colors.Editor);
	}

	static class Colors
	{
		internal static readonly Color ColheadPanel = Color.Thistle;
		internal static readonly Color RowheadPanel = Color.Azure;

		internal static readonly Color FrozenHead   = Color.Moccasin;
		internal static readonly Color FrozenPanel  = Color.OldLace;

		internal static readonly Color Editor       = Color.Crimson;

		internal static readonly Color Text         = SystemColors.ControlText;
	}
}
