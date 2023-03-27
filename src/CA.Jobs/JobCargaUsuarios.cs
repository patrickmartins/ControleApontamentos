using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Jobs.Extensions;
using CA.Jobs.Interfaces;
using CA.Servicos.Channel.Interfaces;
using Microsoft.Extensions.Logging;

namespace CA.Jobs
{
    public class JobCargaUsuarios : Job<UsuarioChannel>
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

            var usuariosServico = await _servico.ObterTodosUsuariosAsync();
            var usuariosBanco = await _repositorioUsuarios.ObterTodosUsuariosAsync();

            LogarInformacao($"{usuariosServico.Count()} usuários obtidos no Channel.");

            var usuariosInserir = usuariosServico.Where(c => !usuariosBanco.Any(x => x.Id == c.Id)).Where(c => !string.IsNullOrEmpty(c.Email)).ParaUsuariosChannel().ToList();
            var usuariosAtualizar = usuariosServico.Where(c => usuariosBanco.Any(x => x.Id == c.Id)).ParaUsuariosChannel().Where(c => usuariosBanco.Any(x => x.Id == c.Id && c != x)).ToList();

            LogarInformacao($"{usuariosInserir.Count} usuários serão inseridos.");
            LogarInformacao($"{usuariosAtualizar.Count} usuários serão atualizados.");

            foreach (var usuario in usuariosInserir)
            {
                var resultado = usuario.Validar();

                if (resultado.Sucesso)
                {
                    await _repositorioUsuarios.InserirUsuarioAsync(usuario);
                }
                else
                {
                    LogarInformacao(@$"Não foi possível inserir o usuário {usuario.Id}. Devido aos erros abaixo:");

                    LogarErros(resultado.Erros.ToArray());
                }
            }

            foreach (var usuarioServico in usuariosAtualizar)
            {
                var usuarioBanco = usuariosBanco.First(c => c.Id == usuarioServico.Id);

                var resultado = usuarioServico.Validar();

                if (resultado.Sucesso)
                {
                    usuarioBanco.Atualizar(usuarioServico);

                    _repositorioUsuarios.AtualizarUsuario(usuarioBanco);
                }
                else
                {
                    LogarInformacao(@$"Não foi possível atualizar o usuário {usuarioServico.Id}. Devido aos erros abaixo:");

                    LogarErros(resultado.Erros.ToArray());
                }
            }

            await _repositorioUsuarios.SalvarAlteracoesAsync();

            LogarInformacao("Finalizando a execução do Job de Carga de Usuários.");
        }
    }
}
