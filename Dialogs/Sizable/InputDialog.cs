using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog for text entry.
	/// </summary>
	/// <remarks>Used for default-value as well as colhead labels.</remarks>
	sealed partial class InputDialog
		: YataDialog
	{
		#region Fields (static)
		internal static string _colabel    = String.Empty;
		internal static string _defaultval = String.Empty;

		const int MIN_w = 275;

		const int hCheckbox = 22;
		const int hCbPad    =  1;

		/// <summary>
		/// This <c>InputDialog</c> is configured to deal with input for the
		/// Default value text in a 2da-file when <c><see cref="_selc"/></c> is
		/// set to <c>DEFVAL</c>. This <c>InputDialog</c> is configured to deal
		/// with input for colhead-label text when <c>_selc</c> is any other
		/// value.
		/// </summary>
		const int DEFVAL = -2;
		#endregion Fields (static)


		#region Fields
		/// <summary>
		/// The col-id iff not set to <c><see cref="DEFVAL"/></c>. Can be
		/// <c>-1</c>.
		/// </summary>
		/// <remarks>This <c>InputDialog</c> is configured to deal with input
		/// for the Default value text in a 2da-file when <c>_selc</c> is set to
		/// <c>DEFVAL</c>. This <c>InputDialog</c> is configured to deal with
		/// input for colhead-label text when <c>_selc</c> is any other value.</remarks>
		int _selc;

		bool _bypasstextchanged;
		bool _cancel;

		CheckBox cb_Punctuation;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f">parent <c><see cref="YataForm"/></c></param>
		/// <param name="selc">the currently selected col-id; default -2 enables
		/// defaultval input</param>
		internal InputDialog(YataForm f, int selc = DEFVAL)
		{
			_f = f;
			_selc = selc;

			InitializeComponent();
			Initialize(YataDialog.METRIC_FUL);

			if (_selc != DEFVAL)
			{
				Text = " yata - Colhead text";
				tb_Input.Text = _colabel;

				if (Settings._strict) // show 'allow extended punctuation' toggle ->
				{
					cb_Punctuation = new CheckBox();
					cb_Punctuation.Text = "accept punctuation";
					cb_Punctuation.TextAlign = ContentAlignment.BottomLeft;
					cb_Punctuation.Padding = new Padding(4,0,0,0);
					cb_Punctuation.Size = new Size(100, hCheckbox);
					cb_Punctuation.Dock = DockStyle.Top;
					cb_Punctuation.TabIndex = 1;
					cb_Punctuation.Checked = true;
					cb_Punctuation.CheckedChanged += checkedchanged_Punctuation;

					Controls.Add(cb_Punctuation);
					cb_Punctuation.BringToFront();

					ClientSize = new Size(ClientSize.Width, ClientSize.Height + hCheckbox + hCbPad);

					cb_Punctuation.Checked = false; // clear disallowed punctuation at start.
				}
			}
			else
			{
				Text = " yata - Default value";
				tb_Input.Text = _defaultval;
			}

			MinimumSize = new Size(MIN_w,          Height);
			MaximumSize = new Size(Int32.MaxValue, Height);


			tb_Input.Select();
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Cancels close if <c><see cref="_cancel"/></c> is <c>true</c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (e.Cancel = _cancel)
				_cancel = false;
			else
			{
				if (_selc != DEFVAL && Settings._strict)
					ClientSize = new Size(ClientSize.Width, ClientSize.Height - hCheckbox - hCbPad);

				base.OnFormClosing(e);
			}
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Handles <c>TextChanged</c> for the TextBox.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Input"/></c></param>
		/// <param name="e"></param>
		void textchanged_Input(object sender, EventArgs e)
		{
			if (!_bypasstextchanged)
			{
				if (_selc != DEFVAL) // colhead
				{
					char character;
					for (int i = 0; i != tb_Input.Text.Length; ++i)
					{
						if ((character = tb_Input.Text[i]) == 32
							|| !(Util.isAsciiAlphanumericOrUnderscore(character)
								|| ((cb_Punctuation == null || cb_Punctuation.Checked)
									&& Util.isPrintableAsciiNotDoublequote(character))))
						{
							_bypasstextchanged = true;
							tb_Input.Text = _colabel; // recurse
							_bypasstextchanged = false;

							tb_Input.SelectionStart = tb_Input.Text.Length;
							return;
						}
					}
				}

				if (_selc != DEFVAL)
					_colabel = tb_Input.Text;
				else
					_defaultval = tb_Input.Text;
			}
		}

		/// <summary>
		/// Clears punctuation from the textbox if user dechecks
		/// <c><see cref="cb_Punctuation"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="cb_Punctuation"/></c></param>
		/// <param name="e"></param>
		void checkedchanged_Punctuation(object sender, EventArgs e)
		{
			if (!cb_Punctuation.Checked)
			{
				var sb = new StringBuilder();

				for (int i = 0; i != tb_Input.Text.Length; ++i)
				if (Util.isAsciiAlphanumericOrUnderscore(tb_Input.Text[i]))
					sb.Append(tb_Input.Text[i]);

				string text = sb.ToString();
				if (text != tb_Input.Text)
				{
					_bypasstextchanged = true;

					_colabel = (tb_Input.Text = text);
					tb_Input.SelectionStart = tb_Input.Text.Length;

					_bypasstextchanged = false;
				}
			}
		}

		/// <summary>
		/// 1. colhead: Cancels close if the input-text is already taken by
		/// another colhead.
		/// 
		/// 2. defaultval:
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Okay"/></c></param>
		/// <param name="e"></param>
		void click_Okay(object sender, EventArgs e)
		{
			if (_selc != DEFVAL) // colhead
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
				string val = tb_Input.Text;
				if (!SpellcheckDefaultval(ref val))
				{
					using (var ib = new Infobox(Infobox.Title_warni,
												"The text has changed.",
												null,
												InfoboxType.Warn))
					{
						ib.ShowDialog(this);
					}
					tb_Input.Text = val;
					_cancel = true;
				}
			}
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Ensures that a Default value is okay.
		/// </summary>
		/// <param name="val">ref to the default-value to check</param>
		/// <param name="forceQuotes"><c>true</c> to assign two double-quotes if
		/// the <c>string</c> is blanked (used by the load routine)</param>
		/// <returns><c>true</c> if <paramref name="val"/> is correct as-is</returns>
		internal static bool SpellcheckDefaultval(ref string val, bool forceQuotes = false)
		{
			string val0 = val.Trim();

			if (val0 != "\"\"")
			{
				if (val0.StartsWith("\"", StringComparison.Ordinal))
					val0 = val0.Substring(1);

				if (val0.EndsWith("\"", StringComparison.Ordinal))
					val0 = val0.Substring(0, val0.Length - 1);

				val0 = val0.Replace("\"", String.Empty);
			}

			if (val0.Length != 0)
			{
				for (int c = 0; c != val0.Length; ++c)
				if (Char.IsWhiteSpace(val0[c]))
				{
					val0 = "\"" + val0 + "\"";
					break;
				}
			}
			else if (forceQuotes)
			{
				val0 = "\"\"";
			}
			else
				val0 = String.Empty;


			if (val0 == val) return true;

			val = val0;
			return false;
		}
		#endregion Methods
	}
}
