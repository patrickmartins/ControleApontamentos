using CA.Aplicacao.Interfaces;
using CA.Core.Entidades.CA;
using CA.Core.Interfaces.Microsoft;
using CA.Identity.Interfaces;
using CA.Jobs.Interfaces;
using Microsoft.Extensions.Logging;

namespace CA.Jobs
{
    public class JobImportacaoUsuariosMicrosoft : Job<UsuarioCA>
    {
        private readonly IServicoMicrosoftGraph _servicoGraph;
        private readonly IServicoIdentidade _servicoIdentidade;
        private readonly IServicoUsuariosCaApp _servicoUsuariosCa;

        public JobImportacaoUsuariosMicrosoft(IServicoMicrosoftGraph servicoGraph, IServicoIdentidade servicoIdentidade, IServicoUsuariosCaApp servicoUsuariosCa, ILogger<UsuarioCA> logger) : base(logger)
        {
            _servicoGraph = servicoGraph;
            _servicoIdentidade = servicoIdentidade;
            _servicoUsuariosCa = servicoUsuariosCa;
        }

        public override async Task ExecutarAsync()
        {
            LogarInformacao("===> Iniciando a execução do Job de Importação de Usuários Microsoft. <===");

            LogarInformacao("Obtendo usuários Microsoft.");

            var usuariosMicrosoft = await _servicoGraph.ObterTodosUsuariosAsync();

            LogarInformacao($"{usuariosMicrosoft.Count()} usuários obtidos.");

            var usuariosLocal = await _servicoIdentidade.ObterTodasContaUsuarioAsync();

            var usuariosMicrosoftImportar = usuariosMicrosoft.Where(usuarioMicrosoft => !usuariosLocal.Any(usuarioLocal => usuarioLocal.Email == usuarioMicrosoft.Email)).ToList();
            var usuariosLocalExcluir = usuariosLocal.Where(usuariosLocal => !usuariosMicrosoft.Any(usuarioMicrosoft => usuarioMicrosoft.Email == usuariosLocal.Email)).ToList();

            LogarInformacao($"{usuariosMicrosoftImportar.Count()} usuários Microsoft serão importados.");
            LogarInformacao($"{usuariosLocalExcluir.Count()} usuários local serão excluídos.");

            if (usuariosMicrosoftImportar.Any())
            {
                LogarInformacao($"===> Importando usuários Microsoft <===");

                foreach (var usuario in usuariosMicrosoftImportar)
                {
                    var resultadoImportacao = await _servicoUsuariosCa.ImportarUsuarioAsync(usuario.Email, usuario.NomeCompleto);

                    if (resultadoImportacao.Sucesso)
                    {
                        LogarInformacao($"Usuário '{usuario.Email}' importado com sucesso.");
                        
                        if(!resultadoImportacao.Valor.PossuiContaTfs)
                            LogarAlerta($"Usuário '{usuario.Email}' não possui conta no Tfs.");

                        if(!resultadoImportacao.Valor.PossuiContaChannel)
                            LogarAlerta($"Usuário '{usuario.Email}' não possui conta no Channel.");

                        if(!resultadoImportacao.Valor.PossuiContaPonto)
                            LogarAlerta($"Usuário '{usuario.Email}' não possui conta no Secullum.");

                        var resultadoConta = await _servicoIdentidade.CriarContaUsuarioAsync(usuario.Email);

                        if (!resultadoConta.Sucesso)
                        {
                            LogarInformacao($"Não foi possível criar a conta do usuário '{usuario.Email}'. Devido aos erros abaixo:");

                            LogarErros(resultadoConta.Erros.ToArray());
                        }
                    }
                    else
                    {
                        LogarInformacao($"Não foi possível importar o usuário '{usuario.Email}'. Devido aos erros abaixo:");

                        LogarErros(resultadoImportacao.Erros.ToArray());
                    }
                }
            }

            if (usuariosLocalExcluir.Any())
            {
                LogarInformacao($"===> Excluindo usuários Microsoft <===");

                foreach (var usuario in usuariosLocalExcluir)
                {
                    var resultadoExclusaoConta = await _servicoIdentidade.ExcluirContaUsuarioAsync(usuario.Email);

                    if (resultadoExclusaoConta.Sucesso)
                    {
                        var resultadoExclusaoUsuario = await _servicoIdentidade.ExcluirContaUsuarioAsync(usuario.Email);

                        LogarInformacao($"Conta do usuário '{usuario.Email}' excluída com sucesso.");
                    }
                    else
                    {
                        LogarInformacao($"Não foi possível excluír a conta do usuário '{usuario.Email}'. Devido aos erros abaixo:");

                        LogarErros(resultadoExclusaoConta.Erros.ToArray());
                    }
                }
            }

            LogarInformacao("===> Finalizando a execução do Job de Importação de Usuários Microsoft. <===");
        }
    }
}
