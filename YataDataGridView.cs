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
		internal enum Frozen
		{
			FreezeOff,
			FreezeFirstCol,
			FreezeSecondCol
		}


		YataForm _f;

		internal string Pfe
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

			CellValueChanged += CellChanged;
			SortCompare      += TableSortCompare;
			Sorted           += TableSortCompared;
			CellPainting     += PaintCell;

			Freeze = Frozen.FreezeOff;
		}

		void PaintCell(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex != -1 && e.ColumnIndex != -1)
			{
				var sf           = new StringFormat(StringFormat.GenericDefault);
//				sf.Alignment     = StringAlignment.Center;
//				sf.LineAlignment = StringAlignment.Center;
//				sf.FormatFlags   = StringFormatFlags.NoClip;

				e.PaintBackground(e.ClipBounds, true);
				e.Graphics.DrawString(Convert.ToString(e.FormattedValue),
									  e.CellStyle.Font,
									  Brushes.Black,
									  e.CellBounds.X,
									  e.CellBounds.Y + 4,
									  sf);
				e.Handled = true;
			}
		}

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

			foreach (string line in lines)
			{
				//logfile.Log("line= " + line);

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
//					string[] row = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
					string[] row = ParseLine(line);

					// test for well-formed, consistent IDs
					++id;

					if (!ignoreErrors)
					{
						int result;
						if (!Int32.TryParse(row[0], out result) || result != id)
						{
							string error = "The 2da-file contains an ID that is not an integer or is out of sequence."
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

//			SizeCols();

			DrawingControl.ResumeDrawing(this);
			_loading = false;

			return true;
		}

/*		/// <summary>
		/// An attempt to layout the cols according to text-widths.
		/// Note that although this is a couple of seconds faster on large 2das
		/// I haven't been able to find an accurate text-measurer ... so will
		/// use the slower but more consistent AutoResizeColumns() funct - see
		/// YataForm.CreateTabPage() - which tends to overestimate the required
		/// width unfortunately.
		/// TODO: This might work if PaintCell() can be designed to use exactly
		/// the same parameters as the text-measurement here.
		/// </summary>
		void SizeCols()
		{
			int rows = Rows.Count;
			if (rows > 1)
			{
				int cols = Columns.Count;
				var widths = new int[cols];

				int width, widthTest;

				// unfortunately, TextRenderer measures alphabetical text too wide
				// and numeric text too short.
//				var font = new Font(Font.ToString(), Font.SizeInPoints + 1);
				var font = Font;

//				var image = new Bitmap(1,1);
//				var graphics = Graphics.FromImage(image);
//				var graphics = Graphics.FromHwnd();

				//Graphics g=Graphics.FromHwnd(YOUR CONTROL HERE.Handle);
				//SizeF s=g.MeasureString("YOUR STRING HERE", Font, NULL, NULL, STRING LENGTH HERE, 1)

//				var dc = new IDeviceContext();
//				IntPtr ptr = dc.GetHdc();

				object val;
				for (int col = 0; col != cols; ++col)
				{
					if (col == 0)
//						width = TextRenderer.MeasureText(dc, "id", font).Width;
//						width = TextRenderer.MeasureText("id", font, new Size(Int32.MaxValue, Int32.MaxValue), TextFormatFlags.NoClipping).Width;
						width = TextRenderer.MeasureText("id", font).Width;
//						width = Convert.ToInt32(graphics.MeasureString("id", font).Width);
					else
//						width = TextRenderer.MeasureText(dc, Columns[col].HeaderText, font).Width;
//						width = TextRenderer.MeasureText(Columns[col].HeaderText, font, new Size(Int32.MaxValue, Int32.MaxValue), TextFormatFlags.NoClipping).Width;
						width = TextRenderer.MeasureText(Columns[col].HeaderText, font).Width;
//						width = Convert.ToInt32(graphics.MeasureString(Columns[col].HeaderText, font).Width);

					width += 15; // pad
//					width += width / 2; // +50%

					if (width < 25)
						width = 25;

					for (int id = 0; id != rows - 1; ++id) // NOTE: Does not include col-headers.
					{
						if ((val = Rows[id].Cells[col].Value) != null)
						{
							widthTest = TextRenderer.MeasureText(val.ToString(), font).Width;
							widthTest += 20; // pad
//							widthTest += width / 5; // +20%
							if (widthTest > width)
								width = widthTest;
						}
					}
					widths[col] = width;
				}

				for (int col = 0; col != cols; ++col)
				{
					Columns[col].Width = widths[col];
				}
//				dc.ReleaseHdc();
			}
		} */

		/// <summary>
		/// TODO: optimize.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		string[] ParseLine(string line)
		{
			var list  = new List<string>();
			var field = new List<char>();

			bool add = false;
			bool inQuotes = false;

			char c;

			int posLast = line.Length + 1; // include an extra iteration to get the last field (that has no whitespace after it)
			for (int pos = 0; pos != posLast; ++pos)
			{
				if (add && pos == line.Length)				// hit lineend: make sure to catch the last field
				{											// if there's no whitespace after it (last fields
					list.Add(new string(field.ToArray()));	// w/ trailing whitespace are dealt with below)
				}
				else if (pos != line.Length)
				{
					c = line[pos];

					if (c == '"' || inQuotes)				// start or continue quotation
					{
						add = true;
						if (inQuotes && c == '"')			// end quotation
						{
							inQuotes = false;
						}
						else
							inQuotes = true;

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


			RowHeadersWidth = 72;
//			AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
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


		internal void ForceScroll(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
		}


#region Sort
		int _a, _b;

		/// <summary>
		/// Sorts fields as integers iff they convert to integers and performs
		/// a secondary sort against their IDs if applicable.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Ensures that the searched-for field is displayed and re-orders the
		/// row-headers.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void TableSortCompared(object sender, EventArgs e)
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
			RelabelRowHeaders();
		}
#endregion Sort
	}
}
