using System;
using System.Drawing;


namespace yata
{
	// Routines for row edits.
	sealed partial class YataGrid
	{
		/// <summary>
		/// Inserts a <c><see cref="Row"/></c> into this <c>YataGrid</c>.
		/// </summary>
		/// <param name="rowid">row-id to insert at</param>
		/// <param name="fields">an array of fields</param>
		/// <param name="brush">a <c>Brush</c> to use for Undo/Redo</param>
		/// <param name="bypassCalibrate"><c>false</c> to re-layout the grid or
		/// <c>true</c> if <c><see cref="Calibrate()">Calibrate()</see></c> will
		/// be done by the caller</param>
		/// <remarks>Ensure that <paramref name="fields"/> is valid.</remarks>
		internal void Insert(int rowid,
							 string[] fields,
							 Brush brush,
							 bool bypassCalibrate = false)
		{
//			if (!bypassCalibrate)
//				DrawRegulator.SuspendDrawing(this);

			var row = new Row(rowid, ColCount, brush, this);

			string field;
			for (int c = 0; c != ColCount; ++c)
			{
				if (c < fields.Length)
					field = fields[c];
				else
					field = gs.Stars;

				row[c] = new Cell(rowid, c, field);
				doTextwidth(row[c]);
			}

			Rows.Insert(rowid, row);
			++RowCount;

			for (int r = rowid + 1; r != RowCount; ++r) // straighten out row._id and cell.y ->
			{
				++(row = Rows[r])._id;
				for (int c = 0; c != ColCount; ++c)
					++row[c].y;
			}

			if (!bypassCalibrate)
			{
				Calibrate(rowid);

				if (rowid < RowCount)
					EnsureDisplayedRow(rowid);

//				DrawRegulator.ResumeDrawing(this);
			}
		}

		/// <summary>
		/// Deletes a <c><see cref="Row"/></c> from this <c>YataGrid</c>.
		/// </summary>
		/// <param name="rowid">row-id to delete</param>
		/// <param name="bypassCalibrate"><c>true</c> to re-layout the grid or
		/// <c>false</c> if <c><see cref="Calibrate()">Calibrate()</see></c>
		/// will be done by the caller</param>
		/// <param name="bypassCreate"><c>true</c> to bypass the creation of a
		/// blank row if all rows are deleted - passed by
		/// <list type="bullet">
		/// <item><c><see cref="UndoRedo"/>.DeleteRow()</c></item>
		/// <item><c><see cref="UndoRedo"/>.DeleteArray()</c></item>
		/// <item><c><see cref="Yata"/>.editrowsclick_PasteRangeReplace()</c></item>
		/// </list></param>
		/// <returns>a <c><see cref="Row"/></c> iff a default row has been
		/// created</returns>
		internal Row Delete(int rowid, bool bypassCalibrate = false, bool bypassCreate = false)
		{
//			if (!bypassCalibrate)
//				DrawRegulator.SuspendDrawing(this);


			Rows.Remove(Rows[rowid]);
			--RowCount;

			Row created;
			for (int r = rowid; r != RowCount; ++r) // straighten out row._id and cell.y ->
			{
				--(created = Rows[r])._id;
				for (int c = 0; c != ColCount; ++c)
					--created[c].y;
			}

			if (RowCount == 0 && !bypassCreate) // add a row of stars so grid is not left blank ->
			{
				++RowCount;

				created = new Row(0, ColCount, ColorOptions._rowcreated, this);

				int c = 0;
				if (Options._autorder)
				{
					created[0] = new Cell(0,0, "0");
					doTextwidth(created[0]);

					++c;
				}

				for (; c != ColCount; ++c)
				{
					created[c] = new Cell(0, c, gs.Stars);
					doTextwidth(created[c]);
				}

				Rows.Add(created);

				if (!bypassCalibrate)
				{
					Calibrate(0);
//					DrawRegulator.ResumeDrawing(this);

					return created; // <- that row needs to be added to UndoRedo after the delete operation
				}
			}
			else
				created = null;

			if (!bypassCalibrate)
			{
				Calibrate();

				if (rowid < RowCount)
					EnsureDisplayedRow(rowid);

//				DrawRegulator.ResumeDrawing(this);
			}
			return created;
		}

		/// <summary>
		/// Deletes a single or multiple <c><see cref="Row">Rows</see></c>.
		/// </summary>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item><c><see cref="Yata"/>.editrowsclick_DeleteRange()</c></item>
		/// <item><c><see cref="Yata"/>.editrowsclick_PasteRangeReplace()</c></item>
		/// </list></remarks>
		internal void DeleteRows(bool bypassCreate = false)
		{
			_f.Obfuscate();
//			DrawRegulator.SuspendDrawing(this);


			int selr = getSelectedRow();

			int range = Math.Abs(RangeSelect);
			Restorable rest = UndoRedo.createArray(range + 1, UndoRedo.UrType.rt_ArrayInsert);

			Row created = null;

			int rFirst, rLast;
			if (RangeSelect > 0) { rFirst = selr; rLast = selr + RangeSelect; }
			else                 { rFirst = selr + RangeSelect; rLast = selr; }

			while (rLast >= rFirst) // reverse delete.
			{
				rest.array[range--] = Rows[rLast].Clone() as Row;
				created = Delete(rLast, true, bypassCreate);

				--rLast;
			}

			if (RowCount == 1 && rLast == -1) // ie. if grid was blanked -> ID #0 was auto-inserted.
				rLast = 0;
			else
				rLast = -1; // calibrate all extant rows.

			Calibrate(rLast); // delete key

			if (selr < RowCount)
				EnsureDisplayedRow(selr);

			if (!Changed)
			{
				Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			_ur.Push(rest);

			if (created != null)
			{
				rest = UndoRedo.createArray(1, UndoRedo.UrType.rt_ArrayDelete);
				rest.array[0] = created.Clone() as Row;
				_ur.Push(rest);
				_ur.SetChained(2);
			}


//			DrawRegulator.ResumeDrawing(this);
			_f.Obfuscate(false);
		}
	}
}
