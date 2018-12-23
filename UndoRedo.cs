using System;
using System.Collections;


namespace yata
{
	/// <summary>
	/// Restorables contain values for the action that's about to be performed
	/// on Undo/Redo.
	/// @note Classvar '_it' is the state that is being undone/redone; it is
	/// used by the action that's invoked.
	/// @note Classvar 'State' is the current state shown in the table; it is
	/// assigned to '_it' for either an Undo or Redo action.
	/// </summary>
	struct Restorable
	{
		internal UndoRedo.UrType RestoreType;
		internal bool Changed;

		internal Cell cell;
		internal string pretext;
		internal string postext;

		internal Row rInsert;
		internal int rDelete;

		internal bool flipped;
	}


	class UndoRedo
	{
		internal enum UrType
		{
			rt_Cell,		// 0
			rt_RowInsert,	// 1
			rt_RowDelete,	// 2
		}


		#region Fields
		readonly YataGrid _grid;

		readonly Stack Undoables = new Stack(); // states that can be Undone to
		readonly Stack Redoables = new Stack(); // states that can be Redone to

		/// <summary>
		/// '_it' is the state that the user has most recently invoked by either
		/// Undo() or Redo().
		/// </summary>
		Restorable _it;
		#endregion Fields


		#region Properties
		/// <summary>
		/// State of a Cell as user sees it. It will not be pushed onto either
		/// stack unless user invokes Undo() or Redo().
		/// </summary>
		internal Restorable State
		{ private get; set; }

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
		/// Instantiates a restorable cell when user changes state.
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		internal Restorable createCell(ICloneable cell)
		{
			Restorable it;
			it.RestoreType = UrType.rt_Cell;
			it.Changed = _grid.Changed;

			it.cell = cell.Clone() as Cell;
			it.pretext = it.cell.text;
			it.postext = String.Empty;

			it.rInsert = null;
			it.rDelete = -1;
			it.flipped = false;

			return it;
		}

		/// <summary>
		/// Instantiates a restorable row when user changes state.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		internal Restorable createRow(ICloneable row, UrType type)
		{
			Restorable it;
			it.RestoreType = type;
			it.Changed = _grid.Changed;

			it.cell    = null;
			it.pretext = null;
			it.postext = null;

			it.rInsert = row.Clone() as Row;
			it.rDelete = it.rInsert._id;
			it.flipped = false;

			return it;
		}


		/// <summary>
		/// User's current state is pushed into Undoables on any regular state-
		/// change. The stack of Redoables is cleared.
		/// </summary>
		/// <param name="it"></param>
		internal void Push(object it)
		{
			Undoables.Push(it);
			Redoables.Clear();
		}


		/// <summary>
		/// Undo's a cell-text change or a row-insert/delete.
		/// </summary>
		internal void Undo()
		{
			logfile.Log("Undo()");

			_it = (Restorable)Undoables.Pop();

			_grid.Changed &= _it.Changed;

			switch (_it.RestoreType)
			{
				case UrType.rt_Cell:
					_it.cell.text = _it.pretext;
					RestoreCell();
					break;

				case UrType.rt_RowInsert:
					InsertRow();
					break;

				case UrType.rt_RowDelete:
					DeleteRow();
					break;
			}


			Restorable state = State;

			// This next section prepares user's current 'State' to be a valid
			// Redoable - only when Undo() is first invoked after a regular
			// state-change.
			if (Redoables.Count == 0)
			{
				switch (state.RestoreType)
				{
					case UrType.rt_Cell:
						if (!state.flipped)
						{
							state.postext = state.pretext;
							state.pretext = _grid[state.cell.y, state.cell.x].text;
							state.flipped = true;
						}
						else
							state.postext = state.cell.text;

						break;

					case UrType.rt_RowInsert:
						if (!state.flipped)
						{
							state.RestoreType = UrType.rt_RowDelete;
							state.flipped = true;
						}
						break;

					case UrType.rt_RowDelete:
						if (!state.flipped)
						{
							state.RestoreType = UrType.rt_RowInsert;
							state.flipped = true;
						}
						break;
				}
			}


			bool changed = _it.Changed;

			_it.Changed = State.Changed;
			Redoables.Push(_it);

			_it.Changed = changed;


			State = _it;
			PrintRestorables();
		}

