namespace CA.Util.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool EHoje(this DateTime data)
        {
            return data.Date == DateTime.Today;
        }
    }
}
