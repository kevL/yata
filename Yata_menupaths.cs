using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Methods (static)
		/// <summary>
		/// Gets the <c>OpenFileDialog.Filter</c> for a specified filetype.
		/// </summary>
		/// <param name="ext">the extension of filetype without dot</param>
		/// <returns></returns>
		internal static string GetFileFilter(string ext)
		{
			return "2da files (*." + ext + ")|*." + ext + "|All files (*.*)|*.*";
		}

		/// <summary>
		/// Gets initial directory for <c>OpenFileDialog</c>.
		/// </summary>
		/// <returns>few really know ...</returns>
		/// <remarks>When the app starts the CurrentDirectory is set to
		/// <c>Application.StartupPath</c> by .NET. But invoking an
		/// <c>OpenFileDialog</c> with that as its <c>InitialDirectory</c> is
		/// bogus - so if the directory is left blank the dialog finds one of
		/// the ComDlg32 MRUs in the registry and uses that instead.
		/// <br/><br/>
		/// Note that <c>OpenFileDialog.InitialDirectory</c> does not always
		/// work - so combine the desired path with the desired filename and
		/// assign it to <c>OpenFileDialog.FileName</c> before calling
		/// <c>ShowDialog()</c>.</remarks>
		internal static string GetCurrentDirectory()
		{
			string dir = Directory.GetCurrentDirectory();
			if (dir != Application.StartupPath)
				return dir;

			return String.Empty;
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
		/// Rifles through various .2das for <c><see cref="Info"/></c>.
		/// </summary>
		/// <param name="dir"></param>
		internal void GropeLabels(string dir)
		{
			// Crafting info ->
			Info.GropeLabels(Path.Combine(dir, "baseitems.2da"),
							 Info.tagLabels,
							 it_PathBaseItems2da,
							 2);

			Info.GropeLabels(Path.Combine(dir, "feat.2da"),
							 Info.featLabels,
							 it_PathFeat2da,
							 1);

			Info.GropeLabels(Path.Combine(dir, "itempropdef.2da"),
							 Info.ipLabels,
							 it_PathItemPropDef2da,
							 2);

			Info.GropeLabels(Path.Combine(dir, "skills.2da"),
							 Info.skillLabels,
							 it_PathSkills2da,
							 1);

			Info.GropeLabels(Path.Combine(dir, "spells.2da"),
							 Info.spellLabels,
							 it_PathSpells2da,
							 1);


			Info.GropeLabels(Path.Combine(dir, "classes.2da"),
							 Info.classLabels,
							 it_PathClasses2da,
							 1);

			Info.GropeLabels(Path.Combine(dir, "disease.2da"),
							 Info.diseaseLabels,
							 it_PathDisease2da,
							 1);

			Info.GropeLabels(Path.Combine(dir, "iprp_ammocost.2da"),
							 Info.ipammoLabels,
							 it_PathIprpAmmoCost2da,
							 2);

			Info.GropeLabels(Path.Combine(dir, "iprp_feats.2da"),
							 Info.ipfeatLabels,
							 it_PathIprpFeats2da,
							 2);

			Info.GropeLabels(Path.Combine(dir, "iprp_onhitspell.2da"),
							 Info.iphitspellLabels,
							 it_PathIprpOnHitSpell2da,
							 1);

			Info.GropeLabels(Path.Combine(dir, "iprp_spells.2da"),
							 Info.ipspellLabels,
							 it_PathIprpSpells2da,
							 1, // label
							 3, // level
							 Info.ipspellLevels);

			Info.GropeLabels(Path.Combine(dir, "racialtypes.2da"),
							 Info.raceLabels,
							 it_PathRaces2da,
							 1);


			// Spells info ->
			Info.GropeLabels(Path.Combine(dir, "categories.2da"),
							 Info.categoryLabels,
							 it_PathCategories2da,
							 1);

			Info.GropeLabels(Path.Combine(dir, "ranges.2da"),
							 Info.rangeLabels,
							 it_PathRanges2da,
							 1, // label
							 2, // range
							 Info.rangeRanges);

			Info.GropeSpellTarget(Path.Combine(dir, "spelltarget.2da"),
								  Info.targetLabels,
								  it_PathSpellTarget2da,
								  1,
								  3,
								  4,
								  Info.targetWidths,
								  Info.targetLengths);


			// Feat info ->
			Info.GropeLabels(Path.Combine(dir, "combatmodes.2da"),
							 Info.combatmodeLabels,
							 it_PathCombatModes2da,
							 1);

			Info.GropeLabels(Path.Combine(dir, "masterfeats.2da"),
							 Info.masterfeatLabels,
							 it_PathMasterFeats2da,
							 1);


			// Classes info ->
			Info.GropeLabels(Path.Combine(dir, "packages.2da"),
							 Info.packageLabels,
							 it_PathPackages2da,
							 1);


			// BaseItems info ->
			Info.GropeLabels(Path.Combine(dir, "inventorysnds.2da"),
							 Info.soundLabels,
							 it_PathInventorySnds2da,
							 1);

			Info.GropeFields(Path.Combine(dir, "itemprops.2da"), // see also ItemTypes.2da
							 Info.propFields,
							 it_PathItemProps2da,
							 21);

			Info.GropeLabels(Path.Combine(dir, "weaponsounds.2da"),
							 Info.weapsoundLabels,
							 it_PathWeaponSounds2da,
							 1);

			Info.GropeLabels(Path.Combine(dir, "ammunitiontypes.2da"),
							 Info.ammoLabels,
							 it_PathAmmunitionTypes2da,
							 1);
		}

		/// <summary>
		/// Rifles through the zipped 2da-files in the NwN2 stock directory
		/// \Data for <c><see cref="Info"/></c>.
		/// </summary>
		internal void GropeZipData()
		{
			string dir = Settings._pathzipdata;

			var zips = new List<string>
			{
				dir + @"\2DA.zip",
				dir + @"\2DA_X1.zip",
				dir + @"\2DA_X2.zip"
			};

			foreach (var zip in zips)
			{
				if (File.Exists(zip))
				{
					using (var fs = new FileStream(zip, FileMode.Open, FileAccess.Read))
					using (var zf = new ZipFile(fs))
					{
						ZipEntry ze;

						// Crafting info ->
						if ((ze = zf.GetEntry("baseitems.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.tagLabels,
											 it_PathBaseItems2da,
											 2);
						}

						if ((ze = zf.GetEntry("feat.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.featLabels,
											 it_PathFeat2da,
											 1);
						}

						if ((ze = zf.GetEntry("itempropdef.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.ipLabels,
											 it_PathItemPropDef2da,
											 2);
						}

						if ((ze = zf.GetEntry("skills.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.skillLabels,
											 it_PathSkills2da,
											 1);
						}

						if ((ze = zf.GetEntry("spells.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.spellLabels,
											 it_PathSpells2da,
											 1);
						}


						if ((ze = zf.GetEntry("classes.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.classLabels,
											 it_PathClasses2da,
											 1);
						}

						if ((ze = zf.GetEntry("disease.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.diseaseLabels,
											 it_PathDisease2da,
											 1);
						}

						if ((ze = zf.GetEntry("iprp_ammocost.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.ipammoLabels,
											 it_PathIprpAmmoCost2da,
											 2);
						}

						if ((ze = zf.GetEntry("iprp_feats.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.ipfeatLabels,
											 it_PathIprpFeats2da,
											 2);
						}

						if ((ze = zf.GetEntry("iprp_onhitspell.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.iphitspellLabels,
											 it_PathIprpOnHitSpell2da,
											 1);
						}

						if ((ze = zf.GetEntry("iprp_spells.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.ipspellLabels,
											 it_PathIprpSpells2da,
											 1, // label
											 3, // level
											 Info.ipspellLevels);
						}

						if ((ze = zf.GetEntry("racialtypes.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.raceLabels,
											 it_PathRaces2da,
											 1);
						}


						// Spells info ->
						if ((ze = zf.GetEntry("categories.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.categoryLabels,
											 it_PathCategories2da,
											 1);
						}

						if ((ze = zf.GetEntry("ranges.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.rangeLabels,
											 it_PathRanges2da,
											 1, // label
											 2, // range
											 Info.rangeRanges);
						}

						if ((ze = zf.GetEntry("spelltarget.2da")) != null)
						{
							Info.GropeSpellTarget(Info.GetZipped2daLines(zf,ze),
												  Info.targetLabels,
												  it_PathSpellTarget2da,
												  1,
												  3,
												  4,
												  Info.targetWidths,
												  Info.targetLengths);
						}

						// Feat info ->
						if ((ze = zf.GetEntry("combatmodes.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.combatmodeLabels,
											 it_PathCombatModes2da,
											 1);
						}

						if ((ze = zf.GetEntry("masterfeats.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.masterfeatLabels,
											 it_PathMasterFeats2da,
											 1);
						}

						// Classes info ->
						if ((ze = zf.GetEntry("packages.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.packageLabels,
											 it_PathPackages2da,
											 1);
						}

						// BaseItems info ->
						if ((ze = zf.GetEntry("inventorysnds.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.soundLabels,
											 it_PathInventorySnds2da,
											 1);
						}

						if ((ze = zf.GetEntry("itemprops.2da")) != null) // see also ItemTypes.2da
						{
							Info.GropeFields(Info.GetZipped2daLines(zf,ze),
											 Info.propFields,
											 it_PathItemProps2da,
											 21);
						}

						if ((ze = zf.GetEntry("weaponsounds.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.weapsoundLabels,
											 it_PathWeaponSounds2da,
											 1);
						}

						if ((ze = zf.GetEntry("ammunitiontypes.2da")) != null)
						{
							Info.GropeLabels(Info.GetZipped2daLines(zf,ze),
											 Info.ammoLabels,
											 it_PathAmmunitionTypes2da,
											 1);
						}
					}
				}
			}
		}

		/// <summary>
		/// Handles clicking the PathBaseItems menuitem.
		/// Intended to add labels from <c>BaseItems.2da</c> to the
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
					ofd.Title  = " Select BaseItems.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "baseitems.2da");
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
		/// Intended to add labels from <c>Feat.2da</c> to the
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
					ofd.Title  = " Select Feat.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "feat.2da");
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
		/// Handles clicking the PathInventorySounds menuitem.
		/// Intended to add labels from <c>InventorySnds.2da</c> to the
		/// <c><see cref="Info.soundLabels">Info.soundLabels</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathInventorySnds2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathInventorySnds2da(object sender, EventArgs e)
		{
			if (!it_PathInventorySnds2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = " Select InventorySnds.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "inventorysnds.2da");
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.soundLabels,
										 it_PathInventorySnds2da,
										 1);
					}
				}
			}
			else
			{
				it_PathInventorySnds2da.Checked = false;
				Info.soundLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathItemPropDef menuitem.
		/// Intended to add labels from <c>ItemPropDef.2da</c> to the
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
					ofd.Title  = " Select ItemPropDef.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "itempropdef.2da");
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
		/// Handles clicking the PathItemProps menuitem.
		/// Intended to add colabels from <c>ItemProps.2da</c> to the
		/// <c><see cref="Info.propFields">Info.propFields</see></c> list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathItemProps2da"/></c></param>
		/// <param name="e"></param>
		/// <remarks>See also <c>ItemTypes.2da</c>.</remarks>
		void itclick_PathItemProps2da(object sender, EventArgs e)
		{
			if (!it_PathItemProps2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = " Select ItemProps.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "itemprops.2da");
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeFields(ofd.FileName,
										 Info.propFields,
										 it_PathItemProps2da,
										 21);
					}
				}
			}
			else
			{
				it_PathItemProps2da.Checked = false;
				Info.propFields.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathSkills menuitem.
		/// Intended to add labels from <c>Skills.2da</c> to the
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
					ofd.Title  = " Select Skills.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "skills.2da");
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
		/// Intended to add labels from <c>Spells.2da</c> to the
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
					ofd.Title  = " Select Spells.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "spells.2da");
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
		/// Intended to add labels from <c>Classes.2da</c> to the
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
					ofd.Title  = " Select Classes.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "classes.2da");
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
		/// Intended to add labels from <c>Disease.2da</c> to the
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
					ofd.Title  = " Select Disease.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "disease.2da");
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
		/// Intended to add labels from <c>Iprp_AmmoCost.2da</c> to the
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
					ofd.Title  = " Select Iprp_AmmoCost.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "iprp_ammocost.2da");
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
		/// Intended to add labels from <c>Iprp_Feats.2da</c> to the
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
					ofd.Title  = " Select Iprp_Feats.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "iprp_feats.2da");
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
		/// Intended to add labels from <c>Iprp_OnHitSpell.2da</c> to the
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
					ofd.Title  = " Select Iprp_OnHitSpell.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "iprp_onhitspell.2da");
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
		/// Intended to add labels and levels from <c>Iprp_Spells.2da</c> to the
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
					ofd.Title  = " Select Iprp_Spells.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "iprp_spells.2da");
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
		/// Intended to add labels from <c>RacialTypes.2da</c> to the
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
					ofd.Title  = " Select RacialTypes.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "racialtypes.2da");
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
		/// Intended to add labels from <c>Categories.2da</c> to the
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
					ofd.Title  = " Select Categories.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "categories.2da");
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
		/// Intended to add labels from <c>Ranges.2da</c> to the
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
					ofd.Title  = " Select Ranges.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "ranges.2da");
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
		/// Intended to add labels from <c>SpellTarget.2da</c> to the
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
					ofd.Title  = " Select SpellTarget.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "spelltarget.2da");
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
		/// Intended to add labels from <c>CombatModes.2da</c> to the
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
					ofd.Title  = " Select CombatModes.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "combatmodes.2da");
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
		/// Intended to add labels from <c>MasterFeats.2da</c> to the
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
					ofd.Title  = " Select MasterFeats.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "masterfeats.2da");
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
		/// Intended to add labels from <c>Packages.2da</c> to the
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
					ofd.Title  = " Select Packages.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "packages.2da");
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

		/// <summary>
		/// Handles clicking the PathWeaponSounds menuitem.
		/// Intended to add labels from <c>WeaponSounds.2da</c> to the
		/// <c><see cref="Info.weapsoundLabels">Info.weapsoundLabels</see></c>
		/// list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathWeaponSounds2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathWeaponSounds2da(object sender, EventArgs e)
		{
			if (!it_PathWeaponSounds2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = " Select WeaponSounds.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "weaponsounds.2da");
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.weapsoundLabels,
										 it_PathWeaponSounds2da,
										 1);
					}
				}
			}
			else
			{
				it_PathWeaponSounds2da.Checked = false;
				Info.weapsoundLabels.Clear();
			}
		}

		/// <summary>
		/// Handles clicking the PathAmmunitionTypes menuitem.
		/// Intended to add labels from <c>AmmunitionTypes.2da</c> to the
		/// <c><see cref="Info.ammoLabels">Info.ammoLabels</see></c>
		/// list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PathAmmunitionTypes2da"/></c></param>
		/// <param name="e"></param>
		void itclick_PathAmmunitionTypes2da(object sender, EventArgs e)
		{
			if (!it_PathAmmunitionTypes2da.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = " Select AmmunitionTypes.2da";
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "ammunitiontypes.2da");
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
					{
						Info.GropeLabels(ofd.FileName,
										 Info.ammoLabels,
										 it_PathAmmunitionTypes2da,
										 1);
					}
				}
			}
			else
			{
				it_PathAmmunitionTypes2da.Checked = false;
				Info.ammoLabels.Clear();
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
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "dialog.tlk");
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
					ofd.Filter = GetFileFilter("2da");

					ofd.FileName = Path.Combine(GetCurrentDirectory(), "*.tlk");
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
						TalkReader.Load(ofd.FileName, it_PathTalkC);
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
