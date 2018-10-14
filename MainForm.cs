using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Yata ....
	/// </summary>
	public partial class MainForm
		:
			Form
	{
#region Fields
		const string STARS = "****";

		string _pfe = String.Empty;

		bool _loading, _changed;
		bool _craftInfo;

		YataDataGridView _table = new YataDataGridView();

		string[] _cols;
		int _colsQty;

		List<string[]> _rows = new List<string[]>();

		int _editId;
		List<string> _copy = new List<string>();

		List<ToolStripItem> _presets = new List<ToolStripItem>();
		string _initialDir = String.Empty;
#endregion Fields


#region Properties
		public static MainForm Instance
		{ get; set; }
#endregion Properties


#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal MainForm()
		{
			InitializeComponent();

			logfile.CreateLog(); // NOTE: The logfile works in debug-builds only.
			// To write a line to the logfile:
			// logfile.Log("what you want to know here");
			//
			// The logfile ought appear in the directory with the executable.

			Instance = this;

			// load an Optional manual settings file
			string pathCfg = Path.Combine(Application.StartupPath, "settings.cfg");
			if (File.Exists(pathCfg))
			{
				using (var fs = File.OpenRead(pathCfg))
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
								_table.Font = tc.ConvertFromInvariantString(line) as Font;
							}
						}
						else if (line.StartsWith("dirpreset=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(10).Trim())
								&& Directory.Exists(line))
							{
								presetFoldersToolStripMenuItem.Visible = true;

								if (_presets.Count == 0)
								{
									var clear = presetFoldersToolStripMenuItem.DropDownItems.Add("clear current");
									_presets.Add(clear);

									clear.Visible = false;
									clear.Click += PresetClick;
								}

								var preset = presetFoldersToolStripMenuItem.DropDownItems.Add(line);
								_presets.Add(preset);

								preset.Click += PresetClick;
							}
						}
					}
				}
			}

			DrawingControl.SetDoubleBuffered(_table);

			toolStripComboBox1.Items.AddRange(new object[]
			{
				"search substring",
				"search wholeword"
			});
			toolStripComboBox1.SelectedIndex = 0;

			// init the Table ->
			panel1.Controls.Add(_table);

			_table.Name = "_table";
			_table.Dock = DockStyle.Fill;

			_table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

			_table.AllowUserToResizeRows = false;

			_table.CellValueChanged    += CellValueChanged;
			_table.RowHeaderMouseClick += RowHeaderContextMenu;
			_table.CellMouseEnter      += PrintCraftInfo;
			_table.KeyUp               += TableSearch;
			_table.SortCompare         += TableSortCompare;
			_table.Sorted              += TableSortCompared;
		}
#endregion cTor


