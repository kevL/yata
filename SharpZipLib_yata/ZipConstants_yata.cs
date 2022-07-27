using System;


namespace yata
{
	/// <summary>
	/// The kind of compression used for an entry in an archive.
	/// </summary>
	enum CompressionMethod
	{
		/// <summary>
		/// A direct copy of the file contents is held in the archive
		/// </summary>
		Stored = 0,

		/// <summary>
		/// Common Zip compression method using a sliding dictionary
		/// of up to 32KB and secondary compression from Huffman/Shannon-Fano trees
		/// </summary>
		Deflated = 8
	}

	/// <summary>
	/// Defines the contents of the general bit flags field for an archive
	/// entry.
	/// </summary>
	[Flags]
	enum GeneralBitFlags
	{
		/// <summary>
		/// Bit 3 if set indicates a trailing data descriptor is appended to the
		/// entry data.
		/// </summary>
		Descriptor = 0x0008,

		/// <summary>
		/// Bit 11 if set indicates that the filename and comment fields for
		/// this file must be encoded using UTF-8.
		/// </summary>
		UnicodeText = 0x0800
	}


	/// <summary>
	/// 
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
		/// Size of local entry header (excluding variable length fields at end)
		/// </summary>
		internal const int LocalHeaderBaseSize = 30;
	}
}
