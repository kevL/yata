using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Loads, formats, and handles 2da-data as a table or grid on YataForm's
	/// tab-pages.
	/// </summary>
	sealed partial class YataGrid
		:
			Control
	{
		#region Enums
		internal enum InfoType
		{
			INFO_NONE,	// 0
			INFO_CRAFT,	// 1
			INFO_SPELL	// 2
		}
		#endregion Enums


		#region Fields (static)
		internal const int SORT_DES = -1;
		internal const int SORT_NOT =  0;
		internal const int SORT_ASC =  1;

		internal static int WidthRowhead;
		internal static int HeightColhead;

		internal static int HeightRow;

		/// <summary>
		/// Is used only for painting in all the various OnPaint events.
		/// </summary>
		internal static Graphics graphics;

		const int _padHori        =  6; // horizontal text padding in the table
		const int _padVert        =  4; // vertical text padding in the table and col/rowheads
		const int _padHoriRowhead =  8; // horizontal text padding for the rowheads
		const int _padHoriSort    = 12; // additional horizontal text padding to the right in the colheads for the sort-arrow

		const int _offsetHoriSort = 23; // horizontal offset for the sort-arrow
		const int _offsetVertSort = 15; // vertical offset for the sort-arrow

		static int _wid; // minimum width of a cell (ergo of a col if width of colhead-text is narrower)

		internal const int FreezeId     = 1; // qty of Cols that are frozen ->
		internal const int FreezeFirst  = 2;
		internal const int FreezeSecond = 3;

		/// <summary>
		/// Hides any info that's currently displayed on the statusbar when the
		/// cursor leaves the table-area. (The OnLeave event is unreliable.)
		/// </summary>
		static Timer _t1 = new Timer();

		/// <summary>
		/// A flag that stops presumptive .NET events from firing ~50 billion.
		/// </summary>
		internal static bool _init;
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

		internal bool _visVert; // Be happy. happy happy
		internal bool _visHori;

		internal int offsetVert;
		internal int offsetHori;

		int HeightTable;
		int WidthTable;

		YataPanelCols _panelCols;
		YataPanelRows _panelRows;

		YataPanelFrozen _panelFrozen;

		Label _labelid     = new Label();
		Label _labelfirst  = new Label();
		Label _labelsecond = new Label();

		/// <summary>
		/// The text-edit box. Note there is only one (1) editor that floats to
		/// wherever it's required.
		/// </summary>
		internal readonly TextBox _editor = new TextBox();

		/// <summary>
		/// The cell that's currently under the editbox.
		/// </summary>
		Cell _editcell;

		/// <summary>
		/// The currently sorted col. Default is #0 "id" col.
		/// </summary>
		internal int _sortcol;

		/// <summary>
		/// The current sort direction. Default is sorted ascending.
		/// </summary>
		internal int _sortdir = SORT_ASC;

		internal PropertyPanel _propanel;

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
				_f.ResetTabText(this);
			}
		}

		internal string[] Fields // 'Fields' does NOT contain #0 col IDs (so that typically needs +1)
		{ get; private set; }

		internal Cell this[int r, int c]
		{
			get { return Rows[r][c]; }
			set { Rows[r][c] = value; }
		}

		internal InfoType Info
		{ get; set; }

		int _frozenCount = FreezeId; // starts out w/ id-col only.
		internal int FrozenCount
		{
			get { return _frozenCount; }
			set
			{
				_frozenCount = value;

				int w = 0;
				for (int c = 0; c != _frozenCount; ++c)
					w += Cols[c].width();

				_panelFrozen.Width = w;

				_panelFrozen.Refresh();

				_labelfirst .Visible = (_frozenCount > FreezeId);
				_labelsecond.Visible = (_frozenCount > FreezeFirst);

				Cell sel = getSelectedCell();
				if (sel != null && sel.x < _frozenCount)
				{
					_editor.Visible =
					sel.selected = false;

					if (ColCount > _frozenCount)
						this[sel.y, _frozenCount].selected = true;

					Refresh();
				}

				_propanel.Refresh(); // update bg-color of PP fields
			}
		}

		/// <summary>
		/// The quantity of rows that are flagged for an operation excluding the
		/// currently selected row.
		/// @note The value will be negative if the range of subselected rows
		/// is above the currently selected row - else positive.
		/// </summary>
		internal int RangeSelect
		{ get; set; }

		internal FileWatcher Watcher
		{ get; private set; }
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor.
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
			_scrollVert.ValueChanged += OnVertScrollValueChanged;

			_scrollHori.Dock = DockStyle.Bottom;
			_scrollHori.ValueChanged += OnHoriScrollValueChanged;

			Controls.Add(_scrollHori);
			Controls.Add(_scrollVert);

			_t1.Interval = 223;
			_t1.Enabled = true; // TODO: stop Timer when no table is loaded /shrug.
			_t1.Tick += t1_Tick;

			_editor.Visible = false;
			_editor.BackColor = Colors.Editor;
			_editor.BorderStyle = BorderStyle.None;
			_editor.KeyDown += keydown_Editor;
			_editor.Leave   += leave_Editor;

			Controls.Add(_editor);

			Leave += leave_Grid;

			AllowDrop = true;
			DragEnter += grid_DragEnter;
			DragDrop  += grid_DragDrop;

			_ur = new UndoRedo(this);

			Watcher = new FileWatcher(this);
		}
		#endregion cTor


		void OnVertScrollValueChanged(object sender, EventArgs e)
		{
			if (_table == null) _table = this;

			_table._editor.Visible = false;

			_table.offsetVert = _table._scrollVert.Value;
			_table.Refresh();


			if (!_f._search)
				Select(); // workaround: refocus the table (bar has to move > 0px)

			var pt = PointToClient(Cursor.Position);
			var args = new MouseEventArgs(MouseButtons.Left, 1, pt.X, pt.Y, 0); // clicks,x,y,delta
			OnMouseMove(args); // update coords on the Statusbar


			if (_f._diff1 != null && _f._diff2 != null)
			{
				VScrollBar vert = null;

				if      (_f._diff1 == this) vert = _f._diff2._scrollVert;
				else if (_f._diff2 == this) vert = _f._diff1._scrollVert;

				if (vert != null && vert.Maximum != 0)
				{
					if (_scrollVert.Value < vert.Maximum - vert.LargeChange)
					{
						vert.Value = _scrollVert.Value;
					}
					else
						vert.Value = vert.Maximum - (vert.LargeChange - 1);

					Select();

					// NOTE: An interesting effect occurs if this is the longer table.
					// When [Ctrl+End] is keyed and the other table fires this funct it
					// causes this table to bounce back to the other table's Max value.
					// It's a convenient stop-mechanism to indicate that the other
					// table is not as long as this one; fortunately a second key
					// [Ctrl+End] allows this table to continue to its final destination.
				}
			}
		}

		void OnHoriScrollValueChanged(object sender, EventArgs e)
		{
			if (_table == null) _table = this;

			_table._editor.Visible = false;

			_table.offsetHori = _table._scrollHori.Value;
			_table.Refresh();


			if (!_f._search)
				Select(); // workaround: refocus the table (bar has to move > 0px)

			var pt = PointToClient(Cursor.Position);
			var args = new MouseEventArgs(MouseButtons.Left, 1, pt.X, pt.Y, 0); // clicks,x,y,delta
			OnMouseMove(args); // update coords on the Statusbar


			if (_f._diff1 != null && _f._diff2 != null)
			{
				HScrollBar hori = null;

				if      (_f._diff1 == this) hori = _f._diff2._scrollHori;
				else if (_f._diff2 == this) hori = _f._diff1._scrollHori;

				if (hori != null && hori.Maximum != 0)
				{
					if (_scrollHori.Value < hori.Maximum - hori.LargeChange)
					{
						hori.Value = _scrollHori.Value;
					}
					else
						hori.Value = hori.Maximum - (hori.LargeChange - 1);

					Select();

					// NOTE: An interesting effect occurs if this is the wider table.
					// When [End] is keyed and the other table fires this funct it
					// causes this table to bounce back to the other table's Max value.
					// It's a convenient stop-mechanism to indicate that the other
					// table is not as wide as this one; fortunately a second key
					// [End] allows this table to continue to its final destination.
				}
			}
		}

		/// <summary>
		/// @note This fires whenever a fly sneezes.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			if (!_init)
			{
				for (int tab = 0; tab != _f.Tabs.TabCount; ++tab)
				{
					_table = _f.Tabs.TabPages[tab].Tag as YataGrid;
					_table.InitScroll();

					// NOTE: The panels can be null during the load sequence.
					if (_table._panelCols   != null) _table._panelCols  .Width  = Width;
					if (_table._panelRows   != null) _table._panelRows  .Height = Height;
					if (_table._panelFrozen != null) _table._panelFrozen.Height = Height;

					if (_table._propanel != null && _table._propanel.Visible)
					{
						_table._propanel.deterTelemetric();
						_table._propanel.InitScroll();

						_table._propanel.DockBot = _table._propanel.DockBot; // redeter Top of propanel
					}

					if (!_f.IsMin) _table.EnsureDisplayedCellOrRow();

//					_table.Refresh();
				}
				_table = null;

//				if (_piColhead != null) _piColhead.Dispose();
//				_piColhead = new Bitmap(_bluePi, new Size(WidthTable, HeightColhead));

//				if (_piRowhead != null) _piRowhead.Dispose();
//				_piRowhead = new Bitmap(_bluePi, new Size(WidthRowhead, HeightTable));
			}

			if (_f.WindowState != FormWindowState.Minimized)
				_f.IsMin = false;

