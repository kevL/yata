using System;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Theirs was fucked so I figured it out. Tks
	/// </summary>
	sealed class YataTabs
		:
			TabControl
	{
		// DoDragDrop/OnDragEnter/OnDragOver/OnDragDrop/OnDragLeave
		// NOTE: The MouseUp event does NOT fire when a drag-drop is released.

		#region Fields
		TabPage _tabDrag, _tabOver;
		#endregion Fields


		#region Properties (override)
		/// <summary>
		/// Prevents flicker.
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000; // enable 'WS_EX_COMPOSITED'
				return cp;
			}
		}
		#endregion Properties (override)


		#region cTor
		internal YataTabs()
		{
			DrawingControl.SetDoubleBuffered(this);
		}
		#endregion cTor


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
				SelectedIndex = dst;
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

			// prevent text-drawing glitches ...
			// Eg. 3 tabs open. DragnDrop the leftmost to the rightmost position
			// or vice versa. The text on the center tab gets superimposed such
			// that it looks like gibberish (until the tab is user-forced to
			// redraw).
			// Unfortunately there still appears to be a single draw-frame in
			// which the old and new texts get superimposed before this refresh
			// happens.
			Refresh();
		}

		// NOTE: Either OnDragDrop fires on a successful target or OnDragLeave
		// fires if drop is on an invalid object.
		protected override void OnDragLeave(EventArgs e)
		{
			ClearTabs();
			base.OnDragLeave(e);
		}
		#endregion Events (override)


		#region OnePage no tabs
		// https://stackoverflow.com/questions/1824036/tabcontrol-how-can-you-remove-the-tabpage-title#answer-1824130

/*		int mPages;

		void checkOnePage()
		{
			if (IsHandleCreated)
			{
				int pages = mPages;
				mPages = TabCount;
				if ((pages == 1 && mPages > 1) || (pages > 1 && mPages == 1))
					RecreateHandle();
			}
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			checkOnePage();
		}

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
			checkOnePage();
		}

		protected override void WndProc(ref Message m)
		{
			// Hide tabs by trapping the TCM_ADJUSTRECT message
			if (m.Msg == 0x1328 && !DesignMode && TabCount == 1)
				m.Result = (IntPtr)1;
			else
				base.WndProc(ref m);
		} */

		// Or try this:
		//tabControl.Top = tabControl.Top - tabControl.ItemSize.Height;
		//tabControl.Height = tabControl.Height + tabControl.ItemSize.Height;
		//tabControl.Region = new Region(new RectangleF(tabPage.Left, tabPage.Top, tabPage.Width, tabPage.Height + tabControl.ItemSize.Height));
		#endregion OnePage no tabs
	}
}
