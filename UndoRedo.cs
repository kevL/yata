using System;
using System.Collections;


namespace yata
{
	struct Restorable
	{
		internal int RestoreType;
		internal bool Changed;

		internal Cell cell;

		internal Row rInsert;
		internal int rDelete;
	}


	class UndoRedo
	{
		#region Fields (static)
		internal const int RestoreType_None       = 0;
		internal const int RestoreType_Cell       = 1;
		internal const int RestoreType_RowInsert  = 2;
		internal const int RestoreType_RowDelete  = 3;
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
			Restorable it;
			it.RestoreType = UndoRedo.RestoreType_Cell;
			it.Changed = _grid.Changed;

			it.cell = cell.Clone() as Cell;

			it.rInsert = null;
			it.rDelete = -1;

			return it;
		}

		internal Restorable createRestorableRowInsert(ICloneable row)
		{
			Restorable it;
			it.RestoreType = UndoRedo.RestoreType_RowInsert;
			it.Changed = _grid.Changed;

			it.rInsert = row.Clone() as Row;

			it.cell = null;
			it.rDelete = -1;

			return it;
		}

		internal Restorable createRestorableRowDelete(int r)
		{
			Restorable it;
			it.RestoreType = UndoRedo.RestoreType_RowDelete;
			it.Changed = _grid.Changed;

			it.rDelete = r;

			it.cell = null;
			it.rInsert = null;

			return it;
		}


		internal void Push(object it)
		{
			Undoables.Push(it);
			Redoables.Clear();
		}


		internal void Undo()
		{
			_it = (Restorable)Undoables.Pop();

			_grid.Changed &= _it.Changed;

			switch (_it.RestoreType)
			{
				case RestoreType_Cell:
					RestoreCell();
					break;

				case RestoreType_RowInsert:
					InsertRow();

					break;

				case RestoreType_RowDelete:
					DeleteRow();

					break;

				default:
					break;
			}

			Redoables.Push(State);
			State = _it;
		}

		public void Redo()
		{
			_it = (Restorable)Redoables.Pop();

			_grid.Changed |= _it.Changed;

			switch (_it.RestoreType)
			{
				case RestoreType_Cell:
					RestoreCell();
					break;

				case RestoreType_RowInsert:
					InsertRow();

					break;

				case RestoreType_RowDelete:
					DeleteRow();

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

		void DeleteRow()
		{
			_grid.SetProHori();

			_grid.Insert(_it.rDelete, null);

			_grid.Refresh();
			_grid._proHori = 0;
		}
		#endregion Methods
	}
}
