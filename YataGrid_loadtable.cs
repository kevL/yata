using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Text;
//using System.Threading;
using System.Windows.Forms;


namespace yata
{
	// Various routines for loading a 2da-file.
	sealed partial class YataGrid
	{
		#region Fields (static)
		const int LINE_VERSION = 0;
		const int LINE_DEFAULT = 1;
		const int LINE_COLABEL = 2;

		internal const int LOADRESULT_FALSE   = 0;
		internal const int LOADRESULT_TRUE    = 1;
		internal const int LOADRESULT_CHANGED = 2;

		static int CodePage = -1;

		/// <summary>
		/// This <c>static bool</c> is set <c>true</c> when loading
		/// Crafting.2da, Spells.2da, Feat.2da, or Classes.2da causes the
		/// <c><see cref="Settings._pathall">Settings._pathall</see></c>
		/// directories to be groped.
		/// </summary>
		static bool Groped;

		static int _heightColheadCached;
		#endregion Fields (static)


		#region Fields
		bool _initFrozenLabels = true;

		/// <summary>
		/// A static <c>List</c> used to pass parsed row-fields from
		/// <c><see cref="LoadTable()">LoadTable()</see></c> to
		/// <c><see cref="CreateRows()">CreateRows()</see></c> and then is
		/// cleared when no longer needed.
		/// </summary>
		static readonly List<string[]> _rows = new List<string[]>();
		#endregion Fields


		#region Methods (static)
		/// <summary>
		/// Checks if <paramref name="codepage"/> is recognized by .NET.
		/// </summary>
		/// <param name="codepage"></param>
		/// <returns></returns>
		internal static bool CheckCodepage(int codepage)
		{
			EncodingInfo[] encs = Encoding.GetEncodings();
			for (int i = 0; i != encs.Length; ++i)
			{
				if (encs[i].CodePage == codepage)
					return true;
			}
			return false;
		}

		/// <summary>
		/// A generic warn-box if something goes wonky while loading a 2da-file.
		/// </summary>
		/// <param name="head">the warning</param>
		/// <param name="copy">copyable text</param>
		/// <returns>a <c>DialogResult</c>
		/// <list type="bullet">
		/// <item><c>Cancel</c> - abort load</item>
		/// <item><c>OK</c> - ignore further errors and try to load the 2da-file</item>
		/// <item><c>Retry</c> - check for next error</item>
		/// </list></returns>
		static DialogResult ShowLoadWarning(string head, string copy)
		{
			using (var ib = new Infobox(Infobox.Title_warni,
										head,
										copy,
										InfoboxType.Warn,
										InfoboxButtons.AbortLoadNext))
			{
				return ib.ShowDialog(Yata.that);
			}
		}

		/// <summary>
		/// Parses a single row of text out to its fields.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		internal static string[] ParseTableRow(string line)
		{
			var list  = new List<string>();
			var field = new List<char>();

			bool @add     = false;
			bool inQuotes = false;

			char c;

			int posLast = line.Length + 1;					// include an extra iteration to get the last field
			for (int pos = 0; pos != posLast; ++pos)
			{
				if (pos == line.Length)						// hit lineend -> add the last field
				{
					if (@add)
					{
						list.Add(new string(field.ToArray()));
					}
				}
				else
				{
					c = line[pos];

					if (c == '"' || inQuotes)				// start or continue quotation
					{
						inQuotes = (!inQuotes || c != '"');	// end quotation

						@add = true;
						field.Add(c);
					}
					else if (c != ' ' && c != '\t')			// any non-whitespace char (except double-quote)
//					else if (!Char.IsWhiteSpace(c))
					{
						@add = true;
						field.Add(c);
					}
					else if (@add)							// hit a space or tab
					{
						@add = false;
						list.Add(new string(field.ToArray()));

						field.Clear();
					}
				}
			}
			return list.ToArray();
		}
		#endregion Methods (static)