		/// <summary>
		/// Redo's a cell-text change or a row-insert/delete.
		/// </summary>
		public void Redo()
		{
			logfile.Log("Redo()");

			_it = (Restorable)Redoables.Pop();

			_grid.Changed |= _it.Changed;

			switch (_it.RestoreType)
			{
				case UrType.rt_Cell:
					_it.cell.text = _it.postext;
					RestoreCell();
					break;

				case UrType.rt_RowInsert:
					DeleteRow();
					break;

				case UrType.rt_RowDelete:
					InsertRow();
					break;
			}


			bool changed = _it.Changed;

			_it.Changed = State.Changed;
			Undoables.Push(_it);

			_it.Changed = changed;


			State = _it;
			PrintRestorables();
		}


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
				fields[i] = _it.rInsert[i].text;

			_grid.Insert(_it.rInsert._id, fields);

			_grid.Refresh();
			_grid._proHori = 0;
		}

		/// <summary>
		/// Deletes a row in accord with Undo() or Redo().
		/// </summary>
		void DeleteRow()
		{
			_grid.SetProHori();

			_grid.Insert(_it.rDelete, null);

			_grid.Refresh();
			_grid._proHori = 0;
		}
		#endregion Methods


		#region debug
		internal void PrintRestorables()
		{
			Restorable it_;

			logfile.Log("UNDOABLES");
			foreach (var it in Undoables)
			{
				it_ = (Restorable)it;
				switch (it_.RestoreType)
				{
					case UrType.rt_Cell:
						logfile.Log(". type Cell [" + it_.cell.y + "," + it_.cell.x + "] \"" + it_.cell.text + "\"");
						logfile.Log(". . pretext= " + it_.pretext);
						logfile.Log(". . postext= " + it_.postext);
						break;
					case UrType.rt_RowInsert:
						logfile.Log(". type Insert");
						logfile.Log(". . insertId= " + it_.rInsert._id);
						break;
					case UrType.rt_RowDelete:
						logfile.Log(". type Delete");
						logfile.Log(". . deleteId= " + it_.rDelete);
						break;
				}
			}

			logfile.Log("REDOABLES");
			foreach (var it in Redoables)
			{
				it_ = (Restorable)it;
				switch (it_.RestoreType)
				{
					case UrType.rt_Cell:
						logfile.Log(". type Cell [" + it_.cell.y + "," + it_.cell.x + "] \"" + it_.cell.text + "\"");
						logfile.Log(". . pretext= " + it_.pretext);
						logfile.Log(". . postext= " + it_.postext);
						break;
					case UrType.rt_RowInsert:
						logfile.Log(". type Insert");
						logfile.Log(". . insertId= " + it_.rInsert._id);
						break;
					case UrType.rt_RowDelete:
						logfile.Log(". type Delete");
						logfile.Log(". . deleteId= " + it_.rDelete);
						break;
				}
			}

			logfile.Log("STATE");
			switch (State.RestoreType)
			{
				case UrType.rt_Cell:
					logfile.Log(". type Cell [" + State.cell.y + "," + State.cell.x + "] \"" + State.cell.text + "\"");
					logfile.Log(". . pretext= " + State.pretext);
					logfile.Log(". . postext= " + State.postext);
					break;
				case UrType.rt_RowInsert:
					logfile.Log(". type Insert");
					logfile.Log(". . insertId= " + State.rInsert._id);
					break;
				case UrType.rt_RowDelete:
					logfile.Log(". type Delete");
					logfile.Log(". . deleteId= " + State.rDelete);
					break;
			}

			logfile.Log("\n");
		}
		#endregion debug
	}
}
