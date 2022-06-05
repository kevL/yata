using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class TalkDialog
	{
		#region Designer
		Panel pa_Head;
		RichTextBox rt_Copyable;
		Panel pa_Copyable;
		Button bu_Cancel;
		Button bu_Accept;
		Button bu_Load;
		Button bu_Forward;
		Button bu_Prevert;
		Button bu_Backward;
		TextBox tb_Strref;
		CheckBox cb_Custo;

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor. Or else POW! RIGHT IN THE KISSER
		/// </summary>
		private void InitializeComponent()
		{
			this.pa_Head = new System.Windows.Forms.Panel();
			this.cb_Custo = new System.Windows.Forms.CheckBox();
			this.tb_Strref = new System.Windows.Forms.TextBox();
			this.bu_Forward = new System.Windows.Forms.Button();
			this.bu_Prevert = new System.Windows.Forms.Button();
			this.bu_Backward = new System.Windows.Forms.Button();
			this.rt_Copyable = new System.Windows.Forms.RichTextBox();
			this.pa_Copyable = new System.Windows.Forms.Panel();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Accept = new System.Windows.Forms.Button();
			this.bu_Load = new System.Windows.Forms.Button();
			this.pa_Head.SuspendLayout();
			this.pa_Copyable.SuspendLayout();
			this.SuspendLayout();
			// 
			// pa_Head
			// 
			this.pa_Head.Controls.Add(this.cb_Custo);
			this.pa_Head.Controls.Add(this.tb_Strref);
			this.pa_Head.Controls.Add(this.bu_Forward);
			this.pa_Head.Controls.Add(this.bu_Prevert);
			this.pa_Head.Controls.Add(this.bu_Backward);
			this.pa_Head.Dock = System.Windows.Forms.DockStyle.Top;
			this.pa_Head.Location = new System.Drawing.Point(0, 0);
			this.pa_Head.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Head.Name = "pa_Head";
			this.pa_Head.Size = new System.Drawing.Size(492, 26);
			this.pa_Head.TabIndex = 0;
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
			this.tb_Strref.WordWrap = false;
			this.tb_Strref.TextChanged += new System.EventHandler(this.textchanged_Strref);
			// 
			// bu_Forward
			// 
			this.bu_Forward.Location = new System.Drawing.Point(130, 1);
			this.bu_Forward.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Forward.Name = "bu_Forward";
			this.bu_Forward.Size = new System.Drawing.Size(30, 23);
			this.bu_Forward.TabIndex = 3;
			this.bu_Forward.Text = "+";
			this.bu_Forward.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.bu_Forward.UseVisualStyleBackColor = true;
			this.bu_Forward.Click += new System.EventHandler(this.click_btnPos);
			// 
			// bu_Prevert
			// 
			this.bu_Prevert.Location = new System.Drawing.Point(100, 1);
			this.bu_Prevert.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Prevert.Name = "bu_Prevert";
			this.bu_Prevert.Size = new System.Drawing.Size(30, 23);
			this.bu_Prevert.TabIndex = 2;
			this.bu_Prevert.Text = "_";
			this.bu_Prevert.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.bu_Prevert.UseVisualStyleBackColor = true;
			this.bu_Prevert.Click += new System.EventHandler(this.click_btnPrevert);
			// 
			// bu_Backward
			// 
			this.bu_Backward.Location = new System.Drawing.Point(70, 1);
			this.bu_Backward.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Backward.Name = "bu_Backward";
			this.bu_Backward.Size = new System.Drawing.Size(30, 23);
			this.bu_Backward.TabIndex = 1;
			this.bu_Backward.Text = "-";
			this.bu_Backward.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.bu_Backward.UseVisualStyleBackColor = true;
			this.bu_Backward.Click += new System.EventHandler(this.click_btnPre);
			// 
			// rt_Copyable
			// 
			this.rt_Copyable.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rt_Copyable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rt_Copyable.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rt_Copyable.HideSelection = false;
			this.rt_Copyable.Location = new System.Drawing.Point(6, 1);
			this.rt_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.rt_Copyable.Name = "rt_Copyable";
			this.rt_Copyable.ReadOnly = true;
			this.rt_Copyable.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.rt_Copyable.Size = new System.Drawing.Size(485, 107);
			this.rt_Copyable.TabIndex = 0;
			this.rt_Copyable.Text = "rt_Copyable";
			// 
			// pa_Copyable
			// 
			this.pa_Copyable.BackColor = System.Drawing.Color.Khaki;
			this.pa_Copyable.Controls.Add(this.rt_Copyable);
			this.pa_Copyable.Dock = System.Windows.Forms.DockStyle.Top;
			this.pa_Copyable.Location = new System.Drawing.Point(0, 26);
			this.pa_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Copyable.Name = "pa_Copyable";
			this.pa_Copyable.Padding = new System.Windows.Forms.Padding(6, 1, 1, 1);
			this.pa_Copyable.Size = new System.Drawing.Size(492, 109);
			this.pa_Copyable.TabIndex = 1;
			this.pa_Copyable.Paint += new System.Windows.Forms.PaintEventHandler(this.paint_CopyPanel);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(400, 140);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(85, 30);
			this.bu_Cancel.TabIndex = 4;
			this.bu_Cancel.Text = "Cancel";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			// 
			// bu_Accept
			// 
			this.bu_Accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Accept.Location = new System.Drawing.Point(310, 140);
			this.bu_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Accept.Name = "bu_Accept";
			this.bu_Accept.Size = new System.Drawing.Size(85, 30);
			this.bu_Accept.TabIndex = 3;
			this.bu_Accept.Text = "Accept";
			this.bu_Accept.UseVisualStyleBackColor = true;
			this.bu_Accept.Click += new System.EventHandler(this.click_btnSelect);
			// 
			// bu_Load
			// 
			this.bu_Load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Load.Location = new System.Drawing.Point(5, 145);
			this.bu_Load.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Load.Name = "bu_Load";
			this.bu_Load.Size = new System.Drawing.Size(70, 25);
			this.bu_Load.TabIndex = 2;
			this.bu_Load.Text = "Load ...";
			this.bu_Load.UseVisualStyleBackColor = true;
			this.bu_Load.Click += new System.EventHandler(this.click_btnLoad);
			// 
			// TalkDialog
			// 
			this.AcceptButton = this.bu_Accept;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(492, 174);
			this.Controls.Add(this.bu_Load);
			this.Controls.Add(this.bu_Accept);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.pa_Copyable);
			this.Controls.Add(this.pa_Head);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.Name = "TalkDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.pa_Head.ResumeLayout(false);
			this.pa_Head.PerformLayout();
			this.pa_Copyable.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
