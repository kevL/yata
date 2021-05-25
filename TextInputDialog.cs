using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog for cell-text entry.
	/// </summary>
	sealed class TextInputDialog
		:
			Form
	{
		#region Fields (static)
		const string head = "Apply sets the copy cell text and applies it to selected cells.";
		#endregion Fields (static)


		#region Fields
		YataForm _f;

		bool _readyTextchanged;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"></param>
		internal TextInputDialog(YataForm f)
		{
			InitializeComponent();

			_f = f;

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			if (Settings._fontf != null)
			{
				tb_Input.Font.Dispose();
				tb_Input.Font = Settings._fontfdialog;
			}

			tb_Input.BackColor = Colors.TextboxBackground;

			la_Head.Text = head;

			tb_Input.Text = _f._copytext;
			tb_Input.SelectionStart = 0;
			tb_Input.SelectionLength = tb_Input.Text.Length;
		}
		#endregion cTor


		#region Events
		/// <summary>
		/// Handles a click on the Cancel button. Closes this dialog harmlessly.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Cancel(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Handles a click on the Okay button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Okay(object sender, EventArgs e)
		{
			if (YataGrid.CheckTextEdit(tb_Input))
			{
				_readyTextchanged = true;
				la_Head.ForeColor = Color.Firebrick;
				la_Head.Text = "The text has changed.";
			}
			else
			{
				_f._copytext = tb_Input.Text;
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void textchanged_Input(object sender, EventArgs e)
		{
			if (_readyTextchanged)
			{
				_readyTextchanged = false;
				la_Head.ForeColor = SystemColors.WindowText;
				la_Head.Text = head;
			}
		}
		#endregion Events


		#region Windows Form Designer generated code
		TextBox tb_Input;
		Button bu_Cancel;
		Button bu_Okay;
		Label la_Head;

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tb_Input = new System.Windows.Forms.TextBox();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Okay = new System.Windows.Forms.Button();
			this.la_Head = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// tb_Input
			// 
			this.tb_Input.Dock = System.Windows.Forms.DockStyle.Top;
			this.tb_Input.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_Input.Location = new System.Drawing.Point(0, 0);
			this.tb_Input.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Input.Name = "tb_Input";
			this.tb_Input.Size = new System.Drawing.Size(292, 22);
			this.tb_Input.TabIndex = 0;
			this.tb_Input.TextChanged += new System.EventHandler(this.textchanged_Input);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(4, 60);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(135, 25);
			this.bu_Cancel.TabIndex = 1;
			this.bu_Cancel.Text = "Cancel";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.click_Cancel);
			// 
			// bu_Okay
			// 
			this.bu_Okay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Okay.Location = new System.Drawing.Point(154, 60);
			this.bu_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Okay.Name = "bu_Okay";
			this.bu_Okay.Size = new System.Drawing.Size(135, 25);
			this.bu_Okay.TabIndex = 2;
			this.bu_Okay.Text = "APPLY";
			this.bu_Okay.UseVisualStyleBackColor = true;
			this.bu_Okay.Click += new System.EventHandler(this.click_Okay);
			// 
			// la_Head
			// 
			this.la_Head.Dock = System.Windows.Forms.DockStyle.Top;
			this.la_Head.Location = new System.Drawing.Point(0, 22);
			this.la_Head.Margin = new System.Windows.Forms.Padding(0);
			this.la_Head.Name = "la_Head";
			this.la_Head.Size = new System.Drawing.Size(292, 35);
			this.la_Head.TabIndex = 3;
			this.la_Head.Text = "la_Head";
			this.la_Head.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TextInputDialog
			// 
			this.AcceptButton = this.bu_Okay;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(292, 89);
			this.Controls.Add(this.la_Head);
			this.Controls.Add(this.bu_Okay);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.tb_Input);
			this.MaximizeBox = false;
			this.Name = "TextInputDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Text input";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Windows Form Designer generated code
	}
}


