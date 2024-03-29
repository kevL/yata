﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Fields
		List<string> Strrefheads = new List<string>();

		internal string _strref; // the strref assigned by 'TalkDialog'

		int _strInt;	// cache for cell-context's dropdown functs
		Cell _sel;		// cache for cell-context's dropdown functs
		#endregion Fields


		#region Methods (cell)
		/// <summary>
		/// Shows the RMB-context on a cell for single-cell edit operations.
		/// </summary>
		/// <remarks><c><see cref="_contextCe"/></c> is not assigned to a
		/// <c><see cref="YataGrid">YataGrid's</see></c> <c>ContextMenuStrip</c>
		/// or <c>ContextMenu</c> since that leads to .net bullshit.</remarks>
		internal void ShowCellContext()
		{
			_sel = Table.getSelectedCell(); // '_sel' shall be valid due to rightclick

			bool flagged = (_sel.loadchanged || _sel.replaced);

			cellit_Edit   .Enabled = !Table.Readonly;

			cellit_Cut    .Enabled = !Table.Readonly;
			cellit_Paste  .Enabled = !Table.Readonly;
			cellit_Clear  .Enabled = !Table.Readonly && (flagged || _sel.text != gs.Stars);

			cellit_Lower  .Enabled = !Table.Readonly && (flagged || _sel.text != _sel.text.ToLower(CultureInfo.CurrentCulture));
			cellit_Upper  .Enabled = !Table.Readonly && (flagged || _sel.text != _sel.text.ToUpper(CultureInfo.CurrentCulture));

			cellit_MergeCe.Enabled =
			cellit_MergeRo.Enabled = isMergeEnabled();

			cellit_Strref .Enabled = isStrrefEnabled();


			cellit_Selectrow.Text = "Select row @ id " + _sel.y;
			cellit_Copyrowid.Text = "Copy row id " + Table.Rows[_sel.y]._cells[0].text;

			cellit_Input_zip.Visible = false;

			switch (Table.Info)
			{
				case YataGrid.InfoType.INFO_NONE:
				case YataGrid.InfoType.INFO_CRAFT:
					cellit_Input.Visible =
					cellit_Input.Enabled = false;
					break;

				// TODO: If table is Readonly allow viewing the InfoInputDialog
				// but disable its controls ->

				case YataGrid.InfoType.INFO_SPELL:
					cellit_Input.Text    = "InfoInput (spells.2da)";
					cellit_Input.Visible = true;
					cellit_Input.Enabled = !Table.Readonly && isInfoInputcol_Spells();
					break;

				case YataGrid.InfoType.INFO_FEAT:
					cellit_Input.Text    = "InfoInput (feat.2da)";
					cellit_Input.Visible = true;
					cellit_Input.Enabled = !Table.Readonly && isInfoInputcol_Feat();
					break;

				case YataGrid.InfoType.INFO_CLASS:
					cellit_Input.Text    = "InfoInput (classes.2da)";
					cellit_Input.Visible = true;
					cellit_Input.Enabled = !Table.Readonly && isInfoInputcol_Classes();
					break;

				case YataGrid.InfoType.INFO_ITEM:
					cellit_Input.Text    = "InfoInput (baseitems.2da)";
					cellit_Input.Visible = true;
					cellit_Input.Enabled = !Table.Readonly && isInfoInputcol_Baseitems();
					break;
			}

			_contextCe.Show(Table, Table.PointToClient(Cursor.Position));
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="ShowCellContext()">ShowCellContext()</see></c>.
		/// </summary>
		/// <returns><c>true</c> if merge operations (cell or row) will be
		/// enabled</returns>
		bool isMergeEnabled()
		{
			if (_sel.diff && _diff1 != null && _diff2 != null)
			{
				YataGrid table = null;
				if      (Table == _diff1) table = _diff2;
				else if (Table == _diff2) table = _diff1;

				return table != null && !table.Readonly
					&& table.ColCount > _sel.x
					&& table.RowCount > _sel.y;
			}
			return false;
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="ShowCellContext()">ShowCellContext()</see></c>.
		/// </summary>
		/// <returns><c>true</c> if the InfoInput operation will show for
		/// <c>Spells.2da</c>.</returns>
		bool isInfoInputcol_Spells()
		{
			switch (_sel.x)
			{
				case InfoInputSpells.School: // these don't rely on 2da-gropes ->
				case InfoInputSpells.Range:
				case InfoInputSpells.Vs:
				case InfoInputSpells.MetaMagic:
				case InfoInputSpells.TargetType:
				case InfoInputSpells.ConjAnim:
				case InfoInputSpells.CastAnim:
				case InfoInputSpells.Proj:
				case InfoInputSpells.ProjType:
				case InfoInputSpells.ProjSpwnPoint:
				case InfoInputSpells.ProjOrientation:
				case InfoInputSpells.ImmunityType:
				case InfoInputSpells.ItemImmunity:
				case InfoInputSpells.UserType:
				case InfoInputSpells.UseConcentration:
				case InfoInputSpells.SpontaneouslyCast:
				case InfoInputSpells.HostileSetting:
				case InfoInputSpells.HasProjectile:
				case InfoInputSpells.AsMetaMagic:
				case InfoInputSpells.CastableOnDead:
				case InfoInputSpells.Removed:
					return true;

				case InfoInputSpells.Category:
					if (Info.categoryLabels.Count != 0)
						return true;
					break;

				case InfoInputSpells.SpontCastClassReq:
					if (Info.classLabels.Count != 0)
						return true;
					break;

				case InfoInputSpells.TargetingUI:
					if (Info.targetLabels.Count != 0)
						return true;
					break;

				case InfoInputSpells.IconResRef:
					cellit_Input.Text = "Select icon ...";
					return true;

				case InfoInputSpells.ImpactScript:
					cellit_Input    .Text = "Select script ...";
					cellit_Input_zip.Text = "Select Data/zip script ...";
					cellit_Input_zip.Visible = true;
					return true;

				case InfoInputSpells.ConjVisual0:
				case InfoInputSpells.LowConjVisual0:
				case InfoInputSpells.ConjVisual1:
				case InfoInputSpells.ConjVisual2:
				case InfoInputSpells.CastVisual0:
				case InfoInputSpells.LowCastVisual0:
				case InfoInputSpells.CastVisual1:
				case InfoInputSpells.CastVisual2:
				case InfoInputSpells.ProjSEF:
				case InfoInputSpells.LowProjSEF:
				case InfoInputSpells.ImpactSEF:
				case InfoInputSpells.LowImpactSEF:
					cellit_Input    .Text = "Select specialeffect ...";
					cellit_Input_zip.Text = "Select Data/zip specialeffect ...";
					cellit_Input_zip.Visible = true;
					return true;

				case InfoInputSpells.ConjSoundVFX:
				case InfoInputSpells.ConjSoundMale:
				case InfoInputSpells.ConjSoundFemale:
				case InfoInputSpells.CastSound:
				case InfoInputSpells.ProjSound:
					cellit_Input    .Text = "Select sound ...";
					cellit_Input_zip.Text = "Select Data/zip sound ...";
					cellit_Input_zip.Visible = true;
					return true;

				case InfoInputSpells.ProjModel:
					cellit_Input    .Text = "Select model ...";
					cellit_Input_zip.Text = "Select Data/zip model ...";
					cellit_Input_zip.Visible = true;
					return true;
			}
			return false;
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="ShowCellContext()">ShowCellContext()</see></c>.
		/// </summary>
		/// <returns><c>true</c> if the InfoInput operation will show for
		/// <c>Feat.2da</c>.</returns>
		bool isInfoInputcol_Feat()
		{
			switch (_sel.x)
			{
				// "MINSPELLLVL"
				// "PREREQFEAT1"		info
				// "PREREQFEAT2"		info
				// "GAINMULTIPLE"
				// "EFFECTSSTACK"
				// "ALLCLASSESCANUSE"
				// "CATEGORY"			info + infoinput
				// "SPELLID"			info
				// "SUCCESSOR"			info
				// "USESMAPFEAT"
				// "MASTERFEAT"			info + infoinput
				// "TARGETSELF"
				// "OrReqFeat0"			info
				// "OrReqFeat1"			info
				// "OrReqFeat2"			info
				// "OrReqFeat3"			info
				// "OrReqFeat4"			info
				// "OrReqFeat5"			info
				// "REQSKILL"			info
				// "REQSKILL2"			info
				// "TOOLSCATEGORIES"	info + infoinput
				// "HostileFeat"
				// "MinLevelClass"
				// "PreReqEpic"
				// "FeatCategory"
				// "IsActive"
				// "IsPersistent"
				// "TogleMode"			info + infoinput
				// "DMFeat"
				// "REMOVED"
				// "AlignRestrict"
				// "ImmunityType"
				// "Instant"

				case InfoInputFeat.Category:
					if (Info.categoryLabels.Count != 0)
						return true;
					break;

				case InfoInputFeat.MasterFeat:
					if (Info.masterfeatLabels.Count != 0)
						return true;
					break;

				case InfoInputFeat.REQSKILL:
				case InfoInputFeat.REQSKILL2:
					if (Info.skillLabels.Count != 0)
						return true;
					break;

				case InfoInputFeat.ToolsCategories: // these don't rely on 2da-gropes ->
				case InfoInputFeat.FeatCategory:
				case InfoInputFeat.GAINMULTIPLE:
				case InfoInputFeat.EFFECTSSTACK:
				case InfoInputFeat.ALLCLASSESCANUSE:
				case InfoInputFeat.TARGETSELF:
				case InfoInputFeat.HostileFeat:
				case InfoInputFeat.PreReqEpic:
				case InfoInputFeat.IsActive:
				case InfoInputFeat.IsPersistent:
				case InfoInputFeat.DMFeat:
				case InfoInputFeat.REMOVED:
				case InfoInputFeat.ImmunityType:
				case InfoInputFeat.Instant:
					return true;

				case InfoInputFeat.MinLevelClass:
					if (Info.classLabels.Count != 0)
						return true;
					break;

				case InfoInputFeat.ToggleMode:
					if (Info.combatmodeLabels.Count != 0)
						return true;
					break;

				case InfoInputFeat.icon:
					cellit_Input.Text = "Select icon ...";
					return true;
			}
			return false;
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="ShowCellContext()">ShowCellContext()</see></c>.
		/// </summary>
		/// <returns><c>true</c> if the InfoInput operation will show for
		/// <c>Classes.2da</c>.</returns>
		bool isInfoInputcol_Classes()
		{
			switch (_sel.x)
			{
				case InfoInputClasses.HitDie: // these don't rely on 2da-gropes ->
				case InfoInputClasses.PrimaryAbil:
				case InfoInputClasses.SpellAbil:
				case InfoInputClasses.AlignRestrict:
				case InfoInputClasses.AlignRstrctType:
				case InfoInputClasses.PlayerClass:
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
					return true;

				case InfoInputClasses.Package:
					if (Info.packageLabels.Count != 0)
						return true;
					break;

				case InfoInputClasses.icon:
				case InfoInputClasses.BorderedIcon:
					cellit_Input.Text = "Select icon ...";
					return true;

				case InfoInputClasses.AttackBonusTable:
				case InfoInputClasses.FeatsTable:
				case InfoInputClasses.SavingThrowTable:
				case InfoInputClasses.SkillsTable:
				case InfoInputClasses.BonusFeatsTable:
				case InfoInputClasses.SpellGainTable:
				case InfoInputClasses.SpellKnownTable:
				case InfoInputClasses.SpontaneousConversionTable:
				case InfoInputClasses.PreReqTable:
				case InfoInputClasses.BonusSpellcasterLevelTable:
				case InfoInputClasses.BonusCasterFeatByClassMap:
					cellit_Input    .Text = "Select 2da ...";
					cellit_Input_zip.Text = "Select Data/zip 2da ...";
					cellit_Input_zip.Visible = true;
					return true;

				case InfoInputClasses.CharGen_Chest:
				case InfoInputClasses.CharGen_Feet:
				case InfoInputClasses.CharGen_Hands:
				case InfoInputClasses.CharGen_Cloak:
				case InfoInputClasses.CharGen_Head:
					cellit_Input    .Text = "Select resref ...";
					cellit_Input_zip.Text = "Select Data/zip resref ...";
					cellit_Input_zip.Visible = true;
					return true;
			}
			return false;
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="ShowCellContext()">ShowCellContext()</see></c>.
		/// </summary>
		/// <returns><c>true</c> if the InfoInput operation will show for
		/// <c>BaseItems.2da</c>.</returns>
		bool isInfoInputcol_Baseitems()
		{
			switch (_sel.x)
			{
				case InfoInputBaseitems.EquipableSlots: // these don't rely on 2da-gropes ->
				case InfoInputBaseitems.ModelType:
				case InfoInputBaseitems.Part1EnvMap:
				case InfoInputBaseitems.Part2EnvMap:
				case InfoInputBaseitems.Part3EnvMap:
				case InfoInputBaseitems.WeaponWield:
				case InfoInputBaseitems.WeaponType:
				case InfoInputBaseitems.WeaponSize:
				case InfoInputBaseitems.StorePanel:
				case InfoInputBaseitems.AC_Enchant:
				case InfoInputBaseitems.QBBehaviour:
				case InfoInputBaseitems.CanRotateIcon:
				case InfoInputBaseitems.GenderSpecific:
				case InfoInputBaseitems.container:
					return true;

				case InfoInputBaseitems.RangedWeapon:
					if (Info.tagLabels.Count != 0)
						return true;
					break;

				case InfoInputBaseitems.InvSoundType:
					if (Info.soundLabels.Count != 0)
						return true;
					break;

				case InfoInputBaseitems.PropColumn:
					if (Info.propFields.Count != 0)
						return true;
					break;

				case InfoInputBaseitems.WeaponMatType:
					if (Info.weapsoundLabels.Count != 0)
						return true;
					break;

				case InfoInputBaseitems.AmmunitionType:
					if (Info.ammoLabels.Count != 0)
						return true;
					break;

				case InfoInputBaseitems.ItemClass:
				case InfoInputBaseitems.DefaultModel:
					cellit_Input    .Text = "Select model ...";
					cellit_Input_zip.Text = "Select Data/zip model ...";
					cellit_Input_zip.Visible = true;
					return true;
			}
			return false;
		}

		/// <summary>
		/// Checks if it's okay to print a TalkTable entry to the statusbar.
		/// </summary>
		/// <returns><c>true</c> if TalkEntry dialog will be enabled</returns>
		bool isStrrefEnabled()
		{
			if (_sel.x != 0 && Strrefheads.Contains(Table.Fields[_sel.x - 1]))
			{
				string strref = _sel.text;
				if (strref == gs.Stars) strref = "0";

				return Int32.TryParse(strref, out _strInt)
					&& _strInt >=  TalkReader.invalid
					&& _strInt <= (TalkReader.bitCusto | TalkReader.strref);
			}
			return false;
		}
		#endregion Methods (cell)


		#region Handlers (cell)
		/// <summary>
		/// Handles singlecell-click edit.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Edit"/></c></param>
		/// <param name="e"></param>
		void cellclick_Edit(object sender, EventArgs e)
		{
			Table.startCelledit(_sel);
		}

		/// <summary>
		/// Handles singlecell-click cut.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Cut"/></c></param>
		/// <param name="e"></param>
		void cellclick_Cut(object sender, EventArgs e)
		{
			cellclick_Copy(  sender, e);
			cellclick_Delete(sender, e);
		}

		/// <summary>
		/// Handles singlecell-click copy.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="cellit_Copy"/></c></item>
		/// <item><c><see cref="cellclick_Cut()">cellclick_Cut()</see></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void cellclick_Copy(object sender, EventArgs e)
		{
			_copytext = new string[,] {{ _sel.text }};

			if (_fclip != null)
				_fclip.SetCellsBufferText();
		}

		/// <summary>
		/// Handles singlecell-click paste.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Paste"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Pass this on to the Cells|Paste handler since this shall
		/// allow pasting of a contiguous block.</remarks>
		void cellclick_Paste(object sender, EventArgs e)
		{
			if (_copytext.Length == 1)
			{
				if (_sel.text != _copytext[0,0])
					Table.ChangeCellText(_sel, _copytext[0,0]);
				else
				{
					if (_sel.replaced)
						Table.ClearReplaced(_sel);

					if (_sel.loadchanged)
						Table.ClearLoadchanged(_sel);
				}
			}
			else
				editcellsclick_PasteCell(sender, e);
		}

		/// <summary>
		/// Handles singlecell-click delete.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="cellit_Clear"/></c></item>
		/// <item><c><see cref="cellclick_Cut()">cellclick_Cut()</see></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void cellclick_Delete(object sender, EventArgs e)
		{
			if (_sel.text != gs.Stars)
				Table.ChangeCellText(_sel, gs.Stars); // does not do a text-check, does Invalidate
			else
			{
				if (_sel.replaced)
					Table.ClearReplaced(_sel);

				if (_sel.loadchanged)
					Table.ClearLoadchanged(_sel);
			}
		}

		/// <summary>
		/// Handles singlecell-click lowercase.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Lower"/></c></param>
		/// <param name="e"></param>
		void cellclick_Lower(object sender, EventArgs e)
		{
			string text = _sel.text.ToLower(CultureInfo.CurrentCulture);
			if (_sel.text != text)
				Table.ChangeCellText(_sel, text); // does not do a text-check, does Invalidate
			else
			{
				if (_sel.replaced)
					Table.ClearReplaced(_sel);

				if (_sel.loadchanged)
					Table.ClearLoadchanged(_sel);
			}
		}

		/// <summary>
		/// Handles singlecell-click uppercase.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Upper"/></c></param>
		/// <param name="e"></param>
		void cellclick_Upper(object sender, EventArgs e)
		{
			string text = _sel.text.ToUpper(CultureInfo.CurrentCulture);
			if (_sel.text != text)
				Table.ChangeCellText(_sel, text); // does not do a text-check, does Invalidate
			else
			{
				if (_sel.replaced)
					Table.ClearReplaced(_sel);

				if (_sel.loadchanged)
					Table.ClearLoadchanged(_sel);
			}
		}

		/// <summary>
		/// Handles a single-cell merge operation.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_MergeCe"/></c></param>
		/// <param name="e"></param>
		void cellclick_MergeCe(object sender, EventArgs e)
		{
			YataGrid table;
			if (Table == _diff1) table = _diff2;
			else                 table = _diff1;

			int r = _sel.y;
			int c = _sel.x;

			table.ChangeCellText(table[r,c], _sel.text); // does not do a text-check, does Invalidate

			_diff1[r,c].diff =
			_diff2[r,c].diff = false;
		}

		/// <summary>
		/// Handles a single-row merge operation.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_MergeRo"/></c></param>
		/// <param name="e"></param>
		void cellclick_MergeRo(object sender, EventArgs e)
		{
			YataGrid table;
			if (Table == _diff1) table = _diff2;
			else                 table = _diff1;

			int r = _sel.y;

			// - store the row's current state to 'rPre' in the Restorable
			Restorable rest = UndoRedo.createRow(table.Rows[r]);


			int c = 0;
			for (; c != table.ColCount && c != Table.ColCount; ++c)
			{
				table[r,c].text = Table[r,c].text;
				table[r,c].diff = false;

				Table[r,c].diff = false;
			}

			if (Options._autorder)
				table[r,0].text = table[r,0].y.ToString(CultureInfo.InvariantCulture);	// not likely to happen. user'd have to load a table w/
																						// an out of order id then merge that row to another table.
			if (table.ColCount > Table.ColCount)
			{
				for (; c != table.ColCount; ++c)
				{
					table[r,c].text = gs.Stars;
					table[r,c].diff = false;
				}
			}
			else if (table.ColCount < Table.ColCount)
			{
				for (; c != Table.ColCount; ++c)
					Table[r,c].diff = false;
			}

			Table.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);


			if (!table.Changed)
			{
				table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}

			// - store the row's changed state to 'rPos' in the Restorable
			rest.rPos = table.Rows[r].Clone() as Row;
			table._ur.Push(rest);
		}


		/// <summary>
		/// Handles a select-row operation.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Selectrow"/></c></param>
		/// <param name="e"></param>
		void cellclick_Selectrow(object sender, EventArgs e)
		{
			Table.ClearSelects();

			Table.SelectRow(_sel.y);

			int invalid = YataGrid.INVALID_GRID
						| YataGrid.INVALID_FROZ
						| YataGrid.INVALID_ROWS;

			if (Table.Propanel != null && Table.Propanel.Visible)
				invalid |= YataGrid.INVALID_PROP;

			Table.Invalidator(invalid);
		}

		/// <summary>
		/// Handles a copy-rowid operation.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Copyrowid"/></c></param>
		/// <param name="e"></param>
		void cellclick_Copyrowid(object sender, EventArgs e)
		{
			ClipboardService.SetText(Table.Rows[_sel.y]._cells[0].text);

			if (_fclip != null)
				_fclip.click_Get(sender, e);
		}


		/// <summary>
		/// Handles a singlecell-click <c><see cref="InfoInputDialog"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Input"/></c></param>
		/// <param name="e"></param>
		void cellclick_InfoInput(object sender, EventArgs e)
		{
			switch (Table.Info)
			{
				case YataGrid.InfoType.INFO_SPELL:
					switch (_sel.x)
					{
						case InfoInputSpells.School: // STRING Input ->
						case InfoInputSpells.Range:
						case InfoInputSpells.Vs:
						case InfoInputSpells.ConjAnim:
						case InfoInputSpells.CastAnim:
						case InfoInputSpells.Proj:
						case InfoInputSpells.ProjType:
						case InfoInputSpells.ProjSpwnPoint:
						case InfoInputSpells.ProjOrientation:
						case InfoInputSpells.ImmunityType:
						case InfoInputSpells.ItemImmunity:
						case InfoInputSpells.UserType:
						case InfoInputSpells.UseConcentration:
						case InfoInputSpells.SpontaneouslyCast:
						case InfoInputSpells.HostileSetting:
						case InfoInputSpells.HasProjectile:
						case InfoInputSpells.CastableOnDead:
						case InfoInputSpells.Removed:
							using (var iis = new InfoInputSpells(this, _sel))
							{
								if (iis.ShowDialog(this) == DialogResult.OK
									&& str1 != str0)
								{
									Table.ChangeCellText(_sel, str1); // does not do a text-check
								}
							}
							break;

						case InfoInputSpells.MetaMagic: // HEX Input ->
						case InfoInputSpells.TargetType:
						case InfoInputSpells.AsMetaMagic:
							using (var iis = new InfoInputSpells(this, _sel))
							{
								if (iis.ShowDialog(this) == DialogResult.OK
									&& int1 != int0)
								{
									string text;
									if (int1 != Info_ASSIGN_STARS)
									{
										string f;
										if (int1 > 0xFF) f = "X6"; // is MetaMagic (invocation)
										else             f = "X2"; // is MetaMagic (standard) or TargetType

										text = "0x" + int1.ToString(f, CultureInfo.InvariantCulture);
									}
									else
										text = gs.Stars;

									Table.ChangeCellText(_sel, text); // does not do a text-check
								}
							}
							break;

						case InfoInputSpells.Category: // INT Input ->
						case InfoInputSpells.SpontCastClassReq:
						case InfoInputSpells.TargetingUI:
							doIntInputSpells();
							break;

						// File input ->
						case InfoInputSpells.IconResRef:
							SelectFile(" Select icon", Yata.GetFileFilter("tga"), ".tga");
							break;

						case InfoInputSpells.ImpactScript:
							SelectFile(" Select script", Yata.GetFileFilter("nss"), ".nss");
							break;

						case InfoInputSpells.ConjVisual0:
						case InfoInputSpells.LowConjVisual0:
						case InfoInputSpells.ConjVisual1:
						case InfoInputSpells.ConjVisual2:
						case InfoInputSpells.CastVisual0:
						case InfoInputSpells.LowCastVisual0:
						case InfoInputSpells.CastVisual1:
						case InfoInputSpells.CastVisual2:
						case InfoInputSpells.ProjSEF:
						case InfoInputSpells.LowProjSEF:
						case InfoInputSpells.ImpactSEF:
						case InfoInputSpells.LowImpactSEF:
							SelectFile(" Select specialeffect", Yata.GetFileFilter("sef"), ".sef");
							break;

						case InfoInputSpells.ConjSoundVFX:
						case InfoInputSpells.ConjSoundMale:
						case InfoInputSpells.ConjSoundFemale:
						case InfoInputSpells.CastSound:
						case InfoInputSpells.ProjSound:
							SelectFile(" Select sound", Yata.GetFileFilter("wav"), ".wav");
							break;

						case InfoInputSpells.ProjModel:
							SelectFile(" Select model", Yata.GetFileFilter("mdb"), ".mdb", true);
							break;
					}
					break;

				case YataGrid.InfoType.INFO_FEAT:
					switch (_sel.x)
					{
						case InfoInputFeat.Category: // INT Input ->
						case InfoInputFeat.MasterFeat:
						case InfoInputFeat.REQSKILL:
						case InfoInputFeat.REQSKILL2:
						case InfoInputFeat.MinLevelClass:
						case InfoInputFeat.ToggleMode:
							doIntInputFeat();
							break;

						case InfoInputFeat.ToolsCategories: // STRING Input ->
						case InfoInputFeat.FeatCategory:
						case InfoInputFeat.GAINMULTIPLE:
						case InfoInputFeat.EFFECTSSTACK:
						case InfoInputFeat.ALLCLASSESCANUSE:
						case InfoInputFeat.TARGETSELF:
						case InfoInputFeat.HostileFeat:
						case InfoInputFeat.PreReqEpic:
						case InfoInputFeat.IsActive:
						case InfoInputFeat.IsPersistent:
						case InfoInputFeat.DMFeat:
						case InfoInputFeat.REMOVED:
						case InfoInputFeat.ImmunityType:
						case InfoInputFeat.Instant:
							using (var iif = new InfoInputFeat(this, _sel))
							{
								if (iif.ShowDialog(this) == DialogResult.OK
									&& str1 != str0)
								{
									Table.ChangeCellText(_sel, str1); // does not do a text-check
								}
							}
							break;

						// File input ->
						case InfoInputFeat.icon:
							SelectFile(" Select icon", Yata.GetFileFilter("tga"), ".tga");
							break;
					}
					break;

				case YataGrid.InfoType.INFO_CLASS:
					switch (_sel.x)
					{
						case InfoInputClasses.HitDie: // STRING Input ->
						case InfoInputClasses.PrimaryAbil:
						case InfoInputClasses.SpellAbil:
						case InfoInputClasses.PlayerClass:
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
							using (var iic = new InfoInputClasses(this, _sel))
							{
								if (iic.ShowDialog(this) == DialogResult.OK
									&& str1 != str0)
								{
									Table.ChangeCellText(_sel, str1); // does not do a text-check
								}
							}
							break;

						case InfoInputClasses.AlignRestrict: // HEX Input ->
						case InfoInputClasses.AlignRstrctType:
							using (var iis = new InfoInputClasses(this, _sel))
							{
								if (iis.ShowDialog(this) == DialogResult.OK
									&& int1 != int0)
								{
									string text;
									if (int1 != Info_ASSIGN_STARS)
									{
										string f;
										if (_sel.x == InfoInputClasses.AlignRestrict) f = "X2";
										else                                          f = "X1"; // _sel.x == InfoInputClasses.AlignRstrctType

										text = "0x" + int1.ToString(f, CultureInfo.InvariantCulture);
									}
									else
										text = gs.Stars;

									Table.ChangeCellText(_sel, text); // does not do a text-check
								}
							}
							break;

						case InfoInputClasses.Package: // INT Input ->
							doIntInputClass();
							break;

						// File input ->
						case InfoInputClasses.icon:
						case InfoInputClasses.BorderedIcon:
							SelectFile(" Select icon", Yata.GetFileFilter("tga"), ".tga");
							break;

						case InfoInputClasses.AttackBonusTable:
						case InfoInputClasses.FeatsTable:
						case InfoInputClasses.SavingThrowTable:
						case InfoInputClasses.SkillsTable:
						case InfoInputClasses.BonusFeatsTable:
						case InfoInputClasses.SpellGainTable:
						case InfoInputClasses.SpellKnownTable:
						case InfoInputClasses.SpontaneousConversionTable:
						case InfoInputClasses.PreReqTable:
						case InfoInputClasses.BonusSpellcasterLevelTable:
						case InfoInputClasses.BonusCasterFeatByClassMap:
							SelectFile(" Select 2da", Yata.GetFileFilter("2da"), ".2da");
							break;

						case InfoInputClasses.CharGen_Chest:
						case InfoInputClasses.CharGen_Feet:
						case InfoInputClasses.CharGen_Hands:
						case InfoInputClasses.CharGen_Cloak:
						case InfoInputClasses.CharGen_Head:
							SelectFile(" Select resref", Yata.GetFileFilter("uti"), ".uti");
							break;
					}
					break;

				case YataGrid.InfoType.INFO_ITEM:
					switch (_sel.x)
					{
						case InfoInputBaseitems.EquipableSlots: // HEX Input ->
							using (var iis = new InfoInputBaseitems(this, _sel))
							{
								if (iis.ShowDialog(this) == DialogResult.OK
									&& int1 != int0)
								{
									string text;
									if (int1 != Info_ASSIGN_STARS)
									{
										text = "0x" + int1.ToString("X5", CultureInfo.InvariantCulture);
									}
									else
										text = gs.Stars;

									Table.ChangeCellText(_sel, text); // does not do a text-check
								}
							}
							break;

						case InfoInputBaseitems.ModelType: // INT Input ->
						case InfoInputBaseitems.Part1EnvMap:
						case InfoInputBaseitems.Part2EnvMap:
						case InfoInputBaseitems.Part3EnvMap:
						case InfoInputBaseitems.WeaponWield:
						case InfoInputBaseitems.WeaponType:
						case InfoInputBaseitems.WeaponSize:
						case InfoInputBaseitems.RangedWeapon:
						case InfoInputBaseitems.InvSoundType:
						case InfoInputBaseitems.PropColumn:
						case InfoInputBaseitems.StorePanel:
						case InfoInputBaseitems.AC_Enchant:
						case InfoInputBaseitems.WeaponMatType:
						case InfoInputBaseitems.AmmunitionType:
						case InfoInputBaseitems.QBBehaviour:
							doIntInputItem();
							break;

						case InfoInputBaseitems.CanRotateIcon: // STRING Input ->
						case InfoInputBaseitems.GenderSpecific:
						case InfoInputBaseitems.container:
							using (var iic = new InfoInputBaseitems(this, _sel))
							{
								if (iic.ShowDialog(this) == DialogResult.OK
									&& str1 != str0)
								{
									Table.ChangeCellText(_sel, str1); // does not do a text-check
								}
							}
							break;

						// File input ->
						case InfoInputBaseitems.ItemClass:
						case InfoInputBaseitems.DefaultModel:
							SelectFile(" Select model", Yata.GetFileFilter("mdb"), ".mdb", true);
							break;
					}
					break;
			}
		}

		/// <summary>
		/// Invokes an <c>OpenFileDialog</c> to select and insert a file-label
		/// as the text in <c><see cref="_sel"/></c>.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="filter"></param>
		/// <param name="default"></param>
		/// <param name="base"><c>true</c> to deal with arbitrary trailing
		/// digits</param>
		void SelectFile(string title, string filter, string @default, bool @base = false)
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.Title  = title;
				ofd.Filter = filter;

				ofd.AutoUpgradeEnabled = false;

				string fe;
				if (_sel.text == gs.Stars)
				{
					fe = "*" + @default;
				}
				else if (!_sel.text.EndsWith(@default, StringComparison.InvariantCultureIgnoreCase))
				{
//					if (@base) fe = _sel.text + "*" + @default; // no good. An asterisk here masks out all .MDB files in the dialog.
//					else
					fe = _sel.text + @default;
				}
				else
					fe = _sel.text;

				ofd.FileName = Path.Combine(GetCurrentDirectory(), fe);


				if (ofd.ShowDialog() == DialogResult.OK)
				{
					string label = Path.GetFileNameWithoutExtension(ofd.FileName);

					if (@base)
					{
						for (int i = 0; i != label.Length; ++i)
						{
							if (Char.IsDigit(label[i]))
							{
								label = label.Substring(0, i);
								break;
							}
						}
					}

					if (label != _sel.text)
					{
						YataGrid.VerifyFileLabel(ref label); // check whitespace and add outer quotes if necessary
						Table.ChangeCellText(_sel, label); // does not do a text-check
					}
				}
			}
		}

		/// <summary>
		/// Handles a singlecell-click for the user to select a file-label
		/// that's in a NwN2 Data/zip file.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Input_zip"/></c></param>
		/// <param name="e"></param>
		void cellclick_InfoInput_zip(object sender, EventArgs e)
		{
			switch (Table.Info)
			{
				case YataGrid.InfoType.INFO_SPELL:
					switch (_sel.x)
					{
						case InfoInputSpells.ImpactScript:
							SelectFile(" Select Data/zip script archive");
							break;

						case InfoInputSpells.ConjVisual0:
						case InfoInputSpells.LowConjVisual0:
						case InfoInputSpells.ConjVisual1:
						case InfoInputSpells.ConjVisual2:
						case InfoInputSpells.CastVisual0:
						case InfoInputSpells.LowCastVisual0:
						case InfoInputSpells.CastVisual1:
						case InfoInputSpells.CastVisual2:
						case InfoInputSpells.ProjSEF:
						case InfoInputSpells.LowProjSEF:
						case InfoInputSpells.ImpactSEF:
						case InfoInputSpells.LowImpactSEF:
							SelectFile(" Select Data/zip specialeffect archive");
							break;

						case InfoInputSpells.ConjSoundVFX:
						case InfoInputSpells.ConjSoundMale:
						case InfoInputSpells.ConjSoundFemale:
						case InfoInputSpells.CastSound:
						case InfoInputSpells.ProjSound:
							SelectFile(" Select Data/zip sound archive");
							break;

						case InfoInputSpells.ProjModel:
							SelectFile(" Select Data/zip model archive", true);
							break;
					}
					break;

				case YataGrid.InfoType.INFO_CLASS:
					switch (_sel.x)
					{
						case InfoInputClasses.AttackBonusTable:
						case InfoInputClasses.FeatsTable:
						case InfoInputClasses.SavingThrowTable:
						case InfoInputClasses.SkillsTable:
						case InfoInputClasses.BonusFeatsTable:
						case InfoInputClasses.SpellGainTable:
						case InfoInputClasses.SpellKnownTable:
						case InfoInputClasses.SpontaneousConversionTable:
						case InfoInputClasses.PreReqTable:
						case InfoInputClasses.BonusSpellcasterLevelTable:
						case InfoInputClasses.BonusCasterFeatByClassMap:
							SelectFile(" Select Data/zip 2da archive");
							break;

						case InfoInputClasses.CharGen_Chest:
						case InfoInputClasses.CharGen_Feet:
						case InfoInputClasses.CharGen_Hands:
						case InfoInputClasses.CharGen_Cloak:
						case InfoInputClasses.CharGen_Head:
							SelectFile(" Select Data/zip resref archive");
							break;
					}
					break;

				case YataGrid.InfoType.INFO_ITEM:
					switch (_sel.x)
					{
						case InfoInputBaseitems.ItemClass:
						case InfoInputBaseitems.DefaultModel:
							SelectFile(" Select Data/zip model archive", true);
							break;
					}
					break;
			}
		}

		/// <summary>
		/// Invokes an <c>OpenFileDialog</c> for the user to insert a zipped
		/// file-label into the <c><see cref="Cell.text">Cell.text</see></c> of
		/// <c><see cref="_sel">_sel</see></c>.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="base"></param>
		/// <remarks>This is waranteed only for the Zipfiles in the NwN2 /Data
		/// folder.</remarks>
		void SelectFile(string title, bool @base = false)
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.Title  = title;
				ofd.Filter = Yata.GetFileFilter("zip");

				ofd.AutoUpgradeEnabled = false;

				string dir;
				if (Directory.Exists(Options._pathzipdata))
				{
					dir = Options._pathzipdata;
					ofd.RestoreDirectory = true;	// while this restores the app's CurrentDirectory it does
				}									// not restore the directory in the registry ComDlg32 MRU - oh well
				else
					dir = GetCurrentDirectory();

				ofd.FileName = Path.Combine(dir, "*.zip");


				if (ofd.ShowDialog() == DialogResult.OK)
				{
					using (var dzld = new DataZipListDialog(this, ofd.FileName))
					{
						if (dzld.ShowDialog() == DialogResult.OK)
						{
							string label = dzld.GetSelectedFile();

							if (@base)
							{
								for (int i = 0; i != label.Length; ++i)
								{
									if (Char.IsDigit(label[i]))
									{
										label = label.Substring(0, i);
										break;
									}
								}
							}

							if (label != _sel.text)
							{
								YataGrid.VerifyFileLabel(ref label); // check whitespace and add outer quotes if necessary
								Table.ChangeCellText(_sel, label); // does not do a text-check
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="cellclick_InfoInput()">cellclick_InfoInput()</see></c>
		/// </summary>
		void doIntInputSpells()
		{
			using (var iis = new InfoInputSpells(this, _sel))
				doIntInput(iis);
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="cellclick_InfoInput()">cellclick_InfoInput()</see></c>
		/// </summary>
		void doIntInputFeat()
		{
			using (var iif = new InfoInputFeat(this, _sel))
				doIntInput(iif);
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="cellclick_InfoInput()">cellclick_InfoInput()</see></c>
		/// </summary>
		void doIntInputClass()
		{
			using (var iic = new InfoInputClasses(this, _sel))
				doIntInput(iic);
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="cellclick_InfoInput()">cellclick_InfoInput()</see></c>
		/// </summary>
		void doIntInputItem()
		{
			using (var iic = new InfoInputBaseitems(this, _sel))
				doIntInput(iic);
		}

		/// <summary>
		/// Shows an <c><see cref="InfoInputDialog"/></c> and handles its
		/// return.
		/// </summary>
		/// <param name="dialog"></param>
		/// <remarks>- helper for
		/// <list type="bullet">
		/// <item><c><see cref="doIntInputSpells()">doIntInputSpells()</see></c></item>
		/// <item><c><see cref="doIntInputFeat()">doIntInputFeat()</see></c></item>
		/// </list></remarks>
		void doIntInput(Form dialog)
		{
			if (dialog.ShowDialog(this) == DialogResult.OK
				&& int1 != int0)
			{
				string text;
				if (int1 != Info_ASSIGN_STARS) text = int1.ToString(CultureInfo.InvariantCulture);
				else                           text = gs.Stars;

				Table.ChangeCellText(_sel, text); // does not do a text-check
			}
		}


		/// <summary>
		/// Handler for the singlecell-context's subit "STRREF"
		/// <c>DropDownOpening</c> event.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Strref"/></c></param>
		/// <param name="e"></param>
		void dropdownopening_Strref(object sender, EventArgs e)
		{
			bool invalid = _strInt == TalkReader.invalid;

			if (invalid || (_strInt & TalkReader.bitCusto) == 0)
				cellit_Strref_custom.Text = "set Custom";
			else
				cellit_Strref_custom.Text = "clear Custom";

			cellit_Strref_custom .Enabled =
			cellit_Strref_invalid.Enabled = !Table.Readonly && !invalid;
		}

		/// <summary>
		/// Handler for singlecell-context "STRREF" click. Opens
		/// <c><see cref="TalkDialog"/></c> that displays the text's
		/// corresponding Dialog.Tlk or special entry in a readonly
		/// <c>RichTextBox</c> for the user's investigation and/or copying.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Strref_talktable"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Check that the cell's text parses to a valid value before
		/// allowing the event to trigger else disable the context it.</remarks>
		/// <seealso cref="ShowCellContext()"><c>ShowCellContext()</c></seealso>
		/// <seealso cref="dropdownopening_Strref()"><c>dropdownopening_Strref()</c></seealso>
		void cellclick_Strref_talktable(object sender, EventArgs e)
		{
			_strref = _sel.text;

			using (var td = new TalkDialog(_sel, this))
			{
				if (td.ShowDialog(this) == DialogResult.OK
					&& _strref != _sel.text)
				{
					Table.ChangeCellText(_sel, _strref); // does not do a text-check
					Invalidate();	// lolziMScopter - else the titlebar and borders can arbitrarily disappear.
				}					// nobody knows why ... q TwilightZone
			}
		}

		/// <summary>
		/// Handler for singlecell-context "set/clear Custom" click. Toggles the
		/// custom-bit flag.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Strref_custom"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Check that the cell's text parses to a valid value before
		/// allowing the event to trigger else disable the context it.
		/// 
		/// 
		/// The invalid strref (-1) cannot be toggled.
		/// </remarks>
		/// <seealso cref="ShowCellContext()"><c>ShowCellContext()</c></seealso>
		/// <seealso cref="dropdownopening_Strref()"><c>dropdownopening_Strref()</c></seealso>
		void cellclick_Strref_custom(object sender, EventArgs e)
		{
			if ((_strInt & TalkReader.bitCusto) != 0)
			{
				_strInt &= TalkReader.strref;	// knocks out 'bitCusto' and any MSB errors
			}
			else
			{
				_strInt &= TalkReader.strref;	// knocks out any MSB errors
				_strInt |= TalkReader.bitCusto;	// flags 'bitCusto'
			}

			Table.ChangeCellText(_sel, _strInt.ToString(CultureInfo.InvariantCulture)); // does not do a text-check
		}

		/// <summary>
		/// Handler for singlecell-context "set Invalid (-1)" click. Sets a
		/// strref to <c>-1</c> if not already.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Strref_invalid"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Check that the cell's text parses to a valid value before
		/// allowing the event to trigger else disable the context it.</remarks>
		/// <seealso cref="ShowCellContext()"><c>ShowCellContext()</c></seealso>
		/// <seealso cref="dropdownopening_Strref()"><c>dropdownopening_Strref()</c></seealso>
		void cellclick_Strref_invalid(object sender, EventArgs e)
		{
			Table.ChangeCellText(_sel, gs.Invalid); // does not do a text-check
		}
		#endregion Handlers (cell)
	}
}
