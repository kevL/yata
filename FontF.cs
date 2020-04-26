using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

// DEBUG ->
using System.IO;
using Microsoft.Win32;


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

		static string ERROR_FONTSTYLE = " does not support the Regular style."
									  + Environment.NewLine + Environment.NewLine
									  + "choose a different style ...";
		const  string ERROR_FONTSIZE = "Point size too small.";

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

		int  _id = -1;	// tracks the current index of the fontlist because .net will treat it as changed when it didn't.
		bool _regular;	// true if the currently chosen font's Regular style is available
		#endregion Fields


		#region Properties
		internal bool Maximized
		{ get; private set; }
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor. Instantiates this font-dialog.
		/// </summary>
		/// <param name="f">the form that owns this dialog</param>
		internal FontF(YataForm f)
		{
			//logfile.Log("\ncTor");
			InitializeComponent();
			_f = f;

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

			if (_w != -1)
				ClientSize = new Size(_w,_h);

			MinimumSize = new Size(Width  - ClientSize.Width  + 300,
								   Height - ClientSize.Height + 240);


			// Safely ensure that Yata's current font is good to go
			// else set defaults ->
			_init = true;

			int fontList_init = -1; // for showing the initial font's characteristics in the list ->
			int fontList_test = -1;

			// DEBUG ->
			//LogValidStyles();
			//LogAllFonts();
			//LogFontFiles();


			Font font;
			foreach (var ff in FontFamily.Families)
			{
				FontStyle style = getStyle(ff);
				if (style != FontStyleInvalid)
				{
					font = new Font(ff, 10, style);
					if (font.Name == ff.Name) // safety.
					{
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
					else
						font.Dispose();
				}
			}

			float size_init;
			FontStyle style_init;

			if (fontList_init != -1)
			{
				list_Font.SelectedIndex = fontList_init;
				style_init = _f.Font.Style;
				size_init  = _f.Font.SizeInPoints;
			}
			else
			{
				list_Font.SelectedIndex = 0;
				style_init = getStyle(_ffs[0]); // you'd better have at least 1 font on your system buckwheat /lol
				size_init  = 10F;
			}

			cb_Bold.Checked = (style_init & FontStyle.Bold)      != 0;
			cb_Ital.Checked = (style_init & FontStyle.Italic)    != 0;
			cb_Undr.Checked = (style_init & FontStyle.Underline) != 0;
			cb_Strk.Checked = (style_init & FontStyle.Strikeout) != 0;

			tb_FontSize.Text = size_init.ToString();
			tb_FontSize.MouseWheel += size_mousewheel; // NOTE: Mousewheel event is not shown in the designer.

			_init = false;

			fontchanged(list_Font, EventArgs.Empty);
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
			//logfile.Log("OnLoad()");
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
			if (WindowState == FormWindowState.Normal)
			{
				_x = Left;
				_y = Top;
				_w = ClientSize.Width;
				_h = ClientSize.Height;
			}

			_f.FontF_closing();

			lbl_Lazydog.Font.Dispose();
		}

		/// <summary>
		/// Handles the form-resize event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnResize(object sender, EventArgs e)
		{
			//logfile.Log("OnResize());
			if (WindowState != FormWindowState.Minimized)
			{
				if (sc_Hori.SplitterDistance > ClientSize.Height - 24)
					sc_Hori.SplitterDistance = Math.Max(0, ClientSize.Height - 24);

				list_Font.TopIndex = list_Font.SelectedIndex;

				if (WindowState == FormWindowState.Normal)
				{
					_w = ClientSize.Width;
					_h = ClientSize.Height;

					Maximized = false;
				}
				else
					Maximized = true;
			}
		}

		void OnSplitterMoved(object sender, SplitterEventArgs e)
		{
			//logfile.Log("OnSplitterMoved()");
			list_Font.TopIndex = list_Font.SelectedIndex;
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
					if (list_Font.SelectedIndex == _id)
						return;

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
				}
				CreateLazydog();
			}
		}
