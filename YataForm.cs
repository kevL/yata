using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	static class Constants
	{
		internal const string Stars = "****";
	}


	/// <summary>
	/// Yata ....
	/// </summary>
	public partial class YataForm
		:
			Form
	{
#region Fields
		YataDataGridView _table;

		List<string> _copy = new List<string>();

		List<ToolStripItem> _presets = new List<ToolStripItem>();
		string _initialDir = String.Empty;
#endregion Fields


#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal YataForm()
		{
			InitializeComponent();

			// IMPORTANT: The Client-area apart from the Menubar and Statusbar
			// has both a TabControl and a Panel that *overlap* each other and
			// fill the area. The Panel is on top and is used only to color the
			// Client-area (else TabControl is pure white) - it is shown when
			// there are no TabPages and hides when there are.

			logfile.CreateLog(); // NOTE: The logfile works in debug-builds only.
			// To write a line to the logfile:
			// logfile.Log("what you want to know here");
			//
			// The logfile ought appear in the directory with the executable.

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
								Font = tc.ConvertFromInvariantString(line) as Font;
							}
						}
						else if (line.StartsWith("dirpreset=", StringComparison.InvariantCulture))
						{
							if (!String.IsNullOrEmpty(line = line.Substring(10).Trim())
								&& Directory.Exists(line))
							{
								it_Folders.Visible = true;

								if (_presets.Count == 0)
								{
									var clear = it_Folders.DropDownItems.Add("clear current");
									_presets.Add(clear);

									clear.Visible = false;
									clear.Click += PresetClick;
								}

								var preset = it_Folders.DropDownItems.Add(line);
								_presets.Add(preset);

								preset.Click += PresetClick;
							}
						}
					}
				}
			}

			cb_SearchOption.Items.AddRange(new object[]
			{
				"search substring",
				"search wholeword"
			});
			cb_SearchOption.SelectedIndex = 0;
		}
#endregion cTor


