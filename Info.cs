using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	static class Info
	{
		#region Crafting caches
		/// <summary>
		/// A list that holds labels for spells in Spells.2da.
		/// - optional
		/// </summary>
		internal static List<string> spellLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for feats in Feat.2da.
		/// - optional
		/// </summary>
		internal static List<string> featsLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for itemproperties in ItemPropDef.2da.
		/// - optional
		/// </summary>
		internal static List<string> ipLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for baseitem-types in BaseItems.2da.
		/// - optional
		/// </summary>
		internal static List<string> tagLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for skills in Skills.2da.
		/// - optional
		/// </summary>
		internal static List<string> skillLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for races in Races.2da.
		/// - optional
		/// </summary>
		internal static List<string> raceLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for classes in Classes.2da.
		/// - optional
		/// </summary>
		internal static List<string> classLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for ip-spells in Iprp_Spells.2da.
		/// - optional
		/// </summary>
		internal static List<string> ipspellsLabels = new List<string>();

		/// <summary>
		/// A list that holds casterlevel for ip-spells in Iprp_Spells.2da.
		/// - optional
		/// </summary>
		internal static List<int> ipspellsLevels = new List<int>();

		/// <summary>
		/// A list that holds labels for diseases in Disease.2da.
		/// - optional
		/// </summary>
		internal static List<string> diseaseLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for onhitspells in Iprp_OnHitSpell.2da.
		/// - optional
		/// </summary>
		internal static List<string> iphitspellLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for feats in Iprp_Feats.2da.
		/// - optional
		/// </summary>
		internal static List<string> ipfeatsLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for ammo in Iprp_AmmoCost.2da.
		/// - optional
		/// </summary>
		internal static List<string> ipammoLabels = new List<string>();
		#endregion Crafting caches


		#region Spells caches
		// NOTE: Also uses Spells.2da for master and child spell-labels.
		// NOTE: Also uses Classes.2da for spontaneous cast class-labels.
		// NOTE: Also uses Feat.2da for feat-id feat-labels.

		/// <summary>
		/// A list that holds labels for spell-ranges in Ranges.2da.
		/// - optional
		/// </summary>
		internal static List<string> rangeLabels = new List<string>();

		/// <summary>
		/// A list that holds ranges for spell-ranges in Ranges.2da.
		/// - optional
		/// </summary>
		internal static List<int> rangeRanges = new List<int>();

		/// <summary>
		/// A list that holds labels for categories in Categories.2da.
		/// - optional
		/// </summary>
		internal static List<string> categoryLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for spell-targets in SpellTarget.2da.
		/// - optional
		/// @note Groping SpellTarget.2da does NOT use the regular grope-
		/// routines. It has 2 fields that are float-values (instead of only 1
		/// optional int-value). So GropeSpellTarget() will be used instead of
		/// the regular GropeLabels() and it needs to be called by the general
		/// YataForm.GropeLabels() function as well as the path-item.
		/// </summary>
		internal static List<string> targetLabels  = new List<string>();
		internal static List<float>  targetWidths  = new List<float>();
		internal static List<float>  targetLengths = new List<float>();
		#endregion Spells caches


		/// <summary>
		/// Gets the label-strings from a given 2da.
		/// </summary>
		/// <param name="pfe2da"></param>
		/// <param name="labels"></param>
		/// <param name="it"></param>
		/// <param name="col">col in the 2da of the label</param>
		/// <param name="col1">col in the 2da of an int (default -1)</param>
		/// <param name="ints">a collection MUST be passed in if col2 is not -1</param>
		/// TODO: Check that the given 2da really has the required cols.
		internal static void GropeLabels(string pfe2da,
										 ICollection<string> labels,
										 ToolStripMenuItem it,
										 int col,
										 int col1 = -1,
										 ICollection<int> ints = null)
		{
			if (File.Exists(pfe2da))
			{
				string[] rows = File.ReadAllLines(pfe2da);

				// WARNING: This does *not* handle quotation marks around 2da fields.
				foreach (string row in rows) // test for double-quote character and exit if found.
				{
					foreach (char character in row)
					{
						if (character == '"')
						{
							const string info = "The 2da-file contains double-quotes. Although that can be"
											  + " valid in a 2da-file this function is not coded to cope."
											  + " Format the 2da-file to not use double-quotes if you want"
											  + " to access it here.";
							MessageBox.Show(info,
											"burp",
											MessageBoxButtons.OK,
											MessageBoxIcon.Error,
											MessageBoxDefaultButton.Button1);
							return;
						}
					}
				}


				labels.Clear();
				if (ints != null) ints.Clear();

				string line;
				string[] cols;

				for (int row = 0; row != rows.Length; ++row)
				{
					if (!String.IsNullOrEmpty(line = rows[row].Trim()))
					{
						cols = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

						if (cols.Length > col && cols.Length > col1)
						{
							int id;
							if (Int32.TryParse(cols[0], out id)) // is a valid 2da row
							{
								labels.Add(cols[col]); // and hope for the best.

								if (col1 != -1)
								{
									int result;
									if (!Int32.TryParse(cols[col1], out result))
									{
										result = -1; // always add an int to keep sync w/ the labels
									}
									ints.Add(result);
								}
							}
						}
					}
				}

				it.Checked = (labels.Count != 0);
			}
		}

		/// <summary>
		/// Gets the label-strings plus width/height values from SpellTarget.2da.
		/// </summary>
		/// <param name="pfe2da">path_file_extension to the 2da</param>
		/// <param name="labels">the cache in which to store the labels</param>
		/// <param name="it">the path-item on which to toggle Checked</param>
		/// <param name="col">col in the 2da of the label</param>
		/// <param name="col1">col in the 2da of a float (default -1)</param>
		/// <param name="col2">col in the 2da of a float (default -1)</param>
		/// <param name="floats1">a collection MUST be passed in if col1 is not -1</param>
		/// <param name="floats2">a collection MUST be passed in if col2 is not -1</param>
		/// TODO: Check that the given 2da really has the required cols.
		internal static void GropeSpellTarget(string pfe2da,
											  ICollection<string> labels,
											  ToolStripMenuItem it,
											  int col,
											  int col1 = -1,
											  int col2 = -1,
											  ICollection<float> floats1 = null,
											  ICollection<float> floats2 = null)
		{
			if (File.Exists(pfe2da))
			{
				string[] lines = File.ReadAllLines(pfe2da);

				// WARNING: This does *not* handle quotation marks around 2da fields.
				foreach (string line in lines) // test for double-quote character and exit if found.
				{
					foreach (char c in line)
					{
						if (c == '"')
						{
							const string info = "The 2da-file contains double-quotes. Although that can be"
											  + " valid in a 2da-file this function is not coded to cope."
											  + " Format the 2da-file to not use double-quotes if you want"
											  + " to access it here.";
							MessageBox.Show(info,
											"burp",
											MessageBoxButtons.OK,
											MessageBoxIcon.Error,
											MessageBoxDefaultButton.Button1);
							return;
						}
					}
				}


				labels.Clear();
				if (floats1 != null) floats1.Clear();
				if (floats2 != null) floats2.Clear();

				string line0;
				string[] fields;

				for (int i = 0; i != lines.Length; ++i)
				{
					if (!String.IsNullOrEmpty(line0 = lines[i].Trim()))
					{
						fields = line0.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

						if (fields.Length > col && fields.Length > col1 && fields.Length > col2)
						{
							int id;
							if (Int32.TryParse(fields[0], out id)) // is a valid 2da row
							{
								labels.Add(fields[col]); // and hope for the best.

								float result;

								if (col1 != -1)
								{
									if (!float.TryParse(fields[col1], out result))
									{
										result = 0.0F; // always add a float to keep sync w/ the labels
									}
									floats1.Add(result);
								}

								if (col2 != -1)
								{
									if (!float.TryParse(fields[col2], out result))
									{
										result = 0.0F; // always add a float to keep sync w/ the labels
									}
									floats2.Add(result);
								}
							}
						}
					}
				}

				it.Checked = (labels.Count != 0);
			}
		}

		/// <summary>
		/// Gets the TCC-type as a string for a given (int)tag.
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		internal static string GetTccType(int tag)
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
			return "bork";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encodedIp"></param>
		/// <returns></returns>
		internal static string GetEncodedParsDescription(string encodedIp)
		{
			string info = String.Empty;

			string ip = String.Empty;

			int pos = encodedIp.IndexOf(',');
			if (pos != -1)
			{
				ip = encodedIp.Substring(0, pos);

				int result;
				if (Int32.TryParse(ip, out result))
				{
					int par;

					string pars = encodedIp.Substring(pos + 1);

					switch (result)
					{
						// Returns Item property ability bonus. You need to specify an
						// ability constant(IP_CONST_ABILITY_*) and the bonus. The bonus should
						// be a positive integer between 1 and 12.
						// itemproperty ItemPropertyAbilityBonus(int nAbility, int nBonus);
						// Iprp_Abilities.2da
						case 0: // ITEM_PROPERTY_ABILITY_BONUS
							info += "[2](";
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

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 13)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property AC bonus. You need to specify the bonus.
						// The bonus should be a positive integer between 1 and 20. The modifier
						// type depends on the item it is being applied to.
						// itemproperty ItemPropertyACBonus(int nBonus);
						case 1: // ITEM_PROPERTY_AC_BONUS
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 21)
							{
								info += "+" + par;
							}
							else
								info += "bork";
							break;

						// Returns Item property AC bonus vs. alignment group. An example of
						// an alignment group is Chaotic, or Good. You need to specify the
						// alignment group constant(IP_CONST_ALIGNMENTGROUP_*) and the AC bonus.
						// The AC bonus should be an integer between 1 and 20. The modifier
						// type depends on the item it is being applied to.
						// itemproperty ItemPropertyACBonusVsAlign(int nAlignGroup, int nACBonus);
						// Iprp_AlignGrp.2da
						case 2: // ITEM_PROPERTY_AC_BONUS_VS_ALIGNMENT_GROUP
							info += "[2](";
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

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property AC bonus vs. Damage type (ie. piercing). You
						// need to specify the damage type constant(IP_CONST_DAMAGETYPE_*) and the
						// AC bonus. The AC bonus should be an integer between 1 and 20. The
						// modifier type depends on the item it is being applied to.
						// NOTE: Only the first 3 damage types may be used here, the 3 basic physical types.
						// itemproperty ItemPropertyACBonusVsDmgType(int nDamageType, int nACBonus);
						// Iprp_CombatDam.2da
						case 3: // ITEM_PROPERTY_AC_BONUS_VS_DAMAGE_TYPE
							info += "[2](";
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

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property AC bonus vs. Racial group. You need to specify
						// the racial group constant(IP_CONST_RACIALTYPE_*) and the AC bonus. The AC
						// bonus should be an integer between 1 and 20. The modifier type depends
						// on the item it is being applied to.
						// itemproperty ItemPropertyACBonusVsRace(int nRace, int nACBonus);
						// RacialTypes.2da
						case 4: // ITEM_PROPERTY_AC_BONUS_VS_RACIAL_GROUP
							info += "[2](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (raceLabels.Count != 0
									&& par < raceLabels.Count)
								{
									info += "vs " + raceLabels[par];
								}
								else
									info += "vs " + par;
							}
							else
								info += "bork";

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property AC bonus vs. Specific alignment. You need to
						// specify the specific alignment constant(IP_CONST_ALIGNMENT_*) and the AC
						// bonus. The AC bonus should be an integer between 1 and 20. The
						// modifier type depends on the item it is being applied to.
						// itemproperty ItemPropertyACBonusVsSAlign(int nAlign, int nACBonus);
						// Iprp_Alignment.2da
						case 5: // ITEM_PROPERTY_AC_BONUS_VS_SPECIFIC_ALIGNMENT
							info += "[2](";
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

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property Enhancement bonus. You need to specify the
						// enhancement bonus. The Enhancement bonus should be an integer between
						// 1 and 20.
						// itemproperty ItemPropertyEnhancementBonus(int nEnhancementBonus);
						case 6: // ITEM_PROPERTY_ENHANCEMENT_BONUS
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 21)
							{
								info += "+" + par;
							}
							else
								info += "bork";
							break;

						// Returns Item property Enhancement bonus vs. an Alignment group. You
						// need to specify the alignment group constant(IP_CONST_ALIGNMENTGROUP_*)
						// and the enhancement bonus. The Enhancement bonus should be an integer
						// between 1 and 20.
						// itemproperty ItemPropertyEnhancementBonusVsAlign(int nAlignGroup, int nBonus);
						// Iprp_AlignGrp.2da
						case 7: // ITEM_PROPERTY_ENHANCEMENT_BONUS_VS_ALIGNMENT_GROUP
							info += "[2](";
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

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property Enhancement bonus vs. Racial group. You need
						// to specify the racial group constant(IP_CONST_RACIALTYPE_*) and the
						// enhancement bonus. The enhancement bonus should be an integer between
						// 1 and 20.
						// itemproperty ItemPropertyEnhancementBonusVsRace(int nRace, int nBonus);
						// RacialTypes.2da
						case 8: // ITEM_PROPERTY_ENHANCEMENT_BONUS_VS_RACIAL_GROUP
							info += "[2](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (raceLabels.Count != 0
									&& par < raceLabels.Count)
								{
									info += "vs " + raceLabels[par];
								}
								else
									info += "vs " + par;
							}
							else
								info += "bork";

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property Enhancement bonus vs. a specific alignment. You
						// need to specify the alignment constant(IP_CONST_ALIGNMENT_*) and the
						// enhancement bonus. The enhancement bonus should be an integer between
						// 1 and 20.
						// itemproperty ItemPropertyEnhancementBonusVsSAlign(int nAlign, int nBonus);
						// Iprp_Alignment.2da
						case 9: // ITEM_PROPERTY_ENHANCEMENT_BONUS_VS_SPECIFIC_ALIGNEMENT
							info += "[2](";
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

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property Enhancment penalty. You need to specify the
						// enhancement penalty. The enhancement penalty should be a POSITIVE
						// integer between 1 and 5 (ie. 1 = -1).
						// itemproperty ItemPropertyEnhancementPenalty(int nPenalty);
						case 10: // ITEM_PROPERTY_DECREASED_ENHANCEMENT_MODIFIER
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 6)
							{
								info += "-" + par;
							}
							else
								info += "bork";
							break;

						// Returns Item property weight reduction. You need to specify the weight
						// reduction constant(IP_CONST_REDUCEDWEIGHT_*).
						// itemproperty ItemPropertyWeightReduction(int nReduction);
						case 11: // ITEM_PROPERTY_BASE_ITEM_WEIGHT_REDUCTION
							info += "[1](";
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

								default: info += "bork"; break;
							}
							break;

						// Returns Item property Bonus Feat. You need to specify the the feat
						// constant(IP_CONST_FEAT_*).
						// itemproperty ItemPropertyBonusFeat(int nFeat);
						// Iprp_Feats.2da
						case 12: // ITEM_PROPERTY_BONUS_FEAT
							info += "[1](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (ipfeatsLabels.Count != 0
									&& par < ipfeatsLabels.Count)
								{
									info += ipfeatsLabels[par];
								}
								else
									info += par.ToString();
							}
							else
								info += "bork";
							break;

						// Returns Item property Bonus level spell (Bonus spell of level). You must
						// specify the class constant(IP_CONST_CLASS_*) of the bonus spell(MUST BE a
						// spell casting class) and the level of the bonus spell. The level of the
						// bonus spell should be an integer between 0 and 9.
						// itemproperty ItemPropertyBonusLevelSpell(int nClass, int nSpellLevel);
						// Classes.2da
						case 13: // ITEM_PROPERTY_BONUS_SPELL_SLOT_OF_LEVEL_N
							info += "[2](";
							if ((par = GetPar(pars, 0)) != -1) // TODO: Check if 'nClass' has "SpellCaster" enabled in Classes.2da
							{
								if (classLabels.Count != 0
									&& par < classLabels.Count)
								{
									info += classLabels[par];
								}
								else
									info += par.ToString();
							}
							else
								info += "bork";

							if ((par = GetPar(pars, 1)) > -1 && par < 10)
							{
								info += " L" + par;
							}
							else
								info += " bork";
							break;

