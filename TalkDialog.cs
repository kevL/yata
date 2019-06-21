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
		const int WIDTH_Max = 900;
		const int HIGHT_Min = 130;
		const int HIGHT_Max = 500;

		const int VERT_PAD_CANCEL = 10;

		static int _x = -1;
		static int _y = -1;
		#endregion Fields (static)


		#region Fields
		YataForm _f;

		int _eId; // talkfile's EntryId
		int _eId_init;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// @note Check that the cell's text parses to a valid non-negative
		/// integer, or blank "****", and that 'DictDialog' contains an entry
		/// for that integer before allowing instantiation - see ShowCellMenu()).
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="f"></param>
		internal TalkDialog(Cell cell, YataForm f)
		{
			InitializeComponent();

			_f = f;

			string strref = cell.text;
			if (strref == gs.Stars) strref = "0";
			_eId_init = Int32.Parse(strref); // NOTE: '_eId' will be set OnTextChanged when 'tb_Strref.Text' gets set below.


			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			if (Settings._fontf != null)
			{
				rtb_Copyable.Font.Dispose();
				tb_Strref   .Font.Dispose();

				rtb_Copyable.Font =
				tb_Strref   .Font = Settings._fontf;
			}

			Text = " tlk - " + strref;

			lbl_Info .Height = YataGraphics.MeasureHeight(strref, Font) + 15; // +15 = label's pad top+bot +5
			lbl_Info .Text =
			tb_Strref.Text = strref;

			string copyable = TalkReader.DictDialog[_eId];

			int w = GetWidth(copyable) + 30;				// +30 = parent panel's pad left+right +5
			pnl_Copyable.Height = GetHeight(copyable) + 20;	// +20 = parent panel's pad top+bot +5

			if      (w < WIDTH_Min) w = WIDTH_Min;
			else if (w > WIDTH_Max) w = WIDTH_Max;

			int h = lbl_Info.Height + pnl_Copyable.Height + btn_Cancel.Height;
			if      (h < HIGHT_Min) h = HIGHT_Min;
			else if (h > HIGHT_Max) h = HIGHT_Max;

			ClientSize = new Size(w + 20,				// +20 = pad real and imagined.
								  h + VERT_PAD_CANCEL);	// +10 = pad above Cancel button

			if (_x == -1) _x = _f.Left + 30;
			if (_y == -1) _y = _f.Top  + 30;

			Left = _x;
			Top  = _y;

			rtb_Copyable.BackColor = Color.Khaki; // <- won't work right in the designer.
			tb_Strref   .BackColor = Colors.TextboxBackground;
		}
		#endregion


		#region Events (override)
		/// <summary>
		/// Handles this dialog's load event. Niceties ...
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			rtb_Copyable.AutoWordSelection = false; // <- needs to be here not in the cTor or designer to work right.
			rtb_Copyable.Select();

			rtb_Copyable.Text = TalkReader.DictDialog[_eId]; // sigh.
		}

		protected override void OnResize(EventArgs e)
		{
			pnl_Copyable.Height = ClientSize.Height
								- lbl_Info  .Height
								- btn_Cancel.Height - VERT_PAD_CANCEL;
			base.OnResize(e);
			pnl_Copyable.Invalidate();
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

			this.Dispose(true); // <- probably unnecessary.
			base.OnFormClosing(e);
		}
		#endregion Events (override)


		#region Events
		/// <summary>
		/// Handles the textchanged event of the strref-box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void textchanged_Strref(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(tb_Strref.Text))
			{
				_eId = _eId_init; // revert to default.
			}
			else
			{
				int result;
				if (!Int32.TryParse(tb_Strref.Text, out result)
					|| result < 0)
				{
					tb_Strref.Text = _eId_init.ToString(); // recurse.
					tb_Strref.SelectionStart = tb_Strref.Text.Length;
				}
				else if (result < TalkReader.lo)
				{
					tb_Strref.Text = TalkReader.lo.ToString(); // recurse.
					tb_Strref.SelectionStart = tb_Strref.Text.Length;
				}
				else if (result > TalkReader.hi)
				{
					tb_Strref.Text = TalkReader.hi.ToString(); // recurse.
					tb_Strref.SelectionStart = tb_Strref.Text.Length;
				}
				else // ie, let the Select button handle it.
				{
					_eId = result; // This is the setter for the '_eId' val.

					if (TalkReader.DictDialog.ContainsKey(_eId))
					{
						rtb_Copyable.Text = TalkReader.DictDialog[_eId];
					}
					else
						rtb_Copyable.Text = String.Empty;
				}
			}
		}

		/// <summary>
		/// Handles a click on the Cancel button. Closes this dialog harmlessly.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_btnCancel(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Handles a click on the Select button. Passes the current strref to
		/// YataForm and closes this dialog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_btnSelect(object sender, EventArgs e)
		{
			if (TalkReader.DictDialog.ContainsKey(_eId)
				|| MessageBox.Show(this,
								   "The strref was not found in the Talkfile.",
								   " Bad Strref",
								   MessageBoxButtons.YesNo,
								   MessageBoxIcon.Warning,
								   MessageBoxDefaultButton.Button2,
								   0) == DialogResult.Yes)
			{
				_f._strref = _eId.ToString();
				Close();
			}
		}


		/// <summary>
		/// Handles a click on the Prevert button. Reverts the strref to its
		/// initial val.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_btnPrevert(object sender, EventArgs e)
		{
			if (_eId != _eId_init)
				tb_Strref.Text = _eId_init.ToString();
		}

		/// <summary>
		/// Handles a click on the Pre button. Steps backward to the antecedent
		/// dialog-entry.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_btnPre(object sender, EventArgs e)
		{
			var dict = TalkReader.DictDialog;

			do
			{
				if (_eId == TalkReader.lo)
					_eId  = TalkReader.hi + 1;

				if (dict.ContainsKey(--_eId))
				{
					tb_Strref.Text = _eId.ToString();
					return;
				}
			}
			while (true);
		}

		/// <summary>
		/// Handles a click on the Pos button. Steps forward to the posterior
		/// dialog-entry.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_btnPos(object sender, EventArgs e)
		{
			var dict = TalkReader.DictDialog;

			do
			{
				if (_eId == TalkReader.hi)
					_eId  = TalkReader.lo - 1;

				if (dict.ContainsKey(++_eId))
				{
					tb_Strref.Text = _eId.ToString();
					return;
				}
			}
			while (true);
		}


		/// <summary>
		/// Draws a 1px border around the copyable-panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void paint_CopyPanel(object sender, PaintEventArgs e)
		{
			int w = pnl_Copyable.Width  - 1;
			int h = pnl_Copyable.Height - 1;

			var tl = new Point(0, 0);
			var tr = new Point(w, 0);
			var br = new Point(w, h);
			var bl = new Point(0, h);

			var graphics = e.Graphics;
			graphics.DrawLine(Pencils.DarkLine, tl, tr);
			graphics.DrawLine(Pencils.DarkLine, tr, br);
			graphics.DrawLine(Pencils.DarkLine, br, bl);
			graphics.DrawLine(Pencils.DarkLine, bl, tl);
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
		Button btn_Cancel;
		Button btn_Select;
		Button btn_Forward;
		Button btn_Prevert;
		Button btn_Backward;
		TextBox tb_Strref;

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
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.btn_Forward = new System.Windows.Forms.Button();
			this.btn_Prevert = new System.Windows.Forms.Button();
			this.btn_Backward = new System.Windows.Forms.Button();
			this.btn_Select = new System.Windows.Forms.Button();
			this.tb_Strref = new System.Windows.Forms.TextBox();
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
			this.rtb_Copyable.Location = new System.Drawing.Point(7, 1);
			this.rtb_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.rtb_Copyable.Name = "rtb_Copyable";
			this.rtb_Copyable.ReadOnly = true;
			this.rtb_Copyable.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.rtb_Copyable.Size = new System.Drawing.Size(484, 98);
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
			this.pnl_Copyable.Padding = new System.Windows.Forms.Padding(7, 1, 1, 1);
			this.pnl_Copyable.Size = new System.Drawing.Size(492, 100);
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
			this.btn_Cancel.TabIndex = 7;
			this.btn_Cancel.Text = "cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.click_btnCancel);
			// 
			// btn_Forward
			// 
			this.btn_Forward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Forward.Location = new System.Drawing.Point(460, 0);
			this.btn_Forward.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Forward.Name = "btn_Forward";
			this.btn_Forward.Size = new System.Drawing.Size(30, 23);
			this.btn_Forward.TabIndex = 5;
			this.btn_Forward.Text = "+";
			this.btn_Forward.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btn_Forward.UseVisualStyleBackColor = true;
			this.btn_Forward.Click += new System.EventHandler(this.click_btnPos);
			// 
			// btn_Prevert
			// 
			this.btn_Prevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Prevert.Location = new System.Drawing.Point(430, 0);
			this.btn_Prevert.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Prevert.Name = "btn_Prevert";
			this.btn_Prevert.Size = new System.Drawing.Size(30, 23);
			this.btn_Prevert.TabIndex = 4;
			this.btn_Prevert.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btn_Prevert.UseVisualStyleBackColor = true;
			this.btn_Prevert.Click += new System.EventHandler(this.click_btnPrevert);
			// 
			// btn_Backward
			// 
			this.btn_Backward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Backward.Location = new System.Drawing.Point(400, 0);
			this.btn_Backward.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Backward.Name = "btn_Backward";
			this.btn_Backward.Size = new System.Drawing.Size(30, 23);
			this.btn_Backward.TabIndex = 3;
			this.btn_Backward.Text = "-";
			this.btn_Backward.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btn_Backward.UseVisualStyleBackColor = true;
			this.btn_Backward.Click += new System.EventHandler(this.click_btnPre);
			// 
			// btn_Select
			// 
			this.btn_Select.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Select.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_Select.Location = new System.Drawing.Point(310, 140);
			this.btn_Select.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Select.Name = "btn_Select";
			this.btn_Select.Size = new System.Drawing.Size(85, 30);
			this.btn_Select.TabIndex = 6;
			this.btn_Select.Text = "select";
			this.btn_Select.UseVisualStyleBackColor = true;
			this.btn_Select.Click += new System.EventHandler(this.click_btnSelect);
			// 
			// tb_Strref
			// 
			this.tb_Strref.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tb_Strref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Strref.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_Strref.Location = new System.Drawing.Point(328, 1);
			this.tb_Strref.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Strref.Name = "tb_Strref";
			this.tb_Strref.Size = new System.Drawing.Size(70, 22);
			this.tb_Strref.TabIndex = 2;
			this.tb_Strref.TextChanged += new System.EventHandler(this.textchanged_Strref);
			// 
			// TalkDialog
			// 
			this.AcceptButton = this.btn_Select;
			this.AutoScroll = true;
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(492, 174);
			this.Controls.Add(this.tb_Strref);
			this.Controls.Add(this.btn_Select);
			this.Controls.Add(this.btn_Backward);
			this.Controls.Add(this.btn_Prevert);
			this.Controls.Add(this.btn_Forward);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.pnl_Copyable);
			this.Controls.Add(this.lbl_Info);
			this.Name = "TalkDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.pnl_Copyable.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}
