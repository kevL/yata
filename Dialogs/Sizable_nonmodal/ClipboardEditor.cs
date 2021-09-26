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

		/// <summary>
		/// Gets the current Windows Clipboard text each time this
		/// <c>ClipboardEditor's</c> <c>Activated</c> event fires.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnActivated(EventArgs e)
		{
			rtb_Clip.Text = ClipboardService.GetText().Trim();
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
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>Get button</item>
		/// <item><c><see cref="YataForm"/>.clipclick_ExportCopy()</c></item>
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
}
