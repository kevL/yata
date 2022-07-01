using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed partial class CodePageList
		: YataDialog
	{
		#region cTor
		/// <summary>
		/// A window that lists all codepages that are recognized by the user's
		/// OS.
		/// </summary>
		/// <param name="f"><c><see cref="CodePageDialog"/></c></param>
		/// <remarks>This dialog shall be allowed to remain open even if its
		/// caller <c>CodePageDialog</c> is closed.</remarks>
		internal CodePageList(CodePageDialog f)
		{
			// WARNING: CodePageList will hold a pointer to CodePageDialog even
			// if CodePageDialog is closed. The pointer will be released when
			// CodePageList is closed also.
			_f = f;

			InitializeComponent();
			Initialize(METRIC_FUL);

			var sb = new StringBuilder();

			EncodingInfo[] encs = Encoding.GetEncodings();
			for (int i = 0; i != encs.Length; ++i)
			{
				if (i != 0) sb.AppendLine();

				sb.AppendLine(encs[i].Name);
				sb.AppendLine(encs[i].DisplayName);
				sb.AppendLine(encs[i].CodePage.ToString(CultureInfo.InvariantCulture));
			}
			tb_Codepages.Text = sb.ToString();

			tb_Codepages.Select();
			Show(); // no owner.
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Handles the <c>FormClosing</c> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			(_f as CodePageDialog).CloseCodepageList();
			base.OnFormClosing(e);
		}

		/// <summary>
		/// Closes this <c>CodePageList</c> when <c>[Esc]</c> is pressed.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Do not use <c>KeyDown</c> because the event will repeat
		/// even if told to suppress itself here and
		/// <c><see cref="CodePageDialog"/></c> would then close also.
		/// 
		/// 
		/// Requires <c>KeyPreview</c> <c>true</c>.</remarks>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
				Close();
		}
		#endregion Handlers (override)
	}
}
