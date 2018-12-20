namespace yata
{
	partial class ClipboardEditor
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.RichTextBox rtb_Text;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btn_Set;
		private System.Windows.Forms.Button btn_Get;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.rtb_Text = new System.Windows.Forms.RichTextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btn_Set = new System.Windows.Forms.Button();
			this.btn_Get = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// rtb_Text
			// 
			this.rtb_Text.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb_Text.Location = new System.Drawing.Point(0, 0);
			this.rtb_Text.Name = "rtb_Text";
			this.rtb_Text.Size = new System.Drawing.Size(542, 94);
			this.rtb_Text.TabIndex = 0;
			this.rtb_Text.Text = "";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btn_Set);
			this.panel1.Controls.Add(this.btn_Get);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 94);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(542, 30);
			this.panel1.TabIndex = 1;
			// 
			// btn_Set
			// 
			this.btn_Set.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Set.Location = new System.Drawing.Point(455, 0);
			this.btn_Set.Name = "btn_Set";
			this.btn_Set.Size = new System.Drawing.Size(80, 30);
			this.btn_Set.TabIndex = 1;
			this.btn_Set.Text = "set";
			this.btn_Set.UseVisualStyleBackColor = true;
			this.btn_Set.Click += new System.EventHandler(this.click_Set);
			// 
			// btn_Get
			// 
			this.btn_Get.Location = new System.Drawing.Point(5, 0);
			this.btn_Get.Name = "btn_Get";
			this.btn_Get.Size = new System.Drawing.Size(80, 30);
			this.btn_Get.TabIndex = 0;
			this.btn_Get.Text = "get";
			this.btn_Get.UseVisualStyleBackColor = true;
			this.btn_Get.Click += new System.EventHandler(this.click_Get);
			// 
			// ClipboardEditor
			// 
			this.ClientSize = new System.Drawing.Size(542, 124);
			this.Controls.Add(this.rtb_Text);
			this.Controls.Add(this.panel1);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.Name = "ClipboardEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Yata - Clipboard editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
