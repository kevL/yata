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

		YataForm _f;
		Font _fontCached;
		bool _load;

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

			int idFont     = -1;
			int idFontTest = -1;

			Font font;
			foreach (var family in FontFamily.Families)
			{
				FontStyle? style = null;

				foreach (FontStyle styleTest in Enum.GetValues(typeof(FontStyle)))
				{
					if (styleTest != FontStyle.Strikeout && styleTest != FontStyle.Underline
						&& family.IsStyleAvailable(styleTest))
					{
						style = styleTest;
						break;
					}
				}

				if (style != null)
				{
					if ((font = new Font(family, 10, style.Value)) != null) // TODO: 10pts is way too big for some fonts.
					{
						++idFontTest;
						if (font.Name == _fontCached.Name)
						{
							idFont = idFontTest;
						}

						_families.Add(family);
						list_Font.Items.Add(font);
					}
				}
			}

			if (idFont != -1)
				list_Font.SelectedIndex = idFont;
			else
				list_Font.SelectedIndex = 0;

			list_Size.Items.AddRange(new object[]
			{
				 "6",
				 "7",
				 "8",
				 "9",
				"10",
				"11",
				"12",
				"13",
				"14",
				"15",
				"16",
				"17",
				"18"
			});
			list_Size.SelectedIndex = 4; // 10pt

			if (idFont != -1)
				tb_Size.Text = _fontCached.SizeInPoints.ToString();


			lbl_Example.Text = EXAMPLE;
		}



		void btnOk_click(object sender, EventArgs e)
		{
			_f.Font = lbl_Example.Font;
			_f.AutosizeColsToolStripMenuItemClick(null, EventArgs.Empty);
			Close();
		}

		void btnApply_click(object sender, EventArgs e)
		{
			_f.Font = lbl_Example.Font;
			_f.AutosizeColsToolStripMenuItemClick(null, EventArgs.Empty);
		}

		void btnCancel_click(object sender, EventArgs e)
		{
			_f.Font = _fontCached;
			_f.AutosizeColsToolStripMenuItemClick(null, EventArgs.Empty);
			Close();
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
			list_Style.Items.Clear();

			var styleLoad = _fontCached.Style;
			int idStyle     = -1;
			int idStyleTest = -1;

			var family = _families[list_Font.SelectedIndex];

			foreach (FontStyle style in Enum.GetValues(typeof(FontStyle))) // determine first available style of Family ->
			{
				if (style != FontStyle.Strikeout && style != FontStyle.Underline
					&& family.IsStyleAvailable(style))
				{
					if (_load)
					{
						++idStyleTest;
						if (style == styleLoad)
						{
							_load = false; // not needed anymore.
							idStyle = idStyleTest;
						}
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
			SetExampleText();
		}


		void sizeList_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index != -1)
			{
				e.DrawBackground();
				e.DrawFocusRectangle();

				logfile.Log(list_Size.Items[e.Index].ToString());
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

					lbl_Example.Refresh();
				}
			}
		}
	}
}


/*	/// <summary>
	/// https://stackoverflow.com/questions/8017927/filtering-out-removing-non-true-type-fonts-from-font-dialog-in-c-sharp#answer-8260245
	/// </summary>
	public class FontListBox
		:
			ListBox
	{
		List<Font> _fonts = new List<Font>();

		Brush _foreBrush;


		/// <summary>
		/// cTor.
		/// </summary>
		internal FontListBox()
		{
			Font font;

			foreach (FontFamily ff in FontFamily.Families)
			{
				// determine the first available style since all fonts don't support all styles
				FontStyle? styleOk = null;
				foreach (FontStyle style in Enum.GetValues(typeof(FontStyle)))
				{
					if (ff.IsStyleAvailable(style))
					{
						styleOk = style;
						break;
					}
				}

				if (styleOk.HasValue)
				{
					if ((font = new Font(ff, 10, styleOk.Value)) != null)
					{
						_fonts.Add(font);
						Items.Add(font);
					}
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (_fonts != null)
			{
				foreach (Font font in _fonts)
				{
					font.Dispose();
				}
				_fonts = null;
			}

			if (_foreBrush != null)
			{
				_foreBrush.Dispose();
				_foreBrush = null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);

			if (e.Index > -1)
			{
				e.DrawBackground();
				e.DrawFocusRectangle();

				var font = Items[e.Index] as Font;
				e.Graphics.DrawString(font.Name,
									  font,
									  ForeBrush,
									  e.Bounds.Left,
									  e.Bounds.Top);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		Brush ForeBrush
		{
			get
			{
				if (_foreBrush == null)
					_foreBrush = new SolidBrush(base.ForeColor);
//					_foreBrush = new SolidBrush(ForeColor);

				return _foreBrush;
			}
		}
	} */

/*		/// <summary>
		/// 
		/// </summary>
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;

				if (_foreBrush != null)
					_foreBrush.Dispose();

				_foreBrush = null;
			}
		} */
