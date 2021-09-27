using System;
using System.Drawing;
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
		internal InputDialog(YataForm f, int selc = -2)
		{
			_f = f;
			_selc = selc;

			InitializeComponent();
			Initialize(YataDialog.METRIC_FUL);

			if (_selc != -2)
			{
				Text = " yata - Colhead text";
				tb_Input.Text = _colabel;
			}
			else
			{
				Text = " yata - Default value";
				tb_Input.Text = _defaultval;
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
				if (tb_Input.Text.Length != 0 && _selc != -2) // colhead
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
							tb_Input.Text = _colabel; // recurse
							_bypasstextchanged = false;

							tb_Input.SelectionStart = tb_Input.Text.Length;
							return;
						}
					}
				}

				if (_selc != -2)
					_colabel = tb_Input.Text;
				else
					_defaultval = tb_Input.Text;
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
