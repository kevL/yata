using System;


namespace yata
{
	/// <summary>
	/// Inflater is used to decompress data that has been compressed according
	/// to the deflate standard described in rfc1951.
	///
	/// 
	/// By default Zlib (rfc1950) headers and footers are expected in the input.
	/// You can use constructor
	/// <c><see cref="Inflator(bool)">Inflater(bool)</see></c> passing
	/// <c>true</c> if there is no Zlib header information.
	///
	/// 
	/// The usage is as following. First you have to set some input with
	/// <c><see cref="SetInput()">SetInput()</see></c> then
	/// <c><see cref="Inflate()">Inflate()</see></c> it. If inflate doesn't
	/// inflate any bytes there may be three reasons:
	/// <list type="bullet">
	/// <item><c><see cref="IsNeedingInput()">IsNeedingInput()</see></c> returns
	/// <c>true</c> because the input buffer is empty. You have to provide more
	/// input with <c><see cref="SetInput()">SetInput()</see></c>
	/// <c>IsNeedingInput()</c> also returns <c>true</c> when the stream is
	/// finished</item>
	/// <item><c><see cref="IsNeedingDictionary()">IsNeedingDictionary()</see></c>
	/// returns <c>true</c> - you have to provide a preset dictionary with
	/// <c><see cref="SetDictionary()">SetDictionary()</see></c></item>
	/// <item><c><see cref="IsFinished"></see></c> is <c>true</c> - the inflator
	/// has finished</item>
	/// </list>
	/// 
	/// 
	/// Once the first output byte is produced a dictionary will not be needed
	/// at a later stage.
	/// </summary>
	/// <remarks>Author of the original java version: John Leuner and Jochen
	/// Hoenicke</remarks>
	sealed class Inflator
	{
		#region Fields (static)
		/// <summary>
		/// Copy lengths for literal codes 257..285.
		/// </summary>
		static readonly int[] CPLENS =
		{
			3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 15, 17, 19, 23, 27, 31, 35, 43, 51,
			59, 67, 83, 99, 115, 131, 163, 195, 227, 258
		};

		/// <summary>
		/// Extra bits for literal codes 257..285.
		/// </summary>
		static readonly int[] CPLEXT =
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4,
			4, 5, 5, 5, 5, 0
		};

		/// <summary>
		/// Copy offsets for distance codes 0..29.
		/// </summary>
		static readonly int[] CPDIST =
		{
			1, 2, 3, 4, 5, 7, 9, 13, 17, 25, 33, 49, 65, 97, 129, 193, 257, 385,
			513, 769, 1025, 1537, 2049, 3073, 4097, 6145, 8193, 12289, 16385,
			24577
		};

		/// <summary>
		/// Extra bits for distance codes.
		/// </summary>
		static readonly int[] CPDEXT =
		{
			0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9,
			10, 10, 11, 11, 12, 12, 13, 13
		};

		// these are the possible states for an inflator ->
		const int DECODE_HEADER           =  0;
		const int DECODE_DICT             =  1;
		const int DECODE_BLOCKS           =  2;
		const int DECODE_STORED_LEN1      =  3;
		const int DECODE_STORED_LEN2      =  4;
		const int DECODE_STORED           =  5;
		const int DECODE_DYN_HEADER       =  6;
		const int DECODE_HUFFMAN          =  7;
		const int DECODE_HUFFMAN_LENBITS  =  8;
		const int DECODE_HUFFMAN_DIST     =  9;
		const int DECODE_HUFFMAN_DISTBITS = 10;
		const int DECODE_CHKSUM           = 11;
		const int DECODE_FINISHED         = 12;

		/// <summary>
		/// Identifies a stored block in <c><see cref="ZipFile"/></c>.
		/// </summary>
		const int STORED_BLOCK = 0;

		/// <summary>
		/// Identifies static tree in <c><see cref="ZipFile"/></c>.
		/// </summary>
		const int STATIC_TREES = 1;

		/// <summary>
		/// Identifies dynamic tree in <c><see cref="ZipFile"/></c>.
		/// </summary>
		const int DYN_TREES = 2;
		#endregion Fields (static)


		#region Fields
		readonly StreamManipulator _input;
		readonly OutputWindow _output;

		/// <summary>
		/// This variable contains the current state.
		/// </summary>
		int _state;

		/// <summary>
		/// The total count of bytes set with
		/// <c><see cref="SetInput()">SetInput()</see></c>.
		/// </summary>
		long _totalIn;

		/// <summary>
		/// The total count of inflated bytes.
		/// </summary>
		long _totalOut;

		/// <summary>
		/// The number of bits needed to complete the current state. This is
		/// valid if <c><see cref="_state"/></c> is
		/// <list type="bullet">
		/// <item><c><see cref="DECODE_DICT"/></c></item>
		/// <item><c><see cref="DECODE_CHKSUM"/></c></item>
		/// <item><c><see cref="DECODE_HUFFMAN_LENBITS"/></c></item>
		/// <item><c><see cref="DECODE_HUFFMAN_DISTBITS"/></c></item>
		/// </list>
		/// </summary>
		int _neededBits;

