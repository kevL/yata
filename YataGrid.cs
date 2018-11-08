using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

using yata.Properties;


namespace yata
{
	static class Pens
	{
		internal static readonly Pen DarkLine = new Pen(SystemColors.ControlDark);
	}

	static class Brushes
	{
		internal static readonly Brush Alice   = new SolidBrush(Color.AliceBlue);
		internal static readonly Brush Blanche = new SolidBrush(Color.BlanchedAlmond);
		internal static readonly Brush Created = new SolidBrush(SystemColors.ControlLight);

		internal static readonly Brush CellSel = new SolidBrush(Color.PaleGreen);
		internal static readonly Brush Edit    = new SolidBrush(Colors.Edit);
	}

	static class Colors
	{
		internal static readonly Color ColheadPanel = Color.Thistle;
		internal static readonly Color RowheadPanel = Color.Azure;

		internal static readonly Color FrozenHead  = Color.Moccasin;
		internal static readonly Color FrozenPanel = Color.OldLace;

		internal static readonly Color Edit = Color.Crimson;

		internal static readonly Color Text = SystemColors.ControlText;
	}


	sealed class YataGrid
		:
			Control
	{
		internal string Pfe // Path-File-Extension (ie. fullpath)
		{ get; set; }

		readonly YataForm _f;
		YataGrid _table; // for cycling through all tables

		bool _changed;
		internal bool Changed
		{
			get { return _changed; }
			set { _f.TableChanged(_changed = value); }
		}

		internal static int HeightColhead;
		internal static int WidthRowhead;

		internal int HeightRow;

		internal int ColCount;
		internal int RowCount;

		internal string[] Fields // 'Fields' does NOT contain #0 col IDs (so that typically needs +1)
		{ get; set; }

		readonly List<string[]> _rows = new List<string[]>();

		internal readonly List<Col> Cols = new List<Col>();
		internal readonly List<Row> Rows = new List<Row>();

		Cell[,] _cells;
		Cell[,] _cells0; // a cache of the originally loaded data that shall be pure
		/// <summary>
		/// Gets the cell at pos [r,c].
		/// NOTE: 1st dimension is rows, 2nd dimension is cols. That's better
		/// for resizing the array.
		/// </summary>
		internal Cell this[int r, int c]
		{
			get
			{
//				if (_cells0 != null)
//					return _cells0[r,c];

				return _cells[r,c];
			}
			set
			{
//				if (_cells0 != null)
//					_cells0[r,c] = value;
//				else
					_cells[r,c] = value;
			}
		}
//		internal Cell this[int r, int c]
//		{
//			get { return _cells[r,c]; }
//			set { _cells[r,c] = value; }
//		}


		internal static Graphics graphics_; // is used to paint crap.
		static Graphics _graphics; // is used only for MeasureText()

//		Bitmap _bluePi = Resources.bluepixel;
//		Bitmap _piColhead;
//		Bitmap _piRowhead;


		const int _padHori        = 6;
		const int _padVert        = 4;
		const int _padHoriRowhead = 8;
		const int _padHoriSort    = 12;


		TextFormatFlags _flags = TextFormatFlags.NoClipping | TextFormatFlags.NoPrefix
															| TextFormatFlags.NoPadding
															| TextFormatFlags.Left
															| TextFormatFlags.VerticalCenter
															| TextFormatFlags.SingleLine;
		Size _size = new Size(int.MaxValue, int.MaxValue);


		internal bool Craft
		{ get; set; }


		readonly VScrollBar _scrollVert = new VScrollBar();
		readonly HScrollBar _scrollHori = new HScrollBar();

		bool _visVert; // Be happy. happy happy
		bool _visHori;

		internal int offsetVert;
		int offsetHori;

		int HeightTable;
		int WidthTable;


		YataPanelCols _panelCols;
		YataPanelRows _panelRows;

		YataPanelFrozen _panelFrozen;

		Label _labelid     = new Label();
		Label _labelfirst  = new Label();
		Label _labelsecond = new Label();

		internal const int FreezeId     = 1; // qty of Cols that are frozen ->
		internal const int FreezeFirst  = 2;
		internal const int FreezeSecond = 3;

		int _frozenCount = FreezeId; // starts out w/ id-col only.
		internal int FrozenCount
		{
			get { return _frozenCount; }
			set
			{
				_frozenCount = value;

				int w = 0;
				for (int c = 0; c != _frozenCount; ++c)
				{
					w += Cols[c].width();
				}
				_panelFrozen.Width = w;

				_panelFrozen.Refresh();

				_labelfirst .Visible = (_frozenCount > FreezeId);
				_labelsecond.Visible = (_frozenCount > FreezeFirst);
			}
		}


		static Timer _t1 = new Timer(); // hides info on the statusbar when mouse leaves the table-area

		internal readonly TextBox _editor = new TextBox();
		Cell _editcell;

		static bool _init;

		int _sortcol;
		int _sortdir;

		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="pfe"></param>
		internal YataGrid(YataForm f, string pfe)
		{
			//logfile.Log("YataGrid() cTor");

//			DrawingControl.SetDoubleBuffered(this);
//			DoubleBuffered = true;

			SetStyle(ControlStyles.OptimizedDoubleBuffer
				   | ControlStyles.AllPaintingInWmPaint
				   | ControlStyles.UserPaint
				   | ControlStyles.ResizeRedraw, true);

			_f = f;
			_graphics = CreateGraphics(); //Graphics.FromHwnd(IntPtr.Zero))

			Pfe = pfe;
			_init = true;

			Dock = DockStyle.Fill;
			BackColor = SystemColors.ControlDark;

//			this.ProcessKeyPreview();
//			this.OnPreviewKeyDown();

			_scrollVert.Dock = DockStyle.Right;
			_scrollVert.ValueChanged += OnVertScrollValueChanged;

			_scrollHori.Dock = DockStyle.Bottom;
			_scrollHori.ValueChanged += OnHoriScrollValueChanged;

			Controls.Add(_scrollHori);
			Controls.Add(_scrollVert);

			_t1.Interval = 223;
			_t1.Enabled = true; // TODO: stop Timer when no table is loaded /shrug.
			_t1.Tick += t1_Tick;

			_editor.BackColor = Colors.Edit;
			_editor.Visible = false;
			_editor.BorderStyle = BorderStyle.None;

			Controls.Add(_editor);
		}


		void OnVertScrollValueChanged(object sender, EventArgs e)
		{
			if (_table == null) _table = this;

			if (!_table._editor.Visible)
			{
				_table.offsetVert = _table._scrollVert.Value;
				_table.Refresh();
			}
			else
				_table._scrollVert.Value = _table.offsetVert;

			if (!_f._search)
				Select(); // workaround: refocus the table (bar has to move > 0px)

			var pt = PointToClient(Cursor.Position);
			var args = new MouseEventArgs(MouseButtons.Left, 1, pt.X, pt.Y, 0); // clicks,x,y,delta
			OnMouseMove(args); // update coords on the Statusbar
		}

		void OnHoriScrollValueChanged(object sender, EventArgs e)
		{
			if (_table == null) _table = this;

			if (!_table._editor.Visible)
			{
				_table.offsetHori = _table._scrollHori.Value;
				_table.Refresh();
			}
			else
				_table._scrollHori.Value = _table.offsetHori;

			if (!_f._search)
				Select(); // workaround: refocus the table (bar has to move > 0px)

			var pt = PointToClient(Cursor.Position);
			var args = new MouseEventArgs(MouseButtons.Left, 1, pt.X, pt.Y, 0); // clicks,x,y,delta
			OnMouseMove(args); // update coords on the Statusbar
		}

		protected override void OnResize(EventArgs e)
		{
			//logfile.Log("OnResize()");
			if (!_init)
			{
				for (int tab = 0; tab != _f.Tabs.TabCount; ++tab)
				{
					_table = _f.Tabs.TabPages[tab].Tag as YataGrid;

					//logfile.Log(". " + Path.GetFileNameWithoutExtension(_table.Pfe));

					_table.InitScrollers();

					// NOTE: The panels can be null during the load sequence.
					if (_table._panelCols   != null) _table._panelCols  .Width  = Width;
					if (_table._panelRows   != null) _table._panelRows  .Height = Height;
					if (_table._panelFrozen != null) _table._panelFrozen.Height = Height;

					_table.Refresh(); // _table-drawing can tear without that.
				}
				_table = null;

//				if (_piColhead != null) _piColhead.Dispose();
//				_piColhead = new Bitmap(_bluePi, new Size(WidthTable, HeightColhead));

//				if (_piRowhead != null) _piRowhead.Dispose();
//				_piRowhead = new Bitmap(_bluePi, new Size(WidthRowhead, HeightTable));
			}
			//else logfile.Log(". bypass");

//			base.OnResize(e);
		}


		/// <summary>
		/// Initializes the vertical and horizontal scrollbars OnResize (which
		/// also happens auto after load).
		/// </summary>
		void InitScrollers()
		{
			//logfile.Log("InitScrollers() " + Path.GetFileNameWithoutExtension(Pfe));
			//logfile.Log(". Height= " + Height);

			HeightTable = HeightColhead + HeightRow * RowCount;

			WidthTable = WidthRowhead;
			for (int c = 0; c != ColCount; ++c)
				WidthTable += Cols[c].width();

			//logfile.Log("Width/Height= " + WidthTable + "/" + HeightTable);

			// NOTE: Height/Width *includes* the height/width of the relevant
			// scrollbar and panel.

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
						 + _scrollVert.LargeChange
						 - Height
						 + (_visHori ? _scrollHori.Height : 0)
						 - 1;

				if (vert < _scrollVert.LargeChange) vert = 0;

				_scrollVert.Maximum = vert;	// NOTE: Do not set this until after deciding
											// whether or not max < 0. 'Cause it fucks everything up. bingo.

				// handle .NET OnResize anomaly ->
				// keep the bottom of the table snuggled against the bottom of the visible area
				if (HeightTable - offsetVert < Height - (_visHori ? _scrollHori.Height : 0))
				{
					_scrollVert.Value = HeightTable - Height + (_visHori ? _scrollHori.Height : 0);
				}
			}
			else
				_scrollVert.Value = 0;

			if (_visHori)
			{
				int hori = WidthTable
						 + _scrollHori.LargeChange
						 - Width
						 + (_visVert ? _scrollVert.Width : 0)
						 - 1;

				if (hori < _scrollHori.LargeChange) hori = 0;

				_scrollHori.Maximum = hori;	// NOTE: Do not set this until after deciding
											// whether or not max < 0. 'Cause it fucks everything up. bingo.

				// handle .NET OnResize anomaly ->
				// keep the right of the table snuggled against the right of the visible area
				if (WidthTable - offsetHori < Width - (_visVert ? _scrollVert.Width : 0))
				{
					_scrollHori.Value = WidthTable - Width + (_visVert ? _scrollVert.Width : 0);
				}
			}
			else
				_scrollHori.Value = 0;
		}

