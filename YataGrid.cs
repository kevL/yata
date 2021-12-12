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
	/// <summary>
	/// Loads, formats, and handles 2da-data as a table or grid on YataForm's
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
			INFO_FEAT	// 3
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

		internal const int FreezeId     = 1; // count of Cols that are frozen ->
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

		/// <summary>
		/// A static <c>List</c> used to pass parsed row-fields from
		/// <c><see cref="LoadTable()">LoadTable()</see></c> to
		/// <c><see cref="CreateRows()">CreateRows()</see></c> and then is
		/// cleared when no longer needed.
		/// </summary>
		static readonly List<string[]> _rows = new List<string[]>();
		#endregion Fields (static)


		#region Fields
		internal readonly YataForm _f;
		YataGrid _table; // for cycling through all tables

		internal int ColCount;
		internal int RowCount;

		internal readonly List<Col> Cols = new List<Col>();
		internal readonly List<Row> Rows = new List<Row>();

		internal readonly VScrollBar _scrollVert = new VScrollBar();
		internal readonly HScrollBar _scrollHori = new HScrollBar();

		int MaxVert; // Since a .NET scrollbar's Maximum value is not
		int MaxHori; // its maximum value calculate and store these. sic

		internal bool _visVert; // Be happy. happy happy
		internal bool _visHori;

				 int OffsetVert; // TODO: these are redundant w/ the scrollbars' Value ->
		internal int OffsetHori;

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
		internal readonly YataEditbox _editor = new YataEditbox(); // TODO: static

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
		int _sortdir = SORT_ASC;

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

						if (_frozenCount < ColCount)
						{
							(_anchorcell = this[sel.y, _frozenCount]).selected = true;

							EnsureDisplayed(_anchorcell);
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
		/// is above the currently selected row.</remarks>
		internal int RangeSelect
		{ get; set; }

		internal DateTime Lastwrite
		{ get; set; }
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor. Instantiates a 2da-file in this <c>YataGrid</c> format/layout.
		/// </summary>
		/// <param name="f">parent</param>
		/// <param name="pfe">path_file_extension</param>
		/// <param name="read">readonly</param>
		internal YataGrid(YataForm f, string pfe, bool read)
		{
//			DrawRegulator.SetDoubleBuffered(this);
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

			_editor.KeyDown    += keydown_Editor;
			_editor.LostFocus  += lostfocus_Editor;

			Controls.Add(_editor);

			AllowDrop = true;

			_ur = new UndoRedo(this);
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

			_table.OffsetVert = _table._scrollVert.Value;

			if (!_f._isEnterkeyedSearch								// <- if not Search by [Enter]
				&& (!Settings._instantgoto || !_f.tb_Goto.Focused))	// <- if not "instantgoto=true" when gotobox has focus
			{
				Select(); // <- workaround: refocus the table when the bar is moved by mousedrag (bar has to move > 0px)
			}

			Point loc = PointToClient(Cursor.Position);
			var args = new MouseEventArgs(MouseButtons.Left, 1, loc.X, loc.Y, 0); // clicks,x,y,delta
			OnMouseMove(args); // update coords on the Statusbar

			if (_table == YataForm.Table
				&&  _f._diff1 != null   && _f._diff2 != null
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

			_table.OffsetHori = _table._scrollHori.Value;

			if (!_f._isEnterkeyedSearch								// <- if not Search by [Enter]
				&& (!Settings._instantgoto || !_f.tb_Goto.Focused))	// <- if not "instantgoto=true" when gotobox has focus
			{
				Select(); // <- workaround: refocus the table when the bar is moved by mousedrag (bar has to move > 0px)
			}

			Point loc = PointToClient(Cursor.Position);
			var args = new MouseEventArgs(MouseButtons.Left, 1, loc.X, loc.Y, 0); // clicks,x,y,delta
			OnMouseMove(args); // update coords on the Statusbar

			if (_table == YataForm.Table
				&&  _f._diff1 != null   && _f._diff2 != null
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
							&&  _f._diff1 != null   && _f._diff2 != null
							&& (_f._diff1 == _table || _f._diff2 == _table))
						{
							doneSync = true;
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

				bool needsVertbar = HeightTable > Height;	// NOTE: Do not refactor this ->
				bool needsHoribar = WidthTable  > Width;	// don't even ask. It works as-is. Be happy. Be very happy.

				_visVert = false; // again don't ask. Be happy.
				_visHori = false;

				_scrollVert.Visible =
				_scrollHori.Visible = false;

				if (needsVertbar && needsHoribar)
				{
					_visVert =
					_visHori = true;

					_scrollVert.Visible =
					_scrollHori.Visible = true;
				}
				else if (needsVertbar)
				{
					_visVert = true;
					_visHori = WidthTable > Width - _scrollVert.Width;

					_scrollVert.Visible = true;
					_scrollHori.Visible = _visHori;
				}
				else if (needsHoribar)
				{
					_visVert = HeightTable > Height - _scrollHori.Height;
					_visHori = true;

					_scrollVert.Visible = _visVert;
					_scrollHori.Visible = true;
				}


				// Do not use .LargeChange in what follows. Use HeightRow instead.
				// If a table does not have a scrollbar and user resizes it such
				// that it needs one .LargeChange is 1 and things go bork.
				// .LargeChange shall be set to HeightRow period.
				//
				// but think .LargeChange ...
				//
				// - not that that helps since gettin' .net scrollbars to behave
				// properly for anything nontrivial is so fuckin' frustratin'.
				//
				// Don't even try setting .LargeChange to HeightRow in this
				// funct since it won't stick if the scrollbar is not visible.

				if (_visVert)
				{
					// NOTE: Do not set Maximum until after deciding whether
					// or not max < 0. 'Cause it fucks everything up. bingo.
					int vert = HeightTable - Height
							 + (HeightRow - 1)
							 + (_visHori ? _scrollHori.Height : 0);

					if (vert < HeightRow)
					{
						_scrollVert.Maximum = MaxVert = 0; // TODO: Perhaps that should zero the Value and recurse.
					}
					else
					{
						MaxVert = (_scrollVert.Maximum = vert) - (HeightRow - 1);

						// handle .NET OnResize anomaly ->
						// keep the bottom of the table snuggled against the bottom
						// of the visible area when resize enlarges the area
						if (HeightTable < Height + OffsetVert - (_visHori ? _scrollHori.Height : 0))
						{
							_scrollVert.Value = MaxVert;
						}
					}
				}
				else
					_scrollVert.Value = _scrollVert.Maximum = MaxVert = 0;

				if (_visHori)
				{
					// NOTE: Do not set Maximum until after deciding whether
					// or not max < 0. 'Cause it fucks everything up. bingo.
					int hori = WidthTable - Width
							 + (HeightRow - 1)
							 + (_visVert ? _scrollVert.Width : 0);

					if (hori < HeightRow)
					{
						_scrollHori.Maximum = MaxHori = 0; // TODO: Perhaps that should zero the Value and recurse.
					}
					else
					{
						MaxHori = (_scrollHori.Maximum = hori) - (HeightRow - 1);

						// handle .NET OnResize anomaly ->
						// keep the right of the table snuggled against the right of
						// the visible area when resize enlarges the area
						if (WidthTable < Width + OffsetHori - (_visVert ? _scrollVert.Width : 0))
							_scrollHori.Value = MaxHori;
					}
				}
				else
					_scrollHori.Value = _scrollHori.Maximum = MaxHori = 0;
			}
		}

		/// <summary>
		/// Scrolls the table by the mousewheel.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Fired from the form's <c>MouseWheel</c> event to catch all
		/// unhandled <c>MouseWheel</c> events hovered on the app (without
		/// firing twice).</remarks>
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
							h = HeightRow;

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
							w = HeightRow;

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
		const int LINE_VERSION = 0;
		const int LINE_DEFAULT = 1;
		const int LINE_COLABEL = 2;

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
		/// <returns>a <c>LOADRESULT_*</c> val
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
				return ib.ShowDialog(YataForm.that);
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
		#endregion Load


		#region Init
		/// <summary>
		/// Initializes a created or reloaded table.
		/// </summary>
		/// <param name="changed"></param>
		/// <param name="reload"></param>
		internal void Init(bool changed = false, bool reload = false)
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
				}

				if (Info != InfoType.INFO_NONE)
				{
					foreach (var dir in Settings._pathall)
						_f.GropeLabels(dir);
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
		/// <param name="rewidthOnly"><c>true</c> to only re-width cols - ie.
		/// Font changed</param>
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

				Cols[c].width(widthtext + _padHori * 2 + _padHoriSort, rewidthOnly);
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
					fields[i] = InputDialog._colabel;

					var col = new Col();
					col.text = InputDialog._colabel;
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
			Fields[selc - 1] = InputDialog._colabel; // the Field-count is 1 less than the col-count
			Cols[selc]._widthtext = YataGraphics.MeasureWidth((Cols[selc].text = InputDialog._colabel),
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
		/// <param name="copyc"></param>
		internal void PasteCol(IList<string> copyc)
		{
			// TODO: do sync-table

			int selc = getSelectedCol(); // 'selc' shall be valid here.

			ClearSelects(true);

			Cols[selc].selected = true;

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

			if ((e.Modifiers & Keys.Alt) == 0)
			{
				bool ctr = (e.Modifiers & Keys.Control) != 0,
					 sft = (e.Modifiers & Keys.Shift)   != 0;

				int invalid  = INVALID_NONE;
				bool display = false;
				bool anchor  = false;

				Cell sel = getSelectedCell();
				int selr = getSelectedRow();

				// TODO: change selected col perhaps

				YataGrid table; // sync-table

				switch (e.KeyCode)
				{
					case Keys.Home:
						if (selr != -1)
						{
							if (!sft)
							{
								if (ctr)
								{
									selr = row_SelectRow(selr, 0);
									invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
								}
								else if (_visHori) _scrollHori.Value = 0;
							}
							else if (!ctr)
							{
								RangeSelect = -selr;
								row_SelectRangeCells(0, selr);

								if ((table = getSynctable()) != null)
								{
									if (selr < table.RowCount)
									{
										table.RangeSelect = -selr;
										table.row_SelectRangeCells(0, selr);
									}
									else
										table.RangeSelect = 0;
								}
								selr = 0;
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
						}
						else if (sft)
						{
							if (allowContiguous() && areSelectedCellsContiguous())
							{
								if (ctr)
								{
									ClearCellSelects();
									SelectCellBlock(0,           _anchorcell.y,
													FrozenCount, _anchorcell.x);

									sel = this[0, FrozenCount];
								}
								else
								{
									int strt_r, stop_r;
									int sel_r = asStartStop_row(out strt_r, out stop_r);

									ClearCellSelects();
									SelectCellBlock(strt_r,      stop_r,
													FrozenCount, _anchorcell.x);

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
									selr = row_SelectRow(selr, RowCount - 1);
									invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
								}
								else if (_visHori) _scrollHori.Value = MaxHori;
							}
							else if (!ctr)
							{
								RangeSelect = RowCount - selr - 1;
								row_SelectRangeCells(selr, RowCount - 1);

								if ((table = getSynctable()) != null)
								{
									if (selr < table.RowCount)
									{
										table.setRangeSelect(selr, RangeSelect);
										table.row_SelectRangeCells(selr, selr + table.RangeSelect);
									}
									else
										table.RangeSelect = 0;
								}
								selr = RowCount - 1;
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
						}
						else if (sft)
						{
							if (allowContiguous() && areSelectedCellsContiguous())
							{
								if (ctr)
								{
									ClearCellSelects();
									SelectCellBlock(_anchorcell.y, RowCount - 1,
													_anchorcell.x, ColCount - 1);

									sel = this[RowCount - 1, ColCount - 1];
								}
								else
								{
									int strt_r, stop_r;
									int sel_r = asStartStop_row(out strt_r, out stop_r);

									ClearCellSelects();
									SelectCellBlock(strt_r,        stop_r,
													_anchorcell.x, ColCount - 1);

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
							if (!ctr)
							{
								if (!sft)
								{
									int r = selr;
									if (r != 0)
									{
										int shift = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;
										if ((r -= shift) < 0) r = 0;
									}
									selr = row_SelectRow(selr, r);
								}
								else
								{
									int range = -(Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;
										range += RangeSelect;

									if (selr + range < 0) RangeSelect = 0 - selr;
									else                  RangeSelect = range;

									int strt_r, stop_r;
									asStartStop_range(selr, out strt_r, out stop_r);
									row_SelectRangeCells(strt_r, stop_r);

									if ((table = getSynctable()) != null)
									{
										if (selr < table.RowCount)
										{
											table.setRangeSelect(selr, RangeSelect);

											table.asStartStop_range(selr, out strt_r, out stop_r);
											table.row_SelectRangeCells(strt_r, stop_r);
										}
										else
											table.RangeSelect = 0;
									}
									selr += RangeSelect;
								}
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
						}
						else if (sft)
						{
							if (allowContiguous() && areSelectedCellsContiguous())
							{
								if (ctr)
								{
									int strt_c, stop_c;
									int sel_c = asStartStop_col(out strt_c, out stop_c);

									ClearCellSelects();
									SelectCellBlock(0,      _anchorcell.y,
													strt_c, stop_c);

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
										SelectCellBlock(strt_r, stop_r,
														strt_c, stop_c);

										sel = this[sel_r, sel_c];
										anchor = true;
									}
								}
							}
						}
//						else if (ctr)
//						{
							// don't use [Ctrl+PageUp] since it is used/consumed by the tabcontrol
							//
							// Note that can be bypassed in YataTabs.OnKeyDown() -
							// which is how [Ctrl+Shift+PageUp/Down] work for selecting contiguous blocks.
							// but I'd prefer to keep [Ctrl+PageUp/Down] for actually changing tabpages
//						}
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
							if (!ctr)
							{
								if (!sft)
								{
									int r = selr;
									if (r != RowCount - 1)
									{
										int shift = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;
										if ((r += shift) > RowCount - 1) r = RowCount - 1;
									}
									selr = row_SelectRow(selr, r);
								}
								else
								{
									int range = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;
										range += RangeSelect;

									if (selr + range >= RowCount) RangeSelect = RowCount - selr - 1;
									else                          RangeSelect = range;

									int strt_r, stop_r;
									asStartStop_range(selr, out strt_r, out stop_r);
									row_SelectRangeCells(strt_r, stop_r);

									if ((table = getSynctable()) != null)
									{
										if (selr < table.RowCount)
										{
											table.setRangeSelect(selr, RangeSelect);

											table.asStartStop_range(selr, out strt_r, out stop_r);
											table.row_SelectRangeCells(strt_r, stop_r);
										}
										else
											table.RangeSelect = 0;
									}
									selr += RangeSelect;
								}
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
						}
						else if (sft)
						{
							if (allowContiguous() && areSelectedCellsContiguous())
							{
								if (ctr)
								{
									int strt_c, stop_c;
									int sel_c = asStartStop_col(out strt_c, out stop_c);

									ClearCellSelects();
									SelectCellBlock(_anchorcell.y, RowCount - 1,
													strt_c,        stop_c);

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
										SelectCellBlock(strt_r, stop_r,
														strt_c, stop_c);

										sel = this[sel_r, sel_c];
										anchor = true;
									}
								}
							}
						}
//						else if (ctr)
//						{
							// don't use [Ctrl+PageDown] since it is used/consumed by the tabcontrol
							//
							// Note that can be bypassed in YataTabs.OnKeyDown() -
							// which is how [Ctrl+Shift+PageUp/Down] work for selecting contiguous blocks.
							// but I'd prefer to keep [Ctrl+PageUp/Down] for actually changing tabpages
//						}
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
							if (!ctr)
							{
								if (!sft)
								{
									selr = row_SelectRow(selr, Math.Max(0, selr - 1));
								}
								else
								{
									if (selr + RangeSelect != 0) --RangeSelect;

									int strt_r, stop_r;
									asStartStop_range(selr, out strt_r, out stop_r);
									row_SelectRangeCells(strt_r, stop_r);

									if ((table = getSynctable()) != null)
									{
										if (selr < table.RowCount)
										{
											table.setRangeSelect(selr, RangeSelect);

											table.asStartStop_range(selr, out strt_r, out stop_r);
											table.row_SelectRangeCells(strt_r, stop_r);
										}
										else
											table.RangeSelect = 0;
									}
									selr += RangeSelect;
								}
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
						}
						else if (sft)
						{
							if (!ctr && allowContiguous() && areSelectedCellsContiguous())
							{
								int sel_r = getAnchorRangedRowid() - 1;
								if (sel_r >= 0)
								{
									int strt_r = Math.Min(sel_r, _anchorcell.y);
									int stop_r = Math.Max(sel_r, _anchorcell.y);

									int strt_c, stop_c;
									int sel_c = asStartStop_col(out strt_c, out stop_c);

									ClearCellSelects();
									SelectCellBlock(strt_r, stop_r,
													strt_c, stop_c);

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
							if (_scrollVert.Value - HeightRow < 0)
								_scrollVert.Value = 0;
							else
								_scrollVert.Value -= HeightRow;
						}
						break;

					case Keys.Down: // NOTE: needs to bypass KeyPreview
						if (selr != -1)
						{
							if (!ctr)
							{
								if (!sft)
								{
									selr = row_SelectRow(selr, Math.Min(RowCount - 1, selr + 1));
								}
								else
								{
									if (selr + RangeSelect != RowCount - 1) ++RangeSelect;

									int strt_r, stop_r;
									asStartStop_range(selr, out strt_r, out stop_r);
									row_SelectRangeCells(strt_r, stop_r);

									if ((table = getSynctable()) != null)
									{
										if (selr < table.RowCount)
										{
											table.setRangeSelect(selr, RangeSelect);

											table.asStartStop_range(selr, out strt_r, out stop_r);
											table.row_SelectRangeCells(strt_r, stop_r);
										}
										else
											table.RangeSelect = 0;
									}
									selr += RangeSelect;
								}
								invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
							}
						}
						else if (sft)
						{
							if (!ctr && allowContiguous() && areSelectedCellsContiguous())
							{
								int sel_r = getAnchorRangedRowid() + 1;
								if (sel_r < RowCount)
								{
									int strt_r = Math.Min(sel_r, _anchorcell.y);
									int stop_r = Math.Max(sel_r, _anchorcell.y);

									int strt_c, stop_c;
									int sel_c = asStartStop_col(out strt_c, out stop_c);

									ClearCellSelects();
									SelectCellBlock(strt_r, stop_r,
													strt_c, stop_c);

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
							if (_scrollVert.Value + HeightRow > MaxVert)
								_scrollVert.Value = MaxVert;
							else
								_scrollVert.Value += HeightRow;
						}
						break;

					case Keys.Left: // NOTE: needs to bypass KeyPreview
						if (!ctr)
						{
							if (sft)
							{
//								if (sel != null)
//								{
//									if (sel.x > FrozenCount)
//									{
//										sel.selected = false;
//
//										int w = Width - getLeft() - (_visVert ? _scrollVert.Width : 0);
//										var pt = getColBounds(sel.x);
//										pt.X += OffsetHori - w;
//
//										int c = -1, tally = 0;
//										while ((tally += Cols[++c].width()) < pt.X)
//										{}
//
//										if (++c >= sel.x)
//											c = sel.x - 1;
//
//										if (c < FrozenCount)
//											c = FrozenCount;
//
//										(sel = this[sel.y, c]).selected = true;
//									}
//									display = true;
//								}

								if (selr == -1 && areSelectedCellsContiguous())
								{
									if (allowContiguous())
									{
										int sel_c = getAnchorRangedColid() - 1;
										if (sel_c >= FrozenCount)
										{
											int strt_c = Math.Min(sel_c, _anchorcell.x);
											int stop_c = Math.Max(sel_c, _anchorcell.x);

											int strt_r, stop_r;
											int sel_r = asStartStop_row(out strt_r, out stop_r);

											ClearCellSelects();
											SelectCellBlock(strt_r, stop_r,
															strt_c, stop_c);

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
								if (_scrollHori.Value - HeightRow < 0)
									_scrollHori.Value = 0;
								else
									_scrollHori.Value -= HeightRow;
							}
						}
						break;

					case Keys.Right: // NOTE: needs to bypass KeyPreview
						if (!ctr)
						{
							if (sft)
							{
//								if (sel != null)
//								{
//									if (sel.x != ColCount - 1)
//									{
//										sel.selected = false;
//
//										int w = Width - getLeft() - (_visVert ? _scrollVert.Width : 0);
//										var pt = getColBounds(sel.x);
//										pt.X += OffsetHori + w;
//
//										int c = -1, tally = 0;
//										while (++c != ColCount && (tally += Cols[c].width()) < pt.X)
//										{}
//
//										if (--c <= sel.x)
//											c = sel.x + 1;
//
//										if (c > ColCount - 1)
//											c = ColCount - 1;
//
//										(sel = this[sel.y, c]).selected = true;
//									}
//									display = true;
//								}

								if (selr == -1 && areSelectedCellsContiguous())
								{
									if (allowContiguous())
									{
										int sel_c = getAnchorRangedColid() + 1;
										if (sel_c < ColCount)
										{
											int strt_c = Math.Min(sel_c, _anchorcell.x);
											int stop_c = Math.Max(sel_c, _anchorcell.x);

											int strt_r, stop_r;
											int sel_r = asStartStop_row(out strt_r, out stop_r);

											ClearCellSelects();
											SelectCellBlock(strt_r, stop_r,
															strt_c, stop_c);

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
								if (_scrollHori.Value + HeightRow > MaxHori)
									_scrollHori.Value = MaxHori;
								else
									_scrollHori.Value += HeightRow;
							}
						}
						break;

					case Keys.Escape: // NOTE: needs to bypass KeyPreview
						if (!ctr && !sft)
						{
							ClearSelects(true);
							_f.ClearSyncSelects();

							selr = -1;
							invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
						}
						break;
				}

				if (invalid != INVALID_NONE)	// -> is a Row operation or [Esc]
				{
					if (selr != -1) EnsureDisplayedRow(selr);

					if (Propanel != null && Propanel.Visible)
						invalid |= INVALID_PROP;

					Invalidator(invalid);
				}
				else if (display)				// -> is a Cell operation
				{
					_anchorcell = sel;

					_f.SyncSelectCell(sel);
					_f.EnableCelleditOperations();

					Invalidator(INVALID_GRID
							  | INVALID_FROZ
							  | EnsureDisplayed(sel));
				}
				else if (anchor)				// -> is a block-select operation
				{
					_f.EnableCelleditOperations();

					Invalidator(INVALID_GRID
							  | EnsureDisplayed(sel));
				}
			}
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


			YataGrid table = _f.ClearSyncSelects();
			if (table != null && r < table.RowCount)
			{
				row = table.Rows[r];

				Row._bypassEnableRowedit = true;
				row.selected = true;
				Row._bypassEnableRowedit = false;

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
		/// <c><see cref="YataForm"/>.OnKeyDown()</c> <c>[Ctrl+Space]</c>.</remarks>
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


		/// <summary>
		/// Handles <c>KeyDown</c> events for the <c><see cref="_editor"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="_editor"/></c></param>
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
		/// <param name="sender"><c><see cref="_editor"/></c></param>
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
		/// <c>[Ctrl+PageUp/PageDown]</c>. Lovely /explode - can be fixed in
		/// <c><see cref="YataForm"/>.tab_SelectedIndexChanged()</c>.</remarks>
		protected override void OnLeave(EventArgs e)
		{
			if (_editor.Visible)
			{
				_editor.Visible = false;
				Invalidator(INVALID_GRID);
			}
		}

		/// <summary>
		/// Processes a so-called dialog-key.
		/// <list type="bullet">
		/// <item><c>[Enter]</c> - starts or accepts celledit</item>
		/// <item><c>[Escape]</c> - cancels celledit</item>
		/// <item><c>[Tab]</c> - fastedit right</item>
		/// <item><c>[Tab+Shift]</c> - fastedit left</item>
		/// <item><c>[Tab+Ctrl]</c>/<c>[Down]</c> - fastedit down</item>
		/// <item><c>[Tab+Ctrl+Shift]</c>/<c>[Up]</c> - fastedit up</item>
		/// </list></summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		/// <remarks><c>[Down]</c> and <c>[Up]</c> require bypassing those keys
		/// in <c><see cref="YataEditbox"/>.IsInputKey()</c>.</remarks>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			//logfile.Log("YataGrid.ProcessDialogKey() keyData= " + keyData);

			switch (keyData)
			{
				case Keys.Enter:
					if (_editor.Visible)
					{
						ApplyCellEdit();
					}
					else if (!Readonly
						&& (_editcell = getSelectedCell()) != null
						&& _editcell.x >= FrozenCount)
					{
						Celledit();
					}
					return true;

				case Keys.Escape:
					hideditor(INVALID_GRID);
					return true;


				// Tab fastedit ->
				case Keys.Tab:
					if (_editor.Visible)
					{
						ApplyCellEdit();

						if (_editcell.x != ColCount - 1)
							startTabedit(+1,0);
					}
					return true;

				case Keys.Tab | Keys.Shift:
					if (_editor.Visible)
					{
						ApplyCellEdit();

						if (_editcell.x != FrozenCount)
							startTabedit(-1,0);
					}
					return true;

				case Keys.Tab | Keys.Control:
				case Keys.Down:
					if (_editor.Visible)
					{
						ApplyCellEdit();

						if (_editcell.y != RowCount - 1)
							startTabedit(0,+1);
					}
					return true; // stop the tabcontrol from responding to [Ctrl+Tab]

				case Keys.Tab | Keys.Control | Keys.Shift:
				case Keys.Up:
					if (_editor.Visible)
					{
						ApplyCellEdit();

						if (_editcell.y != 0)
							startTabedit(0,-1);
					}
					return true; // stop the tabcontrol from responding to [Ctrl+Shift+Tab]
			}
			return base.ProcessDialogKey(keyData);
		}

		/// <summary>
		/// Applies a cell-edit via the <c><see cref="_editor"/></c>.
		/// </summary>
		void ApplyCellEdit()
		{
			int invalid = INVALID_GRID;

			if (_editor.Text != _editcell.text)
			{
				ChangeCellText(_editcell, _editor); // does a text-check

				if (Propanel != null && Propanel.Visible)
					invalid |= INVALID_PROP;
			}
			else if (_editcell.loadchanged)
				ClearLoadchanged(_editcell);

			hideditor(invalid);
		}

		/// <summary>
		/// Hides <c><see cref="_editor"/></c> and focuses this <c>YataGrid</c>.
		/// </summary>
		/// <param name="invalid"></param>
		void hideditor(int invalid)
		{
			_editor.Visible = false;
			Invalidator(invalid);
			Select();
		}

		/// <summary>
		/// Moves cell selection and calls
		/// <c><see cref="Celledit()">Celledit()</see></c> for the next cell in
		/// a Tabfastedit sequence.
		/// </summary>
		/// <param name="dir_hori"></param>
		/// <param name="dir_vert"></param>
		void startTabedit(int dir_hori, int dir_vert)
		{
			this[_editcell.y,
				 _editcell.x].selected = false;

			(_editcell = this[_editcell.y + dir_vert,
							  _editcell.x + dir_hori]).selected = true;

			Celledit();

			_f.SyncSelectCell(_anchorcell = _editcell);
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

			if (VerifyText_edit(tb))
			{
				using (var ib = new Infobox(Infobox.Title_warni,
											"The text that was submitted has been altered.",
											null,
											InfoboxType.Warn))
				{
					ib.ShowDialog(_f);
				}
			}
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
		/// Checks the cell-field text during user-edits.
		/// </summary>
		/// <param name="tb">a <c>TextBox</c> in which user is editing the text</param>
		/// <returns><c>true</c> to notify user if text changes</returns>
		internal static bool VerifyText_edit(Control tb)
		{
			string text = tb.Text; // allow whitespace - do not Trim()
			if (text.Length == 0)
			{
				tb.Text = gs.Stars;
				return false; // NOTE: Don't bother the user if he/she simply wants to blank a field.
			}

			bool changed = VerifyText(ref text);

			tb.Text = text;
			return changed;
		}

		/// <summary>
		/// Checks a cell-field text. Called during file load by
		/// <c><see cref="CreateRows()">CreateRows()</see></c> or after
		/// user-edits by
		/// <c><see cref="VerifyText_edit()">VerifyText_edit()</see></c>.
		/// </summary>
		/// <param name="text">ref to a text-string</param>
		/// <param name="load"><c>true</c> to return <c>true</c> even if a
		/// text-change is insignificant</param>
		/// <returns><c>true</c> if (a) text is changed and user should be
		/// notified or (b) text is changed and its <c><see cref="Cell"/></c>
		/// should be flagged as <c><see cref="Cell.loadchanged"/></c> when a
		/// 2da-file loads</returns>
		static bool VerifyText(ref string text, bool load = false)
		{
			var sb = new StringBuilder(text);

			if (sb.Length != 0
				&& sb[0] != '"' && sb[sb.Length - 1] != '"')
			{
				bool quotes = false;

				char c;
				for (int i = 0; i != sb.Length; ++i)
				{
					c = sb[i];
					if (c == '"')
					{
						quotes = false;
						break;
					}

					if (Char.IsWhiteSpace(c))
						quotes = true;
				}

				if (quotes)
				{
					sb.Insert(0, '"');
					sb.Append('"');

					text = sb.ToString();

					if (load)
						return true;

					return Settings._strict;	// NOTE: Bother the user if he/she
				}								// wants auto-quotes only if Strict.
			}

			sb = sb.Replace("\"", null);

			if (sb.Length == 0)
			{
				text = gs.Stars;
				return true;
			}

			for (int i = 0; i != sb.Length; ++i)
			{
				if (Char.IsWhiteSpace(sb[i]))
				{
					sb.Insert(0, '"');
					sb.Append('"');

					text = sb.ToString();
					return true;
				}
			}

			string verified = sb.ToString();
			bool changed = verified != text;
			text = verified;

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
					bool enablecelledit = false;

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

											ApplyCellEdit();
										}
										else					// NOTE: There's a clickable fringe around the editor so
											_editor.Focus();	//       just refocus the editor if the fringe is clicked.
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
												EnsureDisplayed(_cell, (getSelectedCell() == null));	// <- bypass Propanel.EnsureDisplayed() if
																										//    selectedcell is not the only selected cell
												_anchorcell = _cell;
											}
											else
												_f.ClearSyncSelects();

											enablecelledit = true;

											int invalid = INVALID_GRID;
											if (Propanel != null && Propanel.Visible)
												invalid |= INVALID_PROP;

											Invalidator(invalid);
										}
									}
									else if (sft) // do block selection ->
									{
										if (this != _f._diff1 && this != _f._diff2	// disallow multi-cell select if sync'd
											&& _cell != getSelectedCell()			// else do nothing if clicked cell is the only selected cell
											&& (_anchorcell != null || (_anchorcell = getFirstSelectedCell(FrozenCount)) != null)
											&& areSelectedCellsContiguous())		// else do nothing if no cells are selected or selected cells are not in a contiguous block
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

											enablecelledit = true;

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

										enablecelledit = true;

										Invalidator(INVALID_GRID
												  | INVALID_FROZ
												  | INVALID_ROWS
												  | EnsureDisplayed(_cell));
									}
									else if (!Readonly) // cell is already selected
									{
										_editcell = _cell;
										Celledit();
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
									_anchorcell = _cell;

									ClearSelects(true);
									_cell.selected = true;
									_f.SyncSelectCell(_cell);

									enablecelledit = true;

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

					if (enablecelledit)
						_f.EnableCelleditOperations();
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
			y += OffsetVert;
			if (y > HeightColhead && y < HeightTable)
			{
				x += OffsetHori;
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
		/// Starts a cell-edit from YataForm via the cellmenu.
		/// </summary>
		/// <param name="cell"></param>
		internal void startCelledit(Cell cell)
		{
			_editcell = cell;
			Celledit();
		}

		/// <summary>
		/// Clears all <c><see cref="Cell">Cells</see></c> that are currently
		/// selected.
		/// </summary>
		/// <param name="bypassCol"><c>true</c> to not clear selected col-cells</param>
		/// <remarks>The caller shall call
		/// <c><see cref="YataForm.EnableCelleditOperations()">YataForm.EnableCelleditOperations()</see></c>
		/// after it deters required cell-selects.</remarks>
		internal void ClearCellSelects(bool bypassCol = false)
		{
//			_anchorcell = null; // ~safety. Would need to go through all select patterns.

			foreach (var row in Rows)
			for (int c = 0; c != ColCount; ++c)
			if (!bypassCol || !Cols[c].selected)
				row[c].selected = false;
		}

		/// <summary>
		/// Clears all <c><see cref="Cell">Cells</see></c>/
		/// <c><see cref="Row">Rows</see></c>/
		/// <c><see cref="Col">Cols</see></c> that are currently selected.
		/// </summary>
		/// <param name="bypassEnableCelledit"><c>true</c> to bypass
		/// <c><see cref="YataForm.EnableCelleditOperations()">YataForm.EnableCelleditOperations()</see></c>
		/// - this typically means that the caller shall select a <c>Cell</c>
		/// and call <c>EnableCelleditOperations()</c> itself or that selects
		/// are being cleared from a synced <c><see cref="YataGrid"/></c></param>
		/// <param name="bypassEnableRowedit"><c>true</c> to bypass
		/// <c><see cref="YataForm.EnableRoweditOperations()">YataForm.EnableRoweditOperations()</see></c>
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
					Row._bypassEnableRowedit = bypassEnableRowedit;
					row.selected = false;
					Row._bypassEnableRowedit = false;
				}

				for (int c = 0; c != ColCount; ++c)
					row[c].selected = false;
			}

			RangeSelect = 0; // otherwise Undoing the creation of a row-array leaves RangeSelect wonky.

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
					int y = e.Y + OffsetVert;
					if (y > HeightColhead && y < HeightTable)
					{
						int x = e.X + OffsetHori;
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
			while ((l += Cols[cords.X].width()) < x)
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

			rect.X = WidthRowhead - OffsetHori;
			for (int c = 0; c != cell.x; ++c)
				rect.X += Cols[c].width();

			rect.Y = HeightColhead - OffsetVert;
			for (int r = 0; r != cell.y; ++r)
				rect.Y += HeightRow;

			rect.Width = Cols[cell.x].width();
			rect.Height = HeightRow;

			return rect;
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
		/// <c><see cref="Propanel"/></c> considerations</param>
		/// <returns>a bitwise <c>int</c> defining controls that need to be
		/// invalidated; note that the <c>Propanel's</c> invalidation bit will
		/// be flagged as long as the panel is visible regardless of whether it
		/// really needs to be redrawn</returns>
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

			int selr = getSelectedRow();
			if (selr != -1)
				return EnsureDisplayedRow(selr);

			return INVALID_NONE;
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
				posL += Cols[c].width();

			int posR = posL + Cols[colid].width();

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
		/// Performs a goto when the <c>[Enter]</c> key is pressed and focus is
		/// on the goto-box.
		/// </summary>
		/// <param name="text">a <c>string</c> to parse for a row-id</param>
		/// <param name="selectTable"><c>true</c> to <c>Select()</c> this
		/// <c>YataGrid</c></param>
		internal void doGoto(string text, bool selectTable)
		{
			int selr;
			if (Int32.TryParse(text, out selr)
				&& selr > -1 && selr < RowCount)
			{
				_editor.Visible = false;
				ClearSelects();

				SelectRow(selr);
				EnsureDisplayedRow(selr);

				int invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
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

				int click_r = (e.Y + OffsetVert) / HeightRow;
				if (click_r < RowCount)
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
									if (r != click_r && c != selc
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
										if (r != click_r && c != selcsync)
											table[r,c].selected = false;
									}
								}
							}


							Row._bypassEnableRowedit = true; // call _f.EnableRoweditOperations() later ...

							bool display = false; // deter col for ensure displayed col

							int selr = getSelectedRow();

							if ((ModifierKeys & Keys.Shift) == 0) // select only the clicked row ->
							{
								if (selr != -1 && selr != click_r) // clear any other selected row ->
									Rows[selr].selected = false;

								if (table != null)
								{
									int selrsync = table.getSelectedRow();
									if (selrsync != -1 && selrsync != click_r)
										table.Rows[selrsync].selected = false;
								}


								bool allcellsselected = true;
								foreach (var cell in Rows[click_r]._cells)
								if (!cell.selected)
								{
									allcellsselected = false;
									break;
								}

								// select the clicked row if
								// (a) it is not currently selected
								// (b) or not all cells in the clicked row are currently selected
								// (c) or cells not in the clicked row got deselected above
								bool @select = !Rows[click_r].selected || !allcellsselected || celldeselected;

								if ((Rows[click_r].selected = @select) && FrozenCount < ColCount)
									_anchorcell = this[click_r, FrozenCount];

								for (int c = 0; c != ColCount; ++c) // select or deselect cells in the clicked row ->
								{
									if (@select || c != selc)
										this[click_r,c].selected = @select;
								}

								if (table != null && click_r < table.RowCount) // select or deselect cells in a sync-table's row ->
								{
									table.Rows[click_r].selected = @select;

									for (int c = 0; c != table.ColCount; ++c)
									{
										if (@select || c != selcsync)
											table[click_r,c].selected = @select;
									}
								}

								display = @select;
							}
							else if (selr != -1) // subselect a range of rows ->
							{
								RangeSelect = click_r - selr;
								if (RangeSelect != 0)
								{
									display = true;

									int start, stop;
									if (RangeSelect > 0)
									{
										start = selr; stop = click_r;
									}
									else
									{
										start = click_r; stop = selr;
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

							if (display) EnsureDisplayedRow(click_r);

							int invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
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

								SelectRow(click_r);
								EnsureDisplayedRow(click_r);

								int invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
								if (Propanel != null && Propanel.Visible)
									invalid |= INVALID_PROP;

								Invalidator(invalid);

								_f.ShowRowContext(click_r);
							}
							break;
					}
				}
				else if (ModifierKeys == Keys.None) // click below the last entry ->
				{
					ClearSelects();
					_f.ClearSyncSelects();

					int invalid = INVALID_GRID | INVALID_FROZ | INVALID_ROWS;
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
			if (Rows[r].selected)
				return r;

			return -1;
		}

		/// <summary>
		/// Gets the row-id of the currently selected <c><see cref="Row"/></c>
		/// or a row-id that has <c><see cref="Cell">Cells</see></c> selected
		/// iff only that <c>Row</c> has <c>Cells</c> selected.
		/// </summary>
		/// <returns>the currently selected row-id or the row-id that has
		/// selected <c>Cells</c>; <c>-1</c> if no <c>Row</c> is applicable</returns>
		internal int getSelectedRowOrCells()
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

				int click_c;
				switch (e.Button)
				{
					case MouseButtons.Left:
						if ((click_c = getClickedCol(e.X)) != -1)
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
								for (int r = 0; r != RowCount; ++r)
								for (int c = 0; c != ColCount; ++c)
								{
									if (c != click_c
										&& (   (r > selr && r > selr + RangeSelect)
											|| (r < selr && r < selr + RangeSelect))
										&& this[r,c].selected)
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
										if (c != click_c
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
								if (selc != -1 && selc != click_c) // clear any other selected col ->
									Cols[selc].selected = false;

								if (table != null)
								{
									int selcsync = table.getSelectedCol();
									if (selcsync != -1 && selcsync != click_c)
										table.Cols[selcsync].selected = false;
								}


								bool allcellsselected = true;
								foreach (var row in Rows)
								if (!row[click_c].selected)
								{
									allcellsselected = false;
									break;
								}

								// select the clicked col if
								// (a) it is not currently selected
								// (b) or not all cells in the clicked col are currently selected
								// (c) or cells not in the clicked col got deselected above
								bool @select = !Cols[click_c].selected || !allcellsselected || celldeselected;

								if (Cols[click_c].selected = @select)
									_anchorcell = this[0, click_c];

								for (int r = 0; r != RowCount; ++r) // select or deselect cells in the clicked col ->
								{
									if (@select
										|| (r > selr && r > selr + RangeSelect)
										|| (r < selr && r < selr + RangeSelect))
									{
										this[r,click_c].selected = @select;
									}
								}

								if (table != null && click_c < table.ColCount) // select or deselect cells in a sync-table's col ->
								{
									table.Cols[click_c].selected = @select;

									for (int r = 0; r != table.RowCount; ++r)
									{
										if (@select
											|| (r > selrsync && r > selrsync + table.RangeSelect)
											|| (r < selrsync && r < selrsync + table.RangeSelect))
										{
											table[r,click_c].selected = @select;
										}
									}
								}

								display = @select;
							}
							else if (selc != -1) // subselect a range of cols ->
							{
								int delta = click_c - selc;
								if (delta != 0)
								{
									display = true;

									int start, stop;
									if (delta > 0)
									{
										start = selc; stop = click_c;
									}
									else
									{
										start = click_c; stop = selc;
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


							if (display) EnsureDisplayedCol(click_c);

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
							if ((click_c = getClickedCol(e.X)) != -1)
							{
								ColSort(click_c);
								EnsureDisplayedCol(click_c);
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
			x += OffsetHori;

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
		/// <param name="rowid">row-id to insert at</param>
		/// <param name="fields">an array of fields</param>
		/// <param name="calibrate"><c>true</c> to re-layout the grid or
		/// <c>false</c> if <c><see cref="Calibrate()">Calibrate()</see></c>
		/// will be done by the caller</param>
		/// <param name="brush">a <c>Brush</c> to use for Undo/Redo</param>
		internal void Insert(int      rowid,
							 string[] fields,
							 bool     calibrate = true,
							 Brush    brush     = null)
		{
			if (calibrate)
				DrawRegulator.SuspendDrawing(this);

			if (fields != null)
			{
				if (brush == null)
					brush = Brushes.Created;

				var row = new Row(rowid, ColCount, brush, this);

				string field;
				for (int c = 0; c != ColCount; ++c)
				{
					if (c < fields.Length)
						field = fields[c];
					else
						field = gs.Stars;

					row[c] = new Cell(rowid, c, field);
				}

				Rows.Insert(rowid, row);
				++RowCount;

				for (int r = rowid + 1; r != RowCount; ++r) // straighten out row._id and cell.y ->
				{
					++(row = Rows[r])._id;
					for (int c = 0; c != ColCount; ++c)
						++row[c].y;
				}
			}

			if (calibrate) // is only 1 row (no range) via context single-row edit
			{
				Calibrate(rowid);

				if (rowid < RowCount)
					EnsureDisplayedRow(rowid);

				DrawRegulator.ResumeDrawing(this);
			}
		}

		/// <summary>
		/// Deletes a row from the table.
		/// </summary>
		/// <param name="idr">row-id to delete</param>
		/// <param name="calibrate"><c>true</c> to re-layout the grid or
		/// <c>false</c> if <c><see cref="Calibrate()">Calibrate()</see></c>
		/// will be done by the caller</param>
		internal void Delete(int idr, bool calibrate = true)
		{
			if (calibrate)
				DrawRegulator.SuspendDrawing(this);

			Row row;

			Rows.Remove(Rows[idr]);
			--RowCount;

			for (int r = idr; r != RowCount; ++r) // straighten out row._id and cell.y ->
			{
				--(row = Rows[r])._id;
				for (int c = 0; c != ColCount; ++c)
					--row[c].y;
			}

			if (RowCount == 0) // add a row of stars so grid is not left blank ->
			{
				++RowCount;

				row = new Row(0, ColCount, Brushes.Created, this);

				int c = 0;
				if (Settings._autorder)
					row[c++] = new Cell(0,0, "0");

				for (; c != ColCount; ++c)
					row[c] = new Cell(0, c, gs.Stars);

				Rows.Add(row);

				if (calibrate)
				{
					Calibrate(0);
					DrawRegulator.ResumeDrawing(this);

					return;
				}
			}

			if (calibrate) // is only 1 row (no range) via context single-row edit
			{
				Calibrate();

				if (idr < RowCount)
					EnsureDisplayedRow(idr);

				DrawRegulator.ResumeDrawing(this);
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
			DrawRegulator.SuspendDrawing(this);


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


			DrawRegulator.ResumeDrawing(this);
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
			DrawRegulator.SuspendDrawing(_f);

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

			Sorter.TopDownMergeSort(Rows, _sortcol, _sortdir);


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

			DrawRegulator.ResumeDrawing(_f);
		}
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
