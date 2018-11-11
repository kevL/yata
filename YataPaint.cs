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
		/// Handles the paint event.
		/// @note OnPaint() doesn't want to use the class_var '_graphics'.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			//logfile.Log("OnPaint()");

//			if (ColCount != 0 && RowCount != 0 && _cells != null)

			if (!_init)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

//				ControlPaint.DrawBorder3D(_graphics, ClientRectangle, Border3DStyle.Etched);


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
							var cell = row.cells[c];
							if (cell.selected)
							{
								rect.X -= _padHori;

								if (_editor.Visible && _editcell == cell)
									graphics.FillRectangle(Brushes.Editor, rect);
								else
									graphics.FillRectangle(Brushes.CellSel, rect);

								rect.X += _padHori;
							}

							TextRenderer.DrawText(graphics, cell.text, Font, rect, Colors.Text, YataGraphics.flags);
//							graphics_.DrawRectangle(new Pen(Color.Crimson), rect); // DEBUG
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
						graphics.DrawLine(Pens.DarkLine, Left, y, WidthTable, y);
				}


				// NOTE: Paint vertical lines full-height of table.

				// col lines - scrollable
				int x = WidthRowhead - offsetHori;
				for (c = 0; c != ColCount; ++c)
				{
					if ((x += Cols[c].width()) > Right)
						break;

					if (x > WidthRowhead)
						graphics.DrawLine(Pens.DarkLine, x, Top, x, Bottom);
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
			//logfile.Log("LabelColheads()");

			if (ColCount != 0) // safety.
			{
				var rect = new Rectangle(WidthRowhead - offsetHori + _padHori, 0, 0, HeightColhead);

				for (int c = 0; c != ColCount; ++c)
				{
					if (rect.X + (rect.Width = Cols[c].width()) > Left)
					{
						TextRenderer.DrawText(graphics, Cols[c].text, _f.FontAccent, rect, Colors.Text, YataGraphics.flags);

						if (_sortdir != 0 && c == _sortcol)
						{
							Bitmap sort;
							if (_sortdir == 1) // asc
								sort = Resources.asc_16px;
							else //if (_sortdir == -1) // des
								sort = Resources.des_16px;

							graphics.DrawImage(sort,
												rect.X + rect.Width  - _offsetHoriSort,
												rect.Y + rect.Height - _offsetVertSort);
						}
					}

					if ((rect.X += rect.Width) > Right)
						break;
				}
			}
		}

		/// <summary>
		/// Labels the rowheads when inserting/deleting/sorting rows.
		/// @note Called by OnPaint of the left-panel.
		/// @note OnPaint() doesn't want to use the class_var '_graphics'.
		/// </summary>
		internal void LabelRowheads()
		{
			//logfile.Log("LabelRowheads()");

			if (RowCount != 0) // safety - ought be checked in calling funct.
			{
				var rect = new Rectangle(_padHoriRowhead - 1, 0, WidthRowhead, HeightRow); // NOTE: -1 is a padding tweak.

				for (int r = offsetVert / HeightRow; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r - offsetVert) > Height)
						break;

					TextRenderer.DrawText(graphics, r.ToString(), _f.FontAccent, rect, Colors.Text, YataGraphics.flags);
				}
			}
		}

		void labelid_Paint(object sender, PaintEventArgs e)
		{
			graphics = e.Graphics;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var rect = new Rectangle(WidthRowhead + _padHori, Top, Cols[0].width(), HeightColhead);
			TextRenderer.DrawText(graphics, "id", _f.FontAccent, rect, Colors.Text, YataGraphics.flags);

			graphics.DrawLine(Pens.DarkLine, _labelid.Width, _labelid.Top, _labelid.Width, _labelid.Bottom);

			if (_sortcol == -1) // draw an asc-arrow on the ID frozenlabel when the table loads
			{
				graphics.DrawImage(Resources.asc_16px,
									rect.X               - _offsetHoriSort, // + rect.Width
									rect.Y + rect.Height - _offsetVertSort);
			}
			else if (_sortcol == 0)
			{
				Bitmap sort;
				if (_sortdir == 1)			// asc
					sort = Resources.asc_16px;
				else //if (_sortdir == -1)	// des
					sort = Resources.des_16px;

				graphics.DrawImage(sort,
									rect.X               - _offsetHoriSort, // + rect.Width
									rect.Y + rect.Height - _offsetVertSort);
			}
		}

		void labelfirst_Paint(object sender, PaintEventArgs e)
		{
			graphics = e.Graphics;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var rect = new Rectangle(_padHori, Top, Cols[1].width(), HeightColhead);
			TextRenderer.DrawText(graphics, Cols[1].text, _f.FontAccent, rect, Colors.Text, YataGraphics.flags);

			graphics.DrawLine(Pens.DarkLine, _labelfirst.Width, _labelfirst.Top, _labelfirst.Width, _labelfirst.Bottom);

			if (_sortdir != 0 && _sortcol == 1)
			{
				Bitmap sort;
				if (_sortdir == 1)			// asc
					sort = Resources.asc_16px;
				else //if (_sortdir == -1)	// des
					sort = Resources.des_16px;

				graphics.DrawImage(sort,
									rect.X + rect.Width  - _offsetHoriSort,
									rect.Y + rect.Height - _offsetVertSort);
			}
		}

		void labelsecond_Paint(object sender, PaintEventArgs e)
		{
			graphics = e.Graphics;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var rect = new Rectangle(_padHori, Top, Cols[2].width(), HeightColhead);
			TextRenderer.DrawText(graphics, Cols[2].text, _f.FontAccent, rect, Colors.Text, YataGraphics.flags);

			graphics.DrawLine(Pens.DarkLine, _labelsecond.Width, _labelsecond.Top, _labelsecond.Width, _labelsecond.Bottom);

			if (_sortdir != 0 && _sortcol == 2)
			{
				Bitmap sort;
				if (_sortdir == 1)			// asc
					sort = Resources.asc_16px;
				else //if (_sortdir == -1)	// des
					sort = Resources.des_16px;

				graphics.DrawImage(sort,
									rect.X + rect.Width  - _offsetHoriSort,
									rect.Y + rect.Height - _offsetVertSort);
			}
		}

		/// <summary>
		/// Labels the frozen cols.
		/// @note Called by OnPaint of the frozen panel.
		/// @note OnPaint() doesn't want to use the class_var '_graphics'.
		/// </summary>
		internal void PaintFrozenPanel()
		{
			//logfile.Log("LabelFrozen()");

			if (RowCount != 0) // safety.
			{
				int x = 0;
				int c = 0;
				for (; c != FrozenCount; ++c)
				{
					x += Cols[c].width();
					graphics.DrawLine(Pens.DarkLine, x, 0, x, Height);
				}
//				graphics_.DrawLine(Pens.DarkLine, x - 1, 0, x - 1, Height);

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
						TextRenderer.DrawText(graphics, row.cells[c].text, Font, rect, Colors.Text, YataGraphics.flags);

						rect.X += rect.Width;
					}

					graphics.DrawLine(Pens.DarkLine, 0, rect.Y + HeightRow, rect.X + rect.Width, rect.Y + HeightRow);
				}
			}
		}
	}
}
