using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using yata.Properties;


namespace yata
{
	/// <summary>
	/// Handles painting various controls on a YataGrid.
	/// </summary>
	sealed partial class YataGrid
	{
		/// <summary>
		/// Handles the Paint event for the table itself.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!_init && RowCount != 0)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

//				ControlPaint.DrawBorder3D(graphics, ClientRectangle, Border3DStyle.Etched);

				int r,c;

				// NOTE: Paint backgrounds full-height/width of table ->

				// rows background - scrollable
				var rect = new Rectangle(Left, HeightColhead - offsetVert, WidthTable, HeightRow);

				for (r = 0; r != RowCount; ++r)
				{
					graphics.FillRectangle(Rows[r]._brush, rect);
					rect.Y += HeightRow;
				}

				// cell text - scrollable
				Row row;

				rect.Height = HeightRow;
				int left = WidthRowhead - offsetHori + _padHori - 1; // NOTE: -1 is a padding tweak.

				int yOffset = HeightColhead - offsetVert;
				for (r = offsetVert / HeightRow; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r + yOffset) > Bottom)
						break;

					rect.X = left;

					row = Rows[r];
					for (c = 0; c != ColCount; ++c)
					{
						if (rect.X + (rect.Width = Cols[c].width()) > WidthRowhead)
						{
							var cell = row[c];
							if (cell.selected)
							{
								rect.X -= _padHori;

								if (_editor.Visible && _editcell == cell)
									graphics.FillRectangle(Brushes.Editor, rect);
								else
									graphics.FillRectangle(Brushes.Selected, rect);

								rect.X += _padHori;
							}
							else if (cell.loadchanged)
							{
								rect.X -= _padHori;
								graphics.FillRectangle(Brushes.LoadChanged, rect);
								rect.X += _padHori;
							}

							rect.Width -= _padHori;
							TextRenderer.DrawText(graphics, cell.text, Font, rect, Colors.Text, YataGraphics.flags);
							rect.Width += _padHori;
						}

						if ((rect.X += rect.Width) > Right)
							break;
					}
				}


//				using (var pi = new Bitmap(_bluePi, new Size(WidthTable, HeightColhead)))
//				if (_piColhead != null) graphics_.DrawImage(_piColhead, 0,0);

//				using (var pi = new Bitmap(_bluePi, new Size(WidthRowhead, HeightTable)))
//				if (_piRowhead != null) graphics_.DrawImage(_piRowhead, 0,0);


				// NOTE: Paint horizontal lines full-width of table.

				// row lines - scrollable
				int y;
				for (r = 1; r != RowCount + 1; ++r)
				{
					if ((y = HeightRow * r + yOffset) > Bottom)
						break;

					if (y > HeightColhead)
						graphics.DrawLine(Pens.DarkLine,
										  Left,       y,
										  WidthTable, y);
				}


				// NOTE: Paint vertical lines full-height of table.

				// col lines - scrollable
				int x = WidthRowhead - offsetHori;
				for (c = 0; c != ColCount; ++c)
				{
					if ((x += Cols[c].width()) > Right)
						break;

					if (x > WidthRowhead)
						graphics.DrawLine(Pens.DarkLine,
										  x, Top,
										  x, Bottom);
				}
			}

