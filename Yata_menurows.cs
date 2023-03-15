using System;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Handlers (editrows)
		/// <summary>
		/// Deselects all <c><see cref="Row">Rows</see></c> and subrows as well
		/// as all <c><see cref="Cell">Cells</see></c> in
		/// <c><see cref="YataGrid"/>.FrozenPanel</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_DeselectRows"/></c></param>
		/// <param name="e"></param>
		void editrowsclick_Deselect(object sender, EventArgs e)
		{
			foreach (var row in Table.Rows)
			{
				if (row.selected)
					row.selected = false;

				for (int c = 0; c != Table.ColCount; ++c)
				{
					if (!Table.Cols[c].selected)
						row[c].selected = false;
				}
			}

			YataGrid table; // do special sync ->

			if      (Table == _diff1) table = _diff2;
			else if (Table == _diff2) table = _diff1;
			else                      table = null;

			if (table != null)
			{
				foreach (var row in table.Rows)
				{
					if (row.selected)
					{
						Row.BypassEnableRowedit = true;
						row.selected = false;
						Row.BypassEnableRowedit = false;
					}
	
					for (int c = 0; c != table.ColCount; ++c)
					{
						if (!table.Cols[c].selected)
							row[c].selected = false;
					}
				}
			}

			EnableCelleditOperations();

			Table.Invalidator(YataGrid.INVALID_GRID
							| YataGrid.INVALID_FROZ
							| YataGrid.INVALID_ROWS);
		}


		/// <summary>
		/// Copies the fields of a range of <c><see cref="Row">Rows</see></c>
		/// and deletes the <c>Rows</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CutRange"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Rows|Cut <c>[Ctrl+Shift+x]</c></item>
		/// </list></remarks>
		void editrowsclick_CutRange(object sender, EventArgs e)
		{
			if (!isTableDiffed())
			{
				editrowsclick_CopyRange(  sender, e);
				editrowsclick_DeleteRange(sender, e);
			}
			else
				error_TableDiffed();
		}

		/// <summary>
		/// Copies the fields of a range of <c><see cref="Row">Rows</see></c>
		/// and enables <c><see cref="it_PasteRange"/></c> and
		/// <c><see cref="it_ClipExport"/></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_CopyRange"/></c></item>
		/// <item><c><see cref="it_CutRange"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Rows|Copy <c>[Ctrl+Shift+c]</c></item>
		/// <item>Rows|Cut <c>[Ctrl+Shift+x]</c>
		/// <c><see cref="editrowsclick_CutRange()">editrowsclick_CutRange()</see></c></item>
		/// </list></remarks>
		void editrowsclick_CopyRange(object sender, EventArgs e)
		{
			_copyr.Clear();

			int selr = Table.getSelectedRow();

			int strt, stop;
			if (Table.RangeSelect > 0) { strt = selr; stop = selr + Table.RangeSelect; }
			else                       { strt = selr + Table.RangeSelect; stop = selr; }

			string[] celltexts;
			do
			{
				celltexts = new string[Table.ColCount];
				for (int c = 0; c != Table.ColCount; ++c)
					celltexts[c] = Table[strt, c].text;

				_copyr.Add(celltexts);
			}
			while (++strt <= stop);

			if (_fclip != null)
				_fclip.SetRowsBufferText();

			it_PasteRange.Enabled = !Table.Readonly;
			it_ClipExport.Enabled = true;
		}

		/// <summary>
		/// Creates a range of <c><see cref="Row">Rows</see></c> and pastes
		/// copied fields into them.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PasteRange"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Rows|Paste <c>[Ctrl+Shift+v]</c></item>
		/// </list></remarks>
		void editrowsclick_PasteRange(object sender, EventArgs e)
		{
			if (!isTableDiffed())
			{
				Obfuscate();
				DrawRegulator.SuspendDrawing(Table);


				Restorable rest = UndoRedo.createArray(_copyr.Count, UndoRedo.UrType.rt_ArrayDelete);

				int selr = Table.getSelectedRow();
				if (selr == -1)
					selr = Table.RowCount;

				int r = selr;
				for (int i = 0; i != _copyr.Count; ++i, ++r)
				{
					Table.Insert(r, _copyr[i], Brushers.Created, true);
					rest.array[i] = Table.Rows[r].Clone() as Row;
				}
				Table.Calibrate(selr, _copyr.Count - 1); // paste range

				Table.ClearSelects(false, true);
				Table.Rows[selr].selected = true;
				Table.RangeSelect = _copyr.Count - 1;
				Table.EnsureDisplayedRow(selr);


				if (!Table.Changed)
				{
					Table.Changed = true;
					rest.isSaved = UndoRedo.IsSavedType.is_Undo;
				}
				Table._ur.Push(rest);


				DrawRegulator.ResumeDrawing(Table);
				Obfuscate(false);

				if (Options._autorder && order() != 0) layout();
			}
			else
				error_TableDiffed();
		}

		/// <summary>
		/// Deletes a range of <c><see cref="Row">Rows</see></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_DeleteRange"/></c></item>
		/// <item><c><see cref="it_CutRange"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Rows|Delete <c>[Shift+Delete]</c></item>
		/// <item>Rows|Cut <c>[Ctrl+Shift+x]</c>
		/// <c><see cref="editrowsclick_CutRange()">editrowsclick_CutRange()</see></c></item>
		/// </list></remarks>
		void editrowsclick_DeleteRange(object sender, EventArgs e)
		{
			if (!isTableDiffed())
			{
				Table.DeleteRows();

				EnableRoweditOperations();
				EnableGotoReplaced(Table.anyReplaced());
				EnableGotoLoadchanged(Table.anyLoadchanged());

				if (Options._autorder && order() != 0) layout();
			}
			else
				error_TableDiffed();
		}


		/// <summary>
		/// Instantiates <c><see cref="RowCreatorDialog"/></c> for
		/// inserting/creating multiple rows.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CreateRows"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Rows|Create ... <c>[F2]</c></item>
		/// </list></remarks>
		void editrowsclick_CreateRows(object sender, EventArgs e)
		{
			if (!isTableDiffed())
			{
				int selr = Table.getSelectedRowOrCells();

				using (var rcd = new RowCreatorDialog(this, selr, _copyr.Count != 0))
				{
					if (rcd.ShowDialog(this) == DialogResult.OK)
					{
						Obfuscate();
						DrawRegulator.SuspendDrawing(Table);


						Restorable rest = UndoRedo.createArray(_lengthCr, UndoRedo.UrType.rt_ArrayDelete);

						var cells = new string[Table.ColCount];
						switch (_fillCr)
						{
							case CrFillType.Stars:
								for (int i = 0; i != Table.ColCount; ++i)
									cells[i] = gs.Stars;
								break;

							case CrFillType.Selected:
								for (int i = 0; i != Table.ColCount; ++i)
									cells[i] = Table[selr, i].text;
								break;

							case CrFillType.Copied:
								for (int i = 0; i != Table.ColCount; ++i)
								{
									if (i < _copyr[0].Length)
										cells[i] = _copyr[0][i];
									else
										cells[i] = gs.Stars;
								}
								break;
						}

						int r = _startCr;
						for (int i = 0; i != _lengthCr; ++i, ++r)
						{
							cells[0] = r.ToString(CultureInfo.InvariantCulture);

							Table.Insert(r, cells, Brushers.Created, true);
							rest.array[i] = Table.Rows[r].Clone() as Row;
						}
						Table.Calibrate(_startCr, _lengthCr - 1); // insert range

						Table.ClearSelects(false, true);
						Table.Rows[_startCr].selected = true;
						Table.RangeSelect = _lengthCr - 1;
						Table.EnsureDisplayedRow(_startCr);


						if (!Table.Changed)
						{
							Table.Changed = true;
							rest.isSaved = UndoRedo.IsSavedType.is_Undo;
						}
						Table._ur.Push(rest);


						DrawRegulator.ResumeDrawing(Table);
						Obfuscate(false);

						if (Options._autorder && order() != 0) layout();
					}
				}
			}
			else
				error_TableDiffed();
		}
		#endregion Handlers (editrows)


		#region Methods (editrows)
		/// <summary>
		/// Determines the dis/enabled states of row-edit operations.
		/// </summary>
		internal void EnableRoweditOperations()
		{
			bool isrowselected = (Table.getSelectedRow() != -1);

			it_DeselectRows.Enabled = isrowselected;

			it_CutRange    .Enabled = !Table.Readonly && isrowselected;
			it_CopyRange   .Enabled = isrowselected;
			it_PasteRange  .Enabled = !Table.Readonly && _copyr.Count != 0;
			it_DeleteRange .Enabled = !Table.Readonly && isrowselected;

			it_CreateRows  .Enabled = !Table.Readonly;
		}
		#endregion Methods (editrows)
	}
}
