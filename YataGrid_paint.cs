using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

using yata.Properties;


namespace yata
{
	// handles painting various controls
	sealed partial class YataGrid
	{
		#region Fields (static)
		const int _offsetSortHori = 23; // horizontal offset for the sort-arrow
		const int _offsetSortVert = 15; // vertical offset for the sort-arrow
		#endregion Fields (static)


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>Paint</c> handler for the table itself.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Assigns <c>e.Graphics</c> to
		/// <c><see cref="graphics">YataGrid.graphics</see></c>.</remarks>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!_init)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				int r,c, r_start = OffsetVert / HeightRow;
				int offset_y = HeightColhead - OffsetVert;

				// fill backgrounds ->
				int left = WidthRowhead + FrozenPanel.Width;
				var rect = new Rectangle(left, 0, WidthTable - left, HeightRow);

				Brush brush;
				for (r = r_start; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r + offset_y) > Bottom)
						break;

					switch (Info)
					{
						case InfoType.INFO_SPELL:
							brush = getDisabled(r, InfoInputSpells.Removed);
							break;

						case InfoType.INFO_FEAT:
							brush = getDisabled(r, InfoInputFeat.REMOVED);
							break;

						default:
							brush = Rows[r]._brush;
							break;
					}
					graphics.FillRectangle(brush, rect);
				}


				int x = WidthRowhead - OffsetHori;

				// draw text and fill backgrounds of nondefault cells ->
				Row row; Cell cell;

				int leftcell = x + _padHori - 1;

				for (r = r_start; r != RowCount; ++r)
				{
					if ((rect.Y = HeightRow * r + offset_y) > Bottom)
						break;

					rect.X = leftcell;

					row = Rows[r];
					for (c = 0; c != ColCount; ++c)
					{
						if (rect.X + (rect.Width = Cols[c].Width) > left)
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
												  ColorOptions._tabletext,
												  YataGraphics.flags);
							rect.Width += _padHori;
						}

						if ((rect.X += rect.Width) > Right)
							break;
					}
				}


				// draw horizontal lines ->
				int val = Math.Min(Width, WidthTable);
				int y;
				for (r = r_start + 1; r != RowCount + 1; ++r)
				{
					if ((y = HeightRow * r + offset_y) > Bottom)
						break;

					if (y > HeightColhead)
						graphics.DrawLine(ColorOptions._tablelines,
										  left, y,
										  val,  y);
				}

				// draw vertical lines ->
				val = Math.Min(Height, HeightTable);
				left += 1;
				for (c = 0; c != ColCount; ++c)
				{
					if ((x += Cols[c].Width) > Right)
						break;

					if (x > left)
						graphics.DrawLine(ColorOptions._tablelines,
										  x, HeightColhead,
										  x, val);
				}


//				using (var pi = new Bitmap(_bluePi, new Size(WidthTable, HeightColhead)))
//				if (_piColhead != null) graphics_.DrawImage(_piColhead, 0,0);

