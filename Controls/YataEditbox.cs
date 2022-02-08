using System;
using System.Windows.Forms;


namespace yata
{
	sealed class YataEditbox
		: TextBox
	{
		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal YataEditbox()
		{
			Visible     = false;
			BackColor   = Colors.Editor;
			BorderStyle = BorderStyle.None;
			WordWrap    = false;
			Margin      = new Padding(0);
		}
		#endregion cTor


		#region Methods (override)
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			if ((e.KeyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataEditbox.OnPreviewKeyDown() e.KeyData= " + e.KeyData + " e.IsInputKey= " + e.IsInputKey);

			switch (e.KeyData)
			{
				// Set 'e.IsInputKey' TRUE to bypass Menu shortcuts
				// (keystrokes that are used by menuitems) as well as
				// ProcessCmdKey(), IsInputKey(), and ProcessDialogKey().
				// Only keystrokes that .net uses in TextBox AND that are also
				// used on the Menus need 'e.IsInputKey' set TRUE.
				//
				// Note that forcing IsInputKey() TRUE will *not* bypass
				// ProcessCmdKey() but will bypass ProcessDialogKey(). And do
				// *not* rely on .net to figure out whether or not a keystroke
				// is or should be an input-key in IsInputKey() anyway ... tests
				// have indicated that it doesn't.
				//
				// but unfortunately I can't find a complete list of keystrokes
				// that .net's TextBox treats as input-keys. Some of them are
				// totally whacko - eg. [Ctrl+Backspace]. So, to be safe, set
				// all Menu shortcuts to 'e.IsInputKey' here ->
				//
				// unfortunately again, setting 'e.IsInputKey' TRUE does *not*
				// consume the keystroke, and it will still be sent to the
				// OnKeyDown() handlers, bubbling up through the
				// containing controls, if the key-events of those controls are
				// set to fire ... note that during the bubbling, OnKeyDown()
				// for this Editbox fires *last* - *after* YataTabs.OnKeyDown()
				// and YataForm.OnKeyDown() in that order, only then does
				// YataEditbox.OnKeyDown() execute.
				//
				//
				// Do *not* try to force 'e.IsInputKey' FALSE in OnPreviewKeyDown()
				// because 'IsInputKey' FALSE will be redetermined by .net in
				// its event-chain to be TRUE by the time IsInputKey() fires, if
				// .net determined a key to be a stock-defined input-key ...
				// that is, .net will say that such a key is *not* an input-key
				// here in OnPreviewKeyDown() but will later say that it *is*
				// an input-key in the return from IsInputKey().
				//
				// IsInputKey TRUE makes the KeyDown event(s) fire, bypassing ProcessDialogKey().
				// IsInputKey FALSE executes ProcessDialogKey(), bypassing the KeyDown events.
				//
				// The order of keyed events is as follows:
				// - OnPreviewKeyDown
				// - ProcessCmdKey (if !e.IsInputKey) - bubbles up through containing controls
				// - IsInputKey    (if !e.IsInputKey)
				// - ProcessDialogKey OR OnKeyDown    - bubbles up through containing controls, kinda.
				//
				// THIS IS SO FUCKED. but i think i got it enough to start
				// coding again ...
				//
				//
				// The following keystrokes are used by .net's default TextBox
				// behavior. Keystrokes that are also used outside of the
				// TextBox - eg. in a Menu or in ProcessCmdKey() etc. - need to
				// have 'e.IsInputKey' set TRUE here ->


				case Keys.Control | Keys.X: // Cells|Cut
				case Keys.Control | Keys.C: // Cells|Copy
				case Keys.Control | Keys.V: // Cells|Paste
				case Keys.Delete:           // Cells|Delete

//				case Keys.Shift | Keys.Delete: // appears to not be hooked by textboxes.

				case Keys.Control | Keys.A: // File|SaveAll

				case Keys.Control | Keys.Z: // Edit|Undo
				case Keys.Control | Keys.Y: // Edit|Redo
					logfile.Log(". YataEditbox.OnPreviewKeyDown force e.IsInputKey TRUE");
					e.IsInputKey = true;
					break;
			}
			base.OnPreviewKeyDown(e);
		}

		/// <summary>
		/// Processes so-called command-keys.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataEditbox.ProcessCmdKey() keyData= " + keyData);

			bool ret = base.ProcessCmdKey(ref msg, keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". YataEditbox.ProcessCmdKey ret= " + ret);

			return ret;
		}

		/// <summary>
		/// Disallows <c>[Up]</c>, <c>[Down]</c>, <c>[PageUp]</c>, and
		/// <c>[PageDown]</c> for textbox navigation on this <c>YataEditbox</c>.
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		/// <remarks><c>[Up]</c>, <c>[Down]</c>, <c>[PageUp]</c>, and
		/// <c>[PageDown]</c> shall be used for Tabfastedit by
		/// <c><see cref="YataGrid._editor">YataGrid._editor</see></c> and/or
		/// <c><see cref="Propanel._editor">Propanel._editor</see></c>.</remarks>
		/// <remarks>TAB, RETURN, ESC, and the UP ARROW, DOWN ARROW, LEFT ARROW,
		/// and RIGHT ARROW. kL_note: Also PageUp/Down what else you gnits.</remarks>
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataEditbox.IsInputKey() keyData= " + keyData);

			switch (keyData)
			{
				// return FALSE to disable use of these keystrokes in the
				// textbox, thereby allowing them for use by
				// - ProcessCmdKey (as long as 'e.IsInputKey' was *not* set TRUE in OnPreviewKeyDown())
				// - ProcessDialogKey
				//
				// Note that a return of FALSE bypasses the KeyDown/Up events;
				// ie, handle the keystroke in Process*Key() funct(s).


				case Keys.Up:
				case Keys.Down:
				case Keys.PageUp:
				case Keys.PageDown:
					logfile.Log(". YataEditbox.IsInputKey force FALSE");
					return false;
			}

			bool ret = base.IsInputKey(keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". YataEditbox.IsInputKey ret= " + ret);

			return ret;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataEditbox.ProcessDialogKey() keyData= " + keyData);

			bool ret = base.ProcessDialogKey(keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". YataEditbox.ProcessDialogKey ret= " + ret);

			return ret;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if ((e.KeyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataEditbox.OnKeyDown() e.KeyData= " + e.KeyData);

			base.OnKeyDown(e);
		}
		#endregion Methods (override)
	}
}
