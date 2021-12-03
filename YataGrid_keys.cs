using System;


namespace yata
{
	// Various routines for cell/row/col selection and subselection.
	sealed partial class YataGrid
	{
		#region row selection
		/// <summary>
		/// Selects a <c><see cref="Row"/></c>.
		/// </summary>
		/// <param name="selr">the currently selected row-id</param>
		/// <param name="r">the row-id to select</param>
		/// <returns>the row-id that gets selected</returns>
		int row_SelectRow(int selr, int r)
		{
			Rows[selr].selected = false;

			ClearCellSelects(true);
			SelectRow(r);

			if (FrozenCount < ColCount)
				_anchorcell = this[r, FrozenCount];

			return r;
		}
		#endregion row selection


		#region row subselection
		/// <summary>
		/// Assigns row-id start/stop values.
		/// </summary>
		/// <param name="selr">the currently selected row-id</param>
		/// <param name="strt_r">ref to start row-id</param>
		/// <param name="stop_r">ref to stop row-id</param>
		/// <remarks>Ensure that <c><see cref="RangeSelect"/></c> has been set
		/// properly before call.</remarks>
		void asStartStop_range(int selr, out int strt_r, out int stop_r)
		{
			if (RangeSelect < 0)
			{
				strt_r = selr + RangeSelect;
				stop_r = selr;
			}
			else
			{
				strt_r = selr;
				stop_r = selr + RangeSelect;
			}
		}

		/// <summary>
		/// Subselects a range of <c><see cref="Row">Rows</see></c>.
		/// </summary>
		/// <param name="strt_r">start row-id</param>
		/// <param name="stop_r">stop row-id</param>
		void row_SelectRangeCells(int strt_r, int stop_r)
		{
			ClearCellSelects(true);

			for (int r = strt_r; r <= stop_r;   ++r)
			for (int c = 0;      c != ColCount; ++c)
			{
				this[r,c].selected = true;
			}
		}

		/// <summary>
		/// Gets the sync-table of this <c>YataGrid</c>.
		/// </summary>
		/// <returns><c>null</c> if there is no valid sync-table</returns>
		YataGrid getSynctable()
		{
			if (this == _f._diff1) return _f._diff2;
			if (this == _f._diff2) return _f._diff1;

			return null;
		}

		/// <summary>
		/// Sets this <c>YataGrid's</c> <c><see cref="RangeSelect"/></c> and
		/// ensures that it does not exceed <c><see cref="RowCount"/></c>.
		/// </summary>
		/// <param name="selr">the currently selected row-id</param>
		/// <param name="range">the range of row-ids to select (+/-)</param>
		/// <remarks>Used for a sync-table.</remarks>
		void setRangeSelect(int selr, int range)
		{
			if (selr + (RangeSelect = range) >= RowCount)
				RangeSelect = RowCount - selr - 1;
		}
		#endregion row subselection


		#region contiguous cell selection
		/// <summary>
		/// Checks if a contiguous operation is allowable. But does not check if
		/// the currently selected cells actually are contiguous - call
		/// <c><see cref="areSelectedCellsContiguous()">areSelectedCellsContiguous()</see></c>
		/// also before allowing a contiguous operation.
		/// </summary>
		/// <returns></returns>
		/// <remarks>Don't allow multi-cell select if sync'd.</remarks>
		bool allowContiguous()
		{
			return this != _f._diff1 && this != _f._diff2
				&& (    _anchorcell != null
					|| (_anchorcell = getFirstSelectedCell(FrozenCount)) != null);
		}

