﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text;


namespace yata
{
	static class TalkReader
	{
		#region Fields (static)
		/// <summary>
		/// The dialog-dictionary. The dictionary does not contain unassigned
		/// entries so check if a key is valid before trying to get its value.
		/// </summary>
		/// <remarks>Does not really need to be "Sorted" ...</remarks>
		internal static SortedDictionary<int, string> DictDialo =
					new SortedDictionary<int, string>();

		internal static SortedDictionary<int, string> DictCusto =
					new SortedDictionary<int, string>();

		internal const int bitCusto = 0x01000000; // flag in the strref for Custo talktable
		internal const int strref   = 0x00FFFFFF; // isolates the actual strref-val
		internal const int invalid  = -1;         // no entry, a blank string; note that a valid id without an entry is also a blank string

		const uint TEXT_PRESENT    =  1; // Data flag - a text is present in the Texts area

		const uint HeaderStart     =  0; // start-byte of the byte-array
//		const uint LanguageIdStart =  8; // offset of the field that contains the LanguageId
		const uint TextsCountField = 12; // offset of the field that contains the TextsCount
		const uint TextsStartField = 16; // offset of the field that contains the offset of the Texts area

		const uint DataStart       = 20; // length in bytes of the Header area
		const uint DataLength      = 40; // length in bytes of 1 Data area

		const uint TextStartField  = 28; // offset from the start of a Data ele to its TextStart field
		const uint TextLengthField = 32; // offset from the start of a Data ele to its TextLength field

		const string Ver = "TLK V3.0";

		internal static int loDialo, hiDialo;
		internal static int loCusto, hiCusto;

		internal static string AltLabel;
		#endregion Fields (static)


		#region Methods (static)
		/// <summary>
		/// Adds key-value pairs [(int)strref, (string)text] to
		/// <c><see cref="DictDialo"/></c> or <c><see cref="DictCusto"/></c>.
		/// </summary>
		/// <param name="pfeTlk">fullpath of a talktable</param>
		/// <param name="it">a menuitem to check/uncheck</param>
		/// <remarks>See description of .tlk Format at the bot of this file.</remarks>
		internal static void Load(string pfeTlk, ToolStripMenuItem it)
		{
			SortedDictionary<int, string> dict;

			bool alt = (it == Yata.that.it_PathTalkC);
			if (alt)
			{
				dict = DictCusto;
				AltLabel = null;
			}
			else
				dict = DictDialo;

			dict.Clear();

			if (File.Exists(pfeTlk))
			{
//				using (FileStream fs = File.OpenRead(pfeTlk)){}

				if (alt) AltLabel = Path.GetFileNameWithoutExtension(pfeTlk).ToUpperInvariant();

				byte[] bytes = File.ReadAllBytes(pfeTlk);

				uint pos = HeaderStart;
				uint b;

				var buffer = new byte[8];
				for (b = 0; b != 8; ++b)
					buffer[b] = bytes[pos++];

				string sUtf8 = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
				if (sUtf8 == Ver) // check if v3.0 tlktable
				{
					bool le = BitConverter.IsLittleEndian; // hardware architecture

//					pos = HeaderStart + LanguageIdStart;
//					buffer = new byte[4];
//					for (b = 0; b != 4; ++b)
//						buffer[b] = bytes[pos++];
//					if (!le) Array.Reverse(buffer);
//					uint LanguageId = BitConverter.ToUInt32(buffer, 0);


					pos = HeaderStart + TextsCountField;

					buffer = new byte[4];
					for (b = 0; b != 4; ++b)
						buffer[b] = bytes[pos++];

					if (!le) Array.Reverse(buffer);
					uint TextsCount = BitConverter.ToUInt32(buffer, 0);


					pos = HeaderStart + TextsStartField;

					buffer = new byte[4];
					for (b = 0; b != 4; ++b)
						buffer[b] = bytes[pos++];

					if (!le) Array.Reverse(buffer);
					uint TextsStart = BitConverter.ToUInt32(buffer, 0);
//					uint TextsStart = DataStart + (TextCount * DataLength);


					uint start;

					for (uint i = 0; i != TextsCount; ++i)
					{
						start = DataStart + i * DataLength;
						pos = start;

						buffer = new byte[4];
						for (b = 0; b != 4; ++b)
							buffer[b] = bytes[pos++];

						if (!le) Array.Reverse(buffer);
						uint Flags = BitConverter.ToUInt32(buffer, 0);


						if ((Flags & TEXT_PRESENT) != 0)
						{
							pos = start + TextStartField;

							buffer = new byte[4];
							for (b = 0; b != 4; ++b)
								buffer[b] = bytes[pos++];

							if (!le) Array.Reverse(buffer);
							uint TextStart = BitConverter.ToUInt32(buffer, 0);


							pos = start + TextLengthField;

							buffer = new byte[4];
							for (b = 0; b != 4; ++b)
								buffer[b] = bytes[pos++];

							if (!le) Array.Reverse(buffer);
							uint TextLength = BitConverter.ToUInt32(buffer, 0);


							pos = TextsStart + TextStart;

							buffer = new byte[TextLength];
							for (b = 0; b != TextLength; ++b)
								buffer[b] = bytes[pos++];

							string text = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

							dict.Add((int)i, text);
						}
					}

					if (dict.Count != 0)
					{
						int lo = Int32.MaxValue;
						int hi = Int32.MinValue;

						foreach (int key in dict.Keys)
						{
							if (key > hi) hi = key;
							if (key < lo) lo = key;
						}

						if (alt) { loCusto = lo; hiCusto = hi; }
						else     { loDialo = lo; hiDialo = hi; }

						it.Checked = true;
						return;
					}
				}
			}
			it.Checked = false;
		}

