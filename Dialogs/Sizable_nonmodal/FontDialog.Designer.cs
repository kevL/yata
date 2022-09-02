using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class FontDialog
	{
		SplitContainer sc_Hori;
		ListBox list_Font;
		Panel pa_Right;
		GroupBox gb_Size;
		TextBox tb_FontSize;
		Label lbl_FontSize;
		GroupBox gb_Style;
		CheckBox cb_Bold;
		CheckBox cb_Ital;
		CheckBox cb_Undr;
		CheckBox cb_Strk;
		Button bu_Apply;
		Button bu_Cancel;
		CheckBox cb_Reduced;
		GroupBox gb_Text;
		Label lbl_Lazydog;
		TextBox tb_FontString;

		/// <summary>
		/// This method is required for Windows Forms designer support. Do not
		/// change the method contents inside the source code editor. The Forms
		/// designer might not be able to load this method if it was changed
		/// manually. and you know how that goes ...
		/// </summary>
		private void InitializeComponent()
		{
			this.sc_Hori = new System.Windows.Forms.SplitContainer();
			this.list_Font = new System.Windows.Forms.ListBox();
			this.pa_Right = new System.Windows.Forms.Panel();
			this.cb_Reduced = new System.Windows.Forms.CheckBox();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Apply = new System.Windows.Forms.Button();
			this.gb_Style = new System.Windows.Forms.GroupBox();
			this.cb_Strk = new System.Windows.Forms.CheckBox();
			this.cb_Undr = new System.Windows.Forms.CheckBox();
			this.cb_Ital = new System.Windows.Forms.CheckBox();
			this.cb_Bold = new System.Windows.Forms.CheckBox();
			this.gb_Size = new System.Windows.Forms.GroupBox();
			this.tb_FontSize = new System.Windows.Forms.TextBox();
			this.lbl_FontSize = new System.Windows.Forms.Label();
			this.gb_Text = new System.Windows.Forms.GroupBox();
			this.lbl_Lazydog = new System.Windows.Forms.Label();
			this.tb_FontString = new System.Windows.Forms.TextBox();
			this.sc_Hori.Panel1.SuspendLayout();
			this.sc_Hori.Panel2.SuspendLayout();
			this.sc_Hori.SuspendLayout();
			this.pa_Right.SuspendLayout();
			this.gb_Style.SuspendLayout();
			this.gb_Size.SuspendLayout();
			this.gb_Text.SuspendLayout();
			this.SuspendLayout();
			// 
			// sc_Hori
			// 
			this.sc_Hori.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sc_Hori.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.sc_Hori.Location = new System.Drawing.Point(0, 0);
			this.sc_Hori.Margin = new System.Windows.Forms.Padding(0);
			this.sc_Hori.Name = "sc_Hori";
			this.sc_Hori.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// sc_Hori.Panel1
			// 
			this.sc_Hori.Panel1.Controls.Add(this.list_Font);
			this.sc_Hori.Panel1.Controls.Add(this.pa_Right);
			this.sc_Hori.Panel1MinSize = 220;
			// 
			// sc_Hori.Panel2
			// 
			this.sc_Hori.Panel2.Controls.Add(this.gb_Text);
			this.sc_Hori.Panel2MinSize = 0;
			this.sc_Hori.Size = new System.Drawing.Size(487, 357);
			this.sc_Hori.SplitterDistance = 310;
			this.sc_Hori.SplitterWidth = 3;
			this.sc_Hori.TabIndex = 0;
			this.sc_Hori.Resize += new System.EventHandler(this.OnSplitContainerResize);
			// 
			// list_Font
			// 
			this.list_Font.Dock = System.Windows.Forms.DockStyle.Fill;
			this.list_Font.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.list_Font.IntegralHeight = false;
			this.list_Font.ItemHeight = 20;
			this.list_Font.Location = new System.Drawing.Point(0, 0);
			this.list_Font.Margin = new System.Windows.Forms.Padding(0);
			this.list_Font.Name = "list_Font";
			this.list_Font.Size = new System.Drawing.Size(389, 310);
			this.list_Font.TabIndex = 0;
			this.list_Font.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.fontList_drawitem);
			this.list_Font.SelectedIndexChanged += new System.EventHandler(this.changefont);
			// 
			// pa_Right
			// 
			this.pa_Right.Controls.Add(this.cb_Reduced);
			this.pa_Right.Controls.Add(this.bu_Cancel);
			this.pa_Right.Controls.Add(this.bu_Apply);
			this.pa_Right.Controls.Add(this.gb_Style);
			this.pa_Right.Controls.Add(this.gb_Size);
			this.pa_Right.Dock = System.Windows.Forms.DockStyle.Right;
			this.pa_Right.Location = new System.Drawing.Point(389, 0);
			this.pa_Right.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Right.Name = "pa_Right";
			this.pa_Right.Size = new System.Drawing.Size(98, 310);
			this.pa_Right.TabIndex = 1;
			// 
			// cb_Reduced
			// 
			this.cb_Reduced.Appearance = System.Windows.Forms.Appearance.Button;
			this.cb_Reduced.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.cb_Reduced.Location = new System.Drawing.Point(0, 288);
			this.cb_Reduced.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Reduced.Name = "cb_Reduced";
			this.cb_Reduced.Size = new System.Drawing.Size(98, 22);
			this.cb_Reduced.TabIndex = 4;
			this.cb_Reduced.Text = "reduced";
			this.cb_Reduced.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cb_Reduced.UseVisualStyleBackColor = true;
			this.cb_Reduced.CheckedChanged += new System.EventHandler(this.changefont);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Dock = System.Windows.Forms.DockStyle.Top;
			this.bu_Cancel.Location = new System.Drawing.Point(0, 170);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(98, 35);
			this.bu_Cancel.TabIndex = 3;
			this.bu_Cancel.Text = "— close —";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.click_Cancel);
			// 
			// bu_Apply
			// 
			this.bu_Apply.Dock = System.Windows.Forms.DockStyle.Top;
			this.bu_Apply.Enabled = false;
			this.bu_Apply.Location = new System.Drawing.Point(0, 135);
			this.bu_Apply.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Apply.Name = "bu_Apply";
			this.bu_Apply.Size = new System.Drawing.Size(98, 35);
			this.bu_Apply.TabIndex = 2;
			this.bu_Apply.Text = "— apply —";
			this.bu_Apply.UseVisualStyleBackColor = true;
			this.bu_Apply.Click += new System.EventHandler(this.click_Apply);
			// 
			// gb_Style
			// 
			this.gb_Style.Controls.Add(this.cb_Strk);
			this.gb_Style.Controls.Add(this.cb_Undr);
			this.gb_Style.Controls.Add(this.cb_Ital);
			this.gb_Style.Controls.Add(this.cb_Bold);
			this.gb_Style.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_Style.Location = new System.Drawing.Point(0, 38);
			this.gb_Style.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Style.Name = "gb_Style";
			this.gb_Style.Padding = new System.Windows.Forms.Padding(8, 0, 2, 0);
			this.gb_Style.Size = new System.Drawing.Size(98, 97);
			this.gb_Style.TabIndex = 1;
			this.gb_Style.TabStop = false;
			// 
			// cb_Strk
			// 
			this.cb_Strk.Dock = System.Windows.Forms.DockStyle.Top;
			this.cb_Strk.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Strk.Location = new System.Drawing.Point(8, 73);
			this.cb_Strk.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Strk.Name = "cb_Strk";
			this.cb_Strk.Size = new System.Drawing.Size(88, 20);
			this.cb_Strk.TabIndex = 3;
			this.cb_Strk.Text = "strikeout";
			this.cb_Strk.UseVisualStyleBackColor = true;
			this.cb_Strk.CheckedChanged += new System.EventHandler(this.changefont);
			// 
			// cb_Undr
			// 
			this.cb_Undr.Dock = System.Windows.Forms.DockStyle.Top;
			this.cb_Undr.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Undr.Location = new System.Drawing.Point(8, 53);
			this.cb_Undr.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Undr.Name = "cb_Undr";
			this.cb_Undr.Size = new System.Drawing.Size(88, 20);
			this.cb_Undr.TabIndex = 2;
			this.cb_Undr.Text = "underline";
			this.cb_Undr.UseVisualStyleBackColor = true;
			this.cb_Undr.CheckedChanged += new System.EventHandler(this.changefont);
			// 
			// cb_Ital
			// 
			this.cb_Ital.Dock = System.Windows.Forms.DockStyle.Top;
			this.cb_Ital.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Ital.Location = new System.Drawing.Point(8, 33);
			this.cb_Ital.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Ital.Name = "cb_Ital";
			this.cb_Ital.Size = new System.Drawing.Size(88, 20);
			this.cb_Ital.TabIndex = 1;
			this.cb_Ital.Text = "italic";
			this.cb_Ital.UseVisualStyleBackColor = true;
			this.cb_Ital.CheckedChanged += new System.EventHandler(this.changefont);
			// 
			// cb_Bold
			// 
			this.cb_Bold.Dock = System.Windows.Forms.DockStyle.Top;
			this.cb_Bold.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Bold.Location = new System.Drawing.Point(8, 13);
			this.cb_Bold.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Bold.Name = "cb_Bold";
			this.cb_Bold.Size = new System.Drawing.Size(88, 20);
			this.cb_Bold.TabIndex = 0;
			this.cb_Bold.Text = "bold";
			this.cb_Bold.UseVisualStyleBackColor = true;
			this.cb_Bold.CheckedChanged += new System.EventHandler(this.changefont);
			// 
			// gb_Size
			// 
			this.gb_Size.Controls.Add(this.tb_FontSize);
			this.gb_Size.Controls.Add(this.lbl_FontSize);
			this.gb_Size.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_Size.Location = new System.Drawing.Point(0, 0);
			this.gb_Size.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Size.Name = "gb_Size";
			this.gb_Size.Padding = new System.Windows.Forms.Padding(7, 0, 2, 2);
			this.gb_Size.Size = new System.Drawing.Size(98, 38);
			this.gb_Size.TabIndex = 0;
			this.gb_Size.TabStop = false;
			// 
			// tb_FontSize
			// 
			this.tb_FontSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_FontSize.Dock = System.Windows.Forms.DockStyle.Left;
			this.tb_FontSize.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_FontSize.Location = new System.Drawing.Point(7, 13);
			this.tb_FontSize.Margin = new System.Windows.Forms.Padding(0);
			this.tb_FontSize.Name = "tb_FontSize";
			this.tb_FontSize.Size = new System.Drawing.Size(44, 22);
			this.tb_FontSize.TabIndex = 0;
			this.tb_FontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tb_FontSize.WordWrap = false;
			this.tb_FontSize.TextChanged += new System.EventHandler(this.changefont);
			// 
			// lbl_FontSize
			// 
			this.lbl_FontSize.Location = new System.Drawing.Point(51, 14);
			this.lbl_FontSize.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_FontSize.Name = "lbl_FontSize";
			this.lbl_FontSize.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
			this.lbl_FontSize.Size = new System.Drawing.Size(40, 19);
			this.lbl_FontSize.TabIndex = 1;
			this.lbl_FontSize.Text = "pt";
			this.lbl_FontSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl_FontSize.Click += new System.EventHandler(this.click_pointlabel);
			// 
			// gb_Text
			// 
			this.gb_Text.Controls.Add(this.lbl_Lazydog);
			this.gb_Text.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gb_Text.Location = new System.Drawing.Point(0, 0);
			this.gb_Text.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Text.Name = "gb_Text";
			this.gb_Text.Padding = new System.Windows.Forms.Padding(10, 1, 2, 2);
			this.gb_Text.Size = new System.Drawing.Size(487, 44);
			this.gb_Text.TabIndex = 0;
			this.gb_Text.TabStop = false;
			// 
			// lbl_Lazydog
			// 
			this.lbl_Lazydog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbl_Lazydog.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_Lazydog.Location = new System.Drawing.Point(10, 14);
			this.lbl_Lazydog.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Lazydog.Name = "lbl_Lazydog";
			this.lbl_Lazydog.Size = new System.Drawing.Size(475, 28);
			this.lbl_Lazydog.TabIndex = 0;
			// 
			// tb_FontString
			// 
			this.tb_FontString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_FontString.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tb_FontString.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_FontString.Location = new System.Drawing.Point(0, 357);
			this.tb_FontString.Margin = new System.Windows.Forms.Padding(0);
			this.tb_FontString.Name = "tb_FontString";
			this.tb_FontString.ReadOnly = true;
			this.tb_FontString.Size = new System.Drawing.Size(487, 22);
			this.tb_FontString.TabIndex = 1;
			this.tb_FontString.WordWrap = false;
			// 
			// FontDialog
			// 
			this.AcceptButton = this.bu_Apply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(487, 379);
			this.Controls.Add(this.sc_Hori);
			this.Controls.Add(this.tb_FontString);
			this.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.MinimumSize = new System.Drawing.Size(320, 306);
			this.Name = "FontDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Choose Font ... be patient";
			this.sc_Hori.Panel1.ResumeLayout(false);
			this.sc_Hori.Panel2.ResumeLayout(false);
			this.sc_Hori.ResumeLayout(false);
			this.pa_Right.ResumeLayout(false);
			this.gb_Style.ResumeLayout(false);
			this.gb_Size.ResumeLayout(false);
			this.gb_Size.PerformLayout();
			this.gb_Text.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
