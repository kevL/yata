using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class InputDialogColhead
	{
		#region Designer
		TextBox tb_Input;
		Button bu_Cancel;
		Button bu_Okay;

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tb_Input = new System.Windows.Forms.TextBox();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Okay = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tb_Input
			// 
			this.tb_Input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Input.Dock = System.Windows.Forms.DockStyle.Top;
			this.tb_Input.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_Input.Location = new System.Drawing.Point(0, 0);
			this.tb_Input.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Input.Name = "tb_Input";
			this.tb_Input.Size = new System.Drawing.Size(267, 22);
			this.tb_Input.TabIndex = 0;
			this.tb_Input.WordWrap = false;
			this.tb_Input.TextChanged += new System.EventHandler(this.textchanged_Input);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(6, 25);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(125, 25);
			this.bu_Cancel.TabIndex = 1;
			this.bu_Cancel.Text = "Cancel";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			// 
			// bu_Okay
			// 
			this.bu_Okay.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.bu_Okay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Okay.Location = new System.Drawing.Point(136, 25);
			this.bu_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Okay.Name = "bu_Okay";
			this.bu_Okay.Size = new System.Drawing.Size(125, 25);
			this.bu_Okay.TabIndex = 2;
			this.bu_Okay.Text = "APPLY";
			this.bu_Okay.UseVisualStyleBackColor = true;
			this.bu_Okay.Click += new System.EventHandler(this.click_Okay);
			// 
			// InputDialogColhead
			// 
			this.AcceptButton = this.bu_Okay;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(267, 54);
			this.Controls.Add(this.bu_Okay);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.tb_Input);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.MaximizeBox = false;
			this.Name = "InputDialogColhead";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
