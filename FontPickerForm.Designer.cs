namespace yata
{
	partial class FontPickerForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ListBox list_Font;
		private System.Windows.Forms.Button btn_Ok;
		private System.Windows.Forms.Button btn_Apply;
		private System.Windows.Forms.Button btn_Cancel;
		private System.Windows.Forms.Label lbl_Font;
		private System.Windows.Forms.Label lbl_Style;
		private System.Windows.Forms.Label lbl_Size;
		private System.Windows.Forms.ListBox list_Style;
		private System.Windows.Forms.ListBox list_Size;
		private System.Windows.Forms.Label lbl_Example;
		private System.Windows.Forms.TextBox tb_Size;

		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			DisposeFonts(); // kL_add.

			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.list_Font = new System.Windows.Forms.ListBox();
			this.btn_Ok = new System.Windows.Forms.Button();
			this.btn_Apply = new System.Windows.Forms.Button();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.lbl_Font = new System.Windows.Forms.Label();
			this.lbl_Style = new System.Windows.Forms.Label();
			this.lbl_Size = new System.Windows.Forms.Label();
			this.list_Style = new System.Windows.Forms.ListBox();
			this.list_Size = new System.Windows.Forms.ListBox();
			this.lbl_Example = new System.Windows.Forms.Label();
			this.tb_Size = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// list_Font
			// 
			this.list_Font.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.list_Font.FormattingEnabled = true;
			this.list_Font.ItemHeight = 20;
			this.list_Font.Location = new System.Drawing.Point(5, 20);
			this.list_Font.Name = "list_Font";
			this.list_Font.Size = new System.Drawing.Size(225, 264);
			this.list_Font.TabIndex = 6;
			this.list_Font.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.fontList_DrawItem);
			this.list_Font.SelectedIndexChanged += new System.EventHandler(this.fontList_SelectedIndexChanged);
			// 
			// btn_Ok
			// 
			this.btn_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Ok.Location = new System.Drawing.Point(335, 25);
			this.btn_Ok.Name = "btn_Ok";
			this.btn_Ok.Size = new System.Drawing.Size(208, 35);
			this.btn_Ok.TabIndex = 0;
			this.btn_Ok.Text = "ok";
			this.btn_Ok.UseVisualStyleBackColor = true;
			this.btn_Ok.Click += new System.EventHandler(this.btnOk_click);
			// 
			// btn_Apply
			// 
			this.btn_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Apply.Location = new System.Drawing.Point(335, 65);
			this.btn_Apply.Name = "btn_Apply";
			this.btn_Apply.Size = new System.Drawing.Size(206, 35);
			this.btn_Apply.TabIndex = 1;
			this.btn_Apply.Text = "apply";
			this.btn_Apply.UseVisualStyleBackColor = true;
			this.btn_Apply.Click += new System.EventHandler(this.btnApply_click);
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Cancel.Location = new System.Drawing.Point(335, 105);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(208, 35);
			this.btn_Cancel.TabIndex = 2;
			this.btn_Cancel.Text = "cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.btnCancel_click);
			// 
			// lbl_Font
			// 
			this.lbl_Font.Location = new System.Drawing.Point(10, 5);
			this.lbl_Font.Name = "lbl_Font";
			this.lbl_Font.Size = new System.Drawing.Size(220, 15);
			this.lbl_Font.TabIndex = 3;
			this.lbl_Font.Text = "FONT";
			// 
			// lbl_Style
			// 
			this.lbl_Style.Location = new System.Drawing.Point(245, 10);
			this.lbl_Style.Name = "lbl_Style";
			this.lbl_Style.Size = new System.Drawing.Size(80, 15);
			this.lbl_Style.TabIndex = 4;
			this.lbl_Style.Text = "Style";
			// 
			// lbl_Size
			// 
			this.lbl_Size.Location = new System.Drawing.Point(290, 85);
			this.lbl_Size.Name = "lbl_Size";
			this.lbl_Size.Size = new System.Drawing.Size(35, 15);
			this.lbl_Size.TabIndex = 5;
			this.lbl_Size.Text = "pt";
			// 
			// list_Style
			// 
			this.list_Style.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.list_Style.FormattingEnabled = true;
			this.list_Style.ItemHeight = 15;
			this.list_Style.Location = new System.Drawing.Point(240, 25);
			this.list_Style.Name = "list_Style";
			this.list_Style.Size = new System.Drawing.Size(85, 49);
			this.list_Style.TabIndex = 7;
			this.list_Style.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.styleList_DrawItem);
			this.list_Style.SelectedIndexChanged += new System.EventHandler(this.fontStyle_SelectedIndexChanged);
			// 
			// list_Size
			// 
			this.list_Size.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.list_Size.FormattingEnabled = true;
			this.list_Size.ItemHeight = 15;
			this.list_Size.Location = new System.Drawing.Point(240, 85);
			this.list_Size.Name = "list_Size";
			this.list_Size.Size = new System.Drawing.Size(40, 199);
			this.list_Size.TabIndex = 8;
			this.list_Size.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.sizeList_DrawItem);
			this.list_Size.SelectedIndexChanged += new System.EventHandler(this.fontSize_SelectedIndexChanged);
			// 
			// lbl_Example
			// 
			this.lbl_Example.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.lbl_Example.Location = new System.Drawing.Point(0, 290);
			this.lbl_Example.Name = "lbl_Example";
			this.lbl_Example.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.lbl_Example.Size = new System.Drawing.Size(553, 111);
			this.lbl_Example.TabIndex = 10;
			// 
			// tb_Size
			// 
			this.tb_Size.Location = new System.Drawing.Point(285, 105);
			this.tb_Size.Name = "tb_Size";
			this.tb_Size.Size = new System.Drawing.Size(40, 20);
			this.tb_Size.TabIndex = 11;
			this.tb_Size.TextChanged += new System.EventHandler(this.fontSize_TextChanged);
			// 
			// FontPickerForm
			// 
			this.AcceptButton = this.btn_Ok;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(553, 401);
			this.Controls.Add(this.btn_Ok);
			this.Controls.Add(this.btn_Apply);
			this.Controls.Add(this.tb_Size);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.lbl_Example);
			this.Controls.Add(this.list_Size);
			this.Controls.Add(this.list_Style);
			this.Controls.Add(this.list_Font);
			this.Controls.Add(this.lbl_Size);
			this.Controls.Add(this.lbl_Style);
			this.Controls.Add(this.lbl_Font);
			this.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "FontPickerForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Choose Font";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
