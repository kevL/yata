using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Methods (static)
		/// <summary>
		/// Gets the dialog-filter for <c>.2da</c> files.
		/// </summary>
		/// <returns></returns>
		static string Get2daFilter()
		{
			return "2da files (*.2da)|*.2da|All files (*.*)|*.*";
		}

		/// <summary>
		/// Gets the dialog-filter for <c>.Tlk</c> files.
		/// </summary>
		/// <returns></returns>
		internal static string GetTlkFilter()
		{
			return "tlk files (*.tlk)|*.tlk|All files (*.*)|*.*";
		}
		#endregion Methods (static)


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

				case 3: // "TAGS" - no 2da
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
									info += Info.GetTccType(result);
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
												info += Info.GetDecodedDescription(ipEncoded, result, pos);
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
		/// Handles clicking the PathBaseItems menuitem.
		/// Intended to add labels from BaseItems.2da to the
		/// <c><see cref="Info.tagLabels">Info.tagLabels</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathBaseItems2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathBaseItems2da(object sender, EventArgs e)
		{
			if (!it_PathBaseItems2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select BaseItems.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "baseitems.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.tagLabels,
										 it_PathBaseItems2da,
										 2);
					}
				}
			}
			else
			{
				it_PathBaseItems2da.Checked = false;
				Info.tagLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathFeat menuitem.
		/// Intended to add labels from Feat.2da to the
		/// <c><see cref="Info.featLabels">Info.featLabels</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathFeat2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathFeat2da(object sender, EventArgs e)
		{
			if (!it_PathFeat2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Feat.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "feat.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.featLabels,
										 it_PathFeat2da,
										 1);
					}
				}
			}
			else
			{
				it_PathFeat2da.Checked = false;
				Info.featLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathItemPropDef menuitem.
		/// Intended to add labels from ItemPropDef.2da to the
		/// <c><see cref="Info.ipLabels">Info.ipLabels</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathItemPropDef2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathItemPropDef2da(object sender, EventArgs e)
		{
			if (!it_PathItemPropDef2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select ItemPropDef.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "itempropdef.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.ipLabels,
										 it_PathItemPropDef2da,
										 2);
					}
				}
			}
			else
			{
				it_PathItemPropDef2da.Checked = false;
				Info.ipLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathSkills menuitem.
		/// Intended to add labels from Skills.2da to the
		/// <c><see cref="Info.skillLabels">Info.skillLabels</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathSkills2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathSkills2da(object sender, EventArgs e)
		{
			if (!it_PathSkills2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Skills.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "skills.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.skillLabels,
										 it_PathSkills2da,
										 1);
					}
				}
			}
			else
			{
				it_PathSkills2da.Checked = false;
				Info.skillLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathSpells menuitem.
		/// Intended to add labels from Spells.2da to the
		/// <c><see cref="Info.spellLabels">Info.spellLabels</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathSpells2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathSpells2da(object sender, EventArgs e)
		{
			if (!it_PathSpells2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Spells.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "spells.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.spellLabels,
										 it_PathSpells2da,
										 1);
					}
				}
			}
			else
			{
				it_PathSpells2da.Checked = false;
				Info.spellLabels.Clear();
			}
		}


		// sub-info (for EncodedIPs only) ->

		/// <summary>
		/// Handles clicking the PathClasses menuitem.
		/// Intended to add labels from Classes.2da to the
		/// <c><see cref="Info.classLabels">Info.classLabels</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathClasses2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathClasses2da(object sender, EventArgs e)
		{
			if (!it_PathClasses2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Classes.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "classes.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.classLabels,
										 it_PathClasses2da,
										 1);
					}
				}
			}
			else
			{
				it_PathClasses2da.Checked = false;
				Info.classLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathDisease menuitem.
		/// Intended to add labels from Disease.2da to the
		/// <c><see cref="Info.diseaseLabels">Info.diseaseLabels</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathDisease2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathDisease2da(object sender, EventArgs e)
		{
			if (!it_PathDisease2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Disease.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "disease.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.diseaseLabels,
										 it_PathDisease2da,
										 1);
					}
				}
			}
			else
			{
				it_PathDisease2da.Checked = false;
				Info.diseaseLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathIprpAmmoCost menuitem.
		/// Intended to add labels from Iprp_AmmoCost.2da to the
		/// <c><see cref="Info.ipammoLabels">Info.ipammoLabels</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathIprpAmmoCost2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathIprpAmmoCost2da(object sender, EventArgs e)
		{
			if (!it_PathIprpAmmoCost2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Iprp_AmmoCost.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "iprp_ammocost.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.ipammoLabels,
										 it_PathIprpAmmoCost2da,
										 2);
					}
				}
			}
			else
			{
				it_PathIprpAmmoCost2da.Checked = false;
				Info.ipammoLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathIprpFeats menuitem.
		/// Intended to add labels from Iprp_Feats.2da to the
		/// <c><see cref="Info.ipfeatLabels">Info.ipfeatLabels</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathIprpFeats2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathIprpFeats2da(object sender, EventArgs e)
		{
			if (!it_PathIprpFeats2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Iprp_Feats.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "iprp_feats.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.ipfeatLabels,
										 it_PathIprpFeats2da,
										 2);
					}
				}
			}
			else
			{
				it_PathIprpFeats2da.Checked = false;
				Info.ipfeatLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathIprpOnHitSpell menuitem.
		/// Intended to add labels from Iprp_OnHitSpell.2da to the
		/// <c><see cref="Info.iphitspellLabels">Info.iphitspellLabels</see></c>
		/// list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathIprpOnHitSpell2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathIprpOnHitSpell2da(object sender, EventArgs e)
		{
			if (!it_PathIprpOnHitSpell2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Iprp_OnHitSpell.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "iprp_onhitspell.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.iphitspellLabels,
										 it_PathIprpOnHitSpell2da,
										 1);
					}
				}
			}
			else
			{
				it_PathIprpOnHitSpell2da.Checked = false;
				Info.iphitspellLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathIprpSpells menuitem.
		/// Intended to add labels and levels from Iprp_Spells.2da to the
		/// <c><see cref="Info.ipspellLabels">Info.ipspellLabels</see></c> and
		/// <c><see cref="Info.ipspellLevels">Info.ipspellLevels</see></c>
		/// lists.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathIprpSpells2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathIprpSpells2da(object sender, EventArgs e)
		{
			if (!it_PathIprpSpells2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Iprp_Spells.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "iprp_spells.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.ipspellLabels,
										 it_PathIprpSpells2da,
										 1,
										 3,
										 Info.ipspellLevels);
					}
				}
			}
			else
			{
				it_PathIprpSpells2da.Checked = false;
				Info.ipspellLabels.Clear();
				Info.ipspellLevels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathRaces menuitem.
		/// Intended to add labels from RacialTypes.2da to the
		/// <c><see cref="Info.raceLabels">Info.raceLabels</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathRaces2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathRaces2da(object sender, EventArgs e)
		{
			if (!it_PathRaces2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select RacialTypes.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "racialtypes.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.raceLabels,
										 it_PathRaces2da,
										 1);
					}
				}
			}
			else
			{
				it_PathRaces2da.Checked = false;
				Info.raceLabels.Clear();
			}
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
				case 4: // "School" (SpellSchools.2da) - no 2da
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

				case 5: // "Range" (Ranges.2da) - no 2da
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

				case 7: // "MetaMagic" - no 2da
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

				case 8: // "TargetType" - no 2da
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

				case 47: // "SubRadSpell1" - Spells.2da
				case 48: // "SubRadSpell2"
				case 49: // "SubRadSpell3"
				case 50: // "SubRadSpell4"
				case 51: // "SubRadSpell5"
				case 53: // "Master"
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

				case 52: // "Category" - Categories.2da
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

				case 54: // "UserType" - no 2da
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

				case 58: // "SpontCastClassReq" - Classes.2da
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

				case 61: // "FeatID" - Feat.2da
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

				case 65: // "AsMetaMagic" - no 2da
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

				case 66: // "TargetingUI" - SpellTarget.2da
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


		/// <summary>
		/// Handles clicking the PathCategories menuitem.
		/// Intended to add labels from Categories.2da to the
		/// <c><see cref="Info.categoryLabels">Info.categoryLabels</see></c>
		/// list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathCategories2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathCategories2da(object sender, EventArgs e)
		{
			if (!it_PathCategories2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Categories.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "categories.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.categoryLabels,
										 it_PathCategories2da,
										 1);
					}
				}
			}
			else
			{
				it_PathCategories2da.Checked = false;
				Info.categoryLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathRanges menuitem.
		/// Intended to add labels from Ranges.2da to the
		/// <c><see cref="Info.rangeLabels">Info.rangeLabels</see></c> list and
		/// ranges to the
		/// <c><see cref="Info.rangeRanges">Info.rangeRanges</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathRanges2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathRanges2da(object sender, EventArgs e)
		{
			if (!it_PathRanges2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Ranges.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "ranges.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.rangeLabels,
										 it_PathRanges2da,
										 1,
										 2,
										 Info.rangeRanges);
					}
				}
			}
			else
			{
				it_PathRanges2da.Checked = false;
				Info.rangeLabels.Clear();
				Info.rangeRanges.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathSpellTarget menuitem.
		/// Intended to add labels from SpellTarget.2da to the
		/// <c><see cref="Info.targetLabels">Info.targetLabels</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathSpellTarget2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathSpellTarget2da(object sender, EventArgs e)
		{
			if (!it_PathSpellTarget2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select SpellTarget.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "spelltarget.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeSpellTarget(ofd.FileName,
											  Info.targetLabels,
											  it_PathSpellTarget2da,
											  1,
											  3,
											  4,
											  Info.targetWidths,
											  Info.targetLengths);
					}
				}
			}
			else
			{
				it_PathSpellTarget2da.Checked = false;
				Info.targetLabels.Clear();
			}
		}
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

				case 25: // "CATEGORY" - Categories.2da
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

				case 32: // "MASTERFEAT" - MasterFeats.2da
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

				case 47: // "TOOLSCATEGORIES" - no 2da
					if (!String.IsNullOrEmpty(val = Table[id,col].text)
						&& val != gs.Stars) // NOTE: "****" is 0 which is actually "All Feats"
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result)
							&& result > -1 && result < 7)
						{
							switch (result)
							{
								case 0:  info += "All Feats";           break;
								case 1:  info += "Combat Feats";        break;
								case 2:  info += "Active Combat Feats"; break;
								case 3:  info += "Defensive Feats";     break;
								case 4:  info += "Magical Feats";       break;
								case 5:  info += "Class/Racial Feats";  break;
								case 6:  info += "Other Feats";         break;
							}
						}
						else
							info += gs.bork;
					}
					break;

				case 57: // "ToggleMode" - CombatModes.2da
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

		/// <summary>
		/// Handles clicking the PathMasterFeats menuitem.
		/// Intended to add labels from MasterFeats.2da to the
		/// <c><see cref="Info.masterfeatLabels">Info.masterfeatLabels</see></c>
		/// list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathMasterFeats2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathMasterFeats2da(object sender, EventArgs e)
		{
			if (!it_PathMasterFeats2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select MasterFeats.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "masterfeats.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.masterfeatLabels,
										 it_PathMasterFeats2da,
										 1);
					}
				}
			}
			else
			{
				it_PathMasterFeats2da.Checked = false;
				Info.masterfeatLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathCombatModes menuitem.
		/// Intended to add labels from CombatModes.2da to the
		/// <c><see cref="Info.combatmodeLabels">Info.combatmodeLabels</see></c>
		/// list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathCombatModes2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathCombatModes2da(object sender, EventArgs e)
		{
			if (!it_PathCombatModes2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select CombatModes.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "combatmodes.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.combatmodeLabels,
										 it_PathCombatModes2da,
										 1);
					}
				}
			}
			else
			{
				it_PathCombatModes2da.Checked = false;
				Info.combatmodeLabels.Clear();
			}
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
				case 40: // "PrimaryAbil" - no 2da
				case 41: // "SpellAbil"
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

				case 42: // "AlignRestrict" - no 2da
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

				case 43: // "AlignRstrctType" - no 2da
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

				case 74: // "Package" - Packages.2da
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

		/// <summary>
		/// Handles clicking the PathPackages menuitem.
		/// Intended to add labels from Packages.2da to the
		/// <c><see cref="Info.packageLabels">Info.packageLabels</see></c>
		/// list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathPackages2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathPackages2da(object sender, EventArgs e)
		{
			if (!it_PathPackages2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Packages.2da";
					ofd.Filter = Get2daFilter();

					ofd.FileName = "packages.2da";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.packageLabels,
										 it_PathPackages2da,
										 1);
					}
				}
			}
			else
			{
				it_PathPackages2da.Checked = false;
				Info.packageLabels.Clear();
			}
		}
		#endregion Class info


		#region Paths
		/// <summary>
		/// Opens a <c>FolderBrowserDialog</c> for groping.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathAll"/></c></param>
		/// <param name="e"></param>
		void itclick_PathAll(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				fbd.ShowNewFolderButton = false;
				fbd.Description = "Find a folder to search through 2da-files for extra info.";

				if (fbd.ShowDialog() == DialogResult.OK)
					GropeLabels(fbd.SelectedPath);
			}
		}

		/// <summary>
		/// Rifles through various .2das for info.
		/// </summary>
		/// <param name="directory"></param>
		internal void GropeLabels(string directory)
		{
			// Crafting info ->
			Info.GropeLabels(Path.Combine(directory, "baseitems.2da"),
							 Info.tagLabels,
							 it_PathBaseItems2da,
							 2);

			Info.GropeLabels(Path.Combine(directory, "feat.2da"),
							 Info.featLabels,
							 it_PathFeat2da,
							 1);

			Info.GropeLabels(Path.Combine(directory, "itempropdef.2da"),
							 Info.ipLabels,
							 it_PathItemPropDef2da,
							 2);

			Info.GropeLabels(Path.Combine(directory, "skills.2da"),
							 Info.skillLabels,
							 it_PathSkills2da,
							 1);

			Info.GropeLabels(Path.Combine(directory, "spells.2da"),
							 Info.spellLabels,
							 it_PathSpells2da,
							 1);


			Info.GropeLabels(Path.Combine(directory, "classes.2da"),
							 Info.classLabels,
							 it_PathClasses2da,
							 1);

			Info.GropeLabels(Path.Combine(directory, "disease.2da"),
							 Info.diseaseLabels,
							 it_PathDisease2da,
							 1);

			Info.GropeLabels(Path.Combine(directory, "iprp_ammocost.2da"),
							 Info.ipammoLabels,
							 it_PathIprpAmmoCost2da,
							 2);

			Info.GropeLabels(Path.Combine(directory, "iprp_feats.2da"),
							 Info.ipfeatLabels,
							 it_PathIprpFeats2da,
							 2);

			Info.GropeLabels(Path.Combine(directory, "iprp_onhitspell.2da"),
							 Info.iphitspellLabels,
							 it_PathIprpOnHitSpell2da,
							 1);

			Info.GropeLabels(Path.Combine(directory, "iprp_spells.2da"),
							 Info.ipspellLabels,
							 it_PathIprpSpells2da,
							 1, // label
							 3, // level
							 Info.ipspellLevels);

			Info.GropeLabels(Path.Combine(directory, "racialtypes.2da"),
							 Info.raceLabels,
							 it_PathRaces2da,
							 1);


			// Spells info ->
			Info.GropeLabels(Path.Combine(directory, "categories.2da"),
							 Info.categoryLabels,
							 it_PathCategories2da,
							 1);

			Info.GropeLabels(Path.Combine(directory, "ranges.2da"),
							 Info.rangeLabels,
							 it_PathRanges2da,
							 1, // label
							 2, // range
							 Info.rangeRanges);

			Info.GropeSpellTarget(Path.Combine(directory, "spelltarget.2da"),
								  Info.targetLabels,
								  it_PathSpellTarget2da,
								  1,
								  3,
								  4,
								  Info.targetWidths,
								  Info.targetLengths);


			// Feat info ->
			Info.GropeLabels(Path.Combine(directory, "masterfeats.2da"),
							 Info.masterfeatLabels,
							 it_PathMasterFeats2da,
							 1);

			Info.GropeLabels(Path.Combine(directory, "combatmodes.2da"),
							 Info.combatmodeLabels,
							 it_PathCombatModes2da,
							 1);


			// Classes info ->
			Info.GropeLabels(Path.Combine(directory, "packages.2da"),
							 Info.packageLabels,
							 it_PathPackages2da,
							 1);
		}
		#endregion Paths


		#region Talkfile
		/// <summary>
		/// Handles <c>Click</c> to load <c>Dialog.Tlk</c> file.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathTalkD"/></c></param>
		/// <param name="e"></param>
		void itclick_PathTalkD(object sender, EventArgs e)
		{
			if (!it_PathTalkD.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = " Select Dialog.Tlk";
					ofd.Filter = GetTlkFilter();

					if (ofd.ShowDialog() == DialogResult.OK)
						TalkReader.Load(ofd.FileName, it_PathTalkD);
				}
			}
			else
			{
				it_PathTalkD.Checked = false;
				TalkReader.DictDialo.Clear();
			}
		}

		/// <summary>
		/// Handles <c>Click</c> to load an alternate Talkfile.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathTalkC"/></c></param>
		/// <param name="e"></param>
		void itclick_PathTalkC(object sender, EventArgs e)
		{
			if (!it_PathTalkC.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = " Select a TalkTable";
					ofd.Filter = GetTlkFilter();

					if (ofd.ShowDialog() == DialogResult.OK)
						TalkReader.Load(ofd.FileName, it_PathTalkC, true);
				}
			}
			else
			{
				it_PathTalkC.Checked = false;
				TalkReader.DictCusto.Clear();
				TalkReader.AltLabel = null;
			}
		}
		#endregion Talkfile
	}
}
