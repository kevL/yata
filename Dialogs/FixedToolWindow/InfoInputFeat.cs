using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="InfoInputSpells"><c>InfoInputSpells</c></seealso>
	/// <seealso cref="InfoInputClasses"><c>InfoInputClasses</c></seealso>
	sealed partial class InfoInputFeat
		: InfoInput
	{
		#region Fields (static)
		internal const int Category        = 25; // col in Feat.2da ->
		internal const int MasterFeat      = 32;
		internal const int ToolsCategories = 47;
		internal const int FeatCategory    = 54;
		internal const int ToggleMode      = 57;
		#endregion Fields (static)


		#region cTor
		/// <summary>
		/// A dialog for the user to input <c>Feat.2da</c> info.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="cell"></param>
		internal InfoInputFeat(Yata f, Cell cell)
		{
			_f    = f;		// don't try to pass these to a base.InfoInput cTor
			_cell = cell;	// because the designer will scream blue murder.

			InitializeComponent();

			// NOTE: Don't bother inheriting from YataDialog since setting the
			// font is the only benefit ->
			if (Settings._font2dialog != null)
				Font = Settings._font2dialog;
			else
				Font = Settings._fontdialog;

			init();
		}
		#endregion cTor


		#region init
		/// <summary>
		/// Initializes the dialog based on the current 2da col.
		/// </summary>
		void init()
		{
			_init = true;

			string val = _cell.text;
			switch (_cell.x)
			{
				case Category: // int-val,dropdown,unique
					list_Categories();

					initintvals(val, cbx_Val, btn_Clear);
					break;

				case MasterFeat: // int-val,dropdown,unique
					list_Masterfeats();

					initintvals(val, cbx_Val, btn_Clear);
					break;

				case ToolsCategories: // string-val,checkbox,unique // TODO: change 'ToolsCategories' selection to int-val,dropdown,unique
					_f.str0 = _f.str1 = val;
					prep_ToolsCategories();

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

				case FeatCategory: // string-val,dropdown,unique
					_f.str0 = _f.str1 = val;
					list_FeatCategories();

					switch (val)
					{
						case gs.FeatCatBackground:    cbx_Val.SelectedIndex =  0; break;
						case gs.FeatCatClassability:  cbx_Val.SelectedIndex =  1; break;
						case gs.FeatCatDivine:        cbx_Val.SelectedIndex =  2; break;
						case gs.FeatCatEpic:          cbx_Val.SelectedIndex =  3; break;
						case gs.FeatCatGeneral:       cbx_Val.SelectedIndex =  4; break;
						case gs.FeatCatHeritage:      cbx_Val.SelectedIndex =  5; break;
						case gs.FeatCatHistory:       cbx_Val.SelectedIndex =  6; break;
						case gs.FeatCatItemCreation:  cbx_Val.SelectedIndex =  7; break;
						case gs.FeatCatMetamagic:     cbx_Val.SelectedIndex =  8; break;
						case gs.FeatCatProficiency:   cbx_Val.SelectedIndex =  9; break;
						case gs.FeatCatRacialability: cbx_Val.SelectedIndex = 10; break;
						case gs.FeatCatSkillSave:     cbx_Val.SelectedIndex = 11; break;
						case gs.FeatCatSpellcasting:  cbx_Val.SelectedIndex = 12; break;
						case gs.FeatCatTeamwork:      cbx_Val.SelectedIndex = 13; break;

						case gs.Stars: cbx_Val.SelectedIndex = cbx_Val.Items.Count - 1;
							break;

						default: _f.str1 = gs.Stars; goto case gs.Stars;
					}
					btn_Clear.Enabled = ((lbl_Val.Text = _f.str1) != gs.Stars);
					break;

				case ToggleMode: // int-val,dropdown,unique
					list_CombatModes();

					initintvals(val, cbx_Val, btn_Clear);
					break;
			}
			_init = false;
		}


		/// <summary>
		/// Prepares this dialog for <c><see cref="ToolsCategories"/></c> input.
		/// </summary>
		void prep_ToolsCategories()
		{
			Text = " TOOLSCATEGORIES";

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
		/// Hides the label and shows the <c>ComboBox</c> for dropdown-lists
		/// instead.
		/// </summary>
		void dropdown()
		{
			lbl_Val.Visible = false;
			cbx_Val.Visible = true;

			ClientSize = new Size(ClientSize.Width,
								  ClientSize.Height - 140);
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="Category"/></c>
		/// (Categories.2da) to the <c>ComboBox</c> along with a final stars
		/// item.
		/// </summary>
		void list_Categories()
		{
			Text = " CATEGORY";

			dropdown();

			for (int i = 0; i != Info.categoryLabels.Count; ++i)
			{
				cbx_Val.Items.Add(new tui(i + " - " + Info.categoryLabels[i].ToLowerInvariant()));
			}
			cbx_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="MasterFeat"/></c>
		/// (MasterFeats.2da) to the <c>ComboBox</c> along with a final stars
		/// item.
		/// </summary>
		void list_Masterfeats()
		{
			Text = " MASTERFEAT";

			dropdown();

			for (int i = 0; i != Info.masterfeatLabels.Count; ++i)
			{
				cbx_Val.Items.Add(new tui(i + " - " + Info.masterfeatLabels[i]));
			}
			cbx_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="FeatCategory"/></c> to the
		/// <c>ComboBox</c> along with a final stars item.
		/// </summary>
		void list_FeatCategories()
		{
			Text = " FeatCategory";

			dropdown();

			cbx_Val.Items.Add(new tui(gs.FeatCatBackground));
			cbx_Val.Items.Add(new tui(gs.FeatCatClassability));
			cbx_Val.Items.Add(new tui(gs.FeatCatDivine));
			cbx_Val.Items.Add(new tui(gs.FeatCatEpic));
			cbx_Val.Items.Add(new tui(gs.FeatCatGeneral));
			cbx_Val.Items.Add(new tui(gs.FeatCatHeritage));
			cbx_Val.Items.Add(new tui(gs.FeatCatHistory));
			cbx_Val.Items.Add(new tui(gs.FeatCatItemCreation));
			cbx_Val.Items.Add(new tui(gs.FeatCatMetamagic));
			cbx_Val.Items.Add(new tui(gs.FeatCatProficiency));
			cbx_Val.Items.Add(new tui(gs.FeatCatRacialability));
			cbx_Val.Items.Add(new tui(gs.FeatCatSkillSave));
			cbx_Val.Items.Add(new tui(gs.FeatCatSpellcasting));
			cbx_Val.Items.Add(new tui(gs.FeatCatTeamwork));

			cbx_Val.Items.Add(new tui(gs.Stars));
		}

		/// <summary>
		/// Adds allowable entries for <c><see cref="ToggleMode"/></c>
		/// (CombatModes.2da) to the <c>ComboBox</c> along with a final stars
		/// item.
		/// </summary>
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
		#endregion init


		#region Handlers
		/// <summary>
		/// Handles user changing a <c>CheckBox</c>.
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
		/// - helper for <c><see cref="changed_Checkbox()">changed_Checkbox()</see></c>.
		/// </summary>
		void change_ToolsCategories()
		{
			clearchecks();

			string val;
			if (_cb.Checked)
			{
				if      (_cb == cb_00) val = "0";
				else if (_cb == cb_01) val = "1";
				else if (_cb == cb_02) val = "2";
				else if (_cb == cb_03) val = "3";
				else if (_cb == cb_04) val = "4";
				else if (_cb == cb_05) val = "5";
				else                   val = "6"; // _cb == cb_06
			}
			else
				val = gs.Stars;

			lbl_Val.Text = _f.str1 = val;
			btn_Clear.Enabled = (val != gs.Stars);
		}

		/// <summary>
		/// Handles user changing the <c>ComboBox</c> selection.
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
					_f.int1 = Yata.II_ASSIGN_STARS;
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

						case FeatCategory:
							switch (cbx_Val.SelectedIndex)
							{
								case  0: _f.str1 = gs.FeatCatBackground;    break;
								case  1: _f.str1 = gs.FeatCatClassability;  break;
								case  2: _f.str1 = gs.FeatCatDivine;        break;
								case  3: _f.str1 = gs.FeatCatEpic;          break;
								case  4: _f.str1 = gs.FeatCatGeneral;       break;
								case  5: _f.str1 = gs.FeatCatHeritage;      break;
								case  6: _f.str1 = gs.FeatCatHistory;       break;
								case  7: _f.str1 = gs.FeatCatItemCreation;  break;
								case  8: _f.str1 = gs.FeatCatMetamagic;     break;
								case  9: _f.str1 = gs.FeatCatProficiency;   break;
								case 10: _f.str1 = gs.FeatCatRacialability; break;
								case 11: _f.str1 = gs.FeatCatSkillSave;     break;
								case 12: _f.str1 = gs.FeatCatSpellcasting;  break;
								case 13: _f.str1 = gs.FeatCatTeamwork;      break;
							}
							break;
					}
				}
			}
		}


		/// <summary>
		/// Handles clicking the Clear <c>Button</c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Clear(object sender, EventArgs e)
		{
			switch (_cell.x)
			{
				case Category: // dropdown -> fire changed_Combobox()
				case MasterFeat:
				case FeatCategory:
				case ToggleMode:
					cbx_Val.SelectedIndex = cbx_Val.Items.Count - 1; // fire changed_Combobox()
					break;

				case ToolsCategories: // str,cb,unique
					btn_Clear.Enabled = false;

					lbl_Val.Text = _f.str1 = gs.Stars;

					_cb = null;
					clearchecks();
					break;
			}
		}
		#endregion Handlers
	}
}
