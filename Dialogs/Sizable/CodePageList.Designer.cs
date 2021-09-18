using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class CodePageList
	{
		#region Designer
		TextBox tb_Codepages;

		void InitializeComponent()
		{
			this.tb_Codepages = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tb_Codepages
			// 
			this.tb_Codepages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tb_Codepages.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_Codepages.Location = new System.Drawing.Point(0, 0);
			this.tb_Codepages.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Codepages.Multiline = true;
			this.tb_Codepages.Name = "tb_Codepages";
			this.tb_Codepages.ReadOnly = true;
			this.tb_Codepages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tb_Codepages.Size = new System.Drawing.Size(312, 429);
			this.tb_Codepages.TabIndex = 0;
			this.tb_Codepages.WordWrap = false;
			// 
			// CodePageList
			// 
			this.ClientSize = new System.Drawing.Size(312, 429);
			this.Controls.Add(this.tb_Codepages);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.Name = "CodePageList";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - .net Codepages";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
