namespace yata
{
	partial class RowCreatorForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button btn_Cancel;
		private System.Windows.Forms.Button btn_Okay;
		private System.Windows.Forms.Label lbl_Start;
		private System.Windows.Forms.Label lbl_Stop;
		private System.Windows.Forms.TextBox tb_Start;
		private System.Windows.Forms.TextBox tb_Stop;
		private System.Windows.Forms.TextBox tb_Length;
		private System.Windows.Forms.Label lbl_Length;
		private System.Windows.Forms.RadioButton rb_Stop;
		private System.Windows.Forms.RadioButton rb_Length;
		private System.Windows.Forms.Label lbl_Pad;
		private System.Windows.Forms.CheckBox cb_Pad;
		private System.Windows.Forms.TextBox tb_Pad;
		private System.Windows.Forms.GroupBox gb_Insert;

		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed</param>
		protected override void Dispose(bool disposing)
		{
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
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.btn_Okay = new System.Windows.Forms.Button();
			this.lbl_Start = new System.Windows.Forms.Label();
			this.lbl_Stop = new System.Windows.Forms.Label();
			this.tb_Start = new System.Windows.Forms.TextBox();
			this.tb_Stop = new System.Windows.Forms.TextBox();
			this.tb_Length = new System.Windows.Forms.TextBox();
			this.lbl_Length = new System.Windows.Forms.Label();
			this.rb_Stop = new System.Windows.Forms.RadioButton();
			this.rb_Length = new System.Windows.Forms.RadioButton();
			this.lbl_Pad = new System.Windows.Forms.Label();
			this.cb_Pad = new System.Windows.Forms.CheckBox();
			this.tb_Pad = new System.Windows.Forms.TextBox();
			this.gb_Insert = new System.Windows.Forms.GroupBox();
			this.gb_Insert.SuspendLayout();
			this.SuspendLayout();
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Cancel.Location = new System.Drawing.Point(10, 100);
			this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(110, 35);
			this.btn_Cancel.TabIndex = 4;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.click_Cancel);
			// 
			// btn_Okay
			// 
			this.btn_Okay.Location = new System.Drawing.Point(125, 100);
			this.btn_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Okay.Name = "btn_Okay";
			this.btn_Okay.Size = new System.Drawing.Size(110, 35);
			this.btn_Okay.TabIndex = 5;
			this.btn_Okay.Text = "Ok";
			this.btn_Okay.UseVisualStyleBackColor = true;
			this.btn_Okay.Click += new System.EventHandler(this.click_Ok);
			// 
			// lbl_Start
			// 
			this.lbl_Start.Location = new System.Drawing.Point(5, 20);
			this.lbl_Start.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Start.Name = "lbl_Start";
			this.lbl_Start.Size = new System.Drawing.Size(50, 15);
			this.lbl_Start.TabIndex = 0;
			this.lbl_Start.Text = "START";
			// 
			// lbl_Stop
			// 
			this.lbl_Stop.Location = new System.Drawing.Point(5, 40);
			this.lbl_Stop.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Stop.Name = "lbl_Stop";
			this.lbl_Stop.Size = new System.Drawing.Size(50, 15);
			this.lbl_Stop.TabIndex = 4;
			this.lbl_Stop.Text = "STOP";
			// 
			// tb_Start
			// 
			this.tb_Start.Location = new System.Drawing.Point(55, 17);
			this.tb_Start.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Start.Name = "tb_Start";
			this.tb_Start.Size = new System.Drawing.Size(55, 20);
			this.tb_Start.TabIndex = 1;
			// 
			// tb_Stop
			// 
			this.tb_Stop.Enabled = false;
			this.tb_Stop.Location = new System.Drawing.Point(55, 37);
			this.tb_Stop.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Stop.Name = "tb_Stop";
			this.tb_Stop.Size = new System.Drawing.Size(55, 20);
			this.tb_Stop.TabIndex = 5;
			// 
			// tb_Length
			// 
			this.tb_Length.Location = new System.Drawing.Point(175, 17);
			this.tb_Length.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Length.Name = "tb_Length";
			this.tb_Length.Size = new System.Drawing.Size(55, 20);
			this.tb_Length.TabIndex = 3;
			// 
			// lbl_Length
			// 
			this.lbl_Length.Location = new System.Drawing.Point(115, 20);
			this.lbl_Length.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Length.Name = "lbl_Length";
			this.lbl_Length.Size = new System.Drawing.Size(60, 15);
			this.lbl_Length.TabIndex = 2;
			this.lbl_Length.Text = "LENGTH";
			// 
			// rb_Stop
			// 
			this.rb_Stop.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rb_Stop.Location = new System.Drawing.Point(120, 40);
			this.rb_Stop.Margin = new System.Windows.Forms.Padding(0);
			this.rb_Stop.Name = "rb_Stop";
			this.rb_Stop.Size = new System.Drawing.Size(15, 15);
			this.rb_Stop.TabIndex = 6;
			this.rb_Stop.UseVisualStyleBackColor = true;
			this.rb_Stop.CheckedChanged += new System.EventHandler(this.checkchanged_Insert);
			// 
			// rb_Length
			// 
			this.rb_Length.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rb_Length.Checked = true;
			this.rb_Length.Location = new System.Drawing.Point(140, 40);
			this.rb_Length.Margin = new System.Windows.Forms.Padding(0);
			this.rb_Length.Name = "rb_Length";
			this.rb_Length.Size = new System.Drawing.Size(15, 15);
			this.rb_Length.TabIndex = 7;
			this.rb_Length.TabStop = true;
			this.rb_Length.UseVisualStyleBackColor = true;
			// 
			// lbl_Pad
			// 
			this.lbl_Pad.Location = new System.Drawing.Point(30, 10);
			this.lbl_Pad.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Pad.Name = "lbl_Pad";
			this.lbl_Pad.Size = new System.Drawing.Size(70, 15);
			this.lbl_Pad.TabIndex = 1;
			this.lbl_Pad.Text = "Pad end to";
			// 
			// cb_Pad
			// 
			this.cb_Pad.CheckAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cb_Pad.Checked = true;
			this.cb_Pad.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cb_Pad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cb_Pad.Location = new System.Drawing.Point(15, 10);
			this.cb_Pad.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Pad.Name = "cb_Pad";
			this.cb_Pad.Size = new System.Drawing.Size(15, 15);
			this.cb_Pad.TabIndex = 0;
			this.cb_Pad.UseVisualStyleBackColor = true;
			this.cb_Pad.CheckedChanged += new System.EventHandler(this.checkchanged_Pad);
			// 
			// tb_Pad
			// 
			this.tb_Pad.Location = new System.Drawing.Point(100, 7);
			this.tb_Pad.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Pad.Name = "tb_Pad";
			this.tb_Pad.Size = new System.Drawing.Size(55, 20);
			this.tb_Pad.TabIndex = 2;
			// 
			// gb_Insert
			// 
			this.gb_Insert.Controls.Add(this.lbl_Start);
			this.gb_Insert.Controls.Add(this.lbl_Stop);
			this.gb_Insert.Controls.Add(this.tb_Start);
			this.gb_Insert.Controls.Add(this.tb_Stop);
			this.gb_Insert.Controls.Add(this.rb_Length);
			this.gb_Insert.Controls.Add(this.lbl_Length);
			this.gb_Insert.Controls.Add(this.rb_Stop);
			this.gb_Insert.Controls.Add(this.tb_Length);
			this.gb_Insert.Enabled = false;
			this.gb_Insert.Location = new System.Drawing.Point(5, 30);
			this.gb_Insert.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Insert.Name = "gb_Insert";
			this.gb_Insert.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Insert.Size = new System.Drawing.Size(235, 65);
			this.gb_Insert.TabIndex = 3;
			this.gb_Insert.TabStop = false;
			this.gb_Insert.Text = " or Insert (inclusive) ";
			// 
			// RowCreatorForm
			// 
			this.AcceptButton = this.btn_Okay;
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(244, 141);
			this.Controls.Add(this.gb_Insert);
			this.Controls.Add(this.tb_Pad);
			this.Controls.Add(this.cb_Pad);
			this.Controls.Add(this.lbl_Pad);
			this.Controls.Add(this.btn_Okay);
			this.Controls.Add(this.btn_Cancel);
			this.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RowCreatorForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Create rows";
			this.gb_Insert.ResumeLayout(false);
			this.gb_Insert.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
