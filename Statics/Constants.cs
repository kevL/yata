﻿using System;

#if Keys
using System.Windows.Forms;
#endif


namespace yata
{
	#region Constants
	/// <summary>
	/// Global (const) strings.
	/// </summary>
	static class gs
	{
		internal const string TwodaVer        = "2DA V2.0";
		internal const string TwodaVer_tab    = "2DA\tV2.0";
		internal const string Default         = "DEFAULT: ";
		internal const string DefaultColLabel = "Label";

		internal const string Stars           = "****";
		internal const string Invalid         = "-1";
		internal const string Id              = "id";

		internal const string Space           = " ";
		internal const char   Spacechar       = ' ';
		internal const string Tab             = "\t";

		internal const string Crafting        = "CRAFTING";
		internal const string Spells          = "SPELLS";
		internal const string Feat            = "FEAT";
		internal const string Classes         = "CLASSES";
		internal const string Baseitems       = "BASEITEMS";

		internal const string non             = "n/a";
		internal const string bork            = "bork";

		internal const string Acid            = "Acid"; // immunity types (Spells.2da) ->
		internal const string Cold            = "Cold";
		internal const string Death           = "Death";
		internal const string Disease         = "Disease";
		internal const string Divine          = "Divine";
		internal const string Electricity     = "Electricity";
		internal const string Evil            = "Evil";
		internal const string Fear            = "Fear";
		internal const string Fire            = "Fire";
		internal const string Magical         = "Magical";
		internal const string Mind_Affecting  = "Mind_Affecting";
		internal const string Negative        = "Negative";
		internal const string Paralysis       = "Paralysis";
		internal const string Poison          = "Poison";
		internal const string Positive        = "Positive";
		internal const string Sonic           = "Sonic";
		internal const string Constitution    = "Constitution";
		internal const string Water           = "Water";

		internal const string Knockdown       = "Knockdown"; // immunity types (Feat.2da) ->
		internal const string NonSpirit       = "Non_Spirit";

		internal const string Attack          = "attack";		// ConjAnim types ->
		internal const string Bardsong        = "bardsong";		// I merely pulled these from what's in Spells.2da
		internal const string Defensive       = "defensive";
		internal const string Hand            = "hand";
		internal const string Head            = "head";
		internal const string Major           = "major";
		internal const string Party           = "party";
		internal const string Read            = "read";

		internal const string Area            = "area";		// CastAnim types ->
		internal const string Creature        = "creature";	// I merely pulled these from what's in Spells.2da
		internal const string General         = "general";	// - a few ConjAnim types are also used.
		internal const string Out             = "out";
		internal const string Self            = "self";
		internal const string Touch           = "touch";
		internal const string Up              = "up";

		internal const string Accelerating      = "accelerating";	// ProjTypes ->
		internal const string Ballistic         = "ballistic";		// I merely pulled these from what's in Spells.2da
		internal const string Bounce            = "bounce";
		internal const string Burst             = "burst";
		internal const string Burstup           = "burstup";
		internal const string Highballistic     = "highballistic";
		internal const string Homing            = "homing";
		internal const string Homingspiral      = "homingspiral";
		internal const string Launchedballistic = "launchedballistic";
		internal const string Linked            = "linked";
		internal const string Loworbit          = "loworbit";
		internal const string Thrownballistic   = "thrownballistic";

		internal const string Halo              = "halo";	// ProjSpwnPoint types ->
//		internal const string Head              = "head";	// I merely pulled these from what's in Spells.2da
		internal const string Lrhand            = "lrhand";
		internal const string Monster0          = "monster0";
		internal const string Monster1          = "monster1";
		internal const string Monster2          = "monster2";
		internal const string Monster3          = "monster3";
		internal const string Monster4          = "monster4";
		internal const string Mouth             = "mouth";
		internal const string Rhand             = "rhand";

		internal const string Path              = "path"; // ProjOrientation types -> ditto.

		internal const string BeshadowedBlast = "Beshadowed Blast";
		internal const string BewitchingBlast = "Bewitching Blast";
		internal const string BindingBlast    = "Binding Blast";
		internal const string BrimstoneBlast  = "Brimstone Blast";
		internal const string DrainingBlast   = "Draining Blast";
		internal const string FrightfulBlast  = "Frightful Blast";
		internal const string HellrimeBlast   = "Hellrime Blast";
		internal const string HinderingBlast  = "Hindering Blast";
		internal const string NoxiousBlast    = "Noxious Blast";
		internal const string UtterdarkBlast  = "Utterdark Blast";
		internal const string VitriolicBlast  = "Vitriolic Blast";

		internal const string EldritchChain   = "Eldritch Chain";
		internal const string EldritchCone    = "Eldritch Cone";
		internal const string EldritchDoom    = "Eldritch Doom";
		internal const string EldritchSpear   = "Eldritch Spear";
		internal const string HideousBlow     = "Hideous Blow";

		internal const string FeatCatBackground    = "BACKGROUND_FT_CAT";
		internal const string FeatCatClassability  = "CLASSABILITY_FT_CAT";
		internal const string FeatCatDivine        = "DIVINE_FT_CAT";
		internal const string FeatCatEpic          = "EPIC_FT_CAT";
		internal const string FeatCatGeneral       = "GENERAL_FT_CAT";
		internal const string FeatCatHeritage      = "HERITAGE_FT_CAT";
		internal const string FeatCatHistory       = "HISTORY_FT_CAT";
		internal const string FeatCatItemCreation  = "ITEMCREATION_FT_CAT";
		internal const string FeatCatMetamagic     = "METAMAGIC_FT_CAT";
		internal const string FeatCatProficiency   = "PROFICIENCY_FT_CAT";
		internal const string FeatCatRacialability = "RACIALABILITY_FT_CAT";
		internal const string FeatCatSkillSave     = "SKILLNSAVE_FT_CAT";
		internal const string FeatCatSpellcasting  = "SPELLCASTING_FT_CAT";
		internal const string FeatCatTeamwork      = "TEAMWORK_FT_CAT";


		internal static string[] CRandorLF = { "\r\n", "\r", "\n" };
	}


	/// <summary>
	/// Global (const) !strings.
	/// </summary>
	static class gc
	{
		internal const float epsilon = 0.00001F;

#if Keys
		/// <summary>
		/// Used to bypass [Control] and [Shift] keys - and [Alt] if desired -
		/// in the <c><see cref="logfile"></see>.Logfile</c>.
		/// </summary>
		internal const Keys ControlShift = Keys.Control | Keys.ControlKey	// I don't know why .net does it this way.
										 | Keys.Shift   | Keys.ShiftKey		// But when only 1 key is pressed (Ctrl,Shift,Alt)
										 | Keys.Alt     | Keys.Menu;		// its second flag also registers respectively.
#endif
	}
	#endregion Constants


	#region Util
	static class Util
	{
		internal static bool isAsciiAlphanumericOrUnderscore(char @char)
		{
			int c = @char;
			return  c == 95					// _
				|| (c >= 48 && c <=  57)	// 0..9
				|| (c >= 65 && c <=  90)	// A..Z
				|| (c >= 97 && c <= 122);	// a..z
		}

		internal static bool isPrintableAsciiNotDoublequote(char @char)
		{
			int c = @char;
			return c != 34 && c >= 32 && c <= 126;
		}
	}
	#endregion Util
}