#region File menu
		void MainFormFormClosing(object sender, CancelEventArgs e)
		{
			e.Cancel = _changed
					&& MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
									   "warning",
									   MessageBoxButtons.OKCancel,
									   MessageBoxIcon.Warning,
									   MessageBoxDefaultButton.Button2) == DialogResult.Cancel;
		}

		void QuitToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!_changed
				|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
								   "warning",
								   MessageBoxButtons.OKCancel,
								   MessageBoxIcon.Warning,
								   MessageBoxDefaultButton.Button2) == DialogResult.OK)
			{
				_changed = false;
				Close();
			}
		}

		void CloseToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!_changed
				|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
								   "warning",
								   MessageBoxButtons.OKCancel,
								   MessageBoxIcon.Warning,
								   MessageBoxDefaultButton.Button2) == DialogResult.OK)
			{
				_changed                              =
				_craftInfo                            =
				pathsToolStripMenuItem.Visible        =
				freeze1stColToolStripMenuItem.Checked =
				freeze2ndColToolStripMenuItem.Checked = false;

				_rows         .Clear();
				_table.Columns.Clear();
				_table.Rows   .Clear();

				// NOTE: '_cols' and '_colsQty' could also be cleared but it's not necessary.

				if (sender != null) // ie. is NOT Reload
				{
					_pfe = String.Empty;
					SetTitlebarText();
				}
			}
		}

		// nb. the Create option is disabled in the designer
		void CreateToolStripMenuItemClick(object sender, EventArgs e)
		{}

		void OpenToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!_changed
				|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
								   "warning",
								   MessageBoxButtons.OKCancel,
								   MessageBoxIcon.Warning,
								   MessageBoxDefaultButton.Button2) == DialogResult.OK)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title            = "Select a 2da file";
					ofd.Filter           = "2da files (*.2da)|*.2da|All files (*.*)|*.*";
					ofd.InitialDirectory = _initialDir;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						_pfe = ofd.FileName;
						Load2da();
					}
				}
			}
		}

		void ReloadToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(_pfe))
			{
				CloseToolStripMenuItemClick(null, EventArgs.Empty);
				Load2da();
			}
		}

		const int LABELS = 2;

		/// <summary>
		/// Tries to load a 2da file.
		/// </summary>
		void Load2da()
		{
			Text = "Yata";

			_rows         .Clear();
			_table.Columns.Clear();
			_table.Rows   .Clear();

			_changed                              =
			_craftInfo                            =
			pathsToolStripMenuItem.Visible        =
			freeze1stColToolStripMenuItem.Checked =
			freeze2ndColToolStripMenuItem.Checked = false;

			// NOTE: '_cols' and '_colsQty' could also be cleared but it's not necessary.


			bool ignoreErrors = false;

			string[] lines = File.ReadAllLines(_pfe);

			_cols = lines[LABELS].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
			_colsQty = _cols.Length + 1;

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
									 + _pfe;
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
								 + _pfe;
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
										 + _pfe
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
					if (!ignoreErrors && row.Length != _colsQty)
					{
						string error = "The 2da-file contains fields that do not align with its columns."
									 + Environment.NewLine + Environment.NewLine
									 + _pfe
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
											 + _pfe
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
											 + _pfe
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
							 + _pfe;
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

			_craftInfo                     = // special handling for mouseovers if Crafting.2da is loaded.
			pathsToolStripMenuItem.Visible = (Path.GetFileNameWithoutExtension(_pfe).ToLower() == "crafting");

			if (_craftInfo)
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
								GropeLabels(line);
							}
						}
					}
				}
			}

			_loading = true;
			DrawingControl.SuspendDrawing(_table);

			PopulateColumnHeaders();
			PopulateTableRows();

			_table.AutoResizeColumns();

			DrawingControl.ResumeDrawing(_table);
			_loading = false;

			SetTitlebarText();
		}

		/// <summary>
		/// Sets the titlebar text to something reasonable.
		/// </summary>
		void SetTitlebarText()
		{
			string text = "Yata";

			if (!String.IsNullOrEmpty(_pfe))
			{
				text += " - " + Path.GetFileName(_pfe);

				string path = Path.GetDirectoryName(_pfe);
				if (!String.IsNullOrEmpty(path))
				{
					text += " - " + path;
				}
			}
			Text = text;
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
			_table.ColumnCount = _colsQty;

			_table.Columns[0].Name       =
			_table.Columns[0].HeaderText = "id";
			_table.Columns[0].Frozen     = true;

			int colId = 0;
			foreach (string col in _cols)
			{
				++colId;
				_table.Columns[colId].Name       =
				_table.Columns[colId].HeaderText = col;
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
				_table.Rows.Add(row);
				_table.Rows[++id].HeaderCell.Value = id.ToString(); // label row header

				if (id % 2 == 0)
					color = Color.AliceBlue;
				else
					color = Color.BlanchedAlmond;

				_table.Rows[id].DefaultCellStyle.BackColor = color;

				pb.Step();
			}

			_table.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
		}

		/// <summary>
		/// Relabels the rowheaders when inserting/deleting/sorting rows.
		/// </summary>
		public void RelabelRowHeaders()
		{
			if (_table.Rows.Count > 1) // safety - ought be checked in calling funct.
			{
				_loading = true; // (re)use '_loading' to prevent firing CellChanged events for the RowHeaders

				int rows = _table.Rows.Count - 1;
				for (int id = 0; id != rows; ++id)
				{
					_table.Rows[id].HeaderCell.Value = id.ToString();
				}
				_loading = false;
			}
		}


		void SaveToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(_pfe)
				&& _table.Rows.Count > 1)
			{
				_changed = false;

				using (var sw = new StreamWriter(_pfe))
				{
					sw.WriteLine("2DA V2.0");
					sw.WriteLine("");

					string line = String.Empty;
					foreach (string col in _cols)
					{
						line += " " + col;
					}
					sw.WriteLine(line);

					if (_table.RowCount != 0)
					{
						object val;

						for (int row = 0; row != _table.RowCount - 1; ++row)
						{
							line = String.Empty;

							for (int cell = 0; cell != _table.Rows[row].Cells.Count; ++cell)
							{
								if (cell != 0)
									line += " ";

								val = _table.Rows[row].Cells[cell].Value;
								if (val != null)
									line += val.ToString();
								else
									line += STARS;
							}

							if (!String.IsNullOrEmpty(line))
								sw.WriteLine(line);
						}
					}
				}
			}
		}

		void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_table.Rows.Count > 1)
			{
				using (var sfd = new SaveFileDialog())
				{
					sfd.Title    = "Save as ...";
					sfd.Filter   = "2da files (*.2da)|*.2da|All files (*.*)|*.*";
					sfd.FileName = Path.GetFileName(_pfe);

					if (sfd.ShowDialog() == DialogResult.OK)
					{
						_pfe = sfd.FileName;

						SetTitlebarText();

						SaveToolStripMenuItemClick(null, EventArgs.Empty);
					}
				}
			}
		}

		void CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!_loading)
			{
				_changed = true;

				int id  = e.RowIndex;
				int col = e.ColumnIndex;

				if (id == _table.Rows.Count - 2)
				{
					RelabelRowHeaders();
				}


				// checks for invalid fields ->

				object val = _table.Rows[id].Cells[col].Value;
				if (val == null || String.IsNullOrEmpty(val.ToString()))
				{
					_table.Rows[id].Cells[col].Value = STARS;
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
							_table.Rows[id].Cells[col].Value = STARS;
						}
						else if (field.Length > 2
							&& field.Substring(1, field.Length - 2).Trim() == String.Empty)
						{
							_table.Rows[id].Cells[col].Value = "\"\""; // TODO: this triggers the event a second time ...

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
							_table.Rows[id].Cells[col].Value = field + "\"";
						else
							_table.Rows[id].Cells[col].Value = "\"" + field;

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
							_table.Rows[id].Cells[col].Value = first + fieldTest + last;

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
		/// Handles opening the FileMenu, FolderPresets item and its sub-items.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void FileToolStripMenuItemDropDownOpening(object sender, EventArgs e)
		{
			if (_presets.Count != 0)
			{
				var itDelete = new List<ToolStripItem>();

				foreach (var it in _presets)
				{
					if (it.Text == "clear current")
					{
						it.Visible = !String.IsNullOrEmpty(_initialDir);
					}
					else if (!Directory.Exists(it.Text))
					{
						itDelete.Add(it);
					}
				}

				if (itDelete.Count != 0)
				{
					foreach (var it in itDelete)
					{
						presetFoldersToolStripMenuItem.DropDownItems.Remove(it);
						_presets.Remove(it);
					}
				}

				if (_presets.Count < 2) // ie. no presets or only "clear current" left
				{
					_initialDir = String.Empty;
					presetFoldersToolStripMenuItem.Visible = false;

					presetFoldersToolStripMenuItem.DropDownItems.Clear();
					_presets.Clear();
				}
			}
		}

		/// <summary>
		/// Sets a directory as the initial directory for the FileOpen dialog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void PresetClick(object sender, EventArgs e)
		{
			var it = sender as ToolStripItem;
			if ((_initialDir = it.Text) == "clear current")
				_initialDir = String.Empty;
		}
#endregion File menu


#region Context menu
		void RowHeaderContextMenu(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				_editId = e.RowIndex;
				if (_editId != _table.Rows.Count - 1)
				{
					_table.ClearSelection();
					_table.Rows[_editId].Selected = true;

					_table.CurrentCell = _table.Rows[_editId].Cells[0];

					toolStripMenuItem2.Text = "@ id " + _editId;

					pasteAboveToolStripMenuItem.Enabled =
					pasteToolStripMenuItem     .Enabled =
					pasteBelowToolStripMenuItem.Enabled = (_copy.Count != 0);

					contextMenuStrip1.Show(_table, new Point(_table.RowHeadersWidth - 10,
															 _table.Location.Y      + 10));
				}
			}
		}

		void CopyToolStripMenuItemClick(object sender, EventArgs e)
		{
			_copy.Clear();

			for (int col = 0; col != _colsQty; ++col)
			{
				_copy.Add(_table.Rows[_editId].Cells[col].Value.ToString());
			}
		}

		void CutToolStripMenuItemClick(object sender, EventArgs e)
		{
			CopyToolStripMenuItemClick(null, EventArgs.Empty);
			DeleteToolStripMenuItemClick(null, EventArgs.Empty);

			RelabelRowHeaders();
		}

		void PasteAboveToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_copy.Count == _colsQty)
			{
				_table.Rows.Insert(_editId, _copy.ToArray());

				RelabelRowHeaders();
			}
		}

		void PasteToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_copy.Count == _colsQty)
			{
				for (int col = 0; col != _colsQty; ++col)
				{
					_table.Rows[_editId].Cells[col].Value = _copy[col];
				}
				_table.Rows[_editId].DefaultCellStyle.BackColor = DefaultBackColor;
			}
		}

		void PasteBelowToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_copy.Count == _colsQty)
			{
				_table.Rows.Insert(_editId + 1, _copy.ToArray());

				RelabelRowHeaders();
			}
		}

		void CreateAboveToolStripMenuItemClick(object sender, EventArgs e)
		{
			var row = new string[_colsQty];
			for (int col = 0; col != _colsQty; ++col)
			{
				row[col] = STARS;
			}
			_table.Rows.Insert(_editId, row);

			RelabelRowHeaders();
		}

		void CreateBelowToolStripMenuItemClick(object sender, EventArgs e)
		{
			var row = new string[_colsQty];
			for (int col = 0; col != _colsQty; ++col)
			{
				row[col] = STARS;
			}
			_table.Rows.Insert(_editId + 1, row);

			RelabelRowHeaders();
		}

		void ClearToolStripMenuItemClick(object sender, EventArgs e)
		{
			for (int i = 1; i != _colsQty; ++i)
			{
				_table.Rows[_editId].Cells[i].Value = STARS;
			}
			_table.Rows[_editId].DefaultCellStyle.BackColor = DefaultBackColor;
		}

		void DeleteToolStripMenuItemClick(object sender, EventArgs e)
		{
			_table.Rows.Remove(_table.Rows[_editId]);

			RelabelRowHeaders();
		}
