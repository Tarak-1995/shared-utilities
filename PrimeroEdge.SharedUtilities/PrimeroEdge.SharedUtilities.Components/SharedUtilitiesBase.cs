using Cybersoft.Platform.Utilities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using TimeZoneConverter;

namespace PrimeroEdge.SharedUtilities.Components
{
    public abstract class SharedUtilitiesBase<T> where T : SharedUtilitiesBase<T>
    {
        public ResponseEnvelope ResponseEnvelope { get; set; } = new ResponseEnvelope();
        public Pagination PaginationEnvelope { get; set; } 

        public ResponseEnvelope GetResponseEnvelope()
        {
            return ResponseEnvelope;
        }

        public DateTime GetDistrictDateTime(DateTime utcTime, string districtTimeZone, bool applyDayLightSaving)
        {
            var tzi = TZConvert.GetTimeZoneInfo(districtTimeZone);

            var districtTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);

            if (!applyDayLightSaving)
            {
                districtTime = districtTime.AddHours(-1);
            }

            return districtTime;
        }
    }
}
