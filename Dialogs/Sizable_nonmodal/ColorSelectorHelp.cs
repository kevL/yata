using System;
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

			InitializeComponent();
			Initialize(METRIC_FUL);

			tb_Help.Text = "- keyboard input -"
						 + Environment.NewLine + Environment.NewLine
						 + "The numpad keys navigate the Colorfield directionally."        + Environment.NewLine
						 + "[Subtract] - decreases the Valuefield"                         + Environment.NewLine
						 + "[Add] increases the Valuefield"
						 + Environment.NewLine + Environment.NewLine
						 + "The RGB textboxes accept special keyboard input when focused." + Environment.NewLine
						 + "[Subtract] increases the textbox value"                        + Environment.NewLine
						 + "[Add] decreases the textbox value"                             + Environment.NewLine
						 + "[PageUp] sets the value to 255"                                + Environment.NewLine
						 + "[PageDown] sets the value to 0"                                + Environment.NewLine
						 + "[Enter] defocuses a textbox"                                   + Environment.NewLine
						 + "[Esc] defocuses a textbox or dropdownbox"
						 + Environment.NewLine + Environment.NewLine
						 + "Otherwise the keyboard and mouse operate normally for .NET controls.";

			Show(_f); // ColorSelectorDialog is owner.
		}
		#endregion cTor


		#region handlers (override)
		/// <summary>
		/// Nulls
		/// <c><see cref="ColorSelectorDialog._fhelp">ColorSelectorDialog._fhelp</see></c>
		/// when this <c>ColorSelectorHelp</c> closes.
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
