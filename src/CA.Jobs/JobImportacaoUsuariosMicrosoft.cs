using CA.Core.Interfaces.Microsoft;
using CA.Identity.Entidades;
using CA.Identity.Interfaces;
using CA.Jobs.Interfaces;
using Microsoft.Extensions.Logging;

namespace CA.Jobs
{
    public class JobImportacaoUsuariosMicrosoft : Job<Usuario>
    {
        private readonly IServicoMicrosoftGraph _servicoGraph;
        private readonly IServicoIdentidade _servicoIdentidade;

        public JobImportacaoUsuariosMicrosoft(IServicoMicrosoftGraph servicoGraph, IServicoIdentidade servicoIdentidade, ILogger<Usuario> logger) : base(logger)
        {
            _servicoGraph = servicoGraph;
            _servicoIdentidade = servicoIdentidade;
        }

        public override async Task ExecutarAsync()
        {
            LogarInformacao("===> Iniciando a execução do Job de Importação de Usuários Microsoft. <===");

            LogarInformacao("Obtendo usuários Microsoft.");

            var usuariosMicrosoft = await _servicoGraph.ObterTodosUsuariosAsync();

            LogarInformacao($"{usuariosMicrosoft.Count()} usuários obtidos.");

            var usuariosLocal = await _servicoIdentidade.ObterTodosUsuariosAsync();

            var usuariosMicrosoftImportar = usuariosMicrosoft.Where(usuarioMicrosoft => !usuariosLocal.Any(usuarioLocal => usuarioLocal.Email == usuarioMicrosoft.Email)).ToList();
            var usuariosLocalExcluir = usuariosLocal.Where(usuariosLocal => !usuariosMicrosoft.Any(usuarioMicrosoft => usuarioMicrosoft.Email == usuariosLocal.Email)).ToList();

            LogarInformacao($"{usuariosMicrosoftImportar.Count()} usuários Microsoft serão importados.");
            LogarInformacao($"{usuariosLocalExcluir.Count()} usuários local serão excluídos.");

            if (usuariosMicrosoftImportar.Any())
            {
                LogarInformacao($"===> Importando usuários Microsoft <===");

                foreach (var usuario in usuariosMicrosoftImportar)
                {
                    var resultado = await _servicoIdentidade.ImportarUsuarioAsync(usuario.Email, usuario.NomeUsuario, usuario.NomeCompleto);

                    if (resultado.Sucesso)
                    {
                        LogarInformacao($"Usuário '{usuario.Email}' importado com sucesso.");
                        
                        if(!resultado.Valor.PossuiContaTfs)
                            LogarAlerta($"Usuário '{usuario.Email}' não possui conta no Tfs.");

                        if(!resultado.Valor.PossuiContaChannel)
                            LogarAlerta($"Usuário '{usuario.Email}' não possui conta no Channel.");

                        if(!resultado.Valor.PossuiContaPonto)
                            LogarAlerta($"Usuário '{usuario.Email}' não possui conta no Secullum.");
                    }
                    else
                    {
                        LogarInformacao($"Não foi possível importar o usuário '{usuario.Email}'. Devido aos erros abaixo:");

                        LogarErros(resultado.Erros.ToArray());
                    }
                }
            }

            if (usuariosLocalExcluir.Any())
            {
                LogarInformacao($"===> Excluindo usuários Microsoft <===");

                foreach (var usuario in usuariosLocalExcluir)
                {
                    var resultado = await _servicoIdentidade.ExcluirUsuarioPorIdAsync(usuario.Id);

                    if (resultado.Sucesso)
                    {
                        LogarInformacao($"Usuário '{usuario.Email}' excluído com sucesso.");
                    }
                    else
                    {
                        LogarInformacao($"Não foi possível excluír o usuário '{usuario.Email}'. Devido aos erros abaixo:");

                        LogarErros(resultado.Erros.ToArray());
                    }
                }
            }

            LogarInformacao("===> Finalizando a execução do Job de Importação de Usuários Microsoft. <===");
        }
    }
}
