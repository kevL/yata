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
		Panel pa_Right;
		SplitContainer sc_Hori;
		GroupBox gb_Size;
		Panel pa_Pad;

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
			this.pa_Right = new System.Windows.Forms.Panel();
			this.pa_Pad = new System.Windows.Forms.Panel();
			this.gb_Size = new System.Windows.Forms.GroupBox();
			this.sc_Hori = new System.Windows.Forms.SplitContainer();
			this.gb_Text.SuspendLayout();
			this.gb_Style.SuspendLayout();
			this.pa_Right.SuspendLayout();
			this.gb_Size.SuspendLayout();
			this.sc_Hori.Panel1.SuspendLayout();
			this.sc_Hori.Panel2.SuspendLayout();
			this.sc_Hori.SuspendLayout();
			this.SuspendLayout();
			// 
			// list_Font
			// 
			this.list_Font.Dock = System.Windows.Forms.DockStyle.Fill;
			this.list_Font.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.list_Font.ItemHeight = 20;
			this.list_Font.Location = new System.Drawing.Point(2, 2);
			this.list_Font.Margin = new System.Windows.Forms.Padding(0);
			this.list_Font.Name = "list_Font";
			this.list_Font.Size = new System.Drawing.Size(364, 244);
			this.list_Font.TabIndex = 0;
			this.list_Font.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.fontList_DrawItem);
			this.list_Font.SelectedIndexChanged += new System.EventHandler(this.fontchanged);
			// 
			// bu_Apply
			// 
			this.bu_Apply.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bu_Apply.Enabled = false;
			this.bu_Apply.Location = new System.Drawing.Point(2, 174);
			this.bu_Apply.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Apply.Name = "bu_Apply";
			this.bu_Apply.Size = new System.Drawing.Size(122, 35);
			this.bu_Apply.TabIndex = 3;
			this.bu_Apply.Text = "— apply —";
			this.bu_Apply.UseVisualStyleBackColor = true;
			this.bu_Apply.Click += new System.EventHandler(this.click_Apply);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bu_Cancel.Location = new System.Drawing.Point(2, 209);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(122, 35);
			this.bu_Cancel.TabIndex = 4;
			this.bu_Cancel.Text = "— close —";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.click_Close);
			// 
			// lbl_Size
			// 
			this.lbl_Size.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbl_Size.Location = new System.Drawing.Point(50, 13);
			this.lbl_Size.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Size.Name = "lbl_Size";
			this.lbl_Size.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
			this.lbl_Size.Size = new System.Drawing.Size(70, 23);
			this.lbl_Size.TabIndex = 2;
			this.lbl_Size.Text = "pt";
			this.lbl_Size.Click += new System.EventHandler(this.click_pointlabel);
			// 
			// lbl_Lazydog
			// 
			this.lbl_Lazydog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbl_Lazydog.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_Lazydog.Location = new System.Drawing.Point(10, 18);
			this.lbl_Lazydog.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Lazydog.Name = "lbl_Lazydog";
			this.lbl_Lazydog.Size = new System.Drawing.Size(473, 103);
			this.lbl_Lazydog.TabIndex = 0;
			// 
			// tb_FontSize
			// 
			this.tb_FontSize.Dock = System.Windows.Forms.DockStyle.Left;
			this.tb_FontSize.Location = new System.Drawing.Point(8, 13);
			this.tb_FontSize.Margin = new System.Windows.Forms.Padding(0);
			this.tb_FontSize.Name = "tb_FontSize";
			this.tb_FontSize.Size = new System.Drawing.Size(42, 20);
			this.tb_FontSize.TabIndex = 1;
			this.tb_FontSize.TextChanged += new System.EventHandler(this.fontchanged);
			// 
			// gb_Text
			// 
			this.gb_Text.Controls.Add(this.lbl_Lazydog);
			this.gb_Text.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gb_Text.Location = new System.Drawing.Point(2, 0);
			this.gb_Text.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Text.Name = "gb_Text";
			this.gb_Text.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.gb_Text.Size = new System.Drawing.Size(488, 126);
			this.gb_Text.TabIndex = 2;
			this.gb_Text.TabStop = false;
			// 
			// tb_FontString
			// 
			this.tb_FontString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_FontString.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tb_FontString.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_FontString.HideSelection = false;
			this.tb_FontString.Location = new System.Drawing.Point(2, 126);
			this.tb_FontString.Margin = new System.Windows.Forms.Padding(0);
			this.tb_FontString.Name = "tb_FontString";
			this.tb_FontString.ReadOnly = true;
			this.tb_FontString.Size = new System.Drawing.Size(488, 22);
			this.tb_FontString.TabIndex = 3;
			this.tb_FontString.WordWrap = false;
			// 
			// gb_Style
			// 
			this.gb_Style.Controls.Add(this.cb_Strk);
			this.gb_Style.Controls.Add(this.cb_Undr);
			this.gb_Style.Controls.Add(this.cb_Ital);
			this.gb_Style.Controls.Add(this.cb_Bold);
			this.gb_Style.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_Style.Location = new System.Drawing.Point(2, 38);
			this.gb_Style.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Style.Name = "gb_Style";
			this.gb_Style.Padding = new System.Windows.Forms.Padding(8, 0, 2, 0);
			this.gb_Style.Size = new System.Drawing.Size(122, 97);
			this.gb_Style.TabIndex = 3;
			this.gb_Style.TabStop = false;
			// 
			// cb_Strk
			// 
			this.cb_Strk.Dock = System.Windows.Forms.DockStyle.Top;
			this.cb_Strk.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Strk.Location = new System.Drawing.Point(8, 73);
			this.cb_Strk.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Strk.Name = "cb_Strk";
			this.cb_Strk.Size = new System.Drawing.Size(112, 20);
			this.cb_Strk.TabIndex = 3;
			this.cb_Strk.Text = "strikeout";
			this.cb_Strk.UseVisualStyleBackColor = true;
			this.cb_Strk.CheckedChanged += new System.EventHandler(this.fontchanged);
			// 
			// cb_Undr
			// 
			this.cb_Undr.Dock = System.Windows.Forms.DockStyle.Top;
			this.cb_Undr.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Undr.Location = new System.Drawing.Point(8, 53);
			this.cb_Undr.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Undr.Name = "cb_Undr";
			this.cb_Undr.Size = new System.Drawing.Size(112, 20);
			this.cb_Undr.TabIndex = 2;
			this.cb_Undr.Text = "underline";
			this.cb_Undr.UseVisualStyleBackColor = true;
			this.cb_Undr.CheckedChanged += new System.EventHandler(this.fontchanged);
			// 
			// cb_Ital
			// 
			this.cb_Ital.Dock = System.Windows.Forms.DockStyle.Top;
			this.cb_Ital.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Ital.Location = new System.Drawing.Point(8, 33);
			this.cb_Ital.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Ital.Name = "cb_Ital";
			this.cb_Ital.Size = new System.Drawing.Size(112, 20);
			this.cb_Ital.TabIndex = 1;
			this.cb_Ital.Text = "italic";
			this.cb_Ital.UseVisualStyleBackColor = true;
			this.cb_Ital.CheckedChanged += new System.EventHandler(this.fontchanged);
			// 
			// cb_Bold
			// 
			this.cb_Bold.Dock = System.Windows.Forms.DockStyle.Top;
			this.cb_Bold.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Bold.Location = new System.Drawing.Point(8, 13);
			this.cb_Bold.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Bold.Name = "cb_Bold";
			this.cb_Bold.Size = new System.Drawing.Size(112, 20);
			this.cb_Bold.TabIndex = 0;
			this.cb_Bold.Text = "bold";
			this.cb_Bold.UseVisualStyleBackColor = true;
			this.cb_Bold.CheckedChanged += new System.EventHandler(this.fontchanged);
			// 
			// pa_Right
			// 
			this.pa_Right.Controls.Add(this.pa_Pad);
			this.pa_Right.Controls.Add(this.gb_Style);
			this.pa_Right.Controls.Add(this.gb_Size);
			this.pa_Right.Controls.Add(this.bu_Apply);
			this.pa_Right.Controls.Add(this.bu_Cancel);
			this.pa_Right.Dock = System.Windows.Forms.DockStyle.Right;
			this.pa_Right.Location = new System.Drawing.Point(366, 2);
			this.pa_Right.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Right.Name = "pa_Right";
			this.pa_Right.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this.pa_Right.Size = new System.Drawing.Size(124, 244);
			this.pa_Right.TabIndex = 1;
			// 
			// pa_Pad
			// 
			this.pa_Pad.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pa_Pad.Location = new System.Drawing.Point(2, 135);
			this.pa_Pad.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Pad.Name = "pa_Pad";
			this.pa_Pad.Size = new System.Drawing.Size(122, 39);
			this.pa_Pad.TabIndex = 6;
			// 
			// gb_Size
			// 
			this.gb_Size.Controls.Add(this.lbl_Size);
			this.gb_Size.Controls.Add(this.tb_FontSize);
			this.gb_Size.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_Size.Location = new System.Drawing.Point(2, 0);
			this.gb_Size.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Size.Name = "gb_Size";
			this.gb_Size.Padding = new System.Windows.Forms.Padding(8, 0, 2, 2);
			this.gb_Size.Size = new System.Drawing.Size(122, 38);
			this.gb_Size.TabIndex = 5;
			this.gb_Size.TabStop = false;
			// 
			// sc_Hori
			// 
			this.sc_Hori.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sc_Hori.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.sc_Hori.Location = new System.Drawing.Point(0, 0);
			this.sc_Hori.Margin = new System.Windows.Forms.Padding(0);
			this.sc_Hori.Name = "sc_Hori";
			this.sc_Hori.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// sc_Hori.Panel1
			// 
			this.sc_Hori.Panel1.Controls.Add(this.list_Font);
			this.sc_Hori.Panel1.Controls.Add(this.pa_Right);
			this.sc_Hori.Panel1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 0);
			this.sc_Hori.Panel1MinSize = 213;
			// 
			// sc_Hori.Panel2
			// 
			this.sc_Hori.Panel2.Controls.Add(this.gb_Text);
			this.sc_Hori.Panel2.Controls.Add(this.tb_FontString);
			this.sc_Hori.Panel2.Padding = new System.Windows.Forms.Padding(2, 0, 2, 2);
			this.sc_Hori.Panel2MinSize = 24;
			this.sc_Hori.Size = new System.Drawing.Size(492, 399);
			this.sc_Hori.SplitterDistance = 246;
			this.sc_Hori.SplitterWidth = 3;
			this.sc_Hori.TabIndex = 4;
			this.sc_Hori.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.OnSplitterMoved);
			// 
			// FontF
			// 
			this.AcceptButton = this.bu_Apply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(492, 399);
			this.Controls.Add(this.sc_Hori);
			this.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "FontF";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Choose Font ... be patient";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
			this.Load += new System.EventHandler(this.OnLoad);
			this.Resize += new System.EventHandler(this.OnResize);
			this.gb_Text.ResumeLayout(false);
			this.gb_Style.ResumeLayout(false);
			this.pa_Right.ResumeLayout(false);
			this.gb_Size.ResumeLayout(false);
			this.gb_Size.PerformLayout();
			this.sc_Hori.Panel1.ResumeLayout(false);
			this.sc_Hori.Panel2.ResumeLayout(false);
			this.sc_Hori.Panel2.PerformLayout();
			this.sc_Hori.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
