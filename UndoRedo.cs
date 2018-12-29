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

		internal UndoRedo.IsSavedType isSaved;
	}


	class UndoRedo
	{
		internal enum UrType
		{
			rt_Cell,		// 0 cell action
			rt_Insert,		// 1 row actions ->
			rt_Delete,		// 2
			rt_Overwrite	// 3
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

			it.cell = cell.Clone() as Cell;
			it.pretext = it.cell.text;
			it.postext = String.Empty;

			it.r    =
			it.rPre =
			it.rPos = null;

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

			it.cell = null;
			it.pretext =
			it.postext = null;

			it.r = row.Clone() as Row;

			it.rPre =
			it.rPos = null;

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

			it.cell = null;
			it.pretext =
			it.postext = null;

			it.r = null;

			it.rPre = row.Clone() as Row;
			it.rPos = null;

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
			Undoables.Push(it);
			Redoables.Clear();

			_grid._f.EnableUndo(true);
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
			}

			Redoables.Push(_it);
		}

		/// <summary>
		/// Redo's a cell-text change or a row-insert/delete/overwrite.
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
			}

			Undoables.Push(_it);
		}
		#endregion Methods


		#region Methods (actions)
		/// <summary>
		/// Changes cell-text in accord with Undo() or Redo().
		/// </summary>
		void RestoreCell()
		{
			int
				c = _it.cell.x,
				r = _it.cell.y;

			_grid[r,c] = _it.cell.Clone() as Cell;

			_grid.colRewidth(c, r);
			_grid.UpdateFrozenControls(c);

			_grid._f.Refresh();
		}

		/// <summary>
		/// Inserts a row in accord with Undo() or Redo().
		/// </summary>
		void InsertRow()
		{
			_grid.SetProHori();

			var fields = new string[_grid.ColCount];
			for (int i = 0; i != _grid.ColCount; ++i)
				fields[i] = _it.r[i].text;

			_grid.Insert(_it.r._id, fields, true, _it.r._brush);

			_grid.Refresh();
			_grid._proHori = 0;
		}

		/// <summary>
		/// Deletes a row in accord with Undo() or Redo().
		/// </summary>
		void DeleteRow()
		{
			_grid.SetProHori();

			_grid.Insert(_it.r._id, null);

			_grid.Refresh();
			_grid._proHori = 0;
		}

		/// <summary>
		/// Overwrites a row in accord with Undo() or Redo().
		/// </summary>
		void Overwrite()
		{
			_grid.Rows[_it.r._id] = _it.r.Clone() as Row;

			// TODO: probably wants a colRewidth() and UpdateFrozenControls() here

			_grid.Refresh();
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
