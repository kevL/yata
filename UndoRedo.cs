using System;
using System.Collections.Generic;


namespace yata
{
	/// <summary>
	/// <c>Restorables</c> contain values for the action that's about to be
	/// performed on Undo/Redo. Each <c>Restorable</c> contains an Undo-state
	/// and a Redo-state; they toggle back and forth depending on the action,
	/// <c><see cref="UndoRedo.Undo()">UndoRedo.Undo()</see></c> or
	/// <c><see cref="UndoRedo.Redo()">UndoRedo.Redo()</see></c>.
	/// </summary>
	/// <remarks><c><see cref="UndoRedo"/>._it</c> is the state that is being
	/// undone/redone; it is used by the action that's invoked.</remarks>
	struct Restorable
	{
		internal UndoRedo.UrType RestoreType;

		internal Cell cell;
		internal string pretext;
		internal string postext;

		internal Row r;
		internal Row rPre;
		internal Row rPos;

		internal Row[] array;

		internal UndoRedo.IsSavedType isSaved;
	}


	sealed class UndoRedo
	{
		#region Enums
		internal enum UrType
		{
			rt_Cell,		// 0 cell action

			rt_Insert,		// 1 row actions ->
			rt_Delete,		// 2
			rt_Overwrite,	// 3

			rt_ArrayInsert,	// 4
			rt_ArrayDelete	// 5
		}

		internal enum IsSavedType
		{
			is_None,	// 0 - neither Undo-state nor Redo-state is 2da-saved.
			is_Undo,	// 1 - the Undo-state has been saved.
			is_Redo		// 2 - the Redo-state has been saved.
		}
		#endregion Enums


		#region Fields
		readonly YataGrid _grid;

		readonly Stack<Restorable> Undoables = new Stack<Restorable>(); // states that can be Undone to
		readonly Stack<Restorable> Redoables = new Stack<Restorable>(); // states that can be Redone to

		/// <summary>
		/// <c>_it</c> is the state that the user has most recently invoked by
		/// either <c><see cref="Undo()">Undo()</see></c> or
		/// <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		Restorable _it;
		#endregion Fields


		#region Properties
		/// <summary>
		/// Checks if the Undo operation is allowed.
		/// </summary>
		internal bool CanUndo
		{
			get { return Undoables.Count != 0; }
		}

		/// <summary>
		/// Checks if the Redo operation is allowed.
		/// </summary>
		internal bool CanRedo
		{
			get { return Redoables.Count != 0; }
		}
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="grid">a <c><see cref="YataGrid"/></c> that this
		/// <c>UndoRedo</c> will track changes for</param>
		internal UndoRedo(YataGrid grid)
		{
			_grid = grid;
		}
		#endregion cTor


		#region Methods (static)
		/// <summary>
		/// Instantiates a <c><see cref="Restorable"/></c> cell when user
		/// changes state.
		/// </summary>
		/// <param name="cell">a table <c><see cref="Cell"/></c> object</param>
		/// <returns></returns>
		internal static Restorable createCell(ICloneable cell)
		{
			Restorable it;
			it.RestoreType = UrType.rt_Cell;

			it.cell    = cell.Clone() as Cell;	// is used by current action
			it.pretext = it.cell.text;			// used by Undo
			it.postext = String.Empty;			// used by Redo

			it.r    =							// not used
			it.rPre =							// not used
			it.rPos = null;						// not used

			it.array = null;					// not used

			it.isSaved = IsSavedType.is_None;

			return it;
		}

		/// <summary>
		/// Instantiates a <c><see cref="Restorable"/></c> row when user changes
		/// state - ie inserts or deletes a row.
		/// </summary>
		/// <param name="row">a table <c><see cref="Row"/></c> object</param>
		/// <param name="type"><c><see cref="UrType.rt_Delete">UrType.rt_Delete</see></c>
		/// for a row to be deleted or
		/// <c><see cref="UrType.rt_Insert">UrType.rt_Insert</see></c> for a row
		/// to be inserted</param>
		/// <returns></returns>
		internal static Restorable createRow(ICloneable row, UrType type)
		{
			Restorable it;
			it.RestoreType = type;

			it.cell    = null;			// not used
			it.pretext =				// not used
			it.postext = null;			// not used

			it.r = row.Clone() as Row;	// is used by current action

			it.rPre =					// not used
			it.rPos = null;				// not used

			it.array = null;			// not used

			it.isSaved = IsSavedType.is_None;

			return it;
		}

