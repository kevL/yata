using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

//using yata.Properties;


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
	}


	sealed class YataGrid
		:
			Control
	{
		internal string Pfe // Path-File-Extension (ie. fullpath)
		{ get; set; }

		readonly YataForm _f;

		bool _changed;
		internal bool Changed
		{
			get { return _changed; }
			set { _f.TableChanged(_changed = value); }
		}

		int HeightColhead;
		int WidthRowhead;

		int HeightRow;

		internal int ColCount;
		int RowCount;

		string[] Fields // 'Fields' does NOT contain #0 col IDs (so that often needs +1)
		{ get; set; }

		readonly List<string[]> _rows = new List<string[]>();

		readonly List<Col> Cols = new List<Col>();
		readonly List<Row> Rows = new List<Row>();

		Cell[,] _cells;
		/// <summary>
		/// Gets the cell at pos [c,r].
		/// </summary>
		Cell this[int c, int r]
		{
			get { return _cells[c,r]; }
			set { _cells[c,r] = value; }
		}


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
					w += Cols[c].width;
				}
				_panelFrozen.Width = w;

				_panelFrozen.Refresh();

				_labelfirst .Visible = (_frozenCount > 1);
				_labelsecond.Visible = (_frozenCount > 2);
			}
		}


		Graphics _graphics;

		Color _colorText = SystemColors.ControlText;

//		Bitmap _bluePi = Resources.bluepixel;
//		Bitmap _piColhead;
//		Bitmap _piRowhead;


		const int _padHori = 6;
		const int _padVert = 4;

		const int _padHoriRowhead = 8;

