using System;
using System.Drawing;


namespace yata
{
	/// <summary>
	/// Contains data about a col.
	/// </summary>
	sealed class Col
	{
		internal string text; // the header text
		internal bool selected;

		int _width;
		internal int _widthtext;

		internal bool UserSized
		{ get; set; }

		/// <summary>
		/// Sets the width.
		/// @note When loading a 2da width is set to the colhead first, but then
		/// can be replaced by the widest cell-width under it. The col's width
		/// is thus allowed to only increase when loading a 2da. When a cell's
		/// text changes however the col-width should be allowed to decrease if
		/// it's less wide than the current width (but not if it's less wide
		/// than the colhead's width). Hence the 'force' par.
		/// </summary>
		/// <param name="w">the width in pixels</param>
		/// <param name="force">true to allow decreasing the width</param>
		internal void width(int w, bool force = false)
		{
			if (force || w > _width)
				_width = w;
		}

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <returns></returns>
		internal int width()
		{
			return _width;
		}
	}

	/// <summary>
	/// Contains data about a row.
	/// </summary>
	sealed class Row
	{
		internal int _id;

		readonly YataGrid _grid;

		internal Cell[] cells;
//		/// <summary>
//		/// Gets/sets the cell at pos [c].
//		/// </summary>
//		internal Cell this[int c]
//		{
//			get { return cells[c]; }
//			set { cells[c] = value; }
//		}

		internal Brush _brush;

		bool _selected;
		internal bool selected
		{
			get { return _selected; }
			set
			{
				_selected = value;
				_grid.RangeSelect = 0;
			}
		}

		internal Row(int id, int cols, Brush brush, YataGrid grid)
		{
			_id = id;
			cells = new Cell[cols];

			_grid = grid;

			_brush = brush;
		}
	}

	/// <summary>
	/// Contains data about a cell.
	/// </summary>
	sealed class Cell
	{
		internal string text; // the cell's text

		internal bool selected;
		internal bool loadchanged;

		internal int x;
		internal int y;

		internal int _widthtext;

		internal Cell(int r, int c, string field)
		{
			y = r;
			x = c;
			text = field;
		}
	}
}
