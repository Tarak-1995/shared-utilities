namespace PrimeroEdge.SharedUtilities.Components.Common
{
    using System;
    using TimeZoneConverter;

    public static class Extensions
    {
        /// <summary>
        /// Converts the given time to utc.
        /// </summary>
        /// <param name="theDateTime">This will be the date time value to be converted.</param>
        /// <param name="isToDate">Should be false if start date and true for the end date. </param>
        /// <param name="timezone">Time zone for the district and will be CST as a default.</param>
        /// <returns></returns>
        public static string ToUtcDateTime(this DateTime theDateTime, bool isToDate = false,
            string timezone = "Central Standard Time")
        {
            var temp = DateTime.SpecifyKind(theDateTime, DateTimeKind.Unspecified);
            var originalTimeZone = TZConvert.GetTimeZoneInfo(timezone);
            var cleanTime = temp.Date;
            if (isToDate)
            {
                cleanTime = cleanTime.AddDays(1).AddSeconds(-1);
            }

            var modified = TimeZoneInfo.ConvertTimeToUtc(cleanTime, originalTimeZone);
            return modified.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
        }
    }
}