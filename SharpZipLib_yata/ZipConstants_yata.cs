using System;


namespace yata
{
	/// <summary>
	/// The kind of compression used for a <c><see cref="ZipEntry"/></c>.
	/// </summary>
	enum Method
	{
		/// <summary>
		/// A direct copy of data is held in the <c><see cref="ZipFile"/></c>.
		/// </summary>
		Stored = 0,

		/// <summary>
		/// Common Zip compression method using a sliding dictionary of up to
		/// 32kb and secondary compression from Huffman/Shannon-Fano trees.
		/// </summary>
		Deflated = 8
	}

	/// <summary>
	/// Defines the contents of the general bit flags field for a
	/// <c><see cref="ZipEntry"/></c>.
	/// </summary>
	/// <remarks>These are not set in the Nwn2/Data zip archives. The
	/// <c>Bitflags</c> <c>enum</c> is retained merely for potential use by
	/// <c><see cref="ZipFile"/>.GetEntryDataOffset()</c>.</remarks>
	[Flags]
	enum Bitflags
	{
		/// <summary>
		/// Indicates a trailing data descriptor is appended to the
		/// <c><see cref="ZipEntry"/></c> data.
		/// </summary>
		Descriptor = 0x0008,

		/// <summary>
		/// Indicates that the filename and comment fields for a
		/// <c><see cref="ZipEntry"/></c> must be encoded using UTF-8.
		/// </summary>
		UnicodeText = 0x0800
	}


	/// <summary>
	/// Yata needs only a few zip-specific constants.
	/// </summary>
	static class ZipConstants
	{
		/// <summary>
		/// End of central directory record signature.
		/// </summary>
		internal const int EndOfCentralDirectorySignature =  'P'
														  | ('K' <<  8)
														  | ( 5  << 16)
														  | ( 6  << 24);

		/// <summary>
		/// Size of end of central record (excluding variable fields).
		/// </summary>
		internal const int EndOfCentralRecordBaseSize = 22;

		/// <summary>
		/// Size of local entry header (excluding variable length fields at
		/// end).
		/// </summary>
		internal const int LocalHeaderBaseSize = 30;
	}
}
