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
					logfile.Log("first");
					YataForm.pfe_load = args[0];

					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);	// ie, use GDI aka TextRenderer class. (perhaps, read:
																			// perhaps depends on the Control that's being drawn)
					Application.Run(new YataForm());
				}
				else // if Yata is running pass the arg to that instance
				{
					logfile.Log("NOT first");

					YataForm.pfe_load = args[0];
					logfile.Log("YataForm.pfe_load= " + YataForm.pfe_load);

					var current = Process.GetCurrentProcess();
					int id = current.Id;					

					Process[] processes = Process.GetProcesses();
					foreach (var process in processes)
					{
						logfile.Log(". process= " + process.Id + " : " + process.ProcessName);
						if (process.Id != id && process.ProcessName == "yata")
						{
							logfile.Log(". . process found");

							// https://www.codeproject.com/Tips/1017834/How-to-Send-Data-from-One-Process-to-Another-in-Cs
							IntPtr ptrCopyData = IntPtr.Zero;
							try
							{
								logfile.Log(". . try...");

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
								logfile.Log(". . ptrWnd= " + ptrWnd);

								Crap.SendMessage(ptrWnd, Crap.WM_COPYDATA, IntPtr.Zero, ptrCopyData);
							}
							catch (Exception ex)
							{
								logfile.Log(". . catch...");

								MessageBox.Show(ex.ToString(),
												"Yata",
												MessageBoxButtons.OK,
												MessageBoxIcon.Error);
							}
							finally
							{
								logfile.Log(". . finally...");

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
/*
static void Main(string[] args)
{
	Application.EnableVisualStyles();
	Application.SetCompatibleTextRenderingDefault(false);

	// Get the filename if it exists
	string fileName = null;
	if (args.Length == 1)
	   fileName = args[0];

	// If a mutex with the name below already exists, 
	// one instance of the application is already running 
	bool isNewInstance;
	Mutex singleMutex = new Mutex(true, "MyAppMutex", out isNewInstance);
	if (isNewInstance)
	{
		// Start the form with the file name as a parameter
		Form1 frm = new Form1(fileName);
		Application.Run(frm);
	}
	else
	{
		// Nothing to do here yet, but we can show a message box
		MessageBox.Show(fileName,
						"Received File Name",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information);

		string windowTitle = "SendMessage Demo";
		// Find the window with the name of the main form
		IntPtr ptrWnd = MessageHandler.FindWindow(null, windowTitle);
		if (ptrWnd == IntPtr.Zero)
		{
			MessageBox.Show(String.Format("No window found with the title {0}.", windowTitle),
							"SendMessage Demo",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error);
		}
		else
		{
			IntPtr ptrCopyData = IntPtr.Zero;
			try
			{
				// Create the data structure and fill with data
				Crap.COPYDATASTRUCT copyData = new Crap.COPYDATASTRUCT();
				copyData.dwData = new IntPtr(2);		// Just a number to identify the data type
				copyData.cbData = fileName.Length + 1;	// One extra byte for the \0 character
				copyData.lpData = Marshal.StringToHGlobalAnsi(fileName);
	
				// Allocate memory for the data and copy
				ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
				Marshal.StructureToPtr(copyData, ptrCopyData, false);
	
				// Send the message
				Crap.SendMessage(ptrWnd, MessageHandler.WM_COPYDATA, IntPtr.Zero, ptrCopyData);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "SendMessage Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				// Free the allocated memory after the control has been returned
				if (ptrCopyData != IntPtr.Zero)
					Marshal.FreeCoTaskMem(ptrCopyData);
			}
		}
	}
} */


/*	  bool blnCurrentlyRunning = false;

		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);


		if (!blnCurrentlyRunning)
		{
			Form = new frmMain();
			Application.Run(Form);
		}
		else
		{
			Application.Exit();
		} */
