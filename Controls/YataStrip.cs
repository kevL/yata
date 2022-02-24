using System;
//using System.Reflection;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A custom <c>MenuStrip</c> that accepts clicks on
	/// <c>ToolStripDropDown</c> buttons when <c><see cref="YataForm"/></c> gets
	/// <c>Activated</c>.
	/// <c><see cref="ProcessDialogKey()">ProcessDialogKey()</see></c> shall
	/// process <c>[Tab]</c> and <c>[Right]</c> keys.
	/// </summary>
	public sealed class YataStrip
		: MenuStrip
	{
		#region Fields (static)
		const int WM_MOUSEACTIVATE = 0x21;
		#endregion Fields (static)


		#region Fields
		YataForm _f;
		#endregion Fields


		#region Methods
		/// <summary>
		/// Assigns <c><see cref="YataForm"/></c> to <c><see cref="_f"/></c>.
		/// </summary>
		/// <param name="f"></param>
		/// <remarks>I'm doing it this way so that this <c>YataStrip</c> can be
		/// instantiated and built in <c>YataForm's</c> designer, rather than
		/// passing <paramref name="f"/> into the cTor.
		/// 
		/// 
		/// Cheers. Welcome to winforms workaround #2368.</remarks>
		internal void setYata(YataForm f)
		{
			_f = f;
		}
		#endregion Methods


		#region Methods (override)
		/// <summary>
		/// Focuses this <c>MenuStrip</c> when a mouseclick on this
		/// <c>MenuStrip</c> activates its parent <c>Form</c> so that this
		/// <c>MenuStrip's</c> controls can receive the click immediately.
		/// </summary>
		/// <param name="m"></param>
		/// <remarks>If a <c>ToolStripTextBox</c> is clicked it focuses auto. In
		/// fact they need to be bypassed explicitly or else the quirky
		/// select-all-text workaround doesn't work right - a second click in
		/// the texbox would incorrectly keep selecting all text.</remarks>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_MOUSEACTIVATE	// && CanFocus && !Focused
				&&  _f != null				// <- will be null if in DesignMode.
				&& !_f.isTextboxFocused())	// <- workaround to accommodate SelectAll in the textboxes
			{
				Focus();
			}
			base.WndProc(ref m);
		}


		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			// does not fire if a textbox or combobox is currently selected.

			if ((e.KeyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataStrip.OnPreviewKeyDown() e.KeyData= " + e.KeyData + " e.IsInputKey= " + e.IsInputKey);

			base.OnPreviewKeyDown(e);
		}

		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataStrip.ProcessCmdKey() keyData= " + keyData);

			bool ret = base.ProcessCmdKey(ref m, keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". YataStrip.ProcessCmdKey ret= " + ret);

			return ret;
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataStrip.IsInputKey() keyData= " + keyData);

			bool ret = base.IsInputKey(keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". YataStrip.IsInputKey ret= " + ret);

			return ret;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataStrip.ProcessDialogKey() keyData= " + keyData);

//			for (int i = 0; i != Items.Count; ++i)
//			if (Items[i].Selected)
//				logfile.Log(". " + Items[i].Name + " is selected");

			switch (keyData)
			{
				// These are dialog-keys by default.

				case Keys.Escape:
					// .net's default behavior for [Escape] is to select the
					// previously selected control. Override that here to select
					// the Table if it exists.
					//
					// NOTE: Don't try this in ProcessCmdKey() w/ return true -
					// base.ProcessDialogKey() needs to run in order to clear
					// highlights and release focus from the MenuStrip.

					if (YataForm.Table != null)
					{
						logfile.Log(". YataStrip.ProcessDialogKey (select Table)");
						YataForm.Table.Select();
					}
					else
					{
						logfile.Log(". YataStrip.ProcessDialogKey (select Tabs)");
						_f.Tabs.Select();
					}
					break;

				case Keys.Tab:
				case Keys.Right:
					if (_f.it_MenuCol.Selected)
					{
						logfile.Log(". it_MenuCol");

						// NOTE: 'it_MenuCol' can get stuck highlighted on [Tab]
						// or [Right] to 'tb_Goto'. 'this.Select()' appears to
						// clear it but also causes the currently selected it to
						// be forgotten ->
						Select();

						_f.IsTabbed_goto = true;
						_f.tb_Goto.Focus();
						_f.tb_Goto.SelectAll();

						// return TRUE here or else base.ProcessDialogKey(keyData)
						// will essentially re-fire the key and move focus to
						// the *next* control (ie. to 'tb_Search')
						return true;
					}

					if (_f.tb_Goto.Selected && keyData == Keys.Tab)
					{
						logfile.Log(". tb_Goto");
						_f.IsTabbed_search = true;
						_f.tb_Search.SelectAll();
					}
					break;

				case Keys.Shift | Keys.Tab: // reverse dir
//				case Keys.Left:							// <- is consumed by YataTsCombo.ProcessCmdKey() if 'cb_SearchOption' is selected
					if (_f.cb_SearchOption.Selected)	// and it's irrelevant if 'tb_Search' is selected
					{
						logfile.Log(". cb_SearchOption");
						_f.IsTabbed_search = true;
						_f.tb_Search.SelectAll();
					}
					else if (_f.tb_Search.Selected)// && keyData == (Keys.Shift | Keys.Tab))
					{
						logfile.Log(". tb_Search");
						_f.IsTabbed_goto = true;
						_f.tb_Goto.SelectAll();
					}
					break;
			}

			bool ret = base.ProcessDialogKey(keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". YataStrip.ProcessDialogKey ret= " + ret);

			return ret;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if ((e.KeyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataStrip.OnKeyDown() ke.KeyData= " + e.KeyData);

			base.OnKeyDown(e);
		}
		#endregion Methods (override)
	}
}
