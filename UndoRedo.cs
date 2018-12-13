using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;


namespace yata
{
	// https://pradeep1210.wordpress.com/2011/04/09/add-undoredo-or-backforward-functionality-to-your-application/

	public delegate void UndoHappened(object sender, UndoRedoEventArgs e);
	public delegate void RedoHappened(object sender, UndoRedoEventArgs e);


//	public class UndoRedo<T>
	public class UndoRedo
		:
			INotifyPropertyChanged
	{
		#region INotifyPropertyChanged requirements
		public event PropertyChangedEventHandler PropertyChanged;
		
		public void RaisePropertyChangedEvent(string property)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(property));
		}
		#endregion INotifyPropertyChanged requirements


		#region Fields
		readonly Stack UndoActions = new Stack();
		readonly Stack RedoActions = new Stack();

//		public T Current;
		public object Current;

		public event UndoHappened OnUndo;
		public event RedoHappened OnRedo;
		#endregion Fields


		#region Properties
		public bool CanUndo
		{
			get { return (UndoActions.Count != 0); }
		}

		public bool CanRedo
		{
			get { return (RedoActions.Count != 0); }
		}
		#endregion Properties


		#region cTor
/*		/// <summary>
		/// cTor.
		/// </summary>
		public UndoRedo()
		{
			UndoActions = new Stack();
			RedoActions = new Stack();
		} */
		#endregion cTor


		#region Methods
		public void Clear()
		{
			UndoActions.Clear();
			RedoActions.Clear();

//			Current = default(T);
			Current = default(object);

			RaisePropertyChangedEvent("CanUndo");
			RaisePropertyChangedEvent("CanRedo");
		}


//		public void Add(T it)
		public void Add(object it)
		{
//			if (!EqualityComparer<T>.Default.Equals(Current, default(T)))
//				UndoStack.Push((T)Current);
			if (!EqualityComparer<object>.Default.Equals(Current, default(object)))
				UndoActions.Push((object)Current);

			Current = it;

			RaisePropertyChangedEvent("CanUndo");
			RaisePropertyChangedEvent("CanRedo");
		}


		public void Undo()
		{
//			RedoStack.Push((T)Current);
			RedoActions.Push((object)Current);

//			Current = (T)UndoStack.Pop();
			Current = (object)UndoActions.Pop();

			if (OnUndo != null)
				OnUndo(this, new UndoRedoEventArgs(Current));

			RaisePropertyChangedEvent("CanUndo");
			RaisePropertyChangedEvent("CanRedo");
		}

		public void Redo()
		{
//			UndoStack.Push((T)Current);
			UndoActions.Push((object)Current);

//			Current = (T)RedoStack.Pop();
			Current = (object)RedoActions.Pop();
			if (OnRedo != null)
				OnRedo(this, new UndoRedoEventArgs(Current));

			RaisePropertyChangedEvent("CanUndo");
			RaisePropertyChangedEvent("CanRedo");
		}


//		public List<T> UndoItems()
		public List<object> UndoItems()
		{
//			var list = new List<T>();
//			foreach (T it in UndoStack)
			var list = new List<object>();
			foreach (object it in UndoActions)
				list.Add(it);

			return list;
		}

//		public List<T> RedoItems()
		public List<object> RedoItems()
		{
//			var list = new List<T>();
//			foreach (T it in RedoStack)
			var list = new List<object>();
			foreach (object it in RedoActions)
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
