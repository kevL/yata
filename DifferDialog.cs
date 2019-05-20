using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog for the 2da-differ.
	/// </summary>
	public sealed class DifferDialog
		:
			Form
	{
		#region Fields (static)
		internal static DifferDialog that; // be careful w/ 'that'
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
		public DifferDialog(
				string title,
				string label,
				string copyable,
				YataForm f)
		{
			InitializeComponent();

			if (Settings._font2 != null)
			{
				Font.Dispose();
				Font = Settings._font2;
			}

			Text = title;

			_f = f;
			that = this;

			bool
				bLabel = !String.IsNullOrEmpty(label),
				bCopya = !String.IsNullOrEmpty(copyable);

			int width = 0;
			if (bCopya) // deter total width based on longest copyable line
			{
				width = GetWidth(copyable);
				width += 40; // panel's pad left+right + 5
			}
			else
			{
				pnl_Info.Visible =
				rtb_Info.Visible = false;

				pnl_Info.Height =
				rtb_Info.Height = 0;
			}

			if (width < 350) width = 350;
			var size = new Size(width, Int32.MaxValue);


			if (bLabel)
			{
				int height = GetHeight(label, size);
				lbl_Info.Height = height + 15; // label's pad top+bot +5
			}
			else
			{
				lbl_Info.Visible = false;
				lbl_Info.Height = 0;
			}

			if (bCopya)
			{
				copyable += Environment.NewLine; // add a blank line to bot of the copyable text.
				int height = TextRenderer.MeasureText(
													copyable,
													rtb_Info.Font,
													size).Height;
				pnl_Info.Height = height + 20; // panel's pad top+bot + 5
			}

			ClientSize = new Size(
								width + 25, // +25 for pad real and imagined.
								lbl_Info.Height + rtb_Info.Height + btn_Okay.Height + 20);

			lbl_Info.Text = label;
			rtb_Info.Text = copyable;

			Location = new Point(f.Location.X + 50, f.Location.Y + 50);
		}

		int GetHeight(string text, Size size)
		{
			string[] lines = text.Split(
									new[]{ "\r\n", "\r", "\n" },
									StringSplitOptions.None);

			float heightF;
			int
				height_ = 0,
				height  = 0,
				total   = 0;

			Graphics graphics = CreateGraphics();
			foreach (var line in lines)
			{
				heightF = graphics.MeasureString(line, lbl_Info.Font, size).Height; // NOTE: TextRenderer ain't workin right for that.
				height = (int)Math.Ceiling(heightF);

				if (height_ == 0)
					height_ = height;
				else if (height == 0) // IMPORTANT: 1st line shall not be blank (unless all are blank).
					height = height_;

				total += height;
			}
			return total;
		}

		int GetWidth(string text)
		{
			string[] lines = text.Split(
									new[]{ "\r\n", "\r", "\n" },
									StringSplitOptions.RemoveEmptyEntries);

			int width = 0, test;
			foreach (var line in lines)
			{
				if ((test = TextRenderer.MeasureText(line, rtb_Info.Font).Width) > width)
					width = test;
			}
			return width;
		}


		internal void SetLabelColor(Color color)
		{
			lbl_Info.ForeColor = color;
		}

		internal void ShowGotoButton()
		{
			btn_Goto.Visible = true;
		}

		internal void EnableGotoButton(bool enabled)
		{
			btn_Goto.Enabled = enabled;
		}
		#endregion


		#region Events (override)
		protected override void OnLoad(EventArgs e)
		{
			rtb_Info.AutoWordSelection = false; // <- needs to be here not in the designer to work right.
			rtb_Info.Select();
			rtb_Info.SelectionStart = rtb_Info.Text.Length;
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			that = null;

			this.Dispose(true);
			base.OnFormClosing(e);
		}
		#endregion Events (override)


		#region Events
		void click_btnOkay(object sender, EventArgs e)
		{
			Close();
		}

		void click_btnGoto(object sender, EventArgs e)
		{
			_f.GotoDiffCell();
		}
		#endregion Events



		#region Windows Form Designer generated code
		Container components = null;

		Label lbl_Info;
		RichTextBox rtb_Info;
		Panel pnl_Info;
		Button btn_Goto;
		Button btn_Okay;

		/// <summary>
		/// Clean up any resources being used.
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
			this.rtb_Info = new System.Windows.Forms.RichTextBox();
			this.pnl_Info = new System.Windows.Forms.Panel();
			this.btn_Goto = new System.Windows.Forms.Button();
			this.btn_Okay = new System.Windows.Forms.Button();
			this.pnl_Info.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbl_Info
			// 
			this.lbl_Info.Dock = System.Windows.Forms.DockStyle.Top;
			this.lbl_Info.Location = new System.Drawing.Point(0, 0);
			this.lbl_Info.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Info.Name = "lbl_Info";
			this.lbl_Info.Padding = new System.Windows.Forms.Padding(10, 10, 15, 0);
			this.lbl_Info.Size = new System.Drawing.Size(494, 30);
			this.lbl_Info.TabIndex = 0;
			this.lbl_Info.Text = "lbl_Info";
			this.lbl_Info.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// rtb_Info
			// 
			this.rtb_Info.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtb_Info.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb_Info.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtb_Info.HideSelection = false;
			this.rtb_Info.Location = new System.Drawing.Point(15, 5);
			this.rtb_Info.Margin = new System.Windows.Forms.Padding(0);
			this.rtb_Info.Name = "rtb_Info";
			this.rtb_Info.ReadOnly = true;
			this.rtb_Info.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.rtb_Info.Size = new System.Drawing.Size(469, 90);
			this.rtb_Info.TabIndex = 0;
			this.rtb_Info.Text = "rtb_Info";
			this.rtb_Info.WordWrap = false;
			// 
			// pnl_Info
			// 
			this.pnl_Info.Controls.Add(this.rtb_Info);
			this.pnl_Info.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnl_Info.Location = new System.Drawing.Point(0, 30);
			this.pnl_Info.Margin = new System.Windows.Forms.Padding(0);
			this.pnl_Info.Name = "pnl_Info";
			this.pnl_Info.Padding = new System.Windows.Forms.Padding(15, 5, 10, 5);
			this.pnl_Info.Size = new System.Drawing.Size(494, 100);
			this.pnl_Info.TabIndex = 1;
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
			this.btn_Okay.TabIndex = 3;
			this.btn_Okay.Text = "ok";
			this.btn_Okay.UseVisualStyleBackColor = true;
			this.btn_Okay.Click += new System.EventHandler(this.click_btnOkay);
			// 
			// DifferDialog
			// 
			this.AcceptButton = this.btn_Goto;
			this.AutoScroll = true;
			this.CancelButton = this.btn_Okay;
			this.ClientSize = new System.Drawing.Size(494, 169);
			this.Controls.Add(this.btn_Goto);
			this.Controls.Add(this.btn_Okay);
			this.Controls.Add(this.pnl_Info);
			this.Controls.Add(this.lbl_Info);
			this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximizeBox = false;
			this.Name = "DifferDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.pnl_Info.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
