using System;
using System.Drawing;
using System.IO;
using System.Reflection;
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

		internal static Brush _statusbar    = Brushes.MintCream;		// default color for the statusbar

		internal static Color _tabletext    = SystemColors.ControlText;	// default colors for the tablegrid ->
		internal static Brush _rowa         = Brushes.AliceBlue;
		internal static Brush _rowb         = Brushes.BlanchedAlmond;
		internal static Brush _rowdisableda = Brushes.LavenderBlush;
		internal static Brush _rowdisabledb = Brushes.MistyRose;

		internal static Color _frozentext   = SystemColors.ControlText;	// default colors for the frozenpanel ->
		internal static Color _frozen       = Color.OldLace;

		internal static Color _propaneltext = SystemColors.ControlText;	// default colors for the propanel ->
		internal static Color _propanel     = Color.LightSteelBlue;

		internal static Color _rowpaneltext = SystemColors.ControlText;	// default colors for the rowpanel ->
		internal static Color _rowpanel     = Color.Azure;

		internal static Color _colhead      = Color.Thistle;			// default colors for the colheads ->
		internal static Color _frozenhead   = Color.Moccasin;
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
					string line;
					while ((line = sr.ReadLine()) != null)
					{
						if (line.StartsWith("statusbar=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(10).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Brushes).GetProperty(line);
								if (pi != null)
								{
									_statusbar = pi.GetValue(null,null) as Brush;
								}
							}
						}
						else if (line.StartsWith("tabletext=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(10).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_tabletext = (Color)pi.GetValue(null,null);
								}
							}
						}
						else if (line.StartsWith("rowa=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(5).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Brushes).GetProperty(line);
								if (pi != null)
								{
									_rowa = pi.GetValue(null,null) as Brush;
								}
							}
						}
						else if (line.StartsWith("rowb=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(5).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Brushes).GetProperty(line);
								if (pi != null)
								{
									_rowb = pi.GetValue(null,null) as Brush;
								}
							}
						}
						else if (line.StartsWith("rowdisableda=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(13).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Brushes).GetProperty(line);
								if (pi != null)
								{
									_rowdisableda = pi.GetValue(null,null) as Brush;
								}
							}
						}
						else if (line.StartsWith("rowdisabledb=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(13).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Brushes).GetProperty(line);
								if (pi != null)
								{
									_rowdisabledb = pi.GetValue(null,null) as Brush;
								}
							}
						}
						else if (line.StartsWith("frozentext=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(11).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_frozentext = (Color)pi.GetValue(null,null);
								}
							}
						}
						else if (line.StartsWith("frozen=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(7).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_frozen = (Color)pi.GetValue(null,null);
								}
							}
						}
						else if (line.StartsWith("propanel=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(9).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_propanel = (Color)pi.GetValue(null,null);
								}
							}
						}
						else if (line.StartsWith("propaneltext=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(13).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_propaneltext = (Color)pi.GetValue(null,null);
								}
							}
						}
						else if (line.StartsWith("rowpanel=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(9).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_rowpanel = (Color)pi.GetValue(null,null);
								}
							}
						}
						else if (line.StartsWith("rowpaneltext=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(13).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_rowpaneltext = (Color)pi.GetValue(null,null);
								}
							}
						}
						else if (line.StartsWith("colhead=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(8).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_colhead = (Color)pi.GetValue(null,null);
								}
							}
						}
						else if (line.StartsWith("frozenhead=", StringComparison.Ordinal))
						{
							if ((line = line.Substring(11).Trim()).Length != 0)
							{
								PropertyInfo pi = typeof(Color).GetProperty(line);
								if (pi != null)
								{
									_frozenhead = (Color)pi.GetValue(null,null);
								}
							}
						}
					}
				}
			}
		}
		#endregion Methods (static)


		#region options (static)
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
			options[++i] = "colhead=";
			options[++i] = "frozen=";
			options[++i] = "frozenhead=";
			options[++i] = "frozentext=";
			options[++i] = "propanel=";
			options[++i] = "propaneltext=";
			options[++i] = "rowa=";
			options[++i] = "rowb=";
			options[++i] = "rowdisableda=";
			options[++i] = "rowdisabledb=";
			options[++i] = "rowpanel=";
			options[++i] = "rowpaneltext=";
			options[++i] = "statusbar=";
			options[++i] = "tabletext=";
		}
		#endregion options (static)
	}
}
