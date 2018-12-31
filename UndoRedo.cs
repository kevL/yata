using System;
using System.Collections.Generic;


namespace yata
{
	/// <summary>
	/// Restorables contain values for the action that's about to be performed
	/// on Undo/Redo. Each Restorable contains an Undo-state and a Redo-state;
	/// they toggle back and forth depending on the action, Undo() or Redo().
	/// @note Classvar '_it' is the state that is being undone/redone; it is
	/// used by the action that's invoked.
	/// </summary>
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


		#region Fields
		readonly YataGrid _grid;

		readonly Stack<Restorable> Undoables = new Stack<Restorable>(); // states that can be Undone to
		readonly Stack<Restorable> Redoables = new Stack<Restorable>(); // states that can be Redone to

		/// <summary>
		/// '_it' is the state that the user has most recently invoked by either
		/// Undo() or Redo().
		/// </summary>
		Restorable _it;
		#endregion Fields


		#region Properties
		internal bool CanUndo
		{
			get { return (Undoables.Count != 0); }
		}

		internal bool CanRedo
		{
			get { return (Redoables.Count != 0); }
		}
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal UndoRedo(YataGrid grid)
		{
			_grid = grid;
		}
		#endregion cTor


		#region Methods (static)
		/// <summary>
		/// Instantiates a restorable cell when user changes state.
		/// </summary>
		/// <param name="cell">a table Cell object</param>
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
		/// Instantiates a restorable row when user changes state - ie inserts
		/// or deletes a row.
		/// </summary>
		/// <param name="row">a table Row object</param>
		/// <param name="type">'rt_RowDelete' for a row to be deleted or
		/// 'rt_RowInsert' for a row to be inserted</param>
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
		/// Instantiates a restorable row when user changes state - ie changes
		/// an existing row.
		/// </summary>
		/// <param name="row">a table Row object</param>
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
		/// Instantiates a restorable list of rows when user changes state.
		/// </summary>
		/// <param name="rows">quantity of rows in the Row-array</param>
		/// <param name="type">'rt_ArrayDelete' for row(s) to be deleted or
		/// 'rt_ArrayInsert' for row(s) to be inserted</param>
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
		/// Clears Undoables and Redoables stacks.
		/// </summary>
		internal void Clear()
		{
			Undoables.Clear();
			Redoables.Clear();
		}


		/// <summary>
		/// User's current state is pushed into Undoables on any regular state-
		/// change. The stack of Redoables is cleared.
		/// </summary>
		/// <param name="it">a Restorable object to push onto the top of 'Undoables'</param>
		internal void Push(Restorable it)
		{
			if (_grid.UrEnabled) // NOTE: This could/should be done before 'it' is instantiated by the caller.
			{
				Undoables.Push(it);
				Redoables.Clear();

				_grid._f.EnableUndo(true);
			}
			// TODO: else invoke UndoRedoWarningBox ... w/ checkbox "I promise to remember when columns are sorted."
		}


		/// <summary>
		/// Re-determines the 'isSaved' var of the Restorables when user saves
		/// the 2da-file.
		/// @note It would probably be easier to contain Restorables in Lists
		/// instead of Stacks.
		/// </summary>
		internal void ResetSaved()
		{
			if (Undoables.Count != 0)
			{
				var u = Undoables.ToArray();

				u[0].isSaved = UndoRedo.IsSavedType.is_Redo;

				for (int i = 1; i != u.Length; ++i)
					u[i].isSaved = UndoRedo.IsSavedType.is_None;

				Undoables.Clear();
				for (int i = u.Length - 1; i != -1; --i)
					Undoables.Push(u[i]);
			}

			if (Redoables.Count != 0)
			{
				var r = Redoables.ToArray();

				r[0].isSaved = UndoRedo.IsSavedType.is_Undo;

				for (int i = 1; i != r.Length; ++i)
					r[i].isSaved = UndoRedo.IsSavedType.is_None;

				Redoables.Clear();
				for (int i = r.Length - 1; i != -1; --i)
					Redoables.Push(r[i]);
			}
		}


		/// <summary>
		/// Undo's a cell-text change or a row-insert/delete/overwrite.
		/// </summary>
		internal void Undo()
		{
			if (_grid.UrEnabled) // safety.
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
		}

