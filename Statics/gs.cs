namespace yata
{
	/// <summary>
	/// Global (const) strings.
	/// </summary>
	static class gs
	{
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


		internal static string[] SEPARATORS = { "\r\n", "\r", "\n" };

		internal const string TwodaVer = "2DA V2.0";
	}


	#region Constants
	/// <summary>
	/// Global constants.
	/// </summary>
	static class Constants
	{
		internal const float epsilon = 0.00001F;
	}
	#endregion Constants


	#region Util
	static class Util
	{
		internal static bool isAlphanumeric(char character)
		{
			int c = character;
			return (c >= 48 && c <=  57)
				|| (c >= 65 && c <=  90)
				|| (c >= 97 && c <= 122);
		}
	}
	#endregion Util
}
