using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ColorSelectorD
		: YataDialog
	{
		#region Fields (static)
		const double STEPS_SAT = 201.0; // one less than count because 0
		const double STEPS_VAL = 255.0; // one less than count because 0

		static Color Stored = Color.White;
		#endregion Fields (static)


		#region Fields
		int _x1, _y1; // tracker on the Colortable
		int _val;     // tracker on the Valslider

		double _hue;
		double _sat;

		bool _dragSlider;
		bool _dragTable;

		LinearGradientBrush _gradient; // a gradient brush for the Valslider panel
		Rectangle _valsliderhalfrect;

		string _pretext = String.Empty;
		bool _bypass; // bypasses the textchanged handler

		Panel _panel; // origin panel in 'ColorOptionsF'
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="panel"></param>
		/// <param name="title"></param>
		internal ColorSelectorD(ColorOptionsF f, Panel panel, string title)
		{
			_f = f;
			_panel = panel;
			Text = title;

			InitializeComponent();
			Initialize(METRIC_LOC);

			DrawRegulator.SetDoubleBuffered(pa_Valslider);

			CreateColortable();
			CreateValsliderGradient();

			init(_panel.BackColor);
			bu_Accept.Select();
		}

		/// <summary>
		/// Creates the Colortable and sets it as a background image.
		/// </summary>
		void CreateColortable()
		{
			var pic = new Bitmap(360, 202, PixelFormat.Format24bppRgb);
			BitmapData locked = pic.LockBits(new Rectangle(0,0, pic.Width, pic.Height),
											 ImageLockMode.WriteOnly,
											 pic.PixelFormat);
			unsafe
			{
				byte r,g,b;

				for (int y = 0; y != pic.Height; ++y)
				{
					byte* row = (byte*)locked.Scan0 + (y * locked.Stride);
					int offset = 0;

					for (int x = 0; x != pic.Width; ++x)
					{
						HsvToRgb((double)x, (double)y / STEPS_SAT, 1.0, out r, out g, out b);

						row[offset + 2] = r;
						row[offset + 1] = g;
						row[offset]     = b;

						offset += 3;
					}
				}
			}
			pic.UnlockBits(locked);

			pa_Colortable.BackgroundImage = pic;
		}

		/// <summary>
		/// Creates the Valslider gradient.
		/// </summary>
		void CreateValsliderGradient()
		{
			_valsliderhalfrect = new Rectangle(0,0, pa_Valslider.Width / 2, pa_Valslider.Height);
			_gradient = new LinearGradientBrush(new Point(0, 0),
												new Point(0, pa_Valslider.Height),
												Color.Black, Color.White);
		}

		/// <summary>
		/// Initializes various fields as well as <c><see cref="pa_Color"/></c>.
		/// </summary>
		/// <param name="color"></param>
		void init(Color color)
		{
			pa_Color   .BackColor =
			pa_Colorpre.BackColor = color;

			double val;
			ColorToHSV(color, out _hue, out _sat, out val);

			_val = (int)(val * STEPS_VAL);
			pa_Valslider.Invalidate();

			_x1 = (int)_hue;
			_y1 = (int)(_sat * STEPS_SAT);
			pa_Colortable.Invalidate();

			_bypass = true;
			tb_Red  .Text = color.R.ToString();
			tb_Green.Text = color.G.ToString();
			tb_Blue .Text = color.B.ToString();
			_bypass = false;
		}
		#endregion cTor


		#region Methods (static)
		/// <summary>
		/// Converts HSV values to RGB <c>Color</c>.
		/// </summary>
		/// <param name="hue">0d..360d - the colorwheel</param>
		/// <param name="sat">0d..1d - white to saturated</param>
		/// <param name="val">0d..1d - black to full color</param>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <remarks>https://stackoverflow.com/questions/359612/how-to-convert-rgb-color-to-hsv#answer-1626175<br/>
		/// https://stackoverflow.com/questions/31612232/color-table-algorithm#answer-31626758</remarks>
		static void HsvToRgb(double hue, double sat, double val, out byte r, out byte g, out byte b)
		{
			double huestep = hue / 60.0;
			double huestepfloor = Math.Floor(huestep);
			double f = huestep - huestepfloor;

			val *= 255.0;
			byte v = Convert.ToByte(val);
			byte p = Convert.ToByte(val * (1.0 - sat));
			byte q = Convert.ToByte(val * (1.0 - f * sat));
			byte t = Convert.ToByte(val * (1.0 - (1.0 - f) * sat));

			switch (Convert.ToInt32(huestepfloor) % 6)
			{
				case 0: r = v; g = t; b = p; return;
				case 1: r = q; g = v; b = p; return;
				case 2: r = p; g = v; b = t; return;
				case 3: r = p; g = q; b = v; return;
				case 4: r = t; g = p; b = v; return;
				case 5: r = v; g = p; b = q; return;
			}
			r = (byte)0; g = (byte)0; b = (byte)0; // stop .net whining <-
		}

		/// <summary>
		/// Converts RGB <c>Color</c> to HSV values.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="hue"></param>
		/// <param name="sat"></param>
		/// <param name="val"></param>
		/// <remarks>https://stackoverflow.com/questions/359612/how-to-convert-rgb-color-to-hsv#answer-1626175<br/>
		/// https://stackoverflow.com/questions/31612232/color-table-algorithm#answer-31626758</remarks>
		static void ColorToHSV(Color color, out double hue, out double sat, out double val)
		{
			int i = Math.Max(color.R, Math.Max(color.G, color.B));
			int j = Math.Min(color.R, Math.Min(color.G, color.B));

			hue = color.GetHue();
			sat = (i == 0) ? 0.0 : 1.0 - (1.0 * (double)j / (double)i);
			val = (double)i / 255.0;
		}
		#endregion Methods (static)


		#region Methods
		/// <summary>
		/// Prints the current color in the colorfield and textboxes.
		/// </summary>
		/// <param name="bypass"><c>true</c> to bypass firing textchanged</param>
		void PrintCurrentColor(bool bypass = false)
		{
			byte r,g,b;
			HsvToRgb(_hue, _sat, (double)_val / STEPS_VAL, out r, out g, out b);
			pa_Color.BackColor = Color.FromArgb(r,g,b);

			_bypass = bypass;
			tb_Red  .Text = r.ToString();
			tb_Green.Text = g.ToString();
			tb_Blue .Text = b.ToString();
			_bypass = false;
		}
		#endregion Methods


		#region handlers (color table)
		/// <summary>
		/// Paints crosshairs on the Colortable.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_Colortable"/></c></param>
		/// <param name="e"></param>
		void paint_Colortable(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, _x1 - 5, _y1, _x1 - 2, _y1);
			e.Graphics.DrawLine(Pens.Black, _x1 + 2, _y1, _x1 + 5, _y1);

			e.Graphics.DrawLine(Pens.Black, _x1, _y1 - 5, _x1, _y1 - 2);
			e.Graphics.DrawLine(Pens.Black, _x1, _y1 + 2, _x1, _y1 + 5);
		}

		/// <summary>
		/// Handles the <c>MouseDown</c> event on the Colortable.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_Colortable"/></c></param>
		/// <param name="e"></param>
		void mousedown_Colortable(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && ModifierKeys == Keys.None)
			{
				_x1 = e.X;
				_y1 = e.Y;

				_hue = (double)_x1;
				_sat = (double)_y1 / STEPS_SAT;

				PrintCurrentColor(true);

				pa_Colortable.Invalidate();

				_dragTable = true;
			}
		}

		/// <summary>
		/// Handles the <c>MouseMove</c> event on the Colortable.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_Colortable"/></c></param>
		/// <param name="e"></param>
		void mousemove_Colortable(object sender, MouseEventArgs e)
		{
			if (_dragTable)
			{
				_x1 = Math.Min(Math.Max(0, e.X), pa_Colortable.Width  - 1);
				_y1 = Math.Min(Math.Max(0, e.Y), pa_Colortable.Height - 1);

				_hue = (double)_x1;
				_sat = (double)_y1 / STEPS_SAT;

				PrintCurrentColor(true);

				pa_Colortable.Invalidate();
			}
		}

		/// <summary>
		/// Handles the <c>MouseUp</c> event on the Colortable.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_Colortable"/></c></param>
		/// <param name="e"></param>
		void mouseup_Colortable(object sender, MouseEventArgs e)
		{
			_dragTable = false;
		}
		#endregion handlers (color table)


		#region handlers (value slider)
		/// <summary>
		/// Paints the value-slider.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_Valslider"/></c></param>
		/// <param name="e"></param>
		void paint_Valslider(object sender, PaintEventArgs e)
		{
			e.Graphics.FillRectangle(_gradient, _valsliderhalfrect);
			e.Graphics.DrawLine(Pens.Black, pa_Valslider.Width / 2, _val, pa_Valslider.Width, _val);
		}

		/// <summary>
		/// Handles the <c>MouseDown</c> event on the Valslider.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_Valslider"/></c></param>
		/// <param name="e"></param>
		void mousedown_Valslider(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && ModifierKeys == Keys.None)
			{
				_val = e.Y;
				pa_Valslider.Invalidate();

				PrintCurrentColor(true);

				_dragSlider = true;
			}
		}

		/// <summary>
		/// Handles the <c>MouseMove</c> event on the Valslider.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_Valslider"/></c></param>
		/// <param name="e"></param>
		void mousemove_Valslider(object sender, MouseEventArgs e)
		{
			if (_dragSlider)
			{
				_val = Math.Min(Math.Max(0, e.Y), pa_Valslider.Height - 1);
				pa_Valslider.Invalidate();

				PrintCurrentColor(true);
			}
		}

		/// <summary>
		/// Handles the <c>MouseUp</c> event on the Valslider.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_Valslider"/></c></param>
		/// <param name="e"></param>
		void mouseup_Valslider(object sender, MouseEventArgs e)
		{
			_dragSlider = false;
		}
		#endregion handlers (value slider)


		#region handlers (textboxes)
		/// <summary>
		/// Handles the <c>TextChanged</c> event in the RBG fields.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="tb_Red"/></c></item>
		/// <item><c><see cref="tb_Green"/></c></item>
		/// <item><c><see cref="tb_Blue"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void textchanged_Rgb(object sender, EventArgs e)
		{
			if (!_bypass)
			{
				var tb = sender as TextBox;

				int result;
				if (tb.Text.Length != 0
					&& (!Int32.TryParse(tb.Text, out result)
						|| result < 0 || result > 255))
				{
					_bypass = true;
					tb.Text = _pretext; // recurse^
					_bypass = false;
				}
				else
				{
					_pretext = tb.Text;

					Color color = Color.FromArgb(Int32.Parse((tb_Red  .Text.Length == 0) ? "0" : tb_Red  .Text),
												 Int32.Parse((tb_Green.Text.Length == 0) ? "0" : tb_Green.Text),
												 Int32.Parse((tb_Blue .Text.Length == 0) ? "0" : tb_Blue .Text));
					pa_Color.BackColor = color;

					double val;
					ColorToHSV(color, out _hue, out _sat, out val);

					_val = (int)(val * STEPS_VAL);
					pa_Valslider.Invalidate();

					_x1 = (int)_hue;
					_y1 = (int)(_sat * STEPS_SAT);
					pa_Colortable.Invalidate();
				}
			}
		}

		/// <summary>
		/// Handles the <c>Leave</c> event in the RBG fields.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="tb_Red"/></c></item>
		/// <item><c><see cref="tb_Green"/></c></item>
		/// <item><c><see cref="tb_Blue"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void leave_Rgb(object sender, EventArgs e)
		{
			var tb = sender as TextBox;
			if (tb.Text.Length == 0)
			{
				_bypass = true;
				tb.Text = "0";
				_bypass = false;
			}
		}
		#endregion handlers (textboxes)


		#region handlers (buttons)
		/// <summary>
		/// Sets <c><see cref="_panel"/>.BackColor</c> to the current
		/// <c><see cref="pa_Color"/>.BackColor</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Accept"/></c></param>
		/// <param name="e"></param>
		void click_Accept(object sender, EventArgs e)
		{
			_panel.BackColor = pa_Color.BackColor;
		}
		#endregion handlers (buttons)


		#region handlers (panels)
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="pa_Color"/></c></item>
		/// <item><c><see cref="pa_Colorpre"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void mouseclick_Color(object sender, MouseEventArgs e)
		{
			if ((ModifierKeys & (Keys.Alt | Keys.Shift)) == Keys.None)
			{
				if ((ModifierKeys & Keys.Control) == Keys.None)
				{
					Color color = (sender as Panel).BackColor;

					if (e.Button == MouseButtons.Right)
					{
						if (sender == pa_Color)			// assign 'Stored' color to Color
						{
							pa_Color.BackColor = color = Stored;
						}
						else if (sender == pa_Colorpre)	// assign clicked color to Colorpre
						{
							pa_Color.BackColor = color;
						}
					}

					double val;
					ColorToHSV(color, out _hue, out _sat, out val);

					_val = (int)(val * STEPS_VAL);
					pa_Valslider.Invalidate();

					_x1 = (int)_hue;
					_y1 = (int)(_sat * STEPS_SAT);
					pa_Colortable.Invalidate();

					_bypass = true;
					tb_Red  .Text = color.R.ToString();
					tb_Green.Text = color.G.ToString();
					tb_Blue .Text = color.B.ToString();
					_bypass = false;
				}
				else // store the panel's color in 'Stored' ->
				{
					Stored = (sender as Panel).BackColor;

					using (var ib = new Infobox(Infobox.Title_infor,
												"Color stored."))
					{
						ib.ShowDialog(this);
					}
				}
			}
		}
		#endregion handlers (panels)


		#region Methods (override)
		/// <summary>
		/// Navigates the Colortable and/or Valsider by keyboard.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (!tb_Red.Focused && !tb_Green.Focused && !tb_Blue.Focused)
			{
				switch (keyData)
				{
					case Keys.Up:
						if (_y1 != 0)
						{
							--_y1;
							pa_Colortable.Invalidate();

							_sat = (double)_y1 / STEPS_SAT;

							PrintCurrentColor(true);
						}
						return true;

					case Keys.Down:
						if (_y1 != pa_Colortable.Height - 1)
						{
							++_y1;
							pa_Colortable.Invalidate();

							_sat = (double)_y1 / STEPS_SAT;

							PrintCurrentColor(true);
						}
						return true;

					case Keys.Left:
						if (_x1 != 0)
						{
							--_x1;
							pa_Colortable.Invalidate();

							_hue = (double)_x1;

							PrintCurrentColor(true);
						}
						return true;

					case Keys.Right:
						if (_x1 != pa_Colortable.Width - 1)
						{
							++_x1;
							pa_Colortable.Invalidate();

							_hue = (double)_x1;

							PrintCurrentColor(true);
						}
						return true;

					case Keys.Home:
						if (_x1 != 0 || _y1 != 0)
						{
							if (_x1 != 0) --_x1;
							if (_y1 != 0) --_y1;
							pa_Colortable.Invalidate();

							_hue = (double)_x1;
							_sat = (double)_y1 / STEPS_SAT;

							PrintCurrentColor(true);
						}
						return true;

					case Keys.End:
						if (_x1 != 0 || _y1 != pa_Colortable.Height - 1)
						{
							if (_x1 != 0) --_x1;
							if (_y1 != pa_Colortable.Height - 1) ++_y1;
							pa_Colortable.Invalidate();

							_hue = (double)_x1;
							_sat = (double)_y1 / STEPS_SAT;

							PrintCurrentColor(true);
						}
						return true;

					case Keys.PageUp:
						if (_x1 != pa_Colortable.Width - 1 || _y1 != 0)
						{
							if (_x1 != pa_Colortable.Width - 1) ++_x1;
							if (_y1 != 0) --_y1;
							pa_Colortable.Invalidate();

							_hue = (double)_x1;
							_sat = (double)_y1 / STEPS_SAT;

							PrintCurrentColor(true);
						}
						return true;

					case Keys.PageDown:
						if (_x1 != pa_Colortable.Width - 1 || _y1 != pa_Colortable.Height - 1)
						{
							if (_x1 != pa_Colortable.Width  - 1) ++_x1;
							if (_y1 != pa_Colortable.Height - 1) ++_y1;
							pa_Colortable.Invalidate();

							_hue = (double)_x1;
							_sat = (double)_y1 / STEPS_SAT;

							PrintCurrentColor(true);
						}
						return true;


					case Keys.Subtract:
						if (_val != 0)
						{
							--_val;
							pa_Valslider.Invalidate();

							PrintCurrentColor(true);
						}
						return true;

					case Keys.Add:
						if (_val != pa_Valslider.Height - 1)
						{
							++_val;
							pa_Valslider.Invalidate();

							PrintCurrentColor(true);
						}
						return true;
				}
			}
			else // is RGB textbox ->
			{
				switch (keyData)
				{
					case Keys.Enter:
					case Keys.Escape:
						bu_Accept.Select();
						return true;


					case Keys.Subtract:
					{
						TextBox tb;
						if      (tb_Red  .Focused) tb = tb_Red;
						else if (tb_Green.Focused) tb = tb_Green;
						else                       tb = tb_Blue; // tb_Blue.Focused

						if (tb.Text != "255")
							tb.Text = (Byte.Parse(tb.Text) + 1).ToString();

						return true;
					}

					case Keys.Add:
					{
						TextBox tb;
						if      (tb_Red  .Focused) tb = tb_Red;
						else if (tb_Green.Focused) tb = tb_Green;
						else                       tb = tb_Blue; // tb_Blue.Focused

						if (tb.Text.Length != 0 && tb.Text != "0")
							tb.Text = (Byte.Parse(tb.Text) - 1).ToString();

						return true;
					}

					case Keys.PageUp:
					{
						TextBox tb;
						if      (tb_Red  .Focused) tb = tb_Red;
						else if (tb_Green.Focused) tb = tb_Green;
						else                       tb = tb_Blue; // tb_Blue.Focused

						tb.Text = "255";

						return true;
					}

					case Keys.PageDown:
					{
						TextBox tb;
						if      (tb_Red  .Focused) tb = tb_Red;
						else if (tb_Green.Focused) tb = tb_Green;
						else                       tb = tb_Blue; // tb_Blue.Focused

						tb.Text = "0";

						return true;
					}
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
		#endregion Methods (override)
	}
}