		/// <summary>
		/// The adler checksum of the dictionary or of the decompressed stream
		/// as it is written in the header resp. footer of the compressed
		/// stream. Only valid if <c><see cref="_state"/></c> is
		/// <list type="bullet">
		/// <item><c><see cref="DECODE_DICT"/></c></item>
		/// <item><c><see cref="DECODE_CHKSUM"/></c></item>
		/// </list>
		/// </summary>
		int _readAdler;

		/// <summary>
		/// <c>true</c> if the last block flag was set in the last block of the
		/// inflated stream. This means that the stream ends after the current
		/// block.
		/// </summary>
		bool _isLastBlock;

		InflatorHuffmanTree _litlenTree, _distTree;
		InflatorDynamicHeader _dynHeader;

		int _decoded;

		int _repLength, _repDist;
		#endregion Fields


		#region Properties
		/// <summary>
		/// <c>true</c> if the inflater has finished.
		/// </summary>
		/// <remarks> This means that no input is needed and no output can be
		/// produced.</remarks>
		internal bool IsFinished
		{
			get { return _state == DECODE_FINISHED && _output.GetAvailable() == 0; }
		}

		/// <summary>
		/// <c>true</c> if the input buffer is empty. You should then call
		/// <c><see cref="SetInput()">SetInput()</see></c>.
		/// </summary>
		/// <remarks>This method also returns <c>true</c> when the stream is
		/// finished.</remarks>
		internal bool IsNeedingInput
		{
			get { return _input.IsNeedingInput; }
		}
		#endregion Properties


		#region cTor
		/// <summary>
		/// Creates a new <c>Inflator</c>.
		/// </summary>
		internal Inflator()
		{
			_input = new StreamManipulator();
			_output = new OutputWindow();

			_state = DECODE_BLOCKS;
		}
		#endregion cTor


		#region Methods
		/// <summary>
		/// Sets the input. This should only be called if
		/// <c><see cref="IsNeedingInput"></see></c> is <c>true</c>.
		/// </summary>
		/// <param name="buffer">the source of input data</param>
		/// <param name="index">the index into buffer where the input starts</param>
		/// <param name="count">the count of bytes of input to use</param>
		internal void SetInput(byte[] buffer, int index, int count)
		{
			_input.SetInput(buffer, index, count);
			_totalIn += (long)count;
		}

		/// <summary>
		/// Inflates the compressed stream to the output buffer. If this returns
		/// <c>0</c> you should check whether needsDictionary(), needsInput() or
		/// finished() returns <c>true</c> to determine why no further output is
		/// produced.
		/// </summary>
		/// <param name="buffer">the output buffer</param>
		/// <param name="offset">the offset in buffer where storing starts</param>
		/// <param name="count">the maximum number of bytes to output</param>
		/// <returns>the count of bytes written to the buffer; <c>0</c> if no
		/// further output can be produced</returns>
		internal int Inflate(Array buffer, int offset, int count)
		{
			if (count == 0) // special case: count may be zero
			{
				if (!IsFinished)
					Decode();

				return 0;
			}

			int bytesCopied = 0;

			do
			{
				if (_state != DECODE_CHKSUM)
				{
					// don't give away any output if we are waiting for the
					// checksum in the input stream
					//
					// with this trick we have always: IsNeedingInput and not
					// IsFinished implies more output can be produced

					int copied = _output.CopyOutput(buffer, offset, count);
					if (copied > 0)
					{
						offset      +=       copied;
						bytesCopied +=       copied;
						_totalOut   += (long)copied;

						if ((count -= copied) == 0)
							return bytesCopied;
					}
				}
			}
			while (Decode() || (_output.GetAvailable() > 0 && _state != DECODE_CHKSUM));

			return bytesCopied;
		}

		/// <summary>
		/// Decodes the deflated stream.
		/// </summary>
		/// <returns><c>false</c> if more input is needed or if finished</returns>
		bool Decode()
		{
			switch (_state)
			{
				case DECODE_HEADER:
					return DecodeHeader();

				case DECODE_DICT:
					return DecodeDict();

				case DECODE_CHKSUM:
					return DecodeChksum();

				case DECODE_BLOCKS:
				{
					if (_isLastBlock)
					{
						_state = DECODE_FINISHED;
						return false;
					}

					int segtype = _input.PeekBits(3);
					if (segtype < 0)
						return false;

					_input.DropBits(3);

					_isLastBlock |= (segtype & 1) != 0;

					switch (segtype >> 1)
					{
						case STORED_BLOCK:
							_input.SkipToByteBoundary();
							_state = DECODE_STORED_LEN1;
							break;

						case STATIC_TREES:
							_litlenTree = null;
							_distTree = null;
							_state = DECODE_HUFFMAN;
							break;

						case DYN_TREES:
							_dynHeader = new InflatorDynamicHeader(_input);
							_state = DECODE_DYN_HEADER;
							break;
					}
					return true;
				}

				case DECODE_STORED_LEN1:
					if ((_decoded = _input.PeekBits(16)) < 0)
						return false;

					_input.DropBits(16);

					_state = DECODE_STORED_LEN2;
					goto case DECODE_STORED_LEN2;

				case DECODE_STORED_LEN2:
					if (_input.PeekBits(16) < 0)
						return false;

					_input.DropBits(16);

					_state = DECODE_STORED;
					goto case DECODE_STORED;

				case DECODE_STORED:
					if ((_decoded -= _output.CopyStored(_input, _decoded)) == 0)
					{
						_state = DECODE_BLOCKS;
						return true;
					}
					return !_input.IsNeedingInput;

				case DECODE_DYN_HEADER:
					if (!_dynHeader.AttemptRead())
						return false;

					_litlenTree = _dynHeader.LiteralLengthTree;
					_distTree   = _dynHeader.DistanceTree;

					_state = DECODE_HUFFMAN;
					goto case DECODE_HUFFMAN;

				case DECODE_HUFFMAN:
				case DECODE_HUFFMAN_LENBITS:
				case DECODE_HUFFMAN_DIST:
				case DECODE_HUFFMAN_DISTBITS:
					return DecodeHuffman();

				case DECODE_FINISHED:
					return false;
			}
			return false;
		}

