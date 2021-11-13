using System;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Creates an object that watches for external/disk file-changed events.
	/// </summary>
	/// <remarks>Call <c>Dispose()</c> when it goes out of scope. See
	/// <c><see cref="YataForm"/>.ClosePage()</c>.</remarks>
	sealed class FileWatcher
		: Timer
	{
		#region Fields
		readonly YataGrid _grid;
		DateTime _last;

		/// <summary>
		/// There shall be only 1 <c><see cref="FileWatcherDialog"/></c>
		/// instanced by this <c>FileWatcher</c>.
		/// </summary>
		FileWatcherDialog _dialog;
		#endregion Fields


		#region Properties
		/// <summary>
		/// The fullpath of the file to watch.
		/// </summary>
		internal string Fullpath
		{ private get; set; }

		/// <summary>
		/// <c>true</c> to bypass the <c><see cref="FileWatcherDialog"/> when
		/// the file is saved in Yata itself.</c>
		/// </summary>
		internal bool BypassFileChanged
		{ private get; set; }

		/// <summary>
		/// <c>true</c> to avoid repeatedly invoking the
		/// <c><see cref="FileWatcherDialog"/></c> if user deletes the file from
		/// the hardrive.
		/// </summary>
		internal bool BypassFileDeleted
		{ private get; set; }

		/// <summary>
		/// Forces user to reload the 2da-file after a load-sequence finishes.
		/// </summary>
		/// <remarks>Do not show a <c><see cref="FileWatcherDialog"/></c> while
		/// loading a 2da-file. It creates issues if the user has that dialog
		/// open and decides to change or delete the 2da-file from the hardrive.</remarks>
		internal bool ForceReload
		{ get; set; }
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="grid"></param>
		internal FileWatcher(YataGrid grid)
		{
			_grid = grid;
			Fullpath = _grid.Fullpath;
			_last = File.GetLastWriteTime(Fullpath);

			Interval = 300;
			Start();
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Handels this <c>FileWatcher's</c> <c>Tick</c> event in C#.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Check for a valid <c><see cref="YataGrid"/></c> since
		/// disposal of this <c>FileWatcher</c> could be delayed. See
		/// <c><see cref="YataForm"/>.ClosePage()</c> where the grid is nulled.</remarks>
		protected override void OnTick(EventArgs e)
		{
			if (_grid != null) // ~safety.
			{
				if (!BypassFileDeleted)
				{
					if (!File.Exists(Fullpath))
					{
						BypassFileDeleted = true;

						if (YataGrid._init)
						{
							ForceReload = true;
						}
						else if (_dialog == null)
						{
							using (_dialog = new FileWatcherDialog(_grid, FileWatcherDialog.Fwd.FileDeleted))
								_dialog.ShowDialog(_grid._f);

							_dialog = null;
						}
						else if (_dialog._fwdType == FileWatcherDialog.Fwd.FileChanged)
						{
							_dialog.SetAction(FileWatcherDialog.Fwd.FileDeleted);
						}
					}
					else if (!BypassFileChanged
						&& File.GetLastWriteTime(Fullpath) != _last)
					{
						_last = File.GetLastWriteTime(Fullpath);

						if (YataGrid._init)
						{
							ForceReload = true;
						}
						else if (_dialog == null)
						{
							using (_dialog = new FileWatcherDialog(_grid, FileWatcherDialog.Fwd.FileChanged))
								_dialog.ShowDialog(_grid._f);

							_dialog = null;
						}
					}
				}
				else if (_dialog != null // safety ~perhaps
					&& File.Exists(Fullpath)
					&& _dialog._fwdType == FileWatcherDialog.Fwd.FileDeleted)
				{
					BypassFileDeleted = false;
					_dialog.SetAction(FileWatcherDialog.Fwd.FileChanged);
				}

				if (BypassFileChanged)
				{
					BypassFileChanged = false;
					_last = File.GetLastWriteTime(Fullpath);
				}
			}
		}
		#endregion Handlers (override)
	}
}
