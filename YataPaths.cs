using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	partial class YataForm
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
			string info = "n/a";

			string val;
			int result;

			switch (col)
			{
//				case -1: // rowhead
//				case  0: // id

				case  1: // "CATEGORY"
					if (it_PathSpells2da.Checked)
					{
						if (!String.IsNullOrEmpty(val = Table[id,col].text)
							&& Int32.TryParse(val, out result)
							&& result < Info.spellLabels.Count)
						{
							info = Table.Cols[col].text + ": "
								 + Info.spellLabels[result];
						}
					}
					break;

//				case  2: // "REAGENTS"

				case  3: // "TAGS"
					if (!String.IsNullOrEmpty(val = Table[id,col].text))
					{
						if (val.StartsWith("B", StringComparison.InvariantCulture)) // is in BaseItems.2da
						{
							if (it_PathBaseItems2da.Checked)
							{
								info = Table.Cols[col].text + ": (base) ";

								string[] array = val.Substring(1).Split(','); // lose the "B"
								for (int tag = 0; tag != array.Length; ++tag)
								{
									if (Int32.TryParse(array[tag], out result)
										&& result < Info.tagLabels.Count)
									{
										info += Info.tagLabels[result];

										if (tag != array.Length - 1)
											info += ", ";
									}
								}
							}
						}
						else // is a TCC item-type
						{
							info = Table.Cols[col].text + ": (tcc) ";

							if (val == gs.Stars)
							{
								info += Info.GetTccType(0); // TCC_TYPE_NONE
							}
							else
							{
								string[] array = val.Split(',');
								for (int tag = 0; tag != array.Length; ++tag)
								{
									if (Int32.TryParse(array[tag], out result))
									{
										info += Info.GetTccType(result);

										if (tag != array.Length - 1)
											info += ", ";
									}
								}
							}
						}
					}
					break;

				case  4: // "EFFECTS"
					if (it_PathItemPropDef2da.Checked)
					{
						if (!String.IsNullOrEmpty(val = Table[id,col].text))
						{
							if (val != gs.Stars)
							{
								info = Table.Cols[col].text + ": ";

								string ipEncoded; int pos;

								string[] ips = val.Split(';');
								for (int ip = 0; ip != ips.Length; ++ip)
								{
									ipEncoded = ips[ip];
									if ((pos = ipEncoded.IndexOf(',')) != -1)
									{
										if (Int32.TryParse(ipEncoded.Substring(0, pos), out result)
											&& result < Info.ipLabels.Count)
										{
											info += Info.ipLabels[result];

											if (ipEncoded.Length > pos + 1)
												info += Info.GetEncodedParsDescription(ipEncoded, result, pos);
											else
												info += " bork";

											if (ip != ips.Length - 1)
												info += ", ";
										}
									}
									else // is a PropertySet preparation val.
									{
										info += "PropertySet val=" + ipEncoded; // TODO: description for par.
									}
								}
							}
						}
					}
					break;

//				case  5: // "OUTPUT"

				case  6: // "SKILL"
					if (it_PathFeat2da.Checked)
					{
						if (!String.IsNullOrEmpty(val = Table[id,col].text)
							&& Int32.TryParse(val, out result))
						{
							string cat = Table[id,1].text;
							if (!String.IsNullOrEmpty(cat))
							{
								int result2;
								if (Int32.TryParse(cat, out result2)) // is triggered by spell id
								{
									if (result < Info.featsLabels.Count)
									{
										info = Table.Cols[col].text + ": "
											 + Info.featsLabels[result];
									}
								}
								else // is triggered NOT by spell - but by mold-tag, or is Alchemy or Distillation
								{
									if (result < Info.skillLabels.Count)
									{
										info = Table.Cols[col].text + ": "
											 + Info.skillLabels[result];
									}
								}
							}
						}
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
		/// Intended to add labels from BaseItems.2da to the 'tagLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathBaseItems2da(object sender, EventArgs e)
		{
			if (!it_PathBaseItems2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select BaseItems.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		/// Intended to add labels from Feat.2da to the 'featsLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathFeat2da(object sender, EventArgs e)
		{
			if (!it_PathFeat2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Feat.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.featsLabels,
										 it_PathFeat2da,
										 1);
					}
				}
			}
			else
			{
				it_PathFeat2da.Checked = false;
				Info.featsLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathItemPropDef menuitem.
		/// Intended to add labels from ItemPropDef.2da to the 'ipLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathItemPropDef2da(object sender, EventArgs e)
		{
			if (!it_PathItemPropDef2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select ItemPropDef.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		/// Intended to add labels from Skills.2da to the 'skillLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathSkills2da(object sender, EventArgs e)
		{
			if (!it_PathSkills2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Skills.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		/// Intended to add labels from Spells.2da to the 'spellLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathSpells2da(object sender, EventArgs e)
		{
			if (!it_PathSpells2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Spells.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		/// Intended to add labels from Classes.2da to the 'classLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathClasses2da(object sender, EventArgs e)
		{
			if (!it_PathClasses2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Classes.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		/// Intended to add labels from Disease.2da to the 'diseaseLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathDisease2da(object sender, EventArgs e)
		{
			if (!it_PathDisease2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Disease.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		/// Intended to add labels from Iprp_AmmoCost.2da to the 'ipammoLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathIprpAmmoCost2da(object sender, EventArgs e)
		{
			if (!it_PathIprpAmmoCost2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Iprp_AmmoCost.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		/// Intended to add labels from Iprp_Feats.2da to the 'ipfeatsLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathIprpFeats2da(object sender, EventArgs e)
		{
			if (!it_PathIprpFeats2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Iprp_Feats.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.ipfeatsLabels,
										 it_PathIprpFeats2da,
										 2);
					}
				}
			}
			else
			{
				it_PathIprpFeats2da.Checked = false;
				Info.ipfeatsLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathIprpOnHitSpells menuitem.
		/// Intended to add labels from Iprp_OnHitSpells.2da to the
		/// 'iphitspellsLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathIprpOnHitSpells2da(object sender, EventArgs e)
		{
			if (!it_PathIprpOnHitSpell2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Iprp_OnHitSpells.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		/// 'ipspellsLabels' and 'ipspellsLevels' lists.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathIprpSpells2da(object sender, EventArgs e)
		{
			if (!it_PathIprpSpells2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Iprp_Spells.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.ipspellsLabels,
										 it_PathIprpSpells2da,
										 1,
										 3,
										 Info.ipspellsLevels);
					}
				}
			}
			else
			{
				it_PathIprpSpells2da.Checked = false;
				Info.ipspellsLabels.Clear();
				Info.ipspellsLevels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathRaces menuitem.
		/// Intended to add labels from RacialTypes.2da to the 'raceLabels' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathRaces2da(object sender, EventArgs e)
		{
			if (!it_PathRaces2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select RacialTypes.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
			string info = "n/a";

			string val;
			int result;

			switch (col)
			{
				case  4: // "School" (also SpellSchools.2da)
					if (!String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						switch (val.ToUpper())
						{
							case "A": info += "Abjuration";    break;
							case "C": info += "Conjuration";   break;
							case "D": info += "Divination";    break;
							case "E": info += "Enchantment";   break;
							case "I": info += "Illusion";      break;
							case "N": info += "Necromancy";    break;
							case "T": info += "Transmutation"; break;
							case "V": info += "Evocation";     break;

							default:
								info += "bork";
								break;
						}
					}
					break;

				case  5: // "Range" (Ranges.2da)
					if (!String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						int r = -1;
						switch (val.ToUpper())
						{
							case "P": info += "Personal"; r =  0; break; // NOTE: 'rangeLabels' could be used but
							case "T": info += "Touch";    r =  1; break; // they're abnormal: "SpellRngPers" eg.
							case "S": info += "Short";    r =  2; break;
							case "M": info += "Medium";   r =  3; break;
							case "L": info += "Long";     r =  4; break;
							case "I": info += "Infinite"; r = 14; break;

							default:
								info += "bork";
								break;
						}

						if (r != -1 && r < Info.rangeRanges.Count && it_PathRanges2da.Checked)
						{
							info += " " + Info.rangeRanges[r] + "m";
						}
					}
					break;

				case  7: // "MetaMagic"
					if (!String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val.Length > 2
							&& Int32.TryParse(val.Substring(2),
											  NumberStyles.AllowHexSpecifier,
											  CultureInfo.InvariantCulture,
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
										if (space) info += " ";
										info += "(2)Extend";
										space = true;
									}
									if ((result & META_MAXIMIZE) != 0)
									{
										if (space) info += " ";
										info += "(4)Maximize";
										space = true;
									}
									if ((result & META_QUICKEN) != 0)
									{
										if (space) info += " ";
										info += "(8)Quicken";
										space = true;
									}
									if ((result & META_SILENT) != 0)
									{
										if (space) info += " ";
										info += "(16)Silent";
										space = true;
									}
									if ((result & META_STILL) != 0)
									{
										if (space) info += " ";
										info += "(32)Still";
										space = true;
									}
									if ((result & META_PERSISTENT) != 0)
									{
										if (space) info += " ";
										info += "(64)Persistent";
										space = true;
									}
									if ((result & META_PERMANENT) != 0)
									{
										if (space) info += " ";
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
						else if (val == gs.Stars)
							info += "none";
						else
							info += "bork";
					}
					break;

				case  8: // "TargetType"
					if (!String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val.Length > 2
							&& Int32.TryParse(val.Substring(2),
											  NumberStyles.AllowHexSpecifier,
											  CultureInfo.InvariantCulture,
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
										if (space) info += " ";
										info += "(2)Creatures";
										space = true;
									}
									if ((result & TARGET_GROUND) != 0)
									{
										if (space) info += " ";
										info += "(4)Ground";
										space = true;
									}
									if ((result & TARGET_ITEMS) != 0)
									{
										if (space) info += " ";
										info += "(8)Items";
										space = true;
									}
									if ((result & TARGET_DOORS) != 0)
									{
										if (space) info += " ";
										info += "(16)Doors";
										space = true;
									}
									if ((result & TARGET_PLACEABLES) != 0)
									{
										if (space) info += " ";
										info += "(32)Placeables";
										space = true;
									}
									if ((result & TARGET_TRIGGERS) != 0)
									{
										if (space) info += " ";
										info += "(64)Triggers";
									}
									break;
								}
							}
						}
						else if (val == gs.Stars)
							info += "none";
						else
							info += "bork";
					}
					break;

				case 47: // "SubRadSpell1" (Spells.2da)
				case 48: // "SubRadSpell2"
				case 49: // "SubRadSpell3"
				case 50: // "SubRadSpell4"
				case 51: // "SubRadSpell5"
				case 53: // "Master"
					if (it_PathSpells2da.Checked
					 	&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result))
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
								info += "bork";
						}
						else if (val == gs.Stars)
							info += "n/a";
						else
							info += "bork";
					}
					break;

				case 52: // "Category" (Categories.2da)
					if (it_PathCategories2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result) && result > -1)
						{
							if (result < Info.categoryLabels.Count)
							{
								info += Info.categoryLabels[result];
							}
							else
								info += val;
						}
						else
							info += "bork";
					}
					break;

				case 54: // "UserType"
					if (!String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result))
						{
							switch (result)
							{
								case 1: info += "Spell";           break;
								case 2: info += "Special Ability"; break;
								case 3: info += "Feat";            break;
								case 4: info += "Item Power";      break;

								default:
									info += "bork";
									break;
							}
						}
						else
							info += "bork";
					}
					break;

				case 58: // "SpontCastClassReq" (Classes.2da)
					if (it_PathClasses2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result))
						{
							if (result > -1)
							{
								if (result < Info.classLabels.Count)
								{
									switch (result)
									{
										case 0:				// NOTE: Although #0 Barbarian is valid it's used for n/a here.
											info += "n/a";	// The 2da-field ought be "****" instead ofc.
											break;

										default:
											info += Info.classLabels[result];
											break;
									}
								}
								else
									info += val;
							}
							else
								info += "bork";
						}
						else if (val == gs.Stars)
							info += "n/a";
						else
							info += "bork";
					}
					break;

				case 61: // "FeatID"
					if (it_PathFeat2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result))
						{
							if (result > -1)
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
									right    = (result & FEATSPELL_FEATS);
								}

								if (feat < Info.featsLabels.Count)
								{
									info += Info.featsLabels[feat];
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
								info += "bork";
						}
						else if (val == gs.Stars)
							info += "n/a";
						else
							info += "bork";
					}
					break;

				case 65: // "AsMetaMagic"
					if (!String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (val.Length > 2
							&& Int32.TryParse(val.Substring(2),
											  NumberStyles.AllowHexSpecifier,
											  CultureInfo.InvariantCulture,
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

								default:
									info += "bork";
									break;
							}
						}
						else if (val == gs.Stars)
							info += "n/a";
						else
							info += "bork";
					}
					break;

				case 66: // "TargetingUI"
					if (it_PathSpellTarget2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result))
						{
							if (result > -1)
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
								info += "bork";
						}
						else if (val == gs.Stars)
							info += "point"; // id #0 (shouldn't do that though)
						else
							info += "bork";
					}
					break;
			}

			return info;
		}

		// FeatSpells
		const int FEATSPELL_MASTER = 0x00010000;
		const int FEATSPELL_FEATS  = 0x00001111;

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


		/// <summary>
		/// Handles clicking the PathRanges menuitem.
		/// Intended to add labels from Ranges.2da to the 'rangeLabels' list and
		/// ranges to the 'rangeRanges' list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathRanges2da(object sender, EventArgs e)
		{
			if (!it_PathRanges2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Ranges.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		/// Handles clicking the PathCategories menuitem.
		/// Intended to add labels from Categories.2da to the 'categoryLabels'
		/// list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathCategories2da(object sender, EventArgs e)
		{
			if (!it_PathCategories2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select Categories.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		/// Handles clicking the PathSpellTarget menuitem.
		/// Intended to add labels from SpellTarget.2da to the 'targetLabels'
		/// list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathSpellTarget2da(object sender, EventArgs e)
		{
			if (!it_PathSpellTarget2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select SpellTarget.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		/// <returns></returns>
		string getFeatInfo(int id, int col)
		{
			string info = "n/a";

			string val;
			int result;

			switch (col)
			{
				case 20: // Feat.2da
				case 21:
					if (it_PathFeat2da.Checked
						&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result))
						{
							if (result > -1)
							{
								if (result < Info.featsLabels.Count)
								{
									info += Info.featsLabels[result];
								}
								else
									info += val;
							}
							else
								info += "bork";
						}
						else if (val == gs.Stars)
							info += "n/a";
						else
							info += "bork";
					}
					break;

				case 32: // MasterFeats.2da
					if (it_PathMasterFeats2da.Checked
					 	&& !String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val, out result))
						{
							if (result > -1)
							{
								if (result < Info.masterfeatLabels.Count)
								{
									info += Info.masterfeatLabels[result];
								}
								else
									info += val;
							}
							else
								info += "bork";
						}
						else if (val == gs.Stars) // NOTE: "****" is 0 which is actually "ImprovedCritical"
							info += "n/a";
						else
							info += "bork";
					}
					break;
			}

			return info;
		}

		/// <summary>
		/// Handles clicking the PathMasterFeats menuitem.
		/// Intended to add labels from MasterFeats.2da to the 'masterfeatLabels'
		/// list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathMasterFeats2da(object sender, EventArgs e)
		{
			if (!it_PathMasterFeats2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = "Select MasterFeats.2da";
					ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		#endregion Feat info


		#region Paths
		void itclick_PathAll(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				fbd.ShowNewFolderButton = false;
				fbd.Description = "Find a folder to search through 2da-files for extra info.";

				if (fbd.ShowDialog() == DialogResult.OK)
				{
					GropeLabels(fbd.SelectedPath);
				}
			}
		}

		internal void GropeLabels(string directory)
		{
			// Crafting info ->
			Info.GropeLabels(Path.Combine(directory, "baseitems.2da"),
							 Info.tagLabels,
							 it_PathBaseItems2da,
							 2);

			Info.GropeLabels(Path.Combine(directory, "feat.2da"),
							 Info.featsLabels,
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
							 Info.ipfeatsLabels,
							 it_PathIprpFeats2da,
							 2);

			Info.GropeLabels(Path.Combine(directory, "iprp_onhitspell.2da"),
							 Info.iphitspellLabels,
							 it_PathIprpOnHitSpell2da,
							 1);

			Info.GropeLabels(Path.Combine(directory, "iprp_spells.2da"),
							 Info.ipspellsLabels,
							 it_PathIprpSpells2da,
							 1, // label
							 3, // level
							 Info.ipspellsLevels);

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
		}
		#endregion Paths


		#region Talkfile
		/// <summary>
		/// Handles click to load Dialog.Tlk file.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathTalkD(object sender, EventArgs e)
		{
			if (!it_PathTalkD.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = " Select Dialog.Tlk";
					ofd.Filter = "tlk files (*.tlk)|*.tlk|All files (*.*)|*.*";

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
		/// Handles click to load a special Talkfile.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void itclick_PathTalkC(object sender, EventArgs e)
		{
			if (!it_PathTalkC.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = " Select a TalkTable";
					ofd.Filter = "tlk files (*.tlk)|*.tlk|All files (*.*)|*.*";

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