//		bool _load;


		Font FontAccent;

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

		int offsetVert;
		int offsetHori;

		int HeightTable;
		int WidthTable;


		YataPanelCols _panelCols;
		YataPanelRows _panelRows;

		YataPanelFrozen _panelFrozen;

		Label _labelid     = new Label();
		Label _labelfirst  = new Label();
		Label _labelsecond = new Label();


		Timer _t1 = new Timer();

		readonly TextBox _editor = new TextBox();
		Cell _editcell;


		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="pfe"></param>
		internal YataGrid(YataForm f, string pfe)
		{
//			DrawingControl.SetDoubleBuffered(this);
			DoubleBuffered = true;

			SetStyle(ControlStyles.OptimizedDoubleBuffer
				   | ControlStyles.AllPaintingInWmPaint
				   | ControlStyles.UserPaint
				   | ControlStyles.ResizeRedraw, true);

			_f = f;

			Pfe = pfe;

			Dock = DockStyle.Fill;
			BackColor = SystemColors.ControlDark;

//			this.ProcessKeyPreview();
//			this.OnPreviewKeyDown();

			Font = _f.Font;
			FontAccent = new Font(Font, YataForm.getStyleAccented(Font.FontFamily));


			_scrollVert.Dock = DockStyle.Right;
			_scrollVert.ValueChanged += OnVertScrollValueChanged;

			_scrollHori.Dock = DockStyle.Bottom;
			_scrollHori.ValueChanged += OnHoriScrollValueChanged;

			Controls.Add(_scrollHori);
			Controls.Add(_scrollVert);

			_t1.Interval = 223;
			_t1.Enabled = true;
			_t1.Tick += t1_Tick;

			_editor.BackColor = Colors.Edit;
			_editor.Visible = false;
			_editor.BorderStyle = BorderStyle.None;

			Controls.Add(_editor);
		}


		void OnVertScrollValueChanged(object sender, EventArgs e)
		{
			if (!_editor.Visible)
			{
				offsetVert = _scrollVert.Value;
				Refresh();
			}
			else
				_scrollVert.Value = offsetVert;
		}

		void OnHoriScrollValueChanged(object sender, EventArgs e)
		{
			if (!_editor.Visible)
			{
				offsetHori = _scrollHori.Value;
				Refresh();
			}
			else
				_scrollHori.Value = offsetHori;
		}

		protected override void OnResize(EventArgs e)
		{
			InitScrollers();

//			if (_piColhead != null) _piColhead.Dispose();
//			_piColhead = new Bitmap(_bluePi, new Size(WidthTable, HeightColhead));

//			if (_piRowhead != null) _piRowhead.Dispose();
//			_piRowhead = new Bitmap(_bluePi, new Size(WidthRowhead, HeightTable));

			Refresh(); // table-drawing can tear without that.


//			base.OnResize(e);
		}


		/// <summary>
		/// Initializes the vertical and horizontal scrollbars OnResize (which
		/// also happens auto after load).
		/// </summary>
		void InitScrollers()
		{
			HeightTable = HeightColhead + HeightRow * RowCount;

			WidthTable = WidthRowhead;
			for (int c = 0; c != ColCount; ++c)
				WidthTable += Cols[c].width;

			// NOTE: Height/Width *includes* the height/width of the relevant
			// scrollbar and panel.

			bool visVert = HeightTable > Height;	// NOTE: Do not refactor this ->
			bool visHori = WidthTable  > Width;		// don't even ask. It works as-is. Be happy. Be very happy.

			_scrollVert.Visible =
			_scrollHori.Visible = false;

			if (visVert && visHori)
			{
				_scrollVert.Visible =
				_scrollHori.Visible = true;
			}
			else if (visVert)
			{
				_scrollVert.Visible = true;
				_scrollHori.Visible |= (WidthTable > Width - _scrollVert.Width);
			}
			else if (visHori)
			{
				_scrollHori.Visible = true;
				_scrollVert.Visible |= (HeightTable > Height - _scrollHori.Height);
			}

			if (_scrollVert.Visible)
			{
				int vert = HeightTable
						 + _scrollVert.LargeChange
						 - Height
						 + ((_scrollHori.Visible) ? _scrollHori.Height : 0)
						 - 1;

				if (vert < _scrollVert.LargeChange) vert = 0;

				_scrollVert.Maximum = vert;	// NOTE: Do not set this until after deciding
											// whether or not max < 0. 'Cause it fucks everything up. bingo.

				// handle .NET OnResize anomaly ->
				// keep the bottom of the table snuggled against the bottom of the visible area
				if (HeightTable - offsetVert < Height - ((_scrollHori.Visible) ? _scrollHori.Height : 0))
				{
					_scrollVert.Value = HeightTable - Height + ((_scrollHori.Visible) ? _scrollHori.Height : 0);
				}
			}
			else
				_scrollVert.Value = 0;

			if (_scrollHori.Visible)
			{
				int hori = WidthTable
						 + _scrollHori.LargeChange
						 - Width
						 + ((_scrollVert.Visible) ? _scrollVert.Width : 0)
						 - 1;

				if (hori < _scrollHori.LargeChange) hori = 0;

				_scrollHori.Maximum = hori;	// NOTE: Do not set this until after deciding
											// whether or not max < 0. 'Cause it fucks everything up. bingo.

				// handle .NET OnResize anomaly ->
				// keep the right of the table snuggled against the right of the visible area
				if (WidthTable - offsetHori < Width - ((_scrollVert.Visible) ? _scrollVert.Width : 0))
				{
					_scrollHori.Value = WidthTable - Width + ((_scrollVert.Visible) ? _scrollVert.Width : 0);
				}
			}
			else
				_scrollHori.Value = 0;
		}

		/// <summary>
		/// Scrolls the table vertically by the mousewheel.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(MouseEventArgs e)
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

				OnMouseMove(e);
			}
		}


		/// <summary>
		/// Handles the paint event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (ColCount != 0 && RowCount != 0 && _cells != null)
			{
				_graphics = e.Graphics;
				_graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

//				ControlPaint.DrawBorder3D(_graphics, ClientRectangle, Border3DStyle.Etched);


				int c,r;

				// NOTE: Paint backgrounds full-height/width of table ->

				// rows background - scrollable
				var rect = new Rectangle(Left, HeightColhead - offsetVert, WidthTable, HeightRow);

				Brush brush;
				for (r = 0; r != RowCount; ++r)
				{
					brush = (r % 2 == 0) ? Brushes.Alice
										 : Brushes.Blanche;

					_graphics.FillRectangle(brush, rect);

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
						if (rect.X + (rect.Width = Cols[c].width) > WidthRowhead)
						{
							var cell = this[c,r];
							if (cell.selected)
							{
								rect.X -= _padHori;

								if (_editor.Visible && _editcell == cell)
									_graphics.FillRectangle(Brushes.Edit, rect);
								else
									_graphics.FillRectangle(Brushes.CellSel, rect);

								rect.X += _padHori;
							}

							TextRenderer.DrawText(_graphics, cell.text, Font, rect, _colorText, _flags);
//							_graphics.DrawRectangle(new Pen(Color.Crimson), rect); // DEBUG
						}

						if ((rect.X += rect.Width) > Right)
							break;
					}
				}


