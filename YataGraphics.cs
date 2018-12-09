﻿using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	static class YataGraphics
	{
		internal const string HEIGHT_TEST = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
		internal const string WIDTH_CORDS = "id= 99999 col= 99";

		/// <summary>
		/// An IDeviceContext used for measuring texts.
		/// @note Is defined in the YataForm cTor.
		/// </summary>
		internal static Graphics graphics;

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

	static class Pens
	{
		internal static readonly Pen DarkLine          = new Pen(SystemColors.ControlDark);
		internal static readonly Pen Black             = new Pen(Color.Black);
		internal static readonly Pen LabelSortedBorder = new Pen(Color.Firebrick);
	}

	static class Brushes
	{
		internal static readonly Brush Alice       = new SolidBrush(Color.AliceBlue);
		internal static readonly Brush Blanche     = new SolidBrush(Color.BlanchedAlmond);
		internal static readonly Brush Created     = new SolidBrush(SystemColors.ControlLight);

		internal static readonly Brush CellSel     = new SolidBrush(Color.PaleGreen);
		internal static readonly Brush RowFlag     = new SolidBrush(Color.Honeydew);
		internal static readonly Brush Editor      = new SolidBrush(Colors.Editor);

		internal static readonly Brush LoadChanged = new SolidBrush(Color.Pink);

		internal static readonly Brush Control     = new SolidBrush(SystemColors.Control);
	}

	static class Colors
	{
		internal static readonly Color ColheadPanel = Color.Thistle;
		internal static readonly Color RowheadPanel = Color.Azure;

		internal static readonly Color FrozenHead   = Color.Moccasin;
		internal static readonly Color FrozenPanel  = Color.OldLace;

//		internal static readonly Color LabelSorted  = Color.Violet;

		internal static readonly Color Editor       = Color.Crimson;

		internal static readonly Color Text         = SystemColors.ControlText;
		internal static readonly Color TextColSized = Color.Olive;
		internal static readonly Color TextReadonly = Color.Firebrick;
	}
}
