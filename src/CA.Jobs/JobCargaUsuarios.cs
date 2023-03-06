using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Jobs.Channel.Extensions;
using CA.Jobs.Channel.Interfaces;
using CA.Servicos.Channel.Interfaces;
using Microsoft.Extensions.Logging;

namespace CA.Jobs.Channel
{
    public class JobCargaUsuarios : JobChannel<UsuarioChannel>
    {
        private readonly IRepositorioUsuariosChannel _repositorioUsuarios;

        private readonly IServicoChannelHttp _servico;

        public JobCargaUsuarios(IRepositorioUsuariosChannel repositorioUsuarios, IServicoChannelHttp servico, ILogger<UsuarioChannel> logger) : base(logger)
        {
            _repositorioUsuarios = repositorioUsuarios;

            _servico = servico;
        }

        public override async Task ExecutarAsync()
        {
            LogarInformacao("Iniciando a execução do Job de Carga de Usuários.");
            LogarInformacao("Obtendo usuários no Channel.");

            var usuariosServico = await _servico.ObterUsuariosAtivosAsync();
            var usuariosBanco = await _repositorioUsuarios.ObterTodosUsuariosAsync();

            LogarInformacao($"{usuariosServico.Count()} usuários obtidos no Channel.");

            var novosUsuarios = usuariosServico.Where(c => !usuariosBanco.Any(x => x.Id == c.Id)).Where(c => !string.IsNullOrEmpty(c.Email)).ParaUsuariosChannel().ToList();

            LogarInformacao($"{novosUsuarios.Count} usuários serão inseridos.");

            foreach (var usuario in novosUsuarios)
            {
                var resultado = usuario.Validar();

                if (!resultado.Sucesso)
                {
                    LogarInformacao(@$"Não foi possível inserir o usuário {usuario.Id}. Devido aos erros abaixo:");

                    LogarErros(resultado.Erros.ToArray());
                }
            }

            await _repositorioUsuarios.InserirUsuariosAsync(novosUsuarios);
            await _repositorioUsuarios.SalvarAlteracoesAsync();

            LogarInformacao("Finalizando a execução do Job de Carga de Usuários.");
        }
    }
}
