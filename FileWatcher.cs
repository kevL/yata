﻿using System;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Creates an object that watches for external/disk file-changed events.
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

			Interval = 225;
			Start();
		}
		#endregion cTor


		#region Events (override)
		protected override void OnTick(EventArgs e)
		{
			if (!BypassFileDeleted)
			{
				if (!File.Exists(Pfe))
				{
					BypassFileDeleted = true;

					var fwd = new FileWatcherDialog(_grid,
													FileWatcherDialog.FILE_DEL,
													Pfe);
					fwd.ShowDialog(_grid._f);
				}
				else if (!BypassFileChanged)
				{
					if (File.GetLastWriteTime(Pfe) != _last)
					{
						_last = File.GetLastWriteTime(Pfe);

						var fwd = new FileWatcherDialog(_grid,
														FileWatcherDialog.FILE_WSC,
														Pfe);
						fwd.ShowDialog(_grid._f);
					}
				}
			}

			if (BypassFileChanged)
			{
				BypassFileChanged = false;
				_last = File.GetLastWriteTime(Pfe);
			}
		}
		#endregion Events (override)
	}
}
