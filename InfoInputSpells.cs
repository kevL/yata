using System;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	sealed partial class InfoInputSpells
		:
			Form
	{
		#region Fields (static)
		internal const int School       =  4; // col in Spells.2da ->
		internal const int Range        =  5;
		internal const int MetaMagic    =  7;
		internal const int TargetType   =  8;
		internal const int ImmunityType = 45;
		internal const int Category     = 52;
		internal const int UserType     = 54;
		internal const int AsMetaMagic  = 65;
		internal const int TargetingUI  = 66;
		#endregion Fields (static)


		#region Fields
		YataForm _f;
		YataGrid _grid;
		Cell _cell;

		int ColType;
		bool _init;

		CheckBox _cb;
		#endregion Fields


		#region cTor
		internal InfoInputSpells(YataGrid grid, Cell cell)
		{
			InitializeComponent();

			_grid = grid;
			_f    = _grid._f;
			_cell = cell;

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			init();
		}
		#endregion cTor


		#region Events (override)
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
				Close();
			else
				base.OnKeyDown(e);
		}
		#endregion Events (override)


		#region Events
		void changed_Checkbox(object sender, EventArgs e)
		{
			if (!_init)
			{
				_cb = sender as CheckBox;

				switch (ColType)
				{
					case School:     changed_School();     break;
					case Range:      changed_Range();      break;
					case MetaMagic:  changed_MetaMagic();  break;
					case TargetType: changed_TargetType(); break;
					case UserType:   changed_UserType();   break;
				}
			}
		}

		void changed_Combobox(object sender, EventArgs e)
		{
			if (!_init)
			{
				if (cbx_Val.SelectedIndex == cbx_Val.Items.Count - 1)
				{
					btn_Clear.Enabled = false;

					_f.str1 = gs.Stars;
					_f.int1 = 0;
				}
				else
				{
					btn_Clear.Enabled = true;

					switch (ColType)
					{
						case ImmunityType:
							switch (cbx_Val.SelectedIndex)
							{
								case  0: _f.str1 = gs.Acid;           break;
								case  1: _f.str1 = gs.Cold;           break;
								case  2: _f.str1 = gs.Death;          break;
								case  3: _f.str1 = gs.Disease;        break;
								case  4: _f.str1 = gs.Divine;         break;
								case  5: _f.str1 = gs.Electricity;    break;
								case  6: _f.str1 = gs.Evil;           break;
								case  7: _f.str1 = gs.Fear;           break;
								case  8: _f.str1 = gs.Fire;           break;
								case  9: _f.str1 = gs.Magical;        break;
								case 10: _f.str1 = gs.Mind_Affecting; break;
								case 11: _f.str1 = gs.Negative;       break;
								case 12: _f.str1 = gs.Paralysis;      break;
								case 13: _f.str1 = gs.Poison;         break;
								case 14: _f.str1 = gs.Positive;       break;
								case 15: _f.str1 = gs.Sonic;          break;
								case 16: _f.str1 = gs.Constitution;   break;
								case 17: _f.str1 = gs.Water;          break;
							}
							break;

						case Category:
							_f.int1 = cbx_Val.SelectedIndex;
							break;

						case AsMetaMagic:
							switch (cbx_Val.SelectedIndex)
							{
								case  0: _f.int1 = YataForm.META_I_BESHADOWED_BLAST; break; // Eldritch Essences ->
								case  1: _f.int1 = YataForm.META_I_BEWITCHING_BLAST; break;
								case  2: _f.int1 = YataForm.META_I_BINDING_BLAST;    break;
								case  3: _f.int1 = YataForm.META_I_BRIMSTONE_BLAST;  break;
								case  4: _f.int1 = YataForm.META_I_DRAINING_BLAST;   break;
								case  5: _f.int1 = YataForm.META_I_FRIGHTFUL_BLAST;  break;
								case  6: _f.int1 = YataForm.META_I_HELLRIME_BLAST;   break;
								case  7: _f.int1 = YataForm.META_I_HINDERING_BLAST;  break;
								case  8: _f.int1 = YataForm.META_I_NOXIOUS_BLAST;    break;
								case  9: _f.int1 = YataForm.META_I_UTTERDARK_BLAST;  break;
								case 10: _f.int1 = YataForm.META_I_VITRIOLIC_BLAST;  break;

								case 11: _f.int1 = YataForm.META_I_ELDRITCH_CHAIN;   break; // Blast Shapes ->
								case 12: _f.int1 = YataForm.META_I_ELDRITCH_CONE;    break;
								case 13: _f.int1 = YataForm.META_I_ELDRITCH_DOOM;    break;
								case 14: _f.int1 = YataForm.META_I_ELDRITCH_SPEAR;   break;
								case 15: _f.int1 = YataForm.META_I_HIDEOUS_BLOW;     break;
							}
							break;

						case TargetingUI:
							_f.str1 = cbx_Val.SelectedIndex.ToString();
							break;
					}
				}
			}
		}

		void changed_MetaGroup(object sender, EventArgs e)
		{
			if (!_init)
			{
				var cb = sender as CheckBox;

				_init = true;
				if (cb == cb_MetaAllES || cb == cb_MetaAllE)
				{
					if (cb_08.Checked = cb_09.Checked = cb_10.Checked = cb_11.Checked =
						cb_12.Checked = cb_13.Checked = cb_14.Checked = cb_15.Checked =
						cb_16.Checked = cb_17.Checked = cb_18.Checked = cb.Checked)
					{
						_f.int1 |=  YataForm.META_I_ESSENCES;
					}
					else
						_f.int1 &= ~YataForm.META_I_ESSENCES;
				}

				if (cb == cb_MetaAllES || cb == cb_MetaAllS)
				{
					if (cb_19.Checked = cb_20.Checked = cb_21.Checked =
						cb_22.Checked = cb_23.Checked = cb.Checked)
					{
						_f.int1 |=  YataForm.META_I_SHAPES;
					}
					else
						_f.int1 &= ~YataForm.META_I_SHAPES;
				}
				_init = false;

				SetInfoText(_f.int1);
				EnableMetaMagics(_f.int1);
				checkMetagroups(_f.int1);

				btn_Clear.Enabled = (_f.int1 != 0);
			}
		}


		void click_Clear(object sender, EventArgs e)
		{
			btn_Clear.Enabled = false;

			switch (ColType)
			{
				case School:		// string, checkboxes, exclusive
					if (_f.str1 != gs.Stars)
					{
						_init = true;
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = cb_06.Checked = cb_07.Checked =
						_init = false;

						SetInfoText(-1, _f.str1 = gs.Stars);
					}
					break;

				case Range:			// string, checkboxes, exclusive
					if (_f.str1 != gs.Stars)
					{
						_init = true;
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked =
						_init = false;

						SetInfoText(-1, _f.str1 = gs.Stars);
					}
					break;

				case MetaMagic:		// int,    checkboxes, inclusive
					if (_f.int1 != 0)
					{
						_init = true;
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = cb_06.Checked = cb_07.Checked =
						cb_08.Checked = cb_09.Checked = cb_10.Checked = cb_11.Checked =
						cb_12.Checked = cb_13.Checked = cb_14.Checked = cb_15.Checked =
						cb_16.Checked = cb_17.Checked = cb_18.Checked = cb_19.Checked =
						cb_20.Checked = cb_21.Checked = cb_22.Checked = cb_23.Checked =
						_init = false;

						SetInfoText(_f.int1 = 0);
						EnableMetaMagics(_f.int1);
						checkMetagroups(_f.int1);
					}
					break;

				case TargetType:	// int,    checkboxes, inclusive
					if (_f.int1 != 0)
					{
						_init = true;
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = cb_06.Checked =
						_init = false;

						SetInfoText(_f.int1 = 0);
					}
					break;

				case ImmunityType:	// string, dropdown,   exclusive
				case TargetingUI:	// string, dropdown,   exclusive
					if (_f.str1 != gs.Stars)
					{
						cbx_Val.SelectedIndex = cbx_Val.Items.Count - 1; // "n/a"
					}
					break;

				case Category:		// int,    dropdown,   exclusive
				case AsMetaMagic:	// int,    dropdown,   exclusive
					if (_f.int1 != 0)
					{
						cbx_Val.SelectedIndex = cbx_Val.Items.Count - 1; // "n/a"
					}
					break;

				case UserType:		// string, checkboxes, exclusive
					if (_f.str1 != gs.Stars)
					{
						_init = true;
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						_init = false;

						SetInfoText(-1, _f.str1 = gs.Stars);
					}
					break;
			}
		}