		/// <summary>
		/// Scrolls the table by the mousewheel.
		/// @note Fired from the form's OnMouseWheel event to catch all
		/// unhandled MouseWheel events hovered on the app (without firing
		/// twice).
		/// </summary>
		/// <param name="e"></param>
//		protected override void OnMouseWheel(MouseEventArgs e)
		internal void Scroll(MouseEventArgs e)
		{
//			var args = e as HandledMouseEventArgs;
//			if (args != null)
//				args.Handled = true;

			if (!_editor.Visible)
			{
				if (_scrollVert.Visible)
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
				else if (_scrollHori.Visible)
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

//			base.OnMouseWheel(e);
		}


		/// <summary>
		/// Handles the paint event.
		/// @note OnPaint() doesn't want to use the class_var '_graphics'.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			//logfile.Log("OnPaint()");

//			if (ColCount != 0 && RowCount != 0 && _cells != null)

			if (!_init)
			{
				graphics_ = e.Graphics;
				graphics_.PixelOffsetMode = PixelOffsetMode.HighQuality;

//				ControlPaint.DrawBorder3D(_graphics, ClientRectangle, Border3DStyle.Etched);


				int r,c;

				// NOTE: Paint backgrounds full-height/width of table ->

				// rows background - scrollable
				var rect = new Rectangle(Left, HeightColhead - offsetVert, WidthTable, HeightRow);

				for (r = 0; r != RowCount; ++r)
				{
					graphics_.FillRectangle(Rows[r]._brush, rect);
					rect.Y += HeightRow;
				}

				// cell text - scrollable
				rect.Height = HeightRow;
				int left = WidthRowhead - offsetHori + _padHori;
				int yOffset = HeightColhead - offsetVert;
				for (r = offsetVert / HeightRow; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r + yOffset) > Bottom)
						break;

					rect.X = left;
					for (c = 0; c != ColCount; ++c)
					{
						if (rect.X + (rect.Width = Cols[c].width()) > WidthRowhead)
						{
							var cell = _cells[r,c];
							if (cell.selected)
							{
								rect.X -= _padHori;

								if (_editor.Visible && _editcell == cell)
									graphics_.FillRectangle(Brushes.Edit, rect);
								else
									graphics_.FillRectangle(Brushes.CellSel, rect);

								rect.X += _padHori;
							}

							TextRenderer.DrawText(graphics_, cell.text, Font, rect, Colors.Text, _flags);
//							graphics_.DrawRectangle(new Pen(Color.Crimson), rect); // DEBUG
						}

						if ((rect.X += rect.Width) > Right)
							break;
					}
				}


//				using (var pi = new Bitmap(_bluePi, new Size(WidthTable, HeightColhead)))
//				if (_piColhead != null) graphics_.DrawImage(_piColhead, 0,0);

//				using (var pi = new Bitmap(_bluePi, new Size(WidthRowhead, HeightTable)))
//				if (_piRowhead != null) graphics_.DrawImage(_piRowhead, 0,0);


				// NOTE: Paint horizontal lines full-width of table.

				// row lines - scrollable
				int y;
				for (r = 1; r != RowCount + 1; ++r)
				{
					if ((y = HeightRow * r + yOffset) > Bottom)
						break;

					if (y > HeightColhead)
						graphics_.DrawLine(Pens.DarkLine, Left, y, WidthTable, y);
				}


