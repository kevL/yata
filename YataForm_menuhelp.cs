using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	sealed partial class YataForm
	{
		#region Fields
		/// <summary>
		/// The <c><see cref="SettingsEditor"/></c> dialog/editor.
		/// </summary>
		SettingsEditor _fsettings;
		#endregion Fields


		#region Handlers (help)
		/// <summary>
		/// Handles it-click to open ReadMe.txt.
		/// </summary>
		/// <param name="sender"><c><see cref="it_ReadMe"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>Help|ReadMe.txt <c>[F1]</c></item>
		/// </list></remarks>
		void helpclick_Help(object sender, EventArgs e)
		{
			string pfe = Path.Combine(Application.StartupPath, "ReadMe.txt");
			if (File.Exists(pfe))
				Process.Start(pfe);
			else
			{
				using (var ib = new Infobox(Infobox.Title_error,
											"ReadMe.txt was not found in the application directory.",
											null,
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
		}

		/// <summary>
		/// Handles it-click to open the About box.
		/// </summary>
		/// <param name="sender"><c><see cref="it_About"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>Help|About</item>
		/// </list></remarks>
		void helpclick_About(object sender, EventArgs e)
		{
			using (var f = new About(this))
				f.ShowDialog(this);
		}


		/// <summary>
		/// Handles it-click to open the <c><see cref="SettingsEditor"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Settings"/></c></param>
		/// <param name="e"></param>
		void helpclick_Settings(object sender, EventArgs e)
		{
			if (_fsettings == null)
			{
				string pfe = Path.Combine(Application.StartupPath, Settings.FE);

				if (!File.Exists(pfe))
				{
					const string head = "a Settings.cfg file does not exist in the application directory. Do you want to create one ...";

					using (var ib = new Infobox(Infobox.Title_infor,
												head,
												null,
												InfoboxType.Info,
												InfoboxButtons.CancelYes))
					{
						if (ib.ShowDialog(this) == DialogResult.OK)
						{
							try
							{
								using (var sw = new StreamWriter(File.Open(pfe,
																		   FileMode.Create,
																		   FileAccess.Write,
																		   FileShare.None)))
								{
									sw.WriteLine("#Help|ReadMe.txt describes these settings.");

									if (Settings.options == null)
										Settings.CreateOptions();

									for (int i = 0; i != Settings.ids; ++i)
										sw.WriteLine(Settings.options[i]);
								}
							}
							catch (Exception ex)
							{
								using (var ibo = new Infobox(Infobox.Title_excep,
															"a Settings.cfg file could not be created in the application directory.",
															ex.ToString(),
															InfoboxType.Error))
								{
									ibo.ShowDialog(this);
								}
							}
						}
					}
				}

				if (File.Exists(pfe))
				{
					try
					{
						string[] lines = File.ReadAllLines(pfe);
						_fsettings = new SettingsEditor(this, lines);
						it_Settings.Checked = true;
					}
					catch (Exception ex)
					{
						// the stock MessageBox 'shall' be used if an exception is going to cause a CTD:
						// eg. a stock Font was disposed but the SettingsEditor needs it during its
						// initialization ... The app can't show a Yata-dialog in such a case; but the
						// stock MessageBox will pop up then ... CTD.

//						MessageBox.Show("The Settings.cfg file could not be read in the application directory."
//										+ Environment.NewLine + Environment.NewLine
//										+ ex);

						using (var ib = new Infobox(Infobox.Title_excep,
													"The Settings.cfg file could not be read in the application directory.",
													ex.ToString(),
													InfoboxType.Error))
						{
							ib.ShowDialog(this);
						}
					}
				}
			}
			else
			{
				if (_fsettings.WindowState == FormWindowState.Minimized)
				{
					if (_fsettings.Maximized)
						_fsettings.WindowState = FormWindowState.Maximized;
					else
						_fsettings.WindowState = FormWindowState.Normal;
				}
				_fsettings.BringToFront();
			}
		}
		#endregion Handlers (help)


		#region Methods (help)
		/// <summary>
		/// Clears the check on <c><see cref="it_Settings"/></c> and nulls
		/// <c><see cref="_fsettings"/></c> when the settings-editor closes.
		/// </summary>
		internal void CloseSettingsEditor()
		{
			_fsettings = null;
			it_Settings.Checked = false;
		}
		#endregion Methods (help)
	}
}
