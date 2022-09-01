using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

// DEBUG ->
//using System.IO;
//using Microsoft.Win32;


namespace yata
{
	sealed partial class FontDialog
		: YataDialog
	{
		#region Fields (static)
		static string stLAZYDOG = "01234567890 ` ~ ! @ # $ % ^ & * ( ) _ + - = \\ | [ ] { } ; : ' \" , < . > / ?" + Environment.NewLine
								+ "the quick brown fox jumps over the lazy dog" + Environment.NewLine
								+ "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG";

		static string ERROR_FONTSTYLE = " does not support the Regular style."
									  + Environment.NewLine + Environment.NewLine
									  + "choose a different style ...";
		const  string ERROR_FONTSIZE = "Point size too small.";

		/// <summary>
		/// Caches <c><see cref="sc_Hori"/>.SplitterDistance</c>
		/// between instantiations.
		/// </summary>
		/// <remarks>The designer arbitrarily increments the value of
		/// <c>SplitterDistance</c> so initialize it here.</remarks>
		static int _scDistance = 283;

		const FontStyle FontStyleInvalid = (FontStyle)16; // .net doesn't define Invalid.

		/* GdiPlus.dll - gdiplusheaders.h (#include Gdiplus.h)
		typedef enum FontStyle
		{
			FontStyleRegular,
			FontStyleBold,
			FontStyleItalic,
			FontStyleBoldItalic,
			FontStyleUnderline,
			FontStyleStrikeout
		}; */
		#endregion Fields (static)


		#region Fields
		readonly List<FontFamily> _ffs = new List<FontFamily>();

		/// <summary>
		/// All fonts used to render <c><see cref="list_Font"/></c> will be
		/// disposed.
		/// </summary>
		readonly List<Font> _fonts = new List<Font>();

		/// <summary>
		/// Bypasses the <c><see cref="changefont()">changefont()</see></c>
		/// routine.
		/// </summary>
		bool _init = true;

		/// <summary>
		/// Tracks the current index of <c><see cref="list_Font"/></c> because
		/// .net will treat it as changed when it didn't.
		/// </summary>
		int _id = -1;

		/// <summary>
		/// Tracks the top-id in the font-list.
		/// </summary>
		int _tid;

		readonly Timer _t1 = new Timer();

		/// <summary>
		/// <c>true</c> if the currently chosen font's Regular <c>FontStyle</c>
		/// is available.
		/// </summary>
		bool _regular;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"><c><see cref="Yata"/></c></param>
		internal FontDialog(Yata f)
		{
			_f = f;

			InitializeComponent();
			Initialize(METRIC_FUL, true, true);

			tb_FontSize  .BackColor = Color.White;
			tb_FontString.BackColor = Colors.TextboxBackground;


			// Safely ensure that Yata's current font is good to go
			// else set defaults ->

			int font_init = -1; // for showing the initial font's characteristics in the list ->
			int font_test = -1;

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
						if (font_init == -1) // look for Yata's current font ->
						{
							++font_test;
							if (font.Name == _f.Font.Name)
								font_init = font_test;
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

			if (font_init != -1)
			{
				list_Font.SelectedIndex = font_init;
				style_init = _f.Font.Style;
				size_init  = _f.Font.SizeInPoints;
			}
			else
			{
				list_Font.SelectedIndex = 0; // you'd better have at least 1 font on your system buckwheat /lol
				style_init = getStyle(_ffs[0]);
				size_init  = 10F;
			}

			cb_Bold.Checked = (style_init & FontStyle.Bold)      != 0;
			cb_Ital.Checked = (style_init & FontStyle.Italic)    != 0;
			cb_Undr.Checked = (style_init & FontStyle.Underline) != 0;
			cb_Strk.Checked = (style_init & FontStyle.Strikeout) != 0;

			tb_FontSize.Text = size_init.ToString(CultureInfo.InvariantCulture);
			tb_FontSize.MouseWheel += size_mousewheel; // NOTE: Mousewheel event is not shown in the designer.

			_init = false;

			changefont(list_Font, EventArgs.Empty);

			_t1.Tick += t1_tick;
			_t1.Interval = 100;

			Show(_f); // Yata is owner.
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>Load</c> handler.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>This ought ensure that the FontPicker appears on top. If a
		/// user has a lot of fonts installed on their system the FontPicker
		/// takes a while to load ... if so it tends to get hidden beneath main.
		/// 
		/// 
		/// There are a bazillion ways to accomplish that. Getting one to work
		/// consistently is a dice roll.
		/// <list type="bullet">
		/// <item><c>BringToFront()</c></item>
		/// <item><c>TopMost</c> <c>true</c>/<c>false</c></item>
		/// <item><c>Activate()</c></item>
		/// <item><c>Focus()</c></item>
		/// <item><c>Select()</c></item>
		/// <item><c>FormWindowState.Minimized</c>/<c>FormWindowState.Normal</c>
		/// or <c>.Maximized</c> (as detered by the
		/// <c><see cref="Maximized"/></c> <c>bool</c>)</item>
		/// <item>etc.</item>
		/// </list></remarks>
		protected override void OnLoad(EventArgs e)
		{
			sc_Hori.SplitterDistance = _scDistance;
			BringToFront();
			list_Font.TopIndex = list_Font.SelectedIndex;
			_t1.Start();

			base.OnLoad(e);
		}

		/// <summary>
		/// Overrides the <c>FormClosing</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			(_f as Yata).CloseFontDialog();

			_scDistance = sc_Hori.SplitterDistance;

			_t1.Dispose();
			lbl_Lazydog.Font.Dispose();
			foreach (var font in _fonts)
				font.Dispose();

			base.OnFormClosing(e);
		}


		/// <summary>
		/// Overrides the <c>Resize</c> handler. Restores
		/// <c><see cref="_tid"/></c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			if (WindowState != FormWindowState.Minimized)
			{
				list_Font.BeginUpdate();
				base.OnResize(e);

				if (!_init) list_Font.TopIndex = _tid;
				list_Font.EndUpdate();
			}
			else
				base.OnResize(e);
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// lol bongo.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Fixes the glitch that occurs if
		/// <c><see cref="sc_Hori"/>.Panel1</c> is at
		/// <c><see cref="sc_Hori"/>.Panel1MinSize</c> and user resizes this
		/// <c>FontDialog</c> to lessen its <c>Height</c>.
		/// <c><see cref="sc_Hori"/>.Panel2</c> fails to figure out that it
		/// needs to decrease in height and overflows off under
		/// <c><see cref="tb_FontString"/></c> and off below the
		/// <c><see cref="Form.ClientRectangle"/></c>. Touching
		/// <c><see cref="sc_Hori"/>.SplitterDistance</c> forces the
		/// <c>SplitContainer</c> to recalc ...</remarks>
		void OnSplitContainerResize(object sender, EventArgs e)
		{
			if (WindowState != FormWindowState.Minimized)
			{
				if (sc_Hori.SplitterDistance == sc_Hori.Panel1.Height)
				{
					sc_Hori.SplitterDistance = sc_Hori.SplitterDistance - 1;
					sc_Hori.SplitterDistance = sc_Hori.SplitterDistance + 1;
				}
				else
				{
					sc_Hori.SplitterDistance = sc_Hori.SplitterDistance + 1;
					sc_Hori.SplitterDistance = sc_Hori.SplitterDistance - 1;
				}
			}
		}

		/// <summary>
		/// Tracks the top-id.
		/// </summary>
		/// <param name="sender"><c><see cref="_t1"/></c></param>
		/// <param name="e"></param>
		void t1_tick(object sender, EventArgs e)
		{
			_tid = list_Font.TopIndex;
		}
		#endregion Handlers


		#region button handlers
		/// <summary>
		/// Applies the <c>Font</c> that Yata uses but does not close this
		/// <c>FontDialog</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Apply"/></c></param>
		/// <param name="e"></param>
		void click_Apply(object sender, EventArgs e)
		{
			bu_Apply.Enabled = false;
			(_f as Yata).doFont(lbl_Lazydog.Font.Clone() as Font);

			BringToFront(); // see OnLoad() doc
		}
		#endregion button handlers


		#region font list/size/style handlers
		/// <summary>
		/// Draws the font-entries in <c><see cref="list_Font"/></c> with their
		/// own <c>Font</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="list_Font"/></c></param>
		/// <param name="e"></param>
		void fontList_drawitem(object sender, DrawItemEventArgs e)
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
		/// Fired by any control that causes the lazydog-text to change.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="list_Font"/></c> - <c>SelectedIndexChanged</c> and cTor</item>
		/// <item><c><see cref="tb_FontSize"/></c> - <c>TextChanged</c></item>
		/// <item><c><see cref="cb_Strk"/></c> - <c>CheckedChanged</c></item>
		/// <item><c><see cref="cb_Undr"/></c> - <c>CheckedChanged</c></item>
		/// <item><c><see cref="cb_Ital"/></c> - <c>CheckedChanged</c></item>
		/// <item><c><see cref="cb_Bold"/></c> - <c>CheckedChanged</c></item>
		/// </list></param>
		/// <param name="e"></param>
		void changefont(object sender, EventArgs e)
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
				CreateLazydogFont();
			}
		}

