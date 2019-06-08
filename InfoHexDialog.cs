using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	sealed partial class InfoHexDialog
		:
			Form
	{
		#region Fields (static)
		internal const int School      =  4; // col in Spells.2da ->
		internal const int Range       =  5;
		internal const int MetaMagic   =  7;
		internal const int TargetType  =  8;
		internal const int Category    = 52;
		internal const int TargetingUI = 66;
		#endregion Fields (static)


		#region Fields
		YataForm _f;
		YataGrid _grid;
		Cell _cell;

		int ColType;
		bool _init;
		bool _bypass;
		#endregion Fields


		#region cTor
		internal InfoHexDialog(YataGrid grid, Cell cell)
		{
			InitializeComponent();

			_grid = grid;
			_f    = _grid._f;
			_cell = cell;

			if (Settings._font2 != null)
				Font = Settings._font2;
			else
				Font = _f.Font;

			init();
		}
		#endregion cTor


		#region Methods
		void init()
		{
			_init = true;

			string val = _cell.text;
			if (!String.IsNullOrEmpty(val)) // safety.
			{
				int result;
				switch (_cell.x)
				{
					case School:
						ColType = School;
						Text = " School";
						setVisibleSchools();

						cb_00.Text = "Abjuration";
						cb_01.Text = "Conjuration";
						cb_02.Text = "Divination";
						cb_03.Text = "Enchantment";
						cb_04.Text = "Illusion";
						cb_05.Text = "Necromancy";
						cb_06.Text = "Transmutation";
						cb_07.Text = "Evocation";

						switch (val)
						{
							case "A": cb_00.Checked = true; break;
							case "C": cb_01.Checked = true; break;
							case "D": cb_02.Checked = true; break;
							case "E": cb_03.Checked = true; break;
							case "I": cb_04.Checked = true; break;
							case "N": cb_05.Checked = true; break;
							case "T": cb_05.Checked = true; break;
							case "V": cb_05.Checked = true; break;
						}

						SetInfoText(-1, (_f.stOriginal = _f.stInput = val));
						break;

					case Range:
						ColType = Range;
						Text = " Range";
						setVisibleRanges();

						cb_00.Text = "Personal" + (( 0 < Info.rangeRanges.Count) ? (" " + Info.rangeRanges[ 0] + "m") : String.Empty);
						cb_01.Text = "Touch"    + (( 1 < Info.rangeRanges.Count) ? (" " + Info.rangeRanges[ 1] + "m") : String.Empty);
						cb_02.Text = "Short"    + (( 2 < Info.rangeRanges.Count) ? (" " + Info.rangeRanges[ 2] + "m") : String.Empty);
						cb_03.Text = "Medium"   + (( 3 < Info.rangeRanges.Count) ? (" " + Info.rangeRanges[ 3] + "m") : String.Empty);
						cb_04.Text = "Long"     + (( 4 < Info.rangeRanges.Count) ? (" " + Info.rangeRanges[ 4] + "m") : String.Empty);
						cb_05.Text = "Infinite" + ((14 < Info.rangeRanges.Count) ? (" " + Info.rangeRanges[14] + "m") : String.Empty);

						switch (val)
						{
							case "P": cb_00.Checked = true; break;
							case "T": cb_01.Checked = true; break;
							case "S": cb_02.Checked = true; break;
							case "M": cb_03.Checked = true; break;
							case "L": cb_04.Checked = true; break;
							case "I": cb_05.Checked = true; break;
						}

						SetInfoText(-1, (_f.stOriginal = _f.stInput = val));
						break;

					case MetaMagic:
						ColType = MetaMagic;
						Text = " MetaMagic";
						setVisibleMetaMagics();

						cb_00.Text = "(1)Empower";
						cb_01.Text = "(2)Extend";
						cb_02.Text = "(4)Maximize";
						cb_03.Text = "(8)Quicken";
						cb_04.Text = "(16)Silent";
						cb_05.Text = "(32)Still";
						cb_06.Text = "(64)Persistent";
						cb_07.Text = "(128)Permanent";

						cb_08.Text = "Draining Blast";		//(256) // Eldritch Essences ->
						cb_09.Text = "Frightful Blast";		//(1024)
						cb_10.Text = "Beshadowed Blast";	//(4096)
						cb_11.Text = "Brimstone Blast";		//(8192)
						cb_12.Text = "Hellrime Blast";		//(32768)
						cb_13.Text = "Bewitching Blast";	//(65536)
						cb_14.Text = "Noxious Blast";		//(262144)
						cb_15.Text = "Vitriolic Blast";		//(524288)
						cb_16.Text = "Utterdark Blast";		//(2097152)
						cb_17.Text = "Hindering Blast";		//(4194304)
						cb_18.Text = "Binding Blast";		//(8388608)

						cb_19.Text = "Eldritch Spear";		//(512) // Invocation Shapes ->
						cb_20.Text = "Hideous Blow";		//(2048)
						cb_21.Text = "Eldritch Chain";		//(16384)
						cb_22.Text = "Eldritch Cone";		//(131072)
						cb_23.Text = "Eldritch Doom";		//(1048576)

						if (val == Constants.Stars || val.Length < 3) val = "0x0";
						if (Int32.TryParse(val.Substring(2),
										   NumberStyles.AllowHexSpecifier,
										   CultureInfo.InvariantCulture,
										   out result))
						{
							EnableMetaMagics(result);

							cb_00.Checked = ((result & YataForm.META_EMPOWER)    != 0);
							cb_01.Checked = ((result & YataForm.META_EXTEND)     != 0);
							cb_02.Checked = ((result & YataForm.META_MAXIMIZE)   != 0);
							cb_03.Checked = ((result & YataForm.META_QUICKEN)    != 0);
							cb_04.Checked = ((result & YataForm.META_SILENT)     != 0);
							cb_05.Checked = ((result & YataForm.META_STILL)      != 0);
							cb_06.Checked = ((result & YataForm.META_PERSISTENT) != 0);
							cb_07.Checked = ((result & YataForm.META_PERMANENT)  != 0);

							cb_08.Checked = ((result & YataForm.META_I_DRAINING_BLAST)   != 0); // Eldritch Essences ->
							cb_09.Checked = ((result & YataForm.META_I_FRIGHTFUL_BLAST)  != 0);
							cb_10.Checked = ((result & YataForm.META_I_BESHADOWED_BLAST) != 0);
							cb_11.Checked = ((result & YataForm.META_I_BRIMSTONE_BLAST)  != 0);
							cb_12.Checked = ((result & YataForm.META_I_HELLRIME_BLAST)   != 0);
							cb_13.Checked = ((result & YataForm.META_I_BEWITCHING_BLAST) != 0);
							cb_14.Checked = ((result & YataForm.META_I_NOXIOUS_BLAST)    != 0);
							cb_15.Checked = ((result & YataForm.META_I_VITRIOLIC_BLAST)  != 0);
							cb_16.Checked = ((result & YataForm.META_I_UTTERDARK_BLAST)  != 0);
							cb_17.Checked = ((result & YataForm.META_I_HINDERING_BLAST)  != 0);
							cb_18.Checked = ((result & YataForm.META_I_BINDING_BLAST)    != 0);

							cb_19.Checked = ((result & YataForm.META_I_ELDRITCH_SPEAR)   != 0); // Invocation Shapes ->
							cb_20.Checked = ((result & YataForm.META_I_HIDEOUS_BLOW)     != 0);
							cb_21.Checked = ((result & YataForm.META_I_ELDRITCH_CHAIN)   != 0);
							cb_22.Checked = ((result & YataForm.META_I_ELDRITCH_CONE)    != 0);
							cb_23.Checked = ((result & YataForm.META_I_ELDRITCH_DOOM)    != 0);

						}
						SetInfoText(_f.intOriginal = _f.intInput = result);
						break;

					case TargetType:
						ColType = TargetType;
						Text = " TargetType";
						setVisibleTargetTypes();

						cb_00.Text = "(1)Self";
						cb_01.Text = "(2)Creatures";
						cb_02.Text = "(4)Ground";
						cb_03.Text = "(8)Items";
						cb_04.Text = "(16)Doors";
						cb_05.Text = "(32)Placeables";
						cb_06.Text = "(64)Triggers";

						if (val == Constants.Stars || val.Length < 3) val = "0x0";
						if (Int32.TryParse(val.Substring(2),
										   NumberStyles.AllowHexSpecifier,
										   CultureInfo.InvariantCulture,
										   out result))
						{
							cb_00.Checked = ((result & YataForm.TARGET_SELF)       != 0);
							cb_01.Checked = ((result & YataForm.TARGET_CREATURE)   != 0);
							cb_02.Checked = ((result & YataForm.TARGET_GROUND)     != 0);
							cb_03.Checked = ((result & YataForm.TARGET_ITEMS)      != 0);
							cb_04.Checked = ((result & YataForm.TARGET_DOORS)      != 0);
							cb_05.Checked = ((result & YataForm.TARGET_PLACEABLES) != 0);
							cb_06.Checked = ((result & YataForm.TARGET_TRIGGERS)   != 0);
						}
						SetInfoText(_f.intOriginal = _f.intInput = result);
						break;

					case Category:
						ColType = Category;
						Text = " Category";
						setComboboxVisible();
						populateCategories();

						if (val == Constants.Stars) val = "0";
						if (Int32.TryParse(val, out result)
							&& result > -1 && result < Info.categoryLabels.Count)
						{
							cbx_Val.SelectedIndex = result;
						}
						else
							cbx_Val.SelectedIndex = Info.categoryLabels.Count; // "n/a"

						_f.intOriginal = _f.intInput = result;
						break;

					case TargetingUI:
						ColType = TargetingUI;
						Text = " TargetingUI";
						setComboboxVisible();
						populateTargeters();

						if (val == Constants.Stars) val = "0";
						if (Int32.TryParse(val, out result)
							&& result > -1 && result < Info.targetLabels.Count)
						{
							cbx_Val.SelectedIndex = result;
						}
						else
							cbx_Val.SelectedIndex = Info.targetLabels.Count; // "n/a"

						_f.intOriginal = _f.intInput = result;
						break;
				}
			}
			_init = false;
		}


		void SetInfoText(int result, string val = null)
		{
			if (val == null)
			{
				string format;
				switch (ColType)
				{
					case MetaMagic:
						if (result <= 0xFF) goto default;
						format = "X6";
						break;

					default:
					case TargetType:
						format = "X2";
						break;
				}
				lbl_Val.Text = "0x" + result.ToString(format);
			}
			else
				lbl_Val.Text = (val == Constants.Stars) ? String.Empty : val;
		}

		void setVisibleSchools()
		{
			cb_00.Visible =
			cb_01.Visible =
			cb_02.Visible =
			cb_03.Visible =
			cb_04.Visible =
			cb_05.Visible =
			cb_06.Visible =
			cb_07.Visible = true;
		}

		void setVisibleRanges()
		{
			cb_00.Visible =
			cb_01.Visible =
			cb_02.Visible =
			cb_03.Visible =
			cb_04.Visible =
			cb_05.Visible = true;
		}

		void setVisibleMetaMagics()
		{
			cb_00.Visible =
			cb_01.Visible =
			cb_02.Visible =
			cb_03.Visible =
			cb_04.Visible =
			cb_05.Visible =
			cb_06.Visible =
			cb_07.Visible =

			cb_08.Visible = // Eldritch Essences and Invocation Shapes ->
			cb_09.Visible =
			cb_10.Visible =
			cb_11.Visible =
			cb_12.Visible =
			cb_13.Visible =
			cb_14.Visible =
			cb_15.Visible =
			cb_16.Visible =
			cb_17.Visible =
			cb_18.Visible =
			cb_19.Visible =
			cb_20.Visible =
			cb_21.Visible =
			cb_22.Visible =
			cb_23.Visible = true;
		}

		void EnableMetaMagics(int result)
		{
			cb_00.Enabled =
			cb_01.Enabled =
			cb_02.Enabled =
			cb_03.Enabled =
			cb_04.Enabled =
			cb_05.Enabled =
			cb_06.Enabled =
			cb_07.Enabled = (result <= 0xFF);

			cb_08.Enabled = // Eldritch Essences and Invocation Shapes ->
			cb_09.Enabled =
			cb_10.Enabled =
			cb_11.Enabled =
			cb_12.Enabled =
			cb_13.Enabled =
			cb_14.Enabled =
			cb_15.Enabled =
			cb_16.Enabled =
			cb_17.Enabled =
			cb_18.Enabled =
			cb_19.Enabled =
			cb_20.Enabled =
			cb_21.Enabled =
			cb_22.Enabled =
			cb_23.Enabled = (result == 0x00 || result > 0xFF);
		}

		void setVisibleTargetTypes()
		{
			cb_00.Visible =
			cb_01.Visible =
			cb_02.Visible =
			cb_03.Visible =
			cb_04.Visible =
			cb_05.Visible =
			cb_06.Visible = true;
		}

		void setComboboxVisible()
		{
			lbl_Val.Visible = false;
			cbx_Val.Visible = true;
		}

		void populateCategories()
		{
			for (int i = 0; i != Info.categoryLabels.Count; ++i)
			{
				cbx_Val.Items.Add(new tui(i + " - " + Info.categoryLabels[i].ToLower()));
			}
			cbx_Val.Items.Add(new tui("n/a"));
		}

		const float epsilon = 0.00001F;

		void populateTargeters()
		{
			string text;

			for (int i = 0; i != Info.targetLabels.Count; ++i)
			{
				text = i + " - " + Info.targetLabels[i];

				bool haspars = false;

				float f = Info.targetWidths[i];
				if (Math.Abs(0.0F - f) > epsilon)
				{
					haspars = true;
					text += " (" + f;
				}

				f = Info.targetLengths[i];
				if (Math.Abs(0.0F - f) > epsilon)
				{
					if (!haspars)
					{
						haspars = true;
						text += " (_";
					}
					text += " x " + f;
				}

				if (haspars) text += ")";

				cbx_Val.Items.Add(new tui(text));
			}
			cbx_Val.Items.Add(new tui("n/a"));
		}
		#endregion Methods


		#region Events (override)
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Close();
			else
				base.OnKeyDown(e);
		}
		#endregion Events (override)


		#region Events
		CheckBox _cb;

		void changed_Checkbox(object sender, EventArgs e)
		{
			if (!_init)
			{
				_cb = sender as CheckBox;

				switch (ColType)
				{
					case School:     changed_School();     break;
					case Range:      changed_Range();      break;
					case MetaMagic:  changed_MetaMagic();  break;
					case TargetType: changed_TargetType(); break;
				}
			}
		}

		void changed_School()
		{
			if (!_bypass)
			{
				_bypass = true;

				string text = Constants.Stars;
				if (_cb == cb_00)
				{
					if (_cb.Checked)
					{
						text = "A";
						cb_01.Checked = cb_02.Checked = cb_03.Checked = cb_04.Checked =
						cb_05.Checked = cb_06.Checked = cb_07.Checked = false;
					}
				}
				else if (_cb == cb_01)
				{
					if (_cb.Checked)
					{
						text = "C";
						cb_00.Checked = cb_02.Checked = cb_03.Checked = cb_04.Checked =
						cb_05.Checked = cb_06.Checked = cb_07.Checked = false;
					}
				}
				else if (_cb == cb_02)
				{
					if (_cb.Checked)
					{
						text = "D";
						cb_00.Checked = cb_01.Checked = cb_03.Checked = cb_04.Checked =
						cb_05.Checked = cb_06.Checked = cb_07.Checked = false;
					}
				}
				else if (_cb == cb_03)
				{
					if (_cb.Checked)
					{
						text = "E";
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_04.Checked =
						cb_05.Checked = cb_06.Checked = cb_07.Checked = false;
					}
				}
				else if (_cb == cb_04)
				{
					if (_cb.Checked)
					{
						text = "I";
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_05.Checked = cb_06.Checked = cb_07.Checked = false;
					}
				}
				else if (_cb == cb_05)
				{
					if (_cb.Checked)
					{
						text = "N";
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_06.Checked = cb_07.Checked = false;
					}
				}
				else if (_cb == cb_06)
				{
					if (_cb.Checked)
					{
						text = "T";
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = cb_07.Checked = false;
					}
				}
				else //if (_cb == cb_07)
				{
					if (_cb.Checked)
					{
						text = "V";
						cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = cb_06.Checked = false;
					}
				}
				SetInfoText(-1, (_f.stInput = text));

				_bypass = false;
			}
		}

		void changed_Range()
		{
			if (!_bypass)
			{
				_bypass = true;

				string text = Constants.Stars;
				if (_cb == cb_00)
				{
					if (_cb.Checked)
					{
						text = "P";
						cb_01.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = false;
					}
				}
				else if (_cb == cb_01)
				{
					if (_cb.Checked)
					{
						text = "T";
						cb_00.Checked = cb_02.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = false;
					}
				}
				else if (_cb == cb_02)
				{
					if (_cb.Checked)
					{
						text = "S";
						cb_00.Checked = cb_01.Checked = cb_03.Checked =
						cb_04.Checked = cb_05.Checked = false;
					}
				}
				else if (_cb == cb_03)
				{
					if (_cb.Checked)
					{
						text = "M";
						cb_00.Checked = cb_01.Checked = cb_02.Checked =
						cb_04.Checked = cb_05.Checked = false;
					}
				}
				else if (_cb == cb_04)
				{
					if (_cb.Checked)
					{
						text = "L";
						cb_00.Checked = cb_01.Checked = cb_02.Checked =
						cb_03.Checked = cb_05.Checked = false;
					}
				}
				else //if (_cb == cb_05)
				{
					if (_cb.Checked)
					{
						text = "I";
						cb_00.Checked = cb_01.Checked = cb_02.Checked =
						cb_03.Checked = cb_04.Checked = false;
					}
				}
				SetInfoText(-1, (_f.stInput = text));

				_bypass = false;
			}
		}

		void changed_MetaMagic()
		{
			if (_cb == cb_00)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_EMPOWER;
				else             _f.intInput &= ~YataForm.META_EMPOWER;
			}
			else if (_cb == cb_01)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_EXTEND;
				else             _f.intInput &= ~YataForm.META_EXTEND;
			}
			else if (_cb == cb_02)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_MAXIMIZE;
				else             _f.intInput &= ~YataForm.META_MAXIMIZE;
			}
			else if (_cb == cb_03)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_QUICKEN;
				else             _f.intInput &= ~YataForm.META_QUICKEN;
			}
			else if (_cb == cb_04)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_SILENT;
				else             _f.intInput &= ~YataForm.META_SILENT;
			}
			else if (_cb == cb_05)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_STILL;
				else             _f.intInput &= ~YataForm.META_STILL;
			}
			else if (_cb == cb_06)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_PERSISTENT;
				else             _f.intInput &= ~YataForm.META_PERSISTENT;
			}
			else if (_cb == cb_07)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_PERMANENT;
				else             _f.intInput &= ~YataForm.META_PERMANENT;
			}

			else if (_cb == cb_08) // Eldritch Essences and Invocation Shapes ->
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_DRAINING_BLAST;
				else             _f.intInput &= ~YataForm.META_I_DRAINING_BLAST;
			}
			else if (_cb == cb_09)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_FRIGHTFUL_BLAST;
				else             _f.intInput &= ~YataForm.META_I_FRIGHTFUL_BLAST;
			}
			else if (_cb == cb_10)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_BESHADOWED_BLAST;
				else             _f.intInput &= ~YataForm.META_I_BESHADOWED_BLAST;
			}
			else if (_cb == cb_11)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_BRIMSTONE_BLAST;
				else             _f.intInput &= ~YataForm.META_I_BRIMSTONE_BLAST;
			}
			else if (_cb == cb_12)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_HELLRIME_BLAST;
				else             _f.intInput &= ~YataForm.META_I_HELLRIME_BLAST;
			}
			else if (_cb == cb_13)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_BEWITCHING_BLAST;
				else             _f.intInput &= ~YataForm.META_I_BEWITCHING_BLAST;
			}
			else if (_cb == cb_14)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_NOXIOUS_BLAST;
				else             _f.intInput &= ~YataForm.META_I_NOXIOUS_BLAST;
			}
			else if (_cb == cb_15)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_VITRIOLIC_BLAST;
				else             _f.intInput &= ~YataForm.META_I_VITRIOLIC_BLAST;
			}
			else if (_cb == cb_16)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_UTTERDARK_BLAST;
				else             _f.intInput &= ~YataForm.META_I_UTTERDARK_BLAST;
			}
			else if (_cb == cb_17)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_HINDERING_BLAST;
				else             _f.intInput &= ~YataForm.META_I_HINDERING_BLAST;
			}
			else if (_cb == cb_18)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_BINDING_BLAST;
				else             _f.intInput &= ~YataForm.META_I_BINDING_BLAST;
			}

			else if (_cb == cb_19)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_ELDRITCH_SPEAR;
				else             _f.intInput &= ~YataForm.META_I_ELDRITCH_SPEAR;
			}
			else if (_cb == cb_20)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_HIDEOUS_BLOW;
				else             _f.intInput &= ~YataForm.META_I_HIDEOUS_BLOW;
			}
			else if (_cb == cb_21)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_ELDRITCH_CHAIN;
				else             _f.intInput &= ~YataForm.META_I_ELDRITCH_CHAIN;
			}
			else if (_cb == cb_22)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_ELDRITCH_CONE;
				else             _f.intInput &= ~YataForm.META_I_ELDRITCH_CONE;
			}
			else if (_cb == cb_23)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.META_I_ELDRITCH_DOOM;
				else             _f.intInput &= ~YataForm.META_I_ELDRITCH_DOOM;
			}

			SetInfoText(_f.intInput);
			EnableMetaMagics(_f.intInput);
		}

		void changed_TargetType()
		{
			if (_cb == cb_00)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_SELF;
				else             _f.intInput &= ~YataForm.TARGET_SELF;
			}
			else if (_cb == cb_01)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_CREATURE;
				else             _f.intInput &= ~YataForm.TARGET_CREATURE;
			}
			else if (_cb == cb_02)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_GROUND;
				else             _f.intInput &= ~YataForm.TARGET_GROUND;
			}
			else if (_cb == cb_03)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_ITEMS;
				else             _f.intInput &= ~YataForm.TARGET_ITEMS;
			}
			else if (_cb == cb_04)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_DOORS;
				else             _f.intInput &= ~YataForm.TARGET_DOORS;
			}
			else if (_cb == cb_05)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_PLACEABLES;
				else             _f.intInput &= ~YataForm.TARGET_PLACEABLES;
			}
			else if (_cb == cb_06)
			{
				if (_cb.Checked) _f.intInput |=  YataForm.TARGET_TRIGGERS;
				else             _f.intInput &= ~YataForm.TARGET_TRIGGERS;
			}

			SetInfoText(_f.intInput);
		}

		void changed_Combobox(object sender, EventArgs e)
		{
			if (!_init)
			{
				switch (ColType)
				{
					case Category:
						if (cbx_Val.SelectedIndex == Info.categoryLabels.Count)
							_f.intInput = 0;
						else
							_f.intInput = cbx_Val.SelectedIndex;
						break;

					case TargetingUI:
						if (cbx_Val.SelectedIndex == Info.targetLabels.Count)
							_f.intInput = 0;
						else
							_f.intInput = cbx_Val.SelectedIndex;
						break;
				}
			}
		}


/*		void click_Accept(object sender, EventArgs e)
		{} */
		#endregion Events
	}


	/// <summary>
	/// 
	/// </summary>
	sealed class tui // TargetingUI dropdown-object
	{
		string Label
		{ get; set; }

		internal tui(string label)
		{
			Label = label;
		}

		/// <summary>
		/// Required.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Label;
		}
	}
}
