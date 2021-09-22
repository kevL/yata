using System;
using System.Drawing;
using System.Windows.Forms;


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
		int _selc;

		bool _bypasstextchanged;
		bool _cancel;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f">parent <c><see cref="YataForm"/></c></param>
		/// <param name="selc">the currently selected col-id</param>
		internal InputDialogColhead(YataForm f, int selc)
		{
			_f = f;
			_selc = selc;

			InitializeComponent();
			Initialize(YataDialog.METRIC_FUL);

			tb_Input.Text = _text;

			tb_Input.Select();
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			MinimumSize = new Size(Width,          Height);
			MaximumSize = new Size(Int32.MaxValue, Height);

			base.OnLoad(e);
		}

		/// <summary>
		/// Cancels close if <c><see cref="_cancel"/></c> is <c>true</c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (e.Cancel = _cancel)
				_cancel = false;
			else
				base.OnFormClosing(e);
		}
		#endregion Handlers (override)


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

		/// <summary>
		/// Cancels close if the input-text is already taken by another colhead.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Okay(object sender, EventArgs e)
		{
			string[] fields = YataForm.Table.Fields;
			for (int i = 0; i != fields.Length; ++i)
			{
				if ((_selc == -1 || _selc != i + 1)
					&& String.Equals(fields[i],
									 tb_Input.Text,
									 StringComparison.OrdinalIgnoreCase))
				{
					using (var ib = new Infobox(gs.InfoboxTitle_error,
												"That label is already used by another colhead.",
												null,
												InfoboxType.Error))
					{
						ib.ShowDialog(this);
					}
					_cancel = true;
				}
			}
		}
		#endregion Handlers
	}
}
