using System;
using System.Drawing;


namespace yata
{
	/// <summary>
	/// Contains data about a col.
	/// </summary>
	sealed class Col
	{
		#region Fields
		internal string text; // the colhead text

		/// <summary>
		/// Flags this <c>Col</c> as selected.
		/// </summary>
		/// <remarks>Only 1 <c>Col</c> shall ever be <c>selected</c>.</remarks>
		internal bool selected;

		internal int _widthtext;
		internal int Width;
		#endregion Fields


		#region Properties
		internal bool UserSized
		{ get; set; }
		#endregion Properties


		#region Methods
		/// <summary>
		/// Sets <c><see cref="Width"/></c>.
		/// </summary>
		/// <param name="w">the width in pixels</param>
		/// <param name="allowDecrease"><c>true</c> to allow decreasing the width</param>
		/// <remarks>When loading a 2da width is set to the colhead first, but
		/// then can be replaced by the widest cell-width under it. The col's
		/// width is thus allowed to only increase when loading a 2da. When a
		/// cell's text changes however the col-width should be allowed to
		/// decrease if it's less wide than the current width (but not if it's
		/// less wide than the colhead's width). Hence the
		/// <paramref name="allowDecrease"/> parameter.</remarks>
		internal void SetWidth(int w, bool allowDecrease = false)
		{
			if (allowDecrease || w > Width)
				Width = w;
		}
		#endregion Methods
	}



	/// <summary>
	/// Contains data about a row.
	/// </summary>
	sealed class Row
		: ICloneable
	{
		#region Fields (static)
		/// <summary>
		/// Set <c>BypassEnableRowedit</c> <c>true</c> when clearing
		/// <c><see cref="selected"/></c> for a synced
		/// <c><see cref="YataGrid"/></c> so that the Rowedit its don't go
		/// borky.
		/// </summary>
		internal static bool BypassEnableRowedit;
		#endregion Fields (static)


		#region Fields
		internal int _id;
		internal int _id_presort; // for tracking sorted position by UndoRedo; is set in YataGrid.ColSort()

		readonly YataGrid _grid;

		internal Cell[] _cells;

		internal Brush _brush;
		#endregion Fields


		#region Properties
		/// <summary>
		/// The count of fields in this <c>Row</c>.
		/// </summary>
		internal int Length
		{ get; set; }


		bool _selected;
		/// <summary>
		/// Flags this <c>Row</c> as selected. The setter clears
		/// <c><see cref="YataGrid.RangeSelect">YataGrid.RangeSelect</see></c>
		/// and calls
		/// <c><see cref="Yata.EnableRoweditOperations()">Yata.EnableRoweditOperations()</see></c>
		/// iff <c><see cref="BypassEnableRowedit"/></c> is <c>false</c>.
		/// </summary>
		/// <remarks>Only 1 <c>Row</c> shall ever be <c>selected</c>.</remarks>
		internal bool selected
		{
			get { return _selected; }
			set
			{
				_selected = value;
				_grid.RangeSelect = 0;

				if (!BypassEnableRowedit) _grid._f.EnableRoweditOperations();
			}
		}
		#endregion Properties


		#region Indexers
		/// <summary>
		/// Gets/sets the <c><see cref="Cell"/></c> at pos
		/// <c>[</c><paramref name="c"/><c>]</c>.
		/// </summary>
		internal Cell this[int c]
		{
			get { return _cells[c]; }
			set { _cells[c] = value; }
		}
		#endregion Indexers


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
		: ICloneable
	{
		#region Enums
		internal enum CellState
		{
			Default,
			Selected,
			LoadChanged,
			Diff,
			Replaced
		}
		#endregion Enums


		#region Fields
		/// <summary>
		/// This <c>Cell's</c> text.
		/// </summary>
		internal string text;

		internal int x;
		internal int y;
		internal int y_presort; // for tracking sorted position by UndoRedo; is set in YataGrid.ColSort()

		internal int _widthtext;

		/// <summary>
		/// <c><see cref="state"/></c> is used only for priority of brush-color
		/// by <c><see cref="getBrush()">getBrush()</see></c>.
		/// </summary>
		internal CellState state;
		#endregion Fields


		#region Properties
		bool _selected;
		/// <summary>
		/// Flags this <c>Cell</c> as selected.
		/// </summary>
		internal bool selected
		{
			get { return _selected; }
			set
			{
				_selected = value;
				SetState();

//				Yata.that.EnableCelleditOperations();
			}
		}

		bool _loadchanged;
		/// <summary>
		/// Flags this <c>Cell</c> as loadchanged. The setter can call
		/// <c><see cref="Yata.EnableGotoLoadchanged()">Yata.EnableGotoLoadchanged()</see></c>.
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
					Yata.that.EnableGotoLoadchanged(_loadchanged || Yata.Table.anyLoadchanged());
				}
				// else selecting the tab at initial load/reload deters 'it_GotoLoadchanged*.Enabled'.
			}
		}

		bool _diff;
		/// <summary>
		/// Flags this <c>Cell</c> as diffed.
		/// </summary>
		internal bool diff
		{
			get { return _diff; }
			set { _diff = value; SetState(); }
		}

		bool _replaced;
		/// <summary>
		/// Flags this <c>Cell</c> as replaced by
		/// <c><see cref="ReplaceTextDialog"/></c>.
		/// </summary>
		internal bool replaced
		{
			get { return _replaced; }
			set
			{
				_replaced = value;
				SetState();

				// (_replaced=true) happens only in ReplaceTextDialog where
				// EnableGotoReplaced() is taken care of if so ... it also
				// happens in Undo/Redo but it's taken care of there too.
				if (!_replaced && !YataGrid._init)
				{
					Yata.that.EnableGotoReplaced(Yata.Table.anyReplaced());
				}
			}
		}
		#endregion Properties


		#region cTor
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
		#endregion cTor


		#region Methods
		/// <summary>
		/// Sets <c><see cref="state"/></c> - priority of background
		/// brush-color.
		/// </summary>
		void SetState()
		{
			if      (selected)    state = CellState.Selected;		// highest priority
			else if (replaced)    state = CellState.Replaced;		// 2nd priority
			else if (diff)        state = CellState.Diff;			// 3rd priority
			else if (loadchanged) state = CellState.LoadChanged;	// 4th priority
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
					if (edit) return ColorOptions._celledit;
					return ColorOptions._cellselected;

				case CellState.Diff:
					return ColorOptions._celldiffed;

				case CellState.LoadChanged:
					return ColorOptions._cellloadchanged;

				case CellState.Replaced:
					return ColorOptions._cellreplaced;
			}
			return null; // shall never happen.
		}
		#endregion Methods


		#region ICloneable requirements
		public object Clone()
		{
			var cell = new Cell(y,x, String.Copy(text));
			cell._loadchanged = _loadchanged; // don't set 'loadchanged' w/ the property setter.

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