				// NOTE: Paint vertical lines full-height of table.

				// col lines - scrollable
				int x = WidthRowhead - offsetHori;
				for (c = 0; c != ColCount; ++c)
				{
					if ((x += Cols[c].width()) > Right)
						break;

					if (x > WidthRowhead)
						graphics_.DrawLine(Pens.DarkLine, x, Top, x, Bottom);
				}
			}
//			base.OnPaint(e);
		}

		/// <summary>
		/// Labels the colheads.
		/// @note Called by OnPaint of the top-panel.
		/// @note OnPaint() doesn't want to use the class_var '_graphics'.
		/// </summary>
		internal void LabelColheads()
		{
			//logfile.Log("LabelColheads()");

			if (ColCount != 0) // safety.
			{
				var rect = new Rectangle(WidthRowhead - offsetHori + _padHori, 0, 0, HeightColhead);

				for (int c = 0; c != ColCount; ++c)
				{
					if (rect.X + (rect.Width = Cols[c].width()) > Left)
					{
						TextRenderer.DrawText(graphics_, Cols[c].text, _f.FontAccent, rect, Colors.Text, _flags);

//						if (SortedColumn == null) // draw an asc-arrow on the ID col-header when table loads
//						{
//							if (col == 0)
//								sort = Resources.asc_16px;
//						}
//						else
						if (_sortdir != 0 && c == _sortcol)
						{
							Bitmap sort;
							if (_sortdir == 1) // asc
								sort = Resources.asc_16px;
							else //if (_sortdir == -1) // des
								sort = Resources.des_16px;

							graphics_.DrawImage(sort,
												rect.X + rect.Width  - 22,
												rect.Y + rect.Height - 15);
						}
					}

					if ((rect.X += rect.Width) > Right)
						break;
				}
			}
		}

		/// <summary>
		/// Labels the rowheads when inserting/deleting/sorting rows.
		/// @note Called by OnPaint of the left-panel.
		/// @note OnPaint() doesn't want to use the class_var '_graphics'.
		/// </summary>
		internal void LabelRowheads()
		{
			//logfile.Log("LabelRowheads()");

			if (RowCount != 0) // safety - ought be checked in calling funct.
			{
//				_load = true; // (re)use '_load' to prevent firing CellChanged events for the Rowheads

				var rect = new Rectangle(_padHoriRowhead - 1, 0, WidthRowhead, HeightRow); // NOTE: -1 is a padding tweak.

				for (int r = offsetVert / HeightRow; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r - offsetVert) > Height)
						break;

					TextRenderer.DrawText(graphics_, r.ToString(), _f.FontAccent, rect, Colors.Text, _flags);
				}
