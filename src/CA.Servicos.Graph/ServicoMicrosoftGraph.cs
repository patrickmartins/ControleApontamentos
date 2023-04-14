using CA.Core.Configuracoes;
using CA.Core.Interfaces.Microsoft;
using CA.Servicos.Graph.Entidades;
using CA.Servicos.Graph.Extensions;
using Microsoft.Graph;

namespace CA.Servicos.Graph
{
    public class ServicoMicrosoftGraph : IServicoMicrosoftGraph
    {
        private readonly GraphServiceClient _cliente;
        private readonly ConfiguracaoMicrosoftGraph _configuracoes;

        public ServicoMicrosoftGraph(GraphServiceClient cliente, ConfiguracaoMicrosoftGraph configuracoes)
        {
            _cliente = cliente;
            _configuracoes = configuracoes;
        }

        public async Task<IEnumerable<Usuario>> ObterTodosUsuariosAsync()
        {
            var resposta = await _cliente.Users.GetAsync((config) =>
            {
                config.QueryParameters.Top = 500;
                config.QueryParameters.Filter = $"endswith(mail,'{_configuracoes.Dominio}')";
                config.QueryParameters.Orderby = new string[] { "userPrincipalName" };
                config.QueryParameters.Count = true;
                config.Headers.Add("ConsistencyLevel", "eventual");
            });

            if(resposta is null || resposta.Value is null)
                return Enumerable.Empty<Usuario>();

            return resposta.Value.Select(c => new Usuario
            {
                Email = c.ObterEmail(),
                NomeUsuario = c.ObterNomeUsuario(),
                NomeCompleto = c.ObterNomeCompleto()
            })
            .ToList();            
        }
    }
}
