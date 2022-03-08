using System;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A custom <c>MenuStrip</c> that accepts clicks on
	/// <c>ToolStripDropDown</c> buttons when <c><see cref="YataForm"/></c> gets
	/// <c>Activated</c>.
	/// </summary>
	/// <remarks><c><see cref="ProcessDialogKey()">ProcessDialogKey()</see></c>
	/// shall process <c>[Escape]</c> <c>[Tab]</c> <c>[Right]</c> and
	/// <c>[Shift+Tab]</c> keys.</remarks>
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

#if Keys
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			// does not fire if a textbox or combobox is currently selected.

			if ((e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("YataStrip.OnPreviewKeyDown() e.KeyData= " + e.KeyData + " e.IsInputKey= " + e.IsInputKey);

			base.OnPreviewKeyDown(e);
		}

		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataStrip.ProcessCmdKey() keyData= " + keyData);

			bool ret = base.ProcessCmdKey(ref m, keyData);
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataStrip.ProcessCmdKey ret= " + ret);

			return ret;
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataStrip.IsInputKey() keyData= " + keyData);

			bool ret = base.IsInputKey(keyData);
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataStrip.IsInputKey ret= " + ret);

			return ret;
		}
#endif

		/// <summary>
		/// Processes navigation-keys on this <c>YataStrip</c>.
		/// <list type="bullet">
		/// <item><c>[Escape]</c> - sets focus to
		/// <c><see cref="YataForm.Table">YataForm.Table</see></c> or
		/// <c><see cref="YataTabs"/></c></item>
		/// <item><c>[Tab]</c> - navigates right if
		/// <c><see cref="YataForm.it_MenuCol">YataForm.it_MenuCol</see></c> or
		/// <c><see cref="YataForm.tb_Goto">YataForm.tb_Goto</see></c> is
		/// currently selected</item>
		/// <item><c>[Right]</c> - navigates right if
		/// <c><see cref="YataForm.it_MenuCol">YataForm.it_MenuCol</see></c> is
		/// currently selected</item>
		/// <item><c>[Shift+Tab]</c> - navigates left if
		/// <c><see cref="YataForm.cb_SearchOption">YataForm.cb_SearchOption</see></c>
		/// or <c><see cref="YataForm.tb_Search">YataForm.tb_Search</see></c> is
		/// currently selected</item>
		/// </list>
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		/// <remarks>The dialog-keys typically try to do these things anyway but
		/// special processing is required in order to select all text in the
		/// <c>ToolStripTextBoxes</c> as well as to override .net's default
		/// behavior of <c>ToolStripComboBox</c>.</remarks>
		protected override bool ProcessDialogKey(Keys keyData)
		{
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataStrip.ProcessDialogKey() keyData= " + keyData);
#endif
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
#if Keys
						logfile.Log(". YataStrip.ProcessDialogKey (select Table)");
#endif
						YataForm.Table.Select();
					}
					else
					{
#if Keys
						logfile.Log(". YataStrip.ProcessDialogKey (select Tabs)");
#endif
						_f.Tabs.Select();
					}
					break;

				case Keys.Tab:
					if (_f.tb_Goto.Selected)
					{
#if Keys
						logfile.Log(". tb_Goto");
#endif
						_f.IsTabbed_search = true;
						_f.tb_Search.SelectAll();
					}
					else
						goto case Keys.Right;

					break;

				case Keys.Right:
					if (_f.it_MenuCol.Selected)
					{
#if Keys
						logfile.Log(". it_MenuCol");
#endif
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
					break;

				case Keys.Shift | Keys.Tab: // reverse dir
					if (_f.cb_SearchOption.Selected)
					{
#if Keys
						logfile.Log(". cb_SearchOption");
#endif
						_f.IsTabbed_search = true;
						_f.tb_Search.SelectAll();
					}
					else if (_f.tb_Search.Selected)
					{
#if Keys
						logfile.Log(". tb_Search");
#endif
						_f.IsTabbed_goto = true;
						_f.tb_Goto.SelectAll();
					}
					break;

				// [Left] is consumed by YataTsCombo.ProcessCmdKey() if 'cb_SearchOption'
				// is selected and is processed by the textbox if 'tb_Search' is selected.
			}

			bool ret = base.ProcessDialogKey(keyData);
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataStrip.ProcessDialogKey ret= " + ret);
#endif
			return ret;
		}

#if Keys
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if ((e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("YataStrip.OnKeyDown() ke.KeyData= " + e.KeyData);

			base.OnKeyDown(e);
		}
#endif
		#endregion Methods (override)
	}
}
