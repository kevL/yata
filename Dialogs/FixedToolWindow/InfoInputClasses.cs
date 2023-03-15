using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// An <c><see cref="InfoInputDialog"/></c> to input specific data for
	/// <c>Classes.2da</c>.
	/// </summary>
	/// <seealso cref="InfoInputSpells"><c>InfoInputSpells</c></seealso>
	sealed partial class InfoInputClasses
		: InfoInputDialog
	{
		#region Fields (static)
		// cols in Classes.2da ->
		internal const int icon                       =  6; // ofd only (OpenFileDialog) - lc to not conflict w/ 'Icon'
		internal const int BorderedIcon               =  7; // ofd only
		internal const int HitDie                     =  8;
		internal const int AttackBonusTable           =  9; // ofd only
		internal const int FeatsTable                 = 10; // ofd only
		internal const int SavingThrowTable           = 11; // ofd only
		internal const int SkillsTable                = 12; // ofd only
		internal const int BonusFeatsTable            = 13; // ofd only
		internal const int SpellGainTable             = 15; // ofd only
		internal const int SpellKnownTable            = 16; // ofd only
		internal const int PlayerClass                = 17; // bool
		internal const int SpellCaster                = 18; // bool
		internal const int MetaMagicAllowed           = 19; // bool
		internal const int MemorizesSpells            = 20; // bool
		internal const int HasArcane                  = 21; // bool
		internal const int HasDivine                  = 22; // bool
		internal const int HasSpontaneousSpells       = 23; // bool
		internal const int SpontaneousConversionTable = 24; // ofd only
		internal const int AllSpellsKnown             = 28; // bool
		internal const int HasInfiniteSpells          = 29; // bool
		internal const int HasDomains                 = 30; // bool
		internal const int HasSchool                  = 31; // bool
		internal const int HasFamiliar                = 32; // bool
		internal const int HasAnimalCompanion         = 33; // bool
		internal const int PrimaryAbil                = 40;
		internal const int SpellAbil                  = 41;
		internal const int AlignRestrict              = 42;
		internal const int AlignRstrctType            = 43;
		internal const int InvertRestrict             = 44; // bool
		internal const int PreReqTable                = 66; // ofd only
		internal const int BonusSpellcasterLevelTable = 69; // ofd only
		internal const int BonusCasterFeatByClassMap  = 70; // ofd only
		internal const int Package                    = 74;
		internal const int FEATPracticedSpellcaster   = 75;
		internal const int FEATExtraSlot              = 76;
		internal const int FEATArmoredCaster          = 77;
		internal const int CharGen_Chest              = 81; // ofd only
		internal const int CharGen_Feet               = 82; // ofd only
		internal const int CharGen_Hands              = 83; // ofd only
		internal const int CharGen_Cloak              = 84; // ofd only
		internal const int CharGen_Head               = 85; // ofd only
		#endregion Fields (static)


		#region cTor
		/// <summary>
		/// A dialog for the user to input <c>Classes.2da</c> info.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="cell"></param>
		internal InfoInputClasses(Yata f, Cell cell)
		{
			_f    = f;		// don't try to pass these to a InfoInputDialog.cTor
			_cell = cell;	// because the designer will scream blue murder.

			InitializeComponent();

			Text = pad + Yata.Table.Cols[_cell.x].text;

//			_title = Yata.Table.Cols[_cell.x].text;
//			int border = (Width - ClientSize.Width) / 2;
//			_hTitle = Height - ClientSize.Height - border;
//			_width  = Width;

			// NOTE: Don't bother inheriting from YataDialog since setting the
			// font is the only benefit ->
			if (Options._font2dialog != null)
				Font = Options._font2dialog;
			else
				Font = Options._fontdialog;

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
				case HitDie: // string-val,checkbox,unique
					_f.str0 = _f.str1 = val;
					prep_HitDice();

					switch (val)
					{
						case "4" : cb_00.Checked = true; break;
						case "6" : cb_01.Checked = true; break;
						case "8" : cb_02.Checked = true; break;
						case "10": cb_03.Checked = true; break;
						case "12": cb_04.Checked = true; break;

						case gs.Stars: break;

						default: _f.str1 = gs.Stars; break;
					}
					bu_Clear.Enabled = ((la_Val.Text = _f.str1) != gs.Stars);
					break;

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
					bu_Clear.Enabled = ((la_Val.Text = _f.str1) != gs.Stars);
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

						bu_Clear.Enabled = (result != 0);
					}
					else // is not a valid hex-value ->
					{
						_f.int0 = Yata.Info_INIT_INVALID;
						printHexString(_f.int1 = 0);
						bu_Clear.Enabled = false;
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

						bu_Clear.Enabled = (result != 0);
					}
					else // is not a valid hex-value ->
					{
						_f.int0 = Yata.Info_INIT_INVALID;
						printHexString(_f.int1 = 0);
						bu_Clear.Enabled = false;
					}
					break;

				case Package: // int-val,dropdown,unique
					list_Packages();
					initintvals(val, co_Val, bu_Clear);
					break;

				case PlayerClass: // string-val,checkbox,unique (bools) ->
				case SpellCaster:
				case MetaMagicAllowed:
				case MemorizesSpells:
				case HasArcane:
				case HasDivine:
				case HasSpontaneousSpells:
				case AllSpellsKnown:
				case HasInfiniteSpells:
				case HasDomains:
				case HasSchool:
				case HasFamiliar:
				case HasAnimalCompanion:
				case InvertRestrict:
					_f.str0 = _f.str1 = val;
					prep_bool();

					switch (val)
					{
						case "0": cb_00.Checked = true; break;
						case "1": cb_01.Checked = true; break;

						case gs.Stars: break;

						default: _f.str1 = gs.Stars; break;
					}
					bu_Clear.Enabled = ((la_Val.Text = _f.str1) != gs.Stars);
					break;
			}
			_init = false;
		}


		/// <summary>
		/// Prepares this dialog for <c><see cref="HitDie"/></c> input.
		/// </summary>
		void prep_HitDice()
		{
			cb_00.Text = "d4";
			cb_01.Text = "d6";
			cb_02.Text = "d8";
			cb_03.Text = "d10";
			cb_04.Text = "d12";

			cb_00.Visible = cb_01.Visible = cb_02.Visible =
			cb_03.Visible = cb_04.Visible = true;
		}

		/// <summary>
		/// Prepares this dialog for <c><see cref="PrimaryAbil"/></c> or
		/// <c><see cref="SpellAbil"/></c> input.
		/// </summary>
		void prep_Abilities()
		{
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
			cb_00.Text = "(1)neutral on LawChaos axis";
			cb_01.Text = "(2)neutral on GoodEvil axis";

			cb_00.Visible = cb_01.Visible = true;
		}

		/// <summary>
		/// Prepares this dialog for <c>bool</c> input.
		/// </summary>
		void prep_bool()
		{
			cb_00.Text = "0 - false";
			cb_01.Text = "1 - true";

			cb_00.Visible = cb_01.Visible = true;
		}


		/// <summary>
		/// Hides the label and shows the <c>ComboBox</c> for dropdown-lists
		/// instead.
		/// </summary>
		void dropdown()
		{
			la_Val.Visible = false;
			co_Val.Visible = true;

			ClientSize = new Size(ClientSize.Width,
								  ClientSize.Height - 20 * 6);
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="Package"/></c>
		/// (Packages.2da) to the <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_Packages()
		{
			dropdown();

			for (int i = 0; i != Info.packageLabels.Count; ++i)
			{
				co_Val.Items.Add(new tui(i + " - " + Info.packageLabels[i]));
			}
			co_Val.Items.Add(new tui(gs.Stars));
		}
		#endregion init


		#region Handlers
		/// <summary>
		/// Handles user changing a <c>CheckBox</c>.
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
					case HitDie:               change_HitDie();          break;
					case PrimaryAbil:
					case SpellAbil:            change_Ability();         break;
					case AlignRestrict:        change_AlignRestrict();   break;
					case AlignRstrctType:      change_AlignRstrctType(); break;

					case PlayerClass:
					case SpellCaster:
					case MetaMagicAllowed:
					case MemorizesSpells:
					case HasArcane:
					case HasDivine:
					case HasSpontaneousSpells:
					case AllSpellsKnown:
					case HasInfiniteSpells:
					case HasDomains:
					case HasSchool:
					case HasFamiliar:
					case HasAnimalCompanion:
					case InvertRestrict:       change_bool(); break;
				}
			}
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>.
		/// </summary>
		void change_HitDie()
		{
			clearchecks();

			string val;
			if (_cb.Checked)
			{
				if      (_cb == cb_00) val = "4";
				else if (_cb == cb_01) val = "6";
				else if (_cb == cb_02) val = "8";
				else if (_cb == cb_03) val = "10";
				else                   val = "12"; // _cb == cb_04
			}
			else
				val = gs.Stars;

			la_Val.Text = _f.str1 = val;
			bu_Clear.Enabled = (val != gs.Stars);
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>.
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

			la_Val.Text = _f.str1 = val;
			bu_Clear.Enabled = (val != gs.Stars);
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>.
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

			bu_Clear.Enabled = (_f.int1 != 0);
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>.
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

			bu_Clear.Enabled = (_f.int1 != 0);
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>.
		/// </summary>
		void change_bool()
		{
			clearchecks();

			string val;
			if (_cb.Checked)
			{
				if (_cb == cb_00) val = "0";
				else              val = "1"; // _cb == cb_01
			}
			else
				val = gs.Stars;

			la_Val.Text = _f.str1 = val;
			bu_Clear.Enabled = (val != gs.Stars);
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
				if (co_Val.SelectedIndex == co_Val.Items.Count - 1)
				{
					bu_Clear.Enabled = false;

					_f.str1 = gs.Stars;
					_f.int1 = Yata.Info_ASSIGN_STARS;
				}
				else
				{
					bu_Clear.Enabled = true;

					switch (_cell.x)
					{
						case Package:
							_f.int1 = co_Val.SelectedIndex;
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
				case HitDie: // str,cb,unique ->
				case PrimaryAbil:
				case SpellAbil:
				case PlayerClass:
				case SpellCaster:
				case MetaMagicAllowed:
				case MemorizesSpells:
				case HasArcane:
				case HasDivine:
				case HasSpontaneousSpells:
				case AllSpellsKnown:
				case HasInfiniteSpells:
				case HasDomains:
				case HasSchool:
				case HasFamiliar:
				case HasAnimalCompanion:
				case InvertRestrict:
					bu_Clear.Enabled = false;

					la_Val.Text = _f.str1 = gs.Stars;

					_cb = null;
					clearchecks();
					break;

				case AlignRestrict: // hex,cb,multiple ->
				case AlignRstrctType:
					bu_Clear.Enabled = false;

					_cb = null;
					clearchecks();

					printHexString(_f.int1 = 0);
					break;

				case Package: // dropdown -> fire changed_Combobox()
					co_Val.SelectedIndex = co_Val.Items.Count - 1;
					break;
			}
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Prints a hex-value at the top of the dialog.
		/// </summary>
		/// <param name="result"></param>
		void printHexString(int result)
		{
			string f;
			if (_cell.x == AlignRestrict) f = "X2";
			else                          f = "X1"; // _cell.x == AlignRstrctType

			la_Val.Text = "0x" + result.ToString(f, CultureInfo.InvariantCulture);
		}
		#endregion Methods
	}
}
