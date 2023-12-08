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

		internal Cell cell;						// a Cell that was user-changed
		internal string pretext;				// its text before it was changed
		internal string postext;				// its text after it was changed
		internal uint chain;					// a chain-id for multi-celled operations

		internal Row r;							// a Row that was user-changed
		internal Row rPre;						// the row as it was before the change
		internal Row rPos;						// the row after it was changed

		internal Row[] array;					// an array of Rows that were user-changed

		internal UndoRedo.IsSavedType isSaved;	// tracks if state is the saved state
	}


	/// <summary>
	/// Mind-bending backflips done by gorillas dressed in clown outfits.
	/// </summary>
	sealed class UndoRedo
	{
		#region Enums
		internal enum UrType
		{
			rt_Cell,		// 0 cell action

			rt_Insert,		// 1 row actions ->
			rt_Delete,		// 2
			rt_Overwrite,	// 3

			rt_ArrayInsert,	// 4 row array actions ->
			rt_ArrayDelete	// 5
		}

		internal enum IsSavedType
		{
			is_None,	// 0 - neither Undo-state nor Redo-state is 2da-saved.
			is_Undo,	// 1 - the Undo-state is the saved state.
			is_Redo		// 2 - the Redo-state is the saved state.
		}
		#endregion Enums


		#region Fields
		readonly YataGrid _grid;
		readonly Yata _f;

		readonly Stack<Restorable> Undoables = new Stack<Restorable>(); // states that can be Undone to
		readonly Stack<Restorable> Redoables = new Stack<Restorable>(); // states that can be Redone to

		/// <summary>
		/// <c>_it</c> is the state that the user has most recently invoked by
		/// either <c><see cref="Undo()">Undo()</see></c> or
		/// <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		Restorable _it;

		/// <summary>
		/// When user does consecutive <c><see cref="Cell"/></c> changes that
		/// involve 2+ <c>Cells</c> the Undo/Redo routines need to distinguish
		/// one operation from the next. A <c>UInt32</c> will be successively
		/// incremented and applied to each <c><see cref="Restorable"/></c> of
		/// <c><see cref="UrType.rt_Cell">UrType.rt_Cell</see></c> to define its
		/// chaingroup. The default of <c>0</c> shall be reserved for
		/// <c>Restorables</c> that are *not* chained together.
		/// </summary>
		uint _chain;

		/// <summary>
		/// When <c><see cref="Restorable">Restorables</see></c> of
		/// <c><see cref="UrType.rt_Cell">UrType.rt_Cell</see></c> are chained
		/// the <c>_cols</c> variable caches a <c>List</c> of col-ids that need
		/// to be layed out when the Undo/Redo routine finishes.
		/// </summary>
		readonly List<int> _cols = new List<int>();

		/// <summary>
		/// When <c><see cref="Restorable">Restorables</see></c> of
		/// <c><see cref="UrType.rt_Cell">UrType.rt_Cell</see></c> are chained
		/// the <c>_cells</c> variable caches a <c>List</c> of
		/// <c><see cref="Cell">Cells</see></c> that shall be selected when the
		/// Undo/Redo routine finishes.
		/// </summary>
		readonly List<Cell> _cells = new List<Cell>();
		#endregion Fields


		#region Properties
		/// <summary>
		/// Checks if an Undo action is allowed.
		/// </summary>
		/// <returns><c>true</c> if <c><see cref="Undo()">Undo()</see></c> is
		/// allowed</returns>
		internal bool CanUndo
		{
			get { return Undoables.Count != 0; }
		}

		/// <summary>
		/// Checks if a Redo action is allowed.
		/// </summary>
		/// <returns><c>true</c> if <c><see cref="Redo()">Redo()</see></c> is
		/// allowed</returns>
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
			_f = (_grid = grid)._f;
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
			//logfile.Log("createCell()");

			Restorable it;
			it.RestoreType = UrType.rt_Cell;

			it.cell    = cell.Clone() as Cell;	// is used by current action ->
			it.pretext = it.cell.text;			// used by Undo
			it.postext = String.Empty;			// used by Redo
			it.chain   = (uint)0;				// used by both Undo and Redo

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
			//logfile.Log("createRow() type= " + type);

			Restorable it;
			it.RestoreType = type;

			it.cell    = null;			// not used
			it.pretext =				// not used
			it.postext = null;			// not used
			it.chain   = (uint)0;		// not used

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
			//logfile.Log("createRow()");

			Restorable it;
			it.RestoreType = UrType.rt_Overwrite;

			it.cell    = null;				// not used
			it.pretext =					// not used
			it.postext = null;				// not used
			it.chain   = (uint)0;			// not used

			it.r    = null;					// is used by current action ->
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
			//logfile.Log("createArray() type= " + type);

			Restorable it;
			it.RestoreType = type;

			it.cell    = null;			// not used
			it.pretext =				// not used
			it.postext = null;			// not used
			it.chain   = (uint)0;		// not used

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

			_f.EnableUndo(false);
			_f.EnableRedo(false);
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

			_f.EnableUndo(true);
			_f.EnableRedo(false);
		}


		/// <summary>
		/// Undo's a cell-text change or a row-insert/delete/overwrite or a
		/// row-array insert/delete.
		/// </summary>
		/// <returns><c>true</c> if not bypassed due to diffed
		/// <c><see cref="_grid"/></c></returns>
		internal bool Undo()
		{
			//logfile.Log("Undo() " + Undoables.Peek().RestoreType + " diffed= " + isDiffedTable());

			switch (Undoables.Peek().RestoreType)
			{
				case UrType.rt_Insert:
				case UrType.rt_Delete:
				case UrType.rt_ArrayInsert:
				case UrType.rt_ArrayDelete:
					if (isDiffedTable())
					{
						_f.error_TableDiffed(); // do not allow rows to be created/deleted
						return false;
					}
					break;
			}


			_it = Undoables.Pop();

			bool finish = _it.chain == (uint)0
					   || Undoables.Count == 0
					   || Undoables.Peek().chain != _it.chain;

			switch (_it.RestoreType)
			{
				case UrType.rt_Cell:
					_it.cell.text = _it.pretext;
					RestoreCell(finish);
					break;

				case UrType.rt_Insert:
					InsertRow(finish);
					_it.RestoreType = UrType.rt_Delete;
					break;

				case UrType.rt_Delete:
					DeleteRow(finish);
					_it.RestoreType = UrType.rt_Insert;
					break;

				case UrType.rt_Overwrite:
					_it.r = _it.rPre;
					OverwriteRow();
					break;

				case UrType.rt_ArrayInsert:
					InsertArray(finish);
					_it.RestoreType = UrType.rt_ArrayDelete;
					break;

				case UrType.rt_ArrayDelete:
					DeleteArray(finish);
					_it.RestoreType = UrType.rt_ArrayInsert;
					break;
			}

			Redoables.Push(_it);

			if (finish)
			{
				_grid.Changed = (_it.isSaved != IsSavedType.is_Undo);
				_grid.SyncSelect();
			}
			else
				Undo(); // recurse funct.

			return true;
		}

		/// <summary>
		/// Redo's a cell-text change or a row-insert/delete/overwrite or a
		/// row-array insert/delete.
		/// </summary>
		/// <returns><c>true</c> if not bypassed due to diffed
		/// <c><see cref="_grid"/></c></returns>
		internal bool Redo()
		{
			//logfile.Log("Redo() " + Redoables.Peek().RestoreType + " diffed= " + isDiffedTable());

			switch (Redoables.Peek().RestoreType)
			{
				case UrType.rt_Insert:
				case UrType.rt_Delete:
				case UrType.rt_ArrayInsert:
				case UrType.rt_ArrayDelete:
					if (isDiffedTable())
					{
						_f.error_TableDiffed(); // do not allow rows to be created/deleted
						return false;
					}
					break;
			}


			_it = Redoables.Pop();

			bool finish = _it.chain == (uint)0
					   || Redoables.Count == 0
					   || Redoables.Peek().chain != _it.chain;

			switch (_it.RestoreType)
			{
				case UrType.rt_Cell:
					_it.cell.text = _it.postext;
					RestoreCell(finish);
					break;

				case UrType.rt_Insert:
					InsertRow(finish);
					_it.RestoreType = UrType.rt_Delete;
					break;

				case UrType.rt_Delete:
					DeleteRow(finish);
					_it.RestoreType = UrType.rt_Insert;
					break;

				case UrType.rt_Overwrite:
					_it.r = _it.rPos;
					OverwriteRow();
					break;

				case UrType.rt_ArrayInsert:
					InsertArray(finish);
					_it.RestoreType = UrType.rt_ArrayDelete;
					break;

				case UrType.rt_ArrayDelete:
					DeleteArray(finish);
					_it.RestoreType = UrType.rt_ArrayInsert;
					break;
			}

			Undoables.Push(_it);

			if (finish)
			{
				_grid.Changed = (_it.isSaved != IsSavedType.is_Redo);
				_grid.SyncSelect();
			}
			else
				Redo(); // recurse funct.

			return true;
		}
		#endregion Methods


		#region Methods (actions)
		/// <summary>
		/// Changes cell-text in accord with
		/// <c><see cref="Undo()">Undo()</see></c> or
		/// <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		/// <param name="finish"><c>true</c> to finish chained
		/// <c><see cref="Cell"/></c> actions</param>
		void RestoreCell(bool finish)
		{
			//logfile.Log("UndoRedo.RestoreCell()");

			var cell = _it.cell.Clone() as Cell;

			int c = cell.x;
			_grid[cell.y, c] = cell;

			_cells.Add(cell);

			if (!_cols.Contains(c))
				 _cols.Add(c);

			if (finish)
			{
				//logfile.Log(". finish");

				_grid.ClearSelects(true);
				foreach (var sel in _cells)
					sel.selected = true;
				_cells.Clear();

				if (!isDiffedTable())
				{
					foreach (var col in _cols)
					{
						_grid.Colwidth(col, 0, _grid.RowCount - 1);
						_grid.MetricFrozenControls(col);
					}
				}
				_cols.Clear();

				_grid.EnsureDisplayed(cell);

				_f.EnableCelleditOperations();
				_f.EnableGotoReplaced(_grid.anyReplaced());
				_f.EnableGotoLoadchanged(_grid.anyLoadchanged());

				// TODO: technically this could involve re-ordering rowids

				Invalidate();
			}
		}

		/// <summary>
		/// Inserts a <c><see cref="Row"/></c> in accord with
		/// <c><see cref="Undo()">Undo()</see></c> or
		/// <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		void InsertRow(bool finish)
		{
			//logfile.Log("UndoRedo.InsertRow()");

			Row row = _it.r;

			var fields = new string[row.Length];
			for (int c = 0; c != row.Length; ++c)
				fields[c] = String.Copy(row[c].text);

			int r = row._id;
			_grid.Insert(r, fields, row._brush);

			YataGrid._init = true;
			Cell cell;
			for (int c = 0; c != row.Length; ++c)
			{
				cell = _grid[r,c];
				cell.loadchanged = row[c].loadchanged;
				cell.replaced    = row[c].replaced;
			}
			YataGrid._init = false;

			if (finish)
			{
				//logfile.Log(". finish");

				_f.EnableGotoReplaced(_grid.anyReplaced());
				_f.EnableGotoLoadchanged(_grid.anyLoadchanged());

				_grid.ClearSelects(false, true);
				_grid.Rows[r].selected = true;
				_grid.EnsureDisplayedRow(r);

				if (Options._autorder && Yata.order() != 0)
					_f.layout(true);

				Invalidate();
			}
		}

		/// <summary>
		/// Deletes a <c><see cref="Row"/></c> in accord with
		/// <c><see cref="Undo()">Undo()</see></c> or
		/// <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		void DeleteRow(bool finish)
		{
			//logfile.Log("UndoRedo.DeleteRow()");

			int r = _it.r._id;

			_grid.Delete(r, false, true);

			if (finish)
			{
				//logfile.Log(". finish");

				_grid.ClearSelects(false, true);

				//logfile.Log(". _grid.RowCount= " + _grid.RowCount);

				if (_grid.RowCount != 0)
					_grid.EnsureDisplayedRow(Math.Min(r, _grid.RowCount - 1));

				_f.EnableRoweditOperations();
				_f.EnableGotoReplaced(_grid.anyReplaced());
				_f.EnableGotoLoadchanged(_grid.anyLoadchanged());

				if (Options._autorder && Yata.order() != 0)
					_f.layout(true);

				Invalidate();
			}
		}

		/// <summary>
		/// Overwrites a <c><see cref="Row"/></c> in accord with
		/// <c><see cref="Undo()">Undo()</see></c> or
		/// <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		void OverwriteRow()
		{
			//logfile.Log("UndoRedo.OverwriteRow()");

			Row row = _it.r;
			int r = row._id;

			_grid.Rows[r] = row.Clone() as Row;

			for (int c = 0; c != _grid.ColCount; ++c)
			{
				_grid.doTextwidth(_grid[r,c]);
			}

			if (!isDiffedTable()) _grid.Calibrate(r);

			_grid.ClearSelects(false, true);
			_grid.Rows[r].selected = true;
			_grid.EnsureDisplayedRow(r);

			_f.EnableGotoReplaced(_grid.anyReplaced());
			_f.EnableGotoLoadchanged(_grid.anyLoadchanged());

			if (Options._autorder && Yata.order() != 0)
				_f.layout(true);

			Invalidate();
		}

		/// <summary>
		/// Invalidates a bunch of <c>Controls</c>.
		/// </summary>
		void Invalidate()
		{
			int invalid = YataGrid.INVALID_GRID
						| YataGrid.INVALID_FROZ
						| YataGrid.INVALID_ROWS
						| YataGrid.INVALID_COLS;
			if (_grid.Propanel != null && _grid.Propanel.Visible)
				invalid |= YataGrid.INVALID_PROP;

			_grid.Invalidator(invalid);
		}


		/// <summary>
		/// Inserts an array of <c><see cref="Row">Rows</see></c> in accord with
		/// <c><see cref="Undo()">Undo()</see></c> or
		/// <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		/// <param name="finish"><c>true</c> to finish any chained actions</param>
		void InsertArray(bool finish)
		{
			//logfile.Log("UndoRedo.InsertArray()");

			_f.Obfuscate();
			DrawRegulator.SuspendDrawing(_grid);


			int cols = _it.array[0].Length;
			var fields = new string[cols];

			YataGrid._init = true;
			Row row; Cell cell;
			for (int a = 0; a != _it.array.Length; ++a)
			{
				row = _it.array[a];
				for (int c = 0; c != cols; ++c)
					fields[c] = String.Copy(row[c].text);

				_grid.Insert(row._id, fields, row._brush, true);

				for (int c = 0; c != row.Length; ++c)
				{
					cell = _grid[row._id, c];
					cell.loadchanged = row[c].loadchanged;
					cell.replaced    = row[c].replaced;
				}
			}

			if (finish)
			{
				//logfile.Log(". finish");

				// Calibrate() needs to fire to layout/draw the table
				_grid.Calibrate(0, _grid.RowCount - 1); // that sets 'YataGrid._init' false <-

				_f.EnableGotoReplaced(_grid.anyReplaced());
				_f.EnableGotoLoadchanged(_grid.anyLoadchanged());

				_grid.ClearSelects(false, true);
				int r = _it.array[0]._id;
				_grid.Rows[r].selected = true;
//				_grid.RangeSelect = _it.array.Length - 1;	// that's problematic ... wrt/ re-Sorted cols
															// and since only 1 row shall ever be selected you can't just select them all either.
				_grid.EnsureDisplayedRow(r);				// TODO: EnsureDisplayedRows()

				if (Options._autorder && Yata.order() != 0)
					_f.layout(true);
			}


			DrawRegulator.ResumeDrawing(_grid);
			_f.Obfuscate(false);
		}

		/// <summary>
		/// Deletes an array of <c><see cref="Row">Rows</see></c> in accord with
		/// <c><see cref="Undo()">Undo()</see></c> or
		/// <c><see cref="Redo()">Redo()</see></c>.
		/// </summary>
		/// <param name="finish"><c>true</c> to finish any chained actions</param>
		void DeleteArray(bool finish)
		{
			//logfile.Log("UndoRedo.DeleteArray()");

			_f.Obfuscate();
			DrawRegulator.SuspendDrawing(_grid);


			for (int a = _it.array.Length - 1; a != -1; --a) // reverse delete.
			{
				_grid.Delete(_it.array[a]._id, true, true);
			}

			if (finish)
			{
				//logfile.Log(". finish");

				// Calibrate() needs to fire to layout/draw the table
				_grid.Calibrate();

				_grid.ClearSelects(false, true);

				//logfile.Log(". _grid.RowCount= " + _grid.RowCount);

				if (_grid.RowCount != 0)
					_grid.EnsureDisplayedRow(Math.Min(_it.array[0]._id, _grid.RowCount - 1));

				_f.EnableRoweditOperations();
				_f.EnableGotoReplaced(_grid.anyReplaced());
				_f.EnableGotoLoadchanged(_grid.anyLoadchanged());

				if (Options._autorder && Yata.order() != 0)
					_f.layout(true);
			}


			DrawRegulator.ResumeDrawing(_grid);
			_f.Obfuscate(false);
		}


		/// <summary>
		/// Checks if this <c>UndoRedo</c> belongs to an actively diffed
		/// <c><see cref="YataGrid"/></c>.
		/// </summary>
		/// <returns><c>true</c> if this <c>UndoRedo</c> belongs to one of two
		/// currently diffed tables</returns>
		/// <remarks>See also
		/// <list type="bullet">
		/// <item><c><see cref="Yata"/>.isTableDiffed()</c></item>
		/// <item><c><see cref="YataGrid"/>.isGridDiffed()</c></item>
		/// </list></remarks>
		bool isDiffedTable()
		{
			return _f._diff1 != null && _f._diff2 != null
				&& (_grid == _f._diff1 || _grid == _f._diff2);
		}
		#endregion Methods (actions)


		#region Methods (reset)
		/// <summary>
		/// Chains <c><see cref="Restorable">Restorables</see></c> of
		/// <c><see cref="UrType.rt_Cell">UrType.rt_Cell</see></c> when 2+
		/// <c><see cref="Cell">Cells</see></c> get changed by a single
		/// operation and need to be Undone or Redone by a single action.
		/// </summary>
		/// <param name="length">the count of <c>Restorables</c> to chain
		/// together and rule in a single action</param>
		/// <remarks>Call this only *once* after each multi-celled operation and
		/// mind that a chain is required only if its <paramref name="length"/>
		/// is greater than <c>1</c>.</remarks>
		internal void SetChained(int length)
		{
			if (_chain == UInt32.MaxValue)
				_chain = (uint)0;

			++_chain;

			Restorable[] u = Undoables.ToArray();

			int i = 0;
			for (; i != length; ++i)
				u[i].chain = _chain;

			Undoables.Clear();
			for (i = u.Length - 1; i != -1; --i)
				Undoables.Push(u[i]);
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
				Restorable[] u = Undoables.ToArray();

				int i = 0;
				if (!allchanged)
					u[i++].isSaved = IsSavedType.is_Redo;

				for (; i != u.Length; ++i)
					u[i].isSaved = IsSavedType.is_None;

				Undoables.Clear();
				for (i = u.Length - 1; i != -1; --i)
					Undoables.Push(u[i]);
			}

			if (Redoables.Count != 0)
			{
				Restorable[] r = Redoables.ToArray();

				int i = 0;
				if (!allchanged)
					r[i++].isSaved = IsSavedType.is_Undo;

				for (; i != r.Length; ++i)
					r[i].isSaved = IsSavedType.is_None;

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
		/// <remarks>It would probably be easier to contain
		/// <c><see cref="Restorable">Restorables</see></c> in <c>Lists</c>
		/// instead of <c>Stacks</c>.</remarks>
		internal void ResetY()
		{
			if (Undoables.Count != 0)
			{
				Restorable[] u = Undoables.ToArray();

				ResetY(ref u);

				Undoables.Clear();
				for (int i = u.Length - 1; i != -1; --i)
					Undoables.Push(u[i]);
			}

			if (Redoables.Count != 0)
			{
				Restorable[] r = Redoables.ToArray();

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
		#endregion Methods (reset)


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