#region File menu
		void OpenToolStripMenuItemClick(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.Title            = "Select a 2da file";
				ofd.Filter           = "2da files (*.2da)|*.2da|All files (*.*)|*.*";
				ofd.InitialDirectory = _initialDir;

				if (ofd.ShowDialog() == DialogResult.OK)
				{
					CreateTabPage(ofd.FileName);
				}
			}
		}

		void CreateTabPage(string pfe)
		{
			panel_ColorFill.Show();

			var table = new YataDataGridView(this, pfe);
			if (table.Load2da())
			{
				table.RowHeaderMouseClick += RowHeaderContextMenu;
				table.CellMouseEnter      += PrintCraftInfo;

				var page = new TabPage();
				tabControl.TabPages.Add(page);

				page.Tag = table;
				page.Text = Path.GetFileNameWithoutExtension(pfe);

				page.Controls.Add(table);

				tabControl.SelectedTab = page;

				table.Select(); // focus the table.

				_table = table; // NOTE: Is done also in TabControl1SelectedIndexChanged()
				AutosizeColsToolStripMenuItemClick(null, EventArgs.Empty); // that works ... finally. so to speak
			}

			TabControl1SelectedIndexChanged(null, EventArgs.Empty);
		}


		/// <summary>
		/// Handles tab-selection/deselection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void TabControl1SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl.SelectedIndex != -1)
			{
				_table = tabControl.SelectedTab.Tag as YataDataGridView; // <- very Important <--

				it_MenuPaths.Visible = _table.CraftInfo;
				panel_ColorFill.Hide();

				it_freeze1.Checked = (_table.Freeze == YataDataGridView.Frozen.FreezeFirstCol);
				it_freeze2.Checked = (_table.Freeze == YataDataGridView.Frozen.FreezeSecondCol);
			}
			else
			{
				it_MenuPaths.Visible = false;
				panel_ColorFill.Show();

				it_freeze1.Checked =
				it_freeze2.Checked = false;
			}

			SetTitlebarText();
		}

		/// <summary>
		/// Draws the tab-text in Bold iff selected.
		/// @note Has the peculiar side-effect of dropping the text to the
		/// bottom of the tab-rect. But I like it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void TabControl1DrawItem(object sender, DrawItemEventArgs e)
		{
			var page = tabControl.TabPages[e.Index];

			FontStyle style = (page == tabControl.SelectedTab) ? FontStyle.Bold
															   : FontStyle.Regular;
			var font = new Font(Font, style);

			var sf           = new StringFormat();
			sf.Alignment     = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Far;

			e.Graphics.DrawString(page.Text,
								  font,
								  Brushes.Black,
								  e.Bounds,
								  sf);
		}

		void CloseToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_table != null
				&& (!_table.Changed
					|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
									   "warning",
									   MessageBoxButtons.YesNo,
									   MessageBoxIcon.Warning,
									   MessageBoxDefaultButton.Button2) == DialogResult.Yes))
			{
				tabControl.TabPages.Remove(tabControl.SelectedTab);

				if (tabControl.TabPages.Count == 0)
					_table = null;

				SetTitlebarText();
			}
		}

		void QuitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close(); // let MainFormFormClosing() handle it ...
		}

		void MainFormFormClosing(object sender, CancelEventArgs e)
		{
			var tables = GetChangedTables();
			if (tables.Count != 0)
			{
				string info = String.Empty;
				foreach (string table in tables)
				{
					info += table + Environment.NewLine;
				}

				e.Cancel = MessageBox.Show("Data has changed."
										   + Environment.NewLine + Environment.NewLine
										   + info
										   + Environment.NewLine
										   + "Okay to exit ...",
										   "warning",
										   MessageBoxButtons.YesNo,
										   MessageBoxIcon.Warning,
										   MessageBoxDefaultButton.Button2) == DialogResult.No;
			}
		}

		List<string> GetChangedTables()
		{
			var changed = new List<string>();

			foreach (TabPage page in tabControl.TabPages)
			{
				var table = page.Tag as YataDataGridView;
				if (table != null && table.Changed)
				{
					changed.Add(Path.GetFileNameWithoutExtension(table.Pfe));
				}
			}
			return changed;
		}


		void ReloadToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_table != null && File.Exists(_table.Pfe)
				&& (!_table.Changed
					|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
									   "warning",
									   MessageBoxButtons.YesNo,
									   MessageBoxIcon.Warning,
									   MessageBoxDefaultButton.Button2) == DialogResult.Yes))
			{
				_table.Changed = false;
	
				_table.Columns.Clear();
				_table.Rows   .Clear();

				_table.Load2da();
			}
			// TODO: Show an error if file no longer exists.
		}

		// nb. the Create option is disabled in the designer
		void CreateToolStripMenuItemClick(object sender, EventArgs e)
		{}


		/// <summary>
		/// Sets the titlebar text to something reasonable.
		/// </summary>
		void SetTitlebarText()
		{
			string text = "Yata";

			if (tabControl.SelectedIndex != -1)
			{
				var table = tabControl.SelectedTab.Tag as YataDataGridView;
				if (table != null)
				{
					string pfe = table.Pfe;
					text += " - " + Path.GetFileName(pfe);

					string path = Path.GetDirectoryName(pfe);
					if (!String.IsNullOrEmpty(path))
					{
						text += " - " + path;
					}
				}
			}
			Text = text;
		}


		void SaveToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_table != null)
			{
				string pfe = _table.Pfe;

				if (!String.IsNullOrEmpty(pfe))
				{
					int rows = _table.Rows.Count;
					if (rows > 1)
					{
						_table.Changed = false;

						using (var sw = new StreamWriter(pfe))
						{
							sw.WriteLine("2DA V2.0");
							sw.WriteLine("");

							string line = String.Empty;
							foreach (string col in _table.Cols)
							{
								line += " " + col;
							}
							sw.WriteLine(line);


							object val;

							int cols = _table.Columns.Count;

							for (int row = 0; row != rows - 1; ++row)
							{
								line = String.Empty;

								for (int cell = 0; cell != cols; ++cell)
								{
									if (cell != 0)
										line += " ";

									if ((val = _table.Rows[row].Cells[cell].Value) != null)
										line += val.ToString();
									else
										line += Constants.Stars;
								}

								sw.WriteLine(line);
							}
						}
					}
				}
			}
		}

		void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_table != null && _table.Rows.Count > 1)
			{
				using (var sfd = new SaveFileDialog())
				{
					sfd.Title    = "Save as ...";
					sfd.Filter   = "2da files (*.2da)|*.2da|All files (*.*)|*.*";
					sfd.FileName = Path.GetFileName(_table.Pfe);

					if (sfd.ShowDialog() == DialogResult.OK)
					{
						_table.Pfe = sfd.FileName;

						SetTitlebarText();

						SaveToolStripMenuItemClick(null, EventArgs.Empty);
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
						it_Folders.DropDownItems.Remove(it);
						_presets.Remove(it);
					}
				}

				if (_presets.Count < 2) // ie. no presets or only "clear current" left
				{
					_initialDir = String.Empty;
					it_Folders.Visible = false;

					it_Folders.DropDownItems.Clear();
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
				if (e.RowIndex != _table.Rows.Count - 1)
				{
					_table.ClearSelection();
					_table.Rows[e.RowIndex].Selected = true;

					_table.CurrentCell = _table.Rows[e.RowIndex].Cells[0];

					context_it_Header.Text = "@ id " + e.RowIndex;

					context_it_PasteAbove.Enabled =
					context_it_Paste     .Enabled =
					context_it_PasteBelow.Enabled = (_copy.Count != 0);

					contextEditor.Show(_table, new Point(_table.RowHeadersWidth - 10,
															 _table.Location.Y      + 10));
				}
			}
		}

		void CopyToolStripMenuItemClick(object sender, EventArgs e)
		{
			_copy.Clear();

			int cols = _table.Columns.Count;
			for (int col = 0; col != cols; ++col)
			{
				_copy.Add(_table.SelectedRows[0].Cells[col].Value.ToString());
			}
		}

		void CutToolStripMenuItemClick(object sender, EventArgs e)
		{
			CopyToolStripMenuItemClick(null, EventArgs.Empty);
			DeleteToolStripMenuItemClick(null, EventArgs.Empty);

			_table.RelabelRowHeaders();
		}

		void PasteAboveToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_copy.Count == _table.Columns.Count)
			{
				_table.Rows.Insert(_table.SelectedRows[0].Index, _copy.ToArray());
				_table.RelabelRowHeaders();
			}
		}

		void PasteToolStripMenuItemClick(object sender, EventArgs e)
		{
			int cols = _table.Columns.Count;
			if (_copy.Count == cols)
			{
				for (int col = 0; col != cols; ++col)
				{
					_table.SelectedRows[0].Cells[col].Value = _copy[col];
				}
				_table.SelectedRows[0].DefaultCellStyle.BackColor = DefaultBackColor;
			}
		}

		void PasteBelowToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_copy.Count == _table.Columns.Count)
			{
				_table.Rows.Insert(_table.SelectedRows[0].Index + 1, _copy.ToArray());
				_table.RelabelRowHeaders();
			}
		}

		void CreateAboveToolStripMenuItemClick(object sender, EventArgs e)
		{
			int cols = _table.Columns.Count;

			var row = new string[cols];
			for (int col = 0; col != cols; ++col)
			{
				row[col] = Constants.Stars;
			}
			_table.Rows.Insert(_table.SelectedRows[0].Index, row);
			_table.RelabelRowHeaders();
		}

		void CreateBelowToolStripMenuItemClick(object sender, EventArgs e)
		{
			int cols = _table.Columns.Count;

			var row = new string[cols];
			for (int col = 0; col != cols; ++col)
			{
				row[col] = Constants.Stars;
			}
			_table.Rows.Insert(_table.SelectedRows[0].Index + 1, row);
			_table.RelabelRowHeaders();
		}

		void ClearToolStripMenuItemClick(object sender, EventArgs e)
		{
			int cols = _table.Columns.Count;
			for (int col = 1; col != cols; ++col)
			{
				_table.SelectedRows[0].Cells[col].Value = Constants.Stars;
			}
			_table.SelectedRows[0].DefaultCellStyle.BackColor = DefaultBackColor;
		}

		void DeleteToolStripMenuItemClick(object sender, EventArgs e)
		{
			_table.Rows.Remove(_table.SelectedRows[0]);
			_table.RelabelRowHeaders();
		}
