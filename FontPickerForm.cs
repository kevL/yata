using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	sealed partial class FontPickerForm
		:
			Form
	{
		#region Fields (static)
		/* FontStyle
		Regular   0 - Normal text.
		Bold      1 - Bold text.
		Italic    2 - Italic text.
		Underline 4 - Underlined text.
		Strikeout 8 - Text with a line through the middle.
		*/
		static string EXAMPLE = "01234567890 `~!@#$%^&*()_+-=\\|[]{};:'\",<.>/?" + Environment.NewLine
							  + "the quick brown fox jumps over the lazy dog"    + Environment.NewLine
							  + "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG";

		static int _x = -1;
		static int _y = -1;
		static int _w = -1;
		static int _h = -1;
		#endregion Fields (static)


		#region Fields
		readonly List<FontFamily> _ffs   = new List<FontFamily>();
		readonly List<Font>       _fonts = new List<Font>(); // fonts will be disposed below

		readonly YataForm _f;
		readonly Font _font;
		readonly bool _load;

		bool
			_dirty, // ie. displayed font and its characteristics have changed
			_applied,
			_bypassStyleChanged;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal FontPickerForm(YataForm f)
		{
			InitializeComponent();

			_f = f;
			_font = _f.Font;
			_load = true;

			Owner = _f;

			if (_x == -1) _x = _f.Left + 20;
			if (_y == -1) _y = _f.Top  + 20;

			Left = _x;
			Top  = _y;

			if (_w != -1) Width  = _w;
			if (_h != -1) Height = _h;


			int fontStart  = -1; // for showing the start-font's characteristics
			int fontStartT = -1; // in the lists

//			var fontCollection = new System.Drawing.Text.InstalledFontCollection();
//			foreach (var family in fontCollection.Families)
//			{}

			Font font;
			FontStyle? style;
			foreach (var ff in FontFamily.Families)
			{
				if ((style = getFirstStyle(ff)) != null)
				{
					font = new Font(ff, 10, style.Value);
					if (font.Name == ff.Name)
					{
						++fontStartT;
						if (font.Name == _font.Name)
							fontStart = fontStartT;

						_ffs.Add(ff);
						list_Font.Items.Add(font);

						_fonts.Add(font); // <- is purely for Disposal.
					}
				}
			}

			if (fontStart != -1)
				list_Font.SelectedIndex = fontStart;
			else
			{
				_dirty = true;
				list_Font.SelectedIndex = 0; // you'd better have at least 1 font on your system buckwheat /lol
			}

			list_Size.Items.AddRange(new object[]
			{
				 "6", "7", "8", "9","10","11","12",
				"13","14","15","16","17","18"
			});

			if (fontStart != -1)
				tb_Size.Text = _font.SizeInPoints.ToString();
			else
			{
				_dirty = true;
				tb_Size.Text = "10";
			}

			_load = false;

			lbl_Example.Text = EXAMPLE;

			_f.DefaultFontItem_toggleenabled();
		}
		#endregion cTor


		#region Events
		/// <summary>
		/// Handles the load-event. This ought ensure that the FontPicker
		/// appears on top. If a user has a lot of fonts installed on their
		/// system the FontPicker takes a while to load ... and if so it tends
		/// to get hidden beneath main.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnLoad(object sender, EventArgs e)
		{
			TopMost = true;
			TopMost = false;
		}

		/// <summary>
		/// Handles the form-closing event.
		/// NOTE: This is not the same as Cancel/Revert - this will not revert
		/// the table-font if a different font has been Applied. Rather it
		/// closes the FontPicker and leaves things as they are.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnClosing(object sender, FormClosingEventArgs e)
		{
			_f.FontItem_uncheck();
			_f.DefaultFontItem_toggleenabled();

			_x = Left;
			_y = Top;
			_w = Width;
			_h = Height;

			if (lbl_Example.Font != null)
				lbl_Example.Font.Dispose();
		}
		#endregion Events


		#region Button handlers
		void btnOk_click(object sender, EventArgs e)
		{
			if (_dirty)
				_f.doFont((Font)lbl_Example.Font.Clone());

			lbl_Example.Font.Dispose();
			lbl_Example.Font = null;

			Close();
		}

		void btnApply_click(object sender, EventArgs e)
		{
			if (_dirty)
			{
				_dirty = false;
				_applied = true;

				_f.doFont((Font)lbl_Example.Font.Clone());
				btn_Cancel.Text = "— REVERT —";
			}
		}

		void btnCancel_click(object sender, EventArgs e)
		{
			if (_applied)
				_f.doFont(_font);

			lbl_Example.Font.Dispose();
			lbl_Example.Font = null;

			Close();
		}
		#endregion Button handlers


		#region FontFamily
		void fontList_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index != -1)
			{
				e.DrawBackground();
				e.DrawFocusRectangle();

				var font = list_Font.Items[e.Index] as Font;

				// NOTE: MS doc for DrawText() says that using a Point doesn't work on Win2000 machines.
				TextRenderer.DrawText(e.Graphics,
									  font.Name,
									  font,
									  e.Bounds,
									  Colors.Text,
									  TextFormatFlags.NoPrefix);
			}
		}

		void fontList_SelectedIndexChanged(object sender, EventArgs e)
		{
			_dirty |= !_load;

			var ff = _ffs[list_Font.SelectedIndex];

			cb_Bold  .Visible = ff.IsStyleAvailable(FontStyle.Bold);
			cb_Italic.Visible = ff.IsStyleAvailable(FontStyle.Italic);

			_bypassStyleChanged = true;

			cb_Bold  .Checked =
			cb_Italic.Checked = false;

			if (!_load)
			{
				var style = getFirstStyle(ff); // note: This has been checked in cTor for !null and !Underline and !Strikeout
				switch (style)
				{
					case FontStyle.Regular:
						break;

					case FontStyle.Bold:
						cb_Bold.Checked = true;
						break;

					case FontStyle.Italic:
						cb_Italic.Checked = true;
						break;
				}
			}
			else // loading ...
			{
				cb_Bold  .Checked = _font.Bold;
				cb_Italic.Checked = _font.Italic;
			}

			_bypassStyleChanged = false;

			SetSampleFont();
		}
		#endregion FontFamily


		#region FontStyle
		void cbBold_CheckedChanged(object sender, EventArgs e)
		{
			if (!_bypassStyleChanged)
			{
				if (   !cb_Bold  .Checked
					&& !cb_Italic.Checked
					&& !_ffs[list_Font.SelectedIndex].IsStyleAvailable(FontStyle.Regular))
				{
					cb_Bold.Checked = true;
				}
				else
					_dirty |= !_load;
	
				SetSampleFont();
			}
		}

		void cbItalic_CheckedChanged(object sender, EventArgs e)
		{
			if (!_bypassStyleChanged)
			{
				if (   !cb_Italic.Checked
					&& !cb_Bold  .Checked
					&& !_ffs[list_Font.SelectedIndex].IsStyleAvailable(FontStyle.Regular))
				{
					cb_Italic.Checked = true;
				}
				else
					_dirty |= !_load;
	
				SetSampleFont();
			}
		}
		#endregion FontStyle


		#region FontSize
		void sizeList_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index != -1)
			{
				e.DrawBackground();
				e.DrawFocusRectangle();

				TextRenderer.DrawText(e.Graphics,
									  list_Size.Items[e.Index].ToString(),
									  Font,
									  e.Bounds,
									  Colors.Text,
									  TextFormatFlags.NoPrefix);
			}
		}

		void fontSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			tb_Size.Text = list_Size.SelectedItem.ToString();
		}

		void fontSize_TextChanged(object sender, EventArgs e)
		{
			_dirty |= !_load;
			SetSampleFont();
		}
		#endregion FontSize


		#region Methods
		/// <summary>
		/// Gets the first available style in a given FontFamily.
		/// </summary>
		/// <param name="ff">a FontFamily</param>
		/// <returns>a nullable FontStyle</returns>
		FontStyle? getFirstStyle(FontFamily ff)
		{
			foreach (FontStyle style in Enum.GetValues(typeof(FontStyle)))
			{
				if (ff.IsStyleAvailable(style)
					&& style != FontStyle.Underline
					&& style != FontStyle.Strikeout)
				{
					return style;
				}
			}
			return null;
		}

		void SetSampleFont()
		{
			if (list_Font.SelectedIndex != -1)
			{
				float size;
				if (float.TryParse(tb_Size.Text, out size)
					&& size > 0)
				{
					var style = FontStyle.Regular;	// =0

					if (cb_Bold.Checked)
						style |= FontStyle.Bold;	// =1

					if (cb_Italic.Checked)
						style |= FontStyle.Italic;	// =2

					lbl_Example.Font = new Font(_ffs[list_Font.SelectedIndex].Name, // rely on GC here
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
		#endregion Methods
	}
}
