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
		/// <remarks>These routines require that <paramref name="table"/> has at
		/// least one entry in
		/// <c><see cref="YataGrid.Fields">YataGrid.Fields</see></c> and in
		/// <c><see cref="YataGrid.Cols">YataGrid.Cols</see></c>. Yata is coded
		/// to always maintain at least one <c>Field</c> - aka. colhead - one
		/// <c><see cref="YataGrid.Rows">YataGrid.Row</see></c>, and two
		/// <c>Cols</c> - ie. there shall always be a valid 2da-table ready to
		/// write ... except if the table is created by
		/// <c><see cref="Yata"/>.fileclick_Create()</c>. The latter is handled
		/// by the condition <c>(table.RowCount == 0)</c>.</remarks>
		internal static void Write(YataGrid table)
		{
			using (var sw = new StreamWriter(table.Fullpath))
			{
				sw.WriteLine(gs.TwodaVer);

				if (table._defaultval.Length != 0)
					sw.WriteLine(gs.Default + table._defaultval);
				else
					sw.WriteLine();


				if (table.RowCount != 0) // else isCreate
				{
					int f,r,c, fields;

					switch (Settings._alignoutput)
					{
						case Settings.AoFalse:
							fields = table.Fields.Length - 1;		// colabels ->
							for (f = 0; f != fields; ++f)
								sw.Write(gs.Space + table.Fields[f]);

							sw.WriteLine(gs.Space + table.Fields[f]);

							for (r = 0; r != table.RowCount; ++r)	// celltexts ->
							{
								for (c = 0; c != table.ColCount - 1; ++c)
									sw.Write(table[r,c].text + gs.Space);

								sw.WriteLine(table[r,c].text);
							}
							break;

						case Settings.AoTrue:
						case Settings.AoTabs:
						{
							// find longest string-width in each col (incl/ colheads)
							var widths = new int[table.ColCount];

							// check cols ->
							int width, widthtest;
							for (c = 0; c != table.ColCount; ++c)
							{
								width = 0;
								for (r = 0; r != table.RowCount; ++r)
								{
									if ((widthtest = table[r,c].text.Length) > width)
										width = widthtest;
								}
								widths[c] = width;
							}

							// check colheads -> NOTE: There is one more col than colheads.
							for (f = 0; f != table.Fields.Length; ++f)
							{
								if (widths[f + 1] < table.Fields[f].Length)
									widths[f + 1] = table.Fields[f].Length;
							}


							if (Settings._alignoutput == Settings.AoTrue)
							{
								sw.Write(new string(gs.Spacechar, widths[0]));	// colabels ->
								fields = table.Fields.Length - 1;
								for (f = 0; f != fields; ++f)
									sw.Write(gs.Space + table.Fields[f].PadRight(widths[f + 1]));

								sw.WriteLine(gs.Space + table.Fields[f]);

								for (r = 0; r != table.RowCount; ++r)			// celltexts ->
								{
									for (c = 0; c != table.ColCount - 1; ++c)
										sw.Write(table[r,c].text.PadRight(widths[c] + 1));

									sw.WriteLine(table[r,c].text);
								}
							}
							else // Settings.AoTabs
							{
								const int TabWidth = 4;

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


								int spaces = widths[0] + TabWidth - widths[0] % TabWidth; // insert whitespace at the start of the colheads ->
								do
								{ sw.Write("\t"); }
								while ((spaces -= TabWidth) > 0);

								string val;

								fields = table.Fields.Length - 1;
								for (f = 0; f != fields; ++f)			// colabels ->
								{
									val = table.Fields[f];
									sw.Write(val);

									width = tabstops[f + 2] - tabstops[f + 1] - val.Length;
									int tabs = width / TabWidth;
									if (val.Length % TabWidth != 0) ++tabs;

									while (tabs-- != 0)
										sw.Write("\t");
								}
								sw.WriteLine(table.Fields[f]);


								for (r = 0; r != table.RowCount; ++r)	// celltexts ->
								{
									for (c = 0; c != table.ColCount - 1; ++c)
									{
										val = table[r,c].text;
										sw.Write(val);

										width = tabstops[c + 1] - tabstops[c] - val.Length;
										int tabs = width / TabWidth;
										if (val.Length % TabWidth != 0) ++tabs;

										while (tabs-- != 0)
											sw.Write("\t");
									}
									sw.WriteLine(table[r,c].text);
								}
							}
							break;
						}

						case Settings.AoElectron:
							WriteElectron(table, sw);
							break;

//						default: break; // error
					}
				}
				else // is freshly Created
				{
					switch (Settings._alignoutput)
					{
						case Settings.AoFalse:
							sw.WriteLine(" "  + gs.DefaultColLabel);	// colabels
							sw.WriteLine("0 " + gs.Stars);				// celltexts
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
