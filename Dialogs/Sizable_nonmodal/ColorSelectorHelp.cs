using System;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ColorSelectorHelp
		: YataDialog
	{
		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"></param>
		internal ColorSelectorHelp(ColorSelectorDialog f)
		{
			_f = f;
			_cascade = true;

			InitializeComponent();
			Initialize(METRIC_FUL, true, false, true);

			// https://stackoverflow.com/questions/17284573/rich-text-box-bold#answer-17284741
			// http://www.pindari.com/rtf1.html
			var sb = new StringBuilder();

			sb.Append(@"{\rtf1\ansi ");
			sb.Append(@"MouseClick/Drag adjusts the Hue/Saturation and Value parameters.\line\line ");

			sb.Append(@"   \b - COLORFIELD -\b0\line\line ");

			sb.Append(@"[Left]\tab\tab decreases Hue\line ");
			sb.Append(@"[Right]\tab increases Hue\line ");
			sb.Append(@"[Up]\tab\tab decreases Saturation\line ");
			sb.Append(@"[Down]\tab\tab increases Saturation\line ");
			sb.Append(@"[Home]\tab\tab decreases Hue + decreases Saturation\line ");
			sb.Append(@"[End]\tab\tab decreases Hue + increases Saturation\line ");
			sb.Append(@"[PageUp]\tab increases Hue + decreases Saturation\line ");
			sb.Append(@"[PageDown]\tab increases Hue + increases Saturation\line\line ");

			sb.Append(@"   \b - VALUEFIELD -\b0\line\line ");

			sb.Append(@"[Subtract]\tab decreases Value\line ");
			sb.Append(@"[Add]\tab\tab increases Value\line\line ");

			sb.Append(@"MouseWheel adjusts Value if no textbox/combobox has focus.\line ");
			sb.Append(@"[Shift]+MouseWheel steps Value by 10.\line\line ");

			sb.Append(@"   \b - RGB TextBox focused -\b0\line\line ");

			sb.Append(@"[Subtract]\tab increases the byte\line ");
			sb.Append(@"[Add]\tab\tab decreases the byte\line ");
			sb.Append(@"[PageUp]\tab sets the byte to 255\line ");
			sb.Append(@"[PageDown]\tab sets the byte to 0\line ");
			sb.Append(@"[Escape]\line ");
			sb.Append(@"[Enter]\tab switches focus to the Accept button\line\line ");

			sb.Append(@"MouseWheel adjusts an RGB byte.\line ");
			sb.Append(@"[Shift]+MouseWheel steps the byte by 10.\line\line ");

			sb.Append(@"MouseClick an RGB Label to switch focus to the Accept button.\line\line ");

			sb.Append(@"   \b - COLORS ComboBox focused -\b0\line\line ");

			sb.Append(@"[Escape]\tab switches focus to the Accept button");

//			sb.Append(@"Otherwise the mouse and keyboard have standard .NET behaviors.");
			sb.Append(@"}");

			rt_Help.Rtf = sb.ToString();

			Show(_f); // ColorSelectorDialog is owner.
		}
		#endregion cTor


		#region handlers (override)
		/// <summary>
		/// Overrides this dialog's <c>FormClosing</c> handler. Sets the static
		/// location and nulls
		/// <c><see cref="ColorSelectorDialog._fhelp">ColorSelectorDialog._fhelp</see></c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			(_f as ColorSelectorDialog)._fhelp = null;
			base.OnFormClosing(e);
		}

		/// <summary>
		/// Closes this <c>ColorSelectorHelp</c> on <c>[Enter]</c> or
		/// <c>[Escape]</c>.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Enter:
				case Keys.Escape:
					Close();
					return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// <summary>
		/// Clears and resets the <c>ScrollBars</c> type for
		/// <c><see cref="rt_Help"/></c> when this
		/// <c><see cref="YataDialog"/></c> is resized.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>This fixes a glitch or two with <c>RichTextBox</c> by
		/// clearing and resetting the <c>ScrollBars</c>.</remarks>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			rt_Help.ScrollBars = RichTextBoxScrollBars.None;
			rt_Help.ScrollBars = RichTextBoxScrollBars.Vertical;
		}
		#endregion handlers (override)
	}
}
