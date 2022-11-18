using CA.Core.Valores;

namespace CA.Core.Extensions
{
    public static class ErroExtensions
    {
        public static Dictionary<string, string[]> ParaDicionario(this IEnumerable<Erro> erros)
        {
            return erros.GroupBy(c => c.Origem).ToDictionary(c => c.Key, c => c.Select(x => x.Descricao).ToArray());
        }
    }
}
