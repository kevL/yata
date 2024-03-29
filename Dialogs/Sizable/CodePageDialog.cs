﻿using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed partial class CodePageDialog
		: YataDialog
	{
		#region Fields
		CodePageList _cpList;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal CodePageDialog(Yata f, Encoding enc)
		{
			_f = f;

			InitializeComponent();
			Initialize(METRIC_FUL);

			string head = "The 2da file appears to have Extended ASCII encoding."
						+ " Please enter the codepage of its text.";
			if (enc == null)
			{
				head += Environment.NewLine + Environment.NewLine
					  + "The codepage specified in Settings.Cfg is invalid.";

				enc = Encoding.GetEncoding(0);
			}
			la_Head.Text = head;

			_pre = enc.CodePage;
			tb_Codepage.Text = enc.CodePage.ToString(CultureInfo.InvariantCulture);

			bu_Accept.Select();
		}
		// "The text encoding of the 2da file could not be determined. It
		// appears to contain characters that .NET cannot interpret accurately.
		// So my guess is that it was saved with an extended-ASCII
		// codepage/characterset. To prevent unrecognized characters getting
		// replaced by � ensure that your 2da files are encoded in UTF-8 or
		// ASCII before opening them in Yata."
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Closes the <c><see cref="CodePageList"/></c>
		/// <c><see cref="_cpList"/></c> when this <c>CodePageDialog</c> closes.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>reasons ...</remarks>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (_cpList != null)
				_cpList.Close();

			base.OnFormClosing(e);
		}
		#endregion Handlers (override)


		#region Handlers
		int _pre;

		/// <summary>
		/// Handles textchanged in the Codepage textbox.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Codepage"/></c></param>
		/// <param name="e"></param>
		void tb_Codepage_textchanged(object sender, EventArgs e)
		{
			Encoding enc = null;

			if (tb_Codepage.Text.Length == 0)
			{
				la_CodepageInfo.Text = String.Empty;
				_pre = 0;
			}
			else if (tb_Codepage.Text.StartsWith("0", StringComparison.Ordinal))
			{
				tb_Codepage.Text = tb_Codepage.Text.Substring(1); // recurse
				tb_Codepage.SelectionStart = tb_Codepage.Text.Length;
			}
			else
			{
				int result;
				if (!Int32.TryParse(tb_Codepage.Text, out result)
					|| result < 0 || result > 65535)
				{
					tb_Codepage.Text = _pre.ToString(CultureInfo.InvariantCulture); // recurse
					tb_Codepage.SelectionStart = tb_Codepage.Text.Length;
				}
				else if (YataGrid.CheckCodepage(_pre = result))
				{
					enc = Encoding.GetEncoding(_pre);

					la_CodepageInfo.ForeColor = SystemColors.ControlText;
					la_CodepageInfo.Text = enc.HeaderName   + Environment.NewLine
										 + enc.EncodingName + Environment.NewLine
										 + enc.CodePage;
				}
				else
				{
					la_CodepageInfo.ForeColor = Color.Firebrick;
					la_CodepageInfo.Text = "invalid Codepage";
				}
			}
			bu_Accept.Enabled = (enc != null);
		}


		/// <summary>
		/// Sets the codepage to the user-machine's default characterset.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Default"/></c></param>
		/// <param name="e"></param>
		void bu_Default_click(object sender, EventArgs e)
		{
			tb_Codepage.Text = Encoding.GetEncoding(0).CodePage.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Sets the codepage to UTF-8.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Utf8"/></c></param>
		/// <param name="e"></param>
		void bu_Utf8_click(object sender, EventArgs e)
		{
			tb_Codepage.Text = Encoding.UTF8.CodePage.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Sets the codepage to the value that the user specified in
		/// Settings.Cfg.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Custom"/></c></param>
		/// <param name="e"></param>
		void bu_Custom_click(object sender, EventArgs e)
		{
			tb_Codepage.Text = Options._codepage.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Invokes or closes the <c><see cref="CodePageList"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_List"/></c></param>
		/// <param name="e"></param>
		void bu_List_click(object sender, EventArgs e)
		{
			if (_cpList == null)
				_cpList = new CodePageList(this);
			else if (_cpList.WindowState == FormWindowState.Minimized)
				_cpList.WindowState = FormWindowState.Normal;
			else
				_cpList.Close();
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Gets the desired codepage as a string.
		/// </summary>
		/// <returns></returns>
		internal string GetCodePage()
		{
			return tb_Codepage.Text;
		}

		/// <summary>
		/// Nulls <c><see cref="_cpList"/></c> when the
		/// <c><see cref="CodePageList"/></c> closes.
		/// </summary>
		internal void CloseCodepageList()
		{
			_cpList = null;
		}
		#endregion Methods
	}
}
