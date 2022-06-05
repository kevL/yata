using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class About
	{
		#region Designer
		Label la_Text;
		Button bu_Close;

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.la_Text = new System.Windows.Forms.Label();
			this.bu_Close = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// la_Text
			// 
			this.la_Text.Location = new System.Drawing.Point(10, 10);
			this.la_Text.Margin = new System.Windows.Forms.Padding(0);
			this.la_Text.Name = "la_Text";
			this.la_Text.Size = new System.Drawing.Size(370, 160);
			this.la_Text.TabIndex = 0;
			// 
			// bu_Close
			// 
			this.bu_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Close.Location = new System.Drawing.Point(305, 173);
			this.bu_Close.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Close.Name = "bu_Close";
			this.bu_Close.Size = new System.Drawing.Size(75, 30);
			this.bu_Close.TabIndex = 1;
			this.bu_Close.Text = "heiL";
			this.bu_Close.UseVisualStyleBackColor = true;
			// 
			// About
			// 
			this.AcceptButton = this.bu_Close;
			this.CancelButton = this.bu_Close;
			this.ClientSize = new System.Drawing.Size(384, 206);
			this.Controls.Add(this.la_Text);
			this.Controls.Add(this.bu_Close);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.MaximizeBox = false;
			this.Name = "About";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - About";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
