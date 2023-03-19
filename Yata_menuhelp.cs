using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Fields
		/// <summary>
		/// The <c><see cref="ConfigEditor"/></c> editor for Settings.Cfg.
		/// </summary>
		ConfigEditor _foptions;

		/// <summary>
		/// The <c><see cref="ColorOptionsF"/></c> editor for Colors.Cfg.
		/// </summary>
		ColorOptionsF _fcolors;
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
			if (!File.Exists(pfe))
			{
				using (var ib = new Infobox(Infobox.Title_error,
											"ReadMe.txt was not found in the application directory.",
											null,
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
			else
				Process.Start(pfe);
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
		/// Handles it-click to open the <c><see cref="ConfigEditor"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Options"/></c></param>
		/// <param name="e"></param>
		void helpclick_Options(object sender, EventArgs e)
		{
			if (_foptions == null)
			{
				string pfe = Path.Combine(Application.StartupPath, Options.FE);

				if (!File.Exists(pfe))
				{
					const string head = "Settings.Cfg file does not exist in the application directory. Do you want to create one ...";

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

									if (Options.options == null)
										Options.CreateOptions();

									for (int i = 0; i != Options.ids; ++i)
										sw.WriteLine(Options.options[i]);
								}
							}
							catch (Exception ex)
							{
								using (var ibo = new Infobox(Infobox.Title_excep,
															"Settings.Cfg file could not be created in the application directory.",
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

						_foptions = new ConfigEditor(this, lines);
						it_Options.Checked = true;
					}
					catch (Exception ex)
					{
						// the stock MessageBox 'shall' be used if an exception is going to cause a CTD:
						// eg. a stock Font was disposed but the ConfigEditor needs it during its
						// initialization ... The app can't show a Yata-dialog in such a case; but the
						// stock MessageBox will pop up then ... CTD.

//						MessageBox.Show("The config file could not be read in the application directory."
//										+ Environment.NewLine + Environment.NewLine
//										+ ex);

						using (var ib = new Infobox(Infobox.Title_excep,
													"Settings.Cfg file could not be read in the application directory.",
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
				if (_foptions.WindowState == FormWindowState.Minimized)
				{
					if (_foptions.Maximized)
						_foptions.WindowState = FormWindowState.Maximized;
					else
						_foptions.WindowState = FormWindowState.Normal;
				}
				_foptions.BringToFront();
			}
		}

		/// <summary>
		/// Handles it-click to open <c><see cref="ColorOptionsF"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Colors"/></c></param>
		/// <param name="e"></param>
		void helpclick_ColorOptions(object sender, EventArgs e)
		{
			if (_fcolors == null)
			{
				_fcolors = new ColorOptionsF(this);
				it_Colors.Checked = true;
			}
			else
			{
				if (_fcolors.WindowState == FormWindowState.Minimized)
				{
					if (_fcolors.Maximized)
						_fcolors.WindowState = FormWindowState.Maximized;
					else
						_fcolors.WindowState = FormWindowState.Normal;
				}
				_fcolors.BringToFront();
			}
		}
		#endregion Handlers (help)


		#region Methods (help)
		/// <summary>
		/// Clears the check on <c><see cref="it_Options"/></c> and nulls
		/// <c><see cref="_foptions"/></c> when the
		/// <c><see cref="ConfigEditor"/></c> closes.
		/// </summary>
		internal void CloseOptionsEditor()
		{
			_foptions = null;
			it_Options.Checked = false;
		}

		/// <summary>
		/// Clears the check on <c><see cref="it_Colors"/></c> and nulls
		/// <c><see cref="_fcolors"/></c> when
		/// <c><see cref="ColorOptionsF"/></c> closes.
		/// </summary>
		internal void CloseColorOptions()
		{
			_fcolors = null;
			it_Colors.Checked = false;
		}
		#endregion Methods (help)
	}
}
