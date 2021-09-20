﻿using System;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed partial class SettingsEditor
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
			Initialize(YataDialog.METRIC_FUL);

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
	}
}