using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog for cell-text entry.
	/// </summary>
	sealed partial class TextInputDialog
		: YataDialog
	{
		#region Fields (static)
		const string HEAD = "Apply sets the copy cell text and applies it to selected cells.";
		#endregion Fields (static)


		#region Fields
		bool _readyTextchanged;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"><c><see cref="YataForm"/></c></param>
		internal TextInputDialog(YataForm f)
		{
			_f = f;

			InitializeComponent();
			Initialize(YataDialog.METRIC_FUL);

			la_Head.Text = HEAD;

			tb_Input.Text = (_f as YataForm)._copytext[0,0];


			tb_Input.Select();
		}
		#endregion cTor


		#region Handlers
		/// <summary>
		/// Handles a click on the Okay button.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Okay"/></c></param>
		/// <param name="e"></param>
		void click_Okay(object sender, EventArgs e)
		{
			if (YataGrid.CheckTextEdit(tb_Input))
			{
				_readyTextchanged = true;
				la_Head.ForeColor = Color.Firebrick;
				la_Head.Text = "The text has changed.";
			}
			else
			{
				(_f as YataForm)._copytext = new string[,] {{ tb_Input.Text }};
				DialogResult = DialogResult.OK;
			}
		}

		/// <summary>
		/// Clears warn-state when user inputs text.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Input"/></c></param>
		/// <param name="e"></param>
		void textchanged_Input(object sender, EventArgs e)
		{
			if (_readyTextchanged)
			{
				_readyTextchanged = false;
				la_Head.ForeColor = SystemColors.WindowText;
				la_Head.Text = HEAD;
			}
		}
		#endregion Handlers
	}
}


