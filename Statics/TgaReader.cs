using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
//using System.Text;


namespace yata
{
	/// <summary>
	/// A static class for reading NwN2-compatible TGA-files.
	/// </summary>
	/// <remarks>Specification<br/>
	/// https://en.wikipedia.org/wiki/Truevision_TGA<br/>
	/// https://www.dca.fee.unicamp.br/~martino/disciplinas/ea978/tgaffs.pdf</remarks>
	static class TgaReader
	{
		#region Methods (static)
		/// <summary>
		/// Creates a <c>Bitmap</c> given a fullpath to an NwN2-compatible TGA
		/// file.
		/// </summary>
		/// <param name="pfeTga">fullpath of a TGA imagefile</param>
		/// <param name="info">ref for info on the statusbar</param>
		/// <returns>a <c>Bitmap</c> of the TGA imagefile</returns>
		/// <remarks>Ensure that file exists before call.</remarks>
		internal static Bitmap Create(string pfeTga, ref string info)
		{
			byte[] bytes = File.ReadAllBytes(pfeTga);

//			if (!CheckSignature(bytes))        return null;
//			if (!CheckImageIdLength(bytes[0])) return null; // field 1 (1 byte pos 0)
			if (!CheckColormapType(bytes[1]))  return null; // field 2 (1 byte pos 1)
			if (!CheckImagedataType(bytes[2])) return null; // field 3 (1 byte pos 2)

			bool le = BitConverter.IsLittleEndian; // hardware architecture

			uint b;

			// Field 4: Color Map Specification (5 bytes pos 3)
			// skip this. Colormaps are not supported here.
			// field 4.1 - first entry id (2 bytes pos 3)
			// field 4.2 - color map length (2 bytes pos 5)
			// field 4.3 - color map entry size (1 byte pos 7)

			// Field 5: Image Specification (10 bytes pos 8)
			// field 5.1 - x-origin (2 bytes pos 8) // assume val=0
			// field 5.2 - y-origin (2 bytes pos 10) // assume val=0
			// field 5.3 - width (2 bytes pos 12)
			uint pos = 12;
			var buffer = new byte[2];
			for (b = 0; b != 2; ++b)
				buffer[b] = bytes[pos++];

			if (!le) Array.Reverse(buffer);
			ushort width = BitConverter.ToUInt16(buffer, 0);

			// field 5.4 - height (2 bytes pos 14)
			buffer = new byte[2];
			for (b = 0; b != 2; ++b)
				buffer[b] = bytes[pos++];

			if (!le) Array.Reverse(buffer);
			ushort height = BitConverter.ToUInt16(buffer, 0);

			// field 5.5 - pixel depth (1 byte pos 16)
			byte pixeldepth = bytes[pos++];
			if (!CheckPixeldepth(pixeldepth)) return null;

			int bytesperpixel = pixeldepth / 8;

			// field 5.6 - image descriptor (1 byte pos 17)
			byte descriptor = bytes[pos++];
//			if (!CheckDescriptor(descriptor)) return null;

			PixelFormat pf;
			if      (descriptor == 8) pf = PixelFormat.Format32bppRgb;
			else if (descriptor == 0) pf = PixelFormat.Format24bppRgb;
			else
				return null;


			info = width + " x " + height + " x " + pixeldepth + " bits per pixel";

			// Assume that the ImageID and ColorMapData will not be present in
			// NwN2 TGA files so just continue the stream @ pos ->
//			pos = 18; // + idlength + colormapdatalength;

			int datalength = width * height * bytesperpixel;

			buffer = new byte[datalength];
			for (b = 0; b != datalength; ++b)
				buffer[b] = bytes[pos++];

			var data = new byte[datalength]; // invert row order ->
			int stride = width * bytesperpixel;
			for (int r = 0; r != height; ++r)
			{
				Buffer.BlockCopy(buffer, r * stride,
								 data,   datalength - (r + 1) * stride,
								 stride);
			}

			var pic = new Bitmap(width, height, pf);
			BitmapData locked = pic.LockBits(new Rectangle(0,0, width, height), ImageLockMode.WriteOnly, pf);
			Marshal.Copy(data, 0, locked.Scan0, datalength);
			pic.UnlockBits(locked);

			return pic; // WARNING: Dispose that when done with it.
		}


/*		/// <summary>
		/// Checks that the signature of the TGA-file is new-style.
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		/// <remarks>This check is unnecessary since the new-style TGA format is
		/// backward compatible with the old-style and this <c>TgaReader</c>
		/// doesn't use any new-style data.</remarks>
		static bool CheckSignature(byte[] bytes)
		{
			if (bytes.Length > 26)
			{
				uint pos = (uint)bytes.Length - 18;

				var buffer = new byte[16];
				for (uint b = 0; b != 16; ++b)
					buffer[b] = bytes[pos++];

				string signature = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
				if (signature == "TRUEVISION-XFILE")
					return true;
			}
			return false;
		} */

/*		/// <summary>
		/// Checks Field 1:ImageID length (see field 6) for a nonzero value.
		/// </summary>
		/// <param name="b">the <c>byte</c> to check</param>
		/// <returns><c>true</c> if the TGA-file contains an image</returns>
		/// <remarks>A zero-value is supposed to mean that the TGA-file doesn't
		/// contain an image but NwN2 TGA-files don't appear to respect that.</remarks>
		static bool CheckImageIdLength(byte b)
		{
			return b != 0;
		} */