#endregion Context menu


#region Edit menu
		void CheckRowOrderToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_table.Rows.Count > 1)
			{
				var list = new List<string>();

				object val;
				int result;

				bool stop = false;

				int rows = _table.Rows.Count - 1;
				for (int id = 0; id != rows; ++id)
				{
					val = _table.Rows[id].Cells[0].Value;
					if (val == null)
					{
						if (list.Count == 20) // stop this Madness
						{
							stop = true;
							break;
						}
						list.Add("id " + id + " is not valid");
					}
					else if (!Int32.TryParse(val.ToString(), out result))
					{
						if (list.Count == 20)
						{
							stop = true;
							break;
						}
						list.Add("id " + id + " is not an integer");
					}
					else if (result != id)
					{
						if (list.Count == 20)
						{
							stop = true;
							break;
						}
						list.Add("id " + id + " is out of order");
					}
				}

				if (list.Count != 0)
				{
					string info = String.Empty;
					foreach (string it in list)
					{
						info += it + Environment.NewLine;
					}

					if (stop)
					{
						info += Environment.NewLine
							  + "The check has been stopped at 20 borks.";
					}

					info += Environment.NewLine + Environment.NewLine
						  + "Do you want to auto-sequence the ID fields?";

					if (MessageBox.Show(info,
										"burp",
										MessageBoxButtons.YesNo,
										MessageBoxIcon.Exclamation,
										MessageBoxDefaultButton.Button1) == DialogResult.Yes)
					{
						RenumberToolStripMenuItemClick(null, EventArgs.Empty);
					}
				}
				else
					MessageBox.Show("Row order is Okay.",
									"burp",
									MessageBoxButtons.OK,
									MessageBoxIcon.Information,
									MessageBoxDefaultButton.Button1);
			}
		}

		void RenumberToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_table.Rows.Count > 1)
			{
				DrawingControl.SuspendDrawing(_table); // bongo

				var pb = new ProgBar();
				pb.ValTop = _table.Rows.Count - 1;
				pb.Show();

				object val;
				int result;

				int rows = _table.Rows.Count - 1;
				for (int id = 0; id != rows; ++id)
				{
					if ((val = _table.Rows[id].Cells[0].Value) == null
						|| !Int32.TryParse(val.ToString(), out result)
						|| result != id)
					{
						_table.Rows[id].Cells[0].Value = id;
					}
					pb.Step();
				}

				DrawingControl.ResumeDrawing(_table);
			}
		}

		void RecolorToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_table.Rows.Count > 1)
			{
				Color color;

				int rows = _table.Rows.Count - 1;
				for (int id = 0; id != rows; ++id)
				{
					color = (id % 2 == 0) ? Color.AliceBlue
										  : Color.BlanchedAlmond;
					_table.Rows[id].DefaultCellStyle.BackColor = color;
				}
			}
		}
