using System;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
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

			cellit_Edit   .Enabled = !Table.Readonly;

			cellit_Cut    .Enabled = !Table.Readonly;
			cellit_Paste  .Enabled = !Table.Readonly;
			cellit_Clear  .Enabled = !Table.Readonly && (_sel.text != gs.Stars || _sel.loadchanged);

			cellit_Lower  .Enabled = !Table.Readonly && (_sel.text != _sel.text.ToLower(CultureInfo.CurrentCulture) || _sel.loadchanged);
			cellit_Upper  .Enabled = !Table.Readonly && (_sel.text != _sel.text.ToUpper(CultureInfo.CurrentCulture) || _sel.loadchanged);

			cellit_MergeCe.Enabled =
			cellit_MergeRo.Enabled = isMergeEnabled();

			cellit_Strref .Enabled = isStrrefEnabled();

			switch (Table.Info)
			{
				case YataGrid.InfoType.INFO_NONE:
				case YataGrid.InfoType.INFO_CRAFT:
					cellit_Input.Visible =
					cellit_Input.Enabled = false;
					break;

				// TODO: If table is Readonly allow viewing the InfoInput dialog
				// but disable its controls ->

				case YataGrid.InfoType.INFO_SPELL:
					cellit_Input.Text    = "InfoInput (spells.2da)";
					cellit_Input.Visible = true;
					cellit_Input.Enabled = !Table.Readonly && isSpellsInfoInputCol();
					break;

				case YataGrid.InfoType.INFO_FEAT:
					cellit_Input.Text    = "InfoInput (feat.2da)";
					cellit_Input.Visible = true;
					cellit_Input.Enabled = !Table.Readonly && isFeatInfoInputCol();
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
		/// Spells.2da.</returns>
		bool isSpellsInfoInputCol()
		{
			switch (_sel.x)
			{
				case InfoInputSpells.School: // these don't rely on 2da-gropes ->
				case InfoInputSpells.Range:
				case InfoInputSpells.MetaMagic:
				case InfoInputSpells.TargetType:
				case InfoInputSpells.ImmunityType:
				case InfoInputSpells.UserType:
				case InfoInputSpells.AsMetaMagic:
					return true;

				case InfoInputSpells.Category:
					if (Info.categoryLabels.Count != 0)
						return true;
					break;

				case InfoInputSpells.SpontCastClass:
					if (Info.classLabels.Count != 0)
						return true;
					break;

				case InfoInputSpells.TargetingUI:
					if (Info.targetLabels.Count != 0)
						return true;
					break;
			}
			return false;
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="ShowCellContext()">ShowCellContext()</see></c>.
		/// </summary>
		/// <returns><c>true</c> if the InfoInput operation will show for
		/// Feat.2da.</returns>
		bool isFeatInfoInputCol()
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

				case InfoInputFeat.ToolsCategories: // this doesn't rely on 2da-gropes ->
					return true;

				case InfoInputFeat.ToggleMode:
					if (Info.combatmodeLabels.Count != 0)
						return true;
					break;
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
		/// <item><c><see cref="cellit_Cut"/></c></item>
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
				else if (_sel.loadchanged)
					Table.ClearLoadchanged(_sel);
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
		/// <item><c><see cref="cellit_Cut"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void cellclick_Delete(object sender, EventArgs e)
		{
			if (_sel.text != gs.Stars)
				Table.ChangeCellText(_sel, gs.Stars); // does not do a text-check, does Invalidate
			else if (_sel.loadchanged)
				Table.ClearLoadchanged(_sel);
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
			else if (_sel.loadchanged)
				Table.ClearLoadchanged(_sel);
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
			else if (_sel.loadchanged)
				Table.ClearLoadchanged(_sel);
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

			Cell cell = table[r,c];
			table.ChangeCellText(cell, _sel.text); // does not do a text-check, does Invalidate

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

			if (Settings._autorder)
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

			// TODO: test if this funct needs to re-width a bunch of stuff


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
		/// Handles singlecell-click InfoInput dialog.
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
						case InfoInputSpells.ImmunityType:
						case InfoInputSpells.UserType:
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
									if (int1 == II_ASSIGN_STARS)
									{
										Table.ChangeCellText(_sel, gs.Stars); // does not do a text-check
									}
									else
									{
										string q;
										if (int1 > 0xFF) q = "X6"; // is MetaMagic (invocation)
										else             q = "X2"; // is MetaMagic (standard) or TargetType

										Table.ChangeCellText(_sel, "0x" + int1.ToString(q, CultureInfo.InvariantCulture)); // does not do a text-check
									}
								}
							}
							break;

						case InfoInputSpells.Category: // INT Input ->
						case InfoInputSpells.SpontCastClass:
						case InfoInputSpells.TargetingUI:
							doIntInputSpells();
							break;
					}
					break;

				case YataGrid.InfoType.INFO_FEAT:
					switch (_sel.x)
					{
						case InfoInputFeat.Category: // INT Input ->
						case InfoInputFeat.MasterFeat:
						case InfoInputFeat.ToggleMode:
							doIntInputFeat();
							break;

						case InfoInputFeat.ToolsCategories:
							using (var iif = new InfoInputFeat(this, _sel))
							{
								if (iif.ShowDialog(this) == DialogResult.OK
									&& str1 != str0)
								{
									Table.ChangeCellText(_sel, str1); // does not do a text-check
								}
							}
							break;
					}
					break;
			}
		}

		/// <summary>
		/// - helper for
		/// <c><see cref="cellclick_InfoInput()">cellclick_InfoInput()</see></c>
		/// </summary>
		void doIntInputSpells()
		{
			using (var iis = new InfoInputSpells(this, _sel))
				doIntInput(iis);
		}

		/// <summary>
		/// - helper for
		/// <c><see cref="cellclick_InfoInput()">cellclick_InfoInput()</see></c>
		/// </summary>
		void doIntInputFeat()
		{
			using (var iif = new InfoInputFeat(this, _sel))
				doIntInput(iif);
		}

		/// <summary>
		/// Shows an InfoInput dialog and handles its return.
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
				string val;
				if (int1 == II_ASSIGN_STARS) val = gs.Stars;
				else                         val = int1.ToString(CultureInfo.InvariantCulture);

				Table.ChangeCellText(_sel, val); // does not do a text-check
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
