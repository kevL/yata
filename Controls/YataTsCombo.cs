using System;
using System.Windows.Forms;


namespace yata
{
	public sealed class YataTsCombo
		: ToolStripComboBox
	{
		#region Methods (override)
		// NOTE: Does not have OnPreviewKeyDown().

		/// <summary>
		/// Overrides <c>ToolStripItem.ProcessCmdKey()</c> to bypass .net's
		/// default navigation for
		/// <c><see cref="YataForm.cb_SearchOption">YataForm.cb_SearchOption</see></c>
		/// on <c><see cref="YataForm._bar">YataForm._bar</see></c>.
		/// <list type="bullet">
		/// <item><c>[Left]</c> - navigates to the left control on the Menubar</item>
		/// <item><c>[Right]</c> - navigates to the right control on the Menubar</item>
		/// </list>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		/// <remarks>This fires not only if
		/// <c><see cref="YataForm.cb_SearchOption">YataForm.cb_SearchOption</see></c>
		/// has focus but also if <c><see cref="YataForm"/>.it_MenuFile</c> has
		/// focus etc.</remarks>
		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
#if DEBUG
			if (gc.KeyLog && (keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataTsCombo.ProcessCmdKey() keyData= " + keyData);
#endif
			if (Selected) // workaround since this fires when another it is currently selected.
			{
#if DEBUG
				if (gc.KeyLog) logfile.Log(". is Selected");
#endif
				switch (keyData)
				{
					case Keys.Left:
						YataForm.that.IsTabbed_search = true;
						YataForm.that.tb_Search.Focus();
						YataForm.that.tb_Search.SelectAll();
#if DEBUG
						if (gc.KeyLog) logfile.Log(". YataTsCombo.ProcessCmdKey force TRUE (focus tb_Search)");
#endif
						return true;

					case Keys.Right:
						// NOTE: Without first selecting the MenuStrip the UI
						// looks like 'it_MenuClipboard' has focus but actually
						// 'cb_SearchOption' is still taking key-input.

						(Parent as MenuStrip).Select(); // bingo! Despite '(Parent as MenuStrip).CanSelect' == FALSE.
						YataForm.that.it_MenuClipboard.Select();
#if DEBUG
						if (gc.KeyLog) logfile.Log(". YataTsCombo.ProcessCmdKey force TRUE (select it_MenuClipboard)");
#endif
						return true;
				}
			}

			bool ret = base.ProcessCmdKey(ref m, keyData);
#if DEBUG
			if (gc.KeyLog && (keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataTsCombo.ProcessCmdKey ret= " + ret);
#endif
			return ret;
		}

#if DEBUG
		protected override bool IsInputKey(Keys keyData) // does not fire.
		{
			if (gc.KeyLog && (keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataTsCombo.IsInputKey() keyData= " + keyData);

			bool ret = base.IsInputKey(keyData);
			if (gc.KeyLog && (keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataTsCombo.IsInputKey ret= " + ret);

			return ret;
		}

		protected override bool ProcessDialogKey(Keys keyData) // does not fire.
		{
			if (gc.KeyLog && (keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataTsCombo.ProcessDialogKey() keyData= " + keyData);

			bool ret = base.ProcessDialogKey(keyData);
			if (gc.KeyLog && (keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataTsCombo.ProcessDialogKey ret= " + ret);

			return ret;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (gc.KeyLog && (e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("YataTsCombo.OnKeyDown() ke.KeyData= " + e.KeyData);

			base.OnKeyDown(e);
		}
#endif
		#endregion Methods (override)
	}
}
