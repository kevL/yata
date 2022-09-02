using System;
//using System.Collections.Generic;
using System.Globalization;
//using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Crafting info
		/// <summary>
		/// Gets a readable string when mouseovering cols in Crafting.2da.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		string getCraftInfo(int id, int col)
		{
			string info = gs.non;

			string val;
			int result;

			switch (col)
			{
//				case 0: // id

				case 1: // "CATEGORY" - Spells.2da or no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Acid Fog"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result))
					{
						if (result > -1)
						{
							if (result < Info.spellLabels.Count) // it_PathSpells2da.Checked
							{
								info += Info.spellLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					else
					{
						switch (val)
						{
							case "ALC": info += "Alchemy";      break;
							case "DIS": info += "Distillation"; break;
							default:    info += "Mundane";      break; // 'val' should be the tag of a Mold here
						}
					}
					break;

//				case 2: // "REAGENTS"

				case 3: // "TAGS" - BaseItems.2da or no 2da for TCC-types
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "[TCC]None"
					{
						info += gs.non;
					}
					else if (val.StartsWith("B", StringComparison.Ordinal)) // is in BaseItems.2da
					{
						info += "[BaseItem] ";

						string[] array = val.Substring(1).Split(','); // lose the "B"
						for (int i = 0; i != array.Length; ++i)
						{
							if (Int32.TryParse(array[i], out result)
								&& result > -1)
							{
								if (result < Info.tagLabels.Count) // it_PathBaseItems2da.Checked
								{
									info += Info.tagLabels[result];
								}
								else
									info += array[i];
							}
							else
								info += gs.bork;

							if (i != array.Length - 1)
								info += ", ";
						}
					}
					else // is a TCC item-type
					{
						info += "[TCC] ";

						string[] array = val.Split(',');
						for (int i = 0; i != array.Length; ++i)
						{
							if (Int32.TryParse(array[i], out result)) // 'result' can be less than 0 here
							{
								info += GetTccType(result);
							}
							else
								info += gs.bork;

							if (i != array.Length - 1)
								info += ", ";
						}
					}
					break;

				case 4: // "EFFECTS" - ItemPropDef.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Ability"
					{
						info += gs.non;
					}
					else
					{
						string ipEncoded; int pos;

						string[] ips = val.Split(';');
						for (int i = 0; i != ips.Length; ++i)
						{
							ipEncoded = ips[i];
							if ((pos = ipEncoded.IndexOf(',')) != -1)
							{
								if (pos != 0 && Int32.TryParse(ipEncoded.Substring(0, pos), out result) // 'result' is the nwn2 ip-constant
									&& result > -1)
								{
									if (result < Info.ipLabels.Count) // it_PathItemPropDef2da.Checked
									{
										info += Info.ipLabels[result] + gs.Space;

										if (ipEncoded.Length > pos + 1)
										{
											info += GetDecodedDescription(ipEncoded, result, pos);
										}
										else
											info += gs.bork;
									}
									else
										info += result.ToString(CultureInfo.InvariantCulture);
								}
								else
									info += gs.bork;
							}
							else // is a PropertySet preparation val.
							{
								info += "PropertySet val=" + ipEncoded; // TODO: description for par.
							}

							if (i != ips.Length - 1)
								info += ", ";
						}
					}
					break;

//				case 5: // "OUTPUT"

				case 6: // "SKILL" - Feat.2da and/or Skills.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Alertness" or "DEL_AnimalEmpathy"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result) // TODO: SKILL can be -2 (CATEGORY 376) or 0 (spell,ALC,DIS)
						&& result > -1)
					{
						string cat = Table[id,1].text;
						if (cat == gs.Stars)
						{
							info += gs.non;
						}
						else
						{
							int result2;
							if (Int32.TryParse(cat, out result2))									// is triggered by spell id -> SKILL-col is a feat
							{
								if (result2 > -1)
								{
									if (result < Info.featLabels.Count) // it_PathFeat2da.Checked
									{
										info += Info.featLabels[result];
									}
									else
										info += val;
								}
								else
									info += "[CATEGORY] SpellId is invalid";
							}
							else if (result < Info.skillLabels.Count) // it_PathSkills2da.Checked	// is triggered NOT by spell but by mold-tag or is
							{																		// Alchemy or Distillation -> SKILL-col is a skill
								info += Info.skillLabels[result];
							}
							else
								info += val;
						}
					}
					else
						info += gs.bork;
					break;

//				case  7: // "LEVEL"
//				case  8: // "EXCLUDE"
//				case  9: // "XP"
//				case 10: // "GP"
//				case 11: // "DISABLE"
			}
			return info;
		}

		/// <summary>
		/// Gets the TCC-type as a string for a given (int)tag.
		/// </summary>
		/// <param name="tag">the (int)value of the TAGS col in Crafting.2da -
		/// ie. is not an nwn/2 tag</param>
		/// <returns></returns>
		/// <remarks>TCC (TheCompleteCraftsman) defines its own item-types.</remarks>
		static string GetTccType(int tag)
		{
			switch (tag)
			{
				case -2: return "Equippable";
				case -1: return "Any";
				case  0: return "None";
				case  1: return "Melee";
				case  2: return "Armor/Shield";
				case  3: return "Bow";
				case  4: return "Crossbow";
				case  5: return "Sling";
				case  6: return "Ammo";
				case  7: return "Armor";
				case  8: return "Shield";
				case  9: return "Other";
				case 10: return "Ranged";
				case 11: return "Wrists";
				case 15: return "Instrument";
				case 16: return "Container";
				case 17: return "Helmet";
				case 19: return "Amulet";
				case 21: return "Belt";
				case 26: return "Boots";
				case 36: return "Gloves";
				case 52: return "Ring";
				case 78: return "Bracer";
				case 80: return "Cloak";
			}
			return gs.bork;
		}
		#endregion Crafting info


		#region Spells info
		/// <summary>
		/// Gets a readable string when mouseovering cols in Spells.2da.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		string getSpellInfo(int id, int col)
		{
			string info = gs.non;

			string val;
			int result;

			switch (col)
			{
				case InfoInputSpells.School: // no 2da (SpellSchools.2da)
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "General"
					{
						info += gs.non;
					}
					else
					{
						switch (val.ToUpperInvariant())
						{
							case "A": info += "Abjuration";    break;
							case "C": info += "Conjuration";   break;
							case "D": info += "Divination";    break;
							case "E": info += "Enchantment";   break;
							case "I": info += "Illusion";      break;
							case "N": info += "Necromancy";    break;
							case "T": info += "Transmutation"; break;
							case "V": info += "Evocation";     break;

							default: info += gs.bork; break;
						}
					}
					break;

				case InfoInputSpells.Range: // no 2da (Ranges.2da)
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "SpellRngPers"
					{
						info += gs.non;
					}
					else
					{
						int r;
						switch (val.ToUpperInvariant())
						{
							case "P": info += "Personal"; r =  0; break; // NOTE: 'rangeLabels' could be used but
							case "T": info += "Touch";    r =  1; break; // they're abnormal: "SpellRngPers" eg.
							case "S": info += "Short";    r =  2; break;
							case "M": info += "Medium";   r =  3; break;
							case "L": info += "Long";     r =  4; break;
							case "I": info += "Infinite"; r = 14; break;

							default: info += gs.bork; r = -1; break;
						}

						if (r != -1 && r < Info.rangeRanges.Count) // it_PathRanges2da.Checked
						{
							info += gs.Space + Info.rangeRanges[r] + "m";
						}
					}
					break;

				case InfoInputSpells.Vs: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually invalid
					{
						info += gs.non;
					}
					else
					{
						switch (val.ToUpperInvariant())
						{
							case "V":  info += "verbal";          break;
							case "S":  info += "somatic";         break;
							case "VS": info += "verbal, somatic"; break;

							default: info += gs.bork; break;
						}
					}
					break;

				case InfoInputSpells.MetaMagic: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "none"
					{
						info += gs.non;
					}
					else if (val.Length > 2
						&& Int32.TryParse(val.Substring(2),
										  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notation
										  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
										  out result))
					{
						switch (result)
						{
							case META_NONE:
								info += "none";
								break;
							case META_ANY:
								info += "ANY";
								break;
							case META_I_ALL:
								info += "All Eldritch Essences and Blast Shapes";
								break;
							case META_I_SHAPES:
								// Eldritch Spear, Hideous Blow, Eldritch Chain, Eldritch Cone, Eldritch Doom
								info += "All Blast Shapes";
								break;
							case META_I_ESSENCES:
								// Draining, Frightful, Beshadowed, Brimstone, Hellrime, Bewitching, Noxious,
								// Vitriolic, Utterdark, Hindering, Binding
								info += "All Eldritch Essences";
								break;

							default:
							{
								bool space = false;
								if ((result & META_EMPOWER) != 0)
								{
									info += "(1)Empower";
									space = true;
								}
								if ((result & META_EXTEND) != 0)
								{
									if (space) info += gs.Space;
									info += "(2)Extend";
									space = true;
								}
								if ((result & META_MAXIMIZE) != 0)
								{
									if (space) info += gs.Space;
									info += "(4)Maximize";
									space = true;
								}
								if ((result & META_QUICKEN) != 0)
								{
									if (space) info += gs.Space;
									info += "(8)Quicken";
									space = true;
								}
								if ((result & META_SILENT) != 0)
								{
									if (space) info += gs.Space;
									info += "(16)Silent";
									space = true;
								}
								if ((result & META_STILL) != 0)
								{
									if (space) info += gs.Space;
									info += "(32)Still";
									space = true;
								}
								if ((result & META_PERSISTENT) != 0)
								{
									if (space) info += gs.Space;
									info += "(64)Persistent";
									space = true;
								}
								if ((result & META_PERMANENT) != 0)
								{
									if (space) info += gs.Space;
									info += "(128)Permanent";
									space = true;
								}

								if ((result & META_I_DRAINING_BLAST) != 0) // Eldritch Essences and Blast Shapes ->
								{
									if (space) info += " - "; // that should never happen.
									info += gs.DrainingBlast; //(256)
									space = true;
								}
								if ((result & META_I_ELDRITCH_SPEAR) != 0)
								{
									if (space) info += ", ";
									info += gs.EldritchSpear; //(512)
									space = true;
								}
								if ((result & META_I_FRIGHTFUL_BLAST) != 0)
								{
									if (space) info += ", ";
									info += gs.FrightfulBlast; //(1024)
									space = true;
								}
								if ((result & META_I_HIDEOUS_BLOW) != 0)
								{
									if (space) info += ", ";
									info += gs.HideousBlow; //(2048)
									space = true;
								}
								if ((result & META_I_BESHADOWED_BLAST) != 0)
								{
									if (space) info += ", ";
									info += gs.BeshadowedBlast; //(4096)
									space = true;
								}
								if ((result & META_I_BRIMSTONE_BLAST) != 0)
								{
									if (space) info += ", ";
									info += gs.BrimstoneBlast; //(8192)
									space = true;
								}
								if ((result & META_I_ELDRITCH_CHAIN) != 0)
								{
									if (space) info += ", ";
									info += gs.EldritchChain; //(16384)
									space = true;
								}
								if ((result & META_I_HELLRIME_BLAST) != 0)
								{
									if (space) info += ", ";
									info += gs.HellrimeBlast; //(32768)
									space = true;
								}
								if ((result & META_I_BEWITCHING_BLAST) != 0)
								{
									if (space) info += ", ";
									info += gs.BewitchingBlast; //(65536)
									space = true;
								}
								if ((result & META_I_ELDRITCH_CONE) != 0)
								{
									if (space) info += ", ";
									info += gs.EldritchCone; //(131072)
									space = true;
								}
								if ((result & META_I_NOXIOUS_BLAST) != 0)
								{
									if (space) info += ", ";
									info += gs.NoxiousBlast; //(262144)
									space = true;
								}
								if ((result & META_I_VITRIOLIC_BLAST) != 0)
								{
									if (space) info += ", ";
									info += gs.VitriolicBlast; //(524288)
									space = true;
								}
								if ((result & META_I_ELDRITCH_DOOM) != 0)
								{
									if (space) info += ", ";
									info += gs.EldritchDoom; //(1048576)
									space = true;
								}
								if ((result & META_I_UTTERDARK_BLAST) != 0)
								{
									if (space) info += ", ";
									info += gs.UtterdarkBlast; //(2097152)
									space = true;
								}
								if ((result & META_I_HINDERING_BLAST) != 0)
								{
									if (space) info += ", ";
									info += gs.HinderingBlast; //(4194304)
									space = true;
								}
								if ((result & META_I_BINDING_BLAST) != 0)
								{
									if (space) info += ", ";
									info += gs.BindingBlast; //(8388608)
								}
								break;
							}
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputSpells.TargetType: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "none"
					{
						info += gs.non;
					}
					else if (val.Length > 2
						&& Int32.TryParse(val.Substring(2),
										  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notation
										  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
										  out result))
					{
						switch (result)
						{
							case TARGET_NONE: info += "none"; break;

							default:
							{
								bool space = false;
								if ((result & TARGET_SELF) != 0)
								{
									info += "(1)Self";
									space = true;
								}
								if ((result & TARGET_CREATURE) != 0)
								{
									if (space) info += gs.Space;
									info += "(2)Creatures";
									space = true;
								}
								if ((result & TARGET_GROUND) != 0)
								{
									if (space) info += gs.Space;
									info += "(4)Ground";
									space = true;
								}
								if ((result & TARGET_ITEMS) != 0)
								{
									if (space) info += gs.Space;
									info += "(8)Items";
									space = true;
								}
								if ((result & TARGET_DOORS) != 0)
								{
									if (space) info += gs.Space;
									info += "(16)Doors";
									space = true;
								}
								if ((result & TARGET_PLACEABLES) != 0)
								{
									if (space) info += gs.Space;
									info += "(32)Placeables";
									space = true;
								}
								if ((result & TARGET_TRIGGERS) != 0)
								{
									if (space) info += gs.Space;
									info += "(64)Triggers";
								}
								break;
							}
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputSpells.ConjAnim:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually none/invalid
					{
						info += gs.non;
					}
					else
					{
						switch (val)
						{
							case gs.Attack:
							case gs.Bardsong:
							case gs.Defensive:
							case gs.Hand:
							case gs.Head:
							case gs.Major:
							case gs.Party:
							case gs.Read: info += val; break;

							default: info += gs.bork; break;
						}
					}
					break;

				case InfoInputSpells.CastAnim:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually none/invalid
					{
						info += gs.non;
					}
					else
					{
						switch (val)
						{
							case gs.Area:
							case gs.Attack:
							case gs.Bardsong:
							case gs.Creature:
							case gs.Defensive:
							case gs.General:
							case gs.Major:
							case gs.Out:
							case gs.Self:
							case gs.Touch:
							case gs.Up: info += val; break;

							default: info += gs.bork; break;
						}
					}
					break;

				case InfoInputSpells.ProjType:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually none/invalid
					{
						info += gs.non;
					}
					else
					{
						switch (val)
						{
							case gs.Accelerating:
							case gs.Ballistic:
							case gs.Bounce:
							case gs.Burst:
							case gs.Burstup:
							case gs.Highballistic:
							case gs.Homing:
							case gs.Homingspiral:
							case gs.Launchedballistic:
							case gs.Linked:
							case gs.Loworbit:
							case gs.Thrownballistic: info += val; break;

							default: info += gs.bork; break;
						}
					}
					break;

				case InfoInputSpells.ProjSpwnPoint:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually none/invalid
					{
						info += gs.non;
					}
					else
					{
						switch (val)
						{
							case gs.Halo:
							case gs.Head:
							case gs.Lrhand:
							case gs.Monster0:
							case gs.Monster1:
							case gs.Monster2:
							case gs.Monster3:
							case gs.Monster4:
							case gs.Mouth:
							case gs.Rhand: info += val; break;

							default: info += gs.bork; break;
						}
					}
					break;

				case InfoInputSpells.ProjOrientation:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually none/invalid
					{
						info += gs.non;
					}
					else
					{
						switch (val)
						{
							case gs.Path: info += val; break;

							default: info += gs.bork; break;
						}
					}
					break;

				case InfoInputSpells.ImmunityType:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually none/invalid
					{
						info += gs.non;
					}
					else
						info += val; // "ImmunityType" is technically unsupported so just print 'val'.
					break;

				case InfoInputSpells.SubRadSpell1: // Spells.2da ->
				case InfoInputSpells.SubRadSpell2:
				case InfoInputSpells.SubRadSpell3:
				case InfoInputSpells.SubRadSpell4:
				case InfoInputSpells.SubRadSpell5:
				case InfoInputSpells.Master:
				case InfoInputSpells.Counter1:
				case InfoInputSpells.Counter2:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Acid Fog"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.spellLabels.Count) // it_PathSpells2da.Checked
						{
							info += Info.spellLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputSpells.Category: // Categories.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "****"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.categoryLabels.Count) // it_PathCategories2da.Checked
						{
							info += Info.categoryLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputSpells.UserType: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually invalid
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > 0 && result < 5)
					{
						switch (result)
						{
							case 1: info += "Spell";           break;
							case 2: info += "Special Ability"; break;
							case 3: info += "Feat";            break;
							case 4: info += "Item Power";      break;
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputSpells.SpontCastClassReq: // Classes.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Barbarian"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.classLabels.Count) // it_PathClasses2da.Checked
						{
							info += Info.classLabels[result];	// The stock Spells.2da uses "0" for n/a here.
						}										// The field ought be "****" (or "-1") instead ofc.
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputSpells.FeatID: // Feat.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Alertness"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						int feat, dividend, right;
						if (result < FEATSPELL_MASTER)
						{
							feat     = result;
							dividend = -1;
							right    = -1;
						}
						else
						{
							feat     = (result % FEATSPELL_MASTER);
							dividend = (result / FEATSPELL_MASTER);
							right    = (result & FEATSPELL_FEATS); // TODO: is that real or should it be 0x0000FFFF
						}

						if (feat < Info.featLabels.Count) // it_PathFeat2da.Checked
						{
							info += Info.featLabels[feat];
							if (dividend != -1)
							{
								info += " (d=" + dividend + ")";
								info += " (r=" + right    + ")";
							}
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputSpells.AsMetaMagic: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually none/invalid
					{
						info += gs.non;
					}
					else if (val.Length > 2
						&& Int32.TryParse(val.Substring(2),
										  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notation
										  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
										  out result))
					{
						switch (result)
						{
							case META_I_DRAINING_BLAST:   info += gs.DrainingBlast;   break;
							case META_I_ELDRITCH_SPEAR:   info += gs.EldritchSpear;   break;
							case META_I_FRIGHTFUL_BLAST:  info += gs.FrightfulBlast;  break;
							case META_I_HIDEOUS_BLOW:     info += gs.HideousBlow;     break;
							case META_I_BESHADOWED_BLAST: info += gs.BeshadowedBlast; break;
							case META_I_BRIMSTONE_BLAST:  info += gs.BrimstoneBlast;  break;
							case META_I_ELDRITCH_CHAIN:   info += gs.EldritchChain;   break;
							case META_I_HELLRIME_BLAST:   info += gs.HellrimeBlast;   break;
							case META_I_BEWITCHING_BLAST: info += gs.BewitchingBlast; break;
							case META_I_ELDRITCH_CONE:    info += gs.EldritchCone;    break;
							case META_I_NOXIOUS_BLAST:    info += gs.NoxiousBlast;    break;
							case META_I_VITRIOLIC_BLAST:  info += gs.VitriolicBlast;  break;
							case META_I_ELDRITCH_DOOM:    info += gs.EldritchDoom;    break;
							case META_I_UTTERDARK_BLAST:  info += gs.UtterdarkBlast;  break;
							case META_I_HINDERING_BLAST:  info += gs.HinderingBlast;  break;
							case META_I_BINDING_BLAST:    info += gs.BindingBlast;    break;

							default: info += gs.bork; break;
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputSpells.TargetingUI: // SpellTarget.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "point"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.targetLabels.Count) // it_PathSpellTarget2da.Checked
						{
							info += Info.targetLabels[result];

							bool valid = false;

							float f = Info.targetWidths[result];
							if (Math.Abs(0.0F - f) > 0.00001F)
							{
								valid = true;
								info += " (" + f;
							}

							f = Info.targetLengths[result];
							if (Math.Abs(0.0F - f) > 0.00001F)
							{
								if (!valid)
								{
									valid = true;
									info += " (_";
								}
								info += " x " + f;
							}

							if (valid) info += ")";
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputSpells.Proj: // no 2da (bools) ->
				case InfoInputSpells.ItemImmunity:
				case InfoInputSpells.UseConcentration:
				case InfoInputSpells.SpontaneouslyCast:
				case InfoInputSpells.HostileSetting:
				case InfoInputSpells.HasProjectile:
				case InfoInputSpells.CastableOnDead:
				case InfoInputSpells.Removed:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "false"
					{
						info += gs.non;
					}
					else
					{
						switch (val)
						{
							case "0": info += "false"; break;
							case "1": info += "true";  break;

							default: info += gs.bork; break;
						}
					}
					break;
			}

			return info;
		}

		// FeatSpells
		const int FEATSPELL_MASTER = 0x00010000;
		const int FEATSPELL_FEATS  = 0x00001111; // TODO: is that real or should it be 0x0000FFFF

		// MetaMagic
		internal const int META_NONE               = 0x00000000; //        0
		internal const int META_EMPOWER            = 0x00000001; //        1
		internal const int META_EXTEND             = 0x00000002; //        2
		internal const int META_MAXIMIZE           = 0x00000004; //        4
		internal const int META_QUICKEN            = 0x00000008; //        8
		internal const int META_SILENT             = 0x00000010; //       16
		internal const int META_STILL              = 0x00000020; //       32
		internal const int META_PERSISTENT         = 0x00000040; //       64
		internal const int META_PERMANENT          = 0x00000080; //      128

		internal const int META_STANDARD           = META_EMPOWER | META_EXTEND | META_MAXIMIZE
												   | META_QUICKEN | META_SILENT | META_STILL
												   | META_PERSISTENT | META_PERMANENT;

		internal const int META_I_DRAINING_BLAST   = 0x00000100; //      256
		internal const int META_I_ELDRITCH_SPEAR   = 0x00000200; //      512
		internal const int META_I_FRIGHTFUL_BLAST  = 0x00000400; //     1024
		internal const int META_I_HIDEOUS_BLOW     = 0x00000800; //     2048
		internal const int META_I_BESHADOWED_BLAST = 0x00001000; //     4096
		internal const int META_I_BRIMSTONE_BLAST  = 0x00002000; //     8192
		internal const int META_I_ELDRITCH_CHAIN   = 0x00004000; //    16384
		internal const int META_I_HELLRIME_BLAST   = 0x00008000; //    32768
		internal const int META_I_BEWITCHING_BLAST = 0x00010000; //    65536
		internal const int META_I_ELDRITCH_CONE    = 0x00020000; //   131072
		internal const int META_I_NOXIOUS_BLAST    = 0x00040000; //   262144
		internal const int META_I_VITRIOLIC_BLAST  = 0x00080000; //   524288
		internal const int META_I_ELDRITCH_DOOM    = 0x00100000; //  1048576
		internal const int META_I_UTTERDARK_BLAST  = 0x00200000; //  2097152
		internal const int META_I_HINDERING_BLAST  = 0x00400000; //  4194304
		internal const int META_I_BINDING_BLAST    = 0x00800000; //  8388608

		internal const int META_I_SHAPES           = 0x00124A00; //  1198592 - all blast shapes
		internal const int META_I_ESSENCES         = 0x00EDB500; // 15578368 - all eldritch essences
		internal const int META_I_ALL              = 0x00FFFF00; // 16776960 - all shapes and essences

		internal const int META_ANY                = unchecked((int)0xFFFFFFFF); // 4294967295 - the kitchen sink (not).

		// TargetType
		internal const int TARGET_NONE       = 0x00; //  0
		internal const int TARGET_SELF       = 0x01; //  1
		internal const int TARGET_CREATURE   = 0x02; //  2
		internal const int TARGET_GROUND     = 0x04; //  4
		internal const int TARGET_ITEMS      = 0x08; //  8
		internal const int TARGET_DOORS      = 0x10; // 16
		internal const int TARGET_PLACEABLES = 0x20; // 32
		internal const int TARGET_TRIGGERS   = 0x40; // 64

		internal const int TARGET_TOTAL = TARGET_SELF | TARGET_CREATURE | TARGET_GROUND
										| TARGET_ITEMS | TARGET_DOORS | TARGET_PLACEABLES
										| TARGET_TRIGGERS;
		#endregion Spells info


		#region Feat info
		/// <summary>
		/// Gets a readable string when mouseovering cols in Feat.2da.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="col"></param>
		/// <returns>info text for the statusbar</returns>
		string getFeatInfo(int id, int col)
		{
			string info = gs.non;

			string val;
			int result;

			switch (col)
			{
				case InfoInputFeat.PREREQFEAT1: // Feat.2da ->
				case InfoInputFeat.PREREQFEAT2:
				case InfoInputFeat.SUCCESSOR:
				case InfoInputFeat.USESMAPFEAT:
				case InfoInputFeat.OrReqFeat0:
				case InfoInputFeat.OrReqFeat1:
				case InfoInputFeat.OrReqFeat2:
				case InfoInputFeat.OrReqFeat3:
				case InfoInputFeat.OrReqFeat4:
				case InfoInputFeat.OrReqFeat5:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Alertness"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.featLabels.Count) // it_PathFeat2da.Checked
						{
							info += Info.featLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputFeat.Category: // Categories.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "****"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.categoryLabels.Count) // it_PathCategories2da.Checked
						{
							info += Info.categoryLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputFeat.SPELLID: // Spells.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Acid Fog"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.spellLabels.Count) // it_PathSpells2da.Checked
						{
							info += Info.spellLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputFeat.MasterFeat: // MasterFeats.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "ImprovedCritical"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.masterfeatLabels.Count) // it_PathMasterFeats2da.Checked
						{
							info += Info.masterfeatLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputFeat.REQSKILL: // Skills.2da ->
				case InfoInputFeat.REQSKILL2:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "DEL_AnimalEmpathy"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.skillLabels.Count) // it_PathSkills2da.Checked
						{
							info += Info.skillLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputFeat.ToolsCategories: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "All Feats"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1 && result < 7)
					{
						switch (result)
						{
							case 0: info += "All Feats";           break;
							case 1: info += "Combat Feats";        break;
							case 2: info += "Active Combat Feats"; break;
							case 3: info += "Defensive Feats";     break;
							case 4: info += "Magical Feats";       break;
							case 5: info += "Class/Racial Feats";  break;
							case 6: info += "Other Feats";         break;
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputFeat.MinLevelClass: // Classes.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Barbarian"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.classLabels.Count) // it_PathClasses2da.Checked
						{
							info += Info.classLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputFeat.ToggleMode: // CombatModes.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Detect"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.combatmodeLabels.Count) // it_PathCombatModes2da.Checked
						{
							info += Info.combatmodeLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputFeat.ImmunityType: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually none/invalid
					{
						info += gs.non;
					}
					else
					{
						switch (val)
						{
							case gs.Knockdown: info += gs.Knockdown; break;
							case gs.NonSpirit: info += gs.NonSpirit; break;

							default: info += gs.bork; break;
						}
					}
					break;

				case InfoInputFeat.GAINMULTIPLE: // no 2da (bools) ->
				case InfoInputFeat.EFFECTSSTACK:
				case InfoInputFeat.ALLCLASSESCANUSE:
				case InfoInputFeat.TARGETSELF:
				case InfoInputFeat.HostileFeat:
				case InfoInputFeat.PreReqEpic:
				case InfoInputFeat.IsActive:
				case InfoInputFeat.IsPersistent:
				case InfoInputFeat.DMFeat:
				case InfoInputFeat.REMOVED:
				case InfoInputFeat.Instant:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "false"
					{
						info += gs.non;
					}
					else
					{
						switch (val)
						{
							case "0": info += "false"; break;
							case "1": info += "true";  break;

							default: info += gs.bork; break;
						}
					}
					break;
			}

			return info;
		}
		#endregion Feat info


		#region Class info
		/// <summary>
		/// Gets a readable string when mouseovering cols in Classes.2da.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="col"></param>
		/// <returns>info text for the statusbar</returns>
		string getClassInfo(int id, int col)
		{
			string info = gs.non;

			string val;
			int result;

			switch (col)
			{
				case InfoInputClasses.HitDie: // no 2da ->
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually none/invalid
					{
						info += gs.non;
					}
					else
					{
						switch (val)
						{
							case "4" : info += "d4";  break;
							case "6" : info += "d6";  break;
							case "8" : info += "d8";  break;
							case "10": info += "d10"; break;
							case "12": info += "d12"; break;

							default: info += gs.bork; break;
						}
					}
					break;

				case InfoInputClasses.PrimaryAbil: // no 2da ->
				case InfoInputClasses.SpellAbil:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually none/invalid
					{
						info += gs.non;
					}
					else
					{
						switch (val.ToUpperInvariant())
						{
							case "STR": info += "Strength";     break;
							case "CON": info += "Constitution"; break;
							case "DEX": info += "Dexterity";    break;
							case "INT": info += "Intelligence"; break;
							case "WIS": info += "Wisdom";       break;
							case "CHA": info += "Charisma";     break;

							default: info += gs.bork; break;
						}
					}
					break;

				case InfoInputClasses.AlignRestrict: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "none"
					{
						info += gs.non;
					}
					else if (val.Length > 2
						&& Int32.TryParse(val.Substring(2),
										  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notation
										  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
										  out result))
					{
						switch (result)
						{
							case ALIGNRESTRICT_NONE: info += "none"; break;

							default:
							{
								// check the InvertRestrict col ->
								// NOTE: The NwN engine(s) could allow values other than "1".
								if (col + 2 < Table.ColCount && Table[id, col + 2].text == "1")
								{
									info += "REQUIRED ";
								}
								else
									info += "PROHIBITED ";

								bool space = false;
								if ((result & ALIGNRESTRICT_NEUTRAL) != 0)
								{
									// check the AlignRstrctType col ->
									string art = String.Empty;

									if (col + 1 < Table.ColCount)
									{
										int artresult;

										string text = Table[id, col + 1].text;
										if (text.Length > 2 && text.Substring(0,2) == "0x"
											&& Int32.TryParse(text.Substring(2),
															  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notation
															  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
															  out artresult))
										{
											if ((artresult & ALIGNRESTRICTTYPE_LAWCHAOS) != 0)
												art = " ethics";

											if ((artresult & ALIGNRESTRICTTYPE_GOODEVIL) != 0)
											{
												if (art != String.Empty) art += "/morals";
												else                     art  = " morals";
											}
										}
									}

									info += "(1)neutral" + art;
									space = true;
								}
								if ((result & ALIGNRESTRICT_LAWFUL) != 0)
								{
									if (space) info += gs.Space;
									info += "(2)lawful";
									space = true;
								}
								if ((result & ALIGNRESTRICT_CHAOTIC) != 0)
								{
									if (space) info += gs.Space;
									info += "(4)chaotic";
									space = true;
								}
								if ((result & ALIGNRESTRICT_GOOD) != 0)
								{
									if (space) info += gs.Space;
									info += "(8)good";
									space = true;
								}
								if ((result & ALIGNRESTRICT_EVIL) != 0)
								{
									if (space) info += gs.Space;
									info += "(16)evil";
									space = true;
								}
								break;
							}
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputClasses.AlignRstrctType: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "none"
					{
						info += gs.non;
					}
					else if (val.Length > 2
						&& Int32.TryParse(val.Substring(2),
										  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notation
										  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
										  out result))
					{
						switch (result)
						{
							case ALIGNRESTRICTTYPE_NONE: info += "none"; break;

							default:
							{
								bool space = false;
								if ((result & ALIGNRESTRICTTYPE_LAWCHAOS) != 0)
								{
									info += "(1)law/chaos";
									space = true;
								}
								if ((result & ALIGNRESTRICTTYPE_GOODEVIL) != 0)
								{
									if (space) info += gs.Space;
									info += "(2)good/evil";
									space = true;
								}
								break;
							}
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputClasses.Package: // Packages.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Barbarian"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.packageLabels.Count) // it_PathPackages2da.Checked
						{
							info += Info.packageLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputClasses.FEATPracticedSpellcaster: // Feat.2da ->
				case InfoInputClasses.FEATExtraSlot:
				case InfoInputClasses.FEATArmoredCaster:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Alertness"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.featLabels.Count) // it_PathFeat2da.Checked
						{
							info += Info.featLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputClasses.PlayerClass: // no 2da (bools) ->
				case InfoInputClasses.SpellCaster:
				case InfoInputClasses.MetaMagicAllowed:
				case InfoInputClasses.MemorizesSpells:
				case InfoInputClasses.HasArcane:
				case InfoInputClasses.HasDivine:
				case InfoInputClasses.HasSpontaneousSpells:
				case InfoInputClasses.AllSpellsKnown:
				case InfoInputClasses.HasInfiniteSpells:
				case InfoInputClasses.HasDomains:
				case InfoInputClasses.HasSchool:
				case InfoInputClasses.HasFamiliar:
				case InfoInputClasses.HasAnimalCompanion:
				case InfoInputClasses.InvertRestrict:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "false"
					{
						info += gs.non;
					}
					else
					{
						switch (val)
						{
							case "0": info += "false"; break;
							case "1": info += "true";  break;

							default: info += gs.bork; break;
						}
					}
					break;
			}

			return info;
		}

		// AlignRestrict
		internal const int ALIGNRESTRICT_NONE    = 0x00; //  0
		internal const int ALIGNRESTRICT_NEUTRAL = 0x01; //  1
		internal const int ALIGNRESTRICT_LAWFUL  = 0x02; //  2
		internal const int ALIGNRESTRICT_CHAOTIC = 0x04; //  4
		internal const int ALIGNRESTRICT_GOOD    = 0x08; //  8
		internal const int ALIGNRESTRICT_EVIL    = 0x10; // 16

		internal const int ALIGNRESTRICT_TOTAL = ALIGNRESTRICT_NEUTRAL
											   | ALIGNRESTRICT_LAWFUL
											   | ALIGNRESTRICT_CHAOTIC
											   | ALIGNRESTRICT_GOOD
											   | ALIGNRESTRICT_EVIL;

		// AlignRstrctType
		internal const int ALIGNRESTRICTTYPE_NONE     = 0x0;
		internal const int ALIGNRESTRICTTYPE_LAWCHAOS = 0x1;
		internal const int ALIGNRESTRICTTYPE_GOODEVIL = 0x2;

		internal const int ALIGNRESTRICTTYPE_TOTAL = ALIGNRESTRICTTYPE_LAWCHAOS
												   | ALIGNRESTRICTTYPE_GOODEVIL;
		#endregion Class info


		#region Item info
		/// <summary>
		/// Gets a readable string when mouseovering cols in
		/// <c>BaseItems.2da</c>.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="col"></param>
		/// <returns>info text for the statusbar</returns>
		string getBaseitemInfo(int id, int col)
		{
			string info = gs.non;

			string val;
			int result;

			switch (col)
			{
				case InfoInputBaseitems.EquipableSlots: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "not equipable"
					{
						info += gs.non;
					}
					else if (val.Length > 2
						&& Int32.TryParse(val.Substring(2),
										  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notation
										  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
										  out result))
					{
						switch (result)
						{
							case EQUIPSLOT_NONE: info += "not equipable"; break;

							default:
							{
								bool space = false;
								if ((result & EQUIPSLOT_HELMET) != 0)
								{
									info += "(1)head";
									space = true;
								}
								if ((result & EQUIPSLOT_ARMOR) != 0)
								{
									if (space) info += gs.Space;
									info += "(2)chest";
									space = true;
								}
								if ((result & EQUIPSLOT_BOOTS) != 0)
								{
									if (space) info += gs.Space;
									info += "(4)feet";
									space = true;
								}
								if ((result & EQUIPSLOT_GLOVES) != 0)
								{
									if (space) info += gs.Space;
									info += "(8)arms";
									space = true;
								}
								if ((result & EQUIPSLOT_MAINHAND) != 0)
								{
									if (space) info += gs.Space;
									info += "(16)righthand";
									space = true;
								}
								if ((result & EQUIPSLOT_OFFHAND) != 0)
								{
									if (space) info += gs.Space;
									info += "(32)lefthand";
									space = true;
								}
								if ((result & EQUIPSLOT_CLOAK) != 0)
								{
									if (space) info += gs.Space;
									info += "(64)back";
									space = true;
								}
								if ((result & EQUIPSLOT_RINGS) != 0)
								{
									if (space) info += gs.Space;
									info += "(384)fingers";
									space = true;
								}
								if ((result & EQUIPSLOT_AMULET) != 0)
								{
									if (space) info += gs.Space;
									info += "(512)neck";
									space = true;
								}
								if ((result & EQUIPSLOT_BELT) != 0)
								{
									if (space) info += gs.Space;
									info += "(1024)waist";
									space = true;
								}
								if ((result & EQUIPSLOT_ARROW) != 0)
								{
									if (space) info += gs.Space;
									info += "(2048)arrow";
									space = true;
								}
								if ((result & EQUIPSLOT_BULLET) != 0)
								{
									if (space) info += gs.Space;
									info += "(4096)bullet";
									space = true;
								}
								if ((result & EQUIPSLOT_BOLT) != 0)
								{
									if (space) info += gs.Space;
									info += "(8192)bolt";
									space = true;
								}
								if ((result & EQUIPSLOT_CWEAPON) != 0)
								{
									if (space) info += gs.Space;
									info += "(114688)Cweapon";
									space = true;
								}
								if ((result & EQUIPSLOT_CARMOR) != 0)
								{
									if (space) info += gs.Space;
									info += "(131072)Carmor";
									space = true;
								}
								break;
							}
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.ModelType: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "simple 1-part"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1 && result < 4)
					{
						switch (result)
						{
							case 0: info += "simple 1-part";       break;
							case 1: info += "colored 1-part";      break;
							case 2: info += "configurable 3-part"; break;
							case 3: info += "armor";               break;
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.Part1EnvMap: // no 2da ->
				case InfoInputBaseitems.Part2EnvMap:
				case InfoInputBaseitems.Part3EnvMap:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "transparency"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1 && result < 2)
					{
						switch (result)
						{
							case 0: info += "transparency"; break;
							case 1: info += "reflectivity"; break;
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.WeaponWield: // no 2da
						info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "standard one-handed weapon"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1 && result < 14)
					{
						switch (result)
						{
							case  0: info += "standard one-handed weapon"; break;
							case  1: info += "not wieldable";              break;
							case  2: info += "not used/unknown";           break;
							case  3: info += "not used/unknown";           break;
							case  4: info += "two-handed weapon";          break;
							case  5: info += "bow";                        break;
							case  6: info += "crossbow";                   break;
							case  7: info += "shield";                     break;
							case  8: info += "double-sided weapon";        break;
							case  9: info += "creature weapon";            break;
							case 10: info += "dart or sling";              break;
							case 11: info += "shuriken or throwing axe";   break;
							case 12: info += "spears";                     break;
							case 13: info += "musical instruments";        break;
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.WeaponType: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "none"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1 && result < 6)
					{
						switch (result)
						{
							case 0: info += "none";                 break;
							case 1: info += "piercing";             break;
							case 2: info += "bludgeoning";          break;
							case 3: info += "slashing";             break;
							case 4: info += "piercing/slashing";    break;
							case 5: info += "bludgeoning/piercing"; break;
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.WeaponSize: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "none"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1 && result < 5)
					{
						switch (result)
						{
							case 0: info += "none";   break;
							case 1: info += "tiny";   break;
							case 2: info += "small";  break;
							case 3: info += "medium"; break;
							case 4: info += "large";  break;
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.RangedWeapon: // BaseItems.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "shortsword"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.tagLabels.Count) // it_PathBaseItems2da.Checked
						{
							info += Info.tagLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.InvSoundType: // InventorySnds.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "soft_plate"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.soundLabels.Count) // it_PathInventorySnds2da.Checked
						{
							info += Info.soundLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.PropColumn: // ItemProps.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Ability"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.propFields.Count) // it_PathItemProps2da.Checked
						{
							info += Info.propFields[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.StorePanel: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "armor and clothing"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1 && result < 5)
					{
						switch (result)
						{
							case 0: info += "armor and clothing";    break;
							case 1: info += "weapons";               break;
							case 2: info += "potions and scrolls";   break;
							case 3: info += "wands and magic items"; break;
							case 4: info += "miscellaneous";         break;
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.ReqFeat0: // Feat.2da ->
				case InfoInputBaseitems.ReqFeat1:
				case InfoInputBaseitems.ReqFeat2:
				case InfoInputBaseitems.ReqFeat3:
				case InfoInputBaseitems.ReqFeat4:
				case InfoInputBaseitems.ReqFeat5:
				case InfoInputBaseitems.FEATImprCrit:
				case InfoInputBaseitems.FEATWpnFocus:
				case InfoInputBaseitems.FEATWpnSpec:
				case InfoInputBaseitems.FEATEpicDevCrit:
				case InfoInputBaseitems.FEATEpicWpnFocus:
				case InfoInputBaseitems.FEATEpicWpnSpec:
				case InfoInputBaseitems.FEATOverWhCrit:
				case InfoInputBaseitems.FEATWpnOfChoice:
				case InfoInputBaseitems.FEATGrtrWpnFocus:
				case InfoInputBaseitems.FEATGrtrWpnSpec:
				case InfoInputBaseitems.FEATPowerCrit:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Alertness"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.featLabels.Count) // it_PathFeat2da.Checked
						{
							info += Info.featLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.AC_Enchant: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "dodge"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1 && result < 5)
					{
						switch (result)
						{
							case 0: info += "dodge";      break;
							case 1: info += "natural";    break;
							case 2: info += "armor";      break;
							case 3: info += "shield";     break;
							case 4: info += "deflection"; break;
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.BaseAC:
				case InfoInputBaseitems.ArmorCheckPen:
				case InfoInputBaseitems.ArcaneSpellFailure:
					info = Table.Cols[col].text + ": for shields only (see ArmorRuleStats.2da for armors)";
					break;

				case InfoInputBaseitems.WeaponMatType: // WeaponSounds.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "Unarmed"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1)
					{
						if (result < Info.weapsoundLabels.Count) // it_PathWeaponSounds2da.Checked
						{
							info += Info.weapsoundLabels[result];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.AmmunitionType: // AmmunitionTypes.2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "arrow"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result) // off by 1 ->
						&& result > 0)
					{
						if (result - 1 < Info.ammoLabels.Count) // it_PathAmmunitionTypes2da.Checked
						{
							info += Info.ammoLabels[result - 1];
						}
						else
							info += val;
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.QBBehaviour: // no 2da
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "none"
					{
						info += gs.non;
					}
					else if (Int32.TryParse(val, out result)
						&& result > -1 && result < 3)
					{
						switch (result)
						{
							case 0: info += "none";                                  break;
							case 1: info += "rods instruments wands and misc items"; break;
							case 2: info += "potions and scrolls";                   break;
						}
					}
					else
						info += gs.bork;
					break;

				case InfoInputBaseitems.CanRotateIcon: // no 2da (bools) ->
				case InfoInputBaseitems.GenderSpecific:
				case InfoInputBaseitems.container:
					info = Table.Cols[col].text + ": ";

					if ((val = Table[id,col].text) == gs.Stars) // NOTE: "****" is 0 which is actually "false"
					{
						info += gs.non;
					}
					else
					{
						switch (val)
						{
							case "0": info += "false"; break;
							case "1": info += "true";  break;

							default: info += gs.bork; break;
						}
					}
					break;
			}

			return info;
		}

		// EquipableSlots
		internal const int EQUIPSLOT_NONE     = 0x00000000; //      0
		internal const int EQUIPSLOT_HELMET   = 0x00000001; //      1
		internal const int EQUIPSLOT_ARMOR    = 0x00000002; //      2
		internal const int EQUIPSLOT_BOOTS    = 0x00000004; //      4
		internal const int EQUIPSLOT_GLOVES   = 0x00000008; //      8
		internal const int EQUIPSLOT_MAINHAND = 0x00000010; //     16
		internal const int EQUIPSLOT_OFFHAND  = 0x00000020; //     32
		internal const int EQUIPSLOT_CLOAK    = 0x00000040; //     64
		internal const int EQUIPSLOT_RINGS    = 0x00000180; //    384 // wtf
		internal const int EQUIPSLOT_AMULET   = 0x00000200; //    512
		internal const int EQUIPSLOT_BELT     = 0x00000400; //   1024
		internal const int EQUIPSLOT_ARROW    = 0x00000800; //   2048
		internal const int EQUIPSLOT_BULLET   = 0x00001000; //   4096
		internal const int EQUIPSLOT_BOLT     = 0x00002000; //   8192
		internal const int EQUIPSLOT_CWEAPON  = 0x0001C000; // 114688 // wtf
		internal const int EQUIPSLOT_CARMOR   = 0x00020000; // 131072

		internal const int EQUIPSLOTS_TOTAL = EQUIPSLOT_HELMET
											| EQUIPSLOT_ARMOR
											| EQUIPSLOT_BOOTS
											| EQUIPSLOT_GLOVES
											| EQUIPSLOT_MAINHAND
											| EQUIPSLOT_OFFHAND
											| EQUIPSLOT_CLOAK
											| EQUIPSLOT_RINGS
											| EQUIPSLOT_AMULET
											| EQUIPSLOT_BELT
											| EQUIPSLOT_ARROW
											| EQUIPSLOT_BULLET
											| EQUIPSLOT_BOLT
											| EQUIPSLOT_CWEAPON
											| EQUIPSLOT_CARMOR;
		#endregion Item info
	}
}


/*		enum InfoStructType
		{
			standard,
			spelltarget,
			fields
		}

		List<InfoStruct> _infoStructs = new List<InfoStruct>();

		struct InfoStruct
		{
			internal InfoStructType type;
			internal string file2da;
			internal List<string> labels;
			internal ToolStripMenuItem it;
			internal int col;
			internal int col1;
			internal int col2;
			internal List<int> ints;
			internal List<float> floats1;
			internal List<float> floats2;
			internal int strt;
			internal int stop;
		}

		internal void CreateInfoStructs()
		{
			var @is = new InfoStruct();
			@is.type = InfoStructType.standard;
			@is.file2da = "baseitems.2da";
			@is.labels = Info.tagLabels;
			@is.it = it_PathBaseItems2da;
			@is.col = 1;
			@is.col1 = -1;
			@is.col2 = -1;
			@is.ints = null;
			@is.floats1 = null;
			@is.floats2 = null;
			@is.strt = 0;
			@is.stop = 0;
			_infoStructs.Add(@is);
		}

		internal void ClearInfoStructs()
		{
			_infoStructs.Clear();
		} */
