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
		#region Enumbs
		internal enum Idtype
		{
			non, defval, createhead, relabelhead
		}
		#endregion Enumbs

		#region Fields (static)
		internal static string _colabel    = String.Empty;
		internal static string _defaultval = String.Empty;

		const int MIN_w = 275;

		const int hCheckbox = 20;
		#endregion Fields (static)


		#region Fields
		Idtype _type;

		/// <summary>
		/// The col-id iff <c><see cref="_type"/></c> is
		/// <c><see cref="Idtype.createhead"/></c> or
		/// <c><see cref="Idtype.relabelhead"/></c>.
		/// </summary>
		/// <remarks> Can be <c>-1</c>.</remarks>
		int _selc;

		bool _bypasstextchanged;
		bool _cancel;

		CheckBox cb_Punctuation;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f">parent <c><see cref="Yata"/></c></param>
		/// <param name="type">the <c><see cref="Idtype"/></c> of this
		/// <c>InputDialog</c>
		/// <list type="bullet">
		/// <item><c><see cref="Idtype.defval"/></c></item>
		/// <item><c><see cref="Idtype.createhead"/></c></item>
		/// <item><c><see cref="Idtype.relabelhead"/></c></item>
		/// </list></param>
		/// <param name="selc">the currently selected col-id if
		/// <paramref name="type"/> is <c><see cref="Idtype.createhead"/></c> or
		/// <c><see cref="Idtype.relabelhead"/></c></param>
		internal InputDialog(Yata f, Idtype type, int selc = -1)
		{
			_f = f;
			_selc = selc;

			InitializeComponent();
			Initialize(METRIC_FUL);

			switch (_type = type)
			{
				case Idtype.defval:
					Text = " yata - Default value";
					tb_Input.Text = _defaultval;
					break;

				case Idtype.createhead:
					Text = " yata - Create colhead";
					goto case Idtype.non;

				case Idtype.relabelhead:
					Text = " yata - Relabel colhead";
					tb_Input.Text = _colabel;
					goto case Idtype.non;

				case Idtype.non: // IMPORTANT: Do not use this. It's only a fallthrough case.
					if (Settings._strict) // show 'allow extended punctuation' toggle ->
					{
						cb_Punctuation = new CheckBox();
						cb_Punctuation.Text = "accept nonstandard punctuation";
						cb_Punctuation.TextAlign = ContentAlignment.MiddleLeft;
						cb_Punctuation.Padding = new Padding(9,2,0,0);
						cb_Punctuation.Size = new Size(100, hCheckbox);
						cb_Punctuation.Dock = DockStyle.Top;
						cb_Punctuation.TabIndex = 1;
						cb_Punctuation.Checked = true;
						cb_Punctuation.CheckedChanged += checkedchanged_Punctuation;

						Controls.Add(cb_Punctuation);
						cb_Punctuation.BringToFront();

						ClientSize = new Size(ClientSize.Width, ClientSize.Height + hCheckbox);

						cb_Punctuation.Checked = false; // call checkedchanged_Punctuation() at start.
					}
					break;
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
				if (_type != Idtype.defval && Settings._strict)
					ClientSize = new Size(ClientSize.Width, ClientSize.Height - hCheckbox); // conform static telemetry

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
				if (_type != Idtype.defval) // colhead
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

				switch (_type)
				{
					case Idtype.defval:
						_defaultval = tb_Input.Text;
						break;

					case Idtype.createhead:
					case Idtype.relabelhead:
						_colabel = tb_Input.Text;
						break;
				}
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
		/// another colhead. 2. defaultval: Cancels close if the default-value
		/// fails verification.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Okay"/></c></param>
		/// <param name="e"></param>
		void click_Okay(object sender, EventArgs e)
		{
			switch (_type)
			{
				case Idtype.defval:
				{
					string val = tb_Input.Text;
					if (!VerifyDefaultval(ref val))
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
					break;
				}

				case Idtype.createhead:
				case Idtype.relabelhead:
				{
					string[] fields = Yata.Table.Fields;
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
					break;
				}
			}
		}
		#endregion Handlers


		#region Methods (static)
		/// <summary>
		/// Ensures that a Default value is okay.
		/// </summary>
		/// <param name="val">ref to the default-value to check</param>
		/// <param name="forceQuotes"><c>true</c> to assign two double-quotes if
		/// the <c>string</c> is blanked (used by the load routine)</param>
		/// <returns><c>true</c> if <paramref name="val"/> is correct as-is</returns>
		internal static bool VerifyDefaultval(ref string val, bool forceQuotes = false)
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
		#endregion Methods (static)
	}
}
