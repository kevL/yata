using System;
using System.IO;


namespace yata
{
	public static class logfile
	{
#if DEBUG
		private const string Logfile = "logfile.txt";
#endif

		/// <summary>
		/// Creates a logfile (or cleans the old one if it exists).
		/// </summary>
		public static void CreateLog()
		{
#if DEBUG
			using (var sw = new StreamWriter(File.Open( // clean the old logfile if it exists
													Logfile,
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
		public static void Log(string line)
		{
#if DEBUG
			using (var sw = new StreamWriter(File.Open(
													Logfile,
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
