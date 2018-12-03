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
		internal static Font _font;
		internal static Font _font2;
		internal static Font _font3;

		internal static readonly List<string> _dirpreset = new List<string>();
		internal static readonly List<string> _pathall   = new List<string>();

		internal static int _x = -1;
		internal static int _y = -1;
		internal static int _w = -1;
		internal static int _h = -1;

		internal static bool _strict;
		internal static bool _context;


		internal static void ScanSettings()
		{
			int result;

			string pathCfg = Path.Combine(Application.StartupPath, "settings.cfg");
			if (File.Exists(pathCfg))
			{
				using (var fs = File.OpenRead(pathCfg))
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
						else if (line.StartsWith("context=", StringComparison.InvariantCulture))
						{
							_context = (!String.IsNullOrEmpty(line = line.Substring(8).Trim())
								   && line == "static");
						}
					}
				}
			}
		}
	}
}
