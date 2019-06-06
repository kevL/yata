﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	sealed class InfoHexDialog
		:
			Form
	{
		#region Fields (static)
		const int MetaMagic  = 7;
		const int TargetType = 8;
		#endregion Fields (static)


		#region Fields
		YataForm _f;
		YataGrid _grid;
		Cell _cell;

		int ColType;
		#endregion Fields


		#region cTor
		internal InfoHexDialog(YataGrid grid)
		{
			InitializeComponent();

			_grid =  grid;
			_f    = _grid._f;
			_cell = _grid.getSelectedCell();

			if (Settings._font2 != null)
				Font = Settings._font2;
			else
				Font = _f.Font;

			texts();
		}
		#endregion cTor


		#region Methods
		void texts()
		{
			string val = _cell.text;
			if (!String.IsNullOrEmpty(val)) // safety.
			{
				if (val == Constants.Stars) val = "0x00";

				int result;
				switch (_cell.x)
				{
					case MetaMagic:
						ColType = MetaMagic;
						Text = " MetaMagic";
						setVisibleMetamagicTypes();

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

						if (Int32.TryParse(val.Substring(2),
										   NumberStyles.AllowHexSpecifier,
										   CultureInfo.InvariantCulture,
										   out result))
						{
							EnableMetamagicTypes(result);

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

/*							switch (result)
							{
								case YataForm.META_NONE:
//									info += "none";
									break;
								case YataForm.META_ANY:
//									info += "ANY";
									break;
								case YataForm.META_I_ALL:
//									info += "All Eldritch Essences and Invocation Shapes";
									break;
								case YataForm.META_I_SHAPES:
									// Eldritch Spear, Hideous Blow, Eldritch Chain, Eldritch Cone, Eldritch Doom
//									info += "All Invocation Shapes";
									break;
								case YataForm.META_I_ESSENCES:
									// Draining, Frightful, Beshadowed, Brimstone, Hellrime, Bewitching, Noxious,
									// Vitriolic, Utterdark, Hindering, Binding
//									info += "All Eldritch Essences";
									break;
							} */
						}
						SetInfoText(_f._original = result);
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
						SetInfoText(_f._original = result);
						break;
				}
			}
		}


		void SetInfoText(int result)
		{
			string format;
			switch (ColType)
			{
				case MetaMagic:
					format = (result <= 0xFF) ? "X2" : "X8";
					break;

				default:
				case TargetType:
					format = "X2";
					break;
			}
			lbl_Val.Text = "0x" + result.ToString(format);
		}

		void EnableMetamagicTypes(int result)
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

		void setVisibleMetamagicTypes()
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
		void changed(object sender, EventArgs e)
		{
			var cb = sender as CheckBox;

			switch (ColType)
			{
				case MetaMagic:  changed_MetaMagic(cb);  break;
				case TargetType: changed_TargetType(cb); break;
			}
		}

		//const int META_NONE               = 0x00000000; //        0
		//const int META_EMPOWER            = 0x00000001; //        1
		//const int META_EXTEND             = 0x00000002; //        2
		//const int META_MAXIMIZE           = 0x00000004; //        4
		//const int META_QUICKEN            = 0x00000008; //        8
		//const int META_SILENT             = 0x00000010; //       16
		//const int META_STILL              = 0x00000020; //       32
		//const int META_PERSISTENT         = 0x00000040; //       64
		//const int META_PERMANENT          = 0x00000080; //      128

		//const int META_I_DRAINING_BLAST   = 0x00000100; //      256
		//const int META_I_ELDRITCH_SPEAR   = 0x00000200; //      512
		//const int META_I_FRIGHTFUL_BLAST  = 0x00000400; //     1024
		//const int META_I_HIDEOUS_BLOW     = 0x00000800; //     2048
		//const int META_I_BESHADOWED_BLAST = 0x00001000; //     4096
		//const int META_I_BRIMSTONE_BLAST  = 0x00002000; //     8192
		//const int META_I_ELDRITCH_CHAIN   = 0x00004000; //    16384
		//const int META_I_HELLRIME_BLAST   = 0x00008000; //    32768
		//const int META_I_BEWITCHING_BLAST = 0x00010000; //    65536
		//const int META_I_ELDRITCH_CONE    = 0x00020000; //   131072
		//const int META_I_NOXIOUS_BLAST    = 0x00040000; //   262144
		//const int META_I_VITRIOLIC_BLAST  = 0x00080000; //   524288
		//const int META_I_ELDRITCH_DOOM    = 0x00100000; //  1048576
		//const int META_I_UTTERDARK_BLAST  = 0x00200000; //  2097152
		//const int META_I_HINDERING_BLAST  = 0x00400000; //  4194304
		//const int META_I_BINDING_BLAST    = 0x00800000; //  8388608

		//const int META_I_SHAPES           = 0x00124A00; //  1198592 - all blast shapes
		//const int META_I_ESSENCES         = 0x00EDB500; // 15578368 - all eldritch essences
		//const int META_I_ALL              = 0x00FFFF00; // 16776960 - all shapes and essences

		//const int META_ANY                = unchecked((int)0xFFFFFFFF); // 4294967295 - the kitchen sink (not).
		void changed_MetaMagic(CheckBox cb)
		{
			if (cb == cb_00)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_EMPOWER;
				else            _grid._f._input &= ~YataForm.META_EMPOWER;
			}
			else if (cb == cb_01)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_EXTEND;
				else            _grid._f._input &= ~YataForm.META_EXTEND;
			}
			else if (cb == cb_02)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_MAXIMIZE;
				else            _grid._f._input &= ~YataForm.META_MAXIMIZE;
			}
			else if (cb == cb_03)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_QUICKEN;
				else            _grid._f._input &= ~YataForm.META_QUICKEN;
			}
			else if (cb == cb_04)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_SILENT;
				else            _grid._f._input &= ~YataForm.META_SILENT;
			}
			else if (cb == cb_05)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_STILL;
				else            _grid._f._input &= ~YataForm.META_STILL;
			}
			else if (cb == cb_06)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_PERSISTENT;
				else            _grid._f._input &= ~YataForm.META_PERSISTENT;
			}
			else if (cb == cb_07)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_PERMANENT;
				else            _grid._f._input &= ~YataForm.META_PERMANENT;
			}

			else if (cb == cb_08) // Eldritch Essences and Invocation Shapes ->
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_DRAINING_BLAST;
				else            _grid._f._input &= ~YataForm.META_I_DRAINING_BLAST;
			}
			else if (cb == cb_09)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_FRIGHTFUL_BLAST;
				else            _grid._f._input &= ~YataForm.META_I_FRIGHTFUL_BLAST;
			}
			else if (cb == cb_10)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_BESHADOWED_BLAST;
				else            _grid._f._input &= ~YataForm.META_I_BESHADOWED_BLAST;
			}
			else if (cb == cb_11)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_BRIMSTONE_BLAST;
				else            _grid._f._input &= ~YataForm.META_I_BRIMSTONE_BLAST;
			}
			else if (cb == cb_12)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_HELLRIME_BLAST;
				else            _grid._f._input &= ~YataForm.META_I_HELLRIME_BLAST;
			}
			else if (cb == cb_13)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_BEWITCHING_BLAST;
				else            _grid._f._input &= ~YataForm.META_I_BEWITCHING_BLAST;
			}
			else if (cb == cb_14)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_NOXIOUS_BLAST;
				else            _grid._f._input &= ~YataForm.META_I_NOXIOUS_BLAST;
			}
			else if (cb == cb_15)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_VITRIOLIC_BLAST;
				else            _grid._f._input &= ~YataForm.META_I_VITRIOLIC_BLAST;
			}
			else if (cb == cb_16)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_UTTERDARK_BLAST;
				else            _grid._f._input &= ~YataForm.META_I_UTTERDARK_BLAST;
			}
			else if (cb == cb_17)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_HINDERING_BLAST;
				else            _grid._f._input &= ~YataForm.META_I_HINDERING_BLAST;
			}
			else if (cb == cb_18)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_BINDING_BLAST;
				else            _grid._f._input &= ~YataForm.META_I_BINDING_BLAST;
			}

			else if (cb == cb_19)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_ELDRITCH_SPEAR;
				else            _grid._f._input &= ~YataForm.META_I_ELDRITCH_SPEAR;
			}
			else if (cb == cb_20)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_HIDEOUS_BLOW;
				else            _grid._f._input &= ~YataForm.META_I_HIDEOUS_BLOW;
			}
			else if (cb == cb_21)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_ELDRITCH_CHAIN;
				else            _grid._f._input &= ~YataForm.META_I_ELDRITCH_CHAIN;
			}
			else if (cb == cb_22)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_ELDRITCH_CONE;
				else            _grid._f._input &= ~YataForm.META_I_ELDRITCH_CONE;
			}
			else if (cb == cb_23)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.META_I_ELDRITCH_DOOM;
				else            _grid._f._input &= ~YataForm.META_I_ELDRITCH_DOOM;
			}

			int result = _grid._f._input;
			SetInfoText(result);
			EnableMetamagicTypes(result);
		}

		//internal const int TARGET_NONE       = 0x00; //  0
		//internal const int TARGET_SELF       = 0x01; //  1
		//internal const int TARGET_CREATURE   = 0x02; //  2
		//internal const int TARGET_GROUND     = 0x04; //  4
		//internal const int TARGET_ITEMS      = 0x08; //  8
		//internal const int TARGET_DOORS      = 0x10; // 16
		//internal const int TARGET_PLACEABLES = 0x20; // 32
		//internal const int TARGET_TRIGGERS   = 0x40; // 64
		void changed_TargetType(CheckBox cb)
		{
			if (cb == cb_00)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_SELF;
				else            _grid._f._input &= ~YataForm.TARGET_SELF;
			}
			else if (cb == cb_01)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_CREATURE;
				else            _grid._f._input &= ~YataForm.TARGET_CREATURE;
			}
			else if (cb == cb_02)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_GROUND;
				else            _grid._f._input &= ~YataForm.TARGET_GROUND;
			}
			else if (cb == cb_03)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_ITEMS;
				else            _grid._f._input &= ~YataForm.TARGET_ITEMS;
			}
			else if (cb == cb_04)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_DOORS;
				else            _grid._f._input &= ~YataForm.TARGET_DOORS;
			}
			else if (cb == cb_05)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_PLACEABLES;
				else            _grid._f._input &= ~YataForm.TARGET_PLACEABLES;
			}
			else if (cb == cb_06)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_TRIGGERS;
				else            _grid._f._input &= ~YataForm.TARGET_TRIGGERS;
			}

			int result = _grid._f._input;
			SetInfoText(result);
		}