		/// <summary>
		/// Instantiates a <c><see cref="Restorable"/></c> row when user changes
		/// state - ie changes an existing row.
		/// </summary>
		/// <param name="row">a table <c><see cref="Row"/></c> object</param>
		/// <returns></returns>
		internal static Restorable createRow(ICloneable row)
		{
			Restorable it;
			it.RestoreType = UrType.rt_Overwrite;

			it.cell    = null;				// not used
			it.pretext =					// not used
			it.postext = null;				// not used

			it.r    = null;					// is used by current action
			it.rPre = row.Clone() as Row;	// used by Undo
			it.rPos = null;					// used by Redo

			it.array = null;				// not used

			it.isSaved = IsSavedType.is_None;

			return it;
		}

		/// <summary>
		/// Instantiates a <c><see cref="Restorable"/></c> array of rows when
		/// user changes state.
		/// </summary>
		/// <param name="rows">quantity of rows in the Row-array</param>
		/// <param name="type"><c><see cref="UrType.rt_ArrayDelete">UrType.rt_ArrayDelete</see></c>
		/// for row(s) to be deleted or
		/// <c><see cref="UrType.rt_ArrayInsert">UrType.rt_ArrayInsert</see></c>
		/// for row(s) to be inserted</param>
		/// <returns></returns>
		internal static Restorable createArray(int rows, UrType type)
		{
			Restorable it;
			it.RestoreType = type;

			it.cell    = null;			// not used
			it.pretext =				// not used
			it.postext = null;			// not used

			it.r    =					// not used
			it.rPre =					// not used
			it.rPos = null;				// not used

			it.array = new Row[rows];	// is used by current action

			it.isSaved = IsSavedType.is_None;

			return it;
		}
		#endregion Methods (static)


		#region Methods
		/// <summary>
		/// Clears the <c><see cref="Undoables"/></c> and
		/// <c><see cref="Redoables"/></c> stacks.
		/// </summary>
		internal void Clear()
		{
			Undoables.Clear();
			Redoables.Clear();

			_grid._f.EnableUndo(false);
			_grid._f.EnableRedo(false);
		}


		/// <summary>
		/// User's current state is pushed onto <c><see cref="Undoables"/></c>
		/// on any regular state-change. The stack of
		/// <c><see cref="Redoables"/></c> is cleared.
		/// </summary>
		/// <param name="it">a <c><see cref="Restorable"/></c> object to push
		/// onto the top of the <c>Undoables</c> stack</param>
		internal void Push(Restorable it)
		{
			Undoables.Push(it);
			Redoables.Clear();

			_grid._f.EnableUndo(true);
			_grid._f.EnableRedo(false);
		}


		/// <summary>
		/// Re-determines the
		/// <c><see cref="Restorable.isSaved">Restorable.isSaved</see></c> var
		/// of the <c><see cref="Undoables"/></c> and
		/// <c><see cref="Redoables"/></c> when user saves the 2da-file.
		/// </summary>
		/// <param name="allchanged"><c>true</c> to set all <c>Undoables</c> and
		/// <c>Redoables</c> to
		/// <c><see cref="IsSavedType.is_None">IsSavedType.is_None</see></c></param>
		/// <remarks>It would probably be easier to contain
		/// <c><see cref="Restorable">Restorables</see></c> in <c>Lists</c>
		/// instead of <c>Stacks</c>.</remarks>
		internal void ResetSaved(bool allchanged = false)
		{
			if (Undoables.Count != 0)
			{
				var u = Undoables.ToArray();

				int i = 0;
				if (!allchanged)
					u[i++].isSaved = UndoRedo.IsSavedType.is_Redo;

				for (; i != u.Length; ++i)
					u[i].isSaved = UndoRedo.IsSavedType.is_None;

				Undoables.Clear();
				for (i = u.Length - 1; i != -1; --i)
					Undoables.Push(u[i]);
			}

			if (Redoables.Count != 0)
			{
				var r = Redoables.ToArray();

				int i = 0;
				if (!allchanged)
					r[i++].isSaved = UndoRedo.IsSavedType.is_Undo;

				for (; i != r.Length; ++i)
					r[i].isSaved = UndoRedo.IsSavedType.is_None;

				Redoables.Clear();
				for (i = r.Length - 1; i != -1; --i)
					Redoables.Push(r[i]);
			}
		}