//						case 14: // no case 14 in nwscript: "Boomerang" in ItemPropDef.2da

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
							info += "[2](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (ipspellsLabels.Count != 0
									&& par < ipspellsLabels.Count)
								{
									info += ipspellsLabels[par] + " L" + ipspellsLevels[par];
								}
								else
									info += par.ToString();
							}
							else
								info += "bork";

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

								default: info += " bork"; break;
							}
							break;

						// Returns Item property damage bonus. You must specify the damage type constant
						// (IP_CONST_DAMAGETYPE_*) and the amount of damage constant(IP_CONST_DAMAGEBONUS_*).
						// NOTE: not all the damage types will work, use only the following: Acid, Bludgeoning,
						// Cold, Electrical, Fire, Piercing, Slashing, Sonic.
						// itemproperty ItemPropertyDamageBonus(int nDamageType, int nDamage);
						// Iprp_DamageType.2da
						case 16: // ITEM_PROPERTY_DAMAGE_BONUS
							info += "[2](";
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

								default: info += "bork"; break;
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

								default: info += " bork"; break;
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
							info += "[3](";
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

								default: info += "bork"; break;
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

								default: info += " bork"; break;
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

								default: info += " bork"; break;
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
							info += "[3](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (raceLabels.Count != 0
									&& par < raceLabels.Count)
								{
									info += "vs " + raceLabels[par];
								}
								else
									info += "vs " + par;
							}
							else
								info += "bork";

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

								default: info += " bork"; break;
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

								default: info += " bork"; break;
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
							info += "[3](";
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

								default: info += "bork"; break;
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

								default: info += " bork"; break;
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

								default: info += " bork"; break;
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
							info += "[2](";
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

								default: info += "bork"; break;
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

								default: info += " bork"; break;
							}
							break;

						// Returns Item property damage penalty. You must specify the damage penalty.
						// The damage penalty should be a POSITIVE integer between 1 and 5 (ie. 1 = -1).
						// itemproperty ItemPropertyDamagePenalty(int nPenalty);
						case 21: // ITEM_PROPERTY_DECREASED_DAMAGE
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 6)
							{
								info += "-" + par;
							}
							else
								info += "bork";
							break;

