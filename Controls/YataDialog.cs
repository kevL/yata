using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Base class for dialogs in Yata. Assigns fonts, tracks telemetry, defines
	/// cancel-handler, etc.
	/// </summary>
	public class YataDialog
		: Form
	{
		#region Fields
		/// <summary>
		/// The parent of this <c>YataDialog</c>.
		/// </summary>
		/// <remarks><c>_f</c> shall be set to a valid <c>Control</c>.</remarks>
		protected Control _f;

		/// <summary>
		/// A <c>List</c> of <c>TextBoxBases</c> that shall be initialized w/
		/// consistent values and behaviors.
		/// </summary>
		/// <remarks><c>_tbbs</c> is populated by
		/// <c><see cref="Initialize()">Initialize()</see></c> and can be empty.</remarks>
		internal IList<TextBoxBase> _tbbs = new List<TextBoxBase>();

		/// <summary>
		/// The type of telemetry that this <c>YataDialog</c> respects.
		/// <list type="bullet">
		/// <item><c><see cref="METRIC_NON"/></c> - do not respect telemetry</item>
		/// <item><c><see cref="METRIC_LOC"/></c> - respect location</item>
		/// <item><c><see cref="METRIC_FUL"/></c> - respect location and size</item>
		/// </list>
		/// </summary>
		/// <remarks>The value is set in
		/// <c><see cref="Initialize()">Initialize()</see></c>.</remarks>
		int Metric;

		/// <summary>
		/// Do not respect Telemetry. Location and Size is detered by the
		/// derived class.
		/// </summary>
		internal const int METRIC_NON = 0;

		/// <summary>
		/// Respect only location Telemetry. Size is detered by the derived
		/// class.
		/// </summary>
		internal const int METRIC_LOC = 1;

		/// <summary>
		/// Respect both location and size Telemetry.
		/// </summary>
		internal const int METRIC_FUL = 2;

/*		/// <summary>
		/// Bypasses setting <c><see cref="_w"/></c> and <c><see cref="_h"/></c>
		/// when this <c>FontDialog</c> instantiates. Otherwise when .net
		/// automatically fires the <c>Resize</c> event during instantiation the
		/// values get set in a way that renders the
		/// <c>ClientSize.Width/.Height</c> static metrics irrelevant. This is
		/// why I like Cherios!
		/// </summary>
		protected bool _init = true; */
		#endregion Fields


		#region Properties (static)
		// Static vars in a base class are NOT unique to each instantiation.
		// hence ->

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

		static Dictionary<object, bool> _maximized = new Dictionary<object, bool>();
		/// <summary>
		/// <c>Dictionary</c> that tracks if user has the dialog-type maximized.
		/// </summary>
		internal bool Maximized
		{
			get { return _maximized.ContainsKey(GetType()) && _maximized[GetType()]; }
			private set { _maximized[GetType()] = value; }
		}
		#endregion Properties (static)


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>Load</c> handler.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>The <c><see cref="DifferDialog"/></c> overrides
		/// <c>OnLoad()</c>.</remarks>
		protected override void OnLoad(EventArgs e)
		{
			if (!DesignMode) // else the Designer(s) bork out.
			{
				if (Metric != METRIC_NON)
				{
					if (_x == -1)
					{
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
				}

				RichTextBox rtb;
				foreach (var tbb in _tbbs)
				{
					if ((rtb = tbb as RichTextBox) != null)	// is RichTextBox ->
					{
						rtb.AutoWordSelection = false; // <- needs to be here not in the designer or cTor to work right.
					}
					else									// is TextBox ->
					{
						tbb.SelectionStart = 0;
						
						if (tbb.Multiline)
							tbb.SelectionLength = 0;
						else
							tbb.SelectionLength = tbb.Text.Length;
					}
				}
			}
		}

		/// <summary>
		/// Overrides the <c>FormClosing</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (Metric != METRIC_NON)
			{
				Maximized = WindowState == FormWindowState.Maximized;

//				_init = true;

				SuspendLayout();
				WindowState = FormWindowState.Normal;
				_x = Math.Max(0, Left);
				_y = Math.Max(0, Top);

				if (Metric == METRIC_FUL)
				{
					_w = ClientSize.Width;
					_h = ClientSize.Height;
				}
			}
			base.OnFormClosing(e);
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Closes this <c>YataDialog</c> harmlessly.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Used only for nonmodal dialogs. Use <c>DialogResult</c>
		/// instead if a dialog is modal.</remarks>
		protected void click_Cancel(object sender, EventArgs e)
		{
			Close();
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Forces <c>ClientSize</c> back to what it should be after
		/// <c>InitializeComponent()</c> runs. Also sets fonts.
		/// </summary>
		/// <param name="metric">the type of telemetry that this
		/// <c>YataDialog</c> respects
		/// <list type="bullet">
		/// <item><c><see cref="METRIC_NON"/></c> - do not store telemetry</item>
		/// <item><c><see cref="METRIC_LOC"/></c> - store location</item>
		/// <item><c><see cref="METRIC_FUL"/></c> - store location and size</item>
		/// </list></param>
		/// <param name="bypassColor"><c>true</c> to set the <c>TextBoxBase's</c>
		/// <c>BackColor</c> to the Yata-default.</param>
		/// <param name="bypassFont"><c>true</c> to bypass setting the dialog's
		/// <c>Font</c></param>
		protected void Initialize(int metric, bool bypassColor = false, bool bypassFont = false)
		{
			if ((Metric = metric) == METRIC_FUL && _w != -1)
				ClientSize = new Size(_w,_h); // foff .net

			PopulateTextboxbaseList(this);
			Settings.SetFonts(this, bypassColor, bypassFont);

//			_init = false;
		}

		/// <summary>
		/// Recursive funct that gets all <c>TextBoxBases</c> in a specified
		/// <c>Control</c>.
		/// </summary>
		/// <param name="f">a <c>Control</c> to investigate</param>
		/// <returns>a <c>List</c> of <c>TextBoxBases</c></returns>
		void PopulateTextboxbaseList(Control f)
		{
			TextBoxBase tbb;
			foreach (Control control in f.Controls)
			{
				if ((tbb = control as TextBoxBase) != null)
					_tbbs.Add(tbb);
				else
					PopulateTextboxbaseList(control);
			}
		}
		#endregion Methods
	}
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
