using System;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Handlers (editcells)
		/// <summary>
		/// Deselects all <c><see cref="Cell">Cells</see></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_DeselectCell"/></c></param>
		/// <param name="e"></param>
		void editcellsclick_Deselect(object sender, EventArgs e)
		{
			Table.ClearCellSelects();
			ClearSyncSelects();

			EnableCelleditOperations();

			Table.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
			// TODO: not sure why but that deselects and invalidates a Propanel select also.
		}


		/// <summary>
		/// Cuts an only selected cell or cells in a contiguous block.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CutCell"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Cut <c>[Ctrl+x]</c></item>
		/// </list></remarks>
		void editcellsclick_CutCell(object sender, EventArgs e)
		{
			Cell sel = Table.getFirstSelectedCell();
			Cell cell; string text;

			int invalid = -1;

			_copytext = new string[_copyvert, _copyhori];

			int i = -1, j;
			for (int r = sel.y; r != sel.y + _copyvert; ++r)
			{
				++i; j = -1;
				for (int c = sel.x; c != sel.x + _copyhori; ++c)
				{
					_copytext[i, ++j] = (cell = Table[r,c]).text;

					if (c == 0 && Settings._autorder)
						text = r.ToString(CultureInfo.InvariantCulture);
					else
						text = gs.Stars;

					if (cell.text != text)
					{
						Table.ChangeCellText(cell, text);	// does not do a text-check
						invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
					}
					else
					{
						if (cell.replaced)
							cell.replaced = false;

						if (cell.loadchanged)
							cell.loadchanged = false;

						if (invalid == -1)
							invalid = YataGrid.INVALID_GRID;
					}
				}
			}

			if (_fclip != null)
				_fclip.SetCellsBufferText();

			if (invalid == YataGrid.INVALID_GRID)
				Table.Invalidator(invalid);
		}

		/// <summary>
		/// Copies an only selected cell or cells in a contiguous block.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CopyCell"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Copy <c>[Ctrl+c]</c></item>
		/// </list></remarks>
		void editcellsclick_CopyCell(object sender, EventArgs e)
		{
			Cell sel = Table.getFirstSelectedCell();

			_copytext = new string[_copyvert, _copyhori];

			int i = -1, j;
			for (int r = sel.y; r != sel.y + _copyvert; ++r)
			{
				++i; j = -1;
				for (int c = sel.x; c != sel.x + _copyhori; ++c)
				{
					_copytext[i, ++j] = Table[r,c].text;
				}
			}

			if (_fclip != null)
				_fclip.SetCellsBufferText();
		}

		/// <summary>
		/// Pastes to an only selected cell. If more than one field is in
		/// <c><see cref="_copytext">_copytext[,]</see></c> then the only
		/// selected cell will be the top-left corner of the paste-block; fields
		/// that overflow the table to right or bottom shall be ignored.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_PasteCell"/></c></item>
		/// <item><c><see cref="cellit_Paste"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Paste <c>[Ctrl+v]</c></item>
		/// <item>cell|Paste <c><see cref="cellclick_Paste()">cellclick_Paste()</see></c></item>
		/// </list></remarks>
		void editcellsclick_PasteCell(object sender, EventArgs e)
		{
			Cell sel = Table.getSelectedCell();
			Cell cell; string text;

			int invalid = -1;

			if (sel.x >= Table.FrozenCount)
			{
				for (int r = 0; r != _copytext.GetLength(0) && r + sel.y != Table.RowCount; ++r)
				for (int c = 0; c != _copytext.GetLength(1) && c + sel.x != Table.ColCount; ++c)
				{
					(cell = Table[r + sel.y,
								  c + sel.x]).selected = true;

					if (cell.x == 0 && Settings._autorder)
						text = cell.y.ToString(CultureInfo.InvariantCulture);
					else
						text = _copytext[r,c];

					if (text != cell.text)
					{
						Table.ChangeCellText(cell, text);	// does not do a text-check
						invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
					}
					else
					{
						if (cell.replaced)
							cell.replaced = false;

						if (cell.loadchanged)
							cell.loadchanged = false;

						if (invalid == -1)
							invalid = YataGrid.INVALID_GRID;
					}
				}
			}
			else
			{
				if (sel.x == 0 && Settings._autorder)
					text = sel.y.ToString(CultureInfo.InvariantCulture);
				else
					text = _copytext[0,0];

				if (text != sel.text)
				{
					Table.ChangeCellText(sel, text);	// does not do a text-check
					invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
				}
				else
				{
					if (sel.replaced)
						sel.replaced = false;

					if (sel.loadchanged)
						sel.loadchanged = false;

					invalid = YataGrid.INVALID_GRID;
				}
			}

			if (invalid == YataGrid.INVALID_GRID)
				Table.Invalidator(invalid);

			EnableCelleditOperations();
		}

		/// <summary>
		/// Pastes "****" to all selected cells.
		/// </summary>
		/// <param name="sender"><c><see cref="it_DeleteCell"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Delete <c>[Delete]</c></item>
		/// </list></remarks>
		internal void editcellsclick_Delete(object sender, EventArgs e)
		{
			Cell sel; string text;
			int invalid = -1;

			foreach (var row in Table.Rows)
			for (int c = 0; c != Table.ColCount; ++c)
			{
				if ((sel = row[c]).selected)
				{
					if (c == 0 && Settings._autorder)
						text = sel.y.ToString(CultureInfo.InvariantCulture);
					else
						text = gs.Stars;

					if (sel.text != text)
					{
						Table.ChangeCellText(sel, text);	// does not do a text-check
						invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
					}
					else
					{
						if (sel.replaced)
							sel.replaced = false;

						if (sel.loadchanged)
							sel.loadchanged = false;

						if (invalid == -1)
							invalid = YataGrid.INVALID_GRID;
					}
				}
			}

			if (invalid == YataGrid.INVALID_GRID)
				Table.Invalidator(invalid);
		}

		/// <summary>
		/// Converts all selected cells to lowercase.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Lower"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Lowercase</item>
		/// </list></remarks>
		void editcellsclick_Lower(object sender, EventArgs e)
		{
			Cell sel; string text;
			int invalid = -1;

			foreach (var row in Table.Rows)
			for (int c = 0; c != Table.ColCount; ++c)
			{
				if ((sel = row[c]).selected)
				{
					if (sel.text != (text = sel.text.ToLower(CultureInfo.CurrentCulture)))
					{
						Table.ChangeCellText(sel, text);	// does not do a text-check
						invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
					}
					else
					{
						if (sel.replaced)
							sel.replaced = false;

						if (sel.loadchanged)
							sel.loadchanged = false;

						if (invalid == -1)
							invalid = YataGrid.INVALID_GRID;
					}
				}
			}

			if (invalid == YataGrid.INVALID_GRID)
				Table.Invalidator(invalid);
		}

		/// <summary>
		/// Converts all selected cells to uppercase.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Upper"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Uppercase</item>
		/// </list></remarks>
		void editcellsclick_Upper(object sender, EventArgs e)
		{
			Cell sel; string text;
			int invalid = -1;

			foreach (var row in Table.Rows)
			for (int c = 0; c != Table.ColCount; ++c)
			{
				if ((sel = row[c]).selected)
				{
					if (sel.text != (text = sel.text.ToUpper(CultureInfo.CurrentCulture)))
					{
						Table.ChangeCellText(sel, text);	// does not do a text-check
						invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
					}
					else
					{
						if (sel.replaced)
							sel.replaced = false;

						if (sel.loadchanged)
							sel.loadchanged = false;

						if (invalid == -1)
							invalid = YataGrid.INVALID_GRID;
					}
				}
			}

			if (invalid == YataGrid.INVALID_GRID)
				Table.Invalidator(invalid);
		}

		/// <summary>
		/// Opens a text-input dialog for pasting text to all selected cells.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Apply"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Apply text ...</item>
		/// </list></remarks>
		void editcellsclick_Apply(object sender, EventArgs e)
		{
			using (var tid = new InputCelltextDialog(this))
			{
				if (tid.ShowDialog(this) == DialogResult.OK)
				{
					Cell sel; string text;
					int invalid = -1;

					foreach (var row in Table.Rows)
					for (int c = 0; c != Table.ColCount; ++c)
					{
						if ((sel = row[c]).selected)
						{
							if (c == 0 && Settings._autorder)
								text = sel.y.ToString(CultureInfo.InvariantCulture);
							else
								text = _copytext[0,0];

							if (sel.text != text)
							{
								Table.ChangeCellText(sel, text);	// does not do a text-check
								invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
							}
							else
							{
								if (sel.replaced)
									sel.replaced = false;

								if (sel.loadchanged)
									sel.loadchanged = false;

								if (invalid == -1)
									invalid = YataGrid.INVALID_GRID;
							}
						}
					}

					if (invalid == YataGrid.INVALID_GRID)
						Table.Invalidator(invalid);
				}
			}
		}
		#endregion Handlers (editcells)


		#region Methods (editcells)
		/// <summary>
		/// Determines the dis/enabled states of cell-edit operations.
		/// </summary>
		internal void EnableCelleditOperations()
		{
			bool anyselected = Table.anyCellSelected();
			it_DeselectCell.Enabled = anyselected;

			bool contiguous = Table.areSelectedCellsContiguous();
			it_CutCell     .Enabled = !Table.Readonly && contiguous;
			it_CopyCell    .Enabled = contiguous;

			it_PasteCell   .Enabled = !Table.Readonly && Table.getSelectedCell() != null;

			it_DeleteCell  .Enabled = // TODO: if any selected cell is not 'gs.Stars' or loadchanged
			it_Lower       .Enabled = // TODO: if any selected cell is not lowercase  or loadchanged
			it_Upper       .Enabled = // TODO: if any selected cell is not uppercase  or loadchanged
			it_Apply       .Enabled = !Table.Readonly && anyselected;

			// NOTE: 'it_GotoLoadchanged*.Enabled' shall be detered independently
			// by EnableGotoLoadchanged()
		}
		#endregion Methods (editcells)
	}
}
