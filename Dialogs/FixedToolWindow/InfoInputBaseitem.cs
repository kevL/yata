using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	sealed partial class InfoInputBaseitems
		: InfoInputDialog
	{
		#region Fields (static)
		internal const int EquipableSlots =  5; // col in BaseItems.2da ->
		internal const int ModelType      =  7;
		internal const int WeaponWield    = 18;
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
			}

			_init = false;
		}


		/// <summary>
		/// Prepares this dialog for <c><see cref="EquipableSlots"/></c> input.
		/// </summary>
		void prep_EquipableSlots()
		{
			Text = " EquipableSlots";

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
		/// Adds allowable entries for "ModelType" to the <c>ComboBox</c> along
		/// with a final stars item.
		/// </summary>
		void list_ModelTypes()
		{
			Text = " ModelType";

			dropdown();

			co_Val.Items.Add(new tui("0 - simple 1-part"));
			co_Val.Items.Add(new tui("1 - colored 1-part"));
			co_Val.Items.Add(new tui("2 - configurable 3-part"));
			co_Val.Items.Add(new tui("3 - armor"));

			co_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for "WeaponWield" to the <c>ComboBox</c> along
		/// with a final stars item.
		/// </summary>
		void list_WeaponWields()
		{
			Text = " WeaponWield";

			dropdown();

			co_Val.Items.Add(new tui("0 - standard one-handed weapon"));
			co_Val.Items.Add(new tui("1 - not wieldable"));
			co_Val.Items.Add(new tui("2 - not used/unknown"));
			co_Val.Items.Add(new tui("3 - not used/unknown"));
			co_Val.Items.Add(new tui("4 - two-handed weapon"));
			co_Val.Items.Add(new tui("5 - bow"));
			co_Val.Items.Add(new tui("6 - crossbow"));
			co_Val.Items.Add(new tui("7 - shield"));
			co_Val.Items.Add(new tui("8 - double-sided weapon"));
			co_Val.Items.Add(new tui("9 - creature weapon"));
			co_Val.Items.Add(new tui("10 - dart or sling"));
			co_Val.Items.Add(new tui("11 - shuriken or throwing axe"));
			co_Val.Items.Add(new tui("12 - spears"));
			co_Val.Items.Add(new tui("13 - musical instruments"));

			co_Val.Items.Add(new tui(gs.Stars));
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
		/// - helper for <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>.
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
							_f.int1 = co_Val.SelectedIndex;
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
