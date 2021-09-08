using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
//using System.Threading;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Loads, formats, and handles 2da-data as a table or grid on YataForm's
	/// tab-pages.
	/// </summary>
	sealed partial class YataGrid
		: Control
	{
		/// <summary>
		/// Do not implement <c>IDisposable</c> here.
		/// </summary>
		/// <remarks>Because I hate that 'pattern'.</remarks>
		internal void DisposeWatcher()
		{
			Watcher.Dispose();

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
		}


		#region Enums
		internal enum InfoType
		{
			INFO_NONE,	// 0
			INFO_CRAFT,	// 1
			INFO_SPELL,	// 2
			INFO_FEAT	// 3
		}
		#endregion Enums


		#region Fields (static)
		internal const int SORT_DES = -1;
		internal const int SORT_ASC =  1;

		internal static int WidthRowhead;
		internal static int HeightColhead;

		internal static int HeightRow;

		/// <summary>
		/// Is used only for painting in all the various <c>Paint</c> events.
		/// </summary>
		internal static Graphics graphics;

		const int _padHori        =  6; // horizontal text padding in the table
		const int _padVert        =  4; // vertical text padding in the table and col/rowheads
		const int _padHoriRowhead =  8; // horizontal text padding for the rowheads
		const int _padHoriSort    = 12; // additional horizontal text padding to the right in the colheads for the sort-arrow

		const int _offsetHoriSort = 23; // horizontal offset for the sort-arrow
		const int _offsetVertSort = 15; // vertical offset for the sort-arrow

		/// <summary>
		/// Min width of a cell - ergo of a colhead even if width of
		/// colhead-text is narrower.
		/// </summary>
		internal static int _wId;

		/// <summary>
		/// Width of "****" in pixels.
		/// </summary>
		static int _wStars;

		internal const int FreezeId     = 1; // qty of Cols that are frozen ->
		internal const int FreezeFirst  = 2;
		internal const int FreezeSecond = 3;

		/// <summary>
		/// A flag that stops presumptive .NET events from firing ~50 billion.
		/// </summary>
		internal static bool _init;

		/// <summary>
		/// Another flag that stops presumptive .NET events from firing ~50
		/// billion.
		///
		/// The table scrolls 1 pixel left if refreshing a table when scroll is
		/// far right. OBSOLETE: This funct checks if the table is scrolled far
		/// right and sets and additional 1 pixel offset for InitScroll() to
		/// consider during row insertions and deletions or just
		/// fullrow-changes.
		///
		/// It seems to happen only soon after a table is loaded - then it goes
		/// away. nice ...
		///
		/// The problem appears to be that YataForm.SetTabSize() causes not one
		/// but two Resize events; the first event calculates that the client-
		/// width is 1px greater than it actually is. So I'm going to let the
		/// Snuggle-to-Max routine (horizontal) do its work and bypass the 2nd
		/// Resize event. Note the irony that when setting the TabSize in this
		/// application the form does not need to be resized at all. No thanks
		/// for wasting my day.
		/// </summary>
		internal static bool BypassInitScroll;
		#endregion Fields (static)


		#region Fields
		internal readonly YataForm _f;
		YataGrid _table; // for cycling through all tables

		internal int ColCount;
		internal int RowCount;

		readonly List<string[]> _rows = new List<string[]>();

		internal readonly List<Col> Cols = new List<Col>();
		internal readonly List<Row> Rows = new List<Row>();

		internal readonly VScrollBar _scrollVert = new VScrollBar();
		internal readonly HScrollBar _scrollHori = new HScrollBar();

		int MaxVert; // Since a .NET scrollbar's Maximum value is not
		int MaxHori; // its maximum value calculate and store these. sic

		internal bool _visVert; // Be happy. happy happy
		internal bool _visHori;

		internal int offsetVert; // TODO: these are redundant w/ the scrollbars' Value ->
		internal int offsetHori;

		int HeightTable;
		int WidthTable;

		YataPanelCols _panelCols;
		YataPanelRows _panelRows;

		internal YataPanelFrozen FrozenPanel;

		Label _labelid     = new Label();
		Label _labelfirst  = new Label();
		Label _labelsecond = new Label();

		/// <summary>
		/// The text-edit box. Note there is only one (1) <c>TextBox</c> that
		/// floats to wherever it's required.
		/// </summary>
		internal readonly TextBox _editor = new TextBox(); // TODO: static

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
		internal int _sortcol;

		/// <summary>
		/// The current sort direction. Default is sorted ascending.
		/// </summary>
		internal int _sortdir = SORT_ASC;

		internal PropertyPanel Propanel;

		internal UndoRedo _ur;

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

		internal string[] Fields // 'Fields' does NOT contain #0 col IDs (so that typically needs +/-1)
		{ get; private set; }

		internal Cell this[int r, int c]
		{
			get { return Rows[r][c]; }
			set { Rows[r][c] = value; }
		}

		internal InfoType Info
		{ get; set; }

		int _frozenCount = FreezeId; // initialized w/ id-col only.
		internal int FrozenCount
		{
			get { return _frozenCount; }
			set
			{
				_frozenCount = value;

				Col col;

				int w = 0;
				for (int c = 0; c != _frozenCount; ++c)
				{
					(col = Cols[c]).selected = false; // clear any selected frozen cols
					w += col.width();
				}
				FrozenPanel.Width = w;

				_labelfirst .Visible = (_frozenCount > FreezeId);
				_labelsecond.Visible = (_frozenCount > FreezeFirst);

				int invalid = INVALID_FROZ;

				Cell sel = getSelectedCell();
				if (sel != null) // if only one cell is selected shift selection out of frozen cols ->
				{
					if (sel.x < _frozenCount)
					{
						sel.selected =
						_editor.Visible = false;

						if (ColCount > _frozenCount)
						{
							(sel = _anchorcell = this[sel.y, _frozenCount]).selected = true;

							EnsureDisplayed(sel);
							invalid |= INVALID_GRID;
						}
					}
				}
				else // clear any selected cells that have become frozen unless the row is selected or subselected ->
				{
					int selr = getSelectedRow();

					for (int r = 0; r != RowCount; ++r)
					{
						if (   (r > selr && r > selr + RangeSelect)
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

				if (Propanel != null && Propanel.Visible) // update bg-color of PP fields
					invalid |= INVALID_PROP;

				Invalidator(invalid);
			}
		}

		/// <summary>
		/// The quantity of rows that are flagged for an operation excluding the
		/// currently selected row.
		/// </summary>
		/// <remarks>The value will be negative if the range of subselected rows
		/// is above the currently selected row - else positive.</remarks>
		internal int RangeSelect
		{ get; set; }

		internal FileWatcher Watcher
		{ get; private set; }
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor. Instantiates a 2da-file in this YataGrid format/layout.
		/// </summary>
		/// <param name="f">parent</param>
		/// <param name="pfe">path_file_extension</param>
		/// <param name="read">readonly</param>
		internal YataGrid(YataForm f, string pfe, bool read)
		{
//			DrawingControl.SetDoubleBuffered(this);
			SetStyle(ControlStyles.OptimizedDoubleBuffer
				   | ControlStyles.AllPaintingInWmPaint
				   | ControlStyles.UserPaint
				   | ControlStyles.ResizeRedraw, true);

			_f = f;

			Fullpath = pfe;
			Readonly = read;
			_init = true;

			Dock = DockStyle.Fill;
			BackColor = SystemColors.ControlDark;

			_scrollVert.Dock = DockStyle.Right;
			_scrollVert.ValueChanged += OnScrollValueChanged_vert;

			_scrollHori.Dock = DockStyle.Bottom;
			_scrollHori.ValueChanged += OnScrollValueChanged_hori;

			Controls.Add(_scrollHori);
			Controls.Add(_scrollVert);

			_editor.Visible     = false;
			_editor.BackColor   = Colors.Editor;
			_editor.BorderStyle = BorderStyle.None;
			_editor.WordWrap    = false;
			_editor.Margin      = new Padding(0);
//			_editor.Height      = cf. the PropertyPanel editor
			_editor.KeyDown    += keydown_Editor;
			_editor.LostFocus  += lostfocus_Editor;

			Controls.Add(_editor);

			AllowDrop = true;

			_ur = new UndoRedo(this);

			Watcher = new FileWatcher(this);
		}
		#endregion cTor


		#region Invalidate
		internal const int INVALID_NONE = 0x00;
		internal const int INVALID_GRID = 0x01; // this table
		internal const int INVALID_PROP = 0x02; // the PropertyPanel
		internal const int INVALID_FROZ = 0x04; // the frozen panel (id,1st,2nd)
		internal const int INVALID_ROWS = 0x08; // the rowhead panel
		internal const int INVALID_COLS = 0x10; // the colhead panel
		internal const int INVALID_LBLS = 0x20; // the static labels that head the frozen panel

		/// <summary>
		/// Invalidates various controls of this grid for UI-update based on
		/// the <c><see cref="INVALID_NONE">INVALID</see></c> flags.
		/// </summary>
		/// <param name="invalid"></param>
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


		#region Scrollbars
		/// <summary>
		/// Hides the RMB-contexts for (1) rows, (2) cells, and/or (3) tabs.
		/// </summary>
		void hideContexts()
		{
			if      (_f.ContextRow .Visible) _f.ContextRow .Visible = false;
			else if (_f.ContextCell.Visible) _f.ContextCell.Visible = false;
			else if (_f.ContextTab .Visible) _f.ContextTab .Visible = false;
		}

		/// <summary>
		/// Scrolls the table vertically.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnScrollValueChanged_vert(object sender, EventArgs e)
		{
			hideContexts();

			if (_table == null) _table = this;

			if (_table == YataForm.Table)
			{
				_table._editor.Visible = false;
				_table.Invalidator(INVALID_GRID | INVALID_FROZ | INVALID_ROWS);
			}

			_table.offsetVert = _table._scrollVert.Value;

			if (!_f._isEnterkeyedSearch								// <- if not Search by [Enter]
				&& (!Settings._instantgoto || !_f.tb_Goto.Focused))	// <- if not "instantgoto=true" when gotobox has focus
			{
				Select(); // <- workaround: refocus the table when the bar is moved by mousedrag (bar has to move > 0px)
			}

			Point loc = PointToClient(Cursor.Position);
			var args = new MouseEventArgs(MouseButtons.Left, 1, loc.X, loc.Y, 0); // clicks,x,y,delta
			OnMouseMove(args); // update coords on the Statusbar

			if (_table == YataForm.Table
				&& _f._diff1 != null && _f._diff2 != null
				&& (_f._diff1 == _table || _f._diff2 == _table))
			{
				SyncDiffedGrids();
			}
		}

		/// <summary>
		/// Scrolls the table horizontally.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnScrollValueChanged_hori(object sender, EventArgs e)
		{
			hideContexts();

			if (_table == null) _table = this;

			if (_table == YataForm.Table)
			{
				_table._editor.Visible = false;
				_table.Invalidator(INVALID_GRID | INVALID_COLS);
			}

			_table.offsetHori = _table._scrollHori.Value;

			if (!_f._isEnterkeyedSearch								// <- if not Search by [Enter]
				&& (!Settings._instantgoto || !_f.tb_Goto.Focused))	// <- if not "instantgoto=true" when gotobox has focus
			{
				Select(); // <- workaround: refocus the table when the bar is moved by mousedrag (bar has to move > 0px)
			}

			Point loc = PointToClient(Cursor.Position);
			var args = new MouseEventArgs(MouseButtons.Left, 1, loc.X, loc.Y, 0); // clicks,x,y,delta
			OnMouseMove(args); // update coords on the Statusbar

			if (_table == YataForm.Table
				&& _f._diff1 != null && _f._diff2 != null
				&& (_f._diff1 == _table || _f._diff2 == _table))
			{
				SyncDiffedGrids();
			}
		}

		/// <summary>
		/// Overrides the <c>Resize</c> handler.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>This fires whenever a fly sneezes.</remarks>
		protected override void OnResize(EventArgs e)
		{
			if (!_init)
			{
				bool doneSync = false;

				for (int tab = 0; tab != _f.Tabs.TabCount; ++tab)
				{
					_table = _f.Tabs.TabPages[tab].Tag as YataGrid;

					// BLARG. The table can be invalid when a yata-process is
					// already running and user opens more, multiple files from
					// Windows Explorer via RMB-fileopen. Note that RMB-fileopen
					// exhibits different behavior than selecting files and
					// pressing [Enter]. Note also that Windows w7 does not
					// exhibit consistent behavior here when opening multiple
					// files; all the selected files might not get sent to
					// Program.Main()->YataForm.CreatePage().

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

						if (!doneSync
							&& _f._diff1 != null && _f._diff2 != null
							&& (_f._diff1 == _table || _f._diff2 == _table))
						{
							doneSync = true;
							SyncDiffedGrids();
						}
					}
//					else // this is not reliable either ->
//					{
//						MessageBox.Show("A table failed to open. Windows failed to send a file to Yata.",
//										" burp",
//										MessageBoxButtons.OK,
//										MessageBoxIcon.Error,
//										MessageBoxDefaultButton.Button1);
//					}
				}
				_table = null;

//				if (_piColhead != null) _piColhead.Dispose();
//				_piColhead = new Bitmap(_bluePi, new Size(WidthTable, HeightColhead));

//				if (_piRowhead != null) _piRowhead.Dispose();
//				_piRowhead = new Bitmap(_bluePi, new Size(WidthRowhead, HeightTable));
			}

			if (_f.WindowState != FormWindowState.Minimized)
				_f.IsMin = false;
		}

		/// <summary>
		/// Synchs diffed tables both vertically and horizontally.
		/// </summary>
		void SyncDiffedGrids()
		{
			VScrollBar vert = null;
			HScrollBar hori = null;

			YataGrid table;
			if (_f._diff1 == _table)
			{
				table = _f._diff2;
				vert = table._scrollVert;
				hori = table._scrollHori;
			}
			else //if (_f._diff2 == _table)
			{
				table = _f._diff1;
				vert = table._scrollVert;
				hori = table._scrollHori;
			}

			if (table.MaxVert != 0)
			{
				if (_scrollVert.Value < table.MaxVert)
					vert.Value = _scrollVert.Value;
				else
					vert.Value = table.MaxVert;

				Select();
			}

			if (table.MaxHori != 0)
			{
				if (_scrollHori.Value < table.MaxHori)
					hori.Value = _scrollHori.Value;
				else
					hori.Value = table.MaxHori;

				Select();
			}
		}


		/// <summary>
		/// Initializes the vertical and horizontal scrollbars OnResize (which
		/// also happens auto after load).
		/// </summary>
		internal void InitScroll()
		{
			if (!BypassInitScroll)
			{
				HeightTable = HeightColhead + HeightRow * RowCount;

				WidthTable = WidthRowhead;
				for (int c = 0; c != ColCount; ++c)
					WidthTable += Cols[c].width();

				// NOTE: Height/Width *includes* the height/width of the relevant
				// scrollbar(s) and panel(s).

				bool visVert = HeightTable > Height;	// NOTE: Do not refactor this ->
				bool visHori = WidthTable  > Width;		// don't even ask. It works as-is. Be happy. Be very happy.

				_visVert = false; // again don't ask. Be happy.
				_visHori = false;

				_scrollVert.Visible =
				_scrollHori.Visible = false;

				if (visVert && visHori)
				{
					_visVert =
					_visHori = true;
					_scrollVert.Visible =
					_scrollHori.Visible = true;
				}
				else if (visVert)
				{
					_visVert = true;
					_visHori = (WidthTable > Width - _scrollVert.Width);
					_scrollVert.Visible = true;
					_scrollHori.Visible = _visHori;
				}
				else if (visHori)
				{
					_visVert = (HeightTable > Height - _scrollHori.Height);
					_visHori = true;
					_scrollVert.Visible = _visVert;
					_scrollHori.Visible = true;
				}

				if (_visVert)
				{
					// NOTE: Do not set Maximum until after deciding whether
					// or not max < 0. 'Cause it fucks everything up. bingo.
					int vert = HeightTable - Height
							 + (_scrollVert.LargeChange - 1)
							 + (_visHori ? _scrollHori.Height : 0);

					if (vert < _scrollVert.LargeChange)
						_scrollVert.Maximum = MaxVert = 0; // TODO: Perhaps that should zero the Value and recurse.
					else
					{
						MaxVert = (_scrollVert.Maximum = vert) - (_scrollVert.LargeChange - 1);

						// handle .NET OnResize anomaly ->
						// keep the bottom of the table snuggled against the bottom
						// of the visible area when resize enlarges the area
						if (HeightTable < Height + offsetVert - (_visHori ? _scrollHori.Height : 0))
							_scrollVert.Value = MaxVert;
					}
				}
				else
					_scrollVert.Value = _scrollVert.Maximum = MaxVert = 0;

				if (_visHori)
				{
					// NOTE: Do not set Maximum until after deciding whether
					// or not max < 0. 'Cause it fucks everything up. bingo.
					int hori = WidthTable - Width
							 + (_scrollHori.LargeChange - 1)
							 + (_visVert ? _scrollVert.Width : 0);

					if (hori < _scrollHori.LargeChange)
					{
						_scrollHori.Maximum = MaxHori = 0; // TODO: Perhaps that should zero the Value and recurse.
					}
					else
					{
						MaxHori = (_scrollHori.Maximum = hori) - (_scrollHori.LargeChange - 1);

						// handle .NET OnResize anomaly ->
						// keep the right of the table snuggled against the right of
						// the visible area when resize enlarges the area
						if (WidthTable < Width + offsetHori - (_visVert ? _scrollVert.Width : 0))
							_scrollHori.Value = MaxHori;
					}
				}
				else
					_scrollHori.Value = _scrollHori.Maximum = MaxHori = 0;
			}
		}

		/// <summary>
		/// Scrolls the table by the mousewheel.
		/// @note Fired from the form's OnMouseWheel event to catch all
		/// unhandled MouseWheel events hovered on the app (without firing
		/// twice).
		/// </summary>
		/// <param name="e"></param>
		internal void Scroll(MouseEventArgs e)
		{
			if ((ModifierKeys & Keys.Alt) == 0)
			{
				if (Propanel != null && Propanel._scroll.Visible
					&& e.X > Propanel.Left && e.X < Propanel.Left + Propanel.Width)
				{
					Propanel.Scroll(e);
				}
				else if (!_editor.Visible)
				{
					if (_visVert && (!_visHori || (ModifierKeys & Keys.Control) == 0))
					{
						int h;
						if ((ModifierKeys & Keys.Shift) != 0) // shift grid vertically 1 visible-height per delta
						{
							h = Height - HeightColhead - (_visHori ? _scrollHori.Height : 0);
						}
						else
							h = _scrollVert.LargeChange;

						if (e.Delta > 0)
						{
							if (_scrollVert.Value - h < 0)
								_scrollVert.Value = 0;
							else
								_scrollVert.Value -= h;
						}
						else if (e.Delta < 0)
						{
							if (_scrollVert.Value + h > MaxVert)
								_scrollVert.Value = MaxVert;
							else
								_scrollVert.Value += h;
						}
					}
					else if (_visHori)
					{
						int w;
						if ((ModifierKeys & Keys.Shift) != 0) // shift grid horizontally 1 visible-width per delta
						{
							w = Width - getLeft() - (_visVert ? _scrollVert.Width : 0);
						}
						else
							w = _scrollHori.LargeChange;

						if (e.Delta > 0)
						{
							if (_scrollHori.Value - w < 0)
								_scrollHori.Value = 0;
							else
								_scrollHori.Value -= w;
						}
						else if (e.Delta < 0)
						{
							if (_scrollHori.Value + w > MaxHori)
								_scrollHori.Value = MaxHori;
							else
								_scrollHori.Value += w;
						}
					}
				}
			}
		}
		#endregion Scrollbars


		#region Load
//		const int LINE_HEADER   = 0;
		const int LINE_VALTYPE  = 1;
		const int LINE_COLHEADS = 2;

		internal const int LOADRESULT_FALSE   = 0;
		internal const int LOADRESULT_TRUE    = 1;
		internal const int LOADRESULT_CHANGED = 2;

		static int CodePage = -1;

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
		/// Tries to load a 2da-file.
		/// </summary>
		/// <returns>a LOADRESULT_* val</returns>
		internal int LoadTable()
		{
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

			string[] lines = File.ReadAllLines(Fullpath); // default encoding is UTF-8

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
						else // silently fail.
						{
							_init = false;
							return LOADRESULT_FALSE;
						}
					}
					break;
				}
			}


			bool ignoreErrors = false;

			int id = -1;

			string line = String.Empty;

			int total = lines.Length;
			if (total < LINE_COLHEADS + 1) total = LINE_COLHEADS + 1;

			for (int i = 0; i != total; ++i)
			{
				if (i < lines.Length)
					line = lines[i].Trim();
				else
					line = String.Empty;

				if (i > LINE_COLHEADS)
				{
					string[] fields = ParseTableRow(line);
					if (fields.Length != 0) // allow blank lines on load - they will be removed if/when file is saved.
					{
						++id; // test for well-formed, consistent IDs

						if (!ignoreErrors)
						{
							string info = String.Empty;

							int result;
							if (!Int32.TryParse(fields[0], out result))
								info = "The 2da-file contains an ID that is not an integer.";
							else if (result != id)
								info = "The 2da-file contains an ID that is out of order.";

							if (!String.IsNullOrEmpty(info))
							{
								string error = info
											 + Environment.NewLine + Environment.NewLine
											 + Fullpath
											 + Environment.NewLine + Environment.NewLine
											 + id + " / " + fields[0];
								switch (ShowLoadError(error))
								{
									case DialogResult.Abort:
										_init = false;
										return LOADRESULT_FALSE;
									case DialogResult.Retry:
										break;
									case DialogResult.Ignore:
										ignoreErrors = true;
										break;
								}
							}
						}

						// test for matching fields under columns
						if (!ignoreErrors && fields.Length != Fields.Length + 1)
						{
							string error = "The 2da-file contains fields that do not align with its cols."
										 + Environment.NewLine + Environment.NewLine
										 + Fullpath
										 + Environment.NewLine + Environment.NewLine
										 + "id " + id;
							switch (ShowLoadError(error))
							{
								case DialogResult.Abort:
									_init = false;
									return LOADRESULT_FALSE;
								case DialogResult.Retry:
									break;
								case DialogResult.Ignore:
									ignoreErrors = true;
									break;
							}
						}

						// test for an odd quantity of double-quote characters
						if (!ignoreErrors)
						{
							int quotes = 0;
							foreach (char character in line)
							{
								if (character == '"')
									++quotes;
							}

							if (quotes % 2 == 1)
							{
								string error = "A row contains an odd quantity of double-quote characters."
											 + Environment.NewLine + Environment.NewLine
											 + Fullpath
											 + Environment.NewLine + Environment.NewLine
											 + "id " + id;
								switch (ShowLoadError(error))
								{
									case DialogResult.Abort:
										_init = false;
										return LOADRESULT_FALSE;
									case DialogResult.Retry:
										break;
									case DialogResult.Ignore:
										ignoreErrors = true;
										break;
								}
							}
						}

						// NOTE: Tests for well-formed fields will be done later so that their
						// respective cells can be flagged as 'loadchanged' (if applicable).

						_rows.Add(fields);
					}
				}
				else if (i == LINE_COLHEADS)
				{
					if (String.IsNullOrEmpty(line))
					{
						MessageBox.Show("The 2da-file does not have any fields."
										+ Environment.NewLine
										+ "Yata requires that a file has at least one field on its 3rd line.",
										" burp",
										MessageBoxButtons.OK,
										MessageBoxIcon.Error,
										MessageBoxDefaultButton.Button1);
						_init = false;
						return LOADRESULT_FALSE;
					}

					if (!ignoreErrors)
					{
						foreach (char character in line)
						{
							if (character == '"' // <- always bork on a double-quote
								|| (Settings._strict
									&& !char.IsWhiteSpace(character)
									&& !char.IsLetterOrDigit(character)
									&& character != '_'))
							{
								string error = "Column headers should contain only alpha-numeric characters and underscores."
											 + Environment.NewLine + Environment.NewLine
											 + Fullpath;
								switch (ShowLoadError(error))
								{
									case DialogResult.Abort:
										_init = false;
										return LOADRESULT_FALSE;
									case DialogResult.Retry:
										break;
									case DialogResult.Ignore:
										ignoreErrors = true;
										break;
								}
								break;
							}
						}
					}
					Fields = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
				}
				else if (i == LINE_VALTYPE)
				{
					if (!ignoreErrors && Settings._strict && !String.IsNullOrEmpty(line)) // test for blank 2nd line
					{
						string error = "The 2nd line in the 2da should be blank."
									 + Environment.NewLine
									 + "This editor does not support default value-types."
									 + Environment.NewLine + Environment.NewLine
									 + Fullpath;
						switch (ShowLoadError(error))
						{
							case DialogResult.Abort:
								_init = false;
								return LOADRESULT_FALSE;
							case DialogResult.Retry:
								break;
							case DialogResult.Ignore:
								ignoreErrors = true;
								break;
						}
					}
				}
				else //if (i == LINE_HEADER) // test version header
				{
					if (String.IsNullOrEmpty(line))
					{
						MessageBox.Show("The 2da-file does not have a header on its first line."
										+ Environment.NewLine + Environment.NewLine
										+ "2DA V2.0",
										" burp",
										MessageBoxButtons.OK,
										MessageBoxIcon.Error,
										MessageBoxDefaultButton.Button1);
						_init = false;
						return LOADRESULT_FALSE;
					}

					if (line != "2DA V2.0" && (Settings._strict || line != "2DA\tV2.0"))
					{
						string error = "The 2da-file contains an incorrect version header."
									 + Environment.NewLine
									 + "It will be replaced by the standard header if the file is saved."
									 + Environment.NewLine + Environment.NewLine
									 + Fullpath;
						switch (ShowLoadError(error))
						{
							case DialogResult.Abort:
								_init = false;
								return LOADRESULT_FALSE;
							case DialogResult.Retry:
								break;
							case DialogResult.Ignore:
								ignoreErrors = true;
								break;
						}
					}
				}
			}

			if (_rows.Count == 0) // add a row of stars so grid is not left blank ->
			{
				var cells = new string[Fields.Length + 1]; // NOTE: 'Fields' does not contain the ID-col.
				for (int c = 0; c <= Fields.Length; ++c)
					cells[c] = gs.Stars;

				_rows.Add(cells);
				return LOADRESULT_CHANGED; // flag the Table as changed
			}

			return LOADRESULT_TRUE;
		}

		/// <summary>
		/// A generic error-box if something goes wrong while loading a 2da file.
		/// </summary>
		/// <param name="error"></param>
		static DialogResult ShowLoadError(string error)
		{
			error += Environment.NewLine + Environment.NewLine
				   + "abort\t- stop loading"         + Environment.NewLine
				   + "retry\t- check for next error" + Environment.NewLine
				   + "ignore\t- just load the file";

			return MessageBox.Show(error,
								   " burp",
								   MessageBoxButtons.AbortRetryIgnore,
								   MessageBoxIcon.Exclamation,
								   MessageBoxDefaultButton.Button2);
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
		#endregion Load


		#region Init
		/// <summary>
		/// Initializes a created or reloaded table.
		/// </summary>
		/// <param name="changed"></param>
		/// <param name="reload"></param>
		internal void Init(bool changed, bool reload = false)
		{
			if (reload)
			{
				_init = true;
				_editor.Visible = false;

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
				switch (Path.GetFileNameWithoutExtension(Fullpath).ToLower())
				{
					case "crafting":
						Info = InfoType.INFO_CRAFT;
						goto case "";

					case "spells":
						Info = InfoType.INFO_SPELL;
						goto case "";

					case "feat":
						Info = InfoType.INFO_FEAT;
						goto case "";

					case "":									// NOTE: YataForm.CreatePage() does not allow a blank
						foreach (var dir in Settings._pathall)	// filename to load so this should be (reasonably) safe.
							_f.GropeLabels(dir);
						break;
				}
			}

			Changed = changed;

			_panelCols = new YataPanelCols(this);
			_panelRows = new YataPanelRows(this);

			CreateCols();
			CreateRows();

			FrozenPanel = new YataPanelFrozen(this, Cols[0].width());
			initFrozenLabels();

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
		/// Lays out this <c>YataGrid</c> per
		/// <c><see cref="YataForm.doFont()">YataForm.doFont()</see></c> or
		/// <c><see cref="YataForm"/>.AutosizeCols()</c> or when row(s) are
		/// inserted, deleted, or cleared.
		/// </summary>
		/// <param name="r">first row to consider as changed (<c>-1</c> if
		/// deleting rows)</param>
		/// <param name="range">range of rows to consider as changed (<c>0</c>
		/// for single row)</param>
		internal void Calibrate(int r = -1, int range = 0)
		{
			_init = true;
			_editor.Visible = false;

			for (int c = 0; c != ColCount; ++c)
				Colwidth(c,r, range);

			FrozenCount = FrozenCount; // refresh the Frozen panel

			metricStaticHeads(_f);


			_scrollVert.LargeChange =
			_scrollHori.LargeChange = HeightRow;
			InitScroll();

			Select();
			_init = false;
		}

		/// <summary>
		/// Creates the cols and caches the 2da's colhead data.
		/// </summary>
		/// <param name="calibrate">true to only adjust (ie. Font changed)</param>
		internal void CreateCols(bool calibrate = false)
		{
			int c = 0;
			if (!calibrate)
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

				if (!calibrate)
					Cols[c].text = head;

				widthtext = YataGraphics.MeasureWidth(head, _f.FontAccent);
				Cols[c]._widthtext = widthtext;

				Cols[c].width(widthtext + _padHori * 2 + _padHoriSort, calibrate);
			}
		}

		/// <summary>
		/// Creates a col.
		/// </summary>
		/// <param name="selc"></param>
		internal void CreateCol(int selc)
		{
			--selc; // the Field-count is 1 less than the col-count

			int fieldsLength = Fields.Length + 1; // create a new Fields array ->
			var fields = new string[fieldsLength];
			for (int i = 0; i != fieldsLength; ++i)
			{
				if (i < selc)
				{
					fields[i] = Fields[i];
				}
				else if (i == selc)
				{
					fields[i] = InputDialogColhead._text;

					var col = new Col();
					col.text = InputDialogColhead._text;
					col._widthtext = YataGraphics.MeasureWidth(col.text, _f.FontAccent);
					col.width(col._widthtext + _padHori * 2 + _padHoriSort);
					col.selected = true;

					Cols.Insert(i + 1, col);
					++ColCount;

					for (int r = 0; r != RowCount; ++r)
					{
						var cells = new Cell[ColCount]; // create a new Cells array in each row ->
						for (int c = 0; c != ColCount; ++c)
						{
							if (c < selc + 1)
							{
								cells[c] = this[r,c];
							}
							else if (c == selc + 1)
							{
								cells[c] = new Cell(r,c, gs.Stars);
								cells[c].selected = true;
								cells[c]._widthtext = _wStars;
							}
							else // (c > selc + 1)
							{
								cells[c] = this[r, c - 1];
								cells[c].x += 1;
							}
						}
						Rows[r]._cells = cells;
						Rows[r].Length += 1;
					}
				}
				else // (i > selc)
				{
					fields[i] = Fields[i - 1];
				}
			}
			Fields = fields;

			int w = _wStars + _padHori * 2;
			if (w > Cols[++selc].width()) Cols[selc].width(w);

			InitScroll();
			EnsureDisplayedCol(selc);

			_f.EnableCelleditOperations();

			if (!Changed) Changed = true;
		}

		/// <summary>
		/// Deletes a col.
		/// </summary>
		/// <param name="selc"></param>
		internal void DeleteCol(int selc)
		{
			--selc; // the Field-count is 1 less than the col-count

			int fieldsLength = Fields.Length - 1; // create a new Fields array ->
			var fields = new string[fieldsLength];
			for (int i = 0; i != fieldsLength; ++i)
			{
				if (i < selc) fields[i] = Fields[i];
				else          fields[i] = Fields[i + 1];
			}
			Fields = fields;

			++selc; // revert to col-id

			for (int r = 0; r != RowCount; ++r)
			{
				var cells = new Cell[ColCount - 1]; // create a new Cells array in each row ->
				for (int c = 0; c != ColCount - 1; ++c)
				{
					if (c < selc)
					{
						cells[c] = this[r,c];
					}
					else
					{
						cells[c] = this[r, c + 1];
						cells[c].x -= 1;
					}
				}
				Rows[r]._cells = cells;
				Rows[r].Length -= 1;
			}

			Cols.RemoveAt(selc);
			--ColCount;

			InitScroll();

			_f.EnableCelleditOperations();

			if (!Changed) Changed = true;
		}

		/// <summary>
		/// Relabels a col.
		/// </summary>
		/// <param name="selc"></param>
		internal void RelabelCol(int selc)
		{
			Fields[selc - 1] = InputDialogColhead._text; // the Field-count is 1 less than the col-count
			Cols[selc]._widthtext = YataGraphics.MeasureWidth((Cols[selc].text = InputDialogColhead._text),
															  _f.FontAccent);

			Colwidth(selc);
			InitScroll();
			EnsureDisplayedCol(selc);
			Invalidator(INVALID_COLS);

			if (!Changed) Changed = true;
		}

		/// <summary>
		/// Pastes cell-texts into a col.
		/// </summary>
		/// <param name="selc"></param>
		/// <param name="copyc"></param>
		internal void PasteCol(int selc, IList<string> copyc)
		{
			ClearSelects(true);

			bool changed = false;

			Cell cell;
			for (int r = 0; r != RowCount && r != copyc.Count; ++r)
			{
				cell = this[r, selc];
				cell.selected = true;

				if (cell.text != copyc[r])
				{
					cell.text = copyc[r];
					changed = true;
				}
			}

			Colwidth(selc, 0, RowCount - 1);
			InitScroll();
			EnsureDisplayedCol(selc);

			_f.EnableCelleditOperations();

			if (changed && !Changed) Changed = true;
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
						if (CheckLoadField(ref text))
						{
							changed =
							loadchanged = true;
						}
					}
					else
					{
						text = gs.Stars;
						changed =
						loadchanged = true;
					}

					(this[r,c] = new Cell(r,c, text)).loadchanged = loadchanged;
				}
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
				Cols[c].width(w);
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


		bool _initFrozenLabels = true;

		/// <summary>
		/// Initializes the frozen-labels on the colhead panel.
		/// </summary>
		void initFrozenLabels()
		{
			_labelid    .Visible =
			_labelfirst .Visible =
			_labelsecond.Visible = false;

			if (ColCount != 0)
			{
				_labelid.Visible = true;

				if (_initFrozenLabels) // TODO: FrozenLabels could be instantiated / updated-on-Reload better.
				{
					DrawingControl.SetDoubleBuffered(_labelid);
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
						DrawingControl.SetDoubleBuffered(_labelfirst);
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
							DrawingControl.SetDoubleBuffered(_labelsecond);
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
		/// Sets standard <c><see cref="HeightColhead"/></c>,
		/// <c><see cref="HeightRow"/></c>, and minimum cell-width
		/// <c><see cref="_wId"/></c>. Also caches width of "****" in
		/// <c><see cref="_wStars"/></c>.
		/// </summary>
		/// <param name="f"></param>
		/// <remarks>These values are the same for all loaded tables.</remarks>
		internal static void SetStaticMetrics(YataForm f)
		{
			HeightColhead = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, f.FontAccent) + _padVert * 2;
			HeightRow     = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, f.Font)       + _padVert * 2;

			_wId    = YataGraphics.MeasureWidth(gs.Id,    f.Font) + _padHoriRowhead * 2;
			_wStars = YataGraphics.MeasureWidth(gs.Stars, f.Font);
		}

		/// <summary>
		/// Calculates and maintains rowhead width wrt/ current Font across all
		/// tabs/tables.
		/// </summary>
		/// <param name="f"></param>
		internal static void metricStaticHeads(YataForm f)
		{
			YataGrid table;

			int rows = 0, rowsTest; // row-headers' width stays uniform across all tabpages

			int tabs = f.Tabs.TabCount;
			int tab = 0;
			for (; tab != tabs; ++tab) // find the table w/ most rows ->
			{
				table = f.Tabs.TabPages[tab].Tag as YataGrid;
				if ((rowsTest = table.RowCount - 1) > rows)
					rows = rowsTest;
			}

			string texttest = "9"; // determine how many nines need to be measured ->
			while ((rows /= 10) != 0)
				texttest += "9";

			WidthRowhead = YataGraphics.MeasureWidth(texttest, f.FontAccent) + _padHoriRowhead * 2;

			for (tab = 0; tab != tabs; ++tab)
			{
				table = f.Tabs.TabPages[tab].Tag as YataGrid;

				table.WidthTable = WidthRowhead;
				foreach (var col in table.Cols)
					table.WidthTable += col.width();

				table._panelRows.Width  = WidthRowhead;
				table._panelCols.Height = HeightColhead;

				metricFrozenLabels(table);
			}
		}

		/// <summary>
		/// Re-widths and re-locates the frozen labels.
		/// </summary>
		/// <param name="table"></param>
		internal static void metricFrozenLabels(YataGrid table)
		{
			if (table.ColCount != 0)
			{
				int w0 = table.Cols[0].width();
				table._labelid.Location = new Point(0,0);
				table._labelid.Size = new Size(WidthRowhead + w0, HeightColhead - 1);	// -1 so these don't cover the long
																						// horizontal line under the colhead.
				if (table.ColCount > 1)
				{
					int w1 = table.Cols[1].width();
					table._labelfirst.Location = new Point(WidthRowhead + w0, 0);
					table._labelfirst.Size = new Size(w1, HeightColhead - 1);

					if (table.ColCount > 2)
					{
						table._labelsecond.Location = new Point(WidthRowhead + w0 + w1, 0);
						table._labelsecond.Size = new Size(table.Cols[2].width(), HeightColhead - 1);
					}
				}
			}
		}

		/// <summary>
		/// Re-widths any frozen-labels and the frozen-panel if they need it.
		/// </summary>
		/// <param name="c">col-id that changed its width</param>
		internal void metricFrozenControls(int c)
		{
			if (c < FreezeSecond)
			{
				metricFrozenLabels(this); // re-width the frozen-labels on the colhead

				if (c < FrozenCount)
					FrozenCount = FrozenCount; // re-width the frozen-panel
			}
		}
		#endregion Init


		/// <summary>
		/// Disables textbox navigation etc. keys to allow table scrolling on
		/// certain key-events (iff the editbox is not focused).
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			//logfile.Log();
			//logfile.Log("YataGrid.OnPreviewKeyDown() e.KeyData= " + e.KeyData);

			switch (e.KeyCode)
			{
				case Keys.Up:
				case Keys.Down:
				case Keys.Left:
				case Keys.Right:
				case Keys.Escape:
					e.IsInputKey = true; // as opposed to 'IsDialogKey' ... I'd guess, it's really not transparent.
					break;
			}
		}

		/// <summary>
		/// Handles navigation by keyboard around this <c>YataGrid</c>. Also
		/// handles the <c>[Esc]</c> key to clear all selections.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			//logfile.Log("YataGrid.OnKeyDown() e.KeyData= " + e.KeyData);

			if ((e.Modifiers & Keys.Alt) != 0)
				return;


			bool ctr = (e.Modifiers & Keys.Control) != 0,
				 sft = (e.Modifiers & Keys.Shift)   != 0;

			int invalid  = INVALID_NONE;
			bool display = false;
			bool anchor  = false;

			Cell sel = getSelectedCell();
			int selr = getSelectedRow();

			// TODO: change selected col perhaps

			switch (e.KeyCode)
			{
				case Keys.Home:
					if (selr != -1)
					{
						if (!sft)
						{
							if (ctr)
							{
								if (selr > 0)
								{
									ClearSelects(true);
									SelectRow(0);
								}
								EnsureDisplayedRow(0);
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
							else if (_visHori) _scrollHori.Value = 0;
						}
					}
					else if (sft)
					{
						if (this != _f._diff1 && this != _f._diff2 // don't allow multi-cell select if sync'd
							&& areSelectedCellsContiguous())
						{
							if (ctr)
							{
								ClearCellSelects();

								for (int r = 0; r <= _anchorcell.y; ++r)
								for (int c = FrozenCount; c <= _anchorcell.x; ++c)
								{
									this[r,c].selected = true;
								}
								sel = this[0, FrozenCount];
							}
							else
							{
								int strt_r, stop_r;
								int sel_r = asStartStop_row(out strt_r, out stop_r);

								ClearCellSelects();

								for (int r = strt_r;      r <= stop_r;        ++r)
								for (int c = FrozenCount; c <= _anchorcell.x; ++c)
								{
									this[r,c].selected = true;
								}
								sel = this[sel_r, FrozenCount];
							}
							anchor = true;
						}
					}
					else if (ctr)
					{
						if (sel != null)
						{
							if (sel.x != FrozenCount || sel.y != 0)
							{
								sel.selected = false;
								(sel = this[0, FrozenCount]).selected = true;
							}
							display = true;
						}
						else if (_visVert) _scrollVert.Value = 0;
					}
					else if (sel != null)
					{
						if (sel.x != FrozenCount)
						{
							sel.selected = false;
							(sel = this[sel.y, FrozenCount]).selected = true;
						}
						display = true;
					}
					else if (_visHori) _scrollHori.Value = 0;
					break;

				case Keys.End:
					if (selr != -1)
					{
						if (!sft)
						{
							if (ctr)
							{
								if (selr != RowCount - 1)
								{
									ClearSelects(true);
									SelectRow(RowCount - 1);
								}
								EnsureDisplayedRow(RowCount - 1);
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
							else if (_visHori) _scrollHori.Value = MaxHori;
						}
					}
					else if (sft)
					{
						if (this != _f._diff1 && this != _f._diff2 // don't allow multi-cell select if sync'd
							&& areSelectedCellsContiguous())
						{
							if (ctr)
							{
								ClearCellSelects();

								for (int r = _anchorcell.y; r != RowCount; ++r)
								for (int c = _anchorcell.x; c != ColCount; ++c)
								{
									this[r,c].selected = true;
								}
								sel = this[RowCount - 1, ColCount - 1];
							}
							else
							{
								int strt_r, stop_r;
								int sel_r = asStartStop_row(out strt_r, out stop_r);

								ClearCellSelects();

								for (int r = strt_r;        r <= stop_r;   ++r)
								for (int c = _anchorcell.x; c != ColCount; ++c)
								{
									this[r,c].selected = true;
								}
								sel = this[sel_r, ColCount - 1];
							}
							anchor = true;
						}
					}
					else if (ctr)
					{
						if (sel != null)
						{
							if (sel.x != FrozenCount || sel.y != RowCount - 1)
							{
								sel.selected = false;
								(sel = this[RowCount - 1, FrozenCount]).selected = true;
							}
							display = true;
						}
						else if (_visVert) _scrollVert.Value = MaxVert;
					}
					else if (sel != null)
					{
						if (sel.x != ColCount - 1)
						{
							sel.selected = false;
							(sel = this[sel.y, ColCount - 1]).selected = true;
						}
						display = true;
					}
					else if (_visHori) _scrollHori.Value = MaxHori;
					break;

				case Keys.PageUp:
					if (selr != -1)
					{
						if (!ctr && !sft)
						{
							if (selr > 0)
							{
								ClearSelects(true);

								int shift = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;
								if ((selr -= shift) < 0) selr = 0;

								SelectRow(selr);
							}
							EnsureDisplayedRow(selr);
							invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
						}
					}
					else if (sft)
					{
						if (this != _f._diff1 && this != _f._diff2 // don't allow multi-cell select if sync'd
							&& areSelectedCellsContiguous())
						{
							if (ctr)
							{
								int strt_c, stop_c;
								int sel_c = asStartStop_col(out strt_c, out stop_c);

								ClearCellSelects();

								for (int r = 0;      r <= _anchorcell.y; ++r)
								for (int c = strt_c; c <= stop_c;        ++c)
								{
									this[r,c].selected = true;
								}
								sel = this[0, sel_c];
								anchor = true;
							}
							else
							{
								int sel_r = getAnchorRangedRowid();
								if (sel_r != 0)
								{
									int shift = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;
									sel_r = Math.Max(0, sel_r - shift);

									int strt_r = Math.Min(sel_r, _anchorcell.y);
									int stop_r = Math.Max(sel_r, _anchorcell.y);

									int strt_c, stop_c;
									int sel_c = asStartStop_col(out strt_c, out stop_c);

									ClearCellSelects();

									for (int r = strt_r; r <= stop_r; ++r)
									for (int c = strt_c; c <= stop_c; ++c)
									{
										this[r,c].selected = true;
									}
									sel = this[sel_r, sel_c];
									anchor = true;
								}
							}
						}
					}
//					else if (ctr)
//					{
						// don't use [Ctrl+PageUp] since it is used/consumed by the tabcontrol
						//
						// Note that can be bypassed in YataTabs.OnKeyDown() -
						// which is how [Ctrl+Shift+PageUp/Down] work for selecting contiguous blocks.
						// but I'd prefer to keep [Ctrl+PageUp/Down] for actually changing tabpages
//					}
					else if (!ctr)
					{
						if (sel != null)
						{
							if (sel.y != 0 && sel.x >= FrozenCount)
							{
								sel.selected = false;

								int shift = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;
								if ((selr = sel.y - shift) < 0) selr = 0;

								(sel = this[selr, sel.x]).selected = true;
							}
							display = true;
						}
						else if (_visVert)
						{
							int h = Height - HeightColhead - (_visHori ? _scrollHori.Height : 0);
							if (_scrollVert.Value - h < 0)
								_scrollVert.Value = 0;
							else
								_scrollVert.Value -= h;
						}
					}
					break;

				case Keys.PageDown:
					if (selr != -1)
					{
						if (!ctr && !sft)
						{
							if (selr != RowCount - 1)
							{
								ClearSelects(true);

								int shift = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;
								if ((selr += shift) > RowCount - 1) selr = RowCount - 1;

								SelectRow(selr);
							}
							EnsureDisplayedRow(selr);
							invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
						}
					}
					else if (sft)
					{
						if (this != _f._diff1 && this != _f._diff2 // don't allow multi-cell select if sync'd
							&& areSelectedCellsContiguous())
						{
							if (ctr)
							{
								int strt_c, stop_c;
								int sel_c = asStartStop_col(out strt_c, out stop_c);

								ClearCellSelects();

								for (int r = _anchorcell.y; r != RowCount; ++r)
								for (int c = strt_c;        c <= stop_c;   ++c)
								{
									this[r,c].selected = true;
								}
								sel = this[RowCount - 1, sel_c];
								anchor = true;
							}
							else
							{
								int sel_r = getAnchorRangedRowid();
								if (sel_r != RowCount - 1)
								{
									int shift = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;
									sel_r = Math.Min(RowCount - 1, sel_r + shift);

									int strt_r = Math.Min(sel_r, _anchorcell.y);
									int stop_r = Math.Max(sel_r, _anchorcell.y);

									int strt_c, stop_c;
									int sel_c = asStartStop_col(out strt_c, out stop_c);

									ClearCellSelects();

									for (int r = strt_r; r <= stop_r; ++r)
									for (int c = strt_c; c <= stop_c; ++c)
									{
										this[r,c].selected = true;
									}
									sel = this[sel_r, sel_c];
									anchor = true;
								}
							}
						}
					}
//					else if (ctr)
//					{
						// don't use [Ctrl+PageDown] since it is used/consumed by the tabcontrol
						//
						// Note that can be bypassed in YataTabs.OnKeyDown() -
						// which is how [Ctrl+Shift+PageUp/Down] work for selecting contiguous blocks.
						// but I'd prefer to keep [Ctrl+PageUp/Down] for actually changing tabpages
//					}
					else if (!ctr)
					{
						if (sel != null)
						{
							if (sel.y != RowCount - 1 && sel.x >= FrozenCount)
							{
								sel.selected = false;

								int shift = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;
								if ((selr = sel.y + shift) > RowCount - 1) selr = RowCount - 1;

								(sel = this[selr, sel.x]).selected = true;
							}
							display = true;
						}
						else if (_visVert)
						{
							int h = Height - HeightColhead - (_visHori ? _scrollHori.Height : 0);
							if (_scrollVert.Value + h > MaxVert)
								_scrollVert.Value = MaxVert;
							else
								_scrollVert.Value += h;
						}
					}
					break;

				case Keys.Up: // NOTE: needs to bypass KeyPreview
					if (selr != -1)
					{
						if (!ctr && !sft)
						{
							if (selr != 0)
							{
								ClearSelects(true);
								SelectRow(--selr);
							}
							EnsureDisplayedRow(selr);
							invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
						}
					}
					else if (sft)
					{
						if (!ctr
							&& this != _f._diff1 && this != _f._diff2 // don't allow multi-cell select if sync'd
							&& areSelectedCellsContiguous())
						{
							int sel_r = getAnchorRangedRowid() - 1;
							if (sel_r >= 0)
							{
								int strt_r = Math.Min(sel_r, _anchorcell.y);
								int stop_r = Math.Max(sel_r, _anchorcell.y);

								int strt_c, stop_c;
								int sel_c = asStartStop_col(out strt_c, out stop_c);

								ClearCellSelects();

								for (int r = strt_r; r <= stop_r; ++r)
								for (int c = strt_c; c <= stop_c; ++c)
								{
									this[r,c].selected = true;
								}
								sel = this[sel_r, sel_c];
								anchor = true;
							}
						}
					}
					else if (ctr) // selection to abovest cell
					{
						if (sel != null)
						{
							if (sel.y != 0 && sel.x >= FrozenCount)
							{
								sel.selected = false;
								(sel = this[0, sel.x]).selected = true;
							}
							display = true;
						}
					}
					else if (sel != null) // selection to the cell above
					{
						if (sel.y != 0 && sel.x >= FrozenCount)
						{
							sel.selected = false;
							(sel = this[sel.y - 1, sel.x]).selected = true;
						}
						display = true;
					}
					else if (_visVert)
					{
						if (_scrollVert.Value - _scrollVert.LargeChange < 0)
							_scrollVert.Value = 0;
						else
							_scrollVert.Value -= _scrollVert.LargeChange;
					}
					break;

				case Keys.Down: // NOTE: needs to bypass KeyPreview
					if (selr != -1)
					{
						if (!ctr && !sft)
						{
							if (selr != RowCount - 1)
							{
								ClearSelects(true);
								SelectRow(++selr);
							}
							EnsureDisplayedRow(selr);
							invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
						}
					}
					else if (sft)
					{
						if (!ctr
							&& this != _f._diff1 && this != _f._diff2 // don't allow multi-cell select if sync'd
							&& areSelectedCellsContiguous())
						{
							int sel_r = getAnchorRangedRowid() + 1;
							if (sel_r < RowCount)
							{
								int strt_r = Math.Min(sel_r, _anchorcell.y);
								int stop_r = Math.Max(sel_r, _anchorcell.y);

								int strt_c, stop_c;
								int sel_c = asStartStop_col(out strt_c, out stop_c);

								ClearCellSelects();

								for (int r = strt_r; r <= stop_r; ++r)
								for (int c = strt_c; c <= stop_c; ++c)
								{
									this[r,c].selected = true;
								}
								sel = this[sel_r, sel_c];
								anchor = true;
							}
						}
					}
					else if (ctr) // selection to belowest cell
					{
						if (sel != null)
						{
							if (sel.y != RowCount - 1 && sel.x >= FrozenCount)
							{
								sel.selected = false;
								(sel = this[RowCount - 1, sel.x]).selected = true;
							}
							display = true;
						}
					}
					else if (sel != null) // selection to the cell below
					{
						if (sel.y != RowCount - 1 && sel.x >= FrozenCount)
						{
							sel.selected = false;
							(sel = this[sel.y + 1, sel.x]).selected = true;
						}
						display = true;
					}
					else if (_visVert)
					{
						if (_scrollVert.Value + _scrollVert.LargeChange > MaxVert)
							_scrollVert.Value = MaxVert;
						else
							_scrollVert.Value += _scrollVert.LargeChange;
					}
					break;

				case Keys.Left: // NOTE: needs to bypass KeyPreview
					if (ctr) return;

					if (sft)
					{
//						if (sel != null)
//						{
//							if (sel.x > FrozenCount)
//							{
//								sel.selected = false;
//
//								int w = Width - getLeft() - (_visVert ? _scrollVert.Width : 0);
//								var pt = getColBounds(sel.x);
//								pt.X += offsetHori - w;
//
//								int c = -1, tally = 0;
//								while ((tally += Cols[++c].width()) < pt.X)
//								{}
//
//								if (++c >= sel.x)
//									c = sel.x - 1;
//
//								if (c < FrozenCount)
//									c = FrozenCount;
//
//								(sel = this[sel.y, c]).selected = true;
//							}
//							display = true;
//						}

						if (selr == -1 && areSelectedCellsContiguous())
						{
							if (this != _f._diff1 && this != _f._diff2) // don't allow multi-cell select if sync'd
							{
								int sel_c = getAnchorRangedColid() - 1;
								if (sel_c >= FrozenCount)
								{
									int strt_c = Math.Min(sel_c, _anchorcell.x);
									int stop_c = Math.Max(sel_c, _anchorcell.x);

									int strt_r, stop_r;
									int sel_r = asStartStop_row(out strt_r, out stop_r);

									ClearCellSelects();

									for (int r = strt_r; r <= stop_r; ++r)
									for (int c = strt_c; c <= stop_c; ++c)
									{
										this[r,c].selected = true;
									}
									sel = this[sel_r, sel_c];
									anchor = true;
								}
							}
						}
						else if (_visHori) // shift grid 1 page left
						{
							int w = Width - getLeft() - (_visVert ? _scrollVert.Width : 0);
							if (_scrollHori.Value - w < 0)
								_scrollHori.Value = 0;
							else
								_scrollHori.Value -= w;
						}
					}
					else if (sel != null) // selection to the cell left
					{
						if (sel.x > FrozenCount)
						{
							sel.selected = false;
							(sel = this[sel.y, sel.x - 1]).selected = true;
						}
						display = true;
					}
					else if (_visHori)
					{
						if (_scrollHori.Value - _scrollHori.LargeChange < 0)
							_scrollHori.Value = 0;
						else
							_scrollHori.Value -= _scrollHori.LargeChange;
					}
					break;

				case Keys.Right: // NOTE: needs to bypass KeyPreview
					if (ctr) return;

					if (sft)
					{
//						if (sel != null)
//						{
//							if (sel.x != ColCount - 1)
//							{
//								sel.selected = false;
//
//								int w = Width - getLeft() - (_visVert ? _scrollVert.Width : 0);
//								var pt = getColBounds(sel.x);
//								pt.X += offsetHori + w;
//
//								int c = -1, tally = 0;
//								while (++c != ColCount && (tally += Cols[c].width()) < pt.X)
//								{}
//
//								if (--c <= sel.x)
//									c = sel.x + 1;
//
//								if (c > ColCount - 1)
//									c = ColCount - 1;
//
//								(sel = this[sel.y, c]).selected = true;
//							}
//							display = true;
//						}

						if (selr == -1 && areSelectedCellsContiguous())
						{
							if (this != _f._diff1 && this != _f._diff2) // don't allow multi-cell select if sync'd
							{
								int sel_c = getAnchorRangedColid() + 1;
								if (sel_c < ColCount)
								{
									int strt_c = Math.Min(sel_c, _anchorcell.x);
									int stop_c = Math.Max(sel_c, _anchorcell.x);

									int strt_r, stop_r;
									int sel_r = asStartStop_row(out strt_r, out stop_r);

									ClearCellSelects();

									for (int r = strt_r; r <= stop_r; ++r)
									for (int c = strt_c; c <= stop_c; ++c)
									{
										this[r,c].selected = true;
									}
									sel = this[sel_r, sel_c];
									anchor = true;
								}
							}
						}
						else if (_visHori) // shift grid 1 page right
						{
							int w = Width - getLeft() - (_visVert ? _scrollVert.Width : 0);
							if (_scrollHori.Value + w > MaxHori)
								_scrollHori.Value = MaxHori;
							else
								_scrollHori.Value += w;
						}
					}
					else if (sel != null) // selection to the cell right
					{
						if (sel.x != ColCount - 1)
						{
							sel.selected = false;
							(sel = this[sel.y, sel.x + 1]).selected = true;
						}
						display = true;
					}
					else if (_visHori)
					{
						if (_scrollHori.Value + _scrollHori.LargeChange > MaxHori)
							_scrollHori.Value = MaxHori;
						else
							_scrollHori.Value += _scrollHori.LargeChange;
					}
					break;

				case Keys.Escape: // NOTE: needs to bypass KeyPreview
					if (!ctr && !sft)
					{
						ClearSelects(true);
						_f.ClearSyncSelects();
						invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
					}
					break;
			}

			if (invalid != INVALID_NONE)	// -> is a Row operation or [Esc]
			{
				if (Propanel != null && Propanel.Visible)
					invalid |= INVALID_PROP;

				Invalidator(invalid);
			}
			else if (display)				// -> is a Cell operation
			{
				_anchorcell = sel;

				_f.SyncSelectCell(sel);

				Invalidator(INVALID_GRID
						  | INVALID_FROZ
						  | EnsureDisplayed(sel));
			}
			else if (anchor)				// -> is a block-select operation
			{
				Invalidator(INVALID_GRID
						  | EnsureDisplayed(sel));
			}

			_f.EnableCelleditOperations(); // TODO: tighten that to only if necessary.
		}

		/// <summary>
		/// Assigns the start and stop row-ids that shall be used as a range of
		/// <c><see cref="Row">Rows</see></c> that need to be selected during a
		/// horizontal contiguous block selection.
		/// </summary>
		/// <param name="strt">ref that holds the start row-id</param>
		/// <param name="stop">ref that holds the stop row-id</param>
		/// <returns>the row-id of a <c><see cref="Cell"/></c> that shall be
		/// displayed</returns>
		/// <remarks>Helper for <c><see cref="OnKeyDown()">OnKeyDown()</see></c>
		/// contiguous block selection (horizontal).</remarks>
		int asStartStop_row(out int strt, out int stop)
		{
			if (_f._copyvert == 1
				|| _anchorcell.y == 0
				|| !this[_anchorcell.y - 1, _anchorcell.x].selected)
			{
				strt = _anchorcell.y;
				stop = _anchorcell.y + _f._copyvert - 1;

				return stop;
			}

			strt = _anchorcell.y - _f._copyvert + 1;
			stop = _anchorcell.y;

			return strt;
		}

		/// <summary>
		/// Assigns the start and stop col-ids that shall be used as a range of
		/// <c><see cref="Col">Cols</see></c> that need to be selected during a
		/// vertical contiguous block selection.
		/// </summary>
		/// <param name="strt">ref that holds the start col-id</param>
		/// <param name="stop">ref that holds the stop col-id</param>
		/// <returns>the col-id of a <c><see cref="Cell"/></c> that shall be
		/// displayed</returns>
		/// <remarks>Helper for <c><see cref="OnKeyDown()">OnKeyDown()</see></c>
		/// contiguous block selection (vertical).</remarks>
		int asStartStop_col(out int strt, out int stop)
		{
			if (_f._copyhori == 1
				|| _anchorcell.x == FrozenCount
				|| !this[_anchorcell.y, _anchorcell.x - 1].selected)
			{
				strt = _anchorcell.x;
				stop = _anchorcell.x + _f._copyhori - 1;

				return stop;
			}

			strt = _anchorcell.x - _f._copyhori + 1;
			stop = _anchorcell.x;

			return strt;
		}

		/// <summary>
		/// Gets the col-id with selected cell that is furthest away from the
		/// current <c><see cref="_anchorcell">_anchorcell's</see></c>
		/// <c><see cref="Col"/></c>. Can return the col-id of the
		/// <c>_anchorcell</c> itself.
		/// </summary>
		/// <returns>col-id</returns>
		int getAnchorRangedColid()
		{
			int col = _anchorcell.x;

			for (int c = ColCount - 1; c >= _anchorcell.x; --c)
			if (this[_anchorcell.y, c].selected)
			{
				col = c;
				break;
			}

			if (col == _anchorcell.x)
			{
				for (int c = FrozenCount; c <= _anchorcell.x; ++c)
				if (this[_anchorcell.y, c].selected)
				{
					col = c;
					break;
				}
			}

			return col;
		}

		/// <summary>
		/// Gets the row-id with selected cell that is furthest away from the
		/// current <c><see cref="_anchorcell">_anchorcell's</see></c>
		/// <c><see cref="Row"/></c>. Can return the row-id of the
		/// <c>_anchorcell</c> itself.
		/// </summary>
		/// <returns>row-id</returns>
		int getAnchorRangedRowid()
		{
			int row = _anchorcell.y;

			for (int r = RowCount - 1; r >= _anchorcell.y; --r)
			if (this[r, _anchorcell.x].selected)
			{
				row = r;
				break;
			}

			if (row == _anchorcell.y)
			{
				for (int r = 0; r <= _anchorcell.y; ++r)
				if (this[r, _anchorcell.x].selected)
				{
					row = r;
					break;
				}
			}

			return row;
		}

		/// <summary>
		/// Selects a specified row by Id and flags its cells selected.
		/// </summary>
		/// <param name="r">row-id</param>
		/// <remarks>Check that <paramref name="r"/> doesn't over/underflow
		/// <c><see cref="RowCount"/></c> before call.</remarks>
		internal void SelectRow(int r)
		{
			Row row;

			YataGrid table = _f.ClearSyncSelects();
			if (table != null)
			{
				row = table.Rows[r];

				Row._bypassEnableRowedit = true;
				row.selected = true;
				Row._bypassEnableRowedit = false;

				for (int c = 0; c != table.ColCount; ++c)
					row[c].selected = true;
			}

			row = Rows[r];
			row.selected = true;

			for (int c = 0; c != ColCount; ++c)
				row[c].selected = true;

			_f.EnableCelleditOperations();
		}

		/// <summary>
		/// Selects a specified <c><see cref="Cell"/></c> and invalidates stuff.
		/// </summary>
		/// <param name="cell">a <c>Cell</c> to select</param>
		/// <param name="sync"><c>true</c> to sync the select between diffed
		/// tables; <c>false</c> if sync will be performed by the caller</param>
		/// <remarks>Called by <c><see cref="YataForm"/>.Search()</c> and
		/// <c>YataForm.editclick_GotoLoadchanged()</c> and
		/// <c>YataForm.gotodiff()</c>.</remarks>
		internal void SelectCell(Cell cell, bool sync = true)
		{
			if (sync) _f.SyncSelectCell(cell);

			cell.selected = true;
			Invalidator(INVALID_GRID
					  | INVALID_FROZ
					  | INVALID_ROWS
					  | EnsureDisplayed(cell));

			_f.EnableCelleditOperations();
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
		/// <c><see cref="YataForm"/>.OnKeyDown()</c> <c>[Space]</c>.</remarks>
		internal void SelectFirstCell()
		{
			Select(); // focus table

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
					_f.SyncSelectCell(sel);

					_anchorcell = sel;
				}
				else
				{
					sel = this[r,0]; // just a cell (for its row-id) to pass to EnsureDisplayed() below.
					_f.ClearSyncSelects();

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
		/// Gets the first selected <c><see cref="Cell"/></c> in the table else
		/// <c>null</c>.
		/// </summary>
		/// <returns></returns>
		internal Cell getFirstSelectedCell()
		{
			Cell sel;

			foreach (var row in Rows)
			for (int c = 0; c != ColCount; ++c)
				if ((sel = row[c]).selected)
					return sel;

			return null;
		}


		/// <summary>
		/// Handles <c>KeyDown</c> events for the <c><see cref="_editor"/></c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Works around dweeby .NET behavior if Alt is pressed while
		/// editing.</remarks>
		void keydown_Editor(object sender, KeyEventArgs e)
		{
			//logfile.Log("YataGrid.keydown_Editor() e.KeyData= " + e.KeyData);

			if (e.Alt) _editor.Visible = false;
		}

		/// <summary>
		/// Handles the <c>LostFocus</c> event for the
		/// <c><see cref="_editor"/></c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>This funct is a partial catchall for other places where the
		/// <c>_editor</c> needs to hide.</remarks>
		void lostfocus_Editor(object sender, EventArgs e)
		{
			_editor.Visible = false;
			Invalidator(INVALID_GRID);
		}


		/// <summary>
		/// Handles the <c>Leave</c> event for the grid - hides the
		/// <c><see cref="_editor"/></c> if it is visible.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Obsolete: but it doesn't fire if the tabpage changes w/
		/// <c>[Ctrl+PageUp]/[Ctrl+PageDown]</c>. Lovely /explode - can be fixed
		/// in <c><see cref="YataForm"/>.tab_SelectedIndexChanged()</c>.</remarks>
		protected override void OnLeave(EventArgs e)
		{
			if (_editor.Visible)
			{
				_editor.Visible = false;
				Invalidator(INVALID_GRID);
			}
		}

		/// <summary>
		/// Handles ending editing a cell by pressing
		/// <c>[Enter]</c> or <c>[Escape]</c>/<c>[Tab]</c>.
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			//logfile.Log("YataGrid.ProcessDialogKey() keyData= " + keyData);

			switch (keyData)
			{
				case Keys.Enter:
					if (_editor.Visible)
					{
						ApplyCellEdit();
						goto case Keys.Escape;
					}

					if (!Readonly
						&& (_editcell = getSelectedCell()) != null
						&& _editcell.x >= FrozenCount)
					{
						Celledit();
						Invalidator(INVALID_GRID);
					}
					return true;

				case Keys.Escape:
				case Keys.Tab:
					_editor.Visible = false;
					Invalidator(INVALID_GRID);
					Select();
					return true;
			}
			return base.ProcessDialogKey(keyData);
		}

		/// <summary>
		/// Applies a cell-edit via the <c><see cref="_editor"/></c>.
		/// </summary>
		void ApplyCellEdit()
		{
			if (_editor.Text != _editcell.text)
			{
				ChangeCellText(_editcell, _editor); // does a text-check

				if (Propanel != null && Propanel.Visible)
					Invalidator(INVALID_PROP);
			}
			else if (_editcell.loadchanged)
				ClearLoadchanged(_editcell);
		}

		/// <summary>
		/// Changes a cell's text, recalculates col-width, and sets up Undo/Redo.
		/// </summary>
		/// <param name="cell">a <c><see cref="Cell"/></c></param>
		/// <param name="tb">the editor's <c>TextBox</c> whose text to check for
		/// validity</param>
		/// <remarks>Performs a text-check.</remarks>
		internal void ChangeCellText(Cell cell, Control tb)
		{
			Restorable rest = UndoRedo.createCell(cell);
			if (!Changed)
			{
				Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}


			cell.loadchanged =
			cell.diff = false; // TODO: Check the differ if the celltext is identical or still different.

			if (CheckTextEdit(tb))
				MessageBox.Show("The text that was submitted has been altered.",
								" burp",
								MessageBoxButtons.OK,
								MessageBoxIcon.Exclamation,
								MessageBoxDefaultButton.Button1);

			cell.text = tb.Text;

			Colwidth(cell.x, cell.y);
			metricFrozenControls(cell.x);


			rest.postext = cell.text;
			_ur.Push(rest);
		}

		/// <summary>
		/// Changes a cell's text, recalculates col-width, and sets up Undo/Redo.
		/// </summary>
		/// <param name="cell">a <c><see cref="Cell"/></c></param>
		/// <param name="text">the text to change to</param>
		/// <remarks>Does *not* perform a text-check.</remarks>
		internal void ChangeCellText(Cell cell, string text)
		{
			// TODO: Optimize this for multiple calls/cells.

			Restorable rest = UndoRedo.createCell(cell);
			if (!Changed)
			{
				Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}


			cell.loadchanged =
			cell.diff = false; // TODO: Check the differ if the celltext is identical or still different.

			cell.text = text;

			Colwidth(cell.x, cell.y);
			metricFrozenControls(cell.x);

			int invalid = INVALID_GRID;
			if (Propanel != null && Propanel.Visible)
				invalid |= INVALID_PROP;

			Invalidator(invalid);


			rest.postext = cell.text;
			_ur.Push(rest);
		}


		/// <summary>
		/// Resets the width of col based on the cells in rows r to r + range.
		/// </summary>
		/// <param name="c">col</param>
		/// <param name="r">first row to consider as changed (default -1 if
		/// deleting rows and/or no extant text-widths have changed; ie, no
		/// text-widths need to be re-measured)</param>
		/// <param name="range">range of rows to consider as changed (default 0
		/// for single row)</param>
		internal void Colwidth(int c, int r = -1, int range = 0)
		{
			Col col = Cols[c];

			int colwidth = col._widthtext + _padHoriSort;
			int widthtext;

			if (r != -1) // re-calc '_widthtext' regardless of what happens below ->
			{
				string text;

				int rend = r + range;
				for (; r <= rend; ++r)
				{
					if ((text = this[r,c].text) == gs.Stars) // bingo.
						widthtext = _wStars;
					else
						widthtext = YataGraphics.MeasureWidth(text, Font);

					this[r,c]._widthtext = widthtext;

					if (widthtext > colwidth)
						colwidth = widthtext;
				}
			}

			if (!col.UserSized)	// ie. don't resize a col that user has adjusted. If it needs to
			{					// be forced (eg. on reload) unflag UserSized on all cols first.
				int totalwidth = col.width();

				if ((colwidth += _padHori * 2) > totalwidth)
				{
					col.width(colwidth);
				}
				else if (colwidth < totalwidth) // recalc width on the entire col
				{
					if (c == 0 || _wId > colwidth)
						colwidth = _wId;

					for (r = 0; r != RowCount; ++r)
					{
						widthtext = this[r,c]._widthtext + _padHori * 2;

						if (widthtext > colwidth)
							colwidth = widthtext;
					}
					col.width(colwidth, true);
				}

				if (range == 0 && totalwidth != colwidth)	// if range >0 let Calibrate() handle multiple
				{											// cols or at least the scrollers and do the UI-update
					InitScroll();
					Invalidator(INVALID_GRID | INVALID_COLS);
				}
			}

			if (Propanel != null && Propanel.Visible)
				Propanel.widthValcol(); // TODO: Re-calc the 'c' col only.
		}

		/// <summary>
		/// Checks the text that user submits to a cell.
		/// </summary>
		/// <param name="tb"></param>
		/// <returns><c>true</c> if text is changed/fixed/corrected</returns>
		/// <seealso cref="CheckLoadField()"><c>CheckLoadField()</c></seealso>
		internal static bool CheckTextEdit(Control tb)
		{
			bool changed = false;

			string field = tb.Text.Trim();

			if (String.IsNullOrEmpty(field))
			{
				tb.Text = gs.Stars;
				return false; // NOTE: Don't bother the user if he/she simply wants to blank a field.
			}

			if (field != tb.Text)
				changed = true;

			bool quoteFirst = field.StartsWith("\"", StringComparison.InvariantCulture);
			bool quoteLast  = field.EndsWith(  "\"", StringComparison.InvariantCulture);
			if (quoteFirst && quoteLast)
			{
				if (   field.Length < 3
					|| field.Substring(1, field.Length - 2).Trim() == String.Empty)
				{
					tb.Text = gs.Stars;
					return true;
				}
			}
			else if (( quoteFirst && !quoteLast)
				||   (!quoteFirst &&  quoteLast))
			{
				if (quoteFirst)
					field = field + "\"";
				else
					field = "\"" + field;

				if (field.Length == 2)
				{
					tb.Text = gs.Stars;
					return true;
				}

				changed = true;
			}

			if (field.Length > 2) // lol but it works ->
			{
				string first = field.Substring(0, 1);
				string last  = field.Substring(field.Length - 1, 1);

				string test  = field.Substring(1, field.Length - 2);

				while (test.Contains("\""))
				{
					changed = true;
					test = test.Remove(test.IndexOf('"'), 1);
					field = first + test + last;
				}

				if (test == gs.Stars)
				{
					tb.Text = gs.Stars;
					return true;
				}
			}

			if (!field.Contains("\""))
			{
				char[] chars = field.ToCharArray();
				for (int pos = 0; pos != chars.Length; ++pos)
				{
					if (Char.IsWhiteSpace(chars[pos]))
					{
						changed = true;
						field = "\"" + field + "\"";
						break;
					}
				}
			}

			tb.Text = field;
			return changed;
		}

		/// <summary>
		/// Checks the text in a cell.
		/// </summary>
		/// <param name="field">ref to a text-string</param>
		/// <returns><c>true</c> if text is changed/fixed/corrected</returns>
		/// <seealso cref="CheckTextEdit()"><c>CheckTextEdit()</c></seealso>
		bool CheckLoadField(ref string field)
		{
			bool changed = false;

//			string field = text.Trim(); // this ought be redundant during file-load -->
//			if (String.IsNullOrEmpty(field))
//			{
//				text = gs.Stars;
//				return true;
//			}
//			if (field != text)
//				changed = true;

			bool quoteFirst = field.StartsWith("\"", StringComparison.InvariantCulture);
			bool quoteLast  = field.EndsWith(  "\"", StringComparison.InvariantCulture);
			if (quoteFirst && quoteLast)
			{
				if (   field.Length < 3
					|| field.Substring(1, field.Length - 2).Trim() == String.Empty)
				{
					field = gs.Stars;
					return true;
				}
			}
			else if (( quoteFirst && !quoteLast)
				||   (!quoteFirst &&  quoteLast))
			{
				if (quoteFirst)
					field = field + "\"";
				else
					field = "\"" + field;

				if (field.Length == 2)
				{
					field = gs.Stars;
					return true;
				}

				changed = true;
			}

			if (field.Length > 2) // lol but it works ->
			{
				string first = field.Substring(0, 1);
				string last  = field.Substring(field.Length - 1, 1);

				string test  = field.Substring(1, field.Length - 2);

				while (test.Contains("\""))
				{
					changed = true;
					test = test.Remove(test.IndexOf('"'), 1);
					field = first + test + last;
				}

				if (test == gs.Stars)
				{
					field = gs.Stars;
					return true;
				}
			}

			if (!field.Contains("\""))
			{
				char[] chars = field.ToCharArray();
				for (int pos = 0; pos != chars.Length; ++pos)
				{
					if (Char.IsWhiteSpace(chars[pos]))
					{
						changed = true;
						field = "\"" + field + "\"";
						break;
					}
				}
			}

			return changed;
		}


		/// <summary>
		/// <c>true</c> allows
		/// <c><see cref="OnMouseDoubleClick()">OnMouseDoubleClick()</see></c>.
		/// </summary>
		bool _double;

		/// <summary>
		/// The clicked cell.
		/// </summary>
		/// <remarks>Used by
		/// <list type="bullet">
		/// <item><c><see cref="OnMouseClick()">OnMouseClick()</see></c></item>
		/// <item><c><see cref="OnMouseDoubleClick()">OnMouseDoubleClick()</see></c></item>
		/// </list></remarks>
		Cell _cell;

		/// <summary>
		/// LMB selects a cell or enables/disables the editbox. RMB opens a
		/// cell's context-menu.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks><c>YataGrid.MouseClick</c> does not fire on any of the top
		/// or left panels.</remarks>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			if ((ModifierKeys & Keys.Alt) == 0)
			{
				_double = false;

				if (e.X > WidthTable || e.Y > HeightTable) // click to the right or below the table-area
				{
					Select();

					if (ModifierKeys == Keys.None)
					{
						if (_editor.Visible) // NOTE: The editbox will never be visible here on RMB. for whatever reason ...
						{
//							if (e.Button == MouseButtons.Left) // apply edit only on LMB.
							ApplyCellEdit();
							_editor.Visible = false;
							Invalidator(INVALID_GRID);
						}

//						else if (e.Button == MouseButtons.Right)	// clear all selects - why does a right-click refuse to acknowledge that the editor is Vis
//						{											// Ie. if this codeblock is activated it will cancel the edit *and* clear all selects;
//							foreach (var col in Cols)				// the intent however is to catch the editor (above) OR clear all selects here.
//								col.selected = false;
//
//							foreach (var row in Rows)
//								row.selected = false;
//
//							ClearCellSelects();
//							Invalidator();
//						}
					}
				}
				else
				{
					bool ctr = (ModifierKeys & Keys.Control) != 0,
						 sft = (ModifierKeys & Keys.Shift)   != 0;

					switch (e.Button)
					{
						case MouseButtons.Left:
							if ((_cell = getClickedCell(e.X, e.Y)) != null) // safety.
							{
								if (_editor.Visible)
								{
									if (!ctr && !sft)
									{
										if (_cell != _editcell)
										{
											_double = true;

											Select();

											ApplyCellEdit();
											_editor.Visible = false;
											Invalidator(INVALID_GRID);
										}
										else					// NOTE: There's a clickable fringe around the editor.
											_editor.Focus();	// so just refocus the editor if the fringe is clicked
									}
									else
										Select();
								}
								else
								{
									Select();

									if (ctr) // select/deselect single cell ->
									{
										if (!sft)
										{
											if (_cell.selected = !_cell.selected)
											{
												if (_f.SyncSelectCell(_cell)) // disallow multi-cell select if sync'd
												{
													ClearSelects(true);
													_cell.selected = true;
												}
												EnsureDisplayed(_cell, (getSelectedCell() == null));	// <- bypass PropertyPanel.EnsureDisplayed() if
																										//    selectedcell is not the only selected cell
												_anchorcell = _cell;
											}
											else
												_f.ClearSyncSelects();

											int invalid = INVALID_GRID;
											if (Propanel != null && Propanel.Visible)
												invalid |= INVALID_PROP;

											Invalidator(invalid);
										}
									}
									else if (sft) // do block selection ->
									{
										if (this != _f._diff1 && this != _f._diff2	// disallow multi-cell select if sync'd
											&& areSelectedCellsContiguous()			// else do nothing if no cells are selected or selected cells are not in a contiguous block
											&& _cell != getSelectedCell())			// else do nothing if clicked cell is the only selected cell
										{
											ClearSelects(true);

											int strt_r = Math.Min(_anchorcell.y, _cell.y);
											int stop_r = Math.Max(_anchorcell.y, _cell.y);
											int strt_c = Math.Min(_anchorcell.x, _cell.x);
											int stop_c = Math.Max(_anchorcell.x, _cell.x);

											for (int r = strt_r; r <= stop_r; ++r)
											for (int c = strt_c; c <= stop_c; ++c)
												this[r,c].selected = true;

											EnsureDisplayed(_cell, true);

											int invalid = INVALID_GRID;
											if (Propanel != null && Propanel.Visible)
												invalid |= INVALID_PROP;

											Invalidator(invalid);
										}
									}
									else if (!_cell.selected || getSelectedCell() == null) // select cell if it's not selected or if it's not the only selected cell ->
									{
										_anchorcell = _cell;

										_double = true;

										ClearSelects(true);
										_cell.selected = true;
										_f.SyncSelectCell(_cell);

										Invalidator(INVALID_GRID
												  | INVALID_FROZ
												  | INVALID_ROWS
												  | EnsureDisplayed(_cell));
									}
									else if (!Readonly) // cell is already selected
									{
										_editcell = _cell;
										Celledit();
										Invalidator(INVALID_GRID);
									}
								}
							}
							else
								Select();

							break;

						case MouseButtons.Right:
							if (!ctr && !sft)
							{
								if ((_cell = getClickedCell(e.X, e.Y)) != null) // safety.
								{
									ClearSelects(true);
									_cell.selected = true;
									_f.SyncSelectCell(_cell);

									Invalidator(INVALID_GRID
											  | INVALID_FROZ
											  | INVALID_ROWS
											  | EnsureDisplayed(_cell));

									_f.ShowCellContext();
								}
								else
									Select();
							}
							break;
					}

					_f.EnableCelleditOperations(); // TODO: tighten that to only if necessary.
				}
			}
		}

		/// <summary>
		/// Because sometimes I'm a stupid and double-click to start textedit.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>If a cell is currently being edited any changes to that
		/// cell will be accepted and the cell that is double-clicked if any
		/// shall (sic) enter its edit-state.</remarks>
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (_double)
			{
				if (!_cell.selected)
					OnMouseClick(e);

				OnMouseClick(e);
			}
		}

		/// <summary>
		/// Gets the <c><see cref="Cell"/></c> clicked at x/y.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		Cell getClickedCell(int x, int y)
		{
			y += offsetVert;
			if (y > HeightColhead && y < HeightTable)
			{
				x += offsetHori;
				if (x < WidthTable)
				{
					int left = getLeft();
					if (x > left)
					{
						var cords = getCords(x,y, left);
						return this[cords.Y, cords.X];
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Starts cell-edit on either <c>LMB</c> or <c>[Enter]</c>.
		/// </summary>
		void Celledit()
		{
			EnsureDisplayed(_editcell);

			var rect = getCellRectangle(_editcell); // align the editbox over the text ->
			_editor.Left   = rect.X + 5;
			_editor.Top    = rect.Y + 4;
			_editor.Width  = rect.Width - 6;
			_editor.Height = rect.Height;

			_editor.Text = _editcell.text;

			_editor.SelectionStart = 0;
			_editor.SelectionLength = _editor.Text.Length;

			_editor.Visible = true;
			_editor.Focus();
		}

		/// <summary>
		/// Starts a cell-edit from YataForm via the cellmenu.
		/// </summary>
		/// <param name="cell"></param>
		internal void startCelledit(Cell cell)
		{
			_editcell = cell;
			Celledit();
			Invalidator(INVALID_GRID);
		}

		/// <summary>
		/// Clears all <c><see cref="Cell">Cells</see></c> that are currently
		/// selected.
		/// </summary>
		/// <remarks>The caller shall call
		/// <c><see cref="YataForm.EnableCelleditOperations()">YataForm.EnableCelleditOperations()</see></c>
		/// after it deters required cell-selects.</remarks>
		internal void ClearCellSelects()
		{
//			_cell_anchorshift = null; // ~safety. Would need to go through all select patterns.

			foreach (var row in Rows)
			for (int c = 0; c != ColCount; ++c)
				row[c].selected = false;
		}

		/// <summary>
		/// Clears all <c><see cref="Col">Cols</see></c>/
		/// <c><see cref="Row">Rows</see></c>/
		/// <c><see cref="Cell">Cells</see></c> that are currently selected.
		/// </summary>
		/// <param name="bypassEnableCelledit"><c>true</c> to bypass
		/// <c><see cref="YataForm.EnableCelleditOperations()">YataForm.EnableCelleditOperations()</see></c>
		/// - this typically means that the caller shall select a <c>Cell</c>
		/// and call <c>EnableCelleditOperations()</c> itself or that selects
		/// are being cleared from a synced <c><see cref="YataGrid"/></c></param>
		/// <param name="bypassEnableRowedit"><c>true</c> to bypass
		/// <c><see cref="YataForm.EnableRoweditOperations()">YataForm.EnableRoweditOperations()</see></c>
		/// - this typically means that the caller shall select a <c>Row</c> and
		/// call <c>EnableCelleditOperations()</c> itself or that selects are
		/// being cleared from a synced <c><see cref="YataGrid"/></c></param>
		internal void ClearSelects(bool bypassEnableCelledit = false, bool bypassEnableRowedit = false)
		{
//			_cell_anchorshift = null; // ~safety. Would need to go through all select patterns.

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
					Row._bypassEnableRowedit = bypassEnableRowedit;
					row.selected = false;
					Row._bypassEnableRowedit = false;
				}

				for (int c = 0; c != ColCount; ++c)
					row[c].selected = false;
			}

			if (!bypassEnableCelledit)
				_f.EnableCelleditOperations();
		}


		/// <summary>
		/// Handles mouse-movement over the grid and its background area -
		/// prints coordinates and path-info to the statusbar.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!_init)
			{
				if (   e.X < Width  - (_visVert ? _scrollVert.Width  : 0)
					&& e.Y < Height - (_visHori ? _scrollHori.Height : 0))
				{
					int y = e.Y + offsetVert;
					if (y > HeightColhead && y < HeightTable)
					{
						int x = e.X + offsetHori;
						if (x < WidthTable)
						{
							int left = getLeft();
							if (x > left)
							{
								_f.PrintInfo(getCords(x,y, left));
								return;
							}
						}
					}
				}
				_f.PrintInfo(); // clear
			}
		}


		/// <summary>
		/// Clears statusbar coordinates and path-info when the mouse-cursor
		/// leaves the grid.
		/// </summary>
		/// <remarks>Called by <c><see cref="YataForm"></see>.t1_Tick()</c>. The
		/// <c>MouseLeave</c> event is unreliable.</remarks>
		internal void MouseLeaveTicker()
		{
			int left = getLeft();
			var rect = new Rectangle(left,
									 HeightColhead,
									 Width  - left          - (_visVert ? _scrollVert.Width  : 0),
									 Height - HeightColhead - (_visHori ? _scrollHori.Height : 0));

			if (!rect.Contains(PointToClient(Cursor.Position)))
				_f.PrintInfo(); // clear
		}


		/// <summary>
		/// Checks if there is at least one selected cell and that all selected
		/// cells are in a contiguous block. Stores the count of rows/cols in
		/// <c><see cref="YataForm._copyvert">YataForm._copyvert</see></c> and
		/// <c><see cref="YataForm._copyhori">YataForm._copyhori</see></c> ready
		/// for use by
		/// <c><see cref="YataForm._copytext">YataForm._copytext[,]</see></c>.
		/// </summary>
		/// <returns><c>true</c> if any/all selected cells are in a continguous
		/// rectangular block</returns>
		/// <remarks>This turned out to be more complicated than first expected.
		/// There's probably an easier way using <c>Lists</c> of x/y positions
		/// ...</remarks>
		internal bool areSelectedCellsContiguous()
		{
			//logfile.Log("YataGrid.areSelectedCellsContiguous()");

			int
				r0 = -1,			// tracks current row
				c0 = -1, c1 = -1,	// start and stop of selected cells on 1st row w/ selected cells
				c2,      c3 = -1,	// start and stop of selected cells on subsequent rows
				cols = -1,			// turns to 0 when 1st selected cell is found; actual val is set after 1st found row
				rows =  0;			// count of rows w/ selected cells

			for (int r = 0; r != RowCount; ++r)
			{
				c2 = -1;
				for (int c = 0; c != ColCount; ++c)
				{
					if (this[r,c].selected)
					{
						if (cols == -1) cols = 0;

						if (cols == 0)					// on 1st row w/ selected cell
						{
							if (c0 == -1)
							{
								c0 = c;					// 1st selected cell
								++rows;
							}
							else if (c != c1 + 1)		// 2nd+ cell - fail if not consecutive
								return false;

							c1 = c;						// set 'c1' stop to current cell
						}
						else							// on 2nd+ row w/ selected cell
						{
							if (c2 == -1)				// 1st selected cell
							{
								if ((c2 = c) != c0)		// fail if 1st col doesn't match 1st col
									return false;
							}
							else if (c != c3 + 1)		// 2nd+ cell - fail if not consecutive
								return false;

							if (c >= c2) c3 = c;		// set 'c3' stop to current cell
						}

						if (r0 == -1) r0 = r;			// on 1st row w/ selected cell
						else							// on 2nd+ row w/ selected cell
						{
							if (r != r0 && r != r0 + 1)	// fail if row not consecutive
								return false;

							r0 = r;						// advance 'r0' to current row
						}
					}
				}

				if (cols != -1)							// ie. bypass if a selected cell not found yet
				{
					if (cols == 0) cols = c1 - c0 + 1;	// 1st row w/ selected cell - set count of cols
					else if (c2 != -1					// 2nd+ row w/ selected cell - bypass if selected cell not found on row
						&& (c3 - c2 + 1 != cols			// fail if count of cols don't match or
							|| c3 != c1))				// fail if last col doesn't match
					{
						return false;
					}

					if (c2 != -1) ++rows;
				}
			}

			_f._copyvert = rows;
			_f._copyhori = cols;

			//logfile.Log(". " + (r0 != -1) + " " + rows + "/" + cols);

			return r0 != -1;
		}


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

		/// <summary>
		/// Checks if any <c><see cref="Cell"/></c> is currently loadchanged.
		/// </summary>
		/// <returns><c>true</c> if a <c>Cell</c> is loadchanged</returns>
		internal bool anyLoadchanged()
		{
			foreach (var row in Rows)
			for (int c = 0; c != ColCount; ++c)
			{
				if (row[c].loadchanged)
					return true;
			}
			return false;
		}


		/// <summary>
		/// Clears all cells'
		/// <c><see cref="Cell.loadchanged">Cell.loadchanged</see></c> flags
		/// without resetting the GotoLoadchanged it repeatedly.
		/// </summary>
		internal void ClearLoadchanged()
		{
			_init = true;

			foreach (var row in Rows)
			for (int c = 0; c != ColCount; ++c)
			if (row[c].loadchanged)
				row[c].loadchanged = false;

			_init = false;

			_f.EnableGotoLoadchanged(false);
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
			Invalidator(YataGrid.INVALID_GRID);
		}


		/// <summary>
		/// Gets the col/row coordinates of a <c><see cref="Cell"/></c> based on
		/// the current position of the cursor.
		/// </summary>
		/// <param name="x">mouse x-pos within the table</param>
		/// <param name="y">mouse y-pos within the table</param>
		/// <param name="left">the x-pos of the right edge of the frozen-panel;
		/// ie. the left edge of the visible area of this <c>YataGrid</c></param>
		/// <returns>a <c>Point</c> where <c>X</c> is col-id and <c>Y</c> is
		/// row-id</returns>
		Point getCords(int x, int y, int left)
		{
			int l = left;	// NOTE: That's only to get rid of the erroneous "Parameter
							// is assigned but its value is never used" warning. Notice,
							// however, that now 'l' is never used but ... no warning.
							// Thank god these guys didn't write the code that got to the Moon.
							//
							// ps. My theory is that Stanley Kubrick wanted people to believe
							// that he faked the landings; that's right, he ** faked fake **
							// Moon landings!

			var cords = new Point();

			cords.X = FrozenCount;
			while ((left += Cols[cords.X].width()) < x)
				++cords.X;

			int top = HeightColhead;

			cords.Y = 0;
			while ((top += HeightRow) < y)
				++cords.Y;

			return cords;
		}

		/// <summary>
		/// Gets the cell-rectangle for a given <c><see cref="Cell"/></c>.
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		Rectangle getCellRectangle(Cell cell)
		{
			var rect = new Rectangle();

			rect.X = WidthRowhead - offsetHori;
			for (int c = 0; c != cell.x; ++c)
				rect.X += Cols[c].width();

			rect.Y = HeightColhead - offsetVert;
			for (int r = 0; r != cell.y; ++r)
				rect.Y += HeightRow;

			rect.Width = Cols[cell.x].width();
			rect.Height = HeightRow;

			return rect;
		}

		/// <summary>
		/// Not really a <c>Point</c> but the left and right bounds of a
		/// <c><see cref="Col"/></c>.
		/// </summary>
		/// <param name="c">col-id</param>
		/// <returns></returns>
		Point getColBounds(int c)
		{
			var bounds = new Point();

			bounds.X = WidthRowhead - offsetHori;
			for (int col = 0; col != c; ++col)
				bounds.X += Cols[col].width();

			bounds.Y = (bounds.X + Cols[c].width());

			return bounds;
		}

		/// <summary>
		/// Not really a <c>Point</c> but the upper and lower bounds of a
		/// <c><see cref="Row"/></c>.
		/// </summary>
		/// <param name="r">row-id</param>
		/// <returns></returns>
		Point getRowBounds(int r)
		{
			var bounds = new Point();
			bounds.X = HeightColhead + HeightRow * r - offsetVert;
			bounds.Y = bounds.X + HeightRow;

			return bounds;
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
				left += Cols[c].width();

			return left;
		}


		/// <summary>
		/// Scrolls the table so that a given <c><see cref="Cell"/></c> is (more
		/// or less) completely displayed.
		/// </summary>
		/// <param name="cell">the <c>Cell</c> to display</param>
		/// <param name="bypassPropanel"><c>true</c> to bypass any
		/// <c><see cref="PropertyPanel"/></c> considerations</param>
		/// <returns>a bitwise <c>int</c> defining controls that need to be
		/// invalidated; note that the <c>PropertyPanel's</c> invalidation bit
		/// will be flagged as long as the panel is visible regardless of
		/// whether it really needs to be redrawn</returns>
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
					{																	//    than the table's visible width.
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
			else
			{
				_scrollHori.Value = 0;
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
		/// Scrolls the table so that the currently selected
		/// <c><see cref="Cell"/></c> or <c><see cref="Row"/></c> is (more or
		/// less) completely displayed.
		/// </summary>
		/// <returns>a bitwise <c>int</c> defining controls that need to be
		/// invalidated</returns>
		internal int EnsureDisplayed()
		{
			Cell sel = getSelectedCell();
			if (sel != null)
				return EnsureDisplayed(sel);

			int r = getSelectedRow();
			if (r != -1)
				return EnsureDisplayedRow(r);

			return INVALID_NONE;
		}

		/// <summary>
		/// Scrolls the table so that a given <c><see cref="Row"/></c> is (more
		/// or less) completely displayed.
		/// </summary>
		/// <param name="r">the row-id to display</param>
		/// <returns>a bitwise <c>int</c> defining controls that need to be
		/// invalidated</returns>
		internal int EnsureDisplayedRow(int r)
		{
			Point bounds = getRowBounds(r);
			if (bounds.X != HeightColhead)
			{
				if (bounds.X < HeightColhead)
				{
					_scrollVert.Value -= HeightColhead - bounds.X;
					return (INVALID_GRID | INVALID_FROZ | INVALID_ROWS); // TODO: All those might not be needed ...
				}

				int bar = (_visHori ? _scrollHori.Height : 0);
				if (bounds.Y + bar > Height)
				{
					_scrollVert.Value += bounds.Y + bar - Height;
					return (INVALID_GRID | INVALID_FROZ | INVALID_ROWS); // TODO: All those might not be needed ...
				}
			}

			// TODO: Wait a second. Setting a scrollbar.Value auto-refreshes the grid ...
			return INVALID_NONE;
		}

		/// <summary>
		/// Scrolls the table so that a given <c><see cref="Col"/></c> is (more
		/// or less) completely displayed.
		/// </summary>
		/// <param name="c">the col-id to display</param>
		internal void EnsureDisplayedCol(int c)
		{
			Point bounds = getColBounds(c);

			int left = getLeft();

			if (bounds.X != left)
			{
				int bar = (_visVert ? _scrollVert.Width : 0);
				int right = Width - bar;

				int width = bounds.Y - bounds.X;

				if (bounds.X < left
					|| (width > right - left
						&& (bounds.X > right || bounds.X + left > (right - left) / 2)))
				{
					_scrollHori.Value -= left - bounds.X;
				}
				else if (bounds.Y > right && width < right - left)
				{
					_scrollHori.Value += bounds.X + width + bar - Width;
				}
			}
		}


		/// <summary>
		/// Performs a goto when the <c>[Enter]</c> key is pressed and focus is
		/// on the goto-box.
		/// </summary>
		/// <param name="text">a <c>string</c> to parse for a row-id</param>
		/// <param name="selectTable"><c>true</c> to <c>Select()</c> this
		/// <c>YataGrid</c></param>
		internal void doGoto(string text, bool selectTable)
		{
			int r;
			if (Int32.TryParse(text, out r)
				&& r > -1 && r < RowCount)
			{
				_editor.Visible = false;
				ClearSelects();

				SelectRow(r);
				EnsureDisplayedRow(r);

				int invalid = (INVALID_GRID | INVALID_FROZ | INVALID_ROWS);
				if (Propanel != null && Propanel.Visible)
					invalid |= INVALID_PROP;

				Invalidator(invalid);

				if (selectTable) // on [Enter] ie. not instantgoto
					Select();
			}
		}


		/// <summary>
		/// Handles a <c>MouseClick</c> event on the rowhead. Selects or
		/// deselects <c><see cref="Row">Row(s)</see></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="_panelRows"/></c></param>
		/// <param name="e"></param>
		/// <remarks>.net fucks with <c>RMB</c> and <c>[Alt]</c> differently
		/// than <c>LMB</c> and <c>[Ctrl]</c>/<c>[Shift]</c>.</remarks>
		internal void click_RowheadPanel(object sender, MouseEventArgs e)
		{
			if ((ModifierKeys & Keys.Alt) == 0)
			{
				Select();

				int row = (e.Y + offsetVert) / HeightRow;
				if (row < RowCount)
				{
					switch (e.Button)
					{
						case MouseButtons.Left:
						{
							// get the sync-table if one exists
							YataGrid table;
							if      (this == _f._diff1) table = _f._diff2;
							else if (this == _f._diff2) table = _f._diff1;
							else                        table = null;

							int selcsync = (table != null) ? table.getSelectedCol() : -1;


							int selc = getSelectedCol(); // do not clear cell-selects in a selected col

							// if cells in another row are currently selected and a
							// row that is already selected is clicked just clear those
							// extraneous cells (instead of deselecting the clicked row)
							bool celldeselected = false;

							if ((ModifierKeys & Keys.Control) == 0) // clear all other row's cells ->
							{
								Cell cell;
								for (int r = 0; r != RowCount; ++r)
								for (int c = 0; c != ColCount; ++c)
								{
									if (r != row && c != selc
										&& (cell = this[r,c]).selected)
									{
										cell.selected = false;
										celldeselected = true;
									}
								}

								if (table != null)
								{
									for (int r = 0; r != table.RowCount; ++r)
									for (int c = 0; c != table.ColCount; ++c)
									{
										if (r != row && c != selcsync)
											table[r,c].selected = false;
									}
								}
							}


							Row._bypassEnableRowedit = true; // call _f.EnableRoweditOperations() later ...

							bool display = false; // deter col for ensure displayed col

							int selr = getSelectedRow();

							if ((ModifierKeys & Keys.Shift) == 0) // select only the clicked row ->
							{
								if (selr != -1 && selr != row) // clear any other selected row ->
									Rows[selr].selected = false;

								if (table != null)
								{
									int selrsync = table.getSelectedRow();
									if (selrsync != -1 && selrsync != row)
										table.Rows[selrsync].selected = false;
								}


								bool allcellsselected = true;
								foreach (var cell in Rows[row]._cells)
								if (!cell.selected)
								{
									allcellsselected = false;
									break;
								}

								// select the clicked row if
								// (a) it is not currently selected
								// (b) or not all cells in the clicked row are currently selected
								// (c) or cells not in the clicked row got deselected above
								bool @select = !Rows[row].selected || !allcellsselected || celldeselected;

								Rows[row].selected = @select;

								for (int c = 0; c != ColCount; ++c) // select or deselect cells in the clicked row ->
								{
									if (@select || c != selc)
										this[row,c].selected = @select;
								}

								if (table != null && row < table.RowCount) // select or deselect cells in a sync-table's row ->
								{
									table.Rows[row].selected = @select;

									for (int c = 0; c != table.ColCount; ++c)
									{
										if (@select || c != selcsync)
											table[row,c].selected = @select;
									}
								}

								display = @select;
							}
							else if (selr != -1) // subselect a range of rows ->
							{
								RangeSelect = row - selr;
								if (RangeSelect != 0)
								{
									display = true;

									int start, stop;
									if (RangeSelect > 0)
									{
										start = selr; stop = row;
									}
									else
									{
										start = row; stop = selr;
									}

									for (int r = start; r <= stop; ++r) // select subselected rows' cells ->
									for (int c = 0; c != ColCount; ++c)
									{
										this[r,c].selected = true;
									}

									if (table != null && start < table.RowCount) // select subselected rows' cells in a sync-table ->
									{
										for (int r = start; r <= stop && r != table.RowCount; ++r)
										for (int c = 0; c != table.ColCount; ++c)
										{
											table[r,c].selected = true;
										}
									}
								}
							}

							Row._bypassEnableRowedit = false;

							if (display) EnsureDisplayedRow(row);

							int invalid = (INVALID_GRID | INVALID_FROZ | INVALID_ROWS);
							if (Propanel != null && Propanel.Visible)
								invalid |= INVALID_PROP;

							Invalidator(invalid);

							_f.EnableCelleditOperations();	// TODO: tighten that to only if necessary.
							_f.EnableRoweditOperations();	// TODO: tighten that to only if necessary.
							break;
						}

						case MouseButtons.Right:
							if (ModifierKeys == Keys.None)
							{
								ClearSelects();

								SelectRow(row);
								EnsureDisplayedRow(row);

								int invalid = (INVALID_GRID | INVALID_FROZ | INVALID_ROWS);
								if (Propanel != null && Propanel.Visible)
									invalid |= INVALID_PROP;

								Invalidator(invalid);

								_f.ShowRowContext(row);
							}
							break;
					}
				}
				else if (ModifierKeys == Keys.None) // click below the last entry ->
				{
					ClearSelects();
					_f.ClearSyncSelects();

					int invalid = (INVALID_GRID | INVALID_FROZ | INVALID_ROWS);
					if (Propanel != null && Propanel.Visible)
						invalid |= INVALID_PROP;

					Invalidator(invalid);
				}
			}
		}

		/// <summary>
		/// Gets the row-id of the currently selected <c><see cref="Row"/></c>.
		/// </summary>
		/// <returns>the currently selected row-id; <c>-1</c> if no <c>Row</c>
		/// is currently selected</returns>
		internal int getSelectedRow()
		{
			for (int r = 0; r != RowCount; ++r)
			{
				if (Rows[r].selected)
					return r;
			}
			return -1;
		}

		/// <summary>
		/// Gets the row-id of the currently selected <c><see cref="Row"/></c>
		/// or a row-id that has <c><see cref="Cell">Cells</see></c> selected
		/// iff only that <c>Row</c> has <c>Cells</c> selected.
		/// </summary>
		/// <returns>the currently selected row-id or the row-id that has
		/// selected <c>Cells</c>; <c>-1</c> if no <c>Row</c> is currently valid</returns>
		internal int getRowOrCellsSelected()
		{
			int selr = getSelectedRow();
			if (selr == -1)
			{
				Cell sel = getFirstSelectedCell();
				if (sel != null)
				{
					selr = sel.y;

					for (int r = sel.y + 1; r != RowCount; ++r)
					for (int c = 0; c != ColCount; ++c)
						if (this[r,c].selected)
							return -1;
				}
			}
			return selr;
		}

		/// <summary>
		/// Handles a <c>MouseClick</c> event on the colhead. Selects or
		/// deselects <c><see cref="Col">Col(s)</see></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="_panelCols"/></c></param>
		/// <param name="e"></param>
		/// <remarks>.net fucks with <c>RMB</c> and <c>[Alt]</c> differently
		/// than <c>LMB</c> and <c>[Ctrl]</c>/<c>[Shift]</c>.</remarks>
		internal void click_ColheadPanel(object sender, MouseEventArgs e)
		{
			if (!_panelCols.Grab && (ModifierKeys & Keys.Alt) == 0)
			{
				Select();

				int col;
				switch (e.Button)
				{
					case MouseButtons.Left:
						if ((col = getClickedCol(e.X)) != -1)
						{
							// get the sync-table if one exists
							YataGrid table;
							if      (this == _f._diff1) table = _f._diff2;
							else if (this == _f._diff2) table = _f._diff1;
							else                        table = null;

							int selrsync = (table != null) ? table.getSelectedRow() : -1;


							int selr = getSelectedRow(); // do not clear cell-selects in a selected row

							// if cells in another col are currently selected and a
							// col that is already selected is clicked just clear those
							// extraneous cells (instead of deselecting the clicked col)
							bool celldeselected = false;

							if ((ModifierKeys & Keys.Control) == 0) // clear all other col's cells ->
							{
								Cell cell;
								for (int r = 0; r != RowCount; ++r)
								for (int c = 0; c != ColCount; ++c)
								{
									if (c != col
										&& (   (r > selr && r > selr + RangeSelect)
											|| (r < selr && r < selr + RangeSelect))
										&& (cell = this[r,c]).selected)
									{
										this[r,c].selected = false;
										celldeselected = true;
									}
								}

								if (table != null)
								{
									for (int r = 0; r != table.RowCount; ++r)
									for (int c = 0; c != table.ColCount; ++c)
									{
										if (c != col
											&& (   (r > selrsync && r > selrsync + table.RangeSelect)
												|| (r < selrsync && r < selrsync + table.RangeSelect)))
										{
											table[r,c].selected = false;
										}
									}
								}
							}


							bool display = false; // deter col for ensure displayed col

							int selc = getSelectedCol();

							if ((ModifierKeys & Keys.Shift) == 0)
							{
								if (selc != -1 && selc != col) // clear any other selected col ->
									Cols[selc].selected = false;

								if (table != null)
								{
									int selcsync = table.getSelectedCol();
									if (selcsync != -1 && selcsync != col)
										table.Cols[selcsync].selected = false;
								}


								bool allcellsselected = true;
								foreach (var row in Rows)
								if (!row[col].selected)
								{
									allcellsselected = false;
									break;
								}

								// select the clicked col if
								// (a) it is not currently selected
								// (b) or not all cells in the clicked col are currently selected
								// (c) or cells not in the clicked col got deselected above
								bool @select = !Cols[col].selected || !allcellsselected || celldeselected;

								Cols[col].selected = @select;

								for (int r = 0; r != RowCount; ++r) // select or deselect cells in the clicked col ->
								{
									if (@select
										|| (r > selr && r > selr + RangeSelect)
										|| (r < selr && r < selr + RangeSelect))
									{
										this[r,col].selected = @select;
									}
								}

								if (table != null && col < table.ColCount) // select or deselect cells in a sync-table's col ->
								{
									table.Cols[col].selected = @select;

									for (int r = 0; r != table.RowCount; ++r)
									{
										if (@select
											|| (r > selrsync && r > selrsync + table.RangeSelect)
											|| (r < selrsync && r < selrsync + table.RangeSelect))
										{
											table[r,col].selected = @select;
										}
									}
								}

								display = @select;
							}
							else if (selc != -1) // subselect a range of cols ->
							{
								int delta = col - selc;
								if (delta != 0)
								{
									display = true;

									int start, stop;
									if (delta > 0)
									{
										start = selc; stop = col;
									}
									else
									{
										start = col; stop = selc;
									}

									for (int r = 0; r != RowCount; ++r) // select range-selected cols' cells ->
									for (int c = start; c <= stop; ++c)
									{
										this[r,c].selected = true;
									}

									if (table != null && start < table.ColCount) // select range-selected cols' cells in a sync-table ->
									{
										for (int r = 0; r != table.RowCount; ++r)
										for (int c = start; c <= stop && c != table.ColCount; ++c)
										{
											table[r,c].selected = true;
										}
									}
								}
							}


							if (display) EnsureDisplayedCol(col);

//							invalid |= INVALID_FROZ;					// <- doesn't seem to be needed.
//							if (Propanel != null && Propanel.Visible)
//								invalid |= INVALID_PROP;				// <- doesn't seem to be needed.

							Invalidator(INVALID_COLS | INVALID_GRID);

							_f.EnableCelleditOperations(); // TODO: tighten that to only if necessary.
						}
						break;

					case MouseButtons.Right:
						if (ModifierKeys == Keys.Shift) // sort by col ->
						{
							if ((col = getClickedCol(e.X)) != -1)
							{
								ColSort(col);
								EnsureDisplayedCol(col);
								Invalidator(INVALID_GRID
										  | INVALID_FROZ
										  | INVALID_COLS
										  | INVALID_LBLS);
							}
						}
//						else // popup colhead context
//						{
//							// change colhead text
//							// insert col
//							// fill col-cells w/ text (req. inputbox)
//						}
						break;
				}
			}
		}

		/// <summary>
		/// Gets a selected <c><see cref="Col"/></c> based on x-position of a
		/// mouseclick.
		/// </summary>
		/// <param name="x"></param>
		/// <returns>col-id or <c>-1</c> if out of bounds</returns>
		int getClickedCol(int x)
		{
			x += offsetHori;

			int left = getLeft();
			int c = FrozenCount - 1;
			do
			{
				if (++c == ColCount)
					return -1;
			}
			while ((left += Cols[c].width()) < x);

			return c;
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


		static int _heightColheadCached;
		void label_Resize(object sender, EventArgs e)
		{
			if (_heightColheadCached != HeightColhead)
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

		/// <summary>
		/// Shift+RMB = sort by id-col
		/// </summary>
		/// <param name="sender"><c><see cref="_labelid"/></c></param>
		/// <param name="e"></param>
		void labelid_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				labelSort(0);
		}

		/// <summary>
		/// Shift+RMB = sort by 1st frozen col
		/// </summary>
		/// <param name="sender"><c><see cref="_labelfirst"/></c></param>
		/// <param name="e"></param>
		void labelfirst_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				labelSort(1);
		}

		/// <summary>
		/// Shift+RMB = sort by 2nd frozen col
		/// </summary>
		/// <param name="sender"><c><see cref="_labelsecond"/></c></param>
		/// <param name="e"></param>
		void labelsecond_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				labelSort(2);
		}

		/// <summary>
		/// helper for sorting by labels
		/// </summary>
		/// <param name="col"></param>
		void labelSort(int col)
		{
			if (ModifierKeys == Keys.Shift)
			{
				_editor.Visible = false;
				Select();

				ColSort(col);
				EnsureDisplayed();

				Invalidator(INVALID_GRID
						  | INVALID_FROZ
						  | INVALID_COLS
						  | INVALID_LBLS);
			}
		}


		/// <summary>
		/// Inserts a row into the table.
		/// </summary>
		/// <param name="id">row-id to insert at</param>
		/// <param name="fields">an array of fields</param>
		/// <param name="calibrate"><c>true</c> to re-layout the grid or
		/// <c>false</c> if <c><see cref="Calibrate()">Calibrate()</see></c>
		/// will be done by the caller</param>
		/// <param name="brush">a <c>Brush</c> to use for Undo/Redo</param>
		internal void Insert(int      id,
							 string[] fields,
							 bool     calibrate = true,
							 Brush    brush     = null)
		{
			if (calibrate)
				DrawingControl.SuspendDrawing(this);

			if (fields != null)
			{
				if (brush == null)
					brush = Brushes.Created;

				var row = new Row(id, ColCount, brush, this);

				string field;
				for (int c = 0; c != ColCount; ++c)
				{
					if (c < fields.Length)
						field = fields[c];
					else
						field = gs.Stars;

					row[c] = new Cell(id, c, field);
				}

				Rows.Insert(id, row);
				++RowCount;

				for (int r = id + 1; r != RowCount; ++r) // straighten out row._id and cell.y ->
				{
					++(row = Rows[r])._id;
					for (int c = 0; c != ColCount; ++c)
						++row[c].y;
				}
			}

			if (calibrate) // is only 1 row (no range) via context single-row edit
			{
				Calibrate(id);

				if (id < RowCount)
					EnsureDisplayedRow(id);

				DrawingControl.ResumeDrawing(this);
			}
		}

		/// <summary>
		/// Deletes a row from the table.
		/// </summary>
		/// <param name="id">row-id to delete</param>
		/// <param name="calibrate"><c>true</c> to re-layout the grid or
		/// <c>false</c> if <c><see cref="Calibrate()">Calibrate()</see></c>
		/// will be done by the caller</param>
		internal void Delete(int id, bool calibrate = true)
		{
			if (calibrate)
				DrawingControl.SuspendDrawing(this);

			Row row;

			Rows.Remove(Rows[id]);
			--RowCount;

			for (int r = id; r != RowCount; ++r) // straighten out row._id and cell.y ->
			{
				--(row = Rows[r])._id;
				for (int c = 0; c != ColCount; ++c)
					--row[c].y;
			}

			if (RowCount == 0) // add a row of stars so grid is not left blank ->
			{
				++RowCount;

				row = new Row(id, ColCount, Brushes.Created, this);
				for (int c = 0; c != ColCount; ++c)
					row[c] = new Cell(id, c, gs.Stars);

				Rows.Add(row);

				if (calibrate)
				{
					Calibrate(0);
					DrawingControl.ResumeDrawing(this);

					return;
				}
			}

			if (calibrate) // is only 1 row (no range) via context single-row edit
			{
				Calibrate();

				if (id < RowCount)
					EnsureDisplayedRow(id);

				DrawingControl.ResumeDrawing(this);
			}
		}

		/// <summary>
		/// Deletes a single or multiple rows.
		/// </summary>
		/// <remarks>Called by
		/// <c><see cref="YataForm"/>.editrowsclick_DeleteRange()</c>.</remarks>
		internal void DeleteRows()
		{
			_f.Obfuscate();
			DrawingControl.SuspendDrawing(this);


			int selr = getSelectedRow();

			int range = Math.Abs(RangeSelect);
			Restorable rest = UndoRedo.createArray(range + 1, UndoRedo.UrType.rt_ArrayInsert);

			int rFirst, rLast;
			if (RangeSelect > 0)
			{
				rFirst = selr;
				rLast  = selr + RangeSelect;
			}
			else
			{
				rFirst = selr + RangeSelect;
				rLast  = selr;
			}

			while (rLast >= rFirst) // reverse delete.
			{
				rest.array[range--] = Rows[rLast].Clone() as Row;
				Delete(rLast, false);

				--rLast;
			}

			if (RowCount == 1 && rLast == -1) // ie. if grid was blanked -> ID #0 was auto-inserted.
				rLast = 0;
			else
				rLast = -1; // calibrate all extant rows.

			Calibrate(rLast); // delete key

			if (selr < RowCount)
				EnsureDisplayedRow(selr);


			if (!Changed)
			{
				Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			_ur.Push(rest);


			DrawingControl.ResumeDrawing(this);
			_f.Obfuscate(false);
		}


		#region Sort
		static ToolTip TooltipSort = new ToolTip(); // hint when table isn't sorted by ID-asc.

		/// <summary>
		/// Sorts rows by a col either ascending or descending.
		/// </summary>
		/// <param name="col">the col-id to sort by</param>
		/// <remarks>Performs a mergesort.</remarks>
		void ColSort(int col)
		{
			DrawingControl.SuspendDrawing(_f);

			_f.tabclick_DiffReset(null, EventArgs.Empty);

			Changed = true; // TODO: do Changed only if rows are swapped/order is *actually* changed.
			_ur.ResetSaved(true);

			RangeSelect = 0;

			if (_sortdir != SORT_ASC || _sortcol != col)
				_sortdir = SORT_ASC;
			else
				_sortdir = SORT_DES;

			_sortcol = col;


			if (!Settings._strict) // ASSUME people who use strict settings know what they're doing.
			{
				if (_sortcol == 0 && _sortdir == SORT_ASC)
					TooltipSort.SetToolTip(_labelid, "");
				else
					TooltipSort.SetToolTip(_labelid, "warn : Table is not sorted by ascending ID");
			}

			var rowsT = new List<Row>();
			TopDownMergeSort(Rows, rowsT, RowCount);


			Row row; Cell cell; int presort;
			for (int r = 0; r != RowCount; ++r) // straighten out row._id and cell.y ->
			{
				row = Rows[r];
				presort = row._id;

				row._id_presort = presort;
				row._id = r;

				for (int c = 0; c != ColCount; ++c)
				{
					cell = row[c];

					cell.y_presort = presort;
					cell.y = r;
				}
			}
			_ur.ResetY(); // straighten out row._id and cell.y in UndoRedo's Restorables

			DrawingControl.ResumeDrawing(_f);
		}

		/// <summary>
		/// Starts a mergesort.
		/// https://en.wikipedia.org/wiki/Merge_sort#Top-down_implementation
		/// </summary>
		/// <param name="rows"></param>
		/// <param name="rowsT"></param>
		/// <param name="count"></param>
		/// <remarks>Helper for <see cref="ColSort"/>.</remarks>
		void TopDownMergeSort(List<Row> rows, List<Row> rowsT, int count)
		{
			CopyList(rows, 0, count, rowsT);
			TopDownSplitMerge(rowsT, 0, count, rows);
		}

		/// <summary>
		/// Makes a copy of the rows to sort.
		/// </summary>
		/// <param name="rows"></param>
		/// <param name="iBegin"></param>
		/// <param name="iEnd"></param>
		/// <param name="rowsT"></param>
		/// <remarks>Helper for <see cref="TopDownMergeSort"/>.</remarks>
		void CopyList(IList<Row> rows, int iBegin, int iEnd, ICollection<Row> rowsT)
		{
			for (int i = iBegin; i != iEnd; ++i)
				rowsT.Add(rows[i]);
		}

		/// <summary>
		/// Recursive funct that shuffles through the sort-lists.
		/// </summary>
		/// <param name="rowsT"></param>
		/// <param name="iBegin"></param>
		/// <param name="iEnd"></param>
		/// <param name="rows"></param>
		/// <remarks>Called by <see cref="TopDownMergeSort"/>.</remarks>
		void TopDownSplitMerge(List<Row> rowsT, int iBegin, int iEnd, List<Row> rows)
		{
			if (iEnd - iBegin < 2)
				return;

			int iMiddle = (iEnd + iBegin) / 2;

			TopDownSplitMerge(rows, iBegin,  iMiddle, rowsT);
			TopDownSplitMerge(rows, iMiddle, iEnd,    rowsT);

			TopDownMerge(rowsT, iBegin, iMiddle, iEnd, rows);
		}

		/// <summary>
		/// Shuffles through the sort-lists.
		/// </summary>
		/// <param name="rows"></param>
		/// <param name="iBegin"></param>
		/// <param name="iMiddle"></param>
		/// <param name="iEnd"></param>
		/// <param name="rowsT"></param>
		/// <remarks>Helper for <see cref="TopDownSplitMerge"/>.</remarks>
		void TopDownMerge(IList<Row> rows, int iBegin, int iMiddle, int iEnd, IList<Row> rowsT)
		{
			int i = iBegin, j = iMiddle;

			for (int k = iBegin; k != iEnd; ++k)
			{
				if (i < iMiddle && (j >= iEnd || Sort(rows[i], rows[j]) == _sortdir))
				{
					rowsT[k] = rows[i];
					++i;
				}
				else
				{
					rowsT[k] = rows[j];
					++j;
				}
			}
		}


		string _a,  _b;
		int    _ai, _bi;
		float  _af, _bf;

		/// <summary>
		/// Sorts fields as integers iff they convert to integers, or floats as
		/// floats, else as strings and performs a secondary sort against their
		/// IDs if applicable.
		/// </summary>
		/// <param name="row1">the value of the reference to a 'Row'</param>
		/// <param name="row2">the value of the reference to a 'Row'</param>
		/// <returns>-1 first is first, second is second // TODO: figure that out ...
		///           0 identical
		///           1 first is second, second is first</returns>
		int Sort(Row row1, Row row2)
		{
			_a = row1[_sortcol].text;
			_b = row2[_sortcol].text;

			if (!Settings._casesort)
			{
				_a = _a.ToLower();
				_b = _b.ToLower();
			}

			int result;

			bool a_isStars = (_a == gs.Stars);
			bool b_isStars = (_b == gs.Stars);

			if (a_isStars && b_isStars) // sort stars last.
			{
				result = 0;
			}
			else if (a_isStars && !b_isStars)
			{
				result = 1;
			}
			else if (!a_isStars && b_isStars)
			{
				result = -1;
			}
			else
			{
				bool a_isInt = Int32.TryParse(_a, out _ai);
				bool b_isInt = Int32.TryParse(_b, out _bi);

				if (a_isInt && !b_isInt) // order ints before floats/strings
				{
					result = -1;
				}
				else if (!a_isInt && b_isInt) // order ints before floats/strings
				{
					result = 1;
				}
				else if (a_isInt && b_isInt) // try int comparision
				{
					result = _ai.CompareTo(_bi);
				}
				else if (!_a.Contains(",") && !_b.Contains(",") // NOTE: ... how any library can convert (eg) "1,8,0,0,0" into "18000" and "0,3,10,0,0" to "31000" ...
					&& Single.TryParse(_a, out _af) // try float comparison
					&& Single.TryParse(_b, out _bf))
				{
					result = _af.CompareTo(_bf);
				}
				else
					result = String.CompareOrdinal(_a,_b); // else do string comparison
			}

			if (result == 0 && _sortcol != 0 // secondary sort on id if primary sort matches
				&& Int32.TryParse(row1[0].text, out _ai)
				&& Int32.TryParse(row2[0].text, out _bi))
			{
				result = _ai.CompareTo(_bi);
			}

			if (result < 0) return  1; // NOTE: These vals are reversed for Mergesort.
			if (result > 0) return -1;

			return 0;
		}

/*		void ColSort(int c) // Bubblesort -> hint: don't bother
		{
			if (_sortdir != 1 || _sortcol != c)
				_sortdir = 1;
			else
				_sortdir = -1;

			_sortcol = c;

			bool stop, changed = false;
			Row rowT, row;

			if (_sortdir == 1)
			{
				for (int sort = 0; sort != RowCount; ++sort)
				{
					stop = true;
					for (int r = 0; r != RowCount - 1; ++r)
					{
						if (Sort(Rows[r], Rows[r+1], c) > 0)
						{
							stop = false;
							changed = true;

							rowT = Rows[r];

							row = (Rows[r] = Rows[r+1]);
							row._id = r+1;
							foreach (var cell in row.cells)
								cell.y = r+1;

							row = (Rows[r+1] = rowT);
							row._id = r;
							foreach (var cell in row.cells)
								cell.y = r;
						}
					}
					if (stop) break;
				}
			}
			else //if (_sortdir == -1)
			{
				for (int sort = 0; sort != RowCount; ++sort)
				{
					stop = true;
					for (int r = 0; r != RowCount - 1; ++r)
					{
						if (Sort(Rows[r], Rows[r+1], c) < 0)
						{
							stop = false;
							changed = true;

							rowT = Rows[r];

							row = (Rows[r] = Rows[r+1]);
							row._id = r+1;
							foreach (var cell in row.cells)
								cell.y = r+1;

							Rows[r+1] = rowT;
							row._id = r;
							foreach (var cell in row.cells)
								cell.y = r;
						}
					}
					if (stop) break;
				}
			}
			Changed = changed;
		} */
		#endregion Sort


		#region DragDrop file(s)
		/// <summary>
		/// Handles dragging a file onto the grid.
		/// </summary>
		/// <param name="drgevent"></param>
		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			_f.yata_DragEnter(null, drgevent);
		}

		/// <summary>
		/// Handles dropping a file onto the grid.
		/// </summary>
		/// <param name="drgevent"></param>
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			_f.yata_DragDrop(null, drgevent);
		}
		#endregion DragDrop file(s)
	}
}
