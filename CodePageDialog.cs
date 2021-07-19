using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed class CodePageDialog
		: Form
	{
		#region Properties
		internal CodePageList List
		{ get; set; }
		#endregion Properties


		#region cTor
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
		#endregion cTor


		#region Handlers
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
				tb_Codepage.SelectionStart = tb_Codepage.Text.Length;
			}
			else
			{
				int result;
				if (!Int32.TryParse(tb_Codepage.Text, out result)
					|| result < 0 || result > 65535)
				{
					tb_Codepage.Text = _pre.ToString(); // recurse
					tb_Codepage.SelectionStart = tb_Codepage.Text.Length;
				}
				else if (YataGrid.CheckCodepage(_pre = result))
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


		/// <summary>
		/// Sets the codepage to the user-machine's default characterset.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void bu_Default_click(object sender, EventArgs e)
		{
			tb_Codepage.Text = Encoding.GetEncoding(0).CodePage.ToString();
		}

		/// <summary>
		/// Sets the codepage to UTF-8.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void bu_Utf8_click(object sender, EventArgs e)
		{
			tb_Codepage.Text = Encoding.UTF8.CodePage.ToString();
		}

		/// <summary>
		/// Sets the codepage to the value that the user specified in
		/// Settings.Cfg.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void bu_Custom_click(object sender, EventArgs e)
		{
			tb_Codepage.Text = Settings._codepage.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void bu_List_click(object sender, EventArgs e)
		{
			if (List == null)
				List = new CodePageList(this);
			else
			{
				List.Close();
				List = null;
			}
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Gets the desired codepage as a string.
		/// </summary>
		/// <returns></returns>
		internal string GetCodePage()
		{
			return tb_Codepage.Text;
		}
		#endregion Methods


		#region Designer
		IContainer components;

		Label la_Head;
		TextBox tb_Codepage;
		Label la_CodepageInfo;
		Button bu_Default;
		Button bu_Utf8;
		Button bu_Custom;
		Button bu_List;

		Button bu_Cancel;
		Button bu_Accept;

		ToolTip toolTip1;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}


		/// <summary>
		/// 
		/// </summary>
		void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.la_Head = new System.Windows.Forms.Label();
			this.tb_Codepage = new System.Windows.Forms.TextBox();
			this.bu_Default = new System.Windows.Forms.Button();
			this.bu_Utf8 = new System.Windows.Forms.Button();
			this.bu_Custom = new System.Windows.Forms.Button();
			this.bu_List = new System.Windows.Forms.Button();
			this.la_CodepageInfo = new System.Windows.Forms.Label();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Accept = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// la_Head
			// 
			this.la_Head.Dock = System.Windows.Forms.DockStyle.Top;
			this.la_Head.Location = new System.Drawing.Point(0, 0);
			this.la_Head.Margin = new System.Windows.Forms.Padding(0);
			this.la_Head.Name = "la_Head";
			this.la_Head.Padding = new System.Windows.Forms.Padding(10, 7, 5, 0);
			this.la_Head.Size = new System.Drawing.Size(442, 75);
			this.la_Head.TabIndex = 0;
			this.la_Head.Text = "la_Head";
			// 
			// tb_Codepage
			// 
			this.tb_Codepage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_Codepage.Location = new System.Drawing.Point(10, 75);
			this.tb_Codepage.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Codepage.Name = "tb_Codepage";
			this.tb_Codepage.Size = new System.Drawing.Size(90, 22);
			this.tb_Codepage.TabIndex = 1;
			this.tb_Codepage.WordWrap = false;
			this.tb_Codepage.TextChanged += new System.EventHandler(this.tb_Codepage_textchanged);
			// 
			// bu_Default
			// 
			this.bu_Default.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Default.Location = new System.Drawing.Point(110, 75);
			this.bu_Default.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Default.Name = "bu_Default";
			this.bu_Default.Size = new System.Drawing.Size(75, 20);
			this.bu_Default.TabIndex = 2;
			this.bu_Default.Text = "default";
			this.toolTip1.SetToolTip(this.bu_Default, "Your machine\'s default codepage");
			this.bu_Default.UseVisualStyleBackColor = true;
			this.bu_Default.Click += new System.EventHandler(this.bu_Default_click);
			// 
			// bu_Utf8
			// 
			this.bu_Utf8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Utf8.Location = new System.Drawing.Point(190, 75);
			this.bu_Utf8.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Utf8.Name = "bu_Utf8";
			this.bu_Utf8.Size = new System.Drawing.Size(75, 20);
			this.bu_Utf8.TabIndex = 3;
			this.bu_Utf8.Text = "UTF-8";
			this.toolTip1.SetToolTip(this.bu_Utf8, "The UTF-8 codepage");
			this.bu_Utf8.UseVisualStyleBackColor = true;
			this.bu_Utf8.Click += new System.EventHandler(this.bu_Utf8_click);
			// 
			// bu_Custom
			// 
			this.bu_Custom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Custom.Location = new System.Drawing.Point(270, 75);
			this.bu_Custom.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Custom.Name = "bu_Custom";
			this.bu_Custom.Size = new System.Drawing.Size(75, 20);
			this.bu_Custom.TabIndex = 4;
			this.bu_Custom.Text = "custom";
			this.toolTip1.SetToolTip(this.bu_Custom, "The codepage in Settings.Cfg");
			this.bu_Custom.UseVisualStyleBackColor = true;
			this.bu_Custom.Click += new System.EventHandler(this.bu_Custom_click);
			// 
			// bu_List
			// 
			this.bu_List.Location = new System.Drawing.Point(358, 75);
			this.bu_List.Margin = new System.Windows.Forms.Padding(0);
			this.bu_List.Name = "bu_List";
			this.bu_List.Size = new System.Drawing.Size(75, 20);
			this.bu_List.TabIndex = 5;
			this.bu_List.Text = "List ...";
			this.toolTip1.SetToolTip(this.bu_List, "Shows a list of codepages supported by .NET");
			this.bu_List.UseVisualStyleBackColor = true;
			this.bu_List.Click += new System.EventHandler(this.bu_List_click);
			// 
			// la_CodepageInfo
			// 
			this.la_CodepageInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.la_CodepageInfo.Location = new System.Drawing.Point(10, 100);
			this.la_CodepageInfo.Margin = new System.Windows.Forms.Padding(0);
			this.la_CodepageInfo.Name = "la_CodepageInfo";
			this.la_CodepageInfo.Size = new System.Drawing.Size(430, 70);
			this.la_CodepageInfo.TabIndex = 6;
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
			this.bu_Cancel.TabIndex = 7;
			this.bu_Cancel.Text = "cancel";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			// 
			// bu_Accept
			// 
			this.bu_Accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Accept.Location = new System.Drawing.Point(364, 172);
			this.bu_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Accept.Name = "bu_Accept";
			this.bu_Accept.Size = new System.Drawing.Size(75, 25);
			this.bu_Accept.TabIndex = 8;
			this.bu_Accept.Text = "Accept";
			this.bu_Accept.UseVisualStyleBackColor = true;
			// 
			// CodePageDialog
			// 
			this.AcceptButton = this.bu_Accept;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(442, 199);
			this.Controls.Add(this.bu_List);
			this.Controls.Add(this.bu_Custom);
			this.Controls.Add(this.bu_Utf8);
			this.Controls.Add(this.bu_Default);
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
		#endregion Designer
	}
}
