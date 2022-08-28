using System;
//using System.Collections.Generic;
using System.Globalization;
//using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
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
//				case -1: // rowhead
//				case  0: // id

				case 1: // "CATEGORY" - Spells.2da or no 2da
					if (it_PathSpells2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars) // this is actually AcidFog ...
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result))
						{
							if (result > -1)
							{
								if (result < Info.spellLabels.Count)
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
					}
					break;

//				case 2: // "REAGENTS"

				case 3: // "TAGS" - BaseItems.2da or no 2da for TCC-types
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars)
					{
						if (val.StartsWith("B", StringComparison.Ordinal)) // is in BaseItems.2da
						{
							info = Table.Cols[col].text + ": [BaseItem] ";

							string[] array = val.Substring(1).Split(','); // lose the "B"
							for (int i = 0; i != array.Length; ++i)
							{
								if (Int32.TryParse(array[i], out result)
									&& result > -1)
								{
									if (result < Info.tagLabels.Count)
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
							info = Table.Cols[col].text + ": [TCC] ";

							string[] array = val.Split(',');
							for (int i = 0; i != array.Length; ++i)
							{
								if (Int32.TryParse(array[i], out result))
								{
									info += GetTccType(result);
								}
								else
									info += gs.bork;

								if (i != array.Length - 1)
									info += ", ";
							}
						}
					}
					break;

				case 4: // "EFFECTS" - ItemPropDef.2da
					if (it_PathItemPropDef2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars)
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
									// NOTE: 'result' is the nwn2 ip-constant
									if (pos != 0 && Int32.TryParse(ipEncoded.Substring(0, pos), out result)
										&& result > -1)
									{
										if (result < Info.ipLabels.Count)
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
					}
					break;

//				case 5: // "OUTPUT"

				case 6: // "SKILL" - Feat.2da and/or Skills.2da
					if ((it_PathFeat2da.Checked || it_PathSkills2da.Checked)
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars)
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							string cat = Table[id,1].text;
							if (!String.IsNullOrEmpty(cat) && cat != gs.Stars)
							{
								int result2;
								if (Int32.TryParse(cat, out result2)) // is triggered by spell id -> SKILL-col is a feat
								{
									if (result < Info.featLabels.Count)
									{
										info += Info.featLabels[result];
									}
									else
										info += val;
								}
								else // is triggered NOT by spell but by mold-tag or is Alchemy or Distillation -> SKILL-col is a skill
								{
									if (result < Info.skillLabels.Count)
									{
										info += Info.skillLabels[result];
									}
									else
										info += val;
								}
							}
							else
								info += gs.non;
						}
						else
							info += gs.bork;
					}
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

		/// <summary>
		/// Gets a description of the parameters for a specified IP/EncodedIP.
		/// </summary>
		/// <param name="ipEncoded">the full IP encode</param>
		/// <param name="ip">the IP</param>
		/// <param name="pos">position of the first delimiter (after the IP,
		/// before the Pars)</param>
		/// <returns></returns>
		static string GetDecodedDescription(string ipEncoded, int ip, int pos)
		{
			string info = String.Empty;

			string pars = ipEncoded.Substring(pos + 1);
			int par;
			switch (ip)
			{
				// Returns Item property ability bonus. You need to specify an
				// ability constant(IP_CONST_ABILITY_*) and the bonus. The bonus should
				// be a positive integer between 1 and 12.
				// itemproperty ItemPropertyAbilityBonus(int nAbility, int nBonus);
				// Iprp_Abilities.2da
				case 0: // ITEM_PROPERTY_ABILITY_BONUS
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ABILITY_STR = 0;
						//int IP_CONST_ABILITY_DEX = 1;
						//int IP_CONST_ABILITY_CON = 2;
						//int IP_CONST_ABILITY_INT = 3;
						//int IP_CONST_ABILITY_WIS = 4;
						//int IP_CONST_ABILITY_CHA = 5;
						case 0: info += "str"; break;
						case 1: info += "dex"; break;
						case 2: info += "con"; break;
						case 3: info += "int"; break;
						case 4: info += "wis"; break;
						case 5: info += "cha"; break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 13)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property AC bonus. You need to specify the bonus.
				// The bonus should be a positive integer between 1 and 20. The modifier
				// type depends on the item it is being applied to.
				// itemproperty ItemPropertyACBonus(int nBonus);
				case 1: // ITEM_PROPERTY_AC_BONUS
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 21)
					{
						info += "+" + par;
					}
					else
						info += gs.bork;
					break;

				// Returns Item property AC bonus vs. alignment group. An example of
				// an alignment group is Chaotic, or Good. You need to specify the
				// alignment group constant(IP_CONST_ALIGNMENTGROUP_*) and the AC bonus.
				// The AC bonus should be an integer between 1 and 20. The modifier
				// type depends on the item it is being applied to.
				// itemproperty ItemPropertyACBonusVsAlign(int nAlignGroup, int nACBonus);
				// Iprp_AlignGrp.2da
				case 2: // ITEM_PROPERTY_AC_BONUS_VS_ALIGNMENT_GROUP
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ALIGNMENTGROUP_ALL     = 0;
						//int IP_CONST_ALIGNMENTGROUP_NEUTRAL = 1;
						//int IP_CONST_ALIGNMENTGROUP_LAWFUL  = 2;
						//int IP_CONST_ALIGNMENTGROUP_CHAOTIC = 3;
						//int IP_CONST_ALIGNMENTGROUP_GOOD    = 4;
						//int IP_CONST_ALIGNMENTGROUP_EVIL    = 5;
						case 0: info += "vs all";     break;
						case 1: info += "vs neutral"; break;
						case 2: info += "vs lawful";  break;
						case 3: info += "vs chaotic"; break;
						case 4: info += "vs good";    break;
						case 5: info += "vs evil";    break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property AC bonus vs. Damage type (ie. piercing). You
				// need to specify the damage type constant(IP_CONST_DAMAGETYPE_*) and the
				// AC bonus. The AC bonus should be an integer between 1 and 20. The
				// modifier type depends on the item it is being applied to.
				// NOTE: Only the first 3 damage types may be used here, the 3 basic physical types.
				// itemproperty ItemPropertyACBonusVsDmgType(int nDamageType, int nACBonus);
				// Iprp_CombatDam.2da
				case 3: // ITEM_PROPERTY_AC_BONUS_VS_DAMAGE_TYPE
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_DAMAGETYPE_BLUDGEONING =  0;
						//int IP_CONST_DAMAGETYPE_PIERCING    =  1;
						//int IP_CONST_DAMAGETYPE_SLASHING    =  2;
						//int IP_CONST_DAMAGETYPE_SUBDUAL     =  3; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_PHYSICAL    =  4; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_MAGICAL     =  5; // not valid ->
						//int IP_CONST_DAMAGETYPE_ACID        =  6;
						//int IP_CONST_DAMAGETYPE_COLD        =  7;
						//int IP_CONST_DAMAGETYPE_DIVINE      =  8;
						//int IP_CONST_DAMAGETYPE_ELECTRICAL  =  9;
						//int IP_CONST_DAMAGETYPE_FIRE        = 10;
						//int IP_CONST_DAMAGETYPE_NEGATIVE    = 11;
						//int IP_CONST_DAMAGETYPE_POSITIVE    = 12;
						//int IP_CONST_DAMAGETYPE_SONIC       = 13;
						case 0: info += "vs bludg"; break;
						case 1: info += "vs pierc"; break;
						case 2: info += "vs slash"; break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property AC bonus vs. Racial group. You need to specify
				// the racial group constant(IP_CONST_RACIALTYPE_*) and the AC bonus. The AC
				// bonus should be an integer between 1 and 20. The modifier type depends
				// on the item it is being applied to.
				// itemproperty ItemPropertyACBonusVsRace(int nRace, int nACBonus);
				// RacialTypes.2da
				case 4: // ITEM_PROPERTY_AC_BONUS_VS_RACIAL_GROUP
//					info += "[2](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.raceLabels.Count != 0 && par < Info.raceLabels.Count)
						{
							info += "vs " + Info.raceLabels[par];
						}
						else
							info += "vs " + par;
					}
					else
						info += gs.bork;

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property AC bonus vs. Specific alignment. You need to
				// specify the specific alignment constant(IP_CONST_ALIGNMENT_*) and the AC
				// bonus. The AC bonus should be an integer between 1 and 20. The
				// modifier type depends on the item it is being applied to.
				// itemproperty ItemPropertyACBonusVsSAlign(int nAlign, int nACBonus);
				// Iprp_Alignment.2da
				case 5: // ITEM_PROPERTY_AC_BONUS_VS_SPECIFIC_ALIGNMENT
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ALIGNMENT_LG = 0;
						//int IP_CONST_ALIGNMENT_LN = 1;
						//int IP_CONST_ALIGNMENT_LE = 2;
						//int IP_CONST_ALIGNMENT_NG = 3;
						//int IP_CONST_ALIGNMENT_TN = 4;
						//int IP_CONST_ALIGNMENT_NE = 5;
						//int IP_CONST_ALIGNMENT_CG = 6;
						//int IP_CONST_ALIGNMENT_CN = 7;
						//int IP_CONST_ALIGNMENT_CE = 8;
						case 0: info += "vs LG"; break;
						case 1: info += "vs LN"; break;
						case 2: info += "vs LE"; break;
						case 3: info += "vs NG"; break;
						case 4: info += "vs TN"; break;
						case 5: info += "vs NE"; break;
						case 6: info += "vs CG"; break;
						case 7: info += "vs CN"; break;
						case 8: info += "vs CE"; break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property Enhancement bonus. You need to specify the
				// enhancement bonus. The Enhancement bonus should be an integer between
				// 1 and 20.
				// itemproperty ItemPropertyEnhancementBonus(int nEnhancementBonus);
				case 6: // ITEM_PROPERTY_ENHANCEMENT_BONUS
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 21)
					{
						info += "+" + par;
					}
					else
						info += gs.bork;
					break;

				// Returns Item property Enhancement bonus vs. an Alignment group. You
				// need to specify the alignment group constant(IP_CONST_ALIGNMENTGROUP_*)
				// and the enhancement bonus. The Enhancement bonus should be an integer
				// between 1 and 20.
				// itemproperty ItemPropertyEnhancementBonusVsAlign(int nAlignGroup, int nBonus);
				// Iprp_AlignGrp.2da
				case 7: // ITEM_PROPERTY_ENHANCEMENT_BONUS_VS_ALIGNMENT_GROUP
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ALIGNMENTGROUP_ALL     = 0;
						//int IP_CONST_ALIGNMENTGROUP_NEUTRAL = 1;
						//int IP_CONST_ALIGNMENTGROUP_LAWFUL  = 2;
						//int IP_CONST_ALIGNMENTGROUP_CHAOTIC = 3;
						//int IP_CONST_ALIGNMENTGROUP_GOOD    = 4;
						//int IP_CONST_ALIGNMENTGROUP_EVIL    = 5;
						case 0: info += "vs all";     break;
						case 1: info += "vs neutral"; break;
						case 2: info += "vs lawful";  break;
						case 3: info += "vs chaotic"; break;
						case 4: info += "vs good";    break;
						case 5: info += "vs evil";    break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property Enhancement bonus vs. Racial group. You need
				// to specify the racial group constant(IP_CONST_RACIALTYPE_*) and the
				// enhancement bonus. The enhancement bonus should be an integer between
				// 1 and 20.
				// itemproperty ItemPropertyEnhancementBonusVsRace(int nRace, int nBonus);
				// RacialTypes.2da
				case 8: // ITEM_PROPERTY_ENHANCEMENT_BONUS_VS_RACIAL_GROUP
//					info += "[2](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.raceLabels.Count != 0 && par < Info.raceLabels.Count)
						{
							info += "vs " + Info.raceLabels[par];
						}
						else
							info += "vs " + par;
					}
					else
						info += gs.bork;

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property Enhancement bonus vs. a specific alignment. You
				// need to specify the alignment constant(IP_CONST_ALIGNMENT_*) and the
				// enhancement bonus. The enhancement bonus should be an integer between
				// 1 and 20.
				// itemproperty ItemPropertyEnhancementBonusVsSAlign(int nAlign, int nBonus);
				// Iprp_Alignment.2da
				case 9: // ITEM_PROPERTY_ENHANCEMENT_BONUS_VS_SPECIFIC_ALIGNEMENT
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ALIGNMENT_LG = 0;
						//int IP_CONST_ALIGNMENT_LN = 1;
						//int IP_CONST_ALIGNMENT_LE = 2;
						//int IP_CONST_ALIGNMENT_NG = 3;
						//int IP_CONST_ALIGNMENT_TN = 4;
						//int IP_CONST_ALIGNMENT_NE = 5;
						//int IP_CONST_ALIGNMENT_CG = 6;
						//int IP_CONST_ALIGNMENT_CN = 7;
						//int IP_CONST_ALIGNMENT_CE = 8;
						case 0: info += "vs LG"; break;
						case 1: info += "vs LN"; break;
						case 2: info += "vs LE"; break;
						case 3: info += "vs NG"; break;
						case 4: info += "vs TN"; break;
						case 5: info += "vs NE"; break;
						case 6: info += "vs CG"; break;
						case 7: info += "vs CN"; break;
						case 8: info += "vs CE"; break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property Enhancment penalty. You need to specify the
				// enhancement penalty. The enhancement penalty should be a POSITIVE
				// integer between 1 and 5 (ie. 1 = -1).
				// itemproperty ItemPropertyEnhancementPenalty(int nPenalty);
				case 10: // ITEM_PROPERTY_DECREASED_ENHANCEMENT_MODIFIER
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 6)
					{
						info += "-" + par;
					}
					else
						info += gs.bork;
					break;

				// Returns Item property weight reduction. You need to specify the weight
				// reduction constant(IP_CONST_REDUCEDWEIGHT_*).
				// itemproperty ItemPropertyWeightReduction(int nReduction);
				case 11: // ITEM_PROPERTY_BASE_ITEM_WEIGHT_REDUCTION
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_REDUCEDWEIGHT_80_PERCENT = 1;
						//int IP_CONST_REDUCEDWEIGHT_60_PERCENT = 2;
						//int IP_CONST_REDUCEDWEIGHT_40_PERCENT = 3;
						//int IP_CONST_REDUCEDWEIGHT_20_PERCENT = 4;
						//int IP_CONST_REDUCEDWEIGHT_10_PERCENT = 5;
						//int IP_CONST_REDUCEDWEIGHT_50_PERCENT = 6;
						//int IP_CONST_REDUCEDWEIGHT_30_PERCENT = 7;
						//int IP_CONST_REDUCEDWEIGHT_01_PERCENT = 8;
						//int IP_CONST_REDUCEDWEIGHT_70_PERCENT = 9;
						case 1: info += "80%"; break;
						case 2: info += "60%"; break;
						case 3: info += "40%"; break;
						case 4: info += "20%"; break;
						case 5: info += "10%"; break;
						case 6: info += "50%"; break;
						case 7: info += "30%"; break;
						case 8: info +=  "1%"; break;
						case 9: info += "70%"; break;

						default: info += gs.bork; break;
					}
					break;

				// Returns Item property Bonus Feat. You need to specify the the feat
				// constant(IP_CONST_FEAT_*).
				// itemproperty ItemPropertyBonusFeat(int nFeat);
				// Iprp_Feats.2da
				case 12: // ITEM_PROPERTY_BONUS_FEAT
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.ipfeatLabels.Count != 0 && par < Info.ipfeatLabels.Count)
						{
							info += Info.ipfeatLabels[par];
						}
						else
							info += par.ToString(CultureInfo.InvariantCulture);
					}
					else
						info += gs.bork;
					break;

				// Returns Item property Bonus level spell (Bonus spell of level). You must
				// specify the class constant(IP_CONST_CLASS_*) of the bonus spell(MUST BE a
				// spell casting class) and the level of the bonus spell. The level of the
				// bonus spell should be an integer between 0 and 9.
				// itemproperty ItemPropertyBonusLevelSpell(int nClass, int nSpellLevel);
				// Classes.2da
				case 13: // ITEM_PROPERTY_BONUS_SPELL_SLOT_OF_LEVEL_N
