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

		// strings in Colors.Cfg
		// Table ->
//		internal const string CFG_tabletext       = "tabletext=";
		internal const string CFG_tablelines      = "tablelines=";
		internal const string CFG_rowa            = "rowa=";
		internal const string CFG_rowa_t          = "rowa_t=";
		internal const string CFG_rowb            = "rowb=";
		internal const string CFG_rowb_t          = "rowb_t=";
		internal const string CFG_rowdisableda    = "rowdisableda=";
		internal const string CFG_rowdisableda_t  = "rowdisableda_t=";
		internal const string CFG_rowdisabledb    = "rowdisabledb=";
		internal const string CFG_rowdisabledb_t  = "rowdisabledb_t=";
		internal const string CFG_rowcreated      = "rowcreated=";
		internal const string CFG_rowcreated_t    = "rowcreated_t=";

		// Frozen ->
		internal const string CFG_frozentext      = "frozentext=";
		internal const string CFG_frozenlines     = "frozenlines=";
		internal const string CFG_frozen          = "frozen=";
		internal const string CFG_frozenheadtext  = "frozenheadtext=";
		internal const string CFG_frozenheadlines = "frozenheadlines=";
		internal const string CFG_frozenhead      = "frozenhead=";
		internal const string CFG_frozenidunsort  = "frozenidunsort=";
		internal const string CFG_frozenheadgrada = "frozenheadgrada";
		internal const string CFG_frozenheadgradb = "frozenheadgradb";
		internal const string CFG_frozenidgrada   = "frozenidgrada";
		internal const string CFG_frozenidgradb   = "frozenidgradb";

		// Colhead ->
		internal const string CFG_colheadtext     = "colheadtext=";
		internal const string CFG_colheadtextsel  = "colheadtextsel=";
		internal const string CFG_colheadtextsize = "colheadtextsize=";
		internal const string CFG_colheadlines    = "colheadlines=";
		internal const string CFG_colhead         = "colhead=";
		internal const string CFG_headtextsortasc = "headtextsortasc=";
		internal const string CFG_headtextsortdes = "headtextsortdes=";
		internal const string CFG_colheadgrada    = "colheadgrada";
		internal const string CFG_colheadgradb    = "colheadgradb";

		// Rowpanel ->
		internal const string CFG_rowpaneltext    = "rowpaneltext=";
		internal const string CFG_rowpanellines   = "rowpanellines=";
		internal const string CFG_rowpanel        = "rowpanel=";
		internal const string CFG_rowsel          = "rowsel=";
		internal const string CFG_rowsubsel       = "rowsubsel=";

		// Propanel ->
		internal const string CFG_propaneltext    = "propaneltext=";
		internal const string CFG_propanellines   = "propanellines=";
		internal const string CFG_propanelborder  = "propanelborder=";
		internal const string CFG_propanel        = "propanel=";
		internal const string CFG_propanelfrozen  = "propanelfrozen=";
		internal const string CFG_propanelsel     = "propanelsel=";

		// Statusbar ->
		internal const string CFG_statusbartext   = "statusbartext=";
		internal const string CFG_statusbar       = "statusbar=";

		// Cells ->
		internal const string CFG_cellselected    = "cellselected=";
		internal const string CFG_cellloadchanged = "cellloadchanged=";
		internal const string CFG_celldiffed      = "celldiffed=";
		internal const string CFG_cellreplaced    = "cellreplaced=";
		internal const string CFG_celledit        = "celledit=";

		// color defaults
		// Table ->
