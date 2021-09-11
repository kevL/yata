using System;


namespace yata
{
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
		/// <param name="range">the range of row-ids to select (+/-)</param>
		/// <param name="strt_r">ref to start row-id</param>
		/// <param name="stop_r">ref to stop row-id</param>
		void asStartStop(int selr, int range, out int strt_r, out int stop_r)
		{
			if (range < 0)
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
		void row_RangeSelect(int strt_r, int stop_r)
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
		#endregion contiguous cell selection
	}
}
