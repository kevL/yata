﻿using System;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Creates an object that watches for external/disk file-changed events.
	/// @note Call Dispose() when it goes out of scope: YataForm.CloseTabpage().
	/// </summary>
	sealed class FileWatcher
		:
			Timer
	{
		#region Fields
		readonly YataGrid _grid;
		DateTime _last;
		#endregion Fields


		#region Properties
		internal string Pfe
		{ private get; set; }

		internal bool BypassFileChanged
		{ private get; set; }

		internal bool BypassFileDeleted
		{ private get; set; }
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="grid"></param>
		internal FileWatcher(YataGrid grid)
		{
			_grid = grid;
			Pfe = _grid.Fullpath;
			_last = File.GetLastWriteTime(Pfe);

			Interval = 300;
			Start();
		}
		#endregion cTor


		#region Events (override)
		/// <summary>
		/// Handels this FileWatcher's tick event in C#.
		/// @note Check for a valid YataGrid since disposal of this watcher
		/// could be delayed. See CloseTabpage() where the grid is nulled.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTick(EventArgs e)
		{
			if (_grid != null) // ~safety.
			{
				if (!BypassFileDeleted)
				{
					if (!File.Exists(Pfe))
					{
						BypassFileDeleted = true;

						using (var fwd = new FileWatcherDialog(_grid, FileWatcherDialog.FILE_DEL))
							fwd.ShowDialog(_grid._f);
					}
					else if (!BypassFileChanged
						&& File.GetLastWriteTime(Pfe) != _last)
					{
						_last = File.GetLastWriteTime(Pfe);

						using (var fwd = new FileWatcherDialog(_grid, FileWatcherDialog.FILE_WSC))
							fwd.ShowDialog(_grid._f);
					}
				}

				if (BypassFileChanged)
				{
					BypassFileChanged = false;
					_last = File.GetLastWriteTime(Pfe);
				}
			}
		}
		#endregion Events (override)
	}
}
