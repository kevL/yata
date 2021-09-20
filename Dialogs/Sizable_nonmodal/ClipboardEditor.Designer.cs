using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ClipboardEditor
	{
		RichTextBox rtb_Clip;
		Panel panel1;
		Button btn_Set;
		Button btn_Get;
		Button btn_Done;

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.rtb_Clip = new System.Windows.Forms.RichTextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btn_Done = new System.Windows.Forms.Button();
			this.btn_Set = new System.Windows.Forms.Button();
			this.btn_Get = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// rtb_Clip
			// 
			this.rtb_Clip.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb_Clip.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtb_Clip.HideSelection = false;
			this.rtb_Clip.Location = new System.Drawing.Point(0, 0);
			this.rtb_Clip.Margin = new System.Windows.Forms.Padding(0);
			this.rtb_Clip.Name = "rtb_Clip";
			this.rtb_Clip.Size = new System.Drawing.Size(542, 94);
			this.rtb_Clip.TabIndex = 0;
			this.rtb_Clip.Text = "";
			this.rtb_Clip.WordWrap = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btn_Done);
			this.panel1.Controls.Add(this.btn_Set);
			this.panel1.Controls.Add(this.btn_Get);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 94);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(542, 30);
			this.panel1.TabIndex = 1;
			// 
			// btn_Done
			// 
			this.btn_Done.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Done.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Done.Location = new System.Drawing.Point(86, 0);
			this.btn_Done.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Done.Name = "btn_Done";
			this.btn_Done.Size = new System.Drawing.Size(370, 30);
			this.btn_Done.TabIndex = 1;
			this.btn_Done.Text = "begone foul demon";
			this.btn_Done.UseVisualStyleBackColor = true;
			this.btn_Done.Click += new System.EventHandler(this.click_Cancel);
			// 
			// btn_Set
			// 
			this.btn_Set.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Set.Location = new System.Drawing.Point(457, 0);
			this.btn_Set.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Set.Name = "btn_Set";
			this.btn_Set.Size = new System.Drawing.Size(80, 30);
			this.btn_Set.TabIndex = 2;
			this.btn_Set.Text = "set";
			this.btn_Set.UseVisualStyleBackColor = true;
			this.btn_Set.Click += new System.EventHandler(this.click_Set);
			// 
			// btn_Get
			// 
			this.btn_Get.Location = new System.Drawing.Point(5, 0);
			this.btn_Get.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Get.Name = "btn_Get";
			this.btn_Get.Size = new System.Drawing.Size(80, 30);
			this.btn_Get.TabIndex = 0;
			this.btn_Get.Text = "get";
			this.btn_Get.UseVisualStyleBackColor = true;
			this.btn_Get.Click += new System.EventHandler(this.click_Get);
			// 
			// ClipboardEditor
			// 
			this.CancelButton = this.btn_Done;
			this.ClientSize = new System.Drawing.Size(542, 124);
			this.Controls.Add(this.rtb_Clip);
			this.Controls.Add(this.panel1);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(250, 0);
			this.Name = "ClipboardEditor";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Clipboard editor";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
