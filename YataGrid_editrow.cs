using System;
using System.Drawing;


namespace yata
{
	// Routines for row edits.
	sealed partial class YataGrid
	{
		/// <summary>
		/// Inserts a <c><see cref="Row"/></c> into the table.
		/// </summary>
		/// <param name="rowid">row-id to insert at</param>
		/// <param name="fields">an array of fields</param>
		/// <param name="calibrate"><c>true</c> to re-layout the grid or
		/// <c>false</c> if <c><see cref="Calibrate()">Calibrate()</see></c>
		/// will be done by the caller</param>
		/// <param name="brush">a <c>Brush</c> to use for Undo/Redo</param>
		internal void Insert(int rowid,
							 string[] fields,
							 bool calibrate = true,
							 Brush brush = null)
		{
			if (calibrate)
				DrawRegulator.SuspendDrawing(this);

			if (fields != null)
			{
				if (brush == null)
					brush = Brushes.Created;

				var row = new Row(rowid, ColCount, brush, this);

				string field;
				for (int c = 0; c != ColCount; ++c)
				{
					if (c < fields.Length)
						field = fields[c];
					else
						field = gs.Stars;

					row[c] = new Cell(rowid, c, field);
				}

				Rows.Insert(rowid, row);
				++RowCount;

				for (int r = rowid + 1; r != RowCount; ++r) // straighten out row._id and cell.y ->
				{
					++(row = Rows[r])._id;
					for (int c = 0; c != ColCount; ++c)
						++row[c].y;
				}
			}

			if (calibrate) // is only 1 row (no range) via context single-row edit
			{
				Calibrate(rowid);

				if (rowid < RowCount)
					EnsureDisplayedRow(rowid);

				DrawRegulator.ResumeDrawing(this);
			}
		}

		/// <summary>
		/// Deletes a <c><see cref="Row"/></c> from this <c>YataGrid</c>.
		/// </summary>
		/// <param name="idr">row-id to delete</param>
		/// <param name="calibrate"><c>true</c> to re-layout the grid or
		/// <c>false</c> if <c><see cref="Calibrate()">Calibrate()</see></c>
		/// will be done by the caller</param>
		internal void Delete(int idr, bool calibrate = true)
		{
			if (calibrate)
				DrawRegulator.SuspendDrawing(this);

			Row row;

			Rows.Remove(Rows[idr]);
			--RowCount;

			for (int r = idr; r != RowCount; ++r) // straighten out row._id and cell.y ->
			{
				--(row = Rows[r])._id;
				for (int c = 0; c != ColCount; ++c)
					--row[c].y;
			}

			if (RowCount == 0) // add a row of stars so grid is not left blank ->
			{
				++RowCount;

				row = new Row(0, ColCount, Brushes.Created, this);

				int c = 0;
				if (Settings._autorder)
					row[c++] = new Cell(0,0, "0");

				for (; c != ColCount; ++c)
					row[c] = new Cell(0, c, gs.Stars);

				Rows.Add(row);

				if (calibrate)
				{
					Calibrate(0);
					DrawRegulator.ResumeDrawing(this);

					return;
				}
			}

			if (calibrate) // is only 1 row (no range) via context single-row edit
			{
				Calibrate();

				if (idr < RowCount)
					EnsureDisplayedRow(idr);

				DrawRegulator.ResumeDrawing(this);
			}
		}

		/// <summary>
		/// Deletes a single or multiple <c><see cref="Row">Rows</see></c>.
		/// </summary>
		/// <remarks>Called by
		/// <c><see cref="Yata"/>.editrowsclick_DeleteRange()</c>.</remarks>
		internal void DeleteRows()
		{
			_f.Obfuscate();
			DrawRegulator.SuspendDrawing(this);


			int selr = getSelectedRow();

			int range = Math.Abs(RangeSelect);
			Restorable rest = UndoRedo.createArray(range + 1, UndoRedo.UrType.rt_ArrayInsert);

			int rFirst, rLast;
			if (RangeSelect > 0) { rFirst = selr; rLast = selr + RangeSelect; }
			else                 { rFirst = selr + RangeSelect; rLast = selr; }

			while (rLast >= rFirst) // reverse delete.
			{
				rest.array[range--] = Rows[rLast].Clone() as Row;
				Delete(rLast, false);

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


			DrawRegulator.ResumeDrawing(this);
			_f.Obfuscate(false);
		}
	}
}
