using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Theirs was fucked so I figured it out. Tks
	/// </summary>
	sealed class YataTabs
		: TabControl
	{
		// DoDragDrop/OnDragEnter/OnDragOver/OnDragDrop/OnDragLeave
		// NOTE: The MouseUp event does NOT fire when a drag-drop is released.

		#region Fields
		TabPage _tabDrag;
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
		/// <summary>
		/// cTor.
		/// </summary>
		internal YataTabs()
		{
			DrawRegulator.SetDoubleBuffered(this);

			Name     = "Tabs";
			TabIndex = 3;

			Dock      = DockStyle.Fill;
			Multiline = true;
			AllowDrop = true;
			DrawMode  = TabDrawMode.OwnerDrawFixed;
			SizeMode  = TabSizeMode.Fixed;

			Padding  = new Point(0,0); // Padding uses Point and Margin uses Padding
			Margin   = new Padding(0); // right got it.
		}
		#endregion cTor


		#region Methods
		/// <summary>
		/// Gets the <c>TabPage</c> that the cursor is currently over.
		/// </summary>
		/// <returns></returns>
		TabPage get()
		{
			for (int i = 0; i != TabPages.Count; ++i)
			{
				if (GetTabRect(i).Contains(PointToClient(Cursor.Position)))
					return TabPages[i];
			}
			return null;
		}
		#endregion Methods


		#region Handlers (override)
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			if ((e.KeyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataTabs.OnPreviewKeyDown() e.KeyData= " + e.KeyData + " e.IsInputKey= " + e.IsInputKey);

			base.OnPreviewKeyDown(e);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataTabs.ProcessCmdKey() keyData= " + keyData);

			bool ret = base.ProcessCmdKey(ref msg, keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". YataTabs.ProcessCmdKey ret= " + ret);

			return ret;
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataTabs.IsInputKey() keyData= " + keyData);

			bool ret = base.IsInputKey(keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". YataTabs.IsInputKey ret= " + ret);

			return ret;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataTabs.ProcessDialogKey() keyData= " + keyData);

			bool ret = base.ProcessDialogKey(keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". YataTabs.ProcessDialogKey ret= " + ret);

			return ret;
		}

		/// <summary>
		/// Bypasses <c>[Ctrl+Shift+PageUp/Down]</c> that would change the
		/// active tab so that <c><see cref="YataGrid"/>.OnKeyDown()</c> can
		/// handle it.
		/// </summary>
		/// <remarks><c>[Ctrl+PageUp/Down]</c> still work to change tabpages -
		/// but <c>[Ctrl+Tab]</c> and <c>[Ctrl+Shift+Tab]</c> have been consumed
		/// by <c>YataGrid.ProcessDialogKey()</c>.</remarks>
		protected override void OnKeyDown(KeyEventArgs ke)
		{
			if ((ke.KeyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataTabs.OnKeyDown() ke.KeyData= " + ke.KeyData);

			switch (ke.KeyData)
			{
				case Keys.Shift | Keys.Control | Keys.PageUp:
					logfile.Log(". Keys.Shift | Keys.Control | Keys.PageUp");
					return;

				case Keys.Shift | Keys.Control | Keys.PageDown:
					logfile.Log(". Keys.Shift | Keys.Control | Keys.PageDown");
					return;

//				case Keys.Control | Keys.Tab:
//				case Keys.Shift | Keys.Control | Keys.Tab:
//					return;
			}
			base.OnKeyDown(ke);
		}

		/// <summary>
		/// Overrides the <c>MouseDown</c> eventhandler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left
				&& (_tabDrag = get()) != null)
			{
				DoDragDrop(_tabDrag, DragDropEffects.Move);
			}
			base.OnMouseDown(e);
		}

		/// <summary>
		/// Overrides the <c>DragEnter</c> eventhandler.
		/// </summary>
		/// <param name="drgevent"></param>
		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			drgevent.Effect = DragDropEffects.Move;
			base.OnDragEnter(drgevent);
		}

		/// <summary>
		/// Overrides the <c>DragDrop</c> eventhandler.
		/// </summary>
		/// <param name="drgevent"></param>
		/// <remarks>Either <c>DragDrop</c> fires on a successful target or
		/// <c>DragLeave</c> fires if drop is on an invalid object.</remarks>
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			if (_tabDrag != null)
			{
				TabPage tabOver = get();
				if (tabOver != null && tabOver != _tabDrag)
				{
					int src = TabPages.IndexOf(_tabDrag);
					int dst = TabPages.IndexOf( tabOver);

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
			}

			base.OnDragDrop(drgevent);
		}

		// NOTE: Either OnDragDrop fires on a successful target or OnDragLeave
		// fires if drop is on an invalid object. Or as soon as the cursor
		// leaves the tab-row(s).
//		protected override void OnDragLeave(EventArgs e)
//		{
//			base.OnDragLeave(e);
//		}
		#endregion Handlers (override)


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