#endregion Edit menu


#region Options menu
		/// <summary>
		/// Opens an output-box with the current table-font as a string for
		/// copying to Settings.Cfg if desired.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ShowCurrentFontStringToolStripMenuItemClick(object sender, EventArgs e)
		{
			var f = new TextOutputBox();
			f.Font = _table.Font;

			TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
			f.SetText("font=" + tc.ConvertToString(_table.Font));

			f.ShowDialog();
		}

		void AutosizeColsToolStripMenuItemClick(object sender, EventArgs e)
		{
			_table.AutoResizeColumns();
		}

		void Freeze1stColToolStripMenuItemClick(object sender, EventArgs e)
		{
			freeze2ndColToolStripMenuItem.Checked = false; // first toggle the freeze2 col off
			if (_table.Columns.Count > 2)
			{
				_table.Columns[2].Frozen = false;
			}

			if (_table.Columns.Count > 1) // then do the freeze1 col
			{
				_table.Columns[1].Frozen              =
				freeze1stColToolStripMenuItem.Checked = !freeze1stColToolStripMenuItem.Checked;
			}
		}

		void Freeze2ndColToolStripMenuItemClick(object sender, EventArgs e)
		{
			freeze1stColToolStripMenuItem.Checked = false; // first toggle the freeze1 col off
			if (_table.Columns.Count > 1)
			{
				_table.Columns[1].Frozen = false;

				if (_table.Columns.Count > 2) // then do the freeze2 col
				{
					_table.Columns[2].Frozen              =
					freeze2ndColToolStripMenuItem.Checked = !freeze2ndColToolStripMenuItem.Checked;
				}
			}
		}
