using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog for search and replace text.
	/// </summary>
	sealed partial class ReplaceTextDialog
		: YataDialog
	{
		#region Enums
		enum SearchTyp
		{ Subfield, Fulfield }

		enum SearchDir
		{ U,D }

		enum StartType
		{ Top, Sel, Bot }
		#endregion Enums


		#region structs
		/// <summary>
		/// A container for warnings issued during ReplaceAll.
		/// </summary>
		internal struct Cords
		{
			internal int r;
			internal int c;
		}
		#endregion structs


		#region Fields (static)
		static SearchTyp _searchtyp = SearchTyp.Subfield;
		static SearchDir _searchdir = SearchDir.D;
		static StartType _starttype = StartType.Sel;

		static bool _casesen;
		static bool _replall;
		static bool _autstep;

		static string _pre = String.Empty;
		static string _pos = String.Empty;

		internal static readonly List<Cords> Warns = new List<Cords>();
		#endregion Fields (static)


		#region Fields
		/// <summary>
		/// A <c>string</c> to search for.
		/// </summary>
		/// <remarks>The value is set in
		/// <c><see cref="Search()">Search()</see></c>.</remarks>
		string _search;

		/// <summary>
		/// A <c>string</c> of text to match against
		/// <c><see cref="_search"/></c>.
		/// </summary>
		/// <remarks>The value is set in
		/// <c><see cref="SearchResult()">SearchResult()</see></c>.</remarks>
		string _text;

		/// <summary>
		/// <c>true</c> to search for a subfield instead of a fulfield.
		/// </summary>
		/// <remarks>The value is set in
		/// <c><see cref="Search()">Search()</see></c>.</remarks>
		bool _substr;
		#endregion Fields


		#region cTor
		/// <summary>
		/// Instantiates this <c>ReplaceTextDialog</c>.
		/// </summary>
		/// <param name="f"></param>
		internal ReplaceTextDialog(Yata f)
		{
			_f = f;

			InitializeComponent();

			// set these before loading the YataDialog metrics ->
			// else the MinSize gets stuck wide if user closes this dialog wider
			// than the designer says it should be

			MaximumSize = new Size(Int32.MaxValue, Height);
			MinimumSize = new Size(         Width, Height);

			Initialize(YataDialog.METRIC_FUL);

			switch (_searchtyp)
			{
				case SearchTyp.Subfield: rb_Subfield.Checked = true; break;
				case SearchTyp.Fulfield: rb_Fulfield.Checked = true; break;
			}

			switch (_searchdir) // init searchdir before starttype; starttype can change searchdir
			{
				case SearchDir.U: rb_U.Checked = true; break;
				case SearchDir.D: rb_D.Checked = true; break;
			}

			switch (_starttype) // TODO: allow Sel only if there is a row or cell selected
			{
				case StartType.Top: rb_Top.Checked = true; break;
				case StartType.Sel: rb_Sel.Checked = true; break;
				case StartType.Bot: rb_Bot.Checked = true; break;
			}

			cb_Casesen.Checked = _casesen;
			cb_Replall.Checked = _replall;
			cb_Autstep.Checked = _autstep;

			tb_Pretext.Text = _pre;
			tb_Postext.Text = _pos;


			Show(_f); // Yata is owner.
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides this dialog's <c>FormClosing</c> handler. Sets the static
		/// location and nulls
		/// <c><see cref="Yata._replacer">Yata._replacer</see></c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			(_f as Yata).CloseReplacer();
			base.OnFormClosing(e);
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Searches for text but does not do replace.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Search"/></c></param>
		/// <param name="e"></param>
		void click_Search(object sender, EventArgs e)
		{
			if (ModifierKeys == Keys.None)
			{
				ActiveControl = bu_Search;	// in case user presses [Enter] when another control has focus
											// set focus to the Search button so that the do button can be
											// navigated to quickly.
				if (Search())
				{
					rb_Sel.Checked = true;
				}
				else
				{
					using (var ib = new Infobox(Infobox.Title_warni,
												"Search not found.",
												null,
												InfoboxType.Warn))
					{
						ib.ShowDialog(this);
					}
				}
			}
		}

		/// <summary>
		/// Searches the current table for the text in the search-box.
		/// </summary>
		/// <returns><c>true</c> if a match is found</returns>
		/// <seealso cref="Yata"><c>Yata.Search()</c></seealso>
		bool Search()
		{
			_substr = (_searchtyp == SearchTyp.Subfield); // else is Fulfield search

			if (_casesen) _search = _pre;
			else          _search = _pre.ToUpperInvariant();

			YataGrid table = Yata.Table;

			Cell sel;
			int selr;
			switch (_starttype)
			{
				case StartType.Top:
					selr = -1;
					sel  = table[0,0];
					break;

				default: // StartType.Sel
					selr = table.getSelectedRow();
					sel  = table.getSelectedCell();
					break;

				case StartType.Bot:
					sel = table[selr = table.RowCount - 1,
									   table.ColCount - 1];
					break;
			}

			table.ClearSelects();
			(_f as Yata).ClearSyncSelects();


			bool start = true;

			int r,c;

			if (_searchdir == SearchDir.D) // forward ->
			{
				if (sel != null) { c = sel.x; selr = sel.y; }
				else
				{
					c = -1;
					if (selr == -1) selr = 0;
				}

				for (r = selr; r != table.RowCount; ++r)
				{
					if (start)
					{
						start = false;
						if (++c == table.ColCount)		// if starting on the last cell of a row
						{
							c = 0;

							if (r < table.RowCount - 1)	// jump to the first cell of the next row
								++r;
							else						// or to the top of the table if on the last row
								r = 0;
						}
					}
					else
						c = 0;

					for (; c != table.ColCount; ++c)
					{
						if (c != 0 && SearchResult(r,c)) // don't search the id-col
						{
							table.SelectCell(table[r,c]);
							return true;
						}
					}
				}

				// TODO: tighten exact start/end-cells
				for (r = 0; r != selr + 1;       ++r) // quick and dirty wrap ->
				for (c = 0; c != table.ColCount; ++c)
				{
					if (c != 0 && SearchResult(r,c)) // don't search the id-col
					{
						table.SelectCell(table[r,c]);
						return true;
					}
				}
			}
			else // backward ->
			{
				if (sel != null) { c = sel.x; selr = sel.y; }
				else
				{
					c = table.ColCount;
					if (selr == -1) selr = table.RowCount - 1;
				}

				for (r = selr; r != -1; --r)
				{
					if (start)
					{
						start = false;
						if (--c == -1)	// if starting on the first cell of a row
						{
							c = table.ColCount - 1;

							if (r > 0)	// jump to the last cell of the previous row
								--r;
							else		// or to the bottom of the table if on the first row
								r = table.RowCount - 1;
						}
					}
					else
						c = table.ColCount - 1;

					for (; c != -1; --c)
					{
						if (c != 0 && SearchResult(r,c)) // don't search the id-col
						{
							table.SelectCell(table[r,c]);
							return true;
						}
					}
				}

				// TODO: tighten exact start/end-cells
				for (r = table.RowCount - 1; r != selr - 1; --r) // quick and dirty wrap ->
				for (c = table.ColCount - 1; c != -1;       --c)
				{
					if (c != 0 && SearchResult(r,c)) // don't search the id-col
					{
						table.SelectCell(table[r,c]);
						return true;
					}
				}
			}

			int invalid = YataGrid.INVALID_GRID
						| YataGrid.INVALID_FROZ
						| YataGrid.INVALID_ROWS
						| YataGrid.INVALID_COLS;
			if (table.Propanel != null && table.Propanel.Visible)
				invalid |= YataGrid.INVALID_PROP;

			table.Invalidator(invalid);
			table.Update();

			return false;
		}


		/// <summary>
		/// Replaces text. Replaces all matches if <c><see cref="_replall"/></c>
		/// is <c>true</c> or searches for the next match if
		/// <c><see cref="_autstep"/></c> is <c>true</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Replace"/></c></param>
		/// <param name="e"></param>
		void click_Replace(object sender, EventArgs e)
		{
			if (ModifierKeys == Keys.None)
			{
				if (!_replall)
				{
					Replace(Yata.Table);

					if (_autstep)
						click_Search(bu_Replace, EventArgs.Empty);
				}
				else
					ReplaceAll();
			}
		}

		/// <summary>
		/// Replaces the text in a currently selected cell.
		/// </summary>
		/// <param name="table"></param>
		void Replace(YataGrid table)
		{
			Cell sel = table.getSelectedCell();
			if (sel != null)
			{
				_substr = (_searchtyp == SearchTyp.Subfield); // else is Fulfield search

				if (_casesen) _search = _pre;
				else          _search = _pre.ToUpperInvariant();

				if (SearchResult(sel.y, sel.x))
				{
					bool sanitized = false;

					if (_casesen)
					{
						_text = sel.text.Replace(_pre, tb_Postext.Text);

						sanitized = table.ChangeCellText_repl(sel, _text);
					}
					else
					{
						_text = sel.text;
						if (ReplaceCaseInsensitive(ref _text, _pre, tb_Postext.Text))
						{
							sanitized = table.ChangeCellText_repl(sel, _text);
						}
					}

					int invalid = YataGrid.INVALID_GRID;

					if (sel.x < table.FrozenCount)
						invalid |= YataGrid.INVALID_FROZ;

					if (table.Propanel != null && table.Propanel.Visible)
						invalid |= YataGrid.INVALID_PROP;

					table.Invalidator(invalid);
					table.Update();

					if (sanitized)
					{
						using (var ib = new Infobox(Infobox.Title_warni,
													"The text that resulted has been altered.",
													null,
													InfoboxType.Warn))
						{
							ib.ShowDialog(this);
						}
					}
				}
				else if (!_autstep)
				{
					using (var ib = new Infobox(Infobox.Title_infor,
												"Match not found in cell.",
												null,
												InfoboxType.Warn))
					{
						ib.ShowDialog(this);
					}
				}
			}
			else if (!_autstep)
			{
				using (var ib = new Infobox(Infobox.Title_warni,
											"Search or select a cell.",
											null,
											InfoboxType.Warn))
				{
					ib.ShowDialog(this);
				}
			}
		}

		/// <summary>
		/// Replaces all instances of <c><see cref="_pre"/></c> with
		/// <c><see cref="_pos"/></c> dependent on
		/// <c><see cref="_casesen"/></c> and <c><see cref="_substr"/></c>.
		/// </summary>
		void ReplaceAll()
		{
			bool sanitized = false;

			YataGrid table = Yata.Table;

			if (_casesen) _search = _pre;
			else          _search = _pre.ToUpperInvariant();

			table.ClearSelects();
			(_f as Yata).ClearSyncSelects();
			table.ClearReplaced();


			_substr = (_searchtyp == SearchTyp.Subfield); // else is Fulfield search

			int replaced = 0;

			Cell cell;

			for (int r = 0; r != table.RowCount; ++r)
			for (int c = 0; c != table.ColCount; ++c)
			{
				if (c != 0 && SearchResult(r,c)) // don't search the id-col
				{
					cell = table[r,c];

					if (_casesen)
					{
						cell.replaced = true;
						++replaced;

						_text = cell.text.Replace(_pre, tb_Postext.Text);

						if (table.ChangeCellText_repl(cell, _text))
							sanitized = true;
					}
					else
					{
						_text = cell.text;
						if (ReplaceCaseInsensitive(ref _text, _pre, tb_Postext.Text))
						{
							cell.replaced = true;
							++replaced;

							if (table.ChangeCellText_repl(cell, _text))
								sanitized = true;
						}
					}
				}
			}

			int invalid = YataGrid.INVALID_GRID
						| YataGrid.INVALID_FROZ
						| YataGrid.INVALID_ROWS
						| YataGrid.INVALID_COLS;
			if (table.Propanel != null && table.Propanel.Visible)
				invalid |= YataGrid.INVALID_PROP;

			table.Invalidator(invalid);
			table.Update();


			if (replaced != 0)
			{
				(_f as Yata).EnableGotoReplaced(true);
				EnableReplacedOps(true);

				if (sanitized)
				{
					string copy;
					if (replaced == 1)
					{
						copy = "The text that resulted in this cell has been altered"
							 + Environment.NewLine;
					}
					else
						copy = "The texts that resulted in these cells ("
							 + Warns.Count
							 + ") have been altered"
							 + Environment.NewLine;

					foreach (var warn in Warns)
					{
						copy += Environment.NewLine + warn.r + "," + warn.c;
					}
					Warns.Clear();


					using (var ib = new Infobox(Infobox.Title_warni,
												replaced + " fields replaced.",
												copy,
												InfoboxType.Warn))
					{
						ib.ShowDialog(this);
					}
				}
				else
				{
					using (var ib = new Infobox(Infobox.Title_infor, replaced + " fields replaced."))
					{
						ib.ShowDialog(this);
					}
				}
			}
			else
			{
				using (var ib = new Infobox(Infobox.Title_warni,
											"Search not found.",
											null,
											InfoboxType.Warn))
				{
					ib.ShowDialog(this);
				}
			}
		}


		/// <summary>
		/// Sets static var <c><see cref="_pre"/></c> and dis/enables the search
		/// and do buttons.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Pretext"/></c></param>
		/// <param name="e"></param>
		void textchanged_Pretext(object sender, EventArgs e)
		{
			_pre = tb_Pretext.Text;

			EnableSearch();
			EnableReplace();
		}

		/// <summary>
		/// Sets static var <c><see cref="_pos"/></c> and dis/enables the do
		/// button.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Postext"/></c></param>
		/// <param name="e"></param>
		void textchanged_Postext(object sender, EventArgs e)
		{
			_pos = tb_Postext.Text;

			EnableReplace();
		}

		/// <summary>
		/// Sets static var <c><see cref="_searchtyp"/></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="rb_Subfield"/></c></item>
		/// <item><c><see cref="rb_Fulfield"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void checkedchanged_SearchTyp(object sender, EventArgs e)
		{
			if ((sender as RadioButton).Checked)
			{
				if (sender == rb_Subfield) _searchtyp = SearchTyp.Subfield;
				else                    _searchtyp = SearchTyp.Fulfield; // rb_Field
			}
		}

		/// <summary>
		/// Sets static var <c><see cref="_searchdir"/></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="rb_U"/></c></item>
		/// <item><c><see cref="rb_D"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void checkedchanged_SearchDir(object sender, EventArgs e)
		{
			if ((sender as RadioButton).Checked)
			{
				if (sender == rb_U) _searchdir = SearchDir.U;
				else                _searchdir = SearchDir.D; // rb_D
			}
		}

		/// <summary>
		/// Sets static var <c><see cref="_starttype"/></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="rb_Top"/></c></item>
		/// <item><c><see cref="rb_Sel"/></c></item>
		/// <item><c><see cref="rb_Bot"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void checkedchanged_StartType(object sender, EventArgs e)
		{
			if ((sender as RadioButton).Checked)
			{
				if (sender == rb_Top)
				{
					_starttype = StartType.Top;
					rb_D.Checked = true;
				}
				else if (sender == rb_Sel)
				{
					_starttype = StartType.Sel;
				}
				else
				{
					_starttype = StartType.Bot; // rb_Bot
					rb_U.Checked = true;
				}
			}
		}

		/// <summary>
		/// Sets static var <c><see cref="_casesen"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="cb_Casesen"/></c></param>
		/// <param name="e"></param>
		void checkedchanged_CaseSens(object sender, EventArgs e)
		{
			_casesen = cb_Casesen.Checked;

//			EnableReplace();
		}

		/// <summary>
		/// Sets static var <c><see cref="_replall"/></c> and dis/enables
		/// radio-groups and the Search button.
		/// </summary>
		/// <param name="sender"><c><see cref="cb_Replall"/></c></param>
		/// <param name="e"></param>
		void checkedchanged_ReplaceAll(object sender, EventArgs e)
		{
			gb_Dir  .Enabled =
			gb_Start.Enabled = !(_replall = cb_Replall.Checked);

			EnableSearch();

			if (_replall)
			{
				using (var ib = new Infobox(Infobox.Title_infor,
											"It is strongly suggested to save the table before"
											+ " doing Replace All. If things screw up they can"
											+ " screw up badly ... especially in large tables."))
				{
					ib.ShowDialog(this);
				}
			}
		}

		/// <summary>
		/// Sets static var <c><see cref="_autstep"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="cb_Autstep"/></c></param>
		/// <param name="e"></param>
		void checkedchanged_Autostep(object sender, EventArgs e)
		{
			_autstep = cb_Autstep.Checked;
		}


		/// <summary>
		/// Clears all <c><see cref="Cell.replaced">Cell.replaced</see></c>
		/// flags in the current table.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_ClearReplaced"/></c></param>
		/// <param name="e"></param>
		void click_ClearReplaced(object sender, EventArgs e)
		{
			Yata.Table.ClearReplaced();
		}

		/// <summary>
		/// Selects the next/previous <c><see cref="Cell"/></c> that has its
		/// <c><see cref="Cell.replaced">Cell.replaced</see></c> flag set.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="bu_GotoReplaced"/></c></item>
		/// <item><c><see cref="bu_GotoReplaced_pre"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void click_GotoReplaced(object sender, EventArgs e)
		{
			Yata.Table.GotoReplaced(sender == bu_GotoReplaced);
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Dis/enables the Search button.
		/// </summary>
		void EnableSearch()
		{
			bu_Search.Enabled = Yata.Table != null
							 && !_replall
							 && _pre.Length != 0;
		}

		/// <summary>
		/// Dis/enables the do button.
		/// </summary>
		internal void EnableReplace()
		{
			bu_Replace.Enabled = Yata.Table != null
							  && !Yata.Table.Readonly
							  && _pre.Length != 0
							  && _pre != _pos;
//							  && (_casesens || _pre.ToUpperInvariant() != _pos.ToUpperInvariant());
		}

		/// <summary>
		/// Wrapper for <c><see cref="EnableSearch()"/></c> and
		/// <c><see cref="EnableReplace()"/></c>.
		/// </summary>
		internal void EnableOps()
		{
			EnableSearch();
			EnableReplace();
		}

		/// <summary>
		/// Dis/enables the Replaced buttons.
		/// </summary>
		/// <param name="enabled"><c>true</c> to enable buttons</param>
		internal void EnableReplacedOps(bool enabled)
		{
			bu_ClearReplaced   .Enabled =
			bu_GotoReplaced    .Enabled =
			bu_GotoReplaced_pre.Enabled = enabled;
		}

		/// <summary>
		/// Checks a <c><see cref="Cell"/></c> in the current
		/// <c><see cref="YataGrid"/></c> for a <c>string</c> of text.
		/// </summary>
		/// <param name="r">a row in the <c>YataGrid</c></param>
		/// <param name="c">a col in the <c>YataGrid</c></param>
		/// <returns><c>true</c> if <c><see cref="_text"/></c> matches
		/// <c><see cref="_search"/></c> - dependent on
		/// <c><see cref="_substr"/></c></returns>
		bool SearchResult(int r, int c)
		{
			if (_casesen) _text = Yata.Table[r,c].text;
			else           _text = Yata.Table[r,c].text.ToUpperInvariant();

			return _text == _search || (_substr && _text.Contains(_search));
		}
		#endregion Methods


		#region Methods (static)
		/// <summary>
		/// Replaces a <c>string</c> in a <c>string</c> with a <c>string</c>.
		/// </summary>
		/// <param name="text">ref to a <c>string</c> on which to perform the
		/// replace</param>
		/// <param name="pre">the <c>string</c> to be replaced</param>
		/// <param name="pos">the <c>string</c> to replace with</param>
		/// <returns><c>true</c> if any replacements happen</returns>
		/// <remarks>Several safeties are required before this method is called;
		/// this <c>ReplaceTextDialog</c> implements them in various ways with
		/// its <c>Controls'</c> enabled/disabled and checked states.</remarks>
		static bool ReplaceCaseInsensitive(ref string text, string pre, string pos)
		{
			bool replaced = false;

			int i = text.IndexOf(pre, 0, StringComparison.InvariantCultureIgnoreCase);
			while (i != -1)
			{
				// the next conditions are to force the text to overwrite with
				// the case of 'pos' but NOT if the case is already identical

				if (pos.Length != pre.Length
//					|| text.Length < i + pos.Length // that is redundant because of (pos.Length != pre.Length) ie. IndexOf() will not be found
					|| (pos.Length == pre.Length && text.Substring(i, pos.Length) != pos))
				{
					text = text.Remove(i, pre.Length).Insert(i, pos);
					replaced = true;
				}
				i = text.IndexOf(pre, i + pos.Length, StringComparison.InvariantCultureIgnoreCase);
			}
			return replaced;
		}
		#endregion Methods (static)
	}
}
