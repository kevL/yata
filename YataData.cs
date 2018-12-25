using System;
using System.Drawing;


namespace yata
{
	/// <summary>
	/// Contains data about a col.
	/// </summary>
	sealed class Col
	{
		internal string text; // the colhead text
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
		:
			ICloneable
	{
		#region Fields and Properties
		internal int _id;

		readonly YataGrid _grid;

		readonly Cell[] _cells;

		/// <summary>
		/// Gets/sets the cell at pos [c].
		/// </summary>
		internal Cell this[int c]
		{
			get { return _cells[c]; }
			set { _cells[c] = value; }
		}

		internal Brush _brush;

		bool _selected;
		internal bool selected
		{
			get { return _selected; }
			set
			{
				_selected = value;
				_grid.RangeSelect = 0;

				_grid._f.EnableCopyRange(_selected); // NOTE to Self: I should code in C since encapsulation is shot.
			}
		}
		#endregion Fields and Properties


		#region cTors
		/// <summary>
		/// cTor[0] - primary.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cols"></param>
		/// <param name="brush"></param>
		/// <param name="grid"></param>
		internal Row(int id, int cols, Brush brush, YataGrid grid)
		{
			_id = id;
			_cells = new Cell[cols];

			_grid = grid;

			_brush = brush;
		}

		/// <summary>
		/// cTor[1] - used by Clone().
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cells"></param>
		/// <param name="brush"></param>
		/// <param name="grid"></param>
		internal Row(int id, Cell[] cells, Brush brush, YataGrid grid)
		{
			_id = id;
			_cells = cells;

			_grid = grid;

			_brush = brush;
		}
		#endregion cTors


		#region ICloneable requirements
		public object Clone()
		{
			//logfile.Log("");
			var cells = new Cell[_cells.Length];
			for (int i = 0; i != _cells.Length; ++i)
			{
				cells[i] = _cells[i].Clone() as Cell;
				//logfile.Log("Row.Clone() cell[" + i + "] text= " + cells[i].text);
			}

			return new Row(_id, cells, _brush, _grid);
		}
		#endregion ICloneable requirements
	}

	/// <summary>
	/// Contains data about a cell.
	/// </summary>
	sealed class Cell
		:
			ICloneable
	{
		internal string text; // the cell's text

		internal bool selected;
		internal bool loadchanged;

		internal int x;
		internal int y;

		internal int _widthtext;


		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="r">row</param>
		/// <param name="c">col</param>
		/// <param name="field">text</param>
		internal Cell(int r, int c, string field)
		{
			y = r;
			x = c;
			text = field;
		}


		#region ICloneable requirements
		public object Clone()
		{
//			return MemberwiseClone();

			var cell = new Cell(y,x, String.Copy(text));
			cell.selected = selected;
			cell.loadchanged = loadchanged;
			cell._widthtext = _widthtext;

			//logfile.Log("Cell.Clone() text= " + cell.text);

			return cell;
		}
		#endregion ICloneable requirements


/*		public override string ToString()
		{
			return String.Format("Cell:{0}"
							   + ". text= {1}{0}"
							   + ". y= {2}{0}"
							   + ". x= {3}{0}"
							   + ". selected= {4}{0}"
							   + ". loadchanged= {5}{0}"
							   + ". _widthtext {6}",
								 Environment.NewLine,
								 text,
								 y,
								 x,
								 selected,
								 loadchanged,
								 _widthtext);
		} */
	}
}
