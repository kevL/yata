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


		#region Methods
		/// <summary>
		/// Deters <c><see cref="Cell._widthtext">Cell._widthtext</see></c> for
		/// all <c><see cref="Cell">Cells</see></c> in a specified col-id.
		/// </summary>
		/// <param name="c">col-id</param>
		internal void colTextwidth(int c)
		{
			for (int r = 0; r != RowCount; ++r)
				doTextwidth(this[r,c]);
		}

		/// <summary>
		/// Deters <c><see cref="Cell._widthtext">Cell._widthtext</see></c> for
		/// a specified <c><see cref="Cell"/></c>.
		/// </summary>
		/// <param name="cell">the <c>Cell</c> for which to set the
		/// <c>_widthtext</c></param>
		internal void doTextwidth(Cell cell)
		{
			if (cell.text == gs.Stars) // bingo.
				cell._widthtext = _wStars;
			else
				cell._widthtext = YataGraphics.MeasureWidth(cell.text, Font);
		}

		/// <summary>
		/// Lays out this <c>YataGrid</c> per
		/// <c><see cref="Yata.doFont()">Yata.doFont()</see></c> or
		/// <c><see cref="Yata"/>.AutosizeCols()</c> or when row(s) are
		/// inserted, deleted, pasted, or cleared.
		/// </summary>
		/// <param name="r">first row-id to consider as changed (default
		/// <c>-1</c> if deleting rows and/or no extant text-widths have
		/// changed; ie. no text-widths need to be re-measured)</param>
		/// <param name="range">range of rows to consider as changed (default
		/// <c>0</c> for a single row)</param>
		internal void Calibrate(int r = -1, int range = 0)
		{
			_init = true;

			for (int c = 0; c != ColCount; ++c)
				Colwidth(c,r, range);

			FrozenCount = FrozenCount; // refresh the Frozen panel

			MetricStaticHeads(_f);


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
		/// <param name="c">col-id</param>
		/// <param name="r">first row-id to consider as changed (default
		/// <c>-1</c> if deleting rows and/or no extant text-widths have
		/// changed; ie. no text-widths need to be re-measured)</param>
		/// <param name="range">range of rows to consider as changed (default
		/// <c>0</c> for a single row)</param>
		internal void Colwidth(int c, int r = -1, int range = 0)
		{
			Col col = Cols[c];

			if (!col.UserSized) // ie. don't resize a col that user has adjusted - if it needs to be forced unflag UserSized on all cols first (eg. on reload).
			{
				int width = col._widthtext + _padHoriSort;
				int widthtext;

				// find the greatest widthtext (changed) in the col ->
				if (r != -1)
				{
					int rend = r + range;
					for (; r <= rend; ++r)
					{
						if ((widthtext = this[r,c]._widthtext) > width) // TODO: Ensure that _widthtext gets calc'd before call to Colwidth()
							width = widthtext;
					}
				}


				int widthcurrent = col.Width;

				if ((width += _padHori * 2) > widthcurrent)
				{
					col.SetWidth(width);
				}
				else if (width < widthcurrent) // recalc width on the entire col
				{
					if (c == 0 || _wId > width)
						width = _wId;

					for (r = 0; r != RowCount; ++r)
					{
						widthtext = this[r,c]._widthtext + _padHori * 2;

						if (widthtext > width)
							width = widthtext;
					}
					col.SetWidth(width, true);
				}

				if (range == 0 && widthcurrent != width)	// if range >0 let Calibrate() handle multiple
				{											// cols or at least the scrollers and do the UI-update
					InitScroll();
					Invalidator(INVALID_GRID | INVALID_COLS);
				}
			}

			if (Propanel != null && Propanel.Visible)
			{
				Propanel.widthValcol(); // TODO: Re-calc the 'c' col only.
				Propanel.telemetric();
			}
		}


		/// <summary>
		/// Re-widths any frozen-labels and the frozen-panel if they need it.
		/// </summary>
		/// <param name="c">col-id that changed its width</param>
		internal void MetricFrozenControls(int c)
		{
			if (c < FreezeSecond)
			{
				MetricFrozenLabels(this); // re-width the frozen-labels on the colhead

				if (c < FrozenCount)
					WidthFrozenPanel(); // re-width the frozen-panel
			}
		}

		/// <summary>
		/// Sets the width of the <c><see cref="FrozenPanel"/></c>.
		/// </summary>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item><c><see cref="MetricFrozenControls()">MetricFrozenControls()</see></c></item>
		/// <item><c><see cref="FrozenCount"/></c></item>
		/// </list></remarks>
		void WidthFrozenPanel()
		{
			int width = 0;
			for (int c = 0; c != FrozenCount; ++c)
				width += Cols[c].Width;

			FrozenPanel.Width = width;
		}
		#endregion Methods


		#region Methods (static)
		/// <summary>
		/// Calculates and maintains rowhead width wrt/ current Font across all
		/// tabs/tables.
		/// </summary>
		/// <param name="f"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item><c><see cref="Calibrate()">Calibrate()</see></c></item>
		/// <item><c><see cref="Init()">Init()</see></c></item>
		/// <item><c><see cref="Yata"/>.ClosePage()</c></item>
		/// </list></remarks>
		internal static void MetricStaticHeads(Yata f)
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

			string digits = "9"; // determine how many nines need to be measured ->
			while ((rows /= 10) != 0)
				digits += "9";

			WidthRowhead = YataGraphics.MeasureWidth(digits, f.FontAccent) + _padHoriRowhead * 2;

			for (tab = 0; tab != tabs; ++tab)
			{
				table = f.Tabs.TabPages[tab].Tag as YataGrid;

				table.WidthTable = WidthRowhead;
				foreach (var col in table.Cols)
					table.WidthTable += col.Width;

				table._panelRows.Width  = WidthRowhead;
				table._panelCols.Height = HeightColhead;

				MetricFrozenLabels(table);
			}
		}

		/// <summary>
		/// Re-widths and re-locates the frozen labels.
		/// </summary>
		/// <param name="table"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item><c><see cref="MetricFrozenControls()">MetricFrozenControls()</see></c></item>
		/// <item><c><see cref="MetricStaticHeads()">MetricStaticHeads()</see></c></item>
		/// </list></remarks>
		internal static void MetricFrozenLabels(YataGrid table)
		{
			if (table.ColCount != 0)
			{
				int w0 = table.Cols[0].Width;
				table._labelid.Location = new Point(0,0);
				table._labelid.Size = new Size(WidthRowhead + w0, HeightColhead - 1);	// -1 so these don't cover the long
																						// horizontal line under the colhead.
				if (table.ColCount > 1)
				{
					int w1 = table.Cols[1].Width;
					table._labelfirst.Location = new Point(WidthRowhead + w0, 0);
					table._labelfirst.Size = new Size(w1, HeightColhead - 1);

					if (table.ColCount > 2)
					{
						table._labelsecond.Location = new Point(WidthRowhead + w0 + w1, 0);
						table._labelsecond.Size = new Size(table.Cols[2].Width, HeightColhead - 1);
					}
				}
			}
		}


		/// <summary>
		/// Sets standard <c><see cref="HeightColhead"/></c>,
		/// <c><see cref="HeightRow"/></c>, and minimum cell-width
		/// <c><see cref="_wId"/></c>. Also caches width of "****" in
		/// <c><see cref="_wStars"/></c>.
		/// </summary>
		/// <param name="f"></param>
		/// <remarks>These values are the same for all loaded tables.</remarks>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item><c><see cref="Yata()">Yata()</see></c></item>
		/// <item><c><see cref="Yata.doFont()">Yata.doFont()</see></c></item>
		/// </list></remarks>
		internal static void SetStaticMetrics(Yata f)
		{
			HeightColhead = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, f.FontAccent) + _padVert * 2;
			HeightRow     = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, f.Font)       + _padVert * 2;

			_wId    = YataGraphics.MeasureWidth(gs.Id,    f.Font) + _padHoriRowhead * 2;
			_wStars = YataGraphics.MeasureWidth(gs.Stars, f.Font);
		}
		#endregion Methods (static)
	}
}