//				using (var pi = new Bitmap(_bluePi, new Size(WidthTable, HeightColhead)))
//				if (_piColhead != null) _graphics.DrawImage(_piColhead, 0,0);

//				using (var pi = new Bitmap(_bluePi, new Size(WidthRowhead, HeightTable)))
//				if (_piRowhead != null) _graphics.DrawImage(_piRowhead, 0,0);


				// NOTE: Paint horizontal lines full-width of table.

				// row lines - scrollable
				int y;
				for (r = 1; r != RowCount + 1; ++r)
				{
					if ((y = HeightRow * r + yOffset) > Bottom)
						break;

					if (y > HeightColhead)
						_graphics.DrawLine(Pens.DarkLine, Left, y, WidthTable, y);
				}


				// NOTE: Paint vertical lines full-height of table.

				// col lines - scrollable
				int x = WidthRowhead - offsetHori;
				for (c = 0; c != ColCount; ++c)
				{
					if ((x += Cols[c].width) > Right)
						break;

					if (x > WidthRowhead)
						_graphics.DrawLine(Pens.DarkLine, x, Top, x, Bottom);
				}
			}
		}

		/// <summary>
		/// Labels the colheads.
		/// @note Called by OnPaint of the top-panel.
		/// </summary>
		/// <param name="graphics"></param>
		internal void LabelColHeads(IDeviceContext graphics)
		{
			if (ColCount != 0) // safety.
			{
				var rect = new Rectangle(WidthRowhead - offsetHori + _padHori, 0, 0, HeightColhead);

				for (int c = 0; c != ColCount; ++c)
				{
					if (rect.X + (rect.Width = Cols[c].width) > Left)
						TextRenderer.DrawText(graphics, Cols[c].text, FontAccent, rect, _colorText, _flags);

					if ((rect.X += rect.Width) > Right)
						break;
				}
			}
		}

		/// <summary>
		/// Labels the rowheads when inserting/deleting/sorting rows.
		/// @note Called by OnPaint of the left-panel.
		/// </summary>
		/// <param name="graphics"></param>
		internal void LabelRowheads(IDeviceContext graphics)
		{
			if (RowCount != 0) // safety - ought be checked in calling funct.
			{
//				_load = true; // (re)use '_load' to prevent firing CellChanged events for the Rowheads

				var rect = new Rectangle(_padHoriRowhead, 0, WidthRowhead, HeightRow);

				for (int r = offsetVert / HeightRow; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r - offsetVert) > Height)
						break;

					TextRenderer.DrawText(graphics, r.ToString(), FontAccent, rect, _colorText, _flags);
				}
