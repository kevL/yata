using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class InfoInputFeat
		:
			Form
	{
		#region Fields (static)
		// col in Feat.2da ->
//		internal const int PrereqFeat1 = 20; // "PREREQFEAT1"
//		internal const int PrereqFeat2 = 21; // "PREREQFEAT2"

		internal const int Category    = 25; // "CATEGORY"

//		internal const int SpellId     = 27; // "SPELLID"

		internal const int MasterFeat  = 32; // "MASTERFEAT"
		#endregion Fields (static)


		#region Fields
		YataForm _f;
		YataGrid _grid;
		Cell _cell;

		bool _init;

//		CheckBox _cb;
		#endregion Fields


		#region cTor
		internal InfoInputFeat(YataGrid grid, Cell cell)
		{
			InitializeComponent();

			_grid = grid;
			_f    = grid._f;
			_cell = cell;

			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			initdialog();
		}
		#endregion cTor


		#region init
		/// <summary>
		/// Initializes the dialog based on the current 2da col.
		/// </summary>
		void initdialog()
		{
			_init = true;

			string val = _cell.text;
			if (!String.IsNullOrEmpty(val)) // safety.
			{
				switch (_cell.x)
				{
					case Category: // int-val,dropdown,unique
						list_Categories();

						initintvals(val);
						break;

					case MasterFeat: // int-val,dropdown,unique
						list_Masterfeats();

						initintvals(val);
						break;
				}
			}
			_init = false;
		}


		/// <summary>
		/// Hides the label and shows the combobox for lists instead.
		/// </summary>
		void dropdown()
		{
			lbl_Val.Visible = false;
			cbx_Val.Visible = true;
		}


		/// <summary>
		/// Adds allowable entries for "CATEGORY" (Categories.2da) to the
		/// combobox along with a final stars item.
		/// </summary>
		void list_Categories()
		{
			Text = " Category";

			dropdown();

			for (int i = 0; i != Info.categoryLabels.Count; ++i)
			{
				cbx_Val.Items.Add(new tui(i + " - " + Info.categoryLabels[i].ToLower()));
			}
			cbx_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for "MASTERFEAT" (MasterFeats.2da) to the
		/// combobox along with a final stars item.
		/// </summary>
		void list_Masterfeats()
		{
			Text = " MasterFeat";

			dropdown();

			for (int i = 0; i != Info.masterfeatLabels.Count; ++i)
			{
				cbx_Val.Items.Add(new tui(i + " - " + Info.masterfeatLabels[i]));
			}
			cbx_Val.Items.Add(new tui(gs.Stars));
		}


		/// <summary>
		/// Selects an entry in the combobox and preps the int-vals in Yata to
		/// deal with user-input.
		/// - duplicates InfoInputSpells.initintvals()
		/// </summary>
		/// <param name="val"></param>
		void initintvals(string val)
		{
			int result;
			if (Int32.TryParse(val, out result)
				&& result > -1 && result < cbx_Val.Items.Count - 1)
			{
				_f.int0 = _f.int1 = cbx_Val.SelectedIndex = result;
			}
			else
			{
				btn_Clear.Enabled = false;

				if (val == gs.Stars)
					_f.int0 = YataForm.II_ASSIGN_STARS;
				else
					_f.int0 = YataForm.II_INIT_INVALID;

				_f.int1 = YataForm.II_ASSIGN_STARS;

				cbx_Val.SelectedIndex = cbx_Val.Items.Count - 1;
			}
		}
		#endregion init


		#region Events
		void changed_Checkbox(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles user changing a combobox selection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void changed_Combobox(object sender, EventArgs e)
		{
			if (!_init)
			{
				if (cbx_Val.SelectedIndex == cbx_Val.Items.Count - 1)
				{
					btn_Clear.Enabled = false;

					_f.str1 = gs.Stars;
					_f.int1 = YataForm.II_ASSIGN_STARS;
				}
				else
				{
					btn_Clear.Enabled = true;

					switch (_cell.x)
					{
						case Category:
						case MasterFeat:
							_f.int1 = cbx_Val.SelectedIndex;
							break;
					}
				}
			}
		}


		/// <summary>
		/// Handles clicking the Clear button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Clear(object sender, EventArgs e)
		{
			switch (_cell.x)
			{
				case Category:   // int,dropdown,unique
				case MasterFeat: // int,dropdown,unique
					cbx_Val.SelectedIndex = cbx_Val.Items.Count - 1;
					break;
			}
		}


//		void click_Accept(object sender, EventArgs e)
//		{}
		#endregion Events


		#region Methods
		#endregion Methods


		#region Events (override)
		/// <summary>
		/// Closes the dialog on [Esc].
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
				Close();
			else
				base.OnKeyDown(e);
		}
		#endregion Events (override)
	}
}
