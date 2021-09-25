using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog for text entry.
	/// </summary>
	sealed partial class InputDialogColhead
		: YataDialog
	{
		#region Fields (static)
		internal static string _text           = String.Empty;
		internal static string _textdefaultval = String.Empty;
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
		/// <param name="selc">the currently selected col-id; default -2 enables
		/// defaultval input</param>
		internal InputDialogColhead(YataForm f, int selc = -2)
		{
			_f = f;
			_selc = selc;

			InitializeComponent();
			Initialize(YataDialog.METRIC_FUL);

			if (_selc != -2)
			{
				Text = " yata - Colhead text";
				tb_Input.Text = _text;
			}
			else
			{
				Text = " yata - Default value";
				tb_Input.Text = _textdefaultval;
			}


			tb_Input.Select();
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Sets min/max sizes.
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
		/// Handles <c>TextChanged</c> for the TextBox.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void textchanged_Input(object sender, EventArgs e)
		{
			if (!_bypasstextchanged)
			{
				if (tb_Input.Text.Length != 0)
				{
					if (_selc != -2) // colhead
					{
						char character;
						for (int i = 0; i != tb_Input.Text.Length; ++i)
						{
							character = tb_Input.Text[i];
							if (character == 34 // double-quote
								|| (    (Settings._strict && !Util.isAsciiAlphanumericOrUnderscore(character))
									|| (!Settings._strict && !Util.isPrintableAsciiNotDoublequote( character))))
							{
								_bypasstextchanged = true;
								tb_Input.Text = _text; // recurse
								_bypasstextchanged = false;

								tb_Input.SelectionStart = tb_Input.Text.Length;
								return;
							}
						}
					}
				}

				if (_selc != -2)
					_text = tb_Input.Text;
				else
					_textdefaultval = tb_Input.Text;
			}
		}

		/// <summary>
		/// 1. colhead: Cancels close if the input-text is already taken by
		/// another colhead.
		/// 
		/// 2. defaultval:
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Okay(object sender, EventArgs e)
		{
			if (_selc != -2) // colhead
			{
				string[] fields = YataForm.Table.Fields;
				for (int i = 0; i != fields.Length; ++i)
				{
					if ((_selc == -1 || _selc != i + 1)
						&& String.Equals(fields[i],
										 tb_Input.Text,
										 StringComparison.OrdinalIgnoreCase))
					{
						using (var ib = new Infobox(Infobox.Title_error,
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
			else // defaultval
			{
				if (YataGrid.CheckTextEdit(tb_Input))
				{
					using (var ib = new Infobox(Infobox.Title_warni,
												"The text has changed.",
												null,
												InfoboxType.Warn))
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