//						case 22: // ITEM_PROPERTY_DAMAGE_REDUCTION_DEPRECATED "DamageReduced" in ItemPropDef.2da / Iprp_Protection.2da

						// Returns Item property damage resistance. You must specify the damage type
						// constant(IP_CONST_DAMAGETYPE_*) and the amount of HP of damage constant
						// (IP_CONST_DAMAGERESIST_*) that will be resisted against each round.
						// itemproperty ItemPropertyDamageResistance(int nDamageType, int nHPResist);
						// Iprp_DamageType.2da
						case 23: // ITEM_PROPERTY_DAMAGE_RESISTANCE
							info += "[2](";
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
//								case  3: info += "subdual (don't use this)";  break;
//								case  4: info += "physical (don't use this)"; break;
								case  5: info += "magical";    break;
								case  6: info += "acid";       break;
								case  7: info += "cold";       break;
								case  8: info += "divine";     break;
								case  9: info += "electrical"; break;
								case 10: info += "fire";       break;
								case 11: info += "negative";   break;
								case 12: info += "positive";   break;
								case 13: info += "sonic";      break;

								default: info += "bork"; break;
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

								default: info += " bork"; break;
							}
							break;

						// Returns Item property damage vulnerability. You must specify the damage type
						// constant(IP_CONST_DAMAGETYPE_*) that you want the user to be extra vulnerable to
						// and the percentage vulnerability constant(IP_CONST_DAMAGEVULNERABILITY_*).
						// itemproperty ItemPropertyDamageVulnerability(int nDamageType, int nVulnerability);
						// Iprp_DamageType.2da
						case 24: // ITEM_PROPERTY_DAMAGE_VULNERABILITY
							info += "[2](";
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
//								case  3: info += "subdual (don't use this)";  break;
//								case  4: info += "physical (don't use this)"; break;
								case  5: info += "magical";    break;
								case  6: info += "acid";       break;
								case  7: info += "cold";       break;
								case  8: info += "divine";     break;
								case  9: info += "electrical"; break;
								case 10: info += "fire";       break;
								case 11: info += "negative";   break;
								case 12: info += "positive";   break;
								case 13: info += "sonic";      break;

								default: info += "bork"; break;
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

								default: info += " bork"; break;
							}
							break;