		/// <summary>
		/// Decodes a zlib/RFC1950 header.
		/// </summary>
		/// <returns><c>false</c> if more input is needed</returns>
		bool DecodeHeader()
		{
			int header = _input.PeekBits(16);
			if (header < 0)
				return false;

			_input.DropBits(16);

			// the header is written in reversed byte order
			header = ((header << 8) | (header >> 8)) & 0xffff;
			if ((header & 0x0020) != 0)
			{
				_state = DECODE_DICT; // kL_note: never happens for Nwn2/Data zips
				_neededBits = 32;
			}
			else
				_state = DECODE_BLOCKS;

			return true;
		}

		/// <summary>
		/// Decodes the dictionary checksum after the deflate header.
		/// </summary>
		/// <returns><c>false</c> if more input is needed</returns>
		bool DecodeDict()
		{
			while (_neededBits > 0)
			{
				int dictByte = _input.PeekBits(8);
				if (dictByte < 0)
					return false;

				_input.DropBits(8);

				_readAdler = (_readAdler << 8) | dictByte;
				_neededBits -= 8;
			}
			return false;
		}

		/// <summary>
		/// Decodes the adler checksum after the deflate stream.
		/// </summary>
		/// <returns><c>false</c> if more input is needed</returns>
		bool DecodeChksum()
		{
			while (_neededBits > 0)
			{
				int chkByte = _input.PeekBits(8);
				if (chkByte < 0)
					return false;

				_input.DropBits(8);

				_readAdler = (_readAdler << 8) | chkByte;
				_neededBits -= 8;
			}

			_state = DECODE_FINISHED;
			return false;
		}


		/// <summary>
		/// Decodes the huffman encoded symbols in the input stream.
		/// </summary>
		/// <returns><c>false</c> if more input is needed; <c>true</c> if output
		/// window is full or the current block ends</returns>
		bool DecodeHuffman()
		{
			int free = _output.GetFreeSpace();
			while (free >= 258)
			{
				int symbol;
				switch (_state)
				{
					case DECODE_HUFFMAN:
						while (((symbol = _litlenTree.GetSymbol(_input)) & ~0xff) == 0)
						{
							_output.Write(symbol);

							if (--free < 258)
								return true;
						}

						if (symbol < 257)
						{
							if (symbol < 0)
								return false;

							// symbol == 256: end of block
							_distTree = null;
							_litlenTree = null;
							_state = DECODE_BLOCKS;
							return true;
						}

						_repLength = CPLENS[symbol - 257];
						_neededBits = CPLEXT[symbol - 257];

						goto case DECODE_HUFFMAN_LENBITS;

					case DECODE_HUFFMAN_LENBITS:
						if (_neededBits > 0)
						{
							_state = DECODE_HUFFMAN_LENBITS;
							int i = _input.PeekBits(_neededBits);
							if (i < 0)
								return false;

							_input.DropBits(_neededBits);
							_repLength += i;
						}

						_state = DECODE_HUFFMAN_DIST;
						goto case DECODE_HUFFMAN_DIST;

					case DECODE_HUFFMAN_DIST:
						symbol = _distTree.GetSymbol(_input);
						if (symbol < 0)
							return false;

						_repDist = CPDIST[symbol];
						_neededBits = CPDEXT[symbol];

						goto case DECODE_HUFFMAN_DISTBITS;

					case DECODE_HUFFMAN_DISTBITS:
						if (_neededBits > 0)
						{
							_state = DECODE_HUFFMAN_DISTBITS;
							int i = _input.PeekBits(_neededBits);
							if (i < 0)
								return false;

							_input.DropBits(_neededBits);
							_repDist += i;
						}

						_output.Repeat(_repLength, _repDist);
						free -= _repLength;

						_state = DECODE_HUFFMAN;
						break;
				}
			}
			return true;
		}
		#endregion Methods
	}
}