#endregion Options menu


#region Font
		Font _font;
		bool _fontChanged;

		bool _fontWarned;

		void FontToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_fontWarned
				|| MessageBox.Show("Be patient – changing the font on a table with several"
								   + " thousand rows can take several seconds or longer."
								   + Environment.NewLine + Environment.NewLine
								   + "Further, only legitimately valid TrueType fonts are"
								   + " indeed valid. There are fonts that masquerade as TrueType"
								   + " but are not valid – such fonts will crash the application."
								   + " You should cancel and save any progress first if you're"
								   + " unsure about a font that you may apply or okay in the"
								   + " Font Dialog that appears if you choose to proceed."
								   + Environment.NewLine + Environment.NewLine + Environment.NewLine
								   + "– this warning will not be shown again during this run of"
								   + " the application.",
								   "beware, Grasshoppar",
								   MessageBoxButtons.OKCancel,
								   MessageBoxIcon.Warning,
								   MessageBoxDefaultButton.Button1) == DialogResult.OK)
			{
				_fontChanged = false;

				_font            =
				fontDialog1.Font = _table.Font;

				if (fontDialog1.ShowDialog() == DialogResult.OK)
				{
					if (!_table.Font.Equals(fontDialog1.Font))
					{
						_table.Font = fontDialog1.Font;
						_table.AutoResizeColumns();
					}
				}
				else if (_fontChanged)
				{
					_table.Font = _font;
					_table.AutoResizeColumns();
				}
			}
			_fontWarned = true;
		}

		void FontDialog1Apply(object sender, EventArgs e)
		{
			if (!_table.Font.Equals(fontDialog1.Font))
			{
				_fontChanged = true;
				_table.Font = fontDialog1.Font;
				_table.AutoResizeColumns();
			}
		}
#endregion Font


