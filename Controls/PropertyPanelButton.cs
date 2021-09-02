using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// The button in the upper-right corner of Yata that opens/cycles the
	/// active table's <c><see cref="PropertyPanel"/></c>.
	/// </summary>
	sealed class PropertyPanelButton
		: Button
	{
		#region Fields (static)
		internal static int HEIGHT = 20;
		#endregion Fields (static)


		#region Fields
//		readonly Rectangle _rectBg;
		readonly Rectangle _rectGr;
		#endregion Fields


		#region Properties
		bool _depressed;
		internal void SetDepressed(bool depressed)
		{
			_depressed = depressed;
			Invalidate();
		}
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal PropertyPanelButton()
		{
			DrawingControl.SetDoubleBuffered(this);

			// NOTE: .NET is using the default vals for button's Width/Height
			// here. So set it explicitly.
			Width  = 30;
			Height = HEIGHT;

//			_rectBg = new Rectangle(0,0, Width, Height);
			_rectGr = new Rectangle(3,3, Width - 6, Height - 6);


			Name     = "btn_PropertyPanel";
			TabIndex = 4;
			TabStop  = false;

			Visible = false;

			Anchor = AnchorStyles.Top | AnchorStyles.Right;
			UseVisualStyleBackColor = true;

			Margin = new Padding(0);
		}
		#endregion cTor


		#region Events (override)
		/// <summary>
		/// Since right-click on a button does not visually depress it do this.
		/// </summary>
		/// <param name="pevent"></param>
		protected override void OnPaint(PaintEventArgs pevent)
		{
//			YataGrid.graphics = pevent.Graphics;
//			YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			// and you know it don't come easy - Ringo
//			YataGrid.graphics.FillRectangle(Brushes.PropanelButton, _rectBg);

			if (_depressed)
			{
				YataGrid.graphics = pevent.Graphics;
				YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				YataGrid.graphics.FillRectangle(Gradients.PropanelButton, _rectGr);

				var pen1 = Pens.Black;
				var pen2 = Pencils.DarkLine;

				YataGrid.graphics.DrawLine(pen1, 2, 2,          Width - 2, 2);			// hori top
				YataGrid.graphics.DrawLine(pen2, 2, 3,          Width - 2, 3);

//				YataGrid.graphics.DrawLine(pen2, 2, Height - 2, Width - 3, Height - 2);	// hori bot
				YataGrid.graphics.DrawLine(pen1, 2, Height - 1, Width - 2, Height - 1);

				YataGrid.graphics.DrawLine(pen1, 2, 2,          2, Height - 2);			// vert left
				YataGrid.graphics.DrawLine(pen2, 3, 2,          3, Height - 2);

//				YataGrid.graphics.DrawLine(pen2, Width - 2, 2,  Width - 2, Height - 2);	// vert right
				YataGrid.graphics.DrawLine(pen1, Width - 1, 2,  Width - 1, Height - 2);
			}
			else
				base.OnPaint(pevent);
		}
		#endregion Events (override)
	}
}
