using System;
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
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);	// ie, use GDI aka TextRenderer class. (perhaps, read:
			Application.Run(new YataForm());						// perhaps depends on the Control that's being drawn)
		}
	}
}
