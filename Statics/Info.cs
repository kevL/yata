using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	static class Info
	{
		#region Fields (static)
		#region Crafting caches
		/// <summary>
		/// A list that holds labels for spells in <c>Spells.2da</c> - optional.
		/// </summary>
		internal static List<string> spellLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for feats in <c>Feat.2da</c> - optional.
		/// </summary>
		internal static List<string> featLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for itemproperties in
		/// <c>ItemPropDef.2da</c> - optional.
		/// </summary>
		internal static List<string> ipLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for baseitem-types in <c>BaseItems.2da</c>
		/// - optional.
		/// </summary>
		internal static List<string> tagLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for skills in <c>Skills.2da</c> - optional.
		/// </summary>
		internal static List<string> skillLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for races in <c>Races.2da</c> - optional.
		/// </summary>
		internal static List<string> raceLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for classes in <c>Classes.2da</c> -
		/// optional.
		/// </summary>
		internal static List<string> classLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for ip-spells in <c>Iprp_Spells.2da</c> -
		/// optional.
		/// </summary>
		internal static List<string> ipspellLabels = new List<string>();

		/// <summary>
		/// A list that holds casterlevel for ip-spells in
		/// <c>Iprp_Spells.2da</c> - optional.
		/// </summary>
		internal static List<int> ipspellLevels = new List<int>();

		/// <summary>
		/// A list that holds labels for diseases in <c>Disease.2da</c> -
		/// optional.
		/// </summary>
		internal static List<string> diseaseLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for onhitspell in
		/// <c>Iprp_OnHitSpell.2da</c> - optional.
		/// </summary>
		internal static List<string> iphitspellLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for feats in <c>Iprp_Feats.2da</c> -
		/// optional.
		/// </summary>
		internal static List<string> ipfeatLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for ammo in <c>Iprp_AmmoCost.2da</c> -
		/// optional.
		/// </summary>
		internal static List<string> ipammoLabels = new List<string>();
		#endregion Crafting caches


		#region Spells caches
		// NOTE: Also uses Spells.2da for master and child spell-labels.
		// NOTE: Also uses Classes.2da for spontaneous cast class-labels.
		// NOTE: Also uses Feat.2da for feat-id feat-labels.

		/// <summary>
		/// A list that holds labels for categories in <c>Categories.2da</c> -
		/// optional.
		/// </summary>
		internal static List<string> categoryLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for spell-ranges in <c>Ranges.2da</c> -
		/// optional.
		/// </summary>
		internal static List<string> rangeLabels = new List<string>();

		/// <summary>
		/// A list that holds ranges for spell-ranges in <c>Ranges.2da</c> -
		/// optional.
		/// </summary>
		internal static List<int> rangeRanges = new List<int>();

		/// <summary>
		/// A list that holds labels for spell-targets in <c>SpellTarget.2da</c>
		/// - optional.
		/// </summary>
		/// <remarks>Groping <c>SpellTarget.2da</c> does NOT use the regular
		/// grope-routines. It has 2 fields that are float-values (instead of
		/// only 1 optional int-value). So
		/// <c><see cref="GropeSpellTarget()">GropeSpellTarget()</see></c>
		/// will be used instead of the regular
		/// <c><see cref="GropeLabels()">GropeLabels()</see></c> and it needs to
		/// be called by the general
		/// <c><see cref="Yata.GropeLabels()">Yata.GropeLabels()</see></c>
		/// function as well as the path-item.</remarks>
		internal static List<string> targetLabels  = new List<string>();
		internal static List<float>  targetWidths  = new List<float>();
		internal static List<float>  targetLengths = new List<float>();
		#endregion Spells caches


		#region Feat caches
		// NOTE: Also uses Feat.2da for feat-labels.
		// NOTE: Also uses Spells.2da for spell-labels.
		// NOTE: Also uses Categories.2da for category-labels.
		// NOTE: Also uses Skills.2da for skill-labels.
		// NOTE: Also uses Classes.2da for class-labels.

		/// <summary>
		/// A list that holds labels for combatmodes in <c>CombatModes.2da</c> -
		/// optional.
		/// </summary>
		internal static List<string> combatmodeLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for master-feats in <c>MasterFeats.2da</c>
		/// - optional.
		/// </summary>
		internal static List<string> masterfeatLabels = new List<string>();
		#endregion Feat caches


		#region Class caches
		// NOTE: Also uses Feat.2da for feat-labels.

		/// <summary>
		/// A list that holds labels for packages in <c>Packages.2da</c> -
		/// optional.
		/// </summary>
		internal static List<string> packageLabels = new List<string>();
		#endregion Class caches


		#region BaseItem caches
		// NOTE: Also uses BaseItems.2da for baseitem-labels.
		// NOTE: Also uses Feat.2da for feat-labels.

		/// <summary>
		/// A list that holds labels for inventory-sounds in
		/// <c>InventorySnds.2da</c> - optional.
		/// </summary>
		internal static List<string> soundLabels = new List<string>();

		/// <summary>
		/// A list that holds colabels for itemproperties in
		/// <c>ItemPropDef.2da</c> - optional.
		/// A list that holds labels for allowed itemproperties on baseitemtypes
		/// in <c>ItemProps.2da</c> - optional.
		/// </summary>
		internal static List<string> propFields = new List<string>();

		/// <summary>
		/// A list that holds labels for weapon-sounds in
		/// <c>WeaponSounds.2da</c> - optional.
		/// </summary>
		internal static List<string> weapsoundLabels = new List<string>();

		/// <summary>
		/// A list that holds labels for ammunition-types in
		/// <c>AmmunitionTypes.2da</c> - optional.
		/// </summary>
		internal static List<string> ammoLabels = new List<string>();
		#endregion BaseItem caches


		static bool _helper;
		#endregion Fields (static)


		#region Methods (static)
		/// <summary>
		/// Gets the label-strings from a given 2da.
		/// TODO: Check that the given 2da really has the required cols.
		/// </summary>
		/// <param name="pfe2da">path_file_extension to the 2da</param>
		/// <param name="labels">the cache in which to store the labels</param>
		/// <param name="it">the path-item on which to toggle Checked</param>
		/// <param name="col">col in the 2da of the label</param>
		/// <param name="col1">col in the 2da of an int</param>
		/// <param name="ints">a collection MUST be passed in if
		/// <paramref name="col1"/> is not <c>-1</c></param>
		internal static void GropeLabels(string pfe2da,
										 ICollection<string> labels,
										 ToolStripMenuItem it,
										 int col,
										 int col1 = -1,
										 ICollection<int> ints = null)
		{
			if (File.Exists(pfe2da))
			{
				labels.Clear();
				if (ints != null) ints.Clear();

				string[] lines = File.ReadAllLines(pfe2da);

				// WARNING: This function does *not* handle quotation marks around 2da fields.
				if (!hasQuote(lines, pfe2da))
				{
					_helper = true;
					GropeLabels(lines, labels, it, col, col1, ints);
					_helper = false;
				}
				it.Checked = (labels.Count != 0);
			}
		}

		/// <summary>
		/// Gets the label-strings from a given 2da.
		/// TODO: Check that the given 2da really has the required cols.
		/// </summary>
		/// <param name="lines">an array of lines of a 2da-file</param>
		/// <param name="labels">the cache in which to store the labels</param>
		/// <param name="it">the path-item on which to toggle Checked</param>
		/// <param name="col">col in the 2da of the label</param>
		/// <param name="col1">col in the 2da of an int</param>
		/// <param name="ints">a collection MUST be passed in if
		/// <paramref name="col1"/> is not <c>-1</c></param>
		internal static void GropeLabels(string[] lines,
										 ICollection<string> labels,
										 ToolStripMenuItem it,
										 int col,
										 int col1 = -1,
										 ICollection<int> ints = null)
		{
			if (!_helper)
			{
				labels.Clear();
				if (ints != null) ints.Clear();
			}

			// WARNING: This function does *not* handle quotation marks around 2da fields.
			// And it does not even check for them since ... the stock 2das don't have
			// any - hopefully.

			string line; string[] fields;

			for (int i = 0; i != lines.Length; ++i)
			{
				if ((line = lines[i].Trim()).Length != 0)
				{
					fields = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

					if (fields.Length > col && fields.Length > col1)
					{
						int id;
						if (Int32.TryParse(fields[0], out id)) // is a valid 2da row
						{
							labels.Add(fields[col]); // and hope for the best.

							if (col1 != -1)
							{
								int result;
								if (!Int32.TryParse(fields[col1], out result))
									result = -1; // always add an int to keep sync w/ the labels

								ints.Add(result);
							}
						}
					}
				}
			}

			if (!_helper) it.Checked = (labels.Count != 0);
		}


		/// <summary>
		/// Gets the label-strings plus width/height values from SpellTarget.2da.
		/// TODO: Check that the given 2da really has the required cols.
		/// </summary>
		/// <param name="pfe2da">path_file_extension to the 2da</param>
		/// <param name="labels">the cache in which to store the labels</param>
		/// <param name="it">the path-item on which to toggle Checked</param>
		/// <param name="col">col in the 2da of the label</param>
		/// <param name="col1">col in the 2da of a float (default -1)</param>
		/// <param name="col2">col in the 2da of a float (default -1)</param>
		/// <param name="floats1">a collection MUST be passed in if col1 is not -1</param>
		/// <param name="floats2">a collection MUST be passed in if col2 is not -1</param>
		internal static void GropeSpellTarget(string pfe2da,
											  ICollection<string> labels,
											  ToolStripMenuItem it,
											  int col,
											  int col1,
											  int col2,
											  ICollection<float> floats1,
											  ICollection<float> floats2)
		{
			if (File.Exists(pfe2da))
			{
				labels.Clear();
				floats1.Clear();
				floats2.Clear();

				string[] lines = File.ReadAllLines(pfe2da);

				// WARNING: This function does *not* handle quotation marks around 2da fields.
				if (!hasQuote(lines, pfe2da))
				{
					_helper = true;
					GropeSpellTarget(lines, labels, it, col, col1, col2, floats1, floats2);
					_helper = false;
				}
				it.Checked = (labels.Count != 0);
			}
		}

		/// <summary>
		/// Gets the label-strings plus width/height values from SpellTarget.2da.
		/// TODO: Check that the given 2da really has the required cols.
		/// </summary>
		/// <param name="lines">an array of lines of a 2da-file</param>
		/// <param name="labels">the cache in which to store the labels</param>
		/// <param name="it">the path-item on which to toggle Checked</param>
		/// <param name="col">col in the 2da of the label</param>
		/// <param name="col1">col in the 2da of a float (default -1)</param>
		/// <param name="col2">col in the 2da of a float (default -1)</param>
		/// <param name="floats1">a collection MUST be passed in if col1 is not -1</param>
		/// <param name="floats2">a collection MUST be passed in if col2 is not -1</param>
		internal static void GropeSpellTarget(string[] lines,
											  ICollection<string> labels,
											  ToolStripMenuItem it,
											  int col,
											  int col1,
											  int col2,
											  ICollection<float> floats1,
											  ICollection<float> floats2)
		{
			if (!_helper)
			{
				labels.Clear();
				floats1.Clear();
				floats2.Clear();
			}

			// WARNING: This function does *not* handle quotation marks around 2da fields.
			// And it does not even check for them since ... the stock 2das don't have
			// any - hopefully.

			string line; string[] fields;

			for (int i = 0; i != lines.Length; ++i)
			{
				if ((line = lines[i].Trim()).Length != 0)
				{
					fields = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

					if (fields.Length > col && fields.Length > col1 && fields.Length > col2)
					{
						int id;
						if (Int32.TryParse(fields[0], out id)) // is a valid 2da row
						{
							labels.Add(fields[col]); // and hope for the best.

							float result;

							if (!Single.TryParse(fields[col1], out result))
							{
								result = 0.0F; // always add a float to keep sync w/ the labels
							}
							floats1.Add(result);

							if (!Single.TryParse(fields[col2], out result))
							{
								result = 0.0F; // always add a float to keep sync w/ the labels
							}
							floats2.Add(result);
						}
					}
				}
			}

			if (!_helper) it.Checked = (labels.Count != 0);
		}


		/// <summary>
		/// Gets the colabel-strings from a given 2da.
		/// </summary>
		/// <param name="pfe2da">path_file_extension to the 2da</param>
		/// <param name="labels">the cache in which to store the labels</param>
		/// <param name="it">the path-item on which to toggle Checked</param>
		/// <param name="stop"></param>
		/// <param name="strt"></param>
		internal static void GropeFields(string pfe2da,
										 ICollection<string> labels,
										 ToolStripMenuItem it,
										 int stop,
										 int strt = 0)
		{
			if (File.Exists(pfe2da))
			{
				labels.Clear();

				using (var sr = new StreamReader(pfe2da))
				{
					// WARNING: This function does *not* handle quotation marks around 2da fields.
					// And it does not even check for them since it only gets the colheads.

					string line; string[] fields;

					for (int i = 0; i != 3; ++i)
					{
						if ((line = sr.ReadLine()) != null)
						{
							if (i == 2 && (line = line.Trim()).Length != 0)
							{
								fields = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

								for (int f = strt; f != fields.Length && f <= stop; ++f)
								{
									labels.Add(fields[f]);
								}
							}
						}
						else break;
					}
				}

				it.Checked = (labels.Count != 0);
			}
		}

		/// <summary>
		/// Gets the colabel-strings from a given 2da.
		/// </summary>
		/// <param name="lines">an array of lines of a 2da-file</param>
		/// <param name="labels">the cache in which to store the labels</param>
		/// <param name="it">the path-item on which to toggle Checked</param>
		/// <param name="stop"></param>
		/// <param name="strt"></param>
		internal static void GropeFields(IList<string> lines,
										 ICollection<string> labels,
										 ToolStripMenuItem it,
										 int stop,
										 int strt = 0)
		{
			labels.Clear();

			// WARNING: This function does *not* handle quotation marks around 2da fields.
			// And it does not even check for them since it only gets the colheads.

			string[] fields = lines[2].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

			for (int f = strt; f != fields.Length && f <= stop; ++f)
			{
				labels.Add(fields[f]);
			}

			it.Checked = (labels.Count != 0);
		}


		/// <summary>
		/// Checks for a quotation character and if found shows an error to the
		/// user.
		/// </summary>
		/// <param name="lines">an array of strings</param>
		/// <param name="pfe2da">the fullpath of the 2da-file</param>
		/// <returns><c>true</c> if a quote character is found</returns>
		static bool hasQuote(string[] lines, string pfe2da)
		{
			foreach (string line in lines)
			foreach (char c in line)
			{
				if (c == '"')
				{
					const string head = "The 2da-file contains double-quotes. Although that can be"
									  + " valid in a 2da-file Yata's 2da Info-grope is not coded to cope."
									  + " Format the 2da-file (in a texteditor) to not use double-quotes"
									  + " if you want to access it for 2da Info.";
					using (var ib = new Infobox(Infobox.Title_error,
												head,
												pfe2da,
												InfoboxType.Error))
					{
						ib.ShowDialog(Yata.that);
					}
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the text of a zipped 2da-file as an array of <c>strings</c>
		/// given a
		/// <c><see cref="ZipFile"/></c> and a <c><see cref="ZipEntry"/></c>.
		/// </summary>
		/// <param name="zf"><c>ZipFile</c></param>
		/// <param name="ze"><c>ZipEntry</c></param>
		internal static string[] GetZipped2daLines(ZipFile zf, ZipEntry ze)
		{
			string[] lines = null;

			using (Stream s0 = zf.GetInputStream(ze))
			using (var s1 = new MemoryStream())
			{
				CopyStream(s0,s1);

				string text = Encoding.UTF8.GetString(s1.ToArray());
				//logfile.Log(text);

				lines = text.Split(gs.CRandorLF, StringSplitOptions.RemoveEmptyEntries);
			}
			return lines;
		}

		/// <summary>
		/// Copies a <c>Stream</c> from <paramref name="input"/> to
		/// <paramref name="output"/>.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="output"></param>
		static void CopyStream(Stream input, Stream output)
		{
			var buffer = new byte[4096]; //32768 or 81920
			int read;
			while ((read = input.Read(buffer, 0, buffer.Length)) != 0)
			{
				output.Write(buffer, 0, read);
			}
		}
		#endregion Methods (static)
	}
}