//				_load = false;
			}
		}

		void labelid_Paint(object sender, PaintEventArgs e)
		{
			graphics_ = e.Graphics;
			graphics_.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var rect = new Rectangle(WidthRowhead + _padHori, Top, Cols[0].width(), HeightColhead);
			TextRenderer.DrawText(graphics_, "id", _f.FontAccent, rect, Colors.Text, _flags);

			graphics_.DrawLine(Pens.DarkLine, _labelid.Width, _labelid.Top, _labelid.Width, _labelid.Bottom);

			if (_sortdir != 0 && _sortcol == 0)
			{
				Bitmap sort;
				if (_sortdir == 1) // asc
					sort = Resources.asc_16px;
				else //if (_sortdir == -1) // des
					sort = Resources.des_16px;

				graphics_.DrawImage(sort,
									rect.X + rect.Width  - 22,
									rect.Y + rect.Height - 15);
			}
		}

		void labelfirst_Paint(object sender, PaintEventArgs e)
		{
			graphics_ = e.Graphics;
			graphics_.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var rect = new Rectangle(_padHori, Top, Cols[1].width(), HeightColhead);
			TextRenderer.DrawText(graphics_, Cols[1].text, _f.FontAccent, rect, Colors.Text, _flags);

			graphics_.DrawLine(Pens.DarkLine, _labelfirst.Width, _labelfirst.Top, _labelfirst.Width, _labelfirst.Bottom);

			if (_sortdir != 0 && _sortcol == 1)
			{
				Bitmap sort;
				if (_sortdir == 1) // asc
					sort = Resources.asc_16px;
				else //if (_sortdir == -1) // des
					sort = Resources.des_16px;

				graphics_.DrawImage(sort,
									rect.X + rect.Width  - 22,
									rect.Y + rect.Height - 15);
			}
		}

		void labelsecond_Paint(object sender, PaintEventArgs e)
		{
			graphics_ = e.Graphics;
			graphics_.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var rect = new Rectangle(_padHori, Top, Cols[2].width(), HeightColhead);
			TextRenderer.DrawText(graphics_, Cols[2].text, _f.FontAccent, rect, Colors.Text, _flags);

			graphics_.DrawLine(Pens.DarkLine, _labelsecond.Width, _labelsecond.Top, _labelsecond.Width, _labelsecond.Bottom);

			if (_sortdir != 0 && _sortcol == 2)
			{
				Bitmap sort;
				if (_sortdir == 1) // asc
					sort = Resources.asc_16px;
				else //if (_sortdir == -1) // des
					sort = Resources.des_16px;

				graphics_.DrawImage(sort,
									rect.X + rect.Width  - 22,
									rect.Y + rect.Height - 15);
			}
		}

		/// <summary>
		/// Labels the frozen cols.
		/// @note Called by OnPaint of the frozen panel.
		/// @note OnPaint() doesn't want to use the class_var '_graphics'.
		/// </summary>
		internal void PaintFrozenPanel()
		{
			//logfile.Log("LabelFrozen()");

			if (RowCount != 0) // safety.
			{
				int x = 0;
				int c = 0;
				for (; c != FrozenCount; ++c)
				{
					x += Cols[c].width();
					graphics_.DrawLine(Pens.DarkLine, x, 0, x, Height);
				}
//				graphics_.DrawLine(Pens.DarkLine, x - 1, 0, x - 1, Height);

				var rect = new Rectangle(0,0, 0, HeightRow);

				for (int r = offsetVert / HeightRow; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r - offsetVert) > Height)
						break;

					rect.X = _padHori - 1; // NOTE: -1 is a padding tweak.

					for (c = 0; c != FrozenCount; ++c)
					{
						rect.Width = Cols[c].width();
						TextRenderer.DrawText(graphics_, _cells[r,c].text, Font, rect, Colors.Text, _flags);

						rect.X += rect.Width;
					}

					graphics_.DrawLine(Pens.DarkLine, 0, rect.Y + HeightRow, rect.X + rect.Width, rect.Y + HeightRow);
				}
			}
		}


		const int LABELS = 2;

		/// <summary>
		/// Tries to load a 2da file.
		/// </summary>
		/// <returns>true if 2da loaded successfully perhaps</returns>
		internal bool Load2da()
		{
			//logfile.Log("Load2da()");

			Text = "Yata";

			bool ignoreErrors = false;

			string[] lines = File.ReadAllLines(Pfe);

			Fields = lines[LABELS].Split(new char[0], StringSplitOptions.RemoveEmptyEntries); // TODO: test for double-quotes

			int quotes =  0;
			int id     = -1;
			int lineId = -1;

			// TODO: Test for an even quantity of double-quotes on each line.
			// ie. Account for the fact that ParseLine() needs to ASSUME that quotes are fairly accurate.

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
					string[] row = Parse2daRow(line);

					// allow blank lines on load - they will be removed if/when file is saved.
					if (row.Length != 0)
					{
						// test for well-formed, consistent IDs
						++id;

						if (!ignoreErrors)
						{
							int result;
							if (!Int32.TryParse(row[0], out result) || result != id)
							{
								string error = "The 2da-file contains an ID that is not an integer or is out of order."
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
						if (!ignoreErrors && row.Length != Fields.Length + 1)
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

			return true;
		}


		internal void Init(bool reload = false)
		{
			//logfile.Log("Init()");

			if (reload)
			{
				_init = true;

				Changed = false;

				_editor.Visible = false;

				_scrollVert.Value =
				_scrollHori.Value = 0;

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
			else if (Craft = (Path.GetFileNameWithoutExtension(Pfe).ToLower() == "crafting"))
			{
				foreach (var dir in Settings._pathall)
					_f.GropeLabels(dir);
			}


//			DrawingControl.SuspendDrawing(this);	// NOTE: Drawing resumes after autosize in either
													// YataForm.CreateTabPage() or YataForm.ReloadToolStripMenuItemClick().

			_panelCols = new YataPanelCols(this);
			_panelCols.MouseClick += click_ColPanel;
			_panelRows = new YataPanelRows(this);
			_panelRows.MouseClick += click_RowPanel;

			CreateCols();
			CreateRows();
			CreateCells();

			_panelFrozen = new YataPanelFrozen(this, Cols[0].width());
			FrozenLabelsInit();

			SetStaticHeads();

			Controls.Add(_panelFrozen);
			Controls.Add(_panelRows);
			Controls.Add(_panelCols);


			_scrollVert.LargeChange =
			_scrollHori.LargeChange = HeightRow;
			InitScrollers();

			Select();

			_init = false;
		}

		/// <summary>
		/// Redimensions everything when Font changes.
		/// </summary>
		internal void Calibrate()
		{
			//logfile.Log("Calibrate()");

			_init = true;

			_editor.Visible = false;

//			_scrollVert.Value =
//			_scrollHori.Value = 0;

			Controls.Remove(_panelCols);
			Controls.Remove(_panelRows);
			Controls.Remove(_panelFrozen);

			_panelCols = new YataPanelCols(this);
			_panelCols.MouseClick += click_ColPanel;
			_panelRows = new YataPanelRows(this);
			_panelRows.MouseClick += click_RowPanel;

			CreateCols(true);
			CreateCells(true);

			_panelFrozen = new YataPanelFrozen(this, Cols[0].width());
			FrozenCount = FrozenCount; // refresh the Frozen panel
			FrozenLabelsInit();

			SetStaticHeads();

			Controls.Add(_panelFrozen);
			Controls.Add(_panelRows);
			Controls.Add(_panelCols);


			_scrollVert.LargeChange =
			_scrollHori.LargeChange = HeightRow;
			InitScrollers();

			Select();

			_init = false;
		}

		/// <summary>
		/// Creates the cols and caches the 2da's colhead data.
		/// </summary>
		/// <param name="calibrate">true to only adjust dimensions</param>
		void CreateCols(bool calibrate = false)
		{
			//logfile.Log("CreateCols()");

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
			else
			{
				_panelCols.Height = 10; // reset
				Cols[0].width(0, true); // reset
			}

			Size size;
			int h; c = 0;
			foreach (string head in Fields)
			{
				++c;

				if (!calibrate)
					Cols[c].text = head;

				size = TextRenderer.MeasureText(_graphics, head, _f.FontAccent, _size, _flags);
				Cols[c].width(size.Width + _padHori * 2 + _padHoriSort, calibrate);

				h = size.Height + _padVert * 2;
				if (h > _panelCols.Height)
					_panelCols.Height = h;
			}
		}

		/// <summary>
		/// Creates the rows.
		/// </summary>
		void CreateRows()
		{
			//logfile.Log("CreateRows()");

			RowCount = _rows.Count;

			Brush brush;
			for (int r = 0; r != RowCount; ++r)
			{
				brush = (r % 2 == 0) ? Brushes.Alice
									 : Brushes.Blanche;
				Rows.Add(new Row(r, brush));
			}
		}

		/// <summary>
		/// Creates the cells' 2d-array.
		/// </summary>
		/// <param name="calibrate">true to only adjust (ie. Font changed)</param>
		void CreateCells(bool calibrate = false)
		{
			//logfile.Log("CreateCells()");
			//logfile.Log(". RowCount= " + RowCount);
			//logfile.Log(". ColCount= " + ColCount);

			if (!calibrate)
			{
				_cells0 = new Cell[RowCount, ColCount];	// '_cells0' is not currently used ......
														// To be useful any Changed event would have to compare '_cells' against '_cells0'
														// Or to revert Sorting it would need to track any inserted/deleted cells
				for (int r = 0; r != RowCount; ++r)
				for (int c = 0; c != ColCount; ++c)
				{
					//logfile.Log(". text= " + _rows[r][c]);
					_cells0[r,c] = new Cell(r,c, _rows[r][c]);
				}
				_cells = ArrayCopy<Cell[,]>.CloneCells(_cells0); // '_cells' is the table that gets modified/sorted/saved.
			}
			else
				HeightRow = 0; // reset

			_rows.Clear(); // done w/ '_rows'

			Size size;
			int w, wT, hT;

			//logfile.Log("ColCount= " + ColCount);
			for (int c = 0; c != ColCount; ++c)
			{
				//logfile.Log(". c= " + c);

				w = 20; // cellwidth min.
				for (int r = 0; r != RowCount; ++r)
				{
					//logfile.Log(". . r= " + r + " text= " + _cells[r,c].text);
					size = TextRenderer.MeasureText(_graphics, _cells[r,c].text, Font, _size, _flags);

					wT = size.Width + _padHori * 2 + _padHoriSort;
					if (wT > w) w = wT;

					hT = size.Height + _padVert * 2;
					if (hT > HeightRow) HeightRow = hT;
				}
				Cols[c].width(w);
				//logfile.Log(". c= " + c + " width= " + Cols[c].width());
			}
			//logfile.Log(". HeightRow= " + HeightRow);
		}

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

				DrawingControl.SetDoubleBuffered(_labelid);
				_labelid.BackColor = Colors.FrozenHead;

				_labelid.Paint += labelid_Paint;
				_labelid.MouseClick += click_IdLabel;
				_labelid.MouseClick += (sender, e) => Select();

				_panelCols.Controls.Add(_labelid);

				if (ColCount > 1)
				{
					_labelfirst.Visible = (_frozenCount > FreezeId); // required after Font calibration

					DrawingControl.SetDoubleBuffered(_labelfirst);
					_labelfirst.BackColor = Colors.FrozenHead;

					_labelfirst.Paint += labelfirst_Paint;
					_labelfirst.MouseClick += click_FirstLabel;
					_labelfirst.MouseClick += (sender, e) => Select();

					_panelCols.Controls.Add(_labelfirst);

					if (ColCount > 2)
					{
						_labelsecond.Visible = ((_frozenCount > FreezeFirst)); // required after Font calibration

						DrawingControl.SetDoubleBuffered(_labelsecond);
						_labelsecond.BackColor = Colors.FrozenHead;

						_labelsecond.Paint += labelsecond_Paint;
						_labelsecond.MouseClick += click_SecondLabel;
						_labelsecond.MouseClick += (sender, e) => Select();

						_panelCols.Controls.Add(_labelsecond);
					}
				}
			}
		}


		/// <summary>
		/// Calculates and maintains rowhead width wrt/ current Font across all
		/// tabs/tables.
		/// </summary>
		void SetStaticHeads()
		{
			//logfile.Log("SetStaticHeads()");

			YataGrid table;

			int heightColhead = 10;				// col-headers' height stays uniform across all tabpages
			int widthRowhead  = 20, testWidth;	// row-headers' width stays uniform across all tabpages

			int tabs = _f.Tabs.TabCount;
			int tab = 0;
			for (; tab != tabs; ++tab)
			{
				table = _f.Tabs.TabPages[tab].Tag as YataGrid;
				if (table._panelCols.Height > heightColhead)
					heightColhead = table._panelCols.Height;

				if ((testWidth = table.RowCount - 1) > widthRowhead)
					widthRowhead = testWidth;
			}

			string text = "9";
			int w = 1;
			while ((widthRowhead /= 10) != 0)
			{
				++w;
				text += "9";
			}

			HeightColhead = heightColhead;
			WidthRowhead = TextRenderer.MeasureText(_graphics, text, _f.FontAccent, _size, _flags).Width + _padHoriRowhead * 2;

			for (tab = 0; tab != tabs; ++tab)
			{
				table = _f.Tabs.TabPages[tab].Tag as YataGrid;

				table.WidthTable = WidthRowhead;
				for (int c = 0; c != table.ColCount; ++c)
					table.WidthTable += table.Cols[c].width();

				table._panelCols.Height = HeightColhead;
				table._panelRows.Width  = WidthRowhead;

				FrozenLabelsSet(table);
			}
		}

		void FrozenLabelsSet(YataGrid table)
		{
			if (table.ColCount != 0)
			{
				table._labelid.Location = new Point(0,0);
				table._labelid.Size = new Size(WidthRowhead + table.Cols[0].width(), HeightColhead - 1);

				if (table.ColCount > 1)
				{
					table._labelfirst.Location = new Point(WidthRowhead + table.Cols[0].width(), 0);
					table._labelfirst.Size = new Size(table.Cols[1].width(), HeightColhead - 1);

					if (table.ColCount > 2)
					{
						table._labelsecond.Location = new Point(WidthRowhead + table.Cols[0].width() + table.Cols[1].width(), 0);
						table._labelsecond.Size = new Size(table.Cols[2].width(), HeightColhead - 1);
					}
				}
			}
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
		/// TODO: optimize.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		static string[] Parse2daRow(string line)
		{
			var list  = new List<string>();
			var field = new List<char>();

			bool add      = false;
			bool inQuotes = false;

			char c;

			int posLast = line.Length + 1; // include an extra iteration to get the last field (that has no whitespace after it)
			for (int pos = 0; pos != posLast; ++pos)
			{
				if (pos == line.Length)	// hit lineend -> add the last field
				{						// if there's no whitespace after it (last fields
					if (add)			// w/ trailing whitespace are dealt with below)
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


		/// <summary>
		/// Disables navigation etc. keys to allow table scrolling on certain
		/// key-events.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Up:
				case Keys.Down:
				case Keys.Left:
				case Keys.Right:
				case Keys.Escape:
					e.IsInputKey = true;
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			Cell sel = GetOnlySelectedCell();

			bool display = false;

			switch (e.KeyCode)
			{
				case Keys.Home:
					if ((e.Modifiers & Keys.Control) == Keys.Control)
					{
						if (sel != null)
						{
							if (sel.x != FrozenCount || sel.y != 0)
							{
								display = true;

								sel.selected = false;
								sel = _cells[0, FrozenCount];
								sel.selected = true;
							}
						}
						else if (_scrollVert.Visible)
							_scrollVert.Value = 0;
					}
					else if (sel != null)
					{
						if (sel.x != FrozenCount)
						{
							display = true;

							sel.selected = false;
							sel = _cells[sel.y, FrozenCount];
							sel.selected = true;
						}
					}
					else if (_scrollHori.Visible)
						_scrollHori.Value = 0;

					break;

				case Keys.End:
					if ((e.Modifiers & Keys.Control) == Keys.Control)
					{
						if (sel != null)
						{
							if (sel.x != ColCount - 1 || sel.y != RowCount - 1)
							{
								display = true;

								sel.selected = false;
								sel = _cells[RowCount - 1, ColCount - 1];
								sel.selected = true;
							}
						}
						else if (_scrollVert.Visible)
							_scrollVert.Value = HeightTable - Height + ((_scrollHori.Visible) ? _scrollHori.Height : 0);
					}
					else if (sel != null)
					{
						if (sel.x != ColCount - 1)
						{
							display = true;

							sel.selected = false;
							sel = _cells[sel.y, ColCount - 1];
							sel.selected = true;
						}
					}
					else if (_scrollHori.Visible)
					{
						_scrollHori.Value = WidthTable - Width + ((_scrollVert.Visible) ? _scrollVert.Width : 0);
					}
					break;

				case Keys.PageUp:
					if (sel != null)
					{
						if (sel.y != 0)
						{
							display = true;

							sel.selected = false;
							int rows = (Height - HeightColhead - (_scrollHori.Visible ? _scrollHori.Height : 0)) / HeightRow;
							logfile.Log("rows= " + rows);
							if (sel.y < rows)
								sel = _cells[0, sel.x];
							else
								sel = _cells[sel.y - rows, sel.x];

							sel.selected = true;
						}
					}
					else if (_scrollVert.Visible)
					{
						int h = Height - HeightColhead - (_scrollHori.Visible ? _scrollHori.Height : 0);

						if (_scrollVert.Value - h < 0)
							_scrollVert.Value = 0;
						else
							_scrollVert.Value -= h;
					}
					break;

				case Keys.PageDown:
					if (sel != null)
					{
						if (sel.y != RowCount - 1)
						{
							display = true;

							sel.selected = false;
							int rows = (Height - HeightColhead - (_scrollHori.Visible ? _scrollHori.Height : 0)) / HeightRow;
							logfile.Log("rows= " + rows);
							if (sel.y > RowCount - 1 - rows)
								sel = _cells[RowCount - 1, sel.x];
							else
								sel = _cells[sel.y + rows, sel.x];

							sel.selected = true;
						}
					}
					else if (_scrollVert.Visible)
					{
						int h = Height - HeightColhead - (_scrollHori.Visible ? _scrollHori.Height : 0);

						if (_scrollVert.Value + h + (_scrollVert.LargeChange - 1) > _scrollVert.Maximum)
							_scrollVert.Value = _scrollVert.Maximum - (_scrollVert.LargeChange - 1);
						else
							_scrollVert.Value += h;
					}
					break;

				case Keys.Up: // NOTE: Needs to bypass KeyPreview
					if (sel != null) // selection to the cell above
					{
						if (sel.y != 0)
						{
							// TODO: Multi-selecting cells w/ keyboard would require tracking a "current" cell.
//							cell.selected &= ((ModifierKeys & Keys.Control) == Keys.Control);

							display = true;

							sel.selected = false;
							sel = _cells[sel.y - 1, sel.x];
							sel.selected = true;
						}
					}
					else if (_scrollVert.Visible)
					{
						if (_scrollVert.Value - _scrollVert.LargeChange < 0)
							_scrollVert.Value = 0;
						else
							_scrollVert.Value -= _scrollVert.LargeChange;
					}
					break;

				case Keys.Down: // NOTE: Needs to bypass KeyPreview
					if (sel != null) // selection to the cell below
					{
						if (sel.y != RowCount - 1)
						{
							display = true;

							sel.selected = false;
							sel = _cells[sel.y + 1, sel.x];
							sel.selected = true;
						}
					}
					else if (_scrollVert.Visible) // scroll the table
					{
						if (_scrollVert.Value + _scrollVert.LargeChange + (_scrollVert.LargeChange - 1) > _scrollVert.Maximum) // what is this witchcraft
							_scrollVert.Value = _scrollVert.Maximum - (_scrollVert.LargeChange - 1);
						else
							_scrollVert.Value += _scrollVert.LargeChange;
					}
					break;

				case Keys.Left: // NOTE: Needs to bypass KeyPreview
					if (sel != null) // selection to the cell left
					{
						if (sel.x != FrozenCount)
						{
							display = true;

							sel.selected = false;
							sel = _cells[sel.y, sel.x - 1];
							sel.selected = true;
						}
					}
					else if (_scrollHori.Visible)
					{
						if (_scrollHori.Value - _scrollHori.LargeChange < 0)
							_scrollHori.Value = 0;
						else
							_scrollHori.Value -= _scrollHori.LargeChange;
					}
					break;

				case Keys.Right: // NOTE: Needs to bypass KeyPreview
					if (sel != null) // selection to the cell right
					{
						if (sel.x != ColCount - 1)
						{
							display = true;

							sel.selected = false;
							sel = _cells[sel.y, sel.x + 1];
							sel.selected = true;
						}
					}
					else if (_scrollHori.Visible)
					{
						if (_scrollHori.Value + _scrollHori.LargeChange + (_scrollHori.LargeChange - 1) > _scrollHori.Maximum) // what is this witchcraft
							_scrollHori.Value = _scrollHori.Maximum - (_scrollHori.LargeChange - 1);
						else
							_scrollHori.Value += _scrollHori.LargeChange;
					}
					break;

				case Keys.Escape: // NOTE: Needs to bypass KeyPreview
					ClearCellSelects();
					break;
			}

			if (display) EnsureDisplayed(sel);
			Refresh();

//			e.Handled = true;
//			base.OnKeyDown(e);

//			Input.SetFlag(e.KeyCode);
//			e.Handled = true;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
//			Input.RemoveFlag(e.KeyCode);
//			e.Handled = true;
		}

		/// <summary>
		/// Handles ending editing a cell by pressing Enter or Tab - this fires
		/// during edit.
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Enter:
					if (_editor.Visible)
					{
						SetCellText();
						goto case Keys.Escape;
					}

					if ((_editcell = GetOnlySelectedCell()) != null)
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
		/// Checks if only one cell is currently selected.
		/// </summary>
		/// <returns>the only cell selected</returns>
		internal Cell GetOnlySelectedCell()
		{
			Cell cell0 = null;

			foreach (var cell in _cells)
			{
				if (cell.selected)
				{
					if (cell0 != null)
						return null;

					cell0 = cell;
				}
			}
			return cell0;
		}

		/// <summary>
		/// Sets an edited cell's text and recalculates col width.
		/// </summary>
		void SetCellText()
		{
			if (_editor.Text != _editcell.text)
			{
				Changed = true;

				_editcell.text = _editor.Text;

				int c = _editcell.x;
				int pre = Cols[c].width();

				int w = TextRenderer.MeasureText(_graphics, _editcell.text, Font, _size, _flags).Width + _padHori * 2 + _padHoriSort;
				if (w > pre)
				{
					Cols[c].width(w);
				}
				else if (w < pre) // recalc width on the entire col
				{
					w = TextRenderer.MeasureText(_graphics, Cols[c].text, _f.FontAccent, _size, _flags).Width + _padHori * 2 + _padHoriSort; // cellwidth min.
					int wT;
					for (int r = 0; r != RowCount; ++r)
					{
						wT = TextRenderer.MeasureText(_graphics, _cells[r,c].text, Font, _size, _flags).Width + _padHori * 2 + _padHoriSort;
						if (wT > w) w = wT;
					}
					Cols[c].width(w, true);
				}

				if (Cols[c].width() != pre)
				{
					InitScrollers();
					Refresh(); // is required - and yet another Refresh() will follow ....
				}
			}
		}


		/// <summary>
		/// LMB selects a cell or enables/disables the editbox.
		/// @note MouseClick does not register on any of the top or left panels.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Select();

				foreach (var col in Cols)
					col.selected = false;

				foreach (var row in Rows)
					row.selected = false;

				int x = e.X + offsetHori;
				int y = e.Y + offsetVert;

				int left = getLeft();

				if (   x > left          && x < WidthTable
					&& y > HeightColhead && y < HeightTable)
				{
					var cords = getCords(x,y, left);
					var cell = _cells[cords.Y, cords.X];

					EnsureDisplayed(cell);

					if ((ModifierKeys & Keys.Control) == Keys.Control)
					{
						if (_editor.Visible)
						{
							SetCellText();
							_editor.Visible = false;
							Select();
						}

						cell.selected = !cell.selected;
					}
					else if (cell.selected)
					{
						if (_editor.Visible && cell != _editcell)
						{
							SetCellText();
							_editor.Visible = false;
							Select();
						}
						else
						{
							if (!_editor.Visible) // safety. There's a pseudo-clickable fringe around the textbox.
							{
								_editcell = cell;
								EditCell();
							}
							_editor.Focus();
						}
					}
					else
					{
						if (_editor.Visible)
						{
							SetCellText();
							_editor.Visible = false;
							Select();
						}

						ClearCellSelects();
						cell.selected = true;
					}

					Refresh();
				}
			}

//			base.OnMouseClick(e);
		}

		/// <summary>
		/// Starts cell edit on either LMB or Enter-key.
		/// </summary>
		void EditCell()
		{
			var rect = getCellRectangle(_editcell);
			_editor.Left   = rect.X + 6;
			_editor.Top    = rect.Y + 4;
			_editor.Width  = rect.Width - 7;
			_editor.Height = rect.Height;

			_editor.Visible = true;
			_editor.Text = _editcell.text;

			_editor.SelectionStart = 0; // because .NET
			if (_editor.Text == Constants.Stars)
			{
				_editor.SelectionLength = _editor.Text.Length;
			}
			else
				_editor.SelectionStart = _editor.Text.Length;
		}

		internal void ClearCellSelects()
		{
			foreach (var cell in _cells)
				cell.selected = false;
		}


		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!_init)
			{
				int x = e.X + offsetHori;
				int y = e.Y + offsetVert;

				int left = getLeft();

				if (   x > left          && x < WidthTable  && e.X < Width  - (_visVert ? _scrollVert.Width  : 0)
					&& y > HeightColhead && y < HeightTable && e.Y < Height - (_visHori ? _scrollHori.Height : 0))
				{
					var cords = getCords(x, y, left);
					_f.PrintInfo(cords.Y, cords.X);
				}
				else
					_f.PrintInfo(-1);
			}
//			base.OnMouseMove(e);
		}


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
					_f.PrintInfo(-1);
			}
		}


		Point getCords(int x, int y, int left)
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


		internal void EnsureDisplayed(Cell cell)
		{
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
				}
				else if (rect.X + rect.Width > right && rect.Width < right - left)
				{
					_scrollHori.Value += rect.X + rect.Width + bar - Width;
				}
			}

			if (rect.Y != HeightColhead)
			{
				if (rect.Y < HeightColhead)
				{
					_scrollVert.Value -= HeightColhead - rect.Y;
				}
				else
				{
					bar = _visHori ? _scrollHori.Height : 0;
					if (rect.Y + rect.Height + bar > Height)
					{
						_scrollVert.Value += rect.Y + rect.Height + bar - Height;
					}
				}
			}
		}

		void EnsureDisplayedCol(int c)
		{
			var bounds = getColEdges(c);

			int left = getLeft();

			if (bounds.X != left)
			{
				int bar = _scrollVert.Visible ? _scrollVert.Width : 0;
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

		internal void EnsureDisplayedRow(int r)
		{
			var bounds = getRowEdges(r);

			if (bounds.X != HeightColhead)
			{
				if (bounds.X < HeightColhead)
				{
					_scrollVert.Value -= HeightColhead - bounds.X;
				}
				else
				{
					int bar = _visHori ? _scrollHori.Height : 0;
					if (bounds.Y + bar > Height)
					{
						_scrollVert.Value += bounds.Y + bar - Height;
					}
				}
			}
		}

		internal bool EnsureDisplayedCellOrRow()
		{
			var cell = GetOnlySelectedCell();
			if (cell != null)
			{
				EnsureDisplayed(cell);
				return true;
			}

			for (int r = 0; r != RowCount; ++r)
			if (Rows[r].selected)
			{
				EnsureDisplayedRow(r);
				return true;
			}

			return false;
		}


		/// <summary>
		/// Handles a mouseclick on the rowhead. Selects or deselects row(s).
		/// Fires only on the rowhead panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_RowPanel(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
//				if (_editor.Visible)
//				{
				_editor.Visible = false;
				Select();
//				}

				int r = (e.Y + offsetVert) / HeightRow;

				bool select = false;

				if ((ModifierKeys & Keys.Shift) != Keys.Shift) // Shift always selects
				{
					for (int c = 0; c != ColCount; ++c)
					if (!_cells[r,c].selected)
					{
						select = true;
						break;
					}
				}
				else
					select = true;


				if ((ModifierKeys & Keys.Control) != Keys.Control)
					ClearCellSelects();

				if ((ModifierKeys & Keys.Shift) == Keys.Shift)
				{
					int sel = getSelectedRow();
					if (sel != -1)
					{
						int start, stop;
						if (sel < r)
						{
							start = sel;
							stop  = r;
						}
						else
						{
							start = r;
							stop  = sel;
						}

						while (start != stop + 1)
						{
							if (start != r) // done below
							for (int col = 0; col != ColCount; ++col)
								_cells[start, col].selected = true;

							++start;
						}
					}
				}
				else
				{
					foreach (var row in Rows)
						row.selected = false;

					Rows[r].selected = select;
				}

				if (select)
					EnsureDisplayedRow(r);

				for (int c = 0; c != ColCount; ++c)
					_cells[r,c].selected = select;

				Refresh();
			}
			else if (e.Button == MouseButtons.Right)
			{
				_f.context_(sender, e);
			}
		}

		int getSelectedRow()
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
		void click_ColPanel(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
//				if (_editor.Visible)
//				{
				_editor.Visible = false;
				Select();
//				}

				int x = e.X + offsetHori;
				int left = getLeft();
				int c = FrozenCount - 1;
				do { ++c; }
				while ((left += Cols[c].width()) < x);


				bool select = false;

				if ((ModifierKeys & Keys.Shift) != Keys.Shift) // Shift always selects
				{
					for (int r = 0; r != RowCount; ++r)
					if (!_cells[r,c].selected)
					{
						select = true;
						break;
					}
				}
				else
					select = true;


				if ((ModifierKeys & Keys.Control) != Keys.Control)
					ClearCellSelects();

				if ((ModifierKeys & Keys.Shift) == Keys.Shift)
				{
					int sel = getSelectedCol();
					if (sel != -1)
					{
						int start, stop;
						if (sel < c)
						{
							start = sel;
							stop  = c;
						}
						else
						{
							start = c;
							stop  = sel;
						}

						while (start != stop + 1)
						{
							if (start != c) // done below
							for (int row = 0; row != RowCount; ++row)
								_cells[row, start].selected = true;

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
					_cells[r,c].selected = select;

				Refresh();
			}
			else if (e.Button == MouseButtons.Right)
			{
				if ((ModifierKeys & Keys.Shift) == Keys.Shift) // Shift+RMB = sort by col
				{
//					if (_editor.Visible)
//					{
					_editor.Visible = false;
					Select();
//					}
	
					int x = e.X + offsetHori;
	
					int left = getLeft();
	
					int c = FrozenCount - 1;
					do { ++c; }
					while ((left += Cols[c].width()) < x);

					ColSort(c);
					EnsureDisplayedCellOrRow();
				}
			}
		}

		int getSelectedCol()
		{
			for (int c = 0; c != ColCount; ++c)
			{
				if (Cols[c].selected)
					return c;
			}
			return -1;
		}


		void click_IdLabel(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right
				&& (ModifierKeys & Keys.Shift) == Keys.Shift) // Shift+RMB = sort by id
			{
				_editor.Visible = false;
				Select();

				ColSort(0);
				EnsureDisplayedCellOrRow();
			}
		}

		void click_FirstLabel(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right
				&& (ModifierKeys & Keys.Shift) == Keys.Shift) // Shift+RMB = sort by 1st col
			{
				_editor.Visible = false;
				Select();

				ColSort(1);
				EnsureDisplayedCellOrRow();
			}
		}

		void click_SecondLabel(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right
				&& (ModifierKeys & Keys.Shift) == Keys.Shift) // Shift+RMB = sort by 2nd col
			{
				_editor.Visible = false;
				Select();

				ColSort(2);
				EnsureDisplayedCellOrRow();
			}
		}


		/// <summary>
		/// Inserts/deletes a row into the table.
		/// NOTE: 'Rows' and '_cells' are two entirely separate entities.
		/// </summary>
		/// <param name="id">row</param>
		/// <param name="list">null to delete the row</param>
		internal void Insert(int id, IList<string> list)
		{
			//logfile.Log("Insert() r= " + r);

			DrawingControl.SuspendDrawing(this);

			if (list != null)
			{
				Rows.Insert(id, new Row(id, Brushes.Created));
				++RowCount;

				GrowArray(ref _cells, id);

				for (int c = 0; c != ColCount; ++c)
					_cells[id,c] = new Cell(id,c, list[c]);

				for (int r = id + 1; r != RowCount; ++r)
				{
					++Rows[r]._id;
					for (int c = 0; c != ColCount; ++c)
					{
						//logfile.Log("r= " + row + " c= " + col + " text= " + _cells[row,col].text);
						++_cells[r,c].y;
					}
				}
			}
			else // delete 'r'
			{
				Rows.Remove(Rows[id]);
				--RowCount;

				ShrinkArray<Cell>(ref _cells, id);

				for (int r = id; r != RowCount; ++r)
				{
					--Rows[r]._id;
					for (int c = 0; c != ColCount; ++c)
					{
						//logfile.Log("r= " + row + " c= " + col + " text= " + _cells[row,col].text);
						--_cells[r,c].y;
					}
				}
			}

			InitScrollers();

			if (id < RowCount)
				EnsureDisplayedRow(id);

			DrawingControl.ResumeDrawing(this);
		}


		/// <summary>
		/// ... witchcraft ...
		/// </summary>
		/// <param name="cells"></param>
		/// <param name="insert">id to insert a row at</param>
		void GrowArray<Cell>(ref Cell[,] cells, int insert)
		{
			//logfile.Log("ResizeArray() rows= " + RowCount + " cols= " + ColCount + " insert= " + insert);

			var array = new Cell[RowCount, ColCount];
			//logfile.Log("array length= " + array.Length);

			int preLength = ColCount * insert;
			//logfile.Log("preLength= " + preLength);
			if (preLength != 0)
			{
				Array.Copy(cells,		// source array
						   0,			// source id
						   array,		// destination array
						   0,			// destination id
						   preLength);	// length
			}

			int postLength = ColCount * (RowCount - 1) - preLength;
			//logfile.Log("postLength= " + postLength);
			if (postLength != 0)
			{
				Array.Copy(cells,					// source array
						   preLength,				// source id
						   array,					// destination array
						   preLength + ColCount,	// destination id
						   postLength);				// length
			}

			cells = array;
		}

		/// <summary>
		/// ... witchcraft ...
		/// </summary>
		/// <param name="cells"></param>
		/// <param name="delete">id to delete a row at</param>
		void ShrinkArray<Cell>(ref Cell[,] cells, int delete)
		{
			//logfile.Log("ReduceArray() rows= " + RowCount + " cols= " + ColCount + " delete= " + delete);

			var array = new Cell[RowCount, ColCount];
			//logfile.Log("array length= " + array.Length);

			int preLength = ColCount * delete;
			//logfile.Log("preLength= " + preLength);
			if (preLength != 0)
			{
				Array.Copy(cells,		// source array
						   0,			// source id
						   array,		// destination array
						   0,			// destination id
						   preLength);	// length
			}

			int postLength = ColCount * RowCount - preLength;
			//logfile.Log("postLength= " + postLength);
			if (postLength != 0)
			{
				Array.Copy(cells,					// source array
						   preLength + ColCount,	// source id
						   array,					// destination array
						   preLength,				// destination id
						   postLength);				// length
			}

			cells = array;
		}


		#region Sort
		/// <summary>
		/// Sorts rows by a col either ascending or descending.
		/// </summary>
		/// <param name="c">the col id to sort by</param>
		void ColSort(int c)
		{
			if (_sortdir != 1 || _sortcol != c)
				_sortdir = 1;
			else
				_sortdir = -1;

			_sortcol = c;

			int result;
			bool stop, changed = false;
			var cellT = new Cell[ColCount];
			Row rowT; // NOTE: Cells are NOT properties of Rows; they act independently.

			for (int sort = 0; sort != RowCount; ++sort)
			{
				stop = true;
				for (int r = 0; r != RowCount - 1; ++r)
				{
					result = Sort(r,c);
					if (   (_sortdir ==  1 && result > 0)
						|| (_sortdir == -1 && result < 0))
					{
						stop = false;
						changed = true;

						rowT = Rows[r];
						for (int i = 0; i != ColCount; ++i)
							cellT[i] = _cells[r,i];

						Rows[r] = Rows[r+1];
						for (int i = 0; i != ColCount; ++i)
						{
							_cells[r,i] = _cells[r+1,i];
							_cells[r,i].y -= 1;
						}

						Rows[r+1] = rowT;
						for (int i = 0; i != ColCount; ++i)
						{
							_cells[r+1,i] = cellT[i];
							_cells[r+1,i].y += 1;
						}
					}
				}
				if (stop) break;
			}
			Changed = changed;
		}

		string _a,  _b;
		int    _ai, _bi;
		float  _af, _bf;

		/// <summary>
		/// Sorts fields as integers iff they convert to integers and performs
		/// a secondary sort against their IDs if applicable.
		/// TODO: descending sort
		/// </summary>
		/// <param name="r">row</param>
		/// <param name="c">col</param>
		/// <returns>-1 first is first, second is second
		///           0 identical
		///           1 first is second, second is first</returns>
		int Sort(int r, int c)
		{
			_a = _cells[r  ,c].text;
			_b = _cells[r+1,c].text;

			int result;

			if (   Int32.TryParse(_a, out _ai)			// try int comparision first
				&& Int32.TryParse(_b, out _bi))
			{
				result = _ai.CompareTo(_bi);
			}
			else if (float.TryParse(_a, out _af)		// try float comparison next
				  && float.TryParse(_b, out _bf))
			{
				result = _af.CompareTo(_bf);
			}
			else
				result = String.CompareOrdinal(_a,_b);	// else do string comparison

			if (result != 0)
				return result;

			if (c != 0									// secondary sort on id if primary sort matches
				&& Int32.TryParse(_cells[r  ,0].text, out _ai)
				&& Int32.TryParse(_cells[r+1,0].text, out _bi))
			{
				return _ai.CompareTo(_bi);
			}
			return 0;									// else return identical.
		}
		#endregion Sort
	}


	/// <summary>
	/// Returns a deep copy of the 2d Cell[,] array.
	/// </summary>
	static class ArrayCopy<T>
	{
		internal static T CloneCells(object cells)
		{
			using (var str = new MemoryStream())
			{
				var bf = new BinaryFormatter();
				bf.Serialize(str, cells);
				str.Seek(0, SeekOrigin.Begin);
				return (T)bf.Deserialize(str);
			}
		}
	}
}
