using System;
using System.Collections;


namespace yata
{
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
			rt_None,		// 0
			rt_Cell,		// 1
//			rt_CellRevert,	// 1
//			rt_CellCurrent,	// 2
			rt_RowInsert,	// 2
			rt_RowDelete,	// 3
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
		internal void Clear()
		{
			Undoables.Clear();
			Redoables.Clear();
		}


		internal Restorable createCell(ICloneable cell)
		{
			Restorable it;
			it.RestoreType = UrType.rt_Cell;
			it.Changed = _grid.Changed;

			it.cell = cell.Clone() as Cell;
			it.pretext = String.Empty;
			it.postext = String.Empty;

			it.rInsert = null;
			it.rDelete = -1;
			it.flipped = false;

			return it;
		}

/*		internal Restorable createCellRevert(ICloneable cell)
		{
			Restorable it;
			it.RestoreType = UrType.rt_CellCurrent;
			it.Changed = _grid.Changed;

			it.cell = cell.Clone() as Cell;
			it.pretext = String.Empty;
			it.postext = String.Empty;

			it.rInsert = null;
			it.rDelete = -1;
			it.flipped = false;

			return it;
		}

		internal Restorable createCellCurrent(ICloneable cell)
		{
			Restorable it;
			it.RestoreType = UrType.rt_CellCurrent;
			it.Changed = _grid.Changed;

			it.cell = cell.Clone() as Cell;
			it.pretext = String.Empty;
			it.postext = String.Empty;

			it.rInsert = null;
			it.rDelete = -1;
			it.flipped = false;

			return it;
		} */

		internal Restorable createRowInsert(ICloneable row)
		{
			Restorable it;
			it.RestoreType = UrType.rt_RowInsert;
			it.Changed = _grid.Changed;

			it.cell = null;
			it.pretext = String.Empty;
			it.postext = String.Empty;

			it.rInsert = row.Clone() as Row;
			it.rDelete = it.rInsert._id;
			it.flipped = false;

			return it;
		}

		internal Restorable createRowDelete(ICloneable row)
		{
			Restorable it;
			it.RestoreType = UrType.rt_RowDelete;
			it.Changed = _grid.Changed;

			it.cell = null;
			it.pretext = String.Empty;
			it.postext = String.Empty;

			it.rInsert = row.Clone() as Row;
			it.rDelete = it.rInsert._id;
			it.flipped = false;

			return it;
		}


		internal void Push(object it)
		{
			Undoables.Push(it);
			Redoables.Clear();
		}


		internal void Undo()
		{
			logfile.Log("Undo()");

			_it = (Restorable)Undoables.Pop();

			_grid.Changed &= _it.Changed;

			switch (_it.RestoreType)
			{
				case UrType.rt_Cell:
					RestoreCell();
					break;

/*				case UrType.rt_CellRevert:
					RestoreCell();
					break;
				case UrType.rt_CellCurrent:
					RestoreCell();
					break; */

				case UrType.rt_RowInsert:
					InsertRow();
					break;

				case UrType.rt_RowDelete:
					DeleteRow();
					break;
			}

			Restorable state = State;
/*			if (Redoables.Count != 0)
			{
				switch (state.RestoreType)
				{
					case UrType.rt_Cell:
						state.pretext = state.cell.text;
						state.cell.text = state.postext;

						break;
//					case UrType.rt_CellRevert:
//						break;
//					case UrType.rt_CellCurrent:
//						break;
				}
			}
			else */
			if (!state.flipped)// && Redoables.Count == 0)
			{
				switch (state.RestoreType)
				{
					case UrType.rt_RowInsert:
						state.RestoreType = UrType.rt_RowDelete;
						state.flipped = true;
						break;

					case UrType.rt_RowDelete:
						state.RestoreType = UrType.rt_RowInsert;
						state.flipped = true;
						break;
				}
			}
			Redoables.Push(state);

			State = _it;
			PrintRestorables();
		}

		public void Redo()
		{
			logfile.Log("Redo()");

			if (State.RestoreType == UrType.rt_Cell)
			{
				Restorable state = State;
				state.pretext = state.cell.text;
				state.cell.text = state.postext;
				State = state;

				Redoables.Push(State);
			}

			_it = (Restorable)Redoables.Pop();

			_grid.Changed |= _it.Changed;

			switch (_it.RestoreType)
			{
				case UrType.rt_Cell:
					RestoreCell();
					break;

/*				case UrType.rt_CellRevert:
					RestoreCell();
					break;
				case UrType.rt_CellCurrent:
					RestoreCell();
					break; */

				case UrType.rt_RowInsert:
					DeleteRow();
					break;

				case UrType.rt_RowDelete:
					InsertRow();
					break;
			}


			if (State.RestoreType == UrType.rt_Cell)
			{
				Restorable state = State;
				state.cell.text = state.postext;
				state.postext = String.Empty;
				State = state;

				Redoables.Push(State);
			}
			Undoables.Push(State);

			State = (Restorable)Redoables.Pop();
//			State = _it;
			PrintRestorables();
		}


		void RestoreCell()
		{
			_grid[_it.cell.y, _it.cell.x] = _it.cell.Clone() as Cell;

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
						logfile.Log(". . " + it_.rInsert._id);
						break;
					case UrType.rt_RowDelete:
						logfile.Log(". type Delete");
						logfile.Log(". . " + it_.rDelete);
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
						logfile.Log(". . " + it_.rInsert._id);
						break;
					case UrType.rt_RowDelete:
						logfile.Log(". type Delete");
						logfile.Log(". . " + it_.rDelete);
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
					logfile.Log(". . " + State.rInsert._id);
					break;
				case UrType.rt_RowDelete:
					logfile.Log(". type Delete");
					logfile.Log(". . " + State.rDelete);
					break;
			}

			logfile.Log("\n");
		}
		#endregion debug
	}
}