/* GdiPlus.dll - gdiplusheaders.h (#include Gdiplus.h)
typedef enum FontStyle
{
	FontStyleRegular,
	FontStyleBold,
	FontStyleItalic,
	FontStyleBoldItalic,
	FontStyleUnderline,
	FontStyleStrikeout
};
*/

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
				if (e.Delta > 0)
				{
					if (size >= 1F)
						tb_FontSize.Text = (size + 0.75F).ToString();
				}
				else if (e.Delta < 0)
				{
					if (size >= 1.75F)
						tb_FontSize.Text = (size - 0.75F).ToString();
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
			return FontStyleInvalid; // yes this can happen.
		}

		/// <summary>
		/// Creates the lazydog font from the dialog's current values and prints
		/// the preview text in that font. Also decides whether the Apply button
		/// is enabled.
		/// </summary>
		void CreateLazydog()
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

					if (style == FontStyle.Regular && !_regular) // error
					{
						ErrorLazydog();
						return;
					}

					lbl_Lazydog.ForeColor = SystemColors.ControlText;
					lbl_Lazydog.Font = new Font(_ffs[_id].Name, size, style);
					lbl_Lazydog.Text = LAZYDOG;

					printstring();
				}
				else
				{
					ErrorLazydog(true);
					return;
				}
			}
			bu_Apply.Enabled = !copulate(lbl_Lazydog.Font, _f.Font);
		}

		/// <summary>
		/// Handles lazydog errors.
		/// </summary>
		/// <param name="size">true if error is generated by a point-size error
		/// else error is a font-style error</param>
		void ErrorLazydog(bool size = false)
		{
			lbl_Lazydog.ForeColor = Color.Crimson;
			lbl_Lazydog.Font = new Font("Verdana", 8F, FontStyle.Bold);

			if (size)
				lbl_Lazydog.Text = ERROR_FONTSIZE;
			else
				lbl_Lazydog.Text = _ffs[_id].Name + ERROR_FONTSTYLE;

			tb_FontString.Text = String.Empty;

			bu_Apply.Enabled = false;
		}

		/// <summary>
		/// Compares two fonts for a reasonable identity.
		/// @note .net's Equals() check compares a bunch of stuff that's
		/// irrelevant here.
		/// </summary>
		/// <param name="font1">first font</param>
		/// <param name="font2">second font</param>
		/// <returns>true if 'font1' and 'font2' are reasonably identical</returns>
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
		/// </summary>
		void DisposeFonts()
		{
			foreach (var font in _fonts)
				font.Dispose();
		}
		#endregion Methods


		#region DEBUG
		/// <summary>
		/// Debug funct.
		/// </summary>
		void LogValidStyles()
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

				logfile.Log();
			}
		}

		/// <summary>
		/// Debug funct.
		/// </summary>
		void LogAllFonts()
		{
			using (var ifc = new System.Drawing.Text.InstalledFontCollection())
			{
				logfile.Log("InstalledFontCollection count= " + ifc.Families.Length);
				foreach (var ff in ifc.Families)
					logfile.Log(ff.Name);
			}

			logfile.Log("FontFamily.Familes count= " + FontFamily.Families.Length);
			foreach (var ff in FontFamily.Families)
				logfile.Log(ff.Name);
		}

		/// <summary>
		/// Debug funct.
		/// </summary>
		void LogFontFiles()
		{
			var di = new DirectoryInfo(@"C:\Windows\Fonts");

			var ttfFiles = new List<string>();

			FileInfo[] files = di.GetFiles("*.ttf");
			foreach (FileInfo file in files)
			{
				logfile.Log(file.Name);
				ttfFiles.Add(file.Name.ToLower());
			}
			logfile.Log();
			logfile.Log("COUNT (ttf)= " + files.Length);
			logfile.Log();
			logfile.Log();

			files = di.GetFiles("*.ttc");
			foreach (FileInfo file in files)
			{
				logfile.Log(file.Name);
			}
			logfile.Log();
			logfile.Log("COUNT (ttc)= " + files.Length);
			logfile.Log();
			logfile.Log();

			files = di.GetFiles("*.otf");
			foreach (FileInfo file in files)
			{
				logfile.Log(file.Name);
			}
			logfile.Log();
			logfile.Log("COUNT (otf)= " + files.Length);
			logfile.Log();
			logfile.Log();

			files = di.GetFiles("*.fon");
			foreach (FileInfo file in files)
			{
				logfile.Log(file.Name);
			}
			logfile.Log();
			logfile.Log("COUNT (fon)= " + files.Length);
			logfile.Log();
			logfile.Log();


			int countTtf = 0;
			int countTtc = 0;
			int countOtf = 0;
			int countFon = 0;

			var ttfInstalled = new List<string>();

			RegistryKey fonts = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts", false);
			string[] vars = fonts.GetValueNames();
			foreach (string @var in vars)
			{
				string val = fonts.GetValue(@var) as String;
				logfile.Log(@var + " : " + val);

				switch (val.Substring(val.Length - 3).ToLower())
				{
					case "ttf":
						++countTtf;
						ttfInstalled.Add(val.ToLower());
						break;

					case "ttc": ++countTtc; break;
					case "otf": ++countOtf; break;
					case "fon": ++countFon; break;
				}
			}
			logfile.Log();
			logfile.Log("COUNT (installed)= "     + vars.Length);
			logfile.Log("COUNT (installed ttf)= " + countTtf);
			logfile.Log("COUNT (installed ttc)= " + countTtc);
			logfile.Log("COUNT (installed otf)= " + countOtf);
			logfile.Log("COUNT (installed fon)= " + countFon);
			logfile.Log();
			logfile.Log();

			foreach (string val in ttfFiles)
			{
				if (!ttfInstalled.Contains(val))
					logfile.Log("installed does NOT contain : " + val);
			}
		}
		#endregion DEBUG
	}
}
