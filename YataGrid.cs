using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Loads, formats, and handles 2da-data as a table or grid on Yata's
	/// tab-pages.
	/// </summary>
	sealed partial class YataGrid
		: Control
	{
/*		/// <summary>
		/// Do not implement <c>IDisposable</c> here.
		/// </summary>
		/// <remarks>Because I hate that 'pattern'.</remarks>
		internal void DisposeWatcher()
		{
//			TooltipSort.Dispose(); // is static.

			// these are added to Controls so shouldn't need to be explicitly disposed ->
//			_editor.Dispose();

//			_scrollVert.Dispose();
//			_scrollHori.Dispose();

//			_panelCols .Dispose();
//			_panelRows .Dispose();
//			FrozenPanel.Dispose();

			// these are added to _panelCols.Controls so shouldn't need to be explicitly disposed ->
//			_labelid    .Dispose();
//			_labelfirst .Dispose();
//			_labelsecond.Dispose();
		} */


		#region Enums
		internal enum InfoType
		{
			INFO_NONE,	// 0
			INFO_CRAFT,	// 1
			INFO_SPELL,	// 2
			INFO_FEAT,	// 3
			INFO_CLASS	// 4
		}
		#endregion Enums


		#region Fields (static)
		internal static int WidthRowhead;
		internal static int HeightColhead;

		static int HeightRow;

		const int SORT_DES = -1;
		const int SORT_ASC =  1;

		/// <summary>
		/// Is used only for painting in the various <c>Paint</c> events.
		/// </summary>
		internal static Graphics graphics;

		/// <summary>
		/// Min width of a cell - ergo of a colhead even if width of
		/// colhead-text is narrower.
		/// </summary>
		internal static int _wId;

		/// <summary>
		/// Width of "****" in pixels.
		/// </summary>
		static int _wStars;

		internal const int FreezeId     = 1; // count of Cols that are frozen ->
		internal const int FreezeFirst  = 2;
		internal const int FreezeSecond = 3;

		/// <summary>
		/// A flag that stops presumptive .NET events from firing ~50 billion.
		/// </summary>
		internal static bool _init;
		#endregion Fields (static)


		#region Fields
		internal readonly Yata _f;

		/// <summary>
		/// For cycling through all tables or for scrolling a sync-table.
		/// </summary>
		YataGrid _table;

		internal int ColCount;
		internal int RowCount;

		internal readonly List<Col> Cols = new List<Col>();
		internal readonly List<Row> Rows = new List<Row>();

				 int OffsetVert; // TODO: these are redundant w/ the scrollbars' Value property ->
		internal int OffsetHori;

		internal YataPanelCols _panelCols;
				 YataPanelRows _panelRows;

		YataPanelFrozen FrozenPanel;

		Label _labelid     = new Label();
		Label _labelfirst  = new Label();
		Label _labelsecond = new Label();

		/// <summary>
		/// The text-edit box. Note there is only one (1) <c>TextBox</c> that
		/// floats to wherever it's required.
		/// </summary>
		internal readonly YataEditbox _editor = new YataEditbox();

		/// <summary>
		/// Set this <c>true</c> if you want to explicitly accept or reject the
		/// text in the <c><see cref="_editor"/></c>.
		/// </summary>
		internal bool _bypassleaveditor;

		/// <summary>
		/// The <c><see cref="Cell"/></c> that's currently under the
		/// <c><see cref="_editor"/></c>.
		/// </summary>
		Cell _editcell;

		/// <summary>
		/// Tracks the currently anchored <c><see cref="Cell"/></c> for
		/// selection of multiple cells by <c>[Shift]</c>+click or
		/// <c>[Shift]</c>+keyboard.
		/// </summary>
		internal Cell _anchorcell;

		/// <summary>
		/// The currently sorted col. Default is #0 "id" col.
		/// </summary>
		int _sortcol;

		/// <summary>
		/// The current sort direction. Default is sorted ascending.
		/// </summary>
		/// <remarks>Initialize <c>_sortdir</c> in the cTor or else #develop
		/// suggests "Convert to constant" (due to partial class usage).</remarks>
		int _sortdir;

		internal Propanel Propanel;

		internal UndoRedo _ur;

		internal string _defaultval = String.Empty;

//		Bitmap _bluePi = Resources.bluepixel; // NOTE: Image drawing introduces a noticeable latency.
//		Bitmap _piColhead;
//		Bitmap _piRowhead;
		#endregion Fields


		#region Properties
		internal string Fullpath // Path-File-Extension
		{ get; set; }

		internal bool Readonly
		{ get; set; }

		bool _changed;
		internal bool Changed
		{
			get { return _changed; }
			set
			{
				_changed = value;

				if (!_f.IsSaveAll) // delay setting all tab-texts
					_f.SetTabText(this);
			}
		}

		/// <summary>
		/// The colhead labels.
		/// </summary>
		/// <remarks>Does NOT contain #0 id-col (so it typically needs +/-1).</remarks>
		internal string[] Fields
		{ get; private set; }

		internal InfoType Info
		{ get; set; }

		int _frozenCount = FreezeId; // initialized w/ id-col only.
		/// <summary>
		/// The count of cols that currently appear on the
		/// <c><see cref="FrozenPanel"/></c>.
		/// <list type="bullet">
		/// <item><c><see cref="FreezeId"/></c> - id col only</item>
		/// <item><c><see cref="FreezeFirst"/></c> - +1st colfield</item>
		/// <item><c><see cref="FreezeSecond"/></c> - +2nd colfield</item>
		/// </list>
		/// </summary>
		internal int FrozenCount
		{
			get { return _frozenCount; }
			set
			{
				_frozenCount = value;

				_labelfirst .Visible = (_frozenCount > FreezeId);
				_labelsecond.Visible = (_frozenCount > FreezeFirst);

				WidthFrozenPanel();


				for (int c = 0; c != _frozenCount; ++c)
					Cols[c].selected = false; // clear any selected frozen cols

				// TODO: does that need invalidation^ - not really it gets covered by a frozen-label.

				int invalid = INVALID_FROZ;

				// clear any selected cells in a selected col unless they
				// are on a row that is selected or subselected ->

				Cell sel = getSelectedCell();
//				if (sel != null) // if only one cell is selected shift selection out of frozen cols ->
//				{
//					if (sel.x < _frozenCount)
//					{
//						sel.selected =
//						_editor.Visible = false;
//
//						if (_frozenCount < ColCount)
//						{
//							(_anchorcell = this[sel.y, _frozenCount]).selected = true;
//
//							EnsureDisplayed(_anchorcell);
//							invalid |= INVALID_GRID;
//						}
//					}
//				}
//				else // clear any selected cells that have become frozen unless the row is selected or subselected ->
				if (sel == null) // allow a single selected cell to stay selected
				{
					int selr = getSelectedRow();
					for (int r = 0; r != RowCount; ++r)
					{
						if (selr == -1 // <- this is covered by the next condition but do it for faster execution
							|| (r > selr && r > selr + RangeSelect)
							|| (r < selr && r < selr + RangeSelect))
						{
							for (int c = 0; c != _frozenCount; ++c)
							{
								if ((sel = this[r,c]).selected)
								{
									sel.selected = false;
									invalid |= INVALID_GRID;
								}
							}
						}
					}
				}

				if ((invalid & INVALID_GRID) != 0)
					_f.EnableCelleditOperations();

				if (Propanel != null && Propanel.Visible) // update bg-color of Propanel fields
					invalid |= INVALID_PROP;

				Invalidator(invalid);
			}
		}

		/// <summary>
		/// The quantity of rows that are flagged for an operation excluding the
		/// currently selected row.
		/// </summary>
		/// <remarks>The value will be negative if the range of subselected rows
		/// is above the currently selected row.</remarks>
		internal int RangeSelect
		{ get; set; }

		internal DateTime Lastwrite
		{ get; set; }
		#endregion Properties


		#region Indexers
		/// <summary>
		/// Gets/sets the <c><see cref="Cell"/></c> at loc
		/// <c>[</c><paramref name="r"/>, <paramref name="c"/><c>]</c>.
		/// </summary>
		internal Cell this[int r, int c]
		{
			get { return Rows[r][c]; }
			set { Rows[r][c] = value; }
		}
		#endregion Indexers


		#region cTor
		/// <summary>
		/// cTor. Instantiates a 2da-file in this <c>YataGrid</c> format/layout.
		/// </summary>
		/// <param name="f">parent</param>
		/// <param name="pfe">path_file_extension</param>
		/// <param name="read">readonly</param>
		internal YataGrid(Yata f, string pfe, bool read)
		{
//			DrawRegulator.SetDoubleBuffered(this);
			SetStyle(ControlStyles.OptimizedDoubleBuffer
				   | ControlStyles.AllPaintingInWmPaint
				   | ControlStyles.UserPaint
				   | ControlStyles.ResizeRedraw, true);

			_f = f;

			Fullpath = pfe;
			Readonly = read;

			_sortdir = SORT_ASC;

			_init = true;

			Dock      = DockStyle.Fill;
			BackColor = SystemColors.ControlDark;
			AllowDrop = true;

			_scrollVert.Dock = DockStyle.Right;
			_scrollVert.ValueChanged += OnScrollValueChanged_vert;

			_scrollHori.Dock = DockStyle.Bottom;
			_scrollHori.ValueChanged += OnScrollValueChanged_hori;

			Controls.Add(_scrollHori);
			Controls.Add(_scrollVert);

			_editor.Leave += editor_leave;

			Controls.Add(_editor);

			_ur = new UndoRedo(this);

			if (_t1 == null)
			{
				_t1 = new Timer();
				_t1.Interval = SystemInformation.DoubleClickTime;
				_t1.Tick += _t1_Tick;
			}
		}
		#endregion cTor


		#region Invalidate
		internal const int INVALID_NONE = 0x00;
		internal const int INVALID_GRID = 0x01; // this table
		internal const int INVALID_PROP = 0x02; // the Propanel
		internal const int INVALID_FROZ = 0x04; // the frozen panel (id,1st,2nd)
		internal const int INVALID_ROWS = 0x08; // the rowhead panel
		internal const int INVALID_COLS = 0x10; // the colhead panel
		internal const int INVALID_LBLS = 0x20; // the static labels that head the frozen panel

		/// <summary>
		/// Invalidates various controls of this grid for UI-update based on
		/// the <c><see cref="INVALID_NONE">INVALID</see></c> flags.
		/// </summary>
		/// <param name="invalid">a bitwise <c>int</c> of the
		/// <c><see cref="INVALID_NONE">INVALID</see></c> flags</param>
		/// <remarks>Check that <c><see cref="Propanel"/></c> is valid before a
		/// call w/ <c><see cref="INVALID_PROP"/></c>.</remarks>
		internal void Invalidator(int invalid)
		{
			if ((invalid & INVALID_GRID) != 0)
				Invalidate();

			if ((invalid & INVALID_PROP) != 0)
				Propanel.Invalidate();

			if ((invalid & INVALID_FROZ) != 0)
				FrozenPanel.Invalidate();

			if ((invalid & INVALID_ROWS) != 0)
				_panelRows.Invalidate();

			if ((invalid & INVALID_COLS) != 0)
				_panelCols.Invalidate();

			if ((invalid & INVALID_LBLS) != 0)
			{
				_labelid    .Invalidate();
				_labelfirst .Invalidate();
				_labelsecond.Invalidate();
			}
		}
		#endregion Invalidate


		#region eventhandler Resize
		/// <summary>
		/// Overrides the <c>Resize</c> handler.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>This fires whenever a fly sneezes.</remarks>
		protected override void OnResize(EventArgs e)
		{
			if (!_init)
			{
				//string t = System.IO.Path.GetFileNameWithoutExtension(Fullpath);

				for (int tab = 0; tab != _f.Tabs.TabCount; ++tab)
				{
					_table = _f.Tabs.TabPages[tab].Tag as YataGrid;

					//logfile.Log("(" + t + ") YataGrid.OnResize() tab= " + tab + " (set) _table= " + System.IO.Path.GetFileNameWithoutExtension(_table.Fullpath));

					// BLARG. The table can be invalid when a yata-process is
					// already running and user opens more, multiple files from
					// Windows Explorer via RMB-fileopen. Note that RMB-fileopen
					// exhibits different behavior than selecting files and
					// pressing [Enter]. Note also that Windows w7 does not
					// exhibit consistent behavior here when opening multiple
					// files; all the selected files might not get sent to
					// Program.Main()->Yata.CreatePage().

					if (_table != null)
					{
						_table.InitScroll();

						// NOTE: The panels can be null during the load sequence.
						if (_table._panelCols  != null) _table._panelCols .Width  = Width;
						if (_table._panelRows  != null) _table._panelRows .Height = Height;
						if (_table.FrozenPanel != null) _table.FrozenPanel.Height = Height;

						if (_table.Propanel != null && _table.Propanel.Visible)
							_table.Propanel.telemetric();

						if (!_f.IsMin) _table.EnsureDisplayed();

						if (_table == this
							&&  _f._diff1 != null   && _f._diff2 != null
							&& (_f._diff1 == _table || _f._diff2 == _table))
						{
							SyncDiffedGrids();
						}
					}
//					else // this is not reliable either ->
//					{
//						using (var ib = new Infobox(gs.InfoboxTitle_error,
//													"A table failed to open. Windows failed to send a file to Yata.",
//													null,
//													InfoboxType.Error))
//						{
//							ib.ShowDialog(_f);
//						}
//					}
				}
				//logfile.Log("(" + t + ") YataGrid.OnResize() (set) _table NULL");
				_table = null;

//				if (_piColhead != null) _piColhead.Dispose();
//				_piColhead = new Bitmap(_bluePi, new Size(WidthTable, HeightColhead));

//				if (_piRowhead != null) _piRowhead.Dispose();
//				_piRowhead = new Bitmap(_bluePi, new Size(WidthRowhead, HeightTable));
			}

			if (_f.WindowState != FormWindowState.Minimized)
				_f.IsMin = false;
		}
		#endregion eventhandler Resize


		#region Select (setters)
		/// <summary>
		/// Clears all <c><see cref="Cell">Cells</see></c>/
		/// <c><see cref="Row">Rows</see></c>/
		/// <c><see cref="Col">Cols</see></c> that are currently selected.
		/// </summary>
		/// <param name="bypassEnableCelledit"><c>true</c> to bypass
		/// <c><see cref="Yata.EnableCelleditOperations()">Yata.EnableCelleditOperations()</see></c>
		/// - this typically means that the caller shall select a <c>Cell</c>
		/// and call <c>EnableCelleditOperations()</c> itself or that selects
		/// are being cleared from a synced <c><see cref="YataGrid"/></c></param>
		/// <param name="bypassEnableRowedit"><c>true</c> to bypass
		/// <c><see cref="Yata.EnableRoweditOperations()">Yata.EnableRoweditOperations()</see></c>
		/// - this typically means that the caller shall select a <c>Row</c>
		/// since that calls <c>EnableRoweditOperations()</c> or that selects
		/// are being cleared from a synced <c><see cref="YataGrid"/></c></param>
		internal void ClearSelects(bool bypassEnableCelledit = false, bool bypassEnableRowedit = false)
		{
//			_anchorcell = null; // ~safety. Would need to go through all select patterns.

			foreach (var col in Cols)
			if (col.selected)
			{
				col.selected = false;
				break;
			}

			foreach (var row in Rows)
			{
				if (row.selected)
				{
					Row.BypassEnableRowedit = bypassEnableRowedit;
					row.selected = false;
					Row.BypassEnableRowedit = false;
				}

				for (int c = 0; c != ColCount; ++c)
					row[c].selected = false;
			}

			RangeSelect = 0; // otherwise Undoing the creation of a row-array leaves RangeSelect wonky.

			if (!bypassEnableCelledit)
				_f.EnableCelleditOperations();
		}


		/// <summary>
		/// Selects a specified <c><see cref="Cell"/></c> and invalidates stuff.
		/// </summary>
		/// <param name="cell">a <c>Cell</c> to select</param>
		/// <param name="sync"><c>true</c> to sync the select between diffed
		/// tables; <c>false</c> if sync will be performed by the caller</param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item><c><see cref="Yata"/>.Search()</c></item>
		/// <item><c><see cref="ReplaceTextDialog"/>.Search()</c></item>
		/// <item><c><see cref="YataGrid.GotoReplaced()">YataGrid.GotoReplaced()</see></c></item>
		/// <item><c><see cref="YataGrid.GotoLoadchanged()">YataGrid.GotoLoadchanged()</see></c></item>
		/// <item><c>Yata.SelectDiffCell()</c></item>
		/// <item><c><see cref="Propanel"/>.OnMouseClick()</c></item>
		/// </list></remarks>
		internal void SelectCell(Cell cell, bool sync = true)
		{
			if (sync) SelectSyncCell(cell);

			(_anchorcell = cell).selected = true;
			Invalidator(INVALID_GRID
					  | INVALID_FROZ
					  | INVALID_ROWS
					  | EnsureDisplayed(cell));

			_f.EnableCelleditOperations();
		}

		/// <summary>
		/// Clears all selects in a sync-table and then selects a specified
		/// <c><see cref="Cell">Cell's</see></c> corresponding <c>Cell</c> in
		/// the sync-table.
		/// </summary>
		/// <param name="sel">a <c>Cell</c> in the current table</param>
		/// <returns><c>true</c> if a sync-table is valid</returns>
		/// <remarks>Ensure <paramref name="sel"/> is valid before call.</remarks>
		bool SelectSyncCell(Cell sel)
		{
			YataGrid table = ClearSyncSelects();
			if (table != null)
			{
				if (sel.y < table.RowCount && sel.x < table.ColCount)
					(table._anchorcell = table[sel.y, sel.x]).selected = true;

				return true;
			}
			return false;
		}

		/// <summary>
		/// Clears all selects on a sync-table if that table is valid.
		/// </summary>
		/// <returns>the sync'd <c><see cref="YataGrid"/></c> if a sync-table is
		/// valid</returns>
		internal YataGrid ClearSyncSelects()
		{
			YataGrid table;
			if      (this == _f._diff1) table = _f._diff2;
			else if (this == _f._diff2) table = _f._diff1;
			else
				return null;

			if (table != null)
				table.ClearSelects(true, true);

			return table;
		}

		/// <summary>
		/// Focuses the table and selects only one of
		/// <list type="bullet">
		/// <item>the first <c><see cref="Cell"/></c> in table if no cell is
		/// selected</item>
		/// <item>the <c><see cref="_anchorcell"/></c> if valid</item>
		/// <item>the first selected <c>Cell</c> if multiple cells are currently
		/// selected</item>
		/// </list>
		/// 
		/// 
		/// No change if only one <c>Cell</c> is currently selected.
		/// </summary>
		/// <remarks>Called at the form-level by
		/// <c><see cref="Yata"/>.OnKeyDown()</c> <c>[Space]</c>.</remarks>
		internal void SelectFirstCell()
		{
			Select();

			Cell sel = getSelectedCell();
			if (sel == null)
			{
				if (_anchorcell != null && areSelectedCellsContiguous())
				{
					sel = _anchorcell;
				}
				else
					sel = getFirstSelectedCell();

				int r,c;
				if (sel != null)
				{
					r = sel.y;
					c = Math.Max(FrozenCount, sel.x);
				}
				else
				{
					r = 0;
					c = FrozenCount;
				}

				ClearSelects(true);

				if (c < ColCount)
				{
					sel = this[r,c];
					sel.selected = true;
					SelectSyncCell(sel);

					_anchorcell = sel;
				}
				else
				{
					sel = this[r,0]; // just a cell (for its row-id) to pass to EnsureDisplayed() below.
					ClearSyncSelects();

					_anchorcell = null;
				}

				Invalidator(INVALID_GRID
						  | INVALID_FROZ
						  | INVALID_ROWS
						  | EnsureDisplayed(sel));

				_f.EnableCelleditOperations();
			}

			if (sel == null || sel.x >= FrozenCount)
				_anchorcell = sel;
		}

		/// <summary>
		/// Selects a specified row by Id and flags its cells selected.
		/// </summary>
		/// <param name="r">row-id</param>
		/// <remarks>Check that <paramref name="r"/> doesn't over/underflow
		/// <c><see cref="RowCount"/></c> before call.</remarks>
		internal void SelectRow(int r)
		{
			Row row = Rows[r];

			row.selected = true;

			for (int c = 0; c != ColCount; ++c)
				row[c].selected = true;


			YataGrid table = ClearSyncSelects();
			if (table != null && r < table.RowCount)
			{
				row = table.Rows[r];

				Row.BypassEnableRowedit = true;
				row.selected = true;
				Row.BypassEnableRowedit = false;

				for (int c = 0; c != table.ColCount; ++c)
					row[c].selected = true;
			}

			_f.EnableCelleditOperations();
		}

		/// <summary>
		/// Focuses the table and clears all selects and
		/// <list type="bullet">
		/// <item>selects the first <c><see cref="Row"/></c> in table if no row
		/// is selected</item>
		/// <item>scrolls to the selected <c>Row</c> if a row is selected</item>
		/// </list>
		/// </summary>
		/// <remarks>Called at the form-level by
		/// <c><see cref="Yata"/>.OnKeyDown()</c> <c>[Ctrl+Space]</c>.</remarks>
		internal void SelectFirstRow()
		{
			Select();

			int selr = getSelectedRow();
			if (selr == -1)
			{
				Cell cell = getFirstSelectedCell();
				if (cell != null) selr = cell.y;
				else              selr = 0;
			}

			ClearSelects();

			SelectRow(selr);
			EnsureDisplayedRow(selr);

			int invalid = YataGrid.INVALID_GRID
						| YataGrid.INVALID_FROZ
						| YataGrid.INVALID_ROWS;

			if (Propanel != null && Propanel.Visible)
				invalid |= YataGrid.INVALID_PROP;

			Invalidator(invalid);
		}
		#endregion Select (setters)


		#region Select (getters)
		/// <summary>
		/// Checks if only one <c><see cref="Cell"/></c> is currently selected
		/// and returns it if so.
		/// </summary>
		/// <returns>the only <c>Cell</c> selected, else <c>null</c> if none or
		/// more than one is selected</returns>
		internal Cell getSelectedCell()
		{
			Cell cell0 = null;

			Cell cell;
			foreach (var row in Rows)
			for (int c = 0; c != ColCount; ++c)
			{
				if ((cell = row[c]).selected)
				{
					if (cell0 != null)
						return null;

					cell0 = cell;
				}
			}
			return cell0;
		}

		/// <summary>
		/// Gets the first selected <c><see cref="Cell"/></c> in the table else
		/// <c>null</c>.
		/// </summary>
		/// <param name="strt_c">start col-id usually either 0 (includes frozen
		/// cols) or <c><see cref="FrozenCount"/></c></param>
		/// <returns>the first <c>Cell</c> found else <c>null</c></returns>
		internal Cell getFirstSelectedCell(int strt_c = 0)
		{
			Cell sel;

			foreach (var row in Rows)
			for (int c = strt_c; c != ColCount; ++c)
				if ((sel = row[c]).selected)
					return sel;

			return null;
		}

		/// <summary>
		/// Gets the row-id of the currently selected <c><see cref="Row"/></c>.
		/// </summary>
		/// <returns>the currently selected row-id; <c>-1</c> if no <c>Row</c>
		/// is currently selected</returns>
		internal int getSelectedRow()
		{
			for (int r = 0; r != RowCount; ++r)
			if (Rows[r].selected)
				return r;

			return -1;
		}

		/// <summary>
		/// Gets the row-id of the currently selected <c><see cref="Row"/></c>
		/// or a row-id that has <c><see cref="Cell">Cells</see></c> selected
		/// iff only that <c>Row</c> has <c>Cells</c> selected.
		/// </summary>
		/// <param name="only"><c>true</c> if a <c>Row</c> is selected and you
		/// want to get that row-id only if it's the only <c>Row</c> with
		/// selected <c>Cells</c> - otherwise a selected <c>Row's</c> id shall
		/// be returned even though there can be <c>Cells</c> that are selected
		/// on another <c>Row</c></param>
		/// <returns>the currently selected row-id or the row-id that has
		/// selected <c>Cells</c>; <c>-1</c> if no <c>Row</c> is applicable</returns>
		internal int getSelectedRowOrCells(bool only = false)
		{
			int selr = getSelectedRow();
			if (selr == -1 || only)
			{
				Cell sel = getFirstSelectedCell();
				if (sel != null)
				{
					selr = sel.y;

					for (int r = sel.y + 1; r != RowCount; ++r)
					for (int c = 0;         c != ColCount; ++c)
						if (this[r,c].selected)
							return -1;
				}
			}
			return selr;
		}

		/// <summary>
		/// Gets the col-id of the currently selected <c><see cref="Col"/></c>.
		/// </summary>
		/// <returns>the currently selected col-id; <c>-1</c> if no col is
		/// currently selected</returns>
		internal int getSelectedCol()
		{
			for (int c = 0; c != ColCount; ++c)
			if (Cols[c].selected)
				return c;

			return -1;
		}


		/// <summary>
		/// Checks if any <c><see cref="Cell"/></c> is currently selected.
		/// </summary>
		/// <returns><c>true</c> if a <c>Cell</c> is selected</returns>
		internal bool anyCellSelected()
		{
			foreach (var row in Rows)
			for (int c = 0; c != ColCount; ++c)
			{
				if (row[c].selected)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Checks if any <c><see cref="Cell"/></c>/ <c><see cref="Row"/></c>/
		/// <c><see cref="Col"/></c> is currently selected.
		/// </summary>
		/// <returns><c>true</c> if a <c>Cell</c>/ <c>Row</c>/ <c>Col</c> is
		/// selected</returns>
		internal bool anySelected()
		{
			foreach (var row in Rows)
			{
				if (row.selected)
					return true;

				for (int c = 0; c != ColCount; ++c)
				{
					if (Cols[c].selected || row[c].selected)
						return true;
				}
			}
			return false;
		}
		#endregion Select (getters)


		#region editresult
		/// <summary>
		/// Starts cell-edit on
		/// <list type="bullet">
		/// <item><c>MouseClick</c></item>
		/// <item><c>MouseDoubleClick</c></item>
		/// <item><c><see cref="Yata"/>.cellit_Edit</c></item>
		/// </list>
		/// </summary>
		/// <param name="cell">a <c><see cref="Cell"/> to edit</c></param>
		internal void startCelledit(Cell cell)
		{
			_editcell = cell;
			Celledit();
		}

		/// <summary>
		/// Starts cell-edit on
		/// <list type="bullet">
		/// <item><c>[Enter]</c></item>
		/// <item>Tabfastedit</item>
		/// <item><c><see cref="startCelledit()">startCelledit()</see></c></item>
		/// </list>
		/// </summary>
		/// <remarks>Ensure that <c><see cref="_editcell"/></c> is valid before
		/// call.</remarks>
		void Celledit()
		{
			EnsureDisplayed(_editcell);

			Rectangle rect = getCellRectangle(_editcell); // align the editbox over the text ->
			_editor.Left   = rect.X + 5;
			_editor.Top    = rect.Y + 4;
			_editor.Width  = rect.Width - 6;
			_editor.Height = rect.Height;

			_editor.Text = _editcell.text;

			_editor.SelectionStart = 0;
			_editor.SelectionLength = _editor.Text.Length;

			_editor.Visible = true;
			_editor.Focus();

			Invalidator(INVALID_GRID);
		}

		/// <summary>
		/// Gets the cell-rectangle for a given <c><see cref="Cell"/></c>.
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		Rectangle getCellRectangle(Cell cell)
		{
			var rect = new Rectangle();

			rect.X = WidthRowhead - OffsetHori;
			for (int c = 0; c != cell.x; ++c)
				rect.X += Cols[c].Width;

			rect.Y = HeightColhead - OffsetVert;
			for (int r = 0; r != cell.y; ++r)
				rect.Y += HeightRow;

			rect.Width = Cols[cell.x].Width;
			rect.Height = HeightRow;

			return rect;
		}


		/// <summary>
		/// Handles the <c>Leave</c> event for the <c><see cref="_editor"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="_editor"/></c></param>
		/// <param name="e"></param>
		/// <remarks>It's better to set (<c>_editor.Visible=false</c>) before
		/// the <c>Leave</c> event fires - note that the <c>Leave</c> event will
		/// still consider the editor <c>Visible</c> - otherwise .net fires the
		/// <c>Leave</c> event twice.</remarks>
		/// <seealso cref="Propanel"><c>Propanel.editor_leave()</c></seealso>
		void editor_leave(object sender, EventArgs e)
		{
			if (!_bypassleaveditor)
			{
				if (Settings._acceptedit)
				{
					//logfile.Log(". Settings._acceptedit");
					editresultaccept(); // do NOT focus the table here. Do it in the calling funct if req'd.
				}
				else
				{
					//logfile.Log(". Settings._acceptedit FALSE");
					editresultcancel(INVALID_GRID); // do NOT focus the table.
				}
			}
			else
				_bypassleaveditor = false;
		}

		/// <summary>
		/// Applies a cell-edit via the <c><see cref="_editor"/></c>.
		/// </summary>
		/// <param name="select"><c>true</c> to focus this <c>YataGrid</c></param>
		/// <remarks>Shows an <c><see cref="Infobox"/></c> if the text gets
		/// sanitized.</remarks>
		/// <seealso cref="Propanel.editresultaccept()"><c>Propanel.editresultaccept()</c></seealso>
		internal void editresultaccept(bool @select = false)
		{
			int invalid = INVALID_GRID;

			bool sanitized = false;

			if (_editor.Text != _editcell.text)
			{
				sanitized = ChangeCellText(_editcell, _editor); // does a text-check

				if (Propanel != null && Propanel.Visible)
					invalid |= INVALID_PROP;
			}
			else
			{
				if (_editcell.replaced)
					ClearReplaced(_editcell);

				if (_editcell.loadchanged)
					ClearLoadchanged(_editcell);
			}

			editresultcancel(invalid, @select);

			if (sanitized)
			{
				using (var ib = new Infobox(Infobox.Title_warni,
											"The text that was submitted has been altered.",
											null,
											InfoboxType.Warn))
				{
					ib.ShowDialog(_f);
				}
			}
		}

		/// <summary>
		/// Hides <c><see cref="_editor"/></c> and focuses this <c>YataGrid</c>.
		/// </summary>
		/// <param name="invalid">a bitwise <c>int</c> of the
		/// <c><see cref="INVALID_NONE">INVALID</see></c> flags</param>
		/// <param name="select"><c>true</c> to focus this <c>YataGrid</c> - if
		/// <c>false</c> the calling funct shall focus a <c>Control</c> itself</param>
		/// <seealso cref="Propanel.editresultcancel()"><c>Propanel.editresultcancel()</c></seealso>
		internal void editresultcancel(int invalid, bool @select = false)
		{
			_editor.Visible = false;
			Invalidator(invalid);

			if (@select) Select();
		}

		/// <summary>
		/// Checks for and ensures that both <c><see cref="_editor"/></c> and
		/// <c><see cref="Propanel._editor">Propanel._editor</see></c> hide.
		/// </summary>
		/// <remarks>After calling this ensure that focus changes so that the
		/// respective editor's <c>Leave</c> event fires, which shall either
		/// cancel or accept the text in <c><see cref="YataEditbox"/></c> per
		/// <c><see cref="Settings._acceptedit">Settings._acceptedit</see></c>.</remarks>
		internal void editresultdefault()
		{
			if (_editor.Visible)
				_editor.Visible = false;
			else if (Propanel != null && Propanel._editor.Visible)
				Propanel._editor.Visible = false;
		}


		/// <summary>
		/// Changes a cell's text by either
		/// <c><see cref="YataGrid._editor">YataGrid._editor</see></c> or
		/// <c><see cref="Propanel._editor">Propanel._editor</see></c>,
		/// recalculates col-width, and sets up Undo/Redo.
		/// </summary>
		/// <param name="cell">a <c><see cref="Cell"/></c></param>
		/// <param name="tb">the editor's <c>TextBox</c> whose text to check for
		/// validity</param>
		/// <returns><c>true</c> if the text gets sanitized</returns>
		/// <remarks>Performs text verification. This function does not flag a
		/// table <c><see cref="Changed"/></c> nor does it push an Undo
		/// <c><see cref="Restorable"/></c> etc. if the submitted text is
		/// identical to existing text. It does however clear
		/// <c><see cref="Cell.replaced">Cell.replaced</see></c>,
		/// <c><see cref="Cell.loadchanged">Cell.loadchanged</see></c> and
		/// <c><see cref="Cell.diff">Cell.diff</see></c> flags regardless.</remarks>
		internal bool ChangeCellText(Cell cell, Control tb)
		{
			Restorable rest = UndoRedo.createCell(cell);
			if (!Changed)
			{
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}


			if (cell.replaced)
				cell.replaced = false;

			if (cell.loadchanged)
				cell.loadchanged = false;

			if (cell.diff)
				cell.diff = false; // TODO: Check the differ if the celltext is identical or still different.

			bool sanitized = VerifyText_edit(tb);

			if (cell.text != tb.Text)
			{
				cell.text = tb.Text;

				Colwidth(cell.x, cell.y);
				MetricFrozenControls(cell.x);

				if (!Changed) Changed = true;


				rest.postext = cell.text;
				_ur.Push(rest);
			}

			return sanitized;
		}

		/// <summary>
		/// Changes a cell's text by <c><see cref="ReplaceTextDialog"/></c>,
		/// recalculates col-width, and sets up Undo/Redo.
		/// </summary>
		/// <param name="cell">a <c><see cref="Cell"/></c></param>
		/// <param name="text">the text to change to</param>
		/// <returns><c>true</c> if the text gets sanitized</returns>
		internal bool ChangeCellText_repl(Cell cell, string text)
		{
			Restorable rest = UndoRedo.createCell(cell);
			if (!Changed)
			{
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}


//			if (cell.replaced)
//				cell.replaced = false; // don't clear replaced; this funct *sets* replaced

			if (cell.loadchanged)
				cell.loadchanged = false;

			if (cell.diff)
				cell.diff = false; // TODO: Check the differ if the celltext is identical or still different.

			bool sanitized = VerifyText_repl(ref text);

			if (cell.text != text)
			{
				cell.text = text;

				Colwidth(cell.x, cell.y);
				MetricFrozenControls(cell.x);

				if (!Changed) Changed = true;


				rest.postext = cell.text;
				_ur.Push(rest);
			}

			if (sanitized)
			{
				ReplaceTextDialog.Cords cords;
				cords.r = cell.y;
				cords.c = cell.x;

				ReplaceTextDialog.Warns.Add(cords);
			}

			return sanitized;
		}

		/// <summary>
		/// Changes a cell's text by any of the many stock methods in Yata,
		/// recalculates col-width, and sets up Undo/Redo.
		/// </summary>
		/// <param name="cell">a <c><see cref="Cell"/></c></param>
		/// <param name="text">the text to change to</param>
		/// <remarks>Does *not* perform text verification. A check should be
		/// done for if the texts differ before calling this function since the
		/// table shall be flagged <c><see cref="Changed"/></c>.</remarks>
		internal void ChangeCellText(Cell cell, string text)
		{
			// TODO: Optimize this for multiple calls/cells.

			Restorable rest = UndoRedo.createCell(cell);
			if (!Changed)
			{
				Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}


			if (cell.replaced)
				cell.replaced = false;

			if (cell.loadchanged)
				cell.loadchanged = false;

			if (cell.diff)
				cell.diff = false; // TODO: Check the differ if the celltext is identical or still different.

			cell.text = text;

			Colwidth(cell.x, cell.y);
			MetricFrozenControls(cell.x);

			int invalid = INVALID_GRID;
			if (Propanel != null && Propanel.Visible)
				invalid |= INVALID_PROP;

			Invalidator(invalid);


			rest.postext = cell.text;
			_ur.Push(rest);
		}


		/// <summary>
		/// Verifies a cell-field text during user-edits.
		/// </summary>
		/// <param name="tb">a <c>TextBox</c> in which user is editing the text</param>
		/// <returns><c>true</c> to notify user if text gets changed</returns>
		internal static bool VerifyText_edit(Control tb)
		{
			string text = tb.Text; // allow whitespace - do not Trim()
			if (text.Length == 0)
			{
				tb.Text = gs.Stars;
				return false; // don't bother the user if he/she simply wants to blank a field.
			}

			bool sanitized = VerifyText(ref text);

			tb.Text = text;
			return sanitized;
		}

		/// <summary>
		/// Verifies a cell-field text for
		/// <c><see cref="ReplaceTextDialog"/></c>.
		/// </summary>
		/// <param name="text">the text to verify</param>
		/// <returns><c>true</c> if text gets sanitized</returns>
		static bool VerifyText_repl(ref string text)
		{
			if (text.Length == 0) // allow whitespace - do not Trim()
			{
				text = gs.Stars;
				return false; // don't bother the user if he/she simply wants to blank a field.
			}
			return VerifyText(ref text);
		}

		/// <summary>
		/// Verifies a cell-field text. Called during file load by
		/// <c><see cref="CreateRows()">CreateRows()</see></c> or after
		/// user-edits by
		/// <c><see cref="VerifyText_edit()">VerifyText_edit()</see></c> or
		/// during replace/replaceall by
		/// <c><see cref="VerifyText_repl()">VerifyText_repl()</see></c>.
		/// </summary>
		/// <param name="text">ref to a text-string</param>
		/// <param name="load"><c>true</c> to return <c>true</c> even if a
		/// text-change is insignificant</param>
		/// <returns><c>true</c> if (a) text gets sanitized and user should be
		/// notified or (b) text gets sanitized and its <c><see cref="Cell"/></c>
		/// should be flagged as <c><see cref="Cell.loadchanged"/></c> when a
		/// 2da-file loads</returns>
		static bool VerifyText(ref string text, bool load = false)
		{
			var sb = new StringBuilder(text);

			bool quote = false;

			if (sb.Length != 0
				&& sb[0] != '"' && sb[sb.Length - 1] != '"') // totally unquoted ->
			{
				char c;
				for (int i = 0; i != sb.Length; ++i)
				{
					if ((c = sb[i]) == '"')
					{
						quote = false;
						break;
					}

					if (Char.IsWhiteSpace(c))
						quote = true;
				}

				if (quote) // add quotes and return unless there's a misplaced quotation mark (as detered above) ->
				{
					applyQuotes(sb);

					text = sb.ToString();

					if (load)
						return true;

					return Settings._strict; // <- bother user if he/she wants auto-quotes only if Strict.
				}
			}


			string verified; bool sanitized;

			quote = sb.Length != 0
				 && sb[0] == '"' && sb[sb.Length - 1] == '"'; // -> has outer quotes

			sb = sb.Replace("\"", null); // remove all quotation marks

			if (sb.Length == 0)
			{
				text = gs.Stars;
				return true; // -> inform user
			}

			if (!quote) // not quoted, check for whitespace ->
			{
				for (int i = 0; i != sb.Length; ++i)
				{
					if (Char.IsWhiteSpace(sb[i]))
					{
						applyQuotes(sb);

						text = sb.ToString();
						return true; // -> inform user
					}
				}
			}
			else if (Settings._clearquotes) // quoted but user doesn't want quotes (unless there's whitespace) ->
			{
				bool cleared = true;

				for (int i = 0; i != sb.Length; ++i)
				{
					if (Char.IsWhiteSpace(sb[i]))
					{
						applyQuotes(sb);

						cleared = false;
						break;
					}
				}

				if (cleared) // existing quotes were successfully cleared ->
				{
					verified = sb.ToString();
					sanitized = Settings._strict && verified != text; // <- bother user if he/she wants to clear quotes only if Strict.

					text = verified;
					return sanitized;
				}
			}
			else // reapply quotes ->
			{
				applyQuotes(sb);
			}

			sanitized = ((verified = sb.ToString()) != text); // <- if changed inform user.

			text = verified;
			return sanitized;
		}

		/// <summary>
		/// Applies double-quotes to start and end of specified
		/// <c>StringBuilder</c> data.
		/// </summary>
		/// <param name="sb"><c>StringBuilder</c> data</param>
		static void applyQuotes(StringBuilder sb)
		{
			sb.Insert(0, '"');
			sb.Append('"');
		}
		#endregion editresult


		#region replaced
		/// <summary>
		/// Checks if any <c><see cref="Cell"/></c> is currently
		/// <c><see cref="Cell.replaced">Cell.replaced</see></c>.
		/// </summary>
		/// <returns><c>true</c> if a <c>Cell</c> is <c>replaced</c></returns>
		internal bool anyReplaced()
		{
			foreach (var row in Rows)
			for (int c = 0; c != ColCount; ++c)
			if (row[c].replaced)
				return true;

			return false;
		}

		/// <summary>
		/// Clears all <c><see cref="Cell">Cells'</see></c>
		/// <c><see cref="Cell.replaced">Cell.replaced</see></c> flags without
		/// resetting the GotoReplaced it repeatedly.
		/// </summary>
		/// <param name="activetable"><c>true</c> if the <c>YataGrid</c> to clear is
		/// currently active - see
		/// <c><see cref="Yata.CloseReplacer()">Yata.CloseReplacer()</see></c></param>
		internal void ClearReplaced(bool activetable = true)
		{
			_init = true; // bypass EnableGotoReplaced() in Cell.setter_replaced

			foreach (var row in Rows)
			for (int c = 0; c != ColCount; ++c)
			if (row[c].replaced)
				row[c].replaced = false;

			_init = false;

			_f.EnableGotoReplaced(false);
			if (_f._replacer != null)
				_f._replacer.EnableReplacedOps(false);

			if (activetable)
				Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
		}

		/// <summary>
		/// Clears <c><see cref="Cell.replaced">Cell.replaced</see></c> from a
		/// specified <c><see cref="Cell"/></c> and invalidates the grid.
		/// </summary>
		/// <param name="cell"></param>
		/// <remarks>Check <c>Cell.replaced</c> before call.</remarks>
		internal void ClearReplaced(Cell cell)
		{
			cell.replaced = false;
			Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
		}

		/// <summary>
		/// Selects the next/previous <c><see cref="Cell"/></c> this has its
		/// <c><see cref="Cell.replaced">Cell.replaced</see></c> flag set.
		/// </summary>
		/// <param name="forward"><c>true</c> to perform forward search</param>
		internal void GotoReplaced(bool forward)
		{
//			if (anyReplaced()) // safety-ish. That probably shouldn't be needed.
//			{
			Select();

			Cell sel = getSelectedCell();
			int selr = getSelectedRow();

			ClearSelects();
			// SelectCell() calls ClearSyncSelects()

			int r,c;

			bool start = true;

			if (forward) // forward search ->
			{
				if (sel != null) { c = sel.x; selr = sel.y; }
				else
				{
					c = -1;
					if (selr == -1) selr = 0;
				}

				for (r = selr; r != RowCount; ++r)
				{
					if (start)
					{
						start = false;
						if (++c == ColCount)		// if starting on the last cell of a row
						{
							c = 0;

							if (r < RowCount - 1)	// jump to the first cell of the next row
								++r;
							else					// or to the top of the table if on the last row
								r = 0;
						}
					}
					else
						c = 0;

					for (; c != ColCount; ++c)
					{
						if ((sel = this[r,c]).replaced)
						{
							SelectCell(sel);
							return;
						}
					}
				}

				// TODO: tighten exact start/end-cells
				for (r = 0; r != selr + 1; ++r) // quick and dirty wrap ->
				for (c = 0; c != ColCount; ++c)
				{
					if ((sel = this[r,c]).replaced)
					{
						SelectCell(sel);
						return;
					}
				}
			}
			else // backward search ->
			{
				if (sel != null) { c = sel.x; selr = sel.y; }
				else
				{
					c = ColCount;
					if (selr == -1) selr = RowCount - 1;
				}

				for (r = selr; r != -1; --r)
				{
					if (start)
					{
						start = false;
						if (--c == -1)	// if starting on the first cell of a row
						{
							c = ColCount - 1;

							if (r > 0)	// jump to the last cell of the previous row
								--r;
							else		// or to the bottom of the table if on the first row
								r = RowCount - 1;
						}
					}
					else
						c = ColCount - 1;

					for (; c != -1; --c)
					{
						if ((sel = this[r,c]).replaced)
						{
							SelectCell(sel);
							return;
						}
					}
				}

				// TODO: tighten exact start/end-cells
				for (r = RowCount - 1; r != selr - 1; --r) // quick and dirty wrap ->
				for (c = ColCount - 1; c != -1;       --c)
				{
					if ((sel = this[r,c]).replaced)
					{
						SelectCell(sel);
						return;
					}
				}
			}

//			int invalid = YataGrid.INVALID_GRID
//						| YataGrid.INVALID_FROZ
//						| YataGrid.INVALID_ROWS
//						| YataGrid.INVALID_COLS;
//			if (Propanel != null && Propanel.Visible)
//				invalid |= YataGrid.INVALID_PROP;

//			Invalidator(invalid);
		}
//		}
		#endregion replaced


		#region loadchanged
		/// <summary>
		/// Checks if any <c><see cref="Cell"/></c> is currently
		/// <c><see cref="Cell.loadchanged">Cell.loadchanged</see></c>.
		/// </summary>
		/// <returns><c>true</c> if a <c>Cell</c> is <c>loadchanged</c></returns>
		internal bool anyLoadchanged()
		{
			foreach (var row in Rows)
			for (int c = 0; c != ColCount; ++c)
			if (row[c].loadchanged)
				return true;

			return false;
		}

		/// <summary>
		/// Clears all cells'
		/// <c><see cref="Cell.loadchanged">Cell.loadchanged</see></c> flags
		/// without resetting the GotoLoadchanged it repeatedly.
		/// </summary>
		internal void ClearLoadchanged()
		{
			_init = true; // bypass EnableGotoLoadchanged() in Cell.setter_loadchanged

			foreach (var row in Rows)
			for (int c = 0; c != ColCount; ++c)
			if (row[c].loadchanged)
				row[c].loadchanged = false;

			_init = false;

			_f.EnableGotoLoadchanged(false);

			Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
		}

		/// <summary>
		/// Clears <c><see cref="Cell.loadchanged">Cell.loadchanged</see></c>
		/// from a specified <c><see cref="Cell"/></c>.
		/// </summary>
		/// <param name="cell"></param>
		/// <remarks>Check <c>Cell.loadchanged</c> before call.</remarks>
		internal void ClearLoadchanged(Cell cell)
		{
			cell.loadchanged = false;
			Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
		}

		/// <summary>
		/// Selects the next/previous <c><see cref="Cell"/></c> this has its
		/// <c><see cref="Cell.loadchanged">Cell.loadchanged</see></c> flag set.
		/// </summary>
		/// <param name="forward"><c>true</c> to perform forward search</param>
		internal void GotoLoadchanged(bool forward)
		{
//			if (anyLoadchanged()) // safety-ish. That probably shouldn't be needed.
//			{
			Select();

			Cell sel = getSelectedCell();
			int selr = getSelectedRow();

			ClearSelects();
			// SelectCell() calls ClearSyncSelects()

			int r,c;

			bool start = true;

			if (forward) // forward search ->
			{
				if (sel != null) { c = sel.x; selr = sel.y; }
				else
				{
					c = -1;
					if (selr == -1) selr = 0;
				}

				for (r = selr; r != RowCount; ++r)
				{
					if (start)
					{
						start = false;
						if (++c == ColCount)		// if starting on the last cell of a row
						{
							c = 0;

							if (r < RowCount - 1)	// jump to the first cell of the next row
								++r;
							else					// or to the top of the table if on the last row
								r = 0;
						}
					}
					else
						c = 0;

					for (; c != ColCount; ++c)
					{
						if ((sel = this[r,c]).loadchanged)
						{
							SelectCell(sel);
							return;
						}
					}
				}

				// TODO: tighten exact start/end-cells
				for (r = 0; r != selr + 1; ++r) // quick and dirty wrap ->
				for (c = 0; c != ColCount; ++c)
				{
					if ((sel = this[r,c]).loadchanged)
					{
						SelectCell(sel);
						return;
					}
				}
			}
			else // backward search ->
			{
				if (sel != null) { c = sel.x; selr = sel.y; }
				else
				{
					c = ColCount;
					if (selr == -1) selr = RowCount - 1;
				}

				for (r = selr; r != -1; --r)
				{
					if (start)
					{
						start = false;
						if (--c == -1)	// if starting on the first cell of a row
						{
							c = ColCount - 1;

							if (r > 0)	// jump to the last cell of the previous row
								--r;
							else		// or to the bottom of the table if on the first row
								r = RowCount - 1;
						}
					}
					else
						c = ColCount - 1;

					for (; c != -1; --c)
					{
						if ((sel = this[r,c]).loadchanged)
						{
							SelectCell(sel);
							return;
						}
					}
				}

				// TODO: tighten exact start/end-cells
				for (r = RowCount - 1; r != selr - 1; --r) // quick and dirty wrap ->
				for (c = ColCount - 1; c != -1;       --c)
				{
					if ((sel = this[r,c]).loadchanged)
					{
						SelectCell(sel);
						return;
					}
				}
			}

//			int invalid = YataGrid.INVALID_GRID
//						| YataGrid.INVALID_FROZ
//						| YataGrid.INVALID_ROWS
//						| YataGrid.INVALID_COLS;
//			if (Propanel != null && Propanel.Visible)
//				invalid |= YataGrid.INVALID_PROP;

//			Invalidator(invalid);
//			}
		}
		#endregion loadchanged


		#region ensure displayed
		/// <summary>
		/// Scrolls the table so that the currently selected
		/// <c><see cref="Row"/></c> or first selected <c><see cref="Cell"/></c>
		/// is (more or less) completely displayed.
		/// </summary>
		/// <returns>a bitwise <c>int</c> defining controls that need to be
		/// invalidated</returns>
		/// <remarks>A selected <c>Row</c> has priority over any selected
		/// <c>Cells</c>.</remarks>
		internal int EnsureDisplayed()
		{
			int selr = getSelectedRow();
			if (selr != -1)
				return EnsureDisplayedRow(selr);

			Cell sel = getFirstSelectedCell();
			if (sel != null)
				return EnsureDisplayed(sel);

			return INVALID_NONE;
		}

		/// <summary>
		/// Scrolls the table so that a given <c><see cref="Cell"/></c> is (more
		/// or less) completely displayed.
		/// </summary>
		/// <param name="cell">the <c>Cell</c> to display</param>
		/// <param name="bypassPropanel"><c>true</c> to bypass any
		/// <c><see cref="Propanel"/></c> considerations</param>
		/// <returns>a bitwise <c>int</c> defining controls that need to be
		/// invalidated</returns>
		/// <remarks>The <c>Propanel's</c> invalidation bit will be flagged as
		/// long as the panel is visible regardless of whether it really needs
		/// to be redrawn.</remarks>
		internal int EnsureDisplayed(Cell cell, bool bypassPropanel = false)
		{
			int invalid = INVALID_NONE;

			if (cell.x >= FrozenCount)
			{
				Rectangle rect = getCellRectangle(cell);

				int left = getLeft();
				int bar;

				if (rect.X != left)
				{
					bar = (_visVert ? _scrollVert.Width : 0);
					int right = Width - bar;

					if (rect.X < left
						|| (rect.Width > right - left
							&& (rect.X > right || rect.X + left > (right - left) / 2)))	// <- for cells with width greater
					{																	// than the table's visible width.
						int val = _scrollHori.Value - left + rect.X;
						_scrollHori.Value = Math.Max(val, 0);
						invalid = INVALID_GRID;
					}
					else if (rect.X + rect.Width > right && rect.Width < right - left)
					{
						int val = _scrollHori.Value + rect.X + rect.Width + bar - Width;
						_scrollHori.Value = Math.Min(val, MaxHori);
						invalid = INVALID_GRID;
					}
				}

				if (rect.Y != HeightColhead)
				{
					if (rect.Y < HeightColhead)
					{
						int val = _scrollVert.Value - HeightColhead + rect.Y;
						_scrollVert.Value = Math.Max(val, 0);
						invalid = INVALID_GRID;
					}
					else
					{
						bar = (_visHori ? _scrollHori.Height : 0);
						if (rect.Y + rect.Height + bar > Height)
						{
							int val = _scrollVert.Value + rect.Y + rect.Height + bar - Height;
							_scrollVert.Value = Math.Min(val, MaxVert);
							invalid = INVALID_GRID;
						}
					}
				}
			}
			else // cell is in the FrozenPanel ->
			{
//				_scrollHori.Value = 0; // do not scroll hori; cell is already visible.
				invalid = EnsureDisplayedRow(cell.y);
			}

			if (!bypassPropanel && Propanel != null && Propanel.Visible)
			{
				Propanel.EnsureDisplayed(cell.x);
				invalid |= INVALID_PROP;
			}

			// TODO: Wait a second. Setting a scrollbar.Value auto-refreshes the grid ...
			return invalid;
		}

		/// <summary>
		/// Scrolls the table so that a given <c><see cref="Row"/></c> is (more
		/// or less) completely displayed.
		/// </summary>
		/// <param name="rowid">the row-id to display</param>
		/// <returns>a bitwise <c>int</c> defining controls that need to be
		/// invalidated</returns>
		internal int EnsureDisplayedRow(int rowid)
		{
			int posT = HeightColhead + HeightRow * rowid - OffsetVert;
			if (posT != HeightColhead)
			{
				if (posT < HeightColhead)
				{
					_scrollVert.Value -= HeightColhead - posT;
					return INVALID_GRID
						 | INVALID_FROZ
						 | INVALID_ROWS; // TODO: All those might not be needed ...
				}

				int posB = posT + HeightRow;
				int bar  = (_visHori ? _scrollHori.Height : 0);
				if (posB + bar > Height)
				{
					_scrollVert.Value += posB + bar - Height;
					return INVALID_GRID
						 | INVALID_FROZ
						 | INVALID_ROWS; // TODO: All those might not be needed ...
				}
			}

			// TODO: Wait a second. Setting a scrollbar.Value auto-refreshes the grid ...
			return INVALID_NONE;
		}

		/// <summary>
		/// Scrolls the table so that a given <c><see cref="Col"/></c> is (more
		/// or less) completely displayed.
		/// </summary>
		/// <param name="colid">the col-id to display</param>
		internal void EnsureDisplayedCol(int colid)
		{
			int posL = WidthRowhead - OffsetHori;
			for (int c = 0; c != colid; ++c)
				posL += Cols[c].Width;

			int posR = posL + Cols[colid].Width;

			int left = getLeft();
			if (posL != left)
			{
				int bar = (_visVert ? _scrollVert.Width : 0);
				int right = Width - bar;

				int width = posR - posL;

				if (posL < left
					|| (width > right - left
						&& (posL > right || posL + left > (right - left) / 2)))
				{
					_scrollHori.Value -= left - posL;
				}
				else if (posR > right && width < right - left)
				{
					_scrollHori.Value += posR + bar - Width;
				}
			}
		}

		/// <summary>
		/// Gets the x-pos of the right edge of the frozen-panel; ie. the left
		/// edge of the visible/editable area of this <c>YataGrid</c>.
		/// </summary>
		/// <returns></returns>
		int getLeft()
		{
			int left = WidthRowhead;
			for (int c = 0; c != FrozenCount; ++c)
				left += Cols[c].Width;

			return left;
		}
		#endregion ensure displayed


		#region Goto
		/// <summary>
		/// Performs a goto when the <c>[Enter]</c> key is pressed and focus is
		/// on the goto-box.
		/// </summary>
		/// <param name="text">a <c>string</c> to parse for a row-id</param>
		/// <param name="select"><c>true</c> to <c>Select()</c> this
		/// <c>YataGrid</c></param>
		internal void Goto(string text, bool @select)
		{
			int selr;
			if (Int32.TryParse(text, out selr)
				&& selr > -1 && selr < RowCount)
			{
				ClearSelects();

				SelectRow(selr);
				EnsureDisplayedRow(selr);

				int invalid = INVALID_GRID
							| INVALID_FROZ
							| INVALID_ROWS;
				if (Propanel != null && Propanel.Visible)
					invalid |= INVALID_PROP;

				Invalidator(invalid);

				if (@select) // on [Enter] ie. not instantgoto
					Select();
			}
		}
		#endregion Goto


		#region Search
		/// <summary>
		/// A <c>string</c> to search for.
		/// </summary>
		/// <remarks>The value is set in
		/// <c><see cref="Search()">Search()</see></c>.</remarks>
		string _search;

		/// <summary>
		/// <c>true</c> to search for a subfield instead of a wholefield.
		/// </summary>
		/// <remarks>The value is set in
		/// <c><see cref="Search()">Search()</see></c>.</remarks>
		bool _substr;

		/// <summary>
		/// A <c>string</c> of text to match against
		/// <c><see cref="_search"/></c>.
		/// </summary>
		/// <remarks>The value is set in
		/// <c><see cref="SearchResult()">SearchResult()</see></c>.</remarks>
		string _text;


		/// <summary>
		/// Searches this <c>YataGrid</c> for the text in the search-box.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="substr"></param>
		/// <param name="forward"></param>
		/// <returns><c>true</c> if a match is found</returns>
		/// <remarks>Ensure that <c><see cref="Yata.Table">Yata.Table</see></c>
		/// is valid before call.</remarks>
		/// <seealso cref="ReplaceTextDialog"><c>ReplaceTextDialog.Search()</c></seealso>
		internal bool Search(string text, bool substr, bool forward)
		{
			if ((ModifierKeys & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				if ((_search = text).Length != 0)
				{
					_search = _search.ToUpperInvariant();

					Cell sel = getSelectedCell();
					int selr = getSelectedRow();

					ClearSelects();
					ClearSyncSelects();


					_substr = substr; // else is Wholefield search.

					bool start = true;

					int r,c;

					if (forward) // forward ->
					{
						if (sel != null) { c = sel.x; selr = sel.y; }
						else
						{
							c = -1;
							if (selr == -1) selr = 0;
						}

						for (r = selr; r != RowCount; ++r)
						{
							if (start)
							{
								start = false;
								if (++c == ColCount)		// if starting on the last cell of a row
								{
									c = 0;

									if (r < RowCount - 1)	// jump to the first cell of the next row
										++r;
									else					// or to the top of the table if on the last row
										r = 0;
								}
							}
							else
								c = 0;

							for (; c != ColCount; ++c)
							{
								if (c != 0 && SearchResult(r,c)) // don't search the id-col
								{
									SelectCell(this[r,c]);
									return true;
								}
							}
						}

						// TODO: tighten exact start/end-cells
						for (r = 0; r != selr + 1; ++r) // quick and dirty wrap ->
						for (c = 0; c != ColCount; ++c)
						{
							if (c != 0 && SearchResult(r,c)) // don't search the id-col
							{
								SelectCell(this[r,c]);
								return true;
							}
						}
					}
					else // backward ->
					{
						if (sel != null) { c = sel.x; selr = sel.y; }
						else
						{
							c = ColCount;
							if (selr == -1) selr = RowCount - 1;
						}

						for (r = selr; r != -1; --r)
						{
							if (start)
							{
								start = false;
								if (--c == -1)	// if starting on the first cell of a row
								{
									c = ColCount - 1;

									if (r > 0)	// jump to the last cell of the previous row
										--r;
									else		// or to the bottom of the table if on the first row
										r = RowCount - 1;
								}
							}
							else
								c = ColCount - 1;

							for (; c != -1; --c)
							{
								if (c != 0 && SearchResult(r,c)) // don't search the id-col
								{
									SelectCell(this[r,c]);
									return true;
								}
							}
						}

						// TODO: tighten exact start/end-cells
						for (r = RowCount - 1; r != selr - 1; --r) // quick and dirty wrap ->
						for (c = ColCount - 1; c != -1;       --c)
						{
							if (c != 0 && SearchResult(r,c)) // don't search the id-col
							{
								SelectCell(this[r,c]);
								return true;
							}
						}
					}

					int invalid = YataGrid.INVALID_GRID
								| YataGrid.INVALID_FROZ
								| YataGrid.INVALID_ROWS
								| YataGrid.INVALID_COLS;
					if (Propanel != null && Propanel.Visible)
						invalid |= YataGrid.INVALID_PROP;

					Invalidator(invalid);
					Update();
				}

				using (var ib = new Infobox(Infobox.Title_infor, "Search not found."))
				{
					ib.ShowDialog(this);
				}
			}
			return false;
		}

		/// <summary>
		/// Helper for <c><see cref="Search()">Search()</see></c>
		/// </summary>
		/// <param name="r">row</param>
		/// <param name="c">col</param>
		/// <returns><c>true</c> if <c><see cref="_text"/></c> matches
		/// <c><see cref="_search"/></c> - dependent on
		/// <c><see cref="_substr"/></c></returns>
		bool SearchResult(int r, int c)
		{
			return (_text = this[r,c].text.ToUpperInvariant()) == _search
				|| (_substr && _text.Contains(_search));
		}
		#endregion Search


		#region Diff
		/// <summary>
		/// Selects the next/previous diffed cell in this <c>YataGrid</c> and
		/// its sync-table if valid.
		/// </summary>
		/// <remarks>Frozen cells will be selected but they don't respect
		/// <c><see cref="YataGrid.EnsureDisplayed()">YataGrid.EnsureDisplayed()</see></c>.
		/// They get no respect ...
		/// 
		/// 
		/// Do not focus <c><see cref="YataGrid"/></c> if <c>[Ctrl]</c>
		/// is depressed.</remarks>
		internal void GotoDiff()
		{
			if (   (_f._diff1 != null || _f._diff2 != null)
				&& (_f._diff1 == this || _f._diff2 == this))
			{
				if ((ModifierKeys & Keys.Control) == Keys.None) _f.Activate();
				Select();


				YataGrid table; // the other table - can be null.

				if (this == _f._diff1) table = _f._diff2;
				else                   table = _f._diff1;

				Cell sel = getSelectedCell();
				int selr = getSelectedRow();

				ClearSelects();

				if (table != null)
					table.ClearSelects(true, true);

				int r,c;

				bool start = true;

				if ((ModifierKeys & Keys.Shift) == Keys.None) // forward goto ->
				{
					if (sel != null) { c = sel.x; selr = sel.y; }
					else
					{
						c = -1;
						if (selr == -1) selr = 0;
					}

					for (r = selr; r != RowCount; ++r)
					{
						if (start)
						{
							start = false;
							if (++c == ColCount)		// if starting on the last cell of a row
							{
								c = 0;

								if (r < RowCount - 1)	// jump to the first cell of the next row
									++r;
								else					// or to the top of the table if on the last row
									r = 0;
							}
						}
						else
							c = 0;

						for (; c != ColCount; ++c)
						{
							if ((sel = this[r,c]).diff)
							{
								SelectDiffCell(sel, table);
								return;
							}
						}
					}

					// TODO: tighten exact start/end-cells
					for (r = 0; r != selr + 1; ++r) // quick and dirty wrap ->
					for (c = 0; c != ColCount;   ++c)
					{
						if ((sel = this[r,c]).diff)
						{
							SelectDiffCell(sel, table);
							return;
						}
					}
				}
				else // backward goto ->
				{
					if (sel != null) { c = sel.x; selr = sel.y; }
					else
					{
						c = ColCount;
						if (selr == -1) selr = RowCount - 1;
					}

					for (r = selr; r != -1; --r)
					{
						if (start)
						{
							start = false;
							if (--c == -1)	// if starting on the first cell of a row
							{
								c = ColCount - 1;

								if (r > 0)	// jump to the last cell of the previous row
									--r;
								else		// or to the bottom of the table if on the first row
									r = RowCount - 1;
							}
						}
						else
							c = ColCount - 1;

						for (; c != -1; --c)
						{
							if ((sel = this[r,c]).diff)
							{
								SelectDiffCell(sel, table);
								return;
							}
						}
					}

					// TODO: tighten exact start/end-cells
					for (r = RowCount - 1; r != selr - 1; --r) // quick and dirty wrap ->
					for (c = ColCount - 1; c != -1;       --c)
					{
						if ((sel = this[r,c]).diff)
						{
							SelectDiffCell(sel, table);
							return;
						}
					}
				}
			}
			_f._fdiffer.EnableGotoButton(false);
		}

		/// <summary>
		/// Helper for <c><see cref="GotoDiff()">GotoDiff()</see></c>.
		/// </summary>
		/// <param name="sel">a <c><see cref="Cell"/></c> in the active
		/// <c><see cref="YataGrid"/></c></param>
		/// <param name="table">a sync'd <c>YataGrid</c> - can be <c>null</c></param>
		void SelectDiffCell(Cell sel, YataGrid table)
		{
			SelectCell(sel, false);

			if (table != null
				&& sel.y < table.RowCount
				&& sel.x < table.ColCount)
			{
				table[sel.y, sel.x].selected = true;
			}
		}
		#endregion Diff
	}
}