//		void click_Accept(object sender, EventArgs e)
//		{}
		#endregion Events


		#region Methods
		void init()
		{
			_init = true;

			string val = _cell.text;
			if (!String.IsNullOrEmpty(val)) // safety.
			{
				int result;
				switch (_cell.x)
				{
					case School:
						ColType = School;
						Text = " School";
						setVisibleSchools();

						cb_00.Text = "Abjuration";
						cb_01.Text = "Conjuration";
						cb_02.Text = "Divination";
						cb_03.Text = "Enchantment";
						cb_04.Text = "Illusion";
						cb_05.Text = "Necromancy";
						cb_06.Text = "Transmutation";
						cb_07.Text = "Evocation";

						switch (val)
						{
							case "A": cb_00.Checked = true; break;
							case "C": cb_01.Checked = true; break;
							case "D": cb_02.Checked = true; break;
							case "E": cb_03.Checked = true; break;
							case "I": cb_04.Checked = true; break;
							case "N": cb_05.Checked = true; break;
							case "T": cb_06.Checked = true; break;
							case "V": cb_07.Checked = true; break;
						}

						SetInfoText(-1, _f.str0 = _f.str1 = val);
						btn_Clear.Enabled = (val != gs.Stars);
						break;

					case Range:
						ColType = Range;
						Text = " Range";
						setVisibleRanges();

						cb_00.Text = "Personal" + (( 0 < Info.rangeRanges.Count) ? (" " + Info.rangeRanges[ 0] + "m") : String.Empty);
						cb_01.Text = "Touch"    + (( 1 < Info.rangeRanges.Count) ? (" " + Info.rangeRanges[ 1] + "m") : String.Empty);
						cb_02.Text = "Short"    + (( 2 < Info.rangeRanges.Count) ? (" " + Info.rangeRanges[ 2] + "m") : String.Empty);
						cb_03.Text = "Medium"   + (( 3 < Info.rangeRanges.Count) ? (" " + Info.rangeRanges[ 3] + "m") : String.Empty);
						cb_04.Text = "Long"     + (( 4 < Info.rangeRanges.Count) ? (" " + Info.rangeRanges[ 4] + "m") : String.Empty);
						cb_05.Text = "Infinite" + ((14 < Info.rangeRanges.Count) ? (" " + Info.rangeRanges[14] + "m") : String.Empty);

						switch (val)
						{
							case "P": cb_00.Checked = true; break;
							case "T": cb_01.Checked = true; break;
							case "S": cb_02.Checked = true; break;
							case "M": cb_03.Checked = true; break;
							case "L": cb_04.Checked = true; break;
							case "I": cb_05.Checked = true; break;
						}

						SetInfoText(-1, _f.str0 = _f.str1 = val);
						btn_Clear.Enabled = (val != gs.Stars);
						break;

					case MetaMagic:
						ColType = MetaMagic;
						Text = " MetaMagic";
						setVisibleMetaMagics();

						cb_00.Text = "(1)Empower";
						cb_01.Text = "(2)Extend";
						cb_02.Text = "(4)Maximize";
						cb_03.Text = "(8)Quicken";
						cb_04.Text = "(16)Silent";
						cb_05.Text = "(32)Still";
						cb_06.Text = "(64)Persistent";
						cb_07.Text = "(128)Permanent";

						cb_08.Text = gs.BeshadowedBlast;	//(4096) // Eldritch Essences ->
						cb_09.Text = gs.BewitchingBlast;	//(65536)
						cb_10.Text = gs.BindingBlast;		//(8388608)
						cb_11.Text = gs.BrimstoneBlast;		//(8192)
						cb_12.Text = gs.DrainingBlast;		//(256)
						cb_13.Text = gs.FrightfulBlast;		//(1024)
						cb_14.Text = gs.HellrimeBlast;		//(32768)
						cb_15.Text = gs.HinderingBlast;		//(4194304)
						cb_16.Text = gs.NoxiousBlast;		//(262144)
						cb_17.Text = gs.UtterdarkBlast;		//(2097152)
						cb_18.Text = gs.VitriolicBlast;		//(524288)

						cb_19.Text = gs.EldritchChain;		//(16384) // Blast Shapes ->
						cb_20.Text = gs.EldritchCone;		//(131072)
						cb_21.Text = gs.EldritchDoom;		//(1048576)
						cb_22.Text = gs.EldritchSpear;		//(512)
						cb_23.Text = gs.HideousBlow;		//(2048)

						if (val == gs.Stars || val.Length < 3) val = "0x0";
						if (Int32.TryParse(val.Substring(2),
										   NumberStyles.AllowHexSpecifier,
										   CultureInfo.InvariantCulture,
										   out result))
						{
							EnableMetaMagics(result);

							cb_00.Checked = ((result & YataForm.META_EMPOWER)    != 0);
							cb_01.Checked = ((result & YataForm.META_EXTEND)     != 0);
							cb_02.Checked = ((result & YataForm.META_MAXIMIZE)   != 0);
							cb_03.Checked = ((result & YataForm.META_QUICKEN)    != 0);
							cb_04.Checked = ((result & YataForm.META_SILENT)     != 0);
							cb_05.Checked = ((result & YataForm.META_STILL)      != 0);
							cb_06.Checked = ((result & YataForm.META_PERSISTENT) != 0);
							cb_07.Checked = ((result & YataForm.META_PERMANENT)  != 0);

							cb_08.Checked = ((result & YataForm.META_I_BESHADOWED_BLAST) != 0); // Eldritch Essences ->
							cb_09.Checked = ((result & YataForm.META_I_BEWITCHING_BLAST) != 0);
							cb_10.Checked = ((result & YataForm.META_I_BINDING_BLAST)    != 0);
							cb_11.Checked = ((result & YataForm.META_I_BRIMSTONE_BLAST)  != 0);
							cb_12.Checked = ((result & YataForm.META_I_DRAINING_BLAST)   != 0);
							cb_13.Checked = ((result & YataForm.META_I_FRIGHTFUL_BLAST)  != 0);
							cb_14.Checked = ((result & YataForm.META_I_HELLRIME_BLAST)   != 0);
							cb_15.Checked = ((result & YataForm.META_I_HINDERING_BLAST)  != 0);
							cb_16.Checked = ((result & YataForm.META_I_NOXIOUS_BLAST)    != 0);
							cb_17.Checked = ((result & YataForm.META_I_UTTERDARK_BLAST)  != 0);
							cb_18.Checked = ((result & YataForm.META_I_VITRIOLIC_BLAST)  != 0);

							cb_19.Checked = ((result & YataForm.META_I_ELDRITCH_CHAIN)   != 0); // Blast Shapes ->
							cb_20.Checked = ((result & YataForm.META_I_ELDRITCH_CONE)    != 0);
							cb_21.Checked = ((result & YataForm.META_I_ELDRITCH_DOOM)    != 0);
							cb_22.Checked = ((result & YataForm.META_I_ELDRITCH_SPEAR)   != 0);
							cb_23.Checked = ((result & YataForm.META_I_HIDEOUS_BLOW)     != 0);

							checkMetagroups(result); // WARNING: That will set '_init' false early.
//							cb_MetaAllES.Checked = ((result & YataForm.META_I_ALL)      == YataForm.META_I_ALL);
//							cb_MetaAllE .Checked = ((result & YataForm.META_I_ESSENCES) == YataForm.META_I_ESSENCES);
//							cb_MetaAllS .Checked = ((result & YataForm.META_I_SHAPES)   == YataForm.META_I_SHAPES);
						}
						SetInfoText(_f.int0 = _f.int1 = result);
						btn_Clear.Enabled = (result != 0);
						break;

					case TargetType:
						ColType = TargetType;
						Text = " TargetType";
						setVisibleTargetTypes();

						cb_00.Text = "(1)Self";
						cb_01.Text = "(2)Creatures";
						cb_02.Text = "(4)Ground";
						cb_03.Text = "(8)Items";
						cb_04.Text = "(16)Doors";
						cb_05.Text = "(32)Placeables";
						cb_06.Text = "(64)Triggers";

						if (val == gs.Stars || val.Length < 3) val = "0x0";
						if (Int32.TryParse(val.Substring(2),
										   NumberStyles.AllowHexSpecifier,
										   CultureInfo.InvariantCulture,
										   out result))
						{
							cb_00.Checked = ((result & YataForm.TARGET_SELF)       != 0);
							cb_01.Checked = ((result & YataForm.TARGET_CREATURE)   != 0);
							cb_02.Checked = ((result & YataForm.TARGET_GROUND)     != 0);
							cb_03.Checked = ((result & YataForm.TARGET_ITEMS)      != 0);
							cb_04.Checked = ((result & YataForm.TARGET_DOORS)      != 0);
							cb_05.Checked = ((result & YataForm.TARGET_PLACEABLES) != 0);
							cb_06.Checked = ((result & YataForm.TARGET_TRIGGERS)   != 0);
						}
						SetInfoText(_f.int0 = _f.int1 = result);
						btn_Clear.Enabled = (result != 0);
						break;

					case ImmunityType:
						ColType = ImmunityType;
						Text = " ImmunityType";
						setComboboxVisible();
						populateImmunityTypes();

						switch (val)
						{
							case gs.Acid:           cbx_Val.SelectedIndex =  0; break; // sub
							case gs.Cold:           cbx_Val.SelectedIndex =  1; break; // sub
							case gs.Death:          cbx_Val.SelectedIndex =  2; break;
							case gs.Disease:        cbx_Val.SelectedIndex =  3; break;
							case gs.Divine:         cbx_Val.SelectedIndex =  4; break; // sub
							case gs.Electricity:    cbx_Val.SelectedIndex =  5; break; // sub
							case gs.Evil:           cbx_Val.SelectedIndex =  6; break; // non-standard
							case gs.Fear:           cbx_Val.SelectedIndex =  7; break;
							case gs.Fire:           cbx_Val.SelectedIndex =  8; break; // sub
							case gs.Magical:        cbx_Val.SelectedIndex =  9; break; // sub
							case gs.Mind_Affecting: cbx_Val.SelectedIndex = 10; break;
							case gs.Negative:       cbx_Val.SelectedIndex = 11; break; // sub
							case gs.Paralysis:      cbx_Val.SelectedIndex = 12; break;
							case gs.Poison:         cbx_Val.SelectedIndex = 13; break;
							case gs.Positive:       cbx_Val.SelectedIndex = 14; break; // sub
							case gs.Sonic:          cbx_Val.SelectedIndex = 15; break; // sub
							case gs.Constitution:   cbx_Val.SelectedIndex = 16; break; // non-standard
							case gs.Water:          cbx_Val.SelectedIndex = 17; break; // non-standard

							default:
							case gs.Stars: cbx_Val.SelectedIndex = 18; break;
						}
						_f.str0 = _f.str1 = val;
						btn_Clear.Enabled = (val != gs.Stars);
						break;
/*
int IMMUNITY_TYPE_NONE                      =  0;
int IMMUNITY_TYPE_MIND_SPELLS               =  1; // y
int IMMUNITY_TYPE_POISON                    =  2; // y
int IMMUNITY_TYPE_DISEASE                   =  3; // y
int IMMUNITY_TYPE_FEAR                      =  4; // y
int IMMUNITY_TYPE_TRAP                      =  5;
int IMMUNITY_TYPE_PARALYSIS                 =  6; // y
int IMMUNITY_TYPE_BLINDNESS                 =  7;
int IMMUNITY_TYPE_DEAFNESS                  =  8;
int IMMUNITY_TYPE_SLOW                      =  9;
int IMMUNITY_TYPE_ENTANGLE                  = 10;
int IMMUNITY_TYPE_SILENCE                   = 11;
int IMMUNITY_TYPE_STUN                      = 12;
int IMMUNITY_TYPE_SLEEP                     = 13;
int IMMUNITY_TYPE_CHARM                     = 14;
int IMMUNITY_TYPE_DOMINATE                  = 15;
int IMMUNITY_TYPE_CONFUSED                  = 16;
int IMMUNITY_TYPE_CURSED                    = 17;
int IMMUNITY_TYPE_DAZED                     = 18;
int IMMUNITY_TYPE_ABILITY_DECREASE          = 19;
int IMMUNITY_TYPE_ATTACK_DECREASE           = 20;
int IMMUNITY_TYPE_DAMAGE_DECREASE           = 21;
int IMMUNITY_TYPE_DAMAGE_IMMUNITY_DECREASE  = 22;
int IMMUNITY_TYPE_AC_DECREASE               = 23;
int IMMUNITY_TYPE_MOVEMENT_SPEED_DECREASE   = 24;
int IMMUNITY_TYPE_SAVING_THROW_DECREASE     = 25;
int IMMUNITY_TYPE_SPELL_RESISTANCE_DECREASE = 26;
int IMMUNITY_TYPE_SKILL_DECREASE            = 27;
int IMMUNITY_TYPE_KNOCKDOWN                 = 28;
int IMMUNITY_TYPE_NEGATIVE_LEVEL            = 29;
int IMMUNITY_TYPE_SNEAK_ATTACK              = 30;
int IMMUNITY_TYPE_CRITICAL_HIT              = 31;
int IMMUNITY_TYPE_DEATH                     = 32; // y
*/
					case Category:
						ColType = Category;
						Text = " Category";
						setComboboxVisible();
						populateCategories();

						if (val == gs.Stars) val = "0";
						if (Int32.TryParse(val, out result)
							&& result > -1 && result < Info.categoryLabels.Count)
						{
							cbx_Val.SelectedIndex = result;
						}
						else
							cbx_Val.SelectedIndex = Info.categoryLabels.Count; // "n/a"

						_f.int0 = _f.int1 = result;
						btn_Clear.Enabled = (result != 0);
						break;

					case UserType:
						ColType = UserType;
						Text = " UserType";
						setVisibleUserTypes();

						cb_00.Text = "1 - Spell";
						cb_01.Text = "2 - Special Ability";
						cb_02.Text = "3 - Feat";
						cb_03.Text = "4 - Item Power";

						switch (val)
						{
							case "1": cb_00.Checked = true; break;
							case "2": cb_01.Checked = true; break;
							case "3": cb_02.Checked = true; break;
							case "4": cb_03.Checked = true; break;
						}
						SetInfoText(-1, _f.str0 = _f.str1 = val);
						btn_Clear.Enabled = (val != gs.Stars);
						break;

					case AsMetaMagic:
						ColType = AsMetaMagic;
						Text = " AsMetaMagic";
						setComboboxVisible();
						populateAsMetaMagics();

						if (val == gs.Stars || val.Length < 3) val = "0x0";
						if (Int32.TryParse(val.Substring(2),
										   NumberStyles.AllowHexSpecifier,
										   CultureInfo.InvariantCulture,
										   out result))
						{
							switch (result)
							{
								case YataForm.META_I_BESHADOWED_BLAST: cbx_Val.SelectedIndex =  0; break; // Eldritch Essences ->
								case YataForm.META_I_BEWITCHING_BLAST: cbx_Val.SelectedIndex =  1; break;
								case YataForm.META_I_BINDING_BLAST:    cbx_Val.SelectedIndex =  2; break;
								case YataForm.META_I_BRIMSTONE_BLAST:  cbx_Val.SelectedIndex =  3; break;
								case YataForm.META_I_DRAINING_BLAST:   cbx_Val.SelectedIndex =  4; break;
								case YataForm.META_I_FRIGHTFUL_BLAST:  cbx_Val.SelectedIndex =  5; break;
								case YataForm.META_I_HELLRIME_BLAST:   cbx_Val.SelectedIndex =  6; break;
								case YataForm.META_I_HINDERING_BLAST:  cbx_Val.SelectedIndex =  7; break;
								case YataForm.META_I_NOXIOUS_BLAST:    cbx_Val.SelectedIndex =  8; break;
								case YataForm.META_I_UTTERDARK_BLAST:  cbx_Val.SelectedIndex =  9; break;
								case YataForm.META_I_VITRIOLIC_BLAST:  cbx_Val.SelectedIndex = 10; break;

								case YataForm.META_I_ELDRITCH_CHAIN:   cbx_Val.SelectedIndex = 11; break; // Blast Shapes ->
								case YataForm.META_I_ELDRITCH_CONE:    cbx_Val.SelectedIndex = 12; break;
								case YataForm.META_I_ELDRITCH_DOOM:    cbx_Val.SelectedIndex = 13; break;
								case YataForm.META_I_ELDRITCH_SPEAR:   cbx_Val.SelectedIndex = 14; break;
								case YataForm.META_I_HIDEOUS_BLOW:     cbx_Val.SelectedIndex = 15; break;
							}
						}
						_f.int0 = _f.int1 = result;
						btn_Clear.Enabled = (result != 0);
						break;

					case TargetingUI:
						ColType = TargetingUI;
						Text = " TargetingUI";
						setComboboxVisible();
						populateTargeters();

						if (Int32.TryParse(val, out result)
							&& result > -1 && result < Info.targetLabels.Count)
						{
							cbx_Val.SelectedIndex = result;
						}
						else
							cbx_Val.SelectedIndex = Info.targetLabels.Count; // "n/a"

						_f.str0 = _f.str1 = val;
						btn_Clear.Enabled = (val != gs.Stars);
						break;
				}
			}
			_init = false;
		}


		void SetInfoText(int result, string val = null)
		{
			if (val == null)
			{
				string format;
				switch (ColType)
				{
					case MetaMagic:
						if (result <= 0xFF) goto default;
						format = "X6";
						break;

					default:
					case TargetType:
						format = "X2";
						break;
				}
				lbl_Val.Text = "0x" + result.ToString(format);
			}
			else
				lbl_Val.Text = (val == gs.Stars) ? String.Empty : val;
		}

		void setComboboxVisible()
		{
			lbl_Val.Visible = false;
			cbx_Val.Visible = true;
		}

		void setVisibleSchools()
		{
			cb_00.Visible =
			cb_01.Visible =
			cb_02.Visible =
			cb_03.Visible =
			cb_04.Visible =
			cb_05.Visible =
			cb_06.Visible =
			cb_07.Visible = true;
		}

		void setVisibleRanges()
		{
			cb_00.Visible =
			cb_01.Visible =
			cb_02.Visible =
			cb_03.Visible =
			cb_04.Visible =
			cb_05.Visible = true;
		}

		void setVisibleMetaMagics()
		{
			cb_00.Visible =
			cb_01.Visible =
			cb_02.Visible =
			cb_03.Visible =
			cb_04.Visible =
			cb_05.Visible =
			cb_06.Visible =
			cb_07.Visible =

			cb_08.Visible = // Eldritch Essences and Blast Shapes ->
			cb_09.Visible =
			cb_10.Visible =
			cb_11.Visible =
			cb_12.Visible =
			cb_13.Visible =
			cb_14.Visible =
			cb_15.Visible =
			cb_16.Visible =
			cb_17.Visible =
			cb_18.Visible =
			cb_19.Visible =
			cb_20.Visible =
			cb_21.Visible =
			cb_22.Visible =
			cb_23.Visible =

			gb_MetaGroups.Visible = true;
		}

		void EnableMetaMagics(int result)
		{
			cb_00.Enabled =
			cb_01.Enabled =
			cb_02.Enabled =
			cb_03.Enabled =
			cb_04.Enabled =
			cb_05.Enabled =
			cb_06.Enabled =
			cb_07.Enabled = (result <= 0xFF);

			cb_08.Enabled = // Eldritch Essences and Blast Shapes ->
			cb_09.Enabled =
			cb_10.Enabled =
			cb_11.Enabled =
			cb_12.Enabled =
			cb_13.Enabled =
			cb_14.Enabled =
			cb_15.Enabled =
			cb_16.Enabled =
			cb_17.Enabled =
			cb_18.Enabled =
			cb_19.Enabled =
			cb_20.Enabled =
			cb_21.Enabled =
			cb_22.Enabled =
			cb_23.Enabled =

			gb_MetaGroups.Enabled = (result == 0x00 || result > 0xFF);
		}

		void setVisibleTargetTypes()
		{
			cb_00.Visible =
			cb_01.Visible =
			cb_02.Visible =
			cb_03.Visible =
			cb_04.Visible =
			cb_05.Visible =
			cb_06.Visible = true;
		}

		void populateImmunityTypes()
		{
			cbx_Val.Items.Add(new tui(gs.Acid));
			cbx_Val.Items.Add(new tui(gs.Cold));
			cbx_Val.Items.Add(new tui(gs.Death));
			cbx_Val.Items.Add(new tui(gs.Disease));
			cbx_Val.Items.Add(new tui(gs.Divine));
			cbx_Val.Items.Add(new tui(gs.Electricity));
			cbx_Val.Items.Add(new tui(gs.Evil));
			cbx_Val.Items.Add(new tui(gs.Fear));
			cbx_Val.Items.Add(new tui(gs.Fire));
			cbx_Val.Items.Add(new tui(gs.Magical));
			cbx_Val.Items.Add(new tui(gs.Mind_Affecting));
			cbx_Val.Items.Add(new tui(gs.Negative));
			cbx_Val.Items.Add(new tui(gs.Paralysis));
			cbx_Val.Items.Add(new tui(gs.Poison));
			cbx_Val.Items.Add(new tui(gs.Positive));
			cbx_Val.Items.Add(new tui(gs.Sonic));
			cbx_Val.Items.Add(new tui(gs.Constitution));
			cbx_Val.Items.Add(new tui(gs.Water));

			cbx_Val.Items.Add(new tui(gs.non));
		}

		void populateCategories()
		{
			for (int i = 0; i != Info.categoryLabels.Count; ++i)
			{
				cbx_Val.Items.Add(new tui(i + " - " + Info.categoryLabels[i].ToLower()));
			}
			cbx_Val.Items.Add(new tui(gs.non));
		}

		void setVisibleUserTypes()
		{
			cb_00.Visible =
			cb_01.Visible =
			cb_02.Visible =
			cb_03.Visible = true;
		}

		void populateAsMetaMagics()
		{
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_BESHADOWED_BLAST) + " - " + gs.BeshadowedBlast)); // Eldritch Essences ->
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_BEWITCHING_BLAST) + " - " + gs.BewitchingBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_BINDING_BLAST)    + " - " + gs.BindingBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_BRIMSTONE_BLAST)  + " - " + gs.BrimstoneBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_DRAINING_BLAST)   + " - " + gs.DrainingBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_FRIGHTFUL_BLAST)  + " - " + gs.FrightfulBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_HELLRIME_BLAST)   + " - " + gs.HellrimeBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_HINDERING_BLAST)  + " - " + gs.HinderingBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_NOXIOUS_BLAST)    + " - " + gs.NoxiousBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_UTTERDARK_BLAST)  + " - " + gs.UtterdarkBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_VITRIOLIC_BLAST)  + " - " + gs.VitriolicBlast));

			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_ELDRITCH_CHAIN)   + " - " + gs.EldritchChain)); // Blast Shapes ->
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_ELDRITCH_CONE)    + " - " + gs.EldritchCone));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_ELDRITCH_DOOM)    + " - " + gs.EldritchDoom));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_ELDRITCH_SPEAR)   + " - " + gs.EldritchSpear));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_HIDEOUS_BLOW)     + " - " + gs.HideousBlow));

			cbx_Val.Items.Add(new tui(gs.non));
			cbx_Val.SelectedIndex = 16;
		}

		string hexen(int i) { return "0x" + i.ToString("X6"); }


		void populateTargeters()
		{
			string text;

			for (int i = 0; i != Info.targetLabels.Count; ++i)
			{
				text = i + " - " + Info.targetLabels[i];

				bool haspars = false;

				float f = Info.targetWidths[i];
				if (Math.Abs(0.0F - f) > Constants.epsilon)
				{
					haspars = true;
					text += " (" + f;
				}

				f = Info.targetLengths[i];
				if (Math.Abs(0.0F - f) > Constants.epsilon)
				{
					if (!haspars)
					{
						haspars = true;
						text += " (_";
					}
					text += " x " + f;
				}

				if (haspars) text += ")";

				cbx_Val.Items.Add(new tui(text));
			}
			cbx_Val.Items.Add(new tui(gs.non));
		}


		void changed_School()
		{
			if (!_init)
			{
				_init = true;

				string val = gs.Stars;
				if (_cb == cb_00)
				{
					if (_cb.Checked)
					{
						val = "A";
						cb_01.Checked = cb_02.Checked = cb_03.Checked = cb_04.Checked =
						cb_05.Checked = cb_06.Checked = cb_07.Checked = false;
					}
				}
				else if (_cb == cb_01)
				{
					if (_cb.Checked)
					{
						val = "C";
						cb_00.Checked = cb_02.Checked = cb_03.Checked = cb_04.Checked =
						cb_05.Checked = cb_06.Checked = cb_07.Checked = false;
					}
				}
				else if (_cb == cb_02)
				{
					if (_cb.Checked)
					{
						val = "D";
						cb_00.Checked = cb_01.Checked = cb_03.Checked = cb_04.Checked =
						cb_05.Checked = cb_06.Checked = cb_07.Checked = false;
					}
				}
				else if (_cb == cb_03)
				{
					if (_cb.Checked)
					{
						val = "E";
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_04.Checked =
						cb_05.Checked = cb_06.Checked = cb_07.Checked = false;
					}
				}
				else if (_cb == cb_04)
				{
					if (_cb.Checked)
					{
						val = "I";
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_05.Checked = cb_06.Checked = cb_07.Checked = false;
					}
				}
				else if (_cb == cb_05)
				{
					if (_cb.Checked)
					{
						val = "N";
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_06.Checked = cb_07.Checked = false;
					}
				}
				else if (_cb == cb_06)
				{
					if (_cb.Checked)
					{
						val = "T";
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = cb_07.Checked = false;
					}
				}
				else //if (_cb == cb_07)
				{
					if (_cb.Checked)
					{
						val = "V";
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = cb_06.Checked = false;
					}
				}
				SetInfoText(-1, _f.str1 = val);
				btn_Clear.Enabled = (val != gs.Stars);

				_init = false;
			}
		}

		void changed_Range()
		{
			if (!_init)
			{
				_init = true;

				string val = gs.Stars;
				if (_cb == cb_00)
				{
					if (_cb.Checked)
					{
						val = "P";
						cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = false;
					}
				}
				else if (_cb == cb_01)
				{
					if (_cb.Checked)
					{
						val = "T";
						cb_00.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = false;
					}
				}
				else if (_cb == cb_02)
				{
					if (_cb.Checked)
					{
						val = "S";
						cb_00.Checked = cb_01.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = false;
					}
				}
				else if (_cb == cb_03)
				{
					if (_cb.Checked)
					{
						val = "M";
						cb_00.Checked = cb_01.Checked = cb_02.Checked =
						cb_04.Checked = cb_05.Checked = false;
					}
				}
				else if (_cb == cb_04)
				{
					if (_cb.Checked)
					{
						val = "L";
						cb_00.Checked = cb_01.Checked = cb_02.Checked =
						cb_03.Checked = cb_05.Checked = false;
					}
				}
				else //if (_cb == cb_05)
				{
					if (_cb.Checked)
					{
						val = "I";
						cb_00.Checked = cb_01.Checked = cb_02.Checked =
						cb_03.Checked = cb_04.Checked = false;
					}
				}
				SetInfoText(-1, _f.str1 = val);
				btn_Clear.Enabled = (val != gs.Stars);

				_init = false;
			}
		}

		void changed_MetaMagic()
		{
			if (_cb == cb_00)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_EMPOWER;
				else             _f.int1 &= ~YataForm.META_EMPOWER;
			}
			else if (_cb == cb_01)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_EXTEND;
				else             _f.int1 &= ~YataForm.META_EXTEND;
			}
			else if (_cb == cb_02)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_MAXIMIZE;
				else             _f.int1 &= ~YataForm.META_MAXIMIZE;
			}
			else if (_cb == cb_03)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_QUICKEN;
				else             _f.int1 &= ~YataForm.META_QUICKEN;
			}
			else if (_cb == cb_04)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_SILENT;
				else             _f.int1 &= ~YataForm.META_SILENT;
			}
			else if (_cb == cb_05)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_STILL;
				else             _f.int1 &= ~YataForm.META_STILL;
			}
			else if (_cb == cb_06)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_PERSISTENT;
				else             _f.int1 &= ~YataForm.META_PERSISTENT;
			}
			else if (_cb == cb_07)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_PERMANENT;
				else             _f.int1 &= ~YataForm.META_PERMANENT;
			}

			else if (_cb == cb_08) // Eldritch Essences ->
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_BESHADOWED_BLAST;
				else             _f.int1 &= ~YataForm.META_I_BESHADOWED_BLAST;
			}
			else if (_cb == cb_09)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_BEWITCHING_BLAST;
				else             _f.int1 &= ~YataForm.META_I_BEWITCHING_BLAST;
			}
			else if (_cb == cb_10)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_BINDING_BLAST;
				else             _f.int1 &= ~YataForm.META_I_BINDING_BLAST;
			}
			else if (_cb == cb_11)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_BRIMSTONE_BLAST;
				else             _f.int1 &= ~YataForm.META_I_BRIMSTONE_BLAST;
			}
			else if (_cb == cb_12)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_DRAINING_BLAST;
				else             _f.int1 &= ~YataForm.META_I_DRAINING_BLAST;
			}
			else if (_cb == cb_13)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_FRIGHTFUL_BLAST;
				else             _f.int1 &= ~YataForm.META_I_FRIGHTFUL_BLAST;
			}
			else if (_cb == cb_14)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_HELLRIME_BLAST;
				else             _f.int1 &= ~YataForm.META_I_HELLRIME_BLAST;
			}
			else if (_cb == cb_15)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_HINDERING_BLAST;
				else             _f.int1 &= ~YataForm.META_I_HINDERING_BLAST;
			}
			else if (_cb == cb_16)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_NOXIOUS_BLAST;
				else             _f.int1 &= ~YataForm.META_I_NOXIOUS_BLAST;
			}
			else if (_cb == cb_17)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_UTTERDARK_BLAST;
				else             _f.int1 &= ~YataForm.META_I_UTTERDARK_BLAST;
			}
			else if (_cb == cb_18)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_VITRIOLIC_BLAST;
				else             _f.int1 &= ~YataForm.META_I_VITRIOLIC_BLAST;
			}

			else if (_cb == cb_19) // Blast Shapes ->
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_ELDRITCH_CHAIN;
				else             _f.int1 &= ~YataForm.META_I_ELDRITCH_CHAIN;
			}
			else if (_cb == cb_20)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_ELDRITCH_CONE;
				else             _f.int1 &= ~YataForm.META_I_ELDRITCH_CONE;
			}
			else if (_cb == cb_21)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_ELDRITCH_DOOM;
				else             _f.int1 &= ~YataForm.META_I_ELDRITCH_DOOM;
			}
			else if (_cb == cb_22)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_ELDRITCH_SPEAR;
				else             _f.int1 &= ~YataForm.META_I_ELDRITCH_SPEAR;
			}
			else if (_cb == cb_23)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.META_I_HIDEOUS_BLOW;
				else             _f.int1 &= ~YataForm.META_I_HIDEOUS_BLOW;
			}

			SetInfoText(_f.int1);
			EnableMetaMagics(_f.int1);
			checkMetagroups(_f.int1);

			btn_Clear.Enabled = (_f.int1 != 0);
		}

		void changed_TargetType()
		{
			if (_cb == cb_00)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.TARGET_SELF;
				else             _f.int1 &= ~YataForm.TARGET_SELF;
			}
			else if (_cb == cb_01)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.TARGET_CREATURE;
				else             _f.int1 &= ~YataForm.TARGET_CREATURE;
			}
			else if (_cb == cb_02)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.TARGET_GROUND;
				else             _f.int1 &= ~YataForm.TARGET_GROUND;
			}
			else if (_cb == cb_03)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.TARGET_ITEMS;
				else             _f.int1 &= ~YataForm.TARGET_ITEMS;
			}
			else if (_cb == cb_04)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.TARGET_DOORS;
				else             _f.int1 &= ~YataForm.TARGET_DOORS;
			}
			else if (_cb == cb_05)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.TARGET_PLACEABLES;
				else             _f.int1 &= ~YataForm.TARGET_PLACEABLES;
			}
			else if (_cb == cb_06)
			{
				if (_cb.Checked) _f.int1 |=  YataForm.TARGET_TRIGGERS;
				else             _f.int1 &= ~YataForm.TARGET_TRIGGERS;
			}

			SetInfoText(_f.int1);

			btn_Clear.Enabled = (_f.int1 != 0);
		}

		void changed_UserType()
		{
			if (!_init)
			{
				_init = true;

				string val = gs.Stars;
				if (_cb == cb_00)
				{
					if (_cb.Checked)
					{
						val = "1";
						cb_01.Checked = cb_02.Checked = cb_03.Checked = false;
					}
				}
				else if (_cb == cb_01)
				{
					if (_cb.Checked)
					{
						val = "2";
						cb_00.Checked = cb_02.Checked = cb_03.Checked = false;
					}
				}
				else if (_cb == cb_02)
				{
					if (_cb.Checked)
					{
						val = "3";
						cb_00.Checked = cb_01.Checked = cb_03.Checked = false;
					}
				}
				else //if (_cb == cb_03)
				{
					if (_cb.Checked)
					{
						val = "4";
						cb_00.Checked = cb_01.Checked = cb_02.Checked = false;
					}
				}
				SetInfoText(-1, _f.str1 = val);

				btn_Clear.Enabled = (val != gs.Stars);
				_init = false;
			}
		}

		void checkMetagroups(int result)
		{
			_init = true;

			cb_MetaAllES.Checked = ((result & YataForm.META_I_ALL)      == YataForm.META_I_ALL);
			cb_MetaAllE .Checked = ((result & YataForm.META_I_ESSENCES) == YataForm.META_I_ESSENCES);
			cb_MetaAllS .Checked = ((result & YataForm.META_I_SHAPES)   == YataForm.META_I_SHAPES);

			_init = false;
		}
		#endregion Methods
	}
}
