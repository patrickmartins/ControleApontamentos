using CA.Repositorios.Tfs.ServicosSoap.Models;

namespace CA.Repositorios.Tfs.Extensions
{
    internal static class TeamFoundationIdentityExtensions
    {
        public static TTipo ObterPropriedade<TTipo>(this TeamFoundationIdentity identidade, string propriedade)
        {
            var prop = identidade.Properties.FirstOrDefault(c => c.pname.Equals(propriedade));

            if (prop is null)
                return default;

            var valor = Convert.ChangeType(prop.val, typeof(TTipo));

            if (valor is null)
                return default;

            return (TTipo)valor;
        }
    }
}
