using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Fields (static)
		int _dontbeep; // directs keydown [Enter] to the appropriate funct: Goto or Search
		const int DONTBEEP_DEFAULT = 0;
		const int DONTBEEP_GOTO    = 1;
		const int DONTBEEP_SEARCH  = 2;
		#endregion Fields (static)


#if Keys
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			if ((e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("Yata.OnPreviewKeyDown() e.KeyData= " + e.KeyData + " e.IsInputKey= " + e.IsInputKey);

			base.OnPreviewKeyDown(e);
		}
#endif

		/// <summary>
		/// Processes so-called command-keys.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("Yata.ProcessCmdKey() keyData= " + keyData);
#endif
			switch (keyData)
			{
				case Keys.Menu | Keys.Alt:
					// NOTE: The Menubar container is by default TabStop=FALSE and ... (not) forced TRUE in Yata.cTor <-
					// so it can never take focus - but its subcontrols are fucked re. "focus".
					// ... because they aren't actually 'Controls'.

					if (Table != null)
					{
#if Keys
						logfile.Log(". select Table");
#endif
						// set '_editor.Visible' FALSE else its leave event
						// fires twice when it loses focus ->

						Table.editresultdefault();
						Table.Select();
					}
					else
					{
#if Keys
						logfile.Log(". select Tabs");
#endif
						Tabs.Select();
					}
					return false; // do not return True. [Alt] needs to 'activate' the Menubar.
			}

			bool ret = base.ProcessCmdKey(ref msg, keyData);
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". Yata.ProcessCmdKey ret= " + ret);
#endif
			return ret;
		}

#if Keys
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("Yata.IsInputKey() keyData= " + keyData);

			bool ret = base.IsInputKey(keyData);
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". Yata.IsInputKey ret= " + ret);

			return ret;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("Yata.ProcessDialogKey() keyData= " + keyData);

			bool ret = base.ProcessDialogKey(keyData);
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". Yata.ProcessDialogKey ret= " + ret);

			return ret;
		}
#endif

		/// <summary>
		/// Handles the <c>KeyDown</c> event at the form-level.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Requires the form's <c>KeyPreview</c> property flagged
		/// <c>true</c>.
		/// 
		/// 
		/// Fires repeatedly if a key is held depressed.</remarks>
		protected override void OnKeyDown(KeyEventArgs e)
		{
#if Keys
			if ((e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("Yata.OnKeyDown() e.KeyData= " + e.KeyData);
#endif
			if (Table != null)
			{
				switch (e.KeyData)
				{
					case Keys.Enter: // do this here to get rid of the beep.
					case Keys.Shift | Keys.Enter:
#if Keys
						logfile.Log(". Keys.Enter");
#endif
						if (tb_Search.Focused || cb_SearchOption.Focused)
						{
#if Keys
							if (e.KeyData == Keys.Enter) logfile.Log(". . Search forward");
							else                         logfile.Log(". . Search reverse");
#endif
							_dontbeep = DONTBEEP_SEARCH;
						}
						else if (tb_Goto.Focused && e.KeyData == Keys.Enter)
						{
#if Keys
							logfile.Log(". . Goto");
#endif
							_dontbeep = DONTBEEP_GOTO;
						}
						else
						{
#if Keys
							logfile.Log(". . Search or Goto not focused");
#endif
							_dontbeep = DONTBEEP_DEFAULT;
						}

						if (_dontbeep != DONTBEEP_DEFAULT)
						{
							e.SuppressKeyPress = true;
							BeginInvoke(DontBeepEvent);
						}
						break;

					case Keys.Escape:
#if Keys
						logfile.Log(". Keys.Escape");
#endif
						if (Tabs.Focused || bu_Propanel.Focused)	// btn -> jic. The Propanel button can become focused by
						{											// keyboard (I saw it happen once) but can't figure out how.
#if Keys															// NOTE: It wasn't actually focused, it was a graphical glitch.
							logfile.Log(". . deselect Tabs -> select Grid");
#endif
							e.SuppressKeyPress = true;
							Table.Select();
						}
#if Keys
						else logfile.Log(". . Tabs not focused");
#endif
						break;

					case Keys.Space:
#if Keys
						logfile.Log(". Keys.Space");
#endif
						if (!Table._editor.Visible
							&& (Table.Propanel == null || !Table.Propanel._editor.Visible))
						{
#if Keys
							logfile.Log(". . select first cell");
#endif
							e.SuppressKeyPress = true;
							Table.SelectCell_first();
						}
#if Keys
						else logfile.Log(". . an Editor is visible -> do not select first cell");
#endif
						break;

					case Keys.Control | Keys.Space:
#if Keys
						logfile.Log(". Keys.Control | Keys.Space");
#endif
						if (!Table._editor.Visible
							&& (Table.Propanel == null || !Table.Propanel._editor.Visible))
						{
#if Keys
							logfile.Log(". . select first row");
#endif
							e.SuppressKeyPress = true;
							Table.SelectRow_first();
						}
#if Keys
						else logfile.Log(". . an Editor is visible -> do not select first row");
#endif
						break;
				}


				// clear the col-width adjustor on '_panelCols' ->
				switch (e.KeyCode)
				{
					case Keys.Menu:			// Keys.Alt
					case Keys.ControlKey:	// Keys.Control
					case Keys.ShiftKey:		// Keys.Shift
						Table._panelCols.Cursor = Cursors.Default;
						Table._panelCols.IsCursorSplit = false;
						Table._panelCols.IsGrab = false;
						break;
				}
			}
			base.OnKeyDown(e);
		}

		/// <summary>
		/// Overrides the <c>KeyUp</c> eventhandler. Enables the col-width
		/// adjustor if appropriate.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (Table != null)
			{
				// enable the col-width adjustor on '_panelCols' ->
				switch (e.KeyCode)
				{
					case Keys.Menu:			// Keys.Alt
					case Keys.ControlKey:	// Keys.Control
					case Keys.ShiftKey:		// Keys.Shift
						if (ModifierKeys == Keys.None)
						{
							Point pos = Table._panelCols.PointToClient(Cursor.Position);
							if (Table._panelCols.GetSplitterCol(pos.X) != -1
								&& pos.Y >= Table._panelCols.Location.Y
								&& pos.Y <  Table._panelCols.Location.Y + Table._panelCols.Height)
							{
								Table._panelCols.Cursor = Cursors.VSplit;
								Table._panelCols.IsCursorSplit = true;
							}
						}
						break;
				}
			}
			base.OnKeyUp(e);
		}

		/// <summary>
		/// Forwards a <c>KeyDown</c> <c>[Enter]</c> event to an appropriate
		/// funct.
		/// </summary>
		/// <remarks>Is basically just a convoluted handler for the
		/// <c><see cref="OnKeyDown()">OnKeyDown()</see></c> handler to stop the
		/// *beep* if <c>[Enter]</c> is keyed when a <c>TextBox</c> is focused.</remarks>
		void HandleDontBeepEvent()
		{
			switch (_dontbeep)
			{
				case DONTBEEP_SEARCH:
					Search_keyEnter();
					break;

				case DONTBEEP_GOTO:
					Table.Goto(tb_Goto.Text, true);
					break;
			}
		}
	}
}
