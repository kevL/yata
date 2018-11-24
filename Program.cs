using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			logfile.CreateLog(); // NOTE: The logfile works in debug-builds only.
			// To write a line to the logfile:
			// logfile.Log("what you want to know here");
			//
			// The logfile ought appear in the directory with the executable.


			if (args.Length == 0) // always start a new instance of Yata
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);	// ie, use GDI aka TextRenderer class. (perhaps, read:
																		// perhaps depends on the Control that's being drawn)
				Application.Run(new YataForm());
			}
			else
			{
				bool first;
				var supercalafragelisticexpialadoshus = new System.Threading.Mutex(true, "YataMutex", out first);
				if (first) // if not running start Yata with the arg
				{
					YataForm.pfe_load = args[0];

					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);	// ie, use GDI aka TextRenderer class. (perhaps, read:
																			// perhaps depends on the Control that's being drawn)
					Application.Run(new YataForm());
				}
				else // if Yata is running pass the arg to that instance
				{
					var current = Process.GetCurrentProcess();
					int id = current.Id;					

					Process[] processes = Process.GetProcesses();
					foreach (var process in processes)
					{
						if (process.Id != id && process.ProcessName == "yata")
						{
							// https://www.codeproject.com/Tips/1017834/How-to-Send-Data-from-One-Process-to-Another-in-Cs
							IntPtr ptrCopyData = IntPtr.Zero;
							try
							{
								string arg = args[0];

								// create the data structure and fill with data
								var copyData = new Crap.COPYDATASTRUCT();
								copyData.dwData = new IntPtr(2);	// just a number to identify the data type
								copyData.cbData = arg.Length + 1;	// one extra byte for the \0 character
								copyData.lpData = Marshal.StringToHGlobalAnsi(arg);

								// allocate memory for the data and copy
								ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
								Marshal.StructureToPtr(copyData, ptrCopyData, false);

								// send the message
								IntPtr ptrWnd = process.MainWindowHandle;
								Crap.SendMessage(ptrWnd, Crap.WM_COPYDATA, IntPtr.Zero, ptrCopyData);
							}
							catch (Exception ex)
							{
								MessageBox.Show(ex.ToString(),
												"Yata",
												MessageBoxButtons.OK,
												MessageBoxIcon.Error);
							}
							finally
							{
								// free the allocated memory after the control has been returned
								if (ptrCopyData != IntPtr.Zero)
									Marshal.FreeCoTaskMem(ptrCopyData);
							}

							break;
						}
					}
					Application.Exit();
				}
				YataForm.pfe_load = null;
			}
		}
	}
}
