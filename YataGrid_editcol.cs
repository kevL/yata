using System;
using System.Collections.Generic;


namespace yata
{
	// Routines for col edits.
	sealed partial class YataGrid
	{
		#region Edit cols
		/// <summary>
		/// Creates a col.
		/// </summary>
		/// <param name="selc"></param>
		/// <seealso cref="CreateCols()"><c>CreateCols()</c></seealso>
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
					col.SetWidth(col._widthtext + _padHori * 2 + _padHoriSort);
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
			if (w > Cols[++selc].Width) Cols[selc].SetWidth(w);

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
		#endregion Edit cols
	}
}
