﻿using System;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Global (const) strings.
	/// </summary>
	static class gs
	{
		internal const string TwodaVer        = "2DA V2.0";
		internal const string Default         = "DEFAULT: ";
		internal const string DefaultColLabel = "Label";

		internal const string Stars           = "****";
		internal const string Invalid         = "-1";
		internal const string Id              = "id";

		internal const string Space           = " ";

		internal const string non             = "n/a";
		internal const string bork            = "bork";

		internal const string Acid            = "Acid";
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


		internal static string[] CRandorLF = { "\r\n", "\r", "\n" };
	}


	#region Constants
	/// <summary>
	/// Global (const) !strings.
	/// </summary>
	static class gc
	{
		internal const float epsilon = 0.00001F;

#if DEBUG
		/// <summary>
		/// <c>true</c> to log keyboard processing and handling in the
		/// <c><see cref="logfile"></see>.Logfile</c>.
		/// </summary>
		internal const bool KeyLog = true;

		/// <summary>
		/// <c>true</c> to log <c>MouseDown</c> and <c>MouseUp</c> handling in
		/// the <c><see cref="logfile"></see>.Logfile</c>.
		/// </summary>
		internal const bool ClickLog = true;

		/// <summary>
		/// Used to bypass [Control] and [Shift] keys - and [Alt] if desired -
		/// in the <c><see cref="logfile"></see>.Logfile</c>.
		/// </summary>
		internal const Keys ControlShift = Keys.Control | Keys.ControlKey	// I don't know why .net does it this way.
										 | Keys.Shift   | Keys.ShiftKey;	// But when only 1 key is pressed (Ctrl,Shift,Alt)
//										 | Keys.Alt     | Keys.Menu			// its second flag also registers respectively.
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