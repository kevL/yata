using System;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	sealed partial class InfoHexDialog
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

		const string Acid           = "Acid";
		const string Cold           = "Cold";
		const string Death          = "Death";
		const string Disease        = "Disease";
		const string Divine         = "Divine";
		const string Electricity    = "Electricity";
		const string Evil           = "Evil";
		const string Fear           = "Fear";
		const string Fire           = "Fire";
		const string Magical        = "Magical";
		const string Mind_Affecting = "Mind_Affecting";
		const string Negative       = "Negative";
		const string Paralysis      = "Paralysis";
		const string Poison         = "Poison";
		const string Positive       = "Positive";
		const string Sonic          = "Sonic";
		const string Constitution   = "Constitution";
		const string Water          = "Water";

		const string non = "n/a";

		const string BeshadowedBlast = "Beshadowed Blast";
		const string BewitchingBlast = "Bewitching Blast";
		const string BindingBlast    = "Binding Blast";
		const string BrimstoneBlast  = "Brimstone Blast";
		const string DrainingBlast   = "Draining Blast";
		const string FrightfulBlast  = "Frightful Blast";
		const string HellrimeBlast   = "Hellrime Blast";
		const string HinderingBlast  = "Hindering Blast";
		const string NoxiousBlast    = "Noxious Blast";
		const string UtterdarkBlast  = "Utterdark Blast";
		const string VitriolicBlast  = "Vitriolic Blast";

		const string EldritchChain   = "Eldritch Chain";
		const string EldritchCone    = "Eldritch Cone";
		const string EldritchDoom    = "Eldritch Doom";
		const string EldritchSpear   = "Eldritch Spear";
		const string HideousBlow     = "Hideous Blow";
		#endregion Fields (static)


		#region Fields
		YataForm _f;
		YataGrid _grid;
		Cell _cell;

		int ColType;
		bool _init;
		bool _bypass;

		CheckBox _cb;
		#endregion Fields


		#region cTor
		internal InfoHexDialog(YataGrid grid, Cell cell)
		{
			InitializeComponent();

			_grid = grid;
			_f    = _grid._f;
			_cell = cell;

			if (Settings._font2 != null)
				Font = Settings._font2;
			else
				Font = _f.Font;

			init();
		}
		#endregion cTor


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
							case "T": cb_05.Checked = true; break;
							case "V": cb_05.Checked = true; break;
						}

						SetInfoText(-1, _f.stOriginal = _f.stInput = val);
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

						SetInfoText(-1, _f.stOriginal = _f.stInput = val);
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

						cb_08.Text = BeshadowedBlast;	//(4096) // Eldritch Essences ->
						cb_09.Text = BewitchingBlast;	//(65536)
						cb_10.Text = BindingBlast;		//(8388608)
						cb_11.Text = BrimstoneBlast;	//(8192)
						cb_12.Text = DrainingBlast;		//(256)
						cb_13.Text = FrightfulBlast;	//(1024)
						cb_14.Text = HellrimeBlast;		//(32768)
						cb_15.Text = HinderingBlast;	//(4194304)
						cb_16.Text = NoxiousBlast;		//(262144)
						cb_17.Text = UtterdarkBlast;	//(2097152)
						cb_18.Text = VitriolicBlast;	//(524288)

						cb_19.Text = EldritchChain;		//(16384) // Invocation Shapes ->
						cb_20.Text = EldritchCone;		//(131072)
						cb_21.Text = EldritchDoom;		//(1048576)
						cb_22.Text = EldritchSpear;		//(512)
						cb_23.Text = HideousBlow;		//(2048)

						if (val == Constants.Stars || val.Length < 3) val = "0x0";
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

							cb_19.Checked = ((result & YataForm.META_I_ELDRITCH_CHAIN)   != 0); // Invocation Shapes ->
							cb_20.Checked = ((result & YataForm.META_I_ELDRITCH_CONE)    != 0);
							cb_21.Checked = ((result & YataForm.META_I_ELDRITCH_DOOM)    != 0);
							cb_22.Checked = ((result & YataForm.META_I_ELDRITCH_SPEAR)   != 0);
							cb_23.Checked = ((result & YataForm.META_I_HIDEOUS_BLOW)     != 0);

						}
						SetInfoText(_f.intOriginal = _f.intInput = result);
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

						if (val == Constants.Stars || val.Length < 3) val = "0x0";
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
						SetInfoText(_f.intOriginal = _f.intInput = result);
						break;

					case ImmunityType:
						ColType = ImmunityType;
						Text = " ImmunityType";
						setComboboxVisible();
						populateImmunityTypes();

						switch (val)
						{
							case Acid:            cbx_Val.SelectedIndex =  0; break; // sub
							case Cold:            cbx_Val.SelectedIndex =  1; break; // sub
							case Death:           cbx_Val.SelectedIndex =  2; break;
							case Disease:         cbx_Val.SelectedIndex =  3; break;
							case Divine:          cbx_Val.SelectedIndex =  4; break; // sub
							case Electricity:     cbx_Val.SelectedIndex =  5; break; // sub
							case Evil:            cbx_Val.SelectedIndex =  6; break; // non-standard
							case Fear:            cbx_Val.SelectedIndex =  7; break;
							case Fire:            cbx_Val.SelectedIndex =  8; break; // sub
							case Magical:         cbx_Val.SelectedIndex =  9; break; // sub
							case Mind_Affecting:  cbx_Val.SelectedIndex = 10; break;
							case Negative:        cbx_Val.SelectedIndex = 11; break; // sub
							case Paralysis:       cbx_Val.SelectedIndex = 12; break;
							case Poison:          cbx_Val.SelectedIndex = 13; break;
							case Positive:        cbx_Val.SelectedIndex = 14; break; // sub
							case Sonic:           cbx_Val.SelectedIndex = 15; break; // sub
							case Constitution:    cbx_Val.SelectedIndex = 16; break; // non-standard
							case Water:           cbx_Val.SelectedIndex = 17; break; // non-standard

							default:
							case Constants.Stars: cbx_Val.SelectedIndex = 18; break;
						}
						_f.stOriginal = _f.stInput = val;
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

						if (val == Constants.Stars) val = "0";
						if (Int32.TryParse(val, out result)
							&& result > -1 && result < Info.categoryLabels.Count)
						{
							cbx_Val.SelectedIndex = result;
						}
						else
							cbx_Val.SelectedIndex = Info.categoryLabels.Count; // "n/a"

						_f.intOriginal = _f.intInput = result;
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
						SetInfoText(-1, _f.stOriginal = _f.stInput = val);
						break;

					case AsMetaMagic:
						ColType = AsMetaMagic;
						Text = " AsMetaMagic";
						setComboboxVisible();
						populateAsMetaMagics();

						if (val == Constants.Stars || val.Length < 3) val = "0x0";
						if (Int32.TryParse(val.Substring(2),
										   NumberStyles.AllowHexSpecifier,
										   CultureInfo.InvariantCulture,
										   out result))
						{
							switch (result)
							{
								case YataForm.META_I_BESHADOWED_BLAST: cbx_Val.SelectedIndex =  0; break;
								case YataForm.META_I_BEWITCHING_BLAST: cbx_Val.SelectedIndex =  1; break;
								case YataForm.META_I_BINDING_BLAST:    cbx_Val.SelectedIndex =  2; break;
								case YataForm.META_I_BRIMSTONE_BLAST:  cbx_Val.SelectedIndex =  3; break;
								case YataForm.META_I_DRAINING_BLAST:   cbx_Val.SelectedIndex =  4; break; // Eldritch Essences ->
								case YataForm.META_I_FRIGHTFUL_BLAST:  cbx_Val.SelectedIndex =  5; break;
								case YataForm.META_I_HELLRIME_BLAST:   cbx_Val.SelectedIndex =  6; break;
								case YataForm.META_I_HINDERING_BLAST:  cbx_Val.SelectedIndex =  7; break;
								case YataForm.META_I_NOXIOUS_BLAST:    cbx_Val.SelectedIndex =  8; break;
								case YataForm.META_I_UTTERDARK_BLAST:  cbx_Val.SelectedIndex =  9; break;
								case YataForm.META_I_VITRIOLIC_BLAST:  cbx_Val.SelectedIndex = 10; break;

								case YataForm.META_I_ELDRITCH_CHAIN:   cbx_Val.SelectedIndex = 11; break;
								case YataForm.META_I_ELDRITCH_CONE:    cbx_Val.SelectedIndex = 12; break;
								case YataForm.META_I_ELDRITCH_DOOM:    cbx_Val.SelectedIndex = 13; break;
								case YataForm.META_I_ELDRITCH_SPEAR:   cbx_Val.SelectedIndex = 14; break; // Invocation Shapes ->
								case YataForm.META_I_HIDEOUS_BLOW:     cbx_Val.SelectedIndex = 15; break;
							}
						}
						_f.intOriginal = _f.intInput = result;
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

						_f.stOriginal = _f.stInput = val;
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
				lbl_Val.Text = (val == Constants.Stars) ? String.Empty : val;
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

			cb_08.Visible = // Eldritch Essences and Invocation Shapes ->
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
			cb_23.Visible = true;
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

			cb_08.Enabled = // Eldritch Essences and Invocation Shapes ->
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
			cb_23.Enabled = (result == 0x00 || result > 0xFF);
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

		void setVisibleUserTypes()
		{
			cb_00.Visible =
			cb_01.Visible =
			cb_02.Visible =
			cb_03.Visible = true;
		}

		void setComboboxVisible()
		{
			lbl_Val.Visible = false;
			cbx_Val.Visible = true;
		}

		void populateImmunityTypes()
		{
			cbx_Val.Items.Add(new tui(Acid));
			cbx_Val.Items.Add(new tui(Cold));
			cbx_Val.Items.Add(new tui(Death));
			cbx_Val.Items.Add(new tui(Disease));
			cbx_Val.Items.Add(new tui(Divine));
			cbx_Val.Items.Add(new tui(Electricity));
			cbx_Val.Items.Add(new tui(Evil));
			cbx_Val.Items.Add(new tui(Fear));
			cbx_Val.Items.Add(new tui(Fire));
			cbx_Val.Items.Add(new tui(Magical));
			cbx_Val.Items.Add(new tui(Mind_Affecting));
			cbx_Val.Items.Add(new tui(Negative));
			cbx_Val.Items.Add(new tui(Paralysis));
			cbx_Val.Items.Add(new tui(Poison));
			cbx_Val.Items.Add(new tui(Positive));
			cbx_Val.Items.Add(new tui(Sonic));
			cbx_Val.Items.Add(new tui(Constitution));
			cbx_Val.Items.Add(new tui(Water));

			cbx_Val.Items.Add(new tui(non));
		}

		void populateCategories()
		{
			for (int i = 0; i != Info.categoryLabels.Count; ++i)
			{
				cbx_Val.Items.Add(new tui(i + " - " + Info.categoryLabels[i].ToLower()));
			}
			cbx_Val.Items.Add(new tui(non));
		}

		void populateAsMetaMagics()
		{
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_BESHADOWED_BLAST) + " - " + BeshadowedBlast)); // Eldritch Essences ->
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_BEWITCHING_BLAST) + " - " + BewitchingBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_BINDING_BLAST)    + " - " + BindingBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_BRIMSTONE_BLAST)  + " - " + BrimstoneBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_DRAINING_BLAST)   + " - " + DrainingBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_FRIGHTFUL_BLAST)  + " - " + FrightfulBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_HELLRIME_BLAST)   + " - " + HellrimeBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_HINDERING_BLAST)  + " - " + HinderingBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_NOXIOUS_BLAST)    + " - " + NoxiousBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_UTTERDARK_BLAST)  + " - " + UtterdarkBlast));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_VITRIOLIC_BLAST)  + " - " + VitriolicBlast));

			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_ELDRITCH_CHAIN)   + " - " + EldritchChain)); // Invocation Shapes ->
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_ELDRITCH_CONE)    + " - " + EldritchCone));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_ELDRITCH_DOOM)    + " - " + EldritchDoom));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_ELDRITCH_SPEAR)   + " - " + EldritchSpear));
			cbx_Val.Items.Add(new tui(hexen(YataForm.META_I_HIDEOUS_BLOW)     + " - " + HideousBlow));

			cbx_Val.Items.Add(new tui(non));
			cbx_Val.SelectedIndex = 16;
		}

		string hexen(int i) { return "0x" + i.ToString("X6"); }


		const float epsilon = 0.00001F;

		void populateTargeters()
		{
			string text;

			for (int i = 0; i != Info.targetLabels.Count; ++i)
			{
				text = i + " - " + Info.targetLabels[i];

				bool haspars = false;

				float f = Info.targetWidths[i];
				if (Math.Abs(0.0F - f) > epsilon)
				{
					haspars = true;
					text += " (" + f;
				}

				f = Info.targetLengths[i];
				if (Math.Abs(0.0F - f) > epsilon)
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
			cbx_Val.Items.Add(new tui(non));
		}
		#endregion Methods


		#region Events (override)
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
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

		void changed_School()
		{
			if (!_bypass)
			{
				_bypass = true;

				string val = Constants.Stars;
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
				SetInfoText(-1, _f.stInput = val);

				_bypass = false;
			}
		}

		void changed_Range()
		{
			if (!_bypass)
			{
				_bypass = true;

				string val = Constants.Stars;
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
				SetInfoText(-1, _f.stInput = val);

				_bypass = false;
			}
		}

		void changed_MetaMagic()
		{
			if (_cb == cb_00)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_EMPOWER;
				else             _f.intInput &= ~YataForm.META_EMPOWER;
			}
			else if (_cb == cb_01)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_EXTEND;
				else             _f.intInput &= ~YataForm.META_EXTEND;
			}
			else if (_cb == cb_02)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_MAXIMIZE;
				else             _f.intInput &= ~YataForm.META_MAXIMIZE;
			}
			else if (_cb == cb_03)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_QUICKEN;
				else             _f.intInput &= ~YataForm.META_QUICKEN;
			}
			else if (_cb == cb_04)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_SILENT;
				else             _f.intInput &= ~YataForm.META_SILENT;
			}
			else if (_cb == cb_05)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_STILL;
				else             _f.intInput &= ~YataForm.META_STILL;
			}
			else if (_cb == cb_06)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_PERSISTENT;
				else             _f.intInput &= ~YataForm.META_PERSISTENT;
			}
			else if (_cb == cb_07)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_PERMANENT;
				else             _f.intInput &= ~YataForm.META_PERMANENT;
			}

			else if (_cb == cb_08) // Eldritch Essences ->
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_BESHADOWED_BLAST;
				else             _f.intInput &= ~YataForm.META_I_BESHADOWED_BLAST;
			}
			else if (_cb == cb_09)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_BEWITCHING_BLAST;
				else             _f.intInput &= ~YataForm.META_I_BEWITCHING_BLAST;
			}
			else if (_cb == cb_10)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_BINDING_BLAST;
				else             _f.intInput &= ~YataForm.META_I_BINDING_BLAST;
			}
			else if (_cb == cb_11)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_BRIMSTONE_BLAST;
				else             _f.intInput &= ~YataForm.META_I_BRIMSTONE_BLAST;
			}
			else if (_cb == cb_12)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_DRAINING_BLAST;
				else             _f.intInput &= ~YataForm.META_I_DRAINING_BLAST;
			}
			else if (_cb == cb_13)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_FRIGHTFUL_BLAST;
				else             _f.intInput &= ~YataForm.META_I_FRIGHTFUL_BLAST;
			}
			else if (_cb == cb_14)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_HELLRIME_BLAST;
				else             _f.intInput &= ~YataForm.META_I_HELLRIME_BLAST;
			}
			else if (_cb == cb_15)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_HINDERING_BLAST;
				else             _f.intInput &= ~YataForm.META_I_HINDERING_BLAST;
			}
			else if (_cb == cb_16)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_NOXIOUS_BLAST;
				else             _f.intInput &= ~YataForm.META_I_NOXIOUS_BLAST;
			}
			else if (_cb == cb_17)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_UTTERDARK_BLAST;
				else             _f.intInput &= ~YataForm.META_I_UTTERDARK_BLAST;
			}
			else if (_cb == cb_18)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_VITRIOLIC_BLAST;
				else             _f.intInput &= ~YataForm.META_I_VITRIOLIC_BLAST;
			}

			else if (_cb == cb_19) // Invocation Shapes ->
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_ELDRITCH_CHAIN;
				else             _f.intInput &= ~YataForm.META_I_ELDRITCH_CHAIN;
			}
			else if (_cb == cb_20)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_ELDRITCH_CONE;
				else             _f.intInput &= ~YataForm.META_I_ELDRITCH_CONE;
			}
			else if (_cb == cb_21)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_ELDRITCH_DOOM;
				else             _f.intInput &= ~YataForm.META_I_ELDRITCH_DOOM;
			}
			else if (_cb == cb_22)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_ELDRITCH_SPEAR;
				else             _f.intInput &= ~YataForm.META_I_ELDRITCH_SPEAR;
			}
			else if (_cb == cb_23)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_HIDEOUS_BLOW;
				else             _f.intInput &= ~YataForm.META_I_HIDEOUS_BLOW;
			}

			SetInfoText(_f.intInput);
			EnableMetaMagics(_f.intInput);
		}

		void changed_TargetType()
		{
			if (_cb == cb_00)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_SELF;
				else             _f.intInput &= ~YataForm.TARGET_SELF;
			}
			else if (_cb == cb_01)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_CREATURE;
				else             _f.intInput &= ~YataForm.TARGET_CREATURE;
			}
			else if (_cb == cb_02)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_GROUND;
				else             _f.intInput &= ~YataForm.TARGET_GROUND;
			}
			else if (_cb == cb_03)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_ITEMS;
				else             _f.intInput &= ~YataForm.TARGET_ITEMS;
			}
			else if (_cb == cb_04)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_DOORS;
				else             _f.intInput &= ~YataForm.TARGET_DOORS;
			}
			else if (_cb == cb_05)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_PLACEABLES;
				else             _f.intInput &= ~YataForm.TARGET_PLACEABLES;
			}
			else if (_cb == cb_06)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_TRIGGERS;
				else             _f.intInput &= ~YataForm.TARGET_TRIGGERS;
			}

			SetInfoText(_f.intInput);
		}

		void changed_UserType()
		{
			if (!_bypass)
			{
				_bypass = true;

				string val = Constants.Stars;
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
				SetInfoText(-1, _f.stInput = val);

				_bypass = false;
			}
		}

		void changed_Combobox(object sender, EventArgs e)
		{
			if (!_init)
			{
				if (cbx_Val.SelectedIndex == cbx_Val.Items.Count - 1)
				{
					_f.stInput  = Constants.Stars;
					_f.intInput = 0;
				}
				else
				{
					switch (ColType)
					{
						case ImmunityType:
							switch (cbx_Val.SelectedIndex)
							{
								case  0: _f.stInput = Acid;           break;
								case  1: _f.stInput = Cold;           break;
								case  2: _f.stInput = Death;          break;
								case  3: _f.stInput = Disease;        break;
								case  4: _f.stInput = Divine;         break;
								case  5: _f.stInput = Electricity;    break;
								case  6: _f.stInput = Evil;           break;
								case  7: _f.stInput = Fear;           break;
								case  8: _f.stInput = Fire;           break;
								case  9: _f.stInput = Magical;        break;
								case 10: _f.stInput = Mind_Affecting; break;
								case 11: _f.stInput = Negative;       break;
								case 12: _f.stInput = Paralysis;      break;
								case 13: _f.stInput = Poison;         break;
								case 14: _f.stInput = Positive;       break;
								case 15: _f.stInput = Sonic;          break;
								case 16: _f.stInput = Constitution;   break;
								case 17: _f.stInput = Water;          break;
							}
							break;

						case AsMetaMagic:
							switch (cbx_Val.SelectedIndex)
							{
								case  0: _f.intInput = YataForm.META_I_BESHADOWED_BLAST; break; // Eldritch Essences ->
								case  1: _f.intInput = YataForm.META_I_BEWITCHING_BLAST; break;
								case  2: _f.intInput = YataForm.META_I_BINDING_BLAST;    break;
								case  3: _f.intInput = YataForm.META_I_BRIMSTONE_BLAST;  break;
								case  4: _f.intInput = YataForm.META_I_DRAINING_BLAST;   break;
								case  5: _f.intInput = YataForm.META_I_FRIGHTFUL_BLAST;  break;
								case  6: _f.intInput = YataForm.META_I_HELLRIME_BLAST;   break;
								case  7: _f.intInput = YataForm.META_I_HINDERING_BLAST;  break;
								case  8: _f.intInput = YataForm.META_I_NOXIOUS_BLAST;    break;
								case  9: _f.intInput = YataForm.META_I_UTTERDARK_BLAST;  break;
								case 10: _f.intInput = YataForm.META_I_VITRIOLIC_BLAST;  break;

								case 11: _f.intInput = YataForm.META_I_ELDRITCH_CHAIN;   break; // Invocation Shapes ->
								case 12: _f.intInput = YataForm.META_I_ELDRITCH_CONE;    break;
								case 13: _f.intInput = YataForm.META_I_ELDRITCH_DOOM;    break;
								case 14: _f.intInput = YataForm.META_I_ELDRITCH_SPEAR;   break;
								case 15: _f.intInput = YataForm.META_I_HIDEOUS_BLOW;     break;
							}
							break;

						default:
							_f.intInput = cbx_Val.SelectedIndex;
							break;
					}
				}
			}
		}


/*		void click_Accept(object sender, EventArgs e)
		{} */
		#endregion Events
	}


	/// <summary>
	/// A class-object for populating the 'cbx_Val' dropdown-list.
	/// @note 'tui' stands for ... "A class-object for populating the 'cbx_Val'
	/// dropdown-list".
	/// </summary>
	sealed class tui
	{
		/// <summary>
		/// Appears as the text of this item.
		/// </summary>
		string Label
		{ get; set; }

		/// <summary>
		/// Constructs an item for populating the 'cbx_Val' dropdown-list.
		/// </summary>
		/// <param name="label"></param>
		internal tui(string label)
		{
			Label = label;
		}

		/// <summary>
		/// Required.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Label;
		}
	}
}
