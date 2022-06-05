using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class InfoInputSpells
	{
		Label la_Val;
		ComboBox co_Val;

		CheckBox cb_00;
		CheckBox cb_01;
		CheckBox cb_02;
		CheckBox cb_03;
		CheckBox cb_04;
		CheckBox cb_05;
		CheckBox cb_06;
		CheckBox cb_07;
		CheckBox cb_08;
		CheckBox cb_09;
		CheckBox cb_10;
		CheckBox cb_11;
		CheckBox cb_12;
		CheckBox cb_13;
		CheckBox cb_14;
		CheckBox cb_15;
		CheckBox cb_16;
		CheckBox cb_17;
		CheckBox cb_18;
		CheckBox cb_19;
		CheckBox cb_20;
		CheckBox cb_21;
		CheckBox cb_22;
		CheckBox cb_23;

		GroupBox gb_MetaGroups;
		CheckBox cb_MetaEssenceShape;
		CheckBox cb_MetaEssence;
		CheckBox cb_MetaShape;

		Button bu_Clear;
		Button bu_Accept;

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
			this.bu_Accept = new System.Windows.Forms.Button();
			this.la_Val = new System.Windows.Forms.Label();
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
			this.co_Val = new System.Windows.Forms.ComboBox();
			this.gb_MetaGroups = new System.Windows.Forms.GroupBox();
			this.cb_MetaShape = new System.Windows.Forms.CheckBox();
			this.cb_MetaEssence = new System.Windows.Forms.CheckBox();
			this.cb_MetaEssenceShape = new System.Windows.Forms.CheckBox();
			this.bu_Clear = new System.Windows.Forms.Button();
			this.gb_MetaGroups.SuspendLayout();
			this.SuspendLayout();
			// 
			// cb_00
			// 
			this.cb_00.Location = new System.Drawing.Point(15, 25);
			this.cb_00.Margin = new System.Windows.Forms.Padding(0);
			this.cb_00.Name = "cb_00";
			this.cb_00.Size = new System.Drawing.Size(135, 20);
			this.cb_00.TabIndex = 2;
			this.cb_00.Text = "cb_00";
			this.cb_00.UseVisualStyleBackColor = true;
			this.cb_00.Visible = false;
			this.cb_00.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_01
			// 
			this.cb_01.Location = new System.Drawing.Point(15, 45);
			this.cb_01.Margin = new System.Windows.Forms.Padding(0);
			this.cb_01.Name = "cb_01";
			this.cb_01.Size = new System.Drawing.Size(135, 20);
			this.cb_01.TabIndex = 3;
			this.cb_01.Text = "cb_01";
			this.cb_01.UseVisualStyleBackColor = true;
			this.cb_01.Visible = false;
			this.cb_01.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_02
			// 
			this.cb_02.Location = new System.Drawing.Point(15, 65);
			this.cb_02.Margin = new System.Windows.Forms.Padding(0);
			this.cb_02.Name = "cb_02";
			this.cb_02.Size = new System.Drawing.Size(135, 20);
			this.cb_02.TabIndex = 4;
			this.cb_02.Text = "cb_02";
			this.cb_02.UseVisualStyleBackColor = true;
			this.cb_02.Visible = false;
			this.cb_02.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_03
			// 
			this.cb_03.Location = new System.Drawing.Point(15, 85);
			this.cb_03.Margin = new System.Windows.Forms.Padding(0);
			this.cb_03.Name = "cb_03";
			this.cb_03.Size = new System.Drawing.Size(135, 20);
			this.cb_03.TabIndex = 5;
			this.cb_03.Text = "cb_03";
			this.cb_03.UseVisualStyleBackColor = true;
			this.cb_03.Visible = false;
			this.cb_03.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_04
			// 
			this.cb_04.Location = new System.Drawing.Point(15, 105);
			this.cb_04.Margin = new System.Windows.Forms.Padding(0);
			this.cb_04.Name = "cb_04";
			this.cb_04.Size = new System.Drawing.Size(135, 20);
			this.cb_04.TabIndex = 6;
			this.cb_04.Text = "cb_04";
			this.cb_04.UseVisualStyleBackColor = true;
			this.cb_04.Visible = false;
			this.cb_04.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_05
			// 
			this.cb_05.Location = new System.Drawing.Point(15, 125);
			this.cb_05.Margin = new System.Windows.Forms.Padding(0);
			this.cb_05.Name = "cb_05";
			this.cb_05.Size = new System.Drawing.Size(135, 20);
			this.cb_05.TabIndex = 7;
			this.cb_05.Text = "cb_05";
			this.cb_05.UseVisualStyleBackColor = true;
			this.cb_05.Visible = false;
			this.cb_05.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_06
			// 
			this.cb_06.Location = new System.Drawing.Point(15, 145);
			this.cb_06.Margin = new System.Windows.Forms.Padding(0);
			this.cb_06.Name = "cb_06";
			this.cb_06.Size = new System.Drawing.Size(135, 20);
			this.cb_06.TabIndex = 8;
			this.cb_06.Text = "cb_06";
			this.cb_06.UseVisualStyleBackColor = true;
			this.cb_06.Visible = false;
			this.cb_06.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_07
			// 
			this.cb_07.Location = new System.Drawing.Point(15, 165);
			this.cb_07.Margin = new System.Windows.Forms.Padding(0);
			this.cb_07.Name = "cb_07";
			this.cb_07.Size = new System.Drawing.Size(135, 20);
			this.cb_07.TabIndex = 9;
			this.cb_07.Text = "cb_07";
			this.cb_07.UseVisualStyleBackColor = true;
			this.cb_07.Visible = false;
			this.cb_07.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// bu_Accept
			// 
			this.bu_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Accept.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bu_Accept.Location = new System.Drawing.Point(0, 214);
			this.bu_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Accept.Name = "bu_Accept";
			this.bu_Accept.Size = new System.Drawing.Size(414, 27);
			this.bu_Accept.TabIndex = 28;
			this.bu_Accept.Text = "accept";
			this.bu_Accept.UseVisualStyleBackColor = true;
			// 
			// la_Val
			// 
			this.la_Val.Dock = System.Windows.Forms.DockStyle.Top;
			this.la_Val.Location = new System.Drawing.Point(0, 0);
			this.la_Val.Margin = new System.Windows.Forms.Padding(0);
			this.la_Val.Name = "la_Val";
			this.la_Val.Size = new System.Drawing.Size(414, 20);
			this.la_Val.TabIndex = 0;
			this.la_Val.Text = "la_Val";
			this.la_Val.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// cb_15
			// 
			this.cb_15.Location = new System.Drawing.Point(155, 165);
			this.cb_15.Margin = new System.Windows.Forms.Padding(0);
			this.cb_15.Name = "cb_15";
			this.cb_15.Size = new System.Drawing.Size(125, 20);
			this.cb_15.TabIndex = 17;
			this.cb_15.Text = "cb_15";
			this.cb_15.UseVisualStyleBackColor = true;
			this.cb_15.Visible = false;
			this.cb_15.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_14
			// 
			this.cb_14.Location = new System.Drawing.Point(155, 145);
			this.cb_14.Margin = new System.Windows.Forms.Padding(0);
			this.cb_14.Name = "cb_14";
			this.cb_14.Size = new System.Drawing.Size(125, 20);
			this.cb_14.TabIndex = 16;
			this.cb_14.Text = "cb_14";
			this.cb_14.UseVisualStyleBackColor = true;
			this.cb_14.Visible = false;
			this.cb_14.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_13
			// 
			this.cb_13.Location = new System.Drawing.Point(155, 125);
			this.cb_13.Margin = new System.Windows.Forms.Padding(0);
			this.cb_13.Name = "cb_13";
			this.cb_13.Size = new System.Drawing.Size(125, 20);
			this.cb_13.TabIndex = 15;
			this.cb_13.Text = "cb_13";
			this.cb_13.UseVisualStyleBackColor = true;
			this.cb_13.Visible = false;
			this.cb_13.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_12
			// 
			this.cb_12.Location = new System.Drawing.Point(155, 105);
			this.cb_12.Margin = new System.Windows.Forms.Padding(0);
			this.cb_12.Name = "cb_12";
			this.cb_12.Size = new System.Drawing.Size(125, 20);
			this.cb_12.TabIndex = 14;
			this.cb_12.Text = "cb_12";
			this.cb_12.UseVisualStyleBackColor = true;
			this.cb_12.Visible = false;
			this.cb_12.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_11
			// 
			this.cb_11.Location = new System.Drawing.Point(155, 85);
			this.cb_11.Margin = new System.Windows.Forms.Padding(0);
			this.cb_11.Name = "cb_11";
			this.cb_11.Size = new System.Drawing.Size(125, 20);
			this.cb_11.TabIndex = 13;
			this.cb_11.Text = "cb_11";
			this.cb_11.UseVisualStyleBackColor = true;
			this.cb_11.Visible = false;
			this.cb_11.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_10
			// 
			this.cb_10.Location = new System.Drawing.Point(155, 65);
			this.cb_10.Margin = new System.Windows.Forms.Padding(0);
			this.cb_10.Name = "cb_10";
			this.cb_10.Size = new System.Drawing.Size(125, 20);
			this.cb_10.TabIndex = 12;
			this.cb_10.Text = "cb_10";
			this.cb_10.UseVisualStyleBackColor = true;
			this.cb_10.Visible = false;
			this.cb_10.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_09
			// 
			this.cb_09.Location = new System.Drawing.Point(155, 45);
			this.cb_09.Margin = new System.Windows.Forms.Padding(0);
			this.cb_09.Name = "cb_09";
			this.cb_09.Size = new System.Drawing.Size(125, 20);
			this.cb_09.TabIndex = 11;
			this.cb_09.Text = "cb_09";
			this.cb_09.UseVisualStyleBackColor = true;
			this.cb_09.Visible = false;
			this.cb_09.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_08
			// 
			this.cb_08.Location = new System.Drawing.Point(155, 25);
			this.cb_08.Margin = new System.Windows.Forms.Padding(0);
			this.cb_08.Name = "cb_08";
			this.cb_08.Size = new System.Drawing.Size(125, 20);
			this.cb_08.TabIndex = 10;
			this.cb_08.Text = "cb_08";
			this.cb_08.UseVisualStyleBackColor = true;
			this.cb_08.Visible = false;
			this.cb_08.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_23
			// 
			this.cb_23.Location = new System.Drawing.Point(285, 165);
			this.cb_23.Margin = new System.Windows.Forms.Padding(0);
			this.cb_23.Name = "cb_23";
			this.cb_23.Size = new System.Drawing.Size(125, 20);
			this.cb_23.TabIndex = 25;
			this.cb_23.Text = "cb_23";
			this.cb_23.UseVisualStyleBackColor = true;
			this.cb_23.Visible = false;
			this.cb_23.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_22
			// 
			this.cb_22.Location = new System.Drawing.Point(285, 145);
			this.cb_22.Margin = new System.Windows.Forms.Padding(0);
			this.cb_22.Name = "cb_22";
			this.cb_22.Size = new System.Drawing.Size(125, 20);
			this.cb_22.TabIndex = 24;
			this.cb_22.Text = "cb_22";
			this.cb_22.UseVisualStyleBackColor = true;
			this.cb_22.Visible = false;
			this.cb_22.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_21
			// 
			this.cb_21.Location = new System.Drawing.Point(285, 125);
			this.cb_21.Margin = new System.Windows.Forms.Padding(0);
			this.cb_21.Name = "cb_21";
			this.cb_21.Size = new System.Drawing.Size(125, 20);
			this.cb_21.TabIndex = 23;
			this.cb_21.Text = "cb_21";
			this.cb_21.UseVisualStyleBackColor = true;
			this.cb_21.Visible = false;
			this.cb_21.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_20
			// 
			this.cb_20.Location = new System.Drawing.Point(285, 105);
			this.cb_20.Margin = new System.Windows.Forms.Padding(0);
			this.cb_20.Name = "cb_20";
			this.cb_20.Size = new System.Drawing.Size(125, 20);
			this.cb_20.TabIndex = 22;
			this.cb_20.Text = "cb_20";
			this.cb_20.UseVisualStyleBackColor = true;
			this.cb_20.Visible = false;
			this.cb_20.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_19
			// 
			this.cb_19.Location = new System.Drawing.Point(285, 85);
			this.cb_19.Margin = new System.Windows.Forms.Padding(0);
			this.cb_19.Name = "cb_19";
			this.cb_19.Size = new System.Drawing.Size(125, 20);
			this.cb_19.TabIndex = 21;
			this.cb_19.Text = "cb_19";
			this.cb_19.UseVisualStyleBackColor = true;
			this.cb_19.Visible = false;
			this.cb_19.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_18
			// 
			this.cb_18.Location = new System.Drawing.Point(285, 65);
			this.cb_18.Margin = new System.Windows.Forms.Padding(0);
			this.cb_18.Name = "cb_18";
			this.cb_18.Size = new System.Drawing.Size(125, 20);
			this.cb_18.TabIndex = 20;
			this.cb_18.Text = "cb_18";
			this.cb_18.UseVisualStyleBackColor = true;
			this.cb_18.Visible = false;
			this.cb_18.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_17
			// 
			this.cb_17.Location = new System.Drawing.Point(285, 45);
			this.cb_17.Margin = new System.Windows.Forms.Padding(0);
			this.cb_17.Name = "cb_17";
			this.cb_17.Size = new System.Drawing.Size(125, 20);
			this.cb_17.TabIndex = 19;
			this.cb_17.Text = "cb_17";
			this.cb_17.UseVisualStyleBackColor = true;
			this.cb_17.Visible = false;
			this.cb_17.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_16
			// 
			this.cb_16.Location = new System.Drawing.Point(285, 25);
			this.cb_16.Margin = new System.Windows.Forms.Padding(0);
			this.cb_16.Name = "cb_16";
			this.cb_16.Size = new System.Drawing.Size(125, 20);
			this.cb_16.TabIndex = 18;
			this.cb_16.Text = "cb_16";
			this.cb_16.UseVisualStyleBackColor = true;
			this.cb_16.Visible = false;
			this.cb_16.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// co_Val
			// 
			this.co_Val.Dock = System.Windows.Forms.DockStyle.Top;
			this.co_Val.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.co_Val.FormattingEnabled = true;
			this.co_Val.Location = new System.Drawing.Point(0, 20);
			this.co_Val.Margin = new System.Windows.Forms.Padding(0);
			this.co_Val.MaxDropDownItems = 30;
			this.co_Val.Name = "co_Val";
			this.co_Val.Size = new System.Drawing.Size(414, 21);
			this.co_Val.TabIndex = 1;
			this.co_Val.Visible = false;
			this.co_Val.SelectedIndexChanged += new System.EventHandler(this.changed_Combobox);
			// 
			// gb_MetaGroups
			// 
			this.gb_MetaGroups.Controls.Add(this.cb_MetaShape);
			this.gb_MetaGroups.Controls.Add(this.cb_MetaEssence);
			this.gb_MetaGroups.Controls.Add(this.cb_MetaEssenceShape);
			this.gb_MetaGroups.Location = new System.Drawing.Point(73, 180);
			this.gb_MetaGroups.Margin = new System.Windows.Forms.Padding(0);
			this.gb_MetaGroups.Name = "gb_MetaGroups";
			this.gb_MetaGroups.Padding = new System.Windows.Forms.Padding(0);
			this.gb_MetaGroups.Size = new System.Drawing.Size(340, 32);
			this.gb_MetaGroups.TabIndex = 27;
			this.gb_MetaGroups.TabStop = false;
			this.gb_MetaGroups.Visible = false;
			// 
			// cb_MetaShape
			// 
			this.cb_MetaShape.Location = new System.Drawing.Point(253, 10);
			this.cb_MetaShape.Margin = new System.Windows.Forms.Padding(0);
			this.cb_MetaShape.Name = "cb_MetaShape";
			this.cb_MetaShape.Size = new System.Drawing.Size(82, 20);
			this.cb_MetaShape.TabIndex = 2;
			this.cb_MetaShape.Text = "Shape";
			this.cb_MetaShape.UseVisualStyleBackColor = true;
			this.cb_MetaShape.CheckedChanged += new System.EventHandler(this.changed_MetamagicGroup);
			// 
			// cb_MetaEssence
			// 
			this.cb_MetaEssence.Location = new System.Drawing.Point(155, 10);
			this.cb_MetaEssence.Margin = new System.Windows.Forms.Padding(0);
			this.cb_MetaEssence.Name = "cb_MetaEssence";
			this.cb_MetaEssence.Size = new System.Drawing.Size(95, 20);
			this.cb_MetaEssence.TabIndex = 1;
			this.cb_MetaEssence.Text = "Essence";
			this.cb_MetaEssence.UseVisualStyleBackColor = true;
			this.cb_MetaEssence.CheckedChanged += new System.EventHandler(this.changed_MetamagicGroup);
			// 
			// cb_MetaEssenceShape
			// 
			this.cb_MetaEssenceShape.Location = new System.Drawing.Point(6, 10);
			this.cb_MetaEssenceShape.Margin = new System.Windows.Forms.Padding(0);
			this.cb_MetaEssenceShape.Name = "cb_MetaEssenceShape";
			this.cb_MetaEssenceShape.Size = new System.Drawing.Size(146, 20);
			this.cb_MetaEssenceShape.TabIndex = 0;
			this.cb_MetaEssenceShape.Text = "Essence + Shape";
			this.cb_MetaEssenceShape.UseVisualStyleBackColor = true;
			this.cb_MetaEssenceShape.CheckedChanged += new System.EventHandler(this.changed_MetamagicGroup);
			// 
			// bu_Clear
			// 
			this.bu_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Clear.Location = new System.Drawing.Point(5, 190);
			this.bu_Clear.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Clear.Name = "bu_Clear";
			this.bu_Clear.Size = new System.Drawing.Size(65, 22);
			this.bu_Clear.TabIndex = 26;
			this.bu_Clear.Text = "clear";
			this.bu_Clear.UseVisualStyleBackColor = true;
			this.bu_Clear.Click += new System.EventHandler(this.click_Clear);
			// 
			// InfoInputSpells
			// 
			this.AcceptButton = this.bu_Accept;
			this.ClientSize = new System.Drawing.Size(414, 241);
			this.Controls.Add(this.bu_Clear);
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
			this.Controls.Add(this.cb_07);
			this.Controls.Add(this.cb_06);
			this.Controls.Add(this.cb_05);
			this.Controls.Add(this.cb_04);
			this.Controls.Add(this.cb_03);
			this.Controls.Add(this.cb_02);
			this.Controls.Add(this.cb_01);
			this.Controls.Add(this.cb_00);
			this.Controls.Add(this.co_Val);
			this.Controls.Add(this.la_Val);
			this.Controls.Add(this.bu_Accept);
			this.Controls.Add(this.gb_MetaGroups);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InfoInputSpells";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.gb_MetaGroups.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
