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
		/// Overrides the <c>Load</c> eventhandler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			rtb_Clip.AutoWordSelection = false; // <- needs to be here not in the designer to work right.
			rtb_Clip.Select();
			rtb_Clip.SelectionStart = rtb_Clip.Text.Length;
		}

		/// <summary>
		/// Overrides the <c>Resize</c> eventhandler.
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
		/// Overrides the <c>FormClosing</c> eventhandler.
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
		/// Handles <c>Click</c> on the Done button.
		/// </summary>
		/// <param name="sender"><c><see cref="btn_Done"/></c></param>
		/// <param name="e"></param>
		void click_Done(object sender, EventArgs e)
		{
			Close();
		}

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
		#endregion Events
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
