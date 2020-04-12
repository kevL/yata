using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	sealed partial class FontF
		:
			Form
	{
		#region Fields (static)
		static string LAZYDOG = "01234567890 `~!@#$%^&*()_+-=\\|[]{};:'\",<.>/?" + Environment.NewLine
							  + "the quick brown fox jumps over the lazy dog"    + Environment.NewLine
							  + "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG";

		static string REGULAR_ABSENT = " does not support the Regular style."    + Environment.NewLine + Environment.NewLine
									 + "Choose a different style ...";

		static int _x = -1;
		static int _y = -1;
		static int _w = -1;
		static int _h = -1;

		const FontStyle FontStyleInvalid = (FontStyle)16; // .net doesn't define Invalid.
		#endregion Fields (static)


		#region Fields
		readonly List<FontFamily> _ffs   = new List<FontFamily>();
		readonly List<Font>       _fonts = new List<Font>(); // all fonts used to render the listbox will be disposed

		readonly YataForm _f;
		bool _init;

		int  _id; // tracks the current index of the fontlist because .net will treat it as changed when it didn't.
		bool _regular;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor. Instantiates this dialog.
		/// </summary>
		internal FontF(YataForm f)
		{
			InitializeComponent();

			_f = f;

			_init = true;

			if (Settings._fontf != null)
			{
				tb_FontString.Font.Dispose();
				tb_FontString.Font = Settings._fontfdialog;
			}

			tb_FontSize  .BackColor = Color.White;
			tb_FontString.BackColor = Colors.TextboxBackground;

			if (_x == -1) _x = _f.Left + 20;
			if (_y == -1) _y = _f.Top  + 20;

			Left = _x;
			Top  = _y;

			if (_w != -1) Width  = _w;
			if (_h != -1) Height = _h;


			// Safely ensure that Yata's current font is good to go
			// else set defaults ->

			int fontList_init = -1; // for showing the initial font's characteristics in the list ->
			int fontList_test = -1;

//			var fontCollection = new System.Drawing.Text.InstalledFontCollection();
//			foreach (var family in fontCollection.Families)
//			{}

			Font font;
			foreach (var ff in FontFamily.Families)
			{
				FontStyle style = getStyle(ff);
				if (style != FontStyleInvalid)
				{
					font = new Font(ff, 10, style);

					if (fontList_init == -1) // look for Yata's current font ->
					{
						++fontList_test;
						if (font.Name == _f.Font.Name)
							fontList_init = fontList_test;
					}

					_ffs.Add(ff);
					list_Font.Items.Add(font);

					_fonts.Add(font); // '_fonts' is purely storage for Disposal of the fonts used to render the fontlist.
				}
			}

			float size_init;
			FontStyle style_init;

			if (fontList_init != -1)
			{
				list_Font.SelectedIndex = fontList_init;
				size_init = _f.Font.SizeInPoints;
				tb_FontSize.Text = size_init.ToString();
				style_init = _f.Font.Style;

				cb_Bold.Checked = (style_init & FontStyle.Bold)      != 0;
				cb_Ital.Checked = (style_init & FontStyle.Italic)    != 0;
				cb_Undr.Checked = (style_init & FontStyle.Underline) != 0;
				cb_Strk.Checked = (style_init & FontStyle.Strikeout) != 0;
			}
			else
			{
				bu_Apply.Enabled = true;

				list_Font.SelectedIndex = 0; // you'd better have at least 1 font on your system buckwheat /lol
				size_init = 10F;
				tb_FontSize.Text = "10";
				style_init = getStyle(_ffs[list_Font.SelectedIndex]);
			}

			_id = list_Font.SelectedIndex;
			_regular = _ffs[_id].IsStyleAvailable(FontStyle.Regular);

			lbl_Lazydog.Font = new Font(_ffs[_id].Name, size_init, style_init);
			lbl_Lazydog.Text = LAZYDOG;


			tb_FontSize.MouseWheel += size_mousewheel;


			_init = false;
		}

/*		void LogValidStyles()
		{
			foreach (var ff in FontFamily.Families)
			{
				bool found = false;

				if (ff.IsStyleAvailable(FontStyle.Regular))
				{
					found = true;
					logfile.Log(ff.Name + " - regular");
				}
				if (ff.IsStyleAvailable(FontStyle.Bold))
				{
					found = true;
					logfile.Log(ff.Name + " - bold");
				}
				if (ff.IsStyleAvailable(FontStyle.Italic))
				{
					found = true;
					logfile.Log(ff.Name + " - italic");
				}
				if (ff.IsStyleAvailable(FontStyle.Underline))
				{
					found = true;
					logfile.Log(ff.Name + " - underline");
				}
				if (ff.IsStyleAvailable(FontStyle.Strikeout))
				{
					found = true;
					logfile.Log(ff.Name + " - strikeout");
				}

				if (!found)
					logfile.Log(ff.Name + " - STYLE INVALID");

				logfile.Log("");
			}
		} */
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
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnClosing(object sender, FormClosingEventArgs e)
		{
			_x = Left;
			_y = Top;
			_w = Width;
			_h = Height;

			_f.FontF_closing();

			lbl_Lazydog.Font.Dispose();
		}
		#endregion Events


		#region button handlers
		/// <summary>
		/// Applies the font that currently displays the text but does not close
		/// this dialog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Apply(object sender, EventArgs e)
		{
			bu_Apply.Enabled = false;
			_f.doFont((Font)lbl_Lazydog.Font.Clone());
		}

		/// <summary>
		/// Closes this dialog. See OnClosing().
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Close(object sender, EventArgs e)
		{
			Close();
		}
		#endregion button handlers


		#region font list/size/style handlers
		/// <summary>
		/// Draws the font labels in the font list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Fired by any control that causes the text to change.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fontchanged(object sender, EventArgs e)
		{
			if (!_init)
			{
				if (sender == list_Font)
				{
					// Lovely. Selected index changes when selected index doesn't change.
					if (list_Font.SelectedIndex != _id)
					{
						FontFamily ff = _ffs[(_id = list_Font.SelectedIndex)];

						cb_Bold.Enabled = ff.IsStyleAvailable(FontStyle.Bold);
						cb_Ital.Enabled = ff.IsStyleAvailable(FontStyle.Italic);
						cb_Undr.Enabled = ff.IsStyleAvailable(FontStyle.Underline);
						cb_Strk.Enabled = ff.IsStyleAvailable(FontStyle.Strikeout);

						_init = true; // changes to the checkboxes shall bypass this funct.
						if (!cb_Bold.Enabled) cb_Bold.Checked = false;
						if (!cb_Ital.Enabled) cb_Ital.Checked = false;
						if (!cb_Undr.Enabled) cb_Undr.Checked = false;
						if (!cb_Strk.Enabled) cb_Strk.Checked = false;
						_init = false;

						_regular = ff.IsStyleAvailable(FontStyle.Regular);

						CreateTextFont();
					}
				}
				else
					CreateTextFont();
			}
		}

		/// <summary>
		/// Increases or decreases the pointsize with the mousewheel when the
		/// size-box has focus.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void size_mousewheel(object sender, MouseEventArgs e)
		{
			float size;
			if (float.TryParse(tb_FontSize.Text, out size))
			{
				if (e.Delta > 0 && size >= 1F)
				{
					tb_FontSize.Text = (size + 0.25F).ToString();
				}
				else if (e.Delta < 0 && size >= 1.25F)
				{
					tb_FontSize.Text = (size - 0.25F).ToString();
				}
			}
		}

		/// <summary>
		/// Focuses the size-box when the size-label is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_pointlabel(object sender, EventArgs e)
		{
			tb_FontSize.Focus();
		}
		#endregion font list/size/style handlers


		#region Methods
		/// <summary>
		/// Gets the first available style in a given FontFamily.
		/// NOTE: ALL STYLES ARE ALWAYS AVAILABLE. Thanks .net ! ( not )
		/// </summary>
		/// <param name="ff">a FontFamily</param>
		/// <returns>the first FontStyle found else FontStyleInvalid</returns>
		FontStyle getStyle(FontFamily ff)
		{
			foreach (FontStyle style in Enum.GetValues(typeof(FontStyle)))
			{
				if (ff.IsStyleAvailable(style))
					return style;
			}
			return FontStyleInvalid;
		}

		/// <summary>
		/// Prints sample text in the fontlist's currently selected font.
		/// </summary>
		void CreateTextFont()
		{
			if (_id != -1) // safety.
			{
				float size;
				if (float.TryParse(tb_FontSize.Text, out size)
					&& size >= 1F)
				{
					var style = FontStyle.Regular;						// =0
					if (cb_Bold.Checked) style |= FontStyle.Bold;		// =1
					if (cb_Ital.Checked) style |= FontStyle.Italic;		// =2
					if (cb_Undr.Checked) style |= FontStyle.Underline;	// =4
					if (cb_Strk.Checked) style |= FontStyle.Strikeout;	// =8

					lbl_Lazydog.Font.Dispose();

					if (style == FontStyle.Regular && !_regular)
					{
						lbl_Lazydog.ForeColor = Color.Crimson;
						lbl_Lazydog.Font = new Font("Verdana", 8F, FontStyle.Bold);
						lbl_Lazydog.Text = _ffs[_id].Name + REGULAR_ABSENT;

						tb_FontString.Text = String.Empty;

						bu_Apply.Enabled = false;
						return;
					}

					lbl_Lazydog.ForeColor = SystemColors.ControlText;
					lbl_Lazydog.Font = new Font(_ffs[_id].Name, size, style);
					lbl_Lazydog.Text = LAZYDOG;

					printstring();
				}
			}
			bu_Apply.Enabled = !copulate(lbl_Lazydog.Font, _f.Font);
		}

		/// <summary>
		/// Compares two fonts for a reasonable identity.
		/// </summary>
		/// <param name="font1"></param>
		/// <param name="font2"></param>
		/// <returns></returns>
		static bool copulate(Font font1, Font font2)
		{
			return font1.Name  == font2.Name
				&& font1.Style == font2.Style
				&& Math.Abs(font2.SizeInPoints - font1.SizeInPoints) <= 0.0005F;
		}

		/// <summary>
		/// Prints the .NET font-string of the currently chosen font (for user
		/// to use in Settings.Cfg).
		/// </summary>
		void printstring()
		{
			TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
			tb_FontString.Text = tc.ConvertToString(lbl_Lazydog.Font);
			tb_FontString.SelectionStart = tb_FontString.Text.Length;
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
