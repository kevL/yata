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

		/// <summary>
		/// Flags this <c>Col</c> as selected.
		/// </summary>
		/// <remarks>Only 1 <c>Col</c> shall ever be <c>selected</c>.</remarks>
		internal bool selected;

		int _width;
		internal int _widthtext;

		internal bool UserSized
		{ get; set; }

		/// <summary>
		/// Sets <c><see cref="_width"/></c>.
		/// </summary>
		/// <param name="w">the width in pixels</param>
		/// <param name="force"><c>true</c> to allow decreasing the width</param>
		/// <remarks>When loading a 2da width is set to the colhead first, but
		/// then can be replaced by the widest cell-width under it. The col's
		/// width is thus allowed to only increase when loading a 2da. When a
		/// cell's text changes however the col-width should be allowed to
		/// decrease if it's less wide than the current width (but not if it's
		/// less wide than the colhead's width). Hence the
		/// <paramref name="force"/> parameter.</remarks>
		internal void width(int w, bool force = false)
		{
			if (force || w > _width)
				_width = w;
		}

		/// <summary>
		/// Gets <c><see cref="_width"/></c>.
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
		/// <summary>
		/// Set <c>_bypassEnableRowedit</c> <c>true</c> when clearing
		/// <c><see cref="selected"/></c> for a synced
		/// <c><see cref="YataGrid"/></c> so that the Rowedit its don't go
		/// borky.
		/// </summary>
		internal static bool _bypassEnableRowedit;

		internal int _id;
		internal int _id_presort; // for tracking sorted position by UndoRedo; is set in YataGrid.ColSort()

		readonly YataGrid _grid;

		internal Cell[] _cells;

		/// <summary>
		/// Gets/sets the cell at pos <c>[</c><paramref name="c"/><c>]</c>.
		/// </summary>
		internal Cell this[int c]
		{
			get { return _cells[c]; }
			set { _cells[c] = value; }
		}

		internal int Length
		{ get; set; }

		internal Brush _brush;

		bool _selected;
		/// <summary>
		/// Flags this <c>Row</c> as selected. The setter clears
		/// <c><see cref="YataGrid.RangeSelect">YataGrid.RangeSelect</see></c>
		/// and calls
		/// <c><see cref="YataForm.EnableRoweditOperations()">YataForm.EnableRoweditOperations()</see></c>
		/// iff <c><see cref="_bypassEnableRowedit"/></c> is <c>false</c>.
		/// </summary>
		/// <remarks>Only 1 <c>Row</c> shall ever be <c>selected</c>.</remarks>
		internal bool selected
		{
			get { return _selected; }
			set
			{
				_selected = value;
				_grid.RangeSelect = 0;

				if (!_bypassEnableRowedit)
					_grid._f.EnableRoweditOperations();
			}
		}
		#endregion Fields and Properties


		#region cTors
		/// <summary>
		/// cTor[1].
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cols"></param>
		/// <param name="brush"></param>
		/// <param name="grid"></param>
		internal Row(int id, int cols, Brush brush, YataGrid grid)
		{
			_id = id;
			_cells = new Cell[Length = cols];
			_brush = brush;
			_grid = grid;
		}

		/// <summary>
		/// cTor[2].
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cells"></param>
		/// <param name="brush"></param>
		/// <param name="grid"></param>
		/// <remarks>Used by <c><see cref="Clone()">Clone()</see></c>.</remarks>
		internal Row(int id, Cell[] cells, Brush brush, YataGrid grid)
		{
			_id = id;
			Length = (_cells = cells).Length;
			_brush = brush;
			_grid = grid;
		}
		#endregion cTors


		#region ICloneable requirements
		public object Clone()
		{
			var cells = new Cell[Length];
			for (int i = 0; i != Length; ++i)
				cells[i] = _cells[i].Clone() as Cell;

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
		internal enum CellState
		{
			Default,
			Selected,
			LoadChanged,
			Diff
		}

		/// <summary>
		/// <c><see cref="state"/></c> is used only for priority of brush-color
		/// by <c><see cref="getBrush()">getBrush()</see></c>.
		/// </summary>
		internal CellState state;


		internal string text; // the cell's text

		bool _selected;
		internal bool selected
		{
			get { return _selected; }
			set
			{
				_selected = value;
				SetState();

//				YataForm.that.EnableCelleditOperations();
			}
		}

		bool _loadchanged;
		/// <summary>
		/// Flags this <c>Cell</c> as loadchanged. The setter can call
		/// <c><see cref="YataForm.EnableGotoLoadchanged()">YataForm.EnableGotoLoadchanged()</see></c>.
		/// </summary>
		internal bool loadchanged
		{
			get { return _loadchanged; }
			set
			{
				_loadchanged = value;
				SetState();

				if (!YataGrid._init)
				{
					if (_loadchanged)
						YataForm.that.EnableGotoLoadchanged(true);
					else
						YataForm.that.EnableGotoLoadchanged(YataForm.Table.anyLoadchanged());
				}
				// else selecting the tab at initial load/reload deters 'it_GotoLoadchanged.Enabled'.
			}
		}

		bool _diff;
		internal bool diff
		{
			get { return _diff; }
			set { _diff = value; SetState(); }
		}

		internal int x;
		internal int y;
		internal int y_presort; // for tracking sorted position by UndoRedo; is set in YataGrid.ColSort()

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


		#region Methods
		/// <summary>
		/// Sets <c><see cref="state"/></c> - priority of background
		/// brush-color.
		/// </summary>
		void SetState()
		{
			if      (selected)    state = CellState.Selected;		// highest priority
			else if (diff)        state = CellState.Diff;			// 2nd priority
			else if (loadchanged) state = CellState.LoadChanged;	// 3rd priority
			else                  state = CellState.Default;		// default.
		}

		/// <summary>
		/// Gets a brush for the background color of this <c>Cell</c>.
		/// </summary>
		/// <param name="edit"><c>true</c> if cell is in edit</param>
		/// <returns></returns>
		/// <remarks>Check that <c><see cref="state"/></c> is not
		/// <c><see cref="CellState.Default">CellState.Default</see></c> before
		/// call.
		/// 
		/// 
		/// Called by
		/// <list type="bullet">
		/// <item><c><see cref="YataGrid"/>.OnPaint()</c></item>
		/// <item><c><see cref="YataGrid.PaintFrozenPanel()">YataGrid.PaintFrozenPanel()</see></c></item>
		/// </list></remarks>
		internal Brush getBrush(bool edit = false)
		{
			switch (state)
			{
				case CellState.Selected:
					if (edit) return Brushes.Editor;
					return Brushes.Selected;

				case CellState.Diff:
					return Brushes.Diff;

				case CellState.LoadChanged:
					return Brushes.LoadChanged;
			}
			return null; // shall never happen.
		}
		#endregion Methods


		#region ICloneable requirements
		/// <summary>
		/// Clones this <c>Cell</c>.
		/// </summary>
		/// <returns>a cloned <c>Cell</c> as an <c>object</c></returns>
		public object Clone()
		{
			var cell = new Cell(y,x, String.Copy(text));
			cell._loadchanged = _loadchanged; // don't set 'loadchanged' w/ the property setter.
			cell._widthtext   = _widthtext;

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
							   + ". diff= {6}{0}"
							   + ". _widthtext= {7}{0}"
							   + ". state= {8}{0}"
							   + ". y_presort= {9}",
								 Environment.NewLine,	// 0
								 text,					// 1
								 y,						// 2
								 x,						// 3
								 selected,				// 4
								 loadchanged,			// 5
								 diff,					// 6
								 _widthtext,			// 7
								 state,					// 8
								 y_presort);			// 9
		} */
	}
}
