using System;
using System.IO;


namespace yata
{
	static class FileOutput
	{
		internal static void Write(YataGrid table)
		{
			if (Settings._alignoutput == Settings.AoFalse)
			{
				using (var sw = new StreamWriter(table.Fullpath))
				{
					sw.WriteLine(gs.TwodaVer);						// header ->

					sw.WriteLine(table._defaultval);				// default value ->

					string line = String.Empty;
					for (int i = 0; i != table.Fields.Length; ++i)	// col-fields ->
					{
						line += gs.Space + table.Fields[i];
					}
					sw.WriteLine(line);


					string val;
					for (int r = 0; r != table.RowCount; ++r)		// row-cells ->
					{
						line = String.Empty;

						for (int c = 0; c != table.ColCount; ++c)
						{
							if (c != 0)
								line += gs.Space;

							if (!String.IsNullOrEmpty(val = table[r,c].text)) // safety.
								line += val;
							else
								line += gs.Stars;
						}

						sw.WriteLine(line);
					}
				}
			}
			else
			{
				// find longest string-width in each col (incl/ colheads)
				var widths = new int[table.ColCount];

				// check cols ->
				int width, widthtest;
				for (int c = 0; c != table.ColCount; ++c)
				{
					width = 0;
					for (int r = 0; r != table.RowCount; ++r)
					{
						if ((widthtest = table[r,c].text.Length) > width)
							width = widthtest;
					}
					widths[c] = width;
				}

				// check colheads -> NOTE: There is one more col than colheads.
				for (int i = 0; i != table.Fields.Length; ++i)
				{
					if (widths[i + 1] < table.Fields[i].Length)
						widths[i + 1] = table.Fields[i].Length;
				}


				if (Settings._alignoutput == Settings.AoTrue)
				{
					using (var sw = new StreamWriter(table.Fullpath))
					{
						sw.WriteLine(gs.TwodaVer);						// header ->

						sw.WriteLine(table._defaultval);				// default value ->

						string line = String.Empty;
						for (int i = 0; i != table.Fields.Length; ++i)	// col-fields ->
						{
							if (i == 0)
							for (int j = 0; j != widths[0]; ++j)
								line += gs.Space;

							if (i != table.Fields.Length - 1)
								line += gs.Space + table.Fields[i].PadRight(widths[i + 1]);
							else
								line += gs.Space + table.Fields[i];
						}
						sw.WriteLine(line);


						string val;
						for (int r = 0; r != table.RowCount; ++r)		// row-cells ->
						{
							line = String.Empty;

							for (int c = 0; c != table.ColCount; ++c)
							{
								val = table[r,c].text;

								if (c != table.ColCount - 1)
								{
									if (!String.IsNullOrEmpty(val)) // safety.
										line += val.PadRight(widths[c] + 1);
									else
										line += gs.Stars.PadRight(widths[c] + 1);
								}
								else if (!String.IsNullOrEmpty(val)) // safety.
									line += val;
								else
									line += gs.Stars;
							}

							sw.WriteLine(line);
						}
					}
				}
				else if (Settings._alignoutput == Settings.AoTabs)
				{
					const int TabWidth = 4;

					using (var sw = new StreamWriter(table.Fullpath))
					{
						sw.WriteLine(gs.TwodaVer);						// header ->

						sw.WriteLine(table._defaultval);				// default value ->

						var tabstops = new int[table.ColCount];
						tabstops[0] = 0;
						for (int i = 1; i != widths.Length; ++i)
						{
							// to deter the position of the current tabstop [i]
							// add the start-pos of the preceeding tabstop +
							// the width of the longest preceeding field +
							// any remaining spaces that are required to bring
							// the total to a value that is divisible by TabWidth:
							// the result is the position of the current tabstop [i]

							tabstops[i] = tabstops[i - 1] + widths[i - 1] + TabWidth - widths[i - 1] % TabWidth;
						}


						string line = String.Empty;
						for (int i = 0; i != table.Fields.Length; ++i)	// col-fields ->
						{
							if (i == 0) // insert whitespace at the start of the colheads ->
							{
								int spaces = widths[0] + TabWidth - widths[0] % TabWidth;
								do
								{ line += "\t"; }
								while ((spaces -= TabWidth) > 0);
							}

							if (i != table.Fields.Length - 1)	// 2+ fields in row ->
							{
								line += table.Fields[i];

								width = tabstops[i + 2] - tabstops[i + 1] - table.Fields[i].Length;
								int tabs = width / TabWidth;
								if (table.Fields[i].Length % TabWidth != 0) ++tabs;

								while (tabs-- != 0)
									line += "\t";
							}
							else								// last field in row ->
								line += table.Fields[i];
						}
						sw.WriteLine(line);


						string val;
						for (int r = 0; r != table.RowCount; ++r)		// row-cells ->
						{
							line = String.Empty;

							for (int c = 0; c != table.ColCount; ++c)
							{
								val = table[r,c].text;

								if (String.IsNullOrEmpty(val)) // safety.
									val = gs.Stars;

								if (c != table.ColCount - 1)	// 2+ cells in row ->
								{
									line += val;

									width = tabstops[c + 1] - tabstops[c] - val.Length;
									int tabs = width / TabWidth;
									if (val.Length % TabWidth != 0) ++tabs;

									while (tabs-- != 0)
										line += "\t";
								}
								else							// last cell in row ->
									line += val;
							}

							sw.WriteLine(line);
						}
					}
				}
			}

			table.Watcher.Pfe = table.Fullpath;
			table.Watcher.BypassFileDeleted = false;
			table.Watcher.BypassFileChanged = true;
		}
	}
}
