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

		/// <summary>
		/// Sets the width.
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

		internal Brush _brush;
		internal bool selected;

		internal Row(int id, Brush brush)
		{
			_id = id;
			_brush = brush;
		}
	}

	/// <summary>
	/// Contains data about a cell.
	/// </summary>
	[SerializableAttribute]
	sealed class Cell
	{
		internal string text; // the cell's text
		internal bool selected;

		internal int x;
		internal int y;

		internal Cell(int r, int c, string field)
		{
			y = r;
			x = c;
			text = field;
		}
	}
}
