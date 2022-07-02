using System;
using System.Collections.Generic;
using System.Globalization;


namespace yata
{
	/// <summary>
	/// A static service that sorts the <c><see cref="Row">Rows</see></c> of a
	/// <c><see cref="YataGrid"/></c> given a col-id and a sort-direction.
	/// </summary>
	static class Sorter
	{
		#region Fields (static)
		static int _sortcol, _sortdir;

		static string _a,  _b;
		static int    _ai, _bi;
		static float  _af, _bf;
		#endregion Fields (static)


		#region Methods (static)
		/// <summary>
		/// Starts a top down merge sort.
		/// </summary>
		/// <param name="insitu">the original <c>List</c> of
		/// <c><see cref="Row">Rows</see></c> to sort</param>
		/// <param name="sortcol">the col-id to sort</param>
		/// <param name="sortdir">the direction to sort</param>
		/// <remarks>https://en.wikipedia.org/wiki/Merge_sort#Top-down_implementation</remarks>
		internal static void TopDownMergeSort(IList<Row> insitu, int sortcol, int sortdir)
		{
			_sortcol = sortcol;
			_sortdir = sortdir;

			var r = new List<Row>();
			int total = insitu.Count;

			for (int i = 0; i != total; ++i)
				r.Add(insitu[i]);

			TopDownSplitMerge(r, 0, total, insitu);
		}

		/// <summary>
		/// Recursive funct that shuffles through the <c>Lists</c>.
		/// </summary>
		/// <param name="r">a <c>List</c> of <c><see cref="Row">Rows</see></c>
		/// to sort</param>
		/// <param name="beg">the start row-id</param>
		/// <param name="end">the stop row-Id</param>
		/// <param name="insitu">the <c>List</c> of <c>Rows</c> that sorted
		/// entries get placed into</param>
		/// <remarks>Note that <paramref name="r"/> and
		/// <paramref name="insitu"/> toggle back and forth.</remarks>
		static void TopDownSplitMerge(IList<Row> r, int beg, int end, IList<Row> insitu)
		{
			if (end - beg >= 2)
			{
				int mid = (end + beg) / 2;

				TopDownSplitMerge(insitu, beg, mid, r);
				TopDownSplitMerge(insitu, mid, end, r);

				TopDownMerge(r, beg, mid, end, insitu);
			}
		}

		/// <summary>
		/// Shuffles through the <c>Lists</c> using spooky action at a distance.
		/// </summary>
		/// <param name="r">a <c>List</c> of <c><see cref="Row">Rows</see></c>
		/// to sort</param>
		/// <param name="beg">row-id</param>
		/// <param name="mid">row-id</param>
		/// <param name="end">row-id</param>
		/// <param name="insitu">the <c>List</c> of <c>Rows</c> that sorted
		/// entries get placed into</param>
		static void TopDownMerge(IList<Row> r, int beg, int mid, int end, IList<Row> insitu)
		{
			int i0 = beg, i1 = mid;

			for (int i = beg; i != end; ++i)
			{
				if (i0 < mid && (i1 >= end || Sort(r[i0], r[i1]) == _sortdir))
				{
					insitu[i] = r[i0]; ++i0;
				}
				else
				{
					insitu[i] = r[i1]; ++i1;
				}
			}
		}


		/// <summary>
		/// Sorts fields as integers iff they convert to integers, or floats as
		/// floats, else as strings and performs a secondary sort against their
		/// IDs if applicable.
		/// </summary>
		/// <param name="r1">the value of the reference to a 'Row'</param>
		/// <param name="r2">the value of the reference to a 'Row'</param>
		/// <returns>
		/// <list type="bullet">
		/// <item>-1 : first is first, second is second</item>
		/// <item>0 : identical</item>
		/// <item>1 : first is second, second is first</item>
		/// </list></returns>
		static int Sort(Row r1, Row r2)
		{
			_a = r1[_sortcol].text;
			_b = r2[_sortcol].text;

			if (!Settings._casesort)
			{
				_a = _a.ToUpper(CultureInfo.CurrentCulture);
				_b = _b.ToUpper(CultureInfo.CurrentCulture);
			}

			int result;

			bool a_isStars = (_a == gs.Stars);
			bool b_isStars = (_b == gs.Stars);

			if (a_isStars && b_isStars) // sort stars last.
			{
				result = 0;
			}
			else if (a_isStars && !b_isStars)
			{
				result = 1;
			}
			else if (!a_isStars && b_isStars)
			{
				result = -1;
			}
			else
			{
				bool a_isInt = Int32.TryParse(_a, out _ai);
				bool b_isInt = Int32.TryParse(_b, out _bi);

				if (a_isInt && !b_isInt) // order ints before floats/strings
				{
					result = -1;
				}
				else if (!a_isInt && b_isInt) // order ints before floats/strings
				{
					result = 1;
				}
				else if (a_isInt && b_isInt) // try int comparision
				{
					result = _ai.CompareTo(_bi);
				}
				else if (!_a.Contains(",") && !_b.Contains(",") // NOTE: ... how any library can convert (eg) "1,8,0,0,0" into "18000" and "0,3,10,0,0" to "31000" ...
					&& Single.TryParse(_a, out _af) // try float comparison
					&& Single.TryParse(_b, out _bf))
				{
					result = _af.CompareTo(_bf);
				}
				else
					result = String.CompareOrdinal(_a,_b); // else do string comparison
			}

			if (result == 0 && _sortcol != 0 // secondary sort on id if primary sort matches
				&& Int32.TryParse(r1[0].text, out _ai)
				&& Int32.TryParse(r2[0].text, out _bi))
			{
				result = _ai.CompareTo(_bi);
			}

			if (result < 0) return  1; // NOTE: These vals are reversed for Mergesort.
			if (result > 0) return -1;

			return 0;
		}
		#endregion Methods (static)


/*		void ColSort(int c) // Bubblesort -> hint: don't bother
		{
			if (_sortdir != 1 || _sortcol != c)
				_sortdir = 1;
			else
				_sortdir = -1;

			_sortcol = c;

			bool stop, changed = false;
			Row rowT, row;

			if (_sortdir == 1)
			{
				for (int sort = 0; sort != RowCount; ++sort)
				{
					stop = true;
					for (int r = 0; r != RowCount - 1; ++r)
					{
						if (Sort(Rows[r], Rows[r+1], c) > 0)
						{
							stop = false;
							changed = true;

							rowT = Rows[r];

							row = (Rows[r] = Rows[r+1]);
							row._id = r+1;
							foreach (var cell in row.cells)
								cell.y = r+1;

							row = (Rows[r+1] = rowT);
							row._id = r;
							foreach (var cell in row.cells)
								cell.y = r;
						}
					}
					if (stop) break;
				}
			}
			else // _sortdir == -1
			{
				for (int sort = 0; sort != RowCount; ++sort)
				{
					stop = true;
					for (int r = 0; r != RowCount - 1; ++r)
					{
						if (Sort(Rows[r], Rows[r+1], c) < 0)
						{
							stop = false;
							changed = true;

							rowT = Rows[r];

							row = (Rows[r] = Rows[r+1]);
							row._id = r+1;
							foreach (var cell in row.cells)
								cell.y = r+1;

							Rows[r+1] = rowT;
							row._id = r;
							foreach (var cell in row.cells)
								cell.y = r;
						}
					}
					if (stop) break;
				}
			}
			Changed = changed;
		} */
	}
}
