using System;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed class CodePageDialog
		: Form
	{
		/// <summary>
		/// cTor.
		/// </summary>
		internal CodePageDialog(Encoding enc)
		{
			InitializeComponent();

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			if (Settings._fontf != null)
			{
				tb_Codepage.Font.Dispose();
				tb_Codepage.Font = Settings._fontfdialog;
			}

			tb_Codepage.BackColor = Colors.TextboxBackground;

			string head = "The 2da file appears to be in ANSI."
						+ " Please enter the encoding of its text.";

			if (enc == null)
			{
				head += Environment.NewLine + Environment.NewLine
					  + "The codepage in your Settings.Cfg is invalid.";

				enc = Encoding.GetEncoding(0);
			}

			la_Head.Text = head;

			_pre = enc.CodePage;
			tb_Codepage.Text = enc.CodePage.ToString();

			tb_Codepage.SelectionStart = 0;
			tb_Codepage.SelectionLength = tb_Codepage.Text.Length;

			tb_Codepage.Select();
		}
		// "The text encoding of the 2da file could not be determined. It
		// appears to contain characters that .NET cannot interpret accurately.
		// So my guess is that it was saved with an extended-ASCII
		// codepage/characterset. To prevent unrecognized characters getting
		// replaced by � ensure that your 2da files are encoded in UTF-8 or
		// ASCII before opening them in Yata."


		/// <summary>
		/// Accepts this dialog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void bu_Accept_click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		/// <summary>
		/// Cancels this dialog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void bu_Cancel_click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}


		int _pre;
		/// <summary>
		/// Handles textchanged in the Codepage textbox.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tb_Codepage_textchanged(object sender, EventArgs e)
		{
			if (tb_Codepage.Text.Length == 0)
			{
				la_CodepageInfo.Text = String.Empty;
				_pre = 0;
			}
			else if (tb_Codepage.Text.StartsWith("0", StringComparison.InvariantCulture))
			{
				tb_Codepage.Text = tb_Codepage.Text.Substring(1); // recurse
			}
			else
			{
				int result;
				if (!Int32.TryParse(tb_Codepage.Text, out result)
					|| result < 0 || result > 65535)
				{
					tb_Codepage.Text = _pre.ToString(); // recurse
				}
				else
				{
					_pre = Int32.Parse(tb_Codepage.Text);
					tb_Codepage.SelectionStart = tb_Codepage.Text.Length;

					if (YataGrid.CheckCodepage(_pre))
					{
						Encoding enc = Encoding.GetEncoding(_pre);

						la_CodepageInfo.ForeColor = Colors.Text;
						la_CodepageInfo.Text = enc.HeaderName   + Environment.NewLine
											 + enc.EncodingName + Environment.NewLine
											 + enc.CodePage;
					}
					else
					{
						la_CodepageInfo.ForeColor = Colors.TextReadonly;
						la_CodepageInfo.Text = "Codepage invalid.";
					}
				}
			}
		}

		/// <summary>
		/// Gets the desired codepage as a string.
		/// </summary>
		/// <returns></returns>
		internal string GetCodePage()
		{
			return tb_Codepage.Text;
		}


		private Label la_Head;
		private TextBox tb_Codepage;
		private Label la_CodepageInfo;
		private Button bu_Cancel;
		private Button bu_Accept;

		/// <summary>
		/// 
		/// </summary>
		void InitializeComponent()
		{
			this.la_Head = new System.Windows.Forms.Label();
			this.tb_Codepage = new System.Windows.Forms.TextBox();
			this.la_CodepageInfo = new System.Windows.Forms.Label();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Accept = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// la_Head
			// 
			this.la_Head.Dock = System.Windows.Forms.DockStyle.Top;
			this.la_Head.Location = new System.Drawing.Point(0, 0);
			this.la_Head.Margin = new System.Windows.Forms.Padding(0);
			this.la_Head.Name = "la_Head";
			this.la_Head.Padding = new System.Windows.Forms.Padding(10, 7, 3, 0);
			this.la_Head.Size = new System.Drawing.Size(442, 75);
			this.la_Head.TabIndex = 0;
			this.la_Head.Text = "la_Head";
			// 
			// tb_Codepage
			// 
			this.tb_Codepage.Location = new System.Drawing.Point(10, 75);
			this.tb_Codepage.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Codepage.Name = "tb_Codepage";
			this.tb_Codepage.Size = new System.Drawing.Size(90, 20);
			this.tb_Codepage.TabIndex = 1;
			this.tb_Codepage.WordWrap = false;
			this.tb_Codepage.TextChanged += new System.EventHandler(this.tb_Codepage_textchanged);
			// 
			// la_CodepageInfo
			// 
			this.la_CodepageInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.la_CodepageInfo.Location = new System.Drawing.Point(10, 100);
			this.la_CodepageInfo.Margin = new System.Windows.Forms.Padding(0);
			this.la_CodepageInfo.Name = "la_CodepageInfo";
			this.la_CodepageInfo.Size = new System.Drawing.Size(430, 70);
			this.la_CodepageInfo.TabIndex = 2;
			this.la_CodepageInfo.Text = "la_CodepageInfo";
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(286, 172);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(75, 25);
			this.bu_Cancel.TabIndex = 3;
			this.bu_Cancel.Text = "cancel";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.bu_Cancel_click);
			// 
			// bu_Accept
			// 
			this.bu_Accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Accept.Location = new System.Drawing.Point(364, 172);
			this.bu_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Accept.Name = "bu_Accept";
			this.bu_Accept.Size = new System.Drawing.Size(75, 25);
			this.bu_Accept.TabIndex = 4;
			this.bu_Accept.Text = "Accept";
			this.bu_Accept.UseVisualStyleBackColor = true;
			this.bu_Accept.Click += new System.EventHandler(this.bu_Accept_click);
			// 
			// CodePageDialog
			// 
			this.AcceptButton = this.bu_Accept;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(442, 199);
			this.Controls.Add(this.la_Head);
			this.Controls.Add(this.tb_Codepage);
			this.Controls.Add(this.la_CodepageInfo);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.bu_Accept);
			this.MaximizeBox = false;
			this.Name = "CodePageDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Choose a codepage";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