		#region Methods (load)
		/// <summary>
		/// Tries to load a 2da-file.
		/// </summary>
		/// <returns>
		/// <list type="bullet">
		/// <item><c><see cref="LOADRESULT_FALSE"/></c></item>
		/// <item><c><see cref="LOADRESULT_TRUE"/></c></item>
		/// <item><c><see cref="LOADRESULT_CHANGED"/></c></item>
		/// </list></returns>
		internal int LoadTable()
		{
/*			const string test = "The 2da-file contains double-quotes. Although that can be"
							  + " valid in a 2da-file Yata's 2da Info-grope is not coded to cope."
							  + " Format the 2da-file (in a texteditor) to not use double-quotes"
							  + " if you want to access it for 2da Info.";
			using (var ib = new Infobox(Infobox.Title_error,
										test,
										"A bunch of text. A bunch of text. A bunch of text. A bunch of text."
										+ " A bunch of text. A bunch of text. A bunch of text. A bunch of text."
										+ " A bunch of text. A bunch of text.",
										InfoboxType.Error))
			{
				ib.ShowDialog(_f);
			} */
			// �
//			byte[] asciiBytes = Encoding.ASCII.GetBytes("�");
//			logfile.Log("� = " + asciiBytes);
//			foreach (var b in asciiBytes)
//				logfile.Log(((int)b).ToString());
//
//			byte[] utf8Bytes = Encoding.UTF8.GetBytes("�");
//			logfile.Log("� = " + utf8Bytes);
//			foreach (var b in utf8Bytes)
//				logfile.Log(((int)b).ToString());

//			logfile.Log();
//			logfile.Log("default encoding= " + Encoding.GetEncoding(0));
//			EncodingInfo[] encs = Encoding.GetEncodings();
//			foreach (var enc in encs)
//			{
//				logfile.Log();
//				logfile.Log(". enc= " + enc.Name);
//				logfile.Log(". DisplayName= " + enc.DisplayName);
//				logfile.Log(". CodePage= " + enc.CodePage);
//			}

			Lastwrite = File.GetLastWriteTime(Fullpath);

			_rows.Clear();

			int loadresult = LOADRESULT_TRUE;

			string[] lines = File.ReadAllLines(Fullpath); // default decoding is UTF-8


			// 0. test character decoding ->

			for (int i = 0; i != lines.Length; ++i)
			{
				if (lines[i].Contains("�"))
				{
					if (CodePage == -1)
						CodePage = Settings._codepage; // init.

					Encoding enc;

					if (CodePage == 0 || CheckCodepage(CodePage))
					{
						// CodePage is default or user-valid.
						enc = Encoding.GetEncoding(CodePage);
					}
					else
						enc = null;


					using (var cpd = new CodePageDialog(_f, enc))
					{
						int result;
						if (cpd.ShowDialog(_f) == DialogResult.OK
							&& Int32.TryParse(cpd.GetCodePage(), out result)
							&& result > -1 && result < 65536
							&& CheckCodepage(result))
						{
							lines = File.ReadAllLines(Fullpath, Encoding.GetEncoding(result));
						}
						else
							return LOADRESULT_FALSE; // silently fail.
					}
					break;
				}
			}


			string line, head, copy;

			// 1. test for fatal errors ->

			if (lines.Length > LINE_VERSION) line = lines[LINE_VERSION].Trim();
			else                             line = String.Empty;

			if (line != gs.TwodaVer && line != "2DA\tV2.0") // tab is not fatal - autocorrect it later
			{
				head = "The 2da-file contains an incorrect version header on its 1st line.";
				copy = Fullpath + Environment.NewLine + Environment.NewLine
					 + line;

				using (var ib = new Infobox(Infobox.Title_error,
											head,
											copy,
											InfoboxType.Error,
											InfoboxButtons.Abort))
				{
					ib.ShowDialog(_f);
				}
				return LOADRESULT_FALSE;
			}


			if (lines.Length > LINE_COLABEL) line = lines[LINE_COLABEL].Trim();
			else                             line = String.Empty;

			if (line.Length == 0)
			{
				head = "The 2da-file does not have any fields. Yata wants a file to have at least one colhead label on its 3rd line.";

				using (var ib = new Infobox(Infobox.Title_error,
											head,
											Fullpath,
											InfoboxType.Error,
											InfoboxButtons.Abort))
				{
					ib.ShowDialog(_f);
				}
				return LOADRESULT_FALSE;
			}



			bool quelch = false; // bypass warnings and try to load the file directly.


			// 2. test for Tabs ->

			if (Settings._strict && Settings._alignoutput != Settings.AoTabs)
			{
				for (int i = 0; i != lines.Length; ++i)
				{
					if (i != LINE_DEFAULT && lines[i].Contains("\t"))
					{
						head = "Tab characters are detected in the 2da-file. They will be replaced with space characters (or deleted where redundant) if the file is saved.";

						switch (ShowLoadWarning(head, Fullpath))
						{
							case DialogResult.Cancel:
								return LOADRESULT_FALSE;

							case DialogResult.OK:
								quelch = true;
								goto case DialogResult.Retry;

							case DialogResult.Retry:
								loadresult = LOADRESULT_CHANGED;
								break;
						}
						break;
					}
				}
			}


			bool autordered = false;
			bool whitespacewarned = false;

			string tr;

			int id = -1;

			int total = lines.Length;
			if (total < LINE_COLABEL + 1) total = LINE_COLABEL + 1; // scan at least 3 'lines' in the file

			// 3. test for ignorable/recoverable errors ->

			for (int i = LINE_VERSION; i != total; ++i)
			{
				if (i < lines.Length) line = lines[i];
				else                  line = String.Empty;

				switch (i)
				{
					case LINE_VERSION:
						if (!quelch && Settings._strict)
						{
							if (line != (tr = line.Trim()))
							{
								head = "The 1st line (version header) has extraneous whitespace. It will be trimmed if the file is saved.";
								copy = Fullpath + Environment.NewLine + Environment.NewLine
									 + line;

								switch (ShowLoadWarning(head, copy))
								{
									case DialogResult.Cancel:
										return LOADRESULT_FALSE;

									case DialogResult.OK:
										quelch = true;
										goto case DialogResult.Retry;

									case DialogResult.Retry:
										loadresult = LOADRESULT_CHANGED;
										break;
								}
							}

							if (!quelch && tr.Contains("\t"))
							{
								head = "The 1st line (version header) contains a tab-character. It will be corrected if the file is saved.";
								copy = Fullpath + Environment.NewLine + Environment.NewLine
									 + tr;

								switch (ShowLoadWarning(head, copy))
								{
									case DialogResult.Cancel:
										return LOADRESULT_FALSE;

									case DialogResult.OK:
										quelch = true;
										goto case DialogResult.Retry;

									case DialogResult.Retry:
										loadresult = LOADRESULT_CHANGED;
										break;
								}
							}

//							if (!quelch && tr.Contains("  ")) // don't bother. This is a fatal error above.
//							{
//								head = "The header on the first line contains redundant spaces. It will be corrected if the file is saved.";
//								copy = Fullpath + Environment.NewLine + Environment.NewLine
//									 + tr;
//
//								switch (ShowLoadWarning(head, copy))
//								{
//									case DialogResult.Cancel:
//										return LOADRESULT_FALSE;
//
//									case DialogResult.OK:
//										quelch = true;
//										goto case DialogResult.Retry;
//
//									case DialogResult.Retry:
//										loadresult = LOADRESULT_CHANGED;
//										break;
//								}
//							}
						}
						break;

					case LINE_DEFAULT:
						tr = line.Trim();

						if (!quelch && Settings._strict && line != tr)
						{
							head = "The 2nd line (default value) has extraneous whitespace. It will be trimmed if the file is saved.";
							copy = Fullpath + Environment.NewLine + Environment.NewLine
								 + line;

							switch (ShowLoadWarning(head, copy))
							{
								case DialogResult.Cancel:
									return LOADRESULT_FALSE;

								case DialogResult.OK:
									quelch = true;
									goto case DialogResult.Retry;

								case DialogResult.Retry:
									loadresult = LOADRESULT_CHANGED;
									break;
							}
						}

						if (tr.StartsWith("DEFAULT:", StringComparison.Ordinal)) // do not 'strict' this feedback ->
						{
							_defaultval = tr.Substring(8).TrimStart();

							if (_defaultval.Length == 0)
							{
								if (!quelch)
								{
									head = "The Default is blank. The 2nd line (default value) will be cleared if the file is saved.";
									copy = Fullpath + Environment.NewLine + Environment.NewLine
										 + tr;

									switch (ShowLoadWarning(head, copy))
									{
										case DialogResult.Cancel:
											return LOADRESULT_FALSE;

										case DialogResult.OK:
											quelch = true;
											goto case DialogResult.Retry;

										case DialogResult.Retry:
											if (Settings._strict) loadresult = LOADRESULT_CHANGED;
											break;
									}
								}
							}
							else
							{
								InputDialog.SpellcheckDefaultval(ref _defaultval, true);

								if (!quelch && Settings._strict && tr != gs.Default + _defaultval)
								{
									head = "The Default on the 2nd line has been changed.";
									copy = Fullpath + Environment.NewLine + Environment.NewLine
										 + gs.Default + _defaultval;

									switch (ShowLoadWarning(head, copy))
									{
										case DialogResult.Cancel:
											return LOADRESULT_FALSE;

										case DialogResult.OK:
											quelch = true;
											goto case DialogResult.Retry;

										case DialogResult.Retry:
											loadresult = LOADRESULT_CHANGED;
											break;
									}
								}
							}
						}
						else
						{
							_defaultval = String.Empty;

							if (!quelch && Settings._strict && tr.Length != 0)
							{
								head = "The 2nd line (default value) in the 2da contains garbage. It will be cleared if the file is saved.";
								copy = Fullpath + Environment.NewLine + Environment.NewLine
									 + line; //.Replace("\t", "\u2192")

								switch (ShowLoadWarning(head, copy))
								{
									case DialogResult.Cancel:
										return LOADRESULT_FALSE;

									case DialogResult.OK:
										quelch = true;
										goto case DialogResult.Retry;

									case DialogResult.Retry:
										loadresult = LOADRESULT_CHANGED;
										break;
								}
							}
						}
						break;

					case LINE_COLABEL:
						tr = line.TrimEnd();

						// TODO: check for redundant whitespace at the start of the line also
						// flag Changed if found ...

						if (!quelch && Settings._strict && line != tr)
						{
							head = "The 3nd line (colhead labels) has extraneous whitespace. It will be trimmed if the file is saved.";
							copy = Fullpath + Environment.NewLine + Environment.NewLine
								 + line;

							switch (ShowLoadWarning(head, copy))
							{
								case DialogResult.Cancel:
									return LOADRESULT_FALSE;

								case DialogResult.OK:
									quelch = true;
									goto case DialogResult.Retry;

								case DialogResult.Retry:
									loadresult = LOADRESULT_CHANGED;
									break;
							}
						}

						if (!quelch
							&& Settings._strict													// line.Length shall not be 0
							&&   line[0] != 32													// space
							&& !(line[0] ==  9 && Settings._alignoutput == Settings.AoTabs))	// tab
						{
							// NOTE: This is an autocorrecting error and there was
							// really no need for the Bioware spec. to indent the 3rd line.
							// The fact it's the 3rd line alone is enough to signify
							// that the line is the colhead fields.

							head = "The 3rd line (colhead labels) is not indented properly. It will be corrected if the file is saved.";
							copy = Fullpath + Environment.NewLine + Environment.NewLine
								 + line;

							switch (ShowLoadWarning(head, copy))
							{
								case DialogResult.Cancel:
									return LOADRESULT_FALSE;

								case DialogResult.OK:
									quelch = true;
									goto case DialogResult.Retry;

								case DialogResult.Retry:
									loadresult = LOADRESULT_CHANGED;
									break;
							}
						}

						tr = tr.TrimStart();

						if (!quelch)
						{
							var chars = new List<char>(); // warn only once per character

							foreach (char character in tr)
							{
								// construct this condition in the positive and put a NOT in front of it
								// to avoid logical pretzels ...

								if (!chars.Contains(character)
									&& !(   character == 32 // space
										|| (character ==  9 // tab
											&& (Settings._alignoutput == Settings.AoTabs
												|| !Settings._strict))
										|| Util.isAsciiAlphanumericOrUnderscore(character)
										|| (!Settings._strict
											&& Util.isPrintableAsciiNotDoublequote(character))))
								{
									head = "Detected a suspect character in the colhead labels ...";
									copy = Fullpath + Environment.NewLine + Environment.NewLine
										 + character;
//										 + (character == 9 ? "\u2192" : character.ToString());

									switch (ShowLoadWarning(head, copy))
									{
										case DialogResult.Cancel:
											return LOADRESULT_FALSE;

										case DialogResult.OK:
											quelch = true;
											break;

										case DialogResult.Retry:
											chars.Add(character);
											break;
									}
								}
								if (quelch) break;
							}
						}

						Fields = tr.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
						break;

					default: // line #3+ datarows ->
						tr = line.Trim();

						if (tr.Length == 0)
						{
							if (!quelch && Settings._strict)
							{
								head = "A blank row is detected. It will be deleted if the file is saved.";
								copy = Fullpath;

								switch (ShowLoadWarning(head, copy))
								{
									case DialogResult.Cancel:
										return LOADRESULT_FALSE;

									case DialogResult.OK:
										quelch = true;
										goto case DialogResult.Retry;

									case DialogResult.Retry:
										loadresult = LOADRESULT_CHANGED;
										break;
								}
							}
						}
						else
						{
							++id;

							if (!quelch && Settings._strict && !whitespacewarned && line != tr)
							{
								whitespacewarned = true;

								head = "At least one row has extraneous whitespace. This will be trimmed if the file is saved.";
								copy = Fullpath + Environment.NewLine + Environment.NewLine
									 + "id " + id;

								switch (ShowLoadWarning(head, copy))
								{
									case DialogResult.Cancel:
										return LOADRESULT_FALSE;

									case DialogResult.OK:
										quelch = true;
										goto case DialogResult.Retry;

									case DialogResult.Retry:
										loadresult = LOADRESULT_CHANGED;
										break;
								}
							}

							string[] celltexts = ParseTableRow(tr);

							if (!quelch) // show these warnings even if not Strict.
							{
								// test for id
								int result;
								if (!Int32.TryParse(celltexts[0], out result))
									head = "The 2da-file contains an id that is not an integer.";
								else if (result != id)
									head = "The 2da-file contains an id that is out of order.";
								else
									head = null;

								if (head != null) // show this warning even if Autorder true - table shall be flagged Changed
								{
									copy = Fullpath + Environment.NewLine + Environment.NewLine
										 + "id " + id + " \u2192 " + celltexts[0];

									switch (ShowLoadWarning(head, copy))
									{
										case DialogResult.Cancel:
											return LOADRESULT_FALSE;

										case DialogResult.OK:
											quelch = true;
											break;
									}
								}

								// test for matching cell-fields under cols
								if (!quelch)
								{
									if (celltexts.Length != Fields.Length + 1)
									{
										head = "The 2da-file contains fields that do not align with its cols.";
										copy = Fullpath + Environment.NewLine + Environment.NewLine
											 + "Colcount " + (Fields.Length + 1) + Environment.NewLine
											 + "id " + id + " fields \u2192 " + celltexts.Length;

										switch (ShowLoadWarning(head, copy))
										{
											case DialogResult.Cancel:
												return LOADRESULT_FALSE;

											case DialogResult.OK:
												quelch = true;
												break;
										}
									}
								}

								// test for an odd quantity of double-quote characters
								if (!quelch)
								{
									int quotes = 0;
									foreach (char character in tr)
									if (character == '"')
										++quotes;

									if (quotes % 2 == 1)
									{
										head = "A row contains an odd quantity of double-quote characters. This could be bad ...";
										copy = Fullpath + Environment.NewLine + Environment.NewLine
											 + "id " + id;

										switch (ShowLoadWarning(head, copy))
										{
											case DialogResult.Cancel:
												return LOADRESULT_FALSE;

											case DialogResult.OK:
												quelch = true;
												break;
										}
									}
								}
							}


							if (Settings._autorder && id.ToString(CultureInfo.InvariantCulture) != celltexts[0])
							{
								celltexts[0] = id.ToString(CultureInfo.InvariantCulture);
								autordered = true;
							}

							// NOTE: Tests for well-formed fields will be done later so that their
							//       respective cells can be flagged as loadchanged if applicable.

							_rows.Add(celltexts);
						}
						break;
				}
			}

			if (autordered)
			{
				using (var ib = new Infobox(Infobox.Title_infor, "Row ids have been corrected."))
					ib.ShowDialog(_f);

				loadresult = LOADRESULT_CHANGED;
			}


			if (_rows.Count == 0) // add a row of stars so grid is not left blank ->
			{
				var cells = new string[Fields.Length + 1]; // NOTE: 'Fields' does not contain the ID-col.

				int c = 0;
				if (Settings._autorder)
					cells[c++] = "0";

				for (; c <= Fields.Length; ++c)
					cells[c] = gs.Stars;

				_rows.Add(cells);
				return LOADRESULT_CHANGED; // flag the Table as changed
			}

			return loadresult;
		}