//					info += "[2](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1) // TODO: Check if 'nClass' has "SpellCaster" enabled in Classes.2da
					{
						if (Info.classLabels.Count != 0 && par < Info.classLabels.Count)
						{
							info += Info.classLabels[par];
						}
						else
							info += par.ToString(CultureInfo.InvariantCulture);
					}
					else
						info += gs.bork;

					if ((par = GetPar(pars, 1)) > -1 && par < 10)
					{
						info += " L" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

//				case 14: // no case 14 in nwscript: "Boomerang" in ItemPropDef.2da

				// Returns Item property Cast spell. You must specify the spell constant
				// (IP_CONST_CASTSPELL_*) and the number of uses constant(IP_CONST_CASTSPELL_NUMUSES_*).
				// NOTE: The number after the name of the spell in the constant is the level
				// at which the spell will be cast. Sometimes there are multiple copies
				// of the same spell but they each are cast at a different level. The higher
				// the level, the more cost will be added to the item.
				// NOTE: The list of spells that can be applied to an item will depend on the
				// item type. For instance there are spells that can be applied to a wand
				// that cannot be applied to a potion. Below is a list of the types and the
				// spells that are allowed to be placed on them. If you try to put a cast
				// spell effect on an item that is not allowed to have that effect it will
				// not work.
				// NOTE: Even if spells have multiple versions of different levels they are only
				// listed below once.
				// itemproperty ItemPropertyCastSpell(int nSpell, int nNumUses);
				// Iprp_Spells.2da
				case 15: // ITEM_PROPERTY_CAST_SPELL
//					info += "[2](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.ipspellLabels.Count != 0 && par < Info.ipspellLabels.Count)
						{
							info += Info.ipspellLabels[par] + " L" + Info.ipspellLevels[par];
						}
						else
							info += par.ToString(CultureInfo.InvariantCulture);
					}
					else
						info += gs.bork;

					switch (GetPar(pars, 1))
					{
						//int IP_CONST_CASTSPELL_NUMUSES_SINGLE_USE        =  1;
						//int IP_CONST_CASTSPELL_NUMUSES_5_CHARGES_PER_USE =  2;
						//int IP_CONST_CASTSPELL_NUMUSES_4_CHARGES_PER_USE =  3;
						//int IP_CONST_CASTSPELL_NUMUSES_3_CHARGES_PER_USE =  4;
						//int IP_CONST_CASTSPELL_NUMUSES_2_CHARGES_PER_USE =  5;
						//int IP_CONST_CASTSPELL_NUMUSES_1_CHARGE_PER_USE  =  6;
						//int IP_CONST_CASTSPELL_NUMUSES_0_CHARGES_PER_USE =  7;
						//int IP_CONST_CASTSPELL_NUMUSES_1_USE_PER_DAY     =  8;
						//int IP_CONST_CASTSPELL_NUMUSES_2_USES_PER_DAY    =  9;
						//int IP_CONST_CASTSPELL_NUMUSES_3_USES_PER_DAY    = 10;
						//int IP_CONST_CASTSPELL_NUMUSES_4_USES_PER_DAY    = 11;
						//int IP_CONST_CASTSPELL_NUMUSES_5_USES_PER_DAY    = 12;
						//int IP_CONST_CASTSPELL_NUMUSES_UNLIMITED_USE     = 13;
						case  1: info += " / 1 use";      break;
						case  2: info += " / 5 per use";  break;
						case  3: info += " / 4 per use "; break;
						case  4: info += " / 3 per use";  break;
						case  5: info += " / 2 per use";  break;
						case  6: info += " / 1 per use";  break;
						case  7: info += " / 0 per use";  break;
						case  8: info += " / 1 per day";  break;
						case  9: info += " / 2 per day";  break;
						case 10: info += " / 3 per day";  break;
						case 11: info += " / 4 per day";  break;
						case 12: info += " / 5 per day";  break;
						case 13: info += " / infinite";   break;

						default: info += gs.Space + gs.bork; break;
					}
					break;

				// Returns Item property damage bonus. You must specify the damage type constant
				// (IP_CONST_DAMAGETYPE_*) and the amount of damage constant(IP_CONST_DAMAGEBONUS_*).
				// NOTE: not all the damage types will work, use only the following: Acid, Bludgeoning,
				// Cold, Electrical, Fire, Piercing, Slashing, Sonic.
				// itemproperty ItemPropertyDamageBonus(int nDamageType, int nDamage);
				// Iprp_DamageType.2da
				case 16: // ITEM_PROPERTY_DAMAGE_BONUS
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_DAMAGETYPE_BLUDGEONING =  0;
						//int IP_CONST_DAMAGETYPE_PIERCING    =  1;
						//int IP_CONST_DAMAGETYPE_SLASHING    =  2;
						//int IP_CONST_DAMAGETYPE_SUBDUAL     =  3; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_PHYSICAL    =  4; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_MAGICAL     =  5; // not valid
						//int IP_CONST_DAMAGETYPE_ACID        =  6;
						//int IP_CONST_DAMAGETYPE_COLD        =  7;
						//int IP_CONST_DAMAGETYPE_DIVINE      =  8; // not valid
						//int IP_CONST_DAMAGETYPE_ELECTRICAL  =  9;
						//int IP_CONST_DAMAGETYPE_FIRE        = 10;
						//int IP_CONST_DAMAGETYPE_NEGATIVE    = 11; // not valid
						//int IP_CONST_DAMAGETYPE_POSITIVE    = 12; // not valid
						//int IP_CONST_DAMAGETYPE_SONIC       = 13;
						case  0: info += "bludg";      break;
						case  1: info += "pierc";      break;
						case  2: info += "slash";      break;
						case  5: info += "magical^";   break;
						case  6: info += "acid";       break;
						case  7: info += "cold";       break;
						case  8: info += "divine^";    break;
						case  9: info += "electrical"; break;
						case 10: info += "fire";       break;
						case 11: info += "negative^";  break;
						case 12: info += "positive^";  break;
						case 13: info += "sonic";      break; // TODO: Test other damage-types.

						default: info += gs.bork; break;
					}

					switch (GetPar(pars, 1))
					{
						//int IP_CONST_DAMAGEBONUS_1    =  1;
						//int IP_CONST_DAMAGEBONUS_2    =  2;
						//int IP_CONST_DAMAGEBONUS_3    =  3;
						//int IP_CONST_DAMAGEBONUS_4    =  4;
						//int IP_CONST_DAMAGEBONUS_5    =  5;
						//int IP_CONST_DAMAGEBONUS_1d4  =  6;
						//int IP_CONST_DAMAGEBONUS_1d6  =  7;
						//int IP_CONST_DAMAGEBONUS_1d8  =  8;
						//int IP_CONST_DAMAGEBONUS_1d10 =  9;
						//int IP_CONST_DAMAGEBONUS_2d6  = 10;
						//int IP_CONST_DAMAGEBONUS_2d8  = 11;
						//int IP_CONST_DAMAGEBONUS_2d4  = 12;
						//int IP_CONST_DAMAGEBONUS_2d10 = 13;
						//int IP_CONST_DAMAGEBONUS_1d12 = 14;
						//int IP_CONST_DAMAGEBONUS_2d12 = 15;
						//int IP_CONST_DAMAGEBONUS_6    = 16;
						//int IP_CONST_DAMAGEBONUS_7    = 17;
						//int IP_CONST_DAMAGEBONUS_8    = 18;
						//int IP_CONST_DAMAGEBONUS_9    = 19;
						//int IP_CONST_DAMAGEBONUS_10   = 20;
						//int IP_CONST_DAMAGEBONUS_3d10 = 51;
						//int IP_CONST_DAMAGEBONUS_3d12 = 52;
						//int IP_CONST_DAMAGEBONUS_4d6  = 53;
						//int IP_CONST_DAMAGEBONUS_4d8  = 54;
						//int IP_CONST_DAMAGEBONUS_4d10 = 55;
						//int IP_CONST_DAMAGEBONUS_4d12 = 56;
						//int IP_CONST_DAMAGEBONUS_5d6  = 57;
						//int IP_CONST_DAMAGEBONUS_5d12 = 58;
						//int IP_CONST_DAMAGEBONUS_6d12 = 59;
						//int IP_CONST_DAMAGEBONUS_3d6  = 60;
						//int IP_CONST_DAMAGEBONUS_6d6  = 61;
						case  1: info += " +1";    break;
						case  2: info += " +2";    break;
						case  3: info += " +3";    break;
						case  4: info += " +4";    break;
						case  5: info += " +5";    break;
						case  6: info += " +1d4";  break;
						case  7: info += " +1d6";  break;
						case  8: info += " +1d8";  break;
						case  9: info += " +1d10"; break;
						case 10: info += " +2d6";  break;
						case 11: info += " +2d8";  break;
						case 12: info += " +2d4";  break;
						case 13: info += " +2d10"; break;
						case 14: info += " +1d12"; break;
						case 15: info += " +2d12"; break;
						case 16: info += " +6";    break;
						case 17: info += " +7";    break;
						case 18: info += " +8";    break;
						case 19: info += " +9";    break;
						case 20: info += " +10";   break;
						case 51: info += " +3d10"; break;
						case 52: info += " +3d12"; break;
						case 53: info += " +4d6";  break;
						case 54: info += " +4d8";  break;
						case 55: info += " +4d10"; break;
						case 56: info += " +4d12"; break;
						case 57: info += " +5d6";  break;
						case 58: info += " +5d12"; break;
						case 59: info += " +6d12"; break;
						case 60: info += " +3d6";  break;
						case 61: info += " +6d6";  break;

						default: info += gs.Space + gs.bork; break;
					}
					break;

				// Returns Item property damage bonus vs. Alignment groups. You must specify the
				// alignment group constant(IP_CONST_ALIGNMENTGROUP_*) and the damage type constant
				// (IP_CONST_DAMAGETYPE_*) and the amount of damage constant(IP_CONST_DAMAGEBONUS_*).
				// NOTE: not all the damage types will work, use only the following: Acid, Bludgeoning,
				// Cold, Electrical, Fire, Piercing, Slashing, Sonic.
				// itemproperty ItemPropertyDamageBonusVsAlign(int nAlignGroup, int nDamageType, int nDamage);
				// Iprp_AlignGrp.2da
				case 17: // ITEM_PROPERTY_DAMAGE_BONUS_VS_ALIGNMENT_GROUP
//					info += "[3](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ALIGNMENTGROUP_ALL     = 0;
						//int IP_CONST_ALIGNMENTGROUP_NEUTRAL = 1;
						//int IP_CONST_ALIGNMENTGROUP_LAWFUL  = 2;
						//int IP_CONST_ALIGNMENTGROUP_CHAOTIC = 3;
						//int IP_CONST_ALIGNMENTGROUP_GOOD    = 4;
						//int IP_CONST_ALIGNMENTGROUP_EVIL    = 5;
						case 0: info += "vs all";     break;
						case 1: info += "vs neutral"; break;
						case 2: info += "vs lawful";  break;
						case 3: info += "vs chaotic"; break;
						case 4: info += "vs good";    break;
						case 5: info += "vs evil";    break;

						default: info += gs.bork; break;
					}

					switch (GetPar(pars, 1))
					{
						//int IP_CONST_DAMAGETYPE_BLUDGEONING =  0;
						//int IP_CONST_DAMAGETYPE_PIERCING    =  1;
						//int IP_CONST_DAMAGETYPE_SLASHING    =  2;
						//int IP_CONST_DAMAGETYPE_SUBDUAL     =  3; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_PHYSICAL    =  4; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_MAGICAL     =  5; // not valid
						//int IP_CONST_DAMAGETYPE_ACID        =  6;
						//int IP_CONST_DAMAGETYPE_COLD        =  7;
						//int IP_CONST_DAMAGETYPE_DIVINE      =  8; // not valid
						//int IP_CONST_DAMAGETYPE_ELECTRICAL  =  9;
						//int IP_CONST_DAMAGETYPE_FIRE        = 10;
						//int IP_CONST_DAMAGETYPE_NEGATIVE    = 11; // not valid
						//int IP_CONST_DAMAGETYPE_POSITIVE    = 12; // not valid
						//int IP_CONST_DAMAGETYPE_SONIC       = 13;
						case  0: info += " bludg";      break;
						case  1: info += " pierc";      break;
						case  2: info += " slash";      break;
						case  5: info += " magical^";   break;
						case  6: info += " acid";       break;
						case  7: info += " cold";       break;
						case  8: info += " divine^";    break;
						case  9: info += " electrical"; break;
						case 10: info += " fire";       break;
						case 11: info += " negative^";  break;
						case 12: info += " positive^";  break;
						case 13: info += " sonic";      break; // TODO: Test other damage-types.

						default: info += gs.Space + gs.bork; break;
					}

					switch (GetPar(pars, 2))
					{
						//int IP_CONST_DAMAGEBONUS_1    =  1;
						//int IP_CONST_DAMAGEBONUS_2    =  2;
						//int IP_CONST_DAMAGEBONUS_3    =  3;
						//int IP_CONST_DAMAGEBONUS_4    =  4;
						//int IP_CONST_DAMAGEBONUS_5    =  5;
						//int IP_CONST_DAMAGEBONUS_1d4  =  6;
						//int IP_CONST_DAMAGEBONUS_1d6  =  7;
						//int IP_CONST_DAMAGEBONUS_1d8  =  8;
						//int IP_CONST_DAMAGEBONUS_1d10 =  9;
						//int IP_CONST_DAMAGEBONUS_2d6  = 10;
						//int IP_CONST_DAMAGEBONUS_2d8  = 11;
						//int IP_CONST_DAMAGEBONUS_2d4  = 12;
						//int IP_CONST_DAMAGEBONUS_2d10 = 13;
						//int IP_CONST_DAMAGEBONUS_1d12 = 14;
						//int IP_CONST_DAMAGEBONUS_2d12 = 15;
						//int IP_CONST_DAMAGEBONUS_6    = 16;
						//int IP_CONST_DAMAGEBONUS_7    = 17;
						//int IP_CONST_DAMAGEBONUS_8    = 18;
						//int IP_CONST_DAMAGEBONUS_9    = 19;
						//int IP_CONST_DAMAGEBONUS_10   = 20;
						//int IP_CONST_DAMAGEBONUS_3d10 = 51;
						//int IP_CONST_DAMAGEBONUS_3d12 = 52;
						//int IP_CONST_DAMAGEBONUS_4d6  = 53;
						//int IP_CONST_DAMAGEBONUS_4d8  = 54;
						//int IP_CONST_DAMAGEBONUS_4d10 = 55;
						//int IP_CONST_DAMAGEBONUS_4d12 = 56;
						//int IP_CONST_DAMAGEBONUS_5d6  = 57;
						//int IP_CONST_DAMAGEBONUS_5d12 = 58;
						//int IP_CONST_DAMAGEBONUS_6d12 = 59;
						//int IP_CONST_DAMAGEBONUS_3d6  = 60;
						//int IP_CONST_DAMAGEBONUS_6d6  = 61;
						case  1: info += " +1";    break;
						case  2: info += " +2";    break;
						case  3: info += " +3";    break;
						case  4: info += " +4";    break;
						case  5: info += " +5";    break;
						case  6: info += " +1d4";  break;
						case  7: info += " +1d6";  break;
						case  8: info += " +1d8";  break;
						case  9: info += " +1d10"; break;
						case 10: info += " +2d6";  break;
						case 11: info += " +2d8";  break;
						case 12: info += " +2d4";  break;
						case 13: info += " +2d10"; break;
						case 14: info += " +1d12"; break;
						case 15: info += " +2d12"; break;
						case 16: info += " +6";    break;
						case 17: info += " +7";    break;
						case 18: info += " +8";    break;
						case 19: info += " +9";    break;
						case 20: info += " +10";   break;
						case 51: info += " +3d10"; break;
						case 52: info += " +3d12"; break;
						case 53: info += " +4d6";  break;
						case 54: info += " +4d8";  break;
						case 55: info += " +4d10"; break;
						case 56: info += " +4d12"; break;
						case 57: info += " +5d6";  break;
						case 58: info += " +5d12"; break;
						case 59: info += " +6d12"; break;
						case 60: info += " +3d6";  break;
						case 61: info += " +6d6";  break;

						default: info += gs.Space + gs.bork; break;
					}
					break;

				// Returns Item property damage bonus vs. specific race. You must specify the
				// racial group constant(IP_CONST_RACIALTYPE_*) and the damage type constant
				// (IP_CONST_DAMAGETYPE_*) and the amount of damage constant(IP_CONST_DAMAGEBONUS_*).
				// NOTE: not all the damage types will work, use only the following: Acid, Bludgeoning,
				// Cold, Electrical, Fire, Piercing, Slashing, Sonic.
				// itemproperty ItemPropertyDamageBonusVsRace(int nRace, int nDamageType, int nDamage);
				// RacialTypes.2da
				case 18: // ITEM_PROPERTY_DAMAGE_BONUS_VS_RACIAL_GROUP
