﻿using System.Windows.Forms;


namespace yata
{
	sealed partial class RowCreatorDialog
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		System.ComponentModel.IContainer components = null;

		Button btn_Cancel;
		Button btn_Okay;
		RadioButton rb_StartAdd;
		RadioButton rb_StartInsert;
		GroupBox gb_Start;
		GroupBox gb_Stop;
		RadioButton rb_StopCount;
		RadioButton rb_StopFinish;
		TextBox tb_StartInsert;
		TextBox tb_StartAdd;
		TextBox tb_StopFinish;
		TextBox tb_StopCount;
		Label la_StartInsert;
		Label la_StartAdd;
		Label la_StopFinish;
		Label la_StopCount;


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
			this.rb_StartAdd = new System.Windows.Forms.RadioButton();
			this.rb_StartInsert = new System.Windows.Forms.RadioButton();
			this.gb_Start = new System.Windows.Forms.GroupBox();
			this.la_StartInsert = new System.Windows.Forms.Label();
			this.la_StartAdd = new System.Windows.Forms.Label();
			this.tb_StartInsert = new System.Windows.Forms.TextBox();
			this.tb_StartAdd = new System.Windows.Forms.TextBox();
			this.gb_Stop = new System.Windows.Forms.GroupBox();
			this.la_StopFinish = new System.Windows.Forms.Label();
			this.tb_StopFinish = new System.Windows.Forms.TextBox();
			this.rb_StopFinish = new System.Windows.Forms.RadioButton();
			this.la_StopCount = new System.Windows.Forms.Label();
			this.tb_StopCount = new System.Windows.Forms.TextBox();
			this.rb_StopCount = new System.Windows.Forms.RadioButton();
			this.gb_Start.SuspendLayout();
			this.gb_Stop.SuspendLayout();
			this.SuspendLayout();
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Cancel.Location = new System.Drawing.Point(10, 125);
			this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(110, 33);
			this.btn_Cancel.TabIndex = 2;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			// 
			// btn_Okay
			// 
			this.btn_Okay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Okay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_Okay.Location = new System.Drawing.Point(125, 125);
			this.btn_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Okay.Name = "btn_Okay";
			this.btn_Okay.Size = new System.Drawing.Size(110, 33);
			this.btn_Okay.TabIndex = 3;
			this.btn_Okay.Text = "Ok";
			this.btn_Okay.UseVisualStyleBackColor = true;
			this.btn_Okay.Click += new System.EventHandler(this.click_Ok);
			// 
			// rb_StartAdd
			// 
			this.rb_StartAdd.Location = new System.Drawing.Point(10, 15);
			this.rb_StartAdd.Margin = new System.Windows.Forms.Padding(0);
			this.rb_StartAdd.Name = "rb_StartAdd";
			this.rb_StartAdd.Size = new System.Drawing.Size(110, 20);
			this.rb_StartAdd.TabIndex = 0;
			this.rb_StartAdd.Text = "Add row(s)";
			this.rb_StartAdd.UseVisualStyleBackColor = true;
			this.rb_StartAdd.CheckedChanged += new System.EventHandler(this.checkchanged);
			// 
			// rb_StartInsert
			// 
			this.rb_StartInsert.Location = new System.Drawing.Point(10, 35);
			this.rb_StartInsert.Margin = new System.Windows.Forms.Padding(0);
			this.rb_StartInsert.Name = "rb_StartInsert";
			this.rb_StartInsert.Size = new System.Drawing.Size(110, 20);
			this.rb_StartInsert.TabIndex = 3;
			this.rb_StartInsert.Text = "Insert row(s)";
			this.rb_StartInsert.UseVisualStyleBackColor = true;
			this.rb_StartInsert.CheckedChanged += new System.EventHandler(this.checkchanged);
			// 
			// gb_Start
			// 
			this.gb_Start.Controls.Add(this.la_StartInsert);
			this.gb_Start.Controls.Add(this.la_StartAdd);
			this.gb_Start.Controls.Add(this.tb_StartInsert);
			this.gb_Start.Controls.Add(this.tb_StartAdd);
			this.gb_Start.Controls.Add(this.rb_StartAdd);
			this.gb_Start.Controls.Add(this.rb_StartInsert);
			this.gb_Start.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_Start.Location = new System.Drawing.Point(0, 0);
			this.gb_Start.Name = "gb_Start";
			this.gb_Start.Size = new System.Drawing.Size(244, 60);
			this.gb_Start.TabIndex = 0;
			this.gb_Start.TabStop = false;
			this.gb_Start.Text = " START ";
			// 
			// la_StartInsert
			// 
			this.la_StartInsert.Location = new System.Drawing.Point(190, 35);
			this.la_StartInsert.Margin = new System.Windows.Forms.Padding(0);
			this.la_StartInsert.Name = "la_StartInsert";
			this.la_StartInsert.Size = new System.Drawing.Size(50, 20);
			this.la_StartInsert.TabIndex = 5;
			this.la_StartInsert.Text = "id";
			this.la_StartInsert.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_StartAdd
			// 
			this.la_StartAdd.Location = new System.Drawing.Point(190, 15);
			this.la_StartAdd.Margin = new System.Windows.Forms.Padding(0);
			this.la_StartAdd.Name = "la_StartAdd";
			this.la_StartAdd.Size = new System.Drawing.Size(50, 20);
			this.la_StartAdd.TabIndex = 2;
			this.la_StartAdd.Text = "id";
			this.la_StartAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tb_StartInsert
			// 
			this.tb_StartInsert.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_StartInsert.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tb_StartInsert.Enabled = false;
			this.tb_StartInsert.Location = new System.Drawing.Point(125, 35);
			this.tb_StartInsert.Margin = new System.Windows.Forms.Padding(0);
			this.tb_StartInsert.Name = "tb_StartInsert";
			this.tb_StartInsert.Size = new System.Drawing.Size(60, 20);
			this.tb_StartInsert.TabIndex = 4;
			this.tb_StartInsert.WordWrap = false;
			this.tb_StartInsert.TextChanged += new System.EventHandler(this.textchanged);
			// 
			// tb_StartAdd
			// 
			this.tb_StartAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_StartAdd.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tb_StartAdd.Enabled = false;
			this.tb_StartAdd.Location = new System.Drawing.Point(125, 15);
			this.tb_StartAdd.Margin = new System.Windows.Forms.Padding(0);
			this.tb_StartAdd.Name = "tb_StartAdd";
			this.tb_StartAdd.ReadOnly = true;
			this.tb_StartAdd.Size = new System.Drawing.Size(60, 20);
			this.tb_StartAdd.TabIndex = 1;
			this.tb_StartAdd.WordWrap = false;
			// 
			// gb_Stop
			// 
			this.gb_Stop.Controls.Add(this.la_StopFinish);
			this.gb_Stop.Controls.Add(this.tb_StopFinish);
			this.gb_Stop.Controls.Add(this.rb_StopFinish);
			this.gb_Stop.Controls.Add(this.la_StopCount);
			this.gb_Stop.Controls.Add(this.tb_StopCount);
			this.gb_Stop.Controls.Add(this.rb_StopCount);
			this.gb_Stop.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_Stop.Location = new System.Drawing.Point(0, 60);
			this.gb_Stop.Name = "gb_Stop";
			this.gb_Stop.Size = new System.Drawing.Size(244, 60);
			this.gb_Stop.TabIndex = 1;
			this.gb_Stop.TabStop = false;
			this.gb_Stop.Text = " STOP ";
			// 
			// la_StopFinish
			// 
			this.la_StopFinish.Location = new System.Drawing.Point(190, 15);
			this.la_StopFinish.Margin = new System.Windows.Forms.Padding(0);
			this.la_StopFinish.Name = "la_StopFinish";
			this.la_StopFinish.Size = new System.Drawing.Size(50, 20);
			this.la_StopFinish.TabIndex = 2;
			this.la_StopFinish.Text = "id";
			this.la_StopFinish.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tb_StopFinish
			// 
			this.tb_StopFinish.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_StopFinish.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tb_StopFinish.Enabled = false;
			this.tb_StopFinish.Location = new System.Drawing.Point(125, 15);
			this.tb_StopFinish.Margin = new System.Windows.Forms.Padding(0);
			this.tb_StopFinish.Name = "tb_StopFinish";
			this.tb_StopFinish.Size = new System.Drawing.Size(60, 20);
			this.tb_StopFinish.TabIndex = 1;
			this.tb_StopFinish.WordWrap = false;
			this.tb_StopFinish.TextChanged += new System.EventHandler(this.textchanged);
			// 
			// rb_StopFinish
			// 
			this.rb_StopFinish.Location = new System.Drawing.Point(10, 15);
			this.rb_StopFinish.Margin = new System.Windows.Forms.Padding(0);
			this.rb_StopFinish.Name = "rb_StopFinish";
			this.rb_StopFinish.Size = new System.Drawing.Size(110, 20);
			this.rb_StopFinish.TabIndex = 0;
			this.rb_StopFinish.Text = "row Finish";
			this.rb_StopFinish.UseVisualStyleBackColor = true;
			// 
			// la_StopCount
			// 
			this.la_StopCount.Location = new System.Drawing.Point(190, 35);
			this.la_StopCount.Margin = new System.Windows.Forms.Padding(0);
			this.la_StopCount.Name = "la_StopCount";
			this.la_StopCount.Size = new System.Drawing.Size(50, 20);
			this.la_StopCount.TabIndex = 5;
			this.la_StopCount.Text = "row(s)";
			this.la_StopCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tb_StopCount
			// 
			this.tb_StopCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_StopCount.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tb_StopCount.Enabled = false;
			this.tb_StopCount.Location = new System.Drawing.Point(125, 35);
			this.tb_StopCount.Margin = new System.Windows.Forms.Padding(0);
			this.tb_StopCount.Name = "tb_StopCount";
			this.tb_StopCount.Size = new System.Drawing.Size(60, 20);
			this.tb_StopCount.TabIndex = 4;
			this.tb_StopCount.WordWrap = false;
			this.tb_StopCount.TextChanged += new System.EventHandler(this.textchanged);
			// 
			// rb_StopCount
			// 
			this.rb_StopCount.Location = new System.Drawing.Point(10, 35);
			this.rb_StopCount.Margin = new System.Windows.Forms.Padding(0);
			this.rb_StopCount.Name = "rb_StopCount";
			this.rb_StopCount.Size = new System.Drawing.Size(110, 20);
			this.rb_StopCount.TabIndex = 3;
			this.rb_StopCount.Text = "row Count";
			this.rb_StopCount.UseVisualStyleBackColor = true;
			this.rb_StopCount.CheckedChanged += new System.EventHandler(this.checkchanged);
			// 
			// RowCreatorDialog
			// 
			this.AcceptButton = this.btn_Okay;
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(244, 161);
			this.Controls.Add(this.gb_Stop);
			this.Controls.Add(this.gb_Start);
			this.Controls.Add(this.btn_Okay);
			this.Controls.Add(this.btn_Cancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RowCreatorDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " yata - Create rows";
			this.gb_Start.ResumeLayout(false);
			this.gb_Start.PerformLayout();
			this.gb_Stop.ResumeLayout(false);
			this.gb_Stop.PerformLayout();
			this.ResumeLayout(false);

		}
	}
}
