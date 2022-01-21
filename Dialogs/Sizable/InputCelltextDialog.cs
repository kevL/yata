using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog for cell-text entry.
	/// </summary>
	sealed partial class InputCelltextDialog
		: YataDialog
	{
		#region Fields (static)
		const string HEAD = "Apply sets the copy-cell buffer and applies it to all selected cells.";

		const int w_Min = 300;
		#endregion Fields (static)


		#region Fields
		bool _readyTextchanged;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"><c><see cref="YataForm"/></c></param>
		internal InputCelltextDialog(YataForm f)
		{
			_f = f;

			InitializeComponent();
			Initialize(YataDialog.METRIC_FUL);

			la_Head.Text = HEAD;

			tb_Input.Text = (_f as YataForm)._copytext[0,0];

			MinimumSize = new Size(w_Min,          Height);
			MaximumSize = new Size(Int32.MaxValue, Height);


			tb_Input.Select();
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Closes this <c>InputCelltextDialog</c> on <c>[F11]</c>.
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
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Handles a click on the Okay button.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Okay"/></c></param>
		/// <param name="e"></param>
		void click_Okay(object sender, EventArgs e)
		{
			if (YataGrid.VerifyText_edit(tb_Input))
			{
				_readyTextchanged = true;
				la_Head.ForeColor = Color.Firebrick;
				la_Head.Text = "The text has changed.";
			}
			else
			{
				var f = _f as YataForm;

				f._copytext = new string[,] {{ tb_Input.Text }};

				if (f._fclip != null)
					f._fclip.SetCellsBufferText();

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


