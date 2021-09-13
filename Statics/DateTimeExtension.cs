using System;
using System.IO;
using System.Reflection;


namespace yata
{
	/// <summary>
	/// Lifted from StackOverflow.com:
	/// https://stackoverflow.com/questions/1600962/displaying-the-build-date#answer-1600990
	/// - what a fucking pain in the ass.
	/// </summary>
	static class DateTimeExtension
	{
		/// <summary>
		/// Gets the <c>DateTime</c> that an <c>Assembly</c> was linked up.
		/// </summary>
		/// <param name="ass">the qualifying <c>Assembly</c></param>
		/// <param name="utc"><c>true</c> to return <c>DateTime</c> as UTC</param>
		/// <param name="tzi">the <c>TimeZoneInfo</c> to convert UTC to -
		/// <c>null</c> to use the machine's local <c>TimeZoneInfo</c> - if
		/// <paramref name="utc"/> is <c>false</c></param>
		/// <returns></returns>
		internal static DateTime GetLinkerTime(this Assembly ass, bool utc = false, TimeZoneInfo tzi = null)
		{
			const int c_PeHeaderOffset        = 60;
			const int c_LinkerTimestampOffset = 8;

			string filePath = ass.Location;

			var buffer = new byte[2048];

			using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				stream.Read(buffer, 0, 2048);

			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			int offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
			int secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);

			DateTime linkTimeUtc = epoch.AddSeconds(secondsSince1970);

			if (!utc)
				linkTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tzi ?? TimeZoneInfo.Local);

			return linkTimeUtc;
		}
	}
}