#region Crafting info
		/// <summary>
		/// Mouseover datacells prints info to the statusbar if Crafting.2da is
		/// loaded.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void PrintCraftInfo(object sender, DataGridViewCellEventArgs e)
		{
			int id  = e.RowIndex;
			int col = e.ColumnIndex;

			toolStripStatusLabel1.Text = "id= " + id + " col= " + col;

			if (_craftInfo)
			{
				string info = String.Empty;

				if (   id  > -1 && id  < _table.Rows.Count - 1
					&& col > -1 && col < _table.Columns.Count)
				{
					object val; int result;

					switch (col)
					{
//						case -1: // row-header
//						case  0: // id

						case  1: // "CATEGORY"
							if (pathSpells2daToolStripMenuItem.Checked)
							{
								if ((val = _table.Rows[id].Cells[col].Value) != null
									&& Int32.TryParse(val.ToString(), out result)
									&& result < CraftInfo.spellLabels.Count)
								{
									info = _table.Columns[col].HeaderCell.Value + ": "
										 + CraftInfo.spellLabels[result];
								}
							}
							break;

//						case  2: // "REAGENTS"

						case  3: // "TAGS"
							if ((val = _table.Rows[id].Cells[col].Value) != null)
							{
								string tags = val.ToString();
								if (tags.StartsWith("B", StringComparison.InvariantCulture)) // is in BaseItems.2da
								{
									if (pathBaseItems2daToolStripMenuItem.Checked)
									{
										info = _table.Columns[col].HeaderCell.Value + ": (base) ";

										string[] array = tags.Substring(1).Split(','); // lose the "B"
										for (int tag = 0; tag != array.Length; ++tag)
										{
											if (Int32.TryParse(array[tag], out result)
												&& result < CraftInfo.tagLabels.Count)
											{
												info += CraftInfo.tagLabels[result];

												if (tag != array.Length - 1)
													info += ", ";
											}
										}
									}
								}
								else // is a TCC item-type
								{
									info = _table.Columns[col].HeaderCell.Value + ": (tcc) ";

									if (tags == STARS)
									{
										info += CraftInfo.GetTccType(0); // TCC_TYPE_NONE
									}
									else
									{
										string[] array = tags.Split(',');
										for (int tag = 0; tag != array.Length; ++tag)
										{
											if (Int32.TryParse(array[tag], out result))
											{
												info += CraftInfo.GetTccType(result);

												if (tag != array.Length - 1)
													info += ", ";
											}
										}
									}
								}
							}
							break;

						case  4: // "EFFECTS"
							if (pathItemPropDef2daToolStripMenuItem.Checked)
							{
								if ((val = _table.Rows[id].Cells[col].Value) != null)
								{
									string effects = val.ToString();
									if (effects != STARS)
									{
										info = _table.Columns[col].HeaderCell.Value + ": ";

										string par = String.Empty;
										int pos;

										string[] ips = effects.Split(';');
										for (int ip = 0; ip != ips.Length; ++ip)
										{
											par = ips[ip];
											if ((pos = par.IndexOf(',')) != -1)
											{
												if (Int32.TryParse(par.Substring(0, pos), out result)
													&& result < CraftInfo.ipLabels.Count)
												{
													info += CraftInfo.ipLabels[result]
														  + CraftInfo.GetEncodedParsDescription(par);

													if (ip != ips.Length - 1)
														info += ", ";
												}
											}
											else // is a PropertySet preparation val.
											{
												info += "PropertySet val=" + par; // TODO: description for par.
											}
										}
									}
								}
							}
							break;

//						case  5: // "OUTPUT"

						case  6: // "SKILL"
							if (pathFeat2daToolStripMenuItem.Checked)
							{
								if ((val = _table.Rows[id].Cells[col].Value) != null
									&& Int32.TryParse(val.ToString(), out result))
								{
									object cat = _table.Rows[id].Cells[1].Value;
									if (cat != null)
									{
										int result2;
										if (Int32.TryParse(cat.ToString(), out result2)) // is triggered by spell id
										{
											if (result < CraftInfo.featsLabels.Count)
											{
												info = _table.Columns[col].HeaderCell.Value + ": "
													 + CraftInfo.featsLabels[result];
											}
										}
										else // is triggered NOT by spell - but by mold-tag, or is Alchemy or Distillation
										{
											if (result < CraftInfo.skillLabels.Count)
											{
												info = _table.Columns[col].HeaderCell.Value + ": "
													 + CraftInfo.skillLabels[result];
											}
										}
									}
								}
							}
							break;

//						case  7: // "LEVEL"
//						case  8: // "EXCLUDE"
//						case  9: // "XP"
//						case 10: // "GP"
//						case 11: // "DISABLE"
					}
				}

				toolStripStatusLabel2.Text = info;
			}
		}

		/// <summary>
		/// Handles clicking the PathSpells menuitem.
		/// Intended to add labels from Spells.2da to the 'spellLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void PathSpells2daToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!pathSpells2daToolStripMenuItem.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Spells.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						CraftInfo.GropeLabels(ofd.FileName,
											  CraftInfo.spellLabels,
											  pathSpells2daToolStripMenuItem,
											  1);
					}
				}
			}
			else
			{
				pathSpells2daToolStripMenuItem.Checked = false;
				CraftInfo.spellLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathFeat menuitem.
		/// Intended to add labels from Feat.2da to the 'featsLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void PathFeat2daToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!pathFeat2daToolStripMenuItem.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Feat.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						CraftInfo.GropeLabels(ofd.FileName,
											  CraftInfo.featsLabels,
											  pathFeat2daToolStripMenuItem,
											  1);
					}
				}
			}
			else
			{
				pathFeat2daToolStripMenuItem.Checked = false;
				CraftInfo.featsLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathItemPropDef menuitem.
		/// Intended to add labels from ItemPropDef.2da to the 'ipLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void PathItemPropDef2daToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!pathItemPropDef2daToolStripMenuItem.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select ItemPropDef.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						CraftInfo.GropeLabels(ofd.FileName,
											  CraftInfo.ipLabels,
											  pathItemPropDef2daToolStripMenuItem,
											  2);
					}
				}
			}
			else
			{
				pathItemPropDef2daToolStripMenuItem.Checked = false;
				CraftInfo.ipLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathBaseItems menuitem.
		/// Intended to add labels from BaseItems.2da to the 'tagLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void PathBaseItems2daToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!pathBaseItems2daToolStripMenuItem.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select BaseItems.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						CraftInfo.GropeLabels(ofd.FileName,
											  CraftInfo.tagLabels,
											  pathBaseItems2daToolStripMenuItem,
											  2);
					}
				}
			}
			else
			{
				pathBaseItems2daToolStripMenuItem.Checked = false;
				CraftInfo.tagLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathSkills menuitem.
		/// Intended to add labels from Skills.2da to the 'skillLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void PathSkills2daToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!pathSkills2daToolStripMenuItem.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Skills.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						CraftInfo.GropeLabels(ofd.FileName,
											  CraftInfo.skillLabels,
											  pathSkills2daToolStripMenuItem,
											  1);
					}
				}
			}
			else
			{
				pathSkills2daToolStripMenuItem.Checked = false;
				CraftInfo.skillLabels.Clear();
			}
		}

		void PathAllToolStripMenuItemClick(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				fbd.ShowNewFolderButton = false;
				fbd.Description = "Find a folder to search through 2da-files for"
								+ " extra information about Crafting.2da";

				if (fbd.ShowDialog() == DialogResult.OK)
				{
					GropeLabels(fbd.SelectedPath);
				}
			}
		}

		void GropeLabels(string directory)
		{
			CraftInfo.GropeLabels(Path.Combine(directory, "baseitems.2da"),
								  CraftInfo.tagLabels,
								  pathBaseItems2daToolStripMenuItem,
								  2);

			CraftInfo.GropeLabels(Path.Combine(directory, "feat.2da"),
								  CraftInfo.featsLabels,
								  pathFeat2daToolStripMenuItem,
								  1);

			CraftInfo.GropeLabels(Path.Combine(directory, "itempropdef.2da"),
								  CraftInfo.ipLabels,
								  pathItemPropDef2daToolStripMenuItem,
								  2);

			CraftInfo.GropeLabels(Path.Combine(directory, "skills.2da"),
								  CraftInfo.skillLabels,
								  pathSkills2daToolStripMenuItem,
								  1);

			CraftInfo.GropeLabels(Path.Combine(directory, "spells.2da"),
								  CraftInfo.spellLabels,
								  pathSpells2daToolStripMenuItem,
								  1);



			CraftInfo.GropeLabels(Path.Combine(directory, "classes.2da"),
								  CraftInfo.classLabels,
								  pathClasses2daToolStripMenuItem,
								  1);

			CraftInfo.GropeLabels(Path.Combine(directory, "disease.2da"),
								  CraftInfo.diseaseLabels,
								  pathDisease2daToolStripMenuItem,
								  1);

			CraftInfo.GropeLabels(Path.Combine(directory, "iprp_feats.2da"),
								  CraftInfo.ipfeatsLabels,
								  pathIprpFeats2daToolStripMenuItem,
								  2);

			CraftInfo.GropeLabels(Path.Combine(directory, "iprp_spells.2da"),
								  CraftInfo.ipspellsLabels,
								  pathIprpSpells2daToolStripMenuItem,
								  1, // label
								  3, // level
								  CraftInfo.ipspellsLevels);

			CraftInfo.GropeLabels(Path.Combine(directory, "iprp_onhitspell.2da"),
								  CraftInfo.iphitspellLabels,
								  pathIprpOnHitSpell2daToolStripMenuItem,
								  1);

			CraftInfo.GropeLabels(Path.Combine(directory, "racialtypes.2da"),
								  CraftInfo.raceLabels,
								  pathRaces2daToolStripMenuItem,
								  1);
		}
