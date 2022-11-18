namespace CA.Util.Helpers
{
    public static class DateTimeHelper
    {
        public static IEnumerable<DateTime> ObterIntervalo(DateTime inicio, DateTime fim)
        {
            for (var date = inicio.Date; date.Date <= fim.Date; date = date.AddDays(1))
                yield

            return date;
        }

        public static IEnumerable<DateOnly> ObterIntervalo(DateOnly inicio, DateOnly fim)
        {
            for (var date = inicio; date <= fim; date = date.AddDays(1))
                yield

            return date;
        }

        public static string ObterNomeMes(int mes)
        {
            switch (mes)
            {
                case 1: return "janeiro";
                case 2: return "fevereiro";
                case 3: return "março";
                case 4: return "abril";
                case 5: return "maio";
                case 6: return "junho";
                case 7: return "julho";
                case 8: return "agosto";
                case 9: return "setembro";
                case 10: return "outubro";
                case 11: return "novembro";
                case 12: return "dezembro";
                default: throw new ArgumentException(nameof(mes), "Mês inválido.");
            }
        }
    }
}