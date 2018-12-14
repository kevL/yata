using System;
using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;


namespace yata
{
	// https://pradeep1210.wordpress.com/2011/04/09/add-undoredo-or-backforward-functionality-to-your-application/

//	public delegate void UndoHappened(object sender, UndoRedoEventArgs e);
//	public delegate void RedoHappened(object sender, UndoRedoEventArgs e);


	struct Restorable
	{
		internal int RestoreType;
		internal bool Changed;

		internal Cell cell;
	}


//	public class UndoRedo<T>
	class UndoRedo
//		:
//			INotifyPropertyChanged
	{
		internal const int RestoreType_None = 0;
		internal const int RestoreType_Cell = 1;
		internal const int RestoreType_Row  = 2;

		readonly YataGrid _grid;


		#region INotifyPropertyChanged requirements
/*		public event PropertyChangedEventHandler PropertyChanged;
		
		public void RaisePropertyChangedEvent(string property)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(property));
		} */
		#endregion INotifyPropertyChanged requirements


		#region Fields
		readonly Stack Undoables = new Stack();
		readonly Stack Redoables = new Stack();

//		public T Current;
//		public object Current;
		public Restorable _it;
		public Restorable _latest;

//		public event UndoHappened OnUndo;
//		public event RedoHappened OnRedo;
		#endregion Fields


		#region Properties
		public bool CanUndo
		{
			get { return (Undoables.Count != 0); }
		}

		public bool CanRedo
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
/*		public void Clear()
		{
			UndoActions.Clear();
			RedoActions.Clear();

//			Current = default(T);
			Current = default(object);

			RaisePropertyChangedEvent("CanUndo");
			RaisePropertyChangedEvent("CanRedo");
		} */


//		public void Add(T it)
/*		public void Add(object it)
		{
//			if (!EqualityComparer<T>.Default.Equals(Current, default(T)))
//				UndoStack.Push((T)Current);
			if (!EqualityComparer<object>.Default.Equals(Current, default(object)))
				UndoActions.Push((object)Current);

			Current = it;

			RaisePropertyChangedEvent("CanUndo");
			RaisePropertyChangedEvent("CanRedo");
		} */

		internal void Add(object it)
		{
			_it = (Restorable)it;
			Undoables.Push(_it);
		}

		internal void setLatest(Restorable it)
		{
			_latest = it;
		}


/*		public void Undo()
		{
//			RedoStack.Push((T)Current);
			RedoActions.Push((object)Current);

//			Current = (T)UndoStack.Pop();
			Current = (object)UndoActions.Pop();

			if (OnUndo != null)
				OnUndo(this, new UndoRedoEventArgs(Current));

			RaisePropertyChangedEvent("CanUndo");
			RaisePropertyChangedEvent("CanRedo");
		} */
		internal void Undo()
		{
			if (Redoables.Count != 0)
				Redoables.Push(_it);
			else
				Redoables.Push(_latest);

			_it = (Restorable)Undoables.Pop();

			switch (_it.RestoreType)
			{
				case RestoreType_Cell:
					RestoreCell();
					break;

				default:
					break;
			}
		}

/*		public void Redo()
		{
//			UndoStack.Push((T)Current);
			UndoActions.Push((object)Current);

//			Current = (T)RedoStack.Pop();
			Current = (object)RedoActions.Pop();
			if (OnRedo != null)
				OnRedo(this, new UndoRedoEventArgs(Current));

			RaisePropertyChangedEvent("CanUndo");
			RaisePropertyChangedEvent("CanRedo");
		} */
		public void Redo()
		{
			Undoables.Push(_it);

			if (Redoables.Count != 0)
				_it = (Restorable)Redoables.Pop();
			else
				_it = _latest;

			switch (_it.RestoreType)
			{
				case RestoreType_Cell:
					RestoreCell();
					break;

				default:
					break;
			}
		}

		void RestoreCell()
		{
			_grid.Changed &= _it.Changed;
			_grid[_it.cell.y, _it.cell.x] = _it.cell;

			_grid.colRewidth(_it.cell.x, _it.cell.y);
			_grid.UpdateFrozenControls(_it.cell.x);

			_grid._f.Refresh();
		}


//		public List<T> UndoItems()
/*		public List<object> UndoItems()
		{
//			var list = new List<T>();
//			foreach (T it in UndoStack)
			var list = new List<object>();
			foreach (object it in UndoActions)
				list.Add(it);

			return list;
		} */
//		public List<T> RedoItems()
/*		public List<object> RedoItems()
		{
//			var list = new List<T>();
//			foreach (T it in RedoStack)
			var list = new List<object>();
			foreach (object it in RedoActions)
				list.Add(it);

			return list;
		} */
		#endregion Methods
	}


	#region EventArgs
/*	public class UndoRedoEventArgs
		:
			EventArgs
	{
		readonly object _current;
		public object Current
		{
			get { return _current; }
		}

		public UndoRedoEventArgs(object current)
		{
			_current = current;
		}
	} */
	#endregion EventArgs
}
