using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class InfoInputFeat
	{
		Label lbl_Val;
		ComboBox cbx_Val;

		CheckBox cb_00;
		CheckBox cb_01;
		CheckBox cb_02;
		CheckBox cb_03;
		CheckBox cb_04;
		CheckBox cb_05;
		CheckBox cb_06;


		Button btn_Clear;
		Button btn_Accept;

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
			this.cb_06 = new System.Windows.Forms.CheckBox();
			this.btn_Accept = new System.Windows.Forms.Button();
			this.lbl_Val = new System.Windows.Forms.Label();
			this.cbx_Val = new System.Windows.Forms.ComboBox();
			this.btn_Clear = new System.Windows.Forms.Button();
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
			// cb_06
			// 
			this.cb_06.Location = new System.Drawing.Point(15, 145);
			this.cb_06.Margin = new System.Windows.Forms.Padding(0);
			this.cb_06.Name = "cb_06";
			this.cb_06.Size = new System.Drawing.Size(350, 20);
			this.cb_06.TabIndex = 8;
			this.cb_06.Text = "cb_06";
			this.cb_06.UseVisualStyleBackColor = true;
			this.cb_06.Visible = false;
			this.cb_06.CheckedChanged += new System.EventHandler(this.changed_Checkbox);
			// 
			// btn_Accept
			// 
			this.btn_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_Accept.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btn_Accept.Location = new System.Drawing.Point(0, 194);
			this.btn_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Accept.Name = "btn_Accept";
			this.btn_Accept.Size = new System.Drawing.Size(369, 27);
			this.btn_Accept.TabIndex = 10;
			this.btn_Accept.Text = "accept";
			this.btn_Accept.UseVisualStyleBackColor = true;
			// 
			// lbl_Val
			// 
			this.lbl_Val.Dock = System.Windows.Forms.DockStyle.Top;
			this.lbl_Val.Location = new System.Drawing.Point(0, 0);
			this.lbl_Val.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Val.Name = "lbl_Val";
			this.lbl_Val.Size = new System.Drawing.Size(369, 20);
			this.lbl_Val.TabIndex = 0;
			this.lbl_Val.Text = "lbl_Val";
			this.lbl_Val.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// cbx_Val
			// 
			this.cbx_Val.Dock = System.Windows.Forms.DockStyle.Top;
			this.cbx_Val.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbx_Val.FormattingEnabled = true;
			this.cbx_Val.Location = new System.Drawing.Point(0, 20);
			this.cbx_Val.Margin = new System.Windows.Forms.Padding(0);
			this.cbx_Val.MaxDropDownItems = 30;
			this.cbx_Val.Name = "cbx_Val";
			this.cbx_Val.Size = new System.Drawing.Size(369, 21);
			this.cbx_Val.TabIndex = 1;
			this.cbx_Val.Visible = false;
			this.cbx_Val.SelectedIndexChanged += new System.EventHandler(this.changed_Combobox);
			// 
			// btn_Clear
			// 
			this.btn_Clear.Location = new System.Drawing.Point(5, 170);
			this.btn_Clear.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Clear.Name = "btn_Clear";
			this.btn_Clear.Size = new System.Drawing.Size(65, 20);
			this.btn_Clear.TabIndex = 9;
			this.btn_Clear.Text = "clear";
			this.btn_Clear.UseVisualStyleBackColor = true;
			this.btn_Clear.Click += new System.EventHandler(this.click_Clear);
			// 
			// InfoInputFeat
			// 
			this.AcceptButton = this.btn_Accept;
			this.ClientSize = new System.Drawing.Size(369, 221);
			this.Controls.Add(this.btn_Clear);
			this.Controls.Add(this.cb_06);
			this.Controls.Add(this.cb_05);
			this.Controls.Add(this.cb_04);
			this.Controls.Add(this.cb_03);
			this.Controls.Add(this.cb_02);
			this.Controls.Add(this.cb_01);
			this.Controls.Add(this.cb_00);
			this.Controls.Add(this.cbx_Val);
			this.Controls.Add(this.lbl_Val);
			this.Controls.Add(this.btn_Accept);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InfoInputFeat";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);

		}
	}
}