//				using (var pi = new Bitmap(_bluePi, new Size(WidthRowhead, HeightTable)))
//				if (_piRowhead != null) graphics_.DrawImage(_piRowhead, 0,0);
			}
		}

		/// <summary>
		/// Gets a <c>Brush</c> with which to paint the background of disabled
		/// <c><see cref="Row">Rows</see></c> (for Spells.2da and Feat.2da
		/// only).
		/// </summary>
		/// <param name="r">row-id</param>
		/// <param name="c">col-id of the REMOVED col</param>
		/// <returns><c><see cref="Row._brush">Row._brush</see></c> if the
		/// <c>Row</c> is not disabled</returns>
		/// <remarks>Helper for <c><see cref="OnPaint()">OnPaint()</see></c></remarks>
		Brush getDisabled(int r, int c)
		{
			if (ColCount > c
				&& Fields[c - 1] == "REMOVED"
				&& this[r,c].text != "0")
			{
				return (r % 2 == 0) ? ColorOptions._rowdisableda
									: ColorOptions._rowdisabledb;
			}
			return Rows[r]._brush;
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Paints the frozen-id <c>Label</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="_labelid"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Assigns <c>e.Graphics</c> to
		/// <c><see cref="graphics">YataGrid.graphics</see></c>.</remarks>
		void labelid_Paint(object sender, PaintEventArgs e)
		{
			if (!_init)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				Rectangle rect;

				if (Gradients.FrozenLabel != null)
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
					_labelid.BackColor = ColorOptions._frozenhead;
				else
					_labelid.BackColor = ColorOptions._frozenidunsort;

				rect = new Rectangle(WidthRowhead + _padHori, Top,
									 Cols[0].Width, HeightColhead);

				TextRenderer.DrawText(graphics,
									  gs.Id,
									  _f.FontAccent,
									  rect,
									  ColorOptions._frozenheadtext,
									  YataGraphics.flags);

				graphics.DrawLine(ColorOptions._frozenheadlines,
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
									   rect.X - _offsetSortHori,
									   rect.Y - _offsetSortVert + rect.Height);
				}
			}
		}

		/// <summary>
		/// Paints the frozen-first <c>Label</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="_labelfirst"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Assigns <c>e.Graphics</c> to
		/// <c><see cref="graphics">YataGrid.graphics</see></c>.</remarks>
		void labelfirst_Paint(object sender, PaintEventArgs e)
		{
			if (!_init)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				Rectangle rect;

				if (Gradients.FrozenLabel != null)
				{
					rect = new Rectangle(0,0, _labelfirst.Width, _labelfirst.Height);
					graphics.FillRectangle(Gradients.FrozenLabel, rect);
				}

				Color color;
				rect = new Rectangle(_padHori, Top,
									 Cols[1].Width, HeightColhead);

				if (_sortcol == 1)
				{
					Bitmap sorticon;
					if (_sortdir == SORT_ASC)
					{
						color    = ColorOptions._headtextsortasc;
						sorticon = Resources.asc_16px;
					}
					else
					{
						color    = ColorOptions._headtextsortdes;
						sorticon = Resources.des_16px;
					}
					graphics.DrawImage(sorticon,
									   rect.X + rect.Width  - _offsetSortHori,
									   rect.Y + rect.Height - _offsetSortVert);
				}
				else
					color = ColorOptions._frozenheadtext;

				TextRenderer.DrawText(graphics,
									  Cols[1].text,
									  _f.FontAccent,
									  rect,
									  color,
									  YataGraphics.flags);

				graphics.DrawLine(ColorOptions._frozenheadlines,
								  _labelfirst.Width, _labelfirst.Top,
								  _labelfirst.Width, _labelfirst.Bottom);
			}
		}

		/// <summary>
		/// Paints the frozen-second <c>Label</c>.
		/// </summary>
		/// <param name="sender"><c><see cref="_labelsecond"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Assigns <c>e.Graphics</c> to
		/// <c><see cref="graphics">YataGrid.graphics</see></c>.</remarks>
		void labelsecond_Paint(object sender, PaintEventArgs e)
		{
			if (!_init)
			{
				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				Rectangle rect;

				if (Gradients.FrozenLabel != null)
				{
					rect = new Rectangle(0,0, _labelsecond.Width, _labelsecond.Height);
					graphics.FillRectangle(Gradients.FrozenLabel, rect);
				}

				Color color;
				rect = new Rectangle(_padHori, Top,
									 Cols[2].Width, HeightColhead);

				if (_sortcol == 2)
				{
					Bitmap sorticon;
					if (_sortdir == SORT_ASC)
					{
						color    = ColorOptions._headtextsortasc;
						sorticon = Resources.asc_16px;
					}
					else
					{
						color    = ColorOptions._headtextsortdes;
						sorticon = Resources.des_16px;
					}

					graphics.DrawImage(sorticon,
									   rect.X + rect.Width  - _offsetSortHori,
									   rect.Y + rect.Height - _offsetSortVert);
				}
				else
					color = ColorOptions._frozenheadtext;

				TextRenderer.DrawText(graphics,
									  Cols[2].text,
									  _f.FontAccent,
									  rect,
									  color,
									  YataGraphics.flags);

				graphics.DrawLine(ColorOptions._frozenheadlines,
								  _labelsecond.Width, _labelsecond.Top,
								  _labelsecond.Width, _labelsecond.Bottom);
			}
		}
		#endregion Handlers


		#region Helpers
		/// <summary>
		/// Labels the colheads.
		/// </summary>
		/// <remarks>Requires
		/// <c><see cref="YataGrid.graphics">YataGrid.graphics</see></c> set by
		/// <c><see cref="YataPanelCols"/>.OnPaint()</c>.</remarks>
		internal void LabelColheads()
		{
			var rect = new Rectangle(WidthRowhead - OffsetHori + _padHori, 0,
									 0, HeightColhead);
			int clip;
			Color color;
			Col col;

			for (int c = 0; c != ColCount; ++c)
			{
				if (rect.X + (rect.Width = (col = Cols[c]).Width) > Left)
				{
					if (c == _sortcol)
					{
						Bitmap sort;
						if (_sortdir == SORT_ASC)
						{
							if (col.selected) color = ColorOptions._colheadtextsel;
							else              color = ColorOptions._headtextsortasc;

							sort  = Resources.asc_16px;
						}
						else
						{
							if (col.selected) color = ColorOptions._colheadtextsel;
							else              color = ColorOptions._headtextsortdes;

							sort  = Resources.des_16px;
						}

						graphics.DrawImage(sort,
										   rect.X + rect.Width  - _offsetSortHori,
										   rect.Y + rect.Height - _offsetSortVert);

						clip = _offsetSortHori - 1;
					}
					else
					{
						clip = 7;

						if      (col.selected)  color = ColorOptions._colheadtextsel;
						else if (col.UserSized) color = ColorOptions._colheadtextsize;
						else                    color = ColorOptions._colheadtext;
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
		/// <remarks>Requires
		/// <c><see cref="YataGrid.graphics">YataGrid.graphics</see></c> set by
		/// <c><see cref="YataPanelRows"/>.OnPaint()</c>.</remarks>
		internal void LabelRowheads()
		{
			var rect = new Rectangle(_padHoriRowhead - 1, 0,		// NOTE: (x)-1 is a padding tweak.
									 WidthRowhead - 1, HeightRow);	//       (w)-1 accounts for the double vertical line

			int selr = getSelectedRow();
			Brush brush;

			for (int r = OffsetVert / HeightRow; r != RowCount; ++r)
			{
				if ((rect.Y = HeightRow * r - OffsetVert) > Height)
					break;

				if (selr != -1)
				{
					if (r == selr)
						brush = Brushers.Selected;
					else if ((r < selr && r >= selr + RangeSelect)
						||   (r > selr && r <= selr + RangeSelect))
					{
						brush = Brushers.SubSelected;
					}
					else
						brush = null;

					if (brush != null)
					{
						rect.X -= _padHoriRowhead;
						graphics.FillRectangle(brush, rect);
						rect.X += _padHoriRowhead;

						graphics.DrawLine(ColorOptions._rowpanellines,
										  0,            rect.Y,
										  WidthRowhead, rect.Y);
						graphics.DrawLine(ColorOptions._rowpanellines,
										  0,            rect.Y + HeightRow,
										  WidthRowhead, rect.Y + HeightRow);
					}
				}

				TextRenderer.DrawText(graphics,
									  r.ToString(CultureInfo.InvariantCulture),
									  _f.FontAccent,
									  rect,
									  ColorOptions._rowpaneltext,
									  YataGraphics.flags);
			}
		}


		/// <summary>
		/// Paints the frozen-panel.
		/// </summary>
		/// <remarks>Requires
		/// <c><see cref="YataGrid.graphics">YataGrid.graphics</see></c> set by
		/// <c><see cref="YataPanelFrozen"/>.OnPaint()</c>.</remarks>
		internal void PaintFrozenPanel()
		{
			int x = 0;
			int c = 0;
			for (; c != FrozenCount; ++c)
			{
				x += Cols[c].Width;
				graphics.DrawLine(ColorOptions._frozenlines,
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
					rect.Width = Cols[c].Width;

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
										  ColorOptions._frozentext,
										  YataGraphics.flags);
					rect.X += rect.Width;
				}

				graphics.DrawLine(ColorOptions._frozenlines,
								  0,                   rect.Y + HeightRow,
								  rect.X + rect.Width, rect.Y + HeightRow);
			}
		}
		#endregion Helpers
	}
}
