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
	}

	static class Colors
	{
		internal static readonly Color ColheadPanel = Color.Thistle;
		internal static readonly Color RowheadPanel = Color.Azure;

		internal static readonly Color FrozenHead  = Color.Moccasin;
		internal static readonly Color FrozenPanel = Color.OldLace;
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

		internal string[] Fields // 'Fields' does NOT contain #0 col IDs (so that often needs +1)
		{ get; set; }

		readonly List<string[]> _rows = new List<string[]>();

		internal readonly List<Col> Cols = new List<Col>();
		readonly List<Row> Rows = new List<Row>();

		Cell[,] _cells;
		/// <summary>
		/// Gets the cell at pos [c,r].
		/// </summary>
		public Cell this[int c, int r]
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
//			Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
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
		}


		void OnVertScrollValueChanged(object sender, EventArgs e)
		{
			offsetVert = _scrollVert.Value;
			Refresh();
		}

		void OnHoriScrollValueChanged(object sender, EventArgs e)
		{
			offsetHori = _scrollHori.Value;
			Refresh();
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


//		bool _bypassScroll;

		/// <summary>
		/// Initializes the vertical and horizontal scrollbars OnResize (which
		/// also happens auto after load).
		/// </summary>
		void InitScrollers()
		{
//			if (!_bypassScroll)
			{
//				_bypassScroll = true; // not sure if useful.

				HeightTable = HeightColhead + HeightRow * RowCount;

				WidthTable = WidthRowhead;
				for (int c = 0; c != ColCount; ++c)
					WidthTable += Cols[c].width;

				// NOTE: Height/Width *includes* the height/width of the relevant scrollbar and panel.

				bool visVert = HeightTable > Height;
				bool visHori = WidthTable  > Width;

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

					_scrollVert.Maximum = vert; // NOTE: Do not set these until after deciding

					// handle another .NET scrollbar anomaly ->
					if (HeightTable - offsetVert < Height - ((_scrollHori.Visible) ? _scrollHori.Height : 0))
					{
						_scrollVert.Value =
						offsetVert = HeightTable - Height + ((_scrollHori.Visible) ? _scrollHori.Height : 0);
					}
				}
				else
					offsetVert = 0;

				if (_scrollHori.Visible)
				{
					int hori = WidthTable
							 + _scrollHori.LargeChange
							 - Width
							 + ((_scrollVert.Visible) ? _scrollVert.Width : 0)
							 - 1;

					if (hori < _scrollHori.LargeChange) hori = 0;

					_scrollHori.Maximum = hori; // whether or not max < 0. 'Cause it fucks everything up. bingo.

					// handle another .NET scrollbar anomaly ->
					if (WidthTable - offsetHori < Width - ((_scrollVert.Visible) ? _scrollVert.Width : 0))
					{
						_scrollHori.Value =
						offsetHori = WidthTable - Width + ((_scrollVert.Visible) ? _scrollVert.Width : 0);
					}
				}
				else
					offsetHori = 0;

//				_bypassScroll = false;
			}
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


		/// <summary>
		/// Handles the paint event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (ColCount != 0 && RowCount != 0 && _cells != null)
			{
				//SuspendLayout();

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

				// colhead background - stationary
//				rect.Y = Top;
//				rect.Height = HeightColhead;
//				_graphics.FillRectangle(_brushColhead, rect);


				// cell text - scrollable
				rect.Height = HeightRow;
				int xStart = WidthRowhead - offsetHori + _padHori;
				int yOffset = HeightColhead - offsetVert;
				for (r = offsetVert / HeightRow; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r + yOffset) > Bottom)
						break;

//					if (rect.Y + HeightRow > HeightColhead)
					{
						rect.X = xStart;
						for (c = 0; c != ColCount; ++c)
						{
							if (rect.X + (rect.Width = Cols[c].width) > WidthRowhead)
							{
								TextRenderer.DrawText(_graphics, this[c,r].text, Font, rect, _colorText, _flags);
//								_graphics.DrawRectangle(new Pen(Color.Crimson), rect); // DEBUG
							}

							if ((rect.X += rect.Width) > Right)
								break;
						}
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

				// colhead line - stationary
//				_graphics.DrawLine(Pens.PenLine, Left, HeightColhead, WidthTable, HeightColhead);


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

				// rowhead line - stationary
//				_graphics.DrawLine(Pens.PenLine, WidthRowhead, Top, WidthRowhead, Bottom);


				// rowhead text - stationary
//				LabelRowheads();

				// colhead text - stationary
//				LabelColHeads();

				//ResumeLayout();
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
//				using (var font = new Font(Font, YataForm.getStyleAccented(Font.FontFamily)))
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

//				using (var font = new Font(Font, YataForm.getStyleAccented(Font.FontFamily)))
				{
					var rect = new Rectangle(_padHoriRowhead, 0, WidthRowhead, HeightRow);

					for (int r = offsetVert / HeightRow; r != RowCount; ++r)
					{
						if ((rect.Y = HeightRow * r - offsetVert) > Height)
							break;

//						if (rect.Y + HeightRow > Top)
						TextRenderer.DrawText(graphics, r.ToString(), FontAccent, rect, _colorText, _flags);
					}
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

			CacheCols();
			CacheRows();
			CacheCells();

			_scrollVert.LargeChange =
			_scrollHori.LargeChange = HeightRow;

			SetRowheadWidth();

			_panelCols = new YataPanelCols(this, HeightColhead);
			_panelRows = new YataPanelRows(this, WidthRowhead);

			_panelFrozen = new YataPanelFrozen(this, Cols[0].width);

			Controls.Add(_panelFrozen);

			Controls.Add(_panelRows);
			Controls.Add(_panelCols);

			InitFrozenLabels();

//			_load = false;

			return true;
		}


		void InitFrozenLabels()
		{
			DrawingControl.SetDoubleBuffered(_labelid);
			_labelid.Location = new Point(0,0);
			_labelid.Size = new Size(WidthRowhead + Cols[0].width - 1, HeightColhead - 1);
			_labelid.BackColor = Colors.FrozenHead;

			_labelid.Paint += labelid_Paint;

			DrawingControl.SetDoubleBuffered(_labelfirst);
			_labelfirst.Visible = false;
			_labelfirst.Location = new Point(WidthRowhead + Cols[0].width, 0);
			_labelfirst.Size = new Size(Cols[1].width - 1, HeightColhead - 1);
			_labelfirst.BackColor = Colors.FrozenHead;

			_labelfirst.Paint += labelfirst_Paint;

			DrawingControl.SetDoubleBuffered(_labelsecond);
			_labelsecond.Visible = false;
			_labelsecond.Location = new Point(WidthRowhead + Cols[0].width + Cols[1].width, 0);
			_labelsecond.Size = new Size(Cols[2].width - 1, HeightColhead - 1);
			_labelsecond.BackColor = Colors.FrozenHead;

			_labelsecond.Paint += labelsecond_Paint;


			_panelCols.Controls.Add(_labelid);
			_panelCols.Controls.Add(_labelfirst);
			_panelCols.Controls.Add(_labelsecond);
		}

		// NOTE: DrawLine tends to bork out and doesn't draw lines or draws
		// only part way. Solution: reduce the size of these labels and let
		// other OnPaint events handle stuff fairly reasonably.
		void labelid_Paint(object sender, PaintEventArgs e)
		{
			_graphics = e.Graphics;
			_graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var rect = new Rectangle(WidthRowhead + _padHori, Top, Cols[0].width, HeightColhead);
			TextRenderer.DrawText(_graphics, "id", FontAccent, rect, _colorText, _flags);
		}

		void labelfirst_Paint(object sender, PaintEventArgs e)
		{
			_graphics = e.Graphics;
			_graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var rect = new Rectangle(_padHori, Top, Cols[1].width, HeightColhead);
			TextRenderer.DrawText(_graphics, Cols[1].text, FontAccent, rect, _colorText, _flags);
		}

		void labelsecond_Paint(object sender, PaintEventArgs e)
		{
			_graphics = e.Graphics;
			_graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var rect = new Rectangle(_padHori, Top, Cols[2].width, HeightColhead);
			TextRenderer.DrawText(_graphics, Cols[2].text, FontAccent, rect, _colorText, _flags);
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
		string[] Parse2daRow(string line)
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
		/// Populates the table's colheads.
		/// </summary>
		void CacheCols()
		{
//			var pb = new ProgBar(_f); // not work <-
//			pb.ValTop = Fields.Length;
//			pb.Show();

			ColCount = Fields.Length + 1; // 'Fields' does not include rowhead and id-col

			int c = 0;
			for (; c != ColCount; ++c)
			{
				Cols.Add(new Col()); //c
			}

			Cols[0].text = "id";
//			Cols[0].Frozen = true;

			using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
			{
//				using (var font = new Font(Font, YataForm.getStyleAccented(Font.FontFamily)))
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

//						pb.Step();
					}
				}
			}
		}

		/// <summary>
		/// Populates the table's rows.
		/// </summary>
		void CacheRows()
		{
//			var pb = new ProgBar(_f);
//			pb.ValTop = _rows.Count;
//			pb.Show();

			RowCount = _rows.Count;

			for (int r = 0; r != RowCount; ++r)
			{
				Rows.Add(new Row(_rows[r])); //r
//				pb.Step();
			}
			_rows.Clear(); // done w/ '_rows'
		}


		/// <summary>
		/// Populates the table's cells.
		/// </summary>
		void CacheCells()
		{
			_cells = new Cell[ColCount, RowCount];

			for (int r = 0; r != RowCount; ++r)
			{
				for (int c = 0; c != ColCount; ++c)
				{
					_cells[c,r] = new Cell(Rows[r].fields[c]); //c, r,
				}
			}

			using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
			{
				Size size;
				int w, wT, hT;
				for (int c = 0; c != ColCount; ++c)
				{
					w = 25;
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
		/// Maintains rowhead width wrt current Font across all tabs/tables.
		/// </summary>
		void SetRowheadWidth()
		{
			YataGrid table;

			int widthRowhead = 20, test; // row-headers' width stays uniform across all tabpages ->

			int tabs = _f.Tabs.TabCount;
			int tab = 0;
			for (; tab != tabs; ++tab)
			{
				table = _f.Tabs.TabPages[tab].Tag as YataGrid;
				if ((test = table.RowCount - 1) > widthRowhead)
					widthRowhead = test;
			}

			using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
			{
				widthRowhead = TextRenderer.MeasureText(graphics, widthRowhead.ToString(), Font, _size, _flags).Width + _padHori * 3;
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
						{
							_scrollVert.Value =
							offsetVert = 0;
						}
					}
					else if (_scrollHori.Visible)
					{
						_scrollHori.Value =
						offsetHori = 0;
					}
					break;

				case Keys.End:
					if (e.Modifiers == Keys.Control)
					{
						if (_scrollVert.Visible)
						{
							_scrollVert.Value =
							offsetVert = HeightTable - Height + ((_scrollHori.Visible) ? _scrollHori.Height : 0);
						}
					}
					else if (_scrollHori.Visible)
					{
						_scrollHori.Value =
						offsetHori = WidthTable - Width + ((_scrollVert.Visible) ? _scrollVert.Width : 0);
					}
					break;

				case Keys.PageUp:
					if (_scrollVert.Visible)
					{
						int val;
						if (_scrollVert.Value < Height - _panelCols.Height - (_scrollHori.Visible ? _scrollHori.Height : 0))
							val = _scrollVert.Value;
						else
							val = Height - _panelCols.Height - (_scrollHori.Visible ? _scrollHori.Height : 0);

						_scrollVert.Value =
						offsetVert = (_scrollVert.Value - val);
					}
					break;

				case Keys.PageDown:
					if (_scrollVert.Visible)
					{
						DrawingControl.SuspendDrawing(this);

						int val;
						if (_scrollVert.Value > _scrollVert.Maximum - Height - _panelCols.Height - (_scrollHori.Visible ? _scrollHori.Height : 0))
							val = _scrollVert.Maximum - _scrollVert.Value;
						else
							val = Height - _panelCols.Height - (_scrollHori.Visible ? _scrollHori.Height : 0);

						_scrollVert.Value =
						offsetVert = (_scrollVert.Value + val);


						// handle another .NET scrollbar anomaly ->
						if (HeightTable - offsetVert < Height - ((_scrollHori.Visible) ? _scrollHori.Height : 0))
						{
							_scrollVert.Value =
							offsetVert = HeightTable - Height + ((_scrollHori.Visible) ? _scrollHori.Height : 0);
						}

						DrawingControl.ResumeDrawing(this);
					}
					break;

				case Keys.Up: // NOTE: Needs to bypass KeyPreview
					if (_scrollVert.Visible)
					{
						int val;
						if (_scrollVert.Value < _scrollVert.LargeChange)
							val = _scrollVert.Value;
						else
							val = _scrollVert.LargeChange;

						_scrollVert.Value =
						offsetVert = (_scrollVert.Value - val);
					}
					break;

				case Keys.Down: // NOTE: Needs to bypass KeyPreview
					if (_scrollVert.Visible)
					{
						DrawingControl.SuspendDrawing(this);

						int val;
						if (_scrollVert.Value > _scrollVert.Maximum - _scrollVert.LargeChange)
							val = _scrollVert.Maximum - _scrollVert.Value;
						else
							val = _scrollVert.LargeChange;

						_scrollVert.Value =
						offsetVert = (_scrollVert.Value + val);


						// handle another .NET scrollbar anomaly ->
						if (HeightTable - offsetVert < Height - ((_scrollHori.Visible) ? _scrollHori.Height : 0))
						{
							_scrollVert.Value =
							offsetVert = HeightTable - Height + ((_scrollHori.Visible) ? _scrollHori.Height : 0);
						}

						DrawingControl.ResumeDrawing(this);
					}
					break;

				case Keys.Left: // NOTE: Needs to bypass KeyPreview
					if (_scrollHori.Visible)
					{
					}
					break;

				case Keys.Right: // NOTE: Needs to bypass KeyPreview
					if (_scrollHori.Visible)
					{
					}
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
	}
}