//		internal static Color Def_tabletext       = SystemColors.ControlText;
		internal static Color Def_tablelines      = SystemColors.ControlDark;
		internal static Color Def_rowa            = Color.AliceBlue;
		internal static Color Def_rowa_t          = SystemColors.ControlText;
		internal static Color Def_rowb            = Color.BlanchedAlmond;
		internal static Color Def_rowb_t          = SystemColors.ControlText;
		internal static Color Def_rowdisableda    = Color.LavenderBlush;
		internal static Color Def_rowdisableda_t  = SystemColors.ControlText;
		internal static Color Def_rowdisabledb    = Color.MistyRose;
		internal static Color Def_rowdisabledb_t  = SystemColors.ControlText;
		internal static Color Def_rowcreated      = Color.Gainsboro;
		internal static Color Def_rowcreated_t    = SystemColors.ControlText;

		// Frozen ->
		internal static Color Def_frozentext      = SystemColors.ControlText;
		internal static Color Def_frozenlines     = SystemColors.ControlDark;
		internal static Color Def_frozen          = Color.OldLace;
		internal static Color Def_frozenheadtext  = SystemColors.ControlText;
		internal static Color Def_frozenheadlines = SystemColors.ControlDark;
		internal static Color Def_frozenhead      = Color.Moccasin;
		internal static Color Def_frozenidunsort  = Color.LightCoral;
		internal static Color Def_frozenheadgrada = Color.Cornsilk;
		internal static Color Def_frozenheadgradb = Color.BurlyWood;
		internal static Color Def_frozenidgrada   = Color.LightCoral;
		internal static Color Def_frozenidgradb   = Color.Lavender;

		// Colhead ->
		internal static Color Def_colheadtext     = SystemColors.ControlText;
		internal static Color Def_colheadtextsel  = Color.White;
		internal static Color Def_colheadtextsize = Color.DarkGray;
		internal static Color Def_colheadlines    = SystemColors.ControlDark;
		internal static Color Def_colhead         = Color.Thistle;
		internal static Color Def_headtextsortasc = Color.SteelBlue;
		internal static Color Def_headtextsortdes = Color.DarkGoldenrod;
		internal static Color Def_colheadgrada    = Color.Lavender;
		internal static Color Def_colheadgradb    = Color.MediumOrchid;

		// Rowpanel ->
		internal static Color Def_rowpaneltext    = SystemColors.ControlText;
		internal static Color Def_rowpanellines   = SystemColors.ControlDark;
		internal static Color Def_rowpanel        = Color.Azure;
		internal static Color Def_rowsel          = Color.PaleGreen;
		internal static Color Def_rowsubsel       = Color.Honeydew;

		// Propanel ->
		internal static Color Def_propaneltext    = SystemColors.ControlText;
		internal static Color Def_propanellines   = SystemColors.ControlDark;
		internal static Color Def_propanelborder  = SystemColors.ControlDarkDark;
		internal static Color Def_propanel        = Color.LightSteelBlue;
		internal static Color Def_propanelfrozen  = Color.LightGray;
		internal static Color Def_propanelsel     = Color.PaleGreen;

		// Statusbar ->
		internal static Color Def_statusbartext   = SystemColors.ControlText;
		internal static Color Def_statusbar       = Color.MintCream;

		// Cells ->
		internal static Color Def_cellselected    = Color.PaleGreen;
		internal static Color Def_cellloadchanged = Color.Pink;
		internal static Color Def_celldiffed      = Color.Turquoise;
		internal static Color Def_cellreplaced    = Color.Goldenrod;
		internal static Color Def_celledit        = Color.Crimson;

		// colors brushes pens for the Yata controls
		// Table ->
//		internal static Color _tabletext       = Def_tabletext;						// default colors for the tablegrid ->
		internal static Pen   _tablelines      = new Pen(Def_tablelines);
		internal static Brush _rowa            = new SolidBrush(Def_rowa);
		internal static Color _rowa_t          = Def_rowa_t;
		internal static Brush _rowb            = new SolidBrush(Def_rowb);
		internal static Color _rowb_t          = Def_rowb_t;
		internal static Brush _rowdisableda    = new SolidBrush(Def_rowdisableda);
		internal static Color _rowdisableda_t  = Def_rowdisableda_t;
		internal static Brush _rowdisabledb    = new SolidBrush(Def_rowdisabledb);
		internal static Color _rowdisabledb_t  = Def_rowdisabledb_t;
		internal static Brush _rowcreated      = new SolidBrush(Def_rowcreated);
		internal static Color _rowcreated_t    = Def_rowcreated_t;

		// Frozen ->
		internal static Color _frozentext      = Def_frozentext;					// default colors for the frozenpanel ->
		internal static Pen   _frozenlines     = new Pen(Def_frozenlines);
		internal static Color _frozen          = Def_frozen;
		internal static Color _frozenheadtext  = Def_frozenheadtext;
		internal static Pen   _frozenheadlines = new Pen(Def_frozenheadlines);
		internal static Color _frozenhead      = Def_frozenhead;
		internal static Color _frozenidunsort  = Def_frozenidunsort;
		internal static Color _frozenheadgrada = Def_frozenheadgrada;
		internal static Color _frozenheadgradb = Def_frozenheadgradb;
		internal static Color _frozenidgrada   = Def_frozenidgrada;
		internal static Color _frozenidgradb   = Def_frozenidgradb;

		// Colhead ->
		internal static Color _colheadtext     = Def_colheadtext;					// default colors for the colhead ->
		internal static Color _colheadtextsel  = Def_colheadtextsel;
		internal static Color _colheadtextsize = Def_colheadtextsize;
		internal static Pen   _colheadlines    = new Pen(Def_colheadlines);
		internal static Color _colhead         = Def_colhead;
		internal static Color _headtextsortasc = Def_headtextsortasc;
		internal static Color _headtextsortdes = Def_headtextsortdes;
		internal static Color _colheadgrada    = Def_colheadgrada;
		internal static Color _colheadgradb    = Def_colheadgradb;

		// Rowpanel ->
		internal static Color _rowpaneltext    = Def_rowpaneltext;					// default colors for the rowpanel ->
		internal static Pen   _rowpanellines   = new Pen(Def_rowpanellines);
		internal static Color _rowpanel        = Def_rowpanel;
		internal static Brush _rowsel          = new SolidBrush(Def_rowsel);
		internal static Brush _rowsubsel       = new SolidBrush(Def_rowsubsel);

		// Propanel ->
		internal static Color _propaneltext    = Def_propaneltext;					// default colors for the propanel ->
		internal static Pen   _propanellines   = new Pen(Def_propanellines);
		internal static Pen   _propanelborder  = new Pen(Def_propanelborder);
		internal static Color _propanel        = Def_propanel;
		internal static Brush _propanelfrozen  = new SolidBrush(Def_propanelfrozen);
		internal static Brush _propanelsel     = new SolidBrush(Def_propanelsel);

		// Statusbar ->
		internal static Color _statusbartext   = Def_statusbartext;					// default colors for the statusbar ->
		internal static Brush _statusbar       = new SolidBrush(Def_statusbar);

		// Cells ->
		internal static Brush _cellselected    = new SolidBrush(Def_cellselected);	// default colors for cell texts ->
		internal static Brush _cellloadchanged = new SolidBrush(Def_cellloadchanged);
		internal static Brush _celldiffed      = new SolidBrush(Def_celldiffed);
		internal static Brush _cellreplaced    = new SolidBrush(Def_cellreplaced);
		internal static Brush _celledit        = new SolidBrush(Def_celledit);


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
						// Table ->
