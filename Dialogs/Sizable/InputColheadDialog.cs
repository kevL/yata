using System;


namespace yata
{
	/// <summary>
	/// A dialog for colhead-text entry.
	/// </summary>
	sealed partial class InputDialogColhead
		: YataDialog
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
		/// <param name="f">parent <c><see cref="YataForm"/></c></param>
		internal InputDialogColhead(YataForm f)
		{
			_f = f;

			InitializeComponent();
			Initialize(YataDialog.METRIC_FUL);

			tb_Input.Text = _text;

			tb_Input.Select();
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