//			base.OnPaint(e);
		}

		/// <summary>
		/// Labels the colheads.
		/// @note Called by OnPaint of the top-panel.
		/// @note OnPaint() doesn't want to use the class_var '_graphics'.
		/// </summary>
		internal void LabelColheads()
		{
			if (ColCount != 0) // safety.
			{
				var rect = new Rectangle(WidthRowhead - offsetHori + _padHori, 0, 0, HeightColhead);

				int clip;
				Color color;

				for (int c = 0; c != ColCount; ++c)
				{
					if (rect.X + (rect.Width = Cols[c].width()) > Left)
					{
						if (_sortdir != SORT_NOT && c == _sortcol)
						{
							Bitmap sort;
							if (_sortdir == SORT_ASC)
								sort = Resources.asc_16px;
							else
								sort = Resources.des_16px;

							graphics.DrawImage(sort,
											   rect.X + rect.Width  - _offsetHoriSort,
											   rect.Y + rect.Height - _offsetVertSort);

							clip = _offsetHoriSort - 1;
						}
						else
							clip = 7;

						if (Cols[c].UserSized)
							color = Colors.TextColSized;
						else
							color = Colors.Text;

						rect.Width -= clip;
						TextRenderer.DrawText(graphics, Cols[c].text, _f.FontAccent, rect, color, YataGraphics.flags);
						rect.Width += clip;
					}

					if ((rect.X += rect.Width) > Right)
						break;
				}
			}
		}

		/// <summary>
		/// Labels the rowheads.
		/// @note Called by OnPaint of the left-panel.
		/// @note OnPaint() doesn't want to use the class_var '_graphics'.
		/// </summary>
		internal void LabelRowheads()
		{
			if (RowCount != 0) // safety - ought be checked in calling funct.
			{
				var rect = new Rectangle(_padHoriRowhead - 1, 0, WidthRowhead - 1, HeightRow);	// NOTE: (x)-1 is a padding tweak.
																								//       (w)-1 accounts for the double vertical line

				int selr = getSelectedRow();
				Brush brush;

				for (int r = offsetVert / HeightRow; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r - offsetVert) > Height)
						break;

					if (selr != -1)
					{
						brush = null;

						if (r == selr)
							brush = Brushes.Selected;
						else if ((r < selr && r >= selr + RangeSelect)
							||   (r > selr && r <= selr + RangeSelect))
						{
							brush = Brushes.SubSelected;
						}

						if (brush != null)
						{
							rect.X -= _padHoriRowhead;
							graphics.FillRectangle(brush, rect);
							rect.X += _padHoriRowhead;

							graphics.DrawLine(Pens.DarkLine, 0, rect.Y, WidthRowhead, rect.Y);
							graphics.DrawLine(Pens.DarkLine, 0, rect.Y + HeightRow, WidthRowhead, rect.Y + HeightRow);
						}
					}

					TextRenderer.DrawText(graphics, r.ToString(), _f.FontAccent, rect, Colors.Text, YataGraphics.flags);
				}
			}
		}

		void labelid_Paint(object sender, PaintEventArgs e)
		{
			if (!YataGrid._init)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				if (Settings._gradient)
				{
					Color color1, color2;
					if (_sortcol == 0 && _sortdir == SORT_ASC)
					{
						color1 = Color.Cornsilk;
						color2 = Color.BurlyWood;
					}
					else
					{
						color1 = Color.Lavender;
						color2 = Color.Orchid;
					}

					var brushGrad = new LinearGradientBrush(new Point(0,0), new Point(0, HeightColhead),
															color1, color2);
					var rectGrad  = new Rectangle(0,0, _labelid.Width, _labelid.Height);
					graphics.FillRectangle(brushGrad, rectGrad);
				}
				else
				{
					Color color;
					if (_sortcol == 0 && _sortdir == SORT_ASC)
						color = Colors.FrozenHead;
					else
						color = Colors.LabelSorted;

					_labelid.BackColor = color;
				}

				var rect = new Rectangle(WidthRowhead + _padHori, Top, Cols[0].width(), HeightColhead);
				TextRenderer.DrawText(graphics, "id", _f.FontAccent, rect, Colors.Text, YataGraphics.flags);

				graphics.DrawLine(Pens.DarkLine, _labelid.Width, _labelid.Top, _labelid.Width, _labelid.Bottom);

				if (_sortcol == -1) // draw an asc-arrow on the ID frozenlabel when the table loads
				{
					graphics.DrawImage(Resources.asc_16px,
									   rect.X               - _offsetHoriSort,
									   rect.Y + rect.Height - _offsetVertSort);
				}
				else if (_sortcol == 0)
				{
					Bitmap sort;
					if (_sortdir == SORT_ASC)
						sort = Resources.asc_16px;
					else
						sort = Resources.des_16px;

					graphics.DrawImage(sort,
									   rect.X               - _offsetHoriSort,
									   rect.Y + rect.Height - _offsetVertSort);
				}
			}
		}

		void labelfirst_Paint(object sender, PaintEventArgs e)
		{
			if (!YataGrid._init)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				if (Settings._gradient)
				{
					var brushGrad = new LinearGradientBrush(new Point(0,0), new Point(0, HeightColhead),
															Color.Cornsilk, Color.BurlyWood);
					var rectGrad  = new Rectangle(0,0, _labelfirst.Width, _labelfirst.Height);
					graphics.FillRectangle(brushGrad, rectGrad);
				}

				var rect = new Rectangle(_padHori, Top, Cols[1].width(), HeightColhead);
				TextRenderer.DrawText(graphics, Cols[1].text, _f.FontAccent, rect, Colors.Text, YataGraphics.flags);

				graphics.DrawLine(Pens.DarkLine,
								  _labelfirst.Width, _labelfirst.Top,
								  _labelfirst.Width, _labelfirst.Bottom);

				if (_sortdir != SORT_NOT && _sortcol == 1)
				{
					Bitmap sort;
					if (_sortdir == SORT_ASC)
						sort = Resources.asc_16px;
					else
						sort = Resources.des_16px;

					graphics.DrawImage(sort,
									   rect.X + rect.Width  - _offsetHoriSort,
									   rect.Y + rect.Height - _offsetVertSort);
				}
			}
		}

		void labelsecond_Paint(object sender, PaintEventArgs e)
		{
			if (!YataGrid._init)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				if (Settings._gradient)
				{
					var brushGrad = new LinearGradientBrush(new Point(0,0), new Point(0, HeightColhead),
															Color.Cornsilk, Color.BurlyWood);
					var rectGrad  = new Rectangle(0,0, _labelsecond.Width, _labelsecond.Height);
					graphics.FillRectangle(brushGrad, rectGrad);
				}

				var rect = new Rectangle(_padHori, Top, Cols[2].width(), HeightColhead);
				TextRenderer.DrawText(graphics, Cols[2].text, _f.FontAccent, rect, Colors.Text, YataGraphics.flags);

				graphics.DrawLine(Pens.DarkLine, _labelsecond.Width, _labelsecond.Top, _labelsecond.Width, _labelsecond.Bottom);

				if (_sortdir != SORT_NOT && _sortcol == 2)
				{
					Bitmap sort;
					if (_sortdir == SORT_ASC)
						sort = Resources.asc_16px;
					else
						sort = Resources.des_16px;

					graphics.DrawImage(sort,
									   rect.X + rect.Width  - _offsetHoriSort,
									   rect.Y + rect.Height - _offsetVertSort);
				}
			}
		}

		/// <summary>
		/// Labels the frozen cols.
		/// @note Called by OnPaint of the frozen panel.
		/// @note OnPaint() doesn't want to use the class_var '_graphics'.
		/// </summary>
		internal void PaintFrozenPanel()
		{
			if (RowCount != 0) // safety.
			{
				int x = 0;
				int c = 0;
				for (; c != FrozenCount; ++c)
				{
					x += Cols[c].width();
					graphics.DrawLine(Pens.DarkLine,
									  x, 0,
									  x, Height);
				}

				var rect = new Rectangle(0,0, 0, HeightRow);

				Row row;
				for (int r = offsetVert / HeightRow; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r - offsetVert) > Height)
						break;

					rect.X = _padHori - 1; // NOTE: -1 is a padding tweak.

					row = Rows[r];
					for (c = 0; c != FrozenCount; ++c)
					{
						rect.Width = Cols[c].width();

						if (row[c].loadchanged)
						{
							rect.X     -= _padHori - 1;
							rect.Width -= 1;
							graphics.FillRectangle(Brushes.LoadChanged, rect);
							rect.X     += _padHori - 1;
							rect.Width += 1;
						}

						TextRenderer.DrawText(graphics, row[c].text, Font, rect, Colors.Text, YataGraphics.flags);

						rect.X += rect.Width;
					}

					graphics.DrawLine(Pens.DarkLine,
									  0,                   rect.Y + HeightRow,
									  rect.X + rect.Width, rect.Y + HeightRow);
				}
			}
		}
	}
}
