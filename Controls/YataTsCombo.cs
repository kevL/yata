using System;
//using System.Reflection;
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
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataTsCombo.ProcessCmdKey() keyData= " + keyData);

			if (Selected) // workaround since this fires when another it is currently selected.
			{
				logfile.Log(". is Selected");

				switch (keyData)
				{
					case Keys.Left:
						YataForm.that.IsTabbed_search = true;
						YataForm.that.tb_Search.Focus();
						YataForm.that.tb_Search.SelectAll();

						logfile.Log(". YataTsCombo.ProcessCmdKey force TRUE (focus tb_Search)");
						return true;

					case Keys.Right:
						// NOTE: Without first selecting the MenuStrip the UI
						// looks like 'it_MenuClipboard' has focus but actually
						// 'cb_SearchOption' is still taking key-input.

						(Parent as MenuStrip).Select(); // bingo! Despite '(Parent as MenuStrip).CanSelect' == FALSE.
						YataForm.that.it_MenuClipboard.Select();

						logfile.Log(". YataTsCombo.ProcessCmdKey force TRUE (select it_MenuClipboard)");
						return true;
				}
			}

			bool ret = base.ProcessCmdKey(ref m, keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". YataTsCombo.ProcessCmdKey ret= " + ret);

			return ret;
		}

		protected override bool IsInputKey(Keys keyData) // does not fire.
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataTsCombo.IsInputKey() keyData= " + keyData);

			bool ret = base.IsInputKey(keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". YataTsCombo.IsInputKey ret= " + ret);

			return ret;
		}

		protected override bool ProcessDialogKey(Keys keyData) // does not fire.
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataTsCombo.ProcessDialogKey() keyData= " + keyData);

			bool ret = base.ProcessDialogKey(keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". YataTsCombo.ProcessDialogKey ret= " + ret);

			return ret;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if ((e.KeyData & ~Constants.ControlShift) != 0)
				logfile.Log("YataTsCombo.OnKeyDown() ke.KeyData= " + e.KeyData);

			base.OnKeyDown(e);
		}
		#endregion Methods (override)
	}
}
