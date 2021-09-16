using System;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="InfoInputSpells"><c>InfoInputSpells</c></seealso>
	sealed partial class InfoInputFeat
		: Form
	{
		#region Fields (static)
		internal const int Category        = 25; // col in Feat.2da ->
		internal const int MasterFeat      = 32;
		internal const int ToolsCategories = 47;
		internal const int ToggleMode      = 57;
		#endregion Fields (static)


		#region Fields
		YataForm _f;
		YataGrid _grid;
		Cell _cell;

		bool _init;

		CheckBox _cb;
		#endregion Fields


		#region cTor
		/// <summary>
		/// A dialog for the user to input <c>Feat.2da</c> info.
		/// </summary>
		/// <param name="grid"></param>
		/// <param name="cell"></param>
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

					case ToolsCategories: // string-val,checkbox,unique // TODO: change 'ToolsCategories' selection to int-val,dropdown,unique
						_f.str0 = _f.str1 = val;
						vis_ToolsCategories();

						switch (val)
						{
							case "0": cb_00.Checked = true; break;
							case "1": cb_01.Checked = true; break;
							case "2": cb_02.Checked = true; break;
							case "3": cb_03.Checked = true; break;
							case "4": cb_04.Checked = true; break;
							case "5": cb_05.Checked = true; break;
							case "6": cb_06.Checked = true; break;

							case gs.Stars: break;

							default: _f.str1 = gs.Stars; break;
						}
						btn_Clear.Enabled = ((lbl_Val.Text = _f.str1) != gs.Stars);
						break;

					case ToggleMode:
						list_CombatModes();

						initintvals(val);
						break;
				}
			}
			_init = false;
		}


		void vis_ToolsCategories()
		{
			Text = " ToolsCategories";

			cb_00.Text = "0 - All Feats";
			cb_01.Text = "1 - Combat Feats";
			cb_02.Text = "2 - Active Combat Feats";
			cb_03.Text = "3 - Defensive Feats";
			cb_04.Text = "4 - Magical Feats";
			cb_05.Text = "5 - Class/Racial Feats";
			cb_06.Text = "6 - Other Feats";

			cb_00.Visible = cb_01.Visible = cb_02.Visible = cb_03.Visible =
			cb_04.Visible = cb_05.Visible = cb_06.Visible = true;
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

		void list_CombatModes()
		{
			Text = " ToggleMode";

			dropdown();

			for (int i = 0; i != Info.combatmodeLabels.Count; ++i)
			{
				cbx_Val.Items.Add(new tui(i + " - " + Info.combatmodeLabels[i]));
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
		/// <summary>
		/// Handles user changing a checkbox.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void changed_Checkbox(object sender, EventArgs e)
		{
			if (!_init)
			{
				_cb = sender as CheckBox;

				switch (_cell.x)
				{
					case ToolsCategories: change_ToolsCategories(); break;
				}
			}
		}

		/// <summary>
		/// helper for changed_Checkbox()
		/// </summary>
		void change_ToolsCategories()
		{
			string val = gs.Stars;

			_init = true;
			if (_cb == cb_00)
			{
				if (_cb.Checked)
				{
					val = "0";
					cb_01.Checked = cb_02.Checked = cb_03.Checked = cb_04.Checked =
					cb_05.Checked = cb_06.Checked = false;
				}
			}
			else if (_cb == cb_01)
			{
				if (_cb.Checked)
				{
					val = "1";
					cb_00.Checked = cb_02.Checked = cb_03.Checked = cb_04.Checked =
					cb_05.Checked = cb_06.Checked = false;
				}
			}
			else if (_cb == cb_02)
			{
				if (_cb.Checked)
				{
					val = "2";
					cb_00.Checked = cb_01.Checked = cb_03.Checked = cb_04.Checked =
					cb_05.Checked = cb_06.Checked = false;
				}
			}
			else if (_cb == cb_03)
			{
				if (_cb.Checked)
				{
					val = "3";
					cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_04.Checked =
					cb_05.Checked = cb_06.Checked = false;
				}
			}
			else if (_cb == cb_04)
			{
				if (_cb.Checked)
				{
					val = "4";
					cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
					cb_05.Checked = cb_06.Checked = false;
				}
			}
			else if (_cb == cb_05)
			{
				if (_cb.Checked)
				{
					val = "5";
					cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
					cb_04.Checked = cb_06.Checked = false;
				}
			}
			else //if (_cb == cb_06)
			{
				if (_cb.Checked)
				{
					val = "6";
					cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
					cb_04.Checked = cb_05.Checked = false;
				}
			}
			_init = false;

			lbl_Val.Text = _f.str1 = val;
			btn_Clear.Enabled = (val != gs.Stars);
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
						case ToggleMode:
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
				case ToggleMode: // int,dropdown,unique
					cbx_Val.SelectedIndex = cbx_Val.Items.Count - 1; // fire changed_Combobox()
					break;

				case ToolsCategories: // string,checkbox,unique
					btn_Clear.Enabled = false;

					lbl_Val.Text = _f.str1 = gs.Stars;

					_init = true;
					cb_00.Checked = cb_01.Checked = cb_02.Checked = cb_03.Checked =
					cb_04.Checked = cb_05.Checked = cb_06.Checked = false;
					_init = false;
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
