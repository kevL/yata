using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using yata.Properties;


namespace yata
{
	class YataDataGridView
		:
			DataGridView
	{
		internal enum Frozen
		{
			FreezeOff,
			FreezeFirstCol,
			FreezeSecondCol
		}

		enum ColDisplay
		{
			DisplayOff,
			DisplayDone,
			DisplayReady
		}


		YataForm _f;

		internal string Pfe // Path-File-Extension (ie. fullpath)
		{ get; set; }

		internal string[] Cols // 'Cols' does NOT contain #0 col IDs (so that often needs +1)
		{ get; set; }

		List<string[]> _rows = new List<string[]>();

		bool _loading;

		bool _changed;
		internal bool Changed
		{
			get { return _changed; }
			set { _f.TableChanged(_changed = value); }
		}

		internal bool CraftInfo
		{ get; set; }

		internal Frozen Freeze
		{ get; set; }

		ColDisplay Display
		{ get; set; }

		int _col;

		int _a, _b; // for sorting cols (premature optimization only.)

		const int _padVert = 4;


		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="pfe"></param>
		internal YataDataGridView(YataForm f, string pfe)
		{
			DrawingControl.SetDoubleBuffered(this);

			_f = f;

			Pfe = pfe;

			Dock = DockStyle.Fill;
			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			AllowUserToResizeRows = false;
			AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

			RowHeadersWidth = 45;

			CellValueChanged += CellChanged;
			CellPainting     += PaintCell;

			SelectionChanged += HoriscrollCell;

			SortCompare      += TableSort;
			Sorted           += TableSorted;

			//DefaultCellStyle <- TODO: these <--

			Freeze = Frozen.FreezeOff;
		}


		/// <summary>
		/// Handles clicking a col header. For LMB only.
		/// LMB selects the col, Ctrl+LMB adds the col to current selection(s).
		/// Shift+LMB sorts the col.
		/// NOTE: S for Shift ~ S for Sort
		/// </summary>
		/// <param name="e"></param>
		protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_col = e.ColumnIndex; // NOTE: '_col' is set/reset here only.

				if ((ModifierKeys & Keys.Shift) == Keys.Shift)	// Shift+LMB = sort by col
				{
					// if this is the first sort on the ID col
					// sort it twice. Because in a well-formed 2da nothing will happen on the first sort.
					// (ids are by default Ascending)
					if (_col == 0 && SortOrder == SortOrder.None)
						base.OnColumnHeaderMouseClick(e);

					base.OnColumnHeaderMouseClick(e);			// -> triggers SortCompare
				}
				else											// LMB (no Shift) = select col
				{
					bool sel = false;

					int rows = Rows.Count - 1;

					for (int id = 0; id != rows; ++id)
					{
						if (!Rows[id].Cells[_col].Selected)
						{
							sel = true; // NOTE: get this before wiping selection.
							break;
						}
					}

					Display = ColDisplay.DisplayReady;

					bool ctrl = (ModifierKeys & Keys.Control) == Keys.Control;
					if (!ctrl)
						ClearSelection(); // NOTE: This fires SelectionChanged->DisplayCell only *once*

					if (sel || ctrl)
					{
						for (int id = 0; id != rows; ++id)
						{
							Rows[id].Cells[_col].Selected = sel;
						}
					}

					Display = ColDisplay.DisplayOff;
				}
			}
		}

		/// <summary>
		/// The last cell can be partly or basically completely hidden even when
		/// it's selected; this workaround ensures that if it is selected it is
		/// displayed fully. Also ensures that partly hidden cells to the left
		/// get fully displayed.
		/// NOTE: Cols cannot be selected - all cells in the col are selected
		/// individually instead.
		/// NOTE: This has to run only if (a) user clicked a cell (ergo there's
		/// a valid CurrentCell) or (b) user clicked a col-header (ergo there
		/// might not be any CurrentCell but a col has been selected/unselected
		/// per OnColumnHeaderMouseClick()).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void HoriscrollCell(object sender, EventArgs e)
		{
			if (Display == ColDisplay.DisplayReady)
			{
				Display = ColDisplay.DisplayDone;

				int colLast = Columns.Count - 1;

				if (_col == colLast)
				{
					HorizontalScrollingOffset += Columns[colLast].Width;
				}
				else if (_col == FirstDisplayedScrollingColumnIndex
					&& FirstDisplayedScrollingColumnHiddenWidth != 0)
				{
					HorizontalScrollingOffset -= FirstDisplayedScrollingColumnHiddenWidth;
				}
			}
			else if (CurrentCell != null)// && CurrentCell.Selected)
			{
				int colCell = CurrentCell.ColumnIndex;
				int colLast = Columns.Count - 1;

				if (colCell == colLast && CurrentCell.Selected)
				{
					HorizontalScrollingOffset += Columns[colLast].Width;
				}
				else if (colCell == FirstDisplayedScrollingColumnIndex
					&& FirstDisplayedScrollingColumnHiddenWidth != 0)
				{
					HorizontalScrollingOffset -= FirstDisplayedScrollingColumnHiddenWidth;
				}
			}
		}

		/// <summary>
		/// Handles painting the cells in the table.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void PaintCell(object sender, DataGridViewCellPaintingEventArgs e)
		{
			e.PaintBackground(e.ClipBounds, true);

			int col = e.ColumnIndex;

			if (col != -1)
			{
				e.PaintContent(e.ClipBounds);

				if (e.RowIndex == -1)
				{
					Bitmap sort = null;
					if (SortedColumn == null) // draw an asc-arrow on the ID col-header when table loads
					{
						if (col == 0)
							sort = Resources.asc_16px;
					}
					else if (SortedColumn.Index == col)
					{
						sort = (SortOrder == SortOrder.Ascending) ? Resources.asc_16px
																  : Resources.des_16px;
					}

					if (sort != null)
						e.Graphics.DrawImage(sort,
											 e.CellBounds.X + e.CellBounds.Width - 17,
											 e.CellBounds.Y + 3);
				}
			}
			else if (e.FormattedValue != null) // safety. is row-header
			{
				var flags = TextFormatFlags.NoPrefix;

				// NOTE: MS doc for DrawText() says that using a Point doesn't work on Win2000 machines.
				var rect = e.CellBounds;
				rect.X += 10;
				rect.Y += 4;

				TextRenderer.DrawText(e.Graphics,
									  Convert.ToString(e.FormattedValue),
									  e.CellStyle.Font,
									  rect,
									  e.CellStyle.ForeColor,
									  flags);
			}

			e.Handled = true;
		}


		#region Sort
		/// <summary>
		/// Sorts fields as integers iff they convert to integers and performs
		/// a secondary sort against their IDs if applicable.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void TableSort(object sender, DataGridViewSortCompareEventArgs e)
		{
			if (   Int32.TryParse(e.CellValue1.ToString(), out _a)
				&& Int32.TryParse(e.CellValue2.ToString(), out _b))
			{
				e.SortResult = _a.CompareTo(_b);
				e.Handled = true;
			}

			if (e.Column != Columns[0] // secondary sort on id ->
				&& e.CellValue1.ToString() == e.CellValue2.ToString())
			{
				if (   Int32.TryParse(Rows[e.RowIndex1].Cells[0].Value.ToString(), out _a)
					&& Int32.TryParse(Rows[e.RowIndex2].Cells[0].Value.ToString(), out _b))
				{
					e.SortResult = _a.CompareTo(_b);
					e.Handled = true;
				}
			}
		}

		/// <summary>
		/// Ensures that the searched-for field is displayed and re-orders the
		/// row-headers.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void TableSorted(object sender, EventArgs e)
		{
			Changed = true;

			DisplaySelected();
			RelabelRowHeaders();
		}

		internal void DisplaySelected()
		{
			int sel = -1;

			if (SelectedRows.Count != 0)
			{
				sel = SelectedRows[0].Index;
			}
			else if (SelectedCells.Count != 0)
			{
				sel = SelectedCells[0].RowIndex;
			}

			if (sel != -1 && sel < Rows.Count - 1)
			{
				int visFirst = FirstDisplayedScrollingRowIndex;
				if (sel < visFirst)
				{
					FirstDisplayedScrollingRowIndex = sel;
				}
				else
				{
					int visCount = DisplayedRowCount(false);
					if (sel >= visFirst + visCount)
					{
						FirstDisplayedScrollingRowIndex = sel - visCount + 1;
					}
				}
			}
		}
		#endregion Sort


		const int LABELS = 2;

		/// <summary>
		/// Tries to load a 2da file.
		/// </summary>
		/// <returns>true if 2da loaded successfully perhaps</returns>
		internal bool Load2da()
		{
			Text = "Yata";

			bool ignoreErrors = false;

			string[] lines = File.ReadAllLines(Pfe);

			Cols = lines[LABELS].Split(new char[0], StringSplitOptions.RemoveEmptyEntries); // TODO: test for double-quotes

			int quotes =  0;
			int id     = -1;
			int lineId = -1;

			// TODO: Test for an even quantity of double-quotes on each line.
			// ie. Account for the fact that ParseLine() needs to ASSUME that quotes are fairly accurate.

			foreach (string line in lines)
			{
				// test version header
				if (++lineId == 0)
				{
					string st = line.Trim();
					if (st != "2DA V2.0") // && st != "2DA	V2.0") // 2DA	V2.0 <- uh yeah right
					{
						string error = "The 2da-file contains a malformed version header."
									 + Environment.NewLine + Environment.NewLine
									 + Pfe;
						switch (ShowLoadError(error))
						{
							case DialogResult.Abort:
								return false;

							case DialogResult.Retry:
								break;

							case DialogResult.Ignore:
								ignoreErrors = true;
								break;
						}
					}
				}

				// test for blank 2nd line
				if (!ignoreErrors && lineId == 1 && !String.IsNullOrEmpty(line)) // .Trim() // uh yeah ... right.
				{
					string error = "The 2nd line in the 2da should be blank."
								 + " This editor does not support default value-types."
								 + Environment.NewLine + Environment.NewLine
								 + Pfe;
					switch (ShowLoadError(error))
					{
						case DialogResult.Abort:
							return false;

						case DialogResult.Retry:
							break;

						case DialogResult.Ignore:
							ignoreErrors = true;
							break;
					}
				}

				if (lineId > LABELS)
				{
					string[] row = Parse2daRow(line);

					// test for well-formed, consistent IDs
					++id;

					if (!ignoreErrors)
					{
						int result;
						if (!Int32.TryParse(row[0], out result) || result != id)
						{
							string error = "The 2da-file contains an ID that is not an integer or is out of order."
										 + Environment.NewLine + Environment.NewLine
										 + Pfe
										 + Environment.NewLine + Environment.NewLine
										 + id + " / " + row[0];
							switch (ShowLoadError(error))
							{
								case DialogResult.Abort:
									return false;

								case DialogResult.Retry:
									break;

								case DialogResult.Ignore:
									ignoreErrors = true;
									break;
							}
						}
					}

					// test for matching fields under columns
					if (!ignoreErrors && row.Length != Cols.Length + 1)
					{
						string error = "The 2da-file contains fields that do not align with its cols."
									 + Environment.NewLine + Environment.NewLine
									 + Pfe
									 + Environment.NewLine + Environment.NewLine
									 + "id " + id;
						switch (ShowLoadError(error))
						{
							case DialogResult.Abort:
								return false;

							case DialogResult.Retry:
								break;

							case DialogResult.Ignore:
								ignoreErrors = true;
								break;
						}
					}

					// test for matching double-quote characters on the fly
					if (!ignoreErrors)
					{
						bool quoteFirst, quoteLast;
						int col = -1;
						foreach (string field in row)
						{
							++col;
							quoteFirst = field.StartsWith("\"", StringComparison.InvariantCulture);
							quoteLast  = field.EndsWith(  "\"", StringComparison.InvariantCulture);
							if (   ( quoteFirst && !quoteLast)
								|| (!quoteFirst &&  quoteLast))
							{
								string error = "Found a missing double-quote character."
											 + Environment.NewLine + Environment.NewLine
											 + Pfe
											 + Environment.NewLine + Environment.NewLine
											 + "id " + id + " / col " + col;
								switch (ShowLoadError(error))
								{
									case DialogResult.Abort:
										return false;

									case DialogResult.Retry:
										break;

									case DialogResult.Ignore:
										ignoreErrors = true;
										break;
								}
							}
							else if (quoteFirst && quoteLast
								&& field.Length == 1)
							{
								string error = "Found an isolated double-quote character."
											 + Environment.NewLine + Environment.NewLine
											 + Pfe
											 + Environment.NewLine + Environment.NewLine
											 + "id " + id + " / col " + col;
								switch (ShowLoadError(error))
								{
									case DialogResult.Abort:
										return false;

									case DialogResult.Retry:
										break;

									case DialogResult.Ignore:
										ignoreErrors = true;
										break;
								}
							}
						}
					}

					_rows.Add(row);
				}

				// also test for an odd quantity of double-quote characters
				foreach (char character in line)
				{
					if (character == '"')
						++quotes;
				}
			}

			// safety test for double-quotes (ought be caught above)
			if (quotes % 2 == 1)
			{
				string error = "The 2da-file contains an odd quantity of double-quote characters."
							 + Environment.NewLine + Environment.NewLine
							 + Pfe;
				switch (ShowLoadError(error))
				{
					case DialogResult.Abort:
						return false;

					case DialogResult.Retry:
						break;

					case DialogResult.Ignore:
						ignoreErrors = true;
						break;
				}
			}


			if (CraftInfo = (Path.GetFileNameWithoutExtension(Pfe).ToLower() == "crafting"))
			{
				foreach (var dir in Settings._pathall)
					_f.GropeLabels(dir);
			}


			_loading = true;

			DrawingControl.SuspendDrawing(this);	// NOTE: Drawing resumes after autosize in either
													// YataForm.CreateTabPage() or YataForm.ReloadToolStripMenuItemClick().
			PopulateColumnHeaders();
			PopulateTableRows();

			_loading = false;

			return true;
		}

		/// <summary>
		/// TODO: optimize.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		string[] Parse2daRow(string line)
		{
			var list  = new List<string>();
			var field = new List<char>();

			bool add      = false;
			bool inQuotes = false;

			char c;

			int posLast = line.Length + 1; // include an extra iteration to get the last field (that has no whitespace after it)
			for (int pos = 0; pos != posLast; ++pos)
			{
				if (pos == line.Length)	// hit lineend -> add the last field
				{						// if there's no whitespace after it (last fields
					if (add)			// w/ trailing whitespace are dealt with below)
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

						add = true;
						field.Add(c);
					}
					else if (c != ' ' && c != '\t')			// any non-whitespace char (except double-quote)
					{
						add = true;
						field.Add(c);
					}
					else if (add)							// hit a space or tab
					{
						add = false;
						list.Add(new string(field.ToArray()));

						field.Clear();
					}
				}
			}
			return list.ToArray();
		}

		/// <summary>
		/// A generic error-box if something goes wrong while loading a 2da file.
		/// </summary>
		/// <param name="error"></param>
		DialogResult ShowLoadError(string error)
		{
			error += Environment.NewLine + Environment.NewLine
				   + "abort\t- stop loading"         + Environment.NewLine
				   + "retry\t- check for next error" + Environment.NewLine
				   + "ignore\t- just load the file";

			return MessageBox.Show(error,
								   "burp",
								   MessageBoxButtons.AbortRetryIgnore,
								   MessageBoxIcon.Exclamation,
								   MessageBoxDefaultButton.Button2);
		}

		/// <summary>
		/// Populates the table's columnheaders.
		/// </summary>
		void PopulateColumnHeaders()
		{
			ColumnCount = Cols.Length + 1;

			Columns[0].HeaderText = "id";
			Columns[0].Frozen     = true;

			int colId = 0;
			foreach (string col in Cols)
			{
				Columns[++colId].HeaderText = col;
			}
		}

		/// <summary>
		/// Populates the table's rows.
		/// </summary>
		void PopulateTableRows()
		{
			var pb = new ProgBar();
			pb.ValTop = _rows.Count;
			pb.Show();

			Color color;

			int id = -1;
			foreach (string[] row in _rows)
			{
				Rows.Add(row);
				Rows[++id].HeaderCell.Value = id.ToString(); // label row header

				color = (id % 2 == 0) ? Color.AliceBlue
									  : Color.BlanchedAlmond;

				Rows[id].DefaultCellStyle.BackColor = color;

				pb.Step();
			}
			_rows.Clear();
		}

		/// <summary>
		/// Maintains row-sizing wrt current Font.
		/// </summary>
		internal void SetRowSize()
		{
			int h = TextRenderer.MeasureText("X", _f.Font).Height + _padVert * 2;
			RowTemplate.Height = h;


			for (int row = 0; row != Rows.Count; ++row)
				Rows[row].Height = h;

			YataDataGridView table;

			int w = 25, wT; // row-headers' width stays uniform across all tabpages ->

			int tabs = _f.Tabs.TabCount;
			for (int tab = 0; tab != tabs; ++tab)
			{
				table = _f.Tabs.TabPages[tab].Tag as YataDataGridView;
				if ((wT = table.Rows.Count - 2) > w)
					w = wT;
			}

			w = TextRenderer.MeasureText(w.ToString(), _f.Font).Width + 20;
			for (int tab = 0; tab != tabs; ++tab)
			{
				table = _f.Tabs.TabPages[tab].Tag as YataDataGridView;
				table.RowHeadersWidth = w;
			}
		}

		/// <summary>
		/// Fires when user commits text to a cell.
		/// NOTE: Changing the value of a cell will re-fire this event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void CellChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!_loading)
			{
				Changed = true;

				int id  = e.RowIndex;
				int col = e.ColumnIndex;

				if (id == Rows.Count - 2)	// ie. User created a new row by editing the last
				{							// row so .NET created a new last row under it.
					RelabelRowHeaders();
				}


				// checks for invalid field contents ->

				object val = Rows[id].Cells[col].Value;
				if (val == null || String.IsNullOrEmpty(val.ToString()))
				{
					Rows[id].Cells[col].Value = Constants.Stars;
				}
				else
				{
					string field = val.ToString();
					bool quoteFirst = field.StartsWith("\"", StringComparison.InvariantCulture);
					bool quoteLast  = field.EndsWith(  "\"", StringComparison.InvariantCulture);
					if (quoteFirst && quoteLast)
					{
						if (field.Length == 1)
						{
							Rows[id].Cells[col].Value = Constants.Stars;
						}
						else if (field.Length > 2
							&& field.Substring(1, field.Length - 2).Trim() == String.Empty)
						{
							Rows[id].Cells[col].Value = "\"\""; // TODO: this triggers the event a second time ...

							const string error = "It is not allowed to have double-quotes"
											   + " around whitespace only.";
							MessageBox.Show(error,
											"burp",
											MessageBoxButtons.OK,
											MessageBoxIcon.Exclamation,
											MessageBoxDefaultButton.Button1);
						}
					}
					else if (( quoteFirst && !quoteLast)
						||   (!quoteFirst &&  quoteLast))
					{
						if (quoteFirst)
							Rows[id].Cells[col].Value = field + "\"";
						else
							Rows[id].Cells[col].Value = "\"" + field;

						const string error = "It is not allowed to have only 1 double-quote"
										   + " surrounding a field.";
						MessageBox.Show(error,
										"burp",
										MessageBoxButtons.OK,
										MessageBoxIcon.Exclamation,
										MessageBoxDefaultButton.Button1);
					}

					if (field.Length > 2) // lol but it works ->
					{
						string first     = field.Substring(0, 1);
						string last      = field.Substring(field.Length - 1, 1);
						string fieldTest = field.Substring(1, field.Length - 2);

						if (fieldTest.Contains("\""))
						{
							fieldTest = fieldTest.Remove(fieldTest.IndexOf('"'), 1);
							Rows[id].Cells[col].Value = first + fieldTest + last;

							const string error = "It is not allowed to use double-quotes except"
											   + " at the beginning and end of a field.";
							MessageBox.Show(error,
											"burp",
											MessageBoxButtons.OK,
											MessageBoxIcon.Exclamation,
											MessageBoxDefaultButton.Button1);
						}
					}
				}
			}
		}

		/// <summary>
		/// Relabels the rowheaders when inserting/deleting/sorting rows.
		/// </summary>
		internal void RelabelRowHeaders()
		{
			if (Rows.Count > 1) // safety - ought be checked in calling funct.
			{
				_loading = true; // (re)use '_loading' to prevent firing CellChanged events for the RowHeaders

				int rows = Rows.Count - 1;
				for (int id = 0; id != rows; ++id)
				{
					Rows[id].HeaderCell.Value = id.ToString();
				}
				_loading = false;
			}
		}


		#region Events (override)
		// https://stackoverflow.com/questions/21873361/datagridview-enter-key-event-handling#answer-21882188

		/// <summary>
		/// Handles starting editing a cell by pressing Enter - this fires
		/// before edit begins.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.Enter:
					if (SelectedCells[0] != null)
						CurrentCell = SelectedCells[0];

					if (CurrentCell != null)
						BeginEdit(true);

					e.Handled = true;
					break;

				case Keys.Escape:
					CurrentCell = null;
					for (int cell = 0; cell != SelectedCells.Count; ++cell)
					{
						SelectedCells[cell].Selected = false;
					}
					e.Handled = true;
					break;

				default:
					base.OnKeyDown(e);
					break;
			}
		}

		/// <summary>
		/// Handles relabeling the row-headers after selecting a row and
		/// pressing the Delete key - this does not fire during edit.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.Delete:
					RelabelRowHeaders();
					e.Handled = true;
					break;

				default:
					base.OnKeyUp(e);
					break;
			}
		}

		/// <summary>
		/// Handles ending editing a cell by pressing Enter - this fires during
		/// edit.
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Enter)
			{
				int id  = CurrentCell.RowIndex;
				int col = CurrentCell.ColumnIndex;
				CurrentCell = this[col,id];

				EndEdit();

				return true;
			}
			return base.ProcessDialogKey(keyData);
		}
		#endregion Events (override)


		internal void ForceScroll(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
		}
	}
}
