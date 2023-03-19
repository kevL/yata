﻿using System;
using System.Drawing;
using System.IO;
//using System.Reflection;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A static class for reading and interpreting the Colors.Cfg file.
	/// </summary>
	static class ColorOptions
	{
		#region Fields (static)
		internal const string FE = "colors.cfg";

		internal const string CFG_tabletext    = "tabletext=";
		internal const string CFG_rowa         = "rowa=";
		internal const string CFG_rowb         = "rowb=";
		internal const string CFG_rowdisableda = "rowdisableda=";
		internal const string CFG_rowdisabledb = "rowdisabledb=";
		internal const string CFG_frozentext   = "frozentext=";
		internal const string CFG_frozen       = "frozen=";
		internal const string CFG_frozenhead   = "frozenhead=";
		internal const string CFG_colhead      = "colhead=";
		internal const string CFG_rowpaneltext = "rowpaneltext=";
		internal const string CFG_rowpanel     = "rowpanel=";
		internal const string CFG_propaneltext = "propaneltext=";
		internal const string CFG_propanel     = "propanel=";
		internal const string CFG_statusbar    = "statusbar=";

		internal static Color _tabletext    = SystemColors.ControlText;			// default colors for the tablegrid ->
		internal static Brush _rowa         = new SolidBrush(Color.AliceBlue);
		internal static Brush _rowb         = new SolidBrush(Color.BlanchedAlmond);
		internal static Brush _rowdisableda = new SolidBrush(Color.LavenderBlush);
		internal static Brush _rowdisabledb = new SolidBrush(Color.MistyRose);

		internal static Color _frozentext   = SystemColors.ControlText;			// default colors for the frozenpanel ->
		internal static Color _frozen       = Color.OldLace;
		internal static Color _frozenhead   = Color.Moccasin;

		internal static Color _colhead      = Color.Thistle;					// default colors for the colhead

		internal static Color _rowpaneltext = SystemColors.ControlText;			// default colors for the rowpanel ->
		internal static Color _rowpanel     = Color.Azure;

		internal static Color _propaneltext = SystemColors.ControlText;			// default colors for the propanel ->
		internal static Color _propanel     = Color.LightSteelBlue;

		internal static Brush _statusbar    = new SolidBrush(Color.MintCream);	// default color for the statusbar


//		internal static Color _grid_backcolor = ;
		#endregion Fields (static)


		#region Methods (static)
		/// <summary>
		/// Scans the Colors.Cfg file for color options.
		/// </summary>
		internal static void ScanSettings()
		{
			string pfe = Path.Combine(Application.StartupPath, FE);
			if (File.Exists(pfe))
			{
				using (var fs = File.OpenRead(pfe))
				using (var sr = new StreamReader(fs))
				{
					string line; Color color;
					while ((line = sr.ReadLine()) != null)
					{
						if (line.StartsWith(CFG_tabletext, StringComparison.Ordinal))
						{
							if ((line = line.Substring(10).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_tabletext = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_tabletext = color;
							}
						}
						else if (line.StartsWith(CFG_rowa, StringComparison.Ordinal))
						{
							if ((line = line.Substring(5).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_rowa = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									_rowa = new SolidBrush(color);
							}
						}
						else if (line.StartsWith(CFG_rowb, StringComparison.Ordinal))
						{
							if ((line = line.Substring(5).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_rowb = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									_rowb = new SolidBrush(color);
							}
						}
						else if (line.StartsWith(CFG_rowdisableda, StringComparison.Ordinal))
						{
							if ((line = line.Substring(13).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_rowdisableda = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									_rowdisableda = new SolidBrush(color);
							}
						}
						else if (line.StartsWith(CFG_rowdisabledb, StringComparison.Ordinal))
						{
							if ((line = line.Substring(13).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_rowdisabledb = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									_rowdisabledb = new SolidBrush(color);
							}
						}
						else if (line.StartsWith(CFG_frozentext, StringComparison.Ordinal))
						{
							if ((line = line.Substring(11).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_frozentext = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozentext = color;
							}
						}
						else if (line.StartsWith(CFG_frozen, StringComparison.Ordinal))
						{
							if ((line = line.Substring(7).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_frozen = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozen = color;
							}
						}
						else if (line.StartsWith(CFG_frozenhead, StringComparison.Ordinal))
						{
							if ((line = line.Substring(11).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_frozenhead = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozenhead = color;
							}
						}
						else if (line.StartsWith(CFG_colhead, StringComparison.Ordinal))
						{
							if ((line = line.Substring(8).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_colhead = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_colhead = color;
							}
						}
						else if (line.StartsWith(CFG_rowpaneltext, StringComparison.Ordinal))
						{
							if ((line = line.Substring(13).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_rowpaneltext = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_rowpaneltext = color;
							}
						}
						else if (line.StartsWith(CFG_rowpanel, StringComparison.Ordinal))
						{
							if ((line = line.Substring(9).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_rowpanel = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_rowpanel = color;
							}
						}
						else if (line.StartsWith(CFG_propaneltext, StringComparison.Ordinal))
						{
							if ((line = line.Substring(13).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_propaneltext = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_propaneltext = color;
							}
						}
						else if (line.StartsWith(CFG_propanel, StringComparison.Ordinal))
						{
							if ((line = line.Substring(9).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_propanel = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_propanel = color;
							}
						}
						else if (line.StartsWith(CFG_statusbar, StringComparison.Ordinal))
						{
							if ((line = line.Substring(10).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_statusbar = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									_statusbar = new SolidBrush(color);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Parses a given r,g,b string into a <c>Color</c>.
		/// </summary>
		/// <param name="rgb"></param>
		/// <returns></returns>
		static Color ParseColor(string rgb)
		{
			string test;
			int r,g, result;

			int i = rgb.IndexOf(',');
			if (i != -1)
			{
				if ((test = rgb.Substring(0, i).Trim()).Length != 0
					&& Int32.TryParse(test, out result)
					&& result > -1 && result < 256)
				{
					r = result;

					int j = rgb.IndexOf(',', i + 1);
					if (j != -1)
					{
						if ((test = rgb.Substring(i + 1, j - i - 1).Trim()).Length != 0
							&& Int32.TryParse(test, out result)
							&& result > -1 && result < 256)
						{
							g = result;

							if ((test = rgb.Substring(j + 1).Trim()).Length != 0
								&& Int32.TryParse(test, out result)
								&& result > -1 && result < 256)
							{
								return Color.FromArgb(r,g, result);
							}
						}
					}
				}
			}
			return Color.Empty;
		}
		#endregion Methods (static)


/*		#region options (static)
		/// <summary>
		/// An array of all <c><see cref="options"/></c> <c>strings</c> that are
		/// recognized in the Colors.Cfg file.
		/// </summary>
		/// <remarks>Update if options are added to Yata.</remarks>
		internal static string[] options;

		/// <summary>
		/// The count of options in <c><see cref="options"/></c>.
		/// </summary>
		/// <remarks>Update if options are added to Yata.</remarks>
		internal const int ids = 14;

		/// <summary>
		/// Creates an array of all <c><see cref="options"/></c> <c>strings</c>
		/// that are recognized in the Colors.Cfg file.
		/// </summary>
		/// <remarks>Update if options are added to Yata.</remarks>
		internal static void CreateOptions()
		{
			options = new string[ids];

			int i = -1;
			options[++i] = CFG_tabletext;
			options[++i] = CFG_rowa;
			options[++i] = CFG_rowb;
			options[++i] = CFG_rowdisableda;
			options[++i] = CFG_rowdisabledb;
			options[++i] = CFG_frozentext;
			options[++i] = CFG_frozen;
			options[++i] = CFG_frozenhead;
			options[++i] = CFG_colhead;
			options[++i] = CFG_rowpaneltext;
			options[++i] = CFG_rowpanel;
			options[++i] = CFG_propaneltext;
			options[++i] = CFG_propanel;
			options[++i] = CFG_statusbar;
		}
		#endregion options (static) */
	}
}
