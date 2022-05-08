using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	// Various routines for scroll events.
	sealed partial class YataGrid
	{
		#region Fields (static)
		/// <summary>
		/// Another flag that stops presumptive .NET events from firing ~50
		/// billion.
		/// 
		/// 
		/// The table scrolls 1 pixel left if refreshing a table when scroll is
		/// far right. OBSOLETE: This funct checks if the table is scrolled far
		/// right and sets an additional 1 pixel offset for
		/// <c><see cref="InitScroll()">InitScroll()</see></c> to consider
		/// during row insertions and deletions or just fullrow-changes.
		/// 
		/// 
		/// It seems to happen only soon after a table is loaded - then it goes
		/// away. nice ...
		/// 
		/// 
		/// The problem appears to be that
		/// <c><see cref="Yata"/>.SetTabSize()</c> causes not one but two
		/// <c>Resize</c> events; the first event calculates that the
		/// client-width is 1px greater than it actually is. So I'm going to let
		/// the snuggle-to-max routine (horizontal) do its work and bypass the
		/// 2nd <c>Resize</c> event. Note the irony that when setting the
		/// tabsize in this application the <c>Form</c> does not need to be
		/// resized at all. No thanks for wasting my day.
		/// </summary>
		internal static bool BypassInitScroll;
		#endregion Fields (static)


		#region Fields
		internal readonly VScrollBar _scrollVert = new VScrollBar();
		internal readonly HScrollBar _scrollHori = new HScrollBar();

		internal bool _visVert; // Be happy. happy happy
		internal bool _visHori;

		int MaxVert; // Since a .NET scrollbar's Maximum value is not
		int MaxHori; // its maximum value calculate and store these. sic

		int HeightTable;
		int WidthTable;
		#endregion Fields


		#region Methods
		/// <summary>
		/// Initializes the vertical and horizontal scrollbars OnResize (which
		/// also happens auto after load).
		/// </summary>
		internal void InitScroll()
		{
			if (!BypassInitScroll)
			{
				//logfile.Log("(" + System.IO.Path.GetFileNameWithoutExtension(Fullpath) + ") YataGrid.InitScroll()");
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


				// Do not use LargeChange in what follows. Use HeightRow instead.
				// If a table does not have a scrollbar and user resizes it such
				// that it needs one LargeChange is 1 and things go bork.
				// LargeChange shall be set to HeightRow period.
				//
				// but think LargeChange ...
				//
				// - not that that helps since gettin' .net scrollbars to behave
				// properly for anything nontrivial is so fuckin' frustratin'.
				//
				// Don't even try setting LargeChange to HeightRow in this
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
			if ((ModifierKeys & Keys.Alt) == Keys.None)
			{
				if (Propanel != null && Propanel._scroll.Visible
					&& e.X > Propanel.Left && e.X < Propanel.Left + Propanel.Width)
				{
					if ((ModifierKeys & (Keys.Shift | Keys.Control)) == Keys.None)
						Propanel.Scroll(e);
				}
				else if (!_editor.Visible)
				{
					//logfile.Log("(" + System.IO.Path.GetFileNameWithoutExtension(Fullpath) + ") YataGrid.Scroll()");
					if (_visVert && (!_visHori || (ModifierKeys & Keys.Control) == Keys.None))
					{
						int h;
						if ((ModifierKeys & Keys.Shift) == Keys.Shift) // shift grid vertically 1 visible-height per delta
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
						if ((ModifierKeys & Keys.Shift) == Keys.Shift) // shift grid horizontally 1 visible-width per delta
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
		#endregion Methods


		#region Handlers
		/// <summary>
		/// Scrolls the table vertically.
		/// </summary>
		/// <param name="sender"><c><see cref="_scrollVert"></see></c></param>
		/// <param name="e"></param>
		void OnScrollValueChanged_vert(object sender, EventArgs e)
		{
			hideContexts();

			//string t = System.IO.Path.GetFileNameWithoutExtension(Fullpath);
			//logfile.Log("(" + t + ") YataGrid.OnScrollValueChanged_vert() Yata.Table= " + System.IO.Path.GetFileNameWithoutExtension(Yata.Table.Fullpath));
			if (_table == null)
			{
				_table = this;
				//logfile.Log(". (_table == null) _table= " + System.IO.Path.GetFileNameWithoutExtension(_table.Fullpath));
			}
			//else logfile.Log(". _table= " + System.IO.Path.GetFileNameWithoutExtension(_table.Fullpath));

			if (_table == Yata.Table)
				_table.Invalidator(INVALID_GRID | INVALID_FROZ | INVALID_ROWS);

			_table.OffsetVert = _table._scrollVert.Value;

			if (!_f.IsSearch										// <- if not Search
				&& (!_f.tb_Goto.Focused || !Settings._instantgoto))	// <- if not "instantgoto=true" when gotobox has focus
			{
				editresultdefault();
				Select(); // <- workaround: refocus the table when the bar is moved by mousedrag (bar has to move > 0px)
			}

			Point loc = PointToClient(Cursor.Position);
			var args = new MouseEventArgs(MouseButtons.Left, 1, loc.X, loc.Y, 0); // clicks,x,y,delta
			OnMouseMove(args); // update coords on the Statusbar

			if (_table == Yata.Table
				&&  _f._diff1 != null   && _f._diff2 != null
				&& (_f._diff1 == _table || _f._diff2 == _table))
			{
				SyncDiffedGrids();
			}
		}

		/// <summary>
		/// Scrolls the table horizontally.
		/// </summary>
		/// <param name="sender"><c><see cref="_scrollHori"></see></c></param>
		/// <param name="e"></param>
		void OnScrollValueChanged_hori(object sender, EventArgs e)
		{
			hideContexts();

			if (_table == null) _table = this;

			if (_table == Yata.Table)
				_table.Invalidator(INVALID_GRID | INVALID_COLS);

			//logfile.Log("YataGrid.OnScrollValueChanged_hori() _table= " + System.IO.Path.GetFileNameWithoutExtension(_table.Fullpath));

			_table.OffsetHori = _table._scrollHori.Value;

			if (!_f.IsSearch										// <- if not Search
				&& (!_f.tb_Goto.Focused || !Settings._instantgoto))	// <- if not "instantgoto=true" when gotobox has focus
			{
				editresultdefault();
				Select(); // <- workaround: refocus the table when the bar is moved by mousedrag (bar has to move > 0px)
			}

			Point loc = PointToClient(Cursor.Position);
			var args = new MouseEventArgs(MouseButtons.Left, 1, loc.X, loc.Y, 0); // clicks,x,y,delta
			OnMouseMove(args); // update coords on the Statusbar

			if (_table == Yata.Table
				&&  _f._diff1 != null   && _f._diff2 != null
				&& (_f._diff1 == _table || _f._diff2 == _table))
			{
				SyncDiffedGrids();
			}
		}

		/// <summary>
		/// Hides the RMB-contexts for (1) rows, (2) cells, and/or (3) tabs.
		/// </summary>
		void hideContexts()
		{
			if      (_f._contextRo.Visible) _f._contextRo.Visible = false;
			else if (_f._contextCe.Visible) _f._contextCe.Visible = false;
			else if (_f._contextTa.Visible) _f._contextTa.Visible = false;
		}
		#endregion Handlers


		/// <summary>
		/// Synchs diffed tables both vertically and horizontally.
		/// </summary>
		/// <remarks>Ensure that
		/// <c><see cref="Yata._diff1">Yata._diff1</see></c> and
		/// <c><see cref="Yata._diff2">Yata._diff2</see></c> and
		/// <c><see cref="_table"/></c> are valid before call.</remarks>
		void SyncDiffedGrids()
		{
			//string t = System.IO.Path.GetFileNameWithoutExtension(Fullpath);
			//logfile.Log("(" + t + ") YataGrid.SyncDiffedGrids() _table= " + System.IO.Path.GetFileNameWithoutExtension(_table.Fullpath));

			YataGrid table;

			VScrollBar vert;
			HScrollBar hori;

			if (_f._diff1 == _table)
			{
				table = _f._diff2;

				vert = table._scrollVert;
				hori = table._scrollHori;
			}
			else // _f._diff2 == _table
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
	}
}