		/// <summary>
		/// Adds relevant colhead-fields to a list.
		/// </summary>
		/// <param name="heads">pointer to a string-list</param>
		internal static void LoadTalkingHeads(ICollection<string> heads)
		{
			heads.Add("ActionStrRef");		// KeyMap.2da
			heads.Add("ActivatedName");		// CombatModes.2da
			heads.Add("AltMessage");		// Spells.2da
			heads.Add("Biography");			// RacialSubtypes.2da, RacialTypes.2da
			heads.Add("ConverName");		// RacialSubtypes.2da, RacialTypes.2da
			heads.Add("ConverNameLower");	// RacialSubtypes.2da, RacialTypes.2da
			heads.Add("DeactivatedName");	// CombatModes.2da
			heads.Add("Default");			// StringTokens.2da
			heads.Add("DescID");			// Nwn2_Deities.2da
			heads.Add("DescStrRef");		// Nwn2_BehaviorParams.2da
			heads.Add("DESCRIPTION");		// Feat.2da, Hen_Companion.2da, Hen_Familiar.2da, MasterFeats.2da
			heads.Add("Description");		// Backgrounds.2da, BaseItem.2da, Classes.2da, Domains.2da, ItemPropDef.2da, Packages.2da
											// RacialSubtypes.2da, RacialTypes.2da, Skills.2da, SoundEax.2da, SpellSchools.2da
			heads.Add("DESCRIPTIONS");		// Armor.2da, ArmorRuleStat.2da
			heads.Add("BASEITEMSTATREF");	// Armor.2da, ArmorRuleStat.2da
			heads.Add("BaseItemStatRef");	// BaseItems.2da
			heads.Add("ERRORSTRREF");		// NwConfig.2da
			heads.Add("FAILSTRREF");		// NwConfig.2da
			heads.Add("FEAT");				// Feat.2da
			heads.Add("GameString");		// Iprp_DamageCost.2da
			heads.Add("GameStrRef");		// ItemPropDef.2da
			heads.Add("HINT");				// LoadHints.2da
			heads.Add("Lower");				// Classes.2da
			heads.Add("NAME");				// CaPart.2da, CaType.2da, Iprp_Materials.2da
			heads.Add("Name");				// Armor.2da, ArmorRuleStat.2da, Backgrounds.2da, BaseItem.2da, BodyBag.2da, Classes.2da
											// Container_Preference.2da, CreatureSpeed.2da, Disease.2da, Domains.2da,
											// Iprp_*.2da (not Iprp_Materials.2da), ItemPropDef.2da, MetaMagic.2da, Nwn2_Emotes.2da
											// Nwn2_VoiceMenu.2da, Packages.2da, Poison.2da, RacialSubtypes.2da, RacialTypes.2da, Ranges.2da,
											// Skills.2da Spells.2da, Time.2da
			heads.Add("NAME_REF");			// Nwn2_DmCommands.2da
			heads.Add("NameLower");			// RacialSubtypes.2da, RacialTypes.2da
			heads.Add("NameLowerPlural");	// RacialSubtypes.2da, RacialTypes.2da
			heads.Add("NamePlural");		// RacialSubtypes.2da, RacialTypes.2da
			heads.Add("NameStringref");		// Nwn2_Deities.2da
			heads.Add("NameStrRef");		// Nwn2_BehaviorParams.2da
			heads.Add("PASSSTRREF");		// NwConfig.2da
			heads.Add("Plural");			// Classes.2da
			heads.Add("SHORTWARNSTRREF");	// NwConfig.2da
			heads.Add("SKIPSTRREF");		// NwConfig.2da
			heads.Add("SpellDesc");			// Spells.2da
			heads.Add("STRING_REF");		// Actions.2da, Appearance.2da, DamageLevels.2da, MetaTiles.2da, SkyBoxes.2da, Tiles.2da, Tilesets.2da
			heads.Add("StringRef");			// ItemProps.2da, Rrf_Nss.2da, Rrf_Wav.2da, SpellSchools.2da, TailModel.2da, WingModel.2da
			heads.Add("Stringref");			// Trees.2da
			heads.Add("StringRefGame");		// DoorTypes.2da
			heads.Add("STR_REF");			// Water.2da
			heads.Add("STRREF");			// CreatureSize.2da, EncDifficulty.2da, Environment.2da, Hen_Companion.2da, Hen_Familiar.2da
											// MasterFeats.2da, SoundSet.2da, SoundSetType.2da, TreasureScale.2da, Waypoint.2da
			heads.Add("StrRef");			// EffectIcons.2da, LoadScreens.2da, Placeables.2da, PlaceableTypes.2da
			heads.Add("StrRef1");			// StringTokens.2da
			heads.Add("StrRef2");			// StringTokens.2da
			heads.Add("StrRef3");			// StringTokens.2da
			heads.Add("StrRef4");			// StringTokens.2da
			heads.Add("TITLESTRREF");		// NwConfig.2da
			heads.Add("ToolsetName");		// Armor.2da
			heads.Add("TrapName");			// Traps.2da
			heads.Add("WARNSTRREF");		// NwConfig.2da
		}
		#endregion Methods (static)
	}
}

