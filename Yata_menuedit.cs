using System;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Fields
		/// <summary>
		/// <c>true</c> to prevent <c><see cref="Table"/></c> from being
		/// selected when the table scrolls.
		/// </summary>
		internal bool IsSearch;

		/// <summary>
		/// preps the Search textbox to select all text
		/// </summary>
		bool _selectall_search;

		/// <summary>
		/// I don't know exactly what this is doing or why it works but it does.
		/// It appears to prevent a click on an already selected
		/// <c>ToolStripTextBox</c> from reselecting all text ...
		/// </summary>
		bool _isEditclick_search;

		/// <summary>
		/// I don't know exactly what this is doing or why it works but it does.
		/// It appears to prevent a click on an already selected
		/// <c>ToolStripTextBox</c> from reselecting all text ...
		/// </summary>
		internal bool IsTabbed_search;

		/// <summary>
		/// preps the Goto textbox to select all text
		/// </summary>
		bool _selectall_goto;

		/// <summary>
		/// I don't know exactly what this is doing or why it works but it does.
		/// It appears to prevent a click on an already selected
		/// <c>ToolStripTextBox</c> from reselecting all text ...
		/// </summary>
		bool _isEditclick_goto;

		/// <summary>
		/// I don't know exactly what this is doing or why it works but it does.
		/// It appears to prevent a click on an already selected
		/// <c>ToolStripTextBox</c> from reselecting all text ...
		/// </summary>
		internal bool IsTabbed_goto;
		#endregion Fields


		#region Handlers (edit)
		/// <summary>
		/// Handles the <c>DropDownOpening</c> event for
		/// <c><see cref="it_MenuEdit"/></c>. Deters if
		/// <c><see cref="it_DeselectAll"/></c> ought be enabled.
		/// </summary>
		/// <param name="sender"><c>it_MenuEdit</c></param>
		/// <param name="e"></param>
		void edit_dropdownopening(object sender, EventArgs e)
		{
			it_DeselectAll.Enabled = Table != null && Table.anySelected();
		}


		/// <summary>
		/// Handles it-click to undo the previous operation if possible.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Undo"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Undo <c>[Ctrl+z]</c></item>
		/// </list></remarks>
		void editclick_Undo(object sender, EventArgs e)
		{
			Table._ur.Undo();
			it_Undo.Enabled = Table._ur.CanUndo;
			it_Redo.Enabled = true;
		}

		/// <summary>
		/// Handles it-click to redo the previous operation if possible.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Redo"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Redo <c>[Ctrl+y]</c></item>
		/// </list></remarks>
		void editclick_Redo(object sender, EventArgs e)
		{
			Table._ur.Redo();
			it_Redo.Enabled = Table._ur.CanRedo;
			it_Undo.Enabled = true;
		}


		/// <summary>
		/// Deselects all <c><see cref="Cell">Cells</see></c>/
		/// <c><see cref="Row">Rows</see></c>/
		/// <c><see cref="Col">Cols</see></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_DeselectAll"/></c></item>
		/// <item><c><see cref="YataGrid"/></c> - <c>RMB</c> outside the grid</item>
		/// </list></param>
		/// <param name="e"></param>
		internal void editclick_Deselect(object sender, EventArgs e)
		{
			Table.ClearSelects();
			ClearSyncSelects();

			Table.Invalidator(YataGrid.INVALID_GRID
							| YataGrid.INVALID_FROZ
							| YataGrid.INVALID_ROWS
							| YataGrid.INVALID_COLS);
		}


		/// <summary>
		/// This is used by
		/// <c><see cref="YataStrip"></see>.WndProc()</c> to workaround .net
		/// fuckuppery that causes the <c>TextBoxes</c> on the Menubar to refire
		/// their <c>Enter</c> events even when they are already Entered and
		/// Focused, which screws up the select-all-text routine.
		/// </summary>
		/// <returns><c>true</c> if either <c><see cref="tb_Goto"/></c> or
		/// <c><see cref="tb_Search"/> has focus.</c></returns>
		internal bool isTextboxFocused()
		{
			return tb_Goto.Focused || tb_Search.Focused;
		}


		/// <summary>
		/// Handles select-all hocus-pocus when focus enters the Search
		/// <c>ToolStripTextBox</c> on the <c><see cref="YataStrip"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Search"/></c></param>
		/// <param name="e"></param>
		void enter_Searchbox(object sender, EventArgs e)
		{
			//logfile.Log("Yata.enter_Searchbox()");
//			(sender as ToolStripTextBox).SelectAll(); haha good luck. Text cannot be selected in the Enter event.

			_selectall_search   = !_isEditclick_search && !IsTabbed_search;
			_isEditclick_search =
			IsTabbed_search     = false;
		}

		/// <summary>
		/// Handles selectall hocus-pocus when a <c>MouseDown</c> event occurs
		/// for a <c>ToolStripTextBox</c> on the Menubar.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="tb_Goto"/></c></item>
		/// <item><c><see cref="tb_Search"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void mousedown_Searchbox(object sender, MouseEventArgs e)
		{
			//logfile.Log("Yata.mousedown_Searchbox() _selectall_search= " + _selectall_search);
			if (_selectall_search)
			{
				_selectall_search = false;
				(sender as ToolStripTextBox).SelectAll();
			}
		}

		/// <summary>
		/// Handles selectall hocus-pocus when user clicks the search-box.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Search"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Find <c>[Ctrl+f]</c></item>
		/// </list></remarks>
		void editclick_FocusSearch(object sender, EventArgs e)
		{
			_isEditclick_search = true;
			tb_Search.Focus();
			tb_Search.SelectAll();
		}

		/// <summary>
		/// Handles the <c>TextChanged</c> event on the search-box.
		/// Enables/disables find next/find previous.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Search"/></c></param>
		/// <param name="e"></param>
		void textchanged_Searchbox(object sender, EventArgs e)
		{
			it_Searchnext.Enabled =
			it_Searchprev.Enabled = Table != null
								 && tb_Search.Text.Length != 0;
		}

		/// <summary>
		/// Performs search without changing focus.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Searchnext"/></c> <c>[F3]</c></item>
		/// <item><c><see cref="it_Searchprev"/></c> <c>[Shift+F3]</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Find next <c>[F3]</c></item>
		/// <item>Edit|Find previous <c>[Shift+F3]</c></item>
		/// </list></remarks>
		void editclick_StartSearch(object sender, EventArgs e)
		{
			// the editor will never be visible here because the only way to
			// get here is by click on the Menubar - will close the editor
			// because the editor loses focus - or by [F3] - which will be
			// bypassed if the editor is open - see YataEditbox.OnPreviewKeyDown().

			IsSearch = true;
			Search(sender == it_Searchnext); // [F3] shall not force the Table focused.
			IsSearch = false;
		}

		/// <summary>
		/// Performs a search when <c>[Enter]</c> or [Shift+Enter] is pressed
		/// and focus is on either the search-box or the search-option dropdown.
		/// </summary>
		/// <remarks>[Enter] and [Shift+Enter] change focus to the table.</remarks>
		void Search_keyEnter()
		{
			IsSearch = true;
			if (Search((ModifierKeys & Keys.Shift) == Keys.None))
				Table.Select();

			IsSearch = false;
		}

		/// <summary>
		/// Searches the current table for the text in the search-box.
		/// </summary>
		/// <param name="forward"></param>
		/// <returns><c>true</c> if a match is found</returns>
		/// <remarks>Ensure that <c><see cref="Table"/></c> is valid before
		/// call.</remarks>
		bool Search(bool forward)
		{
			if ((ModifierKeys & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				string search = tb_Search.Text;
				if (search.Length != 0)
				{
					search = search.ToUpper(CultureInfo.CurrentCulture);

					Cell sel = Table.getSelectedCell();
					int selr = Table.getSelectedRow();

					Table.ClearSelects();

					bool substring = (cb_SearchOption.SelectedIndex == 0); // else is wholestring search.
					bool start = true;

					string text;

					int r,c;

					if (forward) // forward search ->
					{
						if (sel != null) { c = sel.x; selr = sel.y; }
						else
						{
							c = -1;
							if (selr == -1) selr = 0;
						}

						for (r = selr; r != Table.RowCount; ++r)
						{
							if (start)
							{
								start = false;
								if (++c == Table.ColCount)		// if starting on the last cell of a row
								{
									c = 0;

									if (r < Table.RowCount - 1)	// jump to the first cell of the next row
										++r;
									else						// or to the top of the table if on the last row
										r = 0;
								}
							}
							else
								c = 0;

							for (; c != Table.ColCount; ++c)
							{
								if (c != 0 // don't search the id-col
									&& ((text = Table[r,c].text.ToUpper(CultureInfo.CurrentCulture)) == search
										|| (substring && text.Contains(search))))
								{
									Table.SelectCell(Table[r,c]);
									return true;
								}
							}
						}

						// TODO: tighten exact start/end-cells
						for (r = 0; r != selr + 1;       ++r) // quick and dirty wrap ->
						for (c = 0; c != Table.ColCount; ++c)
						{
							if (c != 0 // don't search the id-col
								&& ((text = Table[r,c].text.ToUpper(CultureInfo.CurrentCulture)) == search
									|| (substring && text.Contains(search))))
							{
								Table.SelectCell(Table[r,c]);
								return true;
							}
						}
					}
					else // backward search ->
					{
						if (sel != null) { c = sel.x; selr = sel.y; }
						else
						{
							c = Table.ColCount;
							if (selr == -1) selr = Table.RowCount - 1;
						}

						for (r = selr; r != -1; --r)
						{
							if (start)
							{
								start = false;
								if (--c == -1)	// if starting on the first cell of a row
								{
									c = Table.ColCount - 1;

									if (r > 0)	// jump to the last cell of the previous row
										--r;
									else		// or to the bottom of the table if on the first row
										r = Table.RowCount - 1;
								}
							}
							else
								c = Table.ColCount - 1;

							for (; c != -1; --c)
							{
								if (c != 0 // don't search the id-col
									&& ((text = Table[r,c].text.ToUpper(CultureInfo.CurrentCulture)) == search
										|| (substring && text.Contains(search))))
								{
									Table.SelectCell(Table[r,c]);
									return true;
								}
							}
						}

						// TODO: tighten exact start/end-cells
						for (r = Table.RowCount - 1; r != selr - 1; --r) // quick and dirty wrap ->
						for (c = Table.ColCount - 1; c != -1;       --c)
						{
							if (c != 0 // don't search the id-col
								&& ((text = Table[r,c].text.ToUpper(CultureInfo.CurrentCulture)) == search
									|| (substring && text.Contains(search))))
							{
								Table.SelectCell(Table[r,c]);
								return true;
							}
						}
					}
				}
				else // not found ->
				{
					Table.ClearSelects(); // TODO: That should return a bool if any clears happened.
					ClearSyncSelects();
				}

				int invalid = YataGrid.INVALID_GRID
							| YataGrid.INVALID_FROZ
							| YataGrid.INVALID_ROWS;
				if (Table.Propanel != null && Table.Propanel.Visible)
					invalid |= YataGrid.INVALID_PROP;

				Table.Invalidator(invalid);
			}
			return false;
		}


		/// <summary>
		/// Handles select-all hocus-pocus when focus enters the Goto
		/// <c>ToolStripTextBox</c> on the <c><see cref="YataStrip"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Goto"/></c></param>
		/// <param name="e"></param>
		void enter_Gotobox(object sender, EventArgs e)
		{
			//logfile.Log("Yata.enter_Gotobox()");
//			(sender as ToolStripTextBox).SelectAll(); haha good luck. Text cannot be selected in the Enter event.

			_selectall_goto   = !_isEditclick_goto && !IsTabbed_goto;
			_isEditclick_goto =
			IsTabbed_goto     = false;
		}

		/// <summary>
		/// Handles selectall hocus-pocus when a <c>MouseDown</c> event occurs
		/// for a <c>ToolStripTextBox</c> on the Menubar.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="tb_Goto"/></c></item>
		/// <item><c><see cref="tb_Search"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void mousedown_Gotobox(object sender, MouseEventArgs e)
		{
			//logfile.Log("Yata.mousedown_Gotobox() _selectall_goto= " + _selectall_goto);
			if (_selectall_goto)
			{
				_selectall_goto = false;
				(sender as ToolStripTextBox).SelectAll();
			}
		}

		/// <summary>
		/// Handles the <c>Click</c> event to focus the goto box.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Goto"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Goto <c>[Ctrl+g]</c></item>
		/// </list></remarks>
		void editclick_FocusGoto(object sender, EventArgs e)
		{
			_isEditclick_goto = true;
			tb_Goto.Focus();
			tb_Goto.SelectAll();
		}

		/// <summary>
		/// Handles the <c>TextChanged</c> event on the goto box.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Goto"/></c></param>
		/// <param name="e"></param>
		void textchanged_Gotobox(object sender, EventArgs e)
		{
			// TODO: allow a blank string

			int result;
			if (!Int32.TryParse(tb_Goto.Text, out result)
				|| result < 0)
			{
				tb_Goto.Text = "0"; // recurse
			}
			else if (Table != null && Settings._instantgoto)
			{
				Table.doGoto(tb_Goto.Text, false); // NOTE: Text is checked for validity in doGoto().
			}
		}


		/// <summary>
		/// Handles the <c>Click</c> event to edit the 2da-file's defaultval.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Defaultval"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Default value</item>
		/// </list></remarks>
		void editclick_Defaultval(object sender, EventArgs e)
		{
			InputDialog._defaultval = Table._defaultval;
			using (var idc = new InputDialog(this))
			{
				if (idc.ShowDialog(this) == DialogResult.OK
					&& InputDialog._defaultval != Table._defaultval)
				{
					Table._defaultval = InputDialog._defaultval;
					if (!Table.Changed) Table.Changed = true;

					it_Defaultclear.Enabled = Table._defaultval.Length != 0;
				}
			}
		}

		/// <summary>
		/// Handles the <c>Click</c> event to clear the 2da-file's defaultval.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Defaultclear"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Clear Default</item>
		/// </list></remarks>
		void editclick_Defaultclear(object sender, EventArgs e)
		{
			Table._defaultval = String.Empty;
			if (!Table.Changed) Table.Changed = true;

			it_Defaultclear.Enabled = false;
		}
		#endregion Handlers (edit)


		#region Methods (edit)
		/// <summary>
		/// Enables/disables <c><see cref="it_Undo"/></c>.
		/// </summary>
		/// <param name="enable"></param>
		internal void EnableUndo(bool enable)
		{
			it_Undo.Enabled = enable;
		}

		/// <summary>
		/// Enables/disables <c><see cref="it_Redo"/></c>.
		/// </summary>
		/// <param name="enable"></param>
		internal void EnableRedo(bool enable)
		{
			it_Redo.Enabled = enable;
		}

		/// <summary>
		/// Enables/disables <c><see cref="it_GotoLoadchanged"/></c> and
		/// <c><see cref="it_GotoLoadchanged_pre"/></c>.
		/// </summary>
		/// <param name="enabled"></param>
		internal void EnableGotoLoadchanged(bool enabled)
		{
			it_GotoLoadchanged    .Enabled =
			it_GotoLoadchanged_pre.Enabled = enabled;
		}
		#endregion Methods (edit)
	}
}