//						case 25: // no case 25 in nwscript: "Dancing_Scimitar" in ItemPropDef.2da

						// Return Item property Darkvision.
						// itemproperty ItemPropertyDarkvision();
						case 26: // ITEM_PROPERTY_DARKVISION
							info += "[0](";
							break;

						// Return Item property decrease ability score. You must specify the ability
						// constant(IP_CONST_ABILITY_*) and the modifier constant. The modifier must be
						// a POSITIVE integer between 1 and 10 (ie. 1 = -1).
						// itemproperty ItemPropertyDecreaseAbility(int nAbility, int nModifier);
						// Iprp_Ablities.2da
						case 27: // ITEM_PROPERTY_DECREASED_ABILITY_SCORE
							info += "[2](";
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

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 11)
							{
								info += " -" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property decrease Armor Class. You must specify the armor
						// modifier type constant(IP_CONST_ACMODIFIERTYPE_*) and the armor class penalty.
						// The penalty must be a POSITIVE integer between 1 and 5 (ie. 1 = -1).
						// itemproperty ItemPropertyDecreaseAC(int nModifierType, int nPenalty);
						// Iprp_AcModType.2da
						case 28: // ITEM_PROPERTY_DECREASED_AC
							info += "[2](";
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

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 6)
							{
								info += " -" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property decrease skill. You must specify the constant for the
						// skill to be decreased(SKILL_*) and the amount of the penalty. The penalty
						// must be a POSITIVE integer between 1 and 10 (ie. 1 = -1).
						// itemproperty ItemPropertyDecreaseSkill(int nSkill, int nPenalty);
						// Skills.2da
						case 29: // ITEM_PROPERTY_DECREASED_SKILL_MODIFIER
							info += "[2](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (skillLabels.Count != 0
									&& par < skillLabels.Count)
								{
									info += skillLabels[par];
								}
								else
									info += par.ToString();
							}
							else
								info += "bork";

							if ((par = GetPar(pars, 1)) > 0 && par < 11)
							{
								info += " -" + par;
							}
							else
								info += " bork";
							break;