//					info += "[3](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.raceLabels.Count != 0 && par < Info.raceLabels.Count)
						{
							info += "vs " + Info.raceLabels[par];
						}
						else
							info += "vs " + par;
					}
					else
						info += gs.bork;

					switch (GetPar(pars, 1))
					{
						//int IP_CONST_DAMAGETYPE_BLUDGEONING =  0;
						//int IP_CONST_DAMAGETYPE_PIERCING    =  1;
						//int IP_CONST_DAMAGETYPE_SLASHING    =  2;
						//int IP_CONST_DAMAGETYPE_SUBDUAL     =  3; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_PHYSICAL    =  4; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_MAGICAL     =  5; // not valid
						//int IP_CONST_DAMAGETYPE_ACID        =  6;
						//int IP_CONST_DAMAGETYPE_COLD        =  7;
						//int IP_CONST_DAMAGETYPE_DIVINE      =  8; // not valid
						//int IP_CONST_DAMAGETYPE_ELECTRICAL  =  9;
						//int IP_CONST_DAMAGETYPE_FIRE        = 10;
						//int IP_CONST_DAMAGETYPE_NEGATIVE    = 11; // not valid
						//int IP_CONST_DAMAGETYPE_POSITIVE    = 12; // not valid
						//int IP_CONST_DAMAGETYPE_SONIC       = 13;
						case  0: info += " bludg";      break;
						case  1: info += " pierc";      break;
						case  2: info += " slash";      break;
						case  5: info += " magical^";   break;
						case  6: info += " acid";       break;
						case  7: info += " cold";       break;
						case  8: info += " divine^";    break;
						case  9: info += " electrical"; break;
						case 10: info += " fire";       break;
						case 11: info += " negative^";  break;
						case 12: info += " positive^";  break;
						case 13: info += " sonic";      break; // TODO: Test other damage-types.

						default: info += gs.Space + gs.bork; break;
					}

					switch (GetPar(pars, 2))
					{
						//int IP_CONST_DAMAGEBONUS_1    =  1;
						//int IP_CONST_DAMAGEBONUS_2    =  2;
						//int IP_CONST_DAMAGEBONUS_3    =  3;
						//int IP_CONST_DAMAGEBONUS_4    =  4;
						//int IP_CONST_DAMAGEBONUS_5    =  5;
						//int IP_CONST_DAMAGEBONUS_1d4  =  6;
						//int IP_CONST_DAMAGEBONUS_1d6  =  7;
						//int IP_CONST_DAMAGEBONUS_1d8  =  8;
						//int IP_CONST_DAMAGEBONUS_1d10 =  9;
						//int IP_CONST_DAMAGEBONUS_2d6  = 10;
						//int IP_CONST_DAMAGEBONUS_2d8  = 11;
						//int IP_CONST_DAMAGEBONUS_2d4  = 12;
						//int IP_CONST_DAMAGEBONUS_2d10 = 13;
						//int IP_CONST_DAMAGEBONUS_1d12 = 14;
						//int IP_CONST_DAMAGEBONUS_2d12 = 15;
						//int IP_CONST_DAMAGEBONUS_6    = 16;
						//int IP_CONST_DAMAGEBONUS_7    = 17;
						//int IP_CONST_DAMAGEBONUS_8    = 18;
						//int IP_CONST_DAMAGEBONUS_9    = 19;
						//int IP_CONST_DAMAGEBONUS_10   = 20;
						//int IP_CONST_DAMAGEBONUS_3d10 = 51;
						//int IP_CONST_DAMAGEBONUS_3d12 = 52;
						//int IP_CONST_DAMAGEBONUS_4d6  = 53;
						//int IP_CONST_DAMAGEBONUS_4d8  = 54;
						//int IP_CONST_DAMAGEBONUS_4d10 = 55;
						//int IP_CONST_DAMAGEBONUS_4d12 = 56;
						//int IP_CONST_DAMAGEBONUS_5d6  = 57;
						//int IP_CONST_DAMAGEBONUS_5d12 = 58;
						//int IP_CONST_DAMAGEBONUS_6d12 = 59;
						//int IP_CONST_DAMAGEBONUS_3d6  = 60;
						//int IP_CONST_DAMAGEBONUS_6d6  = 61;
						case  1: info += " +1";    break;
						case  2: info += " +2";    break;
						case  3: info += " +3";    break;
						case  4: info += " +4";    break;
						case  5: info += " +5";    break;
						case  6: info += " +1d4";  break;
						case  7: info += " +1d6";  break;
						case  8: info += " +1d8";  break;
						case  9: info += " +1d10"; break;
						case 10: info += " +2d6";  break;
						case 11: info += " +2d8";  break;
						case 12: info += " +2d4";  break;
						case 13: info += " +2d10"; break;
						case 14: info += " +1d12"; break;
						case 15: info += " +2d12"; break;
						case 16: info += " +6";    break;
						case 17: info += " +7";    break;
						case 18: info += " +8";    break;
						case 19: info += " +9";    break;
						case 20: info += " +10";   break;
						case 51: info += " +3d10"; break;
						case 52: info += " +3d12"; break;
						case 53: info += " +4d6";  break;
						case 54: info += " +4d8";  break;
						case 55: info += " +4d10"; break;
						case 56: info += " +4d12"; break;
						case 57: info += " +5d6";  break;
						case 58: info += " +5d12"; break;
						case 59: info += " +6d12"; break;
						case 60: info += " +3d6";  break;
						case 61: info += " +6d6";  break;

						default: info += gs.Space + gs.bork; break;
					}
					break;

				// Returns Item property damage bonus vs. specific alignment. You must specify the
				// specific alignment constant(IP_CONST_ALIGNMENT_*) and the damage type constant
				// (IP_CONST_DAMAGETYPE_*) and the amount of damage constant(IP_CONST_DAMAGEBONUS_*).
				// NOTE: not all the damage types will work, use only the following: Acid, Bludgeoning,
				// Cold, Electrical, Fire, Piercing, Slashing, Sonic.
				// itemproperty ItemPropertyDamageBonusVsSAlign(int nAlign, int nDamageType, int nDamage);
				// Iprp_Alignment.2da
				case 19: // ITEM_PROPERTY_DAMAGE_BONUS_VS_SPECIFIC_ALIGNMENT
//					info += "[3](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ALIGNMENT_LG = 0;
						//int IP_CONST_ALIGNMENT_LN = 1;
						//int IP_CONST_ALIGNMENT_LE = 2;
						//int IP_CONST_ALIGNMENT_NG = 3;
						//int IP_CONST_ALIGNMENT_TN = 4;
						//int IP_CONST_ALIGNMENT_NE = 5;
						//int IP_CONST_ALIGNMENT_CG = 6;
						//int IP_CONST_ALIGNMENT_CN = 7;
						//int IP_CONST_ALIGNMENT_CE = 8;
						case 0: info += "vs LG"; break;
						case 1: info += "vs LN"; break;
						case 2: info += "vs LE"; break;
						case 3: info += "vs NG"; break;
						case 4: info += "vs TN"; break;
						case 5: info += "vs NE"; break;
						case 6: info += "vs CG"; break;
						case 7: info += "vs CN"; break;
						case 8: info += "vs CE"; break;

						default: info += gs.bork; break;
					}

					switch (GetPar(pars, 1))
					{
						//int IP_CONST_DAMAGETYPE_BLUDGEONING =  0;
						//int IP_CONST_DAMAGETYPE_PIERCING    =  1;
						//int IP_CONST_DAMAGETYPE_SLASHING    =  2;
						//int IP_CONST_DAMAGETYPE_SUBDUAL     =  3; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_PHYSICAL    =  4; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_MAGICAL     =  5; // not valid
						//int IP_CONST_DAMAGETYPE_ACID        =  6;
						//int IP_CONST_DAMAGETYPE_COLD        =  7;
						//int IP_CONST_DAMAGETYPE_DIVINE      =  8; // not valid
						//int IP_CONST_DAMAGETYPE_ELECTRICAL  =  9;
						//int IP_CONST_DAMAGETYPE_FIRE        = 10;
						//int IP_CONST_DAMAGETYPE_NEGATIVE    = 11; // not valid
						//int IP_CONST_DAMAGETYPE_POSITIVE    = 12; // not valid
						//int IP_CONST_DAMAGETYPE_SONIC       = 13;
						case  0: info += " bludg";      break;
						case  1: info += " pierc";      break;
						case  2: info += " slash";      break;
						case  5: info += " magical^";   break;
						case  6: info += " acid";       break;
						case  7: info += " cold";       break;
						case  8: info += " divine^";    break;
						case  9: info += " electrical"; break;
						case 10: info += " fire";       break;
						case 11: info += " negative^";  break;
						case 12: info += " positive^";  break;
						case 13: info += " sonic";      break; // TODO: Test other damage-types.

						default: info += gs.Space + gs.bork; break;
					}

					switch (GetPar(pars, 2))
					{
						//int IP_CONST_DAMAGEBONUS_1    =  1;
						//int IP_CONST_DAMAGEBONUS_2    =  2;
						//int IP_CONST_DAMAGEBONUS_3    =  3;
						//int IP_CONST_DAMAGEBONUS_4    =  4;
						//int IP_CONST_DAMAGEBONUS_5    =  5;
						//int IP_CONST_DAMAGEBONUS_1d4  =  6;
						//int IP_CONST_DAMAGEBONUS_1d6  =  7;
						//int IP_CONST_DAMAGEBONUS_1d8  =  8;
						//int IP_CONST_DAMAGEBONUS_1d10 =  9;
						//int IP_CONST_DAMAGEBONUS_2d6  = 10;
						//int IP_CONST_DAMAGEBONUS_2d8  = 11;
						//int IP_CONST_DAMAGEBONUS_2d4  = 12;
						//int IP_CONST_DAMAGEBONUS_2d10 = 13;
						//int IP_CONST_DAMAGEBONUS_1d12 = 14;
						//int IP_CONST_DAMAGEBONUS_2d12 = 15;
						//int IP_CONST_DAMAGEBONUS_6    = 16;
						//int IP_CONST_DAMAGEBONUS_7    = 17;
						//int IP_CONST_DAMAGEBONUS_8    = 18;
						//int IP_CONST_DAMAGEBONUS_9    = 19;
						//int IP_CONST_DAMAGEBONUS_10   = 20;
						//int IP_CONST_DAMAGEBONUS_3d10 = 51;
						//int IP_CONST_DAMAGEBONUS_3d12 = 52;
						//int IP_CONST_DAMAGEBONUS_4d6  = 53;
						//int IP_CONST_DAMAGEBONUS_4d8  = 54;
						//int IP_CONST_DAMAGEBONUS_4d10 = 55;
						//int IP_CONST_DAMAGEBONUS_4d12 = 56;
						//int IP_CONST_DAMAGEBONUS_5d6  = 57;
						//int IP_CONST_DAMAGEBONUS_5d12 = 58;
						//int IP_CONST_DAMAGEBONUS_6d12 = 59;
						//int IP_CONST_DAMAGEBONUS_3d6  = 60;
						//int IP_CONST_DAMAGEBONUS_6d6  = 61;
						case  1: info += " +1";    break;
						case  2: info += " +2";    break;
						case  3: info += " +3";    break;
						case  4: info += " +4";    break;
						case  5: info += " +5";    break;
						case  6: info += " +1d4";  break;
						case  7: info += " +1d6";  break;
						case  8: info += " +1d8";  break;
						case  9: info += " +1d10"; break;
						case 10: info += " +2d6";  break;
						case 11: info += " +2d8";  break;
						case 12: info += " +2d4";  break;
						case 13: info += " +2d10"; break;
						case 14: info += " +1d12"; break;
						case 15: info += " +2d12"; break;
						case 16: info += " +6";    break;
						case 17: info += " +7";    break;
						case 18: info += " +8";    break;
						case 19: info += " +9";    break;
						case 20: info += " +10";   break;
						case 51: info += " +3d10"; break;
						case 52: info += " +3d12"; break;
						case 53: info += " +4d6";  break;
						case 54: info += " +4d8";  break;
						case 55: info += " +4d10"; break;
						case 56: info += " +4d12"; break;
						case 57: info += " +5d6";  break;
						case 58: info += " +5d12"; break;
						case 59: info += " +6d12"; break;
						case 60: info += " +3d6";  break;
						case 61: info += " +6d6";  break;

						default: info += gs.Space + gs.bork; break;
					}
					break;

				// Returns Item property damage immunity. You must specify the damage type constant
				// (IP_CONST_DAMAGETYPE_*) that you want to be immune to and the immune bonus percentage
				// constant(IP_CONST_DAMAGEIMMUNITY_*).
				// NOTE: not all the damage types will work, use only the following: Acid, Bludgeoning,
				// Cold, Electrical, Fire, Piercing, Slashing, Sonic.
				// itemproperty ItemPropertyDamageImmunity(int nDamageType, int nImmuneBonus);
				// Iprp_DamageType.2da
				case 20: // ITEM_PROPERTY_IMMUNITY_DAMAGE_TYPE
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_DAMAGETYPE_BLUDGEONING =  0;
						//int IP_CONST_DAMAGETYPE_PIERCING    =  1;
						//int IP_CONST_DAMAGETYPE_SLASHING    =  2;
						//int IP_CONST_DAMAGETYPE_SUBDUAL     =  3; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_PHYSICAL    =  4; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_MAGICAL     =  5; // not valid
						//int IP_CONST_DAMAGETYPE_ACID        =  6;
						//int IP_CONST_DAMAGETYPE_COLD        =  7;
						//int IP_CONST_DAMAGETYPE_DIVINE      =  8; // not valid
						//int IP_CONST_DAMAGETYPE_ELECTRICAL  =  9;
						//int IP_CONST_DAMAGETYPE_FIRE        = 10;
						//int IP_CONST_DAMAGETYPE_NEGATIVE    = 11; // not valid
						//int IP_CONST_DAMAGETYPE_POSITIVE    = 12; // not valid
						//int IP_CONST_DAMAGETYPE_SONIC       = 13;
						case  0: info += "bludg";      break;
						case  1: info += "pierc";      break;
						case  2: info += "slash";      break;
						case  5: info += "magical^";   break;
						case  6: info += "acid";       break;
						case  7: info += "cold";       break;
						case  8: info += "divine^";    break;
						case  9: info += "electrical"; break;
						case 10: info += "fire";       break;
						case 11: info += "negative^";  break;
						case 12: info += "positive^";  break;
						case 13: info += "sonic";      break; // TODO: Test other damage-types.

						default: info += gs.bork; break;
					}

					switch (GetPar(pars, 1))
					{
						//int IP_CONST_DAMAGEIMMUNITY_5_PERCENT   = 1;
						//int IP_CONST_DAMAGEIMMUNITY_10_PERCENT  = 2;
						//int IP_CONST_DAMAGEIMMUNITY_25_PERCENT  = 3;
						//int IP_CONST_DAMAGEIMMUNITY_50_PERCENT  = 4;
						//int IP_CONST_DAMAGEIMMUNITY_75_PERCENT  = 5;
						//int IP_CONST_DAMAGEIMMUNITY_90_PERCENT  = 6;
						//int IP_CONST_DAMAGEIMMUNITY_100_PERCENT = 7;
						case 1: info +=   " 5%"; break;
						case 2: info +=  " 10%"; break;
						case 3: info +=  " 25%"; break;
						case 4: info +=  " 50%"; break;
						case 5: info +=  " 75%"; break;
						case 6: info +=  " 90%"; break;
						case 7: info += " 100%"; break;

						default: info += gs.Space + gs.bork; break;
					}
					break;

				// Returns Item property damage penalty. You must specify the damage penalty.
				// The damage penalty should be a POSITIVE integer between 1 and 5 (ie. 1 = -1).
				// itemproperty ItemPropertyDamagePenalty(int nPenalty);
				case 21: // ITEM_PROPERTY_DECREASED_DAMAGE
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 6)
					{
						info += "-" + par;
					}
					else
						info += gs.bork;
					break;

//				case 22: // ITEM_PROPERTY_DAMAGE_REDUCTION_DEPRECATED "DamageReduced" in ItemPropDef.2da / Iprp_Protection.2da

				// Returns Item property damage resistance. You must specify the damage type
				// constant(IP_CONST_DAMAGETYPE_*) and the amount of HP of damage constant
				// (IP_CONST_DAMAGERESIST_*) that will be resisted against each round.
				// itemproperty ItemPropertyDamageResistance(int nDamageType, int nHPResist);
				// Iprp_DamageType.2da
				case 23: // ITEM_PROPERTY_DAMAGE_RESISTANCE
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_DAMAGETYPE_BLUDGEONING =  0;
						//int IP_CONST_DAMAGETYPE_PIERCING    =  1;
						//int IP_CONST_DAMAGETYPE_SLASHING    =  2;
						//int IP_CONST_DAMAGETYPE_SUBDUAL     =  3; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_PHYSICAL    =  4; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_MAGICAL     =  5;
						//int IP_CONST_DAMAGETYPE_ACID        =  6;
						//int IP_CONST_DAMAGETYPE_COLD        =  7;
						//int IP_CONST_DAMAGETYPE_DIVINE      =  8;
						//int IP_CONST_DAMAGETYPE_ELECTRICAL  =  9;
						//int IP_CONST_DAMAGETYPE_FIRE        = 10;
						//int IP_CONST_DAMAGETYPE_NEGATIVE    = 11;
						//int IP_CONST_DAMAGETYPE_POSITIVE    = 12;
						//int IP_CONST_DAMAGETYPE_SONIC       = 13;
						case  0: info += "bludg";      break;
						case  1: info += "pierc";      break;
						case  2: info += "slash";      break;
