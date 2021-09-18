using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class About
	{
		#region Designer
		Button btn_Close;
		Label la_Text;

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btn_Close = new System.Windows.Forms.Button();
			this.la_Text = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btn_Close
			// 
			this.btn_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Close.Location = new System.Drawing.Point(305, 173);
			this.btn_Close.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Close.Name = "btn_Close";
			this.btn_Close.Size = new System.Drawing.Size(75, 30);
			this.btn_Close.TabIndex = 0;
			this.btn_Close.Text = "heiL";
			this.btn_Close.UseVisualStyleBackColor = true;
			// 
			// la_Text
			// 
			this.la_Text.Location = new System.Drawing.Point(10, 10);
			this.la_Text.Margin = new System.Windows.Forms.Padding(0);
			this.la_Text.Name = "la_Text";
			this.la_Text.Size = new System.Drawing.Size(370, 160);
			this.la_Text.TabIndex = 1;
			// 
			// About
			// 
			this.AcceptButton = this.btn_Close;
			this.CancelButton = this.btn_Close;
			this.ClientSize = new System.Drawing.Size(384, 206);
			this.Controls.Add(this.la_Text);
			this.Controls.Add(this.btn_Close);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.MaximizeBox = false;
			this.Name = "About";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " yata - About";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