//						case 30: // no case 30 in nwscript: "DoubleStack" in ItemPropDef.2da
//						case 31: // no case 31 in nwscript: "EnhancedContainer_BonusSlot" in ItemPropDef.2da

						// Returns Item property container reduced weight. This is used for special
						// containers that reduce the weight of the objects inside them. You must
						// specify the container weight reduction type constant(IP_CONST_CONTAINERWEIGHTRED_*).
						// itemproperty ItemPropertyContainerReducedWeight(int nContainerType);
						case 32: // ITEM_PROPERTY_ENHANCED_CONTAINER_REDUCED_WEIGHT
							info += "[1](";
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

								default: info += "bork"; break;
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
							info += "[1](";
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

								default: info += "bork"; break;
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
							info += "[1](";
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

								default: info += "bork"; break;
							}
							break;

						// Returns Item property haste.
						// itemproperty ItemPropertyHaste();
						case 35: // ITEM_PROPERTY_HASTE
							info += "[0](";
							break;

						// Returns Item property Holy Avenger.
						// itemproperty ItemPropertyHolyAvenger();
						case 36: // ITEM_PROPERTY_HOLY_AVENGER
							info += "[0](";
							break;

						// Returns Item property immunity to miscellaneous effects. You must specify the
						// effect to which the user is immune, it is a constant(IP_CONST_IMMUNITYMISC_*).
						// itemproperty ItemPropertyImmunityMisc(int nImmunityType);
						// Iprp_Immunity.2da
						case 37: // ITEM_PROPERTY_IMMUNITY_MISCELLANEOUS
							info += "[1](";
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

								default: info += "bork"; break;
							}
							break;

						// Returns Item property improved evasion.
						// itemproperty ItemPropertyImprovedEvasion();
						case 38: // ITEM_PROPERTY_IMPROVED_EVASION
							info += "[0](";
							break;

						// Returns Item property bonus spell resistance.  You must specify the bonus spell
						// resistance constant(IP_CONST_SPELLRESISTANCEBONUS_*).
						// kL_NOTE: This is not a bonus.
						// itemproperty ItemPropertyBonusSpellResistance(int nBonus);
						case 39: // ITEM_PROPERTY_SPELL_RESISTANCE
							info += "[1](";
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

								default: info += "bork"; break;
							}
							break;

						// Returns Item property saving throw bonus to the base type (ie. will, reflex,
						// fortitude). You must specify the base type constant(IP_CONST_SAVEBASETYPE_*)
						// to which the user gets the bonus and the bonus that he/she will get. The
						// bonus must be an integer between 1 and 20.
						// itemproperty ItemPropertyBonusSavingThrow(int nBaseSaveType, int nBonus);
						// Iprp_SaveElement.2da (does not use that)
						case 40: // ITEM_PROPERTY_SAVING_THROW_BONUS
							info += "[2](";
							switch (GetPar(pars, 0))
							{
								//int IP_CONST_SAVEBASETYPE_ALL       = 0;	// does not work IG.
								//int IP_CONST_SAVEBASETYPE_FORTITUDE = 1;
								//int IP_CONST_SAVEBASETYPE_WILL      = 2;
								//int IP_CONST_SAVEBASETYPE_REFLEX    = 3;
//								case 0: info += "all";  break;				// does not work IG.
								case 1: info += "fort"; break;
								case 2: info += "will"; break;
								case 3: info += "refl"; break;

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property saving throw bonus vs. a specific effect or damage type.
						// You must specify the save type constant(IP_CONST_SAVEVS_*) that the bonus is
						// applied to and the bonus that is be applied. The bonus must be an integer
						// between 1 and 20.
						// itemproperty ItemPropertyBonusSavingThrowVsX(int nBonusType, int nBonus);
						// Iprp_SavingThrow.2da (does not use that)
						case 41: // ITEM_PROPERTY_SAVING_THROW_BONUS_SPECIFIC
							info += "[2](";
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

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

//						case 42: // no case 42 in nwscript

						// Returns Item property keen. This means a critical threat range of 19-20 on a
						// weapon will be increased to 17-20 etc.
						// itemproperty ItemPropertyKeen();
						case 43: // ITEM_PROPERTY_KEEN
							info += "[0](";
							break;

						// Returns Item property light. You must specify the intesity constant of the
						// light(IP_CONST_LIGHTBRIGHTNESS_*) and the color constant of the light
						// (IP_CONST_LIGHTCOLOR_*).
						// itemproperty ItemPropertyLight(int nBrightness, int nColor);
						case 44: // ITEM_PROPERTY_LIGHT
							info += "[2](";
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

								default: info += "bork"; break;
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

								default: info += " bork"; break;
							}
							break;

						// Returns Item property Max range strength modification (ie. mighty). You must
						// specify the maximum modifier for strength that is allowed on a ranged weapon.
						// The modifier must be a positive integer between 1 and 20.
						// itemproperty ItemPropertyMaxRangeStrengthMod(int nModifier);
						case 45: // ITEM_PROPERTY_MIGHTY
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 21)
							{
								info += "+" + par;
							}
							else
								info += "bork";
							break;

