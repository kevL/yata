using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A generic outputbox for Info/Warnings/Errors.
	/// </summary>
	/// <remarks>The point is to stop wrapping long path-strings like the stock
	/// .NET <c>MessageBox</c> does. And to stop beeps. And to make it look a
	/// bit nicer.</remarks>
	sealed partial class Infobox
		: Form
	{
		#region Fields (static)
		internal const string Title_infor = " info";
		internal const string Title_warni = " burp";
		internal const string Title_error = " aargh!";
		internal const string Title_excep = " Exception";
		internal const string Title_alert = " Alert"; // <- for save routines only.

		internal const string Title_succf = " Success";

		const int w_Min = 350;
		const int w_Max = 800;
		const int h_Max = 475;
		#endregion Fields (static)


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="title">a caption on the titlebar</param>
		/// <param name="head">info to be displayed with a proportional font -
		/// keep this fairly brief</param>
		/// <param name="copyable">info to be displayed in a fixed-width font as
		/// readily copyable text - can be <c>null</c></param>
		/// <param name="ibt">an <c><see cref="InfoboxType"/></c> to deter the
		/// backcolor of <c><see cref="la_head"/></c></param>
		/// <param name="buttons"><c><see cref="InfoboxButtons"/></c> to show to
		/// the user</param>
		internal Infobox(
				string title,
				string head,
				string copyable = null,
				InfoboxType ibt = InfoboxType.Info,
				InfoboxButtons buttons = InfoboxButtons.Okay)
		{
			// TODO: Store static location and size of the Infobox (if shown non-modally).

			InitializeComponent();

			if (copyable != null)
			{
				InitializePanel();
				pa_Copyable.BringToFront();
				YataDialog.SetTabs(rt_Copyable);

				if (Options._fontf != null)
				{
					rt_Copyable.Font.Dispose();
					rt_Copyable.Font = Options._fontf;
				}
			}
			else
				la_head.Dock = DockStyle.Fill;

			if (Options._fonti != null)
			{
				Font.Dispose();
				Font = Options._fonti;
			}


			Text = title;

			switch (ibt)
			{
				case InfoboxType.Info:    la_head.BackColor = Color.Lavender;   break;
				case InfoboxType.Warn:    la_head.BackColor = Color.Moccasin;   break;
				case InfoboxType.Error:   la_head.BackColor = Color.SandyBrown; break;
				case InfoboxType.Success: la_head.BackColor = Color.Chartreuse; break;
			}

			switch (buttons)
			{
				case InfoboxButtons.Okay:
					bu_Cancel.Text = "ok";
					break;

				case InfoboxButtons.CancelYes:
					bu_Okay.Text = "yes";
					bu_Okay.Visible = true;
					break;

				case InfoboxButtons.AbortLoadNext:
					bu_Cancel.Text = "abort";
					bu_Okay  .Text = "load";
					bu_Retry .Text = "next";

					bu_Okay .Visible =
					bu_Retry.Visible = true;
					break;

				case InfoboxButtons.Abort:
					bu_Cancel.Text = "abort";
					break;
			}


			SuspendLayout();

			int client_w = 0;
			int client_h = 0;

			int widthScroller = SystemInformation.VerticalScrollBarWidth;

			string[] lines;

			if (copyable != null)
			{
				lines = copyable.Split(gs.CRandorLF, StringSplitOptions.None);

				int test;
				foreach (var line in lines)
				{
					test = TextRenderer.MeasureText(line, rt_Copyable.Font).Width;
					if (test > client_w) client_w = test;
				}
				client_w += pa_Copyable.Padding.Horizontal + widthScroller;

				client_h = rt_Copyable.Font.Height + 1;
				pa_Copyable.Height = client_h * (lines.Length + 1)
								   + pa_Copyable.Padding.Vertical;

				rt_Copyable.Text = copyable + Environment.NewLine;
			}

			if      (client_w < w_Min) client_w = w_Min;
			else if (client_w > w_Max) client_w = w_Max;


			int lineshead;

			if (TextRenderer.MeasureText(head, la_head.Font).Width + la_head.Padding.Horizontal > client_w)
			{
				lines = SplitString(head, client_w - la_head.Padding.Horizontal, la_head.Font);

				lineshead = lines.Length;

				head = String.Empty;
				for (int i = 0; i != lineshead; ++i)
				{
					if (i != 0) head += Environment.NewLine;
					head += lines[i];
				}
			}
			else
				lineshead = 1;

			la_head.Text = head;

			client_h = la_head.Font.Height + 1;
			la_head.Height = client_h * lineshead + la_head.Padding.Vertical + 1;


			client_h = la_head.Height
					 + (pa_Copyable != null ? pa_Copyable.Height : 0)
					 + pa_buttons.Height;

			if (client_h > h_Max) client_h = h_Max;

			ClientSize  = new Size(client_w + 2, client_h + 1); // pad.
			MinimumSize = new Size(Width, Height);

			ResumeLayout();

			bu_Cancel.Select();
		}
		#endregion cTor


		#region Handlers (override)
		const int WS_HSCROLL = 0x00100000;
//		const int WS_VSCROLL = 0x00200000;
		[DllImport("user32.dll")]
		extern static int GetWindowLong(IntPtr hWnd, int index);
		static bool HoriScrollBarVisible(IWin32Window control)
		{
			int style = GetWindowLong(control.Handle, -16);
			return (style & WS_HSCROLL) != 0;
//			return (style & WS_VSCROLL) != 0;
		}
		/// <summary>
		/// Overrides the <c>Load</c> handler.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>AutoWordSelection needs to be done here not in the designer
		/// or cTor to work right.</remarks>
		protected override void OnLoad(EventArgs e)
		{
			if (pa_Copyable != null)
			{
				rt_Copyable.AutoWordSelection = false; // <- needs to be here not in the designer to work right.

				if (HoriScrollBarVisible(rt_Copyable))
				{
					int h = SystemInformation.HorizontalScrollBarHeight;
					ClientSize  = new Size(ClientSize.Width, ClientSize.Height + h);
					MinimumSize = new Size(Width, Height);
				}
			}

			if (   Yata.that == null
				|| Yata.that.WindowState == FormWindowState.Minimized)
			{
				CenterToScreen();
			}
			else
				CenterToParent();

//			base.OnLoad(e); // req'd for (StartPosition = FormStartPosition.CenterParent)
		}

		/// <summary>
		/// Overrides the <c>Resize</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			SuspendLayout();

			int width = ClientSize.Width / 3 - 4; // ~2px padding on both sides of buttons

			bu_Retry.Width = bu_Okay.Width = bu_Cancel.Width = width;

			bu_Retry .Left =  4;
			bu_Okay  .Left =  7 + width;
			bu_Cancel.Left = 10 + width * 2;

			ResumeLayout();
		}


		/// <summary>
		/// Copies the text of <c><see cref="rt_Copyable"/></c> to the Windows
		/// Clipboard iff <c>rt_Copyable</c> is not focused.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Requires <c>KeyPreview</c> <c>true</c>.</remarks>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyData == (Keys.Control | Keys.C)
				&& pa_Copyable != null
				&& !rt_Copyable.Focused)
			{
				e.SuppressKeyPress = true;
				ClipboardService.SetText(rt_Copyable.Text);
			}
			else
				base.OnKeyDown(e);
		}

		/// <summary>
		/// Updates the <c><see cref="ClipboardEditor"/></c> when
		/// <c>[Ctrl+c]</c> is released.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyData == (Keys.Control | Keys.C)
				&& Yata.that._fclip != null)
			{
//				e.SuppressKeyPress = true;
				Yata.that._fclip.click_Get(null, EventArgs.Empty);
			}
