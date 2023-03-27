using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ColorSelectorHelp
	{
		#region Designer
		Panel pa_Help;
		RichTextBox rt_Help;

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.rt_Help = new System.Windows.Forms.RichTextBox();
			this.pa_Help = new System.Windows.Forms.Panel();
			this.pa_Help.SuspendLayout();
			this.SuspendLayout();
			// 
			// rt_Help
			// 
			this.rt_Help.BackColor = System.Drawing.SystemColors.Control;
			this.rt_Help.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rt_Help.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rt_Help.Location = new System.Drawing.Point(15, 6);
			this.rt_Help.Margin = new System.Windows.Forms.Padding(0);
			this.rt_Help.Name = "rt_Help";
			this.rt_Help.ReadOnly = true;
			this.rt_Help.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.rt_Help.Size = new System.Drawing.Size(417, 533);
			this.rt_Help.TabIndex = 1;
			this.rt_Help.Text = "";
			// 
			// pa_Help
			// 
			this.pa_Help.BackColor = System.Drawing.SystemColors.Control;
			this.pa_Help.Controls.Add(this.rt_Help);
			this.pa_Help.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pa_Help.Location = new System.Drawing.Point(0, 0);
			this.pa_Help.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Help.Name = "pa_Help";
			this.pa_Help.Padding = new System.Windows.Forms.Padding(15, 6, 10, 5);
			this.pa_Help.Size = new System.Drawing.Size(442, 544);
			this.pa_Help.TabIndex = 0;
			// 
			// ColorSelectorHelp
			// 
			this.ClientSize = new System.Drawing.Size(442, 544);
			this.Controls.Add(this.pa_Help);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.MaximizeBox = false;
			this.Name = "ColorSelectorHelp";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - ColorSelector help";
			this.pa_Help.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
