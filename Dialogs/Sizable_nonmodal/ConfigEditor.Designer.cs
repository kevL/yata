using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ConfigEditor
	{
		#region Designer
		RichTextBox rt_Settings;
		Panel pa_Buttons;
		Button bu_Insert;
		Button bu_Save;
		Button bu_Okay;
		Button bu_Cancel;

		private void InitializeComponent()
		{
			this.rt_Settings = new System.Windows.Forms.RichTextBox();
			this.pa_Buttons = new System.Windows.Forms.Panel();
			this.bu_Insert = new System.Windows.Forms.Button();
			this.bu_Save = new System.Windows.Forms.Button();
			this.bu_Okay = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.pa_Buttons.SuspendLayout();
			this.SuspendLayout();
			// 
			// rt_Settings
			// 
			this.rt_Settings.DetectUrls = false;
			this.rt_Settings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rt_Settings.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rt_Settings.HideSelection = false;
			this.rt_Settings.Location = new System.Drawing.Point(0, 0);
			this.rt_Settings.Margin = new System.Windows.Forms.Padding(0);
			this.rt_Settings.Name = "rt_Settings";
			this.rt_Settings.Size = new System.Drawing.Size(592, 423);
			this.rt_Settings.TabIndex = 0;
			this.rt_Settings.Text = "";
			this.rt_Settings.WordWrap = false;
			// 
			// pa_Buttons
			// 
			this.pa_Buttons.Controls.Add(this.bu_Insert);
			this.pa_Buttons.Controls.Add(this.bu_Save);
			this.pa_Buttons.Controls.Add(this.bu_Okay);
			this.pa_Buttons.Controls.Add(this.bu_Cancel);
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
			this.bu_Insert.ForeColor = System.Drawing.Color.Crimson;
			this.bu_Insert.Location = new System.Drawing.Point(10, 8);
			this.bu_Insert.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Insert.Name = "bu_Insert";
			this.bu_Insert.Size = new System.Drawing.Size(75, 23);
			this.bu_Insert.TabIndex = 0;
			this.bu_Insert.Text = "Update";
			this.bu_Insert.UseVisualStyleBackColor = true;
			this.bu_Insert.Click += new System.EventHandler(this.click_Insert);
			// 
			// bu_Save
			// 
			this.bu_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Save.Location = new System.Drawing.Point(336, 5);
			this.bu_Save.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Save.Name = "bu_Save";
			this.bu_Save.Size = new System.Drawing.Size(80, 26);
			this.bu_Save.TabIndex = 1;
			this.bu_Save.Text = "Save";
			this.bu_Save.UseVisualStyleBackColor = true;
			this.bu_Save.Click += new System.EventHandler(this.click_Save);
			// 
			// bu_Okay
			// 
			this.bu_Okay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Okay.Location = new System.Drawing.Point(421, 5);
			this.bu_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Okay.Name = "bu_Okay";
			this.bu_Okay.Size = new System.Drawing.Size(80, 26);
			this.bu_Okay.TabIndex = 2;
			this.bu_Okay.Text = "Save/close";
			this.bu_Okay.UseVisualStyleBackColor = true;
			this.bu_Okay.Click += new System.EventHandler(this.click_Okay);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(505, 5);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(80, 26);
			this.bu_Cancel.TabIndex = 3;
			this.bu_Cancel.Text = "close";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.click_Cancel);
			// 
			// ConfigEditor
			// 
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(592, 458);
			this.Controls.Add(this.rt_Settings);
			this.Controls.Add(this.pa_Buttons);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.Name = "ConfigEditor";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Settings.Cfg";
			this.pa_Buttons.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
