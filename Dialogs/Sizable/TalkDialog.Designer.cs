using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class TalkDialog
	{
		#region Designer
		Panel pnl_Head;
		RichTextBox rtb_Copyable;
		Panel pnl_Copyable;
		Button btn_Cancel;
		Button btn_Accept;
		Button btn_Load;
		Button btn_Forward;
		Button btn_Prevert;
		Button btn_Backward;
		TextBox tb_Strref;
		CheckBox cb_Custo;

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor. Or else POW! RIGHT IN THE KISSER
		/// </summary>
		private void InitializeComponent()
		{
			this.pnl_Head = new System.Windows.Forms.Panel();
			this.cb_Custo = new System.Windows.Forms.CheckBox();
			this.tb_Strref = new System.Windows.Forms.TextBox();
			this.btn_Forward = new System.Windows.Forms.Button();
			this.btn_Prevert = new System.Windows.Forms.Button();
			this.btn_Backward = new System.Windows.Forms.Button();
			this.rtb_Copyable = new System.Windows.Forms.RichTextBox();
			this.pnl_Copyable = new System.Windows.Forms.Panel();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.btn_Accept = new System.Windows.Forms.Button();
			this.btn_Load = new System.Windows.Forms.Button();
			this.pnl_Head.SuspendLayout();
			this.pnl_Copyable.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnl_Head
			// 
			this.pnl_Head.Controls.Add(this.cb_Custo);
			this.pnl_Head.Controls.Add(this.tb_Strref);
			this.pnl_Head.Controls.Add(this.btn_Forward);
			this.pnl_Head.Controls.Add(this.btn_Prevert);
			this.pnl_Head.Controls.Add(this.btn_Backward);
			this.pnl_Head.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnl_Head.Location = new System.Drawing.Point(0, 0);
			this.pnl_Head.Margin = new System.Windows.Forms.Padding(0);
			this.pnl_Head.Name = "pnl_Head";
			this.pnl_Head.Size = new System.Drawing.Size(492, 26);
			this.pnl_Head.TabIndex = 0;
			// 
			// cb_Custo
			// 
			this.cb_Custo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.cb_Custo.Location = new System.Drawing.Point(170, 3);
			this.cb_Custo.Margin = new System.Windows.Forms.Padding(0);
			this.cb_Custo.Name = "cb_Custo";
			this.cb_Custo.Size = new System.Drawing.Size(320, 20);
			this.cb_Custo.TabIndex = 4;
			this.cb_Custo.Text = "Custom";
			this.cb_Custo.UseVisualStyleBackColor = true;
			this.cb_Custo.CheckedChanged += new System.EventHandler(this.checkchanged_Custo);
			// 
			// tb_Strref
			// 
			this.tb_Strref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Strref.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_Strref.Location = new System.Drawing.Point(1, 1);
			this.tb_Strref.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Strref.Name = "tb_Strref";
			this.tb_Strref.Size = new System.Drawing.Size(66, 22);
			this.tb_Strref.TabIndex = 0;
			this.tb_Strref.Text = "-2";
			this.tb_Strref.TextChanged += new System.EventHandler(this.textchanged_Strref);
			// 
			// btn_Forward
			// 
			this.btn_Forward.Location = new System.Drawing.Point(130, 1);
			this.btn_Forward.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Forward.Name = "btn_Forward";
			this.btn_Forward.Size = new System.Drawing.Size(30, 23);
			this.btn_Forward.TabIndex = 3;
			this.btn_Forward.Text = "+";
			this.btn_Forward.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btn_Forward.UseVisualStyleBackColor = true;
			this.btn_Forward.Click += new System.EventHandler(this.click_btnPos);
			// 
			// btn_Prevert
			// 
			this.btn_Prevert.Location = new System.Drawing.Point(100, 1);
			this.btn_Prevert.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Prevert.Name = "btn_Prevert";
			this.btn_Prevert.Size = new System.Drawing.Size(30, 23);
			this.btn_Prevert.TabIndex = 2;
			this.btn_Prevert.Text = "_";
			this.btn_Prevert.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btn_Prevert.UseVisualStyleBackColor = true;
			this.btn_Prevert.Click += new System.EventHandler(this.click_btnPrevert);
			// 
			// btn_Backward
			// 
			this.btn_Backward.Location = new System.Drawing.Point(70, 1);
			this.btn_Backward.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Backward.Name = "btn_Backward";
			this.btn_Backward.Size = new System.Drawing.Size(30, 23);
			this.btn_Backward.TabIndex = 1;
			this.btn_Backward.Text = "-";
			this.btn_Backward.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btn_Backward.UseVisualStyleBackColor = true;
			this.btn_Backward.Click += new System.EventHandler(this.click_btnPre);
			// 
			// rtb_Copyable
			// 
			this.rtb_Copyable.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtb_Copyable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb_Copyable.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtb_Copyable.HideSelection = false;
			this.rtb_Copyable.Location = new System.Drawing.Point(6, 1);
			this.rtb_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.rtb_Copyable.Name = "rtb_Copyable";
			this.rtb_Copyable.ReadOnly = true;
			this.rtb_Copyable.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.rtb_Copyable.Size = new System.Drawing.Size(485, 107);
			this.rtb_Copyable.TabIndex = 0;
			this.rtb_Copyable.Text = "rtb_Copyable";
			// 
			// pnl_Copyable
			// 
			this.pnl_Copyable.BackColor = System.Drawing.Color.Khaki;
			this.pnl_Copyable.Controls.Add(this.rtb_Copyable);
			this.pnl_Copyable.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnl_Copyable.Location = new System.Drawing.Point(0, 26);
			this.pnl_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.pnl_Copyable.Name = "pnl_Copyable";
			this.pnl_Copyable.Padding = new System.Windows.Forms.Padding(6, 1, 1, 1);
			this.pnl_Copyable.Size = new System.Drawing.Size(492, 109);
			this.pnl_Copyable.TabIndex = 1;
			this.pnl_Copyable.Paint += new System.Windows.Forms.PaintEventHandler(this.paint_CopyPanel);
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Cancel.Location = new System.Drawing.Point(400, 140);
			this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(85, 30);
			this.btn_Cancel.TabIndex = 4;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.click_btnCancel);
			// 
			// btn_Accept
			// 
			this.btn_Accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_Accept.Location = new System.Drawing.Point(310, 140);
			this.btn_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Accept.Name = "btn_Accept";
			this.btn_Accept.Size = new System.Drawing.Size(85, 30);
			this.btn_Accept.TabIndex = 3;
			this.btn_Accept.Text = "Accept";
			this.btn_Accept.UseVisualStyleBackColor = true;
			this.btn_Accept.Click += new System.EventHandler(this.click_btnSelect);
			// 
			// btn_Load
			// 
			this.btn_Load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btn_Load.Location = new System.Drawing.Point(5, 145);
			this.btn_Load.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Load.Name = "btn_Load";
			this.btn_Load.Size = new System.Drawing.Size(70, 25);
			this.btn_Load.TabIndex = 2;
			this.btn_Load.Text = "Load ...";
			this.btn_Load.UseVisualStyleBackColor = true;
			this.btn_Load.Click += new System.EventHandler(this.click_btnLoad);
			// 
			// TalkDialog
			// 
			this.AcceptButton = this.btn_Accept;
			this.AutoScroll = true;
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(492, 174);
			this.Controls.Add(this.btn_Load);
			this.Controls.Add(this.btn_Accept);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.pnl_Copyable);
			this.Controls.Add(this.pnl_Head);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.Name = "TalkDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.pnl_Head.ResumeLayout(false);
			this.pnl_Head.PerformLayout();
			this.pnl_Copyable.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
