using System;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Fields
		/// <summary>
		/// A flag that prevents a Readonly warning/error from showing twice.
		/// </summary>
		bool _warned;
		#endregion Fields


		#region Handlers (file)
		/// <summary>
		/// Handles opening the File menu along with the preset-dirs and
		/// recent-files subits.
		/// </summary>
		/// <param name="sender"><c><see cref="it_MenuFile"/></c></param>
		/// <param name="e"></param>
		void file_dropdownopening(object sender, EventArgs e)
		{
			it_Reload.Enabled = Table != null && File.Exists(Table.Fullpath);


			if (Settings._dirpreset.Count != 0) // directory presets ->
			{
				_preset = String.Empty;

				ToolStripItemCollection presets = it_OpenFolder.DropDownItems;
				for (int i = presets.Count - 1; i != -1; --i)
					presets[i].Dispose();

				presets.Clear();

				ToolStripItem preset;
				foreach (var dir in Settings._dirpreset)
				{
					if (Directory.Exists(dir))
					{
						preset = presets.Add(dir);
						preset.Click += fileclick_OpenFolder;
					}
				}
				it_OpenFolder.Visible = presets.Count != 0;
			}

			if (Settings._recent != 0) // recent files ->
			{
				ToolStripItem it;
				ToolStripItemCollection recents = it_Recent.DropDownItems;
				for (int i = recents.Count - 1; i != -1; --i)
				{
					if (!File.Exists((it = recents[i]).Text))
					{
						recents.Remove(it);
						it.Dispose();
					}
				}
				it_Recent.Visible = recents.Count != 0;
			}
		}


		/// <summary>
		/// Handles it-click to create a new 2da-file.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Create"/></c></param>
		/// <param name="e"></param>
		void fileclick_Create(object sender, EventArgs e)
		{
			Table = new YataGrid(this, String.Empty, false);

			Table.CreateTable(); // <- instead of LoadTable()

			_isCreate = true;
			fileclick_SaveAs(it_SaveAs, EventArgs.Empty); // shall set Fullpath (incl. tab-string).
			_isCreate = false;

			if (File.Exists(Table.Fullpath)) // instead of CreatePage() ->
			{
				DrawRegulator.SuspendDrawing(Table);

				var tab = new TabPage();
				Tabs.TabPages.Add(tab);

				tab.Tag = Table;

				tab.Text = Path.GetFileNameWithoutExtension(Table.Fullpath);

				tab.Controls.Add(Table);
				Tabs.SelectedTab = tab;

				Table.Init();

				DrawRegulator.ResumeDrawing(Table);
			}
			else
			{
				YataGrid._init = false;
				Table.Dispose();
			}

			_bypassVerifyFile = true;
			tab_SelectedIndexChanged(null, EventArgs.Empty);
			_bypassVerifyFile = false;
		}
		bool _isCreate;

		/// <summary>
		/// Handles it-click to open a 2da-file in a preset folder.
		/// </summary>
		/// <param name="sender"><c><see cref="it_OpenFolder"/></c> subits</param>
		/// <param name="e"></param>
		void fileclick_OpenFolder(object sender, EventArgs e)
		{
			_preset = (sender as ToolStripItem).Text;
			fileclick_Open(sender, e);
		}

		/// <summary>
		/// Handles it-click to open a 2da-file.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Open"/></c></item>
		/// <item><c><see cref="it_OpenFolder"/></c> subits</item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|Open <c>[Ctrl+o]</c></item>
		/// <item>File|Open@Folder ...
		/// <c><see cref="fileclick_OpenFolder()">fileclick_OpenFolder()</see></c>
		/// subits</item>
		/// </list></remarks>
		void fileclick_Open(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog())
			{
				if (sender == it_Open)
				{
					string dir = Directory.GetCurrentDirectory();
					if (dir != Application.StartupPath)
					{
//						ofd.InitialDirectory = Directory.GetCurrentDirectory();
						ofd.FileName = Path.Combine(dir, "*.2da");
					}
					// else use the directory of the filetype stored in the Registry
					// ... if it exists.
				}
				else // invoked by fileclick_OpenFolder()
				{
					ofd.RestoreDirectory = true;

//					ofd.InitialDirectory = _preset;					// <- does not always work.
					ofd.FileName = Path.Combine(_preset, "*.2da");	// -> but that forces it to.
				}

				ofd.Title  = "Select a 2da file";
				ofd.Filter = Get2daFilter();

				ofd.ShowReadOnly = // <- that forces (AutoUpgradeEnabled=false)
				ofd.Multiselect  = true;


				if (ofd.ShowDialog() == DialogResult.OK)
				{
					bool read = ofd.ReadOnlyChecked;
					foreach (var pfe in ofd.FileNames)
						CreatePage(pfe, read); // load 1+ file(s) by openfiledialog
				}
			}
		}

		/// <summary>
		/// Handles it-click to reload the current table. Requests
		/// user-confirmation if data has changed etc.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Reload"/></c></item>
		/// <item><c><see cref="tabit_Reload"/></c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|Reload <c>[Ctrl+r]</c></item>
		/// <item>tab|Reload</item>
		/// <item><c><see cref="VerifyCurrentFileState()">VerifyCurrentFileState()</see></c></item>
		/// </list></remarks>
		internal void fileclick_Reload(object sender, EventArgs e)
		{
			if (File.Exists(Table.Fullpath)) // check 'Table.Fullpath' in case user presses [Ctrl+r] after deleting the 2da-file on the hardrive.
			{
				bool reload = !Table.Changed;
				if (!reload)
				{
					using (var ib = new Infobox(Infobox.Title_alert,
												"Data has changed. Okay to reload ...",
												null,
												InfoboxType.Warn,
												InfoboxButtons.CancelYes))
					{
						reload = ib.ShowDialog(this) == DialogResult.OK;
					}
				}

				if (reload)
				{
					Obfuscate();

					if      (_diff1 == Table) _diff1 = null;
					else if (_diff2 == Table) _diff2 = null;


					_bypassVerifyFile = true;

					int result = Table.LoadTable();
					if (result != YataGrid.LOADRESULT_FALSE)
					{
						DrawRegulator.SuspendDrawing(Table);

						Table._ur.Clear();

						it_freeze1.Checked =
						it_freeze2.Checked = false;

						Table.Init(result == YataGrid.LOADRESULT_CHANGED, true);

						if (Table.Propanel != null)
						{
							Table.Controls.Remove(Table.Propanel);
							Table.Propanel = null;
						}

						DrawRegulator.ResumeDrawing(Table);
					}
					else
					{
						Table.Changed = false; // bypass Close warn
						fileclick_ClosePage(sender, e);
					}

					_bypassVerifyFile = false;

					if (Table != null)
					{
						Obfuscate(false);
						VerifyCurrentFileState();
					}
				}
			}
			else
			{
				using (var ib = new Infobox(Infobox.Title_error,
											"File does not exist.",
											Table.Fullpath,
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
		}

		/// <summary>
		/// Opens a 2da-file from the recent-files list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Recent"/></c> subits</param>
		/// <param name="e"></param>
		/// <remarks>Invalid subits get deleted when dropdown opens.</remarks>
		void fileclick_Recent(object sender, EventArgs e)
		{
			CreatePage((sender as ToolStripItem).Text); // load file from recents
		}

		/// <summary>
		/// Allows user to set the current table's readonly flag after it has
		/// been opened.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Readonly"/></c></param>
		/// <param name="e"></param>
		/// <remarks>The readonly setter appears otherwise only in the file-open
		/// dialog.
		/// 
		/// 
		/// Called by
		/// <list type="bullet">
		/// <item>File|Readonly <c>[F12]</c></item>
		/// </list></remarks>
		void fileclick_Readonly(object sender, EventArgs e)
		{
			it_Save   .Enabled = !(Table.Readonly = it_Readonly.Checked);
			it_SaveAll.Enabled = AllowSaveAll();

			EnableCelleditOperations();
			EnableRoweditOperations();
			// col-edit operations are detered by its dropdown event

			if (_replacer != null)
				_replacer.EnableReplace();

			it_OrderRows.Enabled = !Table.Readonly;

			Tabs.Invalidate();
		}


		/// <summary>
		/// Handles several it-clicks that write a <c><see cref="YataGrid"/></c>
		/// to a 2da-file.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Save"/></c></item>
		/// <item><c><see cref="tabit_Save"/></c></item>
		/// <item><c><see cref="it_SaveAs"/></c></item>
		/// <item><c><see cref="it_SaveAll"/></c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|Save <c>[Ctrl+s]</c></item>
		/// <item>tab|Save</item>
		/// <item>File|SaveAs <c>[Ctrl+e]</c> <c><see cref="fileclick_SaveAs()">fileclick_SaveAs()</see></c></item>
		/// <item>File|SaveAll <c>[Ctrl+a]</c> <c><see cref="fileclick_SaveAll()">fileclick_SaveAll()</see></c></item>
		/// <item><c><see cref="VerifyCurrentFileState()">VerifyCurrentFileState()</see></c></item>
		/// </list></remarks>
		internal void fileclick_Save(object sender, EventArgs e)
		{
			bool overwrite; // force a Readonly file to overwrite itself (only if invoked by SaveAs)
			bool bypassReadonly;

			if (sender == it_SaveAs)
			{
				_table = Table;
				// '_pfeT' is set by caller
				overwrite = (_pfeT == _table.Fullpath);
				bypassReadonly = false;
			}
			else if (sender == it_SaveAll)
			{
				// '_table' and '_pfeT' are set by caller
				overwrite = false;
				bypassReadonly = false;
			}
			else // is rego-save or tab-save or 'FileWatcherDialog' save
			{
				_table = Table;
				_pfeT = _table.Fullpath;
				overwrite = false;

				if (sender == it_Save || sender == tabit_Save)
					bypassReadonly = false;
				else
					bypassReadonly = true; // only 'VerifyCurrentFileState()' gets to bypass Readonly.
			}

			_warned = false;

			if (!_table.Readonly || bypassReadonly
				|| (overwrite && SaveWarning("The 2da-file is opened as readonly.")))
			{
//				if ((_table._sortcol == 0 && _table._sortdir == YataGrid.SORT_ASC)
//					|| SaveWarning("The 2da is not sorted by ascending ID."))
//				{
				if (CheckRowOrder() || SaveWarning("Faulty row ids are detected."))
				{
					_table.Fullpath = _pfeT;

					SetTitlebarText();

					if (!_isCreate) // stuff that's unneeded and/or unwanted when creating a 2da ->
					{
						if (overwrite) _table.Readonly = false;	// <- IMPORTANT: If a file that was opened Readonly is saved
																//               *as itself* it loses its Readonly flag.

						_table.Changed = false;
						_table._ur.ResetSaved();

						_table.ClearReplaced(); // these toggle YataGrid._init so don't do that when creating a 2da ->
						_table.ClearLoadchanged();

						if (_table == Table)
							_table.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
					}

					FileOutput.Write(_table);
				}
//				}
			}
			else if (!_warned)
			{
				using (var ib = new Infobox(Infobox.Title_error,
											"The 2da-file is opened as readonly.",
											null,
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
		}

		/// <summary>
		/// Requests user-confirmation when saving a file when readonly or when
		/// a faulty row-id is detected.
		/// </summary>
		/// <param name="head"></param>
		/// <returns><c>true</c> to proceed - <c>false</c> to stop</returns>
		bool SaveWarning(string head)
		{
			_warned = true;
			using (var ib = new Infobox(Infobox.Title_alert,
										head + " Save anyway ...",
										null,
										InfoboxType.Warn,
										InfoboxButtons.CancelYes))
			{
				return ib.ShowDialog(this) == DialogResult.OK;
			}
		}

		/// <summary>
		/// Checks the row-order before save.
		/// </summary>
		/// <returns><c>true</c> if row-order is okay</returns>
		static bool CheckRowOrder()
		{
			int result;
			for (int r = 0; r != Table.RowCount; ++r)
			if (!Int32.TryParse(Table[r,0].text, out result) || result != r)
				return false;

			return true;
		}

		/// <summary>
		/// Handles it-click on File|SaveAs.
		/// </summary>
		/// <param name="sender"><c><see cref="it_SaveAs"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|SaveAs <c>[Ctrl+e]</c></item>
		/// <item>File|Create ...</item>
		/// </list></remarks>
		void fileclick_SaveAs(object sender, EventArgs e)
		{
			using (var sfd = new SaveFileDialog())
			{
				sfd.Title    = "Save as ...";
				sfd.Filter   = Get2daFilter();
				sfd.FileName = Path.GetFileName(Table.Fullpath);

				if (Directory.Exists(_lastSaveasDirectory))
				{
					sfd.InitialDirectory = _lastSaveasDirectory;
				}
				else if (Table.Fullpath.Length != 0)
				{
					string dir = Path.GetDirectoryName(Table.Fullpath);
					if (Directory.Exists(dir))
						sfd.InitialDirectory = dir;
				}


				if (sfd.ShowDialog() == DialogResult.OK)
				{
					_lastSaveasDirectory = Path.GetDirectoryName(sfd.FileName);

					_pfeT = sfd.FileName;
					fileclick_Save(sender, e);
				}
			}
		}

		/// <summary>
		/// Handles it-click on File|SaveAll.
		/// </summary>
		/// <param name="sender"><c><see cref="it_SaveAll"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|SaveAll <c>[Ctrl+a]</c></item>
		/// </list></remarks>
		void fileclick_SaveAll(object sender, EventArgs e)
		{
			IsSaveAll = true;

			bool changed = false;
			for (int i = 0; i != Tabs.TabCount; ++i)
			{
				_table = Tabs.TabPages[i].Tag as YataGrid;
				if (!_table.Readonly)
				{
					if (_table.Changed)
						changed = true;

					_pfeT = _table.Fullpath;
					fileclick_Save(sender, e);
				}
			}

			if (changed) SetAllTabTexts();

			IsSaveAll = false;
		}


		/// <summary>
		/// Handles it-click on File|Close.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Close"/></c></item>
		/// <item><c><see cref="tabit_Close"/></c></item>
		/// <item><c><see cref="it_Reload"/></c></item>
		/// <item><c><see cref="tabit_Reload"/></c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|Close <c>[F4]</c></item>
		/// <item>tab|Close</item>
		/// <item>File|Reload <c>[Ctrl+r]</c>
		/// <c><see cref="fileclick_Reload()">fileclick_Reload()</see></c></item>
		/// <item>tab|Reload 
		/// <c><see cref="fileclick_Reload()">fileclick_Reload()</see></c></item>
		/// <item><c><see cref="VerifyCurrentFileState()">VerifyCurrentFileState()</see></c></item>
		/// </list></remarks>
		internal void fileclick_ClosePage(object sender, EventArgs e)
		{
			bool close = !Table.Changed;
			if (!close)
			{
				using (var ib = new Infobox(Infobox.Title_alert,
											"Data has changed. Okay to close ...",
											null,
											InfoboxType.Warn,
											InfoboxButtons.CancelYes))
				{
					close = ib.ShowDialog(this) == DialogResult.OK;
				}
			}

			if (close)
			{
				TabPage lastpage = _lastpage;
				ClosePage(Tabs.SelectedTab);
				Tabs.SelectedTab = lastpage;
			}
		}

		/// <summary>
		/// Handles it-click on File|CloseAll.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_CloseAll"/></c></item>
		/// <item><c><see cref="tabit_CloseAll"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|CloseAll <c>[Ctrl+F4]</c></item>
		/// <item>tab|CloseAll</item>
		/// </list></remarks>
		void fileclick_CloseAllTabs(object sender, EventArgs e)
		{
			if (!CancelChangedTables("close"))
			{
				for (int tab = Tabs.TabCount - 1; tab != -1; --tab)
					ClosePage(Tabs.TabPages[tab]);
			}
		}


		/// <summary>
		/// Handles it-click on File|Quit.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Quit"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|Quit <c>[Ctrl+q]</c></item>
		/// </list></remarks>
		void fileclick_Quit(object sender, EventArgs e)
		{
			Close(); // let yata_Closing() handle it ...
		}
		#endregion Handlers (file)


		#region Methods (file)
		/// <summary>
		/// Checks if there is a non-readonly table open. Also checks if there
		/// are two non-readonly instances of the same 2da-file open.
		/// </summary>
		/// <returns><c>true</c> if SaveAll is allowed</returns>
		bool AllowSaveAll()
		{
			if (Settings._allowdupls)
			{
				// iterate through all tables and if a different table has the
				// same Fullpath and neither table is Readonly return false ->

				YataGrid table0, table1;
				string pfe;

				for (int i = 0; i != Tabs.TabPages.Count; ++i)
				if (!(table0 = Tabs.TabPages[i].Tag as YataGrid).Readonly)
				{
					pfe = table0.Fullpath;
					for (int j = 0; j != Tabs.TabPages.Count; ++j)
					if (j != i
						&& !(table1 = Tabs.TabPages[j].Tag as YataGrid).Readonly
						&& table1.Fullpath == pfe)
					{
						return false;
					}
				}
			}

			// next just iterate over all tables and if any is not Readonly
			// allow SaveAll ->
			for (int i = 0; i != Tabs.TabCount; ++i)
			if (!(Tabs.TabPages[i].Tag as YataGrid).Readonly)
				return true;

			return false;
		}
		#endregion Methods (file)
	}
}
