﻿using System;
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


		#region Handlers (Paths)
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
			Info.GropeLabels(Path.Combine(directory, "combatmodes.2da"),
							 Info.combatmodeLabels,
							 it_PathCombatModes2da,
							 1);

			Info.GropeLabels(Path.Combine(directory, "masterfeats.2da"),
							 Info.masterfeatLabels,
							 it_PathMasterFeats2da,
							 1);


			// Classes info ->
			Info.GropeLabels(Path.Combine(directory, "packages.2da"),
							 Info.packageLabels,
							 it_PathPackages2da,
							 1);
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
		#endregion Handlers (Paths)


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

					ofd.FileName = "dialog.tlk";
					ofd.AutoUpgradeEnabled = false;

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

					ofd.FileName = "*.tlk";
					ofd.AutoUpgradeEnabled = false;

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