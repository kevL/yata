using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ClipboardEditor
		: YataDialog
	{
		#region cTor
		/// <summary>
		/// cTor. Instantiates Yata's clipboard dialog.
		/// </summary>
		internal ClipboardEditor(YataForm f)
		{
			_f = f;

			InitializeComponent();
			Initialize(YataDialog.METRIC_FUL);

			click_Get(null, EventArgs.Empty);

			rtb_Clip.Select();
			Show(_f); // Yata is owner.
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>FormClosing</c> eventhandler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			(_f as YataForm).CloseClipEditor();
			base.OnFormClosing(e);
		}

		/// <summary>
		/// Closes this <c>ClipboardEditor</c> on <c>[F11]</c>.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Requires <c>KeyPreview</c> <c>true</c>.</remarks>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyData == Keys.F11)
			{
				e.SuppressKeyPress = true;
				Close();
			}
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Handles <c>Click</c> on the Get button.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="btn_Get"/></c></item>
		/// <item><c><see cref="YataForm"/>.it_ClipExport</c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>Get button</item>
		/// <item><c><see cref="YataForm"/>.clipclick_ExportCopy()</c></item>
		/// <item>cTor</item>
		/// </list></remarks>
		internal void click_Get(object sender, EventArgs e)
		{
			rtb_Clip.Text = ClipboardService.GetText().Trim();
		}

		/// <summary>
		/// Handles <c>Click</c> on the Set button.
		/// </summary>
		/// <param name="sender"><c><see cref="btn_Set"/></c></param>
		/// <param name="e"></param>
		void click_Set(object sender, EventArgs e)
		{
			ClipboardService.SetText(rtb_Clip.Text.Replace("\n", Environment.NewLine).Trim());
		}
		#endregion Handlers
	}


	#region Clipboard
	/// <summary>
	/// Gets/Sets text per the Windows Clipboard.
	/// </summary>
	/// <remarks>https://stackoverflow.com/questions/39832057/using-windows-clipboard#answer-39833879</remarks>
	static class ClipboardService
	{
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		static extern IntPtr GetOpenClipboardWindow();

		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		static extern bool OpenClipboard(IntPtr hWndNewOwner);

		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		static extern bool CloseClipboard();

		/// <summary>
		/// Sets a <c>string</c> to the Windows Clipboard after ensuring that
		/// the Clipboard's process has been released by other apps.
		/// </summary>
		/// <param name="clip">the text to set</param>
		internal static void SetText(string clip)
		{
			if (GetOpenClipboardWindow() != IntPtr.Zero)
			{
				OpenClipboard(IntPtr.Zero);
				CloseClipboard();
			}

			if (!String.IsNullOrEmpty(clip))
				Clipboard.SetText(clip);
			else
				Clipboard.Clear();
		}

		/// <summary>
		/// Gets a <c>string</c> from the Windows Clipboard after ensuring that
		/// the Clipboard's process has been released by other apps.
		/// </summary>
		/// <returns>the Clipboard's text</returns>
		internal static string GetText()
		{
			if (GetOpenClipboardWindow() != IntPtr.Zero)
			{
				OpenClipboard(IntPtr.Zero);
				CloseClipboard();
			}

			return Clipboard.GetText(TextDataFormat.Text);
		}
	}
	#endregion Clipboard
}
