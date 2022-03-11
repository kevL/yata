using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	sealed partial class YataForm
	{
		#region Fields
		/// <summary>
		/// Tracks the row-id during single-row edit operations via the context.
		/// </summary>
		int _r;
		#endregion Fields


		#region Methods (row)
		/// <summary>
		/// Shows the RMB-context on the rowhead for single-row edit operations.
		/// </summary>
		/// <param name="r"></param>
		/// <remarks><c><see cref="_contextRo"/></c> is not assigned to a
		/// <c><see cref="YataGrid"/>._panelRows'</c> <c>ContextMenuStrip</c>
		/// or <c>ContextMenu</c>.</remarks>
		internal void ShowRowContext(int r)
		{
			_r = r;

			rowit_Header.Text = "_row @ id " + _r;

			rowit_PasteAbove .Enabled =
			rowit_Paste      .Enabled =
			rowit_PasteBelow .Enabled = !Table.Readonly && _copyr.Count != 0;

			rowit_Cut        .Enabled =
			rowit_CreateAbove.Enabled =
			rowit_Clear      .Enabled =
			rowit_CreateBelow.Enabled =
			rowit_Delete     .Enabled = !Table.Readonly;

			Point loc;
			if (Settings._context)							// static location
			{
				loc = new Point(YataGrid.WidthRowhead,
								YataGrid.HeightColhead);
			}
			else											// cursor location
				loc = Table.PointToClient(Cursor.Position);

			_contextRo.Show(Table, loc);
		}
		#endregion Methods (row)


		#region Handlers (row)
		/// <summary>
		/// Handles context-click on the context-header.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_Header"/></c></param>
		/// <param name="e"></param>
		void rowclick_Header(object sender, EventArgs e)
		{
			_contextRo.Hide();
		}

		/// <summary>
		/// Handles context-click to cut a row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_Cut"/></c></param>
		/// <param name="e"></param>
		void rowclick_Cut(object sender, EventArgs e)
		{
			rowclick_Copy(  sender, e);
			rowclick_Delete(sender, e);
		}

		/// <summary>
		/// Handles context-click to copy a row and enables
		/// <c><see cref="it_PasteRange"/></c> and
		/// <c><see cref="it_ClipExport"/></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="rowit_Copy"/></c></item>
		/// <item><c><see cref="rowit_Cut"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void rowclick_Copy(object sender, EventArgs e)
		{
			_copyr.Clear();

			var fields = new string[Table.ColCount];
			for (int c = 0; c != Table.ColCount; ++c)
				fields[c] = Table[_r,c].text;

			_copyr.Add(fields);

			if (_fclip != null)
				_fclip.SetRowsBufferText();

			it_PasteRange.Enabled = !Table.Readonly;
			it_ClipExport.Enabled = true;
		}

		/// <summary>
		/// Handles context-click to paste above the current row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_PasteAbove"/></c></param>
		/// <param name="e"></param>
		void rowclick_PasteAbove(object sender, EventArgs e)
		{
			Table.Insert(_r, _copyr[0]);


			Restorable rest = UndoRedo.createRow(Table.Rows[_r], UndoRedo.UrType.rt_Delete);
			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			Table._ur.Push(rest);

			if (Settings._autorder && order() != 0) layout();
		}

		/// <summary>
		/// Handles context-click to paste into the current row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_Paste"/></c></param>
		/// <param name="e"></param>
		void rowclick_Paste(object sender, EventArgs e)
		{
			// - store the row's current state to 'rPre' in the Restorable
			Restorable rest = UndoRedo.createRow(Table.Rows[_r]);


			YataGrid._init = true; // bypass EnableGotoLoadchanged() in Cell.setter_loadchanged

			Row row = Table.Rows[_r];

			int c = 0;
			if (Settings._autorder)
			{
				row[c].text = _r.ToString(CultureInfo.InvariantCulture);
				row[c].diff =
				row[c].loadchanged = false;

				++c;
			}

			for (; c != Table.ColCount; ++c)
			{
				if (c < _copyr[0].Length)
					row[c].text = _copyr[0][c];
				else
					row[c].text = gs.Stars; // TODO: perhaps keep any remaining cells as they are.

				row[c].diff =
				row[c].loadchanged = false;
			}
			row._brush = Brushes.Created;

			YataGrid._init = false;
			EnableGotoLoadchanged(Table.anyLoadchanged());

			Table.Calibrate(_r);

			Table.Invalidator(YataGrid.INVALID_GRID);


			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}

			// - store the row's changed state to 'rPos' in the Restorable
			rest.rPos = Table.Rows[_r].Clone() as Row;
			Table._ur.Push(rest);
		}

		/// <summary>
		/// Handles context-click to paste below the current row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_PasteBelow"/></c></param>
		/// <param name="e"></param>
		void rowclick_PasteBelow(object sender, EventArgs e)
		{
			Table.Insert(_r + 1, _copyr[0]);


			Restorable rest = UndoRedo.createRow(Table.Rows[_r + 1], UndoRedo.UrType.rt_Delete);
			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			Table._ur.Push(rest);

			if (Settings._autorder && order() != 0) layout();
		}

		/// <summary>
		/// Handles context-click to create a row above the current row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_CreateAbove"/></c></param>
		/// <param name="e"></param>
		void rowclick_CreateAbove(object sender, EventArgs e)
		{
			var fields = new string[Table.ColCount];
			for (int c = 0; c != Table.ColCount; ++c)
				fields[c] = gs.Stars;

			Table.Insert(_r, fields);


			Restorable rest = UndoRedo.createRow(Table.Rows[_r], UndoRedo.UrType.rt_Delete);
			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			Table._ur.Push(rest);

			if (Settings._autorder && order() != 0) layout();
		}

		/// <summary>
		/// Handles context-click to clear the current row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_Clear"/></c></param>
		/// <param name="e"></param>
		void rowclick_Clear(object sender, EventArgs e)
		{
			// - store the row's current state to 'rPre' in the Restorable
			Restorable rest = UndoRedo.createRow(Table.Rows[_r]);


			YataGrid._init = true; // bypass EnableGotoLoadchanged() in Cell.setter_loadchanged

			Row row = Table.Rows[_r];

			int c = 0;
			if (Settings._autorder)
			{
				row[c].text = _r.ToString(CultureInfo.InvariantCulture);
				row[c].diff =
				row[c].loadchanged = false;

				++c;
			}

			for (; c != Table.ColCount; ++c)
			{
				row[c].text = gs.Stars;
				row[c].diff =
				row[c].loadchanged = false;
			}
			row._brush = Brushes.Created;

			YataGrid._init = false;
			EnableGotoLoadchanged(Table.anyLoadchanged());

			Table.Calibrate(_r);

			Table.Invalidator(YataGrid.INVALID_GRID);


			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}

			// - store the row's changed state to 'rPos' in the Restorable
			rest.rPos = Table.Rows[_r].Clone() as Row;
			Table._ur.Push(rest);
		}

		/// <summary>
		/// Handles context-click to create a row below the current row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_CreateBelow"/></c></param>
		/// <param name="e"></param>
		void rowclick_CreateBelow(object sender, EventArgs e)
		{
			var fields = new string[Table.ColCount];
			for (int c = 0; c != Table.ColCount; ++c)
				fields[c] = gs.Stars;

			Table.Insert(_r + 1, fields);


			Restorable rest = UndoRedo.createRow(Table.Rows[_r + 1], UndoRedo.UrType.rt_Delete);
			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			Table._ur.Push(rest);

			if (Settings._autorder && order() != 0) layout();
		}

		/// <summary>
		/// Handles context-click to delete the current row.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="rowit_Delete"/></c></item>
		/// <item><c><see cref="rowit_Cut"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void rowclick_Delete(object sender, EventArgs e)
		{
			Restorable rest = UndoRedo.createRow(Table.Rows[_r], UndoRedo.UrType.rt_Insert);


			Table.Delete(_r);

			EnableRoweditOperations();


			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			Table._ur.Push(rest);

			if (Settings._autorder && order() != 0) layout();
		}
		#endregion Handlers (row)
	}
}
