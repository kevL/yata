using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ColorOptionsF
	{
		#region Designer

		GroupBox gb_Colors;
		Label la_01;
		Panel pa_01;
		Label la_02;
		Panel pa_02;
		Label la_03;
		Panel pa_03;
		Label la_04;
		Panel pa_04;
		Label la_05;
		Panel pa_05;
		Label la_06;
		Panel pa_06;
		Label la_07;
		Panel pa_07;
		Label la_08;
		Panel pa_08;
		Label la_09;
		Panel pa_09;
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
		Label la_15;
		Panel pa_15;
		Label la_16;
		Panel pa_16;
		Label la_17;
		Panel pa_17;
		Label la_18;
		Panel pa_18;
		Label la_19;
		Panel pa_19;
		Label la_20;
		Panel pa_20;
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
		Label la_38;
		Panel pa_38;

		Panel pa_bot;
		Button bu_Delete;
		Button bu_Defaults;
		Button bu_Save;
		Button bu_Cancel;


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.gb_Colors = new System.Windows.Forms.GroupBox();
			this.la_01 = new System.Windows.Forms.Label();
			this.pa_01 = new System.Windows.Forms.Panel();
			this.la_02 = new System.Windows.Forms.Label();
			this.pa_02 = new System.Windows.Forms.Panel();
			this.la_03 = new System.Windows.Forms.Label();
			this.pa_03 = new System.Windows.Forms.Panel();
			this.la_04 = new System.Windows.Forms.Label();
			this.pa_04 = new System.Windows.Forms.Panel();
			this.la_05 = new System.Windows.Forms.Label();
			this.pa_05 = new System.Windows.Forms.Panel();
			this.la_06 = new System.Windows.Forms.Label();
			this.pa_06 = new System.Windows.Forms.Panel();
			this.la_07 = new System.Windows.Forms.Label();
			this.pa_07 = new System.Windows.Forms.Panel();
			this.la_08 = new System.Windows.Forms.Label();
			this.pa_08 = new System.Windows.Forms.Panel();
			this.la_09 = new System.Windows.Forms.Label();
			this.pa_09 = new System.Windows.Forms.Panel();
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
			this.la_15 = new System.Windows.Forms.Label();
			this.pa_15 = new System.Windows.Forms.Panel();
			this.la_16 = new System.Windows.Forms.Label();
			this.pa_16 = new System.Windows.Forms.Panel();
			this.la_17 = new System.Windows.Forms.Label();
			this.pa_17 = new System.Windows.Forms.Panel();
			this.la_18 = new System.Windows.Forms.Label();
			this.pa_18 = new System.Windows.Forms.Panel();
			this.la_19 = new System.Windows.Forms.Label();
			this.pa_19 = new System.Windows.Forms.Panel();
			this.la_20 = new System.Windows.Forms.Label();
			this.pa_20 = new System.Windows.Forms.Panel();
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
			this.la_38 = new System.Windows.Forms.Label();
			this.pa_38 = new System.Windows.Forms.Panel();
			this.pa_bot = new System.Windows.Forms.Panel();
			this.bu_Save = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Defaults = new System.Windows.Forms.Button();
			this.bu_Delete = new System.Windows.Forms.Button();
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
			this.gb_Colors.Controls.Add(this.la_03);
			this.gb_Colors.Controls.Add(this.pa_03);
			this.gb_Colors.Controls.Add(this.la_04);
			this.gb_Colors.Controls.Add(this.pa_04);
			this.gb_Colors.Controls.Add(this.la_05);
			this.gb_Colors.Controls.Add(this.pa_05);
			this.gb_Colors.Controls.Add(this.la_06);
			this.gb_Colors.Controls.Add(this.pa_06);
			this.gb_Colors.Controls.Add(this.la_07);
			this.gb_Colors.Controls.Add(this.pa_07);
			this.gb_Colors.Controls.Add(this.la_08);
			this.gb_Colors.Controls.Add(this.pa_08);
			this.gb_Colors.Controls.Add(this.la_09);
			this.gb_Colors.Controls.Add(this.pa_09);
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
			this.gb_Colors.Controls.Add(this.la_15);
			this.gb_Colors.Controls.Add(this.pa_15);
			this.gb_Colors.Controls.Add(this.la_16);
			this.gb_Colors.Controls.Add(this.pa_16);
			this.gb_Colors.Controls.Add(this.la_17);
			this.gb_Colors.Controls.Add(this.pa_17);
			this.gb_Colors.Controls.Add(this.la_18);
			this.gb_Colors.Controls.Add(this.pa_18);
			this.gb_Colors.Controls.Add(this.la_19);
			this.gb_Colors.Controls.Add(this.pa_19);
			this.gb_Colors.Controls.Add(this.la_20);
			this.gb_Colors.Controls.Add(this.pa_20);
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
			this.gb_Colors.Controls.Add(this.la_38);
			this.gb_Colors.Controls.Add(this.pa_38);
			this.gb_Colors.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gb_Colors.Location = new System.Drawing.Point(0, 0);
			this.gb_Colors.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Colors.Name = "gb_Colors";
			this.gb_Colors.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Colors.Size = new System.Drawing.Size(792, 400);
			this.gb_Colors.TabIndex = 0;
			this.gb_Colors.TabStop = false;
			// 
			// la_01
			// 
			this.la_01.Location = new System.Drawing.Point(11, 20);
			this.la_01.Margin = new System.Windows.Forms.Padding(0);
			this.la_01.Name = "la_01";
			this.la_01.Size = new System.Drawing.Size(126, 16);
			this.la_01.TabIndex = 0;
			this.la_01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_01
			// 
			this.pa_01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_01.Location = new System.Drawing.Point(142, 15);
			this.pa_01.Margin = new System.Windows.Forms.Padding(0);
			this.pa_01.Name = "pa_01";
			this.pa_01.Size = new System.Drawing.Size(69, 24);
			this.pa_01.TabIndex = 1;
			this.pa_01.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_01.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_02
			// 
			this.la_02.Location = new System.Drawing.Point(11, 45);
			this.la_02.Margin = new System.Windows.Forms.Padding(0);
			this.la_02.Name = "la_02";
			this.la_02.Size = new System.Drawing.Size(126, 16);
			this.la_02.TabIndex = 2;
			this.la_02.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_02
			// 
			this.pa_02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_02.Location = new System.Drawing.Point(142, 40);
			this.pa_02.Margin = new System.Windows.Forms.Padding(0);
			this.pa_02.Name = "pa_02";
			this.pa_02.Size = new System.Drawing.Size(69, 24);
			this.pa_02.TabIndex = 3;
			this.pa_02.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_02.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_03
			// 
			this.la_03.Location = new System.Drawing.Point(11, 70);
			this.la_03.Margin = new System.Windows.Forms.Padding(0);
			this.la_03.Name = "la_03";
			this.la_03.Size = new System.Drawing.Size(126, 16);
			this.la_03.TabIndex = 4;
			this.la_03.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_03
			// 
			this.pa_03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_03.Location = new System.Drawing.Point(142, 65);
			this.pa_03.Margin = new System.Windows.Forms.Padding(0);
			this.pa_03.Name = "pa_03";
			this.pa_03.Size = new System.Drawing.Size(69, 24);
			this.pa_03.TabIndex = 5;
			this.pa_03.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_03.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_04
			// 
			this.la_04.Location = new System.Drawing.Point(11, 95);
			this.la_04.Margin = new System.Windows.Forms.Padding(0);
			this.la_04.Name = "la_04";
			this.la_04.Size = new System.Drawing.Size(126, 16);
			this.la_04.TabIndex = 6;
			this.la_04.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_04
			// 
			this.pa_04.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_04.Location = new System.Drawing.Point(142, 90);
			this.pa_04.Margin = new System.Windows.Forms.Padding(0);
			this.pa_04.Name = "pa_04";
			this.pa_04.Size = new System.Drawing.Size(69, 24);
			this.pa_04.TabIndex = 7;
			this.pa_04.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_04.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_05
			// 
			this.la_05.Location = new System.Drawing.Point(11, 120);
			this.la_05.Margin = new System.Windows.Forms.Padding(0);
			this.la_05.Name = "la_05";
			this.la_05.Size = new System.Drawing.Size(126, 16);
			this.la_05.TabIndex = 8;
			this.la_05.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_05
			// 
			this.pa_05.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_05.Location = new System.Drawing.Point(142, 115);
			this.pa_05.Margin = new System.Windows.Forms.Padding(0);
			this.pa_05.Name = "pa_05";
			this.pa_05.Size = new System.Drawing.Size(69, 24);
			this.pa_05.TabIndex = 9;
			this.pa_05.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_05.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_06
			// 
			this.la_06.Location = new System.Drawing.Point(11, 145);
			this.la_06.Margin = new System.Windows.Forms.Padding(0);
			this.la_06.Name = "la_06";
			this.la_06.Size = new System.Drawing.Size(126, 16);
			this.la_06.TabIndex = 10;
			this.la_06.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_06
			// 
			this.pa_06.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_06.Location = new System.Drawing.Point(142, 140);
			this.pa_06.Margin = new System.Windows.Forms.Padding(0);
			this.pa_06.Name = "pa_06";
			this.pa_06.Size = new System.Drawing.Size(69, 24);
			this.pa_06.TabIndex = 11;
			this.pa_06.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_06.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_07
			// 
			this.la_07.Location = new System.Drawing.Point(11, 170);
			this.la_07.Margin = new System.Windows.Forms.Padding(0);
			this.la_07.Name = "la_07";
			this.la_07.Size = new System.Drawing.Size(126, 16);
			this.la_07.TabIndex = 12;
			this.la_07.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_07
			// 
			this.pa_07.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_07.Location = new System.Drawing.Point(142, 165);
			this.pa_07.Margin = new System.Windows.Forms.Padding(0);
			this.pa_07.Name = "pa_07";
			this.pa_07.Size = new System.Drawing.Size(69, 24);
			this.pa_07.TabIndex = 13;
			this.pa_07.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_07.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_08
			// 
			this.la_08.Location = new System.Drawing.Point(11, 195);
			this.la_08.Margin = new System.Windows.Forms.Padding(0);
			this.la_08.Name = "la_08";
			this.la_08.Size = new System.Drawing.Size(126, 16);
			this.la_08.TabIndex = 14;
			this.la_08.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_08
			// 
			this.pa_08.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_08.Location = new System.Drawing.Point(142, 190);
			this.pa_08.Margin = new System.Windows.Forms.Padding(0);
			this.pa_08.Name = "pa_08";
			this.pa_08.Size = new System.Drawing.Size(69, 24);
			this.pa_08.TabIndex = 15;
			this.pa_08.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_08.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_09
			// 
			this.la_09.Location = new System.Drawing.Point(11, 220);
			this.la_09.Margin = new System.Windows.Forms.Padding(0);
			this.la_09.Name = "la_09";
			this.la_09.Size = new System.Drawing.Size(126, 16);
			this.la_09.TabIndex = 16;
			this.la_09.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_09
			// 
			this.pa_09.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_09.Location = new System.Drawing.Point(142, 215);
			this.pa_09.Margin = new System.Windows.Forms.Padding(0);
			this.pa_09.Name = "pa_09";
			this.pa_09.Size = new System.Drawing.Size(69, 24);
			this.pa_09.TabIndex = 17;
			this.pa_09.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_09.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_10
			// 
			this.la_10.Location = new System.Drawing.Point(11, 245);
			this.la_10.Margin = new System.Windows.Forms.Padding(0);
			this.la_10.Name = "la_10";
			this.la_10.Size = new System.Drawing.Size(126, 16);
			this.la_10.TabIndex = 18;
			this.la_10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_10
			// 
			this.pa_10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_10.Location = new System.Drawing.Point(142, 240);
			this.pa_10.Margin = new System.Windows.Forms.Padding(0);
			this.pa_10.Name = "pa_10";
			this.pa_10.Size = new System.Drawing.Size(69, 24);
			this.pa_10.TabIndex = 19;
			this.pa_10.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_10.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_11
			// 
			this.la_11.Location = new System.Drawing.Point(11, 270);
			this.la_11.Margin = new System.Windows.Forms.Padding(0);
			this.la_11.Name = "la_11";
			this.la_11.Size = new System.Drawing.Size(126, 16);
			this.la_11.TabIndex = 20;
			this.la_11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_11
			// 
			this.pa_11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_11.Location = new System.Drawing.Point(142, 265);
			this.pa_11.Margin = new System.Windows.Forms.Padding(0);
			this.pa_11.Name = "pa_11";
			this.pa_11.Size = new System.Drawing.Size(69, 24);
			this.pa_11.TabIndex = 21;
			this.pa_11.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_11.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_12
			// 
			this.la_12.Location = new System.Drawing.Point(11, 295);
			this.la_12.Margin = new System.Windows.Forms.Padding(0);
			this.la_12.Name = "la_12";
			this.la_12.Size = new System.Drawing.Size(126, 16);
			this.la_12.TabIndex = 22;
			this.la_12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_12
			// 
			this.pa_12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_12.Location = new System.Drawing.Point(142, 290);
			this.pa_12.Margin = new System.Windows.Forms.Padding(0);
			this.pa_12.Name = "pa_12";
			this.pa_12.Size = new System.Drawing.Size(69, 24);
			this.pa_12.TabIndex = 23;
			this.pa_12.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_12.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_13
			// 
			this.la_13.Location = new System.Drawing.Point(11, 320);
			this.la_13.Margin = new System.Windows.Forms.Padding(0);
			this.la_13.Name = "la_13";
			this.la_13.Size = new System.Drawing.Size(126, 16);
			this.la_13.TabIndex = 24;
			this.la_13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_13
			// 
			this.pa_13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_13.Location = new System.Drawing.Point(142, 315);
			this.pa_13.Margin = new System.Windows.Forms.Padding(0);
			this.pa_13.Name = "pa_13";
			this.pa_13.Size = new System.Drawing.Size(69, 24);
			this.pa_13.TabIndex = 25;
			this.pa_13.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_13.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_14
			// 
			this.la_14.Location = new System.Drawing.Point(11, 345);
			this.la_14.Margin = new System.Windows.Forms.Padding(0);
			this.la_14.Name = "la_14";
			this.la_14.Size = new System.Drawing.Size(126, 16);
			this.la_14.TabIndex = 26;
			this.la_14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_14
			// 
			this.pa_14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_14.Location = new System.Drawing.Point(142, 340);
			this.pa_14.Margin = new System.Windows.Forms.Padding(0);
			this.pa_14.Name = "pa_14";
			this.pa_14.Size = new System.Drawing.Size(69, 24);
			this.pa_14.TabIndex = 27;
			this.pa_14.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_14.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_15
			// 
			this.la_15.Location = new System.Drawing.Point(230, 20);
			this.la_15.Margin = new System.Windows.Forms.Padding(0);
			this.la_15.Name = "la_15";
			this.la_15.Size = new System.Drawing.Size(126, 16);
			this.la_15.TabIndex = 28;
			this.la_15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_15
			// 
			this.pa_15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_15.Location = new System.Drawing.Point(361, 15);
			this.pa_15.Margin = new System.Windows.Forms.Padding(0);
			this.pa_15.Name = "pa_15";
			this.pa_15.Size = new System.Drawing.Size(69, 24);
			this.pa_15.TabIndex = 29;
			this.pa_15.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_15.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_16
			// 
			this.la_16.Location = new System.Drawing.Point(230, 45);
			this.la_16.Margin = new System.Windows.Forms.Padding(0);
			this.la_16.Name = "la_16";
			this.la_16.Size = new System.Drawing.Size(126, 16);
			this.la_16.TabIndex = 30;
			this.la_16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_16
			// 
			this.pa_16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_16.Location = new System.Drawing.Point(361, 40);
			this.pa_16.Margin = new System.Windows.Forms.Padding(0);
			this.pa_16.Name = "pa_16";
			this.pa_16.Size = new System.Drawing.Size(69, 24);
			this.pa_16.TabIndex = 31;
			this.pa_16.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_16.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_17
			// 
			this.la_17.Location = new System.Drawing.Point(230, 70);
			this.la_17.Margin = new System.Windows.Forms.Padding(0);
			this.la_17.Name = "la_17";
			this.la_17.Size = new System.Drawing.Size(126, 16);
			this.la_17.TabIndex = 32;
			this.la_17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_17
			// 
			this.pa_17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_17.Location = new System.Drawing.Point(361, 65);
			this.pa_17.Margin = new System.Windows.Forms.Padding(0);
			this.pa_17.Name = "pa_17";
			this.pa_17.Size = new System.Drawing.Size(69, 24);
			this.pa_17.TabIndex = 33;
			this.pa_17.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_17.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_18
			// 
			this.la_18.Location = new System.Drawing.Point(230, 95);
			this.la_18.Margin = new System.Windows.Forms.Padding(0);
			this.la_18.Name = "la_18";
			this.la_18.Size = new System.Drawing.Size(126, 16);
			this.la_18.TabIndex = 34;
			this.la_18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_18
			// 
			this.pa_18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_18.Location = new System.Drawing.Point(361, 90);
			this.pa_18.Margin = new System.Windows.Forms.Padding(0);
			this.pa_18.Name = "pa_18";
			this.pa_18.Size = new System.Drawing.Size(69, 24);
			this.pa_18.TabIndex = 35;
			this.pa_18.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_18.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_19
			// 
			this.la_19.Location = new System.Drawing.Point(230, 120);
			this.la_19.Margin = new System.Windows.Forms.Padding(0);
			this.la_19.Name = "la_19";
			this.la_19.Size = new System.Drawing.Size(126, 16);
			this.la_19.TabIndex = 36;
			this.la_19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_19
			// 
			this.pa_19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_19.Location = new System.Drawing.Point(361, 115);
			this.pa_19.Margin = new System.Windows.Forms.Padding(0);
			this.pa_19.Name = "pa_19";
			this.pa_19.Size = new System.Drawing.Size(69, 24);
			this.pa_19.TabIndex = 37;
			this.pa_19.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_19.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_20
			// 
			this.la_20.Location = new System.Drawing.Point(230, 145);
			this.la_20.Margin = new System.Windows.Forms.Padding(0);
			this.la_20.Name = "la_20";
			this.la_20.Size = new System.Drawing.Size(126, 16);
			this.la_20.TabIndex = 38;
			this.la_20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_20
			// 
			this.pa_20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_20.Location = new System.Drawing.Point(361, 140);
			this.pa_20.Margin = new System.Windows.Forms.Padding(0);
			this.pa_20.Name = "pa_20";
			this.pa_20.Size = new System.Drawing.Size(69, 24);
			this.pa_20.TabIndex = 39;
			this.pa_20.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_20.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_21
			// 
			this.la_21.Location = new System.Drawing.Point(230, 170);
			this.la_21.Margin = new System.Windows.Forms.Padding(0);
			this.la_21.Name = "la_21";
			this.la_21.Size = new System.Drawing.Size(126, 16);
			this.la_21.TabIndex = 40;
			this.la_21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_21
			// 
			this.pa_21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_21.Location = new System.Drawing.Point(361, 165);
			this.pa_21.Margin = new System.Windows.Forms.Padding(0);
			this.pa_21.Name = "pa_21";
			this.pa_21.Size = new System.Drawing.Size(69, 24);
			this.pa_21.TabIndex = 41;
			this.pa_21.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_21.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_22
			// 
			this.la_22.Location = new System.Drawing.Point(230, 195);
			this.la_22.Margin = new System.Windows.Forms.Padding(0);
			this.la_22.Name = "la_22";
			this.la_22.Size = new System.Drawing.Size(126, 16);
			this.la_22.TabIndex = 42;
			this.la_22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_22
			// 
			this.pa_22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_22.Location = new System.Drawing.Point(361, 190);
			this.pa_22.Margin = new System.Windows.Forms.Padding(0);
			this.pa_22.Name = "pa_22";
			this.pa_22.Size = new System.Drawing.Size(69, 24);
			this.pa_22.TabIndex = 43;
			this.pa_22.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_22.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_23
			// 
			this.la_23.Location = new System.Drawing.Point(230, 220);
			this.la_23.Margin = new System.Windows.Forms.Padding(0);
			this.la_23.Name = "la_23";
			this.la_23.Size = new System.Drawing.Size(126, 16);
			this.la_23.TabIndex = 44;
			this.la_23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_23
			// 
			this.pa_23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_23.Location = new System.Drawing.Point(361, 215);
			this.pa_23.Margin = new System.Windows.Forms.Padding(0);
			this.pa_23.Name = "pa_23";
			this.pa_23.Size = new System.Drawing.Size(69, 24);
			this.pa_23.TabIndex = 45;
			this.pa_23.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_23.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_24
			// 
			this.la_24.Location = new System.Drawing.Point(230, 245);
			this.la_24.Margin = new System.Windows.Forms.Padding(0);
			this.la_24.Name = "la_24";
			this.la_24.Size = new System.Drawing.Size(126, 16);
			this.la_24.TabIndex = 46;
			this.la_24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_24
			// 
			this.pa_24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_24.Location = new System.Drawing.Point(361, 240);
			this.pa_24.Margin = new System.Windows.Forms.Padding(0);
			this.pa_24.Name = "pa_24";
			this.pa_24.Size = new System.Drawing.Size(69, 24);
			this.pa_24.TabIndex = 47;
			this.pa_24.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_24.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_25
			// 
			this.la_25.Location = new System.Drawing.Point(230, 270);
			this.la_25.Margin = new System.Windows.Forms.Padding(0);
			this.la_25.Name = "la_25";
			this.la_25.Size = new System.Drawing.Size(126, 16);
			this.la_25.TabIndex = 48;
			this.la_25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_25
			// 
			this.pa_25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_25.Location = new System.Drawing.Point(361, 265);
			this.pa_25.Margin = new System.Windows.Forms.Padding(0);
			this.pa_25.Name = "pa_25";
			this.pa_25.Size = new System.Drawing.Size(69, 24);
			this.pa_25.TabIndex = 49;
			this.pa_25.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_25.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_26
			// 
			this.la_26.Location = new System.Drawing.Point(230, 295);
			this.la_26.Margin = new System.Windows.Forms.Padding(0);
			this.la_26.Name = "la_26";
			this.la_26.Size = new System.Drawing.Size(126, 16);
			this.la_26.TabIndex = 50;
			this.la_26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_26
			// 
			this.pa_26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_26.Location = new System.Drawing.Point(361, 290);
			this.pa_26.Margin = new System.Windows.Forms.Padding(0);
			this.pa_26.Name = "pa_26";
			this.pa_26.Size = new System.Drawing.Size(69, 24);
			this.pa_26.TabIndex = 51;
			this.pa_26.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_26.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_27
			// 
			this.la_27.Location = new System.Drawing.Point(230, 320);
			this.la_27.Margin = new System.Windows.Forms.Padding(0);
			this.la_27.Name = "la_27";
			this.la_27.Size = new System.Drawing.Size(126, 16);
			this.la_27.TabIndex = 52;
			this.la_27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_27
			// 
			this.pa_27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_27.Location = new System.Drawing.Point(361, 315);
			this.pa_27.Margin = new System.Windows.Forms.Padding(0);
			this.pa_27.Name = "pa_27";
			this.pa_27.Size = new System.Drawing.Size(69, 24);
			this.pa_27.TabIndex = 53;
			this.pa_27.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_27.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_28
			// 
			this.la_28.Location = new System.Drawing.Point(230, 345);
			this.la_28.Margin = new System.Windows.Forms.Padding(0);
			this.la_28.Name = "la_28";
			this.la_28.Size = new System.Drawing.Size(126, 16);
			this.la_28.TabIndex = 54;
			this.la_28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_28
			// 
			this.pa_28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_28.Location = new System.Drawing.Point(361, 340);
			this.pa_28.Margin = new System.Windows.Forms.Padding(0);
			this.pa_28.Name = "pa_28";
			this.pa_28.Size = new System.Drawing.Size(69, 24);
			this.pa_28.TabIndex = 55;
			this.pa_28.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_28.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_29
			// 
			this.la_29.Location = new System.Drawing.Point(449, 20);
			this.la_29.Margin = new System.Windows.Forms.Padding(0);
			this.la_29.Name = "la_29";
			this.la_29.Size = new System.Drawing.Size(166, 16);
			this.la_29.TabIndex = 56;
			this.la_29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_29
			// 
			this.pa_29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_29.Location = new System.Drawing.Point(620, 15);
			this.pa_29.Margin = new System.Windows.Forms.Padding(0);
			this.pa_29.Name = "pa_29";
			this.pa_29.Size = new System.Drawing.Size(69, 24);
			this.pa_29.TabIndex = 57;
			this.pa_29.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_29.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_30
			// 
			this.la_30.Location = new System.Drawing.Point(449, 45);
			this.la_30.Margin = new System.Windows.Forms.Padding(0);
			this.la_30.Name = "la_30";
			this.la_30.Size = new System.Drawing.Size(166, 16);
			this.la_30.TabIndex = 58;
			this.la_30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_30
			// 
			this.pa_30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_30.Location = new System.Drawing.Point(620, 40);
			this.pa_30.Margin = new System.Windows.Forms.Padding(0);
			this.pa_30.Name = "pa_30";
			this.pa_30.Size = new System.Drawing.Size(69, 24);
			this.pa_30.TabIndex = 59;
			this.pa_30.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_30.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_31
			// 
			this.la_31.Location = new System.Drawing.Point(449, 70);
			this.la_31.Margin = new System.Windows.Forms.Padding(0);
			this.la_31.Name = "la_31";
			this.la_31.Size = new System.Drawing.Size(166, 16);
			this.la_31.TabIndex = 60;
			this.la_31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_31
			// 
			this.pa_31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_31.Location = new System.Drawing.Point(620, 65);
			this.pa_31.Margin = new System.Windows.Forms.Padding(0);
			this.pa_31.Name = "pa_31";
			this.pa_31.Size = new System.Drawing.Size(69, 24);
			this.pa_31.TabIndex = 61;
			this.pa_31.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_31.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_32
			// 
			this.la_32.Location = new System.Drawing.Point(449, 95);
			this.la_32.Margin = new System.Windows.Forms.Padding(0);
			this.la_32.Name = "la_32";
			this.la_32.Size = new System.Drawing.Size(166, 16);
			this.la_32.TabIndex = 62;
			this.la_32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_32
			// 
			this.pa_32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_32.Location = new System.Drawing.Point(620, 90);
			this.pa_32.Margin = new System.Windows.Forms.Padding(0);
			this.pa_32.Name = "pa_32";
			this.pa_32.Size = new System.Drawing.Size(69, 24);
			this.pa_32.TabIndex = 63;
			this.pa_32.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_32.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_33
			// 
			this.la_33.Location = new System.Drawing.Point(449, 120);
			this.la_33.Margin = new System.Windows.Forms.Padding(0);
			this.la_33.Name = "la_33";
			this.la_33.Size = new System.Drawing.Size(166, 16);
			this.la_33.TabIndex = 64;
			this.la_33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_33
			// 
			this.pa_33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_33.Location = new System.Drawing.Point(620, 115);
			this.pa_33.Margin = new System.Windows.Forms.Padding(0);
			this.pa_33.Name = "pa_33";
			this.pa_33.Size = new System.Drawing.Size(69, 24);
			this.pa_33.TabIndex = 65;
			this.pa_33.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_33.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_34
			// 
			this.la_34.Location = new System.Drawing.Point(449, 145);
			this.la_34.Margin = new System.Windows.Forms.Padding(0);
			this.la_34.Name = "la_34";
			this.la_34.Size = new System.Drawing.Size(166, 16);
			this.la_34.TabIndex = 66;
			this.la_34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_34
			// 
			this.pa_34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_34.Location = new System.Drawing.Point(620, 140);
			this.pa_34.Margin = new System.Windows.Forms.Padding(0);
			this.pa_34.Name = "pa_34";
			this.pa_34.Size = new System.Drawing.Size(69, 24);
			this.pa_34.TabIndex = 67;
			this.pa_34.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_34.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_35
			// 
			this.la_35.Location = new System.Drawing.Point(449, 170);
			this.la_35.Margin = new System.Windows.Forms.Padding(0);
			this.la_35.Name = "la_35";
			this.la_35.Size = new System.Drawing.Size(166, 16);
			this.la_35.TabIndex = 68;
			this.la_35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_35
			// 
			this.pa_35.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_35.Location = new System.Drawing.Point(620, 165);
			this.pa_35.Margin = new System.Windows.Forms.Padding(0);
			this.pa_35.Name = "pa_35";
			this.pa_35.Size = new System.Drawing.Size(69, 24);
			this.pa_35.TabIndex = 69;
			this.pa_35.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_35.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_36
			// 
			this.la_36.Location = new System.Drawing.Point(449, 195);
			this.la_36.Margin = new System.Windows.Forms.Padding(0);
			this.la_36.Name = "la_36";
			this.la_36.Size = new System.Drawing.Size(166, 16);
			this.la_36.TabIndex = 70;
			this.la_36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_36
			// 
			this.pa_36.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_36.Location = new System.Drawing.Point(620, 190);
			this.pa_36.Margin = new System.Windows.Forms.Padding(0);
			this.pa_36.Name = "pa_36";
			this.pa_36.Size = new System.Drawing.Size(69, 24);
			this.pa_36.TabIndex = 71;
			this.pa_36.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_36.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_37
			// 
			this.la_37.Location = new System.Drawing.Point(449, 220);
			this.la_37.Margin = new System.Windows.Forms.Padding(0);
			this.la_37.Name = "la_37";
			this.la_37.Size = new System.Drawing.Size(166, 16);
			this.la_37.TabIndex = 72;
			this.la_37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_37
			// 
			this.pa_37.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_37.Location = new System.Drawing.Point(620, 215);
			this.pa_37.Margin = new System.Windows.Forms.Padding(0);
			this.pa_37.Name = "pa_37";
			this.pa_37.Size = new System.Drawing.Size(69, 24);
			this.pa_37.TabIndex = 73;
			this.pa_37.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_37.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// la_38
			// 
			this.la_38.Location = new System.Drawing.Point(449, 245);
			this.la_38.Margin = new System.Windows.Forms.Padding(0);
			this.la_38.Name = "la_38";
			this.la_38.Size = new System.Drawing.Size(166, 16);
			this.la_38.TabIndex = 74;
			this.la_38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_38
			// 
			this.pa_38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_38.Location = new System.Drawing.Point(620, 240);
			this.pa_38.Margin = new System.Windows.Forms.Padding(0);
			this.pa_38.Name = "pa_38";
			this.pa_38.Size = new System.Drawing.Size(69, 24);
			this.pa_38.TabIndex = 75;
			this.pa_38.BackColorChanged += new System.EventHandler(this.backcolorchanged_ColorPanel);
			this.pa_38.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Colorpanel);
			// 
			// pa_bot
			// 
			this.pa_bot.Controls.Add(this.bu_Save);
			this.pa_bot.Controls.Add(this.bu_Cancel);
			this.pa_bot.Controls.Add(this.bu_Defaults);
			this.pa_bot.Controls.Add(this.bu_Delete);
			this.pa_bot.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pa_bot.Location = new System.Drawing.Point(0, 400);
			this.pa_bot.Margin = new System.Windows.Forms.Padding(0);
			this.pa_bot.Name = "pa_bot";
			this.pa_bot.Size = new System.Drawing.Size(792, 34);
			this.pa_bot.TabIndex = 1;
			// 
			// bu_Save
			// 
			this.bu_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Save.Location = new System.Drawing.Point(577, 4);
			this.bu_Save.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Save.Name = "bu_Save";
			this.bu_Save.Size = new System.Drawing.Size(103, 26);
			this.bu_Save.TabIndex = 2;
			this.bu_Save.Text = "SAVE  FILE";
			this.bu_Save.UseVisualStyleBackColor = true;
			this.bu_Save.Click += new System.EventHandler(this.click_Save);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(685, 4);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(99, 26);
			this.bu_Cancel.TabIndex = 3;
			this.bu_Cancel.Text = "Esc";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.click_Cancel);
			// 
			// bu_Defaults
			// 
			this.bu_Defaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Defaults.Location = new System.Drawing.Point(109, 8);
			this.bu_Defaults.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Defaults.Name = "bu_Defaults";
			this.bu_Defaults.Size = new System.Drawing.Size(115, 22);
			this.bu_Defaults.TabIndex = 1;
			this.bu_Defaults.Text = "restore defaults";
			this.bu_Defaults.UseVisualStyleBackColor = true;
			this.bu_Defaults.Click += new System.EventHandler(this.click_Defaults);
			// 
			// bu_Delete
			// 
			this.bu_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Delete.Location = new System.Drawing.Point(8, 8);
			this.bu_Delete.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Delete.Name = "bu_Delete";
			this.bu_Delete.Size = new System.Drawing.Size(96, 22);
			this.bu_Delete.TabIndex = 0;
			this.bu_Delete.Text = "delete file";
			this.bu_Delete.UseVisualStyleBackColor = true;
			this.bu_Delete.Click += new System.EventHandler(this.click_Delete);
			// 
			// ColorOptionsF
			// 
			this.AcceptButton = this.bu_Save;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(792, 434);
			this.Controls.Add(this.gb_Colors);
			this.Controls.Add(this.pa_bot);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.Name = "ColorOptionsF";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Colors.Cfg";
			this.gb_Colors.ResumeLayout(false);
			this.pa_bot.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
