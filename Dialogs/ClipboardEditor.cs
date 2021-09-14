using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ClipboardEditor
		: Form
	{
		#region Fields (static)
		static int _x = -1, _y;
		static int _w = -1, _h;
		#endregion Fields (static)


		#region Fields
		readonly YataForm _f;

		/// <summary>
		/// Bypasses setting <c><see cref="_w"/></c> and <c><see cref="_h"/></c>
		/// when this <c>FontF</c> dialog instantiates. Otherwise when .net
		/// automatically fires the <c>Resize</c> event during instantiation the
		/// values get set in a way that renders the
		/// <c>ClientSize.Width/.Height</c> static metrics irrelevant. This is
		/// why I like Cherios!
		/// </summary>
		bool _init = true;
		#endregion Fields


		#region Properties (static)
		/// <summary>
		/// Tracks if user has this <c>ClipboardEditor</c> dialog maximized.
		/// </summary>
		internal static bool Maximized
		{ get; private set; }
		#endregion Properties (static)


		#region cTor
		/// <summary>
		/// cTor. Instantiates Yata's clipboard dialog.
		/// </summary>
		internal ClipboardEditor(YataForm f)
		{
			_f = f;

			InitializeComponent();

			rtb_Clip.BackColor = Colors.TextboxBackground;

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			if (Settings._fontf != null)
			{
				rtb_Clip.Font.Dispose();
				rtb_Clip.Font = Settings._fontf;
			}

			if (_x == -1)
			{
				_x = Math.Max(0, _f.Left + 20);
				_y = Math.Max(0, _f.Top  + 20);
			}

			Left = _x;
			Top  = _y;

			if (_w != -1)
				ClientSize = new Size(_w,_h);

			Screen screen = Screen.FromPoint(new Point(Left, Top));
			if (screen.Bounds.Width < Left + Width) // TODO: decrease Width if this shifts the
				Left = screen.Bounds.Width - Width; // window off the left edge of the screen.

			if (screen.Bounds.Height < Top + Height) // TODO: decrease Height if this shifts the
				Top = screen.Bounds.Height - Height; // window off the top edge of the screen.

			if (Maximized)
				WindowState = FormWindowState.Maximized;

			_init = false;

			click_Get(null, EventArgs.Empty);

			Show(_f); // Yata is owner.
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
			rtb_Clip.SelectionStart = 0;
		}

		/// <summary>
		/// Overrides the <c>FormClosing</c> eventhandler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_f.CloseClipEditor();

			_init = true;
			WindowState = FormWindowState.Normal;
			_x = Math.Max(0, Left);
			_y = Math.Max(0, Top);

			base.OnFormClosing(e);
		}

		/// <summary>
		/// Overrides the <c>Resize</c> eventhandler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e); // before cursor shenanigans

			if (!_init)
			{
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

				if (WindowState != FormWindowState.Minimized
					&& !(Maximized = WindowState == FormWindowState.Maximized))
				{
					// coding for .net is inelegant ... but I try.
					// Imagine a figure skater doing a triple-axial and flying into the boards.

					_w = ClientSize.Width;
					_h = ClientSize.Height;
				}
			}
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
				e.Handled = e.SuppressKeyPress = true;
				Close();
			}
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
