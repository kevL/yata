using System.Windows.Forms;


namespace yata
{
	partial class FileWatcherDialog
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		System.ComponentModel.IContainer components = null;

		Button btn_Cancel;
		Button btn_Action;
		Label lbl_Info;
		Button btn_Close2da;
		TextBox tb_Pfe;


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
			this.btn_Action = new System.Windows.Forms.Button();
			this.lbl_Info = new System.Windows.Forms.Label();
			this.btn_Close2da = new System.Windows.Forms.Button();
			this.tb_Pfe = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Cancel.Location = new System.Drawing.Point(395, 55);
			this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(90, 30);
			this.btn_Cancel.TabIndex = 4;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.click_Cancel);
			// 
			// btn_Action
			// 
			this.btn_Action.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.btn_Action.Location = new System.Drawing.Point(10, 55);
			this.btn_Action.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Action.Name = "btn_Action";
			this.btn_Action.Size = new System.Drawing.Size(90, 30);
			this.btn_Action.TabIndex = 2;
			this.btn_Action.UseVisualStyleBackColor = true;
			this.btn_Action.Click += new System.EventHandler(this.click_Action);
			// 
			// lbl_Info
			// 
			this.lbl_Info.Location = new System.Drawing.Point(10, 10);
			this.lbl_Info.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Info.Name = "lbl_Info";
			this.lbl_Info.Size = new System.Drawing.Size(480, 15);
			this.lbl_Info.TabIndex = 0;
			// 
			// btn_Close2da
			// 
			this.btn_Close2da.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Close2da.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.btn_Close2da.Location = new System.Drawing.Point(205, 55);
			this.btn_Close2da.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Close2da.Name = "btn_Close2da";
			this.btn_Close2da.Size = new System.Drawing.Size(90, 30);
			this.btn_Close2da.TabIndex = 3;
			this.btn_Close2da.Text = "Close 2da";
			this.btn_Close2da.UseVisualStyleBackColor = true;
			this.btn_Close2da.Click += new System.EventHandler(this.click_Close2da);
			// 
			// tb_Pfe
			// 
			this.tb_Pfe.Location = new System.Drawing.Point(10, 30);
			this.tb_Pfe.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Pfe.Name = "tb_Pfe";
			this.tb_Pfe.ReadOnly = true;
			this.tb_Pfe.Size = new System.Drawing.Size(480, 20);
			this.tb_Pfe.TabIndex = 1;
			this.tb_Pfe.WordWrap = false;
			// 
			// FileWatcherDialog
			// 
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(494, 86);
			this.Controls.Add(this.tb_Pfe);
			this.Controls.Add(this.btn_Close2da);
			this.Controls.Add(this.lbl_Info);
			this.Controls.Add(this.btn_Action);
			this.Controls.Add(this.btn_Cancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FileWatcherDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " yata - File warn";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