//						case 46: // ITEM_PROPERTY_MIND_BLANK - no function in nwscript

						// Returns Item property no damage. This means the weapon will do no damage in
						// combat.
						// itemproperty ItemPropertyNoDamage();
						case 47: // ITEM_PROPERTY_NO_DAMAGE
							info += "[0](";
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
							info += "[2/3](";
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

								default: info += "bork"; break;
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

								default: info += " bork"; break;
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

										default: info += " bork"; break;
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

										default: info += " bork"; break;
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

										default: info += " bork"; break;
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
										if (diseaseLabels.Count != 0
											&& par < diseaseLabels.Count)
										{
											info += " " + diseaseLabels[par];
										}
										else
											info += " " + par;
									}
									else
										info += " bork";
									break;

								case 21: // slayrace
									if ((par = GetPar(pars, 2)) != -1)
									{
										if (raceLabels.Count != 0
											&& par < raceLabels.Count)
										{
											info += " vs " + raceLabels[par];
										}
										else
											info += " vs " + par;
									}
									else
										info += " bork";
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

										default: info += " bork"; break;
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

										default: info += " bork"; break;
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
							info += "[2](";
							switch (GetPar(pars, 0))
							{
								//int IP_CONST_SAVEBASETYPE_ALL       = 0;	// probably does not work IG - cf #40 SavingThrowBonus.
								//int IP_CONST_SAVEBASETYPE_FORTITUDE = 1;
								//int IP_CONST_SAVEBASETYPE_WILL      = 2;
								//int IP_CONST_SAVEBASETYPE_REFLEX    = 3;
//								case 0: info += "all";  break;				// probably does not work IG - cf #40 SavingThrowBonus.
								case 1: info += "fort"; break;
								case 2: info += "will"; break;
								case 3: info += "refl"; break;

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " -" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property reduced saving throw vs. an effect or damage type. You must
						// specify the constant to which the penalty applies(IP_CONST_SAVEVS_*) and the
						// penalty to be applied. The penalty must be a POSITIVE integer between 1 and 20
						// (ie. 1 = -1).
						// itemproperty ItemPropertyReducedSavingThrowVsX(int nBaseSaveType, int nPenalty);
						// Iprp_SavingThrow.2da (probably does not use that)
						case 50: // ITEM_PROPERTY_DECREASED_SAVING_THROWS_SPECIFIC
							info += "[2](";
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

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " -" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property regeneration. You must specify the regeneration amount.
						// The amount must be an integer between 1 and 20.
						// itemproperty ItemPropertyRegeneration(int nRegenAmount);
						case 51: // ITEM_PROPERTY_REGENERATION
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 21)
							{
								info += par.ToString();
							}
							else
								info += "bork";
							break;

						// Returns Item property skill bonus. You must specify the skill to which the user
						// will get a bonus(SKILL_*) and the amount of the bonus. The bonus amount must
						// be an integer between 1 and 50.
						// itemproperty ItemPropertySkillBonus(int nSkill, int nBonus);
						// Skills.2da
						case 52: // ITEM_PROPERTY_SKILL_BONUS
							info += "[2](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (skillLabels.Count != 0
									&& par < skillLabels.Count)
								{
									info += skillLabels[par];
								}
								else
									info += par.ToString();
							}
							else
								info += "bork";

							if ((par = GetPar(pars, 1)) > 0 && par < 51)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property spell immunity vs. specific spell. You must specify the
						// spell to which the user will be immune(IP_CONST_IMMUNITYSPELL_*).
						// kL_NOTE: Don't be ridiculous. Use true Spell IDs.
						// itemproperty ItemPropertySpellImmunitySpecific(int nSpell);
						case 53: // ITEM_PROPERTY_IMMUNITY_SPECIFIC_SPELL
							info += "[1](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (spellLabels.Count != 0
									&& par < spellLabels.Count)
								{
									info += spellLabels[par];
								}
								else
									info += par.ToString();
							}
							else
								info += "bork";
							break;

						// Returns Item property spell immunity vs. spell school. You must specify the
						// school to which the user will be immune(IP_CONST_SPELLSCHOOL_*).
						// itemproperty ItemPropertySpellImmunitySchool(int nSchool);
						// Iprp_SpellShl.2da
						case 54: // ITEM_PROPERTY_IMMUNITY_SPELL_SCHOOL
							info += "[1](";
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

								default: info += "bork"; break;
							}
							break;

						// Returns Item property Thieves tools.  You must specify the modifier you wish
						// the tools to have.  The modifier must be an integer between 1 and 12.
						// itemproperty ItemPropertyThievesTools(int nModifier);
						case 55: // ITEM_PROPERTY_THIEVES_TOOLS
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 13)
							{
								info += "+" + par;
							}
							else
								info += "bork";
							break;

						// Returns Item property Attack bonus. You must specify an attack bonus. The bonus
						// must be an integer between 1 and 20.
						// itemproperty ItemPropertyAttackBonus(int nBonus);
						case 56: // ITEM_PROPERTY_ATTACK_BONUS
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 21)
							{
								info += "+" + par;
							}
							else
								info += "bork";
							break;

						// Returns Item property Attack bonus vs. alignment group. You must specify the
						// alignment group constant(IP_CONST_ALIGNMENTGROUP_*) and the attack bonus. The
						// bonus must be an integer between 1 and 20.
						// itemproperty ItemPropertyAttackBonusVsAlign(int nAlignGroup, int nBonus);
						// Iprp_AlignGrp.2da
						case 57: // ITEM_PROPERTY_ATTACK_BONUS_VS_ALIGNMENT_GROUP
							info += "[2](";
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

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property attack bonus vs. racial group. You must specify the
						// racial group constant(IP_CONST_RACIALTYPE_*) and the attack bonus. The bonus
						// must be an integer between 1 and 20.
						// itemproperty ItemPropertyAttackBonusVsRace(int nRace, int nBonus);
						// RacialTypes.2da
						case 58: // ITEM_PROPERTY_ATTACK_BONUS_VS_RACIAL_GROUP
							info += "[2](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (raceLabels.Count != 0
									&& par < raceLabels.Count)
								{
									info += "vs " + raceLabels[par];
								}
								else
									info += "vs " + par;
							}
							else
								info += "bork";

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property attack bonus vs. a specific alignment. You must specify
						// the alignment you want the bonus to work against(IP_CONST_ALIGNMENT_*) and the
						// attack bonus. The bonus must be an integer between 1 and 20.
						// itemproperty ItemPropertyAttackBonusVsSAlign(int nAlignment, int nBonus);
						// Iprp_Alignment.2da
						case 59: // ITEM_PROPERTY_ATTACK_BONUS_VS_SPECIFIC_ALIGNMENT
							info += "[2](";
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

								default: info += "bork"; break;
							}

							if ((par = GetPar(pars, 1)) > 0 && par < 21)
							{
								info += " +" + par;
							}
							else
								info += " bork";
							break;

						// Returns Item property attack penalty. You must specify the attack penalty.
						// The penalty must be a POSITIVE integer between 1 and 5 (ie. 1 = -1).
						// itemproperty ItemPropertyAttackPenalty(int nPenalty);
						case 60: // ITEM_PROPERTY_DECREASED_ATTACK_MODIFIER
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 6)
							{
								info += "-" + par;
							}
							else
								info += "bork";
							break;

						// Returns Item property unlimited ammo. If you leave the parameter field blank
						// it will be just a normal bolt, arrow, or bullet. However you may specify that
						// you want the ammunition to do special damage (ie. +1d6 Fire, or +1 enhancement
						// bonus). For this parmeter you use the constants beginning with:
						// IP_CONST_UNLIMITEDAMMO_*.
						// itemproperty ItemPropertyUnlimitedAmmo(int nAmmoDamage=IP_CONST_UNLIMITEDAMMO_BASIC);
						// Iprp_AmmoType.2da (appears to be irrelevant - see Iprp_AmmoCost.2da the CostTable)
						case 61: // ITEM_PROPERTY_UNLIMITED_AMMUNITION
							info += "[1](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (ipammoLabels.Count != 0
									&& par < ipammoLabels.Count)
								{
									info += ipammoLabels[par];
								}
								else
									info += par.ToString();
							}
							else
								info += "bork";
							break;

						// Returns Item property limit use by alignment group. You must specify the
						// alignment group(s) that you want to be able to use this item(IP_CONST_ALIGNMENTGROUP_*).
						// itemproperty ItemPropertyLimitUseByAlign(int nAlignGroup);
						// Iprp_AlignGrp.2da
						case 62: // ITEM_PROPERTY_USE_LIMITATION_ALIGNMENT_GROUP
							info += "[1](";
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

								default: info += "bork"; break;
							}
							break;

						// Returns Item property limit use by class. You must specify the class(es) who
						// are able to use this item(IP_CONST_CLASS_*).
						// itemproperty ItemPropertyLimitUseByClass(int nClass);
						// Classes.2da
						case 63: // ITEM_PROPERTY_USE_LIMITATION_CLASS
							info += "[1](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (classLabels.Count != 0
									&& par < classLabels.Count)
								{
									info += classLabels[par];
								}
								else
									info += par.ToString();
							}
							else
								info += "bork";
							break;

						// Returns Item property limit use by race. You must specify the race(s) who are
						// allowed to use this item(IP_CONST_RACIALTYPE_*).
						// itemproperty ItemPropertyLimitUseByRace(int nRace);
						// RacialTypes.2da
						case 64: // ITEM_PROPERTY_USE_LIMITATION_RACIAL_TYPE
							info += "[1](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (raceLabels.Count != 0
									&& par < raceLabels.Count)
								{
									info += raceLabels[par];
								}
								else
									info += par.ToString();
							}
							else
								info += "bork";
							break;

						// Returns Item property limit use by specific alignment. You must specify the
						// alignment(s) of those allowed to use the item(IP_CONST_ALIGNMENT_*).
						// itemproperty ItemPropertyLimitUseBySAlign(int nAlignment);
						// Iprp_Alignment.2da
						case 65: // ITEM_PROPERTY_USE_LIMITATION_SPECIFIC_ALIGNMENT
							info += "[1](";
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

								default: info += "bork"; break;
							}
							break;

						// Creates a "bonus hitpoints" itemproperty. Note that nBonusType refers
						// to the row in iprp_bonushp.2da which has the bonus HP value, and
						// is not necessarily the amount of HPs added.
						// itemproperty ItemPropertyBonusHitpoints(int nBonusType);
						case 66: // ITEM_PROPERTY_BONUS_HITPOINTS
							info += "[1](";
							info += GetPar(pars, 0) + " raw";	// TODO: pull labels in from Iprp_BonusHP.2da - although see the
							break;								// TargetInfo GUI-script because I think I slap HP onto chars 1:1

						// Returns Item property vampiric regeneration. You must specify the amount of
						// regeneration. The regen amount must be an integer between 1 and 20.
						// itemproperty ItemPropertyVampiricRegeneration(int nRegenAmount);
						case 67: // ITEM_PROPERTY_REGENERATION_VAMPIRIC
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 21)
							{
								info += "+" + par;
							}
							else
								info += "bork";
							break;

