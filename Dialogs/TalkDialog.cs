using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog that displays entries of TalkTable(s).
	/// </summary>
	sealed class TalkDialog
		: Form
	{
		#region Fields (static)
		const int WIDTH_Min = 325;
		const int WIDTH_Max = 900;
		const int HIGHT_Min = 130;
		const int HIGHT_Max = 500;

		const int pad_HORI = 20; // pad real and imagined
		const int pad_VERT = 10; // pad above Cancel button

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
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="f"></param>
		/// <remarks>Check that the <c><see cref="Cell.text">Cell.text</see></c>
		/// of <paramref name="cell"/> parses to a valid value before invoking
		/// this <c>TalkDialog</c>.</remarks>
		internal TalkDialog(Cell cell, YataForm f)
		{
			InitializeComponent();

			_f = f;

			string strref = cell.text;
			if (strref == gs.Stars) strref = "0";

			Text = " tlk - " + strref;

			_eId_init = Int32.Parse(strref);

			if (_eId_init == TalkReader.invalid || (_eId_init & TalkReader.bitCusto) == 0)
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

			if (_eId_init != TalkReader.invalid)
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
				tb_Strref.Font = Settings._fontf_tb;
			}


			int w = GetWidth(rtb_Copyable.Text) + 30;					// +30 = parent panel's pad left+right +5
			pnl_Copyable.Height = GetHeight(rtb_Copyable.Text) + 20;	// +20 = parent panel's pad top+bot +5

			if      (w < WIDTH_Min) w = WIDTH_Min;
			else if (w > WIDTH_Max) w = WIDTH_Max;

			int h = pnl_Head.Height + pnl_Copyable.Height + btn_Cancel.Height;
			if      (h < HIGHT_Min) h = HIGHT_Min;
			else if (h > HIGHT_Max) h = HIGHT_Max;

			ClientSize = new Size(w + pad_HORI,
								  h + pad_VERT);

			if (_x == -1) _x = _f.Left + 30;
			if (_y == -1) _y = _f.Top  + 30;

			Left = _x;
			Top  = _y;


			pokeUi(_dict.Count != 0);

			tb_Strref.BackColor = Colors.TextboxBackground; // <- won't work right in the designer.

			if (TalkReader.AltLabel != null)
				cb_Custo.Text = TalkReader.AltLabel;

			btn_Accept.Enabled = !YataForm.Table.Readonly;
		}
		#endregion


		#region Events (override)
		/// <summary>
		/// Overrides the <c>Load</c> handler. Niceties ...
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			int widthborder = (Width  - ClientSize.Width) / 2;
			int heighttitle = (Height - ClientSize.Height - 2 * widthborder);

			MinimumSize = new Size(WIDTH_Min + pad_HORI + 2 * widthborder,
								   HIGHT_Min + pad_VERT + 2 * widthborder + heighttitle);

			rtb_Copyable.AutoWordSelection = false; // <- needs to be here not in the cTor for designer to work right.
			rtb_Copyable.Select();
		}

		/// <summary>
		/// Overrides the <c>Resize</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			pnl_Copyable.Height = ClientSize.Height
								- pnl_Head  .Height
								- btn_Cancel.Height - pad_VERT;
			base.OnResize(e);
			pnl_Copyable.Invalidate();
		}

		/// <summary>
		/// Overrides the <c>FormClosing</c> handler. Sets the static location.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_x = Left;
			_y = Top;

			base.OnFormClosing(e);
		}
		#endregion Events (override)


		#region Events
		/// <summary>
		/// Handles the textchanged event of the strref-box.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Strref"/></c></param>
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
					|| result < TalkReader.invalid || result > TalkReader.strref)
				{
					tb_Strref.Text = _eId_init.ToString();
					tb_Strref.SelectionStart = tb_Strref.Text.Length;
					return; // recurse.
				}

				// else let the Select button handle it.
				_eId = result; // This is the setter for the '_eId' val.
			}

			if (_dict.ContainsKey(_eId))
				rtb_Copyable.Text = _dict[_eId];
			else
				rtb_Copyable.Text = String.Empty;
		}

		/// <summary>
		/// Handles a click on the Cancel button. Closes this dialog harmlessly.
		/// </summary>
		/// <param name="sender"><c><see cref="btn_Cancel"/></c></param>
		/// <param name="e"></param>
		void click_btnCancel(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Handles a click on the Select button. Passes the current strref to
		/// YataForm and closes this dialog.
		/// </summary>
		/// <param name="sender"><c><see cref="btn_Accept"/></c></param>
		/// <param name="e"></param>
		void click_btnSelect(object sender, EventArgs e)
		{
			if (_eId == TalkReader.invalid
				|| _dict.Count == 0			// -> talkfile not loaded, therefore user knows what he/she
				|| _dict.ContainsKey(_eId)	// is doing (ie, red panel BG) so let it go through.
				|| MessageBox.Show(this,
								   "Entry not found.",
								   " Bad Strref",
								   MessageBoxButtons.OKCancel,
								   MessageBoxIcon.Warning,
								   MessageBoxDefaultButton.Button2,
								   0) == DialogResult.OK)
			{
				if (_eId != TalkReader.invalid && cb_Custo.Checked)
					_eId |= TalkReader.bitCusto;

				_f._strref = _eId.ToString();
				Close();
			}
		}

		/// <summary>
		/// Handles a click on the Load ... button.
		/// </summary>
		/// <param name="sender"><c><see cref="btn_Load"/></c></param>
		/// <param name="e"></param>
		void click_btnLoad(object sender, EventArgs e)
		{
			if (!cb_Custo.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = " Select Dialog.Tlk";
					ofd.Filter = YataForm.GetTlkFilter();

					if (ofd.ShowDialog() == DialogResult.OK)
						TalkReader.Load(ofd.FileName, _f.it_PathTalkD);
				}

				lo = TalkReader.loDialo;
				hi = TalkReader.hiDialo;
			}
			else
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = " Select a TalkTable";
					ofd.Filter = YataForm.GetTlkFilter();

					if (ofd.ShowDialog() == DialogResult.OK)
						TalkReader.Load(ofd.FileName, _f.it_PathTalkC, true);
				}

				lo = TalkReader.loCusto;
				hi = TalkReader.hiCusto;
			}

			pokeUi(_dict.Count != 0);

			if (_dict.ContainsKey(_eId))
				rtb_Copyable.Text = _dict[_eId];
			else
				rtb_Copyable.Text = String.Empty;

			if (TalkReader.AltLabel != null)
				cb_Custo.Text = TalkReader.AltLabel;
			else
				cb_Custo.Text = "Custom";
		}


		/// <summary>
		/// Handles a click on the Prevert button. Reverts the strref to its
		/// initial val.
		/// </summary>
		/// <param name="sender"><c><see cref="btn_Prevert"/></c></param>
		/// <param name="e"></param>
		void click_btnPrevert(object sender, EventArgs e)
		{
			tb_Strref.Text = _eId_init.ToString();
		}

		/// <summary>
		/// Handles a click on the Pre button. Steps backward to the antecedent
		/// dialog-entry.
		/// </summary>
		/// <param name="sender"><c><see cref="btn_Backward"/></c></param>
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
		/// <param name="sender"><c><see cref="btn_Forward"/></c></param>
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
		/// Does a jig.
		/// </summary>
		/// <param name="sender"><c><see cref="cb_Custo"/></c></param>
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

				pokeUi(_dict.Count != 0);

				if (_dict.ContainsKey(_eId))
					rtb_Copyable.Text = _dict[_eId];
				else
					rtb_Copyable.Text = String.Empty;
			}
		}


		/// <summary>
		/// Draws a 1px border around the copyable-panel.
		/// </summary>
		/// <param name="sender"><c><see cref="pnl_Copyable"/></c></param>
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

		/// <summary>
		/// Sets UI elements depending on whether there are any entries in the
		/// current TalkTable's dictionary.
		/// </summary>
		/// <param name="enabled"><c>true</c> if the current dictionary has
		/// entries</param>
		void pokeUi(bool enabled)
		{
			Color color;
			if (btn_Backward.Enabled =
				btn_Forward .Enabled = enabled)
			{
				color = Colors.TalkfileLoaded;
			}
			else
				color = Colors.TalkfileLoaded_f;

			pnl_Copyable.BackColor =
			rtb_Copyable.BackColor = color;
		}
		#endregion Methods


		#region Windows Form Designer generated code
		Panel pnl_Head;
		RichTextBox rtb_Copyable;
		Panel pnl_Copyable;
		Button btn_Cancel;
		Button btn_Accept;
		Button btn_Load;
		Button btn_Forward;
		Button btn_Prevert;
		Button btn_Backward;
		TextBox tb_Strref;
		CheckBox cb_Custo;

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor. Or else POW! RIGHT IN THE KISSER
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
			this.btn_Accept = new System.Windows.Forms.Button();
			this.btn_Load = new System.Windows.Forms.Button();
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
			this.cb_Custo.Location = new System.Drawing.Point(170, 3);
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
			this.tb_Strref.Size = new System.Drawing.Size(66, 22);
			this.tb_Strref.TabIndex = 0;
			this.tb_Strref.Text = "-2";
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
			this.rtb_Copyable.Location = new System.Drawing.Point(6, 1);
			this.rtb_Copyable.Margin = new System.Windows.Forms.Padding(0);
			this.rtb_Copyable.Name = "rtb_Copyable";
			this.rtb_Copyable.ReadOnly = true;
			this.rtb_Copyable.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.rtb_Copyable.Size = new System.Drawing.Size(485, 107);
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
			this.pnl_Copyable.Padding = new System.Windows.Forms.Padding(6, 1, 1, 1);
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
			this.btn_Cancel.TabIndex = 4;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.click_btnCancel);
			// 
			// btn_Accept
			// 
			this.btn_Accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_Accept.Location = new System.Drawing.Point(310, 140);
			this.btn_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Accept.Name = "btn_Accept";
			this.btn_Accept.Size = new System.Drawing.Size(85, 30);
			this.btn_Accept.TabIndex = 3;
			this.btn_Accept.Text = "Accept";
			this.btn_Accept.UseVisualStyleBackColor = true;
			this.btn_Accept.Click += new System.EventHandler(this.click_btnSelect);
			// 
			// btn_Load
			// 
			this.btn_Load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btn_Load.Location = new System.Drawing.Point(5, 145);
			this.btn_Load.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Load.Name = "btn_Load";
			this.btn_Load.Size = new System.Drawing.Size(70, 25);
			this.btn_Load.TabIndex = 2;
			this.btn_Load.Text = "Load ...";
			this.btn_Load.UseVisualStyleBackColor = true;
			this.btn_Load.Click += new System.EventHandler(this.click_btnLoad);
			// 
			// TalkDialog
			// 
			this.AcceptButton = this.btn_Accept;
			this.AutoScroll = true;
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(492, 174);
			this.Controls.Add(this.btn_Load);
			this.Controls.Add(this.btn_Accept);
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
