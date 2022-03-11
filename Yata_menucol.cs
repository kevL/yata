using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Handlers (editcol)
		/// <summary>
		/// Handles the <c>DropDownOpening</c> event for
		/// <c><see cref="it_MenuCol"/></c>. Deters if subits ought be enabled.
		/// </summary>
		/// <param name="sender"><c>it_MenuCol</c></param>
		/// <param name="e"></param>
		void col_dropdownopening(object sender, EventArgs e)
		{
			if (Table != null)
			{
				bool isColSelected = Table.getSelectedCol() > 0; // id-col is disallowed

				it_DeselectCol.Enabled = isColSelected;

				it_CreateHead .Enabled = !Table.Readonly;
				it_DeleteHead .Enabled = !Table.Readonly && isColSelected && Table.ColCount > 2;
				it_RelabelHead.Enabled = !Table.Readonly && isColSelected;

				it_CopyCells  .Enabled = isColSelected;
				it_PasteCells .Enabled = isColSelected && !Table.Readonly && _copyc.Count != 0;
			}
			else
			{
				it_DeselectCol.Enabled =

				it_CreateHead .Enabled =
				it_DeleteHead .Enabled =
				it_RelabelHead.Enabled =

				it_CopyCells  .Enabled =
				it_PasteCells .Enabled = false;
			}
		}


		/// <summary>
		/// Deselects <c><see cref="Col">Col</see></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_DeselectCol"/></c></param>
		/// <param name="e"></param>
		void editcolclick_Deselect(object sender, EventArgs e)
		{
			int selc = Table.getSelectedCol();
			Table.Cols[selc].selected = false;

			int selr = Table.getSelectedRow();
			for (int r = 0; r != Table.RowCount; ++r)
			{
				if (   (r > selr && r > selr + Table.RangeSelect)
					|| (r < selr && r < selr + Table.RangeSelect))
				{
					Table[r, selc].selected = false;
				}
			}

			YataGrid table; // do special sync ->

			if      (Table == _diff1) table = _diff2;
			else if (Table == _diff2) table = _diff1;
			else                      table = null;

			if (table != null)
			{
				selc = table.getSelectedCol();
				table.Cols[selc].selected = false;

				selr = table.getSelectedRow();
				for (int r = 0; r != table.RowCount; ++r)
				{
					if (   (r > selr && r > selr + table.RangeSelect)
						|| (r < selr && r < selr + table.RangeSelect))
					{
						table[r, selc].selected = false;
					}
				}
			}

			EnableCelleditOperations();

			Table.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_COLS); // NOTE: INVALID_COLS does not appear to be needed.
		}


		const string _warnColhead = "This operation cannot be undone. It clears the Undo/Redo stacks.";
