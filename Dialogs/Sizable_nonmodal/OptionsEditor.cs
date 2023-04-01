using System;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed partial class OptionsEditor
		: YataDialog
	{
		#region cTor
		/// <summary>
		/// The editor for the Settings.Cfg file.
		/// </summary>
		/// <param name="f">pointer to <c><see cref="Yata"/></c></param>
		/// <param name="lines">array of <c>strings</c> in user's current
		/// settings file</param>
		internal OptionsEditor(Yata f, string[] lines)
		{
			_f = f;

			InitializeComponent();
			Initialize(METRIC_FUL);

			if (lines.Length != 0)
			{
				var sb = new StringBuilder();
				for (int i = 0; i != lines.Length; ++i)
					sb.AppendLine(lines[i].Trim());

				rt_Settings.Text = sb.ToString();
			}

			bu_Update.Visible = CheckUpdateVisible();

			rt_Settings.Select();
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
			(_f as Yata).CloseOptionsEditor();
			base.OnFormClosing(e);
		}

		/// <summary>
		/// Saves current text to the Settings.Cfg file when <c>[Ctrl+s]</c> is
		/// pressed.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Requires <c>KeyPreview</c> <c>true</c>.</remarks>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyData == (Keys.Control | Keys.S))
			{
				e.SuppressKeyPress = true;
				click_Okay(null, EventArgs.Empty);
			}
			else
				base.OnKeyDown(e);
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Saves current text to Settings.Cfg file.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Save(object sender, EventArgs e)
		{
			click_Okay(null, EventArgs.Empty);
		}

		/// <summary>
		/// Writes the Settings.Cfg file and closes this <c>OptionsEditor</c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="bu_Okay"/></c></item>
		/// <item><c>null</c> - <c><see cref="bu_Save"/></c></item>
		/// <item><c>null</c> - <c>[Ctrl+s]</c></item>
		/// </list></param>
		/// <param name="e"></param>
		void click_Okay(object sender, EventArgs e)
		{
			try
			{
				string pfeT = Path.Combine(Application.StartupPath, Options.FE) + ".t";

				File.WriteAllText(pfeT, rt_Settings.Text);

				if (File.Exists(pfeT))
				{
					string pfe = Path.Combine(Application.StartupPath, Options.FE);

					File.Delete(pfe);
					File.Copy(pfeT, pfe);

					if (File.Exists(pfe))
					{
						File.Delete(pfeT);

						using (var ib = new Infobox(Infobox.Title_infor,
													"Yata must be restarted for any changes to take effect.",
													null,
													InfoboxType.Warn))
						{
							ib.ShowDialog(this);
						}
					}
				}
			}
			catch (Exception ex) // handle locked etc. or file is flagged Readonly
			{
				using (var ib = new Infobox(Infobox.Title_excep,
											"Settings.Cfg file could not be written to the application directory.",
											ex.ToString(),
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
			finally
			{
				if (sender != null) Close();
			}
		}


		/// <summary>
		/// Updates any settings that are not found in the current Settings.Cfg
		/// file.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Update"/></c></param>
		/// <param name="e"></param>
		void click_Update(object sender, EventArgs e)
		{
			bu_Update.Visible = false;

			string text = rt_Settings.Text;

			bool found = false;

			string option;
			for (int i = 0; i != Options.ids; ++i)
			{
				if (!text.Contains(option = Options.options[i]))
				{
					if (!found)
					{
						found = true;
						text += Environment.NewLine;
					}
					text += option + Environment.NewLine;
				}
			}

			rt_Settings.Text = text;
			rt_Settings.SelectionStart = rt_Settings.Text.Length;
			rt_Settings.Focus();
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Checks if the Update button should be visible.
		/// </summary>
		/// <returns><c>true</c> if the button should be visible</returns>
		bool CheckUpdateVisible()
		{
			if (Options.options == null)
				Options.CreateOptions();

			string text = rt_Settings.Text;

			for (int i = 0; i != Options.ids; ++i)
			{
				if (!text.Contains(Options.options[i]))
					return true;
			}
			return false;
		}
		#endregion Methods
	}
}
