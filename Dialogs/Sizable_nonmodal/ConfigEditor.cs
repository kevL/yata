using System;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ConfigEditor
		: YataDialog
	{
		#region Fields
		/// <summary>
		/// <c>true</c> if invoked to edit Colors.Cfg - <c>false</c> if invoked
		/// to edit Settings.Cfg
		/// </summary>
		bool _isColors;
		#endregion Fields


		#region cTor
		/// <summary>
		/// The editor for Settings.Cfg and/or Colors.Cfg.
		/// </summary>
		/// <param name="f">pointer to <c><see cref="Yata"/></c></param>
		/// <param name="lines">array of <c>strings</c> in user's current
		/// settings file</param>
		/// <param name="isColors"><c>true</c> if editing Colors.Cfg</param>
		internal ConfigEditor(Yata f, string[] lines, bool isColors)
		{
			_f = f;
			_isColors = isColors;

			InitializeComponent();
			Initialize(METRIC_FUL);

			if (lines.Length != 0)
			{
				var sb = new StringBuilder();
				for (int i = 0; i != lines.Length; ++i)
					sb.AppendLine(lines[i].Trim());

				rt_Settings.Text = sb.ToString();
			}

			bu_Insert.Visible = CheckInsertVisible();

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
			(_f as Yata).CloseSettingsEditor(_isColors);
			base.OnFormClosing(e);
		}

		/// <summary>
		/// Saves current text to Settings.Cfg file when <c>[Ctrl+s]</c> is
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
		/// Writes file and closes this <c>ConfigEditor</c>.
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
				string pfeT = _isColors ? ColorOptions.FE : Settings.FE;
					   pfeT = Path.Combine(Application.StartupPath, pfeT) + ".t";

				File.WriteAllText(pfeT, rt_Settings.Text);

				if (File.Exists(pfeT))
				{
					string pfe = _isColors ? ColorOptions.FE : Settings.FE;
						   pfe = Path.Combine(Application.StartupPath, pfe);

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
			catch (Exception ex)
			{
				using (var ib = new Infobox(Infobox.Title_excep,
											"The config file could not be written to the application directory.",
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
		/// Inserts any settings that are not found in the current Settings.Cfg
		/// file or Colors.Cfg file.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Insert"/></c></param>
		/// <param name="e"></param>
		void click_Insert(object sender, EventArgs e)
		{
			bu_Insert.Visible = false;

			string text = rt_Settings.Text;

			bool found = false;

			if (_isColors)
			{
				string option;
				for (int i = 0; i != ColorOptions.ids; ++i)
				{
					if (!text.Contains(option = ColorOptions.options[i]))
					{
						if (!found)
						{
							found = true;
							text += Environment.NewLine;
						}
						text += option + Environment.NewLine;
					}
				}
			}
			else
			{
				string option;
				for (int i = 0; i != Settings.ids; ++i)
				{
					if (!text.Contains(option = Settings.options[i]))
					{
						if (!found)
						{
							found = true;
							text += Environment.NewLine;
						}
						text += option + Environment.NewLine;
					}
				}
			}

			rt_Settings.Text = text;
			rt_Settings.SelectionStart = rt_Settings.Text.Length;
			rt_Settings.Focus();
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Checks if the Insert button should be visible.
		/// </summary>
		/// <returns><c>true</c> if the button should be visible</returns>
		bool CheckInsertVisible()
		{
			if (_isColors)
			{
				if (ColorOptions.options == null)
					ColorOptions.CreateOptions();

				string text = rt_Settings.Text;

				for (int i = 0; i != ColorOptions.ids; ++i)
				{
					if (!text.Contains(ColorOptions.options[i]))
						return true;
				}
			}
			else
			{
				if (Settings.options == null)
					Settings.CreateOptions();

				string text = rt_Settings.Text;

				for (int i = 0; i != Settings.ids; ++i)
				{
					if (!text.Contains(Settings.options[i]))
						return true;
				}
			}
			return false;
		}
		#endregion Methods
	}
}