//			else
//				base.OnKeyUp(e);
		}


		/// <summary>
		/// Overrides the <c>Paint</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, 0,0, 0, Height - 1);
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Paints border lines left/top on the head.
		/// </summary>
		/// <param name="sender"><c><see cref="la_head"/></c></param>
		/// <param name="e"></param>
		void OnPaintHead(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, 0,0, 0, la_head.Height - 1);
			e.Graphics.DrawLine(Pens.Black, 1,0, la_head.Width - 1, 0);
		}

		/// <summary>
		/// Paints border lines left/top on the copyable panel.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_Copyable"/></c></param>
		/// <param name="e"></param>
		void OnPaintPanel(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, 0,0, 0, pa_Copyable.Height - 1);
			e.Graphics.DrawLine(Pens.Black, 1,0, pa_Copyable.Width - 1, 0);

//			e.Graphics.DrawLine(Pens.Blue, rt_Copyable.Left,  rt_Copyable.Top - 1,
//										   rt_Copyable.Right, rt_Copyable.Top - 1);
//			e.Graphics.DrawLine(Pens.Blue, rt_Copyable.Left,  rt_Copyable.Bottom,
//										   rt_Copyable.Right, rt_Copyable.Bottom);
//
//			e.Graphics.DrawLine(Pens.Red, rt_Copyable.Left - 1, rt_Copyable.Top,
//										  rt_Copyable.Left - 1, rt_Copyable.Top + rt_Copyable.Font.Height);
		}

		/// <summary>
		/// Paints border lines left/top on the buttons-panel.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_buttons"/></c></param>
		/// <param name="e"></param>
		void OnPaintBot(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, 0,0, 0, pa_buttons.Height - 1);

			if (pa_Copyable == null)
				e.Graphics.DrawLine(Pens.Black, 1,0, pa_buttons.Width - 1, 0);
		}
		#endregion Handlers


		#region Methods (static)
		/// <summary>
		/// Takes an input-string and splices it with newlines every width in
		/// pixels.
		/// </summary>
		/// <param name="text">input only a single trimmed sentence with no
		/// newlines and keep words shorter than width</param>
		/// <param name="width">desired width in pixels - lines of output shall
		/// not exceed width</param>
		/// <param name="font"></param>
		/// <returns>text split into lines no longer than width</returns>
		static string[] SplitString(string text, int width, Font font)
		{
			string[] words = text.Split(new []{ " " }, StringSplitOptions.RemoveEmptyEntries);

			var lines = new List<string>();

			var sb = new StringBuilder();

			foreach (var word in words)
			{
				if (TextRenderer.MeasureText(sb + word, font).Width > width)
				{
					sb.Length = sb.Length - 1; // delete " "
					lines.Add(sb.ToString());
					sb.Length = 0;
				}
				sb.Append(word + " ");
			}

			if (sb.Length != 0)
			{
				sb.Length = sb.Length - 1; // delete " "
				lines.Add(sb.ToString());
			}

			return lines.ToArray();
		}

/*		/// <summary>
		/// Takes an input-string and splices it with newlines every width in
		/// chars.
		/// </summary>
		/// <param name="text">input only a single trimmed sentence with no
		/// newlines and keep words shorter than width</param>
		/// <param name="width">desired width in chars - lines of output shall
		/// not exceed width</param>
		/// <returns>text split into lines no longer than width</returns>
		internal static string SplitString(string text, int width = 80)
		{
			string[] array = text.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries);
			var words = new List<string>(array);

			var sb = new StringBuilder();

			int tally = 0;

			string word;
			for (int i = 0; i != words.Count; ++i)
			{
				word = words[i];

				if (i == 0)
				{
					sb.Append(word);
					tally = word.Length;
				}
				else if (tally + word.Length < width - 1)
				{
					sb.Append(" " + word);
					tally += word.Length + 1;
				}
				else
				{
					sb.AppendLine();
					sb.Append(word);
					tally = word.Length;
				}
			}
			return sb.ToString();
		} */
		#endregion Methods (static)
	}
}
