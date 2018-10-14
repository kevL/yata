using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	class YataDataGridView
		:
			DataGridView
	{
		YataForm _f;

		internal string Pfe
		{ get; set; }

		internal string[] Cols // 'Cols' does NOT contain #0 col IDs (so that often needs +1)
		{ get; set; }

		List<string[]> _rows = new List<string[]>();

		bool _loading;

		internal bool Changed
		{ get; set; }

		internal bool CraftInfo
		{ get; set; }


		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="pfe"></param>
		internal YataDataGridView(YataForm f, string pfe)
		{
			_f = f;

			Pfe = pfe;

			Dock = DockStyle.Fill;

			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

			AllowUserToResizeRows = false;

			DrawingControl.SetDoubleBuffered(this);

			CellValueChanged += CellChanged;
			SortCompare      += TableSortCompare;
			Sorted           += TableSortCompared;
		}

		const int LABELS = 2;

		/// <summary>
		/// Tries to load a 2da file.
		/// </summary>
		internal void Load2da()
		{
			Text = "Yata";

//			freeze1stColToolStripMenuItem.Checked =
//			freeze2ndColToolStripMenuItem.Checked = false;

			bool ignoreErrors = false;

			string[] lines = File.ReadAllLines(Pfe);

			Cols = lines[LABELS].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

			int quotes =  0;
			int id     = -1;
			int lineId = -1;

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
								return;

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
							return;

						case DialogResult.Retry:
							break;

						case DialogResult.Ignore:
							ignoreErrors = true;
							break;
					}
				}

				if (lineId > LABELS)
				{
					string[] row = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

					// test for well-formed, consistent IDs
					++id;

					if (!ignoreErrors)
					{
						int result;
						if (!Int32.TryParse(row[0], out result) || result != id)
						{
							string error = "The 2da-file contains an ID that is either not an integer or out of sequence."
										 + Environment.NewLine + Environment.NewLine
										 + Pfe
										 + Environment.NewLine + Environment.NewLine
										 + id + " / " + row[0];
							switch (ShowLoadError(error))
							{
								case DialogResult.Abort:
									return;

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
						string error = "The 2da-file contains fields that do not align with its columns."
									 + Environment.NewLine + Environment.NewLine
									 + Pfe
									 + Environment.NewLine + Environment.NewLine
									 + "id " + id;
						switch (ShowLoadError(error))
						{
							case DialogResult.Abort:
								return;

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
										return;

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
										return;

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
						return;

					case DialogResult.Retry:
						break;

					case DialogResult.Ignore:
						ignoreErrors = true;
						break;
				}
			}

			CraftInfo = (Path.GetFileNameWithoutExtension(Pfe).ToLower() == "crafting");

			if (CraftInfo)
			{
				// load all paths for Crafting from the optional manual settings file
				string pathCfg = Path.Combine(Application.StartupPath, "settings.cfg");
				if (File.Exists(pathCfg))
				{
					using (var fs = File.OpenRead(pathCfg))
					using (var sr = new StreamReader(fs))
					{
						string line;
						while ((line = sr.ReadLine()) != null)
						{
							if (line.StartsWith("pathall=", StringComparison.InvariantCulture)
								&& !String.IsNullOrEmpty(line = line.Substring(8).Trim())
								&& Directory.Exists(line))
							{
								_f.GropeLabels(line);
							}
						}
					}
				}
			}

			_loading = true;
			DrawingControl.SuspendDrawing(this);

			PopulateColumnHeaders();
			PopulateTableRows();

			DrawingControl.ResumeDrawing(this);
			_loading = false;
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


			RowHeadersWidth = 62;
			AutoResizeColumns();
			//AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
		}

		void CellChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!_loading)
			{
				Changed = true;

				int id  = e.RowIndex;
				int col = e.ColumnIndex;

				if (id == Rows.Count - 2)
				{
					RelabelRowHeaders();
				}


				// checks for invalid fields ->

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
			if (e.KeyData == Keys.Enter)
			{
				if (SelectedCells[0] != null)
					CurrentCell = SelectedCells[0];

				if (CurrentCell != null)
					BeginEdit(true);

				e.Handled = true;
			}
			base.OnKeyDown(e);
		}

		/// <summary>
		/// Handles relabeling the row-headers after selecting a row and
		/// pressing the Delete key.
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

				case Keys.F3:
					_f.ToolStripTextBox1KeyUp(null, null);
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
				CurrentCell = this[col, id];

				EndEdit();

				return true;
			}
			return base.ProcessDialogKey(keyData);
		}
#endregion Events (override)


#region Sort
		int _a, _b;

		void TableSortCompare(object sender, DataGridViewSortCompareEventArgs e)
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

		void TableSortCompared(object sender, EventArgs e)
		{
			EnsureVisibleRow();
			RelabelRowHeaders();
		}

		void EnsureVisibleRow()
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
	}
}
