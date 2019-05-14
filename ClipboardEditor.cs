using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ClipboardEditor
		:
			Form
	{
		static int _x = -1;
		static int _y = -1;
		static int _w = -1;
		static int _h = -1;


		/// <summary>
		/// cTor.
		/// </summary>
		internal ClipboardEditor(YataForm f)
		{
			InitializeComponent();

			Font = f.Font;

			Owner = f;

			// TODO: controls are not resizing per Font correctly.
			// See FontCopyForm ... vid. AutoScaleMode=

			if (_x == -1) _x = f.Left + 20;
			if (_y == -1) _y = f.Top  + 20;

			Left = _x;
			Top  = _y;

			if (_w != -1) Width  = _w;
			if (_h != -1) Height = _h;

			click_Get(null, EventArgs.Empty);
		}


		void click_Done(object sender, EventArgs e)
		{
			Close();
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


		void OnClosing(object sender, FormClosingEventArgs e)
		{
			_x = Left;
			_y = Top;
			_w = Width;
			_h = Height;
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