		/// <summary>
		/// Redo's a cell-text change or a row-insert/delete/overwrite.
		/// </summary>
		public void Redo()
		{
			if (_grid.UrEnabled) // safety.
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
		}
		#endregion Methods


		#region Methods (actions)
		/// <summary>
		/// Changes cell-text in accord with Undo() or Redo().
		/// </summary>
		void RestoreCell()
		{
			var cell = _it.cell.Clone() as Cell;

			int
				c = _it.cell.x,
				r = _it.cell.y;

			_grid[r,c] = cell;

			_grid.colRewidth(c, r);
			_grid.UpdateFrozenControls(c);

			_grid.ClearSelects();
			cell.selected = true;
			_grid.EnsureDisplayed(cell);

			_grid._f.Refresh();
		}

		/// <summary>
		/// Inserts a row in accord with Undo() or Redo().
		/// </summary>
		void InsertRow()
		{
			_grid.SetProHori();

			Row row = _it.r;

			var fields = new string[row.CellCount];
			for (int i = 0; i != row.CellCount; ++i)
				fields[i] = String.Copy(row[i].text);

			int r = row._id;
			_grid.Insert(r, fields, true, row._brush);

			_grid.ClearSelects();
			_grid.Rows[r].selected = true;
			_grid.EnsureDisplayedRow(r);

			_grid.Refresh();
			_grid._proHori = 0;
		}

		/// <summary>
		/// Deletes a row in accord with Undo() or Redo().
		/// </summary>
		void DeleteRow()
		{
			_grid.SetProHori();

			int r = _it.r._id;

			_grid.Insert(r, null);

			_grid.ClearSelects();
			if (r >= _grid.RowCount)
				r  = _grid.RowCount - 1;
			_grid.EnsureDisplayedRow(r);

			_grid.Refresh();
			_grid._proHori = 0;
		}

		/// <summary>
		/// Overwrites a row in accord with Undo() or Redo().
		/// </summary>
		void Overwrite()
		{
			Row row = _it.r;
			int r = row._id;

			_grid.Rows[r] = row.Clone() as Row;
			_grid.Calibrate(r);

			_grid.ClearSelects();
			_grid.EnsureDisplayedRow(r);

			_grid.Refresh();
		}

		/// <summary>
		/// Inserts an array of rows in accord with Undo() or Redo().
		/// </summary>
		void InsertArray()
		{
			_grid._f.ShowColorPanel();
			DrawingControl.SuspendDrawing(_grid);


			int cols = _it.array[0].CellCount;
			var fields = new string[cols];

			Row row;

			int r = _it.array[0]._id;
			for (int i = 0; i != _it.array.Length; ++i, ++r)
			{
				row = _it.array[i];
				for (int j = 0; j != cols; ++j)
					fields[j] = String.Copy(row[j].text);

				_grid.Insert(r, fields, false, row._brush);
			}

			r = _it.array[0]._id;
			_grid.Calibrate(r, _it.array.Length - 1);

			_grid.ClearSelects();
			_grid.Rows[r].selected = true;
			_grid.RangeSelect = _it.array.Length - 1;
			_grid.EnsureDisplayedRow(r); // TODO: EnsureDisplayedRows()
			// NOTE: Does not select cells.


			_grid._f.ShowColorPanel(false);
			DrawingControl.ResumeDrawing(_grid);
		}

		/// <summary>
		/// Deletes an array of rows in accord with Undo() or Redo().
		/// </summary>
		void DeleteArray()
		{
			_grid._f.ShowColorPanel();
			DrawingControl.SuspendDrawing(_grid);


			int rFirst = _it.array[0]._id;
			int rLast = rFirst + _it.array.Length - 1;

			while (rLast >= rFirst) // reverse delete.
			{
				_grid.Insert(rLast--, null, false);
			}

			_grid.Calibrate();

			_grid.ClearSelects();
			if (rFirst >= _grid.RowCount)
				rFirst  = _grid.RowCount - 1;
			_grid.EnsureDisplayedRow(rFirst);


			_grid._f.ShowColorPanel(false);
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
