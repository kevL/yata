using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Handlers (tab)
		/// <summary>
		/// Handles the <c>MouseClick</c> event on the <c>TabControl</c>. Shows
		/// <c><see cref="_contextTa"/></c> when a tab is rightclicked.
		/// </summary>
		/// <param name="sender"><c><see cref="Tabs"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Do not use the <c>TabControl's</c> <c>ContextMenuStrip</c>
		/// since a rightclick on any tabpage's <c><see cref="YataGrid"/></c>
		/// would cause
		/// <c><see cref="opening_TabContext()">opening_TabContext()</see></c>
		/// to fire. Although the context does not show rightclicks are used for
		/// other things by <c>YataGrid</c>.</remarks>
		void click_Tabs(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				_contextTa.Show(Tabs, e.Location);
		}


		/// <summary>
		/// Sets the selected tab when a right-click on a tab is about to open
		/// the context.
		/// </summary>
		/// <param name="sender"><c><see cref="_contextTa"/></c></param>
		/// <param name="e"></param>
		/// <remarks><c>_contextTa</c> is assigned to
		/// <c><see cref="Tabs"/>.ContextMenuStrip</c>.</remarks>
		void opening_TabContext(object sender, CancelEventArgs e)
		{
			bool found = false;

			Point loc = Tabs.PointToClient(Cursor.Position); // activate the Tab ->
			for (int tab = 0; tab != Tabs.TabCount; ++tab)
			{
				if (Tabs.GetTabRect(tab).Contains(loc))
				{
					Tabs.SelectedIndex = tab;
					found = true;
					break;
				}
			}

			if (found)
			{
				tabit_CloseAll      .Enabled =
				tabit_CloseAllOthers.Enabled = Tabs.TabCount != 1;

				tabit_Save          .Enabled = !Table.Readonly;

				tabit_Reload        .Enabled = File.Exists(Table.Fullpath);

				tabit_Diff2    .Enabled = _diff1 != null && _diff1 != Table;
				tabit_DiffReset.Enabled = _diff1 != null || _diff2 != null;
				tabit_DiffSync .Enabled = _diff1 != null && _diff2 != null;

				if (_diff1 != null)
					tabit_Diff1.Text = "diff1 - " + Path.GetFileNameWithoutExtension(_diff1.Fullpath);
				else
					tabit_Diff1.Text = "Select diff1";

				if (_diff2 != null)
					tabit_Diff2.Text = "diff2 - " + Path.GetFileNameWithoutExtension(_diff2.Fullpath);
				else
					tabit_Diff2.Text = "Select diff2";
			}
			else
				e.Cancel = true;
		}


		/// <summary>
		/// Closes all other tables when a tab's context-closeall item is
		/// clicked.
		/// </summary>
		/// <param name="sender"><c><see cref="tabit_CloseAllOthers"/></c></param>
		/// <param name="e"></param>
		void tabclick_CloseAllOtherTabs(object sender, EventArgs e)
		{
			if (!CancelChangedTables("close", true))
			{
				DrawRegulator.SuspendDrawing(this); // stops tab-flickering on Remove tab

				for (int tab = Tabs.TabCount - 1; tab != -1; --tab)
				{
					if (tab != Tabs.SelectedIndex)
						ClosePage(Tabs.TabPages[tab]);
				}

				SetTabSize();

				it_SaveAll.Enabled = AllowSaveAll();

				DrawRegulator.ResumeDrawing(this);
			}
		}

		// TODO: FreezeFirst/Second, gotoloadchanged, etc.


		/// <summary>
		/// Selects <c><see cref="_diff1"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="tabit_Diff1"/></c></param>
		/// <param name="e"></param>
		void tabclick_Diff1(object sender, EventArgs e)
		{
			tabclick_DiffReset(sender, e);
			_diff1 = Table;
		}

		/// <summary>
		/// Selects <c><see cref="_diff2"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="tabit_Diff2"/></c></param>
		/// <param name="e"></param>
		void tabclick_Diff2(object sender, EventArgs e)
		{
			if (_fdiffer != null) _fdiffer.Close();
			if (_diff2   != null) DiffReset(_diff2);

			_diff2 = Table;
			if (Diff())
				tabclick_DiffSync(sender, e);
			else
				_diff1 = _diff2 = null;
		}

		/// <summary>
		/// Clears all diffed cells and nulls any pointers to diffed tables.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="tabit_DiffReset"/></c></item>
		/// <item><c><see cref="DifferDialog"/>.bu_Reset</c></item>
		/// <item><c><see cref="tabit_Diff1"/></c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>tab|Diff reset</item>
		/// <item><c><see cref="DifferDialog"/>.click_btnReset()</c></item>
		/// <item><c><see cref="tabclick_Diff1()">tabclick_Diff1()</see></c></item>
		/// <item><c><see cref="steadystate()">steadystate()</see></c></item>
		/// <item><c><see cref="YataGrid"/>.ColSort()</c></item>
		/// </list></remarks>
		internal void tabclick_DiffReset(object sender, EventArgs e)
		{
			if (_fdiffer != null) _fdiffer.Close();

			if (_diff1 != null)
			{
				DiffReset(_diff1);
				_diff1 = null;
			}

			if (_diff2 != null)
			{
				DiffReset(_diff2);
				_diff2 = null;
			}

			Table.Select();
		}

		/// <summary>
		/// Aligns the two diffed tables for easy switching back and forth.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="tabit_DiffSync"/></c></item>
		/// <item><c><see cref="tabit_Diff2"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>tab|Diff sync tables</item>
		/// <item><c><see cref="tabclick_Diff2()">tabclick_Diff2()</see></c></item>
		/// </list></remarks>
		void tabclick_DiffSync(object sender, EventArgs e)
		{
			int w1, w2;

			int cols = Math.Min(_diff1.ColCount, _diff2.ColCount);
			for (int c = 0; c != cols; ++c)
			{
				w1 = _diff1.Cols[c].Width;
				w2 = _diff2.Cols[c].Width;

				if      (w1 > w2) _diff2.Cols[c].SetWidth(w1, true);
				else if (w2 > w1) _diff1.Cols[c].SetWidth(w2, true);
			}

			cols = Math.Min(YataGrid.FreezeSecond, _diff1.ColCount);
			for (int c = 0; c != cols; ++c)
				_diff1.MetricFrozenControls(c);

			cols = Math.Min(YataGrid.FreezeSecond, _diff2.ColCount);
			for (int c = 0; c != cols; ++c)
				_diff2.MetricFrozenControls(c);

			_diff1._scrollVert.Value =
			_diff1._scrollHori.Value =
			_diff2._scrollVert.Value =
			_diff2._scrollHori.Value = 0; // keep it simple stupid.

			_diff1.InitScroll();
			_diff2.InitScroll();

			YataGrid table;
			if      (_diff1 == Table) table = _diff1;
			else if (_diff2 == Table) table = _diff2;
			else                      table = null;

			if (table != null)
				table.Invalidator(YataGrid.INVALID_GRID);
		}
		#endregion Handlers (tab)


		#region Methods (tab)
		/// <summary>
		/// Helper for
		/// <c><see cref="tabclick_DiffReset()">tabclick_DiffReset()</see></c>.
		/// </summary>
		/// <param name="table">a <c><see cref="YataGrid"/></c></param>
		/// <remarks>Check that <paramref name="table"/> is not null before
		/// call.</remarks>
		void DiffReset(YataGrid table)
		{
			for (int r = 0; r != table.RowCount; ++r)
			for (int c = 0; c != table.ColCount; ++c)
			{
				table[r,c].diff = false;
			}

			if (table == Table)
				opsclick_AutosizeCols(null, EventArgs.Empty);
			else
				AutosizeCols(table);
		}

		/// <summary>
		/// The yata-diff routine.
		/// </summary>
		/// <returns><c>true</c> if differences are found</returns>
		bool Diff()
		{
			_diff1.ClearSelects(true, true);	// sync table
			_diff2.ClearSelects();				// currently active table


			bool isDiff = false;

			string copyable = String.Empty;

			int fields1 = _diff1.Fields.Length;				// check colhead count ->
			int fields2 = _diff2.Fields.Length;
			if (fields1 != fields2)
			{
				isDiff = true;
				copyable = "Head count: (a) " + fields1 + "  (b) " + fields2;
			}

			int fields = Math.Min(fields1, fields2);		// diff fields ->
			for (int f = 0; f != fields; ++f)
			{
				if (_diff1.Fields[f] != _diff2.Fields[f])
				{
					isDiff = true;
					if (copyable.Length != 0)
						copyable += Environment.NewLine;

					copyable += "Head #" + f + ": (a) " + _diff1.Fields[f] + "  (b) " + _diff2.Fields[f];
				}
			}


			bool prelinedone = false;

			int cols1 = _diff1.ColCount;					// check col count ->
			int cols2 = _diff2.ColCount;
			if (cols1 != cols2)
			{
				isDiff = true;
				if (copyable.Length != 0)
				{
					copyable += Environment.NewLine + Environment.NewLine;
					prelinedone = true;
				}
				copyable += "Col count: (a) " + cols1 + "  (b) " + cols2;
			}

			int rows1 = _diff1.RowCount;					// check row count ->
			int rows2 = _diff2.RowCount;
			if (rows1 != rows2)
			{
				isDiff = true;
				if (copyable.Length != 0)
				{
					copyable += Environment.NewLine;
					if (!prelinedone)
						copyable += Environment.NewLine;
				}
				copyable += "Row count: (a) " + rows1 + "  (b) " + rows2;
			}


			prelinedone = false;

			int celldiffs = 0;

			int cols = Math.Min(cols1, cols2);				// diff cells ->
			int rows = Math.Min(rows1, rows2);
			for (int r = 0; r != rows; ++r)
			for (int c = 0; c != cols; ++c)
			{
				if (_diff1[r,c].text != _diff2[r,c].text)
				{
					++celldiffs;

					_diff1[r,c].diff =
					_diff2[r,c].diff = true;
				}
			}

			bool @goto = false;
			if (celldiffs != 0)
			{
				@goto = true;
				if (copyable.Length != 0)
				{
					copyable += Environment.NewLine + Environment.NewLine;
					prelinedone = true;
				}
				copyable += "Cell texts: " + celldiffs + " (inclusive)";
			}

			celldiffs = 0;

			if (cols1 > cols2)								// diff cols of the wider table ->
			{
				for (int c = cols2; c != cols1; ++c)
				for (int r = 0;     r != rows1; ++r)
				{
					++celldiffs;
					_diff1[r,c].diff = true;
				}
			}
			else if (cols2 > cols1)
			{
				for (int c = cols1; c != cols2; ++c)
				for (int r = 0;     r != rows2; ++r)
				{
					++celldiffs;
					_diff2[r,c].diff = true;
				}
			}

			if (rows1 > rows2)								// diff rows of the longer table ->
			{
				for (int c = 0;     c != cols;  ++c)
				for (int r = rows2; r != rows1; ++r)
				{
					++celldiffs;
					_diff1[r,c].diff = true;
				}
			}
			else if (rows2 > rows1)
			{
				for (int c = 0;     c != cols;  ++c)
				for (int r = rows1; r != rows2; ++r)
				{
					++celldiffs;
					_diff2[r,c].diff = true;
				}
			}

			if (celldiffs != 0)
			{
				@goto = true;
				if (copyable.Length != 0)
				{
					copyable += Environment.NewLine;
					if (!prelinedone)
						copyable += Environment.NewLine;
				}
				copyable += "Cell texts: " + celldiffs + " (exclusive)";
			}


			string label;
			Color color;
			if (copyable.Length == 0)
			{
				label = "Tables are identical.";
				color = Colors.Text;
			}
			else
			{
				label = "Tables are different.";
				color = Color.Firebrick;
			}

			string title = " diff (a) "
						 + Path.GetFileNameWithoutExtension(_diff1.Fullpath)
						 + " - (b) "
						 + Path.GetFileNameWithoutExtension(_diff2.Fullpath);

			_fdiffer = new DifferDialog(title,
										label,
										copyable,
										this,
										color,
										@goto,
										@goto || isDiff);
			return isDiff || @goto;
		}

		/// <summary>
		/// Selects the next diffed cell in the table (or both tables if both
		/// are valid).
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
			if ((ModifierKeys & Keys.Alt) == Keys.None)
			{
				if (WindowState == FormWindowState.Minimized)
					WindowState  = FormWindowState.Normal;
				else
				{
					TopMost = true;
					TopMost = false;
				}

				bool bypassFocus = (ModifierKeys & Keys.Control) == Keys.Control;
				if (bypassFocus) _fdiffer.Activate();


				if (Table != null
					&& (_diff1 != null  || _diff2 != null)
					&& (_diff1 == Table || _diff2 == Table))
				{
					if (!bypassFocus) Table.Select();


					YataGrid table; // the other table - can be null.

					if (Table == _diff1) table = _diff2;
					else                 table = _diff1;

					Cell sel = Table.getSelectedCell();
					int rStart = Table.getSelectedRow();

					Table.ClearSelects();

					if (table != null)
						table.ClearSelects(true, true);

					int r,c;

					bool start = true;

					if ((ModifierKeys & Keys.Shift) == Keys.None) // forward goto ->
					{
						if (sel != null) { c = sel.x; rStart = sel.y; }
						else
						{
							c = -1;
							if (rStart == -1) rStart = 0;
						}

						for (r = rStart; r != Table.RowCount; ++r)
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
								if ((sel = Table[r,c]).diff)
								{
									SelectDiffCell(sel, table);
									return;
								}
							}
						}

						// TODO: tighten exact start/end-cells
						for (r = 0; r != rStart + 1;     ++r) // quick and dirty wrap ->
						for (c = 0; c != Table.ColCount; ++c)
						{
							if ((sel = Table[r,c]).diff)
							{
								SelectDiffCell(sel, table);
								return;
							}
						}
					}
					else // backward goto ->
					{
						if (sel != null) { c = sel.x; rStart = sel.y; }
						else
						{
							c = Table.ColCount;
							if (rStart == -1) rStart = Table.RowCount - 1;
						}

						for (r = rStart; r != -1; --r)
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
								if ((sel = Table[r,c]).diff)
								{
									SelectDiffCell(sel, table);
									return;
								}
							}
						}

						// TODO: tighten exact start/end-cells
						for (r = Table.RowCount - 1; r != rStart - 1; --r) // quick and dirty wrap ->
						for (c = Table.ColCount - 1; c != -1;         --c)
						{
							if ((sel = Table[r,c]).diff)
							{
								SelectDiffCell(sel, table);
								return;
							}
						}
					}
				}
			}
			_fdiffer.EnableGotoButton(false);
		}

		/// <summary>
		/// Helper for <c><see cref="GotoDiff()">GotoDiff()</see></c>.
		/// </summary>
		/// <param name="sel">a <c><see cref="Cell"/></c> in the active
		/// <c><see cref="YataGrid"/></c></param>
		/// <param name="table">a sync'd <c>YataGrid</c> - can be <c>null</c></param>
		static void SelectDiffCell(Cell sel, YataGrid table)
		{
			Table.SelectCell(sel, false);

			if (table != null
				&& sel.y < table.RowCount
				&& sel.x < table.ColCount)
			{
				table[sel.y, sel.x].selected = true;
			}
		}


		/// <summary>
		/// Clears all selects on a sync-table if that table is valid.
		/// </summary>
		/// <returns>the sync'd <c><see cref="YataGrid"/></c> if a sync-table is
		/// valid</returns>
		internal YataGrid ClearSyncSelects()
		{
			YataGrid table;
			if      (Table == _diff1) table = _diff2;
			else if (Table == _diff2) table = _diff1;
			else
				return null;

			if (table != null)
				table.ClearSelects(true, true);

			return table;
		}
		#endregion Methods (tab)
	}
}
