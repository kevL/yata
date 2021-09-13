﻿using System;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed class SettingsEditor
		: Form
	{
		#region Fields (static)
		static int _x = -1;
		static int _y = -1;
		static int _w = -1;
		static int _h = -1;
		#endregion Fields (static)


		YataForm _f;


		#region cTor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="f"></param>
		/// <param name="lines"></param>
		internal SettingsEditor(YataForm f, string[] lines)
		{
			_f = f;

			InitializeComponent();

			rtb_Settings.BackColor = Colors.TextboxBackground;

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			if (Settings._fontf_tb != null)
			{
				rtb_Settings.Font.Dispose();
				rtb_Settings.Font = Settings._fontf_tb;
			}

			if (_x == -1) _x = _f.Left + 20;
			if (_y == -1) _y = _f.Top  + 20;

			Left = _x;
			Top  = _y;

			if (_w != -1) Width  = _w;
			if (_h != -1) Height = _h;


			if (lines.Length != 0)
			{
				var sb = new StringBuilder();
				for (int i = 0; i != lines.Length; ++i)
					sb.AppendLine(lines[i].Trim());

				rtb_Settings.Text = sb.ToString();
			}

			Show(); // no owner.
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>Load</c> eventhandler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			rtb_Settings.AutoWordSelection = false; // <- needs to be here not in the designer to work right.
			rtb_Settings.Select();
			rtb_Settings.SelectionStart = 0;
		}

		/// <summary>
		/// Overrides the <c>FormClosing</c> eventhandler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_f.CloseSettingsEditor();

			_x = Left;
			_y = Top;
			_w = Width;
			_h = Height;

			base.OnFormClosing(e);
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Closes this <c>SettingsEditor</c> harmlessly.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Cancel"/></c></param>
		/// <param name="e"></param>
		void click_Cancel(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Writes file and closes this <c>SettingsEditor</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Okay"/></c></param>
		/// <param name="e"></param>
		void click_Okay(object sender, EventArgs e)
		{
			try
			{
				string pfeT = Path.Combine(Application.StartupPath, "settings.cfg.t");

				File.WriteAllText(pfeT, rtb_Settings.Text);

				if (File.Exists(pfeT))
				{
					string pfe = Path.Combine(Application.StartupPath, "settings.cfg");

					File.Delete(pfe);
					File.Copy(pfeT, pfe);

					if (File.Exists(pfe))
					{
						File.Delete(pfeT);

						MessageBox.Show("Yata must be reloaded before changes take effect.",
										" Settings changed",
										MessageBoxButtons.OK,
										MessageBoxIcon.Information,
										MessageBoxDefaultButton.Button1,
										0);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("The Settings.cfg file could not be written."
							  + Environment.NewLine + Environment.NewLine
							  + ex,
								" Error",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error,
								MessageBoxDefaultButton.Button1,
								0);
			}
			finally
			{
				Close();
			}
		}
		#endregion Handlers



		#region Designer
		RichTextBox rtb_Settings;
		Panel pa_Buttons;
		Button bu_Cancel;
		Button bu_Okay;

		private void InitializeComponent()
		{
			this.rtb_Settings = new System.Windows.Forms.RichTextBox();
			this.pa_Buttons = new System.Windows.Forms.Panel();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Okay = new System.Windows.Forms.Button();
			this.pa_Buttons.SuspendLayout();
			this.SuspendLayout();
			// 
			// rtb_Settings
			// 
			this.rtb_Settings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb_Settings.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtb_Settings.HideSelection = false;
			this.rtb_Settings.Location = new System.Drawing.Point(0, 0);
			this.rtb_Settings.Margin = new System.Windows.Forms.Padding(0);
			this.rtb_Settings.Name = "rtb_Settings";
			this.rtb_Settings.Size = new System.Drawing.Size(592, 439);
			this.rtb_Settings.TabIndex = 0;
			this.rtb_Settings.Text = "";
			this.rtb_Settings.WordWrap = false;
			// 
			// pa_Buttons
			// 
			this.pa_Buttons.Controls.Add(this.bu_Cancel);
			this.pa_Buttons.Controls.Add(this.bu_Okay);
			this.pa_Buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pa_Buttons.Location = new System.Drawing.Point(0, 439);
			this.pa_Buttons.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Buttons.Name = "pa_Buttons";
			this.pa_Buttons.Size = new System.Drawing.Size(592, 35);
			this.pa_Buttons.TabIndex = 1;
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(430, 5);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(75, 25);
			this.bu_Cancel.TabIndex = 0;
			this.bu_Cancel.Text = "cancel";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			this.bu_Cancel.Click += new System.EventHandler(this.click_Cancel);
			// 
			// bu_Okay
			// 
			this.bu_Okay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Okay.Location = new System.Drawing.Point(511, 5);
			this.bu_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Okay.Name = "bu_Okay";
			this.bu_Okay.Size = new System.Drawing.Size(75, 25);
			this.bu_Okay.TabIndex = 1;
			this.bu_Okay.Text = "Okay";
			this.bu_Okay.UseVisualStyleBackColor = true;
			this.bu_Okay.Click += new System.EventHandler(this.click_Okay);
			// 
			// SettingsEditor
			// 
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(592, 474);
			this.Controls.Add(this.rtb_Settings);
			this.Controls.Add(this.pa_Buttons);
			this.Name = "SettingsEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Settings.Cfg";
			this.pa_Buttons.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}