//						case  3: info += "subdual (don't use this)";  break;
//						case  4: info += "physical (don't use this)"; break;
						case  5: info += "magical";    break;
						case  6: info += "acid";       break;
						case  7: info += "cold";       break;
						case  8: info += "divine";     break;
						case  9: info += "electrical"; break;
						case 10: info += "fire";       break;
						case 11: info += "negative";   break;
						case 12: info += "positive";   break;
						case 13: info += "sonic";      break;

						default: info += gs.bork; break;
					}

					switch (GetPar(pars, 1))
					{
						//int IP_CONST_DAMAGERESIST_5  =  1;
						//int IP_CONST_DAMAGERESIST_10 =  2;
						//int IP_CONST_DAMAGERESIST_15 =  3;
						//int IP_CONST_DAMAGERESIST_20 =  4;
						//int IP_CONST_DAMAGERESIST_25 =  5;
						//int IP_CONST_DAMAGERESIST_30 =  6;
						//int IP_CONST_DAMAGERESIST_35 =  7;
						//int IP_CONST_DAMAGERESIST_40 =  8;
						//int IP_CONST_DAMAGERESIST_45 =  9;
						//int IP_CONST_DAMAGERESIST_50 = 10;
						case  1: info +=  " 5"; break;
						case  2: info += " 10"; break;
						case  3: info += " 15"; break;
						case  4: info += " 20"; break;
						case  5: info += " 25"; break;
						case  6: info += " 30"; break;
						case  7: info += " 35"; break;
						case  8: info += " 40"; break;
						case  9: info += " 45"; break;
						case 10: info += " 50"; break;

						default: info += gs.Space + gs.bork; break;
					}
					break;

				// Returns Item property damage vulnerability. You must specify the damage type
				// constant(IP_CONST_DAMAGETYPE_*) that you want the user to be extra vulnerable to
				// and the percentage vulnerability constant(IP_CONST_DAMAGEVULNERABILITY_*).
				// itemproperty ItemPropertyDamageVulnerability(int nDamageType, int nVulnerability);
				// Iprp_DamageType.2da
				case 24: // ITEM_PROPERTY_DAMAGE_VULNERABILITY
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_DAMAGETYPE_BLUDGEONING =  0;
						//int IP_CONST_DAMAGETYPE_PIERCING    =  1;
						//int IP_CONST_DAMAGETYPE_SLASHING    =  2;
						//int IP_CONST_DAMAGETYPE_SUBDUAL     =  3; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_PHYSICAL    =  4; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_MAGICAL     =  5;
						//int IP_CONST_DAMAGETYPE_ACID        =  6;
						//int IP_CONST_DAMAGETYPE_COLD        =  7;
						//int IP_CONST_DAMAGETYPE_DIVINE      =  8;
						//int IP_CONST_DAMAGETYPE_ELECTRICAL  =  9;
						//int IP_CONST_DAMAGETYPE_FIRE        = 10;
						//int IP_CONST_DAMAGETYPE_NEGATIVE    = 11;
						//int IP_CONST_DAMAGETYPE_POSITIVE    = 12;
						//int IP_CONST_DAMAGETYPE_SONIC       = 13;
						case  0: info += "bludg";      break;
						case  1: info += "pierc";      break;
						case  2: info += "slash";      break;
//						case  3: info += "subdual (don't use this)";  break;
//						case  4: info += "physical (don't use this)"; break;
						case  5: info += "magical";    break;
						case  6: info += "acid";       break;
						case  7: info += "cold";       break;
						case  8: info += "divine";     break;
						case  9: info += "electrical"; break;
						case 10: info += "fire";       break;
						case 11: info += "negative";   break;
						case 12: info += "positive";   break;
						case 13: info += "sonic";      break;

						default: info += gs.bork; break;
					}

					switch (GetPar(pars, 1))
					{
						//int IP_CONST_DAMAGEVULNERABILITY_5_PERCENT   = 1;
						//int IP_CONST_DAMAGEVULNERABILITY_10_PERCENT  = 2;
						//int IP_CONST_DAMAGEVULNERABILITY_25_PERCENT  = 3;
						//int IP_CONST_DAMAGEVULNERABILITY_50_PERCENT  = 4;
						//int IP_CONST_DAMAGEVULNERABILITY_75_PERCENT  = 5;
						//int IP_CONST_DAMAGEVULNERABILITY_90_PERCENT  = 6;
						//int IP_CONST_DAMAGEVULNERABILITY_100_PERCENT = 7;
						case 1: info +=   " 5%"; break;
						case 2: info +=  " 10%"; break;
						case 3: info +=  " 25%"; break;
						case 4: info +=  " 50%"; break;
						case 5: info +=  " 75%"; break;
						case 6: info +=  " 90%"; break;
						case 7: info += " 100%"; break;

						default: info += gs.Space + gs.bork; break;
					}
					break;

//				case 25: // no case 25 in nwscript: "Dancing_Scimitar" in ItemPropDef.2da

				// Return Item property Darkvision.
				// itemproperty ItemPropertyDarkvision();
				case 26: // ITEM_PROPERTY_DARKVISION
//					info += "[0](";
					info += "(";
					break;

				// Return Item property decrease ability score. You must specify the ability
				// constant(IP_CONST_ABILITY_*) and the modifier constant. The modifier must be
				// a POSITIVE integer between 1 and 10 (ie. 1 = -1).
				// itemproperty ItemPropertyDecreaseAbility(int nAbility, int nModifier);
				// Iprp_Ablities.2da
				case 27: // ITEM_PROPERTY_DECREASED_ABILITY_SCORE
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ABILITY_STR = 0;
						//int IP_CONST_ABILITY_DEX = 1;
						//int IP_CONST_ABILITY_CON = 2;
						//int IP_CONST_ABILITY_INT = 3;
						//int IP_CONST_ABILITY_WIS = 4;
						//int IP_CONST_ABILITY_CHA = 5;
						case 0: info += "str"; break;
						case 1: info += "dex"; break;
						case 2: info += "con"; break;
						case 3: info += "int"; break;
						case 4: info += "wis"; break;
						case 5: info += "cha"; break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 11)
					{
						info += " -" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property decrease Armor Class. You must specify the armor
				// modifier type constant(IP_CONST_ACMODIFIERTYPE_*) and the armor class penalty.
				// The penalty must be a POSITIVE integer between 1 and 5 (ie. 1 = -1).
				// itemproperty ItemPropertyDecreaseAC(int nModifierType, int nPenalty);
				// Iprp_AcModType.2da
				case 28: // ITEM_PROPERTY_DECREASED_AC
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ACMODIFIERTYPE_DODGE      = 0;
						//int IP_CONST_ACMODIFIERTYPE_NATURAL    = 1;
						//int IP_CONST_ACMODIFIERTYPE_ARMOR      = 2;
						//int IP_CONST_ACMODIFIERTYPE_SHIELD     = 3;
						//int IP_CONST_ACMODIFIERTYPE_DEFLECTION = 4;
						case 0: info += "dodge";      break;
						case 1: info += "natural";    break;
						case 2: info += "armor";      break;
						case 3: info += "shield";     break;
						case 4: info += "deflection"; break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 6)
					{
						info += " -" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property decrease skill. You must specify the constant for the
				// skill to be decreased(SKILL_*) and the amount of the penalty. The penalty
				// must be a POSITIVE integer between 1 and 10 (ie. 1 = -1).
				// itemproperty ItemPropertyDecreaseSkill(int nSkill, int nPenalty);
				// Skills.2da
				case 29: // ITEM_PROPERTY_DECREASED_SKILL_MODIFIER
//					info += "[2](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.skillLabels.Count != 0 && par < Info.skillLabels.Count)
						{
							info += Info.skillLabels[par];
						}
						else
							info += par.ToString(CultureInfo.InvariantCulture);
					}
					else
						info += gs.bork;

					if ((par = GetPar(pars, 1)) > 0 && par < 11)
					{
						info += " -" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

//				case 30: // no case 30 in nwscript: "DoubleStack" in ItemPropDef.2da
//				case 31: // no case 31 in nwscript: "EnhancedContainer_BonusSlot" in ItemPropDef.2da

				// Returns Item property container reduced weight. This is used for special
				// containers that reduce the weight of the objects inside them. You must
				// specify the container weight reduction type constant(IP_CONST_CONTAINERWEIGHTRED_*).
				// itemproperty ItemPropertyContainerReducedWeight(int nContainerType);
				case 32: // ITEM_PROPERTY_ENHANCED_CONTAINER_REDUCED_WEIGHT
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_CONTAINERWEIGHTRED_20_PERCENT  = 1;
						//int IP_CONST_CONTAINERWEIGHTRED_40_PERCENT  = 2;
						//int IP_CONST_CONTAINERWEIGHTRED_60_PERCENT  = 3;
						//int IP_CONST_CONTAINERWEIGHTRED_80_PERCENT  = 4;
						//int IP_CONST_CONTAINERWEIGHTRED_100_PERCENT = 5;
						case 1: info +=  "20%"; break;
						case 2: info +=  "40%"; break;
						case 3: info +=  "60%"; break;
						case 4: info +=  "80%"; break;
						case 5: info += "100%"; break;

						default: info += gs.bork; break;
					}
					break;

				// Returns Item property extra melee damage type. You must specify the extra
				// melee base damage type that you want applied. It is a constant(IP_CONST_DAMAGETYPE_*).
				// NOTE: only the first 3 base types (piercing, slashing, & bludgeoning are applicable
				// here.
				// NOTE: It is also only applicable to melee weapons.
				// itemproperty ItemPropertyExtraMeleeDamageType(int nDamageType);
				// Iprp_CombatDam.2da
				case 33: // ITEM_PROPERTY_EXTRA_MELEE_DAMAGE_TYPE
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_DAMAGETYPE_BLUDGEONING =  0;
						//int IP_CONST_DAMAGETYPE_PIERCING    =  1;
						//int IP_CONST_DAMAGETYPE_SLASHING    =  2;
						//int IP_CONST_DAMAGETYPE_SUBDUAL     =  3; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_PHYSICAL    =  4; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_MAGICAL     =  5; // not valid ->
						//int IP_CONST_DAMAGETYPE_ACID        =  6;
						//int IP_CONST_DAMAGETYPE_COLD        =  7;
						//int IP_CONST_DAMAGETYPE_DIVINE      =  8;
						//int IP_CONST_DAMAGETYPE_ELECTRICAL  =  9;
						//int IP_CONST_DAMAGETYPE_FIRE        = 10;
						//int IP_CONST_DAMAGETYPE_NEGATIVE    = 11;
						//int IP_CONST_DAMAGETYPE_POSITIVE    = 12;
						//int IP_CONST_DAMAGETYPE_SONIC       = 13;
						case 0: info += "bludg"; break;
						case 1: info += "pierc"; break;
						case 2: info += "slash"; break;

						default: info += gs.bork; break;
					}
					break;

				// Returns Item property extra ranged damage type. You must specify the extra
				// melee base damage type that you want applied. It is a constant(IP_CONST_DAMAGETYPE_*).
				// NOTE: only the first 3 base types (piercing, slashing, & bludgeoning are applicable
				// here.
				// NOTE: It is also only applicable to ranged weapons.
				// itemproperty ItemPropertyExtraRangeDamageType(int nDamageType);
				// Iprp_CombatDam.2da
				case 34: // ITEM_PROPERTY_EXTRA_RANGED_DAMAGE_TYPE
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_DAMAGETYPE_BLUDGEONING =  0;
						//int IP_CONST_DAMAGETYPE_PIERCING    =  1;
						//int IP_CONST_DAMAGETYPE_SLASHING    =  2;
						//int IP_CONST_DAMAGETYPE_SUBDUAL     =  3; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_PHYSICAL    =  4; // don't use this dType at all.
						//int IP_CONST_DAMAGETYPE_MAGICAL     =  5; // not valid ->
						//int IP_CONST_DAMAGETYPE_ACID        =  6;
						//int IP_CONST_DAMAGETYPE_COLD        =  7;
						//int IP_CONST_DAMAGETYPE_DIVINE      =  8;
						//int IP_CONST_DAMAGETYPE_ELECTRICAL  =  9;
						//int IP_CONST_DAMAGETYPE_FIRE        = 10;
						//int IP_CONST_DAMAGETYPE_NEGATIVE    = 11;
						//int IP_CONST_DAMAGETYPE_POSITIVE    = 12;
						//int IP_CONST_DAMAGETYPE_SONIC       = 13;
						case 0: info += "bludg"; break;
						case 1: info += "pierc"; break;
						case 2: info += "slash"; break;

						default: info += gs.bork; break;
					}
					break;

				// Returns Item property haste.
				// itemproperty ItemPropertyHaste();
				case 35: // ITEM_PROPERTY_HASTE
//					info += "[0](";
					info += "(";
					break;

				// Returns Item property Holy Avenger.
				// itemproperty ItemPropertyHolyAvenger();
				case 36: // ITEM_PROPERTY_HOLY_AVENGER
//					info += "[0](";
					info += "(";
					break;

				// Returns Item property immunity to miscellaneous effects. You must specify the
				// effect to which the user is immune, it is a constant(IP_CONST_IMMUNITYMISC_*).
				// itemproperty ItemPropertyImmunityMisc(int nImmunityType);
				// Iprp_Immunity.2da
				case 37: // ITEM_PROPERTY_IMMUNITY_MISCELLANEOUS
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_IMMUNITYMISC_BACKSTAB         = 0;
						//int IP_CONST_IMMUNITYMISC_LEVEL_ABIL_DRAIN = 1;
						//int IP_CONST_IMMUNITYMISC_MINDSPELLS       = 2;
						//int IP_CONST_IMMUNITYMISC_POISON           = 3;
						//int IP_CONST_IMMUNITYMISC_DISEASE          = 4;
						//int IP_CONST_IMMUNITYMISC_FEAR             = 5;
						//int IP_CONST_IMMUNITYMISC_KNOCKDOWN        = 6;
						//int IP_CONST_IMMUNITYMISC_PARALYSIS        = 7;
						//int IP_CONST_IMMUNITYMISC_CRITICAL_HITS    = 8;
						//int IP_CONST_IMMUNITYMISC_DEATH_MAGIC      = 9;
						case 0: info += "vs backstab";           break;
						case 1: info += "vs level/abilitydrain"; break;
						case 2: info += "vs mindspells";         break;
						case 3: info += "vs poison";             break;
						case 4: info += "vs disease";            break;
						case 5: info += "vs fear";               break;
						case 6: info += "vs knockdown";          break;
						case 7: info += "vs paralysis";          break;
						case 8: info += "vs criticals";          break;
						case 9: info += "vs deathmagic";         break;

						default: info += gs.bork; break;
					}
					break;

				// Returns Item property improved evasion.
				// itemproperty ItemPropertyImprovedEvasion();
				case 38: // ITEM_PROPERTY_IMPROVED_EVASION
//					info += "[0](";
					info += "(";
					break;

				// Returns Item property bonus spell resistance.  You must specify the bonus spell
				// resistance constant(IP_CONST_SPELLRESISTANCEBONUS_*).
				// kL_NOTE: This is not a bonus.
				// itemproperty ItemPropertyBonusSpellResistance(int nBonus);
				case 39: // ITEM_PROPERTY_SPELL_RESISTANCE
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_SPELLRESISTANCEBONUS_10 =  0;
						//int IP_CONST_SPELLRESISTANCEBONUS_12 =  1;
						//int IP_CONST_SPELLRESISTANCEBONUS_14 =  2;
						//int IP_CONST_SPELLRESISTANCEBONUS_16 =  3;
						//int IP_CONST_SPELLRESISTANCEBONUS_18 =  4;
						//int IP_CONST_SPELLRESISTANCEBONUS_20 =  5;
						//int IP_CONST_SPELLRESISTANCEBONUS_22 =  6;
						//int IP_CONST_SPELLRESISTANCEBONUS_24 =  7;
						//int IP_CONST_SPELLRESISTANCEBONUS_26 =  8;
						//int IP_CONST_SPELLRESISTANCEBONUS_28 =  9;
						//int IP_CONST_SPELLRESISTANCEBONUS_30 = 10;
						//int IP_CONST_SPELLRESISTANCEBONUS_32 = 11;
						case  0: info += "10"; break;
						case  1: info += "12"; break;
						case  2: info += "14"; break;
						case  3: info += "16"; break;
						case  4: info += "18"; break;
						case  5: info += "20"; break;
						case  6: info += "22"; break;
						case  7: info += "24"; break;
						case  8: info += "26"; break;
						case  9: info += "28"; break;
						case 10: info += "30"; break;
						case 11: info += "32"; break;

						default: info += gs.bork; break;
					}
					break;

				// Returns Item property saving throw bonus to the base type (ie. will, reflex,
				// fortitude). You must specify the base type constant(IP_CONST_SAVEBASETYPE_*)
				// to which the user gets the bonus and the bonus that he/she will get. The
				// bonus must be an integer between 1 and 20.
				// itemproperty ItemPropertyBonusSavingThrow(int nBaseSaveType, int nBonus);
				// Iprp_SaveElement.2da (does not use that)
				case 40: // ITEM_PROPERTY_SAVING_THROW_BONUS
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_SAVEBASETYPE_ALL       = 0;	// does not work IG.
						//int IP_CONST_SAVEBASETYPE_FORTITUDE = 1;
						//int IP_CONST_SAVEBASETYPE_WILL      = 2;
						//int IP_CONST_SAVEBASETYPE_REFLEX    = 3;
