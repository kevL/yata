﻿using System;
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

							if (val == Constants.Stars)
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
							if (val != Constants.Stars)
							{
								info = Table.Cols[col].text + ": ";

								string par = String.Empty;
								int pos;

								string[] ips = val.Split(';');
								for (int ip = 0; ip != ips.Length; ++ip)
								{
									par = ips[ip];
									if ((pos = par.IndexOf(',')) != -1)
									{
										if (Int32.TryParse(par.Substring(0, pos), out result)
											&& result < Info.ipLabels.Count)
										{
											info += Info.ipLabels[result]
												  + Info.GetEncodedParsDescription(par);

											if (ip != ips.Length - 1)
												info += ", ";
										}
									}
									else // is a PropertySet preparation val.
									{
										info += "PropertySet val=" + par; // TODO: description for par.
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
//				case -1: // rowhead
//				case  0: // id
//				case  1: // "Label"
//				case  2: // "Name"
//				case  3: // "IconResRef"

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
							info += " " + Info.rangeRanges[r];
						}
					}
					break;

//				case  6: // "VS"

				case  7: // "MetaMagic"
					if (!String.IsNullOrEmpty(val = Table[id,col].text))
					{
						info = Table.Cols[col].text + ": ";

						if (Int32.TryParse(val.Substring(2),
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
									info += "All Blast Shapes and Eldritch Essences";
									break;
								case META_I_SHAPES:
									info += "All Blast Shapes";
									break;
								case META_I_ESSENCES:
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

									if ((result & META_I_DRAINING_BLAST) != 0) // Invocation Shapes and Essences ->
									{
										if (space) info += " - "; // that should never happen.
										info += "Draining Blast"; //(256)
										space = true;
									}
									if ((result & META_I_ELDRITCH_SPEAR) != 0)
									{
										if (space) info += ", ";
										info += "Eldritch Spear"; //(512)
										space = true;
									}
									if ((result & META_I_FRIGHTFUL_BLAST) != 0)
									{
										if (space) info += ", ";
										info += "Frightful Blast"; //(1024)
										space = true;
									}
									if ((result & META_I_HIDEOUS_BLOW) != 0)
									{
										if (space) info += ", ";
										info += "Hideous Blow"; //(2048)
										space = true;
									}
									if ((result & META_I_BESHADOWED_BLAST) != 0)
									{
										if (space) info += ", ";
										info += "Beshadowed Blast"; //(4096)
										space = true;
									}
									if ((result & META_I_BRIMSTONE_BLAST) != 0)
									{
										if (space) info += ", ";
										info += "Brimstone Blast"; //(8192)
										space = true;
									}
									if ((result & META_I_ELDRITCH_CHAIN) != 0)
									{
										if (space) info += ", ";
										info += "Eldritch Chain"; //(16384)
										space = true;
									}
									if ((result & META_I_HELLRIME_BLAST) != 0)
									{
										if (space) info += ", ";
										info += "Hellrime Blast"; //(32768)
										space = true;
									}
									if ((result & META_I_BEWITCHING_BLAST) != 0)
									{
										if (space) info += ", ";
										info += "Bewitching Blast"; //(65536)
										space = true;
									}
									if ((result & META_I_ELDRITCH_CONE) != 0)
									{
										if (space) info += ", ";
										info += "Eldritch Cone"; //(131072)
										space = true;
									}
									if ((result & META_I_NOXIOUS_BLAST) != 0)
									{
										if (space) info += ", ";
										info += "Noxious Blast"; //(262144)
										space = true;
									}
									if ((result & META_I_VITRIOLIC_BLAST) != 0)
									{
										if (space) info += ", ";
										info += "Vitriolic Blast"; //(524288)
										space = true;
									}
									if ((result & META_I_ELDRITCH_DOOM) != 0)
									{
										if (space) info += ", ";
										info += "Eldritch Doom"; //(1048576)
										space = true;
									}
									if ((result & META_I_UTTERDARK_BLAST) != 0)
									{
										if (space) info += ", ";
										info += "Utterdark Blast"; //(2097152)
										space = true;
									}
									if ((result & META_I_HINDERING_BLAST) != 0)
									{
										if (space) info += ", ";
										info += "Hindering Blast"; //(4194304)
										space = true;
									}
									if ((result & META_I_BINDING_BLAST) != 0)
									{
										if (space) info += ", ";
										info += "Binding Blast"; //(8388608)
									}
									break;
								}
							}
						}
						else if (val == Constants.Stars)
							info += "none";
						else
							info += "bork";
					}
					break;
			}

			return info;
		}

		const int META_NONE               = 0x00000000; //       0
		const int META_EMPOWER            = 0x00000001; //       1
		const int META_EXTEND             = 0x00000002; //       2
		const int META_MAXIMIZE           = 0x00000004; //       4
		const int META_QUICKEN            = 0x00000008; //       8
		const int META_SILENT             = 0x00000010; //      16
		const int META_STILL              = 0x00000020; //      32
		const int META_PERSISTENT         = 0x00000040; //      64
		const int META_PERMANENT          = 0x00000080; //     128

		const int META_I_DRAINING_BLAST   = 0x00000100; //     256
		const int META_I_ELDRITCH_SPEAR   = 0x00000200; //     512
		const int META_I_FRIGHTFUL_BLAST  = 0x00000400; //    1024
		const int META_I_HIDEOUS_BLOW     = 0x00000800; //    2048
		const int META_I_BESHADOWED_BLAST = 0x00001000; //    4096
		const int META_I_BRIMSTONE_BLAST  = 0x00002000; //    8192
		const int META_I_ELDRITCH_CHAIN   = 0x00004000; //   16384
		const int META_I_HELLRIME_BLAST   = 0x00008000; //   32768
		const int META_I_BEWITCHING_BLAST = 0x00010000; //   65536
		const int META_I_ELDRITCH_CONE    = 0x00020000; //  131072
		const int META_I_NOXIOUS_BLAST    = 0x00040000; //  262144
		const int META_I_VITRIOLIC_BLAST  = 0x00080000; //  524288
		const int META_I_ELDRITCH_DOOM    = 0x00100000; // 1048576
		const int META_I_UTTERDARK_BLAST  = 0x00200000; // 2097152
		const int META_I_HINDERING_BLAST  = 0x00400000; // 4194304
		const int META_I_BINDING_BLAST    = 0x00800000; // 8388608

		const int META_I_SHAPES           = 0x00124A00; //  1198592 - all blast shapes
		const int META_I_ESSENCES         = 0x00EDB500; // 15578368 - all eldritch essences
		const int META_I_ALL              = 0x00FFFF00; // 16776960 - all shapes and essences

		const int META_ANY                = unchecked((int)0xFFFFFFFF); // 4294967295 - the kitchen sink (not).


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
		#endregion Spells info


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
			Info.GropeLabels(Path.Combine(directory, "ranges.2da"),
							 Info.rangeLabels,
							 it_PathRanges2da,
							 1, // label
							 2, // range
							 Info.rangeRanges);
		}
	}
}