#endregion Crafting info


#region Search
		void ToolStripTextBox1KeyUp(object sender, KeyEventArgs e)
		{
			if (_table.Rows.Count > 1
				&& (sender == null || e.KeyCode == Keys.Enter))
			{
				string search = toolStripTextBox1.Text;
				if (!String.IsNullOrEmpty(search))
				{
					search = search.ToLower();

					int startId  = _table.CurrentCell.RowIndex;
					int startCol = _table.CurrentCell.ColumnIndex;

					if (startId  == -1) startId  = 0;
					if (startCol == -1) startCol = 0;

					object val;

					int id;
					int col;

					bool start = true;
					bool stop = false;

					string st;

					for (id = startId; id != _table.Rows.Count - 1 && !stop; ++id)
					{
						if (start)
						{
							start = false;
							col = startCol + 1;
						}
						else
							col = 0;

						if (col == _table.Columns.Count)
							col = 0;

						for (; col != _table.Columns.Count && !stop; ++col)
						{
							val = _table.Rows[id].Cells[col].Value;
							if (val != null)
							{
								st = val.ToString().ToLower();
								if (   (toolStripComboBox1.SelectedIndex == 0 && st.Contains(search))
									|| (toolStripComboBox1.SelectedIndex == 1 && st == search))
								{
									_table.CurrentCell = _table.Rows[id].Cells[col];
									stop = true;
								}
							}
						}
					}
				}
			}
		}

		void TableSearch(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F3)
			{
				ToolStripTextBox1KeyUp(null, null);
			}
		}

		void ToolStripComboBox1KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F3 || e.KeyCode == Keys.Enter)
			{
				ToolStripTextBox1KeyUp(null, null);
			}
		}
