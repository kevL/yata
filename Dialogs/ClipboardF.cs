using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ClipboardF
		: Form
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
		/// cTor. Instantiates Yata's clipboard dialog.
		/// </summary>
		internal ClipboardF(YataForm f)
		{
			InitializeComponent();

			_f = f;

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			if (Settings._fontf != null)
			{
				rtb_Clip.Font.Dispose();
				rtb_Clip.Font = Settings._fontf;
			}

			rtb_Clip.BackColor = Colors.TextboxBackground;

			// TODO: controls are not resizing per Font correctly.
			// vid. AutoScaleMode=
			// yeah I noticed that; hence the "dialog" fonts ...

			if (_x == -1) _x = _f.Left + 20;
			if (_y == -1) _y = _f.Top  + 20;

			Left = _x;
			Top  = _y;

			if (_w != -1) Width  = _w;
			if (_h != -1) Height = _h;

			click_Get(null, EventArgs.Empty);
		}
		#endregion cTor


		#region Events (override)
		/// <summary>
		/// Handles the <c>Load</c> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			rtb_Clip.AutoWordSelection = false; // <- needs to be here not in the designer to work right.
			rtb_Clip.Select();
			rtb_Clip.SelectionStart = rtb_Clip.Text.Length;
		}

		/// <summary>
		/// Handles the <c>Resize</c> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			// If the vertical scrollbar is visible and user pulls the bottom of
			// the window down past the end of the text -> keep the last line of
			// the text snuggled against the bottom of the window. Thanks.
			//
			// The following code forces the scrollbar/text to re-layout which
			// is all that's needed to keep the last line snuggled against the
			// bottom of the control.

			int pos = rtb_Clip.SelectionStart;
			int len = rtb_Clip.SelectionLength;

			rtb_Clip.SelectionStart  =
			rtb_Clip.SelectionLength = 0;

			rtb_Clip.SelectionStart  = pos;
			rtb_Clip.SelectionLength = len;
		}

		/// <summary>
		/// Handles the <c>FormClosing</c> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_f.ClipEditor_uncheck();

			_x = Left;
			_y = Top;
			_w = Width;
			_h = Height;

			base.OnFormClosing(e);
		}
		#endregion Events (override)


		#region Events
		/// <summary>
		/// Handles click on the Done button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Done(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Handles click on the Get button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Get(object sender, EventArgs e)
		{
			string clip = Clipboard.GetText(TextDataFormat.Text).Trim();
			if (!String.IsNullOrEmpty(clip))
				rtb_Clip.Text = clip;
			else
				rtb_Clip.Text = String.Empty;
		}

		/// <summary>
		/// Handles click on the Set button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Set(object sender, EventArgs e)
		{
			ClipAssist.SetText(rtb_Clip.Text.Replace("\n", Environment.NewLine).Trim());
		}
		#endregion Events
	}


	#region Clipboard
	/// <summary>
	/// https://stackoverflow.com/questions/39832057/using-windows-clipboard#answer-39833879
	/// </summary>
	static class ClipAssist
	{
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		static extern IntPtr GetOpenClipboardWindow();

		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		static extern bool OpenClipboard(IntPtr hWndNewOwner);

		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		static extern bool CloseClipboard();

		/// <summary>
		/// Sets a string of text to the Windows Clipboard after ensuring that
		/// the Clipboard's handle has been freed.
		/// </summary>
		/// <param name="text">the text to set</param>
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