		/// <summary>
		/// Increases or decreases the pointsize with the mousewheel when the
		/// size-box has focus.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_FontSize"/></c></param>
		/// <param name="e"></param>
		void size_mousewheel(object sender, MouseEventArgs e)
		{
			float size;
			if (Single.TryParse(tb_FontSize.Text, out size))
			{
				if (e.Delta > 0)
				{
					if (size >= 1F)
						tb_FontSize.Text = (size + 0.75F).ToString(CultureInfo.InvariantCulture);
				}
				else if (e.Delta < 0)
				{
					if (size >= 1.75F)
						tb_FontSize.Text = (size - 0.75F).ToString(CultureInfo.InvariantCulture);
				}
			}
		}

		/// <summary>
		/// Focuses the size-box when the size-label is clicked.
		/// </summary>
		/// <param name="sender"><c><see cref="lbl_FontSize"/></c></param>
		/// <param name="e"></param>
		void click_pointlabel(object sender, EventArgs e)
		{
			tb_FontSize.Focus();
			tb_FontSize.SelectionStart = tb_FontSize.Text.Length;
		}
		#endregion font list/size/style handlers


		#region Methods
		/// <summary>
		/// Gets the first available <c>FontStyle</c> in a given
		/// <c>FontFamily</c>.
		/// </summary>
		/// <param name="ff">a <c>FontFamily</c></param>
		/// <returns>the first <c>FontStyle</c> found else
		/// <c><see cref="FontStyleInvalid"/></c></returns>
		/// <remarks>ALL STYLES ARE ALWAYS AVAILABLE. Thanks .net! .not</remarks>
		static FontStyle getStyle(FontFamily ff)
		{
			foreach (FontStyle style in Enum.GetValues(typeof(FontStyle)))
			if (ff.IsStyleAvailable(style))
				return style;

			return FontStyleInvalid; // yes this can happen.
		}

