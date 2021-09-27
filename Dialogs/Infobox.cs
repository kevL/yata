using System;
using System.Collections.Generic;
using System.Drawing;
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

		const int w_Min = 345;
		const int h_Max = 470;
		#endregion Fields (static)


		#region Designer (workaround)
		/// <summary>
		/// Since the programmers of .net couldn't figure out that when you set
		/// a label's height to 0 and invisible it ought maintain a height of 0,
		/// I need to *not* instantiate said label unless it is required.
		/// </summary>
		/// <remarks>Don't forget to do null-checks.</remarks>
		Label lbl_Head;
		#endregion Designer (workaround)


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="title">a caption on the titlebar</param>
		/// <param name="head">info to be displayed with a proportional font -
		/// keep this brief</param>
		/// <param name="copyable">info to be displayed in a fixed-width font as
		/// readily copyable text</param>
		/// <param name="ibt">an <c><see cref="InfoboxType"/></c> to deter the
		/// head's backcolor - is valid only with head-text specified</param>
		/// <param name="buttons"><c><see cref="InfoboxButtons"/></c> to show</param>
		/// <remarks>Limit the length of 'head' to ~100 chars max or break it
		/// into lines with <c><see cref="SplitString()">SplitString()</see></c>
		/// if greater.</remarks>
		internal Infobox(
				string title,
				string head,
				string copyable = null,
				InfoboxType ibt = InfoboxType.Info,
				InfoboxButtons buttons = InfoboxButtons.Okay)
		{
			// TODO: Store static location and size of the Infobox (if shown non-modally).

			InitializeComponent();

			if (Settings._fonti != null)
			{
				Font.Dispose();
				Font = Settings._fonti;
			}

			if (Settings._fontf != null)
			{
				rt_Copyable.Font.Dispose();
				rt_Copyable.Font = Settings._fontf;
			}

			Text = title;

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

//				case InfoboxButtons.CancelOkay:
//					bu_Okay.Visible = true;
//					break;

//				case InfoboxButtons.CancelOkayRetry:
//					bu_Okay .Visible =
//					bu_Retry.Visible = true;
//					break;

//				case InfoboxButtons.CancelYesNo:
//					bu_Okay .Text = "yes";
//					bu_Retry.Text = "no";
//
//					bu_Okay .Visible =
//					bu_Retry.Visible = true;
//					break;
			}


			SuspendLayout();

			Size size;

			int width  = 0;
			int height = 0;

			int widthScroller = SystemInformation.VerticalScrollBarWidth;

			if (copyable != null) // deter total width based on longest copyable line
			{
				string[] lines = copyable.Split(gs.CRandorLF, StringSplitOptions.None);

				int test;
				foreach (var line in lines)
				{
					size = TextRenderer.MeasureText(line, rt_Copyable.Font);
					if ((test = size.Width) > width)
						width = test;
				}
				width += pa_Copyable.Padding.Horizontal + widthScroller;

				height = rt_Copyable.Font.Height;
				pa_Copyable.Height = (height - 1) * (lines.Length + 1) + pa_Copyable.Padding.Vertical;

				rt_Copyable.Text = copyable + Environment.NewLine; // add a blank line to bot of the copyable text.
			}
			else
			{
				pa_Copyable.Height =
				rt_Copyable.Height = 0;

				pa_Copyable.Visible =
				rt_Copyable.Visible = false;
			}

			if (width < w_Min)
				width = w_Min;


//			head = head.Trim();
//			if (!String.IsNullOrEmpty(head))
//			{
			lbl_Head = new Label();
			lbl_Head.Name      = "lbl_Head";
			lbl_Head.Dock      = DockStyle.Top;
			lbl_Head.Margin    = new Padding(0);
			lbl_Head.Padding   = new Padding(8,0,0,0);
			lbl_Head.TextAlign = ContentAlignment.MiddleLeft;
			lbl_Head.TabIndex  = 0;
			lbl_Head.Paint += OnPaintHead;
			Controls.Add(lbl_Head);

			switch (ibt)
			{
				case InfoboxType.Info:  lbl_Head.BackColor = Color.Lavender;   break;
				case InfoboxType.Warn:  lbl_Head.BackColor = Color.Moccasin;   break;
				case InfoboxType.Error: lbl_Head.BackColor = Color.SandyBrown; break;
			}

			size = TextRenderer.MeasureText((lbl_Head.Text = head), lbl_Head.Font);
			lbl_Head.Height = size.Height + 10;

			if (size.Width + lbl_Head.Padding.Horizontal + widthScroller > width)
				width = size.Width + lbl_Head.Padding.Horizontal + widthScroller;
//			}


			height = (lbl_Head != null ? lbl_Head.Height : 0)
				   + pa_Copyable.Height
				   + bu_Cancel  .Height + bu_Cancel.Margin.Vertical;

			if (height > h_Max)
			{
				pa_Copyable.Height -= height - h_Max;	// <- The only way that (height > h_Max) is
				height = h_Max;							// because 'pa_Copyable' contains a lot of text.
			}

			height += 1; // pad
			ClientSize  = new Size(width, height);
			MinimumSize = new Size(Width, Height);

			ResumeLayout();


			bu_Cancel.Select();
		}
		#endregion cTor


		#region Handlers (override)
		const int WS_HSCROLL = 0x00100000;
//		const int WS_VSCROLL = 0x00200000;
		[System.Runtime.InteropServices.DllImport("user32.dll")]
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
			rt_Copyable.AutoWordSelection = false;

			if (HoriScrollBarVisible(rt_Copyable))
			{
				int hScrolbar = SystemInformation.HorizontalScrollBarHeight;
				ClientSize  = new Size(ClientSize.Width, ClientSize.Height + hScrolbar);
				MinimumSize = new Size(Width, Height);
			}

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

			if (pa_Copyable.Visible)
			{
				pa_Copyable.Height = ClientSize.Height
								   - (lbl_Head != null ? lbl_Head.Height : 0)
								   - bu_Cancel.Height - bu_Cancel.Margin.Vertical;
				pa_Copyable.Invalidate();
			}

			int width = ClientSize.Width / 3 - 4; // ~2px padding on both sides of buttons

			bu_Retry.Width = bu_Okay.Width = bu_Cancel.Width = width;

			bu_Retry .Left =  4;
			bu_Okay  .Left =  7 + width;
			bu_Cancel.Left = 10 + width * 2;

			bu_Retry .Top =
			bu_Okay  .Top =
			bu_Cancel.Top = ClientSize.Height
						  - bu_Cancel .Height - bu_Cancel.Margin.Bottom;
		}

		/// <summary>
		/// Overrides the <c>Paint</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, 0,0, 0, Height - 1);
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
				&& !rt_Copyable.Focused)
			{
				e.SuppressKeyPress = true;
				ClipboardService.SetText(rt_Copyable.Text);
			}
			else
				base.OnKeyDown(e);
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Paints border lines left/top on the head.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnPaintHead(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, 0,0, 0, lbl_Head.Height - 1);
			e.Graphics.DrawLine(Pens.Black, 1,0, lbl_Head.Width - 1, 0);
		}

		/// <summary>
		/// Paints border lines left/top on the copyable panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnPaintPanel(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, 0,0, 0, pa_Copyable.Height - 1);
			e.Graphics.DrawLine(Pens.Black, 1,0, pa_Copyable.Width - 1, 0);
		}
		#endregion Handlers


		#region Methods (static)
		/// <summary>
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
		}
		#endregion Methods (static)
	}
}
