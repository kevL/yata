using System;


namespace yata
{
	sealed partial class YataGrid
	{
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
			SelectRow(selr = r);

			if (FrozenCount < ColCount)
				_anchorcell = this[selr, FrozenCount];

			return selr;
		}
	}
}
