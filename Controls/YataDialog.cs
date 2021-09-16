﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Base class for a nonmodal dialogs in Yata. Tracks telemetry, assigns
	/// fonts, defines cancel-handler, etc.
	/// </summary>
	public class YataDialog
		: Form
	{
		#region Properties (static)
		static Dictionary<object, int> x = new Dictionary<object, int>();
		/// <summary>
		/// <c>Dictionary</c> that holds x-locations of dialogs.
		/// </summary>
		protected int _x
		{
			get { return x.ContainsKey(GetType()) ? x[GetType()] : -1; }
			set { x[GetType()] = value; }
		}

		static Dictionary<object, int> y = new Dictionary<object, int>();
		/// <summary>
		/// <c>Dictionary</c> that holds y-locations of dialogs.
		/// </summary>
		protected int _y
		{
			get { return y.ContainsKey(GetType()) ? y[GetType()] : -1; }
			set { y[GetType()] = value; }
		}

		static Dictionary<object, int> w = new Dictionary<object, int>();
		/// <summary>
		/// <c>Dictionary</c> that holds client-widths of dialogs.
		/// </summary>
		int _w
		{
			get { return w.ContainsKey(GetType()) ? w[GetType()] : -1; }
			set { w[GetType()] = value; }
		}

		static Dictionary<object, int> h = new Dictionary<object, int>();
		/// <summary>
		/// <c>Dictionary</c> that holds client-heights of dialogs.
		/// </summary>
		int _h
		{
			get { return h.ContainsKey(GetType()) ? h[GetType()] : -1; }
			set { h[GetType()] = value; }
		}
		#endregion Properties (static)


		#region Fields
		protected YataForm _f;

		protected RichTextBox _rtb;

/*		/// <summary>
		/// Bypasses setting <c><see cref="_w"/></c> and <c><see cref="_h"/></c>
		/// when this <c>FontF</c> dialog instantiates. Otherwise when .net
		/// automatically fires the <c>Resize</c> event during instantiation the
		/// values get set in a way that renders the
		/// <c>ClientSize.Width/.Height</c> static metrics irrelevant. This is
		/// why I like Cherios!
		/// </summary>
		protected bool _init = true; */
		#endregion Fields


		#region Properties (static)
		/// <summary>
		/// Tracks if user has this <c>YataDialog</c> dialog maximized.
		/// </summary>
		internal static bool Maximized
		{ get; private set; }
		#endregion Properties (static)


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>Load</c> eventhandler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			if (!DesignMode) // else the Designer(s) bork out.
			{
//				Point loc = PointToScreen(new Point(_f.Left + _f.ClientSize.Width,
//													_f.Top  + 20));

				// NOTE: '_f.Top' does not include Yata's menubar, but does include its titlebar.
//				_y = loc.Y;

				if (_x == -1)
				{
//					Screen screen = Screen.FromControl(_f);
/*					Screen screen = Screen.FromPoint(new Point(Left, Top));
					if (screen.Bounds.Contains(new Point(loc.X + Width, loc.Y)))
					{
						_x = loc.X;
					}
					else if (screen.Bounds.Contains(new Point(_f.Left - Width, loc.Y)))
					{
						_x = _f.Left - Width;
					}
					else
						_x = loc.X - Width; */


					_x = Math.Max(0, _f.Left + 20);
					_y = Math.Max(0, _f.Top  + 20);
				}
				Left = _x;
				Top  = _y;

				Screen screen = Screen.FromPoint(new Point(Left, Top));
				if (screen.Bounds.Width < Left + Width) // TODO: decrease Width if this shifts the
					Left = screen.Bounds.Width - Width; // window off the left edge of the screen.

				if (screen.Bounds.Height < Top + Height) // TODO: decrease Height if this shifts the
					Top = screen.Bounds.Height - Height; // window off the top edge of the screen.

				if (Maximized)
					WindowState = FormWindowState.Maximized;

				_rtb.AutoWordSelection = false; // <- needs to be here not in the designer to work right.
				_rtb.Select();
			}
		}

		/// <summary>
		/// Overrides the <c>FormClosing</c> eventhandler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			Maximized = WindowState == FormWindowState.Maximized;

//			_init = true;

			SuspendLayout();
			WindowState = FormWindowState.Normal;
			_x = Math.Max(0, Left);
			_y = Math.Max(0, Top);
			_w = ClientSize.Width;
			_h = ClientSize.Height;

			base.OnFormClosing(e);
		}


//		[System.Runtime.InteropServices.DllImport("user32.dll")]
//		extern static int GetWindowLong(IntPtr hWnd, int index);
//		
//		public static bool VerticalScrollBarVisible(Control ctl)
//		{
//			int style = GetWindowLong(ctl.Handle, -16);
//			return (style & 0x200000) != 0;
//		}

//		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
//		static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
//		const int WM_VSCROLL = 277;
//		const int SB_PAGEBOTTOM = 7;
//
//		internal static void ScrollToBottom(TextBoxBase richTextBox)
//		{
//			SendMessage(richTextBox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
//			richTextBox.SelectionStart = richTextBox.Text.Length;
//		}

/*		/// <summary>
		/// Handles the <c>Resize</c> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e); // before cursor shenanigans

			if (!_init && WindowState != FormWindowState.Minimized
				&& VerticalScrollBarVisible(_rtb))
			{
				// If the vertical scrollbar is visible and user pulls the bottom of
				// the window down past the end of the text -> keep the last line of
				// the text snuggled against the bottom of the window. Thanks.
				//
				// The following code forces the scrollbar/text to re-layout which
				// is all that's needed to keep the last line snuggled against the
				// bottom of the control.

				int pos = _rtb.SelectionStart;
				int len = _rtb.SelectionLength;

				_rtb.SelectionStart  =
				_rtb.SelectionLength = 0;

				_rtb.SelectionStart  = pos;
				_rtb.SelectionLength = len;

//				ScrollToBottom(_rtb);
//				var isAtBottom = rt.GetPositionFromCharIndex(rt.Text.Length).Y < rt.Height;
			}
		} */
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Closes this <c>YataDialog</c> harmlessly.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void click_Cancel(object sender, EventArgs e)
		{
			Close();
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Forces <c>ClientSize</c> back to what it should be after
		/// <c>InitializeComponent()</c> runs. Also sets fonts and
		/// <c><see cref="_rtb"/></c>.
		/// </summary>
		protected void Initialize(RichTextBox rtb)
		{
			if (_w != -1) ClientSize = new Size(_w,_h); // foff .net

			Settings.SetFonts(this, _rtb = rtb);

//			_init = false;
		}
		#endregion Methods
	}
}