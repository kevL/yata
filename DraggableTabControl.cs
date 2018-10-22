using System;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Theirs was fucked so I figured it out. Tks
	/// </summary>
	class DraggableTabControl
		:
			TabControl
	{
		// DoDragDrop/OnDragEnter/OnDragOver/OnDragDrop/OnDragLeave
		// NOTE: The MouseUp event does NOT fire when a drag-drop is underway.

		#region Fields
		TabPage _tabDrag, _tabOver;
		#endregion Fields


		#region Methods
		TabPage get()
		{
			for (int i = 0; i != TabPages.Count; ++i)
			{
				if (GetTabRect(i).Contains(PointToClient(Cursor.Position)))
					return TabPages[i];
			}
			return null;
		}

		void DropDrag()
		{
			if (_tabDrag != null
				&& (_tabOver = get()) != null
				&& _tabDrag != _tabOver)
			{
				int src = TabPages.IndexOf(_tabDrag);
				int dst = TabPages.IndexOf(_tabOver);

				if (src < dst) // NOTE: The start and stop IDs won't be the same.
				{
					do
					{
						TabPages[src] = TabPages[++src];
					}
					while (src != dst);
				}
				else //if (dst < src)
				{
					do
					{
						TabPages[src] = TabPages[--src];
					}
					while (src != dst);
				}

				TabPages[dst] = _tabDrag;
				SelectedIndex = TabPages.IndexOf(_tabDrag);
			}
		}

		void ClearTabs()
		{
			_tabDrag =
			_tabOver = null;
		}
		#endregion Methods


		#region Events (override)
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left
				&& (_tabDrag = get()) != null)
			{
				DoDragDrop(_tabDrag, DragDropEffects.Move);
			}
			base.OnMouseDown(e);
		}

		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			drgevent.Effect = DragDropEffects.Move;
			base.OnDragEnter(drgevent);
		}

		// NOTE: Either OnDragDrop fires on a successful target or OnDragLeave
		// fires if drop is on an invalid object.
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			if (_tabDrag != null)
			{
				DropDrag();
				ClearTabs();
			}
			base.OnDragDrop(drgevent);
		}

		// NOTE: Either OnDragDrop fires on a successful target or OnDragLeave
		// fires if drop is on an invalid object.
		protected override void OnDragLeave(EventArgs e)
		{
			ClearTabs();
			base.OnDragLeave(e);
		}
		#endregion Events (override)
	}
}
