using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="InfoInputSpells"><c>InfoInputSpells</c></seealso>
	/// <seealso cref="InfoInputFeat"><c>InfoInputFeat</c></seealso>
	sealed partial class InfoInputClasses
		: Form
	{
		#region Fields (static)
		internal const int PrimaryAbil                 = 40; // col in Classes.2da ->
		internal const int SpellAbil                   = 41;
		internal const int AlignRestrict               = 42;
		internal const int AlignRstrctType             = 43;
		internal const int Package                     = 74;
		#endregion Fields (static)


		#region Fields
		Yata _f;
		Cell _cell;

		/// <summary>
		/// <c>true</c> bypasses the <c>CheckedChanged</c> handler for
		/// <c>CheckBoxes</c> and the <c>SelectedIndexChanged</c> handler for
		/// the <c>ComboBox</c>.
		/// </summary>
		/// <remarks>Initialization will configure this dialog without invoking
		/// the handlers.</remarks>
		bool _init;

		CheckBox _cb;
		#endregion Fields


		#region cTor
		/// <summary>
		/// A dialog for the user to input <c>Classes.2da</c> info.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="cell"></param>
		internal InfoInputClasses(Yata f, Cell cell)
		{
			_f    = f;
			_cell = cell;

			InitializeComponent();

			// NOTE: Don't bother inheriting from YataDialog since setting the
			// font is the only benefit ->
			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			init();
		}
		#endregion cTor


		#region init
		/// <summary>
		/// Initializes the dialog based on the current 2da col.
		/// </summary>
		void init()
		{
			_init = true;

			string val = _cell.text;

			int result;
			switch (_cell.x)
			{
				case PrimaryAbil: // string-val,checkbox,unique
				case SpellAbil:
					_f.str0 = _f.str1 = val;
					prep_Abilities();

					switch (val.ToUpperInvariant())
					{
						case "STR": cb_00.Checked = true; break;
						case "CON": cb_01.Checked = true; break;
						case "DEX": cb_02.Checked = true; break;
						case "INT": cb_03.Checked = true; break;
						case "WIS": cb_04.Checked = true; break;
						case "CHA": cb_05.Checked = true; break;

						case gs.Stars: break;

						default: _f.str1 = gs.Stars; break;
					}
					btn_Clear.Enabled = ((lbl_Val.Text = _f.str1) != gs.Stars);
					break;

				case AlignRestrict: // int-val(hex),checkbox,multiple
					// NOTE: Types that bitwise multiple values shall assign
					// a default value of "0x00" instead of the usual "****"
					// when the value passed into this dialog is considered
					// invalid.

					prep_AlignmentRestrictions();

					if (val.Length > 2 && val.Substring(0,2) == "0x"
						&& Int32.TryParse(val.Substring(2),
										  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
										  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
										  out result))
					{
						cb_00.Checked = ((result & Yata.ALIGNRESTRICT_NEUTRAL) != 0);
						cb_01.Checked = ((result & Yata.ALIGNRESTRICT_LAWFUL)  != 0);
						cb_02.Checked = ((result & Yata.ALIGNRESTRICT_CHAOTIC) != 0);
						cb_03.Checked = ((result & Yata.ALIGNRESTRICT_GOOD)    != 0);
						cb_04.Checked = ((result & Yata.ALIGNRESTRICT_EVIL)    != 0);

						if ((result & ~Yata.ALIGNRESTRICT_TOTAL) != 0) // invalid - bits outside allowed range
						{
							_f.int0 = result;
							printHexString(_f.int1 = (result &= Yata.ALIGNRESTRICT_TOTAL));
						}
						else
							printHexString(_f.int0 = _f.int1 = result);

						btn_Clear.Enabled = (result != 0);
					}
					else // is not a valid hex-value ->
					{
						_f.int0 = Yata.II_INIT_INVALID;
						printHexString(_f.int1 = 0);
						btn_Clear.Enabled = false;
					}
					break;

				case AlignRstrctType: // int-val(hex),checkbox,multiple
					// NOTE: Types that bitwise multiple values shall assign
					// a default value of "0x00" instead of the usual "****"
					// when the value passed into this dialog is considered
					// invalid.

					prep_AlignmentRestrictionType();

					if (val.Length > 2 && val.Substring(0,2) == "0x"
						&& Int32.TryParse(val.Substring(2),
										  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
										  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
										  out result))
					{
						cb_00.Checked = ((result & Yata.ALIGNRESTRICTTYPE_LAWCHAOS) != 0);
						cb_01.Checked = ((result & Yata.ALIGNRESTRICTTYPE_GOODEVIL) != 0);

						if ((result & ~Yata.ALIGNRESTRICTTYPE_TOTAL) != 0) // invalid - bits outside allowed range
						{
							_f.int0 = result;
							printHexString(_f.int1 = (result &= Yata.ALIGNRESTRICTTYPE_TOTAL));
						}
						else
							printHexString(_f.int0 = _f.int1 = result);

						btn_Clear.Enabled = (result != 0);
					}
					else // is not a valid hex-value ->
					{
						_f.int0 = Yata.II_INIT_INVALID;
						printHexString(_f.int1 = 0);
						btn_Clear.Enabled = false;
					}
					break;

				case Package: // int-val,dropdown,unique
					list_Packages();

					initintvals(val);
					break;
			}
			_init = false;
		}


		/// <summary>
		/// Prepares this dialog for <c><see cref="PrimaryAbil"/></c> or
		/// <c><see cref="SpellAbil"/></c> input.
		/// </summary>
		void prep_Abilities()
		{
			if (_cell.x == PrimaryAbil) Text = " PrimaryAbil";
			else                        Text = " SpellAbil"; // _cell.x == SpellAbil

			cb_00.Text = "STR";
			cb_01.Text = "CON";
			cb_02.Text = "DEX";
			cb_03.Text = "INT";
			cb_04.Text = "WIS";
			cb_05.Text = "CHA";

			cb_00.Visible = cb_01.Visible = cb_02.Visible =
			cb_03.Visible = cb_04.Visible = cb_05.Visible = true;
		}

		/// <summary>
		/// Prepares this dialog for <c><see cref="AlignRestrict"/></c> input.
		/// </summary>
		void prep_AlignmentRestrictions()
		{
			Text = " AlignRestrict";

			cb_00.Text = "(1)neutral";
			cb_01.Text = "(2)lawful";
			cb_02.Text = "(4)chaotic";
			cb_03.Text = "(8)good";
			cb_04.Text = "(16)evil";

			cb_00.Visible = cb_01.Visible = cb_02.Visible =
			cb_03.Visible = cb_04.Visible = true;
		}

		/// <summary>
		/// Prepares this dialog for <c><see cref="AlignRstrctType"/></c> input.
		/// </summary>
		void prep_AlignmentRestrictionType()
		{
			Text = " AlignRstrctType";

			cb_00.Text = "(1)neutral as LawChaos";
			cb_01.Text = "(2)neutral as GoodEvil)";

			cb_00.Visible = cb_01.Visible = true;
		}


		/// <summary>
		/// Hides the label and shows the <c>ComboBox</c> for dropdown-lists
		/// instead.
		/// </summary>
		void dropdown()
		{
			lbl_Val.Visible = false;
			cbx_Val.Visible = true;

			ClientSize = new Size(ClientSize.Width,
								  ClientSize.Height - 120);
		}

		/// <summary>
		/// Adds allowable entries for "Packages" (Packages.2da) to the
		/// <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_Packages()
		{
			Text = " Package";

			dropdown();

			for (int i = 0; i != Info.packageLabels.Count; ++i)
			{
				cbx_Val.Items.Add(new tui(i + " - " + Info.packageLabels[i]));
			}
			cbx_Val.Items.Add(new tui(gs.Stars));
		}


		/// <summary>
		/// Selects an entry in the <c>ComboBox</c> and preps the int-vals in
		/// Yata to deal with user-input.
		/// 
		/// 
		/// - duplicates <c><see cref="InfoInputSpells"/>.initintvals()</c>
		/// 
		/// - duplicates <c><see cref="InfoInputFeat"/>.initintvals()</c>
		/// </summary>
		/// <param name="val"></param>
		void initintvals(string val)
		{
			int result;
			if (Int32.TryParse(val, out result)
				&& result > -1 && result < cbx_Val.Items.Count - 1)
			{
				cbx_Val.SelectedIndex = _f.int0 = _f.int1 = result;
			}
			else
			{
				btn_Clear.Enabled = false;

				if (val == gs.Stars) _f.int0 = Yata.II_ASSIGN_STARS;
				else                 _f.int0 = Yata.II_INIT_INVALID;

				_f.int1 = Yata.II_ASSIGN_STARS;

				cbx_Val.SelectedIndex = cbx_Val.Items.Count - 1;
			}
		}
		#endregion init


		#region Handlers
		/// <summary>
		/// Handles user changing a checkbox.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void changed_Checkbox(object sender, EventArgs e)
		{
			if (!_init)
			{
				_cb = sender as CheckBox;

				switch (_cell.x)
				{
					case PrimaryAbil:
					case SpellAbil:       change_Ability();         break;
					case AlignRestrict:   change_AlignRestrict();   break;
					case AlignRstrctType: change_AlignRstrctType(); break;
				}
			}
		}

		/// <summary>
		/// - helper for <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>.
		/// </summary>
		void change_Ability()
		{
			clearchecks();

			string val;
			if (_cb.Checked)
			{
				if      (_cb == cb_00) val = "STR";
				else if (_cb == cb_01) val = "CON";
				else if (_cb == cb_02) val = "DEX";
				else if (_cb == cb_03) val = "INT";
				else if (_cb == cb_04) val = "WIS";
				else                   val = "CHA"; // _cb == cb_05
			}
			else
				val = gs.Stars;

			lbl_Val.Text = _f.str1 = val;
			btn_Clear.Enabled = (val != gs.Stars);
		}

		/// <summary>
		/// - helper for <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>.
		/// </summary>
		void change_AlignRestrict()
		{
			if (_cb == cb_00)
			{
				if (_cb.Checked) _f.int1 |=  Yata.ALIGNRESTRICT_NEUTRAL;
				else             _f.int1 &= ~Yata.ALIGNRESTRICT_NEUTRAL;
			}
			else if (_cb == cb_01)
			{
				if (_cb.Checked) _f.int1 |=  Yata.ALIGNRESTRICT_LAWFUL;
				else             _f.int1 &= ~Yata.ALIGNRESTRICT_LAWFUL;
			}
			else if (_cb == cb_02)
			{
				if (_cb.Checked) _f.int1 |=  Yata.ALIGNRESTRICT_CHAOTIC;
				else             _f.int1 &= ~Yata.ALIGNRESTRICT_CHAOTIC;
			}
			else if (_cb == cb_03)
			{
				if (_cb.Checked) _f.int1 |=  Yata.ALIGNRESTRICT_GOOD;
				else             _f.int1 &= ~Yata.ALIGNRESTRICT_GOOD;
			}
			else // _cb == cb_04
			{
				if (_cb.Checked) _f.int1 |=  Yata.ALIGNRESTRICT_EVIL;
				else             _f.int1 &= ~Yata.ALIGNRESTRICT_EVIL;
			}

			printHexString(_f.int1);

			btn_Clear.Enabled = (_f.int1 != 0);
		}

		/// <summary>
		/// - helper for <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>.
		/// </summary>
		void change_AlignRstrctType()
		{
			if (_cb == cb_00)
			{
				if (_cb.Checked) _f.int1 |=  Yata.ALIGNRESTRICTTYPE_LAWCHAOS;
				else             _f.int1 &= ~Yata.ALIGNRESTRICTTYPE_LAWCHAOS;
			}
			else // _cb == cb_01
			{
				if (_cb.Checked) _f.int1 |=  Yata.ALIGNRESTRICTTYPE_GOODEVIL;
				else             _f.int1 &= ~Yata.ALIGNRESTRICTTYPE_GOODEVIL;
			}

			printHexString(_f.int1);

			btn_Clear.Enabled = (_f.int1 != 0);
		}


		/// <summary>
		/// Handles user changing the <c>ComboBox</c> selection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void changed_Combobox(object sender, EventArgs e)
		{
			if (!_init)
			{
				if (cbx_Val.SelectedIndex == cbx_Val.Items.Count - 1)
				{
					btn_Clear.Enabled = false;

					_f.str1 = gs.Stars;
					_f.int1 = Yata.II_ASSIGN_STARS;
				}
				else
				{
					btn_Clear.Enabled = true;

					switch (_cell.x)
					{
						case Package:
							_f.int1 = cbx_Val.SelectedIndex;
							break;
					}
				}
			}
		}


		/// <summary>
		/// Handles clicking the Clear <c>Button</c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Clear(object sender, EventArgs e)
		{
			switch (_cell.x)
			{
				case PrimaryAbil: // str,cb,unique
				case SpellAbil:
					btn_Clear.Enabled = false;

					lbl_Val.Text = _f.str1 = gs.Stars;

					_cb = null;
					clearchecks();
					break;

				case AlignRestrict: // hex,cb,multiple
				case AlignRstrctType:
					btn_Clear.Enabled = false;

					_cb = null;
					clearchecks();

					printHexString(_f.int1 = 0);
					break;

				case Package: // int,dropdown,unique
					cbx_Val.SelectedIndex = cbx_Val.Items.Count - 1; // fire changed_Combobox()
					break;
			}
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Clears all <c>CheckBoxes</c> except the current <c>CheckBox</c>
		/// <c><see cref="_cb"/></c> (if valid).
		/// 
		/// 
		/// - duplicates <c><see cref="InfoInputSpells"/>.clearchecks()</c>
		/// 
		/// - duplicates <c><see cref="InfoInputFeat"/>.clearchecks()</c>
		/// </summary>
		/// <remarks>Set <c>(_cb = null)</c> to clear all <c>Checkboxes</c>.</remarks>
		void clearchecks()
		{
			_init = true;

			CheckBox cb;
			foreach (var control in Controls)
			{
				if ((cb = control as CheckBox) != null
					&& cb.Checked && (_cb == null || cb != _cb))
				{
					cb.Checked = false;
				}
			}
			_init = false;
		}

		/// <summary>
		/// Prints a hex-value at the top of the dialog.
		/// </summary>
		/// <param name="result"></param>
		void printHexString(int result)
		{
			string f;
			if (_cell.x == AlignRestrict) f = "X2";
			else                          f = "X1"; // _cell.x == AlignRstrctType

			lbl_Val.Text = "0x" + result.ToString(f, CultureInfo.InvariantCulture);
		}
		#endregion Methods


		#region Handlers (override)
		/// <summary>
		/// Closes the dialog on [Esc].
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
				Close();
			else
				base.OnKeyDown(e);
		}
		#endregion Handlers (override)
	}
}