//			base.OnResize(e);
		}


		/// <summary>
		/// Initializes the vertical and horizontal scrollbars OnResize (which
		/// also happens auto after load).
		/// </summary>
		internal void InitScroll()
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
				int vert = HeightTable
						 + _scrollVert.LargeChange - 1
						 - Height
						 + (_visHori ? _scrollHori.Height : 0);

				if (vert < _scrollVert.LargeChange) vert = 0;

				_scrollVert.Maximum = vert;	// NOTE: Do not set this until after deciding
											// whether or not max < 0. 'Cause it fucks everything up. bingo.

				// handle .NET OnResize anomaly ->
				// keep the bottom of the table snuggled against the bottom of the visible area
				// when resize enlarges the area
				if (HeightTable < Height + offsetVert - (_visHori ? _scrollHori.Height : 0))
				{
					_scrollVert.Value = HeightTable - Height + (_visHori ? _scrollHori.Height : 0);
				}
			}
			else
			{
				_scrollVert.Value =
				_scrollVert.Maximum = 0;
			}

			if (_visHori)
			{
				int hori = WidthTable
						 + _scrollHori.LargeChange - 1
						 - Width
						 + (_visVert ? _scrollVert.Width : 0);

				if (hori < _scrollHori.LargeChange) hori = 0;

				_scrollHori.Maximum = hori;	// NOTE: Do not set this until after deciding
											// whether or not max < 0. 'Cause it fucks everything up. bingo.

				// handle .NET OnResize anomaly ->
				// keep the right of the table snuggled against the right of the visible area
				// when resize enlarges the area
				if (WidthTable < Width + offsetHori - (_visVert ? _scrollVert.Width : 0))
				{
					_scrollHori.Value = WidthTable - Width + (_visVert ? _scrollVert.Width : 0) + _proHori;
				}
			}
			else
			{
				_scrollHori.Value =
				_scrollHori.Maximum = 0;
			}
		}

		internal int _proHori;

		/// <summary>
		/// The table scrolls 1 pixel left if refreshing a table when scroll is
		/// far right. This funct checks if the table is scrolled far right and
		/// sets and additional 1 pixel offset for InitScrollers() to consider
		/// during row insertions and deletions.
		/// </summary>
		internal void SetProHori()
		{
			if (WidthTable == Width + offsetHori - (_visVert ? _scrollVert.Width : 0))
				_proHori = 1;
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
			if (_propanel != null && _propanel._scroll.Visible
				&& e.X > _propanel.Location.X && e.X < _propanel.Location.X + _propanel.Width)
			{
				_propanel.Scroll(e);
			}
			else if (!_editor.Visible)
			{
				if (_visVert && (!_visHori || (ModifierKeys & Keys.Control) != Keys.Control))
				{
					if ((ModifierKeys & Keys.Shift) == Keys.Shift) // shift grid vertically 1 visible-height per delta
					{
						int h = Height - HeightColhead - (_visHori ? _scrollHori.Height : 0);
						if (e.Delta > 0)
						{
							if (_scrollVert.Value - h < 0)
								_scrollVert.Value = 0;
							else
								_scrollVert.Value -= h;
						}
						else if (e.Delta < 0)
						{
							if (_scrollVert.Value + h + (_scrollVert.LargeChange - 1) > _scrollVert.Maximum)
								_scrollVert.Value = _scrollVert.Maximum - (_scrollVert.LargeChange - 1);
							else
								_scrollVert.Value += h;
						}
					}
					else
					{
						if (e.Delta > 0)
						{
							if (_scrollVert.Value - _scrollVert.LargeChange < 0)
								_scrollVert.Value = 0;
							else
								_scrollVert.Value -= _scrollVert.LargeChange;
						}
						else if (e.Delta < 0)
						{
							if (_scrollVert.Value + _scrollVert.LargeChange + (_scrollVert.LargeChange - 1) > _scrollVert.Maximum)
								_scrollVert.Value = _scrollVert.Maximum - (_scrollVert.LargeChange - 1);
							else
								_scrollVert.Value += _scrollVert.LargeChange;
						}
					}
				}
				else if (_visHori)
				{
					if ((ModifierKeys & Keys.Shift) == Keys.Shift) // shift grid horizontally 1 visible-width per delta
					{
						int w = WidthRowhead;
						for (int c = 0; c != FrozenCount; ++c)
							w += Cols[c].width();

						w = Width - w - (_visVert ? _scrollVert.Width : 0);
						if (w > 0) // TODO: not sure if <- needed
						{
							if (e.Delta > 0)
							{
								if (_scrollHori.Value - w < 0)
									_scrollHori.Value = 0;
								else
									_scrollHori.Value -= w;
							}
							else if (e.Delta < 0)
							{
								if (_scrollHori.Value + w > _scrollHori.Maximum)
									_scrollHori.Value = _scrollHori.Maximum - (_scrollHori.LargeChange - 1);
								else
									_scrollHori.Value += w;
							}
						}
					}
					else
					{
						if (e.Delta > 0)
						{
							if (_scrollHori.Value - _scrollHori.LargeChange < 0)
								_scrollHori.Value = 0;
							else
								_scrollHori.Value -= _scrollHori.LargeChange;
						}
						else if (e.Delta < 0)
						{
							if (_scrollHori.Value + _scrollHori.LargeChange + (_scrollHori.LargeChange - 1) > _scrollHori.Maximum)
								_scrollHori.Value = _scrollHori.Maximum - (_scrollHori.LargeChange - 1);
							else
								_scrollHori.Value += _scrollHori.LargeChange;
						}
					}
				}
			}
		}