		/// <summary>
		/// Initializes a loaded, reloaded, or created
		/// <c><see cref="YataGrid"/></c>.
		/// </summary>
		/// <param name="changed"></param>
		/// <param name="reload"></param>
		internal void Init(bool changed = false, bool reload = false)
		{
			if (reload)
			{
				_init = true;

				_scrollVert.Value =
				_scrollHori.Value = 0;

				_sortcol = 0;
				_sortdir = SORT_ASC;

				RangeSelect = 0;

				FrozenCount = YataGrid.FreezeId;

				Cols.Clear();
				Rows.Clear();

				Controls.Remove(_panelCols);
				Controls.Remove(_panelRows);
				Controls.Remove(FrozenPanel);

//				_panelCols  .Dispose(); // breaks the frozen-labels
//				_panelRows  .Dispose();
//				_panelFrozen.Dispose();
			}
			else
			{
				switch (Path.GetFileNameWithoutExtension(Fullpath).ToUpperInvariant())
				{
					case "CRAFTING":
						Info = InfoType.INFO_CRAFT;
						break;

					case "SPELLS":
						Info = InfoType.INFO_SPELL;
						break;

					case "FEAT":
						Info = InfoType.INFO_FEAT;
						break;

					case "CLASSES":
						Info = InfoType.INFO_CLASS;
						break;
				}

				if (!Groped && Info != InfoType.INFO_NONE)
				{
					Groped = true;
					foreach (var dir in Settings._pathall)
						_f.GropeLabels(dir);
				}
			}

			Changed = changed;

			_panelCols = new YataPanelCols(this);
			_panelRows = new YataPanelRows(this);

			CreateCols();
			CreateRows();

			FrozenPanel = new YataPanelFrozen(this, Cols[0].Width);
			InitializeFrozenLabels();

			metricStaticHeads(_f);

			Controls.Add(FrozenPanel);
			Controls.Add(_panelRows);
			Controls.Add(_panelCols);


			_scrollVert.LargeChange =
			_scrollHori.LargeChange = HeightRow;
			InitScroll();

			Select();
			_init = false;
		}