		/// <summary>
		/// Re-determines the y-position (aka row) of all
		/// <c><see cref="Restorable">Restorables</see></c> when user sorts cols.
		/// </summary>
		/// <remarks>The presort-vars do not need to be cleared.
		/// 
		/// 
		/// It would probably be easier to contain
		/// <c><see cref="Restorable">Restorables</see></c> in <c>Lists</c>
		/// instead of <c>Stacks</c>.</remarks>
		internal void ResetY()
		{
			if (Undoables.Count != 0)
			{
				var u = Undoables.ToArray();

				ResetY(ref u);

				Undoables.Clear();
				for (int i = u.Length - 1; i != -1; --i)
					Undoables.Push(u[i]);
			}

			if (Redoables.Count != 0)
			{
				var r = Redoables.ToArray();

				ResetY(ref r);

				Redoables.Clear();
				for (int i = r.Length - 1; i != -1; --i)
					Redoables.Push(r[i]);
			}
		}

		/// <summary>
		/// Helper for <c><see cref="ResetY()">ResetY()</see></c>.
		/// </summary>
		/// <param name="rests"></param>
		void ResetY(ref Restorable[] rests)
		{
			int y;

			Restorable rest;
			for (int i = 0; i != rests.Length; ++i)
			{
				rest = rests[i];

				switch (rest.RestoreType)
				{
					case UrType.rt_Cell: // cell
					{
						y = rest.cell.y;

						int c = rest.cell.x;
						for (int r = 0; r != _grid.RowCount; ++r)
						{
							if (_grid[r,c].y_presort == y)
							{
								rest.cell.y = _grid[r,c].y;
								break;
							}
						}
						break;
					}

					case UrType.rt_Insert: // r
					case UrType.rt_Delete:
						y = rest.r._id;

						for (int r = 0; r != _grid.RowCount; ++r)
						{
							if (_grid.Rows[r]._id_presort == y)
							{
								rest.r._id = _grid.Rows[r]._id;
								break;
							}
						}
						break;

					case UrType.rt_Overwrite: // r,rPre,rPos
						y = rest.rPre._id;

						for (int r = 0; r != _grid.RowCount; ++r)
						{
							if (_grid.Rows[r]._id_presort == y)
							{
//								rest.r   ._id = // no need don't bother gets reset on each Undo/Redo.
								rest.rPre._id =
								rest.rPos._id = _grid.Rows[r]._id;
								break;
							}
						}
						break;

					case UrType.rt_ArrayInsert: // array
					case UrType.rt_ArrayDelete:
						for (int a = 0; a != rest.array.Length; ++a)
						{
							y = rest.array[a]._id;

							for (int r = 0; r != _grid.RowCount; ++r)
							{
								if (_grid.Rows[r]._id_presort == y)
								{
									rest.array[a]._id = _grid.Rows[r]._id;
									break;
								}
							}
						}

						// Sort by id-asc
						Array.Sort(rest.array, (a,b) => a._id.CompareTo(b._id));
						break;
				}
			}
		}


		/// <summary>
		/// Undo's a cell-text change or a row-insert/delete/overwrite or a
		/// row-array insert/delete.
		/// </summary>
		internal void Undo()
		{
			_it = Undoables.Pop();

			_grid.Changed = (_it.isSaved != UndoRedo.IsSavedType.is_Undo);

			switch (_it.RestoreType)
			{
				case UrType.rt_Cell:
					_it.cell.text = _it.pretext;
					RestoreCell();
					break;

				case UrType.rt_Insert:
					InsertRow();
					_it.RestoreType = UrType.rt_Delete;
					break;

				case UrType.rt_Delete:
					DeleteRow();
					_it.RestoreType = UrType.rt_Insert;
					break;

				case UrType.rt_Overwrite:
					_it.r = _it.rPre;
					Overwrite();
					break;

				case UrType.rt_ArrayInsert:
					InsertArray();
					_it.RestoreType = UrType.rt_ArrayDelete;
					break;

				case UrType.rt_ArrayDelete:
					DeleteArray();
					_it.RestoreType = UrType.rt_ArrayInsert;
					break;
			}

			Redoables.Push(_it);
		}

