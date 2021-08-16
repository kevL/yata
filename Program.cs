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


			//string st = String.Empty; // debug ->
			//foreach (var arg in args)
			//	st += Environment.NewLine + arg;
			//logfile.Log("Main() args= " + st);

			//logfile.Log(Environment.NewLine + "--------");
			//string a = String.Empty;
			//for (int i = 0; i != args.Length; ++i)
			//	a += ";" + args[i] + ";";
			//logfile.Log("Main() args=" + a);


			Process proc = null;

			if (args.Length != 0) // else always start a new instance of Yata
			{
				var current  = Process.GetCurrentProcess();
				string label = current.ProcessName;

				//int id = current.Id;
				//logfile.Log(". current process= " + id + " : " + label + " h= " + current.Handle);

				DateTime dt = DateTime.Now, dtTest;
				//logfile.Log(". dt= " + dt.Minute + ":" + dt.Second + ":" + dt.Millisecond);

				Process[] processes = Process.GetProcesses();
				//logfile.Log(". qty processes= " + processes.Length);

				foreach (var process in processes)
				{
					//logfile.Log(". . process= " + process.Id + "\t: " + process.ProcessName);

					if (process.MainWindowHandle != IntPtr.Zero // NOTE: 'current' won't have a MainWindow (ie. bypass current proc.)
						&& process.ProcessName == label)
					{
						//logfile.Log(". . . found other Yata process");
						//logfile.Log(". . . h= " + process.Handle + " MainWindowHandle= " + process.MainWindowHandle);

						// find longest-running instance -> use it
						dtTest = process.StartTime;
						//logfile.Log(". . . dtTest= " + dtTest.Minute + ":" + dtTest.Second + ":" + dtTest.Millisecond);
						if (dtTest < dt)
						{
							//logfile.Log(". . . . is less");
							dt = dtTest;
							proc = process;
						}
						//logfile.Log(". . . dt= " + dt);
						//logfile.Log(". . . proc h= " + proc.MainWindowHandle);
					}
				}
			}

			if (proc != null)
			{
				//logfile.Log(". FOUND INSTANCE proc= " + proc.Id + " : " + proc.ProcessName);

				// https://www.codeproject.com/Tips/1017834/How-to-Send-Data-from-One-Process-to-Another-in-Cs
				IntPtr ptrCopyData = IntPtr.Zero;
				try
				{
					string arg = args[0];

					// create the data structure and fill with data
					var copyData = new Crap.COPYDATASTRUCT();
					copyData.dwData = new IntPtr(Crap.CopyDataStructType);	// just a number to identify the data type
					copyData.cbData = arg.Length + 1;						// one extra byte for the \0 character
					copyData.lpData = Marshal.StringToHGlobalAnsi(arg);

					// allocate memory for the data and copy
					ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
					Marshal.StructureToPtr(copyData, ptrCopyData, false);

					// send the message
					IntPtr ptrWnd = proc.MainWindowHandle;
					Crap.SendMessage(ptrWnd, Crap.WM_COPYDATA, IntPtr.Zero, ptrCopyData);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString(),
									" Yata",
									MessageBoxButtons.OK,
									MessageBoxIcon.Error);
				}
				finally
				{
					// free the allocated memory after execution has returned
					if (ptrCopyData != IntPtr.Zero)
						Marshal.FreeCoTaskMem(ptrCopyData);
				}
			}
			else
			{
				if (args.Length != 0)
				{
					//logfile.Log(". pass arg to new instance");
					YataForm.PfeLoad = args[0];
				}
				//else logfile.Log(". no args - start new instance");

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);	// ie, use GDI aka TextRenderer class. (perhaps, read:
																		// perhaps depends on the Control that's being drawn)
				Application.Run(new YataForm());

				YataForm.PfeLoad = null;
			}
		}
	}
}
