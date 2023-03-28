using System;
using System.Drawing;
using System.IO;
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

		// strings in the Colors.Cfg file
		// Table ->
		internal const string CFG_rowlines          = "rowlines=";
		internal const string CFG_rowa              = "rowa=";
		internal const string CFG_rowa_t            = "rowa_t=";
		internal const string CFG_rowb              = "rowb=";
		internal const string CFG_rowb_t            = "rowb_t=";
		internal const string CFG_rowdisableda      = "rowdisableda=";
		internal const string CFG_rowdisableda_t    = "rowdisableda_t=";
		internal const string CFG_rowdisabledb      = "rowdisabledb=";
		internal const string CFG_rowdisabledb_t    = "rowdisabledb_t=";
		internal const string CFG_rowcreated        = "rowcreated=";
		internal const string CFG_rowcreated_t      = "rowcreated_t=";

		// Frozen ->
		internal const string CFG_frozenlines       = "frozenlines=";
		internal const string CFG_frozen            = "frozen=";
		internal const string CFG_frozen_t          = "frozen_t=";
		internal const string CFG_frozenheadlines   = "frozenheadlines=";
		internal const string CFG_frozenhead        = "frozenhead=";
		internal const string CFG_frozenhead_t      = "frozenhead_t=";
		internal const string CFG_frozenidunsort    = "frozenidunsort=";
		internal const string CFG_frozenidunsort_t  = "frozenidunsort_t=";
		internal const string CFG_frozenheadgrada   = "frozenheadgrada";
		internal const string CFG_frozenheadgradb   = "frozenheadgradb";
		internal const string CFG_frozenidgrada     = "frozenidgrada";
		internal const string CFG_frozenidgradb     = "frozenidgradb";

		// Colhead ->
		internal const string CFG_colheadlines      = "colheadlines=";
		internal const string CFG_colhead           = "colhead=";
		internal const string CFG_colhead_t         = "colhead_t=";
		internal const string CFG_colheadsel_t      = "colheadsel_t=";
		internal const string CFG_colheadsize_t     = "colheadsize_t=";
		internal const string CFG_headsortasc_t     = "headsortasc_t=";
		internal const string CFG_headsortdes_t     = "headsortdes_t=";
		internal const string CFG_colheadgrada      = "colheadgrada";
		internal const string CFG_colheadgradb      = "colheadgradb";

		// Rowpanel ->
		internal const string CFG_rowpanellines     = "rowpanellines=";
		internal const string CFG_rowpanel          = "rowpanel=";
		internal const string CFG_rowpanel_t        = "rowpanel_t=";
		internal const string CFG_rowsel            = "rowsel=";
		internal const string CFG_rowsel_t          = "rowsel_t=";
		internal const string CFG_rowsubsel         = "rowsubsel=";
		internal const string CFG_rowsubsel_t       = "rowsubsel_t=";

		// Propanel ->
		internal const string CFG_propanellines     = "propanellines=";
		internal const string CFG_propanelborder    = "propanelborder=";
		internal const string CFG_propanel          = "propanel=";
		internal const string CFG_propanel_t        = "propanel_t=";
		internal const string CFG_propanelfrozen    = "propanelfrozen=";
		internal const string CFG_propanelfrozen_t  = "propanelfrozen_t=";
		internal const string CFG_propanelsel       = "propanelsel=";
		internal const string CFG_propanelsel_t     = "propanelsel_t=";

		// Cells ->
		internal const string CFG_cellselected      = "cellselected=";
		internal const string CFG_cellselected_t    = "cellselected_t=";
		internal const string CFG_cellloadchanged   = "cellloadchanged=";
		internal const string CFG_cellloadchanged_t = "cellloadchanged_t=";
		internal const string CFG_celldiffed        = "celldiffed=";
		internal const string CFG_celldiffed_t      = "celldiffed_t=";
		internal const string CFG_cellreplaced      = "cellreplaced=";
		internal const string CFG_cellreplaced_t    = "cellreplaced_t=";
		internal const string CFG_celledit          = "celledit=";
		internal const string CFG_celledit_t        = "celledit_t=";

		// Statusbar ->
		internal const string CFG_statusbar         = "statusbar=";
		internal const string CFG_statusbar_t       = "statusbar_t=";

		// default colors
		// Table ->
		internal static Color Def_rowlines          = SystemColors.ControlDark;
		internal static Color Def_rowa              = Color.AliceBlue;
		internal static Color Def_rowa_t            = SystemColors.ControlText;
		internal static Color Def_rowb              = Color.BlanchedAlmond;
		internal static Color Def_rowb_t            = SystemColors.ControlText;
		internal static Color Def_rowdisableda      = Color.LavenderBlush;
		internal static Color Def_rowdisableda_t    = SystemColors.ControlText;
		internal static Color Def_rowdisabledb      = Color.MistyRose;
		internal static Color Def_rowdisabledb_t    = SystemColors.ControlText;
		internal static Color Def_rowcreated        = Color.Gainsboro;
		internal static Color Def_rowcreated_t      = SystemColors.ControlText;

		// Frozen ->
		internal static Color Def_frozenlines       = SystemColors.ControlDark;
		internal static Color Def_frozen            = Color.OldLace;
		internal static Color Def_frozen_t          = SystemColors.ControlText;
		internal static Color Def_frozenheadlines   = SystemColors.ControlDark;
		internal static Color Def_frozenhead        = Color.Moccasin;
		internal static Color Def_frozenhead_t      = SystemColors.ControlText;
		internal static Color Def_frozenidunsort    = Color.LightCoral;
		internal static Color Def_frozenidunsort_t  = SystemColors.ControlText;
		internal static Color Def_frozenheadgrada   = Color.Cornsilk;
		internal static Color Def_frozenheadgradb   = Color.BurlyWood;
		internal static Color Def_frozenidgrada     = Color.LightCoral;
		internal static Color Def_frozenidgradb     = Color.Lavender;

		// Colhead ->
		internal static Color Def_colheadlines      = SystemColors.ControlDark;
		internal static Color Def_colhead           = Color.Thistle;
		internal static Color Def_colhead_t         = SystemColors.ControlText;
		internal static Color Def_colheadsel_t      = Color.White;
		internal static Color Def_colheadsize_t     = Color.DarkGray;
		internal static Color Def_headsortasc_t     = Color.SteelBlue;
		internal static Color Def_headsortdes_t     = Color.DarkGoldenrod;
		internal static Color Def_colheadgrada      = Color.Lavender;
		internal static Color Def_colheadgradb      = Color.MediumOrchid;

		// Rowpanel ->
		internal static Color Def_rowpanellines     = SystemColors.ControlDark;
		internal static Color Def_rowpanel          = Color.Azure;
		internal static Color Def_rowpanel_t        = SystemColors.ControlText;
		internal static Color Def_rowsel            = Color.PaleGreen;
		internal static Color Def_rowsel_t          = SystemColors.ControlText;
		internal static Color Def_rowsubsel         = Color.Honeydew;
		internal static Color Def_rowsubsel_t       = SystemColors.ControlText;

		// Propanel ->
		internal static Color Def_propanellines     = SystemColors.ControlDark;
		internal static Color Def_propanelborder    = SystemColors.ControlDarkDark;
		internal static Color Def_propanel          = Color.LightSteelBlue;
		internal static Color Def_propanel_t        = SystemColors.ControlText;
		internal static Color Def_propanelfrozen    = Color.LightGray;
		internal static Color Def_propanelfrozen_t  = SystemColors.ControlText;
		internal static Color Def_propanelsel       = Color.PaleGreen;
		internal static Color Def_propanelsel_t     = SystemColors.ControlText;

		// Cells ->
		internal static Color Def_cellselected      = Color.PaleGreen;
		internal static Color Def_cellselected_t    = SystemColors.ControlText;
		internal static Color Def_cellloadchanged   = Color.Pink;
		internal static Color Def_cellloadchanged_t = SystemColors.ControlText;
		internal static Color Def_celldiffed        = Color.Turquoise;
		internal static Color Def_celldiffed_t      = SystemColors.ControlText;
		internal static Color Def_cellreplaced      = Color.Goldenrod;
		internal static Color Def_cellreplaced_t    = SystemColors.ControlText;
		internal static Color Def_celledit          = Color.Crimson;
		internal static Color Def_celledit_t        = SystemColors.ControlText;

		// Statusbar ->
		internal static Color Def_statusbar         = Color.MintCream;
		internal static Color Def_statusbar_t       = SystemColors.ControlText;

		// colors brushes pens for the Yata controls (current)
		// Table ->
		internal static Pen   _rowlines          = new Pen(Def_rowlines);
		internal static Brush _rowa              = new SolidBrush(Def_rowa);
		internal static Color _rowa_t            = Def_rowa_t;
		internal static Brush _rowb              = new SolidBrush(Def_rowb);
		internal static Color _rowb_t            = Def_rowb_t;
		internal static Brush _rowdisableda      = new SolidBrush(Def_rowdisableda);
		internal static Color _rowdisableda_t    = Def_rowdisableda_t;
		internal static Brush _rowdisabledb      = new SolidBrush(Def_rowdisabledb);
		internal static Color _rowdisabledb_t    = Def_rowdisabledb_t;
		internal static Brush _rowcreated        = new SolidBrush(Def_rowcreated);
		internal static Color _rowcreated_t      = Def_rowcreated_t;

		// Frozen ->
		internal static Pen   _frozenlines       = new Pen(Def_frozenlines);			// default colors for the frozenpanel ->
		internal static Color _frozen            = Def_frozen;
		internal static Color _frozen_t          = Def_frozen_t;
		internal static Pen   _frozenheadlines   = new Pen(Def_frozenheadlines);
		internal static Color _frozenhead        = Def_frozenhead;
		internal static Color _frozenhead_t      = Def_frozenhead_t;
		internal static Color _frozenidunsort    = Def_frozenidunsort;
		internal static Color _frozenidunsort_t  = Def_frozenidunsort_t;
		internal static Color _frozenheadgrada   = Def_frozenheadgrada;
		internal static Color _frozenheadgradb   = Def_frozenheadgradb;
		internal static Color _frozenidgrada     = Def_frozenidgrada;
		internal static Color _frozenidgradb     = Def_frozenidgradb;

		// Colhead ->
		internal static Pen   _colheadlines      = new Pen(Def_colheadlines);			// default colors for the colhead ->
		internal static Color _colhead           = Def_colhead;
		internal static Color _colhead_t         = Def_colhead_t;
		internal static Color _colheadsel_t      = Def_colheadsel_t;
		internal static Color _colheadsize_t     = Def_colheadsize_t;
		internal static Color _headsortasc_t     = Def_headsortasc_t;
		internal static Color _headsortdes_t     = Def_headsortdes_t;
		internal static Color _colheadgrada      = Def_colheadgrada;
		internal static Color _colheadgradb      = Def_colheadgradb;

		// Rowpanel ->
		internal static Pen   _rowpanellines     = new Pen(Def_rowpanellines);			// default colors for the rowpanel ->
		internal static Color _rowpanel          = Def_rowpanel;
		internal static Color _rowpanel_t        = Def_rowpanel_t;
		internal static Brush _rowsel            = new SolidBrush(Def_rowsel);
		internal static Color _rowsel_t          = Def_rowsel_t;
		internal static Brush _rowsubsel         = new SolidBrush(Def_rowsubsel);
		internal static Color _rowsubsel_t       = Def_rowsubsel_t;

		// Propanel ->
		internal static Pen   _propanellines     = new Pen(Def_propanellines);			// default colors for the propanel ->
		internal static Pen   _propanelborder    = new Pen(Def_propanelborder);
		internal static Color _propanel          = Def_propanel;
		internal static Color _propanel_t        = Def_propanel_t;
		internal static Brush _propanelfrozen    = new SolidBrush(Def_propanelfrozen);
		internal static Color _propanelfrozen_t  = Def_propanelfrozen_t;
		internal static Brush _propanelsel       = new SolidBrush(Def_propanelsel);
		internal static Color _propanelsel_t     = Def_propanelsel_t;

		// Cells ->
		internal static Brush _cellselected      = new SolidBrush(Def_cellselected);	// default colors for special cells ->
		internal static Color _cellselected_t    = Def_cellselected_t;
		internal static Brush _cellloadchanged   = new SolidBrush(Def_cellloadchanged);
		internal static Color _cellloadchanged_t = Def_cellloadchanged_t;
		internal static Brush _celldiffed        = new SolidBrush(Def_celldiffed);
		internal static Color _celldiffed_t      = Def_celldiffed_t;
		internal static Brush _cellreplaced      = new SolidBrush(Def_cellreplaced);
		internal static Color _cellreplaced_t    = Def_cellreplaced_t;
		internal static Brush _celledit          = new SolidBrush(Def_celledit);
		internal static Color _celledit_t        = Def_celledit_t;

		// Statusbar ->
		internal static Brush _statusbar         = new SolidBrush(Def_statusbar);		// default colors for the statusbar ->
		internal static Color _statusbar_t       = Def_statusbar_t;


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
				ParseColorsFile(pfe);

			Yata.that.UpdateStatusbarTextColor(); // update all 'ToolStripStatusLabels'
		}

		/// <summary>
		/// Parses the Colors.Cfg file for color options.
		/// </summary>
		/// <param name="pfe"></param>
		/// <returns></returns>
		static void ParseColorsFile(string pfe)
		{
			using (var fs = File.OpenRead(pfe))
			using (var sr = new StreamReader(fs))
			{
				string line; Color color;
				while ((line = sr.ReadLine()) != null)
				{
					// Table ->
					if (line.StartsWith(CFG_rowlines, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowlines.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_rowlines.Color = color;
						}
					}
					else if (line.StartsWith(CFG_rowa, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowa.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_rowa as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_rowa_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowa_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_rowa_t = color;
						}
					}
					else if (line.StartsWith(CFG_rowb, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowb.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_rowb as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_rowb_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowb_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_rowb_t = color;
						}
					}
					else if (line.StartsWith(CFG_rowdisableda, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowdisableda.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_rowdisableda as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_rowdisableda_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowdisableda_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_rowdisableda_t = color;
						}
					}
					else if (line.StartsWith(CFG_rowdisabledb, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowdisabledb.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_rowdisabledb as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_rowdisabledb_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowdisabledb_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_rowdisabledb_t = color;
						}
					}
					else if (line.StartsWith(CFG_rowcreated, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowcreated.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_rowcreated as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_rowcreated_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowcreated_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_rowcreated_t = color;
						}
					}

					// Frozen ->
					else if (line.StartsWith(CFG_frozenlines, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_frozenlines.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_frozenlines.Color = color;
						}
					}
					else if (line.StartsWith(CFG_frozen, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_frozen.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_frozen = color;
						}
					}
					else if (line.StartsWith(CFG_frozen_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_frozen_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_frozen_t = color;
						}
					}
					else if (line.StartsWith(CFG_frozenheadlines, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_frozenheadlines.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_frozenheadlines.Color = color;
						}
					}
					else if (line.StartsWith(CFG_frozenhead, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_frozenhead.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_frozenhead = color;
						}
					}
					else if (line.StartsWith(CFG_frozenhead_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_frozenhead_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_frozenhead_t = color;
						}
					}
					else if (line.StartsWith(CFG_frozenidunsort, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_frozenidunsort.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_frozenidunsort = color;
						}
					}
					else if (line.StartsWith(CFG_frozenidunsort_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_frozenidunsort_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_frozenidunsort_t = color;
						}
					}
					else if (line.StartsWith(CFG_frozenheadgrada, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_frozenheadgrada.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_frozenheadgrada = color;
						}
					}
					else if (line.StartsWith(CFG_frozenheadgradb, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_frozenheadgradb.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_frozenheadgradb = color;
						}
					}
					else if (line.StartsWith(CFG_frozenidgrada, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_frozenidgrada.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_frozenidgrada = color;
						}
					}
					else if (line.StartsWith(CFG_frozenidgradb, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_frozenidgradb.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_frozenidgradb = color;
						}
					}

					// Colhead ->
					else if (line.StartsWith(CFG_colheadlines, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_colheadlines.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_colheadlines.Color = color;
						}
					}
					else if (line.StartsWith(CFG_colhead, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_colhead.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_colhead = color;
						}
					}
					else if (line.StartsWith(CFG_colhead_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_colhead_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_colhead_t = color;
						}
					}
					else if (line.StartsWith(CFG_colheadsel_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_colheadsel_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_colheadsel_t = color;
						}
					}
					else if (line.StartsWith(CFG_colheadsize_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_colheadsize_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_colheadsize_t = color;
						}
					}
					else if (line.StartsWith(CFG_headsortasc_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_headsortasc_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_headsortasc_t = color;
						}
					}
					else if (line.StartsWith(CFG_headsortdes_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_headsortdes_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_headsortdes_t = color;
						}
					}
					else if (line.StartsWith(CFG_colheadgrada, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_colheadgrada.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_colheadgrada = color;
						}
					}
					else if (line.StartsWith(CFG_colheadgradb, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_colheadgradb.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_colheadgradb = color;
						}
					}

					// Rowpanel ->
					else if (line.StartsWith(CFG_rowpanellines, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowpanellines.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_rowpanellines.Color = color;
						}
					}
					else if (line.StartsWith(CFG_rowpanel, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowpanel.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_rowpanel = color;
						}
					}
					else if (line.StartsWith(CFG_rowpanel_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowpanel_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_rowpanel_t = color;
						}
					}
					else if (line.StartsWith(CFG_rowsel, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowsel.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_rowsel as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_rowsel_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowsel_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_rowsel_t = color;
						}
					}
					else if (line.StartsWith(CFG_rowsubsel, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowsubsel.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_rowsubsel as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_rowsubsel_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_rowsubsel_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_rowsubsel_t = color;
						}
					}

					// Propanel ->
					else if (line.StartsWith(CFG_propanellines, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_propanellines.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_propanellines.Color = color;
						}
					}
					else if (line.StartsWith(CFG_propanelborder, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_propanelborder.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_propanelborder.Color = color;
						}
					}
					else if (line.StartsWith(CFG_propanel, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_propanel.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_propanel = color;
						}
					}
					else if (line.StartsWith(CFG_propanel_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_propanel_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_propanel_t = color;
						}
					}
					else if (line.StartsWith(CFG_propanelfrozen, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_propanelfrozen.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_propanelfrozen as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_propanelfrozen_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_propanelfrozen_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_propanelfrozen_t = color;
						}
					}
					else if (line.StartsWith(CFG_propanelsel, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_propanelsel.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_propanelsel as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_propanelsel_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_propanelsel_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_propanelsel_t = color;
						}
					}

					// Cells ->
					else if (line.StartsWith(CFG_cellselected, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_cellselected.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_cellselected as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_cellselected_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_cellselected_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_cellselected_t = color;
						}
					}
					else if (line.StartsWith(CFG_cellloadchanged, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_cellloadchanged.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_cellloadchanged as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_cellloadchanged_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_cellloadchanged_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_cellloadchanged_t = color;
						}
					}
					else if (line.StartsWith(CFG_celldiffed, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_celldiffed.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_celldiffed as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_celldiffed_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_celldiffed_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_celldiffed_t = color;
						}
					}
					else if (line.StartsWith(CFG_cellreplaced, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_cellreplaced.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_cellreplaced as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_cellreplaced_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_cellreplaced_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_cellreplaced_t = color;
						}
					}
					else if (line.StartsWith(CFG_celledit, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_celledit.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_celledit as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_celledit_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_celledit_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_celledit_t = color;
						}
					}

					// Statusbar ->
					else if (line.StartsWith(CFG_statusbar, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_statusbar.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							(_statusbar as SolidBrush).Color = color;
						}
					}
					else if (line.StartsWith(CFG_statusbar_t, StringComparison.Ordinal))
					{
						if ((line = line.Substring(CFG_statusbar_t.Length).Trim()).Length != 0
							&& (color = ParseColor(line)) != Color.Empty)
						{
							_statusbar_t = color;
						}
					}
				}
			}
		}

		/// <summary>
		/// Parses a specified r,g,b string into a <c>Color</c>.
		/// </summary>
		/// <param name="rgb">eg. "255,255,255"</param>
		/// <returns>a valid <c>Color</c> else <c>Color.Empty</c> if
		/// <paramref name="rgb"/> fails to parse correctly</returns>
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
	}
}
