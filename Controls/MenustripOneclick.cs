using System;
using System.Windows.Forms;


namespace yata
{
	public sealed class MenustripOneclick
		: MenuStrip
	{
		#region Fields (static)
		const int WM_MOUSEACTIVATE = 0x21;
		#endregion Fields (static)


		#region Methods (override)
		/// <summary>
		/// Focuses this <c>MenuStrip</c> when a mouseclick on this
		/// <c>MenuStrip</c> activates its parent <c>Form</c> so that this
		/// <c>MenuStrip's</c> controls can receive the click immediately.
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_MOUSEACTIVATE) // && CanFocus && !Focused
				Focus();

			base.WndProc(ref m);
		}
		#endregion Methods (override)
	}
}