		/// <summary>
		/// Creates the <c>Font</c> for <c><see cref="stLAZYDOG"/></c> from the
		/// dialog's current values and prints the preview text in that font.
		/// Also decides whether the Apply button is enabled.
		/// </summary>
		void CreateLazydogFont()
		{
			if (_id != -1) // safety.
			{
				lbl_Lazydog.Font.Dispose();

				float size;
				if (Single.TryParse(tb_FontSize.Text, out size)
					&& size >= 1F)
				{
					FontStyle style = FontStyle.Regular;				// =0
					if (cb_Bold.Checked) style |= FontStyle.Bold;		// =1
					if (cb_Ital.Checked) style |= FontStyle.Italic;		// =2
					if (cb_Undr.Checked) style |= FontStyle.Underline;	// =4
					if (cb_Strk.Checked) style |= FontStyle.Strikeout;	// =8

					if (style == FontStyle.Regular && !_regular) // error
					{
						LazydogError();
						return;
					}

					lbl_Lazydog.ForeColor = SystemColors.ControlText;
					lbl_Lazydog.Font = new Font(_ffs[_id].Name, size, style);
					lbl_Lazydog.Text = stLAZYDOG;

					printstring();
				}
				else
				{
					LazydogError(true);
					return;
				}
			}
			bu_Apply.Enabled = !copulate(lbl_Lazydog.Font, _f.Font);
		}

		/// <summary>
		/// Handles lazydog errors.
		/// </summary>
		/// <param name="fontsize"><c>true</c> if error is generated by a
		/// point-size error; <c>false</c> if error is a font-style error</param>
		void LazydogError(bool fontsize = false)
		{
			lbl_Lazydog.ForeColor = Color.Crimson;
			lbl_Lazydog.Font = new Font("Verdana", 8F, FontStyle.Bold);

			if (fontsize)
				lbl_Lazydog.Text = ERROR_FONTSIZE;
			else
				lbl_Lazydog.Text = _ffs[_id].Name + ERROR_FONTSTYLE;

			tb_FontString.Text = String.Empty;

			bu_Apply.Enabled = false;
		}

		/// <summary>
		/// Compares two <c>Fonts</c> for a reasonable identity.
		/// </summary>
		/// <param name="font1">first <c>Font</c></param>
		/// <param name="font2">second <c>Font</c></param>
		/// <returns>true if <paramref name="font1"/> and
		/// <paramref name="font2"/> are reasonably identical</returns>
		/// <remarks>.net's Equals() check compares a bunch of stuff that's
		/// irrelevant here.</remarks>
		static bool copulate(Font font1, Font font2)
		{
			return font1.Name  == font2.Name
				&& font1.Style == font2.Style
				&& Math.Abs(font2.SizeInPoints - font1.SizeInPoints) <= 0.0005F;
		}

		/// <summary>
		/// Prints the .NET font-string of the currently chosen <c>Font</c>.
		/// </summary>
		/// <remarks>For user to use in Settings.Cfg.</remarks>
		void printstring()
		{
			TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
			tb_FontString.Text = tc.ConvertToString(lbl_Lazydog.Font);
			tb_FontString.SelectionStart = tb_FontString.Text.Length;
		}
		#endregion Methods


/*		#region DEBUG
		/// <summary>
		/// Debug funct.
		/// </summary>
		static void LogValidStyles()
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
		static void LogAllFonts()
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
		static void LogFontFiles()
		{
			var di = new DirectoryInfo(@"C:\Windows\Fonts");

			var ttfFiles = new List<string>();

			FileInfo[] files = di.GetFiles("*.ttf");
			foreach (FileInfo file in files)
			{
				logfile.Log(file.Name);
				ttfFiles.Add(file.Name.ToLowerInvariant());
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

				switch (val.Substring(val.Length - 3).ToLowerInvariant())
				{
					case "ttf":
						++countTtf;
						ttfInstalled.Add(val.ToLowerInvariant());
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
		#endregion DEBUG */
	}
}