//						case 0: info += "all";  break;				// does not work IG.
						case 1: info += "fort"; break;
						case 2: info += "will"; break;
						case 3: info += "refl"; break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property saving throw bonus vs. a specific effect or damage type.
				// You must specify the save type constant(IP_CONST_SAVEVS_*) that the bonus is
				// applied to and the bonus that is be applied. The bonus must be an integer
				// between 1 and 20.
				// itemproperty ItemPropertyBonusSavingThrowVsX(int nBonusType, int nBonus);
				// Iprp_SavingThrow.2da (does not use that)
				case 41: // ITEM_PROPERTY_SAVING_THROW_BONUS_SPECIFIC
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_SAVEVS_UNIVERSAL     =  0;
						//int IP_CONST_SAVEVS_ACID          =  1;
						//int IP_CONST_SAVEVS_COLD          =  3;
						//int IP_CONST_SAVEVS_DEATH         =  4;
						//int IP_CONST_SAVEVS_DISEASE       =  5;
						//int IP_CONST_SAVEVS_DIVINE        =  6;
						//int IP_CONST_SAVEVS_ELECTRICAL    =  7;
						//int IP_CONST_SAVEVS_FEAR          =  8;
						//int IP_CONST_SAVEVS_FIRE          =  9;
						//int IP_CONST_SAVEVS_MINDAFFECTING = 11;
						//int IP_CONST_SAVEVS_NEGATIVE      = 12;
						//int IP_CONST_SAVEVS_POISON        = 13;
						//int IP_CONST_SAVEVS_POSITIVE      = 14;
						//int IP_CONST_SAVEVS_SONIC         = 15;
						case  0: info += "universal";     break;
						case  1: info += "acid";          break;
						case  3: info += "cold";          break;
						case  4: info += "death";         break;
						case  5: info += "disease";       break;
						case  6: info += "divine";        break;
						case  7: info += "electrical";    break;
						case  8: info += "fear";          break;
						case  9: info += "fire";          break;
						case 11: info += "mindaffecting"; break;
						case 12: info += "negative";      break;
						case 13: info += "poison";        break;
						case 14: info += "positive";      break;
						case 15: info += "sonic";         break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

//				case 42: // no case 42 in nwscript

				// Returns Item property keen. This means a critical threat range of 19-20 on a
				// weapon will be increased to 17-20 etc.
				// itemproperty ItemPropertyKeen();
				case 43: // ITEM_PROPERTY_KEEN
//					info += "[0](";
					info += "(";
					break;

				// Returns Item property light. You must specify the intesity constant of the
				// light(IP_CONST_LIGHTBRIGHTNESS_*) and the color constant of the light
				// (IP_CONST_LIGHTCOLOR_*).
				// itemproperty ItemPropertyLight(int nBrightness, int nColor);
				case 44: // ITEM_PROPERTY_LIGHT
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_LIGHTBRIGHTNESS_DIM    = 1;
						//int IP_CONST_LIGHTBRIGHTNESS_LOW    = 2;
						//int IP_CONST_LIGHTBRIGHTNESS_NORMAL = 3;
						//int IP_CONST_LIGHTBRIGHTNESS_BRIGHT = 4;
						case 1: info += "dim";    break;
						case 2: info += "low";    break;
						case 3: info += "normal"; break;
						case 4: info += "bright"; break;

						default: info += gs.bork; break;
					}

					switch (GetPar(pars, 1))
					{
						//int IP_CONST_LIGHTCOLOR_BLUE   = 0;
						//int IP_CONST_LIGHTCOLOR_YELLOW = 1;
						//int IP_CONST_LIGHTCOLOR_PURPLE = 2;
						//int IP_CONST_LIGHTCOLOR_RED    = 3;
						//int IP_CONST_LIGHTCOLOR_GREEN  = 4;
						//int IP_CONST_LIGHTCOLOR_ORANGE = 5;
						//int IP_CONST_LIGHTCOLOR_WHITE  = 6;
						case 0: info += " blue";   break;
						case 1: info += " yellow"; break;
						case 2: info += " purple"; break;
						case 3: info += " red";    break;
						case 4: info += " green";  break;
						case 5: info += " orange"; break;
						case 6: info += " white";  break;

						default: info += gs.Space + gs.bork; break;
					}
					break;

				// Returns Item property Max range strength modification (ie. mighty). You must
				// specify the maximum modifier for strength that is allowed on a ranged weapon.
				// The modifier must be a positive integer between 1 and 20.
				// itemproperty ItemPropertyMaxRangeStrengthMod(int nModifier);
				case 45: // ITEM_PROPERTY_MIGHTY
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 21)
					{
						info += "+" + par;
					}
					else
						info += gs.bork;
					break;

//				case 46: // ITEM_PROPERTY_MIND_BLANK - no function in nwscript

				// Returns Item property no damage. This means the weapon will do no damage in
				// combat.
				// itemproperty ItemPropertyNoDamage();
				case 47: // ITEM_PROPERTY_NO_DAMAGE
//					info += "[0](";
					info += "(";
					break;

				// Returns Item property on hit -> do effect property. You must specify the on
				// hit property constant(IP_CONST_ONHIT_*) and the save DC constant(IP_CONST_ONHIT_SAVEDC_*).
				// Some of the item properties require a special parameter as well. If the
				// property does not require one you may leave out the last one. The list of
				// the ones with 3 parameters and what they are are as follows:
				// ABILITYDRAIN       nSpecial is the ability it is to drain.
				//                    IP_CONST_ABILITY_*
				// BLINDNESS          nSpecial is the duration/percentage of effecting victim.
				//                    IP_CONST_ONHIT_DURATION_*
				// CONFUSION          nSpecial is the duration/percentage of effecting victim.
				//                    IP_CONST_ONHIT_DURATION_*
				// DAZE               nSpecial is the duration/percentage of effecting victim.
				//                    IP_CONST_ONHIT_DURATION_*
				// DEAFNESS           nSpecial is the duration/percentage of effecting victim.
				//                    IP_CONST_ONHIT_DURATION_*
				// DISEASE            nSpecial is the type of desease that will effect the victim.
				//                    DISEASE_*
				// DOOM               nSpecial is the duration/percentage of effecting victim.
				//                    IP_CONST_ONHIT_DURATION_*
				// FEAR               nSpecial is the duration/percentage of effecting victim.
				//                    IP_CONST_ONHIT_DURATION_*
				// HOLD               nSpecial is the duration/percentage of effecting victim.
				//                    IP_CONST_ONHIT_DURATION_*
				// ITEMPOISON         nSpecial is the type of poison that will effect the victim.
				//                    IP_CONST_POISON_*
				// SILENCE            nSpecial is the duration/percentage of effecting victim.
				//                    IP_CONST_ONHIT_DURATION_*
				// SLAYRACE           nSpecial is the race that will be slain.
				//                    IP_CONST_RACIALTYPE_*
				// SLAYALIGNMENTGROUP nSpecial is the alignment group that will be slain(eg. chaotic).
				//                    IP_CONST_ALIGNMENTGROUP_*
				// SLAYALIGNMENT      nSpecial is the specific alignment that will be slain.
				//                    IP_CONST_ALIGNMENT_*
				// SLEEP              nSpecial is the duration/percentage of effecting victim.
				//                    IP_CONST_ONHIT_DURATION_*
				// SLOW               nSpecial is the duration/percentage of effecting victim.
				//                    IP_CONST_ONHIT_DURATION_*
				// STUN               nSpecial is the duration/percentage of effecting victim.
				//                    IP_CONST_ONHIT_DURATION_*
				// kL_NOTE: For dispel-type spells 'nSaveDC' is used to pass in casterlevel.
				// kL_NOTE: See Iprp_OnHit.2da
				// itemproperty ItemPropertyOnHitProps(int nProperty, int nSaveDC, int nSpecial=0);
				// Iprp_OnHit.2da
				case 48: // ITEM_PROPERTY_ON_HIT_PROPERTIES
//					info += "[2/3](";
					info += "(";
					switch (par = GetPar(pars, 0))
					{
						//int IP_CONST_ONHIT_SLEEP              =  0;
						//int IP_CONST_ONHIT_STUN               =  1;
						//int IP_CONST_ONHIT_HOLD               =  2;
						//int IP_CONST_ONHIT_CONFUSION          =  3;
						//int IP_CONST_ONHIT_DAZE               =  5;
						//int IP_CONST_ONHIT_DOOM               =  6;
						//int IP_CONST_ONHIT_FEAR               =  7;
						//int IP_CONST_ONHIT_KNOCK              =  8;
						//int IP_CONST_ONHIT_SLOW               =  9;
						//int IP_CONST_ONHIT_LESSERDISPEL       = 10;
						//int IP_CONST_ONHIT_DISPELMAGIC        = 11;
						//int IP_CONST_ONHIT_GREATERDISPEL      = 12;
						//int IP_CONST_ONHIT_MORDSDISJUNCTION   = 13;
						//int IP_CONST_ONHIT_SILENCE            = 14;
						//int IP_CONST_ONHIT_DEAFNESS           = 15;
						//int IP_CONST_ONHIT_BLINDNESS          = 16;
						//int IP_CONST_ONHIT_LEVELDRAIN         = 17;
						//int IP_CONST_ONHIT_ABILITYDRAIN       = 18;
						//int IP_CONST_ONHIT_ITEMPOISON         = 19;
						//int IP_CONST_ONHIT_DISEASE            = 20;
						//int IP_CONST_ONHIT_SLAYRACE           = 21;
						//int IP_CONST_ONHIT_SLAYALIGNMENTGROUP = 22;
						//int IP_CONST_ONHIT_SLAYALIGNMENT      = 23;
						//int IP_CONST_ONHIT_VORPAL             = 24;
						//int IP_CONST_ONHIT_WOUNDING           = 25;
						case  0: info += "sleep";              break;
						case  1: info += "stun";               break;
						case  2: info += "hold";               break;
						case  3: info += "confusion";          break;
						case  5: info += "daze";               break;
						case  6: info += "doom";               break;
						case  7: info += "fear";               break;
						case  8: info += "knock";              break;
						case  9: info += "slow";               break;
						case 10: info += "lesserdispel";       break;
						case 11: info += "dispelmagic";        break;
						case 12: info += "greaterdispel";      break;
						case 13: info += "disjunction";        break;
						case 14: info += "silence";            break;
						case 15: info += "deafness";           break;
						case 16: info += "blindness";          break;
						case 17: info += "leveldrain";         break;
						case 18: info += "abilitydrain";       break;
						case 19: info += "poison";             break;
						case 20: info += "disease";            break;
						case 21: info += "slayrace";           break;
						case 22: info += "slayalignmentgroup"; break;
						case 23: info += "slayalignment";      break;
						case 24: info += "vorpal";             break;
						case 25: info += "wounding";           break;

						default: info += gs.bork; break;
					}

					switch (GetPar(pars, 1))
					{
						//int IP_CONST_ONHIT_SAVEDC_14 =  0;
						//int IP_CONST_ONHIT_SAVEDC_16 =  1;
						//int IP_CONST_ONHIT_SAVEDC_18 =  2;
						//int IP_CONST_ONHIT_SAVEDC_20 =  3;
						//int IP_CONST_ONHIT_SAVEDC_22 =  4;
						//int IP_CONST_ONHIT_SAVEDC_24 =  5;
						//int IP_CONST_ONHIT_SAVEDC_26 =  6;
						//int IP_CONST_ONHIT_SAVEDC_28 =  7;
						//int IP_CONST_ONHIT_SAVEDC_30 =  8;
						//int IP_CONST_ONHIT_SAVEDC_32 =  9;
						//int IP_CONST_ONHIT_SAVEDC_34 = 10;
						//int IP_CONST_ONHIT_SAVEDC_36 = 11;
						//int IP_CONST_ONHIT_SAVEDC_38 = 12;
						//int IP_CONST_ONHIT_SAVEDC_40 = 13;
						case  0: info += " dc14"; break;
						case  1: info += " dc16"; break;
						case  2: info += " dc18"; break;
						case  3: info += " dc20"; break;
						case  4: info += " dc22"; break;
						case  5: info += " dc24"; break;
						case  6: info += " dc26"; break;
						case  7: info += " dc28"; break;
						case  8: info += " dc30"; break;
						case  9: info += " dc32"; break;
						case 10: info += " dc34"; break;
						case 11: info += " dc36"; break;
						case 12: info += " dc38"; break;
						case 13: info += " dc40"; break;

						default: info += gs.Space + gs.bork; break;
					}

					switch (par)
					{
						case  0: // sleep
						case  1: // stun
						case  2: // hold
						case  3: // confusion
						case  5: // daze
						case  6: // doom
						case  7: // fear
						case  9: // slow
						case 14: // silence
						case 15: // deafness
						case 16: // blindness
							switch (GetPar(pars, 2))
							{
								//int IP_CONST_ONHIT_DURATION_5_PERCENT_5_ROUNDS  =  0;
								//int IP_CONST_ONHIT_DURATION_10_PERCENT_4_ROUNDS =  1;
								//int IP_CONST_ONHIT_DURATION_25_PERCENT_3_ROUNDS =  2;
								//int IP_CONST_ONHIT_DURATION_50_PERCENT_2_ROUNDS =  3;
								//int IP_CONST_ONHIT_DURATION_75_PERCENT_1_ROUND  =  4;
								//int IP_CONST_ONHIT_DURATION_5_PERCENT_1_ROUNDS  =  5;
								//int IP_CONST_ONHIT_DURATION_5_PERCENT_2_ROUNDS  =  6;
								//int IP_CONST_ONHIT_DURATION_5_PERCENT_3_ROUNDS  =  7;
								//int IP_CONST_ONHIT_DURATION_5_PERCENT_4_ROUNDS  =  8;
								//int IP_CONST_ONHIT_DURATION_10_PERCENT_1_ROUNDS =  9;
								//int IP_CONST_ONHIT_DURATION_10_PERCENT_2_ROUNDS = 10;
								//int IP_CONST_ONHIT_DURATION_10_PERCENT_3_ROUNDS = 11;
								//int IP_CONST_ONHIT_DURATION_10_PERCENT_5_ROUNDS = 12;
								//int IP_CONST_ONHIT_DURATION_25_PERCENT_1_ROUNDS = 13;
								//int IP_CONST_ONHIT_DURATION_25_PERCENT_2_ROUNDS = 14;
								//int IP_CONST_ONHIT_DURATION_25_PERCENT_4_ROUNDS = 15;
								//int IP_CONST_ONHIT_DURATION_25_PERCENT_5_ROUNDS = 16;
								//int IP_CONST_ONHIT_DURATION_33_PERCENT_1_ROUNDS = 17;
								//int IP_CONST_ONHIT_DURATION_33_PERCENT_2_ROUNDS = 18;
								//int IP_CONST_ONHIT_DURATION_33_PERCENT_3_ROUNDS = 19;
								//int IP_CONST_ONHIT_DURATION_33_PERCENT_4_ROUNDS = 20;
								//int IP_CONST_ONHIT_DURATION_33_PERCENT_5_ROUNDS = 21;
								//int IP_CONST_ONHIT_DURATION_50_PERCENT_1_ROUNDS = 22;
								//int IP_CONST_ONHIT_DURATION_50_PERCENT_3_ROUNDS = 23;
								//int IP_CONST_ONHIT_DURATION_50_PERCENT_4_ROUNDS = 24;
								//int IP_CONST_ONHIT_DURATION_50_PERCENT_5_ROUNDS = 25;
								//int IP_CONST_ONHIT_DURATION_66_PERCENT_1_ROUNDS = 26;
								//int IP_CONST_ONHIT_DURATION_66_PERCENT_2_ROUNDS = 27;
								//int IP_CONST_ONHIT_DURATION_66_PERCENT_3_ROUNDS = 28;
								//int IP_CONST_ONHIT_DURATION_66_PERCENT_4_ROUNDS = 29;
								//int IP_CONST_ONHIT_DURATION_66_PERCENT_5_ROUNDS = 30;
								//int IP_CONST_ONHIT_DURATION_75_PERCENT_2_ROUND  = 31;
								//int IP_CONST_ONHIT_DURATION_75_PERCENT_3_ROUND  = 32;
								//int IP_CONST_ONHIT_DURATION_75_PERCENT_4_ROUND  = 33;
								//int IP_CONST_ONHIT_DURATION_75_PERCENT_5_ROUND  = 34;
								//int IP_CONST_ONHIT_DURATION_100_PERCENT_1_ROUND = 35;
								//int IP_CONST_ONHIT_DURATION_100_PERCENT_2_ROUND = 36;
								//int IP_CONST_ONHIT_DURATION_100_PERCENT_3_ROUND = 37;
								//int IP_CONST_ONHIT_DURATION_100_PERCENT_4_ROUND = 38;
								//int IP_CONST_ONHIT_DURATION_100_PERCENT_5_ROUND = 39;
								case  0: info +=   " 5%/5 rnd"; break;
								case  1: info +=  " 10%/4 rnd"; break;
								case  2: info +=  " 25%/3 rnd"; break;
								case  3: info +=  " 50%/2 rnd"; break;
								case  4: info +=  " 75%/1 rnd"; break;
								case  5: info +=   " 5%/1 rnd"; break;
								case  6: info +=   " 5%/2 rnd"; break;
								case  7: info +=   " 5%/3 rnd"; break;
								case  8: info +=   " 5%/4 rnd"; break;
								case  9: info +=  " 10%/1 rnd"; break;
								case 10: info +=  " 10%/2 rnd"; break;
								case 11: info +=  " 10%/3 rnd"; break;
								case 12: info +=  " 10%/5 rnd"; break;
								case 13: info +=  " 25%/1 rnd"; break;
								case 14: info +=  " 25%/2 rnd"; break;
								case 15: info +=  " 25%/4 rnd"; break;
								case 16: info +=  " 25%/5 rnd"; break;
								case 17: info +=  " 33%/1 rnd"; break;
								case 18: info +=  " 33%/2 rnd"; break;
								case 19: info +=  " 33%/3 rnd"; break;
								case 20: info +=  " 33%/4 rnd"; break;
								case 21: info +=  " 33%/5 rnd"; break;
								case 22: info +=  " 50%/1 rnd"; break;
								case 23: info +=  " 50%/3 rnd"; break;
								case 24: info +=  " 50%/4 rnd"; break;
								case 25: info +=  " 50%/5 rnd"; break;
								case 26: info +=  " 66%/1 rnd"; break;
								case 27: info +=  " 66%/2 rnd"; break;
								case 28: info +=  " 66%/3 rnd"; break;
								case 29: info +=  " 66%/4 rnd"; break;
								case 30: info +=  " 66%/5 rnd"; break;
								case 31: info +=  " 75%/2 rnd"; break;
								case 32: info +=  " 75%/3 rnd"; break;
								case 33: info +=  " 75%/4 rnd"; break;
								case 34: info +=  " 75%/5 rnd"; break;
								case 35: info += " 100%/1 rnd"; break;
								case 36: info += " 100%/2 rnd"; break;
								case 37: info += " 100%/3 rnd"; break;
								case 38: info += " 100%/4 rnd"; break;
								case 39: info += " 100%/5 rnd"; break;

								default: info += gs.Space + gs.bork; break;
							}
							break;

						case 18: // abilitydrain
							switch (GetPar(pars, 2))
							{
								//int IP_CONST_ABILITY_STR = 0;
								//int IP_CONST_ABILITY_DEX = 1;
								//int IP_CONST_ABILITY_CON = 2;
								//int IP_CONST_ABILITY_INT = 3;
								//int IP_CONST_ABILITY_WIS = 4;
								//int IP_CONST_ABILITY_CHA = 5;
								case 0: info += " str"; break;
								case 1: info += " dex"; break;
								case 2: info += " con"; break;
								case 3: info += " int"; break;
								case 4: info += " wis"; break;
								case 5: info += " cha"; break;

								default: info += gs.Space + gs.bork; break;
							}
							break;

						case 19: // poison
							switch (GetPar(pars, 2))
							{
								//int IP_CONST_POISON_1D2_STRDAMAGE = 0;
								//int IP_CONST_POISON_1D2_DEXDAMAGE = 1;
								//int IP_CONST_POISON_1D2_CONDAMAGE = 2;
								//int IP_CONST_POISON_1D2_INTDAMAGE = 3;
								//int IP_CONST_POISON_1D2_WISDAMAGE = 4;
								//int IP_CONST_POISON_1D2_CHADAMAGE = 5;
								case 0: info += " -d2 str"; break;
								case 1: info += " -d2 dex"; break;
								case 2: info += " -d2 con"; break;
								case 3: info += " -d2 int"; break;
								case 4: info += " -d2 wis"; break;
								case 5: info += " -d2 cha"; break;

								default: info += gs.Space + gs.bork; break;
							}
							break;

						case 20: // disease
							//int DISEASE_BLINDING_SICKNESS =  0; // these are stock but Disease.2da
							//int DISEASE_CACKLE_FEVER      =  1; // can be elaborated ...
							//int DISEASE_DEVIL_CHILLS      =  2;
							//int DISEASE_DEMON_FEVER       =  3;
							//int DISEASE_FILTH_FEVER       =  4;
							//int DISEASE_MINDFIRE          =  5;
							//int DISEASE_MUMMY_ROT         =  6;
							//int DISEASE_RED_ACHE          =  7;
							//int DISEASE_SHAKES            =  8;
							//int DISEASE_SLIMY_DOOM        =  9;
							//int DISEASE_RED_SLAAD_EGGS    = 10;
							//int DISEASE_GHOUL_ROT         = 11;
							//int DISEASE_ZOMBIE_CREEP      = 12;
							//int DISEASE_DREAD_BLISTERS    = 13;
							//int DISEASE_BURROW_MAGGOTS    = 14;
							//int DISEASE_SOLDIER_SHAKES    = 15;
							//int DISEASE_VERMIN_MADNESS    = 16;
							if ((par = GetPar(pars, 2)) != -1)
							{
								if (Info.diseaseLabels.Count != 0 && par < Info.diseaseLabels.Count)
								{
									info += gs.Space + Info.diseaseLabels[par];
								}
								else
									info += gs.Space + par;
							}
							else
								info += gs.Space + gs.bork;
							break;

						case 21: // slayrace
							if ((par = GetPar(pars, 2)) != -1)
							{
								if (Info.raceLabels.Count != 0 && par < Info.raceLabels.Count)
								{
									info += " vs " + Info.raceLabels[par];
								}
								else
									info += " vs " + par;
							}
							else
								info += gs.Space + gs.bork;
							break;

						case 22: // slayalignmentgroup
							switch (GetPar(pars, 2))
							{
								//int IP_CONST_ALIGNMENTGROUP_ALL     = 0;
								//int IP_CONST_ALIGNMENTGROUP_NEUTRAL = 1;
								//int IP_CONST_ALIGNMENTGROUP_LAWFUL  = 2;
								//int IP_CONST_ALIGNMENTGROUP_CHAOTIC = 3;
								//int IP_CONST_ALIGNMENTGROUP_GOOD    = 4;
								//int IP_CONST_ALIGNMENTGROUP_EVIL    = 5;
								case 0: info += " vs all";     break;
								case 1: info += " vs neutral"; break;
								case 2: info += " vs lawful";  break;
								case 3: info += " vs chaotic"; break;
								case 4: info += " vs good";    break;
								case 5: info += " vs evil";    break;

								default: info += gs.Space + gs.bork; break;
							}
							break;

						case 23: // slayalignment
							switch (GetPar(pars, 2))
							{
								//int IP_CONST_ALIGNMENT_LG = 0;
								//int IP_CONST_ALIGNMENT_LN = 1;
								//int IP_CONST_ALIGNMENT_LE = 2;
								//int IP_CONST_ALIGNMENT_NG = 3;
								//int IP_CONST_ALIGNMENT_TN = 4;
								//int IP_CONST_ALIGNMENT_NE = 5;
								//int IP_CONST_ALIGNMENT_CG = 6;
								//int IP_CONST_ALIGNMENT_CN = 7;
								//int IP_CONST_ALIGNMENT_CE = 8;
								case 0: info += " vs LG"; break;
								case 1: info += " vs LN"; break;
								case 2: info += " vs LE"; break;
								case 3: info += " vs NG"; break;
								case 4: info += " vs TN"; break;
								case 5: info += " vs NE"; break;
								case 6: info += " vs CG"; break;
								case 7: info += " vs CN"; break;
								case 8: info += " vs CE"; break;

								default: info += gs.Space + gs.bork; break;
							}
							break;
					}
					break;

				// Returns Item property reduced saving to base type. You must specify the base
				// type to which the penalty applies (ie. will, reflex, or fortitude) and the penalty
				// to be applied. The constant for the base type starts with (IP_CONST_SAVEBASETYPE_*).
				// The penalty must be a POSITIVE integer between 1 and 20 (ie. 1 = -1).
				// itemproperty ItemPropertyReducedSavingThrow(int nBonusType, int nPenalty);
				// Iprp_SaveElement.2da (probably does not use that)
				case 49: // ITEM_PROPERTY_DECREASED_SAVING_THROWS
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_SAVEBASETYPE_ALL       = 0;	// probably does not work IG - cf #40 SavingThrowBonus.
						//int IP_CONST_SAVEBASETYPE_FORTITUDE = 1;
						//int IP_CONST_SAVEBASETYPE_WILL      = 2;
						//int IP_CONST_SAVEBASETYPE_REFLEX    = 3;
