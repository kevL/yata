using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class DifferDialog
	{
		#region Designer
		Label la_Info;
		RichTextBox rt_Copyable;
		Panel pa_Copyable;
		Button bu_Goto;
		Button bu_Okay;
		Button bu_Reset;

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.la_Info = new System.Windows.Forms.Label();
			this.rt_Copyable = new System.Windows.Forms.RichTextBox();
			this.pa_Copyable = new System.Windows.Forms.Panel();
			this.bu_Goto = new System.Windows.Forms.Button();
			this.bu_Okay = new System.Windows.Forms.Button();
			this.bu_Reset = new System.Windows.Forms.Button();
			this.pa_Copyable.SuspendLayout();
			this.SuspendLayout();
			// 
			// la_Info
			// 
			this.la_Info.Dock = System.Windows.Forms.DockStyle.Top;
			this.la_Info.Location = new System.Drawing.Point(0, 0);
			this.la_Info.Margin = new System.Windows.Forms.Padding(0);
			this.la_Info.Name = "la_Info";
			this.la_Info.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this.la_Info.Size = new System.Drawing.Size(494, 30);
			this.la_Info.TabIndex = 0;
			this.la_Info.Text = "la_Info";
			this.la_Info.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// rt_Copyable
			// 
			this.rt_Copyable.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rt_Copyable.DetectUrls = false;
			this.rt_Copyable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rt_Copyable.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rt_Copyable.HideSelection = false;
			this.rt_Copyable.Location = new System.Drawing.Point(15, 5);
			this.rt_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.rt_Copyable.Name = "rt_Copyable";
			this.rt_Copyable.ReadOnly = true;
			this.rt_Copyable.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.rt_Copyable.Size = new System.Drawing.Size(469, 90);
			this.rt_Copyable.TabIndex = 0;
			this.rt_Copyable.Text = "rt_Copyable";
			this.rt_Copyable.WordWrap = false;
			// 
			// pa_Copyable
			// 
			this.pa_Copyable.Controls.Add(this.rt_Copyable);
			this.pa_Copyable.Dock = System.Windows.Forms.DockStyle.Top;
			this.pa_Copyable.Location = new System.Drawing.Point(0, 30);
			this.pa_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Copyable.Name = "pa_Copyable";
			this.pa_Copyable.Padding = new System.Windows.Forms.Padding(15, 5, 10, 5);
			this.pa_Copyable.Size = new System.Drawing.Size(494, 100);
			this.pa_Copyable.TabIndex = 1;
			// 
			// bu_Goto
			// 
			this.bu_Goto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Goto.Location = new System.Drawing.Point(5, 135);
			this.bu_Goto.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Goto.Name = "bu_Goto";
			this.bu_Goto.Size = new System.Drawing.Size(85, 30);
			this.bu_Goto.TabIndex = 2;
			this.bu_Goto.Text = "goto";
			this.bu_Goto.UseVisualStyleBackColor = true;
			this.bu_Goto.Visible = false;
			this.bu_Goto.Click += new System.EventHandler(this.click_btnGoto);
			// 
			// bu_Okay
			// 
			this.bu_Okay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Okay.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Okay.Location = new System.Drawing.Point(405, 135);
			this.bu_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Okay.Name = "bu_Okay";
			this.bu_Okay.Size = new System.Drawing.Size(85, 30);
			this.bu_Okay.TabIndex = 4;
			this.bu_Okay.Text = "ok";
			this.bu_Okay.UseVisualStyleBackColor = true;
			this.bu_Okay.Click += new System.EventHandler(this.click_Cancel);
			// 
			// bu_Reset
			// 
			this.bu_Reset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Reset.Location = new System.Drawing.Point(95, 135);
			this.bu_Reset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Reset.Name = "bu_Reset";
			this.bu_Reset.Size = new System.Drawing.Size(305, 30);
			this.bu_Reset.TabIndex = 3;
			this.bu_Reset.Text = "reset";
			this.bu_Reset.UseVisualStyleBackColor = true;
			this.bu_Reset.Visible = false;
			this.bu_Reset.Click += new System.EventHandler(this.click_btnReset);
			// 
			// DifferDialog
			// 
			this.AutoScroll = true;
			this.CancelButton = this.bu_Okay;
			this.ClientSize = new System.Drawing.Size(494, 169);
			this.Controls.Add(this.bu_Okay);
			this.Controls.Add(this.bu_Reset);
			this.Controls.Add(this.bu_Goto);
			this.Controls.Add(this.pa_Copyable);
			this.Controls.Add(this.la_Info);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "DifferDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.pa_Copyable.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
