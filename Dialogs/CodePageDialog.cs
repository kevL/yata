﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed class CodePageDialog
		: Form
	{
		#region Fields (static)
		static int _x = -1, _y;
		static int _w = -1, _h;
		#endregion Fields (static)


		#region Fields
		CodePageList _cpList;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal CodePageDialog(YataForm f, Encoding enc)
		{
			InitializeComponent();

			tb_Codepage.BackColor = Colors.TextboxBackground;

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			if (Settings._fontf_tb != null)
			{
				tb_Codepage.Font.Dispose();
				tb_Codepage.Font = Settings._fontf_tb;
			}

			if (_x == -1)
			{
				_x = Math.Max(0, f.Left + 20);
				_y = Math.Max(0, f.Top  + 20);
			}

			Left = _x;
			Top  = _y;

			if (_w != -1)
				ClientSize = new Size(_w,_h);

			Screen screen = Screen.FromPoint(new Point(Left, Top));
			if (screen.Bounds.Width < Left + Width) // TODO: decrease Width if this shifts the
				Left = screen.Bounds.Width - Width; // window off the left edge of the screen.

			if (screen.Bounds.Height < Top + Height) // TODO: decrease Height if this shifts the
				Top = screen.Bounds.Height - Height; // window off the top edge of the screen.


			string head = "The 2da file appears to have ANSI encoding."
						+ " Please enter the codepage of its text.";

			if (enc == null)
			{
				head += Environment.NewLine + Environment.NewLine
					  + "The #codepage in Settings.Cfg is invalid.";

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


		#region Handlers (override)
		/// <summary>
		/// Handles the <c>FormClosing</c> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			WindowState = FormWindowState.Normal;
			_x = Math.Max(0, Left);
			_y = Math.Max(0, Top);
			_w = ClientSize.Width;
			_h = ClientSize.Height;

			base.OnFormClosing(e);
		}
		#endregion Handlers (override)


		#region Handlers
		int _pre;

		/// <summary>
		/// Handles textchanged in the Codepage textbox.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tb_Codepage_textchanged(object sender, EventArgs e)
		{
			Encoding enc = null;

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
					enc = Encoding.GetEncoding(_pre);

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
			bu_Accept.Enabled = (enc != null);
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
			if (_cpList == null)
				_cpList = new CodePageList(this);
			else
			{
				_cpList.Close();
				_cpList = null;
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

		/// <summary>
		/// 
		/// </summary>
		internal void CloseCodepageList()
		{
			_cpList = null;
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
			this.la_Head.Size = new System.Drawing.Size(427, 75);
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
			this.bu_Default.Location = new System.Drawing.Point(109, 75);
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
			this.bu_Utf8.Location = new System.Drawing.Point(188, 75);
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
			this.bu_Custom.Location = new System.Drawing.Point(267, 75);
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
			this.bu_List.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_List.Location = new System.Drawing.Point(346, 75);
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
			this.la_CodepageInfo.Size = new System.Drawing.Size(415, 70);
			this.la_CodepageInfo.TabIndex = 6;
			this.la_CodepageInfo.Text = "la_CodepageInfo";
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(271, 172);
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
			this.bu_Accept.Location = new System.Drawing.Point(349, 172);
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
			this.ClientSize = new System.Drawing.Size(427, 199);
			this.Controls.Add(this.bu_Custom);
			this.Controls.Add(this.bu_Utf8);
			this.Controls.Add(this.bu_Default);
			this.Controls.Add(this.la_Head);
			this.Controls.Add(this.tb_Codepage);
			this.Controls.Add(this.bu_List);
			this.Controls.Add(this.la_CodepageInfo);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.bu_Accept);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(435, 225);
			this.Name = "CodePageDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Choose codepage";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
