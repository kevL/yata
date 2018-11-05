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
		#region Fields & Properties
		YataGrid Table
		{ get; set; }

		List<string> _copy = new List<string>();

		List<ToolStripItem> _presets = new List<ToolStripItem>();
		string _initialDir = String.Empty;

		internal TabControl Tabs
		{ get { return tabControl; } }

		Font FontDefault
		{ get; set; }
		#endregion Fields & Properties

		internal Font FontAccent;

		internal bool _search;


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


			FontDefault = Font;

			Settings.ScanSettings(); // load an Optional manual settings file

			if (Settings._font != null)
			{
				Font = Settings._font;
			}

			FontAccent = new Font(Font, getStyleAccented(Font.FontFamily));

			if (Settings._font2 != null)
			{
				menubar.Font.Dispose();
				menubar.Font = Settings._font2;

				contextEditor.Font.Dispose();
				contextEditor.Font = Settings._font2;

				statusbar.Font.Dispose();
				statusbar.Font = Settings._font2;

				statusbar_label_Info.Font.Dispose();
				statusbar_label_Info.Font = new Font(Settings._font2.FontFamily,
													 Settings._font2.SizeInPoints + 1.5f);

				int hBar = TextRenderer.MeasureText("X", statusbar_label_Info.Font).Height + 2;

				statusbar             .Height = (hBar + 5 < 22) ? 22 : hBar + 5;
				statusbar_label_Coords.Height =
				statusbar_label_Info  .Height = (hBar     < 17) ? 17 : hBar;

				int wCoords0 = statusbar_label_Coords.Width;
				int wCoords = TextRenderer.MeasureText("id= 99999 col= 99", statusbar_label_Info.Font).Width + 10;
				statusbar_label_Coords.Width = (wCoords < wCoords0) ? wCoords0 : wCoords;


				context_it_Header.Font.Dispose();
				context_it_Header.Font = new Font(Settings._font2.FontFamily,
												  Settings._font2.SizeInPoints + 1.0f,
												  getStyleAccented(Settings._font2.FontFamily));
			}

			if (Settings._dirpreset.Count != 0)
			{
				it_Folders.Visible = true;

				var clear = it_Folders.DropDownItems.Add("clear current");
				_presets.Add(clear);

				clear.Visible = false;
				clear.Click += PresetClick;

				foreach (var dir in Settings._dirpreset)
				{
					var preset = it_Folders.DropDownItems.Add(dir);
					_presets.Add(preset);

					preset.Click += PresetClick;
				}
			}

			int
				x = Settings._x,
				y = Settings._y,
				w = Settings._w,
				h = Settings._h;

			if (x != -1 || y != -1)
			{
				StartPosition = FormStartPosition.Manual;
				if (x == -1) x = Left;
				if (y == -1) y = Top;
				Location = new Point(x,y);
			}

			if (w == -1) w = Width;
			if (h == -1) h = Height;
			ClientSize = new Size(w,h);


			cb_SearchOption.Items.AddRange(new object[]
			{
				"find substring",
				"find wholeword"
			});
			cb_SearchOption.SelectedIndex = 0;


			// debug ->
