using System;
using System.Drawing;


namespace yata
{
	// Routines for detering control-sizes and distances in and around the grid.
	sealed partial class YataGrid
	{
		#region Fields (static)
		const int _padHori        =  6; // horizontal text padding in the table
		const int _padVert        =  4; // vertical text padding in the table and col/rowheads
		const int _padHoriRowhead =  8; // horizontal text padding for the rowheads
		const int _padHoriSort    = 12; // additional horizontal text padding to the right in the colheads for the sort-arrow
		#endregion Fields (static)


		#region Methods (static)
		/// <summary>
		/// Sets standard <c><see cref="HeightColhead"/></c>,
		/// <c><see cref="HeightRow"/></c>, and minimum cell-width
		/// <c><see cref="_wId"/></c>. Also caches width of "****" in
		/// <c><see cref="_wStars"/></c>.
		/// </summary>
		/// <param name="f"></param>
		/// <remarks>These values are the same for all loaded tables.</remarks>
		internal static void SetStaticMetrics(Yata f)
		{
			HeightColhead = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, f.FontAccent) + _padVert * 2;
			HeightRow     = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, f.Font)       + _padVert * 2;

			_wId    = YataGraphics.MeasureWidth(gs.Id,    f.Font) + _padHoriRowhead * 2;
			_wStars = YataGraphics.MeasureWidth(gs.Stars, f.Font);
		}


		/// <summary>
		/// Calculates and maintains rowhead width wrt/ current Font across all
		/// tabs/tables.
		/// </summary>
		/// <param name="f"></param>
		internal static void metricStaticHeads(Yata f)
		{
			YataGrid table;

			int rows = 0, rowsTest; // row-headers' width stays uniform across all tabpages

			int tabs = f.Tabs.TabCount;
			int tab = 0;
			for (; tab != tabs; ++tab) // find the table w/ most rows ->
			{
				table = f.Tabs.TabPages[tab].Tag as YataGrid;
				if ((rowsTest = table.RowCount - 1) > rows)
					rows = rowsTest;
			}

			string texttest = "9"; // determine how many nines need to be measured ->
			while ((rows /= 10) != 0)
				texttest += "9";

			WidthRowhead = YataGraphics.MeasureWidth(texttest, f.FontAccent) + _padHoriRowhead * 2;

			for (tab = 0; tab != tabs; ++tab)
			{
				table = f.Tabs.TabPages[tab].Tag as YataGrid;

				table.WidthTable = WidthRowhead;
				foreach (var col in table.Cols)
					table.WidthTable += col.width();

				table._panelRows.Width  = WidthRowhead;
				table._panelCols.Height = HeightColhead;

				metricFrozenLabels(table);
			}
		}

		/// <summary>
		/// Re-widths and re-locates the frozen labels.
		/// </summary>
		/// <param name="table"></param>
		internal static void metricFrozenLabels(YataGrid table)
		{
			if (table.ColCount != 0)
			{
				int w0 = table.Cols[0].width();
				table._labelid.Location = new Point(0,0);
				table._labelid.Size = new Size(WidthRowhead + w0, HeightColhead - 1);	// -1 so these don't cover the long
																						// horizontal line under the colhead.
				if (table.ColCount > 1)
				{
					int w1 = table.Cols[1].width();
					table._labelfirst.Location = new Point(WidthRowhead + w0, 0);
					table._labelfirst.Size = new Size(w1, HeightColhead - 1);

					if (table.ColCount > 2)
					{
						table._labelsecond.Location = new Point(WidthRowhead + w0 + w1, 0);
						table._labelsecond.Size = new Size(table.Cols[2].width(), HeightColhead - 1);
					}
				}
			}
		}
		#endregion Methods (static)


		#region Methods
		/// <summary>
		/// Re-widths any frozen-labels and the frozen-panel if they need it.
		/// </summary>
		/// <param name="c">col-id that changed its width</param>
		internal void metricFrozenControls(int c)
		{
			if (c < FreezeSecond)
			{
				metricFrozenLabels(this); // re-width the frozen-labels on the colhead

				if (c < FrozenCount)
					widthFrozenPanel(); // re-width the frozen-panel
			}
		}

		/// <summary>
		/// Sets the width of the <c><see cref="FrozenPanel"/></c>.
		/// </summary>
		void widthFrozenPanel()
		{
			int width = 0;
			for (int c = 0; c != FrozenCount; ++c)
				width += Cols[c].width();

			FrozenPanel.Width = width;
		}


		/// <summary>
		/// Lays out this <c>YataGrid</c> per
		/// <c><see cref="Yata.doFont()">Yata.doFont()</see></c> or
		/// <c><see cref="Yata"/>.AutosizeCols()</c> or when row(s) are
		/// inserted, deleted, or cleared.
		/// </summary>
		/// <param name="r">first row to consider as changed (<c>-1</c> if
		/// deleting rows)</param>
		/// <param name="range">range of rows to consider as changed (<c>0</c>
		/// for single row)</param>
		internal void Calibrate(int r = -1, int range = 0)
		{
			_init = true;

			for (int c = 0; c != ColCount; ++c)
				Colwidth(c,r, range);

			FrozenCount = FrozenCount; // refresh the Frozen panel

			metricStaticHeads(_f);


			_scrollVert.LargeChange =
			_scrollHori.LargeChange = HeightRow;
			InitScroll();

			Select();
			_init = false;
		}

		/// <summary>
		/// Resets the width of a specified col based on the cells in rows
		/// <paramref name="r"/> to <paramref name="r"/> +
		/// <paramref name="range"/>.
		/// </summary>
		/// <param name="c">col</param>
		/// <param name="r">first row to consider as changed (default -1 if
		/// deleting rows and/or no extant text-widths have changed; ie, no
		/// text-widths need to be re-measured)</param>
		/// <param name="range">range of rows to consider as changed (default 0
		/// for a single row)</param>
		internal void Colwidth(int c, int r = -1, int range = 0)
		{
			Col col = Cols[c];

			int colwidth = col._widthtext + _padHoriSort;
			int widthtext;

			if (r != -1) // re-calc '_widthtext' regardless of what happens below ->
			{
				string text;

				int rend = r + range;
				for (; r <= rend; ++r)
				{
					if ((text = this[r,c].text) == gs.Stars) // bingo.
						widthtext = _wStars;
					else
						widthtext = YataGraphics.MeasureWidth(text, Font);

					this[r,c]._widthtext = widthtext;

					if (widthtext > colwidth)
						colwidth = widthtext;
				}
			}

			if (!col.UserSized)	// ie. don't resize a col that user has adjusted. If it needs to
			{					// be forced (eg. on reload) unflag UserSized on all cols first.
				int totalwidth = col.width();

				if ((colwidth += _padHori * 2) > totalwidth)
				{
					col.width(colwidth);
				}
				else if (colwidth < totalwidth) // recalc width on the entire col
				{
					if (c == 0 || _wId > colwidth)
						colwidth = _wId;

					for (r = 0; r != RowCount; ++r)
					{
						widthtext = this[r,c]._widthtext + _padHori * 2;

						if (widthtext > colwidth)
							colwidth = widthtext;
					}
					col.width(colwidth, true);
				}

				if (range == 0 && totalwidth != colwidth)	// if range >0 let Calibrate() handle multiple
				{											// cols or at least the scrollers and do the UI-update
					InitScroll();
					Invalidator(INVALID_GRID | INVALID_COLS);
				}
			}

			if (Propanel != null && Propanel.Visible)
				Propanel.widthValcol(); // TODO: Re-calc the 'c' col only.
		}
		#endregion Methods
	}
}
