using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog that displays entries of TalkTable(s).
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

		int _eId; // talktable's EntryId
		int _eId_init;

		SortedDictionary<int, string> _dict;
		int lo, hi;

		bool _init;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// @note Check that the cell's text parses to a valid non-negative
		/// integer, or blank "****", before allowing instantiation - see
		/// YataForm.ShowCellMenu()).
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="f"></param>
		internal TalkDialog(Cell cell, YataForm f)
		{
			InitializeComponent();

			_f = f;

			string strref = cell.text;
			if (strref == gs.Stars) strref = "0";

			Text = " tlk - " + strref;

			_eId_init = Int32.Parse(strref);

			if ((_eId_init & TalkReader.bitCusto) == 0)
			{
				_dict = TalkReader.DictDialo;
				lo = TalkReader.loDialo;
				hi = TalkReader.hiDialo;
			}
			else
			{
				_dict = TalkReader.DictCusto;
				lo = TalkReader.loCusto;
				hi = TalkReader.hiCusto;

				_init = true;
				cb_Custo.Checked = true;
				_init = false;
			}

			_eId_init &= TalkReader.strref;

			tb_Strref.Text = _eId_init.ToString(); // <- sets '_eId' and 'rtb_Copyable.Text'


			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			if (Settings._fontf != null)
			{
				rtb_Copyable.Font.Dispose();
				rtb_Copyable.Font = Settings._fontf;

				tb_Strref.Font.Dispose();
				tb_Strref.Font = Settings._fontfdialog;
			}


			int w = GetWidth(rtb_Copyable.Text) + 30;					// +30 = parent panel's pad left+right +5
			pnl_Copyable.Height = GetHeight(rtb_Copyable.Text) + 20;	// +20 = parent panel's pad top+bot +5

			if      (w < WIDTH_Min) w = WIDTH_Min;
			else if (w > WIDTH_Max) w = WIDTH_Max;

			int h = pnl_Head.Height + pnl_Copyable.Height + btn_Cancel.Height;
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

			btn_Backward.Enabled =
			btn_Forward .Enabled = _dict.Count != 0;

			if (TalkReader.AltLabel != null)
				cb_Custo.Text = TalkReader.AltLabel;
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
		}

		protected override void OnResize(EventArgs e)
		{
			pnl_Copyable.Height = ClientSize.Height
								- pnl_Head  .Height
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
				rtb_Copyable.Text = _dict[_eId = _eId_init]; // revert to default.
			}
			else
			{
				int result;
				if (!Int32.TryParse(tb_Strref.Text, out result)
					|| result < 0 || result > TalkReader.strref)
				{
					tb_Strref.Text = _eId_init.ToString(); // recurse.
					tb_Strref.SelectionStart = tb_Strref.Text.Length;
				}
				else // let the Select button handle it.
				{
					_eId = result; // This is the setter for the '_eId' val.

					if (_dict.ContainsKey(_eId))
					{
						rtb_Copyable.Text = _dict[_eId];
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
			if (_dict.ContainsKey(_eId)
				|| MessageBox.Show(this,
								   "Strref not found.",
								   " Bad Strref",
								   MessageBoxButtons.OKCancel,
								   MessageBoxIcon.Warning,
								   MessageBoxDefaultButton.Button2,
								   0) == DialogResult.OK)
			{
				if (cb_Custo.Checked)
					_eId |= TalkReader.bitCusto;

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
			do
			{
				if (_eId <= lo)
					_eId = hi + 1;

				if (_dict.ContainsKey(--_eId))
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
			do
			{
				if (_eId >= hi)
					_eId = lo - 1;

				if (_dict.ContainsKey(++_eId))
				{
					tb_Strref.Text = _eId.ToString();
					return;
				}
			}
			while (true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void checkchanged_Custo(object sender, EventArgs e)
		{
			if (!_init)
			{
				if (!((CheckBox)sender).Checked)
				{
					_dict = TalkReader.DictDialo;
					lo = TalkReader.loDialo;
					hi = TalkReader.hiDialo;
				}
				else
				{
					_dict = TalkReader.DictCusto;
					lo = TalkReader.loCusto;
					hi = TalkReader.hiCusto;
				}

				if (_dict.ContainsKey(_eId))
				{
					rtb_Copyable.Text = _dict[_eId];
				}
				else
					rtb_Copyable.Text = String.Empty;

				btn_Backward.Enabled =
				btn_Forward .Enabled = _dict.Count != 0;
			}
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

		Panel pnl_Head;
		RichTextBox rtb_Copyable;
		Panel pnl_Copyable;
		Button btn_Cancel;
		Button btn_Select;
		Button btn_Forward;
		Button btn_Prevert;
		Button btn_Backward;
		TextBox tb_Strref;
		CheckBox cb_Custo;

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
			this.pnl_Head = new System.Windows.Forms.Panel();
			this.cb_Custo = new System.Windows.Forms.CheckBox();
			this.tb_Strref = new System.Windows.Forms.TextBox();
			this.btn_Forward = new System.Windows.Forms.Button();
			this.btn_Prevert = new System.Windows.Forms.Button();
			this.btn_Backward = new System.Windows.Forms.Button();
			this.rtb_Copyable = new System.Windows.Forms.RichTextBox();
			this.pnl_Copyable = new System.Windows.Forms.Panel();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.btn_Select = new System.Windows.Forms.Button();
			this.pnl_Head.SuspendLayout();
			this.pnl_Copyable.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnl_Head
			// 
			this.pnl_Head.Controls.Add(this.cb_Custo);
			this.pnl_Head.Controls.Add(this.tb_Strref);
			this.pnl_Head.Controls.Add(this.btn_Forward);
			this.pnl_Head.Controls.Add(this.btn_Prevert);
			this.pnl_Head.Controls.Add(this.btn_Backward);
			this.pnl_Head.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnl_Head.Location = new System.Drawing.Point(0, 0);
			this.pnl_Head.Margin = new System.Windows.Forms.Padding(0);
			this.pnl_Head.Name = "pnl_Head";
			this.pnl_Head.Size = new System.Drawing.Size(492, 26);
			this.pnl_Head.TabIndex = 0;
			// 
			// cb_Custo
			// 
			this.cb_Custo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.cb_Custo.Location = new System.Drawing.Point(172, 3);
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
			this.tb_Strref.Size = new System.Drawing.Size(62, 22);
			this.tb_Strref.TabIndex = 0;
			this.tb_Strref.Text = "-1";
			this.tb_Strref.TextChanged += new System.EventHandler(this.textchanged_Strref);
			// 
			// btn_Forward
			// 
			this.btn_Forward.Location = new System.Drawing.Point(130, 1);
			this.btn_Forward.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Forward.Name = "btn_Forward";
			this.btn_Forward.Size = new System.Drawing.Size(30, 23);
			this.btn_Forward.TabIndex = 3;
			this.btn_Forward.Text = "+";
			this.btn_Forward.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btn_Forward.UseVisualStyleBackColor = true;
			this.btn_Forward.Click += new System.EventHandler(this.click_btnPos);
			// 
			// btn_Prevert
			// 
			this.btn_Prevert.Location = new System.Drawing.Point(100, 1);
			this.btn_Prevert.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Prevert.Name = "btn_Prevert";
			this.btn_Prevert.Size = new System.Drawing.Size(30, 23);
			this.btn_Prevert.TabIndex = 2;
			this.btn_Prevert.Text = "_";
			this.btn_Prevert.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btn_Prevert.UseVisualStyleBackColor = true;
			this.btn_Prevert.Click += new System.EventHandler(this.click_btnPrevert);
			// 
			// btn_Backward
			// 
			this.btn_Backward.Location = new System.Drawing.Point(70, 1);
			this.btn_Backward.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Backward.Name = "btn_Backward";
			this.btn_Backward.Size = new System.Drawing.Size(30, 23);
			this.btn_Backward.TabIndex = 1;
			this.btn_Backward.Text = "-";
			this.btn_Backward.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btn_Backward.UseVisualStyleBackColor = true;
			this.btn_Backward.Click += new System.EventHandler(this.click_btnPre);
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
			this.rtb_Copyable.Size = new System.Drawing.Size(481, 107);
			this.rtb_Copyable.TabIndex = 0;
			this.rtb_Copyable.Text = "rtb_Copyable";
			// 
			// pnl_Copyable
			// 
			this.pnl_Copyable.BackColor = System.Drawing.Color.Khaki;
			this.pnl_Copyable.Controls.Add(this.rtb_Copyable);
			this.pnl_Copyable.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnl_Copyable.Location = new System.Drawing.Point(0, 26);
			this.pnl_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.pnl_Copyable.Name = "pnl_Copyable";
			this.pnl_Copyable.Padding = new System.Windows.Forms.Padding(7, 1, 4, 1);
			this.pnl_Copyable.Size = new System.Drawing.Size(492, 109);
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
			this.btn_Cancel.TabIndex = 3;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.click_btnCancel);
			// 
			// btn_Select
			// 
			this.btn_Select.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Select.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_Select.Location = new System.Drawing.Point(310, 140);
			this.btn_Select.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Select.Name = "btn_Select";
			this.btn_Select.Size = new System.Drawing.Size(85, 30);
			this.btn_Select.TabIndex = 2;
			this.btn_Select.Text = "Select";
			this.btn_Select.UseVisualStyleBackColor = true;
			this.btn_Select.Click += new System.EventHandler(this.click_btnSelect);
			// 
			// TalkDialog
			// 
			this.AcceptButton = this.btn_Select;
			this.AutoScroll = true;
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(492, 174);
			this.Controls.Add(this.btn_Select);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.pnl_Copyable);
			this.Controls.Add(this.pnl_Head);
			this.Name = "TalkDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.pnl_Head.ResumeLayout(false);
			this.pnl_Head.PerformLayout();
			this.pnl_Copyable.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
