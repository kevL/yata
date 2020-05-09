using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog for the 2da-differ.
	/// </summary>
	sealed class DifferDialog
		:
			Form
	{
		#region Fields (static)
		const int WIDTH_Min = 325;

		static int _x = -1;
		static int _y = -1;
		#endregion Fields (static)


		#region Fields
		readonly YataForm _f;
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
		internal DifferDialog(
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
				copyable += Environment.NewLine; // add a blank line to bot of the copyable text.

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
								  lbl_Info.Height + pnl_Copyable.Height + btn_Okay.Height);

			if (_x == -1) _x = _f.Left + 50;
			if (_y == -1) _y = _f.Top  + 50;

			Left = _x;
			Top  = _y;

			MinimumSize = new Size(Width, Height);
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
			rtb_Copyable.SelectionStart = rtb_Copyable.Text.Length;
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
		/// Handles a click on the Reset button. Clears and desyncs the diff'd
		/// tables. Closes this dialog via 'YataForm'.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_btnReset(object sender, EventArgs e)
		{
			_f.tabclick_DiffReset(null, EventArgs.Empty);
		}

		/// <summary>
		/// Handles a click on the Goto button. Goes to the next diff'd cell or
		/// the previous diff'd cell if [Shift] is depressed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_btnGoto(object sender, EventArgs e)
		{
			_f.GotoDiffCell();
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


		/// <summary>
		/// Sets the text-color of the info.
		/// </summary>
		/// <param name="color"></param>
		internal void SetLabelColor(Color color)
		{
			lbl_Info.ForeColor = color;
		}

		/// <summary>
		/// Visibles the reset button.
		/// </summary>
		internal void ShowResetButton()
		{
			btn_Reset.Visible = true;
		}

		/// <summary>
		/// Visibles the goto button.
		/// </summary>
		internal void ShowGotoButton()
		{
			btn_Goto.Visible = true;
		}

		/// <summary>
		/// Enables/disables the goto button.
		/// </summary>
		/// <param name="enabled">true to enable</param>
		internal void EnableGotoButton(bool enabled)
		{
			btn_Goto.Enabled = enabled;
		}
		#endregion Methods


		#region Windows Form Designer generated code
		Container components = null;

		Label lbl_Info;
		RichTextBox rtb_Copyable;
		Panel pnl_Copyable;
		Button btn_Goto;
		Button btn_Okay;
		Button btn_Reset;

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
			this.btn_Goto = new System.Windows.Forms.Button();
			this.btn_Okay = new System.Windows.Forms.Button();
			this.btn_Reset = new System.Windows.Forms.Button();
			this.pnl_Copyable.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbl_Info
			// 
			this.lbl_Info.Dock = System.Windows.Forms.DockStyle.Top;
			this.lbl_Info.Location = new System.Drawing.Point(0, 0);
			this.lbl_Info.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Info.Name = "lbl_Info";
			this.lbl_Info.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this.lbl_Info.Size = new System.Drawing.Size(494, 30);
			this.lbl_Info.TabIndex = 0;
			this.lbl_Info.Text = "lbl_Info";
			this.lbl_Info.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
			this.rtb_Copyable.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.rtb_Copyable.Size = new System.Drawing.Size(469, 90);
			this.rtb_Copyable.TabIndex = 0;
			this.rtb_Copyable.Text = "rtb_Copyable";
			this.rtb_Copyable.WordWrap = false;
			// 
			// pnl_Copyable
			// 
			this.pnl_Copyable.Controls.Add(this.rtb_Copyable);
			this.pnl_Copyable.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnl_Copyable.Location = new System.Drawing.Point(0, 30);
			this.pnl_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.pnl_Copyable.Name = "pnl_Copyable";
			this.pnl_Copyable.Padding = new System.Windows.Forms.Padding(15, 5, 10, 5);
			this.pnl_Copyable.Size = new System.Drawing.Size(494, 100);
			this.pnl_Copyable.TabIndex = 1;
			// 
			// btn_Goto
			// 
			this.btn_Goto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btn_Goto.Location = new System.Drawing.Point(5, 135);
			this.btn_Goto.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Goto.Name = "btn_Goto";
			this.btn_Goto.Size = new System.Drawing.Size(85, 30);
			this.btn_Goto.TabIndex = 2;
			this.btn_Goto.Text = "goto";
			this.btn_Goto.UseVisualStyleBackColor = true;
			this.btn_Goto.Visible = false;
			this.btn_Goto.Click += new System.EventHandler(this.click_btnGoto);
			// 
			// btn_Okay
			// 
			this.btn_Okay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Okay.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Okay.Location = new System.Drawing.Point(405, 135);
			this.btn_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Okay.Name = "btn_Okay";
			this.btn_Okay.Size = new System.Drawing.Size(85, 30);
			this.btn_Okay.TabIndex = 4;
			this.btn_Okay.Text = "ok";
			this.btn_Okay.UseVisualStyleBackColor = true;
			this.btn_Okay.Click += new System.EventHandler(this.click_btnOkay);
			// 
			// btn_Reset
			// 
			this.btn_Reset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Reset.Location = new System.Drawing.Point(95, 135);
			this.btn_Reset.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Reset.Name = "btn_Reset";
			this.btn_Reset.Size = new System.Drawing.Size(305, 30);
			this.btn_Reset.TabIndex = 3;
			this.btn_Reset.Text = "reset";
			this.btn_Reset.UseVisualStyleBackColor = true;
			this.btn_Reset.Visible = false;
			this.btn_Reset.Click += new System.EventHandler(this.click_btnReset);
			// 
			// DifferDialog
			// 
			this.AcceptButton = this.btn_Goto;
			this.AutoScroll = true;
			this.CancelButton = this.btn_Okay;
			this.ClientSize = new System.Drawing.Size(494, 169);
			this.Controls.Add(this.btn_Okay);
			this.Controls.Add(this.btn_Reset);
			this.Controls.Add(this.btn_Goto);
			this.Controls.Add(this.pnl_Copyable);
			this.Controls.Add(this.lbl_Info);
			this.MaximizeBox = false;
			this.Name = "DifferDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.pnl_Copyable.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
