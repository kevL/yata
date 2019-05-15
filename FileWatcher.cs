using System;
using System.IO;


namespace yata
{
	/// <summary>
	/// Creates a 'FileSystemWatcher' and handles its events.
	/// </summary>
	static class FileWatcher
	{
		static FileSystemWatcher CreateFileWatcher(string path)
		{
			var watcher = new FileSystemWatcher();

			watcher.Path         = path;
			watcher.Filter       = "*.2da";
			watcher.NotifyFilter = NotifyFilters.LastAccess
								 | NotifyFilters.LastWrite
								 | NotifyFilters.DirectoryName
								 | NotifyFilters.FileName;

			watcher.Changed += OnFileChanged;
			watcher.Created += OnFileChanged;
			watcher.Deleted += OnFileChanged;
			watcher.Renamed += OnFileRenamed;

			watcher.EnableRaisingEvents = true;

			return watcher;
		}

		/// <summary>
		/// Specifies what is done when a file is changed, created, or deleted.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		static void OnFileChanged(object source, FileSystemEventArgs e)
		{
			logfile.Log("OnFileChanged() source= " + source);
			logfile.Log(". e.ChangeType= " + e.ChangeType);
			logfile.Log(". e.FullPath= " + e.FullPath);
		}

		/// <summary>
		/// Specifies what is done when a file is renamed.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		static void OnFileRenamed(object source, RenamedEventArgs e)
		{
			logfile.Log("OnFileRenamed() source= " + source);
			logfile.Log(". e.ChangeType= " + e.ChangeType);
			logfile.Log(". e.OldFullPath= " + e.OldFullPath + " -> " + e.FullPath);
		}
	}
}
