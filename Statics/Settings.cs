using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;


namespace yata
{
	static class Settings
	{
		#region Fields (static)
		internal const string FE = "settings.cfg";

		internal static Font _font;			// the grid's font
		internal static Font _fontdialog;	// the font to be used in all dialogs unless '_font2dialog' is valid.

		internal static Font _font2;		// _bar, _contextRo, _contextCe, _contextTa, statusbar
		internal static Font _font2dialog;	// all dialogs if valid.

		internal static Font _font3;		// propanel

		internal static Font _fontf;		// richtextboxes (preferably fixed-font)
		internal static Font _fontf_tb;		// textboxes (preferably fixed-font)

		internal static Font _fonti;		// infobox heads

		internal static readonly List<string> _dirpreset = new List<string>();
		internal static readonly List<string> _pathall   = new List<string>();
		internal static string _pathzipdata;

		internal static int _x = -1;
		internal static int _y = -1;
		internal static int _w = -1;
		internal static int _h = -1;

		internal static bool _strict;
		internal static bool _gradient;
		internal static bool _context;
		internal static bool _maximized;
		internal static bool _instantgoto;
		internal static bool _casesort;
		internal static bool _autorder;
		internal static bool _allowdupls;
		internal static bool _acceptedit;
		internal static bool _clearquotes;

		internal static int _recent;
		internal static int _alignoutput;
		internal static int _codepage;

		internal static string _diff;
		internal static string _dialog;
		internal static string _dialogalt;
		internal static string _icondir;
		internal static string _icondiralt;

		internal static Brush _colorstatusbar    = Brushes.MintCream;			// default color for the statusbar

		internal static Color _colortabletext    = SystemColors.ControlText;	// default colors for the tablegrid ->
		internal static Brush _colorrowa         = Brushes.AliceBlue;
		internal static Brush _colorrowb         = Brushes.BlanchedAlmond;
		internal static Brush _colorrowdisableda = Brushes.LavenderBlush;
		internal static Brush _colorrowdisabledb = Brushes.MistyRose;

		internal static Color _colorfrozentext   = SystemColors.ControlText;	// default colors for the frozenpanel ->
		internal static Color _colorfrozen       = Color.OldLace;

		internal static Color _colorpropanel     = Color.LightSteelBlue;		// default colors for the propanel ->
		internal static Color _colorpropaneltext = SystemColors.ControlText;


		internal const int AoFalse    = 0; // '_alignoutput' vals ->
		internal const int AoTrue     = 1;
		internal const int AoTabs     = 2;
		internal const int AoElectron = 3;
		#endregion Fields (static)


