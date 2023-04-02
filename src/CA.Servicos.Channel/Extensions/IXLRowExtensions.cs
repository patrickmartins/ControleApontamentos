using CA.Util.Extensions;
using ClosedXML.Excel;

namespace CA.Servicos.Channel.Extensions
{
    public static class IXLRowExtensions
    {
        public static int BuscarColunaPorValor(this IXLRow linha, string valor)
        {
            foreach(var coluna in linha.CellsUsed())
            {
                if (!coluna.Value.IsBlank && !coluna.Value.IsError && coluna.Value.ToString().ToLower().RemoverQuebrasDeLinha().Equals(valor.ToLower()))
                    return coluna.Address.ColumnNumber;
            }

            return -1;
        }
    }
}