//		const int HEADERS   = 0;
		const int VALUETYPE = 1;
		const int LABELS    = 2;

		internal const int LOADRESULT_FALSE   = 0;
		internal const int LOADRESULT_TRUE    = 1;
		internal const int LOADRESULT_CHANGED = 2;

		/// <summary>
		/// Tries to load a 2da file.
		/// </summary>
		/// <returns>a LOADRESULT_* val</returns>
		internal int Load2da()
		{
			bool ignoreErrors = false;

			int id = -1;

			string[] lines = File.ReadAllLines(Fullpath);
			string line = String.Empty;

			int total = lines.Length;
			if (total < LABELS + 1) total = LABELS + 1;

			for (int i = 0; i != total; ++i)
			{
				if (i < lines.Length)
					line = lines[i].Trim();
				else
					line = String.Empty;

				if (i > LABELS)
				{
					string[] cells = Parse2daRow(line);
					if (cells.Length != 0) // allow blank lines on load - they will be removed if/when file is saved.
					{
						++id; // test for well-formed, consistent IDs

						if (!ignoreErrors)
						{
							string info = String.Empty;

							int result;
							if (!Int32.TryParse(cells[0], out result))
								info = "The 2da-file contains an ID that is not an integer.";
							else if (result != id)
								info = "The 2da-file contains an ID that is out of order.";

							if (!String.IsNullOrEmpty(info))
							{
								string error = info
											 + Environment.NewLine + Environment.NewLine
											 + Fullpath
											 + Environment.NewLine + Environment.NewLine
											 + id + " / " + cells[0];
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
						if (!ignoreErrors && cells.Length != Fields.Length + 1)
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

						_rows.Add(cells);
					}
				}
				else if (i == LABELS)
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
				else if (i == VALUETYPE)
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
				else //if (i == HEADERS) // test version header
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
					cells[c] = Constants.Stars;

				_rows.Add(cells);
				return LOADRESULT_CHANGED; // used to flag the Table as changed.
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
		internal static string[] Parse2daRow(string line)
		{
			var list  = new List<string>();
			var field = new List<char>();

			bool add      = false;
			bool inQuotes = false;

			char c;

			int posLast = line.Length + 1;					// include an extra iteration to get the last field
			for (int pos = 0; pos != posLast; ++pos)
			{
				if (pos == line.Length)						// hit lineend -> add the last field
				{
					if (add)
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

				FrozenCount = YataGrid.FreezeId;

				Cols.Clear();
				Rows.Clear();

				Controls.Remove(_panelCols);
				Controls.Remove(_panelRows);
				Controls.Remove(_panelFrozen);

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

					case "":									// NOTE: YataForm.CreateTabPage() does not allow a blank
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

			_panelFrozen = new YataPanelFrozen(this, Cols[0].width());
			FrozenLabelsInit();

			SetStaticHeads();

			Controls.Add(_panelFrozen);
			Controls.Add(_panelRows);
			Controls.Add(_panelCols);


			_scrollVert.LargeChange =
			_scrollHori.LargeChange = HeightRow;
			InitScroll();

			Select();

			CheckCellTexts();

			_init = false;
		}

		/// <summary>
		/// Re-layouts the table when Font changes or on Autosize cols or when
		/// row(s) are inserted/deleted.
		/// </summary>
		/// <param name="r">first row to consider as changed (default -1 if
		/// deleting rows)</param>
		/// <param name="range">range of rows to consider as changed (default 0
		/// for single row)</param>
		internal void Calibrate(int r = -1, int range = 0)
		{
			_init = true;

			_editor.Visible = false;

			for (int c = 0; c != ColCount; ++c)
				colRewidth(c, r, range);

			FrozenCount = FrozenCount; // refresh the Frozen panel

			SetStaticHeads();


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
				ColCount = Fields.Length + 1; // 'Fields' does not include rowhead and id-col

				for (; c != ColCount; ++c)
				{
					Cols.Add(new Col());
				}

				Cols[0].text = "id"; // NOTE: Is not measured - the cells below it determine col-width.
			}

			int w; c = 0;
			foreach (string head in Fields) // colheads only.
			{
				++c; // start at col 1 - skip id col

				if (!calibrate)
					Cols[c].text = head;

				w = YataGraphics.MeasureWidth(head, _f.FontAccent);
				Cols[c]._widthtext = w;

				Cols[c].width(w + _padHori * 2 + _padHoriSort, calibrate);
			}
		}

		/// <summary>
		/// Creates the rows and adds cells to each row.
		/// </summary>
		void CreateRows()
		{
			RowCount = _rows.Count;

			Cell cell;
			string text = String.Empty;
			Brush brush;
			bool stars, changed = false;

			for (int r = 0; r != RowCount; ++r)
			{
				changed = changed || (_rows[r].Length > ColCount); // flag Changed if any field(s) get cut off.

				brush = (r % 2 == 0) ? Brushes.Alice
									 : Brushes.Blanche;

				Rows.Add(new Row(r, ColCount, brush, this));
				for (int c = 0; c != ColCount; ++c)
				{
					if (c < _rows[r].Length)
					{
						text = _rows[r][c];
						stars = false;
					}
					else
					{
						text = Constants.Stars;
						stars = true;
					}

					cell = (this[r,c] = new Cell(r,c, text));
					if (stars)
					{
						cell.loadchanged = true;
						changed = true;
					}
				}
			}
			Changed |= changed;

			_rows.Clear(); // done w/ '_rows'


			int w, wT;

			for (int c = 0; c != ColCount; ++c)
			{
				w = _wid;
				for (int r = 0; r != RowCount; ++r)
				{
					wT = YataGraphics.MeasureWidth(this[r,c].text, Font);
					this[r,c]._widthtext = wT;

					wT += _padHori * 2;
					if (wT > w) w = wT;
				}
				Cols[c].width(w);
			}
		}


		bool _initFrozenLabels = true;

		/// <summary>
		/// Initializes the frozen-labels on the colhead panel.
		/// </summary>
		void FrozenLabelsInit()
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

					_labelid.Paint += labelid_Paint;
					_labelid.MouseClick += click_IdLabel;
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

						_labelfirst.Paint += labelfirst_Paint;
						_labelfirst.MouseClick += click_FirstLabel;
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

							_labelsecond.Paint += labelsecond_Paint;
							_labelsecond.MouseClick += click_SecondLabel;
							_labelsecond.MouseClick += (sender, e) => Select();
						}

						_panelCols.Controls.Add(_labelsecond);
					}
				}
			}
			_initFrozenLabels = false;
		}


		/// <summary>
		/// Sets standard HeightColhead, HeightRow, and minimum cell width.
		/// These values are the same for all loaded tables.
		/// </summary>
		/// <param name="f"></param>
		internal static void SetStaticMetrics(YataForm f)
		{
			HeightColhead = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, f.FontAccent) + _padVert * 2;
			HeightRow     = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, f.Font)       + _padVert * 2;

			_wid = YataGraphics.MeasureWidth("id", f.Font) + _padHoriRowhead * 2;
		}

		/// <summary>
		/// Calculates and maintains rowhead width wrt/ current Font across all
		/// tabs/tables.
		/// </summary>
		void SetStaticHeads()
		{
			YataGrid table;

			int rows = 0, rowsTest; // row-headers' width stays uniform across all tabpages

			int tabs = _f.Tabs.TabCount;
			int tab = 0;
			for (; tab != tabs; ++tab) // find the table w/ most rows ->
			{
				table = _f.Tabs.TabPages[tab].Tag as YataGrid;
				if ((rowsTest = table.RowCount - 1) > rows)
					rows = rowsTest;
			}

			string text = "9"; // determine how many nines need to be measured ->
			int w = 1;
			while ((rows /= 10) != 0)
			{
				++w;
				text += "9";
			}

			WidthRowhead = YataGraphics.MeasureWidth(text, _f.FontAccent) + _padHoriRowhead * 2;

			for (tab = 0; tab != tabs; ++tab)
			{
				table = _f.Tabs.TabPages[tab].Tag as YataGrid;

				table.WidthTable = WidthRowhead;
				for (int c = 0; c != table.ColCount; ++c)
					table.WidthTable += table.Cols[c].width();

				table._panelRows.Width  = WidthRowhead;
				table._panelCols.Height = HeightColhead;

				FrozenLabelsSet(table);
			}
		}

		internal void FrozenLabelsSet(YataGrid table = null)
		{
			if (table == null) table = this;

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
		/// <param name="c">col that changed its width</param>
		internal void UpdateFrozenControls(int c)
		{
			if (c < FreezeSecond)
			{
				FrozenLabelsSet(); // re-width the frozen-labels on the colhead

				if (c < FrozenCount)
					FrozenCount = FrozenCount; // re-width the frozen-panel
			}
		}


		/// <summary>
		/// Disables textbox navigation etc. keys to allow table scrolling on
		/// certain key-events (iff the editbox is not focused).
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			//logfile.Log("");
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

//			base.OnPreviewKeyDown(e);
		}

		/// <summary>
		/// Handles navigation by keyboard around the table. Also handles the
		/// delete-key for selected row(s) as well as the escape-key to clear
		/// all selections.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			//logfile.Log("YataGrid.OnKeyDown() e.KeyData= " + e.KeyData);

			Cell sel = getSelectedCell();
			int selr = getSelectedRow();

			Row row;

			// TODO: change selected col perhaps

			switch (e.KeyCode)
			{
				case Keys.Home:
					if (selr != -1)
					{
						if ((e.Modifiers & Keys.Control) == Keys.Control)
						{
							if (selr > 0)
							{
								ClearSelects();

								(row = Rows[0]).selected = true;
								for (int c = 0; c != ColCount; ++c)
									row[c].selected = true;

								EnsureDisplayedRow(0);
							}
						}
						else if (_visHori)
							_scrollHori.Value = 0;
					}
					else if ((e.Modifiers & Keys.Control) == Keys.Control)
					{
						if (sel != null)
						{
							if (sel.x != FrozenCount || sel.y != 0)
							{
								sel.selected = false;
								(sel = this[0, FrozenCount]).selected = true;

								EnsureDisplayed(sel);
							}
						}
						else if (_visVert)
							_scrollVert.Value = 0;
					}
					else if (sel != null)
					{
						if (sel.x != FrozenCount)
						{
							sel.selected = false;
							(sel = this[sel.y, FrozenCount]).selected = true;

							EnsureDisplayed(sel);
						}
					}
					else if (_visHori)
						_scrollHori.Value = 0;

					break;

				case Keys.End:
					if (selr != -1)
					{
						if ((e.Modifiers & Keys.Control) == Keys.Control)
						{
							if (selr != RowCount - 1)
							{
								ClearSelects();

								(row = Rows[RowCount - 1]).selected = true;
								for (int c = 0; c != ColCount; ++c)
									row[c].selected = true;

								EnsureDisplayedRow(RowCount - 1);
							}
						}
						else if (_visHori)
							_scrollHori.Value = WidthTable - Width + (_visVert ? _scrollVert.Width : 0);
					}
					else if ((e.Modifiers & Keys.Control) == Keys.Control)
					{
						if (sel != null)
						{
							if (sel.x != ColCount - 1 || sel.y != RowCount - 1)
							{
								sel.selected = false;
								(sel = this[RowCount - 1, ColCount - 1]).selected = true;

								EnsureDisplayed(sel);
							}
						}
						else if (_visVert)
							_scrollVert.Value = HeightTable - Height + (_visHori ? _scrollHori.Height : 0);
					}
					else if (sel != null)
					{
						if (sel.x != ColCount - 1)
						{
							sel.selected = false;
							(sel = this[sel.y, ColCount - 1]).selected = true;

							EnsureDisplayed(sel);
						}
					}
					else if (_visHori)
						_scrollHori.Value = WidthTable - Width + (_visVert ? _scrollVert.Width : 0);

					break;

				case Keys.PageUp:
					if (selr != -1)
					{
						if (selr > 0)
						{
							ClearSelects();

							int rows = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;

							int r;
							if (selr < rows) r = 0;
							else             r = selr - rows;

							(row = Rows[r]).selected = true;
							for (int c = 0; c != ColCount; ++c)
								row[c].selected = true;

							EnsureDisplayedRow(r);
						}
					}
					else if (sel != null)
					{
						if (sel.y != 0)
						{
							sel.selected = false;
							int rows = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;

							int r;
							if (sel.y < rows) r = 0;
							else              r = sel.y - rows;

							(sel = this[r, sel.x]).selected = true;

							EnsureDisplayed(sel);
						}
					}
					else if (_visVert)
					{
						int h = Height - HeightColhead - (_visHori ? _scrollHori.Height : 0);

						if (_scrollVert.Value - h < 0)
							_scrollVert.Value = 0;
						else
							_scrollVert.Value -= h;
					}
					break;

				case Keys.PageDown:
					if (selr != -1)
					{
						if (selr != RowCount - 1)
						{
							ClearSelects();

							int rows = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;

							int r;
							if (selr > RowCount - 1 - rows) r = RowCount - 1;
							else                            r = selr + rows;

							(row = Rows[r]).selected = true;
							for (int c = 0; c != ColCount; ++c)
								row[c].selected = true;

							EnsureDisplayedRow(r);
						}
					}
					else if (sel != null)
					{
						if (sel.y != RowCount - 1)
						{
							sel.selected = false;
							int rows = (Height - HeightColhead - (_visHori ? _scrollHori.Height : 0)) / HeightRow;

							int r;
							if (sel.y > RowCount - 1 - rows) r = RowCount - 1;
							else                             r = sel.y + rows;

							(sel = this[r, sel.x]).selected = true;

							EnsureDisplayed(sel);
						}
					}
					else if (_visVert)
					{
						int h = Height - HeightColhead - (_visHori ? _scrollHori.Height : 0);

						if (_scrollVert.Value + h + (_scrollVert.LargeChange - 1) > _scrollVert.Maximum)
							_scrollVert.Value = _scrollVert.Maximum - (_scrollVert.LargeChange - 1);
						else
							_scrollVert.Value += h;
					}
					break;

				case Keys.Up: // NOTE: Needs to bypass KeyPreview
					if (selr != -1)
					{
						if (selr > 0)
						{
							ClearSelects();

							(row = Rows[selr - 1]).selected = true;
							for (int c = 0; c != ColCount; ++c)
								row[c].selected = true;

							EnsureDisplayedRow(selr - 1);
						}
					}
					else if (sel != null) // selection to the cell above
					{
						if (sel.y != 0)
						{
							// TODO: Multi-selecting cells w/ keyboard would require tracking a "current" cell.
//							cell.selected &= ((ModifierKeys & Keys.Control) == Keys.Control);

							sel.selected = false;
							(sel = this[sel.y - 1, sel.x]).selected = true;

							EnsureDisplayed(sel);
						}
					}
					else if (_visVert)
					{
						if (_scrollVert.Value - _scrollVert.LargeChange < 0)
							_scrollVert.Value = 0;
						else
							_scrollVert.Value -= _scrollVert.LargeChange;
					}
					break;

				case Keys.Down: // NOTE: Needs to bypass KeyPreview
					if (selr != -1)
					{
						if (selr != RowCount - 1)
						{
							ClearSelects();

							(row = Rows[selr + 1]).selected = true;
							for (int c = 0; c != ColCount; ++c)
								row[c].selected = true;

							EnsureDisplayedRow(selr + 1);
						}
					}
					else if (sel != null) // selection to the cell below
					{
						if (sel.y != RowCount - 1)
						{
							sel.selected = false;
							(sel = this[sel.y + 1, sel.x]).selected = true;

							EnsureDisplayed(sel);
						}
					}
					else if (_visVert) // scroll the table
					{
						if (_scrollVert.Value + _scrollVert.LargeChange + (_scrollVert.LargeChange - 1) > _scrollVert.Maximum)
							_scrollVert.Value = _scrollVert.Maximum - (_scrollVert.LargeChange - 1);
						else
							_scrollVert.Value += _scrollVert.LargeChange;
					}
					break;

				case Keys.Left: // NOTE: Needs to bypass KeyPreview
					if ((e.Modifiers & Keys.Shift) == Keys.Shift) // shift grid 1 page left
					{
						if (_visHori)
						{
							int w = WidthRowhead;
							for (int c = 0; c != FrozenCount; ++c)
								w += Cols[c].width();

							w = Width - w - (_visVert ? _scrollVert.Width : 0);
							if (w > 0)
							{
								if (_scrollHori.Value - w < 0)
									_scrollHori.Value = 0;
								else
									_scrollHori.Value -= w;
							}
						}
					}
					else if (sel != null) // selection to the cell left
					{
						if (sel.x != FrozenCount)
						{
							sel.selected = false;
							(sel = this[sel.y, sel.x - 1]).selected = true;

							EnsureDisplayed(sel);
						}
					}
					else if (_visHori)
					{
						if (_scrollHori.Value - _scrollHori.LargeChange < 0)
							_scrollHori.Value = 0;
						else
							_scrollHori.Value -= _scrollHori.LargeChange;
					}
					break;

				case Keys.Right: // NOTE: Needs to bypass KeyPreview
					if ((e.Modifiers & Keys.Shift) == Keys.Shift) // shift grid 1 page right
					{
						if (_visHori)
						{
							int w = WidthRowhead;
							for (int c = 0; c != FrozenCount; ++c)
								w += Cols[c].width();

							w = Width - w - (_visVert ? _scrollVert.Width : 0);
							if (_scrollHori.Value + w > _scrollHori.Maximum)
								_scrollHori.Value = _scrollHori.Maximum - (_scrollHori.LargeChange - 1);
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

							EnsureDisplayed(sel);
						}
					}
					else if (_visHori)
					{
						if (_scrollHori.Value + _scrollHori.LargeChange + (_scrollHori.LargeChange - 1) > _scrollHori.Maximum)
							_scrollHori.Value = _scrollHori.Maximum - (_scrollHori.LargeChange - 1);
						else
							_scrollHori.Value += _scrollHori.LargeChange;
					}
					break;

				case Keys.Escape: // NOTE: Needs to bypass KeyPreview
					ClearSelects();
					break;

				case Keys.Delete:
					if (!Readonly)
					{
						SetProHori();
						Delete();
						_proHori = 0;
					}
					break;
			}

			Refresh();

