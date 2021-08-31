using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class RowCreatorDialog
	{
		Button btn_Cancel;
		Button btn_Accept;
		RadioButton rb_StartAdd;
		RadioButton rb_StartInsert;
		GroupBox gb_Start;
		GroupBox gb_Stop;
		RadioButton rb_StopCount;
		RadioButton rb_StopFinish;
		TextBox tb_StartInsert;
		TextBox tb_StartAdd;
		TextBox tb_StopFinish;
		TextBox tb_StopCount;
		Label la_StartInsert;
		Label la_StartAdd;
		Label la_StopFinish;
		Label la_StopCount;
		GroupBox gb_Fillstyle;
		RadioButton rb_FillCopied;
		RadioButton rb_FillStars;
		RadioButton rb_FillSelected;
		Label la_FillCopied;
		Label la_FillSelected;
		Label la_FillStars;

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.btn_Accept = new System.Windows.Forms.Button();
			this.rb_StartAdd = new System.Windows.Forms.RadioButton();
			this.rb_StartInsert = new System.Windows.Forms.RadioButton();
			this.gb_Start = new System.Windows.Forms.GroupBox();
			this.tb_StartInsert = new System.Windows.Forms.TextBox();
			this.la_StartInsert = new System.Windows.Forms.Label();
			this.la_StartAdd = new System.Windows.Forms.Label();
			this.tb_StartAdd = new System.Windows.Forms.TextBox();
			this.gb_Stop = new System.Windows.Forms.GroupBox();
			this.tb_StopCount = new System.Windows.Forms.TextBox();
			this.la_StopFinish = new System.Windows.Forms.Label();
			this.tb_StopFinish = new System.Windows.Forms.TextBox();
			this.rb_StopFinish = new System.Windows.Forms.RadioButton();
			this.la_StopCount = new System.Windows.Forms.Label();
			this.rb_StopCount = new System.Windows.Forms.RadioButton();
			this.gb_Fillstyle = new System.Windows.Forms.GroupBox();
			this.la_FillCopied = new System.Windows.Forms.Label();
			this.la_FillSelected = new System.Windows.Forms.Label();
			this.la_FillStars = new System.Windows.Forms.Label();
			this.rb_FillSelected = new System.Windows.Forms.RadioButton();
			this.rb_FillCopied = new System.Windows.Forms.RadioButton();
			this.rb_FillStars = new System.Windows.Forms.RadioButton();
			this.gb_Start.SuspendLayout();
			this.gb_Stop.SuspendLayout();
			this.gb_Fillstyle.SuspendLayout();
			this.SuspendLayout();
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Cancel.Location = new System.Drawing.Point(10, 200);
			this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(110, 33);
			this.btn_Cancel.TabIndex = 3;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			// 
			// btn_Accept
			// 
			this.btn_Accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_Accept.Location = new System.Drawing.Point(125, 200);
			this.btn_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Accept.Name = "btn_Accept";
			this.btn_Accept.Size = new System.Drawing.Size(110, 33);
			this.btn_Accept.TabIndex = 4;
			this.btn_Accept.UseVisualStyleBackColor = true;
			this.btn_Accept.Click += new System.EventHandler(this.click_Ok);
			// 
			// rb_StartAdd
			// 
			this.rb_StartAdd.Location = new System.Drawing.Point(10, 15);
			this.rb_StartAdd.Margin = new System.Windows.Forms.Padding(0);
			this.rb_StartAdd.Name = "rb_StartAdd";
			this.rb_StartAdd.Size = new System.Drawing.Size(110, 20);
			this.rb_StartAdd.TabIndex = 0;
			this.rb_StartAdd.Text = "Add row(s)";
			this.rb_StartAdd.UseVisualStyleBackColor = true;
			this.rb_StartAdd.CheckedChanged += new System.EventHandler(this.checkedchanged);
			// 
			// rb_StartInsert
			// 
			this.rb_StartInsert.Location = new System.Drawing.Point(10, 35);
			this.rb_StartInsert.Margin = new System.Windows.Forms.Padding(0);
			this.rb_StartInsert.Name = "rb_StartInsert";
			this.rb_StartInsert.Size = new System.Drawing.Size(110, 20);
			this.rb_StartInsert.TabIndex = 3;
			this.rb_StartInsert.Text = "Insert row(s)";
			this.rb_StartInsert.UseVisualStyleBackColor = true;
			// 
			// gb_Start
			// 
			this.gb_Start.Controls.Add(this.tb_StartInsert);
			this.gb_Start.Controls.Add(this.la_StartInsert);
			this.gb_Start.Controls.Add(this.la_StartAdd);
			this.gb_Start.Controls.Add(this.tb_StartAdd);
			this.gb_Start.Controls.Add(this.rb_StartAdd);
			this.gb_Start.Controls.Add(this.rb_StartInsert);
			this.gb_Start.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_Start.Location = new System.Drawing.Point(0, 0);
			this.gb_Start.Name = "gb_Start";
			this.gb_Start.Size = new System.Drawing.Size(244, 60);
			this.gb_Start.TabIndex = 0;
			this.gb_Start.TabStop = false;
			this.gb_Start.Text = " START ";
			// 
			// tb_StartInsert
			// 
			this.tb_StartInsert.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_StartInsert.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tb_StartInsert.Enabled = false;
			this.tb_StartInsert.Location = new System.Drawing.Point(125, 35);
			this.tb_StartInsert.Margin = new System.Windows.Forms.Padding(0);
			this.tb_StartInsert.Name = "tb_StartInsert";
			this.tb_StartInsert.Size = new System.Drawing.Size(60, 20);
			this.tb_StartInsert.TabIndex = 4;
			this.tb_StartInsert.WordWrap = false;
			this.tb_StartInsert.TextChanged += new System.EventHandler(this.textchanged);
			// 
			// la_StartInsert
			// 
			this.la_StartInsert.Location = new System.Drawing.Point(190, 35);
			this.la_StartInsert.Margin = new System.Windows.Forms.Padding(0);
			this.la_StartInsert.Name = "la_StartInsert";
			this.la_StartInsert.Size = new System.Drawing.Size(50, 20);
			this.la_StartInsert.TabIndex = 5;
			this.la_StartInsert.Text = "id";
			this.la_StartInsert.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_StartAdd
			// 
			this.la_StartAdd.Location = new System.Drawing.Point(190, 15);
			this.la_StartAdd.Margin = new System.Windows.Forms.Padding(0);
			this.la_StartAdd.Name = "la_StartAdd";
			this.la_StartAdd.Size = new System.Drawing.Size(50, 20);
			this.la_StartAdd.TabIndex = 2;
			this.la_StartAdd.Text = "id";
			this.la_StartAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tb_StartAdd
			// 
			this.tb_StartAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_StartAdd.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tb_StartAdd.Enabled = false;
			this.tb_StartAdd.Location = new System.Drawing.Point(125, 14);
			this.tb_StartAdd.Margin = new System.Windows.Forms.Padding(0);
			this.tb_StartAdd.Name = "tb_StartAdd";
			this.tb_StartAdd.ReadOnly = true;
			this.tb_StartAdd.Size = new System.Drawing.Size(60, 20);
			this.tb_StartAdd.TabIndex = 1;
			this.tb_StartAdd.WordWrap = false;
			// 
			// gb_Stop
			// 
			this.gb_Stop.Controls.Add(this.tb_StopCount);
			this.gb_Stop.Controls.Add(this.la_StopFinish);
			this.gb_Stop.Controls.Add(this.tb_StopFinish);
			this.gb_Stop.Controls.Add(this.rb_StopFinish);
			this.gb_Stop.Controls.Add(this.la_StopCount);
			this.gb_Stop.Controls.Add(this.rb_StopCount);
			this.gb_Stop.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_Stop.Location = new System.Drawing.Point(0, 60);
			this.gb_Stop.Name = "gb_Stop";
			this.gb_Stop.Size = new System.Drawing.Size(244, 60);
			this.gb_Stop.TabIndex = 1;
			this.gb_Stop.TabStop = false;
			this.gb_Stop.Text = " STOP ";
			// 
			// tb_StopCount
			// 
			this.tb_StopCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_StopCount.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tb_StopCount.Enabled = false;
			this.tb_StopCount.Location = new System.Drawing.Point(125, 35);
			this.tb_StopCount.Margin = new System.Windows.Forms.Padding(0);
			this.tb_StopCount.Name = "tb_StopCount";
			this.tb_StopCount.Size = new System.Drawing.Size(60, 20);
			this.tb_StopCount.TabIndex = 4;
			this.tb_StopCount.WordWrap = false;
			this.tb_StopCount.TextChanged += new System.EventHandler(this.textchanged);
			// 
			// la_StopFinish
			// 
			this.la_StopFinish.Location = new System.Drawing.Point(190, 15);
			this.la_StopFinish.Margin = new System.Windows.Forms.Padding(0);
			this.la_StopFinish.Name = "la_StopFinish";
			this.la_StopFinish.Size = new System.Drawing.Size(50, 20);
			this.la_StopFinish.TabIndex = 2;
			this.la_StopFinish.Text = "id";
			this.la_StopFinish.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tb_StopFinish
			// 
			this.tb_StopFinish.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_StopFinish.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tb_StopFinish.Enabled = false;
			this.tb_StopFinish.Location = new System.Drawing.Point(125, 14);
			this.tb_StopFinish.Margin = new System.Windows.Forms.Padding(0);
			this.tb_StopFinish.Name = "tb_StopFinish";
			this.tb_StopFinish.Size = new System.Drawing.Size(60, 20);
			this.tb_StopFinish.TabIndex = 1;
			this.tb_StopFinish.WordWrap = false;
			this.tb_StopFinish.TextChanged += new System.EventHandler(this.textchanged);
			// 
			// rb_StopFinish
			// 
			this.rb_StopFinish.Location = new System.Drawing.Point(10, 15);
			this.rb_StopFinish.Margin = new System.Windows.Forms.Padding(0);
			this.rb_StopFinish.Name = "rb_StopFinish";
			this.rb_StopFinish.Size = new System.Drawing.Size(110, 20);
			this.rb_StopFinish.TabIndex = 0;
			this.rb_StopFinish.Text = "row Finish";
			this.rb_StopFinish.UseVisualStyleBackColor = true;
			this.rb_StopFinish.CheckedChanged += new System.EventHandler(this.checkedchanged);
			// 
			// la_StopCount
			// 
			this.la_StopCount.Location = new System.Drawing.Point(190, 35);
			this.la_StopCount.Margin = new System.Windows.Forms.Padding(0);
			this.la_StopCount.Name = "la_StopCount";
			this.la_StopCount.Size = new System.Drawing.Size(50, 20);
			this.la_StopCount.TabIndex = 5;
			this.la_StopCount.Text = "row(s)";
			this.la_StopCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rb_StopCount
			// 
			this.rb_StopCount.Location = new System.Drawing.Point(10, 35);
			this.rb_StopCount.Margin = new System.Windows.Forms.Padding(0);
			this.rb_StopCount.Name = "rb_StopCount";
			this.rb_StopCount.Size = new System.Drawing.Size(110, 20);
			this.rb_StopCount.TabIndex = 3;
			this.rb_StopCount.Text = "row Count";
			this.rb_StopCount.UseVisualStyleBackColor = true;
			// 
			// gb_Fillstyle
			// 
			this.gb_Fillstyle.Controls.Add(this.la_FillCopied);
			this.gb_Fillstyle.Controls.Add(this.la_FillSelected);
			this.gb_Fillstyle.Controls.Add(this.la_FillStars);
			this.gb_Fillstyle.Controls.Add(this.rb_FillSelected);
			this.gb_Fillstyle.Controls.Add(this.rb_FillCopied);
			this.gb_Fillstyle.Controls.Add(this.rb_FillStars);
			this.gb_Fillstyle.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_Fillstyle.Location = new System.Drawing.Point(0, 120);
			this.gb_Fillstyle.Name = "gb_Fillstyle";
			this.gb_Fillstyle.Size = new System.Drawing.Size(244, 78);
			this.gb_Fillstyle.TabIndex = 2;
			this.gb_Fillstyle.TabStop = false;
			this.gb_Fillstyle.Text = " Fields ";
			// 
			// la_FillCopied
			// 
			this.la_FillCopied.Location = new System.Drawing.Point(25, 54);
			this.la_FillCopied.Margin = new System.Windows.Forms.Padding(0);
			this.la_FillCopied.Name = "la_FillCopied";
			this.la_FillCopied.Size = new System.Drawing.Size(215, 20);
			this.la_FillCopied.TabIndex = 5;
			this.la_FillCopied.Text = "use Copied row";
			this.la_FillCopied.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.la_FillCopied.Click += new System.EventHandler(this.click_Fill);
			// 
			// la_FillSelected
			// 
			this.la_FillSelected.Location = new System.Drawing.Point(25, 34);
			this.la_FillSelected.Margin = new System.Windows.Forms.Padding(0);
			this.la_FillSelected.Name = "la_FillSelected";
			this.la_FillSelected.Size = new System.Drawing.Size(215, 20);
			this.la_FillSelected.TabIndex = 3;
			this.la_FillSelected.Text = "use Selected row";
			this.la_FillSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.la_FillSelected.Click += new System.EventHandler(this.click_Fill);
			// 
			// la_FillStars
			// 
			this.la_FillStars.Location = new System.Drawing.Point(25, 16);
			this.la_FillStars.Margin = new System.Windows.Forms.Padding(0);
			this.la_FillStars.Name = "la_FillStars";
			this.la_FillStars.Size = new System.Drawing.Size(215, 20);
			this.la_FillStars.TabIndex = 1;
			this.la_FillStars.Text = "****";
			this.la_FillStars.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.la_FillStars.Click += new System.EventHandler(this.click_Fill);
			// 
			// rb_FillSelected
			// 
			this.rb_FillSelected.Location = new System.Drawing.Point(10, 35);
			this.rb_FillSelected.Margin = new System.Windows.Forms.Padding(0);
			this.rb_FillSelected.Name = "rb_FillSelected";
			this.rb_FillSelected.Size = new System.Drawing.Size(15, 20);
			this.rb_FillSelected.TabIndex = 2;
			this.rb_FillSelected.UseVisualStyleBackColor = true;
			// 
			// rb_FillCopied
			// 
			this.rb_FillCopied.Location = new System.Drawing.Point(10, 55);
			this.rb_FillCopied.Margin = new System.Windows.Forms.Padding(0);
			this.rb_FillCopied.Name = "rb_FillCopied";
			this.rb_FillCopied.Size = new System.Drawing.Size(15, 20);
			this.rb_FillCopied.TabIndex = 4;
			this.rb_FillCopied.UseVisualStyleBackColor = true;
			// 
			// rb_FillStars
			// 
			this.rb_FillStars.Checked = true;
			this.rb_FillStars.Location = new System.Drawing.Point(10, 15);
			this.rb_FillStars.Margin = new System.Windows.Forms.Padding(0);
			this.rb_FillStars.Name = "rb_FillStars";
			this.rb_FillStars.Size = new System.Drawing.Size(15, 20);
			this.rb_FillStars.TabIndex = 0;
			this.rb_FillStars.TabStop = true;
			this.rb_FillStars.UseVisualStyleBackColor = true;
			// 
			// RowCreatorDialog
			// 
			this.AcceptButton = this.btn_Accept;
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(244, 236);
			this.Controls.Add(this.gb_Fillstyle);
			this.Controls.Add(this.gb_Stop);
			this.Controls.Add(this.gb_Start);
			this.Controls.Add(this.btn_Accept);
			this.Controls.Add(this.btn_Cancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RowCreatorDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " yata - Create rows";
			this.gb_Start.ResumeLayout(false);
			this.gb_Start.PerformLayout();
			this.gb_Stop.ResumeLayout(false);
			this.gb_Stop.PerformLayout();
			this.gb_Fillstyle.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
