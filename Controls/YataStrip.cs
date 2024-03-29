﻿using System;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A custom <c>MenuStrip</c> that accepts clicks on
	/// <c>ToolStripDropDown</c> buttons when <c><see cref="Yata"/></c> gets
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
		Yata _f;
		#endregion Fields


		#region Methods
		/// <summary>
		/// Assigns <c><see cref="Yata"/></c> to <c><see cref="_f"/></c>.
		/// </summary>
		/// <param name="f"></param>
		/// <remarks>I'm doing it this way so that this <c>YataStrip</c> can be
		/// instantiated and built in <c>Yata's</c> designer, rather than
		/// passing <paramref name="f"/> into the cTor.
		/// <br/><br/>
		/// Cheers. Welcome to winforms workaround #2368.</remarks>
		internal void setYata(Yata f)
		{
			_f = f;
		}
		#endregion Methods


		#region Methods (static)
		/// <summary>
		/// Checks if a specified keystroke is a shortcut on Yata's Menubar.
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns><c>true</c> if <paramref name="keyData"/> is a shortcut</returns>
		internal static bool isShortcut(Keys keyData)
		{
			ToolStripMenuItem it;

			ToolStripItemCollection its = Yata.that._bar.Items;
			for (int i = 0; i != its.Count; ++i) // rifle through the top-level Menu its ->
			{
				if ((it = its[i] as ToolStripMenuItem) != null
					&& it.Visible && it.Enabled)
				{
//					if ((e.KeyData & ~gc.ControlShift) != 0)
//						logfile.Log(it.Text);

					if (hasShortcut(it, keyData))
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks menus and any submenus that they may have for a specified
		/// shortcut recursively.
		/// </summary>
		/// <param name="it"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		static bool hasShortcut(ToolStripDropDownItem it, Keys keyData)
		{
			ToolStripMenuItem it0;

			ToolStripItemCollection its = it.DropDownItems;
			for (int i = 0; i != its.Count; ++i)
			{
				if ((it0 = its[i] as ToolStripMenuItem) != null)
//					&& it0.Enabled // check *all* its. Ie, don't allow their shortcuts to be used in any textboxes at all.
				{
//					if ((keyData & ~gc.ControlShift) != 0)
//						logfile.Log(". " + it0.Text + " hasSub= " + it0.HasDropDownItems + " shortcut= " + it0.ShortcutKeys);

					if (it0.ShortcutKeys == keyData
						|| (it0.HasDropDownItems && hasShortcut(it0, keyData)))
					{
						return true;
					}
				}
			}
			return false;
		}
		#endregion Methods (static)


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
			// https://stackoverflow.com/questions/19209047/menustrip-cant-get-focus-on-load#answer-19210787
			// This is a flaw in the MenuStrip and ToolStrip classes, they don't
			// properly handle the WM_MOUSEACTIVATE notification. These classes
			// have plenty of quirks, they support window-less controls and
			// getting them to emulate the exact same behavior as a normal
			// Windows window is never not a problem. These quirks didn't get
			// addressed, the Winforms team got broken up shortly after the
			// .NET 2.0 release to dedicate resources to WPF.

			if (m.Msg == WM_MOUSEACTIVATE	// && CanFocus && !Focused
				&&  _f != null				// <- will be null if in DesignMode.
				&& !_f.isTextboxFocused())	// <- Workaround to pacify SelectAll in the goto/search textboxes:
			{								//    if this condition is not present here, a click can no longer
				Focus();					//    be used to set the caret position; all text gets reselected.
			}
			base.WndProc(ref m);

			// TODO: All text gets selected only if click is to the right of the
			// text; ie. only text that's on the left of the click gets selected
			// ... wtf.
		}

#if Keys
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			// does not fire if a textbox or combobox is currently selected.

			if ((e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("YataStrip.OnPreviewKeyDown() e.KeyData= " + e.KeyData + " e.IsInputKey= " + e.IsInputKey);

			base.OnPreviewKeyDown(e);
		}
#endif

		/// <summary>
		/// Processes a keystroke. I'm not sure what this is even doing anymore;
		/// but somehow it prevents shortcuts on the Menu from firing and allows
		/// a default keystroke like [Ctrl+a] SelectAllText to work in the
		/// goto/search textboxes.
		/// </summary>
		/// <param name="m"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		/// <seealso cref="YataEditbox"><c>YataEditbox.OnPreviewKeyDown()</c></seealso>
		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataStrip.ProcessCmdKey() keyData= " + keyData);
#endif
			if (_f.isTextboxFocused())
			{
				switch (keyData)
				{
					// no need to check these keys for Menu shortcuts ->
					case Keys.Enter:	// handled in YataGrid.ProcessCmdKey()
					case Keys.Escape:	// handled in YataGrid.ProcessCmdKey()
						break;

					// allow [F3] and [Shift+F3] to work if 'tb_Search' has focus ->
					case Keys.F3:
					case Keys.F3 | Keys.Shift:
						if (Yata.Table != null && Yata.that.tb_Search.Focused)
						{
							// latest in .NET newsflash: if the textbox has text
							// then base.ProcessCmdKey() returns true; but if
							// there is no text in the textbox then base.ProcessCmdKey()
							// returns false. wtf
							//
							// I want it to act as if it has text so that an Infobox
							// can be displayed. Force it true ->
							//
							// Unfortunately that alone makes no difference to what I want ...
							// so call the search routine here and return true
							// (false appears to invoke the routine twice).

							Yata.that.IsSearch = true;
							Yata.Table.Search(Yata.that.tb_Search.Text,
											  Yata.that.cb_SearchOption.SelectedIndex == 0,
											  keyData == Keys.F3);
							Yata.that.IsSearch = false;
#if Keys
							logfile.Log(". YataStrip.ProcessCmdKey force TRUE (Search)");
#endif
							return true;
						}

						goto default;

					default:
						if (isShortcut(keyData))
						{
#if Keys
							logfile.Log(". YataStrip.ProcessCmdKey force FALSE (shortcut)");
#endif
							return false; // do not fuckin' ask why. Thanks.
							// Let's just say that I should have stuck with .net's old MainMenu object.
						}
						break;
				}
			}

			bool ret = base.ProcessCmdKey(ref m, keyData);
#if Keys
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataStrip.ProcessCmdKey ret= " + ret);
#endif
			return ret;
		}

#if Keys
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
		/// <c><see cref="Yata.Table">Yata.Table</see></c> or
		/// <c><see cref="YataTabs"/></c></item>
		/// <item><c>[Tab]</c> - navigates right if
		/// <c><see cref="Yata.it_MenuCol">Yata.it_MenuCol</see></c> or
		/// <c><see cref="Yata.tb_Goto">Yata.tb_Goto</see></c> is currently
		/// selected</item>
		/// <item><c>[Right]</c> - navigates right if
		/// <c><see cref="Yata.it_MenuCol">Yata.it_MenuCol</see></c> is
		/// currently selected</item>
		/// <item><c>[Shift+Tab]</c> - navigates left if
		/// <c><see cref="Yata.cb_SearchOption">Yata.cb_SearchOption</see></c>
		/// or <c><see cref="Yata.tb_Search">Yata.tb_Search</see></c> is
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

					if (Yata.Table != null)
					{
#if Keys
						logfile.Log(". YataStrip.ProcessDialogKey (select Table)");
#endif
						Yata.Table.Select();
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
