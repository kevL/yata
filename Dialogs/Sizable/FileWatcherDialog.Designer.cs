using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class FileWatcherDialog
	{
		Panel pnl_Top;
		Label la_Info;
		TextBox tb_Pfe;

		Panel pnl_Bot;
		Button bu_Action;
		Button bu_Close2da;
		Button bu_Cancel;


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.pnl_Top = new System.Windows.Forms.Panel();
			this.tb_Pfe = new System.Windows.Forms.TextBox();
			this.la_Info = new System.Windows.Forms.Label();
			this.pnl_Bot = new System.Windows.Forms.Panel();
			this.bu_Action = new System.Windows.Forms.Button();
			this.bu_Close2da = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.pnl_Top.SuspendLayout();
			this.pnl_Bot.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnl_Top
			// 
			this.pnl_Top.Controls.Add(this.tb_Pfe);
			this.pnl_Top.Controls.Add(this.la_Info);
			this.pnl_Top.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnl_Top.Location = new System.Drawing.Point(0, 0);
			this.pnl_Top.Margin = new System.Windows.Forms.Padding(0);
			this.pnl_Top.Name = "pnl_Top";
			this.pnl_Top.Size = new System.Drawing.Size(457, 45);
			this.pnl_Top.TabIndex = 0;
			// 
			// tb_Pfe
			// 
			this.tb_Pfe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Pfe.Dock = System.Windows.Forms.DockStyle.Top;
			this.tb_Pfe.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_Pfe.Location = new System.Drawing.Point(0, 22);
			this.tb_Pfe.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Pfe.Name = "tb_Pfe";
			this.tb_Pfe.ReadOnly = true;
			this.tb_Pfe.Size = new System.Drawing.Size(457, 22);
			this.tb_Pfe.TabIndex = 1;
			this.tb_Pfe.WordWrap = false;
			// 
			// la_Info
			// 
			this.la_Info.Dock = System.Windows.Forms.DockStyle.Top;
			this.la_Info.ForeColor = System.Drawing.Color.Firebrick;
			this.la_Info.Location = new System.Drawing.Point(0, 0);
			this.la_Info.Margin = new System.Windows.Forms.Padding(0);
			this.la_Info.Name = "la_Info";
			this.la_Info.Size = new System.Drawing.Size(457, 22);
			this.la_Info.TabIndex = 0;
			this.la_Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnl_Bot
			// 
			this.pnl_Bot.Controls.Add(this.bu_Action);
			this.pnl_Bot.Controls.Add(this.bu_Close2da);
			this.pnl_Bot.Controls.Add(this.bu_Cancel);
			this.pnl_Bot.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnl_Bot.Location = new System.Drawing.Point(0, 45);
			this.pnl_Bot.Margin = new System.Windows.Forms.Padding(0);
			this.pnl_Bot.Name = "pnl_Bot";
			this.pnl_Bot.Size = new System.Drawing.Size(457, 33);
			this.pnl_Bot.TabIndex = 1;
			// 
			// bu_Action
			// 
			this.bu_Action.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.bu_Action.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.bu_Action.Enabled = false;
			this.bu_Action.Location = new System.Drawing.Point(15, 4);
			this.bu_Action.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Action.Name = "bu_Action";
			this.bu_Action.Size = new System.Drawing.Size(100, 25);
			this.bu_Action.TabIndex = 0;
			this.bu_Action.UseVisualStyleBackColor = true;
			// 
			// bu_Close2da
			// 
			this.bu_Close2da.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Close2da.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.bu_Close2da.Enabled = false;
			this.bu_Close2da.Location = new System.Drawing.Point(120, 4);
			this.bu_Close2da.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Close2da.Name = "bu_Close2da";
			this.bu_Close2da.Size = new System.Drawing.Size(215, 25);
			this.bu_Close2da.TabIndex = 1;
			this.bu_Close2da.Text = "Close 2da";
			this.bu_Close2da.UseVisualStyleBackColor = true;
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Enabled = false;
			this.bu_Cancel.Location = new System.Drawing.Point(340, 4);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(100, 25);
			this.bu_Cancel.TabIndex = 2;
			this.bu_Cancel.Text = "Cancel";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			// 
			// FileWatcherDialog
			// 
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(457, 78);
			this.Controls.Add(this.pnl_Bot);
			this.Controls.Add(this.pnl_Top);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FileWatcherDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - File warn";
			this.pnl_Top.ResumeLayout(false);
			this.pnl_Top.PerformLayout();
			this.pnl_Bot.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
