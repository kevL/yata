using System;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed class SettingsEditor
		: YataDialog
	{
		#region cTor
		/// <summary>
		/// The editor for Settings.Cfg.
		/// </summary>
		/// <param name="f">pointer to <c><see cref="YataForm"/></c></param>
		/// <param name="lines">array of <c>strings</c> in user's current
		/// settings file</param>
		internal SettingsEditor(YataForm f, string[] lines)
		{
			_f = f;

			InitializeComponent();
			Initialize(rtb_Settings);

			if (lines.Length != 0)
			{
				var sb = new StringBuilder();
				for (int i = 0; i != lines.Length; ++i)
					sb.AppendLine(lines[i].Trim());

				rtb_Settings.Text = sb.ToString();
			}

			bu_Insert.Visible = CheckInsertVisible();

			rtb_Settings.Select();
			Show(_f); // Yata is owner.
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>FormClosing</c> eventhandler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			(_f as YataForm).CloseSettingsEditor();
			base.OnFormClosing(e);
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Writes file and closes this <c>SettingsEditor</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Okay"/></c></param>
		/// <param name="e"></param>
		void click_Okay(object sender, EventArgs e)
		{
			try
			{
				string pfeT = Path.Combine(Application.StartupPath, Settings.FE) + ".t";

				File.WriteAllText(pfeT, rtb_Settings.Text);

				if (File.Exists(pfeT))
				{
					string pfe = Path.Combine(Application.StartupPath, Settings.FE);

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


		/// <summary>
		/// Inserts any settings that are not found in the current Settings.Cfg
		/// file.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Insert"/></c></param>
		/// <param name="e"></param>
		void click_Insert(object sender, EventArgs e)
		{
			bu_Insert.Visible = false;

			string text = rtb_Settings.Text;

			bool found = false;

			string option;
			for (int i = 0; i != Settings.ids; ++i)
			{
				option = Settings.options[i];
				if (!text.Contains(option))
				{
					if (!found)
					{
						found = true;
						text += Environment.NewLine;
					}
					text += option + Environment.NewLine;
				}
			}
			rtb_Settings.Text = text;
			rtb_Settings.SelectionStart = rtb_Settings.Text.Length;
			rtb_Settings.Focus();
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Checks if the Insert button should be visible.
		/// </summary>
		/// <returns></returns>
		bool CheckInsertVisible()
		{
			if (Settings.options == null)
				Settings.CreateOptions();

			string text = rtb_Settings.Text;

			for (int i = 0; i != Settings.ids; ++i)
			{
				if (!text.Contains(Settings.options[i]))
					return true;
			}
			return false;
		}
		#endregion Methods



		#region Designer
		RichTextBox rtb_Settings;
		Panel pa_Buttons;
		Button bu_Cancel;
		Button bu_Okay;
		Button bu_Insert;

		private void InitializeComponent()
		{
			this.rtb_Settings = new System.Windows.Forms.RichTextBox();
			this.pa_Buttons = new System.Windows.Forms.Panel();
			this.bu_Insert = new System.Windows.Forms.Button();
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
			this.rtb_Settings.Size = new System.Drawing.Size(592, 389);
			this.rtb_Settings.TabIndex = 0;
			this.rtb_Settings.Text = "";
			this.rtb_Settings.WordWrap = false;
			// 
			// pa_Buttons
			// 
			this.pa_Buttons.Controls.Add(this.bu_Insert);
			this.pa_Buttons.Controls.Add(this.bu_Cancel);
			this.pa_Buttons.Controls.Add(this.bu_Okay);
			this.pa_Buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pa_Buttons.Location = new System.Drawing.Point(0, 389);
			this.pa_Buttons.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Buttons.Name = "pa_Buttons";
			this.pa_Buttons.Size = new System.Drawing.Size(592, 35);
			this.pa_Buttons.TabIndex = 1;
			// 
			// bu_Insert
			// 
			this.bu_Insert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Insert.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Insert.Location = new System.Drawing.Point(10, 5);
			this.bu_Insert.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Insert.Name = "bu_Insert";
			this.bu_Insert.Size = new System.Drawing.Size(75, 25);
			this.bu_Insert.TabIndex = 0;
			this.bu_Insert.Text = "UPDATE";
			this.bu_Insert.UseVisualStyleBackColor = true;
			this.bu_Insert.Click += new System.EventHandler(this.click_Insert);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(430, 5);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(75, 25);
			this.bu_Cancel.TabIndex = 1;
			this.bu_Cancel.Text = "no";
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
			this.bu_Okay.TabIndex = 2;
			this.bu_Okay.Text = "Save";
			this.bu_Okay.UseVisualStyleBackColor = true;
			this.bu_Okay.Click += new System.EventHandler(this.click_Okay);
			// 
			// SettingsEditor
			// 
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(592, 424);
			this.Controls.Add(this.rtb_Settings);
			this.Controls.Add(this.pa_Buttons);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.Name = "SettingsEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - Settings.Cfg";
			this.pa_Buttons.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
