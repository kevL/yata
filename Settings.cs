using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	static class Settings
	{
		const string FE = "settings.cfg";

		internal static Font _font,  _fontdialog;	// grid
		internal static Font _font2, _font2dialog;	// menus, dialogs
		internal static Font _font3;				// propanel
		internal static Font _fontf, _fontfdialog;	// dialog textboxes etc.

		internal static readonly List<string> _dirpreset = new List<string>();
		internal static readonly List<string> _pathall   = new List<string>();

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

		internal static int _recent;
		internal static int _alignoutput;
		internal static int _codepage;

		internal static string _diff;
		internal static string _dialog;
		internal static string _dialogalt;


		internal const int AoFalse = 0;
		internal const int AoTrue  = 1;
		internal const int AoTabs  = 2;


		internal static void ScanSettings()
		{
			int result;

			string pfe = Path.Combine(Application.StartupPath, FE);
			if (File.Exists(pfe))
			{
				using (var fs = File.OpenRead(pfe))
				using (var sr = new StreamReader(fs))
				{
					string line;
					while ((line = sr.ReadLine()) != null)
					{
						if (line.StartsWith("font=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(5).Trim()))
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
						else if (line.StartsWith("font2=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(6).Trim()))
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
						else if (line.StartsWith("font3=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(6).Trim()))
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
						else if (line.StartsWith("fontf=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(6).Trim()))
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
									_fontfdialog = CreateDialogFont(_fontf);
							}
						}
						else if (line.StartsWith("dirpreset=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(10).Trim())
								&& Directory.Exists(line))
							{
								_dirpreset.Add(line);
							}
						}
						else if (line.StartsWith("pathall=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(8).Trim())
								&& Directory.Exists(line))
							{
								_pathall.Add(line);
							}
						}
						else if (line.StartsWith("x=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(2).Trim())
								&& Int32.TryParse(line, out result) && result > -1)
							{
								_x = result;
							}
						}
						else if (line.StartsWith("y=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(2).Trim())
								&& Int32.TryParse(line, out result) && result > -1)
							{
								_y = result;
							}
						}
						else if (line.StartsWith("w=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(2).Trim())
								&& Int32.TryParse(line, out result) && result > -1)
							{
								_w = result;
							}
						}
						else if (line.StartsWith("h=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(2).Trim())
								&& Int32.TryParse(line, out result) && result > -1)
							{
								_h = result;
							}
						}
						else if (line.StartsWith("strict=", StringComparison.InvariantCulture))
						{
							_strict = (!String.IsNullOrEmpty(line = line.Substring(7).Trim())
								   && line == "true");
						}
						else if (line.StartsWith("gradient=", StringComparison.InvariantCulture))
						{
							_gradient = (!String.IsNullOrEmpty(line = line.Substring(9).Trim())
									 && line == "true");
						}
						else if (line.StartsWith("context=", StringComparison.InvariantCulture))
						{
							_context = (!String.IsNullOrEmpty(line = line.Substring(8).Trim())
									&& line == "static");
						}
						else if (line.StartsWith("recent=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(7).Trim())
								&& Int32.TryParse(line, out result) && result > 0)
							{
								if (result > 16) result = 16;
								_recent = result;
							}
						}
						else if (line.StartsWith("diff=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(5).Trim()))
							{
								_diff = line;
							}
						}
						else if (line.StartsWith("dialog=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(7).Trim()))
							{
								_dialog = line;
							}
						}
						else if (line.StartsWith("dialogalt=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(10).Trim()))
							{
								_dialogalt = line;
							}
						}
						else if (line.StartsWith("maximized=", StringComparison.InvariantCulture))
						{
							_maximized = (!String.IsNullOrEmpty(line = line.Substring(10).Trim())
									  && line == "true");
						}
						else if (line.StartsWith("instantgoto=", StringComparison.InvariantCulture))
						{
							_instantgoto = (!String.IsNullOrEmpty(line = line.Substring(12).Trim())
										&& line == "true");
						}
						else if (line.StartsWith("casesort=", StringComparison.InvariantCulture))
						{
							_casesort = (!String.IsNullOrEmpty(line = line.Substring(9).Trim())
									 && line == "true");
						}
						else if (line.StartsWith("alignoutput=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(12).Trim()))
							{
								switch (line)
								{
									case "true": _alignoutput = AoTrue; break;
									case "tabs": _alignoutput = AoTabs; break;
								}
							}
						}
						else if (line.StartsWith("codepage=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(9).Trim())
								&& Int32.TryParse(line, out result) && result > -1)
							{
								_codepage = result;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="font"></param>
		/// <returns>a font that's roughly the size of Yata's default font</returns>
		internal static Font CreateDialogFont(Font font)
		{
			if (YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, font) > YataGraphics.hFontDefault)
			{
				FontStyle style = YataForm.getStyleStandard(font.FontFamily);
				float pts = font.SizeInPoints;
				var fontdialog = new Font(font.Name, pts, style);
				while (YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, fontdialog) > YataGraphics.hFontDefault)
				{
					fontdialog.Dispose();
					fontdialog = new Font(font.Name, pts -= 1F, YataForm.getStyleStandard(font.FontFamily));
				}
				return fontdialog;
			}
			return font;
		}
	}
}
