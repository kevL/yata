using System;
using System.Globalization;
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
		/// <c><see cref="Options._alignoutput">Options._alignoutput</see></c>.
		/// <list type="bullet">
		/// <item><c><see cref="Options.AoFalse">Options.AoFalse</see></c></item>
		/// <item><c><see cref="Options.AoTrue">Options.AoTrue</see></c></item>
		/// <item><c><see cref="Options.AoTabs">Options.AoTabs</see></c></item>
		/// <item><c><see cref="Options.AoElectron">Options.AoElectron</see></c></item>
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
					int f,r,c, fields, width; int[] widths;

					switch (Options._alignoutput)
					{
						case Options.AoFalse:
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

						case Options.AoTrue:
							widths = GetWidths(table);

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
							break;

						case Options.AoTabs:
						{
							widths = GetWidths(table);

							const int TabWidth = 4;

							var tabstops = new int[table.ColCount];
							tabstops[0] = 0;
							for (c = 1; c != widths.Length; ++c)
							{
								// to deter the position of the current tabstop [i]
								// add the start-pos of the preceeding tabstop +
								// the width of the longest preceeding field +
								// any remaining spaces that are required to bring
								// the total to a value that is divisible by TabWidth:
								// the result is the position of the current tabstop [i]

								width = widths[c - 1];
								tabstops[c] = tabstops[c - 1] + width + TabWidth - width % TabWidth;
							}


							string val; int tabs;

							width = widths[0] + TabWidth - widths[0] % TabWidth; // insert whitespace at the start of the colheads ->
							do { sw.Write(gs.Tab); } while ((width -= TabWidth) > 0);

							fields = table.Fields.Length - 1;
							for (f = 0; f != fields; ++f)			// colabels ->
							{
								val = table.Fields[f];
								sw.Write(val);

								width = tabstops[f + 2] - tabstops[f + 1] - val.Length;
								tabs = width / TabWidth;
								if (val.Length % TabWidth != 0) ++tabs;

								while (tabs-- != 0) sw.Write(gs.Tab);
							}
							sw.WriteLine(table.Fields[f]);


							for (r = 0; r != table.RowCount; ++r)	// celltexts ->
							{
								for (c = 0; c != table.ColCount - 1; ++c)
								{
									val = table[r,c].text;
									sw.Write(val);

									width = tabstops[c + 1] - tabstops[c] - val.Length;
									tabs = width / TabWidth;
									if (val.Length % TabWidth != 0) ++tabs;

									while (tabs-- != 0) sw.Write(gs.Tab);
								}
								sw.WriteLine(table[r,c].text);
							}
							break;
						}

						case Options.AoElectron:
						{
							// writes to file using the Electron toolset routine -
							// this routine is based on OEIShared.IO.TwoDA.TwoDAFile.Save(string)

							width = 1; // at least 1-space indent (no reason, just because)
							r = table.RowCount;
							do { ++width; } while ((r /= 10) != 0);

							widths = GetWidthsElectron(table);
				
							sw.Write(new string(gs.Spacechar, width + 1)); // indent col-fields (add another space, just because)

							for (f = 0; f != table.Fields.Length; ++f)		// colabels ->
								sw.Write(table.Fields[f].PadLeft(widths[f]));

							sw.WriteLine();

							Row row;
							for (r = 0; r != table.RowCount; r++)			// celltexts ->
							{
								sw.Write(r.ToString(CultureInfo.InvariantCulture).PadLeft(width)); // DOES NOT WRITE THE VALUE IN THE ID-CELL

								row = table.Rows[r];
								for (f = 0; f != table.Fields.Length; ++f)
									sw.Write(row[f + 1].text.PadLeft(widths[f]));

								sw.WriteLine();
							}
							break;
						}

//						default: break; // error
					}
				}
				else // is freshly Created
				{
					switch (Options._alignoutput)
					{
						case Options.AoFalse:
							sw.WriteLine(" "  + gs.DefaultColLabel);	// colabels
							sw.WriteLine("0 " + gs.Stars);				// celltexts
							break;

						case Options.AoTrue:
							sw.WriteLine("  " + gs.DefaultColLabel);
							sw.WriteLine("0 " + gs.Stars);
							break;

						case Options.AoTabs:
							sw.WriteLine("\t"  + gs.DefaultColLabel);
							sw.WriteLine("0\t" + gs.Stars);
							break;

						case Options.AoElectron:
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
		/// Gets an array with the longest width of text in each
		/// <c><see cref="Col"/></c>.
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		static int[] GetWidths(YataGrid table)
		{
			// find longest string-width in each col (incl/ colheads)
			var widths = new int[table.ColCount];

			// check cols ->
			int width;
			for (int c = 0; c != table.ColCount; ++c)
			{
				widths[c] = 0;
				for (int r = 0; r != table.RowCount; ++r)
				{
					if ((width = table[r,c].text.Length) > widths[c])
						widths[c] = width;
				}
			}

			// check colheads -> NOTE: There is one more col than colheads.
			for (int f = 0; f != table.Fields.Length; ++f)
			{
				if ((width = table.Fields[f].Length) > widths[f + 1])
					widths[f + 1] = width;
			}

			return widths;
		}

		/// <summary>
		/// Gets an array with the width required (text + pad) for each
		/// <c><see cref="YataGrid.Fields">YataGrid.Field</see></c>.
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		static int[] GetWidthsElectron(YataGrid table)
		{
			var widths = new int[table.Fields.Length];

			int width;
			for (int f = 0; f != table.Fields.Length; ++f)
			{
				widths[f] = table.Fields[f].Length + 5; // why +5 ... just because

				for (int r = 0; r != table.RowCount; ++r)
				{
					// don't do the length-comparison with +5 accounted for, just because
					if ((width = table[r, f + 1].text.Length + 1) > widths[f])
						widths[f] = width + 5;
				}
			}
			return widths;
		}
	}
}
