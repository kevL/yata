using System;
using System.IO;


namespace yata
{
	static class FileOutput
	{
		internal static void Write(YataGrid table)
		{
			if (!Settings._alignoutput)
			{
				using (var sw = new StreamWriter(table.Fullpath))
				{
					sw.WriteLine("2DA V2.0");						// header ->
					sw.WriteLine();

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


				using (var sw = new StreamWriter(table.Fullpath))
				{
					sw.WriteLine("2DA V2.0");						// header ->
					sw.WriteLine();

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

			table.Watcher.Pfe = table.Fullpath;
			table.Watcher.BypassFileDeleted = false;
			table.Watcher.BypassFileChanged = true;
		}
	}
}
