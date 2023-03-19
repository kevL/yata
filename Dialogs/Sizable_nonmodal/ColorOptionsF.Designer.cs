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
			this.bu_Save = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Delete = new System.Windows.Forms.Button();
			this.bu_Defaults = new System.Windows.Forms.Button();
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
			this.gb_Colors.SuspendLayout();
			this.SuspendLayout();
			// 
			// gb_Colors
			// 
			this.gb_Colors.Controls.Add(this.bu_Save);
			this.gb_Colors.Controls.Add(this.bu_Cancel);
			this.gb_Colors.Controls.Add(this.bu_Delete);
			this.gb_Colors.Controls.Add(this.bu_Defaults);
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
			this.gb_Colors.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gb_Colors.Location = new System.Drawing.Point(0, 0);
			this.gb_Colors.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Colors.Name = "gb_Colors";
			this.gb_Colors.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Colors.Size = new System.Drawing.Size(452, 401);
			this.gb_Colors.TabIndex = 0;
			this.gb_Colors.TabStop = false;
			// 
			// bu_Save
			// 
			this.bu_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Save.Location = new System.Drawing.Point(237, 369);
			this.bu_Save.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Save.Name = "bu_Save";
			this.bu_Save.Size = new System.Drawing.Size(103, 26);
			this.bu_Save.TabIndex = 30;
			this.bu_Save.Text = "SAVE  FILE";
			this.bu_Save.UseVisualStyleBackColor = true;
			this.bu_Save.Click += new System.EventHandler(this.click_Save);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(345, 369);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(99, 26);
			this.bu_Cancel.TabIndex = 31;
			this.bu_Cancel.Text = "Esc";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.click_Cancel);
			// 
			// bu_Delete
			// 
			this.bu_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Delete.Location = new System.Drawing.Point(8, 373);
			this.bu_Delete.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Delete.Name = "bu_Delete";
			this.bu_Delete.Size = new System.Drawing.Size(96, 22);
			this.bu_Delete.TabIndex = 28;
			this.bu_Delete.Text = "delete file";
			this.bu_Delete.UseVisualStyleBackColor = true;
			this.bu_Delete.Click += new System.EventHandler(this.click_Delete);
			// 
			// bu_Defaults
			// 
			this.bu_Defaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Defaults.Location = new System.Drawing.Point(109, 373);
			this.bu_Defaults.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Defaults.Name = "bu_Defaults";
			this.bu_Defaults.Size = new System.Drawing.Size(115, 22);
			this.bu_Defaults.TabIndex = 29;
			this.bu_Defaults.Text = "restore defaults";
			this.bu_Defaults.UseVisualStyleBackColor = true;
			this.bu_Defaults.Click += new System.EventHandler(this.click_Defaults);
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
			this.pa_01.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_02.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_03.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_04.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_05.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_06.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_07.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_08.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_09.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_10.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_11.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_12.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_13.Click += new System.EventHandler(this.click_Colorpanel);
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
			this.pa_14.Click += new System.EventHandler(this.click_Colorpanel);
			// 
			// ColorOptionsF
			// 
			this.AcceptButton = this.bu_Save;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(452, 401);
			this.Controls.Add(this.gb_Colors);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.Name = "ColorOptionsF";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Colors.Cfg";
			this.gb_Colors.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
