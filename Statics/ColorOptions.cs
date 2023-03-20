using System;
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

		internal const string CFG_tabletext       = "tabletext=";
		internal const string CFG_tablelines      = "tablelines=";
		internal const string CFG_rowa            = "rowa=";
		internal const string CFG_rowb            = "rowb=";
		internal const string CFG_rowdisableda    = "rowdisableda=";
		internal const string CFG_rowdisabledb    = "rowdisabledb=";

		internal const string CFG_frozentext      = "frozentext=";
		internal const string CFG_frozenlines     = "frozenlines=";
		internal const string CFG_frozen          = "frozen=";
		internal const string CFG_frozenheadtext  = "frozenheadtext=";
		internal const string CFG_frozenheadlines = "frozenheadlines=";
		internal const string CFG_frozenhead      = "frozenhead=";
		internal const string CFG_frozenidunsort  = "frozenidunsort=";

		internal const string CFG_colheadtext     = "colheadtext=";
		internal const string CFG_colheadtextsel  = "colheadtextsel=";
		internal const string CFG_colheadtextsize = "colheadtextsize=";
		internal const string CFG_headtextsortasc = "headtextsortasc";
		internal const string CFG_headtextsortdes = "headtextsortdes";
		internal const string CFG_colheadlines    = "colheadlines=";
		internal const string CFG_colhead         = "colhead=";

		internal const string CFG_rowpaneltext    = "rowpaneltext=";
		internal const string CFG_rowpanellines   = "rowpanellines=";
		internal const string CFG_rowpanel        = "rowpanel=";

		internal const string CFG_propaneltext    = "propaneltext=";
		internal const string CFG_propanellines   = "propanellines=";
		internal const string CFG_propanelborder  = "propanelborder=";
		internal const string CFG_propanel        = "propanel=";

		internal const string CFG_statusbar       = "statusbar=";

		internal const string CFG_cellselected    = "cellselected=";
		internal const string CFG_cellloadchanged = "cellloadchanged=";
		internal const string CFG_celldiffed      = "celldiffed=";
		internal const string CFG_cellreplaced    = "cellreplaced=";
		internal const string CFG_celledit        = "celledit=";

		// TODO: PropanelFrozen row(s)
		// TODO: row selected + subselected


		internal static Color _tabletext       = SystemColors.ControlText;			// default colors for the tablegrid ->
		internal static Pen   _tablelines      = new Pen(SystemColors.ControlDark);
		internal static Brush _rowa            = new SolidBrush(Color.AliceBlue);
		internal static Brush _rowb            = new SolidBrush(Color.BlanchedAlmond);
		internal static Brush _rowdisableda    = new SolidBrush(Color.LavenderBlush);
		internal static Brush _rowdisabledb    = new SolidBrush(Color.MistyRose);

		internal static Color _frozentext      = SystemColors.ControlText;			// default colors for the frozenpanel ->
		internal static Pen   _frozenlines     = new Pen(SystemColors.ControlDark);
		internal static Color _frozen          = Color.OldLace;
		internal static Color _frozenheadtext  = SystemColors.ControlText;
		internal static Pen   _frozenheadlines = new Pen(SystemColors.ControlDark);
		internal static Color _frozenhead      = Color.Moccasin;
		internal static Color _frozenidunsort  = Color.LightCoral;

		internal static Color _colheadtext     = SystemColors.ControlText;			// default colors for the colhead ->
		internal static Color _colheadtextsel  = Color.White;
		internal static Color _colheadtextsize = Color.DarkGray;
		internal static Color _headtextsortasc = Color.SteelBlue;
		internal static Color _headtextsortdes = Color.DarkGoldenrod;
		internal static Pen   _colheadlines    = new Pen(SystemColors.ControlDark);
		internal static Color _colhead         = Color.Thistle;

		internal static Color _rowpaneltext    = SystemColors.ControlText;			// default colors for the rowpanel ->
		internal static Pen   _rowpanellines   = new Pen(SystemColors.ControlDark);
		internal static Color _rowpanel        = Color.Azure;

		internal static Color _propaneltext    = SystemColors.ControlText;			// default colors for the propanel ->
		internal static Pen   _propanellines   = new Pen(SystemColors.ControlDark);
		internal static Pen   _propanelborder  = new Pen(SystemColors.ControlDarkDark);
		internal static Color _propanel        = Color.LightSteelBlue;

		internal static Brush _statusbar       = new SolidBrush(Color.MintCream);	// default color for the statusbar

		internal static Brush _cellselected    = new SolidBrush(Color.PaleGreen);
		internal static Brush _cellloadchanged = new SolidBrush(Color.Pink);
		internal static Brush _celldiffed      = new SolidBrush(Color.Turquoise);
		internal static Brush _cellreplaced    = new SolidBrush(Color.Goldenrod);
		internal static Brush _celledit        = new SolidBrush(Color.Crimson);


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
							if ((line = line.Substring(CFG_tabletext.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_tabletext = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_tabletext = color;
							}
						}
						else if (line.StartsWith(CFG_tablelines, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_tablelines.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Pencils).GetProperty(line);
//								if (pi != null)
//									_tablelines.Color = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_tablelines.Color = color;
							}
						}
						else if (line.StartsWith(CFG_rowa, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowa.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_rowa = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_rowa as SolidBrush).Color = color;
							}
						}
						else if (line.StartsWith(CFG_rowb, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowb.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_rowb = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_rowb as SolidBrush).Color = color;
							}
						}
						else if (line.StartsWith(CFG_rowdisableda, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowdisableda.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_rowdisableda = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_rowdisableda as SolidBrush).Color = color;
							}
						}
						else if (line.StartsWith(CFG_rowdisabledb, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowdisabledb.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_rowdisabledb = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_rowdisabledb as SolidBrush).Color = color;
							}
						}
						else if (line.StartsWith(CFG_frozentext, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_frozentext.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_frozentext = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozentext = color;
							}
						}
						else if (line.StartsWith(CFG_frozenlines, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_frozenlines.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Pencils).GetProperty(line);
//								if (pi != null)
//									_frozenlines.Color = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozenlines.Color = color;
							}
						}
						else if (line.StartsWith(CFG_frozen, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_frozen.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_frozen = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozen = color;
							}
						}
						else if (line.StartsWith(CFG_frozenheadtext, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_frozenheadtext.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_frozenheadtext = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozenheadtext = color;
							}
						}
						else if (line.StartsWith(CFG_frozenheadlines, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_frozenheadlines.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Pencils).GetProperty(line);
//								if (pi != null)
//									_frozenheadlines.Color = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozenheadlines.Color = color;
							}
						}
						else if (line.StartsWith(CFG_frozenhead, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_frozenhead.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_frozenhead = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozenhead = color;
							}
						}
						else if (line.StartsWith(CFG_frozenidunsort, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_frozenidunsort.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_frozenidunsorted = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozenidunsort = color;
							}
						}
						else if (line.StartsWith(CFG_colheadtext, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_colheadtext.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_colheadtext = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_colheadtext = color;
							}
						}
						else if (line.StartsWith(CFG_colheadtextsel, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_colheadtextsel.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_colheadtextselected = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_colheadtextsel = color;
							}
						}
						else if (line.StartsWith(CFG_colheadtextsize, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_colheadtextsize.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_colheadtextsized = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_colheadtextsize = color;
							}
						}
						else if (line.StartsWith(CFG_headtextsortasc, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_headtextsortasc.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_colheadtextsortasc = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_headtextsortasc = color;
							}
						}
						else if (line.StartsWith(CFG_headtextsortdes, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_headtextsortdes.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_colheadtextsortdes = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_headtextsortdes = color;
							}
						}
						else if (line.StartsWith(CFG_colheadlines, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_colheadlines.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_colheadlines = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_colheadlines.Color = color;
							}
						}
						else if (line.StartsWith(CFG_colhead, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_colhead.Length).Trim()).Length != 0)
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
							if ((line = line.Substring(CFG_rowpaneltext.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_rowpaneltext = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_rowpaneltext = color;
							}
						}
						else if (line.StartsWith(CFG_rowpanellines, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowpanellines.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_rowpanellines = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_rowpanellines.Color = color;
							}
						}
						else if (line.StartsWith(CFG_rowpanel, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowpanel.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_rowpanel = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_rowpanel = color;
							}
						}
						else if (line.StartsWith(CFG_propanellines, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_propanellines.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_propanellines = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_propanellines.Color = color;
							}
						}
						else if (line.StartsWith(CFG_propanelborder, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_propanelborder.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_propanelborder = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_propanelborder.Color = color;
							}
						}
						else if (line.StartsWith(CFG_propaneltext, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_propaneltext.Length).Trim()).Length != 0)
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
							if ((line = line.Substring(CFG_propanel.Length).Trim()).Length != 0)
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
							if ((line = line.Substring(CFG_statusbar.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_statusbar = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_statusbar as SolidBrush).Color = color;
							}
						}
						else if (line.StartsWith(CFG_cellselected, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_cellselected.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_cellselected = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_cellselected as SolidBrush).Color = color;
							}
						}
						else if (line.StartsWith(CFG_cellloadchanged, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_cellloadchanged.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_cellloadchanged = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_cellloadchanged as SolidBrush).Color = color;
							}
						}
						else if (line.StartsWith(CFG_celldiffed, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_celldiffed.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_celldiffed = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_celldiffed as SolidBrush).Color = color;
							}
						}
						else if (line.StartsWith(CFG_cellreplaced, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_cellreplaced.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_cellreplaced = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_cellreplaced as SolidBrush).Color = color;
							}
						}
						else if (line.StartsWith(CFG_celledit, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_celledit.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_celledit = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_celledit as SolidBrush).Color = color;
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
			string val;
			int r,g, result;

			int i = rgb.IndexOf(',');
			if (i != -1)
			{
				if ((val = rgb.Substring(0, i).Trim()).Length != 0
					&& Int32.TryParse(val, out result)
					&& result > -1 && result < 256)
				{
					r = result;

					int j = rgb.IndexOf(',', i + 1);
					if (j != -1)
					{
						if ((val = rgb.Substring(i + 1, j - i - 1).Trim()).Length != 0
							&& Int32.TryParse(val, out result)
							&& result > -1 && result < 256)
						{
							g = result;

							if ((val = rgb.Substring(j + 1).Trim()).Length != 0
								&& Int32.TryParse(val, out result)
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
