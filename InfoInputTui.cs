using System;


namespace yata
{
	/// <summary>
	/// A class-object for populating the 'cbx_Val' dropdown-list in
	/// InfoInputSpells or InfoInputFeat.
	/// @note 'tui' stands for ... "A class-object for populating the 'cbx_Val'
	/// dropdown-list".
	/// </summary>
	sealed class tui
	{
		/// <summary>
		/// Appears as the text of this item.
		/// </summary>
		string Label
		{ get; set; }

		/// <summary>
		/// Constructs an item for populating the 'cbx_Val' dropdown-list.
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