//						case 68: // no case 68 in nwscript: "Vorpal" in ItemPropDef.2da
//						case 69: // no case 69 in nwscript: "Wounding" in ItemPropDef.2da

						// Returns Item property Trap. You must specify the trap level constant
						// (IP_CONST_TRAPSTRENGTH_*) and the trap type constant(IP_CONST_TRAPTYPE_*).
						// itemproperty ItemPropertyTrap(int nTrapLevel, int nTrapType);
						// Iprp_Traps.2da
						case 70: // ITEM_PROPERTY_TRAP
							info += "[2](";
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
//								case 5: fatal

								default: info += "bork"; break;
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

								default: info += " bork"; break;
							}
							break;

						// Returns Item property true seeing.
						// itemproperty ItemPropertyTrueSeeing();
						case 71: // ITEM_PROPERTY_TRUE_SEEING
							info += "[0](";
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
//						case 72: // ITEM_PROPERTY_ON_MONSTER_HIT
							// kL_NOTE: I don't think we'll be enchanting any creature-items ....
							// But if so see case 48: // ITEM_PROPERTY_ON_HIT_PROPERTIES
//							break;

						// Returns Item property turn resistance. You must specify the resistance bonus.
						// The bonus must be an integer between 1 and 50.
						// itemproperty ItemPropertyTurnResistance(int nModifier);
						case 73: // ITEM_PROPERTY_TURN_RESISTANCE
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 51)
							{
								info += "+" + par;
							}
							else
								info += "bork";
							break;

						// Returns Item property Massive Critical. You must specify the extra damage
						// constant(IP_CONST_DAMAGEBONUS_*) of the criticals.
						// itemproperty ItemPropertyMassiveCritical(int nDamage);
						case 74: // ITEM_PROPERTY_MASSIVE_CRITICALS
							info += "[1](";
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

								default: info += "bork"; break;
							}
							break;

						// Returns Item property free action.
						// itemproperty ItemPropertyFreeAction();
						case 75: // ITEM_PROPERTY_FREEDOM_OF_MOVEMENT
							info += "[0](";
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
//						case 77: // ITEM_PROPERTY_MONSTER_DAMAGE
							// kL_NOTE: I don't think we'll be enchanting any creature-items ....
