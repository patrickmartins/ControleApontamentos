using CA.Core.Entidades.Channel;
using CA.Facade.Channel.Interfaces;
using CA.Servicos.Channel.Interfaces;
using System.Net;

namespace CA.Facade.Channel
{
    public class ServicoChannelFacade : IServicoChannelFacade
    {
        private readonly IServicoChannelHttp _servicoHttp;

        public ServicoChannelFacade(IServicoChannelHttp servicoHttp) 
        {
            _servicoHttp = servicoHttp;
        }

        public async Task<IEnumerable<ProjetoChannel>> ObterProjetosAtivosAsync()
        {
            var projetos = await _servicoHttp.ObterProjetosAsync();

            return projetos.Select(c => new ProjetoChannel
            {
                Id = c.Id,
                Nome = WebUtility.UrlDecode(c.Nome)
            })
            .ToList();
        }

        public async Task<IEnumerable<AtividadeProjetoChannel>> ObterAtividadesPorProjetoAsync(int idProjeto)
        {
            var atividades = await _servicoHttp.ObterAtividadesPorProjetoAsync(idProjeto);

            return atividades.Select(c => new AtividadeProjetoChannel
            {
                Id = c.Id,
                Nome = WebUtility.UrlDecode(c.Nome)
            })
            .ToList();
        }

        public async Task<IEnumerable<ApontamentoChannel>> ObterApontamentosPorPeriodoAsync(DateOnly inicio, DateOnly fim)
        {
            //var relatorio = await _servicoHttp.ObterRelatorioApontamentosPorPeriodo(inicio, fim);

            //return ApontamentoChannelHelper.ExtrairApontamentosDeRelatorio(relatorio);

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UsuarioChannel>> ObterUsuariosAtivosAsync()
        {
            var usuarios = await _servicoHttp.ObterUsuariosAtivosAsync();

            return usuarios.Select(c => new UsuarioChannel
            {
                Id = c.Id,
                Email = WebUtility.UrlDecode(c.Conta.Email),
                NomeUsuario = WebUtility.UrlDecode(c.Conta.NomeUsuario),
                NomeCompleto = WebUtility.UrlDecode(c.Nome)
            })
            .ToList();
        }
    }
}