		/// <summary>
		/// Assigns the start and stop row-ids that shall be used as a range of
		/// <c><see cref="Row">Rows</see></c> that need to be selected during a
		/// horizontal contiguous block selection.
		/// </summary>
		/// <param name="strt_r">ref that holds the start row-id</param>
		/// <param name="stop_r">ref that holds the stop row-id</param>
		/// <returns>the row-id of a <c><see cref="Cell"/></c> that shall be
		/// displayed</returns>
		/// <remarks>Helper for <c><see cref="OnKeyDown()">OnKeyDown()</see></c>
		/// contiguous block selection (horizontal).</remarks>
		int asStartStop_row(out int strt_r, out int stop_r)
		{
			if (_f._copyvert == 1
				|| _anchorcell.y == 0
				|| !this[_anchorcell.y - 1, _anchorcell.x].selected)
			{
				strt_r = _anchorcell.y;
				stop_r = _anchorcell.y + _f._copyvert - 1;

				return stop_r;
			}

			strt_r = _anchorcell.y - _f._copyvert + 1;
			stop_r = _anchorcell.y;

			return strt_r;
		}

		/// <summary>
		/// Assigns the start and stop col-ids that shall be used as a range of
		/// <c><see cref="Col">Cols</see></c> that need to be selected during a
		/// vertical contiguous block selection.
		/// </summary>
		/// <param name="strt_c">ref that holds the start col-id</param>
		/// <param name="stop_c">ref that holds the stop col-id</param>
		/// <returns>the col-id of a <c><see cref="Cell"/></c> that shall be
		/// displayed</returns>
		/// <remarks>Helper for <c><see cref="OnKeyDown()">OnKeyDown()</see></c>
		/// contiguous block selection (vertical).</remarks>
		int asStartStop_col(out int strt_c, out int stop_c)
		{
			if (_f._copyhori == 1
				|| _anchorcell.x == FrozenCount
				|| !this[_anchorcell.y, _anchorcell.x - 1].selected)
			{
				strt_c = _anchorcell.x;
				stop_c = _anchorcell.x + _f._copyhori - 1;

				return stop_c;
			}

			strt_c = _anchorcell.x - _f._copyhori + 1;
			stop_c = _anchorcell.x;

			return strt_c;
		}

		/// <summary>
		/// Gets the col-id with selected cell that is furthest away from the
		/// current <c><see cref="_anchorcell">_anchorcell's</see></c>
		/// <c><see cref="Col"/></c>. Can return the col-id of the
		/// <c>_anchorcell</c> itself.
		/// </summary>
		/// <returns>col-id</returns>
		int getAnchorRangedColid()
		{
			int colid = _anchorcell.x;

			for (int c = ColCount - 1; c >= _anchorcell.x; --c)
			if (this[_anchorcell.y, c].selected)
			{
				colid = c;
				break;
			}

			if (colid == _anchorcell.x)
			{
				for (int c = FrozenCount; c <= _anchorcell.x; ++c)
				if (this[_anchorcell.y, c].selected)
				{
					colid = c;
					break;
				}
			}

			return colid;
		}

		/// <summary>
		/// Gets the row-id with selected cell that is furthest away from the
		/// current <c><see cref="_anchorcell">_anchorcell's</see></c>
		/// <c><see cref="Row"/></c>. Can return the row-id of the
		/// <c>_anchorcell</c> itself.
		/// </summary>
		/// <returns>row-id</returns>
		int getAnchorRangedRowid()
		{
			int rowid = _anchorcell.y;

			for (int r = RowCount - 1; r >= _anchorcell.y; --r)
			if (this[r, _anchorcell.x].selected)
			{
				rowid = r;
				break;
			}

			if (rowid == _anchorcell.y)
			{
				for (int r = 0; r <= _anchorcell.y; ++r)
				if (this[r, _anchorcell.x].selected)
				{
					rowid = r;
					break;
				}
			}

			return rowid;
		}


		/// <summary>
		/// Selects <c><see cref="Cell">Cells</see> in a contiguous block.</c>
		/// </summary>
		/// <param name="strt_r">the start row-id</param>
		/// <param name="stop_r">the stop row-id</param>
		/// <param name="strt_c">the start col-id</param>
		/// <param name="stop_c">the stop col-id</param>
		void SelectCellBlock(int strt_r, int stop_r, int strt_c, int stop_c)
		{
			for (int r = strt_r; r <= stop_r; ++r)
			for (int c = strt_c; c <= stop_c; ++c)
			{
				this[r,c].selected = true;
			}
		}
		#endregion contiguous cell selection
	}
}
