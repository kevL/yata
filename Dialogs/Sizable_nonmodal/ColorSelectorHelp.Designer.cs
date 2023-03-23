using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ColorSelectorHelp
	{
		#region Designer
		TextBox tb_Help;

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.tb_Help = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tb_Help
			// 
			this.tb_Help.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Help.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tb_Help.Location = new System.Drawing.Point(0, 0);
			this.tb_Help.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Help.Multiline = true;
			this.tb_Help.Name = "tb_Help";
			this.tb_Help.ReadOnly = true;
			this.tb_Help.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tb_Help.Size = new System.Drawing.Size(392, 239);
			this.tb_Help.TabIndex = 0;
			// 
			// ColorSelectorHelp
			// 
			this.ClientSize = new System.Drawing.Size(392, 239);
			this.Controls.Add(this.tb_Help);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.MaximizeBox = false;
			this.Name = "ColorSelectorHelp";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - ColorSelector help";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