//				_load = false;
			}
		}

		/// <summary>
		/// Labels the frozen cols.
		/// @note Called by OnPaint of the frozen panel.
		/// </summary>
		/// <param name="graphics"></param>
		internal void LabelFrozen(Graphics graphics)
		{
			if (RowCount != 0) // safety.
			{
				int x = 0;
				int c = 0;
				for (; c != FrozenCount; ++c)
				{
					x += Cols[c].width;
					graphics.DrawLine(Pens.DarkLine, x, 0, x, Height);
				}

				var rect = new Rectangle(0, 0, 0, HeightRow);

				for (int r = offsetVert / HeightRow; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r - offsetVert) > Height)
						break;

					rect.X = _padHori;

					for (c = 0; c != FrozenCount; ++c)
					{
						rect.Width = Cols[c].width;
						TextRenderer.DrawText(graphics, _cells[c,r].text, Font, rect, _colorText, _flags);

						rect.X += rect.Width;
					}

					graphics.DrawLine(Pens.DarkLine, 0, rect.Y + HeightRow, rect.X + rect.Width, rect.Y + HeightRow);
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


			if (Craft = (Path.GetFileNameWithoutExtension(Pfe).ToLower() == "crafting"))
			{
				foreach (var dir in Settings._pathall)
					_f.GropeLabels(dir);
			}


//			_load = true;

//			DrawingControl.SuspendDrawing(this);	// NOTE: Drawing resumes after autosize in either
													// YataForm.CreateTabPage() or YataForm.ReloadToolStripMenuItemClick().

			CreateCols();
			CreateRows();
			CreateCells();

			_scrollVert.LargeChange =
			_scrollHori.LargeChange = HeightRow;

			SetWidthRowhead();

			_panelCols = new YataPanelCols(this, HeightColhead);
			_panelCols.MouseClick += click_ColPanel;
			_panelRows = new YataPanelRows(this, WidthRowhead);
			_panelRows.MouseClick += click_RowPanel;

			_panelFrozen = new YataPanelFrozen(this, Cols[0].width);

			Controls.Add(_panelFrozen);

			Controls.Add(_panelRows);
			Controls.Add(_panelCols);

			InitFrozenLabels();

//			_load = false;

			return true;
		}


		void InitFrozenLabels() // TODO: enable/disable Menu items per table
		{
			_labelid    .Visible =
			_labelfirst .Visible =
			_labelsecond.Visible = false;

			if (ColCount > 0)
			{
				_labelid.Visible = true;

				DrawingControl.SetDoubleBuffered(_labelid);
				_labelid.Location = new Point(0,0);
				_labelid.Size = new Size(WidthRowhead + Cols[0].width, HeightColhead - 1);
				_labelid.BackColor = Colors.FrozenHead;

				_labelid.Paint += labelid_Paint;

				_panelCols.Controls.Add(_labelid);

				if (ColCount > 1)
				{
					DrawingControl.SetDoubleBuffered(_labelfirst);
					_labelfirst.Location = new Point(WidthRowhead + Cols[0].width, 0);
					_labelfirst.Size = new Size(Cols[1].width, HeightColhead - 1);
					_labelfirst.BackColor = Colors.FrozenHead;

					_labelfirst.Paint += labelfirst_Paint;

					_panelCols.Controls.Add(_labelfirst);

					if (ColCount > 2)
					{
						DrawingControl.SetDoubleBuffered(_labelsecond);
						_labelsecond.Location = new Point(WidthRowhead + Cols[0].width + Cols[1].width, 0);
						_labelsecond.Size = new Size(Cols[2].width, HeightColhead - 1);
						_labelsecond.BackColor = Colors.FrozenHead;

						_labelsecond.Paint += labelsecond_Paint;

						_panelCols.Controls.Add(_labelsecond);
					}
				}
			}
		}

		void labelid_Paint(object sender, PaintEventArgs e)
		{
			_graphics = e.Graphics;
			_graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var rect = new Rectangle(WidthRowhead + _padHori, Top, Cols[0].width, HeightColhead);
			TextRenderer.DrawText(_graphics, "id", FontAccent, rect, _colorText, _flags);

			_graphics.DrawLine(Pens.DarkLine, _labelid.Width, _labelid.Top, _labelid.Width, _labelid.Bottom);
		}

		void labelfirst_Paint(object sender, PaintEventArgs e)
		{
			_graphics = e.Graphics;
			_graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var rect = new Rectangle(_padHori, Top, Cols[1].width, HeightColhead);
			TextRenderer.DrawText(_graphics, Cols[1].text, FontAccent, rect, _colorText, _flags);

			_graphics.DrawLine(Pens.DarkLine, _labelfirst.Width, _labelfirst.Top, _labelfirst.Width, _labelfirst.Bottom);
		}

		void labelsecond_Paint(object sender, PaintEventArgs e)
		{
			_graphics = e.Graphics;
			_graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var rect = new Rectangle(_padHori, Top, Cols[2].width, HeightColhead);
			TextRenderer.DrawText(_graphics, Cols[2].text, FontAccent, rect, _colorText, _flags);

			_graphics.DrawLine(Pens.DarkLine, _labelsecond.Width, _labelsecond.Top, _labelsecond.Width, _labelsecond.Bottom);
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
		/// Creates the cols and caches the 2da's colhead data.
		/// </summary>
		void CreateCols()
		{
			ColCount = Fields.Length + 1; // 'Fields' does not include rowhead and id-col

			int c = 0;
			for (; c != ColCount; ++c)
			{
				Cols.Add(new Col()); //c
			}

			Cols[0].text = "id"; // NOTE: Is not measured - the cells below it determine col-width.

			using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
			{
				Size size;
				int h;
				c = 0;
				foreach (string head in Fields)
				{
					Cols[++c].text = head;
					size = TextRenderer.MeasureText(graphics, head, FontAccent, _size, _flags);
					Cols[c].width = size.Width + _padHori * 2;

					h = size.Height + _padVert * 2;
					if (h > HeightColhead)
						HeightColhead = h;
				}
			}
		}

		/// <summary>
		/// Creates the rows.
		/// </summary>
		void CreateRows()
		{
			RowCount = _rows.Count;

			for (int r = 0; r != RowCount; ++r)
				Rows.Add(new Row(_rows[r])); //r

			_rows.Clear(); // done w/ '_rows'
		}

		/// <summary>
		/// Creates the cells' 2d-array.
		/// </summary>
		internal void CreateCells()
		{
			_cells = new Cell[ColCount, RowCount];

			for (int r = 0; r != RowCount; ++r)
			for (int c = 0; c != ColCount; ++c)
				_cells[c,r] = new Cell(c, r, Rows[r].fields[c]);

			using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
			{
				Size size;
				int w, wT, hT;
				for (int c = 0; c != ColCount; ++c)
				{
					w = 25; // cellwidth min.
					for (int r = 0; r != RowCount; ++r)
					{
						size = TextRenderer.MeasureText(graphics, this[c,r].text, Font, _size, _flags);

						wT = size.Width + _padHori * 2;
						if (wT > w) w = wT;

						hT = size.Height + _padVert * 2;
						if (hT > HeightRow) HeightRow = hT;
					}
					Cols[c].width = w;
				}
			}
		}

		/// <summary>
		/// Calculates and maintains rowhead width wrt/ current Font across all
		/// tabs/tables.
		/// </summary>
		void SetWidthRowhead()
		{
			YataGrid table;

			int widthRowhead = 20, test; // row-headers' width stays uniform across all tabpages ->

			int tabs = _f.Tabs.TabCount;
			int tab = 0;
			for (; tab != tabs; ++tab)
			{
				table = _f.Tabs.TabPages[tab].Tag as YataGrid;	// NOTE: is quick and inaccurate (a lower val
				if ((test = table.RowCount - 1) > widthRowhead)	// can actually be longer than a higher val).
					widthRowhead = test;
			}

			using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
			{
				widthRowhead = TextRenderer.MeasureText(graphics, widthRowhead.ToString(), Font, _size, _flags).Width + _padHoriRowhead * 2;
			}

			for (tab = 0; tab != tabs; ++tab)
			{
				table = _f.Tabs.TabPages[tab].Tag as YataGrid;
				table.WidthRowhead = widthRowhead;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		internal void ForceScroll(MouseEventArgs e)
		{
//			base.OnMouseWheel(e); TODO: should probably call this.OnMouseWheel()
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
			switch (e.KeyCode)
			{
				case Keys.Home:
					if (e.Modifiers == Keys.Control)
					{
						if (_scrollVert.Visible)
							_scrollVert.Value = 0;
					}
					else if (_scrollHori.Visible)
					{
						_scrollHori.Value = 0;
					}
					break;

				case Keys.End:
					if (e.Modifiers == Keys.Control)
					{
						if (_scrollVert.Visible)
							_scrollVert.Value = HeightTable - Height + ((_scrollHori.Visible) ? _scrollHori.Height : 0);
					}
					else if (_scrollHori.Visible)
					{
						_scrollHori.Value = WidthTable - Width + ((_scrollVert.Visible) ? _scrollVert.Width : 0);
					}
					break;

				case Keys.PageUp:
					if (_scrollVert.Visible)
					{
						int h = Height - HeightColhead - (_scrollHori.Visible ? _scrollHori.Height : 0);

						if (_scrollVert.Value - h < 0)
							_scrollVert.Value = 0;
						else
							_scrollVert.Value -= h;
					}
					break;

				case Keys.PageDown:
					if (_scrollVert.Visible)
					{
						int h = Height - HeightColhead - (_scrollHori.Visible ? _scrollHori.Height : 0);

						if (_scrollVert.Value + h + (_scrollVert.LargeChange - 1) > _scrollVert.Maximum)
							_scrollVert.Value = _scrollVert.Maximum - (_scrollVert.LargeChange - 1);
						else
							_scrollVert.Value += h;
					}
					break;

				case Keys.Up: // NOTE: Needs to bypass KeyPreview
				{
					var cell = GetOnlySelectedCell();
					if (cell != null) // selection to the cell above
					{
						if (cell.y != 0)
						{
							// TODO: Multi-selecting cells w/ keyboard would require tracking a "current" cell.
//							cell.selected &= ((ModifierKeys & Keys.Control) == Keys.Control);

							cell.selected = false;
							cell = _cells[cell.x, cell.y - 1];
							cell.selected = true;

							EnsureDisplayed(cell);
							Refresh();
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
				}

				case Keys.Down: // NOTE: Needs to bypass KeyPreview
				{
					var cell = GetOnlySelectedCell();
					if (cell != null) // selection to the cell below
					{
						if (cell.y != RowCount - 1)
						{
							cell.selected = false;
							cell = _cells[cell.x, cell.y + 1];
							cell.selected = true;

							EnsureDisplayed(cell);
							Refresh();
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
				}

				case Keys.Left: // NOTE: Needs to bypass KeyPreview
				{
					var cell = GetOnlySelectedCell();
					if (cell != null) // selection to the cell left
					{
						if (cell.x != FrozenCount)
						{
							cell.selected = false;
							cell = _cells[cell.x - 1, cell.y];
							cell.selected = true;

							EnsureDisplayed(cell);
							Refresh();
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
				}

				case Keys.Right: // NOTE: Needs to bypass KeyPreview
				{
					var cell = GetOnlySelectedCell();
					if (cell != null) // selection to the cell right
					{
						if (cell.x != ColCount - 1)
						{
							cell.selected = false;
							cell = _cells[cell.x + 1, cell.y];
							cell.selected = true;

							EnsureDisplayed(cell);
							Refresh();
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
				}

				case Keys.Escape: // NOTE: Needs to bypass KeyPreview
					ClearCellSelects();
					Refresh();
					break;
			}
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
						var rect = getCellRectangle(_editcell);
						_editor.Left   = rect.X + 6;
						_editor.Top    = rect.Y + 4;
						_editor.Width  = rect.Width - 7;
						_editor.Height = rect.Height;

						_editor.Visible = true;
						_editor.Text = _editcell.text;
						_editor.SelectionStart = 0; // because .NET
						_editor.SelectionStart = _editor.Text.Length;

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
		Cell GetOnlySelectedCell()
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
		/// Sets the edited cell's text and recalculates col width.
		/// </summary>
		void SetCellText()
		{
			_editcell.text = _editor.Text;

			int c = _editcell.x;
			int pre = Cols[c].width;

			using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
			{
				int w = TextRenderer.MeasureText(graphics, _editcell.text, Font, _size, _flags).Width + _padHori * 2;

				if (w > pre)
				{
					Cols[c].width = w;
				}
				else if (w < pre) // recalc width on the entire col
				{
					w = TextRenderer.MeasureText(graphics, Cols[c].text, FontAccent, _size, _flags).Width + _padHori * 2; // cellwidth min.
					int wT;
					for (int r = 0; r != RowCount; ++r)
					{
						wT = TextRenderer.MeasureText(graphics, this[c,r].text, Font, _size, _flags).Width + _padHori * 2;
						if (wT > w) w = wT;
					}
					Cols[c].SetColWidth(w);
				}
			}

			if (Cols[c].width != pre)
			{
				InitScrollers();
				Refresh(); // is required - and yet another Refresh() will follow ....
			}
		}


		/// <summary>
		/// - mouseclick position does not register on any of the top or leftside
		///   panels
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			int left = getLeft();

			if (   e.X > left          && e.X < WidthTable
				&& e.Y > HeightColhead && e.Y < HeightTable)
			{
				var coords = getCoords(e.X, e.Y, left);
				var cell = _cells[coords.c, coords.r];

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

							var rect = getCellRectangle(cell);
							_editor.Left   = rect.X + 6;
							_editor.Top    = rect.Y + 4;
							_editor.Width  = rect.Width - 7;
							_editor.Height = rect.Height;

							_editor.Visible = true;
							_editor.Text = cell.text;
							_editor.SelectionStart = 0; // because .NET
							_editor.SelectionStart = _editor.Text.Length;
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

//			base.OnMouseClick(e);
		}

		void ClearCellSelects()
		{
			foreach (var cell in _cells)
				cell.selected = false;
		}


		protected override void OnMouseMove(MouseEventArgs e)
		{
			int left = getLeft();

			if (   e.X > left          && e.X < WidthTable
				&& e.Y > HeightColhead && e.Y < HeightTable)
			{
				var coords = getCoords(e.X, e.Y, left);
				_f.PrintInfo(coords.r, coords.c, "n/a");
			}
			else
				_f.PrintInfo(-1,-1,"");

//			base.OnMouseMove(e);
		}


		void t1_Tick(object sender, EventArgs e)
		{
			int left = getLeft();
			var rect = new Rectangle(left,
									 HeightColhead,
									 Width - left - (_scrollVert.Visible ? _scrollVert.Width : 0),
									 Height - HeightColhead - (_scrollHori.Visible ? _scrollHori.Height : 0));

			if (!rect.Contains(PointToClient(Cursor.Position)))
				_f.PrintInfo(-1,-1,"");
		}


		struct Coords
		{
			internal int c;
			internal int r;
		}

		Coords getCoords(int x, int y, int left)
		{
			var coords = new Coords();

			x += offsetHori;
			coords.c = FrozenCount - 1;
			do
			{
				++coords.c;
			}
			while ((left += Cols[coords.c].width) < x);

			int top = HeightColhead;
			y += offsetVert;
			coords.r = 0;
			while ((top += HeightRow) < y)
			{
				++coords.r;
			}

			return coords;
		}


		Rectangle getCellRectangle(Cell cell)
		{
			var rect = new Rectangle();

			rect.X = WidthRowhead - offsetHori;
			for (int c = 0; c != cell.x; ++c)
			{
				rect.X += Cols[c].width;
			}

			rect.Y = HeightColhead - offsetVert;
			for (int r = 0; r != cell.y; ++r)
			{
				rect.Y += HeightRow;
			}

			rect.Width = Cols[cell.x].width;
			rect.Height = HeightRow;

			return rect;
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
		/// Not really a point but the left and right bounds of a col.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		Point getColEdges(int c)
		{
			var bounds = new Point();
			bounds.X = WidthRowhead - offsetHori;
			for (int col = 0; col != c; ++col)
			{
				bounds.X += Cols[col].width;
			}
			bounds.Y = (bounds.X + Cols[c].width);

			return bounds;
		}

		int getLeft()
		{
			int left = WidthRowhead + Cols[0].width;
			if (FrozenCount > 1) left += Cols[1].width;
			if (FrozenCount > 2) left += Cols[2].width;

			return left;
		}


		void EnsureDisplayed(Cell cell)
		{
			var rect = getCellRectangle(cell);

			int left = getLeft();

			if (rect.X < left)
			{
				_scrollHori.Value -= left - rect.X;
			}
			else
			{
				int bar = _scrollVert.Visible ? _scrollVert.Width : 0;
				if (rect.X + rect.Width + bar > Width)
					_scrollHori.Value += rect.X + rect.Width + bar - Width;
			}

			if (rect.Y < HeightColhead)
			{
				_scrollVert.Value -= HeightColhead - rect.Y;
			}
			else
			{
				int bar = _scrollHori.Visible ? _scrollHori.Height : 0;
				if (rect.Y + rect.Height + bar > Height)
					_scrollVert.Value += rect.Y + rect.Height + bar - Height;
			}

//			getTableWidth();
		}

		void EnsureDisplayedRow(int r)
		{
			var bounds = getRowEdges(r);

			if (bounds.X < HeightColhead)
			{
				_scrollVert.Value -= HeightColhead - bounds.X;
			}
			else
			{
				int bar = _scrollHori.Visible ? _scrollHori.Height : 0;
				if (bounds.Y + bar > Height)
					_scrollVert.Value += bounds.Y + bar - Height;
			}
		}

		void EnsureDisplayedCol(int c)
		{
			var bounds = getColEdges(c);

			int left = getLeft();

			if (bounds.X < left)
			{
				_scrollHori.Value -= left - bounds.X;
			}
			else
			{
				int bar = _scrollVert.Visible ? _scrollVert.Width : 0;
				if (bounds.Y + bar > Width)
					_scrollHori.Value += bounds.Y + bar - Width;
			}
		}


/*		int getTableWidth()
		{
			int w = WidthRowhead;
			foreach (var col in Cols)
			{
				w += col.width;
			}
			logfile.Log("w= " + w);
			logfile.Log("Width= " + Width);
			logfile.Log("Right= " + Right);
			return w;
		} */


		/// <summary>
		/// Handles a mouseclick on the rowhead. Selects or deselects a row.
		/// Fires on the rowhead panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_RowPanel(object sender, MouseEventArgs e)
		{
			if (_editor.Visible)
			{
				_editor.Visible = false;
				Select();
			}

			int r = (e.Y + offsetVert) / HeightRow;

			bool select = false;
			for (int c = 0; c != ColCount; ++c)
			{
				if (!_cells[c,r].selected)
				{
					select = true;
					break;
				}
			}

			if ((ModifierKeys & Keys.Control) != Keys.Control)
				ClearCellSelects();

			if (select)
				EnsureDisplayedRow(r);

			for (int c = 0; c != ColCount; ++c)
				_cells[c,r].selected = select;

			Refresh();
		}

		/// <summary>
		/// Handles a mouseclick on the colhead. Selects or deselects a col.
		/// Fires on the colhead panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_ColPanel(object sender, MouseEventArgs e)
		{
			if (_editor.Visible)
			{
				_editor.Visible = false;
				Select();
			}

			int x = e.X + offsetHori;

			int left = getLeft();

			int c = FrozenCount - 1;
			do
			{
				++c;
			}
			while ((left += Cols[c].width) < x);


			bool select = false;
			for (int r = 0; r != RowCount; ++r)
			{
				if (!_cells[c,r].selected)
				{
					select = true;
					break;
				}
			}

			if ((ModifierKeys & Keys.Control) != Keys.Control)
				ClearCellSelects();

			if (select)
				EnsureDisplayedCol(c);

			for (int r = 0; r != RowCount; ++r)
				_cells[c,r].selected = select;

			Refresh();
		}
	}
}
