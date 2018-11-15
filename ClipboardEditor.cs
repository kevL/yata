using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ClipboardEditor
		:
			Form
	{
		/// <summary>
		/// cTor.
		/// </summary>
		internal ClipboardEditor(YataForm f)
		{
			InitializeComponent();

			Font = f.Font; // NOTE: font is *not* inherited from YataForm. Cf FontCopyForm ...

			// TODO: controls are not resizing per Font correctly.
			// See FontCopyForm ... vid. AutoScaleMode=

			Left = f.Left + 20;
			Top  = f.Top  + 20;

			click_Get(null, EventArgs.Empty);
		}


		void click_Get(object sender, EventArgs e)
		{
			string clip = Clipboard.GetText(TextDataFormat.Text).Trim();
			if (!String.IsNullOrEmpty(clip))
				rtb_Text.Text = clip;
		}

		void click_Set(object sender, EventArgs e)
		{
			ClipboardHelper.SetText(rtb_Text.Text.Replace("\n", Environment.NewLine).Trim());
		}
	}


	/// <summary>
	/// https://stackoverflow.com/questions/39832057/using-windows-clipboard#answer-39833879
	/// </summary>
	static class ClipboardHelper
	{
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		static extern IntPtr GetOpenClipboardWindow();

		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		static extern bool OpenClipboard(IntPtr hWndNewOwner);

		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		static extern bool CloseClipboard();

//		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
//		static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

		public static void SetText(string text)
		{
			if (GetOpenClipboardWindow() != IntPtr.Zero)
			{
				OpenClipboard(IntPtr.Zero);
				CloseClipboard();
			}

			if (!String.IsNullOrEmpty(text))
				Clipboard.SetText(text);
			else
				Clipboard.Clear();
		}
	}
}
