using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// The About box. Stops the dang beep.
	/// </summary>
	sealed partial class About
		: YataDialog
	{
		#region cTor
		/// <summary>
		/// About dialog.
		/// </summary>
		/// <param name="f">parent</param>
		internal About(YataForm f)
		{
			_f = f;

			InitializeComponent();
			Initialize(YataDialog.METRIC_LOC);

			AssemblyName an = Assembly.GetExecutingAssembly().GetName();
			string ver = "Ver "
					   + an.Version.Major + "."
					   + an.Version.Minor + "."
					   + an.Version.Build + "."
					   + an.Version.Revision;
#if DEBUG
			ver += " - debug";
#else
			ver += " - release";
#endif
			DateTime dt = Assembly.GetExecutingAssembly().GetLinkerTime(true);

			ver += Environment.NewLine + Environment.NewLine
				 + String.Format(System.Globalization.CultureInfo.CurrentCulture,
								 "{0:yyyy MMM d}  {0:HH}:{0:mm}:{0:ss} UTC", // {0:zzz}
								 dt);

			ver += Environment.NewLine + Environment.NewLine
				 + "This is a derivative work of the guy who invented the wheel and"
				 + " that bloke who made the notches on a 40,000 year old piece of"
				 + " petrified wood that are thought to be the beginnings of"
				 + " mathematics. But I'd guess their copyrights are out of date.";

			ver += Environment.NewLine + Environment.NewLine
				 + "Executive Producer: Arnie the stuffed armadillo";

			la_Text.Text = ver;

			btn_Close.Select();
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Draws a nice border.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, 0,0, ClientSize.Width, 0);
			e.Graphics.DrawLine(Pens.Black, 0,0, 0, ClientSize.Height);
		}
		#endregion Handlers (override)
	}
}
