using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ReplaceTextDialog
	{
		#region Designer
		Label la_Pretext;
		TextBox tb_Pretext;
		Label la_Postext;
		TextBox tb_Postext;

		GroupBox gb_Match;
		RadioButton rb_Subfield;
		RadioButton rb_Fulfield;
		CheckBox cb_Casesen;

		GroupBox gb_Dir;
		RadioButton rb_U;
		RadioButton rb_D;

		GroupBox gb_Start;
		RadioButton rb_Top;
		RadioButton rb_Sel;
		RadioButton rb_Bot;
		CheckBox cb_Autstep;

		Button bu_Search;
		Button bu_Replace;
		Button bu_Cancel;

		GroupBox gb_Replall;
		CheckBox cb_Replall;
		Button bu_ClearReplaced;
		Button bu_GotoReplaced_pre;
		Button bu_GotoReplaced;


		void InitializeComponent()
		{
			this.la_Pretext = new System.Windows.Forms.Label();
			this.tb_Pretext = new System.Windows.Forms.TextBox();
			this.la_Postext = new System.Windows.Forms.Label();
			this.tb_Postext = new System.Windows.Forms.TextBox();
			this.gb_Match = new System.Windows.Forms.GroupBox();
			this.rb_Subfield = new System.Windows.Forms.RadioButton();
			this.rb_Fulfield = new System.Windows.Forms.RadioButton();
			this.cb_Casesen = new System.Windows.Forms.CheckBox();
			this.gb_Dir = new System.Windows.Forms.GroupBox();
			this.rb_U = new System.Windows.Forms.RadioButton();
			this.rb_D = new System.Windows.Forms.RadioButton();
			this.gb_Start = new System.Windows.Forms.GroupBox();
			this.rb_Top = new System.Windows.Forms.RadioButton();
			this.rb_Sel = new System.Windows.Forms.RadioButton();
			this.rb_Bot = new System.Windows.Forms.RadioButton();
			this.cb_Autstep = new System.Windows.Forms.CheckBox();
			this.bu_Search = new System.Windows.Forms.Button();
			this.bu_Replace = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.gb_Replall = new System.Windows.Forms.GroupBox();
			this.cb_Replall = new System.Windows.Forms.CheckBox();
			this.bu_ClearReplaced = new System.Windows.Forms.Button();
			this.bu_GotoReplaced_pre = new System.Windows.Forms.Button();
			this.bu_GotoReplaced = new System.Windows.Forms.Button();
			this.gb_Match.SuspendLayout();
			this.gb_Dir.SuspendLayout();
			this.gb_Start.SuspendLayout();
			this.gb_Replall.SuspendLayout();
			this.SuspendLayout();
			// 
			// la_Pretext
			// 
			this.la_Pretext.Location = new System.Drawing.Point(0, 2);
			this.la_Pretext.Margin = new System.Windows.Forms.Padding(0);
			this.la_Pretext.Name = "la_Pretext";
			this.la_Pretext.Size = new System.Drawing.Size(40, 20);
			this.la_Pretext.TabIndex = 0;
			this.la_Pretext.Text = "From";
			this.la_Pretext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tb_Pretext
			// 
			this.tb_Pretext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tb_Pretext.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_Pretext.Location = new System.Drawing.Point(43, 2);
			this.tb_Pretext.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Pretext.Name = "tb_Pretext";
			this.tb_Pretext.Size = new System.Drawing.Size(248, 22);
			this.tb_Pretext.TabIndex = 1;
			this.tb_Pretext.TextChanged += new System.EventHandler(this.textchanged_Pretext);
			// 
			// la_Postext
			// 
			this.la_Postext.Location = new System.Drawing.Point(0, 25);
			this.la_Postext.Margin = new System.Windows.Forms.Padding(0);
			this.la_Postext.Name = "la_Postext";
			this.la_Postext.Size = new System.Drawing.Size(40, 20);
			this.la_Postext.TabIndex = 2;
			this.la_Postext.Text = "To";
			this.la_Postext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tb_Postext
			// 
			this.tb_Postext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tb_Postext.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_Postext.Location = new System.Drawing.Point(43, 25);
			this.tb_Postext.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Postext.Name = "tb_Postext";
			this.tb_Postext.Size = new System.Drawing.Size(248, 22);
			this.tb_Postext.TabIndex = 3;
			this.tb_Postext.TextChanged += new System.EventHandler(this.textchanged_Postext);
			// 
			// gb_Match
			// 
			this.gb_Match.Controls.Add(this.rb_Subfield);
			this.gb_Match.Controls.Add(this.rb_Fulfield);
			this.gb_Match.Controls.Add(this.cb_Casesen);
			this.gb_Match.Location = new System.Drawing.Point(0, 50);
			this.gb_Match.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Match.Name = "gb_Match";
			this.gb_Match.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Match.Size = new System.Drawing.Size(118, 79);
			this.gb_Match.TabIndex = 4;
			this.gb_Match.TabStop = false;
			this.gb_Match.Text = " Match ";
			// 
			// rb_Subfield
			// 
			this.rb_Subfield.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.rb_Subfield.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rb_Subfield.Location = new System.Drawing.Point(9, 15);
			this.rb_Subfield.Margin = new System.Windows.Forms.Padding(0);
			this.rb_Subfield.Name = "rb_Subfield";
			this.rb_Subfield.Size = new System.Drawing.Size(104, 20);
			this.rb_Subfield.TabIndex = 0;
			this.rb_Subfield.Text = "subfield";
			this.rb_Subfield.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rb_Subfield.UseVisualStyleBackColor = true;
			this.rb_Subfield.CheckedChanged += new System.EventHandler(this.checkedchanged_SearchTyp);
			// 
			// rb_Fulfield
			// 
			this.rb_Fulfield.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.rb_Fulfield.Checked = true;
			this.rb_Fulfield.Location = new System.Drawing.Point(9, 35);
			this.rb_Fulfield.Margin = new System.Windows.Forms.Padding(0);
			this.rb_Fulfield.Name = "rb_Fulfield";
			this.rb_Fulfield.Size = new System.Drawing.Size(104, 20);
			this.rb_Fulfield.TabIndex = 1;
			this.rb_Fulfield.TabStop = true;
			this.rb_Fulfield.Text = "wholefield";
			this.rb_Fulfield.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rb_Fulfield.UseVisualStyleBackColor = true;
			this.rb_Fulfield.CheckedChanged += new System.EventHandler(this.checkedchanged_SearchTyp);
			// 
			// cb_Casesen
			// 
			this.cb_Casesen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.cb_Casesen.Location = new System.Drawing.Point(9, 56);
			this.cb_Casesen.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Casesen.Name = "cb_Casesen";
			this.cb_Casesen.Size = new System.Drawing.Size(104, 20);
			this.cb_Casesen.TabIndex = 2;
			this.cb_Casesen.Text = "matchcase";
			this.cb_Casesen.UseVisualStyleBackColor = true;
			this.cb_Casesen.CheckedChanged += new System.EventHandler(this.checkedchanged_CaseSens);
			// 
			// gb_Dir
			// 
			this.gb_Dir.Controls.Add(this.rb_U);
			this.gb_Dir.Controls.Add(this.rb_D);
			this.gb_Dir.Location = new System.Drawing.Point(118, 50);
			this.gb_Dir.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Dir.Name = "gb_Dir";
			this.gb_Dir.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Dir.Size = new System.Drawing.Size(50, 59);
			this.gb_Dir.TabIndex = 5;
			this.gb_Dir.TabStop = false;
			this.gb_Dir.Text = " Dir ";
			// 
			// rb_U
			// 
			this.rb_U.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.rb_U.Image = global::yata.Properties.Resources.asc_16px;
			this.rb_U.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.rb_U.Location = new System.Drawing.Point(9, 15);
			this.rb_U.Margin = new System.Windows.Forms.Padding(0);
			this.rb_U.Name = "rb_U";
			this.rb_U.Size = new System.Drawing.Size(36, 20);
			this.rb_U.TabIndex = 0;
			this.rb_U.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rb_U.UseVisualStyleBackColor = true;
			this.rb_U.CheckedChanged += new System.EventHandler(this.checkedchanged_SearchDir);
			// 
			// rb_D
			// 
			this.rb_D.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.rb_D.Checked = true;
			this.rb_D.Image = global::yata.Properties.Resources.des_16px;
			this.rb_D.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.rb_D.Location = new System.Drawing.Point(9, 35);
			this.rb_D.Margin = new System.Windows.Forms.Padding(0);
			this.rb_D.Name = "rb_D";
			this.rb_D.Size = new System.Drawing.Size(36, 20);
			this.rb_D.TabIndex = 1;
			this.rb_D.TabStop = true;
			this.rb_D.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rb_D.UseVisualStyleBackColor = true;
			this.rb_D.CheckedChanged += new System.EventHandler(this.checkedchanged_SearchDir);
			// 
			// gb_Start
			// 
			this.gb_Start.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.gb_Start.Controls.Add(this.rb_Top);
			this.gb_Start.Controls.Add(this.rb_Sel);
			this.gb_Start.Controls.Add(this.rb_Bot);
			this.gb_Start.Controls.Add(this.cb_Autstep);
			this.gb_Start.Location = new System.Drawing.Point(168, 50);
			this.gb_Start.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Start.Name = "gb_Start";
			this.gb_Start.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Start.Size = new System.Drawing.Size(124, 99);
			this.gb_Start.TabIndex = 6;
			this.gb_Start.TabStop = false;
			this.gb_Start.Text = " Start ";
			// 
			// rb_Top
			// 
			this.rb_Top.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.rb_Top.Location = new System.Drawing.Point(9, 15);
			this.rb_Top.Margin = new System.Windows.Forms.Padding(0);
			this.rb_Top.Name = "rb_Top";
			this.rb_Top.Size = new System.Drawing.Size(110, 20);
			this.rb_Top.TabIndex = 0;
			this.rb_Top.Text = "top";
			this.rb_Top.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rb_Top.UseVisualStyleBackColor = true;
			this.rb_Top.CheckedChanged += new System.EventHandler(this.checkedchanged_StartType);
			// 
			// rb_Sel
			// 
			this.rb_Sel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.rb_Sel.Checked = true;
			this.rb_Sel.Location = new System.Drawing.Point(9, 35);
			this.rb_Sel.Margin = new System.Windows.Forms.Padding(0);
			this.rb_Sel.Name = "rb_Sel";
			this.rb_Sel.Size = new System.Drawing.Size(110, 20);
			this.rb_Sel.TabIndex = 1;
			this.rb_Sel.TabStop = true;
			this.rb_Sel.Text = "selected";
			this.rb_Sel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rb_Sel.UseVisualStyleBackColor = true;
			this.rb_Sel.CheckedChanged += new System.EventHandler(this.checkedchanged_StartType);
			// 
			// rb_Bot
			// 
			this.rb_Bot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.rb_Bot.Location = new System.Drawing.Point(9, 55);
			this.rb_Bot.Margin = new System.Windows.Forms.Padding(0);
			this.rb_Bot.Name = "rb_Bot";
			this.rb_Bot.Size = new System.Drawing.Size(110, 20);
			this.rb_Bot.TabIndex = 2;
			this.rb_Bot.Text = "bottom";
			this.rb_Bot.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rb_Bot.UseVisualStyleBackColor = true;
			this.rb_Bot.CheckedChanged += new System.EventHandler(this.checkedchanged_StartType);
			// 
			// cb_Autstep
			// 
			this.cb_Autstep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.cb_Autstep.Location = new System.Drawing.Point(9, 76);
			this.cb_Autstep.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Autstep.Name = "cb_Autstep";
			this.cb_Autstep.Size = new System.Drawing.Size(110, 20);
			this.cb_Autstep.TabIndex = 3;
			this.cb_Autstep.Text = "autostep";
			this.cb_Autstep.UseVisualStyleBackColor = true;
			this.cb_Autstep.CheckedChanged += new System.EventHandler(this.checkedchanged_Autostep);
			// 
			// bu_Search
			// 
			this.bu_Search.Enabled = false;
			this.bu_Search.Location = new System.Drawing.Point(4, 152);
			this.bu_Search.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Search.Name = "bu_Search";
			this.bu_Search.Size = new System.Drawing.Size(79, 25);
			this.bu_Search.TabIndex = 7;
			this.bu_Search.Text = "Search";
			this.bu_Search.UseVisualStyleBackColor = true;
			this.bu_Search.Click += new System.EventHandler(this.click_Search);
			// 
			// bu_Replace
			// 
			this.bu_Replace.Enabled = false;
			this.bu_Replace.Location = new System.Drawing.Point(85, 152);
			this.bu_Replace.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Replace.Name = "bu_Replace";
			this.bu_Replace.Size = new System.Drawing.Size(79, 25);
			this.bu_Replace.TabIndex = 8;
			this.bu_Replace.Text = "do";
			this.bu_Replace.UseVisualStyleBackColor = true;
			this.bu_Replace.Click += new System.EventHandler(this.click_Replace);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(166, 152);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(123, 25);
			this.bu_Cancel.TabIndex = 9;
			this.bu_Cancel.Text = "Cancel";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.click_Cancel);
			// 
			// gb_Replall
			// 
			this.gb_Replall.Controls.Add(this.cb_Replall);
			this.gb_Replall.Controls.Add(this.bu_ClearReplaced);
			this.gb_Replall.Controls.Add(this.bu_GotoReplaced_pre);
			this.gb_Replall.Controls.Add(this.bu_GotoReplaced);
			this.gb_Replall.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.gb_Replall.Location = new System.Drawing.Point(0, 180);
			this.gb_Replall.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Replall.Name = "gb_Replall";
			this.gb_Replall.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Replall.Size = new System.Drawing.Size(292, 68);
			this.gb_Replall.TabIndex = 10;
			this.gb_Replall.TabStop = false;
			this.gb_Replall.Text = " Replace all ";
			// 
			// cb_Replall
			// 
			this.cb_Replall.Location = new System.Drawing.Point(10, 17);
			this.cb_Replall.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Replall.Name = "cb_Replall";
			this.cb_Replall.Size = new System.Drawing.Size(95, 20);
			this.cb_Replall.TabIndex = 0;
			this.cb_Replall.Text = "do ALL";
			this.cb_Replall.UseVisualStyleBackColor = true;
			this.cb_Replall.CheckedChanged += new System.EventHandler(this.checkedchanged_ReplaceAll);
			// 
			// bu_ClearReplaced
			// 
			this.bu_ClearReplaced.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.bu_ClearReplaced.Enabled = false;
			this.bu_ClearReplaced.Location = new System.Drawing.Point(126, 11);
			this.bu_ClearReplaced.Margin = new System.Windows.Forms.Padding(0);
			this.bu_ClearReplaced.Name = "bu_ClearReplaced";
			this.bu_ClearReplaced.Size = new System.Drawing.Size(161, 25);
			this.bu_ClearReplaced.TabIndex = 1;
			this.bu_ClearReplaced.Text = "Clear replaced";
			this.bu_ClearReplaced.UseVisualStyleBackColor = true;
			this.bu_ClearReplaced.Click += new System.EventHandler(this.click_ClearReplaced);
			// 
			// bu_GotoReplaced_pre
			// 
			this.bu_GotoReplaced_pre.Enabled = false;
			this.bu_GotoReplaced_pre.Location = new System.Drawing.Point(6, 38);
			this.bu_GotoReplaced_pre.Margin = new System.Windows.Forms.Padding(0);
			this.bu_GotoReplaced_pre.Name = "bu_GotoReplaced_pre";
			this.bu_GotoReplaced_pre.Size = new System.Drawing.Size(117, 25);
			this.bu_GotoReplaced_pre.TabIndex = 2;
			this.bu_GotoReplaced_pre.Text = "Goto pre";
			this.bu_GotoReplaced_pre.UseVisualStyleBackColor = true;
			this.bu_GotoReplaced_pre.Click += new System.EventHandler(this.click_GotoReplaced);
			// 
			// bu_GotoReplaced
			// 
			this.bu_GotoReplaced.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.bu_GotoReplaced.Enabled = false;
			this.bu_GotoReplaced.Location = new System.Drawing.Point(126, 38);
			this.bu_GotoReplaced.Margin = new System.Windows.Forms.Padding(0);
			this.bu_GotoReplaced.Name = "bu_GotoReplaced";
			this.bu_GotoReplaced.Size = new System.Drawing.Size(161, 25);
			this.bu_GotoReplaced.TabIndex = 3;
			this.bu_GotoReplaced.Text = "Goto next";
			this.bu_GotoReplaced.UseVisualStyleBackColor = true;
			this.bu_GotoReplaced.Click += new System.EventHandler(this.click_GotoReplaced);
			// 
			// ReplaceTextDialog
			// 
			this.AcceptButton = this.bu_Search;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(292, 248);
			this.Controls.Add(this.gb_Replall);
			this.Controls.Add(this.la_Pretext);
			this.Controls.Add(this.tb_Pretext);
			this.Controls.Add(this.la_Postext);
			this.Controls.Add(this.tb_Postext);
			this.Controls.Add(this.gb_Match);
			this.Controls.Add(this.gb_Dir);
			this.Controls.Add(this.gb_Start);
			this.Controls.Add(this.bu_Search);
			this.Controls.Add(this.bu_Replace);
			this.Controls.Add(this.bu_Cancel);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.MaximizeBox = false;
			this.Name = "ReplaceTextDialog";
			this.Text = " Replace text";
			this.gb_Match.ResumeLayout(false);
			this.gb_Dir.ResumeLayout(false);
			this.gb_Start.ResumeLayout(false);
			this.gb_Replall.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
