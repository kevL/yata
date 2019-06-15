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
		Panel panel1;
		Panel panel2;


		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			Font.Dispose();
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Cancel.Location = new System.Drawing.Point(225, 5);
			this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(100, 30);
			this.btn_Cancel.TabIndex = 2;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			// 
			// btn_Action
			// 
			this.btn_Action.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left)));
			this.btn_Action.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.btn_Action.Location = new System.Drawing.Point(15, 5);
			this.btn_Action.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Action.Name = "btn_Action";
			this.btn_Action.Size = new System.Drawing.Size(100, 30);
			this.btn_Action.TabIndex = 0;
			this.btn_Action.UseVisualStyleBackColor = true;
			// 
			// lbl_Info
			// 
			this.lbl_Info.Dock = System.Windows.Forms.DockStyle.Top;
			this.lbl_Info.ForeColor = System.Drawing.Color.Firebrick;
			this.lbl_Info.Location = new System.Drawing.Point(0, 0);
			this.lbl_Info.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Info.Name = "lbl_Info";
			this.lbl_Info.Size = new System.Drawing.Size(342, 20);
			this.lbl_Info.TabIndex = 0;
			this.lbl_Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btn_Close2da
			// 
			this.btn_Close2da.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Close2da.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.btn_Close2da.Location = new System.Drawing.Point(120, 5);
			this.btn_Close2da.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Close2da.Name = "btn_Close2da";
			this.btn_Close2da.Size = new System.Drawing.Size(100, 30);
			this.btn_Close2da.TabIndex = 1;
			this.btn_Close2da.Text = "Close 2da";
			this.btn_Close2da.UseVisualStyleBackColor = true;
			// 
			// tb_Pfe
			// 
			this.tb_Pfe.Dock = System.Windows.Forms.DockStyle.Top;
			this.tb_Pfe.Location = new System.Drawing.Point(0, 20);
			this.tb_Pfe.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Pfe.Name = "tb_Pfe";
			this.tb_Pfe.ReadOnly = true;
			this.tb_Pfe.Size = new System.Drawing.Size(342, 20);
			this.tb_Pfe.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btn_Close2da);
			this.panel1.Controls.Add(this.btn_Cancel);
			this.panel1.Controls.Add(this.btn_Action);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 40);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(342, 39);
			this.panel1.TabIndex = 1;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.tb_Pfe);
			this.panel2.Controls.Add(this.lbl_Info);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Margin = new System.Windows.Forms.Padding(0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(342, 40);
			this.panel2.TabIndex = 0;
			// 
			// FileWatcherDialog
			// 
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(342, 79);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(350, 105);
			this.Name = "FileWatcherDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " yata - File warn";
			this.TopMost = true;
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}
	}
}
