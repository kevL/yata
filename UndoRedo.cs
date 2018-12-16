using System;
using System.Collections;


namespace yata
{
	struct Restorable
	{
		internal int RestoreType;
		internal bool Changed;

		internal Cell cell;
		internal Row row;
	}


	class UndoRedo
	{
		#region Fields (static)
		internal const int RestoreType_None = 0;
		internal const int RestoreType_Cell = 1;
		internal const int RestoreType_Row  = 2;
		#endregion Fields (static)


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
		internal void Clear()
		{
			Undoables.Clear();
			Redoables.Clear();
		}


		internal Restorable createRestorableCell(ICloneable cell)
		{
			//logfile.Log("\ncreateRestorableCell() u= " + Undoables.Count + " r= " + Redoables.Count);

			Restorable it;
			it.RestoreType = UndoRedo.RestoreType_Cell;
			it.Changed = _grid.Changed;
			it.cell = cell.Clone() as Cell;
			it.row = null;

			//logfile.Log(". it.Changed= " + it.Changed);
			//logfile.Log(it.cell.ToString());

			return it;
		}

		internal Restorable createRestorableRow(ICloneable row)
		{
			Restorable it;
			it.RestoreType = UndoRedo.RestoreType_Row;
			it.Changed = _grid.Changed;
			it.row = row.Clone() as Row;
			it.cell = null;

			return it;
		}


		internal void Push(object it)
		{
			Undoables.Push(it);
			Redoables.Clear();
		}


		internal void Undo()
		{
			//logfile.Log("\nUndo()");

			_it = (Restorable)Undoables.Pop();

			switch (_it.RestoreType)
			{
				case RestoreType_Cell:
					_grid.Changed &= _it.Changed;
					RestoreCell();
					break;

				default:
					break;
			}

			Redoables.Push(State);
			State = _it;
		}

		public void Redo()
		{
			//logfile.Log("\nRedo()");

			_it = (Restorable)Redoables.Pop();

			switch (_it.RestoreType)
			{
				case RestoreType_Cell:
					_grid.Changed |= _it.Changed;
					RestoreCell();
					break;

				default:
					break;
			}

			Undoables.Push(State);
			State = _it;
		}

		void RestoreCell()
		{
			_grid[_it.cell.y, _it.cell.x] = _it.cell;

			_grid.colRewidth(_it.cell.x, _it.cell.y);
			_grid.UpdateFrozenControls(_it.cell.x);

			_grid._f.Refresh();
		}
		#endregion Methods
	}
}
