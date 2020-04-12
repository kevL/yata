using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace yata
{
	sealed partial class FontF
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		IContainer components = null; // not used.

		ListBox list_Font;
		Button bu_Apply;
		Button bu_Cancel;
		Label lbl_Size;
		Label lbl_Lazydog;
		TextBox tb_FontSize;
		GroupBox gb_Text;
		CheckBox cb_Bold;
		CheckBox cb_Ital;
		CheckBox cb_Undr;
		CheckBox cb_Strk;
		TextBox tb_FontString;
		GroupBox gb_Style;

		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed</param>
		protected override void Dispose(bool disposing)
		{
			DisposeFonts(); // kL_add.

			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.list_Font = new System.Windows.Forms.ListBox();
			this.bu_Apply = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.lbl_Size = new System.Windows.Forms.Label();
			this.lbl_Lazydog = new System.Windows.Forms.Label();
			this.tb_FontSize = new System.Windows.Forms.TextBox();
			this.gb_Text = new System.Windows.Forms.GroupBox();
			this.tb_FontString = new System.Windows.Forms.TextBox();
			this.gb_Style = new System.Windows.Forms.GroupBox();
			this.cb_Strk = new System.Windows.Forms.CheckBox();
			this.cb_Undr = new System.Windows.Forms.CheckBox();
			this.cb_Ital = new System.Windows.Forms.CheckBox();
			this.cb_Bold = new System.Windows.Forms.CheckBox();
			this.gb_Text.SuspendLayout();
			this.gb_Style.SuspendLayout();
			this.SuspendLayout();
			// 
			// list_Font
			// 
			this.list_Font.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.list_Font.FormattingEnabled = true;
			this.list_Font.ItemHeight = 20;
			this.list_Font.Location = new System.Drawing.Point(5, 5);
			this.list_Font.Margin = new System.Windows.Forms.Padding(0);
			this.list_Font.Name = "list_Font";
			this.list_Font.Size = new System.Drawing.Size(270, 244);
			this.list_Font.TabIndex = 0;
			this.list_Font.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.fontList_DrawItem);
			this.list_Font.SelectedIndexChanged += new System.EventHandler(this.fontchanged);
			// 
			// bu_Apply
			// 
			this.bu_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Apply.Enabled = false;
			this.bu_Apply.Location = new System.Drawing.Point(280, 150);
			this.bu_Apply.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Apply.Name = "bu_Apply";
			this.bu_Apply.Size = new System.Drawing.Size(175, 35);
			this.bu_Apply.TabIndex = 4;
			this.bu_Apply.Text = "— apply —";
			this.bu_Apply.UseVisualStyleBackColor = true;
			this.bu_Apply.Click += new System.EventHandler(this.click_Apply);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(280, 189);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(175, 35);
			this.bu_Cancel.TabIndex = 5;
			this.bu_Cancel.Text = "— close —";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.click_Close);
			// 
			// lbl_Size
			// 
			this.lbl_Size.Location = new System.Drawing.Point(322, 10);
			this.lbl_Size.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Size.Name = "lbl_Size";
			this.lbl_Size.Size = new System.Drawing.Size(26, 15);
			this.lbl_Size.TabIndex = 2;
			this.lbl_Size.Text = "pt";
			this.lbl_Size.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl_Size.Click += new System.EventHandler(this.click_pointlabel);
			// 
			// lbl_Lazydog
			// 
			this.lbl_Lazydog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.lbl_Lazydog.Location = new System.Drawing.Point(5, 12);
			this.lbl_Lazydog.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Lazydog.Name = "lbl_Lazydog";
			this.lbl_Lazydog.Size = new System.Drawing.Size(440, 88);
			this.lbl_Lazydog.TabIndex = 0;
			// 
			// tb_FontSize
			// 
			this.tb_FontSize.Location = new System.Drawing.Point(282, 10);
			this.tb_FontSize.Margin = new System.Windows.Forms.Padding(0);
			this.tb_FontSize.Name = "tb_FontSize";
			this.tb_FontSize.Size = new System.Drawing.Size(40, 20);
			this.tb_FontSize.TabIndex = 1;
			this.tb_FontSize.TextChanged += new System.EventHandler(this.fontchanged);
			// 
			// gb_Text
			// 
			this.gb_Text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.gb_Text.Controls.Add(this.lbl_Lazydog);
			this.gb_Text.Location = new System.Drawing.Point(5, 250);
			this.gb_Text.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Text.Name = "gb_Text";
			this.gb_Text.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Text.Size = new System.Drawing.Size(450, 105);
			this.gb_Text.TabIndex = 6;
			this.gb_Text.TabStop = false;
			// 
			// tb_FontString
			// 
			this.tb_FontString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tb_FontString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_FontString.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_FontString.HideSelection = false;
			this.tb_FontString.Location = new System.Drawing.Point(5, 358);
			this.tb_FontString.Margin = new System.Windows.Forms.Padding(0);
			this.tb_FontString.Name = "tb_FontString";
			this.tb_FontString.ReadOnly = true;
			this.tb_FontString.Size = new System.Drawing.Size(450, 22);
			this.tb_FontString.TabIndex = 7;
			this.tb_FontString.WordWrap = false;
			// 
			// gb_Style
			// 
			this.gb_Style.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.gb_Style.Controls.Add(this.cb_Strk);
			this.gb_Style.Controls.Add(this.cb_Undr);
			this.gb_Style.Controls.Add(this.cb_Ital);
			this.gb_Style.Controls.Add(this.cb_Bold);
			this.gb_Style.Location = new System.Drawing.Point(280, 31);
			this.gb_Style.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Style.Name = "gb_Style";
			this.gb_Style.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Style.Size = new System.Drawing.Size(175, 100);
			this.gb_Style.TabIndex = 3;
			this.gb_Style.TabStop = false;
			// 
			// cb_Strk
			// 
			this.cb_Strk.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Strk.Location = new System.Drawing.Point(10, 75);
			this.cb_Strk.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Strk.Name = "cb_Strk";
			this.cb_Strk.Size = new System.Drawing.Size(80, 20);
			this.cb_Strk.TabIndex = 3;
			this.cb_Strk.Text = "strikeout";
			this.cb_Strk.UseVisualStyleBackColor = true;
			this.cb_Strk.CheckedChanged += new System.EventHandler(this.fontchanged);
			// 
			// cb_Undr
			// 
			this.cb_Undr.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Undr.Location = new System.Drawing.Point(10, 55);
			this.cb_Undr.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Undr.Name = "cb_Undr";
			this.cb_Undr.Size = new System.Drawing.Size(80, 20);
			this.cb_Undr.TabIndex = 2;
			this.cb_Undr.Text = "underline";
			this.cb_Undr.UseVisualStyleBackColor = true;
			this.cb_Undr.CheckedChanged += new System.EventHandler(this.fontchanged);
			// 
			// cb_Ital
			// 
			this.cb_Ital.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Ital.Location = new System.Drawing.Point(10, 35);
			this.cb_Ital.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Ital.Name = "cb_Ital";
			this.cb_Ital.Size = new System.Drawing.Size(55, 20);
			this.cb_Ital.TabIndex = 1;
			this.cb_Ital.Text = "italic";
			this.cb_Ital.UseVisualStyleBackColor = true;
			this.cb_Ital.CheckedChanged += new System.EventHandler(this.fontchanged);
			// 
			// cb_Bold
			// 
			this.cb_Bold.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Bold.Location = new System.Drawing.Point(10, 15);
			this.cb_Bold.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Bold.Name = "cb_Bold";
			this.cb_Bold.Size = new System.Drawing.Size(55, 20);
			this.cb_Bold.TabIndex = 0;
			this.cb_Bold.Text = "bold";
			this.cb_Bold.UseVisualStyleBackColor = true;
			this.cb_Bold.CheckedChanged += new System.EventHandler(this.fontchanged);
			// 
			// FontF
			// 
			this.AcceptButton = this.bu_Apply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(460, 384);
			this.Controls.Add(this.gb_Style);
			this.Controls.Add(this.tb_FontString);
			this.Controls.Add(this.gb_Text);
			this.Controls.Add(this.bu_Apply);
			this.Controls.Add(this.tb_FontSize);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.list_Font);
			this.Controls.Add(this.lbl_Size);
			this.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "FontF";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Choose Font ... be patient";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
			this.Load += new System.EventHandler(this.OnLoad);
			this.gb_Text.ResumeLayout(false);
			this.gb_Style.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
