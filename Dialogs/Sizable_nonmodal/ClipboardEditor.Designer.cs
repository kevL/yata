using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ClipboardEditor
	{
		RichTextBox rtb_Clip;
		Panel pa_bot;
		Button bu_Set;
		Button bu_Get;
		Button bu_Done;

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.rtb_Clip = new System.Windows.Forms.RichTextBox();
			this.pa_bot = new System.Windows.Forms.Panel();
			this.bu_Done = new System.Windows.Forms.Button();
			this.bu_Set = new System.Windows.Forms.Button();
			this.bu_Get = new System.Windows.Forms.Button();
			this.pa_bot.SuspendLayout();
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
			// pa_bot
			// 
			this.pa_bot.Controls.Add(this.bu_Done);
			this.pa_bot.Controls.Add(this.bu_Set);
			this.pa_bot.Controls.Add(this.bu_Get);
			this.pa_bot.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pa_bot.Location = new System.Drawing.Point(0, 94);
			this.pa_bot.Margin = new System.Windows.Forms.Padding(0);
			this.pa_bot.Name = "pa_bot";
			this.pa_bot.Size = new System.Drawing.Size(542, 30);
			this.pa_bot.TabIndex = 1;
			// 
			// bu_Done
			// 
			this.bu_Done.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Done.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Done.Location = new System.Drawing.Point(86, 0);
			this.bu_Done.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Done.Name = "bu_Done";
			this.bu_Done.Size = new System.Drawing.Size(370, 30);
			this.bu_Done.TabIndex = 1;
			this.bu_Done.Text = "begone foul demon";
			this.bu_Done.UseVisualStyleBackColor = true;
			this.bu_Done.Click += new System.EventHandler(this.click_Cancel);
			// 
			// bu_Set
			// 
			this.bu_Set.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Set.Location = new System.Drawing.Point(457, 0);
			this.bu_Set.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Set.Name = "bu_Set";
			this.bu_Set.Size = new System.Drawing.Size(80, 30);
			this.bu_Set.TabIndex = 2;
			this.bu_Set.Text = "set";
			this.bu_Set.UseVisualStyleBackColor = true;
			this.bu_Set.Click += new System.EventHandler(this.click_Set);
			// 
			// bu_Get
			// 
			this.bu_Get.Location = new System.Drawing.Point(5, 0);
			this.bu_Get.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Get.Name = "bu_Get";
			this.bu_Get.Size = new System.Drawing.Size(80, 30);
			this.bu_Get.TabIndex = 0;
			this.bu_Get.Text = "get";
			this.bu_Get.UseVisualStyleBackColor = true;
			this.bu_Get.Click += new System.EventHandler(this.click_Get);
			// 
			// ClipboardEditor
			// 
			this.CancelButton = this.bu_Done;
			this.ClientSize = new System.Drawing.Size(542, 124);
			this.Controls.Add(this.rtb_Clip);
			this.Controls.Add(this.pa_bot);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(250, 0);
			this.Name = "ClipboardEditor";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Clipboard editor";
			this.pa_bot.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
