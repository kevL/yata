using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ColorSelectorDialog
	{
		Panel pa_Colortable;
		Panel pa_Valslider;
		Panel pa_Color;
		Panel pa_Colorpre;
		TextBox tb_Red;
		TextBox tb_Green;
		TextBox tb_Blue;
		Label la_Red;
		Label la_Green;
		Label la_Blue;
		ComboBox cb_NetColors;
		ComboBox cb_SysColors;
		Button bu_Help;
		Button bu_Accept;
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
			this.pa_Colortable = new System.Windows.Forms.Panel();
			this.pa_Valslider = new System.Windows.Forms.Panel();
			this.pa_Color = new System.Windows.Forms.Panel();
			this.pa_Colorpre = new System.Windows.Forms.Panel();
			this.tb_Red = new System.Windows.Forms.TextBox();
			this.tb_Green = new System.Windows.Forms.TextBox();
			this.tb_Blue = new System.Windows.Forms.TextBox();
			this.la_Red = new System.Windows.Forms.Label();
			this.la_Green = new System.Windows.Forms.Label();
			this.la_Blue = new System.Windows.Forms.Label();
			this.cb_NetColors = new System.Windows.Forms.ComboBox();
			this.cb_SysColors = new System.Windows.Forms.ComboBox();
			this.bu_Help = new System.Windows.Forms.Button();
			this.bu_Accept = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// pa_Colortable
			// 
			this.pa_Colortable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_Colortable.Location = new System.Drawing.Point(5, 5);
			this.pa_Colortable.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Colortable.Name = "pa_Colortable";
			this.pa_Colortable.Size = new System.Drawing.Size(360, 202);
			this.pa_Colortable.TabIndex = 0;
			this.pa_Colortable.Paint += new System.Windows.Forms.PaintEventHandler(this.paint_Colortable);
			this.pa_Colortable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_Colortable);
			this.pa_Colortable.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mousemove_Colortable);
			this.pa_Colortable.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_Colortable);
			// 
			// pa_Valslider
			// 
			this.pa_Valslider.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_Valslider.Location = new System.Drawing.Point(371, 5);
			this.pa_Valslider.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Valslider.Name = "pa_Valslider";
			this.pa_Valslider.Size = new System.Drawing.Size(50, 256);
			this.pa_Valslider.TabIndex = 1;
			this.pa_Valslider.Paint += new System.Windows.Forms.PaintEventHandler(this.paint_Valslider);
			this.pa_Valslider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_Valslider);
			this.pa_Valslider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mousemove_Valslider);
			this.pa_Valslider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_Valslider);
			// 
			// pa_Color
			// 
			this.pa_Color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_Color.Location = new System.Drawing.Point(5, 213);
			this.pa_Color.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Color.Name = "pa_Color";
			this.pa_Color.Size = new System.Drawing.Size(75, 35);
			this.pa_Color.TabIndex = 2;
			this.toolTip1.SetToolTip(this.pa_Color, "LMB - set values\r\n[Ctrl]+LMB - copy color to buffer\r\nRMB - paste color from buffe" +
		"r");
			this.pa_Color.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Color);
			// 
			// pa_Colorpre
			// 
			this.pa_Colorpre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_Colorpre.Location = new System.Drawing.Point(5, 247);
			this.pa_Colorpre.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Colorpre.Name = "pa_Colorpre";
			this.pa_Colorpre.Size = new System.Drawing.Size(75, 35);
			this.pa_Colorpre.TabIndex = 3;
			this.toolTip1.SetToolTip(this.pa_Colorpre, "LMB - set values\r\n[Ctrl]+LMB - copy color to buffer\r\nRMB - set values and assign " +
		"color");
			this.pa_Colorpre.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseclick_Color);
			// 
			// tb_Red
			// 
			this.tb_Red.Location = new System.Drawing.Point(91, 213);
			this.tb_Red.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Red.Name = "tb_Red";
			this.tb_Red.Size = new System.Drawing.Size(40, 20);
			this.tb_Red.TabIndex = 4;
			this.tb_Red.WordWrap = false;
			this.tb_Red.TextChanged += new System.EventHandler(this.textchanged_tb_Rgb);
			this.tb_Red.Leave += new System.EventHandler(this.leave_tb_Rgb);
			// 
			// tb_Green
			// 
			this.tb_Green.Location = new System.Drawing.Point(91, 238);
			this.tb_Green.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Green.Name = "tb_Green";
			this.tb_Green.Size = new System.Drawing.Size(40, 20);
			this.tb_Green.TabIndex = 6;
			this.tb_Green.WordWrap = false;
			this.tb_Green.TextChanged += new System.EventHandler(this.textchanged_tb_Rgb);
			this.tb_Green.Leave += new System.EventHandler(this.leave_tb_Rgb);
			// 
			// tb_Blue
			// 
			this.tb_Blue.Location = new System.Drawing.Point(91, 263);
			this.tb_Blue.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Blue.Name = "tb_Blue";
			this.tb_Blue.Size = new System.Drawing.Size(40, 20);
			this.tb_Blue.TabIndex = 8;
			this.tb_Blue.WordWrap = false;
			this.tb_Blue.TextChanged += new System.EventHandler(this.textchanged_tb_Rgb);
			this.tb_Blue.Leave += new System.EventHandler(this.leave_tb_Rgb);
			// 
			// la_Red
			// 
			this.la_Red.Location = new System.Drawing.Point(135, 213);
			this.la_Red.Margin = new System.Windows.Forms.Padding(0);
			this.la_Red.Name = "la_Red";
			this.la_Red.Size = new System.Drawing.Size(15, 16);
			this.la_Red.TabIndex = 5;
			this.la_Red.Text = "r";
			this.la_Red.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.la_Red.Click += new System.EventHandler(this.click_la_Rgb);
			// 
			// la_Green
			// 
			this.la_Green.Location = new System.Drawing.Point(135, 238);
			this.la_Green.Margin = new System.Windows.Forms.Padding(0);
			this.la_Green.Name = "la_Green";
			this.la_Green.Size = new System.Drawing.Size(15, 16);
			this.la_Green.TabIndex = 7;
			this.la_Green.Text = "g";
			this.la_Green.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.la_Green.Click += new System.EventHandler(this.click_la_Rgb);
			// 
			// la_Blue
			// 
			this.la_Blue.Location = new System.Drawing.Point(135, 263);
			this.la_Blue.Margin = new System.Windows.Forms.Padding(0);
			this.la_Blue.Name = "la_Blue";
			this.la_Blue.Size = new System.Drawing.Size(15, 16);
			this.la_Blue.TabIndex = 9;
			this.la_Blue.Text = "b";
			this.la_Blue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.la_Blue.Click += new System.EventHandler(this.click_la_Rgb);
			// 
			// cb_NetColors
			// 
			this.cb_NetColors.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cb_NetColors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cb_NetColors.FormattingEnabled = true;
			this.cb_NetColors.Location = new System.Drawing.Point(175, 213);
			this.cb_NetColors.Margin = new System.Windows.Forms.Padding(0);
			this.cb_NetColors.Name = "cb_NetColors";
			this.cb_NetColors.Size = new System.Drawing.Size(190, 21);
			this.cb_NetColors.TabIndex = 10;
			this.cb_NetColors.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.drawitem_cb_Colors);
			this.cb_NetColors.SelectedIndexChanged += new System.EventHandler(this.selectedindexchanged_cb_Colors);
			// 
			// cb_SysColors
			// 
			this.cb_SysColors.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cb_SysColors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cb_SysColors.FormattingEnabled = true;
			this.cb_SysColors.Location = new System.Drawing.Point(175, 240);
			this.cb_SysColors.Margin = new System.Windows.Forms.Padding(0);
			this.cb_SysColors.Name = "cb_SysColors";
			this.cb_SysColors.Size = new System.Drawing.Size(190, 21);
			this.cb_SysColors.TabIndex = 11;
			this.cb_SysColors.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.drawitem_cb_Colors);
			this.cb_SysColors.SelectedIndexChanged += new System.EventHandler(this.selectedindexchanged_cb_Colors);
			// 
			// bu_Help
			// 
			this.bu_Help.Location = new System.Drawing.Point(175, 272);
			this.bu_Help.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Help.Name = "bu_Help";
			this.bu_Help.Size = new System.Drawing.Size(75, 21);
			this.bu_Help.TabIndex = 12;
			this.bu_Help.Text = "hep";
			this.bu_Help.UseVisualStyleBackColor = true;
			this.bu_Help.Click += new System.EventHandler(this.click_Help);
			// 
			// bu_Accept
			// 
			this.bu_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Accept.Location = new System.Drawing.Point(262, 267);
			this.bu_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Accept.Name = "bu_Accept";
			this.bu_Accept.Size = new System.Drawing.Size(80, 26);
			this.bu_Accept.TabIndex = 13;
			this.bu_Accept.Text = "accept";
			this.bu_Accept.UseVisualStyleBackColor = true;
			this.bu_Accept.Click += new System.EventHandler(this.click_Accept);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(343, 267);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(80, 26);
			this.bu_Cancel.TabIndex = 14;
			this.bu_Cancel.Text = "Esc";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 8000;
			this.toolTip1.InitialDelay = 100;
			this.toolTip1.ReshowDelay = 50;
			// 
			// ColorSelectorDialog
			// 
			this.AcceptButton = this.bu_Accept;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(426, 296);
			this.Controls.Add(this.bu_Help);
			this.Controls.Add(this.pa_Colortable);
			this.Controls.Add(this.pa_Valslider);
			this.Controls.Add(this.pa_Color);
			this.Controls.Add(this.pa_Colorpre);
			this.Controls.Add(this.tb_Blue);
			this.Controls.Add(this.tb_Green);
			this.Controls.Add(this.tb_Red);
			this.Controls.Add(this.la_Blue);
			this.Controls.Add(this.la_Green);
			this.Controls.Add(this.la_Red);
			this.Controls.Add(this.cb_NetColors);
			this.Controls.Add(this.cb_SysColors);
			this.Controls.Add(this.bu_Accept);
			this.Controls.Add(this.bu_Cancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "ColorSelectorDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
