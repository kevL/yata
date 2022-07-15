using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ClipboardEditor
	{
		Panel pa_top;
		RadioButton rb_Clipboard;
		RadioButton rb_RowsBuffer;
		RadioButton rb_ColBuffer;
		RadioButton rb_CellsBuffer;

		RichTextBox rtb_Clip;

		Panel pa_bot;
		Button bu_Get;
		Button bu_Begone;
		Button bu_Set;


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.pa_top = new System.Windows.Forms.Panel();
			this.rb_Clipboard = new System.Windows.Forms.RadioButton();
			this.rb_RowsBuffer = new System.Windows.Forms.RadioButton();
			this.rb_ColBuffer = new System.Windows.Forms.RadioButton();
			this.rb_CellsBuffer = new System.Windows.Forms.RadioButton();
			this.rtb_Clip = new System.Windows.Forms.RichTextBox();
			this.pa_bot = new System.Windows.Forms.Panel();
			this.bu_Get = new System.Windows.Forms.Button();
			this.bu_Begone = new System.Windows.Forms.Button();
			this.bu_Set = new System.Windows.Forms.Button();
			this.pa_top.SuspendLayout();
			this.pa_bot.SuspendLayout();
			this.SuspendLayout();
			// 
			// pa_top
			// 
			this.pa_top.Controls.Add(this.rb_Clipboard);
			this.pa_top.Controls.Add(this.rb_RowsBuffer);
			this.pa_top.Controls.Add(this.rb_ColBuffer);
			this.pa_top.Controls.Add(this.rb_CellsBuffer);
			this.pa_top.Dock = System.Windows.Forms.DockStyle.Top;
			this.pa_top.Location = new System.Drawing.Point(0, 0);
			this.pa_top.Margin = new System.Windows.Forms.Padding(0);
			this.pa_top.Name = "pa_top";
			this.pa_top.Size = new System.Drawing.Size(642, 21);
			this.pa_top.TabIndex = 0;
			this.pa_top.Paint += new System.Windows.Forms.PaintEventHandler(this.paint_Top);
			// 
			// rb_Clipboard
			// 
			this.rb_Clipboard.Location = new System.Drawing.Point(0, 1);
			this.rb_Clipboard.Margin = new System.Windows.Forms.Padding(0);
			this.rb_Clipboard.Name = "rb_Clipboard";
			this.rb_Clipboard.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.rb_Clipboard.Size = new System.Drawing.Size(130, 19);
			this.rb_Clipboard.TabIndex = 0;
			this.rb_Clipboard.Text = "Clipboard editor";
			this.rb_Clipboard.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.rb_Clipboard.UseVisualStyleBackColor = true;
			this.rb_Clipboard.CheckedChanged += new System.EventHandler(this.checkedchanged);
			// 
			// rb_RowsBuffer
			// 
			this.rb_RowsBuffer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rb_RowsBuffer.Location = new System.Drawing.Point(250, 1);
			this.rb_RowsBuffer.Margin = new System.Windows.Forms.Padding(0);
			this.rb_RowsBuffer.Name = "rb_RowsBuffer";
			this.rb_RowsBuffer.Size = new System.Drawing.Size(130, 19);
			this.rb_RowsBuffer.TabIndex = 1;
			this.rb_RowsBuffer.Text = "view Rows buffer";
			this.rb_RowsBuffer.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.rb_RowsBuffer.UseVisualStyleBackColor = true;
			this.rb_RowsBuffer.CheckedChanged += new System.EventHandler(this.checkedchanged);
			// 
			// rb_ColBuffer
			// 
			this.rb_ColBuffer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rb_ColBuffer.Location = new System.Drawing.Point(510, 1);
			this.rb_ColBuffer.Margin = new System.Windows.Forms.Padding(0);
			this.rb_ColBuffer.Name = "rb_ColBuffer";
			this.rb_ColBuffer.Size = new System.Drawing.Size(130, 19);
			this.rb_ColBuffer.TabIndex = 3;
			this.rb_ColBuffer.Text = "view Col buffer";
			this.rb_ColBuffer.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.rb_ColBuffer.UseVisualStyleBackColor = true;
			this.rb_ColBuffer.CheckedChanged += new System.EventHandler(this.checkedchanged);
			// 
			// rb_CellsBuffer
			// 
			this.rb_CellsBuffer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rb_CellsBuffer.Location = new System.Drawing.Point(380, 1);
			this.rb_CellsBuffer.Margin = new System.Windows.Forms.Padding(0);
			this.rb_CellsBuffer.Name = "rb_CellsBuffer";
			this.rb_CellsBuffer.Size = new System.Drawing.Size(130, 19);
			this.rb_CellsBuffer.TabIndex = 2;
			this.rb_CellsBuffer.Text = "view Cells buffer";
			this.rb_CellsBuffer.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.rb_CellsBuffer.UseVisualStyleBackColor = true;
			this.rb_CellsBuffer.CheckedChanged += new System.EventHandler(this.checkedchanged);
			// 
			// rtb_Clip
			// 
			this.rtb_Clip.DetectUrls = false;
			this.rtb_Clip.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb_Clip.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtb_Clip.HideSelection = false;
			this.rtb_Clip.Location = new System.Drawing.Point(0, 21);
			this.rtb_Clip.Margin = new System.Windows.Forms.Padding(0);
			this.rtb_Clip.Name = "rtb_Clip";
			this.rtb_Clip.Size = new System.Drawing.Size(642, 243);
			this.rtb_Clip.TabIndex = 1;
			this.rtb_Clip.Text = "";
			this.rtb_Clip.WordWrap = false;
			// 
			// pa_bot
			// 
			this.pa_bot.Controls.Add(this.bu_Get);
			this.pa_bot.Controls.Add(this.bu_Begone);
			this.pa_bot.Controls.Add(this.bu_Set);
			this.pa_bot.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pa_bot.Location = new System.Drawing.Point(0, 264);
			this.pa_bot.Margin = new System.Windows.Forms.Padding(0);
			this.pa_bot.Name = "pa_bot";
			this.pa_bot.Size = new System.Drawing.Size(642, 30);
			this.pa_bot.TabIndex = 2;
			this.pa_bot.Paint += new System.Windows.Forms.PaintEventHandler(this.paint_Bot);
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
			// bu_Begone
			// 
			this.bu_Begone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Begone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Begone.Location = new System.Drawing.Point(86, 0);
			this.bu_Begone.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Begone.Name = "bu_Begone";
			this.bu_Begone.Size = new System.Drawing.Size(470, 30);
			this.bu_Begone.TabIndex = 1;
			this.bu_Begone.Text = "begone foul demon";
			this.bu_Begone.UseVisualStyleBackColor = true;
			this.bu_Begone.Click += new System.EventHandler(this.click_Cancel);
			// 
			// bu_Set
			// 
			this.bu_Set.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Set.Location = new System.Drawing.Point(557, 0);
			this.bu_Set.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Set.Name = "bu_Set";
			this.bu_Set.Size = new System.Drawing.Size(80, 30);
			this.bu_Set.TabIndex = 2;
			this.bu_Set.Text = "set";
			this.bu_Set.UseVisualStyleBackColor = true;
			this.bu_Set.Click += new System.EventHandler(this.click_Set);
			// 
			// ClipboardEditor
			// 
			this.CancelButton = this.bu_Begone;
			this.ClientSize = new System.Drawing.Size(642, 294);
			this.Controls.Add(this.rtb_Clip);
			this.Controls.Add(this.pa_top);
			this.Controls.Add(this.pa_bot);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.Name = "ClipboardEditor";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Clipboard editor";
			this.pa_top.ResumeLayout(false);
			this.pa_bot.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
