using System;
using System.IO;


namespace yata
{
	/// <summary>
	/// Static class that writes 2da-files.
	/// </summary>
	static class FileOutput
	{
		/// <summary>
		/// Writes a 2da-file to
		/// <c><see cref="YataGrid.Fullpath">YataGrid.Fullpath</see></c> based
		/// on user's
		/// <c><see cref="Settings._alignoutput">Settings._alignoutput</see></c>.
		/// <list type="bullet">
		/// <item><c><see cref="Settings.AoFalse">Settings.AoFalse</see></c></item>
		/// <item><c><see cref="Settings.AoTrue">Settings.AoTrue</see></c></item>
		/// <item><c><see cref="Settings.AoTabs">Settings.AoTabs</see></c></item>
		/// <item><c><see cref="Settings.AoElectron">Settings.AoElectron</see></c></item>
		/// </list>
		/// </summary>
		/// <param name="table">a <c><see cref="YataGrid"/> to write the data
		/// for</c></param>
		internal static void Write(YataGrid table)
		{
			if (table.RowCount != 0) // else isCreate
			{
				switch (Settings._alignoutput)
				{
					case Settings.AoFalse:
						using (var sw = new StreamWriter(table.Fullpath))
						{
							sw.WriteLine(gs.TwodaVer);							// header ->

							if (table._defaultval.Length != 0)
								sw.WriteLine(gs.Default + table._defaultval);	// default value ->
							else
								sw.WriteLine();

							string line = String.Empty;
							for (int i = 0; i != table.Fields.Length; ++i)		// col-fields ->
							{
								line += gs.Space + table.Fields[i];
							}
							sw.WriteLine(line);


							string val;
							for (int r = 0; r != table.RowCount; ++r)			// row-cells ->
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
						break;

					case Settings.AoTrue:
					case Settings.AoTabs:
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
								sw.WriteLine(gs.TwodaVer);							// header ->

								if (table._defaultval.Length != 0)
									sw.WriteLine(gs.Default + table._defaultval);	// default value ->
								else
									sw.WriteLine();

								string line = String.Empty;
								for (int i = 0; i != table.Fields.Length; ++i)		// col-fields ->
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
								for (int r = 0; r != table.RowCount; ++r)			// row-cells ->
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
						else // Settings.AoTabs
						{
							const int TabWidth = 4;

							using (var sw = new StreamWriter(table.Fullpath))
							{
								sw.WriteLine(gs.TwodaVer);							// header ->

								if (table._defaultval.Length != 0)
									sw.WriteLine(gs.Default + table._defaultval);	// default value ->
								else
									sw.WriteLine();

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
								for (int i = 0; i != table.Fields.Length; ++i)		// col-fields ->
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
								for (int r = 0; r != table.RowCount; ++r)			// row-cells ->
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
						break;
					}

					case Settings.AoElectron:
						using (var sw = new StreamWriter(table.Fullpath))
							WriteElectron(table, sw);

						break;

//					default: break; // error
				}
			}
			else // is freshly Created
			{
				using (var sw = new StreamWriter(table.Fullpath))
				{
					sw.WriteLine(gs.TwodaVer);							// header
					sw.WriteLine();										// default value

					switch (Settings._alignoutput)
					{
						case Settings.AoFalse:
							sw.WriteLine(" "  + gs.DefaultColLabel);	// col-fields
							sw.WriteLine("0 " + gs.Stars);				// row-cells
							break;

						case Settings.AoTrue:
							sw.WriteLine("  " + gs.DefaultColLabel);
							sw.WriteLine("0 " + gs.Stars);
							break;

						case Settings.AoTabs:
							sw.WriteLine("\t"  + gs.DefaultColLabel);
							sw.WriteLine("0\t" + gs.Stars);
							break;

						case Settings.AoElectron:
							sw.WriteLine("        " + gs.DefaultColLabel);
							sw.WriteLine(" 0      " + gs.Stars);
							break;
					}
				}
			}

			if (File.Exists(table.Fullpath)) // TODO: else Error (Exception was likely thrown in the save routine)
				table.Lastwrite = File.GetLastWriteTime(table.Fullpath);
		}


		/// <summary>
		/// Writes a specified <c><see cref="YataGrid"/></c> to file using the
		/// Electron toolset routine.
		/// </summary>
		/// <param name="table"></param>
		/// <param name="sw"></param>
		/// <remarks>The routine is based on
		/// <c>OEIShared.IO.TwoDA.TwoDAFile.Save(string)</c>.</remarks>
		static void WriteElectron(YataGrid table, TextWriter sw)
		{
			sw.WriteLine(gs.TwodaVer);							// header ->

			if (table._defaultval.Length != 0)
				sw.WriteLine(gs.Default + table._defaultval);	// default value ->
			else
				sw.WriteLine();


			int width_idcol = 1; // at least 1-space indent (no reason, just because)
			int rCount = table.RowCount;
			do { ++width_idcol; } while ((rCount /= 10) != 0);

			int[] widths = GetWidths(table);

			sw.Write(new string(' ', width_idcol + 1)); // indent col-fields (add another space, just because)

			for (int f = 0; f != table.Fields.Length; ++f)		// col-fields ->
				sw.Write(table.Fields[f].PadLeft(widths[f]));

			sw.WriteLine();

			Row row;
			for (int r = 0; r != table.RowCount; r++)			// row-cells ->
			{
				sw.Write(r.ToString().PadLeft(width_idcol)); // DOES NOT WRITE THE VALUE IN THE ID-CELL

				row = table.Rows[r];
				for (int f = 0; f != table.Fields.Length; ++f)
					sw.Write(row[f + 1].text.PadLeft(widths[f], ' '));

				sw.WriteLine();
			}
		}

		/// <summary>
		/// Gets the total length required for each <c><see cref="Col"/></c>.
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		static int[] GetWidths(YataGrid table)
		{
			var widths = new int[table.ColCount - 1];

			int width, widthtest;
			for (int f = 0; f != table.Fields.Length; ++f)
			{
				width = table.Fields[f].Length + 5; // why +5 ... just because

				for (int r = 0; r != table.RowCount; ++r)
				{
					if ((widthtest = table[r, f + 1].text.Length + 1) > width) // don't do the length-comparison with +5 accounted for, just because)
						width = widthtest + 5;
				}
				widths[f] = width;
			}
			return widths;
		}
	}
}
