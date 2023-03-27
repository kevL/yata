using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ColorOptionsDialog
	{
		#region Designer
		GroupBox gb_Colors;
		Label la_01;
		Panel pa_01;
		Label la_02;
		Panel pa_02;
		Panel pa_02_t;
		Label la_03;
		Panel pa_03;
		Panel pa_03_t;
		Label la_04;
		Panel pa_04;
		Panel pa_04_t;
		Label la_05;
		Panel pa_05;
		Panel pa_05_t;
		Label la_07;
		Panel pa_07;
		Panel pa_07_t;
		Label la_08;
		Panel pa_08;
		Panel pa_08_t;
		Label la_09;
		Panel pa_09;
		Panel pa_09_t;
		Label la_10;
		Panel pa_10;
		Label la_11;
		Panel pa_11;
		Label la_12;
		Panel pa_12;
		Label la_13;
		Panel pa_13;
		Label la_14;
		Panel pa_14;
		Panel pa_14_t;
		Label la_15;
		Panel pa_15;
		Label la_16;
		Panel pa_16;
		Label la_17;
		Panel pa_17;
		Label la_19;
		Panel pa_19;
		Label la_21;
		Panel pa_21;
		Label la_22;
		Panel pa_22;
		Label la_23;
		Panel pa_23;
		Label la_24;
		Panel pa_24;
		Label la_25;
		Panel pa_25;
		Label la_26;
		Panel pa_26;
		Label la_27;
		Panel pa_27;
		Label la_28;
		Panel pa_28;
		Label la_29;
		Panel pa_29;
		Label la_30;
		Panel pa_30;
		Label la_31;
		Panel pa_31;
		Label la_32;
		Panel pa_32;
		Label la_33;
		Panel pa_33;
		Label la_34;
		Panel pa_34;
		Label la_35;
		Panel pa_35;
		Label la_36;
		Panel pa_36;
		Label la_37;
		Panel pa_37;
		Panel pa_37_t;
		Label la_38;
		Panel pa_38;
		Label la_40;
		Panel pa_40;
		Label la_41;
		Panel pa_41;
		Label la_42;
		Panel pa_42;
		Label la_43;
		Panel pa_43;
		Label la_44;
		Panel pa_44;
		Label la_45;
		Panel pa_45;
		Label la_Help;

		Panel pa_bot;
		Button bu_Delete;
		Button bu_Defaults;
		Button bu_Save;
		Button bu_Cancel;

		ToolTip toolTip1;

		IContainer components;


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.gb_Colors = new System.Windows.Forms.GroupBox();
			this.la_01 = new System.Windows.Forms.Label();
			this.pa_01 = new System.Windows.Forms.Panel();
			this.la_02 = new System.Windows.Forms.Label();
			this.pa_02 = new System.Windows.Forms.Panel();
			this.pa_02_t = new System.Windows.Forms.Panel();
			this.la_03 = new System.Windows.Forms.Label();
			this.pa_03 = new System.Windows.Forms.Panel();
			this.pa_03_t = new System.Windows.Forms.Panel();
			this.la_04 = new System.Windows.Forms.Label();
			this.pa_04 = new System.Windows.Forms.Panel();
			this.pa_04_t = new System.Windows.Forms.Panel();
			this.la_05 = new System.Windows.Forms.Label();
			this.pa_05 = new System.Windows.Forms.Panel();
			this.pa_05_t = new System.Windows.Forms.Panel();
			this.la_07 = new System.Windows.Forms.Label();
			this.pa_07 = new System.Windows.Forms.Panel();
			this.pa_07_t = new System.Windows.Forms.Panel();
			this.la_08 = new System.Windows.Forms.Label();
			this.pa_08 = new System.Windows.Forms.Panel();
			this.pa_08_t = new System.Windows.Forms.Panel();
			this.la_09 = new System.Windows.Forms.Label();
			this.pa_09 = new System.Windows.Forms.Panel();
			this.pa_09_t = new System.Windows.Forms.Panel();
			this.la_10 = new System.Windows.Forms.Label();
			this.pa_10 = new System.Windows.Forms.Panel();
			this.la_11 = new System.Windows.Forms.Label();
			this.pa_11 = new System.Windows.Forms.Panel();
			this.la_12 = new System.Windows.Forms.Label();
			this.pa_12 = new System.Windows.Forms.Panel();
			this.la_13 = new System.Windows.Forms.Label();
			this.pa_13 = new System.Windows.Forms.Panel();
			this.la_14 = new System.Windows.Forms.Label();
			this.pa_14 = new System.Windows.Forms.Panel();
			this.pa_14_t = new System.Windows.Forms.Panel();
			this.la_15 = new System.Windows.Forms.Label();
			this.pa_15 = new System.Windows.Forms.Panel();
			this.la_16 = new System.Windows.Forms.Label();
			this.pa_16 = new System.Windows.Forms.Panel();
			this.la_17 = new System.Windows.Forms.Label();
			this.pa_17 = new System.Windows.Forms.Panel();
			this.la_19 = new System.Windows.Forms.Label();
			this.pa_19 = new System.Windows.Forms.Panel();
			this.la_21 = new System.Windows.Forms.Label();
			this.pa_21 = new System.Windows.Forms.Panel();
			this.la_22 = new System.Windows.Forms.Label();
			this.pa_22 = new System.Windows.Forms.Panel();
			this.la_23 = new System.Windows.Forms.Label();
			this.pa_23 = new System.Windows.Forms.Panel();
			this.la_24 = new System.Windows.Forms.Label();
			this.pa_24 = new System.Windows.Forms.Panel();
			this.la_25 = new System.Windows.Forms.Label();
			this.pa_25 = new System.Windows.Forms.Panel();
			this.la_26 = new System.Windows.Forms.Label();
			this.pa_26 = new System.Windows.Forms.Panel();
			this.la_27 = new System.Windows.Forms.Label();
			this.pa_27 = new System.Windows.Forms.Panel();
			this.la_28 = new System.Windows.Forms.Label();
			this.pa_28 = new System.Windows.Forms.Panel();
			this.la_29 = new System.Windows.Forms.Label();
			this.pa_29 = new System.Windows.Forms.Panel();
			this.la_30 = new System.Windows.Forms.Label();
			this.pa_30 = new System.Windows.Forms.Panel();
			this.la_31 = new System.Windows.Forms.Label();
			this.pa_31 = new System.Windows.Forms.Panel();
			this.la_32 = new System.Windows.Forms.Label();
			this.pa_32 = new System.Windows.Forms.Panel();
			this.la_33 = new System.Windows.Forms.Label();
			this.pa_33 = new System.Windows.Forms.Panel();
			this.la_34 = new System.Windows.Forms.Label();
			this.pa_34 = new System.Windows.Forms.Panel();
			this.la_35 = new System.Windows.Forms.Label();
			this.pa_35 = new System.Windows.Forms.Panel();
			this.la_36 = new System.Windows.Forms.Label();
			this.pa_36 = new System.Windows.Forms.Panel();
			this.la_37 = new System.Windows.Forms.Label();
			this.pa_37 = new System.Windows.Forms.Panel();
			this.pa_37_t = new System.Windows.Forms.Panel();
			this.la_38 = new System.Windows.Forms.Label();
			this.pa_38 = new System.Windows.Forms.Panel();
			this.la_40 = new System.Windows.Forms.Label();
			this.pa_40 = new System.Windows.Forms.Panel();
			this.la_41 = new System.Windows.Forms.Label();
			this.pa_41 = new System.Windows.Forms.Panel();
			this.la_42 = new System.Windows.Forms.Label();
			this.pa_42 = new System.Windows.Forms.Panel();
			this.la_43 = new System.Windows.Forms.Label();
			this.pa_43 = new System.Windows.Forms.Panel();
			this.la_44 = new System.Windows.Forms.Label();
			this.pa_44 = new System.Windows.Forms.Panel();
			this.la_45 = new System.Windows.Forms.Label();
			this.pa_45 = new System.Windows.Forms.Panel();
			this.la_Help = new System.Windows.Forms.Label();
			this.pa_bot = new System.Windows.Forms.Panel();
			this.bu_Save = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Defaults = new System.Windows.Forms.Button();
			this.bu_Delete = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.gb_Colors.SuspendLayout();
			this.pa_bot.SuspendLayout();
			this.SuspendLayout();
			// 
			// gb_Colors
			// 
			this.gb_Colors.Controls.Add(this.la_01);
			this.gb_Colors.Controls.Add(this.pa_01);
			this.gb_Colors.Controls.Add(this.la_02);
			this.gb_Colors.Controls.Add(this.pa_02);
			this.gb_Colors.Controls.Add(this.pa_02_t);
			this.gb_Colors.Controls.Add(this.la_03);
			this.gb_Colors.Controls.Add(this.pa_03);
			this.gb_Colors.Controls.Add(this.pa_03_t);
			this.gb_Colors.Controls.Add(this.la_04);
			this.gb_Colors.Controls.Add(this.pa_04);
			this.gb_Colors.Controls.Add(this.pa_04_t);
			this.gb_Colors.Controls.Add(this.la_05);
			this.gb_Colors.Controls.Add(this.pa_05);
			this.gb_Colors.Controls.Add(this.pa_05_t);
			this.gb_Colors.Controls.Add(this.la_07);
			this.gb_Colors.Controls.Add(this.pa_07);
			this.gb_Colors.Controls.Add(this.pa_07_t);
			this.gb_Colors.Controls.Add(this.la_08);
			this.gb_Colors.Controls.Add(this.pa_08);
			this.gb_Colors.Controls.Add(this.pa_08_t);
			this.gb_Colors.Controls.Add(this.la_09);
			this.gb_Colors.Controls.Add(this.pa_09);
			this.gb_Colors.Controls.Add(this.pa_09_t);
			this.gb_Colors.Controls.Add(this.la_10);
			this.gb_Colors.Controls.Add(this.pa_10);
			this.gb_Colors.Controls.Add(this.la_11);
			this.gb_Colors.Controls.Add(this.pa_11);
			this.gb_Colors.Controls.Add(this.la_12);
			this.gb_Colors.Controls.Add(this.pa_12);
			this.gb_Colors.Controls.Add(this.la_13);
			this.gb_Colors.Controls.Add(this.pa_13);
			this.gb_Colors.Controls.Add(this.la_14);
			this.gb_Colors.Controls.Add(this.pa_14);
			this.gb_Colors.Controls.Add(this.pa_14_t);
			this.gb_Colors.Controls.Add(this.la_15);
			this.gb_Colors.Controls.Add(this.pa_15);
			this.gb_Colors.Controls.Add(this.la_16);
			this.gb_Colors.Controls.Add(this.pa_16);
			this.gb_Colors.Controls.Add(this.la_17);
			this.gb_Colors.Controls.Add(this.pa_17);
			this.gb_Colors.Controls.Add(this.la_19);
			this.gb_Colors.Controls.Add(this.pa_19);
			this.gb_Colors.Controls.Add(this.la_21);
			this.gb_Colors.Controls.Add(this.pa_21);
			this.gb_Colors.Controls.Add(this.la_22);
			this.gb_Colors.Controls.Add(this.pa_22);
			this.gb_Colors.Controls.Add(this.la_23);
			this.gb_Colors.Controls.Add(this.pa_23);
			this.gb_Colors.Controls.Add(this.la_24);
			this.gb_Colors.Controls.Add(this.pa_24);
			this.gb_Colors.Controls.Add(this.la_25);
			this.gb_Colors.Controls.Add(this.pa_25);
			this.gb_Colors.Controls.Add(this.la_26);
			this.gb_Colors.Controls.Add(this.pa_26);
			this.gb_Colors.Controls.Add(this.la_27);
			this.gb_Colors.Controls.Add(this.pa_27);
			this.gb_Colors.Controls.Add(this.la_28);
			this.gb_Colors.Controls.Add(this.pa_28);
			this.gb_Colors.Controls.Add(this.la_29);
			this.gb_Colors.Controls.Add(this.pa_29);
			this.gb_Colors.Controls.Add(this.la_30);
			this.gb_Colors.Controls.Add(this.pa_30);
			this.gb_Colors.Controls.Add(this.la_31);
			this.gb_Colors.Controls.Add(this.pa_31);
			this.gb_Colors.Controls.Add(this.la_32);
			this.gb_Colors.Controls.Add(this.pa_32);
			this.gb_Colors.Controls.Add(this.la_33);
			this.gb_Colors.Controls.Add(this.pa_33);
			this.gb_Colors.Controls.Add(this.la_34);
			this.gb_Colors.Controls.Add(this.pa_34);
			this.gb_Colors.Controls.Add(this.la_35);
			this.gb_Colors.Controls.Add(this.pa_35);
			this.gb_Colors.Controls.Add(this.la_36);
			this.gb_Colors.Controls.Add(this.pa_36);
			this.gb_Colors.Controls.Add(this.la_37);
			this.gb_Colors.Controls.Add(this.pa_37);
			this.gb_Colors.Controls.Add(this.pa_37_t);
			this.gb_Colors.Controls.Add(this.la_38);
			this.gb_Colors.Controls.Add(this.pa_38);
			this.gb_Colors.Controls.Add(this.la_40);
			this.gb_Colors.Controls.Add(this.pa_40);
			this.gb_Colors.Controls.Add(this.la_41);
			this.gb_Colors.Controls.Add(this.pa_41);
			this.gb_Colors.Controls.Add(this.la_42);
			this.gb_Colors.Controls.Add(this.pa_42);
			this.gb_Colors.Controls.Add(this.la_43);
			this.gb_Colors.Controls.Add(this.pa_43);
			this.gb_Colors.Controls.Add(this.la_44);
			this.gb_Colors.Controls.Add(this.pa_44);
			this.gb_Colors.Controls.Add(this.la_45);
			this.gb_Colors.Controls.Add(this.pa_45);
			this.gb_Colors.Controls.Add(this.la_Help);
			this.gb_Colors.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gb_Colors.Location = new System.Drawing.Point(0, 0);
			this.gb_Colors.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Colors.Name = "gb_Colors";
			this.gb_Colors.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Colors.Size = new System.Drawing.Size(667, 741);
			this.gb_Colors.TabIndex = 0;
			this.gb_Colors.TabStop = false;
			// 
			// la_01
			// 
			this.la_01.Location = new System.Drawing.Point(4, 17);
			this.la_01.Margin = new System.Windows.Forms.Padding(0);
			this.la_01.Name = "la_01";
			this.la_01.Size = new System.Drawing.Size(183, 16);
			this.la_01.TabIndex = 0;
			this.la_01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_01
			// 
			this.pa_01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_01.Location = new System.Drawing.Point(262, 12);
			this.pa_01.Margin = new System.Windows.Forms.Padding(0);
			this.pa_01.Name = "pa_01";
			this.pa_01.Size = new System.Drawing.Size(69, 24);
			this.pa_01.TabIndex = 1;
			this.pa_01.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_01.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_02
			// 
			this.la_02.Location = new System.Drawing.Point(4, 67);
			this.la_02.Margin = new System.Windows.Forms.Padding(0);
			this.la_02.Name = "la_02";
			this.la_02.Size = new System.Drawing.Size(183, 16);
			this.la_02.TabIndex = 4;
			this.la_02.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_02
			// 
			this.pa_02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_02.Location = new System.Drawing.Point(191, 62);
			this.pa_02.Margin = new System.Windows.Forms.Padding(0);
			this.pa_02.Name = "pa_02";
			this.pa_02.Size = new System.Drawing.Size(69, 24);
			this.pa_02.TabIndex = 5;
			this.pa_02.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_02.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// pa_02_t
			// 
			this.pa_02_t.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_02_t.Location = new System.Drawing.Point(262, 62);
			this.pa_02_t.Margin = new System.Windows.Forms.Padding(0);
			this.pa_02_t.Name = "pa_02_t";
			this.pa_02_t.Size = new System.Drawing.Size(69, 24);
			this.pa_02_t.TabIndex = 91;
			this.pa_02_t.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_02_t.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_03
			// 
			this.la_03.Location = new System.Drawing.Point(4, 92);
			this.la_03.Margin = new System.Windows.Forms.Padding(0);
			this.la_03.Name = "la_03";
			this.la_03.Size = new System.Drawing.Size(183, 16);
			this.la_03.TabIndex = 6;
			this.la_03.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_03
			// 
			this.pa_03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_03.Location = new System.Drawing.Point(191, 87);
			this.pa_03.Margin = new System.Windows.Forms.Padding(0);
			this.pa_03.Name = "pa_03";
			this.pa_03.Size = new System.Drawing.Size(69, 24);
			this.pa_03.TabIndex = 7;
			this.pa_03.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_03.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// pa_03_t
			// 
			this.pa_03_t.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_03_t.Location = new System.Drawing.Point(262, 87);
			this.pa_03_t.Margin = new System.Windows.Forms.Padding(0);
			this.pa_03_t.Name = "pa_03_t";
			this.pa_03_t.Size = new System.Drawing.Size(69, 24);
			this.pa_03_t.TabIndex = 92;
			this.pa_03_t.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_03_t.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_04
			// 
			this.la_04.Location = new System.Drawing.Point(4, 117);
			this.la_04.Margin = new System.Windows.Forms.Padding(0);
			this.la_04.Name = "la_04";
			this.la_04.Size = new System.Drawing.Size(183, 16);
			this.la_04.TabIndex = 8;
			this.la_04.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_04
			// 
			this.pa_04.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_04.Location = new System.Drawing.Point(191, 112);
			this.pa_04.Margin = new System.Windows.Forms.Padding(0);
			this.pa_04.Name = "pa_04";
			this.pa_04.Size = new System.Drawing.Size(69, 24);
			this.pa_04.TabIndex = 9;
			this.pa_04.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_04.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// pa_04_t
			// 
			this.pa_04_t.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_04_t.Location = new System.Drawing.Point(262, 112);
			this.pa_04_t.Margin = new System.Windows.Forms.Padding(0);
			this.pa_04_t.Name = "pa_04_t";
			this.pa_04_t.Size = new System.Drawing.Size(69, 24);
			this.pa_04_t.TabIndex = 93;
			this.pa_04_t.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_04_t.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_05
			// 
			this.la_05.Location = new System.Drawing.Point(4, 142);
			this.la_05.Margin = new System.Windows.Forms.Padding(0);
			this.la_05.Name = "la_05";
			this.la_05.Size = new System.Drawing.Size(183, 16);
			this.la_05.TabIndex = 10;
			this.la_05.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_05
			// 
			this.pa_05.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_05.Location = new System.Drawing.Point(191, 137);
			this.pa_05.Margin = new System.Windows.Forms.Padding(0);
			this.pa_05.Name = "pa_05";
			this.pa_05.Size = new System.Drawing.Size(69, 24);
			this.pa_05.TabIndex = 11;
			this.pa_05.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_05.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// pa_05_t
			// 
			this.pa_05_t.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_05_t.Location = new System.Drawing.Point(262, 137);
			this.pa_05_t.Margin = new System.Windows.Forms.Padding(0);
			this.pa_05_t.Name = "pa_05_t";
			this.pa_05_t.Size = new System.Drawing.Size(69, 24);
			this.pa_05_t.TabIndex = 94;
			this.pa_05_t.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_05_t.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_07
			// 
			this.la_07.Location = new System.Drawing.Point(4, 252);
			this.la_07.Margin = new System.Windows.Forms.Padding(0);
			this.la_07.Name = "la_07";
			this.la_07.Size = new System.Drawing.Size(183, 16);
			this.la_07.TabIndex = 18;
			this.la_07.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_07
			// 
			this.pa_07.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_07.Location = new System.Drawing.Point(191, 247);
			this.pa_07.Margin = new System.Windows.Forms.Padding(0);
			this.pa_07.Name = "pa_07";
			this.pa_07.Size = new System.Drawing.Size(69, 24);
			this.pa_07.TabIndex = 19;
			this.pa_07.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_07.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// pa_07_t
			// 
			this.pa_07_t.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_07_t.Location = new System.Drawing.Point(262, 247);
			this.pa_07_t.Margin = new System.Windows.Forms.Padding(0);
			this.pa_07_t.Name = "pa_07_t";
			this.pa_07_t.Size = new System.Drawing.Size(69, 24);
			this.pa_07_t.TabIndex = 15;
			this.pa_07_t.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_07_t.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_08
			// 
			this.la_08.Location = new System.Drawing.Point(4, 327);
			this.la_08.Margin = new System.Windows.Forms.Padding(0);
			this.la_08.Name = "la_08";
			this.la_08.Size = new System.Drawing.Size(183, 16);
			this.la_08.TabIndex = 24;
			this.la_08.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_08
			// 
			this.pa_08.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_08.Location = new System.Drawing.Point(191, 322);
			this.pa_08.Margin = new System.Windows.Forms.Padding(0);
			this.pa_08.Name = "pa_08";
			this.pa_08.Size = new System.Drawing.Size(69, 24);
			this.pa_08.TabIndex = 25;
			this.pa_08.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_08.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// pa_08_t
			// 
			this.pa_08_t.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_08_t.Location = new System.Drawing.Point(262, 322);
			this.pa_08_t.Margin = new System.Windows.Forms.Padding(0);
			this.pa_08_t.Name = "pa_08_t";
			this.pa_08_t.Size = new System.Drawing.Size(69, 24);
			this.pa_08_t.TabIndex = 21;
			this.pa_08_t.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_08_t.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_09
			// 
			this.la_09.Location = new System.Drawing.Point(4, 587);
			this.la_09.Margin = new System.Windows.Forms.Padding(0);
			this.la_09.Name = "la_09";
			this.la_09.Size = new System.Drawing.Size(183, 16);
			this.la_09.TabIndex = 36;
			this.la_09.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_09
			// 
			this.pa_09.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_09.Location = new System.Drawing.Point(191, 582);
			this.pa_09.Margin = new System.Windows.Forms.Padding(0);
			this.pa_09.Name = "pa_09";
			this.pa_09.Size = new System.Drawing.Size(69, 24);
			this.pa_09.TabIndex = 37;
			this.pa_09.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_09.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// pa_09_t
			// 
			this.pa_09_t.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_09_t.Location = new System.Drawing.Point(262, 582);
			this.pa_09_t.Margin = new System.Windows.Forms.Padding(0);
			this.pa_09_t.Name = "pa_09_t";
			this.pa_09_t.Size = new System.Drawing.Size(69, 24);
			this.pa_09_t.TabIndex = 29;
			this.pa_09_t.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_09_t.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_10
			// 
			this.la_10.Location = new System.Drawing.Point(368, 16);
			this.la_10.Margin = new System.Windows.Forms.Padding(0);
			this.la_10.Name = "la_10";
			this.la_10.Size = new System.Drawing.Size(139, 16);
			this.la_10.TabIndex = 42;
			this.la_10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_10
			// 
			this.pa_10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_10.Location = new System.Drawing.Point(581, 11);
			this.pa_10.Margin = new System.Windows.Forms.Padding(0);
			this.pa_10.Name = "pa_10";
			this.pa_10.Size = new System.Drawing.Size(69, 24);
			this.pa_10.TabIndex = 43;
			this.pa_10.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_10.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_11
			// 
			this.la_11.Location = new System.Drawing.Point(368, 66);
			this.la_11.Margin = new System.Windows.Forms.Padding(0);
			this.la_11.Name = "la_11";
			this.la_11.Size = new System.Drawing.Size(139, 16);
			this.la_11.TabIndex = 46;
			this.la_11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_11
			// 
			this.pa_11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_11.Location = new System.Drawing.Point(510, 61);
			this.pa_11.Margin = new System.Windows.Forms.Padding(0);
			this.pa_11.Name = "pa_11";
			this.pa_11.Size = new System.Drawing.Size(69, 24);
			this.pa_11.TabIndex = 47;
			this.pa_11.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_11.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_12
			// 
			this.la_12.Location = new System.Drawing.Point(368, 151);
			this.la_12.Margin = new System.Windows.Forms.Padding(0);
			this.la_12.Name = "la_12";
			this.la_12.Size = new System.Drawing.Size(139, 16);
			this.la_12.TabIndex = 52;
			this.la_12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_12
			// 
			this.pa_12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_12.Location = new System.Drawing.Point(581, 146);
			this.pa_12.Margin = new System.Windows.Forms.Padding(0);
			this.pa_12.Name = "pa_12";
			this.pa_12.Size = new System.Drawing.Size(69, 24);
			this.pa_12.TabIndex = 53;
			this.pa_12.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_12.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_13
			// 
			this.la_13.Location = new System.Drawing.Point(368, 226);
			this.la_13.Margin = new System.Windows.Forms.Padding(0);
			this.la_13.Name = "la_13";
			this.la_13.Size = new System.Drawing.Size(139, 16);
			this.la_13.TabIndex = 58;
			this.la_13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_13
			// 
			this.pa_13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_13.Location = new System.Drawing.Point(510, 221);
			this.pa_13.Margin = new System.Windows.Forms.Padding(0);
			this.pa_13.Name = "pa_13";
			this.pa_13.Size = new System.Drawing.Size(69, 24);
			this.pa_13.TabIndex = 59;
			this.pa_13.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_13.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_14
			// 
			this.la_14.Location = new System.Drawing.Point(368, 336);
			this.la_14.Margin = new System.Windows.Forms.Padding(0);
			this.la_14.Name = "la_14";
			this.la_14.Size = new System.Drawing.Size(139, 16);
			this.la_14.TabIndex = 66;
			this.la_14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_14
			// 
			this.pa_14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_14.Location = new System.Drawing.Point(510, 331);
			this.pa_14.Margin = new System.Windows.Forms.Padding(0);
			this.pa_14.Name = "pa_14";
			this.pa_14.Size = new System.Drawing.Size(69, 24);
			this.pa_14.TabIndex = 67;
			this.pa_14.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_14.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// pa_14_t
			// 
			this.pa_14_t.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_14_t.Location = new System.Drawing.Point(581, 331);
			this.pa_14_t.Margin = new System.Windows.Forms.Padding(0);
			this.pa_14_t.Name = "pa_14_t";
			this.pa_14_t.Size = new System.Drawing.Size(69, 24);
			this.pa_14_t.TabIndex = 65;
			this.pa_14_t.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_14_t.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_15
			// 
			this.la_15.Location = new System.Drawing.Point(4, 42);
			this.la_15.Margin = new System.Windows.Forms.Padding(0);
			this.la_15.Name = "la_15";
			this.la_15.Size = new System.Drawing.Size(183, 16);
			this.la_15.TabIndex = 2;
			this.la_15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_15
			// 
			this.pa_15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_15.Location = new System.Drawing.Point(191, 37);
			this.pa_15.Margin = new System.Windows.Forms.Padding(0);
			this.pa_15.Name = "pa_15";
			this.pa_15.Size = new System.Drawing.Size(69, 24);
			this.pa_15.TabIndex = 3;
			this.pa_15.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_15.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_16
			// 
			this.la_16.Location = new System.Drawing.Point(4, 227);
			this.la_16.Margin = new System.Windows.Forms.Padding(0);
			this.la_16.Name = "la_16";
			this.la_16.Size = new System.Drawing.Size(183, 16);
			this.la_16.TabIndex = 16;
			this.la_16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_16
			// 
			this.pa_16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_16.Location = new System.Drawing.Point(191, 222);
			this.pa_16.Margin = new System.Windows.Forms.Padding(0);
			this.pa_16.Name = "pa_16";
			this.pa_16.Size = new System.Drawing.Size(69, 24);
			this.pa_16.TabIndex = 17;
			this.pa_16.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_16.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_17
			// 
			this.la_17.Location = new System.Drawing.Point(4, 302);
			this.la_17.Margin = new System.Windows.Forms.Padding(0);
			this.la_17.Name = "la_17";
			this.la_17.Size = new System.Drawing.Size(183, 16);
			this.la_17.TabIndex = 22;
			this.la_17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_17
			// 
			this.pa_17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_17.Location = new System.Drawing.Point(191, 297);
			this.pa_17.Margin = new System.Windows.Forms.Padding(0);
			this.pa_17.Name = "pa_17";
			this.pa_17.Size = new System.Drawing.Size(69, 24);
			this.pa_17.TabIndex = 23;
			this.pa_17.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_17.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_19
			// 
			this.la_19.Location = new System.Drawing.Point(368, 41);
			this.la_19.Margin = new System.Windows.Forms.Padding(0);
			this.la_19.Name = "la_19";
			this.la_19.Size = new System.Drawing.Size(139, 16);
			this.la_19.TabIndex = 44;
			this.la_19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_19
			// 
			this.pa_19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_19.Location = new System.Drawing.Point(510, 36);
			this.pa_19.Margin = new System.Windows.Forms.Padding(0);
			this.pa_19.Name = "pa_19";
			this.pa_19.Size = new System.Drawing.Size(69, 24);
			this.pa_19.TabIndex = 45;
			this.pa_19.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_19.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_21
			// 
			this.la_21.Location = new System.Drawing.Point(4, 562);
			this.la_21.Margin = new System.Windows.Forms.Padding(0);
			this.la_21.Name = "la_21";
			this.la_21.Size = new System.Drawing.Size(183, 16);
			this.la_21.TabIndex = 34;
			this.la_21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_21
			// 
			this.pa_21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_21.Location = new System.Drawing.Point(191, 557);
			this.pa_21.Margin = new System.Windows.Forms.Padding(0);
			this.pa_21.Name = "pa_21";
			this.pa_21.Size = new System.Drawing.Size(69, 24);
			this.pa_21.TabIndex = 35;
			this.pa_21.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_21.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_22
			// 
			this.la_22.Location = new System.Drawing.Point(368, 176);
			this.la_22.Margin = new System.Windows.Forms.Padding(0);
			this.la_22.Name = "la_22";
			this.la_22.Size = new System.Drawing.Size(139, 16);
			this.la_22.TabIndex = 54;
			this.la_22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_22
			// 
			this.pa_22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_22.Location = new System.Drawing.Point(510, 171);
			this.pa_22.Margin = new System.Windows.Forms.Padding(0);
			this.pa_22.Name = "pa_22";
			this.pa_22.Size = new System.Drawing.Size(69, 24);
			this.pa_22.TabIndex = 55;
			this.pa_22.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_22.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_23
			// 
			this.la_23.Location = new System.Drawing.Point(368, 201);
			this.la_23.Margin = new System.Windows.Forms.Padding(0);
			this.la_23.Name = "la_23";
			this.la_23.Size = new System.Drawing.Size(139, 16);
			this.la_23.TabIndex = 56;
			this.la_23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_23
			// 
			this.pa_23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_23.Location = new System.Drawing.Point(510, 196);
			this.pa_23.Margin = new System.Windows.Forms.Padding(0);
			this.pa_23.Name = "pa_23";
			this.pa_23.Size = new System.Drawing.Size(69, 24);
			this.pa_23.TabIndex = 57;
			this.pa_23.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_23.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_24
			// 
			this.la_24.Location = new System.Drawing.Point(368, 371);
			this.la_24.Margin = new System.Windows.Forms.Padding(0);
			this.la_24.Name = "la_24";
			this.la_24.Size = new System.Drawing.Size(139, 16);
			this.la_24.TabIndex = 68;
			this.la_24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_24
			// 
			this.pa_24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_24.Location = new System.Drawing.Point(510, 366);
			this.pa_24.Margin = new System.Windows.Forms.Padding(0);
			this.pa_24.Name = "pa_24";
			this.pa_24.Size = new System.Drawing.Size(69, 24);
			this.pa_24.TabIndex = 69;
			this.pa_24.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_24.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_25
			// 
			this.la_25.Location = new System.Drawing.Point(368, 396);
			this.la_25.Margin = new System.Windows.Forms.Padding(0);
			this.la_25.Name = "la_25";
			this.la_25.Size = new System.Drawing.Size(139, 16);
			this.la_25.TabIndex = 70;
			this.la_25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_25
			// 
			this.pa_25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_25.Location = new System.Drawing.Point(510, 391);
			this.pa_25.Margin = new System.Windows.Forms.Padding(0);
			this.pa_25.Name = "pa_25";
			this.pa_25.Size = new System.Drawing.Size(69, 24);
			this.pa_25.TabIndex = 71;
			this.pa_25.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_25.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_26
			// 
			this.la_26.Location = new System.Drawing.Point(368, 421);
			this.la_26.Margin = new System.Windows.Forms.Padding(0);
			this.la_26.Name = "la_26";
			this.la_26.Size = new System.Drawing.Size(139, 16);
			this.la_26.TabIndex = 72;
			this.la_26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_26
			// 
			this.pa_26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_26.Location = new System.Drawing.Point(510, 416);
			this.pa_26.Margin = new System.Windows.Forms.Padding(0);
			this.pa_26.Name = "pa_26";
			this.pa_26.Size = new System.Drawing.Size(69, 24);
			this.pa_26.TabIndex = 73;
			this.pa_26.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_26.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_27
			// 
			this.la_27.Location = new System.Drawing.Point(368, 446);
			this.la_27.Margin = new System.Windows.Forms.Padding(0);
			this.la_27.Name = "la_27";
			this.la_27.Size = new System.Drawing.Size(139, 16);
			this.la_27.TabIndex = 74;
			this.la_27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_27
			// 
			this.pa_27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_27.Location = new System.Drawing.Point(510, 441);
			this.pa_27.Margin = new System.Windows.Forms.Padding(0);
			this.pa_27.Name = "pa_27";
			this.pa_27.Size = new System.Drawing.Size(69, 24);
			this.pa_27.TabIndex = 75;
			this.pa_27.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_27.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_28
			// 
			this.la_28.Location = new System.Drawing.Point(368, 471);
			this.la_28.Margin = new System.Windows.Forms.Padding(0);
			this.la_28.Name = "la_28";
			this.la_28.Size = new System.Drawing.Size(139, 16);
			this.la_28.TabIndex = 76;
			this.la_28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_28
			// 
			this.pa_28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_28.Location = new System.Drawing.Point(510, 466);
			this.pa_28.Margin = new System.Windows.Forms.Padding(0);
			this.pa_28.Name = "pa_28";
			this.pa_28.Size = new System.Drawing.Size(69, 24);
			this.pa_28.TabIndex = 77;
			this.pa_28.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_28.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_29
			// 
			this.la_29.Location = new System.Drawing.Point(4, 352);
			this.la_29.Margin = new System.Windows.Forms.Padding(0);
			this.la_29.Name = "la_29";
			this.la_29.Size = new System.Drawing.Size(183, 16);
			this.la_29.TabIndex = 26;
			this.la_29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_29
			// 
			this.pa_29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_29.Location = new System.Drawing.Point(191, 347);
			this.pa_29.Margin = new System.Windows.Forms.Padding(0);
			this.pa_29.Name = "pa_29";
			this.pa_29.Size = new System.Drawing.Size(69, 24);
			this.pa_29.TabIndex = 27;
			this.pa_29.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_29.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_30
			// 
			this.la_30.Location = new System.Drawing.Point(4, 512);
			this.la_30.Margin = new System.Windows.Forms.Padding(0);
			this.la_30.Name = "la_30";
			this.la_30.Size = new System.Drawing.Size(183, 16);
			this.la_30.TabIndex = 30;
			this.la_30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_30
			// 
			this.pa_30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_30.Location = new System.Drawing.Point(262, 507);
			this.pa_30.Margin = new System.Windows.Forms.Padding(0);
			this.pa_30.Name = "pa_30";
			this.pa_30.Size = new System.Drawing.Size(69, 24);
			this.pa_30.TabIndex = 31;
			this.pa_30.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_30.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_31
			// 
			this.la_31.Location = new System.Drawing.Point(4, 537);
			this.la_31.Margin = new System.Windows.Forms.Padding(0);
			this.la_31.Name = "la_31";
			this.la_31.Size = new System.Drawing.Size(183, 16);
			this.la_31.TabIndex = 32;
			this.la_31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_31
			// 
			this.pa_31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_31.Location = new System.Drawing.Point(262, 532);
			this.pa_31.Margin = new System.Windows.Forms.Padding(0);
			this.pa_31.Name = "pa_31";
			this.pa_31.Size = new System.Drawing.Size(69, 24);
			this.pa_31.TabIndex = 33;
			this.pa_31.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_31.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_32
			// 
			this.la_32.Location = new System.Drawing.Point(4, 612);
			this.la_32.Margin = new System.Windows.Forms.Padding(0);
			this.la_32.Name = "la_32";
			this.la_32.Size = new System.Drawing.Size(183, 16);
			this.la_32.TabIndex = 38;
			this.la_32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_32
			// 
			this.pa_32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_32.Location = new System.Drawing.Point(262, 607);
			this.pa_32.Margin = new System.Windows.Forms.Padding(0);
			this.pa_32.Name = "pa_32";
			this.pa_32.Size = new System.Drawing.Size(69, 24);
			this.pa_32.TabIndex = 39;
			this.pa_32.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_32.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_33
			// 
			this.la_33.Location = new System.Drawing.Point(4, 637);
			this.la_33.Margin = new System.Windows.Forms.Padding(0);
			this.la_33.Name = "la_33";
			this.la_33.Size = new System.Drawing.Size(183, 16);
			this.la_33.TabIndex = 40;
			this.la_33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_33
			// 
			this.pa_33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_33.Location = new System.Drawing.Point(262, 632);
			this.pa_33.Margin = new System.Windows.Forms.Padding(0);
			this.pa_33.Name = "pa_33";
			this.pa_33.Size = new System.Drawing.Size(69, 24);
			this.pa_33.TabIndex = 41;
			this.pa_33.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_33.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_34
			// 
			this.la_34.Location = new System.Drawing.Point(368, 251);
			this.la_34.Margin = new System.Windows.Forms.Padding(0);
			this.la_34.Name = "la_34";
			this.la_34.Size = new System.Drawing.Size(139, 16);
			this.la_34.TabIndex = 60;
			this.la_34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_34
			// 
			this.pa_34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_34.Location = new System.Drawing.Point(510, 246);
			this.pa_34.Margin = new System.Windows.Forms.Padding(0);
			this.pa_34.Name = "pa_34";
			this.pa_34.Size = new System.Drawing.Size(69, 24);
			this.pa_34.TabIndex = 61;
			this.pa_34.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_34.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_35
			// 
			this.la_35.Location = new System.Drawing.Point(368, 91);
			this.la_35.Margin = new System.Windows.Forms.Padding(0);
			this.la_35.Name = "la_35";
			this.la_35.Size = new System.Drawing.Size(139, 16);
			this.la_35.TabIndex = 48;
			this.la_35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_35
			// 
			this.pa_35.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_35.Location = new System.Drawing.Point(510, 86);
			this.pa_35.Margin = new System.Windows.Forms.Padding(0);
			this.pa_35.Name = "pa_35";
			this.pa_35.Size = new System.Drawing.Size(69, 24);
			this.pa_35.TabIndex = 49;
			this.pa_35.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_35.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_36
			// 
			this.la_36.Location = new System.Drawing.Point(368, 116);
			this.la_36.Margin = new System.Windows.Forms.Padding(0);
			this.la_36.Name = "la_36";
			this.la_36.Size = new System.Drawing.Size(139, 16);
			this.la_36.TabIndex = 50;
			this.la_36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_36
			// 
			this.pa_36.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_36.Location = new System.Drawing.Point(510, 111);
			this.pa_36.Margin = new System.Windows.Forms.Padding(0);
			this.pa_36.Name = "pa_36";
			this.pa_36.Size = new System.Drawing.Size(69, 24);
			this.pa_36.TabIndex = 51;
			this.pa_36.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_36.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_37
			// 
			this.la_37.Location = new System.Drawing.Point(4, 167);
			this.la_37.Margin = new System.Windows.Forms.Padding(0);
			this.la_37.Name = "la_37";
			this.la_37.Size = new System.Drawing.Size(183, 16);
			this.la_37.TabIndex = 12;
			this.la_37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_37
			// 
			this.pa_37.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_37.Location = new System.Drawing.Point(191, 162);
			this.pa_37.Margin = new System.Windows.Forms.Padding(0);
			this.pa_37.Name = "pa_37";
			this.pa_37.Size = new System.Drawing.Size(69, 24);
			this.pa_37.TabIndex = 13;
			this.pa_37.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_37.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// pa_37_t
			// 
			this.pa_37_t.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_37_t.Location = new System.Drawing.Point(262, 162);
			this.pa_37_t.Margin = new System.Windows.Forms.Padding(0);
			this.pa_37_t.Name = "pa_37_t";
			this.pa_37_t.Size = new System.Drawing.Size(69, 24);
			this.pa_37_t.TabIndex = 95;
			this.pa_37_t.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_37_t.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_38
			// 
			this.la_38.Location = new System.Drawing.Point(368, 276);
			this.la_38.Margin = new System.Windows.Forms.Padding(0);
			this.la_38.Name = "la_38";
			this.la_38.Size = new System.Drawing.Size(139, 16);
			this.la_38.TabIndex = 62;
			this.la_38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_38
			// 
			this.pa_38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_38.Location = new System.Drawing.Point(510, 271);
			this.pa_38.Margin = new System.Windows.Forms.Padding(0);
			this.pa_38.Name = "pa_38";
			this.pa_38.Size = new System.Drawing.Size(69, 24);
			this.pa_38.TabIndex = 63;
			this.pa_38.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_38.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_40
			// 
			this.la_40.Location = new System.Drawing.Point(4, 662);
			this.la_40.Margin = new System.Windows.Forms.Padding(0);
			this.la_40.Name = "la_40";
			this.la_40.Size = new System.Drawing.Size(183, 16);
			this.la_40.TabIndex = 79;
			this.la_40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_40
			// 
			this.pa_40.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_40.Location = new System.Drawing.Point(191, 657);
			this.pa_40.Margin = new System.Windows.Forms.Padding(0);
			this.pa_40.Name = "pa_40";
			this.pa_40.Size = new System.Drawing.Size(69, 24);
			this.pa_40.TabIndex = 80;
			this.pa_40.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_40.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_41
			// 
			this.la_41.Location = new System.Drawing.Point(4, 687);
			this.la_41.Margin = new System.Windows.Forms.Padding(0);
			this.la_41.Name = "la_41";
			this.la_41.Size = new System.Drawing.Size(183, 16);
			this.la_41.TabIndex = 81;
			this.la_41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_41
			// 
			this.pa_41.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_41.Location = new System.Drawing.Point(191, 682);
			this.pa_41.Margin = new System.Windows.Forms.Padding(0);
			this.pa_41.Name = "pa_41";
			this.pa_41.Size = new System.Drawing.Size(69, 24);
			this.pa_41.TabIndex = 82;
			this.pa_41.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_41.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_42
			// 
			this.la_42.Location = new System.Drawing.Point(4, 377);
			this.la_42.Margin = new System.Windows.Forms.Padding(0);
			this.la_42.Name = "la_42";
			this.la_42.Size = new System.Drawing.Size(183, 16);
			this.la_42.TabIndex = 83;
			this.la_42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_42
			// 
			this.pa_42.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_42.Location = new System.Drawing.Point(191, 372);
			this.pa_42.Margin = new System.Windows.Forms.Padding(0);
			this.pa_42.Name = "pa_42";
			this.pa_42.Size = new System.Drawing.Size(69, 24);
			this.pa_42.TabIndex = 84;
			this.pa_42.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_42.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_43
			// 
			this.la_43.Location = new System.Drawing.Point(4, 402);
			this.la_43.Margin = new System.Windows.Forms.Padding(0);
			this.la_43.Name = "la_43";
			this.la_43.Size = new System.Drawing.Size(183, 16);
			this.la_43.TabIndex = 85;
			this.la_43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_43
			// 
			this.pa_43.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_43.Location = new System.Drawing.Point(191, 397);
			this.pa_43.Margin = new System.Windows.Forms.Padding(0);
			this.pa_43.Name = "pa_43";
			this.pa_43.Size = new System.Drawing.Size(69, 24);
			this.pa_43.TabIndex = 86;
			this.pa_43.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_43.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_44
			// 
			this.la_44.Location = new System.Drawing.Point(4, 427);
			this.la_44.Margin = new System.Windows.Forms.Padding(0);
			this.la_44.Name = "la_44";
			this.la_44.Size = new System.Drawing.Size(183, 16);
			this.la_44.TabIndex = 87;
			this.la_44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_44
			// 
			this.pa_44.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_44.Location = new System.Drawing.Point(191, 422);
			this.pa_44.Margin = new System.Windows.Forms.Padding(0);
			this.pa_44.Name = "pa_44";
			this.pa_44.Size = new System.Drawing.Size(69, 24);
			this.pa_44.TabIndex = 88;
			this.pa_44.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_44.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_45
			// 
			this.la_45.Location = new System.Drawing.Point(4, 452);
			this.la_45.Margin = new System.Windows.Forms.Padding(0);
			this.la_45.Name = "la_45";
			this.la_45.Size = new System.Drawing.Size(183, 16);
			this.la_45.TabIndex = 89;
			this.la_45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_45
			// 
			this.pa_45.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_45.Location = new System.Drawing.Point(191, 447);
			this.pa_45.Margin = new System.Windows.Forms.Padding(0);
			this.pa_45.Name = "pa_45";
			this.pa_45.Size = new System.Drawing.Size(69, 24);
			this.pa_45.TabIndex = 90;
			this.pa_45.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_45.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_Help
			// 
			this.la_Help.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.la_Help.Location = new System.Drawing.Point(370, 500);
			this.la_Help.Margin = new System.Windows.Forms.Padding(0);
			this.la_Help.Name = "la_Help";
			this.la_Help.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this.la_Help.Size = new System.Drawing.Size(210, 56);
			this.la_Help.TabIndex = 78;
			this.la_Help.Text = "LMB - opens the Color Selector\r\nRMB - restore default color";
			this.la_Help.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_bot
			// 
			this.pa_bot.Controls.Add(this.bu_Save);
			this.pa_bot.Controls.Add(this.bu_Cancel);
			this.pa_bot.Controls.Add(this.bu_Defaults);
			this.pa_bot.Controls.Add(this.bu_Delete);
			this.pa_bot.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pa_bot.Location = new System.Drawing.Point(0, 741);
			this.pa_bot.Margin = new System.Windows.Forms.Padding(0);
			this.pa_bot.Name = "pa_bot";
			this.pa_bot.Size = new System.Drawing.Size(667, 33);
			this.pa_bot.TabIndex = 1;
			// 
			// bu_Save
			// 
			this.bu_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Save.Location = new System.Drawing.Point(399, 4);
			this.bu_Save.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Save.Name = "bu_Save";
			this.bu_Save.Size = new System.Drawing.Size(164, 26);
			this.bu_Save.TabIndex = 2;
			this.bu_Save.Text = "SAVE  FILE / close";
			this.toolTip1.SetToolTip(this.bu_Save, "Saves Colors.Cfg file\r\n[Ctrl]+click to not close dialog");
			this.bu_Save.UseVisualStyleBackColor = true;
			this.bu_Save.Click += new System.EventHandler(this.click_Save);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(564, 4);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(99, 26);
			this.bu_Cancel.TabIndex = 3;
			this.bu_Cancel.Text = "Esc";
			this.toolTip1.SetToolTip(this.bu_Cancel, "go away");
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.click_Cancel);
			// 
			// bu_Defaults
			// 
			this.bu_Defaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Defaults.Location = new System.Drawing.Point(90, 8);
			this.bu_Defaults.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Defaults.Name = "bu_Defaults";
			this.bu_Defaults.Size = new System.Drawing.Size(112, 22);
			this.bu_Defaults.TabIndex = 1;
			this.bu_Defaults.Text = "restore defaults";
			this.toolTip1.SetToolTip(this.bu_Defaults, "restores all colors to hardcoded defaults");
			this.bu_Defaults.UseVisualStyleBackColor = true;
			this.bu_Defaults.Click += new System.EventHandler(this.click_RestoreDefaults);
			// 
			// bu_Delete
			// 
			this.bu_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Delete.Location = new System.Drawing.Point(4, 8);
			this.bu_Delete.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Delete.Name = "bu_Delete";
			this.bu_Delete.Size = new System.Drawing.Size(85, 22);
			this.bu_Delete.TabIndex = 0;
			this.bu_Delete.Text = "delete file";
			this.toolTip1.SetToolTip(this.bu_Delete, "deletes Colors.Cfg file");
			this.bu_Delete.UseVisualStyleBackColor = true;
			this.bu_Delete.Click += new System.EventHandler(this.click_Delete);
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 8000;
			this.toolTip1.InitialDelay = 100;
			this.toolTip1.ReshowDelay = 50;
			// 
			// ColorOptionsDialog
			// 
			this.AcceptButton = this.bu_Save;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(667, 774);
			this.Controls.Add(this.gb_Colors);
			this.Controls.Add(this.pa_bot);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.Name = "ColorOptionsDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Colors.Cfg";
			this.gb_Colors.ResumeLayout(false);
			this.pa_bot.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
