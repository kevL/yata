using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace yata
{
	static class YataGraphics
	{
		internal const string HEIGHT_TEST = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()-_=+[]{}\\|;:'\",.<>/?";
		internal const string WIDTH_CORDS = " id= 99999  col= 99";

		/// <summary>
		/// An <c>IDeviceContext</c> used for measuring texts.
		/// </summary>
		/// <remarks>Is defined in the <c><see cref="Yata()"/></c>
		/// <c>cTor</c>.</remarks>
		internal static Graphics graphics;

		/// <summary>
		/// The height of Yata's default font.
		/// </summary>
		internal static int hFontDefault;

		/// <summary>
		/// Flags used when measuring texts.
		/// </summary>
		internal const TextFormatFlags flags = TextFormatFlags.NoPrefix
											 | TextFormatFlags.NoPadding
											 | TextFormatFlags.Left
											 | TextFormatFlags.VerticalCenter
											 | TextFormatFlags.SingleLine; // TextFormatFlags.NoClipping

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


	static class Brushers
	{
		// table
		internal static readonly Brush Created     = SystemBrushes.ControlLight;

		internal static readonly Brush Selected    = Brushes.PaleGreen;
		internal static readonly Brush SubSelected = Brushes.Honeydew;
	}

	static class Colors
	{
		internal static readonly Color Text              = SystemColors.ControlText;
		internal static readonly Color TextReadonly      = Color.Firebrick;

		internal static readonly Color TextboxBackground = Color.WhiteSmoke;
		internal static readonly Color TextboxSelected   = Color.FloralWhite;
		internal static readonly Color TextboxReadonly   = Color.MistyRose;

		internal static readonly Color TalkfileLoaded    = Color.Khaki;
		internal static readonly Color TalkfileLoaded_f  = Color.IndianRed;
	}

	static class Gradients
	{
		internal static LinearGradientBrush ColheadPanel, FrozenLabel, Disordered;

		internal static readonly LinearGradientBrush ppBrush = new LinearGradientBrush(new Point(0, 0),
																					   new Point(0, PropanelButton.HEIGHT),
																					   Color.White, Color.CornflowerBlue);
	}
}
