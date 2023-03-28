using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Infobox
	{
		#region Designer
		Label la_head;

		Panel pa_Copyable;
		RichTextBox rt_Copyable;

		Panel pa_buttons;
		Button bu_Retry;
		Button bu_Okay;
		Button bu_Cancel;


		/// <summary>
		/// Initializes <c><see cref="pa_Copyable"/></c>.
		/// </summary>
		/// <remarks>The copy-panel is instantiated only if copyable text is
		/// passed into <c><see cref="Infobox()">Infobox()</see></c>.</remarks>
		void InitializePanel()
		{
			pa_Copyable = new Panel();
			rt_Copyable = new RichTextBox();
			pa_Copyable.SuspendLayout();

			pa_Copyable.Controls.Add(rt_Copyable);
			pa_Copyable.Dock = DockStyle.Fill;
			pa_Copyable.Location = new Point(0, 25);
			pa_Copyable.Margin = new Padding(0);
			pa_Copyable.Name = "pa_Copyable";
			pa_Copyable.Padding = new Padding(17, 9, 2, 5);
			pa_Copyable.Size = new Size(394, 121);
			pa_Copyable.TabIndex = 1;
			pa_Copyable.Paint += OnPaintPanel;

			rt_Copyable.BorderStyle = BorderStyle.None;
			rt_Copyable.DetectUrls = false;
			rt_Copyable.Dock = DockStyle.Fill;
			rt_Copyable.Font = new Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			rt_Copyable.HideSelection = false;
			rt_Copyable.Location = new Point(17, 9);
			rt_Copyable.Margin = new Padding(0);
			rt_Copyable.Name = "rt_Copyable";
			rt_Copyable.ReadOnly = true;
			rt_Copyable.Size = new Size(375, 107);
			rt_Copyable.TabIndex = 0;
			rt_Copyable.Text = "";
			rt_Copyable.WordWrap = false;

			Controls.Add(pa_Copyable);
			pa_Copyable.ResumeLayout(false);
		}

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.la_head = new System.Windows.Forms.Label();
			this.pa_buttons = new System.Windows.Forms.Panel();
			this.bu_Retry = new System.Windows.Forms.Button();
			this.bu_Okay = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.pa_buttons.SuspendLayout();
			this.SuspendLayout();
			// 
			// la_head
			// 
			this.la_head.Dock = System.Windows.Forms.DockStyle.Top;
			this.la_head.Location = new System.Drawing.Point(0, 0);
			this.la_head.Margin = new System.Windows.Forms.Padding(0);
			this.la_head.Name = "la_head";
			this.la_head.Padding = new System.Windows.Forms.Padding(7, 4, 3, 4);
			this.la_head.Size = new System.Drawing.Size(394, 24);
			this.la_head.TabIndex = 0;
			this.la_head.Text = "head";
			this.la_head.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.la_head.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaintHead);
			// 
			// pa_buttons
			// 
			this.pa_buttons.Controls.Add(this.bu_Retry);
			this.pa_buttons.Controls.Add(this.bu_Okay);
			this.pa_buttons.Controls.Add(this.bu_Cancel);
			this.pa_buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pa_buttons.Location = new System.Drawing.Point(0, 146);
			this.pa_buttons.Margin = new System.Windows.Forms.Padding(0);
			this.pa_buttons.Name = "pa_buttons";
			this.pa_buttons.Size = new System.Drawing.Size(394, 30);
			this.pa_buttons.TabIndex = 2;
			this.pa_buttons.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaintBot);
			// 
			// bu_Retry
			// 
			this.bu_Retry.DialogResult = System.Windows.Forms.DialogResult.Retry;
			this.bu_Retry.Location = new System.Drawing.Point(96, 3);
			this.bu_Retry.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Retry.Name = "bu_Retry";
			this.bu_Retry.Size = new System.Drawing.Size(95, 25);
			this.bu_Retry.TabIndex = 0;
			this.bu_Retry.Text = "retry";
			this.bu_Retry.UseVisualStyleBackColor = true;
			this.bu_Retry.Visible = false;
			// 
			// bu_Okay
			// 
			this.bu_Okay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Okay.Location = new System.Drawing.Point(195, 3);
			this.bu_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Okay.Name = "bu_Okay";
			this.bu_Okay.Size = new System.Drawing.Size(95, 25);
			this.bu_Okay.TabIndex = 1;
			this.bu_Okay.Text = "ok";
			this.bu_Okay.UseVisualStyleBackColor = true;
			this.bu_Okay.Visible = false;
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(295, 3);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(95, 25);
			this.bu_Cancel.TabIndex = 2;
			this.bu_Cancel.Text = "cancel";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			// 
			// Infobox
			// 
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(394, 176);
			this.Controls.Add(this.la_head);
			this.Controls.Add(this.pa_buttons);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Infobox";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.pa_buttons.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