//			CreateTabPage(@"C:\Users\User\Documents\Neverwinter Nights 2\override\2da\baseitems.2da");
		}
		#endregion cTor


		#region Methods (static)
		internal static FontStyle getStyleAccented(FontFamily ff)
		{
			FontStyle style;
			if (!ff.IsStyleAvailable(style = FontStyle.Bold))
			if (!ff.IsStyleAvailable(style = FontStyle.Underline))
			if (!ff.IsStyleAvailable(style = FontStyle.Italic))
			foreach (FontStyle styleTest in Enum.GetValues(typeof(FontStyle))) // determine first available style of Family ->
			{
				if (ff.IsStyleAvailable(styleTest))
				{
					style = styleTest;
					break;
				}
			}
			return style;
		}

		static FontStyle getStyleStandard(FontFamily ff)
		{
			FontStyle style;
			if (!ff.IsStyleAvailable(style = FontStyle.Regular))
			if (!ff.IsStyleAvailable(style = FontStyle.Italic))
			if (!ff.IsStyleAvailable(style = FontStyle.Bold))
			foreach (FontStyle styleTest in Enum.GetValues(typeof(FontStyle))) // determine first available style of Family ->
			{
				if (ff.IsStyleAvailable(styleTest))
				{
					style = styleTest;
					break;
				}
			}
			return style;
		}
		#endregion Methods (static)


		#region File menu
		void fileclick_Open(object sender, EventArgs e)
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


		/// <summary>
		/// IMPORTANT: Assumes 'pfe' is VALID.
		/// </summary>
		/// <param name="pfe"></param>
		void CreateTabPage(string pfe)
		{
			//logfile.Log("CreateTabPage()");

			panel_ColorFill.Show();

			Enabled = false;

			var table = new YataGrid(this, pfe);

			if (table.Load2da())
			{
				Table = table; // NOTE: Is done also in tab_SelectedIndexChanged()

				var tab = new TabPage();
				Tabs.TabPages.Add(tab);

				tab.Tag = Table;

				tab.Text = Path.GetFileNameWithoutExtension(pfe);
				SetTabSize();

				tab.Controls.Add(Table);
				Tabs.SelectedTab = tab;

				Table.Init();

				//DrawingControl.ResumeDrawing(table);
			}

			Enabled = true;

			tab_SelectedIndexChanged(null, EventArgs.Empty);
		}


		/// <summary>
		/// Sets the width of the tabs on the TabControl.
		/// </summary>
		void SetTabSize()
		{
			if (Tabs != null && Tabs.TabCount != 0)
			{
				int w = 25, wT;
				for (int tab = 0; tab != Tabs.TabCount; ++tab)
				{
					if ((wT = TextRenderer.MeasureText(Tabs.TabPages[tab].Text, Font).Width) > w)
						w = wT;
				}
				Tabs.ItemSize = new Size(w + 8,0);
				Tabs.Refresh(); // prevent text-drawing glitches ...
			}
		}


		/// <summary>
		/// Handles tab-selection/deselection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tab_SelectedIndexChanged(object sender, EventArgs e)
		{
			//logfile.Log("tab_SelectedIndexChanged id= " + Tabs.SelectedIndex);

			if (Tabs.SelectedIndex != -1)
			{
				Table = Tabs.SelectedTab.Tag as YataGrid; // <- very Important <--||

				panel_ColorFill.Hide();

				it_MenuPaths.Visible = Table.Craft;

				it_freeze1.Checked = (Table.FrozenCount == YataGrid.FreezeFirst);
				it_freeze2.Checked = (Table.FrozenCount == YataGrid.FreezeSecond);
			}
			else
			{
				panel_ColorFill.Show();

				it_MenuPaths.Visible = false;

				it_freeze1.Checked =
				it_freeze2.Checked = false;
			}

			SetTitlebarText();
		}

		/// <summary>
		/// Draws the tab-text in Bold iff selected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tab_DrawItem(object sender, DrawItemEventArgs e)
		{
			var tab = Tabs.TabPages[e.Index];

			int y;

			FontStyle style;
			if (tab == Tabs.SelectedTab)
			{
				style = getStyleAccented(Font.FontFamily);
				y = 6;
			}
			else
			{
				style = getStyleStandard(Font.FontFamily);
				y = 3;
			}

			var font = new Font(Font.Name, Font.SizeInPoints - 0.5f, style);

			// NOTE: MS doc for DrawText() says that using a Point doesn't work on Win2000 machines.
			int w = TextRenderer.MeasureText(tab.Text, font).Width;
			var rect = e.Bounds;
			rect.X   = e.Bounds.X + (e.Bounds.Width - w) / 2;
			rect.Y   = e.Bounds.Y + y;

			TextRenderer.DrawText(e.Graphics,
								  tab.Text,
								  font,
								  rect,
								  SystemColors.ControlText,
								  TextFormatFlags.NoClipping | TextFormatFlags.NoPrefix);
		}

		void fileclick_Close(object sender, EventArgs e)
		{
			if (Table != null
				&& (!Table.Changed
					|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
									   "warning",
									   MessageBoxButtons.YesNo,
									   MessageBoxIcon.Warning,
									   MessageBoxDefaultButton.Button2) == DialogResult.Yes))
			{
				Tabs.TabPages.Remove(Tabs.SelectedTab);

				if (Tabs.TabCount == 0)
					Table = null;

				SetTabSize();
				SetTitlebarText();
			}
		}

		void fileclick_Quit(object sender, EventArgs e)
		{
			Close(); // let MainFormFormClosing() handle it ...
		}

		void yata_Closing(object sender, CancelEventArgs e)
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

		/// <summary>
		/// Returns a list of currently loaded tables that have been modified.
		/// </summary>
		/// <returns></returns>
		List<string> GetChangedTables()
		{
			var changed = new List<string>();

			foreach (TabPage page in Tabs.TabPages)
			{
				var table = page.Tag as YataGrid;
				if (table != null && table.Changed)
				{
					changed.Add(Path.GetFileNameWithoutExtension(table.Pfe).ToUpperInvariant());
				}
			}
			return changed;
		}


		void fileclick_Reload(object sender, EventArgs e)
		{
			if (Table != null && File.Exists(Table.Pfe)
				&& (!Table.Changed
					|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
									   "warning",
									   MessageBoxButtons.YesNo,
									   MessageBoxIcon.Warning,
									   MessageBoxDefaultButton.Button2) == DialogResult.Yes))
			{
				panel_ColorFill.Show();

				if (Table.Load2da())
				{
					it_freeze1.Checked =
					it_freeze2.Checked = false;

					Table.Init(true);

//					DrawingControl.ResumeDrawing(Table);
				}
				else
					fileclick_Close(null, EventArgs.Empty);

				if (Tabs.TabCount != 0)
					panel_ColorFill.Hide();
			}
			// TODO: Show an error if file no longer exists.
		}

		// nb. the Create option is disabled in the designer
		void fileclick_Create(object sender, EventArgs e)
		{}


		/// <summary>
		/// Sets the titlebar text to something reasonable.
		/// </summary>
		void SetTitlebarText()
		{
			string text = "Yata";

			if (Tabs.SelectedIndex != -1)
			{
				var table = Tabs.SelectedTab.Tag as YataGrid;
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


		void fileclick_Save(object sender, EventArgs e)
		{
/*			if (Table != null)
			{
				string pfe = Table.Pfe;

				if (!String.IsNullOrEmpty(pfe))
				{
					int rows = Table.Rows.Count;
					if (rows > 1)
					{
						Table.Changed = false;

						using (var sw = new StreamWriter(pfe))
						{
							sw.WriteLine("2DA V2.0");
							sw.WriteLine("");

							string line = String.Empty;
							foreach (string col in Table.Cols)
							{
								line += " " + col;
							}
							sw.WriteLine(line);


							object val;

							int cols = Table.Columns.Count;

							for (int row = 0; row != rows - 1; ++row)
							{
								line = String.Empty;

								for (int cell = 0; cell != cols; ++cell)
								{
									if (cell != 0)
										line += " ";

									if ((val = Table.Rows[row].Cells[cell].Value) != null)
										line += val.ToString();
									else
										line += Constants.Stars;
								}

								sw.WriteLine(line);
							}
						}
					}
				}
			} */
		}

		void fileclick_SaveAs(object sender, EventArgs e)
		{
/*			if (Table != null && Table.Rows.Count > 1)
			{
				using (var sfd = new SaveFileDialog())
				{
					sfd.Title    = "Save as ...";
					sfd.Filter   = "2da files (*.2da)|*.2da|All files (*.*)|*.*";
					sfd.FileName = Path.GetFileName(Table.Pfe);

					if (sfd.ShowDialog() == DialogResult.OK)
					{
						Table.Pfe = sfd.FileName;
						Tabs.TabPages[Tabs.SelectedIndex].Text = Path.GetFileNameWithoutExtension(Table.Pfe);

						SetTitlebarText();

						fileclick_Save(null, EventArgs.Empty);
					}
				}
			} */
		}

		/// <summary>
		/// Handles opening the FileMenu, FolderPresets item and its sub-items.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void file_dropdownopening_FolderPresets(object sender, EventArgs e)
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
		int _r;

		internal void context_(object sender, MouseEventArgs e)
		{
			_r = (e.Y + Table.offsetVert) / Table.HeightRow;

			Table._editor.Visible = false;
			Table.ClearCellSelects();
			for (int c = 0; c != Table.ColCount; ++c)
				Table[_r,c].selected = true;

			foreach (var row in Table.Rows)
				row.selected = false;

			Table.Rows[_r].selected = true;
			Table.EnsureDisplayedRow(_r);
			Table.Refresh();

			context_it_Header.Text = "_row @ id " + _r;

			context_it_PasteAbove.Enabled =
			context_it_Paste     .Enabled =
			context_it_PasteBelow.Enabled = (Table.ColCount == _copy.Count);

			contextEditor.Show(Table, new Point(YataGrid.WidthRowhead,
												YataGrid.HeightColhead));
		}

		void contextclick_EditCopy(object sender, EventArgs e)
		{
			_copy.Clear();

			for (int c = 0; c != Table.ColCount; ++c)
				_copy.Add(Table[_r,c].text);
		}

		void contextclick_EditCut(object sender, EventArgs e)
		{
			contextclick_EditCopy(null, EventArgs.Empty);
			contextclick_EditDelete(null, EventArgs.Empty);
		}

		void contextclick_EditPasteAbove(object sender, EventArgs e)
		{
			if (_copy.Count == Table.ColCount)
			{
				Table.Changed = true;
				Table.Insert(_r, _copy);
				Table.Refresh();
			}
		}

		void contextclick_EditPaste(object sender, EventArgs e)
		{
			int cols = Table.ColCount;
			if (_copy.Count == cols)
			{
				Table.Changed = true;
				for (int c = 0; c != cols; ++c)
				{
					Table[_r,c].text = _copy[c];
				}
				Table.Rows[_r]._brush = Brushes.Created;
				Table.Refresh();
			}
		}

		void contextclick_EditPasteBelow(object sender, EventArgs e)
		{
			if (_copy.Count == Table.ColCount)
			{
				Table.Changed = true;
				Table.Insert(_r + 1, _copy);
				Table.Refresh();
			}
		}

		void contextclick_EditCreateAbove(object sender, EventArgs e)
		{
			Table.Changed = true;
			int cols = Table.ColCount;

			var fields = new string[cols];
			for (int c = 0; c != cols; ++c)
			{
				fields[c] = Constants.Stars;
			}
			Table.Insert(_r, fields);
			Table.Refresh();
		}

		void contextclick_EditClear(object sender, EventArgs e)
		{
			Table.Changed = true;
			int cols = Table.ColCount;
			for (int c = 1; c != cols; ++c)
			{
				Table[_r,c].text = Constants.Stars;
			}
			Table.Rows[_r]._brush = Brushes.Created;
			Table.Refresh();
		}

		void contextclick_EditCreateBelow(object sender, EventArgs e)
		{
			Table.Changed = true;
			int cols = Table.ColCount;

			var fields = new string[cols];
			for (int c = 0; c != cols; ++c)
			{
				fields[c] = Constants.Stars;
			}
			Table.Insert(_r + 1, fields);
			Table.Refresh();
		}

		void contextclick_EditDelete(object sender, EventArgs e)
		{
			Table.Changed = true;

			Table.Insert(_r, null);
			Table.Refresh();
		}
		#endregion Context menu


		#region Edit menu
		void editclick_Search(object sender, EventArgs e)
		{
			tb_Search.Focus();
		}

		void editclick_SearchNext(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0)
			{
				Table.Select(); // F3 shall focus the table, Enter shall keep focus on the tb/cbx.
				Search();
			}
		}

		/// <summary>
		/// Performs a search when the Enter-key is released and focus is on
		/// either the search-box or the search-option dropdown.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void SearchKeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter
				&& Table != null && Table.RowCount != 0)
			{
				_search = true; // Enter shall keep focus on the tb/cbx, F3 shall focus the table.
				Search();
				_search = false;
			}
		}

		/// <summary>
		/// Searches the current table for the string in the search-box.
		/// NOTE: Ensure that 'Table' is valid before call.
		/// </summary>
		void Search()
		{
			Table._editor.Visible = false;

			// TODO: Allow frozen col(s) to be searched through also.
			// TODO: option to invert the search direction (or at least back to
			//       previous find)

			string search = tb_Search.Text;
			if (!String.IsNullOrEmpty(search))
			{
				search = search.ToLower();

				int startRow;
				int startCol;

				Cell sel = Table.GetOnlySelectedCell();
				if (sel != null)
				{
					startRow = sel.y;
					startCol = sel.x;
				}
				else
				{
					startRow =
					startCol = 0;
				}


				string val;
				int r,c;

				bool start = true;
				bool substring = (cb_SearchOption.SelectedIndex == 0); // else is wholestring search.

				string field;

				for (r = startRow; r != Table.RowCount; ++r)
				{
					if (start)
					{
						start = false;
						c = startCol + 1;

						if (c == Table.ColCount)		// if starting on the last cell of a row
						{
							c = 0;

							if (r < Table.RowCount - 1)	// jump to the first cell of the next row
							{
								++r;
							}
							else						// or to the top of the table if on the last row(s)
								r = 0;
						}
					}
					else
						c = 0;

					for (; c != Table.ColCount; ++c)
					{
						if (c >= Table.FrozenCount && !String.IsNullOrEmpty(val = Table[r,c].text))
						{
							field = val.ToLower();
							if (field == search
								|| (substring && field.Contains(search)))
							{
								if (sel != null)
									sel.selected = false;

								Table[r,c].selected = true;
								Table.EnsureDisplayed(Table[r,c]);
								Table.Refresh();
								return;
							}
						}
					}
				}

				// TODO: tighten exact start/end-cells
				for (r = 0; r != startRow + 1; ++r) // quick and dirty wrap ->
				{
					for (c = 0; c != Table.ColCount; ++c)
					{
						if (c >= Table.FrozenCount && !String.IsNullOrEmpty(val = Table[r,c].text))
						{
							field = val.ToLower();
							if (field == search
								|| (substring && field.Contains(search)))
							{
								if (sel != null)
									sel.selected = false;

								Table[r,c].selected = true;
								Table.EnsureDisplayed(Table[r,c]);
								Table.Refresh();
								return;
							}
						}
					}
				}
			}
		}
		#endregion Edit menu


		#region 2da Ops menu
		void opsclick_CheckRowOrder(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0)
			{
				var list = new List<string>();

				string val;
				int result;

				bool stop = false;

				for (int id = 0; id != Table.RowCount; ++id)
				{
					if (String.IsNullOrEmpty(val = Table[id,0].text))
					{
						if (list.Count == 20) // stop this Madness
						{
							stop = true;
							break;
						}
						list.Add("id " + id + " is not valid");
					}
					else if (!Int32.TryParse(val, out result))
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
						opsclick_Reorder(null, EventArgs.Empty);
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

		void opsclick_Reorder(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0)
			{
//				DrawingControl.SuspendDrawing(Table); // bongo

				var pb = new ProgBar(this);
				pb.ValTop = Table.RowCount;
				pb.Show();

				bool changed = false;

				string val;
				int result;

				for (int id = 0; id != Table.RowCount; ++id)
				{
					if (String.IsNullOrEmpty(val = Table[id,0].text)
						|| !Int32.TryParse(val, out result)
						|| result != id)
					{
						Table[id,0].text = id.ToString();
						changed = true;
					}
					pb.Step();
				}

				Table.Changed = changed;

//				DrawingControl.ResumeDrawing(Table);
			}
		}

		void opsclick_Recolor(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0)
			{
				Brush brush;
				for (int id = 0; id != Table.RowCount; ++id)
				{
					brush = (id % 2 == 0) ? Brushes.Alice
										  : Brushes.Blanche;
					Table.Rows[id]._brush = brush;
				}
				Table.Refresh();
			}
		}

		internal void opsclick_AutosizeCols(object sender, EventArgs e) // NOTE: Disabled in designer w/ Visible=false
		{
//			if (Table != null)
//				Table.AutoResizeColumns();
		}


		void opsclick_Freeze1stCol(object sender, EventArgs e)
		{
			if (Table != null && Table.ColCount > 1)
			{
				Table.Select();

				it_freeze2.Checked = false;

				if (it_freeze1.Checked = !it_freeze1.Checked)
				{
					Table.FrozenCount = YataGrid.FreezeFirst;
				}
				else
					Table.FrozenCount = YataGrid.FreezeId;
			}
		}

		void opsclick_Freeze2ndCol(object sender, EventArgs e)
		{
			if (Table != null && Table.ColCount > 2)
			{
				Table.Select();

				it_freeze1.Checked = false;

				if (it_freeze2.Checked = !it_freeze2.Checked)
				{
					Table.FrozenCount = YataGrid.FreezeSecond;
				}
				else
					Table.FrozenCount = YataGrid.FreezeId;
			}
		}
		#endregion 2da Ops menu


		#region Font menu
		/// <summary>
		/// Opens the FontPicker form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fontclick_Font(object sender, EventArgs e)
		{
			var f = Application.OpenForms["FontPickerForm"];
			if (f == null)
			{
				f = new FontPickerForm(this);
				f.Show();
			}
			else
				f.BringToFront();
		}

		/// <summary>
		/// Opens an output-box with the current table-font as a string for
		/// copying to Settings.Cfg if desired.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fontclick_CurrentFont(object sender, EventArgs e)
		{
			var f = new FontCopyForm();
			f.Font = Font;

			TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
			f.SetText("font=" + tc.ConvertToString(Font));

			f.ShowDialog();
		}

		/// <summary>
		/// Sets the form's font to the default Font.
		/// See also: FontPickerForm.doFont()
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fontclick_Default(object sender, EventArgs e)
		{
			if (!Font.Equals(FontDefault))
				doFont(FontDefault);
		}

		internal void ToggleFontDefaultEnabled()
		{
			it_FontDefault.Enabled = !it_FontDefault.Enabled;
		}

		/// <summary>
		/// Applies a specified font to the Form.
		/// </summary>
		/// <param name="font"></param>
		internal void doFont(Font font)
		{
			DrawingControl.SuspendDrawing(this);

			// NOTE: Cf f.AutoScaleMode (None,Font,DPI,Inherit)
			// Since I'm doing all the necessary scaling due to font-changes
			// w/ code the AutoScaleMode should not be set to default "Font".
			// It might better be set to "DPI" for those weirdos and I don't
			// know what "Inherit" means (other than the obvious).
			// AutoScaleMode is currently set to "None".
			//
			// See also SetProcessDPIAware()

//			int w = Width; // grab these before auto-scaling/re-sizing happens -->
//			int h = Height;

//			int w2 = tb_Search      .Width;
//			int w3 = cb_SearchOption.Width;

//			int h2 = statusbar.Height;

			Font = font; // rely on GC here
			FontAccent = new Font(Font, getStyleAccented(Font.FontFamily));

			if (Table != null)
			{
				YataGrid table;
				for (int tab = 0; tab != Tabs.TabCount; ++tab)
				{
					table = Tabs.TabPages[tab].Tag as YataGrid;
					table.Calibrate();
				}
				SetTabSize();
			}

//			Width  = w;
//			Height = h;

//			tb_Search      .Width = w2;
//			cb_SearchOption.Width = w3;

//			statusbar.Height = h2;

			if (Table != null)
			{
				var cell = Table.GetOnlySelectedCell();
				if (cell != null)
				{
					Table.EnsureDisplayed(cell);
					Refresh(); // for big tables ...
				}
			}

			DrawingControl.ResumeDrawing(this);
		}
		#endregion Font menu


		#region Crafting info
		/// <summary>
		/// Mouseover datacells prints info to the statusbar if Crafting.2da is
		/// loaded.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="col"></param>
		internal void PrintInfo(int id, int col = -1)
		{
			string st = String.Empty;

			if (id != -1)
			{
				statusbar_label_Coords.Text = "id= " + id + " col= " + col;

				if (Table.Craft && id < Table.RowCount && col < Table.ColCount) // NOTE: mouseover pos can register in the scrollbars
					statusbar_label_Info.Text = getCraftInfo(id, col);
				else
					statusbar_label_Info.Text = st;
			}
			else
			{
				statusbar_label_Coords.Text =
				statusbar_label_Info  .Text = st;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		string getCraftInfo(int id, int col)
		{
			string info = "n/a";

			string val;
			int result;

			switch (col)
			{
//				case -1: // row-header
//				case  0: // id

				case  1: // "CATEGORY"
					if (it_PathSpells2da.Checked)
					{
						if (!String.IsNullOrEmpty(val = Table[id,col].text)
							&& Int32.TryParse(val, out result)
							&& result < CraftInfo.spellLabels.Count)
						{
							info = Table.Cols[col].text + ": "
								 + CraftInfo.spellLabels[result];
						}
					}
					break;

//				case  2: // "REAGENTS"

				case  3: // "TAGS"
					if (!String.IsNullOrEmpty(val = Table[id,col].text))
					{
						if (val.StartsWith("B", StringComparison.InvariantCulture)) // is in BaseItems.2da
						{
							if (it_PathBaseItems2da.Checked)
							{
								info = Table.Cols[col].text + ": (base) ";

								string[] array = val.Substring(1).Split(','); // lose the "B"
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
							info = Table.Cols[col].text + ": (tcc) ";

							if (val == Constants.Stars)
							{
								info += CraftInfo.GetTccType(0); // TCC_TYPE_NONE
							}
							else
							{
								string[] array = val.Split(',');
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
						if (!String.IsNullOrEmpty(val = Table[id,col].text))
						{
							if (val != Constants.Stars)
							{
								info = Table.Cols[col].text + ": ";

								string par = String.Empty;
								int pos;

								string[] ips = val.Split(';');
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

//				case  5: // "OUTPUT"

				case  6: // "SKILL"
					if (it_PathFeat2da.Checked)
					{
						if (!String.IsNullOrEmpty(val = Table[id,col].text)
							&& Int32.TryParse(val, out result))
						{
							string cat = Table[id,1].text;
							if (!String.IsNullOrEmpty(cat))
							{
								int result2;
								if (Int32.TryParse(cat, out result2)) // is triggered by spell id
								{
									if (result < CraftInfo.featsLabels.Count)
									{
										info = Table.Cols[col].text + ": "
											 + CraftInfo.featsLabels[result];
									}
								}
								else // is triggered NOT by spell - but by mold-tag, or is Alchemy or Distillation
								{
									if (result < CraftInfo.skillLabels.Count)
									{
										info = Table.Cols[col].text + ": "
											 + CraftInfo.skillLabels[result];
									}
								}
							}
						}
					}
					break;

//				case  7: // "LEVEL"
//				case  8: // "EXCLUDE"
//				case  9: // "XP"
//				case 10: // "GP"
//				case 11: // "DISABLE"
			}
			return info;
		}

		/// <summary>
		/// Handles clicking the PathSpells menuitem.
		/// Intended to add labels from Spells.2da to the 'spellLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathSpells2da(object sender, EventArgs e)
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
		void itclick_PathFeat2da(object sender, EventArgs e)
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
		void itclick_PathItemPropDef2da(object sender, EventArgs e)
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
		void itclick_PathBaseItems2da(object sender, EventArgs e)
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
		void itclick_PathSkills2da(object sender, EventArgs e)
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

		void itclick_PathAll(object sender, EventArgs e)
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

			CraftInfo.GropeLabels(Path.Combine(directory, "iprp_ammocost.2da"),
								  CraftInfo.ipammoLabels,
								  it_PathIprpAmmoCost2da,
								  2);

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


		#region Tabmenu
		/// <summary>
		/// Sets the selected tab when a right-click on a tab is about to open
		/// a context.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tabMenu_Opening(object sender, CancelEventArgs e)
		{
			var pt = Tabs.PointToClient(Cursor.Position);
			for (int i = 0; i != Tabs.TabCount; ++i)
			{
				if (Tabs.GetTabRect(i).Contains(pt))
				{
					Tabs.SelectedIndex = i; // i is the index of tab under cursor
					return;
				}
			}
			e.Cancel = true;
		}

		/// <summary>
		/// Closes a table when a tab's context-close item is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tabclick_Close(object sender, EventArgs e)
		{
			fileclick_Close(null, EventArgs.Empty);
		}
		#endregion Tabmenu


		internal void TableChanged(bool changed)
		{
			DrawingControl.SuspendDrawing(this);

			string asterisk = changed ? " *"
									  : "";
			Tabs.TabPages[Tabs.SelectedIndex].Text = Path.GetFileNameWithoutExtension(Table.Pfe) + asterisk;
			SetTabSize();

			DrawingControl.ResumeDrawing(this);
		}


		#region Events (override)
		/// <summary>
		/// Sends (unhandled) mousewheel events on the Form to the table.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (Table != null)
				Table.Scroll(e);

//			base.OnMouseWheel(e);
		}
		#endregion Events (override)
	}
}
