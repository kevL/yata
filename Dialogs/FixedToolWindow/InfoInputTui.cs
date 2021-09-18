using System;


namespace yata
{
	/// <summary>
	/// A class-object for populating the <c>cbx_Val</c> dropdown-list in
	/// <c><see cref="InfoInputSpells"/></c> or
	/// <c><see cref="InfoInputFeat"/></c>.
	/// </summary>
	/// <remarks><c>tui</c> stands for ... "A class-object for populating the
	/// <c>cbx_Val</c> dropdown-list".</remarks>
	sealed class tui
	{
		/// <summary>
		/// Appears as the text of this item.
		/// </summary>
		string Label
		{ get; set; }

		/// <summary>
		/// Constructs an item for populating the <c>cbx_Val</c> dropdown-list.
		/// </summary>
		/// <param name="label"></param>
		internal tui(string label)
		{
			Label = label;
		}

		/// <summary>
		/// Required.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Label;
		}
	}
}
