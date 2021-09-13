using System;
using System.Drawing;
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
			this.Icon = global::yata.Properties.Resources.yata_icon;
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
}