//							break;

						// Returns Item property immunity to spell level. You must specify the level of
						// which that and below the user will be immune. The level must be an integer
						// between 1 and 9. By putting in a 3 it will mean the user is immune to all
						// 3rd level and lower spells.
						// itemproperty ItemPropertyImmunityToSpellLevel(int nLevel);
						case 78: // ITEM_PROPERTY_IMMUNITY_SPELLS_BY_LEVEL
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 10)
							{
								info += "L" + par;
							}
							else
								info += "bork";
							break;

						// Returns Item property special walk. If no parameters are specified it will
						// automatically use the zombie walk. This will apply the special walk animation
						// to the user.
						// itemproperty ItemPropertySpecialWalk(int nWalkType=0);
						// Iprp_Walk.2da
						case 79: // ITEM_PROPERTY_SPECIAL_WALK
							info += "[1](";
							switch (GetPar(pars, 0))
							{
								case 0:
								case 1: info += "zombie"; break;

								default: info += "bork"; break;
							}
							break;

						// Returns Item property healers kit. You must specify the level of the kit.
						// The modifier must be an integer between 1 and 12.
						// itemproperty ItemPropertyHealersKit(int nModifier);
						case 80: // ITEM_PROPERTY_HEALERS_KIT
							info += "[1](";
							if ((par = GetPar(pars, 0)) > 0 && par < 13)
							{
								info += "+" + par;
							}
							else
								info += "bork";
							break;

						// Returns Item property weight increase. You must specify the weight increase
						// constant(IP_CONST_WEIGHTINCREASE_*).
						// itemproperty ItemPropertyWeightIncrease(int nWeight);
						case 81: // ITEM_PROPERTY_WEIGHT_INCREASE
							info += "[1](";
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

								default: info += "bork"; break;
							}
							break;

						// Creates an item property that (when applied to a weapon item) causes a spell to be cast
						// when a successful strike is made, or (when applied to armor) is struck by an opponent.
						// - nSpell uses the IP_CONST_ONHIT_CASTSPELL_* constants
						// itemproperty ItemPropertyOnHitCastSpell(int nSpell, int nLevel);
						// Iprp_OnHitSpells.2da
						case 82: // ITEM_PROPERTY_ONHITCASTSPELL
							info += "[2](";
							if ((par = GetPar(pars, 0)) != -1)
							{
								if (iphitspellLabels.Count != 0
									&& par < iphitspellLabels.Count)
								{
									info += iphitspellLabels[par];
								}
								else
									info += par;
							}
							else
								info += "bork";

							if ((par = GetPar(pars, 1)) > 0)
							{
								info += " L" + par;
							}
							else
								info += " bork";
							break;

						// Creates a visual effect (ITEM_VISUAL_*) that may be applied to
						// melee weapons only.
						// itemproperty ItemPropertyVisualEffect(int nEffect);
						// Iprp_VisualFx.2da
						case 83: // ITEM_PROPERTY_VISUALEFFECT
							info += "[1](";
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

								default: info += "bork"; break;
							}
							break;

						// Creates an item property that offsets the effect on arcane spell failure
						// that a particular item has. Parameters come from the ITEM_PROP_ASF_* group.
						// kL_NOTE: There is no "ITEM_PROP_ASF_* group" - perhaps see Iprp_ArcSpell.2da ...
						// itemproperty ItemPropertyArcaneSpellFailure(int nModLevel);
						case 84: // ITEM_PROPERTY_ARCANE_SPELL_FAILURE
							info += "[1](";
							info += GetPar(pars, 0) + " raw"; // TODO: investigate
							break;

//						case 85: // no case 85 in nwscript: "ArrowCatching"
//						case 86: // no case 86 in nwscript: "Bashing"
//						case 87: // no case 87 in nwscript: "Animated"
//						case 88: // no case 88 in nwscript: "Wild"
//						case 89: // no case 89 in nwscript: "Etherealness"

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
							info += "[4](ip DamageReduction cannot be scripted.";
							break;
					}
				}
			}
			else
			{
				ip = encodedIp;
			}

			return info + ")";
		}

		/// <summary>
		/// Gets a par-value as an int out of a comma-delimited string of pars.
		/// </summary>
		/// <param name="pars">comma-delimited string of pars</param>
		/// <param name="par">position of the par-value to retrieve</param>
		/// <returns>the par-value as an int; -1 if it didn't parse</returns>
		static int GetPar(string pars, int par)
		{
			string[] ips = pars.Split(',');

			if (par < ips.Length)
			{
				int result;
				if (Int32.TryParse(ips[par], out result))
					return result;
			}
			return -1;
		}
	}
}