/*						if (line.StartsWith(CFG_tabletext, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_tabletext.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_tabletext = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_tabletext = color;
							}
						} */
						if (line.StartsWith(CFG_tablelines, StringComparison.Ordinal))
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
						else if (line.StartsWith(CFG_rowa_t, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowa_t.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_rowa_t = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_rowa_t = color;
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
						else if (line.StartsWith(CFG_rowb_t, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowb_t.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_rowb_t = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_rowb_t = color;
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
						else if (line.StartsWith(CFG_rowdisableda_t, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowdisableda_t.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_rowdisableda_t = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_rowdisableda_t = color;
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
						else if (line.StartsWith(CFG_rowdisabledb_t, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowdisabledb_t.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_rowdisabledb_t = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_rowdisabledb_t = color;
							}
						}
						else if (line.StartsWith(CFG_rowcreated, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowcreated.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_rowcreated = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_rowcreated as SolidBrush).Color = color;
							}
						}
						else if (line.StartsWith(CFG_rowcreated_t, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowcreated_t.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_rowcreated_t = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_rowcreated_t = color;
							}
						}

						// Frozen ->
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
						else if (line.StartsWith(CFG_frozenheadgrada, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_frozenheadgrada.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_frozenheadgrada = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozenheadgrada = color;
							}
						}
						else if (line.StartsWith(CFG_frozenheadgradb, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_frozenheadgradb.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_frozenheadgradb = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozenheadgradb = color;
							}
						}
						else if (line.StartsWith(CFG_frozenidgrada, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_frozenidgrada.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_frozenidgrada = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozenidgrada = color;
							}
						}
						else if (line.StartsWith(CFG_frozenidgradb, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_frozenidgradb.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_frozenidgradb = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_frozenidgradb = color;
							}
						}

						// Colhead ->
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
						else if (line.StartsWith(CFG_headtextsortasc, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_headtextsortasc.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_headtextsortasc = (Color)pi.GetValue(null,null);

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
//									_headtextsortdes = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_headtextsortdes = color;
							}
						}
						else if (line.StartsWith(CFG_colheadgrada, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_colheadgrada.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_colheadgrada = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_colheadgrada = color;
							}
						}
						else if (line.StartsWith(CFG_colheadgradb, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_colheadgradb.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_colheadgradb = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_colheadgradb = color;
							}
						}

						// Rowpanel ->
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
						else if (line.StartsWith(CFG_rowsel, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowsel.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_rowsel = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_rowsel as SolidBrush).Color = color;
							}
						}
						else if (line.StartsWith(CFG_rowsubsel, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_rowsubsel.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_rowsubsel = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_rowsubsel as SolidBrush).Color = color;
							}
						}

						// Propanel ->
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
						else if (line.StartsWith(CFG_propanelfrozen, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_propanelfrozen.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_propanelfrozen = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_propanelfrozen as SolidBrush).Color = color;
							}
						}
						else if (line.StartsWith(CFG_propanelsel, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_propanelsel.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Brushes).GetProperty(line);
//								if (pi != null)
//									_propanelsel = pi.GetValue(null,null) as Brush;

								if ((color = ParseColor(line)) != Color.Empty)
									(_propanelsel as SolidBrush).Color = color;
							}
						}

						// Statusbar ->
						else if (line.StartsWith(CFG_statusbartext, StringComparison.Ordinal))
						{
							if ((line = line.Substring(CFG_statusbartext.Length).Trim()).Length != 0)
							{
//								PropertyInfo pi = typeof(Color).GetProperty(line);
//								if (pi != null)
//									_statusbartext = (Color)pi.GetValue(null,null);

								if ((color = ParseColor(line)) != Color.Empty)
									_statusbartext = color;
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

						// Cells ->
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
