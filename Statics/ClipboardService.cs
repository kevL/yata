using System;
using System.Windows.Forms;


namespace yata
{
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
}
