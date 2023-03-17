using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ColorSelectorD
	{
		Panel pa_Colortable;
		Panel pa_Valslider;
		Panel pa_Color;
		TextBox tb_Red;
		TextBox tb_Green;
		TextBox tb_Blue;
		Label la_Red;
		Label la_Green;
		Label la_Blue;
		Button bu_Accept;
		Button bu_Cancel;

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.pa_Colortable = new System.Windows.Forms.Panel();
			this.pa_Valslider = new System.Windows.Forms.Panel();
			this.pa_Color = new System.Windows.Forms.Panel();
			this.tb_Red = new System.Windows.Forms.TextBox();
			this.tb_Green = new System.Windows.Forms.TextBox();
			this.tb_Blue = new System.Windows.Forms.TextBox();
			this.la_Red = new System.Windows.Forms.Label();
			this.la_Green = new System.Windows.Forms.Label();
			this.la_Blue = new System.Windows.Forms.Label();
			this.bu_Accept = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pa_Colortable
			// 
			this.pa_Colortable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_Colortable.Location = new System.Drawing.Point(14, 7);
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
			this.pa_Valslider.Location = new System.Drawing.Point(381, 7);
			this.pa_Valslider.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Valslider.Name = "pa_Valslider";
			this.pa_Valslider.Size = new System.Drawing.Size(50, 202);
			this.pa_Valslider.TabIndex = 1;
			this.pa_Valslider.Paint += new System.Windows.Forms.PaintEventHandler(this.paint_Valslider);
			this.pa_Valslider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_Valslider);
			this.pa_Valslider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mousemove_Valslider);
			this.pa_Valslider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_Valslider);
			// 
			// pa_Color
			// 
			this.pa_Color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_Color.Location = new System.Drawing.Point(14, 215);
			this.pa_Color.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Color.Name = "pa_Color";
			this.pa_Color.Size = new System.Drawing.Size(75, 35);
			this.pa_Color.TabIndex = 2;
			// 
			// tb_Red
			// 
			this.tb_Red.Location = new System.Drawing.Point(100, 215);
			this.tb_Red.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Red.Name = "tb_Red";
			this.tb_Red.Size = new System.Drawing.Size(40, 20);
			this.tb_Red.TabIndex = 3;
			this.tb_Red.WordWrap = false;
			this.tb_Red.TextChanged += new System.EventHandler(this.textchanged_Rgb);
			this.tb_Red.Leave += new System.EventHandler(this.leave_Rgb);
			// 
			// tb_Green
			// 
			this.tb_Green.Location = new System.Drawing.Point(100, 240);
			this.tb_Green.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Green.Name = "tb_Green";
			this.tb_Green.Size = new System.Drawing.Size(40, 20);
			this.tb_Green.TabIndex = 5;
			this.tb_Green.WordWrap = false;
			this.tb_Green.TextChanged += new System.EventHandler(this.textchanged_Rgb);
			this.tb_Green.Leave += new System.EventHandler(this.leave_Rgb);
			// 
			// tb_Blue
			// 
			this.tb_Blue.Location = new System.Drawing.Point(100, 265);
			this.tb_Blue.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Blue.Name = "tb_Blue";
			this.tb_Blue.Size = new System.Drawing.Size(40, 20);
			this.tb_Blue.TabIndex = 7;
			this.tb_Blue.WordWrap = false;
			this.tb_Blue.TextChanged += new System.EventHandler(this.textchanged_Rgb);
			this.tb_Blue.Leave += new System.EventHandler(this.leave_Rgb);
			// 
			// la_Red
			// 
			this.la_Red.Location = new System.Drawing.Point(144, 215);
			this.la_Red.Margin = new System.Windows.Forms.Padding(0);
			this.la_Red.Name = "la_Red";
			this.la_Red.Size = new System.Drawing.Size(15, 16);
			this.la_Red.TabIndex = 4;
			this.la_Red.Text = "r";
			this.la_Red.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_Green
			// 
			this.la_Green.Location = new System.Drawing.Point(144, 240);
			this.la_Green.Margin = new System.Windows.Forms.Padding(0);
			this.la_Green.Name = "la_Green";
			this.la_Green.Size = new System.Drawing.Size(15, 16);
			this.la_Green.TabIndex = 6;
			this.la_Green.Text = "g";
			this.la_Green.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_Blue
			// 
			this.la_Blue.Location = new System.Drawing.Point(144, 265);
			this.la_Blue.Margin = new System.Windows.Forms.Padding(0);
			this.la_Blue.Name = "la_Blue";
			this.la_Blue.Size = new System.Drawing.Size(15, 16);
			this.la_Blue.TabIndex = 8;
			this.la_Blue.Text = "b";
			this.la_Blue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// bu_Accept
			// 
			this.bu_Accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Accept.Location = new System.Drawing.Point(269, 344);
			this.bu_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Accept.Name = "bu_Accept";
			this.bu_Accept.Size = new System.Drawing.Size(80, 26);
			this.bu_Accept.TabIndex = 9;
			this.bu_Accept.Text = "accept";
			this.bu_Accept.UseVisualStyleBackColor = true;
			this.bu_Accept.Click += new System.EventHandler(this.click_Accept);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(357, 344);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(80, 26);
			this.bu_Cancel.TabIndex = 10;
			this.bu_Cancel.Text = "Esc";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			// 
			// ColorSelectorD
			// 
			this.AcceptButton = this.bu_Accept;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(444, 376);
			this.Controls.Add(this.pa_Colortable);
			this.Controls.Add(this.pa_Valslider);
			this.Controls.Add(this.pa_Color);
			this.Controls.Add(this.tb_Blue);
			this.Controls.Add(this.tb_Green);
			this.Controls.Add(this.tb_Red);
			this.Controls.Add(this.la_Blue);
			this.Controls.Add(this.la_Green);
			this.Controls.Add(this.la_Red);
			this.Controls.Add(this.bu_Accept);
			this.Controls.Add(this.bu_Cancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "ColorSelectorD";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Color Selector";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
