namespace CA.Util.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ConverterParaFusoBrasil(this DateTime data)
        {
            return TimeZoneInfo.ConvertTime(data, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }

        public static bool EHoje(this DateTime data)
        {
            return data.Date == DateTime.Today;
        }
    }
}
