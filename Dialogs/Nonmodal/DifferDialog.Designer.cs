using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class DifferDialog
	{
		#region Designer
		Label lbl_Info;
		RichTextBox rtb_Copyable;
		Panel pnl_Copyable;
		Button btn_Goto;
		Button btn_Okay;
		Button btn_Reset;

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lbl_Info = new System.Windows.Forms.Label();
			this.rtb_Copyable = new System.Windows.Forms.RichTextBox();
			this.pnl_Copyable = new System.Windows.Forms.Panel();
			this.btn_Goto = new System.Windows.Forms.Button();
			this.btn_Okay = new System.Windows.Forms.Button();
			this.btn_Reset = new System.Windows.Forms.Button();
			this.pnl_Copyable.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbl_Info
			// 
			this.lbl_Info.Dock = System.Windows.Forms.DockStyle.Top;
			this.lbl_Info.Location = new System.Drawing.Point(0, 0);
			this.lbl_Info.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Info.Name = "lbl_Info";
			this.lbl_Info.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this.lbl_Info.Size = new System.Drawing.Size(494, 30);
			this.lbl_Info.TabIndex = 0;
			this.lbl_Info.Text = "lbl_Info";
			this.lbl_Info.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// rtb_Copyable
			// 
			this.rtb_Copyable.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtb_Copyable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb_Copyable.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtb_Copyable.HideSelection = false;
			this.rtb_Copyable.Location = new System.Drawing.Point(15, 5);
			this.rtb_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.rtb_Copyable.Name = "rtb_Copyable";
			this.rtb_Copyable.ReadOnly = true;
			this.rtb_Copyable.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.rtb_Copyable.Size = new System.Drawing.Size(469, 90);
			this.rtb_Copyable.TabIndex = 0;
			this.rtb_Copyable.Text = "rtb_Copyable";
			this.rtb_Copyable.WordWrap = false;
			// 
			// pnl_Copyable
			// 
			this.pnl_Copyable.Controls.Add(this.rtb_Copyable);
			this.pnl_Copyable.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnl_Copyable.Location = new System.Drawing.Point(0, 30);
			this.pnl_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.pnl_Copyable.Name = "pnl_Copyable";
			this.pnl_Copyable.Padding = new System.Windows.Forms.Padding(15, 5, 10, 5);
			this.pnl_Copyable.Size = new System.Drawing.Size(494, 100);
			this.pnl_Copyable.TabIndex = 1;
			// 
			// btn_Goto
			// 
			this.btn_Goto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btn_Goto.Location = new System.Drawing.Point(5, 135);
			this.btn_Goto.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Goto.Name = "btn_Goto";
			this.btn_Goto.Size = new System.Drawing.Size(85, 30);
			this.btn_Goto.TabIndex = 2;
			this.btn_Goto.Text = "goto";
			this.btn_Goto.UseVisualStyleBackColor = true;
			this.btn_Goto.Visible = false;
			this.btn_Goto.Click += new System.EventHandler(this.click_btnGoto);
			// 
			// btn_Okay
			// 
			this.btn_Okay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Okay.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Okay.Location = new System.Drawing.Point(405, 135);
			this.btn_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Okay.Name = "btn_Okay";
			this.btn_Okay.Size = new System.Drawing.Size(85, 30);
			this.btn_Okay.TabIndex = 4;
			this.btn_Okay.Text = "ok";
			this.btn_Okay.UseVisualStyleBackColor = true;
			this.btn_Okay.Click += new System.EventHandler(this.click_Cancel);
			// 
			// btn_Reset
			// 
			this.btn_Reset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Reset.Location = new System.Drawing.Point(95, 135);
			this.btn_Reset.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Reset.Name = "btn_Reset";
			this.btn_Reset.Size = new System.Drawing.Size(305, 30);
			this.btn_Reset.TabIndex = 3;
			this.btn_Reset.Text = "reset";
			this.btn_Reset.UseVisualStyleBackColor = true;
			this.btn_Reset.Visible = false;
			this.btn_Reset.Click += new System.EventHandler(this.click_btnReset);
			// 
			// DifferDialog
			// 
			this.AutoScroll = true;
			this.CancelButton = this.btn_Okay;
			this.ClientSize = new System.Drawing.Size(494, 169);
			this.Controls.Add(this.btn_Okay);
			this.Controls.Add(this.btn_Reset);
			this.Controls.Add(this.btn_Goto);
			this.Controls.Add(this.pnl_Copyable);
			this.Controls.Add(this.lbl_Info);
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "DifferDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.pnl_Copyable.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
