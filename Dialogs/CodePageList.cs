using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed class CodePageList
		: Form
	{
		#region Fields (static)
		static int _x = -1, _y;
		static int _w = -1, _h;
		#endregion Fields (static)


		#region Fields
		CodePageDialog _cpd;
		#endregion Fields


		#region cTor
		internal CodePageList(CodePageDialog cpd)
		{
			_cpd = cpd;

			InitializeComponent();

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			if (Settings._fontf_tb != null)
			{
				tb_List.Font.Dispose();
				tb_List.Font = Settings._fontf_tb;
			}

			if (_x == -1)
			{
				_x = Math.Max(0, cpd.Left + 20);
				_y = Math.Max(0, cpd.Top  + 20);
			}

			Left = _x;
			Top  = _y;

			if (_w != -1)
				ClientSize = new Size(_w,_h);

			Screen screen = Screen.FromPoint(new Point(Left, Top));
			if (screen.Bounds.Width < Left + Width) // TODO: decrease Width if this shifts the
				Left = screen.Bounds.Width - Width; // window off the left edge of the screen.

			if (screen.Bounds.Height < Top + Height) // TODO: decrease Height if this shifts the
				Top = screen.Bounds.Height - Height; // window off the top edge of the screen.


			var sb = new StringBuilder();

			EncodingInfo[] encs = Encoding.GetEncodings();
			for (int i = 0; i != encs.Length; ++i)
			{
				if (i != 0) sb.AppendLine();

				sb.AppendLine(encs[i].Name);
				sb.AppendLine(encs[i].DisplayName);
				sb.AppendLine(encs[i].CodePage.ToString());
			}
			tb_List.Text = sb.ToString();

			tb_List.SelectionLength =
			tb_List.SelectionStart  = 0;

			Show(); // no owner.
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Handles the <c>FormClosing</c> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_cpd.CloseCodepageList();

			_x = Math.Max(0, Left);
			_y = Math.Max(0, Top);
			_w = ClientSize.Width;
			_h = ClientSize.Height;

			base.OnFormClosing(e);
		}


		/// <summary>
		/// Closes this <c>CodePageList</c> when <c>[Esc]</c> is pressed.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Do not use <c>KeyDown</c> because the event will repeat
		/// even if told to suppress itself here and
		/// <c><see cref="CodePageDialog"/></c> would then close also.
		/// 
		/// 
		/// Requires <c>KeyPreview</c>.</remarks>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
				Close();
		}
		#endregion Handlers (override)



		#region Designer
		TextBox tb_List;

		void InitializeComponent()
		{
			this.tb_List = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tb_List
			// 
			this.tb_List.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tb_List.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_List.Location = new System.Drawing.Point(0, 0);
			this.tb_List.Margin = new System.Windows.Forms.Padding(0);
			this.tb_List.Multiline = true;
			this.tb_List.Name = "tb_List";
			this.tb_List.ReadOnly = true;
			this.tb_List.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tb_List.Size = new System.Drawing.Size(312, 429);
			this.tb_List.TabIndex = 0;
			this.tb_List.WordWrap = false;
			// 
			// CodePageList
			// 
			this.ClientSize = new System.Drawing.Size(312, 429);
			this.Controls.Add(this.tb_List);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.Name = "CodePageList";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - .net Codepages";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
