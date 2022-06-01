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
		: Form
	{
		#region Fields (static)
		internal const int Category        = 25; // col in Feat.2da ->
		internal const int MasterFeat      = 32;
		internal const int ToolsCategories = 47;
		internal const int ToggleMode      = 57;
		#endregion Fields (static)


		#region Fields
		Yata _f;
		Cell _cell;

		/// <summary>
		/// <c>true</c> bypasses the <c>CheckedChanged</c> handler for
		/// <c>CheckBoxes</c> and the <c>SelectedIndexChanged</c> handler for
		/// the <c>ComboBox</c>.
		/// </summary>
		/// <remarks>Initialization will configure this dialog without invoking
		/// the handlers.</remarks>
		bool _init;

		CheckBox _cb;
		#endregion Fields


		#region cTor
		/// <summary>
		/// A dialog for the user to input <c>Feat.2da</c> info.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="cell"></param>
		internal InfoInputFeat(Yata f, Cell cell)
		{
			_f    = f;
			_cell = cell;

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

					initintvals(val);
					break;

				case MasterFeat: // int-val,dropdown,unique
					list_Masterfeats();

					initintvals(val);
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

				case ToggleMode: // int-val,dropdown,unique
					list_CombatModes();

					initintvals(val);
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


		/// <summary>
		/// Selects an entry in the <c>ComboBox</c> and preps the int-vals in
		/// Yata to deal with user-input.
		/// 
		/// 
		/// - duplicates <c><see cref="InfoInputSpells"/>.initintvals()</c>
		/// 
		/// - duplicates <c><see cref="InfoInputClasses"/>.initintvals()</c>
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
					_f.int0 = Yata.II_ASSIGN_STARS;
				else
					_f.int0 = Yata.II_INIT_INVALID;

				_f.int1 = Yata.II_ASSIGN_STARS;

				cbx_Val.SelectedIndex = cbx_Val.Items.Count - 1;
			}
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
				case Category: // int,dropdown,unique
				case MasterFeat:
				case ToggleMode:
					cbx_Val.SelectedIndex = cbx_Val.Items.Count - 1; // fire changed_Combobox()
					break;

				case ToolsCategories: // string,checkbox,unique
					btn_Clear.Enabled = false;

					lbl_Val.Text = _f.str1 = gs.Stars;

					_cb = null;
					clearchecks();
					break;
			}
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Clears all <c>CheckBoxes</c> except the current <c>CheckBox</c>
		/// <c><see cref="_cb"/></c> (if valid).
		/// 
		/// 
		/// - duplicates <c><see cref="InfoInputSpells"/>.clearchecks()</c>
		/// 
		/// - duplicates <c><see cref="InfoInputClasses"/>.clearchecks()</c>
		/// </summary>
		/// <remarks>Set <c>(_cb = null)</c> to clear all <c>Checkboxes</c>.</remarks>
		void clearchecks()
		{
			_init = true;

			CheckBox cb;
			foreach (var control in Controls)
			{
				if ((cb = control as CheckBox) != null
					&& cb.Checked && (_cb == null || cb != _cb))
				{
					cb.Checked = false;
				}
			}
			_init = false;
		}
		#endregion Methods


		#region Handlers (override)
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
		#endregion Handlers (override)
	}
}
