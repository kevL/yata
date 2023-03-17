using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ColorOptionsF
	{
		#region Designer
		Button bu_Cancel;
		GroupBox gb_Region;
		Label la_Grid;
		RadioButton rb_Grid;
		Label la_Backcolor;
		Panel pa_Backcolor;

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.gb_Region = new System.Windows.Forms.GroupBox();
			this.la_Grid = new System.Windows.Forms.Label();
			this.rb_Grid = new System.Windows.Forms.RadioButton();
			this.la_Backcolor = new System.Windows.Forms.Label();
			this.pa_Backcolor = new System.Windows.Forms.Panel();
			this.gb_Region.SuspendLayout();
			this.SuspendLayout();
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(674, 410);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(80, 26);
			this.bu_Cancel.TabIndex = 0;
			this.bu_Cancel.Text = "close";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.click_Cancel);
			// 
			// gb_Region
			// 
			this.gb_Region.Controls.Add(this.la_Grid);
			this.gb_Region.Controls.Add(this.rb_Grid);
			this.gb_Region.Dock = System.Windows.Forms.DockStyle.Left;
			this.gb_Region.Location = new System.Drawing.Point(0, 0);
			this.gb_Region.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Region.Name = "gb_Region";
			this.gb_Region.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Region.Size = new System.Drawing.Size(200, 442);
			this.gb_Region.TabIndex = 1;
			this.gb_Region.TabStop = false;
			this.gb_Region.Text = " region ";
			// 
			// la_Grid
			// 
			this.la_Grid.Location = new System.Drawing.Point(30, 25);
			this.la_Grid.Margin = new System.Windows.Forms.Padding(0);
			this.la_Grid.Name = "la_Grid";
			this.la_Grid.Size = new System.Drawing.Size(160, 16);
			this.la_Grid.TabIndex = 1;
			this.la_Grid.Text = "grid";
			// 
			// rb_Grid
			// 
			this.rb_Grid.Checked = true;
			this.rb_Grid.Location = new System.Drawing.Point(10, 25);
			this.rb_Grid.Margin = new System.Windows.Forms.Padding(0);
			this.rb_Grid.Name = "rb_Grid";
			this.rb_Grid.Size = new System.Drawing.Size(12, 16);
			this.rb_Grid.TabIndex = 0;
			this.rb_Grid.TabStop = true;
			this.rb_Grid.UseVisualStyleBackColor = true;
			// 
			// la_Backcolor
			// 
			this.la_Backcolor.Location = new System.Drawing.Point(220, 24);
			this.la_Backcolor.Margin = new System.Windows.Forms.Padding(0);
			this.la_Backcolor.Name = "la_Backcolor";
			this.la_Backcolor.Size = new System.Drawing.Size(100, 16);
			this.la_Backcolor.TabIndex = 2;
			this.la_Backcolor.Text = "backcolor";
			this.la_Backcolor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pa_Backcolor
			// 
			this.pa_Backcolor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_Backcolor.Location = new System.Drawing.Point(320, 20);
			this.pa_Backcolor.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Backcolor.Name = "pa_Backcolor";
			this.pa_Backcolor.Size = new System.Drawing.Size(25, 25);
			this.pa_Backcolor.TabIndex = 3;
			this.pa_Backcolor.Click += new System.EventHandler(this.click_colorpanel);
			// 
			// ColorOptionsF
			// 
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(761, 442);
			this.Controls.Add(this.pa_Backcolor);
			this.Controls.Add(this.la_Backcolor);
			this.Controls.Add(this.gb_Region);
			this.Controls.Add(this.bu_Cancel);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.Name = "ColorOptionsF";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Colors.Cfg";
			this.gb_Region.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
