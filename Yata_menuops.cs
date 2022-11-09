using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Handlers (2daOps)
		/// <summary>
		/// Handles opening the 2daOpsMenu, determines if various items ought be
		/// enabled.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ops_dropdownopening(object sender, EventArgs e)
		{
			it_ClearUr.Enabled =  Table != null
							  && (Table._ur.CanUndo || Table._ur.CanRedo);
		}


		/// <summary>
		/// Handles it-click to order row-ids.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_OrderRows"/></c></item>
		/// <item><c><see cref="it_CheckRows"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Order rows <c>[Ctrl+d]</c></item>
		/// <item>2daOps|Test rows <c>[Ctrl+t]</c>
		/// <c><see cref="opsclick_TestOrder()">opsclick_TestOrder()</see></c></item>
		/// </list></remarks>
		void opsclick_Order(object sender, EventArgs e)
		{
			string title, head; InfoboxType ibt;

			int changed = order();
			if (changed != 0)
			{
				layout();

				title = Infobox.Title_warni;
				head  = changed + " id" + (changed == 1 ? String.Empty : "s") + " corrected.";
				ibt   = InfoboxType.Warn;
			}
			else
			{
				title = Infobox.Title_infor;
				head  = "Row order is Okay - no change.";
				ibt   = InfoboxType.Info;
			}

			using (var ib = new Infobox(title,
										head,
										null,
										ibt))
			{
				ib.ShowDialog(this);
			}
		}

		/// <summary>
		/// Orders row-ids.
		/// </summary>
		/// <returns>the count of changed row-ids</returns>
		static internal int order()
		{
			int changed = 0;

			int result;
			for (int r = 0; r != Table.RowCount; ++r)
			if (!Int32.TryParse(Table[r,0].text, out result)
				|| result != r)
			{
				Table[r,0].text = r.ToString(CultureInfo.InvariantCulture);
				++changed;
			}
			return changed;
		}

		/// <summary>
		/// Lays out table after rows are auto-ordered.
		/// </summary>
		/// <param name="bypassInvalidate"></param>
		internal void layout(bool bypassInvalidate = false)
		{
			DrawRegulator.SuspendDrawing(this);


			if (!Table.Changed)
				 Table.Changed = true;

			Table._ur.ResetSaved(true);

			if      (_diff1 == Table) _diff1 = null;
			else if (_diff2 == Table) _diff2 = null;

			Table.Colwidth(0, 0, Table.RowCount - 1);
			Table.MetricFrozenControls(FROZEN_COL_Id);

			Table.InitScroll();

			if (!bypassInvalidate)
			{
				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
				if (Table.Propanel != null && Table.Propanel.Visible)
					invalid |= YataGrid.INVALID_PROP;

				Table.Invalidator(invalid);
			}

			DrawRegulator.ResumeDrawing(this);
		}

		/// <summary>
		/// Handles it-click to test row-ids.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CheckRows"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Test rows <c>[Ctrl+t]</c></item>
		/// </list></remarks>
		void opsclick_TestOrder(object sender, EventArgs e)
		{
			var borks = new List<string>();

			bool stop = false;

			int result;
			for (int r = 0; r != Table.RowCount; ++r)
			{
				if (!Int32.TryParse(Table[r,0].text, out result))
				{
					if (borks.Count == 16) // stop this Madness
					{
						stop = true;
						break;
					}
					borks.Add("id " + r + " is not an integer");
				}
				else if (result != r)
				{
					if (borks.Count == 16) // stop this Madness
					{
						stop = true;
						break;
					}
					borks.Add("id " + r + " is out of order");
				}
			}

			string title, head;
			string copy = String.Empty;
			InfoboxType ibt;

			if (borks.Count != 0)
			{
				foreach (string bork in borks)
				{
					if (copy.Length != 0) copy += Environment.NewLine;
					copy += bork;
				}

				title = Infobox.Title_warni;
				head  = "Row order is borked.";
				ibt   = InfoboxType.Warn;

				if (!Table.Readonly)
					head += " Do you want to auto-order the ids ...";
			}
			else
			{
				title = Infobox.Title_infor;
				head  = "Row order is Okay.";
				ibt   = InfoboxType.Info;
			}

			using (var ib = new Infobox(title,
										(stop ? "The test has been stopped at 16 borks. " : String.Empty) + head,
										(copy.Length != 0 ? copy + (stop ? Environment.NewLine + "..." : String.Empty) : null),
										ibt,
										(copy.Length != 0 && !Table.Readonly ? InfoboxButtons.CancelYes : InfoboxButtons.Okay)))
			{
				if (ib.ShowDialog(this) == DialogResult.OK)
					opsclick_Order(sender, e);
			}
		}


		/// <summary>
		/// Handles it-click to recolor rows. Also clears all
		/// <c><see cref="Cell.loadchanged">Cell.loadchanged</see></c> flags.
		/// </summary>
		/// <param name="sender"><c><see cref="it_ColorRows"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Recolor rows <c>[Ctrl+l]</c></item>
		/// </list></remarks>
		void opsclick_Recolor(object sender, EventArgs e)
		{
			for (int r = 0; r != Table.RowCount; ++r)
			{
				Table.Rows[r]._brush = (r % 2 == 0) ? Brushes.Alice
													: Brushes.Bob;
			}
			Table.ClearReplaced();
			Table.ClearLoadchanged();

			Table.Invalidator(YataGrid.INVALID_GRID);
		}

		/// <summary>
		/// Handles it-click to autosize cols.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_AutoCols"/></c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Autosize cols <c>[Ctrl+i]</c></item>
		/// <item><c><see cref="DiffReset()">DiffReset()</see></c></item>
		/// </list></remarks>
		internal void opsclick_AutosizeCols(object sender, EventArgs e)
		{
			Obfuscate();
			DrawRegulator.SuspendDrawing(Table);

			AutosizeCols(Table);

			DrawRegulator.ResumeDrawing(Table);
			Obfuscate(false);
		}

		/// <summary>
		/// Autosizes all cols of a given <c><see cref="YataGrid"/></c>.
		/// </summary>
		/// <param name="table"></param>
		/// <remarks>Helper for
		/// <c><see cref="opsclick_AutosizeCols()">opsclick_AutosizeCols()</see></c>
		/// and <c><see cref="DiffReset()">DiffReset()</see></c>.</remarks>
		static void AutosizeCols(YataGrid table)
		{
			foreach (var col in table.Cols)
				col.UserSized = false;

			table.Calibrate(0, table.RowCount - 1);
		}


		/// <summary>
		/// Handles it-click to freeze first col.
		/// </summary>
		/// <param name="sender"><c><see cref="it_freeze1"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Freeze 1st col <c>[F5]</c></item>
		/// </list></remarks>
		void opsclick_Freeze1stCol(object sender, EventArgs e)
		{
			Table.Select();

			it_freeze2.Checked = false;

			if (it_freeze1.Checked = !it_freeze1.Checked)
			{
				Col col = Table.Cols[FROZEN_COL_First];
				if (col.UserSized)
				{
					col.UserSized = false;
					Table.Colwidth(FROZEN_COL_First);
				}
				Table.FrozenCount = YataGrid.FreezeFirst;
			}
			else
				Table.FrozenCount = YataGrid.FreezeId;
		}

		/// <summary>
		/// Handles it-click to freeze second col.
		/// </summary>
		/// <param name="sender"><c><see cref="it_freeze2"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Freeze 2nd col <c>[F6]</c></item>
		/// </list></remarks>
		void opsclick_Freeze2ndCol(object sender, EventArgs e)
		{
			Table.Select();

			it_freeze1.Checked = false;

			if (it_freeze2.Checked = !it_freeze2.Checked)
			{
				Col col = Table.Cols[FROZEN_COL_First];
				if (col.UserSized)
				{
					col.UserSized = false;
					Table.Colwidth(FROZEN_COL_First);
				}

				col = Table.Cols[FROZEN_COL_Second];
				if (col.UserSized)
				{
					col.UserSized = false;
					Table.Colwidth(FROZEN_COL_Second);
				}

				Table.FrozenCount = YataGrid.FreezeSecond;
			}
			else
				Table.FrozenCount = YataGrid.FreezeId;
		}


		/// <summary>
		/// Toggles the <c><see cref="Propanel"/></c> of the current
		/// <c><see cref="Table"/></c> - creates a <c>Propanel</c> if
		/// required.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Propanel"/></c></item>
		/// <item><c><see cref="bu_Propanel"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Propanel <c>[F7]</c></item>
		/// <item><c><see cref="mouseup_buPropanel()">mouseup_buPropanel()</see></c></item>
		/// </list></remarks>
		void opsclick_Propanel(object sender, EventArgs e)
		{
			// TODO: hide Table._editor

			if (Table.Propanel == null
				|| (Table.Propanel.Visible = !Table.Propanel.Visible))
			{
				if (Table.Propanel == null)
					Table.Propanel = new Propanel(Table);
				else
					Table.Propanel.tele();

				Table.Propanel.Show();
				Table.Propanel.BringToFront();

				Table.Propanel.Dockstate = Table.Propanel.Dockstate;

				it_Propanel       .Checked =
				it_PropanelLoc    .Enabled =
				it_PropanelLoc_pre.Enabled = true;
			}
			else
			{
				Table.Propanel.Hide();

				it_Propanel       .Checked =
				it_PropanelLoc    .Enabled =
				it_PropanelLoc_pre.Enabled = false;
			}
		}

		/// <summary>
		/// Cycles the location of the <c><see cref="Propanel"/></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_PropanelLoc"/></c></item>
		/// <item><c><see cref="it_PropanelLoc_pre"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Propanel location<c>[F8]</c></item>
		/// <item>2daOps|Propanel location pre<c>[Shift+F8]</c></item>
		/// </list></remarks>
		/// <seealso cref="mouseup_buPropanel()"><c>mouseup_buPropanel()</c></seealso>
		void opsclick_PropanelLocation(object sender, EventArgs e)
		{
			// TODO: hide Table._editor

			if (Table.Propanel != null && Table.Propanel.Visible)
				Table.Propanel.Dockstate = Table.Propanel.getNextDockstate(sender == it_PropanelLoc_pre);
		}


		/// <summary>
		/// Starts an external diff/merger program with the two diffed files
		/// opened. Usually WinMerge although kdiff3 and perhaps other
		/// file-comparision utilities - their first 2 commandline arguments
		/// must be the fullpaths of the files to compare. If
		/// <c><see cref="_diff1"/></c> or <c><see cref="_diff2"/></c> doesn't
		/// exist then try to start the app with the current
		/// <c><see cref="Table"/></c> loaded.
		/// </summary>
		/// <param name="sender"><c><see cref="it_ExternDiff"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|External diff <c>[Ctrl+m]</c></item>
		/// </list></remarks>
		void opsclick_ExternalDiff(object sender, EventArgs e)
		{
			if (File.Exists(Settings._diff))
			{
				var differ = new Process();
				differ.StartInfo.FileName = Settings._diff;

				if (_diff1 != null && _diff2 != null
					&& File.Exists(_diff1.Fullpath)
					&& File.Exists(_diff2.Fullpath))
				{
					differ.StartInfo.Arguments = " \"" + _diff1.Fullpath + "\" \"" + _diff2.Fullpath + "\"";
				}
				else
					differ.StartInfo.Arguments = " \"" + Table.Fullpath + "\"";

				differ.Start();
			}
		}


		/// <summary>
		/// Clears the Undo/Redo stacks.
		/// </summary>
		/// <param name="sender"><c><see cref="it_ClearUr"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Clear undo/redo</item>
		/// </list></remarks>
		void opsclick_ClearUr(object sender, EventArgs e)
		{
			// after first run (clears ~300..500kb) this appears to clear
			// exactly 0 bytes per Clear.

			long bytes = GetUsage();

			Table._ur.Clear();

			GC.Collect();
			GC.WaitForPendingFinalizers();

			bytes -= GetUsage();

			string head = "Estimated memory freed : " + String.Format(CultureInfo.InvariantCulture, "{0:n0}", bytes) + " bytes";
			using (var ib = new Infobox(Infobox.Title_infor, head))
				ib.ShowDialog(this);
		}
		#endregion Handlers (2daOps)


		#region Methods (2daOps)
		/// <summary>
		/// Determines the dis/enabled states of 2daOps operations.
		/// </summary>
		void Enable2daOperations()
		{
			it_OrderRows      .Enabled = !Table.Readonly;
			it_CheckRows      .Enabled =

			it_ColorRows      .Enabled =
			it_AutoCols       .Enabled = true;

			it_freeze1        .Enabled = Table.ColCount > 1;
			it_freeze2        .Enabled = Table.ColCount > 2;

			it_Propanel       .Enabled = true;
			it_PropanelLoc    .Enabled =
			it_PropanelLoc_pre.Enabled =
			it_Propanel       .Checked = Table.Propanel != null && Table.Propanel.Visible;

			it_ExternDiff     .Enabled = File.Exists(Settings._diff);
		}

		/// <summary>
		/// Gets Yata's current memory usage in bytes.
		/// </summary>
		/// <returns></returns>
		static long GetUsage()
		{
			long bytes;
			using (Process proc = Process.GetCurrentProcess())
			{
				// The proc.PrivateMemorySize64 will return the private memory usage in bytes.
				// - to convert to Megabytes divide it by 2^20
				bytes = proc.PrivateMemorySize64; // / (1024*1024);

//				using (var ib = new Infobox(" bytes used", String.Format("{0:n0}", bytes)))
//					ib.ShowDialog(this);
			}
			return bytes;
		}
		#endregion Methods (2daOps)
	}
}
