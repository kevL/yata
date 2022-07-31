﻿using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// This dialog has a variety of initial configurations. It loads either as
	/// a series of checkboxes that might be either actual checkboxes that
	/// allow the user to select multiple choices or checkboxes that act like
	/// radiobuttons if only a unique choice is allowed; alternately the dialog
	/// can load as a <c>ComboBox</c> with a list of unique choices. The return
	/// to Yata can be a <c>string</c>, an <c>int</c>, or a hexadecimal
	/// <c>int</c>.
	/// 
	/// 
	/// Two <c><see cref="Yata"/></c> variables shall be initialized: either
	/// <c><see cref="Yata.int0"/></c> and <c><see cref="Yata.int1"/></c> or
	/// <c><see cref="Yata.str0"/></c> and <c><see cref="Yata.str1"/></c>
	/// according to whether the return will be an <c>int</c> or a
	/// <c>string</c>. <c>int0</c> or <c>str0</c> is the initial value that was
	/// passed into this dialog; <c>int1</c> or <c>str1</c> will be the return
	/// value to <c>Yata</c> itself. Two conditions must be met before
	/// <c>Yata</c> does anything with a return: (1) the user must click the
	/// Accept button (2) the return value must be different than the value that
	/// was passed into this dialog.
	/// 
	/// 
	/// If a value is passed in from <c>Yata</c> that this dialog does not
	/// recognize as valid (ie. the value is not listed as any of the choices
	/// that user can select) then default values shall be assigned to the
	/// <c>0</c> and <c>1</c> variables. <c>str0</c> and <c>str1</c> are
	/// initialized with the passed in value unless the passed in value is
	/// considered invalid, for which <c>str1</c> will be initialzed to
	/// <c>****</c>; if not the user needs to choose a different string-value to
	/// return or else <c>Yata</c> won't bother with it.
	/// 
	/// 
	/// Integer returns, however, are trickier. Very tricky ...
	/// 
	/// 
	/// The value displayed at the top of a checkbox-configuration shall be the
	/// value that will be returned to <c>Yata</c> iff user clicks the Accept
	/// button. A <c>ComboBox</c> configuration displays the value that will be
	/// returned (iff user clicks the Accept button) in the <c>ComboBox</c>
	/// itself.
	/// </summary>
	/// <seealso cref="InfoInputFeat"><c>InfoInputFeat</c></seealso>
	/// <seealso cref="InfoInputClasses"><c>InfoInputClasses</c></seealso>
	sealed partial class InfoInputSpells
		: InfoInputDialog
	{
		#region Fields (static)
		// cols in Spells.2da ->
		internal const int IconResRef        =  3; // ofd only (OpenFileDialog)
		internal const int School            =  4;
		internal const int Range             =  5;
		internal const int MetaMagic         =  7;
		internal const int TargetType        =  8;
		internal const int ImpactScript      =  9; // ofd only
		internal const int ConjVisual0       = 20; // ofd only
		internal const int LowConjVisual0    = 21; // ofd only
		internal const int ConjVisual1       = 22; // ofd only
		internal const int ConjVisual2       = 23; // ofd only
		internal const int ConjSoundVFX      = 24; // ofd only
		internal const int ConjSoundMale     = 25; // ofd only
		internal const int ConjSoundFemale   = 26; // ofd only
		internal const int CastVisual0       = 30; // ofd only
		internal const int LowCastVisual0    = 31; // ofd only
		internal const int CastVisual1       = 32; // ofd only
		internal const int CastVisual2       = 33; // ofd only
		internal const int CastSound         = 34; // ofd only
		internal const int ProjSEF           = 37; // ofd only
		internal const int LowProjSEF        = 38; // ofd only
		internal const int ImpactSEF         = 43; // ofd only
		internal const int LowImpactSEF      = 44; // ofd only
		internal const int ImmunityType      = 45;
		internal const int Category          = 52;
		internal const int UserType          = 54;
		internal const int SpontCastClassReq = 58;
		internal const int AsMetaMagic       = 65;
		internal const int TargetingUI       = 66;
		#endregion Fields (static)


		#region cTor
		/// <summary>
		/// A dialog for the user to input <c>Spells.2da</c> info.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="cell"></param>
		internal InfoInputSpells(Yata f, Cell cell)
		{
			_f    = f;		// don't try to pass these to a InfoInputDialog.cTor
			_cell = cell;	// because the designer will scream blue murder.

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
				case School: // string-val,checkbox,unique
					_f.str0 = _f.str1 = val;
					prep_Schools();

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

						case gs.Stars: break;

						default: _f.str1 = gs.Stars; break;
					}
					bu_Clear.Enabled = ((la_Val.Text = _f.str1) != gs.Stars);
					break;

				case Range: // string-val,checkbox,unique
					_f.str0 = _f.str1 = val;
					prep_Ranges();

					switch (val)
					{
						case "P": cb_00.Checked = true; break;
						case "T": cb_01.Checked = true; break;
						case "S": cb_02.Checked = true; break;
						case "M": cb_03.Checked = true; break;
						case "L": cb_04.Checked = true; break;
						case "I": cb_05.Checked = true; break;

						case gs.Stars: break;

						default: _f.str1 = gs.Stars; break;
					}
					bu_Clear.Enabled = ((la_Val.Text = _f.str1) != gs.Stars);
					break;

				case MetaMagic: // int-val(hex),checkbox,multiple
					// NOTE: Types that bitwise multiple values shall assign
					// a default value of "0x00" instead of the usual "****"
					// when the value passed into this dialog is considered
					// invalid.

					prep_MetaMagics();

					if (val.Length > 2 && val.Substring(0,2) == "0x"
						&& Int32.TryParse(val.Substring(2),
										  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
										  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
										  out result))
					{
						cb_00.Checked = ((result & Yata.META_EMPOWER)            != 0); // standard ->
						cb_01.Checked = ((result & Yata.META_EXTEND)             != 0);
						cb_02.Checked = ((result & Yata.META_MAXIMIZE)           != 0);
						cb_03.Checked = ((result & Yata.META_QUICKEN)            != 0);
						cb_04.Checked = ((result & Yata.META_SILENT)             != 0);
						cb_05.Checked = ((result & Yata.META_STILL)              != 0);
						cb_06.Checked = ((result & Yata.META_PERSISTENT)         != 0);
						cb_07.Checked = ((result & Yata.META_PERMANENT)          != 0);

						cb_08.Checked = ((result & Yata.META_I_BESHADOWED_BLAST) != 0); // Eldritch Essences ->
						cb_09.Checked = ((result & Yata.META_I_BEWITCHING_BLAST) != 0);
						cb_10.Checked = ((result & Yata.META_I_BINDING_BLAST)    != 0);
						cb_11.Checked = ((result & Yata.META_I_BRIMSTONE_BLAST)  != 0);
						cb_12.Checked = ((result & Yata.META_I_DRAINING_BLAST)   != 0);
						cb_13.Checked = ((result & Yata.META_I_FRIGHTFUL_BLAST)  != 0);
						cb_14.Checked = ((result & Yata.META_I_HELLRIME_BLAST)   != 0);
						cb_15.Checked = ((result & Yata.META_I_HINDERING_BLAST)  != 0);
						cb_16.Checked = ((result & Yata.META_I_NOXIOUS_BLAST)    != 0);
						cb_17.Checked = ((result & Yata.META_I_UTTERDARK_BLAST)  != 0);
						cb_18.Checked = ((result & Yata.META_I_VITRIOLIC_BLAST)  != 0);

						cb_19.Checked = ((result & Yata.META_I_ELDRITCH_CHAIN)   != 0); // Blast Shapes ->
						cb_20.Checked = ((result & Yata.META_I_ELDRITCH_CONE)    != 0);
						cb_21.Checked = ((result & Yata.META_I_ELDRITCH_DOOM)    != 0);
						cb_22.Checked = ((result & Yata.META_I_ELDRITCH_SPEAR)   != 0);
						cb_23.Checked = ((result & Yata.META_I_HIDEOUS_BLOW)     != 0);

						ResolveMetagroups(result);

						// TODO: There is an issue. If an unconventional value is passed
						// in from Yata it could have bits for both standard metamagic
						// and invocation metamagic which should be disallowed here.
						// The initialization routine will then check disabled checkboxes,
						// which should never happen ...
						//
						// It's not a major concern but it's definitely awkward. The
						// problem is I'd have to ask the user which set he/she wants
						// to keep: standard or invocation ... and I don't want to
						// set that up.

						if ((result & ~(Yata.META_STANDARD | Yata.META_I_ALL)) != 0) // invalid - bits outside allowed range
						{
							_f.int0 = result;
							// crop 'result' so 'int1' differs from 'int0' ->
							printHexString(_f.int1 = (result &= (Yata.META_STANDARD | Yata.META_I_ALL)));
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

				case TargetType: // int-val(hex),checkbox,multiple
					// NOTE: Types that bitwise multiple values shall assign
					// a default value of "0x00" instead of the usual "****"
					// when the value passed into this dialog is considered
					// invalid.

					prep_TargetTypes();

					if (val.Length > 2 && val.Substring(0,2) == "0x"
						&& Int32.TryParse(val.Substring(2),
										  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
										  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
										  out result))
					{
						cb_00.Checked = ((result & Yata.TARGET_SELF)       != 0);
						cb_01.Checked = ((result & Yata.TARGET_CREATURE)   != 0);
						cb_02.Checked = ((result & Yata.TARGET_GROUND)     != 0);
						cb_03.Checked = ((result & Yata.TARGET_ITEMS)      != 0);
						cb_04.Checked = ((result & Yata.TARGET_DOORS)      != 0);
						cb_05.Checked = ((result & Yata.TARGET_PLACEABLES) != 0);
						cb_06.Checked = ((result & Yata.TARGET_TRIGGERS)   != 0);

						if ((result & ~Yata.TARGET_TOTAL) != 0) // invalid - bits outside allowed range
						{
							_f.int0 = result;
							printHexString(_f.int1 = (result &= Yata.TARGET_TOTAL));
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

				case ImmunityType: // string-val,dropdown,unique
					// NOTE: ImmunityTypes are not used by the NwN2 engine but their
					// string-values can be accessed by script regardless. What follows
					// are generally accepted values but they do not correspond exactly
					// to the IMMUNITY_TYPE_* constants in NwScript.nss.

					_f.str0 = _f.str1 = val;
					list_ImmunityTypes();

					switch (val)
					{
						case gs.Acid:           co_Val.SelectedIndex =  0; break; // sub
						case gs.Cold:           co_Val.SelectedIndex =  1; break; // sub
						case gs.Death:          co_Val.SelectedIndex =  2; break;
						case gs.Disease:        co_Val.SelectedIndex =  3; break;
						case gs.Divine:         co_Val.SelectedIndex =  4; break; // sub
						case gs.Electricity:    co_Val.SelectedIndex =  5; break; // sub
						case gs.Evil:           co_Val.SelectedIndex =  6; break; // non-standard
						case gs.Fear:           co_Val.SelectedIndex =  7; break;
						case gs.Fire:           co_Val.SelectedIndex =  8; break; // sub
						case gs.Magical:        co_Val.SelectedIndex =  9; break; // sub
						case gs.Mind_Affecting: co_Val.SelectedIndex = 10; break;
						case gs.Negative:       co_Val.SelectedIndex = 11; break; // sub
						case gs.Paralysis:      co_Val.SelectedIndex = 12; break;
						case gs.Poison:         co_Val.SelectedIndex = 13; break;
						case gs.Positive:       co_Val.SelectedIndex = 14; break; // sub
						case gs.Sonic:          co_Val.SelectedIndex = 15; break; // sub
						case gs.Constitution:   co_Val.SelectedIndex = 16; break; // non-standard
						case gs.Water:          co_Val.SelectedIndex = 17; break; // non-standard

						case gs.Stars: co_Val.SelectedIndex = co_Val.Items.Count - 1;
							break;

						default: _f.str1 = gs.Stars; goto case gs.Stars;
					}
					bu_Clear.Enabled = ((la_Val.Text = _f.str1) != gs.Stars);
					break;

				case Category: // int-val,dropdown,unique
					list_Categories();
					initintvals(val, co_Val, bu_Clear);
					break;

				case UserType: // string-val,checkbox,unique // TODO: change 'UserType' selection to int-val,dropdown,unique
					_f.str0 = _f.str1 = val;
					prep_UserTypes();

					switch (val)
					{
						case "1": cb_00.Checked = true; break;
						case "2": cb_01.Checked = true; break;
						case "3": cb_02.Checked = true; break;
						case "4": cb_03.Checked = true; break;

						case gs.Stars: break;

						default: _f.str1 = gs.Stars; break;
					}
					bu_Clear.Enabled = ((la_Val.Text = _f.str1) != gs.Stars);
					break;

				case SpontCastClassReq: // int-val,dropdown,unique
					list_SpontCastClasses();
					initintvals(val, co_Val, bu_Clear);
					break;

				case AsMetaMagic: // int-val(hex),dropdown,unique
					list_AsMetaMagics();

					co_Val.SelectedIndex = co_Val.Items.Count - 1;

					if (val == gs.Stars)
					{
						_f.int0 = _f.int1 = Yata.Info_ASSIGN_STARS;
						bu_Clear.Enabled = false;
						break;
					}

					if (val.Length > 2 && val.Substring(0,2) == "0x"
						&& Int32.TryParse(val.Substring(2),
										  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
										  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
										  out result))
					{
						switch (result)
						{
							case Yata.META_I_BESHADOWED_BLAST: co_Val.SelectedIndex =  0; break; // Eldritch Essences ->
							case Yata.META_I_BEWITCHING_BLAST: co_Val.SelectedIndex =  1; break;
							case Yata.META_I_BINDING_BLAST:    co_Val.SelectedIndex =  2; break;
							case Yata.META_I_BRIMSTONE_BLAST:  co_Val.SelectedIndex =  3; break;
							case Yata.META_I_DRAINING_BLAST:   co_Val.SelectedIndex =  4; break;
							case Yata.META_I_FRIGHTFUL_BLAST:  co_Val.SelectedIndex =  5; break;
							case Yata.META_I_HELLRIME_BLAST:   co_Val.SelectedIndex =  6; break;
							case Yata.META_I_HINDERING_BLAST:  co_Val.SelectedIndex =  7; break;
							case Yata.META_I_NOXIOUS_BLAST:    co_Val.SelectedIndex =  8; break;
							case Yata.META_I_UTTERDARK_BLAST:  co_Val.SelectedIndex =  9; break;
							case Yata.META_I_VITRIOLIC_BLAST:  co_Val.SelectedIndex = 10; break;

							case Yata.META_I_ELDRITCH_CHAIN:   co_Val.SelectedIndex = 11; break; // Blast Shapes ->
							case Yata.META_I_ELDRITCH_CONE:    co_Val.SelectedIndex = 12; break;
							case Yata.META_I_ELDRITCH_DOOM:    co_Val.SelectedIndex = 13; break;
							case Yata.META_I_ELDRITCH_SPEAR:   co_Val.SelectedIndex = 14; break;
							case Yata.META_I_HIDEOUS_BLOW:     co_Val.SelectedIndex = 15; break;
						}

						if (co_Val.SelectedIndex != co_Val.Items.Count - 1)
						{
							_f.int0 = _f.int1 = result;
							break;
						}
					}

					_f.int0 = Yata.Info_INIT_INVALID;
					_f.int1 = Yata.Info_ASSIGN_STARS;
					break;

				case TargetingUI: // int-val,dropdown,unique
					list_Targeters();
					initintvals(val, co_Val, bu_Clear);
					break;
			}
			_init = false;
		}


		/// <summary>
		/// Prepares this dialog for <c><see cref="School"/></c> input.
		/// </summary>
		void prep_Schools()
		{
			Text = "  School";

			cb_00.Text = "Abjuration";
			cb_01.Text = "Conjuration";
			cb_02.Text = "Divination";
			cb_03.Text = "Enchantment";
			cb_04.Text = "Illusion";
			cb_05.Text = "Necromancy";
			cb_06.Text = "Transmutation";
			cb_07.Text = "Evocation";

			cb_00.Visible = cb_01.Visible = cb_02.Visible = cb_03.Visible =
			cb_04.Visible = cb_05.Visible = cb_06.Visible = cb_07.Visible = true;
		}

		/// <summary>
		/// Prepares this dialog for <c><see cref="Range"/></c> input.
		/// </summary>
		void prep_Ranges()
		{
			Text = "  Range";

			cb_00.Text = "Personal" + (( 0 < Info.rangeRanges.Count) ? (gs.Space + Info.rangeRanges[ 0] + "m") : String.Empty);
			cb_01.Text = "Touch"    + (( 1 < Info.rangeRanges.Count) ? (gs.Space + Info.rangeRanges[ 1] + "m") : String.Empty);
			cb_02.Text = "Short"    + (( 2 < Info.rangeRanges.Count) ? (gs.Space + Info.rangeRanges[ 2] + "m") : String.Empty);
			cb_03.Text = "Medium"   + (( 3 < Info.rangeRanges.Count) ? (gs.Space + Info.rangeRanges[ 3] + "m") : String.Empty);
			cb_04.Text = "Long"     + (( 4 < Info.rangeRanges.Count) ? (gs.Space + Info.rangeRanges[ 4] + "m") : String.Empty);
			cb_05.Text = "Infinite" + ((14 < Info.rangeRanges.Count) ? (gs.Space + Info.rangeRanges[14] + "m") : String.Empty);

			cb_00.Visible = cb_01.Visible = cb_02.Visible = cb_03.Visible =
			cb_04.Visible = cb_05.Visible = true;
		}

		/// <summary>
		/// Prepares this dialog for <c><see cref="MetaMagic"/></c> input.
		/// </summary>
		void prep_MetaMagics()
		{
			Text = "  MetaMagic";

			// standard ->
			cb_00.Text = "(1)Empower";
			cb_01.Text = "(2)Extend";
			cb_02.Text = "(4)Maximize";
			cb_03.Text = "(8)Quicken";
			cb_04.Text = "(16)Silent";
			cb_05.Text = "(32)Still";
			cb_06.Text = "(64)Persistent";
			cb_07.Text = "(128)Permanent";

			// Eldritch Essences and Blast Shapes ->
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

			// standard ->
			cb_00.Visible = cb_01.Visible = cb_02.Visible = cb_03.Visible =
			cb_04.Visible = cb_05.Visible = cb_06.Visible = cb_07.Visible =

			// Eldritch Essences and Blast Shapes ->
			cb_08.Visible = cb_09.Visible = cb_10.Visible = cb_11.Visible =
			cb_12.Visible = cb_13.Visible = cb_14.Visible = cb_15.Visible =
			cb_16.Visible = cb_17.Visible = cb_18.Visible = cb_19.Visible =
			cb_20.Visible = cb_21.Visible = cb_22.Visible = cb_23.Visible =

			gb_MetaGroups.Visible = true;
		}

		/// <summary>
		/// Prepares this dialog for <c><see cref="TargetType"/></c> input.
		/// </summary>
		void prep_TargetTypes()
		{
			Text = "  TargetType";

			cb_00.Text = "(1)Self";
			cb_01.Text = "(2)Creatures";
			cb_02.Text = "(4)Ground";
			cb_03.Text = "(8)Items";
			cb_04.Text = "(16)Doors";
			cb_05.Text = "(32)Placeables";
			cb_06.Text = "(64)Triggers";

			cb_00.Visible = cb_01.Visible = cb_02.Visible = cb_03.Visible =
			cb_04.Visible = cb_05.Visible = cb_06.Visible = true;
		}

		/// <summary>
		/// Prepares this dialog for <c><see cref="UserType"/></c> input.
		/// </summary>
		void prep_UserTypes()
		{
			Text = "  UserType";

			cb_00.Text = "1 - Spell";
			cb_01.Text = "2 - Special Ability";
			cb_02.Text = "3 - Feat";
			cb_03.Text = "4 - Item Power";

			cb_00.Visible = cb_01.Visible = cb_02.Visible = cb_03.Visible = true;
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
								  ClientSize.Height - 20 * 8);
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="ImmunityType"/></c> to the
		/// <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_ImmunityTypes()
		{
			Text = "  ImmunityType";

			dropdown();

			co_Val.Items.Add(new tui(gs.Acid));
			co_Val.Items.Add(new tui(gs.Cold));
			co_Val.Items.Add(new tui(gs.Death));
			co_Val.Items.Add(new tui(gs.Disease));
			co_Val.Items.Add(new tui(gs.Divine));
			co_Val.Items.Add(new tui(gs.Electricity));
			co_Val.Items.Add(new tui(gs.Evil));
			co_Val.Items.Add(new tui(gs.Fear));
			co_Val.Items.Add(new tui(gs.Fire));
			co_Val.Items.Add(new tui(gs.Magical));
			co_Val.Items.Add(new tui(gs.Mind_Affecting));
			co_Val.Items.Add(new tui(gs.Negative));
			co_Val.Items.Add(new tui(gs.Paralysis));
			co_Val.Items.Add(new tui(gs.Poison));
			co_Val.Items.Add(new tui(gs.Positive));
			co_Val.Items.Add(new tui(gs.Sonic));
			co_Val.Items.Add(new tui(gs.Constitution));
			co_Val.Items.Add(new tui(gs.Water));

			co_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="Category"/></c>
		/// (Categories.2da) to the <c>ComboBox</c> along with a final stars
		/// item.
		/// </summary>
		void list_Categories()
		{
			Text = "  Category";

			dropdown();

			for (int i = 0; i != Info.categoryLabels.Count; ++i)
			{
				co_Val.Items.Add(new tui(i + " - " + Info.categoryLabels[i].ToLowerInvariant()));
			}
			co_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="SpontCastClassReq"/></c>
		/// (Classes.2da) to the <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_SpontCastClasses()
		{
			Text = "  SpontCastClassReq";

			dropdown();

			for (int i = 0; i != Info.classLabels.Count; ++i)
			{
				co_Val.Items.Add(new tui(i + " - " + Info.classLabels[i]));
			}
			co_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="AsMetaMagic"/></c> to the
		/// <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_AsMetaMagics()
		{
			Text = "  AsMetaMagic";

			dropdown();

			co_Val.Items.Add(new tui(toHexString(Yata.META_I_BESHADOWED_BLAST) + " - " + gs.BeshadowedBlast)); // Eldritch Essences ->
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_BEWITCHING_BLAST) + " - " + gs.BewitchingBlast));
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_BINDING_BLAST)    + " - " + gs.BindingBlast));
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_BRIMSTONE_BLAST)  + " - " + gs.BrimstoneBlast));
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_DRAINING_BLAST)   + " - " + gs.DrainingBlast));
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_FRIGHTFUL_BLAST)  + " - " + gs.FrightfulBlast));
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_HELLRIME_BLAST)   + " - " + gs.HellrimeBlast));
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_HINDERING_BLAST)  + " - " + gs.HinderingBlast));
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_NOXIOUS_BLAST)    + " - " + gs.NoxiousBlast));
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_UTTERDARK_BLAST)  + " - " + gs.UtterdarkBlast));
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_VITRIOLIC_BLAST)  + " - " + gs.VitriolicBlast));

			co_Val.Items.Add(new tui(toHexString(Yata.META_I_ELDRITCH_CHAIN)   + " - " + gs.EldritchChain)); // Blast Shapes ->
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_ELDRITCH_CONE)    + " - " + gs.EldritchCone));
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_ELDRITCH_DOOM)    + " - " + gs.EldritchDoom));
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_ELDRITCH_SPEAR)   + " - " + gs.EldritchSpear));
			co_Val.Items.Add(new tui(toHexString(Yata.META_I_HIDEOUS_BLOW)     + " - " + gs.HideousBlow));

			co_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// - helper for <c><see cref="list_AsMetaMagics()">list_AsMetaMagics()</see></c>
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		static string toHexString(int bits) { return "0x" + bits.ToString("X6", CultureInfo.InvariantCulture); }


		/// <summary>
		/// Adds allowable entries for <c><see cref="TargetingUI"/></c>
		/// (SpellTarget.2da) to the <c>ComboBox</c> along with a final stars
		/// item.
		/// </summary>
		void list_Targeters()
		{
			Text = "  TargetingUI";

			dropdown();

			string text;
			for (int i = 0; i != Info.targetLabels.Count; ++i)
			{
				text = i + " - " + Info.targetLabels[i];

				bool haspars = false;

				float f = Info.targetWidths[i];
				if (Math.Abs(0.0F - f) > gc.epsilon)
				{
					haspars = true;
					text += " (" + f;
				}

				f = Info.targetLengths[i];
				if (Math.Abs(0.0F - f) > gc.epsilon)
				{
					if (!haspars)
					{
						haspars = true;
						text += " (_";
					}
					text += " x " + f;
				}

				if (haspars) text += ")";

				co_Val.Items.Add(new tui(text));
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
					case School:     change_School();     break;
					case Range:      change_Range();      break;
					case MetaMagic:  change_MetaMagic();  break;
					case TargetType: change_TargetType(); break;
					case UserType:   change_UserType();   break;
				}
			}
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>
		/// </summary>
		void change_School()
		{
			clearchecks();

			string val;
			if (_cb.Checked)
			{
				if      (_cb == cb_00) val = "A";
				else if (_cb == cb_01) val = "C";
				else if (_cb == cb_02) val = "D";
				else if (_cb == cb_03) val = "E";
				else if (_cb == cb_04) val = "I";
				else if (_cb == cb_05) val = "N";
				else if (_cb == cb_06) val = "T";
				else                   val = "V"; // _cb == cb_07
			}
			else
				val = gs.Stars;

			la_Val.Text = _f.str1 = val;
			bu_Clear.Enabled = (val != gs.Stars);
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>
		/// </summary>
		void change_Range()
		{
			clearchecks();

			string val;
			if (_cb.Checked)
			{
				if      (_cb == cb_00) val = "P";
				else if (_cb == cb_01) val = "T";
				else if (_cb == cb_02) val = "S";
				else if (_cb == cb_03) val = "M";
				else if (_cb == cb_04) val = "L";
				else                   val = "I"; // _cb == cb_05
			}
			else
				val = gs.Stars;

			la_Val.Text = _f.str1 = val;
			bu_Clear.Enabled = (val != gs.Stars);
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>
		/// </summary>
		void change_MetaMagic()
		{
			if (_cb == cb_00) // standard ->
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_EMPOWER;
				else             _f.int1 &= ~Yata.META_EMPOWER;
			}
			else if (_cb == cb_01)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_EXTEND;
				else             _f.int1 &= ~Yata.META_EXTEND;
			}
			else if (_cb == cb_02)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_MAXIMIZE;
				else             _f.int1 &= ~Yata.META_MAXIMIZE;
			}
			else if (_cb == cb_03)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_QUICKEN;
				else             _f.int1 &= ~Yata.META_QUICKEN;
			}
			else if (_cb == cb_04)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_SILENT;
				else             _f.int1 &= ~Yata.META_SILENT;
			}
			else if (_cb == cb_05)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_STILL;
				else             _f.int1 &= ~Yata.META_STILL;
			}
			else if (_cb == cb_06)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_PERSISTENT;
				else             _f.int1 &= ~Yata.META_PERSISTENT;
			}
			else if (_cb == cb_07)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_PERMANENT;
				else             _f.int1 &= ~Yata.META_PERMANENT;
			}

			else if (_cb == cb_08) // Eldritch Essences ->
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_BESHADOWED_BLAST;
				else             _f.int1 &= ~Yata.META_I_BESHADOWED_BLAST;
			}
			else if (_cb == cb_09)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_BEWITCHING_BLAST;
				else             _f.int1 &= ~Yata.META_I_BEWITCHING_BLAST;
			}
			else if (_cb == cb_10)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_BINDING_BLAST;
				else             _f.int1 &= ~Yata.META_I_BINDING_BLAST;
			}
			else if (_cb == cb_11)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_BRIMSTONE_BLAST;
				else             _f.int1 &= ~Yata.META_I_BRIMSTONE_BLAST;
			}
			else if (_cb == cb_12)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_DRAINING_BLAST;
				else             _f.int1 &= ~Yata.META_I_DRAINING_BLAST;
			}
			else if (_cb == cb_13)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_FRIGHTFUL_BLAST;
				else             _f.int1 &= ~Yata.META_I_FRIGHTFUL_BLAST;
			}
			else if (_cb == cb_14)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_HELLRIME_BLAST;
				else             _f.int1 &= ~Yata.META_I_HELLRIME_BLAST;
			}
			else if (_cb == cb_15)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_HINDERING_BLAST;
				else             _f.int1 &= ~Yata.META_I_HINDERING_BLAST;
			}
			else if (_cb == cb_16)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_NOXIOUS_BLAST;
				else             _f.int1 &= ~Yata.META_I_NOXIOUS_BLAST;
			}
			else if (_cb == cb_17)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_UTTERDARK_BLAST;
				else             _f.int1 &= ~Yata.META_I_UTTERDARK_BLAST;
			}
			else if (_cb == cb_18)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_VITRIOLIC_BLAST;
				else             _f.int1 &= ~Yata.META_I_VITRIOLIC_BLAST;
			}

			else if (_cb == cb_19) // Blast Shapes ->
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_ELDRITCH_CHAIN;
				else             _f.int1 &= ~Yata.META_I_ELDRITCH_CHAIN;
			}
			else if (_cb == cb_20)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_ELDRITCH_CONE;
				else             _f.int1 &= ~Yata.META_I_ELDRITCH_CONE;
			}
			else if (_cb == cb_21)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_ELDRITCH_DOOM;
				else             _f.int1 &= ~Yata.META_I_ELDRITCH_DOOM;
			}
			else if (_cb == cb_22)
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_ELDRITCH_SPEAR;
				else             _f.int1 &= ~Yata.META_I_ELDRITCH_SPEAR;
			}
			else // _cb == cb_23
			{
				if (_cb.Checked) _f.int1 |=  Yata.META_I_HIDEOUS_BLOW;
				else             _f.int1 &= ~Yata.META_I_HIDEOUS_BLOW;
			}

			_init = true;
			ResolveMetagroups(_f.int1);
			_init = false;

			printHexString(_f.int1);
			bu_Clear.Enabled = (_f.int1 != 0);
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>
		/// </summary>
		void change_TargetType()
		{
			if (_cb == cb_00)
			{
				if (_cb.Checked) _f.int1 |=  Yata.TARGET_SELF;
				else             _f.int1 &= ~Yata.TARGET_SELF;
			}
			else if (_cb == cb_01)
			{
				if (_cb.Checked) _f.int1 |=  Yata.TARGET_CREATURE;
				else             _f.int1 &= ~Yata.TARGET_CREATURE;
			}
			else if (_cb == cb_02)
			{
				if (_cb.Checked) _f.int1 |=  Yata.TARGET_GROUND;
				else             _f.int1 &= ~Yata.TARGET_GROUND;
			}
			else if (_cb == cb_03)
			{
				if (_cb.Checked) _f.int1 |=  Yata.TARGET_ITEMS;
				else             _f.int1 &= ~Yata.TARGET_ITEMS;
			}
			else if (_cb == cb_04)
			{
				if (_cb.Checked) _f.int1 |=  Yata.TARGET_DOORS;
				else             _f.int1 &= ~Yata.TARGET_DOORS;
			}
			else if (_cb == cb_05)
			{
				if (_cb.Checked) _f.int1 |=  Yata.TARGET_PLACEABLES;
				else             _f.int1 &= ~Yata.TARGET_PLACEABLES;
			}
			else // _cb == cb_06
			{
				if (_cb.Checked) _f.int1 |=  Yata.TARGET_TRIGGERS;
				else             _f.int1 &= ~Yata.TARGET_TRIGGERS;
			}

			printHexString(_f.int1);

			bu_Clear.Enabled = (_f.int1 != 0);
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>
		/// </summary>
		void change_UserType()
		{
			clearchecks();

			string val;
			if (_cb.Checked)
			{
				if      (_cb == cb_00) val = "1";
				else if (_cb == cb_01) val = "2";
				else if (_cb == cb_02) val = "3";
				else                   val = "4"; // _cb == cb_03
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
						case ImmunityType:
							switch (co_Val.SelectedIndex)
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

						case AsMetaMagic:
							switch (co_Val.SelectedIndex)
							{
								case  0: _f.int1 = Yata.META_I_BESHADOWED_BLAST; break; // Eldritch Essences ->
								case  1: _f.int1 = Yata.META_I_BEWITCHING_BLAST; break;
								case  2: _f.int1 = Yata.META_I_BINDING_BLAST;    break;
								case  3: _f.int1 = Yata.META_I_BRIMSTONE_BLAST;  break;
								case  4: _f.int1 = Yata.META_I_DRAINING_BLAST;   break;
								case  5: _f.int1 = Yata.META_I_FRIGHTFUL_BLAST;  break;
								case  6: _f.int1 = Yata.META_I_HELLRIME_BLAST;   break;
								case  7: _f.int1 = Yata.META_I_HINDERING_BLAST;  break;
								case  8: _f.int1 = Yata.META_I_NOXIOUS_BLAST;    break;
								case  9: _f.int1 = Yata.META_I_UTTERDARK_BLAST;  break;
								case 10: _f.int1 = Yata.META_I_VITRIOLIC_BLAST;  break;

								case 11: _f.int1 = Yata.META_I_ELDRITCH_CHAIN;   break; // Blast Shapes ->
								case 12: _f.int1 = Yata.META_I_ELDRITCH_CONE;    break;
								case 13: _f.int1 = Yata.META_I_ELDRITCH_DOOM;    break;
								case 14: _f.int1 = Yata.META_I_ELDRITCH_SPEAR;   break;
								case 15: _f.int1 = Yata.META_I_HIDEOUS_BLOW;     break;
							}
							break;

						case Category:
						case SpontCastClassReq:
						case TargetingUI:
							_f.int1 = co_Val.SelectedIndex;
							break;
					}
				}
			}
		}


		/// <summary>
		/// Handles changing an invocation MetaMagic group <c>CheckBox</c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void changed_MetamagicGroup(object sender, EventArgs e)
		{
			if (!_init)
			{
				_init = true;

				var cb = sender as CheckBox;

				if (cb == cb_MetaEssenceShape || cb == cb_MetaEssence)
				{
					if (cb_08.Checked = cb_09.Checked = cb_10.Checked = cb_11.Checked =
						cb_12.Checked = cb_13.Checked = cb_14.Checked = cb_15.Checked =
						cb_16.Checked = cb_17.Checked = cb_18.Checked = cb.Checked)
					{
						_f.int1 |=  Yata.META_I_ESSENCES;
					}
					else
						_f.int1 &= ~Yata.META_I_ESSENCES;
				}

				if (cb == cb_MetaEssenceShape || cb == cb_MetaShape)
				{
					if (cb_19.Checked = cb_20.Checked = cb_21.Checked =
						cb_22.Checked = cb_23.Checked = cb.Checked)
					{
						_f.int1 |=  Yata.META_I_SHAPES;
					}
					else
						_f.int1 &= ~Yata.META_I_SHAPES;
				}

				ResolveMetagroups(_f.int1);
				_init = false;

				printHexString(_f.int1);
				bu_Clear.Enabled = (_f.int1 != 0);
			}
		}


		/// <summary>
		/// Determines which set of MetaMagic <c>CheckBoxes</c> should be
		/// enabled (standard or invocation) and checks invocation-group
		/// <c>Checkboxes</c> if applicable.
		/// </summary>
		/// <param name="result"></param>
		void ResolveMetagroups(int result)
		{
			// standard ->
			cb_00.Enabled = cb_01.Enabled = cb_02.Enabled = cb_03.Enabled =
			cb_04.Enabled = cb_05.Enabled = cb_06.Enabled = cb_07.Enabled = (result <= 0xFF);

			// Eldritch Essences and Blast Shapes ->
			cb_08.Enabled = cb_09.Enabled = cb_10.Enabled = cb_11.Enabled =
			cb_12.Enabled = cb_13.Enabled = cb_14.Enabled = cb_15.Enabled =
			cb_16.Enabled = cb_17.Enabled = cb_18.Enabled = cb_19.Enabled =
			cb_20.Enabled = cb_21.Enabled = cb_22.Enabled = cb_23.Enabled =

			gb_MetaGroups.Enabled = (result == 0x00 || result > 0xFF);


			cb_MetaEssenceShape.Checked = ((result & Yata.META_I_ALL)      == Yata.META_I_ALL);
			cb_MetaEssence     .Checked = ((result & Yata.META_I_ESSENCES) == Yata.META_I_ESSENCES);
			cb_MetaShape       .Checked = ((result & Yata.META_I_SHAPES)   == Yata.META_I_SHAPES);
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
				case School: // str,cb,unique
				case Range:
				case UserType:
					bu_Clear.Enabled = false;

					la_Val.Text = _f.str1 = gs.Stars;

					_cb = null;
					clearchecks();
					break;

				case MetaMagic: // hex,cb,multiple
					bu_Clear.Enabled = false;

					_cb = null;
					clearchecks();

					_init = true;
					ResolveMetagroups(_f.int1 = 0);
					_init = false;

					printHexString(_f.int1);
					break;

				case TargetType: // hex,cb,multiple
					bu_Clear.Enabled = false;

					_cb = null;
					clearchecks();

					printHexString(_f.int1 = 0);
					break;

				case ImmunityType: // dropdown -> fire changed_Combobox()
				case Category:
				case SpontCastClassReq:
				case AsMetaMagic:
				case TargetingUI:
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
			if (result > 0xFF) f = "X6";
			else               f = "X2";

			la_Val.Text = "0x" + result.ToString(f, CultureInfo.InvariantCulture);
		}
		#endregion Methods
	}
}
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
