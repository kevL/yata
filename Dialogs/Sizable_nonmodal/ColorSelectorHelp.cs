using System;
using System.Drawing;
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
			Initialize(METRIC_FUL);

			// https://stackoverflow.com/questions/17284573/rich-text-box-bold#answer-17284741
			// http://www.pindari.com/rtf1.html
			var sb = new StringBuilder();

			sb.Append(@"{\rtf1\ansi ");
			sb.Append(@"MouseClick and MouseDrag change the color and value fields.\line\line ");
			sb.Append(@"   \b - COLORFIELD -\b0\line\line ");
			sb.Append(@"[Left]\tab\tab decreases Hue\line ");
			sb.Append(@"[Right]\tab\tab increases Hue\line ");
			sb.Append(@"[Up]\tab\tab decreases Saturation\line ");
			sb.Append(@"[Down]\tab\tab increases Saturation\line ");
			sb.Append(@"[Home]\tab\tab decreases Hue + decreases Saturation\line ");
			sb.Append(@"[End]\tab\tab decreases Hue + increases Saturation\line ");
			sb.Append(@"[PageUp]\tab increases Hue + decreases Saturation\line ");
			sb.Append(@"[PageDown]\tab increases Hue + increases Saturation\line\line ");
			sb.Append(@"   \b - VALUEFIELD -\b0\line\line ");
			sb.Append(@"[Subtract]\tab decreases Value\line ");
			sb.Append(@"[Add]\tab\tab increases Value\line\line ");
			sb.Append(@"MouseWheel decreases or increases Value if no textbox/combobox has focus.\line\line ");
			sb.Append(@"   \b - RGB TextBox focused -\b0\line\line ");
			sb.Append(@"[Subtract]\tab increments the byte\line ");
			sb.Append(@"[Add]\tab\tab decrements the byte\line ");
			sb.Append(@"[PageUp]\tab sets the byte to 255\line ");
			sb.Append(@"[PageDown]\tab sets the byte to 0\line ");
			sb.Append(@"[Escape]\line ");
			sb.Append(@"[Enter]\tab\tab switches focus to the Accept button\line\line ");
			sb.Append(@"MouseWheel increases or decreases an RGB byte.\line\line ");
			sb.Append(@"MouseClick on an RGB Label switches focus to the Accept button.\line\line ");
			sb.Append(@"   \b - COLORS ComboBox focused -\b0\line\line ");
			sb.Append(@"[Escape]\tab switches focus to the Accept button\line\line ");
			sb.Append(@"Otherwise the mouse and keyboard respect standard .NET behavior.");
			sb.Append(@"}");

			rt_Help.Rtf = sb.ToString();

			rt_Help.BackColor = SystemColors.Control; // go figur. cf. DifferDialog panel/rtb
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
		#endregion handlers (override)
	}
}