		/// <summary>
		/// Creates the cols and caches the 2da's colhead data.
		/// </summary>
		/// <param name="rewidthOnly"><c>true</c> to only re-width cols - ie.
		/// Font changed</param>
		/// <seealso cref="CreateCol()"><c>CreateCol()</c></seealso>
		internal void CreateCols(bool rewidthOnly = false)
		{
			int c = 0;
			if (!rewidthOnly)
			{
				ColCount = Fields.Length + 1; // 'Fields' does not include rowhead or id-col

				for (; c != ColCount; ++c)
					Cols.Add(new Col());

				Cols[0].text = gs.Id; // NOTE: Is not measured - the cells below it determine col-width.
			}

			int widthtext; c = 0;
			foreach (string head in Fields) // set initial col-widths based on colheads only ->
			{
				++c; // start at col 1 - skip id col

				if (!rewidthOnly)
					Cols[c].text = head;

				widthtext = YataGraphics.MeasureWidth(head, _f.FontAccent);
				Cols[c]._widthtext = widthtext;

				Cols[c].SetWidth(widthtext + _padHori * 2 + _padHoriSort, rewidthOnly);
			}
		}

		/// <summary>
		/// Creates the <c><see cref="Row">Rows</see></c> and adds
		/// <c><see cref="Cell">Cells</see></c> to each <c>Row</c>. Also sets
		/// <c><see cref="Cell.loadchanged">Cell.loadchanged</see></c> if a
		/// cell-field's text was altered/corrected while loading the 2da for
		/// this <c>YataGrid</c>.
		/// </summary>
		void CreateRows()
		{
			RowCount = _rows.Count;

			bool changed = false, loadchanged; string text;
			bool isLoadchanged = false;

			for (int r = 0; r != RowCount; ++r)
			{
				changed = changed
					   || _rows[r].Length > ColCount; // flag Changed if any field(s) get cut off.

				Rows.Add(new Row(r,
								 ColCount,
								 (r % 2 == 0) ? Brushes.Alice
											  : Brushes.Bob,
								 this));

				for (int c = 0; c != ColCount; ++c)
				{
					loadchanged = false;
					if (c < _rows[r].Length)
					{
						text = _rows[r][c];
						if (VerifyText(ref text, true))
						{
							changed = loadchanged = isLoadchanged = true;
						}
					}
					else
					{
						text = gs.Stars;
						changed = loadchanged = isLoadchanged = true;
					}

					(this[r,c] = new Cell(r,c, text)).loadchanged = loadchanged;
				}
			}

			if (isLoadchanged) // inform user regardless of Strict setting ->
			{
				_f.EnableGotoLoadchanged(anyLoadchanged());

				using (var ib = new Infobox(Infobox.Title_infor, "Cell-texts changed."))
					ib.ShowDialog(_f);
			}
			Changed |= changed;

			_rows.Clear(); // done w/ '_rows'


			int w, wT; // adjust col-widths based on fields ->
			for (int c = 0; c != ColCount; ++c)
			{
				w = _wId; // start each col at min colwidth
				for (int r = 0; r != RowCount; ++r)
				{
					if ((text = this[r,c].text) == gs.Stars) // bingo.
						wT = _wStars;
					else
						wT = YataGraphics.MeasureWidth(text, Font);

					this[r,c]._widthtext = wT;

					if ((wT += _padHori * 2) > w) w = wT;
				}
				Cols[c].SetWidth(w);
			}


//			var threads = new Thread[ColCount];
//			for (int c = 0; c != ColCount; ++c)
//			{
//				int cT = c;
//				threads[c] = new Thread(() => doCol(cT));
//				//logfile.Log("c= " + c + " IsBackground= " + threads[c].IsBackground); // default false
//				threads[c].IsBackground = true;
//				threads[c].Start();
//			}

//			int procs = Environment.ProcessorCount;
//			logfile.Log("ProcessorCount= " + procs);
//			var threads = new Thread[procs];
//			for (int i = 0; i != procs; ++i)
//			{
//				threads[i] = new Thread(()=> doCol(0, RowCount/procs)); // wont work - a lone doCol() method would be totally thread unsafe.
//			}

//			int c0 = 0;
//			int c1 = ColCount / 4;
//			int c2 = ColCount / 2;
//			int c3 = ColCount * 3 / 4;
//			int c4 = ColCount;
//
//			logfile.Log("c0= " + c0);
//			logfile.Log("c1= " + c1);
//			logfile.Log("c2= " + c2);
//			logfile.Log("c3= " + c3);
//			logfile.Log("c4= " + c4);
//
//			var threads = new Thread[4];
//			threads[0] = new Thread(()=> doCol0(c0,c1));
//			threads[0].IsBackground = true;
//			threads[0].Start();
//			threads[1] = new Thread(()=> doCol1(c1,c2));
//			threads[1].IsBackground = true;
//			threads[1].Start();
//			threads[2] = new Thread(()=> doCol2(c2,c3));
//			threads[2].IsBackground = true;
//			threads[2].Start();
//			threads[3] = new Thread(()=> doCol3(c3,c4));
//			threads[3].IsBackground = true;
//			threads[3].Start();
//
//			foreach (var thread in threads)
//				thread.Join();
		}

/*		void doCol0(int c0, int c1)
		{
			for (int c = c0; c != c1; ++c)
			{
				int w = 0, wT;
				for (int r = 0; r != RowCount; ++r)
				{
					lock (Font)
					{
					//logfile.Log("doCol0 c= " + c + " r= " + r);
					wT = YataGraphics.MeasureWidth(this[r,c].text, Font);
					}
					this[r,c]._widthtext = wT;
					if ((wT += _padHori * 2) > w) w = wT;
				}
				Cols[c].width(w);
			}
		}
		void doCol1(int c1, int c2)
		{
			for (int c = c1; c != c2; ++c)
			{
				int w = 0, wT;
				for (int r = 0; r != RowCount; ++r)
				{
					lock (Font)
					{
					//logfile.Log("doCol1 c= " + c + " r= " + r);
					wT = YataGraphics.MeasureWidth(this[r,c].text, Font);
					}
					this[r,c]._widthtext = wT;
					if ((wT += _padHori * 2) > w) w = wT;
				}
				Cols[c].width(w);
			}
		}
		void doCol2(int c2, int c3)
		{
			for (int c = c2; c != c3; ++c)
			{
				int w = 0, wT;
				for (int r = 0; r != RowCount; ++r)
				{
					lock (Font)
					{
					//logfile.Log("doCol2 c= " + c + " r= " + r);
					wT = YataGraphics.MeasureWidth(this[r,c].text, Font);
					}
					this[r,c]._widthtext = wT;
					if ((wT += _padHori * 2) > w) w = wT;
				}
				Cols[c].width(w);
			}
		}
		void doCol3(int c3, int c4)
		{
			for (int c = c3; c != c4; ++c)
			{
				int w = 0, wT;
				for (int r = 0; r != RowCount; ++r)
				{
					lock (Font)
					{
					//logfile.Log("doCol3 c= " + c + " r= " + r);
					wT = YataGraphics.MeasureWidth(this[r,c].text, Font);
					}
					this[r,c]._widthtext = wT;
					if ((wT += _padHori * 2) > w) w = wT;
				}
				Cols[c].width(w);
			}
		} */