		/// <summary>
		/// Checks Field 2:ColorMapType for a zero value.
		/// </summary>
		/// <param name="b">the <c>byte</c> to check</param>
		/// <returns><c>true</c> if there is no colormap</returns>
		static bool CheckColormapType(byte b)
		{
			return b == 0;
		}

		/// <summary>
		/// Checks Field 3:ImageDataType for a value of <c>2</c>.
		/// </summary>
		/// <param name="b">the <c>byte</c> to check</param>
		/// <returns><c>true</c> if the image is an uncompressed true-color
		/// image</returns>
		static bool CheckImagedataType(byte b)
		{
			return b == 2;

//			switch (b)
//			{
//				case  0: logfile.Log(". no image data included");                   break;
//				case  1: logfile.Log(". uncompressed color-mapped image");          break;
//				case  2: logfile.Log(". uncompressed true-color image");            break;
//				case  3: logfile.Log(". uncompressed black-and-white image");       break;
//				case  9: logfile.Log(". run-length encoded color-mapped image");    break;
//				case 10: logfile.Log(". run-length encoded true-color image");      break;
//				case 11: logfile.Log(". run-length encoded black-and-white image"); break;
//			}
		}

		/// <summary>
		/// Checks Field 5.5:ImageSpecification (pixeldepth) for a value of
		/// <c>24</c> or <c>32</c>.
		/// </summary>
		/// <param name="b">the <c>byte</c> to check</param>
		/// <returns><c>true</c> if the pixeldepth is 24- or 32-bits</returns>
		/// <remarks>This check is a bit redundant since the pixeldepth can
		/// likely be calculated from the data in ImageDataType and the
		/// Descriptor.</remarks>
		static bool CheckPixeldepth(byte b)
		{
			return b == 24 || b == 32;
		}

/*		/// <summary>
		/// Checks Field 5.6:ImageSpecification (descriptor) for a value of
		/// <c>0</c> or <c>8</c>.
		/// </summary>
		/// <param name="b">the <c>byte</c> to check</param>
		/// <returns><c>true</c> if the image is drawn with default scans either
		/// with or without an 8-bit alphachannel</returns>
		static bool CheckDescriptor(byte b)
		{
			return b == 0 || b == 8;

//			if ((b & 0x80) != 0) logfile.Log(". bit7 set");
//			if ((b & 0x40) != 0) logfile.Log(". bit6 set");
//			if ((b & 0x20) != 0) logfile.Log(". bit5 set - top to bottom");
//			else                 logfile.Log(". bit5 unset - bottom to top");
//			if ((b & 0x10) != 0) logfile.Log(". bit4 set - right to left");
//			else                 logfile.Log(". bit4 unset - left to right");
//
//			logfile.Log(". alpha channel depth= " + (b & 0xF));
		} */
		#endregion Methods (static)
	}
}