#endregion Context menu


#region Edit menu
		void CheckRowOrderToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_table != null && _table.Rows.Count > 1)
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
			if (_table != null && _table.Rows.Count > 1)
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
			if (_table != null && _table.Rows.Count > 1)
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
			var f = new TextCopyForm();
			f.Font = Font;

			TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
			f.SetText("font=" + tc.ConvertToString(Font));

			f.ShowDialog();
		}

		void AutosizeColsToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_table != null)
				_table.AutoResizeColumns();
		}

		void Freeze1stColToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_table != null)
			{
				if (_table.Columns.Count > 1)
				{
					if (_table.Columns.Count > 2)
					{
						it_freeze2.Checked = false; // first toggle the freeze2 col off
						_table.Columns[2].Frozen = false;
					}

					if (!it_freeze1.Checked)
					{
						_table.Freeze = YataDataGridView.Frozen.FreezeFirstCol;
					}
					else
						_table.Freeze = YataDataGridView.Frozen.FreezeOff;

					it_freeze1.Checked = // then do the freeze1 col
					_table.Columns[1].Frozen = (_table.Freeze == YataDataGridView.Frozen.FreezeFirstCol);
				}
			}
		}

		void Freeze2ndColToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_table != null)
			{
				if (_table.Columns.Count > 1)
				{
					it_freeze1.Checked = false; // first toggle the freeze1 col off
					_table.Columns[1].Frozen = false;

					if (_table.Columns.Count > 2)
					{
						if (!it_freeze2.Checked)
						{
							_table.Freeze = YataDataGridView.Frozen.FreezeSecondCol;
						}
						else
							_table.Freeze = YataDataGridView.Frozen.FreezeOff;

						it_freeze2.Checked = // then do the freeze2 col
						_table.Columns[2].Frozen = (_table.Freeze == YataDataGridView.Frozen.FreezeSecondCol);
					}
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
				dialog_Font.Font = Font;

				if (dialog_Font.ShowDialog() == DialogResult.OK)
				{
					if (!Font.Equals(dialog_Font.Font))
					{
						Font = dialog_Font.Font;
						if (_table != null)
							_table.AutoResizeColumns();
					}
				}
				else if (_fontChanged)
				{
					Font = _font;
					if (_table != null)
						_table.AutoResizeColumns();
				}
			}
			_fontWarned = true;
		}

		void FontDialog1Apply(object sender, EventArgs e)
		{
			if (!Font.Equals(dialog_Font.Font))
			{
				_fontChanged = true;
				Font = dialog_Font.Font;
				if (_table != null)
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

			statusbar_label_Coords.Text = "id= " + id + " col= " + col;

			if (_table != null && _table.CraftInfo)
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
							if (it_PathSpells2da.Checked)
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
									if (it_PathBaseItems2da.Checked)
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

									if (tags == Constants.Stars)
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
							if (it_PathItemPropDef2da.Checked)
							{
								if ((val = _table.Rows[id].Cells[col].Value) != null)
								{
									string effects = val.ToString();
									if (effects != Constants.Stars)
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
							if (it_PathFeat2da.Checked)
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

				statusbar_label_CraftInfo.Text = info;
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
			if (!it_PathSpells2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Spells.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						CraftInfo.GropeLabels(ofd.FileName,
											  CraftInfo.spellLabels,
											  it_PathSpells2da,
											  1);
					}
				}
			}
			else
			{
				it_PathSpells2da.Checked = false;
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
			if (!it_PathFeat2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Feat.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						CraftInfo.GropeLabels(ofd.FileName,
											  CraftInfo.featsLabels,
											  it_PathFeat2da,
											  1);
					}
				}
			}
			else
			{
				it_PathFeat2da.Checked = false;
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
			if (!it_PathItemPropDef2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select ItemPropDef.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						CraftInfo.GropeLabels(ofd.FileName,
											  CraftInfo.ipLabels,
											  it_PathItemPropDef2da,
											  2);
					}
				}
			}
			else
			{
				it_PathItemPropDef2da.Checked = false;
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
			if (!it_PathBaseItems2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select BaseItems.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						CraftInfo.GropeLabels(ofd.FileName,
											  CraftInfo.tagLabels,
											  it_PathBaseItems2da,
											  2);
					}
				}
			}
			else
			{
				it_PathBaseItems2da.Checked = false;
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
			if (!it_PathSkills2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Skills.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						CraftInfo.GropeLabels(ofd.FileName,
											  CraftInfo.skillLabels,
											  it_PathSkills2da,
											  1);
					}
				}
			}
			else
			{
				it_PathSkills2da.Checked = false;
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

		internal void GropeLabels(string directory)
		{
			CraftInfo.GropeLabels(Path.Combine(directory, "baseitems.2da"),
								  CraftInfo.tagLabels,
								  it_PathBaseItems2da,
								  2);

			CraftInfo.GropeLabels(Path.Combine(directory, "feat.2da"),
								  CraftInfo.featsLabels,
								  it_PathFeat2da,
								  1);

			CraftInfo.GropeLabels(Path.Combine(directory, "itempropdef.2da"),
								  CraftInfo.ipLabels,
								  it_PathItemPropDef2da,
								  2);

			CraftInfo.GropeLabels(Path.Combine(directory, "skills.2da"),
								  CraftInfo.skillLabels,
								  it_PathSkills2da,
								  1);

			CraftInfo.GropeLabels(Path.Combine(directory, "spells.2da"),
								  CraftInfo.spellLabels,
								  it_PathSpells2da,
								  1);



			CraftInfo.GropeLabels(Path.Combine(directory, "classes.2da"),
								  CraftInfo.classLabels,
								  it_PathClasses2da,
								  1);

			CraftInfo.GropeLabels(Path.Combine(directory, "disease.2da"),
								  CraftInfo.diseaseLabels,
								  it_PathDisease2da,
								  1);

			CraftInfo.GropeLabels(Path.Combine(directory, "iprp_feats.2da"),
								  CraftInfo.ipfeatsLabels,
								  it_PathIprpFeats2da,
								  2);

			CraftInfo.GropeLabels(Path.Combine(directory, "iprp_spells.2da"),
								  CraftInfo.ipspellsLabels,
								  it_PathIprpSpells2da,
								  1, // label
								  3, // level
								  CraftInfo.ipspellsLevels);

			CraftInfo.GropeLabels(Path.Combine(directory, "iprp_onhitspell.2da"),
								  CraftInfo.iphitspellLabels,
								  it_PathIprpOnHitSpell2da,
								  1);

			CraftInfo.GropeLabels(Path.Combine(directory, "racialtypes.2da"),
								  CraftInfo.raceLabels,
								  it_PathRaces2da,
								  1);
		}
#endregion Crafting info


#region Search
		internal void ToolStripTextBox1KeyUp(object sender, KeyEventArgs e)
		{
			if (_table != null && _table.Rows.Count > 1
				&& (sender == null
					|| e.KeyCode == Keys.Enter || e.KeyCode == Keys.F3))
			{
				string search = tb_Search.Text;
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
							if ((val = _table.Rows[id].Cells[col].Value) != null)
							{
								st = val.ToString().ToLower();
								if (   (cb_SearchOption.SelectedIndex == 0 && st.Contains(search))
									|| (cb_SearchOption.SelectedIndex == 1 && st == search))
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

		void ToolStripComboBox1KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F3 || e.KeyCode == Keys.Enter)
			{
				ToolStripTextBox1KeyUp(null, null);
			}
		}
#endregion Search


#region Events (override)
		/// <summary>
		/// Sends (unhandled) mousewheel events on the Form to the table.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (_table != null)
				_table.ForceScroll(e);
		}
#endregion Events (override)
	}
}