//			base.OnKeyDown(e);
		}

		/// <summary>
		/// Deletes a single or multiple rows on keypress Delete.
		/// </summary>
		void Delete()
		{
			int selr = getSelectedRow();
			if (selr != -1)
			{
				_f.ShowColorPanel();
				DrawingControl.SuspendDrawing(this);


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
					Insert(rLast, null, false);

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


				_f.ShowColorPanel(false);
				DrawingControl.ResumeDrawing(this);
			}
		}


		/// <summary>
		/// Handles KeyDown events in the cell-editor.
		/// @note Works around dweeby .NET behavior if Alt is pressed while
		/// editing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void keydown_Editor(object sender, KeyEventArgs e)
		{
			//logfile.Log("YataGrid.keydown_Editor() e.KeyData= " + e.KeyData);

			if (e.Alt)
			{
				_editor.Visible = false;
				Refresh();
			}
		}

		/// <summary>
		/// Handles the Leave event in the cell-editor.
		/// @note Works around dweeby .NET behavior if Ctrl+PageUp/Down is
		/// pressed while editing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void leave_Editor(object sender, EventArgs e)
		{
			if ((ModifierKeys & Keys.Control) == Keys.Control)
			{
				_editor.Focus(); // ie. don't leave editor.
			}
		}

		/// <summary>
		/// Handles the Leave event for the grid: hides the editbox if it is
		/// visible.
		/// @note But it doesn't fire if the tabpage changes w/ key
		/// Ctrl+PageUp/PageDown. Lovely /explode - is fixed in
		/// YataForm.tab_SelectedIndexChanged().
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void leave_Grid(object sender, EventArgs e)
		{
			if (_editor.Visible)
			{
				_editor.Visible = false;
				Refresh();
			}
		}

		/// <summary>
		/// Handles ending editing a cell by pressing Enter or Escape/Tab - this
		/// fires during edit or so.
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
						ApplyTextEdit();
						goto case Keys.Escape;
					}

					if (!Readonly && (_editcell = getSelectedCell()) != null)
					{
						EditCell();
						_editor.Focus();
						Refresh();
					}
					return true;

				case Keys.Escape:
				case Keys.Tab:
					_editor.Visible = false;
					Select();
					Refresh();
					return true;
			}
			return base.ProcessDialogKey(keyData);
		}

		/// <summary>
		/// Applies a text-edit via the editbox.
		/// </summary>
		void ApplyTextEdit()
		{
			string text = _editor.Text;
			if (text != _editcell.text)
				ChangeCellText(_editcell, _editor);
		}

		/// <summary>
		/// Changes a cell's text, recalculates col-width, and sets up Undo/Redo.
		/// </summary>
		/// <param name="cell">a Cell</param>
		/// <param name="tb">the editor's textbox whose text to check for validity</param>
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

			colRewidth(cell.x, cell.y);
			UpdateFrozenControls(cell.x);


			rest.postext = cell.text;
			_ur.Push(rest);
		}

		/// <summary>
		/// Changes a cell's text, recalculates col-width, and sets up Undo/Redo.
		/// </summary>
		/// <param name="cell">a Cell</param>
		/// <param name="text">the text to change to</param>
		internal void ChangeCellText(Cell cell, string text)
		{
			Restorable rest = UndoRedo.createCell(cell);
			if (!Changed)
			{
				Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}


			cell.loadchanged =
			cell.diff = false; // TODO: Check the differ if the celltext is identical or still different.

			cell.text = text;

			colRewidth(cell.x, cell.y);
			UpdateFrozenControls(cell.x);

			Refresh();


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
		internal void colRewidth(int c, int r = -1, int range = 0)
		{
			var col = Cols[c];

			int w = col._widthtext + _padHoriSort;
			int wT;

			if (r != -1) // NOTE: re-calc '_widthtext' regardless of what happens below ->
			{
				int r1 = r + range;
				for (; r <= r1; ++r)
				{
					wT = YataGraphics.MeasureWidth(this[r,c].text, Font);
					this[r,c]._widthtext = wT;
					if (wT > w) w = wT;
				}
			}

			if (!col.UserSized)	// ie. don't resize a col that user has adjusted. If it needs to
			{					// be forced (eg. on reload) unflag UserSized on all cols first.
				w += _padHori * 2;

				int width = col.width();
				if (w > width)
				{
					col.width(w);
				}
				else if (w < width) // recalc width on the entire col
				{
					if (c == 0 || _wid > w)
						w = _wid;

					for (r = 0; r != RowCount; ++r)
					{
						wT = this[r,c]._widthtext + _padHori * 2;
						if (wT > w) w = wT;
					}
					col.width(w, true);
				}

				if (range == 0 && w != width)	// if range >0 let Calibrate() handle multiple
				{								// cols or at least Scrollers and Refresh
					InitScroll();
					Refresh(); // is required - and yet another Refresh() will follow ....
				}
			}

			if (_propanel != null && _propanel.Visible)
			{
				_propanel.deterValfieldWidth(); // TODO: Re-calc the 'c' col only.
				_propanel.deterTelemetric();
				_propanel.InitScroll();
			}
		}

		/// <summary>
		/// Checks the text that user submits to a cell.
		/// Cf CheckCellText().
		/// </summary>
		/// <param name="tb"></param>
		/// <returns>true if text is changed/fixed/corrected</returns>
		internal static bool CheckTextEdit(Control tb)
		{
			bool changed = false;

			string field = tb.Text.Trim();

			if (String.IsNullOrEmpty(field))
			{
				tb.Text = Constants.Stars;
				return false; // NOTE: Don't bother the user if he/she simply wants to blank a field.
			}

			bool quoteFirst = field.StartsWith("\"", StringComparison.InvariantCulture);
			bool quoteLast  = field.EndsWith(  "\"", StringComparison.InvariantCulture);
			if (quoteFirst && quoteLast)
			{
				if (   field.Length < 3
					|| field.Substring(1, field.Length - 2).Trim() == String.Empty)
				{
					tb.Text = Constants.Stars;
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
					tb.Text = Constants.Stars;
					return true;
				}

				tb.Text = field;
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
					tb.Text = first + test + last;
				}

				if (test == Constants.Stars)
				{
					tb.Text = Constants.Stars;
					return true;
				}
			}

			return changed;
		}

		/// <summary>
		/// Checks the texts in all cells. Used only when loading or re-loading
		/// a 2da - it can flag a cell's 'loadchanged' variable.
		/// </summary>
		void CheckCellTexts()
		{
			bool changed = false;

			Cell cell;
			foreach (var row in Rows)
			{
				for (int c = 0; c != ColCount; ++c)
				{
					string text = (cell = row[c]).text;
					if (CheckCellText(ref text))
					{
						cell.text = text;
						cell.loadchanged = true;
						changed = true;
					}
				}
			}
			Changed |= changed;
		}

		/// <summary>
		/// Checks the text in a cell.
		/// Cf CheckTextEdit().
		/// </summary>
		/// <param name="text">ref to a text-string</param>
		/// <returns>true if text is changed/fixed/corrected</returns>
		bool CheckCellText(ref string text)
		{
			bool changed = false;

			string field = text.Trim();

			if (String.IsNullOrEmpty(field))
			{
				text = Constants.Stars;
				return true;
			}

			bool quoteFirst = field.StartsWith("\"", StringComparison.InvariantCulture);
			bool quoteLast  = field.EndsWith(  "\"", StringComparison.InvariantCulture);
			if (quoteFirst && quoteLast)
			{
				if (   field.Length < 3
					|| field.Substring(1, field.Length - 2).Trim() == String.Empty)
				{
					text = Constants.Stars;
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
					text = Constants.Stars;
					return true;
				}

				text = field;
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
					text = first + test + last;
				}

				if (test == Constants.Stars)
				{
					text = Constants.Stars;
					changed = true;
				}
			}

			return changed;
		}


		/// <summary>
		/// LMB selects a cell or enables/disables the editbox. RMB opens a
		/// cell's context-menu.
		/// @note MouseClick does not register on any of the top or left panels.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			Select();

			if (e.X > WidthTable || e.Y > HeightTable) // click to the right or below the table-area
			{
				if (_editor.Visible) // NOTE: The editbox will never be visible here on RMB. for whatever reason ...
				{
//					if (e.Button == MouseButtons.Left) // apply edit only on LMB.
					ApplyTextEdit();

					_editor.Visible = false;
					Refresh();
				}
/*				else if (e.Button == MouseButtons.Right)	// clear all selects - why does a right-click refuse to acknowledge that the editor is Vis
				{											// Ie. if this codeblock is activated it will cancel the edit *and* clear all selects;
					foreach (var col in Cols)				// the intent however is to catch the editor (above) OR clear all selects here.
						col.selected = false;

					foreach (var row in Rows)
						row.selected = false;

					ClearCellSelects();
					Refresh();
				} */
			}
			else if (e.Button == MouseButtons.Left)
			{
				foreach (var col in Cols)
					col.selected = false;

				Cell cell = getClickedCell(e.X, e.Y);
				if (cell != null) // safety.
				{
					if (!_editor.Visible)
					{
						if ((ModifierKeys & Keys.Control) == Keys.Control)
						{
							if (cell.selected = !cell.selected)
								EnsureDisplayed(cell);
						}
						else if (!cell.selected || getSelectedCell() == null) // cell is not selected or it's not the only selected cell
						{
							foreach (var row in Rows)
								row.selected = false;

							ClearCellSelects();
							cell.selected = true;
							EnsureDisplayed(cell);
						}
						else if (!Readonly) // cell is already selected
						{
							_editcell = cell;
							EditCell();
							_editor.Focus();
						}
					}
					else // editor is Vis
					{
						if (cell != _editcell)
						{
							ApplyTextEdit();
							_editor.Visible = false;
						}
						else // NOTE: There's a clickable fringe around the editor.
							_editor.Focus();
					}

					Refresh();
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				ClearSelects();

				Cell cell = getClickedCell(e.X, e.Y);
				if (cell != null) // safety.
				{
					cell.selected = true;
					Refresh();

					_f.ShowCellMenu();
				}
			}

			_f.it_CopyCell .Enabled =
			_f.it_PasteCell.Enabled = (getSelectedCell() != null);

//			base.OnMouseClick(e);
		}

		/// <summary>
		/// Because sometimes I'm a stupid and double-click to start textedit.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				OnMouseClick(e);