		#region Methods (static)
		internal static void ScanSettings()
		{
			string pfe = Path.Combine(Application.StartupPath, FE);
			if (File.Exists(pfe))
			{
				using (var fs = File.OpenRead(pfe))
				using (var sr = new StreamReader(fs))
				{
					int result;

					string line;
					while ((line = sr.ReadLine()) != null)
					{
						if (line.StartsWith("font=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(5).Trim()).Length != 0)
							{
								TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
								_font = tc.ConvertFromInvariantString(line) as Font;

								int pos = line.IndexOf(',');
								if (pos == -1)
									pos = line.Length;

								if (line.Substring(0, pos) != _font.Name)
								{
									_font.Dispose(); // NOTE: Fail silently.
									_font = null;
								}
								else
									_fontdialog = CreateDialogFont(_font);
							}
						}
						else if (line.StartsWith("font2=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(6).Trim()).Length != 0)
							{
								TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
								_font2 = tc.ConvertFromInvariantString(line) as Font;

								int pos = line.IndexOf(',');
								if (pos == -1)
									pos = line.Length;

								if (line.Substring(0, pos) != _font2.Name)
								{
									_font2.Dispose(); // NOTE: Fail silently.
									_font2 = null;
								}
								else
									_font2dialog = CreateDialogFont(_font2);
							}
						}
						else if (line.StartsWith("font3=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(6).Trim()).Length != 0)
							{
								TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
								_font3 = tc.ConvertFromInvariantString(line) as Font;

								int pos = line.IndexOf(',');
								if (pos == -1)
									pos = line.Length;

								if (line.Substring(0, pos) != _font3.Name)
								{
									_font3.Dispose(); // NOTE: Fail silently.
									_font3 = null;
								}
							}
						}
						else if (line.StartsWith("fontf=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(6).Trim()).Length != 0)
							{
								TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
								_fontf = tc.ConvertFromInvariantString(line) as Font;

								int pos = line.IndexOf(',');
								if (pos == -1)
									pos = line.Length;

								if (line.Substring(0, pos) != _fontf.Name)
								{
									_fontf.Dispose(); // NOTE: Fail silently.
									_fontf = null;
								}
								else
									_fontf_tb = CreateDialogFont(_fontf);
							}
						}
						else if (line.StartsWith("fonti=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(6).Trim()).Length != 0)
							{
								TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
								_fonti = tc.ConvertFromInvariantString(line) as Font;

								int pos = line.IndexOf(',');
								if (pos == -1)
									pos = line.Length;

								if (line.Substring(0, pos) != _fonti.Name)
								{
									_fonti.Dispose(); // NOTE: Fail silently.
									_fonti = null;
								}
							}
						}
						else if (line.StartsWith("dirpreset=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(10).Trim()).Length != 0
								&& Directory.Exists(line))
							{
								_dirpreset.Add(line);
							}
						}
						else if (line.StartsWith("pathall=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(8).Trim()).Length != 0
								&& Directory.Exists(line))
							{
								_pathall.Add(line);
							}
						}
						else if (line.StartsWith("pathzipdata=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(12).Trim()).Length != 0
								&& Directory.Exists(line))
							{
								_pathzipdata = line;
							}
						}
						else if (line.StartsWith("x=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(2).Trim()).Length != 0
								&& Int32.TryParse(line, out result) && result > -1)
							{
								_x = result;
							}
						}
						else if (line.StartsWith("y=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(2).Trim()).Length != 0
								&& Int32.TryParse(line, out result) && result > -1)
							{
								_y = result;
							}
						}
						else if (line.StartsWith("w=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(2).Trim()).Length != 0
								&& Int32.TryParse(line, out result) && result > -1)
							{
								_w = result;
							}
						}
						else if (line.StartsWith("h=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(2).Trim()).Length != 0
								&& Int32.TryParse(line, out result) && result > -1)
							{
								_h = result;
							}
						}
						else if (line.StartsWith("strict=", StringComparison.Ordinal))
						{
							_strict = (line = line.Substring(7).Trim()).Length != 0
								   && (line == "1" || line.ToLowerInvariant() == "true");
						}
						else if (line.StartsWith("gradient=", StringComparison.Ordinal))
						{
							_gradient = (line = line.Substring(9).Trim()).Length != 0
									 && (line == "1" || line.ToLowerInvariant() == "true");
						}
						else if (line.StartsWith("context=", StringComparison.Ordinal))
						{
							_context = (line = line.Substring(8).Trim()).Length != 0
									&& line.ToLowerInvariant() == "static";
						}
						else if (line.StartsWith("recent=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(7).Trim()).Length != 0
								&& Int32.TryParse(line, out result) && result > 0)
							{
								if (result > 16) result = 16;
								_recent = result;
							}
						}
						else if (line.StartsWith("diff=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(5).Trim()).Length != 0)
							{
								_diff = line;
							}
						}
						else if (line.StartsWith("dialog=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(7).Trim()).Length != 0)
							{
								_dialog = line;
							}
						}
						else if (line.StartsWith("dialogalt=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(10).Trim()).Length != 0)
							{
								_dialogalt = line;
							}
						}
						else if (line.StartsWith("maximized=", StringComparison.Ordinal))
						{
							_maximized = (line = line.Substring(10).Trim()).Length != 0
									  && (line == "1" || line.ToLowerInvariant() == "true");
						}
						else if (line.StartsWith("instantgoto=", StringComparison.Ordinal))
						{
							_instantgoto = (line = line.Substring(12).Trim()).Length != 0
										&& (line == "1" || line.ToLowerInvariant() == "true");
						}
						else if (line.StartsWith("casesort=", StringComparison.Ordinal))
						{
							_casesort = (line = line.Substring(9).Trim()).Length != 0
									 && (line == "1" || line.ToLowerInvariant() == "true");
						}
						else if (line.StartsWith("alignoutput=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(12).Trim()).Length != 0)
							{
								switch (line.ToLowerInvariant())
								{
									case "1":
									case "true": _alignoutput = AoTrue; break;

									case "2":
									case "tabs": _alignoutput = AoTabs; break;

									case "3":
									case "electron": _alignoutput = AoElectron; break;
								}
							}
						}
						else if (line.StartsWith("codepage=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(9).Trim()).Length != 0
								&& Int32.TryParse(line, out result)
								&& result > -1 && result < 65536)
							{
								_codepage = result;
							}
						}
						else if (line.StartsWith("autorder=", StringComparison.Ordinal))
						{
							_autorder = (line = line.Substring(9).Trim()).Length != 0
									 && (line == "1" || line.ToLowerInvariant() == "true");
						}
						else if (line.StartsWith("allowdupls=", StringComparison.Ordinal))
						{
							_allowdupls = (line = line.Substring(11).Trim()).Length != 0
									   && (line == "1" || line.ToLowerInvariant() == "true");
						}
						else if (line.StartsWith("acceptedit=", StringComparison.Ordinal))
						{
							_acceptedit = (line = line.Substring(11).Trim()).Length != 0
									   && (line == "1" || line.ToLowerInvariant() == "true");
						}
						else if (line.StartsWith("clearquotes=", StringComparison.Ordinal))
						{
							_clearquotes = (line = line.Substring(12).Trim()).Length != 0
										&& (line == "1" || line.ToLowerInvariant() == "true");
						}
						else if (line.StartsWith("icondir=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(8).Trim()).Length != 0)
							{
								_icondir = line;
							}
						}
						else if (line.StartsWith("icondiralt=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(11).Trim()).Length != 0)
							{
								_icondiralt = line;
							}
						}
						else if (line.StartsWith("colorstatusbar=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(15).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Brushes).GetProperty(line);
								if (pi != null)
								{
									_colorstatusbar = pi.GetValue(null,null) as Brush;
								}
							}
						}
						else if (line.StartsWith("colortabletext=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(15).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_colortabletext = (Color)pi.GetValue(null,null);
								}
							}
						}
						else if (line.StartsWith("colorrowa=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(10).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Brushes).GetProperty(line);
								if (pi != null)
								{
									_colorrowa = pi.GetValue(null,null) as Brush;
								}
							}
						}
						else if (line.StartsWith("colorrowb=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(10).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Brushes).GetProperty(line);
								if (pi != null)
								{
									_colorrowb = pi.GetValue(null,null) as Brush;
								}
							}
						}
						else if (line.StartsWith("colorrowdisableda=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(18).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Brushes).GetProperty(line);
								if (pi != null)
								{
									_colorrowdisableda = pi.GetValue(null,null) as Brush;
								}
							}
						}
						else if (line.StartsWith("colorrowdisabledb=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(18).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Brushes).GetProperty(line);
								if (pi != null)
								{
									_colorrowdisabledb = pi.GetValue(null,null) as Brush;
								}
							}
						}
						else if (line.StartsWith("colorfrozentext=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(16).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_colorfrozentext = (Color)pi.GetValue(null,null);
								}
							}
						}
						else if (line.StartsWith("colorfrozen=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(12).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_colorfrozen = (Color)pi.GetValue(null,null);
								}
							}
						}
						else if (line.StartsWith("colorpropanel=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(14).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_colorpropanel = (Color)pi.GetValue(null,null);
								}
							}
						}
						else if (line.StartsWith("colorpropaneltext=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(18).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_colorpropaneltext = (Color)pi.GetValue(null,null);
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Clones a specified <c>Font</c> and reduces it in size (if necessary)
		/// to fit in dialogs.
		/// </summary>
		/// <param name="font">a <c>Font</c> to potentially reduce in size</param>
		/// <returns>a <c>Font</c> that's roughly the size of Yata's default
		/// <c>Font</c></returns>
		internal static Font CreateDialogFont(ICloneable font)
		{
			var fontdialog = font.Clone() as Font;

			string label = fontdialog.Name;
			FontStyle style = Yata.getStyleStandard(fontdialog.FontFamily);

			// the font as it appears in a dialog is smaller than the same font
			// with the SAME pointsize as it appears on the table ...

			float pts = fontdialog.SizeInPoints;
			while (YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, fontdialog) > YataGraphics.hFontDefault)
			{
				fontdialog.Dispose();
				fontdialog = new Font(label, pts -= 0.75F, style);
			}
			return fontdialog;
		}


		/// <summary>
		/// Sets <c>Fonts</c> for a <c><see cref="YataDialog"/></c>.
		/// </summary>
		/// <param name="f">a <c><see cref="YataDialog"/></c></param>
		/// <param name="bypassColor"><c>true</c> to bypass setting the
		/// <c>TextBoxBases'</c> <c>BackColors</c> to the Yata-default</param>
		/// <param name="bypassFont"><c>true</c> to bypass setting the dialog's
		/// <c>Font</c></param>
		/// <remarks>IMPORTANT: Make sure that the <c>Font</c> for any
		/// <c>TextBoxBases</c> ARE INSTANTIATED in the Designer - this funct
		/// will <c>Dispose()</c> those <c>Fonts</c>. If a <c>TextBoxBase</c>
		/// happens to use the .net default <c>Font</c> it will get disposed and
		/// then the app is borked since the .net default <c>Font</c> will no
		/// longer be available at all.</remarks>
		internal static void SetFonts(YataDialog f, bool bypassColor, bool bypassFont)
		{
			if (!bypassFont)
			{
				if (_font2dialog != null)
					f.Font = _font2dialog;
				else
					f.Font = _fontdialog;
			}

			foreach (var tbb in f._tbbs)
			{
				if (_fontf != null)
				{
					tbb.Font.Dispose();

					if (tbb is RichTextBox) tbb.Font = _fontf;
					else                    tbb.Font = _fontf_tb; // is TextBox
				}

				if (!bypassColor)
					tbb.BackColor = Colors.TextboxBackground;
			}
		}
		#endregion Methods (static)


		#region options (static)
		/// <summary>
		/// An array of all <c><see cref="options"/></c> <c>strings</c> that are
		/// recognized in the Settings.Cfg file.
		/// </summary>
		/// <remarks>Update if options are added to Yata.</remarks>
		internal static string[] options;

		/// <summary>
		/// The count of options in <c><see cref="options"/></c>.
		/// </summary>
		/// <remarks>Update if options are added to Yata.</remarks>
		internal const int ids = 40;

		/// <summary>
		/// Creates an array of all <c><see cref="options"/></c> <c>strings</c>
		/// that are recognized in the Settings.Cfg file.
		/// </summary>
		/// <remarks>Update if options are added to Yata.</remarks>
		internal static void CreateOptions()
		{
			options = new string[ids];

			int i = -1;
			options[++i] = "font=";
			options[++i] = "font2=";
			options[++i] = "font3=";
			options[++i] = "fontf=";
			options[++i] = "fonti=";
			options[++i] = "x=";
			options[++i] = "y=";
			options[++i] = "w=";
			options[++i] = "h=";
			options[++i] = "maximized=";
			options[++i] = "acceptedit=";
			options[++i] = "alignoutput=";
			options[++i] = "allowdupls=";
			options[++i] = "autorder=";
			options[++i] = "casesort=";
			options[++i] = "clearquotes=";
			options[++i] = "codepage=";
			options[++i] = "colorfrozen=";
			options[++i] = "colorfrozentext=";
			options[++i] = "colorpropanel=";
			options[++i] = "colorpropaneltext=";
			options[++i] = "colorrowa=";
			options[++i] = "colorrowb=";
			options[++i] = "colorrowdisableda=";
			options[++i] = "colorrowdisabledb=";
			options[++i] = "colorstatusbar=";
			options[++i] = "colortabletext=";
			options[++i] = "context=";
			options[++i] = "dialog=";
			options[++i] = "dialogalt=";
			options[++i] = "diff=";
			options[++i] = "dirpreset=";
			options[++i] = "gradient=";
			options[++i] = "icondir=";
			options[++i] = "icondiralt=";
			options[++i] = "instantgoto=";
			options[++i] = "pathall=";
			options[++i] = "pathzipdata=";
			options[++i] = "recent=";
			options[++i] = "strict=";
		}
		#endregion options (static)

/*		/// <summary>
		/// Recursive funct that gets all <c>Controls</c> of a specified
		/// <c>Type</c> in a specified <c>Control</c>.
		/// </summary>
		/// <typeparam name="T">the <c>Type</c> of <c>Control</c> to get a list
		/// of instances of</typeparam>
		/// <param name="f">a <c>Control</c> to investigate</param>
		/// <returns>a <c>List</c> of <c>Controls</c> of <c>Type</c></returns>
		static IList<T> GetAllControls<T>(Control f) where T : Control
		{
			var controls = new List<T>();

			T ctr;
			foreach (Control control in f.Controls)
			{
				if ((ctr = control as T) != null)
					controls.Add(ctr);
				else
					controls.AddRange(GetAllControls<T>(control));
			}
			return controls;
		} */
	}
}