		/// <summary>
		/// Redo's a cell-text change or a row-insert/delete/overwrite or a
		/// row-array insert/delete.
		/// </summary>
		public void Redo()
		{
			_it = Redoables.Pop();

			_grid.Changed = (_it.isSaved != UndoRedo.IsSavedType.is_Redo);

			switch (_it.RestoreType)
			{
				case UrType.rt_Cell:
					_it.cell.text = _it.postext;
					RestoreCell();
					break;

				case UrType.rt_Insert:
					InsertRow();
					_it.RestoreType = UrType.rt_Delete;
					break;

				case UrType.rt_Delete:
					DeleteRow();
					_it.RestoreType = UrType.rt_Insert;
					break;

				case UrType.rt_Overwrite:
					_it.r = _it.rPos;
					Overwrite();
					break;

				case UrType.rt_ArrayInsert:
					InsertArray();
					_it.RestoreType = UrType.rt_ArrayDelete;
					break;

				case UrType.rt_ArrayDelete:
					DeleteArray();
					_it.RestoreType = UrType.rt_ArrayInsert;
					break;
			}

			Undoables.Push(_it);
		}
		#endregion Methods


		#region Methods (actions)
		/// <summary>
		/// Changes cell-text in accord with <c><see cref="Undo()">Undo()</see></c> or
		/// <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		void RestoreCell()
		{
			var cell = _it.cell.Clone() as Cell;

			int
				c = _it.cell.x,
				r = _it.cell.y;

			_grid[r,c] = cell;

			_grid.Colwidth(c,r);
			_grid.metricFrozenControls(c);

			_grid.ClearSelects(true);
			cell.selected = true;
			_grid.EnsureDisplayed(cell);

			_grid._f.EnableCelleditOperations();


			int invalid = YataGrid.INVALID_GRID
						| YataGrid.INVALID_FROZ
						| YataGrid.INVALID_ROWS;
			if (_grid.Propanel != null && _grid.Propanel.Visible)
				invalid |= YataGrid.INVALID_PROP;

			_grid.Invalidator(invalid);
		}

		/// <summary>
		/// Inserts a row in accord with <c><see cref="Undo()">Undo()</see></c>
		/// or <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		void InsertRow()
		{
			Row row = _it.r;

			var fields = new string[row.Length];
			for (int c = 0; c != row.Length; ++c)
				fields[c] = String.Copy(row[c].text);

			int r = row._id;
			_grid.Insert(r, fields, true, row._brush);

			for (int c = 0; c != row.Length; ++c)
				_grid[r,c].loadchanged = row[c].loadchanged;

			_grid.ClearSelects(false, true);
			_grid.Rows[r].selected = true;
			_grid.EnsureDisplayedRow(r);


			int invalid = YataGrid.INVALID_GRID
						| YataGrid.INVALID_FROZ
						| YataGrid.INVALID_ROWS;
			if (_grid.Propanel != null && _grid.Propanel.Visible)
				invalid |= YataGrid.INVALID_PROP;

			_grid.Invalidator(invalid);
		}

		/// <summary>
		/// Deletes a row in accord with <c><see cref="Undo()">Undo()</see></c>
		/// or <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		void DeleteRow()
		{
			int r = _it.r._id;

			_grid.Insert(r);

			_grid.ClearSelects();
			if (r >= _grid.RowCount)
				r  = _grid.RowCount - 1;
			_grid.EnsureDisplayedRow(r);


			int invalid = YataGrid.INVALID_GRID
						| YataGrid.INVALID_FROZ
						| YataGrid.INVALID_ROWS;
			if (_grid.Propanel != null && _grid.Propanel.Visible)
				invalid |= YataGrid.INVALID_PROP;

			_grid.Invalidator(invalid);
		}

