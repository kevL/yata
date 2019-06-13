using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ClipboardF
		:
			Form
	{
		#region Fields (static)
		static int _x = -1;
		static int _y = -1;
		static int _w = -1;
		static int _h = -1;
		#endregion Fields (static)


		#region Fields
		readonly YataForm _f;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal ClipboardF(YataForm f)
		{
			InitializeComponent();

			_f = f;

			if (Settings._font2 != null)
				Font = Settings._font2;
			else
				Font = _f.Font;

			if (Settings._fontf != null)
			{
				rtb_Clip.Font.Dispose();
				rtb_Clip.Font = Settings._fontf;
			}

			// TODO: controls are not resizing per Font correctly.
			// vid. AutoScaleMode=

			if (_x == -1) _x = _f.Left + 20;
			if (_y == -1) _y = _f.Top  + 20;

			Left = _x;
			Top  = _y;

			if (_w != -1) Width  = _w;
			if (_h != -1) Height = _h;

			click_Get(null, EventArgs.Empty);
		}
		#endregion cTor


		#region Events
		void click_Done(object sender, EventArgs e)
		{
			Close();
		}

		void click_Get(object sender, EventArgs e)
		{
			string clip = Clipboard.GetText(TextDataFormat.Text).Trim();
			if (!String.IsNullOrEmpty(clip))
				rtb_Clip.Text = clip;
			else
				rtb_Clip.Text = String.Empty;
		}

		void click_Set(object sender, EventArgs e)
		{
			ClipboardAssistant.SetText(rtb_Clip.Text.Replace("\n", Environment.NewLine).Trim());
		}


		void OnClosing(object sender, FormClosingEventArgs e)
		{
			_f.Clip_uncheck();

			_x = Left;
			_y = Top;
			_w = Width;
			_h = Height;
		}
		#endregion Events
	}


	#region Clipboard
	/// <summary>
	/// https://stackoverflow.com/questions/39832057/using-windows-clipboard#answer-39833879
	/// </summary>
	static class ClipboardAssistant
	{
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		static extern IntPtr GetOpenClipboardWindow();

		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		static extern bool OpenClipboard(IntPtr hWndNewOwner);

		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		static extern bool CloseClipboard();

		internal static void SetText(string text)
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
	#endregion Clipboard
}
