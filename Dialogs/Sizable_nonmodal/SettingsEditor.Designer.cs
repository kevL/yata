using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class SettingsEditor
	{
		#region Designer
		RichTextBox rtb_Settings;
		Panel pa_Buttons;
		Button bu_Cancel;
		Button bu_Okay;
		Button bu_Insert;

		private void InitializeComponent()
		{
			this.rtb_Settings = new System.Windows.Forms.RichTextBox();
			this.pa_Buttons = new System.Windows.Forms.Panel();
			this.bu_Insert = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Okay = new System.Windows.Forms.Button();
			this.pa_Buttons.SuspendLayout();
			this.SuspendLayout();
			// 
			// rtb_Settings
			// 
			this.rtb_Settings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb_Settings.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtb_Settings.HideSelection = false;
			this.rtb_Settings.Location = new System.Drawing.Point(0, 0);
			this.rtb_Settings.Margin = new System.Windows.Forms.Padding(0);
			this.rtb_Settings.Name = "rtb_Settings";
			this.rtb_Settings.Size = new System.Drawing.Size(592, 423);
			this.rtb_Settings.TabIndex = 0;
			this.rtb_Settings.Text = "";
			this.rtb_Settings.WordWrap = false;
			// 
			// pa_Buttons
			// 
			this.pa_Buttons.Controls.Add(this.bu_Insert);
			this.pa_Buttons.Controls.Add(this.bu_Cancel);
			this.pa_Buttons.Controls.Add(this.bu_Okay);
			this.pa_Buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pa_Buttons.Location = new System.Drawing.Point(0, 423);
			this.pa_Buttons.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Buttons.Name = "pa_Buttons";
			this.pa_Buttons.Size = new System.Drawing.Size(592, 35);
			this.pa_Buttons.TabIndex = 1;
			// 
			// bu_Insert
			// 
			this.bu_Insert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Insert.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Insert.ForeColor = System.Drawing.Color.RoyalBlue;
			this.bu_Insert.Location = new System.Drawing.Point(10, 8);
			this.bu_Insert.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Insert.Name = "bu_Insert";
			this.bu_Insert.Size = new System.Drawing.Size(75, 23);
			this.bu_Insert.TabIndex = 0;
			this.bu_Insert.Text = "UPDATE";
			this.bu_Insert.UseVisualStyleBackColor = true;
			this.bu_Insert.Click += new System.EventHandler(this.click_Insert);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(430, 5);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(75, 26);
			this.bu_Cancel.TabIndex = 1;
			this.bu_Cancel.Text = "cancel";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.click_Cancel);
			// 
			// bu_Okay
			// 
			this.bu_Okay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Okay.Location = new System.Drawing.Point(511, 5);
			this.bu_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Okay.Name = "bu_Okay";
			this.bu_Okay.Size = new System.Drawing.Size(75, 26);
			this.bu_Okay.TabIndex = 2;
			this.bu_Okay.Text = "Save";
			this.bu_Okay.UseVisualStyleBackColor = true;
			this.bu_Okay.Click += new System.EventHandler(this.click_Okay);
			// 
			// SettingsEditor
			// 
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(592, 458);
			this.Controls.Add(this.rtb_Settings);
			this.Controls.Add(this.pa_Buttons);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.Name = "SettingsEditor";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Settings.Cfg";
			this.pa_Buttons.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
