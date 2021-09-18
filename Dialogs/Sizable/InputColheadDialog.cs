using System;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog for colhead-text entry.
	/// </summary>
	sealed partial class InputDialogColhead
		: Form
	{
		#region Fields (static)
		internal static string _text = String.Empty;
		#endregion Fields (static)


		#region Fields
		bool _bypasstextchanged;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal InputDialogColhead()
		{
			InitializeComponent();

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			if (Settings._fontf_tb != null)
			{
				tb_Input.Font.Dispose();
				tb_Input.Font = Settings._fontf_tb;
			}

			tb_Input.BackColor = Colors.TextboxBackground;

			tb_Input.Text = _text;
			tb_Input.SelectionStart = 0;
			tb_Input.SelectionLength = tb_Input.Text.Length;
		}
		#endregion cTor


		#region Handlers
		/// <summary>
		/// Handles text-input in the TextBox.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Col-head text shall be alphanumeric or underscore.</remarks>
		void textchanged_Input(object sender, EventArgs e)
		{
			if (!_bypasstextchanged)
			{
				if (tb_Input.Text.Length != 0)
				{
					for (int i = 0; i != tb_Input.Text.Length; ++i)
					{
						int ascii = tb_Input.Text[i];
						if (ascii != 95
							&& (    ascii < 48
								|| (ascii > 57 && ascii < 65)
								|| (ascii > 90 && ascii < 97)
								||  ascii > 122))
						{
							_bypasstextchanged = true;
							tb_Input.Text = _text; // recurse
							_bypasstextchanged = false;

							tb_Input.SelectionStart = tb_Input.Text.Length;
							return;
						}
					}
				}
				_text = tb_Input.Text;
			}
		}
		#endregion Handlers
	}
}


