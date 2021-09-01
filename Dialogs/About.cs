using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// The About box. Stops the dang beep.
	/// </summary>
	sealed class About
		: Form
	{
		#region cTor
		internal About()
		{
			InitializeComponent();

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			walkabout();
		}
		#endregion cTor


		#region Methods
		void walkabout()
		{
			var an = Assembly.GetExecutingAssembly().GetName();
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
			DateTime dt = Assembly.GetExecutingAssembly().GetLinkerTime();
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
		}
		#endregion Methods


		#region Designer
		Button btn_Close;
		Label la_Text;

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btn_Close = new System.Windows.Forms.Button();
			this.la_Text = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btn_Close
			// 
			this.btn_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Close.Location = new System.Drawing.Point(305, 173);
			this.btn_Close.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Close.Name = "btn_Close";
			this.btn_Close.Size = new System.Drawing.Size(75, 30);
			this.btn_Close.TabIndex = 0;
			this.btn_Close.Text = "heiL";
			this.btn_Close.UseVisualStyleBackColor = true;
			// 
			// la_Text
			// 
			this.la_Text.Location = new System.Drawing.Point(10, 10);
			this.la_Text.Margin = new System.Windows.Forms.Padding(0);
			this.la_Text.Name = "la_Text";
			this.la_Text.Size = new System.Drawing.Size(370, 160);
			this.la_Text.TabIndex = 1;
			// 
			// About
			// 
			this.AcceptButton = this.btn_Close;
			this.CancelButton = this.btn_Close;
			this.ClientSize = new System.Drawing.Size(384, 206);
			this.Controls.Add(this.la_Text);
			this.Controls.Add(this.btn_Close);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "About";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " yata - About";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}


	/// <summary>
	/// Lifted from StackOverflow.com:
	/// https://stackoverflow.com/questions/1600962/displaying-the-build-date#answer-1600990
	/// - what a fucking pain in the ass.
	/// </summary>
	static class DateTimeExtension
	{
		internal static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
		{
			var filePath = assembly.Location;
			const int c_PeHeaderOffset = 60;
			const int c_LinkerTimestampOffset = 8;

			var buffer = new byte[2048];

			using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				stream.Read(buffer, 0, 2048);

			var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
			var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			return epoch.AddSeconds(secondsSince1970);
/*			var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

			var tz = target ?? TimeZoneInfo.Local;
			var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

			return localTime; */
		}
	}
}
