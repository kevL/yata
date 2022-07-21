using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class InfoInputClasses
	{
		Label la_Val;
		ComboBox co_Val;

		CheckBox cb_00;
		CheckBox cb_01;
		CheckBox cb_02;
		CheckBox cb_03;
		CheckBox cb_04;
		CheckBox cb_05;


		Button bu_Clear;
		Button bu_Accept;

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cb_00 = new System.Windows.Forms.CheckBox();
			this.cb_01 = new System.Windows.Forms.CheckBox();
			this.cb_02 = new System.Windows.Forms.CheckBox();
			this.cb_03 = new System.Windows.Forms.CheckBox();
			this.cb_04 = new System.Windows.Forms.CheckBox();
			this.cb_05 = new System.Windows.Forms.CheckBox();
			this.bu_Accept = new System.Windows.Forms.Button();
			this.la_Val = new System.Windows.Forms.Label();
			this.co_Val = new System.Windows.Forms.ComboBox();
			this.bu_Clear = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cb_00
			// 
			this.cb_00.Location = new System.Drawing.Point(15, 25);
			this.cb_00.Margin = new System.Windows.Forms.Padding(0);
			this.cb_00.Name = "cb_00";
			this.cb_00.Size = new System.Drawing.Size(350, 20);
			this.cb_00.TabIndex = 2;
			this.cb_00.Text = "cb_00";
			this.cb_00.UseVisualStyleBackColor = true;
			this.cb_00.Visible = false;
			this.cb_00.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_01
			// 
			this.cb_01.Location = new System.Drawing.Point(15, 45);
			this.cb_01.Margin = new System.Windows.Forms.Padding(0);
			this.cb_01.Name = "cb_01";
			this.cb_01.Size = new System.Drawing.Size(350, 20);
			this.cb_01.TabIndex = 3;
			this.cb_01.Text = "cb_01";
			this.cb_01.UseVisualStyleBackColor = true;
			this.cb_01.Visible = false;
			this.cb_01.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_02
			// 
			this.cb_02.Location = new System.Drawing.Point(15, 65);
			this.cb_02.Margin = new System.Windows.Forms.Padding(0);
			this.cb_02.Name = "cb_02";
			this.cb_02.Size = new System.Drawing.Size(350, 20);
			this.cb_02.TabIndex = 4;
			this.cb_02.Text = "cb_02";
			this.cb_02.UseVisualStyleBackColor = true;
			this.cb_02.Visible = false;
			this.cb_02.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_03
			// 
			this.cb_03.Location = new System.Drawing.Point(15, 85);
			this.cb_03.Margin = new System.Windows.Forms.Padding(0);
			this.cb_03.Name = "cb_03";
			this.cb_03.Size = new System.Drawing.Size(350, 20);
			this.cb_03.TabIndex = 5;
			this.cb_03.Text = "cb_03";
			this.cb_03.UseVisualStyleBackColor = true;
			this.cb_03.Visible = false;
			this.cb_03.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_04
			// 
			this.cb_04.Location = new System.Drawing.Point(15, 105);
			this.cb_04.Margin = new System.Windows.Forms.Padding(0);
			this.cb_04.Name = "cb_04";
			this.cb_04.Size = new System.Drawing.Size(350, 20);
			this.cb_04.TabIndex = 6;
			this.cb_04.Text = "cb_04";
			this.cb_04.UseVisualStyleBackColor = true;
			this.cb_04.Visible = false;
			this.cb_04.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// cb_05
			// 
			this.cb_05.Location = new System.Drawing.Point(15, 125);
			this.cb_05.Margin = new System.Windows.Forms.Padding(0);
			this.cb_05.Name = "cb_05";
			this.cb_05.Size = new System.Drawing.Size(350, 20);
			this.cb_05.TabIndex = 7;
			this.cb_05.Text = "cb_05";
			this.cb_05.UseVisualStyleBackColor = true;
			this.cb_05.Visible = false;
			this.cb_05.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// bu_Accept
			// 
			this.bu_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Accept.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bu_Accept.Location = new System.Drawing.Point(0, 174);
			this.bu_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Accept.Name = "bu_Accept";
			this.bu_Accept.Size = new System.Drawing.Size(369, 27);
			this.bu_Accept.TabIndex = 9;
			this.bu_Accept.Text = "accept";
			this.bu_Accept.UseVisualStyleBackColor = true;
			// 
			// la_Val
			// 
			this.la_Val.Dock = System.Windows.Forms.DockStyle.Top;
			this.la_Val.Location = new System.Drawing.Point(0, 0);
			this.la_Val.Margin = new System.Windows.Forms.Padding(0);
			this.la_Val.Name = "la_Val";
			this.la_Val.Size = new System.Drawing.Size(369, 20);
			this.la_Val.TabIndex = 0;
			this.la_Val.Text = "la_Val";
			this.la_Val.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// co_Val
			// 
			this.co_Val.Dock = System.Windows.Forms.DockStyle.Top;
			this.co_Val.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.co_Val.FormattingEnabled = true;
			this.co_Val.Location = new System.Drawing.Point(0, 20);
			this.co_Val.Margin = new System.Windows.Forms.Padding(0);
			this.co_Val.MaxDropDownItems = 30;
			this.co_Val.Name = "co_Val";
			this.co_Val.Size = new System.Drawing.Size(369, 21);
			this.co_Val.TabIndex = 1;
			this.co_Val.Visible = false;
			this.co_Val.SelectedIndexChanged += new System.EventHandler(this.changed_Combobox);
			// 
			// bu_Clear
			// 
			this.bu_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Clear.Location = new System.Drawing.Point(5, 150);
			this.bu_Clear.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Clear.Name = "bu_Clear";
			this.bu_Clear.Size = new System.Drawing.Size(65, 22);
			this.bu_Clear.TabIndex = 8;
			this.bu_Clear.Text = "clear";
			this.bu_Clear.UseVisualStyleBackColor = true;
			this.bu_Clear.Click += new System.EventHandler(this.click_Clear);
			// 
			// InfoInputClasses
			// 
			this.AcceptButton = this.bu_Accept;
			this.ClientSize = new System.Drawing.Size(369, 201);
			this.Controls.Add(this.bu_Clear);
			this.Controls.Add(this.cb_05);
			this.Controls.Add(this.cb_04);
			this.Controls.Add(this.cb_03);
			this.Controls.Add(this.cb_02);
			this.Controls.Add(this.cb_01);
			this.Controls.Add(this.cb_00);
			this.Controls.Add(this.co_Val);
			this.Controls.Add(this.la_Val);
			this.Controls.Add(this.bu_Accept);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InfoInputClasses";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.ResumeLayout(false);

		}
	}
}