//						case 0: info += "all";  break;				// probably does not work IG - cf #40 SavingThrowBonus.
						case 1: info += "fort"; break;
						case 2: info += "will"; break;
						case 3: info += "refl"; break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " -" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property reduced saving throw vs. an effect or damage type. You must
				// specify the constant to which the penalty applies(IP_CONST_SAVEVS_*) and the
				// penalty to be applied. The penalty must be a POSITIVE integer between 1 and 20
				// (ie. 1 = -1).
				// itemproperty ItemPropertyReducedSavingThrowVsX(int nBaseSaveType, int nPenalty);
				// Iprp_SavingThrow.2da (probably does not use that)
				case 50: // ITEM_PROPERTY_DECREASED_SAVING_THROWS_SPECIFIC
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_SAVEVS_UNIVERSAL     =  0;
						//int IP_CONST_SAVEVS_ACID          =  1;
						//int IP_CONST_SAVEVS_COLD          =  3;
						//int IP_CONST_SAVEVS_DEATH         =  4;
						//int IP_CONST_SAVEVS_DISEASE       =  5;
						//int IP_CONST_SAVEVS_DIVINE        =  6;
						//int IP_CONST_SAVEVS_ELECTRICAL    =  7;
						//int IP_CONST_SAVEVS_FEAR          =  8;
						//int IP_CONST_SAVEVS_FIRE          =  9;
						//int IP_CONST_SAVEVS_MINDAFFECTING = 11;
						//int IP_CONST_SAVEVS_NEGATIVE      = 12;
						//int IP_CONST_SAVEVS_POISON        = 13;
						//int IP_CONST_SAVEVS_POSITIVE      = 14;
						//int IP_CONST_SAVEVS_SONIC         = 15;
						case  0: info += "universal";     break;
						case  1: info += "acid";          break;
						case  3: info += "cold";          break;
						case  4: info += "death";         break;
						case  5: info += "disease";       break;
						case  6: info += "divine";        break;
						case  7: info += "electrical";    break;
						case  8: info += "fear";          break;
						case  9: info += "fire";          break;
						case 11: info += "mindaffecting"; break;
						case 12: info += "negative";      break;
						case 13: info += "poison";        break;
						case 14: info += "positive";      break;
						case 15: info += "sonic";         break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " -" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property regeneration. You must specify the regeneration amount.
				// The amount must be an integer between 1 and 20.
				// itemproperty ItemPropertyRegeneration(int nRegenAmount);
				case 51: // ITEM_PROPERTY_REGENERATION
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 21)
					{
						info += par.ToString(CultureInfo.InvariantCulture);
					}
					else
						info += gs.bork;
					break;

				// Returns Item property skill bonus. You must specify the skill to which the user
				// will get a bonus(SKILL_*) and the amount of the bonus. The bonus amount must
				// be an integer between 1 and 50.
				// itemproperty ItemPropertySkillBonus(int nSkill, int nBonus);
				// Skills.2da
				case 52: // ITEM_PROPERTY_SKILL_BONUS
//					info += "[2](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.skillLabels.Count != 0 && par < Info.skillLabels.Count)
						{
							info += Info.skillLabels[par];
						}
						else
							info += par.ToString(CultureInfo.InvariantCulture);
					}
					else
						info += gs.bork;

					if ((par = GetPar(pars, 1)) > 0 && par < 51)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property spell immunity vs. specific spell. You must specify the
				// spell to which the user will be immune(IP_CONST_IMMUNITYSPELL_*).
				// kL_NOTE: Don't be ridiculous. Use true Spell IDs.
				// itemproperty ItemPropertySpellImmunitySpecific(int nSpell);
				case 53: // ITEM_PROPERTY_IMMUNITY_SPECIFIC_SPELL
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.spellLabels.Count != 0 && par < Info.spellLabels.Count)
						{
							info += Info.spellLabels[par];
						}
						else
							info += par.ToString(CultureInfo.InvariantCulture);
					}
					else
						info += gs.bork;
					break;

				// Returns Item property spell immunity vs. spell school. You must specify the
				// school to which the user will be immune(IP_CONST_SPELLSCHOOL_*).
				// itemproperty ItemPropertySpellImmunitySchool(int nSchool);
				// Iprp_SpellShl.2da
				case 54: // ITEM_PROPERTY_IMMUNITY_SPELL_SCHOOL
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_SPELLSCHOOL_ABJURATION    = 0;
						//int IP_CONST_SPELLSCHOOL_CONJURATION   = 1;
						//int IP_CONST_SPELLSCHOOL_DIVINATION    = 2;
						//int IP_CONST_SPELLSCHOOL_ENCHANTMENT   = 3;
						//int IP_CONST_SPELLSCHOOL_EVOCATION     = 4;
						//int IP_CONST_SPELLSCHOOL_ILLUSION      = 5;
						//int IP_CONST_SPELLSCHOOL_NECROMANCY    = 6;
						//int IP_CONST_SPELLSCHOOL_TRANSMUTATION = 7;
						case 0: info += "abjuration";    break;
						case 1: info += "conjuration";   break;
						case 2: info += "divination";    break;
						case 3: info += "enchantment";   break;
						case 4: info += "evocation";     break;
						case 5: info += "illusion";      break;
						case 6: info += "necromancy";    break;
						case 7: info += "transmutation"; break;

						default: info += gs.bork; break;
					}
					break;

				// Returns Item property Thieves tools.  You must specify the modifier you wish
				// the tools to have.  The modifier must be an integer between 1 and 12.
				// itemproperty ItemPropertyThievesTools(int nModifier);
				case 55: // ITEM_PROPERTY_THIEVES_TOOLS
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 13)
					{
						info += "+" + par;
					}
					else
						info += gs.bork;
					break;

				// Returns Item property Attack bonus. You must specify an attack bonus. The bonus
				// must be an integer between 1 and 20.
				// itemproperty ItemPropertyAttackBonus(int nBonus);
				case 56: // ITEM_PROPERTY_ATTACK_BONUS
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 21)
					{
						info += "+" + par;
					}
					else
						info += gs.bork;
					break;

				// Returns Item property Attack bonus vs. alignment group. You must specify the
				// alignment group constant(IP_CONST_ALIGNMENTGROUP_*) and the attack bonus. The
				// bonus must be an integer between 1 and 20.
				// itemproperty ItemPropertyAttackBonusVsAlign(int nAlignGroup, int nBonus);
				// Iprp_AlignGrp.2da
				case 57: // ITEM_PROPERTY_ATTACK_BONUS_VS_ALIGNMENT_GROUP
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ALIGNMENTGROUP_ALL     = 0;
						//int IP_CONST_ALIGNMENTGROUP_NEUTRAL = 1;
						//int IP_CONST_ALIGNMENTGROUP_LAWFUL  = 2;
						//int IP_CONST_ALIGNMENTGROUP_CHAOTIC = 3;
						//int IP_CONST_ALIGNMENTGROUP_GOOD    = 4;
						//int IP_CONST_ALIGNMENTGROUP_EVIL    = 5;
						case 0: info += "vs all";     break;
						case 1: info += "vs neutral"; break;
						case 2: info += "vs lawful";  break;
						case 3: info += "vs chaotic"; break;
						case 4: info += "vs good";    break;
						case 5: info += "vs evil";    break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property attack bonus vs. racial group. You must specify the
				// racial group constant(IP_CONST_RACIALTYPE_*) and the attack bonus. The bonus
				// must be an integer between 1 and 20.
				// itemproperty ItemPropertyAttackBonusVsRace(int nRace, int nBonus);
				// RacialTypes.2da
				case 58: // ITEM_PROPERTY_ATTACK_BONUS_VS_RACIAL_GROUP
