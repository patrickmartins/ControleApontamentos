using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Core.Valores;
using CA.Jobs.Channel.Extensions;
using CA.Jobs.Channel.Interfaces;
using CA.Servicos.Channel.Interfaces;

namespace CA.Jobs.Channel
{
    public class JobCargaUsuarios : IJobChannel<UsuarioChannel>
    {
        private readonly IRepositorioUsuariosChannel _repositorioUsuarios;

        private readonly IServicoChannelHttp _servico;

        public JobCargaUsuarios(IRepositorioUsuariosChannel repositorioUsuarios, IServicoChannelHttp servico)
        {
            _repositorioUsuarios = repositorioUsuarios;

            _servico = servico;
        }

        public async Task ExecutarAsync()
        {
            var resultado = Resultado.DeSucesso();

            var usuariosServico = await _servico.ObterUsuariosAtivosAsync();
            var usuariosBanco = await _repositorioUsuarios.ObterTodosUsuariosAsync();

            var novosUsuarios = usuariosServico.Where(c => !usuariosBanco.Any(x => x.Id == c.Id)).Where(c => !string.IsNullOrEmpty(c.Email)).ParaUsuariosChannel();

            foreach (var usuario in novosUsuarios)
            {
                resultado = usuario.Validar();

                if (!resultado.Sucesso)                
                    return;                
            }

            await _repositorioUsuarios.InserirUsuariosAsync(novosUsuarios);
            await _repositorioUsuarios.SalvarAlteracoesAsync();
        }
    }
}
