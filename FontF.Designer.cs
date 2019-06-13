using System.Windows.Forms;


namespace yata
{
	sealed partial class FontF
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		System.ComponentModel.IContainer components = null;

		ListBox list_Font;
		Button btn_Ok;
		Button btn_Apply;
		Button btn_Cancel;
		Label lbl_Size;
		ListBox list_Size;
		Label lbl_Example;
		TextBox tb_Size;
		GroupBox gb_Example;
		CheckBox cb_Bold;
		CheckBox cb_Italic;
		TextBox tb_NetString;

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
			this.btn_Ok = new System.Windows.Forms.Button();
			this.btn_Apply = new System.Windows.Forms.Button();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.lbl_Size = new System.Windows.Forms.Label();
			this.list_Size = new System.Windows.Forms.ListBox();
			this.lbl_Example = new System.Windows.Forms.Label();
			this.tb_Size = new System.Windows.Forms.TextBox();
			this.gb_Example = new System.Windows.Forms.GroupBox();
			this.cb_Bold = new System.Windows.Forms.CheckBox();
			this.cb_Italic = new System.Windows.Forms.CheckBox();
			this.tb_NetString = new System.Windows.Forms.TextBox();
			this.gb_Example.SuspendLayout();
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
			this.list_Font.Size = new System.Drawing.Size(230, 244);
			this.list_Font.TabIndex = 0;
			this.list_Font.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.fontList_DrawItem);
			this.list_Font.SelectedIndexChanged += new System.EventHandler(this.fontList_SelectedIndexChanged);
			// 
			// btn_Ok
			// 
			this.btn_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Ok.Location = new System.Drawing.Point(275, 115);
			this.btn_Ok.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Ok.Name = "btn_Ok";
			this.btn_Ok.Size = new System.Drawing.Size(185, 40);
			this.btn_Ok.TabIndex = 6;
			this.btn_Ok.Text = "— ok —";
			this.btn_Ok.UseVisualStyleBackColor = true;
			this.btn_Ok.Click += new System.EventHandler(this.btnOk_click);
			// 
			// btn_Apply
			// 
			this.btn_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Apply.Location = new System.Drawing.Point(275, 160);
			this.btn_Apply.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Apply.Name = "btn_Apply";
			this.btn_Apply.Size = new System.Drawing.Size(185, 40);
			this.btn_Apply.TabIndex = 7;
			this.btn_Apply.Text = "— apply —";
			this.btn_Apply.UseVisualStyleBackColor = true;
			this.btn_Apply.Click += new System.EventHandler(this.btnApply_click);
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Cancel.Location = new System.Drawing.Point(275, 205);
			this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(185, 40);
			this.btn_Cancel.TabIndex = 8;
			this.btn_Cancel.Text = "— cancel —";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.btnCancel_click);
			// 
			// lbl_Size
			// 
			this.lbl_Size.Location = new System.Drawing.Point(315, 10);
			this.lbl_Size.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Size.Name = "lbl_Size";
			this.lbl_Size.Size = new System.Drawing.Size(20, 20);
			this.lbl_Size.TabIndex = 3;
			this.lbl_Size.Text = "pt";
			this.lbl_Size.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// list_Size
			// 
			this.list_Size.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.list_Size.FormattingEnabled = true;
			this.list_Size.ItemHeight = 15;
			this.list_Size.Location = new System.Drawing.Point(240, 5);
			this.list_Size.Margin = new System.Windows.Forms.Padding(0);
			this.list_Size.Name = "list_Size";
			this.list_Size.Size = new System.Drawing.Size(30, 244);
			this.list_Size.TabIndex = 1;
			this.list_Size.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.sizeList_DrawItem);
			this.list_Size.SelectedIndexChanged += new System.EventHandler(this.fontSize_SelectedIndexChanged);
			// 
			// lbl_Example
			// 
			this.lbl_Example.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.lbl_Example.Location = new System.Drawing.Point(5, 12);
			this.lbl_Example.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Example.Name = "lbl_Example";
			this.lbl_Example.Size = new System.Drawing.Size(445, 88);
			this.lbl_Example.TabIndex = 0;
			// 
			// tb_Size
			// 
			this.tb_Size.Location = new System.Drawing.Point(275, 10);
			this.tb_Size.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Size.Name = "tb_Size";
			this.tb_Size.Size = new System.Drawing.Size(40, 20);
			this.tb_Size.TabIndex = 2;
			this.tb_Size.TextChanged += new System.EventHandler(this.fontSize_TextChanged);
			// 
			// gb_Example
			// 
			this.gb_Example.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.gb_Example.Controls.Add(this.lbl_Example);
			this.gb_Example.Location = new System.Drawing.Point(5, 250);
			this.gb_Example.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Example.Name = "gb_Example";
			this.gb_Example.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Example.Size = new System.Drawing.Size(455, 105);
			this.gb_Example.TabIndex = 9;
			this.gb_Example.TabStop = false;
			// 
			// cb_Bold
			// 
			this.cb_Bold.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Bold.Location = new System.Drawing.Point(280, 40);
			this.cb_Bold.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Bold.Name = "cb_Bold";
			this.cb_Bold.Size = new System.Drawing.Size(55, 15);
			this.cb_Bold.TabIndex = 4;
			this.cb_Bold.Text = "bold";
			this.cb_Bold.UseVisualStyleBackColor = true;
			this.cb_Bold.CheckedChanged += new System.EventHandler(this.cbBold_CheckedChanged);
			// 
			// cb_Italic
			// 
			this.cb_Italic.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb_Italic.Location = new System.Drawing.Point(280, 60);
			this.cb_Italic.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Italic.Name = "cb_Italic";
			this.cb_Italic.Size = new System.Drawing.Size(55, 15);
			this.cb_Italic.TabIndex = 5;
			this.cb_Italic.Text = "italic";
			this.cb_Italic.UseVisualStyleBackColor = true;
			this.cb_Italic.CheckedChanged += new System.EventHandler(this.cbItalic_CheckedChanged);
			// 
			// tb_NetString
			// 
			this.tb_NetString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tb_NetString.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_NetString.HideSelection = false;
			this.tb_NetString.Location = new System.Drawing.Point(5, 358);
			this.tb_NetString.Margin = new System.Windows.Forms.Padding(0);
			this.tb_NetString.Name = "tb_NetString";
			this.tb_NetString.ReadOnly = true;
			this.tb_NetString.Size = new System.Drawing.Size(455, 22);
			this.tb_NetString.TabIndex = 10;
			this.tb_NetString.WordWrap = false;
			// 
			// FontF
			// 
			this.AcceptButton = this.btn_Ok;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(465, 384);
			this.Controls.Add(this.tb_NetString);
			this.Controls.Add(this.cb_Italic);
			this.Controls.Add(this.cb_Bold);
			this.Controls.Add(this.gb_Example);
			this.Controls.Add(this.btn_Ok);
			this.Controls.Add(this.btn_Apply);
			this.Controls.Add(this.tb_Size);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.list_Size);
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
			this.gb_Example.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
