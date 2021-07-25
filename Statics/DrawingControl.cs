using System;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;

// https://stackoverflow.com/questions/118528/horrible-redraw-performance-of-the-datagridview-on-one-of-my-two-screens#answer-16625788

namespace yata
{
	static class DrawingControl
	{
		[DllImport("user32.dll")]
		internal static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

		const int WM_SETREDRAW = 11;

		/// <summary>
		/// Some controls, such as the <c>DataGridView</c>, do not allow setting
		/// the <c>DoubleBuffered</c> property. It is set as a protected
		/// property. This method is a work-around to allow setting it. Call
		/// this in the constructor just after <c>InitializeComponent()</c>.
		/// </summary>
		/// <param name="control">the <c>Control</c> on which to set
		/// <c>DoubleBuffered</c> <c>true</c></param>
		internal static void SetDoubleBuffered(object control)
		{
			// if not remote desktop session then enable double-buffering optimization
			if (!SystemInformation.TerminalServerSession)
			{
				// set instance non-public property with name "DoubleBuffered" to true
				typeof(Control).InvokeMember("DoubleBuffered",
											 BindingFlags.SetProperty | BindingFlags.Instance
											 						  | BindingFlags.NonPublic,
											 null,
											 control,
											 new object[] { true });
			}
		}

		/// <summary>
		/// Suspend drawing updates for the specified <c>IWin32Window</c>
		/// control. After the control has been updated call
		/// <c><see cref="ResumeDrawing()">ResumeDrawing()</see></c>.
		/// </summary>
		/// <param name="control">the control to suspend draw updates on</param>
		internal static void SuspendDrawing(IWin32Window control)
		{
			SendMessage(control.Handle, WM_SETREDRAW, false, 0);
		}

		/// <summary>
		/// Resume drawing updates for the specified <c>Control</c>.
		/// </summary>
		/// <param name="control">the <c>Control</c> to resume draw updates on</param>
		internal static void ResumeDrawing(Control control)
		{
			SendMessage(control.Handle, WM_SETREDRAW, true, 0);
			control.Refresh();
		}
	}
}