#endregion Search


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

			if (e.Column != _table.Columns[0] // secondary sort on id ->
				&& e.CellValue1.ToString() == e.CellValue2.ToString())
			{
				if (   Int32.TryParse(_table.Rows[e.RowIndex1].Cells[0].Value.ToString(), out _a)
					&& Int32.TryParse(_table.Rows[e.RowIndex2].Cells[0].Value.ToString(), out _b))
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

			if (_table.SelectedRows.Count != 0)
			{
				sel = _table.SelectedRows[0].Index;
			}
			else if (_table.SelectedCells.Count != 0)
			{
				sel = _table.SelectedCells[0].RowIndex;
			}

			if (sel != -1 && sel < _table.RowCount - 1)
			{
				int visFirst = _table.FirstDisplayedScrollingRowIndex;
				if (sel < visFirst)
				{
					_table.FirstDisplayedScrollingRowIndex = sel;
				}
				else
				{
					int visCount = _table.DisplayedRowCount(false);
					if (sel >= visFirst + visCount)
					{
						_table.FirstDisplayedScrollingRowIndex = sel - visCount + 1;
					}
				}
			}
		}
#endregion Sort
	}


	// https://stackoverflow.com/questions/21873361/datagridview-enter-key-event-handling#answer-21882188
	class YataDataGridView
		:
			DataGridView
	{
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
			if (e.KeyData == Keys.Delete)
			{
				MainForm.Instance.RelabelRowHeaders();
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
	}
}
