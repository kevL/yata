using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// An <c><see cref="InfoInputDialog"/></c> to input specific data for
	/// <c>BaseItems.2da</c>.
	/// </summary>
	/// <seealso cref="InfoInputSpells"><c>InfoInputSpells</c></seealso>
	sealed partial class InfoInputBaseitems
		: InfoInputDialog
	{
		#region Fields (static)
		// cols in BaseItems.2da ->
		internal const int EquipableSlots     =  5;
		internal const int ModelType          =  7;
		internal const int ItemClass          =  9; // ofd only (OpenFileDialog)
		internal const int DefaultModel       = 14; // ofd only
//		internal const int NWN2_DefaultIcon   = 15; // req. images i guess
		internal const int WeaponWield        = 18;
		internal const int WeaponType         = 19;
		internal const int WeaponSize         = 20;
		internal const int RangedWeapon       = 21; // baseitems.2da
//		internal const int Category           = 29; // TODO <-
		internal const int InvSoundType       = 34; // inventorysnds.2da
		internal const int PropColumn         = 37; // itemprops.2da
		internal const int StorePanel         = 38;

		internal const int ReqFeat0           = 39; // feat.2da ->
		internal const int ReqFeat1           = 40;
		internal const int ReqFeat2           = 41;
		internal const int ReqFeat3           = 42;
		internal const int ReqFeat4           = 43;
		internal const int ReqFeat5           = 44;

		internal const int AC_Enchant         = 45;

		internal const int BaseAC             = 46; // info: "shields only"
		internal const int ArmorCheckPen      = 47; // info: "shields only"

//		internal const int BaseItemStatRef    = 48; // is this applicable to shields/armor

		internal const int WeaponMatType      = 52; // weaponsounds.2da
		internal const int AmmunitionType     = 53; // index+1 into ammunitiontypes.2da
		internal const int QBBehaviour        = 54;

		internal const int ArcaneSpellFailure = 55; // info: "shields only"

		internal const int FEATImprCrit       = 61; // feat.2da ->
		internal const int FEATWpnFocus       = 62;
		internal const int FEATWpnSpec        = 63;
		internal const int FEATEpicDevCrit    = 64;
		internal const int FEATEpicWpnFocus   = 65;
		internal const int FEATEpicWpnSpec    = 66;
		internal const int FEATOverWhCrit     = 67;
		internal const int FEATWpnOfChoice    = 68;
		internal const int FEATGrtrWpnFocus   = 69;
		internal const int FEATGrtrWpnSpec    = 70;
		internal const int FEATPowerCrit      = 71;

//		internal const int GMaterialType      = 72; // Material Type.2da - doesn't exist. not used.
		#endregion Fields (static)


		#region cTor
		/// <summary>
		/// A dialog for the user to input <c>BaseItems.2da</c> info.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="cell"></param>
		internal InfoInputBaseitems(Yata f, Cell cell)
		{
			_f    = f;		// don't try to pass these to a InfoInputDialog.cTor
			_cell = cell;	// because the designer will scream blue murder.

			InitializeComponent();

			Text = pad + Yata.Table.Cols[_cell.x].text;

//			_title = Yata.Table.Cols[_cell.x].text;
//			int border = (Width - ClientSize.Width) / 2;
//			_hTitle = Height - ClientSize.Height - border;
//			_width  = Width;

			// NOTE: Don't bother inheriting from YataDialog since setting the
			// font is the only benefit ->
			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			init();
		}
		#endregion cTor


		#region init
		/// <summary>
		/// Initializes the dialog based on the current 2da col.
		/// </summary>
		void init()
		{
			_init = true;

			string val = _cell.text;

			int result;
			switch (_cell.x)
			{
				case EquipableSlots: // int-val(hex),checkbox,multiple
					// NOTE: Types that bitwise multiple values shall assign
					// a default value of "0x00" instead of the usual "****"
					// when the value passed into this dialog is considered
					// invalid.

					prep_EquipableSlots();

					if (val.Length > 2 && val.Substring(0,2) == "0x"
						&& Int32.TryParse(val.Substring(2),
										  NumberStyles.AllowHexSpecifier, // <- that treats the string as hexadecimal notatation
										  CultureInfo.InvariantCulture,   //    but does *not* allow the hex-specifier "0x"
										  out result))
					{
						cb_00.Checked = ((result & Yata.EQUIPSLOT_HELMET)   != 0);
						cb_01.Checked = ((result & Yata.EQUIPSLOT_ARMOR)    != 0);
						cb_02.Checked = ((result & Yata.EQUIPSLOT_BOOTS)    != 0);
						cb_03.Checked = ((result & Yata.EQUIPSLOT_GLOVES)   != 0);
						cb_04.Checked = ((result & Yata.EQUIPSLOT_MAINHAND) != 0);
						cb_05.Checked = ((result & Yata.EQUIPSLOT_OFFHAND)  != 0);
						cb_06.Checked = ((result & Yata.EQUIPSLOT_CLOAK)    != 0);
						cb_07.Checked = ((result & Yata.EQUIPSLOT_RINGS)    != 0);
						cb_08.Checked = ((result & Yata.EQUIPSLOT_AMULET)   != 0);
						cb_09.Checked = ((result & Yata.EQUIPSLOT_BELT)     != 0);
						cb_10.Checked = ((result & Yata.EQUIPSLOT_ARROW)    != 0);
						cb_11.Checked = ((result & Yata.EQUIPSLOT_BULLET)   != 0);
						cb_12.Checked = ((result & Yata.EQUIPSLOT_BOLT)     != 0);
						cb_13.Checked = ((result & Yata.EQUIPSLOT_CWEAPON)  != 0);
						cb_14.Checked = ((result & Yata.EQUIPSLOT_CARMOR)   != 0);

						if ((result & ~Yata.EQUIPSLOTS_TOTAL) != 0) // invalid - bits outside allowed range
						{
							_f.int0 = result;
							printHexString(_f.int1 = (result &= Yata.ALIGNRESTRICT_TOTAL));
						}
						else
							printHexString(_f.int0 = _f.int1 = result);

						bu_Clear.Enabled = (result != 0);
					}
					else // is not a valid hex-value ->
					{
						_f.int0 = Yata.Info_INIT_INVALID;
						printHexString(_f.int1 = 0);
						bu_Clear.Enabled = false;
					}
					break;

				case ModelType: // int-val,dropdown,unique
					list_ModelTypes();
					initintvals(val, co_Val, bu_Clear);
					break;

				case WeaponWield: // int-val,dropdown,unique
					list_WeaponWields();
					initintvals(val, co_Val, bu_Clear);
					break;

				case WeaponType: // int-val,dropdown,unique
					list_WeaponTypes();
					initintvals(val, co_Val, bu_Clear);
					break;

				case WeaponSize: // int-val,dropdown,unique
					list_WeaponSizes();
					initintvals(val, co_Val, bu_Clear);
					break;

				case RangedWeapon: // int-val,dropdown,unique
					list_RangedWeapons();
					initintvals(val, co_Val, bu_Clear);
					break;

				case InvSoundType: // int-val,dropdown,unique
					list_InventorySounds();
					initintvals(val, co_Val, bu_Clear);
					break;

				case PropColumn: // int-val,dropdown,unique
					list_PropertyCols();
					initintvals(val, co_Val, bu_Clear);
					break;

				case StorePanel:
					list_StorePanels();
					initintvals(val, co_Val, bu_Clear);
					break;

				case AC_Enchant:
					list_AcEnchants();
					initintvals(val, co_Val, bu_Clear);
					break;

				case WeaponMatType:
					list_WeaponMatTypes();
					initintvals(val, co_Val, bu_Clear);
					break;

				case AmmunitionType:
					list_AmmunitionTypes();
					initintvals_1(val, co_Val, bu_Clear); // off by 1
					break;

				case QBBehaviour:
					list_QBBehaviours();
					initintvals(val, co_Val, bu_Clear);
					break;
			}

			_init = false;
		}


		/// <summary>
		/// Prepares this dialog for <c><see cref="EquipableSlots"/></c> input.
		/// </summary>
		void prep_EquipableSlots()
		{
			cb_00.Text = "(1)head";
			cb_01.Text = "(2)chest";
			cb_02.Text = "(4)feet";
			cb_03.Text = "(8)arms";
			cb_04.Text = "(16)righthand";
			cb_05.Text = "(32)lefthand";
			cb_06.Text = "(64)back";
			cb_07.Text = "(384)fingers";
			cb_08.Text = "(512)neck";
			cb_09.Text = "(1024)waist";
			cb_10.Text = "(2048)arrow";
			cb_11.Text = "(4096)bullet";
			cb_12.Text = "(8192)bolt";
			cb_13.Text = "(114688)Cweapon";
			cb_14.Text = "(131072)Carmor";

			cb_00.Visible = cb_01.Visible = cb_02.Visible = cb_03.Visible =
			cb_04.Visible = cb_05.Visible = cb_06.Visible = cb_07.Visible =
			cb_08.Visible = cb_09.Visible = cb_10.Visible = cb_11.Visible =
			cb_12.Visible = cb_13.Visible = cb_14.Visible = true;
		}


		/// <summary>
		/// Hides the label and shows the <c>ComboBox</c> for dropdown-lists
		/// instead.
		/// </summary>
		void dropdown()
		{
			la_Val.Visible = false;
			co_Val.Visible = true;

			ClientSize = new Size(ClientSize.Width,
								  ClientSize.Height - 20 * 5);
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="ModelType"/></c> to the
		/// <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_ModelTypes()
		{
			dropdown();

			co_Val.Items.AddRange(new []
			{
				new tui("0 - simple 1-part"),
				new tui("1 - colored 1-part"),
				new tui("2 - configurable 3-part"),
				new tui("3 - armor"),
				new tui(gs.Stars)
			});
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="WeaponWield"/></c> to the
		/// <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_WeaponWields()
		{
			dropdown();

			co_Val.Items.AddRange(new []
			{
				new tui("0 - standard one-handed weapon"),
				new tui("1 - not wieldable"),
				new tui("2 - not used/unknown"),
				new tui("3 - not used/unknown"),
				new tui("4 - two-handed weapon"),
				new tui("5 - bow"),
				new tui("6 - crossbow"),
				new tui("7 - shield"),
				new tui("8 - double-sided weapon"),
				new tui("9 - creature weapon"),
				new tui("10 - dart or sling"),
				new tui("11 - shuriken or throwing axe"),
				new tui("12 - spears"),
				new tui("13 - musical instruments"),
				new tui(gs.Stars)
			});
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="WeaponType"/></c> to the
		/// <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_WeaponTypes()
		{
			dropdown();

			co_Val.Items.AddRange(new []
			{
				new tui("0 - none"),
				new tui("1 - piercing"),
				new tui("2 - bludgeoning"),
				new tui("3 - slashing"),
				new tui("4 - piercing/slashing"),
				new tui("5 - bludgeoning/piercing"),
				new tui(gs.Stars)
			});
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="WeaponSize"/></c> to the
		/// <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_WeaponSizes()
		{
			dropdown();

			co_Val.Items.AddRange(new []
			{
				new tui("0 - none"),
				new tui("1 - tiny"),
				new tui("2 - small"),
				new tui("3 - medium"),
				new tui("4 - large"),
				new tui(gs.Stars)
			});
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="RangedWeapon"/></c>
		/// (BaseItems.2da) to the <c>ComboBox</c> along with a final stars
		/// item.
		/// </summary>
		void list_RangedWeapons()
		{
			dropdown();

			for (int i = 0; i != Info.tagLabels.Count; ++i)
			{
				co_Val.Items.Add(new tui(i + " - " + Info.tagLabels[i]));
			}
			co_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="InvSoundType"/></c>
		/// (InventorySnds.2da) to the <c>ComboBox</c> along with a final stars
		/// item.
		/// </summary>
		void list_InventorySounds()
		{
			dropdown();

			for (int i = 0; i != Info.soundLabels.Count; ++i)
			{
				co_Val.Items.Add(new tui(i + " - " + Info.soundLabels[i]));
			}
			co_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="PropColumn"/></c>
		/// (ItemProps.2da) to the <c>ComboBox</c> along with a final stars
		/// item.
		/// </summary>
		void list_PropertyCols()
		{
			dropdown();

			for (int i = 0; i != Info.propFields.Count; ++i)
			{
				co_Val.Items.Add(new tui(i + " - " + Info.propFields[i]));
			}
			co_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="StorePanel"/></c> to the
		/// <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_StorePanels()
		{
			dropdown();

			co_Val.Items.AddRange(new []
			{
				new tui("0 - armor and clothing"),
				new tui("1 - weapons"),
				new tui("2 - potions and scrolls"),
				new tui("3 - wands and magic items"),
				new tui("4 - miscellaneous"),
				new tui(gs.Stars)
			});
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="AC_Enchant"/></c> to the
		/// <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_AcEnchants()
		{
			dropdown();

			co_Val.Items.AddRange(new []
			{
				new tui("0 - dodge"),
				new tui("1 - natural"),
				new tui("2 - armor"),
				new tui("3 - shield"),
				new tui("4 - deflection"),
				new tui(gs.Stars)
			});
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="WeaponMatType"/></c>
		/// (WeaponSounds.2da) to the <c>ComboBox</c> along with a final stars
		/// item.
		/// </summary>
		void list_WeaponMatTypes()
		{
			dropdown();

			for (int i = 0; i != Info.weapsoundLabels.Count; ++i)
			{
				co_Val.Items.Add(new tui(i + " - " + Info.weapsoundLabels[i]));
			}
			co_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="AmmunitionType"/></c>
		/// (AmmunitionTypes.2da) to the <c>ComboBox</c> along with a final
		/// stars item.
		/// </summary>
		void list_AmmunitionTypes()
		{
			dropdown();

			for (int i = 0; i != Info.ammoLabels.Count; ++i)
			{
				co_Val.Items.Add(new tui((i + 1) + " - " + Info.ammoLabels[i])); // lovely. off by 1
			}
			co_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="QBBehaviour"/></c> to the
		/// <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_QBBehaviours()
		{
			dropdown();

			co_Val.Items.AddRange(new []
			{
				new tui("0 - none"),
				new tui("1 - rods instruments wands and misc items"),
				new tui("2 - potions and scrolls"),
				new tui(gs.Stars)
			});
		}
		#endregion init


		#region Handlers
		/// <summary>
		/// Handles user changing a <c>CheckBox</c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void changed_Checkbox(object sender, EventArgs e)
		{
			if (!_init)
			{
				_cb = sender as CheckBox;

				switch (_cell.x)
				{
					case EquipableSlots:
						change_EquipableSlots();
						break;
				}
			}
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>.
		/// </summary>
		void change_EquipableSlots()
		{
			if (_cb == cb_00)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_HELMET;
				else             _f.int1 &= ~Yata.EQUIPSLOT_HELMET;
			}
			else if (_cb == cb_01)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_ARMOR;
				else             _f.int1 &= ~Yata.EQUIPSLOT_ARMOR;
			}
			else if (_cb == cb_02)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_BOOTS;
				else             _f.int1 &= ~Yata.EQUIPSLOT_BOOTS;
			}
			else if (_cb == cb_03)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_GLOVES;
				else             _f.int1 &= ~Yata.EQUIPSLOT_GLOVES;
			}
			else if (_cb == cb_04)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_MAINHAND;
				else             _f.int1 &= ~Yata.EQUIPSLOT_MAINHAND;
			}
			else if (_cb == cb_05)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_OFFHAND;
				else             _f.int1 &= ~Yata.EQUIPSLOT_OFFHAND;
			}
			else if (_cb == cb_06)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_CLOAK;
				else             _f.int1 &= ~Yata.EQUIPSLOT_CLOAK;
			}
			else if (_cb == cb_07)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_RINGS;
				else             _f.int1 &= ~Yata.EQUIPSLOT_RINGS;
			}
			else if (_cb == cb_08)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_AMULET;
				else             _f.int1 &= ~Yata.EQUIPSLOT_AMULET;
			}
			else if (_cb == cb_09)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_BELT;
				else             _f.int1 &= ~Yata.EQUIPSLOT_BELT;
			}
			else if (_cb == cb_10)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_ARROW;
				else             _f.int1 &= ~Yata.EQUIPSLOT_ARROW;
			}
			else if (_cb == cb_11)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_BULLET;
				else             _f.int1 &= ~Yata.EQUIPSLOT_BULLET;
			}
			else if (_cb == cb_12)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_BOLT;
				else             _f.int1 &= ~Yata.EQUIPSLOT_BOLT;
			}
			else if (_cb == cb_13)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_CWEAPON;
				else             _f.int1 &= ~Yata.EQUIPSLOT_CWEAPON;
			}
			else if (_cb == cb_14)
			{
				if (_cb.Checked) _f.int1 |=  Yata.EQUIPSLOT_CARMOR;
				else             _f.int1 &= ~Yata.EQUIPSLOT_CARMOR;
			}

			printHexString(_f.int1);

			bu_Clear.Enabled = (_f.int1 != 0);
		}


		/// <summary>
		/// Handles user changing the <c>ComboBox</c> selection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void changed_Combobox(object sender, EventArgs e)
		{
			if (!_init)
			{
				if (co_Val.SelectedIndex == co_Val.Items.Count - 1)
				{
					bu_Clear.Enabled = false;

					_f.str1 = gs.Stars;
					_f.int1 = Yata.Info_ASSIGN_STARS;
				}
				else
				{
					bu_Clear.Enabled = true;

					switch (_cell.x)
					{
						case ModelType:
						case WeaponWield:
						case WeaponType:
						case WeaponSize:
						case RangedWeapon:
						case InvSoundType:
						case PropColumn:
						case StorePanel:
						case AC_Enchant:
						case WeaponMatType:
						case QBBehaviour:
							_f.int1 = co_Val.SelectedIndex;
							break;

						case AmmunitionType:
							_f.int1 = co_Val.SelectedIndex + 1; // off by 1
							break;
					}
				}
			}
		}


		/// <summary>
		/// Handles clicking the Clear <c>Button</c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Clear(object sender, EventArgs e)
		{
			switch (_cell.x)
			{
				case EquipableSlots: // hex,cb,multiple
					bu_Clear.Enabled = false;

					_cb = null;
					clearchecks();

					printHexString(_f.int1 = 0);
					break;

				case ModelType: // dropdown -> fire changed_Combobox()
				case WeaponWield:
				case WeaponType:
				case WeaponSize:
				case RangedWeapon:
				case InvSoundType:
				case PropColumn:
				case StorePanel:
				case AC_Enchant:
				case WeaponMatType:
				case AmmunitionType:
				case QBBehaviour:
					co_Val.SelectedIndex = co_Val.Items.Count - 1;
					break;
			}
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Prints a hex-value at the top of the dialog.
		/// </summary>
		/// <param name="result"></param>
		void printHexString(int result)
		{
			la_Val.Text = "0x" + result.ToString("X5", CultureInfo.InvariantCulture);
		}
		#endregion Methods
	}
}
