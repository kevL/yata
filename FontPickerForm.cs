using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	partial class FontPickerForm
		:
			Form
	{
		static string EXAMPLE = "01234567890 `~!@#$%^&*()_+-=\\|[]{};:'\",<.>/?" + Environment.NewLine
							  + "the quick brown fox jumps over the lazy dog"    + Environment.NewLine
							  + "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG";

		List<FontFamily> _families = new List<FontFamily>();
		List<Font>       _fonts    = new List<Font>(); // fonts will be disposed below

		YataForm _f;
		Font _fontCached;
		bool
			_load,
			_dirty; // ie. displayed font and its characteristics have changed

		/// <summary>
		/// cTor.
		/// </summary>
		internal FontPickerForm(YataForm f)
		{
			InitializeComponent();

			_f = f;
			_fontCached = _f.Font;
			_load = true;

//			Font = _f.Font;

			int fontStart  = -1; // for showing the start-font's characteristics
			int fontStartT = -1; // in the lists

			Font font;
			foreach (var family in FontFamily.Families)
			{
				FontStyle? style = null;

				foreach (FontStyle styleTest in Enum.GetValues(typeof(FontStyle)))
				{
					if (styleTest != FontStyle.Strikeout && styleTest != FontStyle.Underline
						&& family.IsStyleAvailable(styleTest))
					{
						style = styleTest; // NOTE: not all fonts have fontstyle Regular.
						break;
					}
				}

				if (style != null
					&& (font = new Font(family, 10, style.Value)) != null) // WARNING: 10pts is way too big for some fonts.
				{
					++fontStartT;
					if (font.Name == _fontCached.Name)
						fontStart = fontStartT;

					_families.Add(family);
					list_Font.Items.Add(font);

					_fonts.Add(font); // is purely for Disposal.
				}
			}

			if (fontStart != -1)
				list_Font.SelectedIndex = fontStart;
			else
			{
				_dirty = true;
				list_Font.SelectedIndex = 0;
			}

			list_Size.Items.AddRange(new object[]
			{
				 "6", "7", "8", "9","10","11","12",
				"13","14","15","16","17","18"
			});
			list_Size.SelectedIndex = 4; // 10pt

			if (fontStart != -1)
				tb_Size.Text = _fontCached.SizeInPoints.ToString();
			else
				_dirty = true;

			_load = false;

			lbl_Example.Text = EXAMPLE;
		}



		bool _applied;

		void btnOk_click(object sender, EventArgs e)
		{
			if (_dirty)
				doFont(lbl_Example.Font);

			Close();
		}

		void btnApply_click(object sender, EventArgs e)
		{
			if (_dirty)
			{
				_dirty = false;
				_applied = true;

				doFont(lbl_Example.Font);
			}
		}

		void btnCancel_click(object sender, EventArgs e)
		{
			if (_applied)
				doFont(_fontCached);

			Close();
		}

		void doFont(Font font)
		{
			DrawingControl.SuspendDrawing(_f);

			int w = _f.Width;
			int h = _f.Height;

			_f.Font = font;
			_f.AutosizeColsToolStripMenuItemClick(null, EventArgs.Empty);
			_f.Table.SetRowMetric(false);

			_f.Width  = w;
			_f.Height = h;

			DrawingControl.ResumeDrawing(_f);
		}


		void fontList_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index != -1)
			{
				e.DrawBackground();
				e.DrawFocusRectangle();

				var font = list_Font.Items[e.Index] as Font;
				TextRenderer.DrawText(e.Graphics,
									  font.Name,
									  font,
									  new Point(e.Bounds.X, e.Bounds.Y),
									  SystemColors.ControlText);
//				TextRenderer.DrawText(e.Graphics,
//									  font.Name,
//									  e.CellStyle.Font,
//									  new Point(e.CellBounds.X + x, e.CellBounds.Y + 4),
//									  e.CellStyle.ForeColor);
//				e.Graphics.DrawString(font.Name,
//									  font,
//									  e.Style.ForeBrush,
//									  e.Bounds.Left,
//									  e.Bounds.Top);
			}
		}

		void fontList_SelectedIndexChanged(object sender, EventArgs e)
		{
			_dirty |= !_load;

			list_Style.Items.Clear();

			var styleLoad = _fontCached.Style;
			int idStyle   = -1;
			int idStyleT  = -1;

			var family = _families[list_Font.SelectedIndex];

			foreach (FontStyle style in Enum.GetValues(typeof(FontStyle))) // determine first available style of Family ->
			{
				if (style != FontStyle.Strikeout && style != FontStyle.Underline
					&& family.IsStyleAvailable(style))
				{
					if (_load)
					{
						++idStyleT;
						if (style == styleLoad)
							idStyle = idStyleT;
					}
					list_Style.Items.Add(style);
				}
			}

			if (idStyle != -1)
				list_Style.SelectedIndex = idStyle;
			else
				list_Style.SelectedIndex = 0;

			SetExampleText();
		}


		void styleList_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index != -1)
			{
				e.DrawBackground();
				e.DrawFocusRectangle();

				TextRenderer.DrawText(e.Graphics,
									  list_Style.Items[e.Index].ToString(),
									  Font,
									  new Point(e.Bounds.X, e.Bounds.Y),
									  SystemColors.ControlText);
			}
		}

		void fontStyle_SelectedIndexChanged(object sender, EventArgs e)
		{
			_dirty |= !_load;
			SetExampleText();
		}


		void sizeList_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index != -1)
			{
				e.DrawBackground();
				e.DrawFocusRectangle();

				TextRenderer.DrawText(e.Graphics,
									  list_Size.Items[e.Index].ToString(),
									  Font,
									  new Point(e.Bounds.X, e.Bounds.Y),
									  SystemColors.ControlText);
			}
		}

		void fontSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			tb_Size.Text = list_Size.SelectedItem.ToString();
		}

		void fontSize_TextChanged(object sender, EventArgs e)
		{
			_dirty |= !_load;
			SetExampleText();
		}


		void SetExampleText()
		{
			if (list_Font.SelectedIndex != -1)
			{
				float size;
				if (float.TryParse(tb_Size.Text, out size)
					&& size > 0)
				{
					var style = (FontStyle)list_Style.SelectedItem;
					lbl_Example.Font = new Font(_families[list_Font.SelectedIndex].Name,
												size, style);
				}
			}
		}


		/// <summary>
		/// Disposes the fonts in the fontList.
		/// not sure if this is required but here it is ...
		/// </summary>
		void DisposeFonts()
		{
			foreach (var font in _fonts)
				font.Dispose();
		}
	}
}
