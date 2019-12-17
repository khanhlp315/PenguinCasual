using System;

namespace Penguin.Utilities
{
    public static class DateTimeUtil
    {
        static DateTime EpochTime = new DateTime(1970, 1, 1);
        public static long TimeSinceEpoch()
        {
            TimeSpan t = DateTime.Now - EpochTime;
            return (long)t.TotalSeconds;
        }

		public static bool IsSameDate (this DateTime dt1, DateTime dt2) {
			return dt1.Date == dt2.Date;
		}

		public static string GetTimeString (TimeSpan span) {
			return GetTimeString((long)span.TotalSeconds);
		}

		public static string GetTimeString(long remainSeconds)
		{
			string result = String.Empty;
			int day = (int)(remainSeconds / 86400);
			remainSeconds -= day * 86400;
			int hour = (int)(remainSeconds / 3600);
			remainSeconds -= hour * 3600;
			int min = (int)(remainSeconds / 60);
			remainSeconds -= min * 60;
			int sec = (int)remainSeconds;

			string strDay = string.Empty;

			if (day > 0)
			{
				if (day > 1)
				{
					strDay = string.Format("{0} days ", day);
				}
				else
				{
					strDay = string.Format("{0} day ", day);
				}
			}

			result = string.Format("{0}{1}:{2}:{3}", strDay, hour.ToString("00"), min.ToString("00"), sec.ToString("00"));

			return result;
		}
	}
}