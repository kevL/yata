using System;
using System.Windows.Forms;


namespace yata
{
	public sealed class MenustripOneclick
		: MenuStrip
	{
		#region Fields (static)
		const int WM_MOUSEACTIVATE = 0x21;
		#endregion Fields (static)


		#region Methods (override)
		/// <summary>
		/// Focuses this <c>MenuStrip</c> when a mouseclick on this
		/// <c>MenuStrip</c> activates its parent <c>Form</c> so that this
		/// <c>MenuStrip's</c> controls can receive the click immediately.
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_MOUSEACTIVATE) // && CanFocus && !Focused
				Focus();

			base.WndProc(ref m);
		}


		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			if ((e.KeyData & ~Constants.ControlShift) != 0)
				logfile.Log("MenustripOneclick.OnPreviewKeyDown() e.KeyData= " + e.KeyData + " e.IsInputKey= " + e.IsInputKey);

			base.OnPreviewKeyDown(e);
		}

		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("MenustripOneclick.ProcessCmdKey() keyData= " + keyData);

			bool ret = base.ProcessCmdKey(ref m, keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". MenustripOneclick.ProcessCmdKey ret= " + ret);

			return ret;
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("MenustripOneclick.IsInputKey() keyData= " + keyData);

			bool ret = base.IsInputKey(keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". MenustripOneclick.IsInputKey ret= " + ret);

			return ret;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log("MenustripOneclick.ProcessDialogKey() keyData= " + keyData);

			bool ret = base.ProcessDialogKey(keyData);
			if ((keyData & ~Constants.ControlShift) != 0)
				logfile.Log(". MenustripOneclick.ProcessDialogKey ret= " + ret);

			return ret;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if ((e.KeyData & ~Constants.ControlShift) != 0)
				logfile.Log("MenustripOneclick.OnKeyDown() ke.KeyData= " + e.KeyData);

			base.OnKeyDown(e);
		}
		#endregion Methods (override)
	}
}
