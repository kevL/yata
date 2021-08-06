using System;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog for colhead-text entry.
	/// </summary>
	sealed class InputDialogColhead
		:
			Form
	{
		#region Fields (static)
		internal static string _text = String.Empty;
		#endregion Fields (static)


		#region Fields
		bool _bypasstextchanged;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal InputDialogColhead()
		{
			InitializeComponent();

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			if (Settings._fontf_tb != null)
			{
				tb_Input.Font.Dispose();
				tb_Input.Font = Settings._fontf_tb;
			}

			tb_Input.BackColor = Colors.TextboxBackground;

			tb_Input.Text = _text;
			tb_Input.SelectionStart = 0;
			tb_Input.SelectionLength = tb_Input.Text.Length;
		}
		#endregion cTor


		#region Events
		/// <summary>
		/// Handles text-input in the TextBox.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Col-head text shall be alphanumeric or underscore.</remarks>
		void textchanged_Input(object sender, EventArgs e)
		{
			if (!_bypasstextchanged)
			{
				if (tb_Input.Text.Length != 0)
				{
					for (int i = 0; i != tb_Input.Text.Length; ++i)
					{
						int ascii = tb_Input.Text[i];
						if (ascii != 95
							&& (    ascii < 48
								|| (ascii > 57 && ascii < 65)
								|| (ascii > 90 && ascii < 97)
								||  ascii > 122))
						{
							_bypasstextchanged = true;
							tb_Input.Text = _text; // recurse
							_bypasstextchanged = false;

							tb_Input.SelectionStart = tb_Input.Text.Length;
							return;
						}
					}
				}
				_text = tb_Input.Text;
			}
		}
		#endregion Events


		#region Designer
		TextBox tb_Input;
		Button bu_Cancel;
		Button bu_Okay;

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tb_Input = new System.Windows.Forms.TextBox();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Okay = new System.Windows.Forms.Button();
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
			this.bu_Cancel.Location = new System.Drawing.Point(4, 25);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(135, 25);
			this.bu_Cancel.TabIndex = 1;
			this.bu_Cancel.Text = "Cancel";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			// 
			// bu_Okay
			// 
			this.bu_Okay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Okay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Okay.Location = new System.Drawing.Point(154, 25);
			this.bu_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Okay.Name = "bu_Okay";
			this.bu_Okay.Size = new System.Drawing.Size(135, 25);
			this.bu_Okay.TabIndex = 2;
			this.bu_Okay.Text = "APPLY";
			this.bu_Okay.UseVisualStyleBackColor = true;
			// 
			// InputDialogColhead
			// 
			this.AcceptButton = this.bu_Okay;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(292, 54);
			this.Controls.Add(this.bu_Okay);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.tb_Input);
			this.MaximizeBox = false;
			this.Name = "InputDialogColhead";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Colhead text";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}