/*		Color HsvToRgb(double hue, double sat, double val)
		{
			double huestep = hue / 60.0;
			double huestepfloor = Math.Floor(huestep);
			double f = huestep - huestepfloor;

			val *= 255.0;
			int v = Convert.ToInt32(val);
			int p = Convert.ToInt32(val * (1.0 - sat));
			int q = Convert.ToInt32(val * (1.0 - f * sat));
			int t = Convert.ToInt32(val * (1.0 - (1.0 - f) * sat));

			switch (Convert.ToInt32(huestepfloor) % 6)
			{
				case 0: return Color.FromArgb(255, v, t, p);
				case 1: return Color.FromArgb(255, q, v, p);
				case 2: return Color.FromArgb(255, p, v, t);
				case 3: return Color.FromArgb(255, p, q, v);
				case 4: return Color.FromArgb(255, t, p, v);
				case 5: return Color.FromArgb(255, v, p, q);

				default: return Color.FromArgb(255, v, p, q);
			}
		} */

/*		/// <summary>
		/// Creates the Valslider and sets it as a background image.
		/// </summary>
		void CreateValslider()
		{
			var pic = new Bitmap(50, 202, PixelFormat.Format32bppRgb);
			BitmapData locked = pic.LockBits(new Rectangle(0,0, pic.Width, pic.Height),
											 ImageLockMode.WriteOnly,
											 pic.PixelFormat);
			unsafe
			{
//				byte r,g,b;

				for (int y = 0; y != pic.Height; ++y)
				{
					byte* row = (byte*)locked.Scan0 + (y * locked.Stride);
					int offset = 0;

					for (int x = 0; x != pic.Width; ++x)
					{
						if (x < pic.Width / 2)
						{
							row[offset + 3] = (byte)255; // .net Panel does not respect transparency
							row[offset + 2] =
							row[offset + 1] =
							row[offset]     = (byte)(y * 255 / 201);
						}
						else
						{
							row[offset + 3] = (byte)0; // .net Panel does not respect transparency
							row[offset + 2] =
							row[offset + 1] =
							row[offset]     = (byte)255;
						}
						offset += 4;
					}
				}
			}
			pic.UnlockBits(locked);

			pa_Valslider.BackgroundImage = pic;
		} */