		/// <summary>
		/// Initializes the frozen-labels on the colhead panel.
		/// </summary>
		void InitializeFrozenLabels()
		{
			_labelid    .Visible =
			_labelfirst .Visible =
			_labelsecond.Visible = false;

			if (ColCount != 0)
			{
				_labelid.Visible = true;

				if (_initFrozenLabels) // TODO: FrozenLabels could be instantiated / updated-on-Reload better.
				{
					DrawRegulator.SetDoubleBuffered(_labelid);
					_labelid.BackColor = Colors.FrozenHead;

					_labelid.Resize     += label_Resize;
					_labelid.Paint      += labelid_Paint;
					_labelid.MouseClick += labelid_MouseClick;
					_labelid.MouseClick += (sender, e) => Select();
				}
				_panelCols.Controls.Add(_labelid);

				if (ColCount > 1)
				{
					_labelfirst.Visible = (FrozenCount > FreezeId); // required after Font calibration

					if (_initFrozenLabels)
					{
						DrawRegulator.SetDoubleBuffered(_labelfirst);
						_labelfirst.BackColor = Colors.FrozenHead;

						_labelfirst.Resize     += label_Resize;
						_labelfirst.Paint      += labelfirst_Paint;
						_labelfirst.MouseClick += labelfirst_MouseClick;
						_labelfirst.MouseClick += (sender, e) => Select();
					}
					_panelCols.Controls.Add(_labelfirst);

					if (ColCount > 2)
					{
						_labelsecond.Visible = (FrozenCount > FreezeFirst); // required after Font calibration

						if (_initFrozenLabels)
						{
							DrawRegulator.SetDoubleBuffered(_labelsecond);
							_labelsecond.BackColor = Colors.FrozenHead;

							_labelsecond.Resize     += label_Resize;
							_labelsecond.Paint      += labelsecond_Paint;
							_labelsecond.MouseClick += labelsecond_MouseClick;
							_labelsecond.MouseClick += (sender, e) => Select();
						}
						_panelCols.Controls.Add(_labelsecond);
					}
				}
			}
			_initFrozenLabels = false;
		}

