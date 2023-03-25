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

			tb_Help.Text = "- COLORFIELD -"
						 + Environment.NewLine + Environment.NewLine
						 + "[Left] decreases Hue" + Environment.NewLine
						 + "[Right] increases Hue" + Environment.NewLine
						 + "[Up] decreases Saturation" + Environment.NewLine
						 + "[Down] increases Saturation" + Environment.NewLine
						 + "[Home] decreases Hue + decreases Saturation" + Environment.NewLine
						 + "[End] decreases Hue + increases Saturation" + Environment.NewLine
						 + "[PageUp] increases Hue + decreases Saturation" + Environment.NewLine
						 + "[PageDown] increases Hue + increases Saturation"
						 + Environment.NewLine + Environment.NewLine
						 + "- VALUEFIELD -"
						 + Environment.NewLine + Environment.NewLine
						 + "[Subtract] decreases Value" + Environment.NewLine
						 + "[Add] increases Value"
						 + Environment.NewLine + Environment.NewLine
						 + "MouseWheel increases or decreases Value if no textbox/combobox has focus." + Environment.NewLine
						 + "MouseClick and MouseDrag adjust the fields."
						 + Environment.NewLine + Environment.NewLine
						 + "- RGB focused -"
						 + Environment.NewLine + Environment.NewLine
						 + "[Subtract] increases the byte" + Environment.NewLine
						 + "[Add] decreases the byte" + Environment.NewLine
						 + "[PageUp] sets the byte to 255" + Environment.NewLine
						 + "[PageDown] sets the byte to 0" + Environment.NewLine
						 + "[Escape] or [Enter] switches focus to the Accept button"
						 + Environment.NewLine + Environment.NewLine
						 + "MouseWheel increases or decreases a focused RGB byte."
						 + Environment.NewLine + Environment.NewLine
						 + "- COLORS focused -"
						 + Environment.NewLine + Environment.NewLine
						 + "[Escape] switches focus to the Accept button"
						 + Environment.NewLine + Environment.NewLine
						 + "MouseClick on an RGB label switches focus to the Accept button."
						 + Environment.NewLine + Environment.NewLine
						 + "Otherwise the keyboard and mouse behave normally for .NET controls.";

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