//					info += "[2](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.raceLabels.Count != 0 && par < Info.raceLabels.Count)
						{
							info += "vs " + Info.raceLabels[par];
						}
						else
							info += "vs " + par;
					}
					else
						info += gs.bork;

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property attack bonus vs. a specific alignment. You must specify
				// the alignment you want the bonus to work against(IP_CONST_ALIGNMENT_*) and the
				// attack bonus. The bonus must be an integer between 1 and 20.
				// itemproperty ItemPropertyAttackBonusVsSAlign(int nAlignment, int nBonus);
				// Iprp_Alignment.2da
				case 59: // ITEM_PROPERTY_ATTACK_BONUS_VS_SPECIFIC_ALIGNMENT
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ALIGNMENT_LG = 0;
						//int IP_CONST_ALIGNMENT_LN = 1;
						//int IP_CONST_ALIGNMENT_LE = 2;
						//int IP_CONST_ALIGNMENT_NG = 3;
						//int IP_CONST_ALIGNMENT_TN = 4;
						//int IP_CONST_ALIGNMENT_NE = 5;
						//int IP_CONST_ALIGNMENT_CG = 6;
						//int IP_CONST_ALIGNMENT_CN = 7;
						//int IP_CONST_ALIGNMENT_CE = 8;
						case 0: info += "vs LG"; break;
						case 1: info += "vs LN"; break;
						case 2: info += "vs LE"; break;
						case 3: info += "vs NG"; break;
						case 4: info += "vs TN"; break;
						case 5: info += "vs NE"; break;
						case 6: info += "vs CG"; break;
						case 7: info += "vs CN"; break;
						case 8: info += "vs CE"; break;

						default: info += gs.bork; break;
					}

					if ((par = GetPar(pars, 1)) > 0 && par < 21)
					{
						info += " +" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Returns Item property attack penalty. You must specify the attack penalty.
				// The penalty must be a POSITIVE integer between 1 and 5 (ie. 1 = -1).
				// itemproperty ItemPropertyAttackPenalty(int nPenalty);
				case 60: // ITEM_PROPERTY_DECREASED_ATTACK_MODIFIER
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 6)
					{
						info += "-" + par;
					}
					else
						info += gs.bork;
					break;

				// Returns Item property unlimited ammo. If you leave the parameter field blank
				// it will be just a normal bolt, arrow, or bullet. However you may specify that
				// you want the ammunition to do special damage (ie. +1d6 Fire, or +1 enhancement
				// bonus). For this parmeter you use the constants beginning with:
				// IP_CONST_UNLIMITEDAMMO_*.
				// itemproperty ItemPropertyUnlimitedAmmo(int nAmmoDamage=IP_CONST_UNLIMITEDAMMO_BASIC);
				// Iprp_AmmoType.2da (appears to be irrelevant - see Iprp_AmmoCost.2da the CostTable)
				case 61: // ITEM_PROPERTY_UNLIMITED_AMMUNITION
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.ipammoLabels.Count != 0 && par < Info.ipammoLabels.Count)
						{
							info += Info.ipammoLabels[par];
						}
						else
							info += par.ToString(CultureInfo.InvariantCulture);
					}
					else
						info += gs.bork;
					break;

				// Returns Item property limit use by alignment group. You must specify the
				// alignment group(s) that you want to be able to use this item(IP_CONST_ALIGNMENTGROUP_*).
				// itemproperty ItemPropertyLimitUseByAlign(int nAlignGroup);
				// Iprp_AlignGrp.2da
				case 62: // ITEM_PROPERTY_USE_LIMITATION_ALIGNMENT_GROUP
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ALIGNMENTGROUP_ALL     = 0;
						//int IP_CONST_ALIGNMENTGROUP_NEUTRAL = 1;
						//int IP_CONST_ALIGNMENTGROUP_LAWFUL  = 2;
						//int IP_CONST_ALIGNMENTGROUP_CHAOTIC = 3;
						//int IP_CONST_ALIGNMENTGROUP_GOOD    = 4;
						//int IP_CONST_ALIGNMENTGROUP_EVIL    = 5;
						case 0: info += "vs all";     break;
						case 1: info += "vs neutral"; break;
						case 2: info += "vs lawful";  break;
						case 3: info += "vs chaotic"; break;
						case 4: info += "vs good";    break;
						case 5: info += "vs evil";    break;

						default: info += gs.bork; break;
					}
					break;

				// Returns Item property limit use by class. You must specify the class(es) who
				// are able to use this item(IP_CONST_CLASS_*).
				// itemproperty ItemPropertyLimitUseByClass(int nClass);
				// Classes.2da
				case 63: // ITEM_PROPERTY_USE_LIMITATION_CLASS
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.classLabels.Count != 0 && par < Info.classLabels.Count)
						{
							info += Info.classLabels[par];
						}
						else
							info += par.ToString(CultureInfo.InvariantCulture);
					}
					else
						info += gs.bork;
					break;

				// Returns Item property limit use by race. You must specify the race(s) who are
				// allowed to use this item(IP_CONST_RACIALTYPE_*).
				// itemproperty ItemPropertyLimitUseByRace(int nRace);
				// RacialTypes.2da
				case 64: // ITEM_PROPERTY_USE_LIMITATION_RACIAL_TYPE
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.raceLabels.Count != 0 && par < Info.raceLabels.Count)
						{
							info += Info.raceLabels[par];
						}
						else
							info += par.ToString(CultureInfo.InvariantCulture);
					}
					else
						info += gs.bork;
					break;

				// Returns Item property limit use by specific alignment. You must specify the
				// alignment(s) of those allowed to use the item(IP_CONST_ALIGNMENT_*).
				// itemproperty ItemPropertyLimitUseBySAlign(int nAlignment);
				// Iprp_Alignment.2da
				case 65: // ITEM_PROPERTY_USE_LIMITATION_SPECIFIC_ALIGNMENT
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_ALIGNMENT_LG = 0;
						//int IP_CONST_ALIGNMENT_LN = 1;
						//int IP_CONST_ALIGNMENT_LE = 2;
						//int IP_CONST_ALIGNMENT_NG = 3;
						//int IP_CONST_ALIGNMENT_TN = 4;
						//int IP_CONST_ALIGNMENT_NE = 5;
						//int IP_CONST_ALIGNMENT_CG = 6;
						//int IP_CONST_ALIGNMENT_CN = 7;
						//int IP_CONST_ALIGNMENT_CE = 8;
						case 0: info += "vs LG"; break;
						case 1: info += "vs LN"; break;
						case 2: info += "vs LE"; break;
						case 3: info += "vs NG"; break;
						case 4: info += "vs TN"; break;
						case 5: info += "vs NE"; break;
						case 6: info += "vs CG"; break;
						case 7: info += "vs CN"; break;
						case 8: info += "vs CE"; break;

						default: info += gs.bork; break;
					}
					break;

				// Creates a "bonus hitpoints" itemproperty. Note that nBonusType refers
				// to the row in iprp_bonushp.2da which has the bonus HP value, and
				// is not necessarily the amount of HPs added.
				// itemproperty ItemPropertyBonusHitpoints(int nBonusType);
				case 66: // ITEM_PROPERTY_BONUS_HITPOINTS
//					info += "[1](";
					info += "(";
					info += GetPar(pars, 0) + " raw";	// TODO: pull labels in from Iprp_BonusHP.2da - although see the
					break;								// TargetInfo GUI-script because I think I slap HP onto chars 1:1

				// Returns Item property vampiric regeneration. You must specify the amount of
				// regeneration. The regen amount must be an integer between 1 and 20.
				// itemproperty ItemPropertyVampiricRegeneration(int nRegenAmount);
				case 67: // ITEM_PROPERTY_REGENERATION_VAMPIRIC
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 21)
					{
						info += "+" + par;
					}
					else
						info += gs.bork;
					break;

//				case 68: // no case 68 in nwscript: "Vorpal" in ItemPropDef.2da
//				case 69: // no case 69 in nwscript: "Wounding" in ItemPropDef.2da

				// Returns Item property Trap. You must specify the trap level constant
				// (IP_CONST_TRAPSTRENGTH_*) and the trap type constant(IP_CONST_TRAPTYPE_*).
				// itemproperty ItemPropertyTrap(int nTrapLevel, int nTrapType);
				// Iprp_Traps.2da
				case 70: // ITEM_PROPERTY_TRAP
//					info += "[2](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_TRAPSTRENGTH_MINOR   = 0;
						//int IP_CONST_TRAPSTRENGTH_AVERAGE = 1;
						//int IP_CONST_TRAPSTRENGTH_STRONG  = 2;
						//int IP_CONST_TRAPSTRENGTH_DEADLY  = 3;
						case 0: info += "minor";   break;
						case 1: info += "average"; break;
						case 2: info += "strong";  break;
						case 3: info += "deadly";  break;
						case 4: info += "epic";    break; // NOTE: no IP_CONST_TRAPSTRENGTH_* defined in nwscript.
//						case 5: fatal

						default: info += gs.bork; break;
					}

					switch (GetPar(pars, 1))
					{
						//int IP_CONST_TRAPTYPE_SPIKE       =  1;
						//int IP_CONST_TRAPTYPE_HOLY        =  2;
						//int IP_CONST_TRAPTYPE_TANGLE      =  3;
						//int IP_CONST_TRAPTYPE_BLOBOFACID  =  4;
						//int IP_CONST_TRAPTYPE_FIRE        =  5;
						//int IP_CONST_TRAPTYPE_ELECTRICAL  =  6;
						//int IP_CONST_TRAPTYPE_GAS         =  7;
						//int IP_CONST_TRAPTYPE_FROST       =  8;
						//int IP_CONST_TRAPTYPE_ACID_SPLASH =  9;
						//int IP_CONST_TRAPTYPE_SONIC       = 10;
						//int IP_CONST_TRAPTYPE_NEGATIVE    = 11;
						case  1: info += " spike";      break;
						case  2: info += " holy";       break;
						case  3: info += " tangle";     break;
						case  4: info += " blobofacid"; break;
						case  5: info += " fire";       break;
						case  6: info += " electrical"; break;
						case  7: info += " gas";        break;
						case  8: info += " frost";      break;
						case  9: info += " acidsplash"; break;
						case 10: info += " sonic";      break;
						case 11: info += " negative";   break;

						default: info += gs.Space + gs.bork; break;
					}
					break;

				// Returns Item property true seeing.
				// itemproperty ItemPropertyTrueSeeing();
				case 71: // ITEM_PROPERTY_TRUE_SEEING
//					info += "[0](";
					info += "(";
					break;

				// Returns Item property Monster on hit apply effect property. You must specify
				// the property that you want applied on hit. There are some properties that
				// require an additional special parameter to be specified. The others that
				// don't require any additional parameter you may just put in the one. The
				// special cases are as follows:
				// ABILITYDRAIN nSpecial is the ability to drain.
				//              IP_CONST_ABILITY_*
				// DISEASE      nSpecial is the disease that you want applied.
				//              DISEASE_*
				// LEVELDRAIN   nSpecial is the number of levels that you want drained.
				//              integer between 1 and 5.
				// POISON       nSpecial is the type of poison that will effect the victim.
				//              IP_CONST_POISON_*
				// WOUNDING     nSpecial is the amount of wounding.
				//              integer between 1 and 5.
				// NOTE: Any that do not appear in the above list do not require the second
				// parameter.
				// NOTE: These can only be applied to monster NATURAL weapons (ie. bite, claw,
				// gore, and slam). IT WILL NOT WORK ON NORMAL WEAPONS.
				// itemproperty ItemPropertyOnMonsterHitProperties(int nProperty, int nSpecial=0);
				// Iprp_MonsterHit.2da
//				case 72: // ITEM_PROPERTY_ON_MONSTER_HIT
					// kL_NOTE: I don't think we'll be enchanting any creature-items ....
					// But if so see case 48: // ITEM_PROPERTY_ON_HIT_PROPERTIES
//					break;

				// Returns Item property turn resistance. You must specify the resistance bonus.
				// The bonus must be an integer between 1 and 50.
				// itemproperty ItemPropertyTurnResistance(int nModifier);
				case 73: // ITEM_PROPERTY_TURN_RESISTANCE
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 51)
					{
						info += "+" + par;
					}
					else
						info += gs.bork;
					break;

				// Returns Item property Massive Critical. You must specify the extra damage
				// constant(IP_CONST_DAMAGEBONUS_*) of the criticals.
				// itemproperty ItemPropertyMassiveCritical(int nDamage);
				case 74: // ITEM_PROPERTY_MASSIVE_CRITICALS
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_DAMAGEBONUS_1    =  1;
						//int IP_CONST_DAMAGEBONUS_2    =  2;
						//int IP_CONST_DAMAGEBONUS_3    =  3;
						//int IP_CONST_DAMAGEBONUS_4    =  4;
						//int IP_CONST_DAMAGEBONUS_5    =  5;
						//int IP_CONST_DAMAGEBONUS_1d4  =  6;
						//int IP_CONST_DAMAGEBONUS_1d6  =  7;
						//int IP_CONST_DAMAGEBONUS_1d8  =  8;
						//int IP_CONST_DAMAGEBONUS_1d10 =  9;
						//int IP_CONST_DAMAGEBONUS_2d6  = 10;
						//int IP_CONST_DAMAGEBONUS_2d8  = 11;
						//int IP_CONST_DAMAGEBONUS_2d4  = 12;
						//int IP_CONST_DAMAGEBONUS_2d10 = 13;
						//int IP_CONST_DAMAGEBONUS_1d12 = 14;
						//int IP_CONST_DAMAGEBONUS_2d12 = 15;
						//int IP_CONST_DAMAGEBONUS_6    = 16;
						//int IP_CONST_DAMAGEBONUS_7    = 17;
						//int IP_CONST_DAMAGEBONUS_8    = 18;
						//int IP_CONST_DAMAGEBONUS_9    = 19;
						//int IP_CONST_DAMAGEBONUS_10   = 20;
						//int IP_CONST_DAMAGEBONUS_3d10 = 51;
						//int IP_CONST_DAMAGEBONUS_3d12 = 52;
						//int IP_CONST_DAMAGEBONUS_4d6  = 53;
						//int IP_CONST_DAMAGEBONUS_4d8  = 54;
						//int IP_CONST_DAMAGEBONUS_4d10 = 55;
						//int IP_CONST_DAMAGEBONUS_4d12 = 56;
						//int IP_CONST_DAMAGEBONUS_5d6  = 57;
						//int IP_CONST_DAMAGEBONUS_5d12 = 58;
						//int IP_CONST_DAMAGEBONUS_6d12 = 59;
						//int IP_CONST_DAMAGEBONUS_3d6  = 60;
						//int IP_CONST_DAMAGEBONUS_6d6  = 61;
						case  1: info += "+1";    break;
						case  2: info += "+2";    break;
						case  3: info += "+3";    break;
						case  4: info += "+4";    break;
						case  5: info += "+5";    break;
						case  6: info += "+1d4";  break;
						case  7: info += "+1d6";  break;
						case  8: info += "+1d8";  break;
						case  9: info += "+1d10"; break;
						case 10: info += "+2d6";  break;
						case 11: info += "+2d8";  break;
						case 12: info += "+2d4";  break;
						case 13: info += "+2d10"; break;
						case 14: info += "+1d12"; break;
						case 15: info += "+2d12"; break;
						case 16: info += "+6";    break;
						case 17: info += "+7";    break;
						case 18: info += "+8";    break;
						case 19: info += "+9";    break;
						case 20: info += "+10";   break;
						case 51: info += "+3d10"; break;
						case 52: info += "+3d12"; break;
						case 53: info += "+4d6";  break;
						case 54: info += "+4d8";  break;
						case 55: info += "+4d10"; break;
						case 56: info += "+4d12"; break;
						case 57: info += "+5d6";  break;
						case 58: info += "+5d12"; break;
						case 59: info += "+6d12"; break;
						case 60: info += "+3d6";  break;
						case 61: info += "+6d6";  break;

						default: info += gs.bork; break;
					}
					break;

				// Returns Item property free action.
				// itemproperty ItemPropertyFreeAction();
				case 75: // ITEM_PROPERTY_FREEDOM_OF_MOVEMENT
//					info += "[0](";
					info += "(";
					break;

				// Poison.2da
				case 76: // ITEM_PROPERTY_POISON
					info += "(obsolete - use IP 48: ITEM_PROPERTY_ON_HIT_PROPERTIES";
					break;

				// Returns Item property monster damage. You must specify the amount of damage
				// the monster's attack will do(IP_CONST_MONSTERDAMAGE_*).
				// NOTE: These can only be applied to monster NATURAL weapons (ie. bite, claw,
				// gore, and slam). IT WILL NOT WORK ON NORMAL WEAPONS.
				// itemproperty ItemPropertyMonsterDamage(int nDamage);
//				case 77: // ITEM_PROPERTY_MONSTER_DAMAGE
					// kL_NOTE: I don't think we'll be enchanting any creature-items ....
//					break;

				// Returns Item property immunity to spell level. You must specify the level of
				// which that and below the user will be immune. The level must be an integer
				// between 1 and 9. By putting in a 3 it will mean the user is immune to all
				// 3rd level and lower spells.
				// itemproperty ItemPropertyImmunityToSpellLevel(int nLevel);
				case 78: // ITEM_PROPERTY_IMMUNITY_SPELLS_BY_LEVEL
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 10)
					{
						info += "L" + par;
					}
					else
						info += gs.bork;
					break;

				// Returns Item property special walk. If no parameters are specified it will
				// automatically use the zombie walk. This will apply the special walk animation
				// to the user.
				// itemproperty ItemPropertySpecialWalk(int nWalkType=0);
				// Iprp_Walk.2da
				case 79: // ITEM_PROPERTY_SPECIAL_WALK
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						case 0:
						case 1: info += "zombie"; break;

						default: info += gs.bork; break;
					}
					break;

				// Returns Item property healers kit. You must specify the level of the kit.
				// The modifier must be an integer between 1 and 12.
				// itemproperty ItemPropertyHealersKit(int nModifier);
				case 80: // ITEM_PROPERTY_HEALERS_KIT
//					info += "[1](";
					info += "(";
					if ((par = GetPar(pars, 0)) > 0 && par < 13)
					{
						info += "+" + par;
					}
					else
						info += gs.bork;
					break;

				// Returns Item property weight increase. You must specify the weight increase
				// constant(IP_CONST_WEIGHTINCREASE_*).
				// itemproperty ItemPropertyWeightIncrease(int nWeight);
				case 81: // ITEM_PROPERTY_WEIGHT_INCREASE
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int IP_CONST_WEIGHTINCREASE_5_LBS   = 0;
						//int IP_CONST_WEIGHTINCREASE_10_LBS  = 1;
						//int IP_CONST_WEIGHTINCREASE_15_LBS  = 2;
						//int IP_CONST_WEIGHTINCREASE_30_LBS  = 3;
						//int IP_CONST_WEIGHTINCREASE_50_LBS  = 4;
						//int IP_CONST_WEIGHTINCREASE_100_LBS = 5;
						case 0: info +=   "+5 lb"; break;
						case 1: info +=  "+10 lb"; break;
						case 2: info +=  "+15 lb"; break;
						case 3: info +=  "+30 lb"; break;
						case 4: info +=  "+50 lb"; break;
						case 5: info += "+100 lb"; break;

						default: info += gs.bork; break;
					}
					break;

				// Creates an item property that (when applied to a weapon item) causes a spell to be cast
				// when a successful strike is made, or (when applied to armor) is struck by an opponent.
				// - nSpell uses the IP_CONST_ONHIT_CASTSPELL_* constants
				// itemproperty ItemPropertyOnHitCastSpell(int nSpell, int nLevel);
				// Iprp_OnHitSpell.2da
				case 82: // ITEM_PROPERTY_ONHITCASTSPELL