//								  + " Tip: tidy and save the 2da first.";

		/// <summary>
		/// Opens a text-input dialog for creating a col at a selected col-id or
		/// at the far right if no col is selected.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CreateHead"/></c></param>
		/// <param name="e"></param>
		void editcolclick_CreateHead(object sender, EventArgs e)
		{
			const string head = _warnColhead + " Are you sure you want to create a col ...";

			using (var ib = new Infobox(Infobox.Title_infor,
										head,
										null,
										InfoboxType.Info,
										InfoboxButtons.CancelYes))
			{
				if (ib.ShowDialog(this) == DialogResult.OK)
				{
					int selc = Table.getSelectedCol();
					using (var idc = new InputDialog(this, selc))
					{
						if (idc.ShowDialog(this) == DialogResult.OK
							&& InputDialog._colabel.Length != 0)
						{
							Obfuscate();
							DrawRegulator.SuspendDrawing(Table);

							// create at far right if no col selected
							if (selc < Table.FrozenCount) // ~safety.
								selc = Table.ColCount;

							steadystate();

							Table.CreateCol(selc);

							it_freeze1.Enabled = Table.ColCount > 1;
							it_freeze2.Enabled = Table.ColCount > 2;

							DrawRegulator.ResumeDrawing(Table);
							Obfuscate(false);
						}
					}
				}
			}
		}

		/// <summary>
		/// Deletes a selected col w/ confirmation.
		/// </summary>
		/// <param name="sender"><c><see cref="it_DeleteHead"/></c></param>
		/// <param name="e"></param>
		void editcolclick_DeleteHead(object sender, EventArgs e)
		{
			int selc = Table.getSelectedCol();

			const string head = _warnColhead + " Are you sure you want to delete the selected col ...";
			using (var ib = new Infobox(Infobox.Title_infor,
										head,
										Table.Fields[selc - 1],
										InfoboxType.Info,
										InfoboxButtons.CancelYes))
			{
				if (ib.ShowDialog(this) == DialogResult.OK)
				{
					Obfuscate();
					DrawRegulator.SuspendDrawing(Table);

					steadystate();

					Table.DeleteCol(selc);

					it_freeze1.Enabled = Table.ColCount > 1;
					it_freeze2.Enabled = Table.ColCount > 2;

					DrawRegulator.ResumeDrawing(Table);
					Obfuscate(false);
				}
			}
		}

		/// <summary>
		/// Puts the table in a neutral state.
		/// </summary>
		/// <remarks>Helper for
		/// <list type="bullet">
		/// <item><c><see cref="editcolclick_CreateHead()">editcolclick_CreateHead()</see></c></item>
		/// <item><c><see cref="editcolclick_DeleteHead()">editcolclick_DeleteHead()</see></c></item>
		/// </list></remarks>
		void steadystate()
		{
			it_freeze1.Checked =
			it_freeze2.Checked = false;
			Table.FrozenCount = YataGrid.FreezeId;

			Table.ClearSelects();
			Table.ClearLoadchanged();

			tabclick_DiffReset(null, EventArgs.Empty);

			if (Table.Propanel != null && Table.Propanel.Visible)
			{
				Table.Propanel.Hide();
				it_PropanelLoc    .Enabled =
				it_PropanelLoc_pre.Enabled = false;
			}

			Table._ur.Clear();
		}

		/// <summary>
		/// Relabels the colhead of a selected col.
		/// </summary>
		/// <param name="sender"><c><see cref="it_RelabelHead"/></c></param>
		/// <param name="e"></param>
		void editcolclick_RelabelHead(object sender, EventArgs e)
		{
			int selc = Table.getSelectedCol();

			string head = Table.Fields[selc - 1];
			InputDialog._colabel = head;
			using (var idc = new InputDialog(this, selc))
			{
				if (idc.ShowDialog(this) == DialogResult.OK
					&& InputDialog._colabel.Length != 0
					&& InputDialog._colabel != head)
				{
					Table.RelabelCol(selc);
				}
			}
		}

		/// <summary>
		/// Copies all cell-fields in a selected col to
		/// <c><see cref="_copyc"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CopyCell"/></c></param>
		/// <param name="e"></param>
		void editcolclick_CopyCol(object sender, EventArgs e)
		{
			_copyc.Clear();

			int selc = Table.getSelectedCol();

			for (int r = 0; r != Table.RowCount; ++r)
				_copyc.Add(Table[r, selc].text);

			if (_fclip != null)
				_fclip.SetColBufferText();
		}

		/// <summary>
		/// Pastes <c><see cref="_copyc"/></c> to the cell-fields of a selected
		/// col.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PasteCell"/></c></param>
		/// <param name="e"></param>
		void editcolclick_PasteCol(object sender, EventArgs e)
		{
			int diff; string head;
			if (Table.RowCount < _copyc.Count)
			{
				diff = _copyc.Count - Table.RowCount;
				head = "The table has " + diff + " less row" + (diff == 1 ? String.Empty : "s") + " than the copy.";
			}
			else if (Table.RowCount > _copyc.Count)
			{
				diff = Table.RowCount - _copyc.Count;
				head = "The copy has " + diff + " less row" + (diff == 1 ? String.Empty : "s") + " than the table.";
			}
			else { diff = 0; head = null; }

			if (diff != 0)
			{
				using (var ib = new Infobox(Infobox.Title_warni,
											head + " Proceed ...",
											null,
											InfoboxType.Warn,
											InfoboxButtons.CancelYes))
				{
					if (ib.ShowDialog(this) == DialogResult.OK)
						diff = 0;
				}
			}

			if (diff == 0)
			{
				Obfuscate();
				DrawRegulator.SuspendDrawing(Table);

				Table.PasteCol(_copyc);

				DrawRegulator.ResumeDrawing(Table);
				Obfuscate(false);
			}
		}
		#endregion Handlers (editcol)
	}
}