/*		void click_Accept(object sender, EventArgs e)
		{} */
		#endregion Events


		#region Windows Form Designer generated code
		Container components = null;

		CheckBox cb_00;
		CheckBox cb_01;
		CheckBox cb_02;
		CheckBox cb_03;
		CheckBox cb_04;
		CheckBox cb_05;
		CheckBox cb_06;
		CheckBox cb_07;
		Button btn_Accept;
		Label lbl_Val;
		CheckBox cb_15;
		CheckBox cb_14;
		CheckBox cb_13;
		CheckBox cb_12;
		CheckBox cb_11;
		CheckBox cb_10;
		CheckBox cb_09;
		CheckBox cb_08;
		CheckBox cb_23;
		CheckBox cb_22;
		CheckBox cb_21;
		CheckBox cb_20;
		CheckBox cb_19;
		CheckBox cb_18;
		CheckBox cb_17;
		CheckBox cb_16;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cb_00 = new System.Windows.Forms.CheckBox();
			this.cb_01 = new System.Windows.Forms.CheckBox();
			this.cb_02 = new System.Windows.Forms.CheckBox();
			this.cb_03 = new System.Windows.Forms.CheckBox();
			this.cb_04 = new System.Windows.Forms.CheckBox();
			this.cb_05 = new System.Windows.Forms.CheckBox();
			this.cb_06 = new System.Windows.Forms.CheckBox();
			this.cb_07 = new System.Windows.Forms.CheckBox();
			this.btn_Accept = new System.Windows.Forms.Button();
			this.lbl_Val = new System.Windows.Forms.Label();
			this.cb_15 = new System.Windows.Forms.CheckBox();
			this.cb_14 = new System.Windows.Forms.CheckBox();
			this.cb_13 = new System.Windows.Forms.CheckBox();
			this.cb_12 = new System.Windows.Forms.CheckBox();
			this.cb_11 = new System.Windows.Forms.CheckBox();
			this.cb_10 = new System.Windows.Forms.CheckBox();
			this.cb_09 = new System.Windows.Forms.CheckBox();
			this.cb_08 = new System.Windows.Forms.CheckBox();
			this.cb_23 = new System.Windows.Forms.CheckBox();
			this.cb_22 = new System.Windows.Forms.CheckBox();
			this.cb_21 = new System.Windows.Forms.CheckBox();
			this.cb_20 = new System.Windows.Forms.CheckBox();
			this.cb_19 = new System.Windows.Forms.CheckBox();
			this.cb_18 = new System.Windows.Forms.CheckBox();
			this.cb_17 = new System.Windows.Forms.CheckBox();
			this.cb_16 = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// cb_00
			// 
			this.cb_00.Location = new System.Drawing.Point(5, 25);
			this.cb_00.Margin = new System.Windows.Forms.Padding(0);
			this.cb_00.Name = "cb_00";
			this.cb_00.Size = new System.Drawing.Size(120, 20);
			this.cb_00.TabIndex = 1;
			this.cb_00.Text = "cb_00";
			this.cb_00.UseVisualStyleBackColor = true;
			this.cb_00.Visible = false;
			this.cb_00.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_01
			// 
			this.cb_01.Location = new System.Drawing.Point(5, 45);
			this.cb_01.Margin = new System.Windows.Forms.Padding(0);
			this.cb_01.Name = "cb_01";
			this.cb_01.Size = new System.Drawing.Size(120, 20);
			this.cb_01.TabIndex = 2;
			this.cb_01.Text = "cb_01";
			this.cb_01.UseVisualStyleBackColor = true;
			this.cb_01.Visible = false;
			this.cb_01.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_02
			// 
			this.cb_02.Location = new System.Drawing.Point(5, 65);
			this.cb_02.Margin = new System.Windows.Forms.Padding(0);
			this.cb_02.Name = "cb_02";
			this.cb_02.Size = new System.Drawing.Size(120, 20);
			this.cb_02.TabIndex = 3;
			this.cb_02.Text = "cb_02";
			this.cb_02.UseVisualStyleBackColor = true;
			this.cb_02.Visible = false;
			this.cb_02.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_03
			// 
			this.cb_03.Location = new System.Drawing.Point(5, 85);
			this.cb_03.Margin = new System.Windows.Forms.Padding(0);
			this.cb_03.Name = "cb_03";
			this.cb_03.Size = new System.Drawing.Size(120, 20);
			this.cb_03.TabIndex = 4;
			this.cb_03.Text = "cb_03";
			this.cb_03.UseVisualStyleBackColor = true;
			this.cb_03.Visible = false;
			this.cb_03.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_04
			// 
			this.cb_04.Location = new System.Drawing.Point(5, 105);
			this.cb_04.Margin = new System.Windows.Forms.Padding(0);
			this.cb_04.Name = "cb_04";
			this.cb_04.Size = new System.Drawing.Size(120, 20);
			this.cb_04.TabIndex = 5;
			this.cb_04.Text = "cb_04";
			this.cb_04.UseVisualStyleBackColor = true;
			this.cb_04.Visible = false;
			this.cb_04.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_05
			// 
			this.cb_05.Location = new System.Drawing.Point(5, 125);
			this.cb_05.Margin = new System.Windows.Forms.Padding(0);
			this.cb_05.Name = "cb_05";
			this.cb_05.Size = new System.Drawing.Size(120, 20);
			this.cb_05.TabIndex = 6;
			this.cb_05.Text = "cb_05";
			this.cb_05.UseVisualStyleBackColor = true;
			this.cb_05.Visible = false;
			this.cb_05.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_06
			// 
			this.cb_06.Location = new System.Drawing.Point(5, 145);
			this.cb_06.Margin = new System.Windows.Forms.Padding(0);
			this.cb_06.Name = "cb_06";
			this.cb_06.Size = new System.Drawing.Size(120, 20);
			this.cb_06.TabIndex = 7;
			this.cb_06.Text = "cb_06";
			this.cb_06.UseVisualStyleBackColor = true;
			this.cb_06.Visible = false;
			this.cb_06.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_07
			// 
			this.cb_07.Location = new System.Drawing.Point(5, 165);
			this.cb_07.Margin = new System.Windows.Forms.Padding(0);
			this.cb_07.Name = "cb_07";
			this.cb_07.Size = new System.Drawing.Size(120, 20);
			this.cb_07.TabIndex = 8;
			this.cb_07.Text = "cb_07";
			this.cb_07.UseVisualStyleBackColor = true;
			this.cb_07.Visible = false;
			this.cb_07.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// btn_Accept
			// 
			this.btn_Accept.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_Accept.Location = new System.Drawing.Point(5, 188);
			this.btn_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Accept.Name = "btn_Accept";
			this.btn_Accept.Size = new System.Drawing.Size(385, 25);
			this.btn_Accept.TabIndex = 25;
			this.btn_Accept.Text = "accept";
			this.btn_Accept.UseVisualStyleBackColor = true;
			// 
			// lbl_Val
			// 
			this.lbl_Val.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.lbl_Val.Location = new System.Drawing.Point(5, 5);
			this.lbl_Val.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Val.Name = "lbl_Val";
			this.lbl_Val.Size = new System.Drawing.Size(385, 15);
			this.lbl_Val.TabIndex = 0;
			this.lbl_Val.Text = "lbl_Val";
			this.lbl_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cb_15
			// 
			this.cb_15.Location = new System.Drawing.Point(130, 165);
			this.cb_15.Margin = new System.Windows.Forms.Padding(0);
			this.cb_15.Name = "cb_15";
			this.cb_15.Size = new System.Drawing.Size(120, 20);
			this.cb_15.TabIndex = 16;
			this.cb_15.Text = "cb_15";
			this.cb_15.UseVisualStyleBackColor = true;
			this.cb_15.Visible = false;
			this.cb_15.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_14
			// 
			this.cb_14.Location = new System.Drawing.Point(130, 145);
			this.cb_14.Margin = new System.Windows.Forms.Padding(0);
			this.cb_14.Name = "cb_14";
			this.cb_14.Size = new System.Drawing.Size(120, 20);
			this.cb_14.TabIndex = 15;
			this.cb_14.Text = "cb_14";
			this.cb_14.UseVisualStyleBackColor = true;
			this.cb_14.Visible = false;
			this.cb_14.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_13
			// 
			this.cb_13.Location = new System.Drawing.Point(130, 125);
			this.cb_13.Margin = new System.Windows.Forms.Padding(0);
			this.cb_13.Name = "cb_13";
			this.cb_13.Size = new System.Drawing.Size(120, 20);
			this.cb_13.TabIndex = 14;
			this.cb_13.Text = "cb_13";
			this.cb_13.UseVisualStyleBackColor = true;
			this.cb_13.Visible = false;
			this.cb_13.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_12
			// 
			this.cb_12.Location = new System.Drawing.Point(130, 105);
			this.cb_12.Margin = new System.Windows.Forms.Padding(0);
			this.cb_12.Name = "cb_12";
			this.cb_12.Size = new System.Drawing.Size(120, 20);
			this.cb_12.TabIndex = 13;
			this.cb_12.Text = "cb_12";
			this.cb_12.UseVisualStyleBackColor = true;
			this.cb_12.Visible = false;
			this.cb_12.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_11
			// 
			this.cb_11.Location = new System.Drawing.Point(130, 85);
			this.cb_11.Margin = new System.Windows.Forms.Padding(0);
			this.cb_11.Name = "cb_11";
			this.cb_11.Size = new System.Drawing.Size(120, 20);
			this.cb_11.TabIndex = 12;
			this.cb_11.Text = "cb_11";
			this.cb_11.UseVisualStyleBackColor = true;
			this.cb_11.Visible = false;
			this.cb_11.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_10
			// 
			this.cb_10.Location = new System.Drawing.Point(130, 65);
			this.cb_10.Margin = new System.Windows.Forms.Padding(0);
			this.cb_10.Name = "cb_10";
			this.cb_10.Size = new System.Drawing.Size(120, 20);
			this.cb_10.TabIndex = 11;
			this.cb_10.Text = "cb_10";
			this.cb_10.UseVisualStyleBackColor = true;
			this.cb_10.Visible = false;
			this.cb_10.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_09
			// 
			this.cb_09.Location = new System.Drawing.Point(130, 45);
			this.cb_09.Margin = new System.Windows.Forms.Padding(0);
			this.cb_09.Name = "cb_09";
			this.cb_09.Size = new System.Drawing.Size(120, 20);
			this.cb_09.TabIndex = 10;
			this.cb_09.Text = "cb_09";
			this.cb_09.UseVisualStyleBackColor = true;
			this.cb_09.Visible = false;
			this.cb_09.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_08
			// 
			this.cb_08.Location = new System.Drawing.Point(130, 25);
			this.cb_08.Margin = new System.Windows.Forms.Padding(0);
			this.cb_08.Name = "cb_08";
			this.cb_08.Size = new System.Drawing.Size(120, 20);
			this.cb_08.TabIndex = 9;
			this.cb_08.Text = "cb_08";
			this.cb_08.UseVisualStyleBackColor = true;
			this.cb_08.Visible = false;
			this.cb_08.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_23
			// 
			this.cb_23.Location = new System.Drawing.Point(255, 165);
			this.cb_23.Margin = new System.Windows.Forms.Padding(0);
			this.cb_23.Name = "cb_23";
			this.cb_23.Size = new System.Drawing.Size(120, 20);
			this.cb_23.TabIndex = 24;
			this.cb_23.Text = "cb_23";
			this.cb_23.UseVisualStyleBackColor = true;
			this.cb_23.Visible = false;
			this.cb_23.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_22
			// 
			this.cb_22.Location = new System.Drawing.Point(255, 145);
			this.cb_22.Margin = new System.Windows.Forms.Padding(0);
			this.cb_22.Name = "cb_22";
			this.cb_22.Size = new System.Drawing.Size(120, 20);
			this.cb_22.TabIndex = 23;
			this.cb_22.Text = "cb_22";
			this.cb_22.UseVisualStyleBackColor = true;
			this.cb_22.Visible = false;
			this.cb_22.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_21
			// 
			this.cb_21.Location = new System.Drawing.Point(255, 125);
			this.cb_21.Margin = new System.Windows.Forms.Padding(0);
			this.cb_21.Name = "cb_21";
			this.cb_21.Size = new System.Drawing.Size(120, 20);
			this.cb_21.TabIndex = 22;
			this.cb_21.Text = "cb_21";
			this.cb_21.UseVisualStyleBackColor = true;
			this.cb_21.Visible = false;
			this.cb_21.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_20
			// 
			this.cb_20.Location = new System.Drawing.Point(255, 105);
			this.cb_20.Margin = new System.Windows.Forms.Padding(0);
			this.cb_20.Name = "cb_20";
			this.cb_20.Size = new System.Drawing.Size(120, 20);
			this.cb_20.TabIndex = 21;
			this.cb_20.Text = "cb_20";
			this.cb_20.UseVisualStyleBackColor = true;
			this.cb_20.Visible = false;
			this.cb_20.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_19
			// 
			this.cb_19.Location = new System.Drawing.Point(255, 85);
			this.cb_19.Margin = new System.Windows.Forms.Padding(0);
			this.cb_19.Name = "cb_19";
			this.cb_19.Size = new System.Drawing.Size(120, 20);
			this.cb_19.TabIndex = 20;
			this.cb_19.Text = "cb_19";
			this.cb_19.UseVisualStyleBackColor = true;
			this.cb_19.Visible = false;
			this.cb_19.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_18
			// 
			this.cb_18.Location = new System.Drawing.Point(255, 65);
			this.cb_18.Margin = new System.Windows.Forms.Padding(0);
			this.cb_18.Name = "cb_18";
			this.cb_18.Size = new System.Drawing.Size(120, 20);
			this.cb_18.TabIndex = 19;
			this.cb_18.Text = "cb_18";
			this.cb_18.UseVisualStyleBackColor = true;
			this.cb_18.Visible = false;
			this.cb_18.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_17
			// 
			this.cb_17.Location = new System.Drawing.Point(255, 45);
			this.cb_17.Margin = new System.Windows.Forms.Padding(0);
			this.cb_17.Name = "cb_17";
			this.cb_17.Size = new System.Drawing.Size(120, 20);
			this.cb_17.TabIndex = 18;
			this.cb_17.Text = "cb_17";
			this.cb_17.UseVisualStyleBackColor = true;
			this.cb_17.Visible = false;
			this.cb_17.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_16
			// 
			this.cb_16.Location = new System.Drawing.Point(255, 25);
			this.cb_16.Margin = new System.Windows.Forms.Padding(0);
			this.cb_16.Name = "cb_16";
			this.cb_16.Size = new System.Drawing.Size(120, 20);
			this.cb_16.TabIndex = 17;
			this.cb_16.Text = "cb_16";
			this.cb_16.UseVisualStyleBackColor = true;
			this.cb_16.Visible = false;
			this.cb_16.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// InfoHexDialog
			// 
			this.AcceptButton = this.btn_Accept;
			this.ClientSize = new System.Drawing.Size(394, 216);
			this.Controls.Add(this.cb_23);
			this.Controls.Add(this.cb_22);
			this.Controls.Add(this.cb_21);
			this.Controls.Add(this.cb_20);
			this.Controls.Add(this.cb_19);
			this.Controls.Add(this.cb_18);
			this.Controls.Add(this.cb_17);
			this.Controls.Add(this.cb_16);
			this.Controls.Add(this.cb_15);
			this.Controls.Add(this.cb_14);
			this.Controls.Add(this.cb_13);
			this.Controls.Add(this.cb_12);
			this.Controls.Add(this.cb_11);
			this.Controls.Add(this.cb_10);
			this.Controls.Add(this.cb_09);
			this.Controls.Add(this.cb_08);
			this.Controls.Add(this.lbl_Val);
			this.Controls.Add(this.btn_Accept);
			this.Controls.Add(this.cb_07);
			this.Controls.Add(this.cb_06);
			this.Controls.Add(this.cb_05);
			this.Controls.Add(this.cb_04);
			this.Controls.Add(this.cb_03);
			this.Controls.Add(this.cb_02);
			this.Controls.Add(this.cb_01);
			this.Controls.Add(this.cb_00);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InfoHexDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);

		}
		#endregion
	}
}