//					info += "[2](";
					info += "(";
					if ((par = GetPar(pars, 0)) != -1)
					{
						if (Info.iphitspellLabels.Count != 0 && par < Info.iphitspellLabels.Count)
						{
							info += Info.iphitspellLabels[par];
						}
						else
							info += par;
					}
					else
						info += gs.bork;

					if ((par = GetPar(pars, 1)) > 0)
					{
						info += " L" + par;
					}
					else
						info += gs.Space + gs.bork;
					break;

				// Creates a visual effect (ITEM_VISUAL_*) that may be applied to
				// melee weapons only.
				// itemproperty ItemPropertyVisualEffect(int nEffect);
				// Iprp_VisualFx.2da
				case 83: // ITEM_PROPERTY_VISUALEFFECT
//					info += "[1](";
					info += "(";
					switch (GetPar(pars, 0))
					{
						//int ITEM_VISUAL_ACID       = 0;
						//int ITEM_VISUAL_COLD       = 1;
						//int ITEM_VISUAL_ELECTRICAL = 2;
						//int ITEM_VISUAL_FIRE       = 3;
						//int ITEM_VISUAL_SONIC      = 4;
						//int ITEM_VISUAL_HOLY       = 5;
						//int ITEM_VISUAL_EVIL       = 6;
						case 0: info += "acid";       break;
						case 1: info += "cold";       break;
						case 2: info += "electrical"; break;
						case 3: info += "fire";       break;
						case 4: info += "sonic";      break;
						case 5: info += "holy";       break;
						case 6: info += "evil";       break;

						default: info += gs.bork; break;
					}
					break;

				// Creates an item property that offsets the effect on arcane spell failure
				// that a particular item has. Parameters come from the ITEM_PROP_ASF_* group.
				// kL_NOTE: There is no "ITEM_PROP_ASF_* group" - perhaps see Iprp_ArcSpell.2da ...
				// itemproperty ItemPropertyArcaneSpellFailure(int nModLevel);
				case 84: // ITEM_PROPERTY_ARCANE_SPELL_FAILURE
//					info += "[1](";
					info += "(";
					info += GetPar(pars, 0) + " raw"; // TODO: investigate
					break;

//				case 85: // no case 85 in nwscript: "ArrowCatching"
//				case 86: // no case 86 in nwscript: "Bashing"
//				case 87: // no case 87 in nwscript: "Animated"
//				case 88: // no case 88 in nwscript: "Wild"
//				case 89: // no case 89 in nwscript: "Etherealness"

				// JLR-OEI 04/03/06: This version is REPLACING the old DEPRECATED one.
				// Returns Item property damage reduction. You must specify:
				// - nAmount: amount of damage reduction
				// - nDmgBonus: (dependent on the nDRType)
				//   - DR_TYPE_NONE:       ()
				//   - DR_TYPE_DMGTYPE:    DAMAGE_TYPE_*
				//   - DR_TYPE_MAGICBONUS: (DAMAGE_POWER_*)
				//   - DR_TYPE_EPIC:       ()
				//   - DR_TYPE_GMATERIAL:  GMATERIAL_*
				//   - DR_TYPE_ALIGNMENT:  ALIGNMENT_*
				//   - DR_TYPE_NON_RANGED: ()
				// - nLimit: How much damage the effect can absorb before disappearing.
				//   Set to zero for infinite
				// - nDRType: DR_TYPE_*
				// itemproperty ItemPropertyDamageReduction(int nAmount, int nDRSubType, int nLimit=0, int nDRType=DR_TYPE_MAGICBONUS);
				// Iprp_DamageReduction.2da
				case 22: // ITEM_PROPERTY_DAMAGE_REDUCTION_DEPRECATED
				case 90: // ITEM_PROPERTY_DAMAGE_REDUCTION
//					info += "[4](ip DamageReduction cannot be scripted.";
					info += "(ip DamageReduction cannot be scripted";
					break;
			}

			if (info.Length != 0) info += ")";
			return info;
		}

		/// <summary>
		/// Gets a par-value as an int out of a comma-delimited string of pars.
		/// </summary>
		/// <param name="pars">comma-delimited string of pars</param>
		/// <param name="pos">position of the par-value to retrieve</param>
		/// <returns>the par-value as an int; -1 if it didn't parse</returns>
		static int GetPar(string pars, int pos)
		{
			string[] ips = pars.Split(',');

			if (pos < ips.Length)
			{
				int result;
				if (Int32.TryParse(ips[pos], out result))
					return result;
			}
			return -1;
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
				case InfoInputSpells.School: // (SpellSchools.2da) - no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars)
					{
						info = Table.Cols[col].text + ": ";

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

							default:  info += gs.bork;         break;
						}
					}
					break;

				case InfoInputSpells.Range: // (Ranges.2da) - no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars)
					{
						info = Table.Cols[col].text + ": ";

						int r;
						switch (val.ToUpperInvariant())
						{
							case "P": info += "Personal"; r =  0; break; // NOTE: 'rangeLabels' could be used but
							case "T": info += "Touch";    r =  1; break; // they're abnormal: "SpellRngPers" eg.
							case "S": info += "Short";    r =  2; break;
							case "M": info += "Medium";   r =  3; break;
							case "L": info += "Long";     r =  4; break;
							case "I": info += "Infinite"; r = 14; break;

							default:  info += gs.bork;    r = -1; break;
						}

						if (r != -1 && r < Info.rangeRanges.Count) //&& it_PathRanges2da.Checked <- redundant
						{
							info += gs.Space + Info.rangeRanges[r] + "m";
						}
					}
					break;

				case InfoInputSpells.Vs: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars)
					{
						info = Table.Cols[col].text + ": ";

						switch (val.ToUpperInvariant())
						{
							case "V":  info += "verbal";          break;
							case "S":  info += "somatic";         break;
							case "VS": info += "verbal, somatic"; break;

							default:   info += gs.bork;           break;
						}
					}
					break;

				case InfoInputSpells.MetaMagic: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars)
					{
						info = Table.Cols[col].text + ": ";

						if (val.Length > 2
							&& Int32.TryParse(val.Substring(2),
											  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
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
					}
					break;

				case InfoInputSpells.TargetType: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars)
					{
						info = Table.Cols[col].text + ": ";

						if (val.Length > 2
							&& Int32.TryParse(val.Substring(2),
											  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
											  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
											  out result))
						{
							switch (result)
							{
								case TARGET_NONE:
									info += "none";
									break;

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
					}
					break;

				case InfoInputSpells.SubRadSpell1: // Spells.2da ->
				case InfoInputSpells.SubRadSpell2:
				case InfoInputSpells.SubRadSpell3:
				case InfoInputSpells.SubRadSpell4:
				case InfoInputSpells.SubRadSpell5:
				case InfoInputSpells.Master:
				case InfoInputSpells.Counter1:
				case InfoInputSpells.Counter2:
					if (it_PathSpells2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars)
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.spellLabels.Count)
							{
								info += Info.spellLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputSpells.Category: // Categories.2da
					if (it_PathCategories2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars)
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.categoryLabels.Count)
							{
								info += Info.categoryLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputSpells.UserType: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars)
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result))
						{
							switch (result)
							{
								case 1:  info += "Spell";           break;
								case 2:  info += "Special Ability"; break;
								case 3:  info += "Feat";            break;
								case 4:  info += "Item Power";      break;

								default: info += gs.bork;           break;
							}
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputSpells.SpontCastClassReq: // Classes.2da
					if (it_PathClasses2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars)
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.classLabels.Count)
							{
								// NOTE: The stock Spells.2da uses "0" for n/a here.
								// The field ought be "****" (or "-1") instead ofc.
								info += Info.classLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputSpells.FeatID: // Feat.2da
					if (it_PathFeat2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars)
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

							if (feat < Info.featLabels.Count)
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
					}
					break;

				case InfoInputSpells.AsMetaMagic: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars)
					{
						info = Table.Cols[col].text + ": ";

						if (val.Length > 2
							&& Int32.TryParse(val.Substring(2),
											  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
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

								default:                      info += gs.bork;            break;
							}
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputSpells.TargetingUI: // SpellTarget.2da
					if (it_PathSpellTarget2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars)
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.targetLabels.Count)
							{
								info += Info.targetLabels[result];

								bool b = false;

								float f = Info.targetWidths[result];
								if (Math.Abs(0.0F - f) > 0.00001F)
								{
									b = true;
									info += " (" + f;
								}

								f = Info.targetLengths[result];
								if (Math.Abs(0.0F - f) > 0.00001F)
								{
									if (!b)
									{
										b = true;
										info += " (_";
									}
									info += " x " + f;
								}

								if (b) info += ")";
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputSpells.ItemImmunity: // no 2da (bools) ->
				case InfoInputSpells.UseConcentration:
				case InfoInputSpells.SpontaneouslyCast:
				case InfoInputSpells.HostileSetting:
				case InfoInputSpells.HasProjectile:
				case InfoInputSpells.CastableOnDead:
				case InfoInputSpells.Removed:
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars)
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result))
						{
							switch (result)
							{
								case 0:  info += "false"; break;
								case 1:  info += "true";  break;

								default: info += gs.bork; break;
							}
						}
						else
							info += gs.bork;
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
				case 20: // "PREREQFEAT1" - Feat.2da
				case 21: // "PREREQFEAT2"
				case 28: // "SUCCESSOR"
				case 34: // "OrReqFeat0"
				case 35: // "OrReqFeat1"
				case 36: // "OrReqFeat2"
				case 37: // "OrReqFeat3"
				case 38: // "OrReqFeat4"
				case 39: // "OrReqFeat5"
					if (it_PathFeat2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars) // NOTE: "****" is 0 which is actually ""
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.featLabels.Count)
							{
								info += Info.featLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputFeat.Category: // Categories.2da
					if (it_PathCategories2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars) // NOTE: "****" is 0 which is actually "****" /hah
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.categoryLabels.Count)
							{
								info += Info.categoryLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case 27: // "SPELLID" - Spells.2da
					if (it_PathSpells2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars) // NOTE: "****" is 0 which is actually ""
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.spellLabels.Count)
							{
								info += Info.spellLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputFeat.MasterFeat: // MasterFeats.2da
					if (it_PathMasterFeats2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars) // NOTE: "****" is 0 which is actually "ImprovedCritical"
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.masterfeatLabels.Count)
							{
								info += Info.masterfeatLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case 40: // "REQSKILL" - Skills.2da
				case 43: // "REQSKILL2"
					if (it_PathSkills2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars) // NOTE: "****" is 0 which is actually ""
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.skillLabels.Count)
							{
								info += Info.skillLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputFeat.ToolsCategories: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars) // NOTE: "****" is 0 which is actually "All Feats"
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result)
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
					}
					break;

				case InfoInputFeat.ToggleMode: // CombatModes.2da
					if (it_PathCombatModes2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars) // NOTE: "****" is 0 which is actually ""
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.combatmodeLabels.Count)
							{
								info += Info.combatmodeLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
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
				case InfoInputClasses.PrimaryAbil: // no 2da ->
				case InfoInputClasses.SpellAbil:
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars)
					{
						info = Table.Cols[col].text + ": ";

						switch (val.ToUpperInvariant())
						{
							case "STR": info += "Strength";     break;
							case "CON": info += "Constitution"; break;
							case "DEX": info += "Dexterity";    break;
							case "INT": info += "Intelligence"; break;
							case "WIS": info += "Wisdom";       break;
							case "CHA": info += "Charisma";     break;

							default: info += gs.bork;           break;
						}
					}
					break;

				case InfoInputClasses.AlignRestrict: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars)
					{
						info = Table.Cols[col].text + ": ";

						if (val.Length > 2
							&& Int32.TryParse(val.Substring(2),
											  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
											  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
											  out result))
						{
							switch (result)
							{
								case ALIGNRESTRICT_NONE:
									info += "none";
									break;

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
																  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
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
					}
					break;

				case InfoInputClasses.AlignRstrctType: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars)
					{
						info = Table.Cols[col].text + ": ";

						if (val.Length > 2
							&& Int32.TryParse(val.Substring(2),
											  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
											  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
											  out result))
						{
							switch (result)
							{
								case ALIGNRESTRICTTYPE_NONE:
									info += "none";
									break;

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
					}
					break;

				case InfoInputClasses.Package: // Packages.2da
					if (it_PathPackages2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars) // NOTE: "****" is 0 which is actually ""
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.packageLabels.Count)
							{
								info += Info.packageLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case 75: // "FEATPracticedSpellcaster" - Feat.2da
				case 76: // "FEATExtraSlot"
				case 77: // "FEATArmoredCaster"
					if (it_PathFeat2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars) // NOTE: "****" is 0 which is actually ""
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.featLabels.Count)
							{
								info += Info.featLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
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
		string getItemInfo(int id, int col)
		{
			string info = gs.non;

			string val;
			int result;

			switch (col)
			{
				case InfoInputBaseitems.EquipableSlots: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars) // NOTE: "****" is 0 which is actually "not equipable"
					{
						info = Table.Cols[col].text + ": ";

						if (val.Length > 2
							&& Int32.TryParse(val.Substring(2),
											  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
											  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
											  out result))
						{
							switch (result)
							{
								case EQUIPSLOT_NONE:
									info += "not equipable";
									break;

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
					}
					break;

				case InfoInputBaseitems.ModelType: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars) // NOTE: "****" is 0 which is actually "simple 1-part"
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result)
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
					}
					break;

				case InfoInputBaseitems.WeaponWield: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars) // NOTE: "****" is 0 which is actually "standard one-handed weapon"
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result)
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
					}
					break;

				case InfoInputBaseitems.WeaponType: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars) // NOTE: "****" is 0 which is actually "none"
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result)
							&& result > -1 && result < 6)
						{
							switch (result)
							{
								case  0: info += "none";                 break;
								case  1: info += "piercing";             break;
								case  2: info += "bludgeoning";          break;
								case  3: info += "slashing";             break;
								case  4: info += "piercing/slashing";    break;
								case  5: info += "bludgeoning/piercing"; break;
							}
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputBaseitems.WeaponSize: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars) // NOTE: "****" is 0 which is actually "none"
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result)
							&& result > -1 && result < 5)
						{
							switch (result)
							{
								case  0: info += "none";   break;
								case  1: info += "tiny";   break;
								case  2: info += "small";  break;
								case  3: info += "medium"; break;
								case  4: info += "large";  break;
							}
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputBaseitems.RangedWeapon: // BaseItems.2da
					if (it_PathBaseItems2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars)
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.tagLabels.Count)
							{
								info += Info.tagLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputBaseitems.InvSoundType: // InventorySnds.2da
					if (it_PathInventorySnds2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars)
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.soundLabels.Count)
							{
								info += Info.soundLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputBaseitems.PropColumn: // ItemProps.2da
					if (it_PathItemProps2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars)
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.propFields.Count)
							{
								info += Info.propFields[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputBaseitems.StorePanel: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars) // NOTE: "****" is 0 which is actually "armor and clothing"
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result)
							&& result > -1 && result < 5)
						{
							switch (result)
							{
								case  0: info += "armor and clothing";    break;
								case  1: info += "weapons";               break;
								case  2: info += "potions and scrolls";   break;
								case  3: info += "wands and magic items"; break;
								case  4: info += "miscellaneous";         break;
							}
						}
						else
							info += gs.bork;
					}
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
					if (it_PathFeat2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars) // NOTE: "****" is 0 which is actually ""
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.featLabels.Count)
							{
								info += Info.featLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputBaseitems.AC_Enchant: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars) // NOTE: "****" is 0 which is actually "dodge"
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result)
							&& result > -1 && result < 5)
						{
							switch (result)
							{
								case  0: info += "dodge";      break;
								case  1: info += "natural";    break;
								case  2: info += "armor";      break;
								case  3: info += "shield";     break;
								case  4: info += "deflection"; break;
							}
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputBaseitems.BaseAC:
				case InfoInputBaseitems.ArmorCheckPen:
				case InfoInputBaseitems.ArcaneSpellFailure:
					info = Table.Cols[col].text + ": for shields only";
					break;

				case InfoInputBaseitems.WeaponMatType: // WeaponSounds.2da
					if (it_PathWeaponSounds2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars)
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result)
							&& result > -1)
						{
							if (result < Info.weapsoundLabels.Count)
							{
								info += Info.weapsoundLabels[result];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputBaseitems.AmmunitionType: // AmmunitionTypes.2da
					if (it_PathAmmunitionTypes2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val == gs.Stars)
						{
							info += gs.non;
						}
						else if (Int32.TryParse(val, out result) // off by 1 ->
							&& result > 0)
						{
							if (result - 1 < Info.ammoLabels.Count)
							{
								info += Info.ammoLabels[result - 1];
							}
							else
								info += val;
						}
						else
							info += gs.bork;
					}
					break;

				case InfoInputBaseitems.QBBehaviour: // no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars) // NOTE: "****" is 0 which is actually "none"
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result)
							&& result > -1 && result < 3)
						{
							switch (result)
							{
								case  0: info += "none";                                  break;
								case  1: info += "rods instruments wands and misc items"; break;
								case  2: info += "potions and scrolls";                   break;
							}
						}
						else
							info += gs.bork;
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