//			base.OnMouseDoubleClick(e);
		}

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
		/// Starts cell edit on either LMB or Enter-key.
		/// </summary>
		void EditCell()
		{
			EnsureDisplayed(_editcell);

			var rect = getCellRectangle(_editcell); // align the editbox over the text ->
			_editor.Left   = rect.X + 5;
			_editor.Top    = rect.Y + 4;
			_editor.Width  = rect.Width - 6;
			_editor.Height = rect.Height;

			_editor.Visible = true;
			_editor.Text = _editcell.text;

			_editor.SelectionStart = 0; // because .NET
//			if (_editor.Text == Constants.Stars)
//			{
			_editor.SelectionLength = _editor.Text.Length;
//			}
//			else
//				_editor.SelectionStart = _editor.Text.Length;
		}

		/// <summary>
		/// Clears all cells that are currently selected.
		/// </summary>
		internal void ClearCellSelects()
		{
			foreach (var row in Rows)
			{
				for (int c = 0; c != ColCount; ++c)
					row[c].selected = false;
			}

			_f.it_CopyCell .Enabled =
			_f.it_PasteCell.Enabled = false;
		}

		/// <summary>
		/// Clears all rows, cells and cols that are currently selected.
		/// </summary>
		internal void ClearSelects()
		{
			foreach (var row in Rows)
			{
				row.selected = false;
				for (int c = 0; c != ColCount; ++c)
					row[c].selected = false;
			}

			foreach (var col in Cols)
				col.selected = false;

			_f.it_CopyCell .Enabled =
			_f.it_PasteCell.Enabled = false;
		}


		/// <summary>
		/// Handles mouse-movement over the grid - prints coordinates and info
		/// to the statusbar.
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
								var cords = getCords(x, y, left);
								_f.PrintInfo(new Point(cords.X, cords.Y));

								return;
							}
						}
					}
				}
				_f.PrintInfo(); // clear
			}

