using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

using yata.Properties;


namespace yata
{
	/// <summary>
	/// Handles painting various controls on this <c>YataGrid</c>.
	/// </summary>
	sealed partial class YataGrid
	{
		/// <summary>
		/// Overrides the <c>Paint</c> handler for the table itself.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!_init)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

//				ControlPaint.DrawBorder3D(graphics, ClientRectangle, Border3DStyle.Etched);

				int r,c;

				// NOTE: Paint backgrounds full-height/width of table ->

				int offset_y = HeightColhead - OffsetVert;

				// rows background - scrollable
				var rect = new Rectangle(Left,       offset_y,
										 WidthTable, HeightRow);

				for (r = 0; r != RowCount; ++r)
				{
					graphics.FillRectangle(Rows[r]._brush, rect);
					rect.Y += HeightRow;
				}

				// cell text - scrollable
				Row row; Cell cell;

				int left = WidthRowhead - OffsetHori + _padHori - 1;

				for (r = OffsetVert / HeightRow; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r + offset_y) > Bottom)
						break;

					rect.X = left;

					row = Rows[r];
					for (c = 0; c != ColCount; ++c)
					{
						if (rect.X + (rect.Width = Cols[c].width()) > WidthRowhead)
						{
							if ((cell = row[c]).state != Cell.CellState.Default)
							{
								rect.X -= _padHori;
								graphics.FillRectangle(cell.getBrush(_editor.Visible && _editcell == cell), rect);
								rect.X += _padHori;
							}

							rect.Width -= _padHori;
							TextRenderer.DrawText(graphics,
												  cell.text,
												  Font,
												  rect,
												  Colors.Text,
												  YataGraphics.flags);
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
					if ((y = HeightRow * r + offset_y) > Bottom)
						break;

					if (y > HeightColhead)
						graphics.DrawLine(Pencils.DarkLine,
										  Left,       y,
										  WidthTable, y);
				}


				// NOTE: Paint vertical lines full-height of table.

				// col lines - scrollable
				int x = WidthRowhead - OffsetHori;
				for (c = 0; c != ColCount; ++c)
				{
					if ((x += Cols[c].width()) > Right)
						break;

					if (x > WidthRowhead)
						graphics.DrawLine(Pencils.DarkLine,
										  x, Top,
										  x, Bottom);
				}
			}
		}

		/// <summary>
		/// Labels the colheads.
		/// </summary>
		/// <remarks>Called by <c><see cref="YataPanelCols"/>.OnPaint()</c>.</remarks>
		internal void LabelColheads()
		{
			var rect = new Rectangle(WidthRowhead - OffsetHori + _padHori, 0,
									 0,                                    HeightColhead);
			int clip;
			Color color;
			Col col;

			for (int c = 0; c != ColCount; ++c)
			{
				if (rect.X + (rect.Width = (col = Cols[c]).width()) > Left)
				{
					if (c == _sortcol)
					{
						Bitmap sort;
						if (_sortdir == SORT_ASC)
						{
							if (col.selected) color = Colors.TextColSelected;
							else              color = Colors.TextColSorted_asc;

							sort  = Resources.asc_16px;
						}
						else
						{
							if (col.selected) color = Colors.TextColSelected;
							else              color = Colors.TextColSorted_des;

							sort  = Resources.des_16px;
						}

						graphics.DrawImage(sort,
										   rect.X + rect.Width  - _offsetHoriSort,
										   rect.Y + rect.Height - _offsetVertSort);

						clip = _offsetHoriSort - 1;
					}
					else
					{
						clip = 7;

						if      (col.selected)  color = Colors.TextColSelected;
						else if (col.UserSized) color = Colors.TextColSized;
						else                    color = Colors.Text;
					}

					rect.Width -= clip;
					TextRenderer.DrawText(graphics,
										  col.text,
										  _f.FontAccent,
										  rect,
										  color,
										  YataGraphics.flags);
					rect.Width += clip;
				}

				if ((rect.X += rect.Width) > Right)
					break;
			}
		}

		/// <summary>
		/// Labels the rowheads.
		/// </summary>
		/// <remarks>Called by <c><see cref="YataPanelRows"/>.OnPaint()</c>.</remarks>
		internal void LabelRowheads()
		{
			var rect = new Rectangle(_padHoriRowhead - 1, 0, WidthRowhead - 1, HeightRow);	// NOTE: (x)-1 is a padding tweak.
																							//       (w)-1 accounts for the double vertical line
			int selr = getSelectedRow();
			Brush brush;

			for (int r = OffsetVert / HeightRow; r != RowCount; ++r)
			{
				if ((rect.Y = HeightRow * r - OffsetVert) > Height)
					break;

				if (selr != -1)
				{
					if (r == selr)
						brush = Brushes.Selected;
					else if ((r < selr && r >= selr + RangeSelect)
						||   (r > selr && r <= selr + RangeSelect))
					{
						brush = Brushes.SubSelected;
					}
					else
						brush = null;

					if (brush != null)
					{
						rect.X -= _padHoriRowhead;
						graphics.FillRectangle(brush, rect);
						rect.X += _padHoriRowhead;

						graphics.DrawLine(Pencils.DarkLine,
										  0,            rect.Y,
										  WidthRowhead, rect.Y);
						graphics.DrawLine(Pencils.DarkLine,
										  0,            rect.Y + HeightRow,
										  WidthRowhead, rect.Y + HeightRow);
					}
				}

				TextRenderer.DrawText(graphics,
									  r.ToString(CultureInfo.InvariantCulture),
									  _f.FontAccent,
									  rect,
									  Colors.Text,
									  YataGraphics.flags);
			}
		}

		/// <summary>
		/// Paints the frozen-id <c>Label</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="_labelid"/></c></param>
		/// <param name="e"></param>
		void labelid_Paint(object sender, PaintEventArgs e)
		{
			if (!_init)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				Rectangle rect;

				if (Settings._gradient)
				{
					LinearGradientBrush grad;
					if (_sortcol == 0 && _sortdir == SORT_ASC)
						grad = Gradients.FrozenLabel;
					else
						grad = Gradients.Disordered;

					rect = new Rectangle(0,0, _labelid.Width, _labelid.Height);
					graphics.FillRectangle(grad, rect);
				}
				else if (_sortcol == 0 && _sortdir == SORT_ASC)
					_labelid.BackColor = Colors.FrozenHead;
				else
					_labelid.BackColor = Colors.LabelSorted;

				rect = new Rectangle(WidthRowhead + _padHori, Top, Cols[0].width(), HeightColhead);
				TextRenderer.DrawText(graphics,
									  gs.Id,
									  _f.FontAccent,
									  rect,
									  Colors.Text,
									  YataGraphics.flags);

				graphics.DrawLine(Pencils.DarkLine,
								  _labelid.Width, _labelid.Top,
								  _labelid.Width, _labelid.Bottom);

				if (_sortcol == 0)
				{
					Bitmap sorticon;
					if (_sortdir == SORT_ASC)
						sorticon = Resources.asc_16px;
					else
						sorticon = Resources.des_16px;

					graphics.DrawImage(sorticon,
									   rect.X               - _offsetHoriSort,
									   rect.Y + rect.Height - _offsetVertSort);
				}
			}
		}

		/// <summary>
		/// Paints the frozen-first <c>Label</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="_labelfirst"/></c></param>
		/// <param name="e"></param>
		void labelfirst_Paint(object sender, PaintEventArgs e)
		{
			if (!_init)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				Rectangle rect;

				if (Settings._gradient)
				{
					rect = new Rectangle(0,0, _labelfirst.Width, _labelfirst.Height);
					graphics.FillRectangle(Gradients.FrozenLabel, rect);
				}

				Color color;
				rect = new Rectangle(_padHori, Top, Cols[1].width(), HeightColhead);

				if (_sortcol == 1)
				{
					Bitmap sorticon;
					if (_sortdir == SORT_ASC)
					{
						color    = Colors.TextColSorted_asc;
						sorticon = Resources.asc_16px;
					}
					else
					{
						color    = Colors.TextColSorted_des;
						sorticon = Resources.des_16px;
					}
					graphics.DrawImage(sorticon,
									   rect.X + rect.Width  - _offsetHoriSort,
									   rect.Y + rect.Height - _offsetVertSort);
				}
				else
					color = Colors.Text;

				TextRenderer.DrawText(graphics,
									  Cols[1].text,
									  _f.FontAccent,
									  rect,
									  color,
									  YataGraphics.flags);

				graphics.DrawLine(Pencils.DarkLine,
								  _labelfirst.Width, _labelfirst.Top,
								  _labelfirst.Width, _labelfirst.Bottom);
			}
		}

		/// <summary>
		/// Paints the frozen-second <c>Label</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="_labelsecond"/></c></param>
		/// <param name="e"></param>
		void labelsecond_Paint(object sender, PaintEventArgs e)
		{
			if (!_init)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				Rectangle rect;

				if (Settings._gradient)
				{
					rect = new Rectangle(0,0, _labelsecond.Width, _labelsecond.Height);
					graphics.FillRectangle(Gradients.FrozenLabel, rect);
				}

				Color color;
				rect = new Rectangle(_padHori, Top, Cols[2].width(), HeightColhead);

				if (_sortcol == 2)
				{
					Bitmap sorticon;
					if (_sortdir == SORT_ASC)
					{
						color    = Colors.TextColSorted_asc;
						sorticon = Resources.asc_16px;
					}
					else
					{
						color    = Colors.TextColSorted_des;
						sorticon = Resources.des_16px;
					}

					graphics.DrawImage(sorticon,
									   rect.X + rect.Width  - _offsetHoriSort,
									   rect.Y + rect.Height - _offsetVertSort);
				}
				else
					color = Colors.Text;

				TextRenderer.DrawText(graphics,
									  Cols[2].text,
									  _f.FontAccent,
									  rect,
									  color,
									  YataGraphics.flags);

				graphics.DrawLine(Pencils.DarkLine,
								  _labelsecond.Width, _labelsecond.Top,
								  _labelsecond.Width, _labelsecond.Bottom);
			}
		}

		/// <summary>
		/// Paints the frozen-panel.
		/// </summary>
		/// <remarks>Called by <c><see cref="YataPanelFrozen"/>.OnPaint()</c>.</remarks>
		internal void PaintFrozenPanel()
		{
			int x = 0;
			int c = 0;
			for (; c != FrozenCount; ++c)
			{
				x += Cols[c].width();
				graphics.DrawLine(Pencils.DarkLine,
								  x, 0,
								  x, Height);
			}

			var rect = new Rectangle(0,0, 0, HeightRow);

			Row row; Cell cell;
			for (int r = OffsetVert / HeightRow; r != RowCount; ++r)
			{
				if ((rect.Y = HeightRow * r - OffsetVert) > Height)
					break;

				rect.X = _padHori - 1;

				row = Rows[r];
				for (c = 0; c != FrozenCount; ++c)
				{
					rect.Width = Cols[c].width();

					if ((cell = row[c]).state != Cell.CellState.Default)
					{
						rect.X     -= _padHori - 1;
						rect.Width -= 1;
						graphics.FillRectangle(cell.getBrush(), rect);
						rect.X     += _padHori - 1;
						rect.Width += 1;
					}

					TextRenderer.DrawText(graphics,
										  cell.text,
										  Font,
										  rect,
										  Colors.Text,
										  YataGraphics.flags);

					rect.X += rect.Width;
				}

				graphics.DrawLine(Pencils.DarkLine,
								  0,                   rect.Y + HeightRow,
								  rect.X + rect.Width, rect.Y + HeightRow);
			}
		}
	}
}
