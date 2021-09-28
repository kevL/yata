using System;
#if DEBUG
using System.IO;
using System.Windows.Forms;
#endif


namespace yata
{
	public static class logfile
	{
#if DEBUG
		const string Logfile = "logfile.txt";
#endif

		/// <summary>
		/// Creates a logfile (overwrites the previous logfile if it exists).
		/// </summary>
		public static void CreateLog()
		{
#if DEBUG
			string pfe = Path.Combine(Application.StartupPath, Logfile);
//			string pfe = Path.Combine(Application.StartupPath, "logfile" + System.Diagnostics.Process.GetCurrentProcess().Id + ".txt");
			using (var sw = new StreamWriter(File.Open(pfe,
													   FileMode.Create,
													   FileAccess.Write,
													   FileShare.None)))
			{}
#endif
		}

		/// <summary>
		/// Writes a line to the logfile.
		/// </summary>
		/// <param name="line">the line to write</param>
		public static void Log(string line = "")
		{
#if DEBUG
			string pfe = Path.Combine(Application.StartupPath, Logfile);
//			string pfe = Path.Combine(Application.StartupPath, "logfile" + System.Diagnostics.Process.GetCurrentProcess().Id + ".txt");
			using (var sw = new StreamWriter(File.Open(pfe,
													   FileMode.Append,
													   FileAccess.Write,
													   FileShare.None)))
			{
				sw.WriteLine(line);
			}
#endif
		}
	}
}