/*
__HEADER__
FileType            4-char "TLK "												0x00000000
FileVersion         4-char "V3.0"												0x00000020
LanguageID          DWORD  Language ID											0x00000040
StringCount         DWORD  Count of strings										0x00000060
StringEntriesOffset DWORD  Offset from start of file to the StringEntryTable	0x00000080

__STRINGDATAELEMENT__															0x000000A0 [start] bit 160 / byte 20
Flags          DWORD   Flags about this StrRef.									0x00000000
SoundResRef    16-char ResRef of the wave file associated with this string.		0x00000020
                       Unused characters are nulls.
VolumeVariance DWORD   not used													0x000000A0
PitchVariance  DWORD   not used													0x000000C0
OffsetToString DWORD   Offset from StringEntriesOffset to the beginning of the	0x000000E0
                       StrRef's text.
StringSize     DWORD   Number of bytes in the string. Null terminating			0x00000100
                       characters are not stored, so this size does not include
                       a null terminator.
SoundLength    FLOAT   Duration in seconds of the associated wave file			0x00000120
																				0x00000140 [length] 320 bits / 40 bytes

__STRINGENTRYTABLE__
The StringEntryTable begins at the StringEntriesOffset specified in the Header
of the file, and continues to the end of the file. All the localized text is
contained in the StringEntryTable as non-null-terminated strings. As soon as one
string ends, the next one begins. kL_note: Consider it UTF8.
*/

// dialog.tlk -> 16777215 MaxVal 0x00FFFFFF
/*
The StrRef is a 32-bit unsigned integer that serves as an index into the table
of strings stored in the talk table. To specify an invalid StrRef, the talk
table system uses a StrRef in which all the bits are 1 (ie. 4294967295, or
0xFFFFFFFF, the maximum possible 32-bit unsigned value, or -1 if it were a
signed 32-bit value). When presented with the invalid StrRef value, the text
returned should be a blank string.

Valid StrRefs can have values of up to 0x00FFFFFF, or 16777215. Any higher
values will have the upper 2 bytes masked off and set to 0, so 0x01000001, or
16777217, for example, will be treated as StrRef 1.

If a module uses an alternate talk table, then bit 0x01000000 of a StrRef
specifies whether the StrRef should be fetched from the normal dialog.tlk or
from the alternate tlk file. If the bit is 0, the StrRef is fetched as normal
from dialog.tlk. If the bit is 1, then the StrRef is fetched from the
alternative talk table.

If the alternate tlk file does not exist, could not be loaded, or does not
contain the requested StrRef, then the StrRef is fetched as normal from the
standard dialog.tlk file. kL_note: But that's just silly.
*/

/*
string convert = "This is the string to be converted.";

// string to byte-array
byte[] buffer = System.Text.Encoding.UTF8.GetBytes(convert);

// byte-array to string
string s = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
*/