		/// <summary>
		/// Creates <c>LinearGradientBrushes</c> for
		/// <list type="bullet">
		/// <item><c><see cref="labelid_Paint()">labelid_Paint()</see></c></item>
		/// <item><c><see cref="labelfirst_Paint()">labelfirst_Paint()</see></c></item>
		/// <item><c><see cref="labelsecond_Paint()">labelsecond_Paint()</see></c></item>
		/// </list>
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="_labelid"/></c></item>
		/// <item><c><see cref="_labelfirst"/></c></item>
		/// <item><c><see cref="_labelsecond"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void label_Resize(object sender, EventArgs e)
		{
			if (Settings._gradient && _heightColheadCached != HeightColhead)
			{
				_heightColheadCached = HeightColhead;

				if (Gradients.FrozenLabel != null)
					Gradients.FrozenLabel.Dispose();

				Gradients.FrozenLabel = new LinearGradientBrush(new Point(0, 0),
																new Point(0, HeightColhead),
																Color.Cornsilk, Color.BurlyWood);

				if (Gradients.Disordered != null)
					Gradients.Disordered.Dispose();

				Gradients.Disordered = new LinearGradientBrush(new Point(0, 0),
															   new Point(0, HeightColhead),
															   Color.LightCoral, Color.Lavender);
			}
		}
		#endregion Methods (load)


		#region Methods (create)
		/// <summary>
		/// Creates a table from scratch w/ 1 row and 1 colhead.
		/// </summary>
		internal void CreateTable()
		{
			Fields = new[] { gs.DefaultColLabel };

			var cells = new string[Fields.Length + 1]; // NOTE: 'Fields' does not contain the ID-col.

			cells[0] = "0"; // force 'Settings._autorder'

			for (int c = 1; c <= Fields.Length; ++c)
				cells[c] = gs.Stars;

			_rows.Clear();
			_rows.Add(cells);
		}
		#endregion Methods (create)
	}
}
