namespace yata
{
	/// <summary>
	/// Contains data about a col.
	/// </summary>
	sealed class Col
	{
//		internal int id;
		internal string text; // the header text

		int _width;
		internal int width
		{
			get { return _width; }
			set
			{
				if (value > _width) // TODO: not exactly. If user shortens a field '_width' could decrease.
					_width = value;
			}
		}

/*		internal Col() //int c
		{
//			id = c;
		} */

		internal void SetColWidth(int w)
		{
			_width = w;
		}
	}

	/// <summary>
	/// Contains data about a row.
	/// </summary>
	sealed class Row
	{
//		internal int id;
		internal string[] fields; // the row's fields

		internal Row(string[] cells) //int r,
		{
//			id = r;
			fields = cells;
		}
	}

	/// <summary>
	/// Contains data about a cell.
	/// </summary>
	sealed class Cell
	{
		internal string text; // the field's text

		internal int x;
		internal int y;

		internal bool selected;

		internal Cell(int c, int r, string field)
		{
			x = c;
			y = r;
			text = field;
		}
	}
}
