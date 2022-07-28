using System;


namespace yata
{
	/// <summary>
	/// This class represents an entry in a zip archive. This can be a file or a
	/// directory. <c><see cref="ZipFile"/></c> will give you instances of this
	/// class as information about the members in an archive.
	/// </summary>
	/// <remarks>Author of the original java version: Jochen Hoenicke</remarks>
	sealed class ZipEntry
		: ICloneable
	{
		#region Properties
		/// <summary>
		/// Returns this <c>ZipEntry's</c> label.
		/// </summary>
		/// <remarks>The unix naming convention is followed. Path components in
		/// the entry should always separated by forward slashes ('/'). Dos
		/// device names like C: should also be removed.</remarks>
		internal string Label
		{ get; private set; }

		/// <summary>
		/// Gets/sets offset for use in central header.
		/// </summary>
		internal long Offset
		{ get; set; }

		/// <summary>
		/// Gets/Sets the size of the compressed data.
		/// </summary>
		/// <returns>the compressed entry size or -1 if unknown</returns>
		internal long CompressedSize
		{ get; set; }

		/// <summary>
		/// Gets/sets the compression method for this <c>ZipEntry</c>.
		/// </summary>
		internal Method Method
		{ get; private set; }
		#endregion Properties


		#region cTor
		/// <summary>
		/// Initializes an entry with the given name and made by information.
		/// </summary>
		/// <param name="label">label for this entry</param>
		/// <param name="method"><c><see cref="Method"/></c> for this entry</param>
		/// <remarks>This constructor is used by <c><see cref="ZipFile"/></c>
		/// when reading from the central header. It is not generally useful,
		/// use the constructor specifying the name only.</remarks>
		internal ZipEntry(string label, Method method)
		{
//			logfile.Log("ZipEntry.cTor");
//			logfile.Log(". label= " + label);
//			logfile.Log(". method= " + method);

			Label = label;
			Method = method;
		}
		#endregion cTor


		#region ICloneable
		/// <summary>
		/// Creates a clone of this <c>ZipEntry</c>.
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return MemberwiseClone();
		}
		#endregion ICloneable
	}
}