		/// <summary>
		/// Overwrites a row in accord with
		/// <c><see cref="Undo()">Undo()</see></c> or
		/// <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		void Overwrite()
		{
			Row row = _it.r;
			int r = row._id;

			_grid.Rows[r] = row.Clone() as Row;
			_grid.Calibrate(r);

			_grid.ClearSelects(false, true);
			_grid.Rows[r].selected = true;
			_grid.EnsureDisplayedRow(r);


			_grid.Invalidator(YataGrid.INVALID_GRID
							| YataGrid.INVALID_ROWS);
		}

		/// <summary>
		/// Inserts an array of rows in accord with
		/// <c><see cref="Undo()">Undo()</see></c> or
		/// <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		void InsertArray()
		{
			_grid._f.Obfuscate();
			DrawingControl.SuspendDrawing(_grid);


			int cols = _it.array[0].Length;
			var fields = new string[cols];

			Row row;
			for (int a = 0; a != _it.array.Length; ++a)
			{
				row = _it.array[a];
				for (int c = 0; c != cols; ++c)
					fields[c] = String.Copy(row[c].text);

				_grid.Insert(row._id, fields, false, row._brush);

				for (int c = 0; c != row.Length; ++c)
					_grid[row._id, c].loadchanged = row[c].loadchanged;
			}
			_grid.Calibrate(0, _grid.RowCount - 1);


			_grid.ClearSelects(false, true);
			int r = _it.array[0]._id;
			_grid.Rows[r].selected = true;
//			_grid.RangeSelect = _it.array.Length - 1;	// that's problematic ... wrt/ re-Sorted cols
														// and since only 1 row shall ever be selected you can't just select them all either.

			_grid.EnsureDisplayedRow(r); // TODO: EnsureDisplayedRows()
			// NOTE: Does not select the row's cells.


			_grid._f.Obfuscate(false);
			DrawingControl.ResumeDrawing(_grid);
		}

		/// <summary>
		/// Deletes an array of rows in accord with
		/// <c><see cref="Undo()">Undo()</see></c> or
		/// <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		void DeleteArray()
		{
			_grid._f.Obfuscate();
			DrawingControl.SuspendDrawing(_grid);


			for (int a = _it.array.Length - 1; a != -1; --a) // reverse delete.
			{
				_grid.Insert(_it.array[a]._id, null, false);
			}

			_grid.Calibrate();

			_grid.ClearSelects();
			int r = _it.array[0]._id;
			if (r >= _grid.RowCount)
				r  = _grid.RowCount - 1;
			_grid.EnsureDisplayedRow(r);


			_grid._f.Obfuscate(false);
			DrawingControl.ResumeDrawing(_grid);
		}
		#endregion Methods (actions)


/*		#region debug
		internal void PrintRestorables()
		{
			logfile.Log("UNDOABLES");
			foreach (var it in Undoables)
			{
				switch (it.RestoreType)
				{
					case UrType.rt_Cell:
						logfile.Log(". type Cell [" + it.cell.y + "," + it.cell.x + "] \"" + it.cell.text + "\"");
						logfile.Log(". . pretext= " + it.pretext);
						logfile.Log(". . postext= " + it.postext);
						break;
					case UrType.rt_Insert:
						logfile.Log(". type Insert " + it.rInsert._id);
						break;
					case UrType.rt_Delete:
						logfile.Log(". type Delete " + it.rDelete);
						break;
				}
			}

			logfile.Log("REDOABLES");
			foreach (var it in Redoables)
			{
				switch (it.RestoreType)
				{
					case UrType.rt_Cell:
						logfile.Log(". type Cell [" + it.cell.y + "," + it.cell.x + "] \"" + it.cell.text + "\"");
						logfile.Log(". . pretext= " + it.pretext);
						logfile.Log(". . postext= " + it.postext);
						break;
					case UrType.rt_Insert:
						logfile.Log(". type Insert " + it.rInsert._id);
						break;
					case UrType.rt_Delete:
						logfile.Log(". type Delete " + it.rDelete);
						break;
				}
			}

			logfile.Log("\n");
		}
		#endregion debug */
	}
}
