using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog that displays an entry of the TalkTable. Opens non-modally and
	/// there can be as many as you like.
	/// </summary>
	sealed class TalkDialog
		:
			Form
	{
		#region Fields (static)
		const int WIDTH_Min = 325;

		static int _x = -1;
		static int _y = -1;
		#endregion Fields (static)


		#region Fields
		YataForm _f;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="title">a caption on the titlebar</param>
		/// <param name="label">info to be displayed with a proportional font</param>
		/// <param name="copyable">info to be displayed with a fixed font in a
		/// RichTextBox so it can be copied</param>
		/// <param name="f">caller</param>
		internal TalkDialog(
				string title,
				string label,
				string copyable,
				YataForm f)
		{
			InitializeComponent();

			_f = f;

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			if (Settings._fontf != null)
			{
				rtb_Copyable.Font.Dispose();
				rtb_Copyable.Font = Settings._fontf;
			}

			Text = title;

			lbl_Info.Height = YataGraphics.MeasureHeight(label, Font) + 15; // +15 = label's pad top+bot +5
			lbl_Info.Text = label;

			int w;
			if (!String.IsNullOrEmpty(copyable))
			{
				w = GetWidth(copyable) + 30;					// +30 = parent panel's pad left+right +5
				pnl_Copyable.Height = GetHeight(copyable) + 20;	// +20 = parent panel's pad top+bot +5

				rtb_Copyable.Text = copyable;
			}
			else
			{
				pnl_Copyable.Visible = false;
				pnl_Copyable.Height = w = 0;
			}

			if (w < WIDTH_Min) w = WIDTH_Min;

			ClientSize = new Size(w + 20, // +20 = pad real and imagined.
								  lbl_Info.Height + pnl_Copyable.Height + btn_Okay.Height + 15); // +15 = pad above Okay button

			if (_x == -1) _x = _f.Left + 30;
			if (_y == -1) _y = _f.Top  + 30;

			Left = _x;
			Top  = _y;

			MinimumSize = new Size(Width, Height);
			MaximumSize = new Size(1000,  Height);

			rtb_Copyable.BackColor = Color.Khaki;
		}
		#endregion

		#region Events (override)
		/// <summary>
		/// Handles this dialog's load event. Niceties ...
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			rtb_Copyable.AutoWordSelection = false; // <- needs to be here not in the designer to work right.
			rtb_Copyable.Select();
		}

		/// <summary>
		/// Handles this dialog's closing event. Sets the static location and
		/// nulls the differ in 'YataForm'.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_x = Left;
			_y = Top;

			_f._fdiffer = null;

			this.Dispose(true); // <- probably unnecessary.
			base.OnFormClosing(e);
		}
		#endregion Events (override)


		#region Events
		/// <summary>
		/// Handles a click on the Okay button. Closes this dialog without doing
		/// anything else.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_btnOkay(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void paint_CopyPanel(object sender, PaintEventArgs e)
		{
			var TL = new Point(0, 0);
			var TR = new Point(pnl_Copyable.Width - 1, 0);
			var BR = new Point(pnl_Copyable.Width - 1, pnl_Copyable.Height - 1);
			var BL = new Point(0, pnl_Copyable.Height - 1);

			var graphics = e.Graphics;
			graphics.DrawLine(Pencils.DarkLine, TL, TR);
			graphics.DrawLine(Pencils.DarkLine, TR, BR);
			graphics.DrawLine(Pencils.DarkLine, BR, BL);
			graphics.DrawLine(Pencils.DarkLine, BL, TL);
		}
		#endregion Events


		#region Methods
		/// <summary>
		/// Deters width based on longest copyable line.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		int GetWidth(string text)
		{
			string[] lines = text.Split(gs.SEPARATORS, StringSplitOptions.RemoveEmptyEntries);

			int width = 0, test;
			foreach (var line in lines)
			{
				if ((test = YataGraphics.MeasureWidth(line, rtb_Copyable.Font)) > width)
					width = test;
			}
			return width;
		}

		/// <summary>
		/// Deters height based on line-height * lines.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		int GetHeight(string text)
		{
			string[] lines = text.Split(gs.SEPARATORS, StringSplitOptions.None);

			return YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, rtb_Copyable.Font)
				 * lines.Length;
		}
		#endregion Methods


		#region Windows Form Designer generated code
		Container components = null;

		Label lbl_Info;
		RichTextBox rtb_Copyable;
		Panel pnl_Copyable;
		Button btn_Okay;

		/// <summary>
		/// Cleans up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lbl_Info = new System.Windows.Forms.Label();
			this.rtb_Copyable = new System.Windows.Forms.RichTextBox();
			this.pnl_Copyable = new System.Windows.Forms.Panel();
			this.btn_Okay = new System.Windows.Forms.Button();
			this.pnl_Copyable.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbl_Info
			// 
			this.lbl_Info.Dock = System.Windows.Forms.DockStyle.Top;
			this.lbl_Info.Location = new System.Drawing.Point(0, 0);
			this.lbl_Info.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Info.Name = "lbl_Info";
			this.lbl_Info.Padding = new System.Windows.Forms.Padding(10, 10, 0, 0);
			this.lbl_Info.Size = new System.Drawing.Size(492, 30);
			this.lbl_Info.TabIndex = 0;
			this.lbl_Info.Text = "lbl_Info";
			// 
			// rtb_Copyable
			// 
			this.rtb_Copyable.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtb_Copyable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb_Copyable.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtb_Copyable.HideSelection = false;
			this.rtb_Copyable.Location = new System.Drawing.Point(15, 5);
			this.rtb_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.rtb_Copyable.Name = "rtb_Copyable";
			this.rtb_Copyable.ReadOnly = true;
			this.rtb_Copyable.Size = new System.Drawing.Size(467, 90);
			this.rtb_Copyable.TabIndex = 0;
			this.rtb_Copyable.Text = "rtb_Copyable";
			// 
			// pnl_Copyable
			// 
			this.pnl_Copyable.BackColor = System.Drawing.Color.Khaki;
			this.pnl_Copyable.Controls.Add(this.rtb_Copyable);
			this.pnl_Copyable.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnl_Copyable.Location = new System.Drawing.Point(0, 30);
			this.pnl_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.pnl_Copyable.Name = "pnl_Copyable";
			this.pnl_Copyable.Padding = new System.Windows.Forms.Padding(15, 5, 10, 5);
			this.pnl_Copyable.Size = new System.Drawing.Size(492, 100);
			this.pnl_Copyable.TabIndex = 1;
			this.pnl_Copyable.Paint += new System.Windows.Forms.PaintEventHandler(this.paint_CopyPanel);
			// 
			// btn_Okay
			// 
			this.btn_Okay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Okay.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Okay.Location = new System.Drawing.Point(403, 140);
			this.btn_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Okay.Name = "btn_Okay";
			this.btn_Okay.Size = new System.Drawing.Size(85, 30);
			this.btn_Okay.TabIndex = 4;
			this.btn_Okay.Text = "ok";
			this.btn_Okay.UseVisualStyleBackColor = true;
			this.btn_Okay.Click += new System.EventHandler(this.click_btnOkay);
			// 
			// TalkDialog
			// 
			this.AutoScroll = true;
			this.CancelButton = this.btn_Okay;
			this.ClientSize = new System.Drawing.Size(492, 174);
			this.Controls.Add(this.btn_Okay);
			this.Controls.Add(this.pnl_Copyable);
			this.Controls.Add(this.lbl_Info);
			this.MaximizeBox = false;
			this.Name = "TalkDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.pnl_Copyable.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
