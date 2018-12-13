using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;


namespace yata
{
	// https://pradeep1210.wordpress.com/2011/04/09/add-undoredo-or-backforward-functionality-to-your-application/

	public delegate void UndoHappened(object sender, UndoRedoEventArgs e);
	public delegate void RedoHappened(object sender, UndoRedoEventArgs e);


	public class UndoRedo<T>
		:
			INotifyPropertyChanged
	{
		#region INotifyPropertyChanged requirements
		public event PropertyChangedEventHandler PropertyChanged;
		
		public void RaisePropertyChangedEvent(string property)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(property));
		}
		#endregion INotifyPropertyChanged requirements


		#region Fields
		readonly Stack UndoStack = new Stack();
		readonly Stack RedoStack = new Stack();

		public T Current;

		public event UndoHappened OnUndo;
		public event RedoHappened OnRedo;
		#endregion Fields


		#region Properties
		public bool CanUndo
		{
			get { return (UndoStack.Count != 0); }
		}

		public bool CanRedo
		{
			get { return (RedoStack.Count != 0); }
		}
		#endregion Properties


		#region cTor
/*		/// <summary>
		/// cTor.
		/// </summary>
		public UndoRedo()
		{
			UndoStack = new Stack();
			RedoStack = new Stack();
		} */
		#endregion cTor


		#region Methods
		public void Clear()
		{
			UndoStack.Clear();
			RedoStack.Clear();

			Current = default(T);

			RaisePropertyChangedEvent("CanUndo");
			RaisePropertyChangedEvent("CanRedo");
		}


		public void Add(T it)
		{
			if (!EqualityComparer<T>.Default.Equals(Current, default(T)))
				UndoStack.Push((T)Current);

			Current = it;

			RaisePropertyChangedEvent("CanUndo");
			RaisePropertyChangedEvent("CanRedo");
		}


		public void Undo()
		{
			RedoStack.Push((T)Current);

			Current = (T)UndoStack.Pop();

			if (OnUndo != null)
				OnUndo(this, new UndoRedoEventArgs(Current));

			RaisePropertyChangedEvent("CanUndo");
			RaisePropertyChangedEvent("CanRedo");
		}

		public void Redo()
		{
			UndoStack.Push((T)Current);

			Current = (T)RedoStack.Pop();
			if (OnRedo != null)
				OnRedo(this, new UndoRedoEventArgs(Current));

			RaisePropertyChangedEvent("CanUndo");
			RaisePropertyChangedEvent("CanRedo");
		}


		public List<T> UndoItems()
		{
			var list = new List<T>();
			foreach (T it in UndoStack)
				list.Add(it);

			return list;
		}

		public List<T> RedoItems()
		{
			var list = new List<T>();
			foreach (T it in RedoStack)
				list.Add(it);

			return list;
		}
		#endregion Methods
	}


	#region EventArgs
	public class UndoRedoEventArgs
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
	}
	#endregion EventArgs
}
