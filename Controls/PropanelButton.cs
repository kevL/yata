using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// The button in the upper-right corner of Yata that opens/cycles the
	/// active table's <c><see cref="Propanel"/></c>.
	/// </summary>
	sealed class PropanelButton
		: Button
	{
		#region Fields (static)
		internal static int HEIGHT = 21;

		static Pen pen1, pen2;
		#endregion Fields (static)


		#region Fields
//		readonly Rectangle _rectBg;
		readonly Rectangle _rectGr;

		bool _depressed;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal PropanelButton()
		{
			DrawRegulator.SetDoubleBuffered(this);

			// NOTE: .NET is using the default vals for button's Width/Height
			// here. So set it explicitly.
			Width  = 26;
			Height = HEIGHT;

			pen1 = Pens.Black;
			pen2 = Pencils.DarkLine;

//			_rectBg = new Rectangle(0,0, Width, Height);
			_rectGr = new Rectangle(3,3, Width - 6, Height - 6);


			Name    = "bu_Propanel";
			Visible = false;
			TabStop = false;

			Anchor = AnchorStyles.Top | AnchorStyles.Right;
			UseVisualStyleBackColor = true;

			Margin = new Padding(0);

			Image = global::yata.Properties.Resources.ppb;
		}
		#endregion cTor


		#region Methods
		/// <summary>
		/// Sets this <c>PropanelButton's</c> <c><see cref="_depressed"/></c>
		/// bool.
		/// </summary>
		/// <param name="true">I am depressed.</param>
		internal void depressed(bool @true)
		{
			_depressed = @true;
			Invalidate();
		}
		#endregion Methods


		#region Handlers (override)
		/// <summary>
		/// Since right-click on a button does not visually depress it do this.
		/// </summary>
		/// <param name="pevent"></param>
		protected override void OnPaint(PaintEventArgs pevent)
		{
			// and you know it don't come easy - Ringo
//			graphics.FillRectangle(Brushers.PropanelButton, _rectBg);

			if (_depressed)
			{
				Graphics graphics = pevent.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				graphics.FillRectangle(Gradients.ppBrush, _rectGr);

				graphics.DrawLine(pen1, 2,         2,          Width - 2, 2);			// hori top
				graphics.DrawLine(pen2, 2,         3,          Width - 2, 3);

//				graphics.DrawLine(pen2, 2,         Height - 2, Width - 3, Height - 2);	// hori bot
				graphics.DrawLine(pen1, 2,         Height - 1, Width - 2, Height - 1);

				graphics.DrawLine(pen1, 2,         2,          2,         Height - 2);	// vert left
				graphics.DrawLine(pen2, 3,         2,          3,         Height - 2);

//				graphics.DrawLine(pen2, Width - 2, 2,          Width - 2, Height - 2);	// vert right
				graphics.DrawLine(pen1, Width - 1, 2,          Width - 1, Height - 2);
			}
			else
				base.OnPaint(pevent);
		}


#if Keys
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			if ((e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("PropanelButton.OnPreviewKeyDown() e.KeyData= " + e.KeyData + " e.IsInputKey= " + e.IsInputKey);

			base.OnPreviewKeyDown(e);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("PropanelButton.ProcessCmdKey() keyData= " + keyData);

			bool ret = base.ProcessCmdKey(ref msg, keyData);
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". PropanelButton.ProcessCmdKey ret= " + ret);

			return ret;
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("PropanelButton.IsInputKey() keyData= " + keyData);

			bool ret = base.IsInputKey(keyData);
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". PropanelButton.IsInputKey ret= " + ret);

			return ret;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log("PropanelButton.ProcessDialogKey() keyData= " + keyData);

			bool ret = base.ProcessDialogKey(keyData);
			if ((keyData & ~gc.ControlShift) != 0)
				logfile.Log(". PropanelButton.ProcessDialogKey ret= " + ret);

			return ret;
		}

		protected override void OnKeyDown(KeyEventArgs kevent)
		{
			if ((kevent.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("PropanelButton.OnKeyDown() kevent.KeyData= " + kevent.KeyData);

			base.OnKeyDown(kevent);
		}
#endif
		#endregion Handlers (override)
	}
}