//			base.OnMouseMove(e);
		}


		/// <summary>
		/// Handles timer ticks - clears statusbar coordinates and info when the
		/// mouse-cursor leaves the grid. (MouseLeave is unreliable.)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void t1_Tick(object sender, EventArgs e)
		{
			if (!_init && _f.Tabs.TabCount != 0 && _f.Tabs.SelectedTab.Tag == this) // required for CloseAll ...
			{
				int left = getLeft();
				var rect = new Rectangle(left,
										 HeightColhead,
										 Width  - left          - (_visVert ? _scrollVert.Width  : 0),
										 Height - HeightColhead - (_visHori ? _scrollHori.Height : 0));

				if (!rect.Contains(PointToClient(Cursor.Position)))
					_f.PrintInfo(); // clear
			}
		}


		/// <summary>
		/// Checks if only one cell is currently selected and returns it if so.
		/// </summary>
		/// <returns>the only cell selected, null if none or more than one cell
		/// is currently selected</returns>
		internal Cell getSelectedCell()
		{
			Cell cell0 = null;

			Cell cell;
			foreach (var row in Rows)
			{
				for (int c = 0; c != ColCount; ++c)
				{
					if ((cell = row[c]).selected)
					{
						if (cell0 != null)
							return null;

						cell0 = cell;
					}
				}
			}
			return cell0;
		}

		/// <summary>
		/// Gets the col/row coordinates of a cell based on the current mouse-
		/// position.
		/// </summary>
		/// <param name="x">mouse x-pos within the table</param>
		/// <param name="y">mouse y-pos within the table</param>
		/// <param name="left">the x-pos of the right edge of the frozen-panel;
		/// ie, the left edge of the visible area of the table</param>
		/// <returns></returns>
		Point getCords(int x, int y, int left) // "Parameter is assigned but its value is never used" - bzzt.
		{
			var cords = new Point();

			cords.X = FrozenCount - 1;
			do { ++cords.X; }
			while ((left += Cols[cords.X].width()) < x);


			int top = HeightColhead;

			cords.Y = 0;
			while ((top += HeightRow) < y)
				++cords.Y;

			return cords;
		}

		/// <summary>
		/// Gets the cell-rectangle for a given cell.
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
		/// Not really a point but the left and right bounds of a col.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		Point getColEdges(int c)
		{
			var bounds = new Point();

			bounds.X = WidthRowhead - offsetHori;
			for (int col = 0; col != c; ++col)
				bounds.X += Cols[col].width();

			bounds.Y = (bounds.X + Cols[c].width());

			return bounds;
		}

		/// <summary>
		/// Not really a point but the upper and lower bounds of a row.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		Point getRowEdges(int r)
		{
			var bounds = new Point();
			bounds.X = HeightColhead + HeightRow * r - offsetVert;
			bounds.Y = bounds.X + HeightRow;

			return bounds;
		}

		/// <summary>
		/// Gets the x-pos of the right edge of the frozen-panel; ie, the left
		/// edge of the visible/editable area of the table
		/// </summary>
		/// <returns></returns>
		int getLeft()
		{
			int left = WidthRowhead;
			switch (FrozenCount)
			{
				case FreezeSecond:
					left += Cols[2].width();
					goto case FreezeFirst;

				case FreezeFirst:
					left += Cols[1].width();
					goto case FreezeId;

				case FreezeId:
					left += Cols[0].width();
					break;
			}

			return left;
		}


		/// <summary>
		/// Scrolls the table so that a given cell is (more or less) completely
		/// displayed.
		/// </summary>
		/// <param name="cell">the cell to display</param>
		/// <returns>true if the table had to be scrolled - ie. needs Refresh</returns>
		internal bool EnsureDisplayed(Cell cell)
		{
			bool ret = false;

			var rect = getCellRectangle(cell);

			int left = getLeft();
			int bar;

			if (rect.X != left)
			{
				bar = _visVert ? _scrollVert.Width : 0;
				int right = Width - bar;

				if (rect.X < left
					|| (rect.Width > right - left
						&& (rect.X > right || rect.X + left > (right - left) / 2)))
				{
					_scrollHori.Value -= left - rect.X;
					ret = true;
				}
				else if (rect.X + rect.Width > right && rect.Width < right - left)
				{
					_scrollHori.Value += rect.X + rect.Width + bar - Width;
					ret = true;
				}
			}

			if (rect.Y != HeightColhead)
			{
				if (rect.Y < HeightColhead)
				{
					_scrollVert.Value -= HeightColhead - rect.Y;
					ret = true;
				}
				else
				{
					bar = _visHori ? _scrollHori.Height : 0;
					if (rect.Y + rect.Height + bar > Height)
					{
						_scrollVert.Value += rect.Y + rect.Height + bar - Height;
						ret = true;
					}
				}
			}

			return ret;
		}

		/// <summary>
		/// Scrolls the table so that a given row is (more or less) completely
		/// displayed.
		/// </summary>
		/// <param name="r">the row-id display</param>
		/// <returns>true if the table had to be scrolled - ie. needs Refresh</returns>
		internal bool EnsureDisplayedRow(int r)
		{
			var bounds = getRowEdges(r);

			if (bounds.X != HeightColhead)
			{
				if (bounds.X < HeightColhead)
				{
					_scrollVert.Value -= HeightColhead - bounds.X;
					return true;
				}

				int bar = _visHori ? _scrollHori.Height : 0;
				if (bounds.Y + bar > Height)
				{
					_scrollVert.Value += bounds.Y + bar - Height;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Scrolls the table so that the currently selected cell or row is
		/// (more or less) completely displayed.
		/// </summary>
		/// <returns>true if the table had to be scrolled - ie. needs Refresh</returns>
		internal bool EnsureDisplayedCellOrRow()
		{
			Cell sel = getSelectedCell();
			if (sel != null)
				return EnsureDisplayed(sel);

			for (int r = 0; r != RowCount; ++r)
			{
				if (Rows[r].selected)
					return EnsureDisplayedRow(r);
			}
			return false;
		}

		/// <summary>
		/// Scrolls the table so that a given col is (more or less) completely
		/// displayed.
		/// </summary>
		/// <param name="c">the col to display</param>
		void EnsureDisplayedCol(int c)
		{
			var bounds = getColEdges(c);

			int left = getLeft();

			if (bounds.X != left)
			{
				int bar = _visVert ? _scrollVert.Width : 0;
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
		/// Handles a mouseclick on the rowhead. Selects or deselects row(s).
		/// Fires only on the rowhead panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void click_RowPanel(object sender, MouseEventArgs e)
		{
			if (RowCount != 0)
			{
				int r = (e.Y + offsetVert) / HeightRow;
				if (r < RowCount)
				{
					if (e.Button == MouseButtons.Left)
					{
						_editor.Visible = false;
						Select();

						var row = Rows[r];

						bool select;

						if ((ModifierKeys & Keys.Shift) != Keys.Shift) // else Shift always selects
						{
							select = false;
							for (int c = 0; c != ColCount; ++c)
							{
								if (!row[c].selected)
								{
									select = true;
									break;
								}
							}
						}
						else
							select = true;


						foreach (var col in Cols) // always clear col-selects
							col.selected = false;

						if ((ModifierKeys & Keys.Control) != Keys.Control)
							ClearCellSelects();

						if ((ModifierKeys & Keys.Shift) == Keys.Shift)
						{
							int selr = getSelectedRow();
							if (selr != -1)
							{
								RangeSelect = (r - selr);

								int start, stop;
								if (selr < r)
								{
									start = selr;
									stop  = r;
								}
								else
								{
									start = r;
									stop  = selr;
								}

								Row ro;
								while (start != stop + 1)
								{
									if (start != r) // done below
									{
										ro = Rows[start];
										for (int c = 0; c != ColCount; ++c)
											ro[c].selected = true;
									}
									++start;
								}
							}
						}
						else
						{
							foreach (var ro in Rows)
								ro.selected = false;

							row.selected = select;
						}

						if (select)
							EnsureDisplayedRow(r);

						for (int c = 0; c != ColCount; ++c)
							row[c].selected = select;

						Refresh();
					}
					else if (e.Button == MouseButtons.Right)
					{
						_f.context_(r);
					}
				}
				else
				{
					int selr = getSelectedRow();
					if (selr != -1)
					{
						Rows[selr].selected = false;
						Refresh();
					}
				}
			}
		}

		/// <summary>
		/// Gets the row-id of the currently selected row.
		/// </summary>
		/// <returns>the currently selected row-id</returns>
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
		/// Handles a mouseclick on the colhead. Selects or deselects col(s).
		/// Fires only on the colhead panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void click_ColPanel(object sender, MouseEventArgs e)
		{
			if (!_panelCols._grab)
			{
				if (RowCount != 0)
				{
					if (e.Button == MouseButtons.Left)
					{
						Select();

						_editor.Visible = false;

						int x = e.X + offsetHori;

						int left = getLeft();
						int c = FrozenCount - 1;
						do
						{
							if (++c == ColCount)
							{
								// NOTE: Clearing the selected col is confusing
								// without clearing all cells in the col also.
//								int selc = getSelectedCol();
//								if (selc != -1)
//									Cols[selc].selected = false;

								return;
							}
						}
						while ((left += Cols[c].width()) < x);


						bool select = false;

						if ((ModifierKeys & Keys.Shift) != Keys.Shift) // else Shift always selects all col-cells
						{
							for (int r = 0; r != RowCount; ++r)
							{
								if (!this[r,c].selected)
								{
									select = true;
									break;
								}
							}
						}
						else
							select = true;


						if ((ModifierKeys & Keys.Control) != Keys.Control)
						{
							if (!select)
							{
								for (int i = 0; i != RowCount; ++i) // if other col-cells are currently selected and Ctrl is NOT
								for (int j = 0; j != ColCount; ++j) // pressed force the col to select after clearing all selects.
								{
									if (j != c && this[i,j].selected)
									{
										select = true;
										break;
									}
								}
							}

							ClearCellSelects();
							foreach (var row in Rows) // clear row-select if Ctrl is NOT pressed
								row.selected = false;
						}

						if ((ModifierKeys & Keys.Shift) == Keys.Shift)
						{
							int selc = getSelectedCol();
							if (selc != -1)
							{
								int start, stop;
								if (selc < c)
								{
									start = selc;
									stop  = c;
								}
								else
								{
									start = c;
									stop  = selc;
								}

								while (start != stop + 1)
								{
									if (start != c) // done below
									{
										for (int r = 0; r != RowCount; ++r)
											this[r,start].selected = true;
									}
									++start;
								}
							}
						}
						else
						{
							foreach (var col in Cols)
								col.selected = false;

							Cols[c].selected = select;
						}

						if (select)
							EnsureDisplayedCol(c);

						for (int r = 0; r != RowCount; ++r)
							this[r,c].selected = select;

						Refresh();
					}
					else if (e.Button == MouseButtons.Right)
					{
						if ((ModifierKeys & Keys.Shift) == Keys.Shift) // Shift+RMB = sort by col
						{
							_editor.Visible = false;
							Select();

							int x = e.X + offsetHori;

							int left = getLeft();
							int c = FrozenCount - 1;
							do
							{
								if (++c == ColCount)
								{
									// NOTE: Clearing the selected col is confusing
									// without clearing all cells in the col also.
//									int selc = getSelectedCol();
//									if (selc != -1)
//										Cols[selc].selected = false;

									return;
								}
							}
							while ((left += Cols[c].width()) < x);

							ColSort(c);
							EnsureDisplayedCellOrRow();
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets the col-id of the currently selected col.
		/// </summary>
		/// <returns>the currently selected col-id</returns>
		int getSelectedCol()
		{
			for (int c = 0; c != ColCount; ++c)
			{
				if (Cols[c].selected)
					return c;
			}
			return -1;
		}


		/// <summary>
		/// Shift+RMB = sort by id
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_IdLabel(object sender, MouseEventArgs e)
		{
			if (RowCount != 0
				&& e.Button == MouseButtons.Right
				&& (ModifierKeys & Keys.Shift) == Keys.Shift)
			{
				_editor.Visible = false;
				Select();

				ColSort(0);
				EnsureDisplayedCellOrRow();
			}
		}

		/// <summary>
		/// Shift+RMB = sort by 1st col
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_FirstLabel(object sender, MouseEventArgs e)
		{
			if (RowCount != 0
				&& e.Button == MouseButtons.Right
				&& (ModifierKeys & Keys.Shift) == Keys.Shift)
			{
				_editor.Visible = false;
				Select();

				ColSort(1);
				EnsureDisplayedCellOrRow();
			}
		}

		/// <summary>
		/// Shift+RMB = sort by 2nd col
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_SecondLabel(object sender, MouseEventArgs e)
		{
			if (RowCount != 0
				&& e.Button == MouseButtons.Right
				&& (ModifierKeys & Keys.Shift) == Keys.Shift)
			{
				_editor.Visible = false;
				Select();

				ColSort(2);
				EnsureDisplayedCellOrRow();
			}
		}


		/// <summary>
		/// Inserts/deletes a row into the table.
		/// </summary>
		/// <param name="id">row-id to insert at or to delete</param>
		/// <param name="fields">null to delete the row</param>
		/// <param name="calibrate">true to re-layout the grid</param>
		/// <param name="brush">a brush to use for Undo/Redo</param>
		internal void Insert(int id, string[] fields = null, bool calibrate = true, Brush brush = null)
		{
			if (calibrate)
				DrawingControl.SuspendDrawing(this);

			Row row;

			if (fields != null)
			{
				if (brush == null)
					brush = Brushes.Created;

				row = new Row(id, ColCount, brush, this);

				string field;
				for (int c = 0; c != ColCount; ++c)
				{
					if (c < fields.Length)
						field = fields[c];
					else
						field = Constants.Stars;

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
			else // delete 'id'
			{
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
						row[c] = new Cell(id, c, Constants.Stars);

					Rows.Add(row);

					if (calibrate)
					{
						Calibrate(0);
						DrawingControl.ResumeDrawing(this);

						return;
					}
				}
			}

			if (calibrate) // is only 1 row (no range) via context single-row edit
			{
				Calibrate((fields != null) ? id : -1); // insert()

				if (id < RowCount)
					EnsureDisplayedRow(id);

				DrawingControl.ResumeDrawing(this);
			}
		}


		#region Sort
		ToolTip TooltipSort = new ToolTip(); // hint when table isn't sorted by ID-asc.

		/// <summary>
		/// Sorts rows by a col either ascending or descending.
		/// @note Mergesort.
		/// </summary>
		/// <param name="col">the col-id to sort by</param>
		void ColSort(int col)
		{
			_f.tabclick_DiffReset(null, EventArgs.Empty);

			Changed = true; // TODO: do Changed only if rows are swapped/order is *actually* changed.
			_ur.ResetSaved(true);

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
		}

		/// <summary>
		/// https://en.wikipedia.org/wiki/Merge_sort#Top-down_implementation
		/// </summary>
		/// <param name="rows"></param>
		/// <param name="rowsT"></param>
		/// <param name="count"></param>
		void TopDownMergeSort(List<Row> rows, List<Row> rowsT, int count)
		{
			CopyArray(rows, 0, count, rowsT);
			TopDownSplitMerge(rowsT, 0, count, rows);
		}

		void CopyArray(IList<Row> rows, int iBegin, int iEnd, ICollection<Row> rowsT)
		{
			for (int i = iBegin; i != iEnd; ++i)
				rowsT.Add(rows[i]);
		}

		void TopDownSplitMerge(List<Row> rowsT, int iBegin, int iEnd, List<Row> rows)
		{
			if (iEnd - iBegin < 2)
				return;

			int iMiddle = (iEnd + iBegin) / 2;

			TopDownSplitMerge(rows, iBegin,  iMiddle, rowsT);
			TopDownSplitMerge(rows, iMiddle, iEnd,    rowsT);

			TopDownMerge(rowsT, iBegin, iMiddle, iEnd, rows);
		}

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

			int result;

			bool a_isStars = (_a == Constants.Stars);
			bool b_isStars = (_b == Constants.Stars);

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
					&& float.TryParse(_a, out _af) // try float comparison
					&& float.TryParse(_b, out _bf))
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
		void grid_DragEnter(object sender, DragEventArgs e)
		{
			_f.yata_DragEnter(sender, e);
		}

		void grid_DragDrop(object sender, DragEventArgs e)
		{
			_f.yata_DragDrop(sender, e);
		}
		#endregion DragDrop file(s)
	}
}
