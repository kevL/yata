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
		internal const string FE = "settings.cfg";

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
		internal const int ids = 23;


		internal static Font _font;			// the grid's font
		internal static Font _fontdialog;	// the font to be used in all dialogs unless '_font2dialog' is valid.

		internal static Font _font2;		// menubar, ContextRow, ContextCell, ContextTab, statusbar
		internal static Font _font2dialog;	// all dialogs if valid.

		internal static Font _font3;		// propanel

		internal static Font _fontf;		// richtextboxes (preferably fixed-font)
		internal static Font _fontf_tb;		// textboxes (preferably fixed-font)

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
		internal static bool _autorder;

		internal static int _recent;
		internal static int _alignoutput;
		internal static int _codepage;

		internal static string _diff;
		internal static string _dialog;
		internal static string _dialogalt;


		internal const int AoFalse = 0; // Align_Output vals ->
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
									_fontf_tb = CreateDialogFont(_fontf);
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
								&& Int32.TryParse(line, out result)
								&& result > -1 && result < 65536)
							{
								_codepage = result;
							}
						}
						else if (line.StartsWith("autorder=", StringComparison.InvariantCulture))
						{
							_autorder = (!String.IsNullOrEmpty(line = line.Substring(9).Trim())
									 && line == "true");
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
			FontStyle style = YataForm.getStyleStandard(fontdialog.FontFamily);

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
		/// Creates an array of all <c><see cref="options"/></c> <c>strings</c>
		/// that are recognized in the Settings.Cfg file.
		/// </summary>
		/// <remarks>Update if options are added to Yata.</remarks>
		internal static void CreateOptions()
		{
			options = new string[ids];

			options[ 0] = "font=";
			options[ 1] = "font2=";
			options[ 2] = "font3=";
			options[ 3] = "fontf=";
			options[ 4] = "pathall=";
			options[ 5] = "dirpreset=";
			options[ 6] = "x=";
			options[ 7] = "y=";
			options[ 8] = "w=";
			options[ 9] = "h=";
			options[10] = "strict=";
			options[11] = "gradient=";
			options[12] = "context=";
			options[13] = "recent=";
			options[14] = "diff=";
			options[15] = "dialog=";
			options[16] = "dialogalt=";
			options[17] = "maximized=";
			options[18] = "instantgoto=";
			options[19] = "casesort=";
			options[20] = "alignoutput=";
			options[21] = "codepage=";
			options[22] = "autorder=";
		}


		/// <summary>
		/// Sets <c>Fonts</c> for a dialog.
		/// </summary>
		/// <param name="f">the dialog <c>Form</c></param>
		/// <param name="color"><c>true</c> to set the <c>TextBoxBase's</c>
		/// <c>BackColor</c> to the Yata-default.</param>
		internal static void SetFonts(Control f, bool color = true)
		{
			if (_font2dialog != null)
				f.Font = _font2dialog;
			else
				f.Font = _fontdialog;

			var dialog = f as YataDialog;
			foreach (var tbb in dialog._tbbs)
			{
				if (_fontf != null)
				{
					tbb.Font.Dispose();

					if (tbb is RichTextBox) tbb.Font = _fontf;
					else                    tbb.Font = _fontf_tb; // is TextBox
				}

				if (color)
					tbb.BackColor = Colors.TextboxBackground;
			}
		}

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
