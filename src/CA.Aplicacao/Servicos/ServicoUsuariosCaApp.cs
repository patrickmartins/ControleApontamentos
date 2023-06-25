using CA.Aplicacao.Extensions;
using CA.Aplicacao.Interfaces;
using CA.Aplicacao.Models;
using CA.Core.Configuracoes;
using CA.Core.Entidades.CA;
using CA.Core.Entidades.Channel;
using CA.Core.Entidades.Ponto;
using CA.Core.Entidades.Tfs;
using CA.Core.Interfaces.CA;
using CA.Core.Interfaces.Channel;
using CA.Core.Interfaces.Ponto;
using CA.Core.Interfaces.Tfs;
using CA.Core.Valores;
using CA.Util.Extensions;

namespace CA.Core.Servicos.CA
{
    public class ServicoUsuariosCaApp : IServicoUsuariosCaApp
    {
        private readonly ConfiguracaoGerais _configuracoesGerais;

        private readonly IServicoUsuariosCa _servicoUsuarioCa;
        private readonly IServicoChannel _servicoChannel;
        private readonly IServicoTfs _servicoTfs;
        private readonly IServicoPonto _servicoPonto;
        private readonly IServicoAdministracao _servicoAdministracao;

        public ServicoUsuariosCaApp(ConfiguracaoGerais configuracoesGerais, IServicoUsuariosCa servicoUsuarioCa, IServicoChannel servicoChannel, IServicoTfs servicoTfs, IServicoPonto servicoPonto, IServicoAdministracao servicoAdministracao)
        {
            _configuracoesGerais = configuracoesGerais;

            _servicoUsuarioCa = servicoUsuarioCa;
            _servicoChannel = servicoChannel;
            _servicoTfs = servicoTfs;
            _servicoPonto = servicoPonto;
            _servicoAdministracao = servicoAdministracao;
        }

        public async Task<Resultado<UsuarioModel>> AtualizarUsuarioAsync(AtualizarUsuarioModel atualizacao)
        {
            var resultadoUsuario = _servicoUsuarioCa.ObterUsuarioPorId(atualizacao.IdUsuario.ToString());

            if(!resultadoUsuario.Sucesso)
                return Resultado.DeErros<UsuarioModel>(resultadoUsuario.Erros);

            var usuario = resultadoUsuario.Valor;

            if (!string.IsNullOrEmpty(atualizacao.IdUsuarioTfs))
            {
                var resultadoUsuarioTfs = await ObterUsuarioTfsPorIdAsync(atualizacao.IdUsuarioTfs);

                if (!resultadoUsuarioTfs.Sucesso)
                    return Resultado.DeErros<UsuarioModel>(resultadoUsuarioTfs.Erros);
                
                var usuarioTfs = resultadoUsuarioTfs.Valor;

                usuario.ParametrosIntegracoes.IdUsuarioTfs = usuarioTfs.Identidade?.Id;
                usuario.ParametrosIntegracoes.TipoIdUsuarioTfs = usuarioTfs.Identidade?.Tipo;
                usuario.ParametrosIntegracoes.DominioTfs = usuarioTfs.Dominio;
                usuario.ParametrosIntegracoes.NomeUsuarioTfs = usuarioTfs.NomeUsuario;
            }
            else
            {
                usuario.ParametrosIntegracoes.IdUsuarioTfs = null;
                usuario.ParametrosIntegracoes.TipoIdUsuarioTfs = null;
                usuario.ParametrosIntegracoes.DominioTfs = null;
                usuario.ParametrosIntegracoes.NomeUsuarioTfs = null;
            }

            if (atualizacao.IdUsuarioChannel.HasValue)
            {
                var resultadoUsuarioChannel = ObterUsuarioChannelPorId(atualizacao.IdUsuarioChannel.Value);

                if (!resultadoUsuarioChannel.Sucesso)
                    return Resultado.DeErros<UsuarioModel>(resultadoUsuarioChannel.Erros);

                var usuarioChannel = resultadoUsuarioChannel.Valor;

                usuario.ParametrosIntegracoes.IdUsuarioChannel = usuarioChannel.Id;
                usuario.ParametrosIntegracoes.NomeUsuarioChannel = usuarioChannel.NomeUsuario;
            }
            else
            {
                usuario.ParametrosIntegracoes.IdUsuarioChannel = null;
                usuario.ParametrosIntegracoes.NomeUsuarioChannel = null;
            }

            if (atualizacao.IdFuncionarioPonto.HasValue)
            {
                var resultadoFuncionarioPonto = await ObterFuncionarioPontoPorIdAsync(atualizacao.IdFuncionarioPonto.Value);

                if (!resultadoFuncionarioPonto.Sucesso)
                    return Resultado.DeErros<UsuarioModel>(resultadoFuncionarioPonto.Erros);

                var funcionario = resultadoFuncionarioPonto.Valor;

                usuario.ParametrosIntegracoes.IdFuncionarioPonto = funcionario.Id;
                usuario.ParametrosIntegracoes.PisFuncionarioPonto = funcionario.NumeroPis;
            }
            else
            {
                usuario.ParametrosIntegracoes.IdFuncionarioPonto = null;
                usuario.ParametrosIntegracoes.PisFuncionarioPonto = null;
            }

            if (!string.IsNullOrEmpty(atualizacao.IdUnidade))
            {
                var resultadoUnidade = _servicoAdministracao.ObterUnidadePorId(atualizacao.IdUnidade);

                if (!resultadoUnidade.Sucesso)
                    return Resultado.DeErros<UsuarioModel>(resultadoUnidade.Erros);

                usuario.Unidade = resultadoUnidade.Valor;
            }
            else
            {
                usuario.IdUnidade = null;
                usuario.Unidade = null;
            }

            if (!string.IsNullOrEmpty(atualizacao.IdGerente))
            {
                var resultadoGerente = _servicoUsuarioCa.ObterUsuarioPorId(atualizacao.IdGerente);

                if (!resultadoGerente.Sucesso)
                    return Resultado.DeErros<UsuarioModel>(resultadoGerente.Erros);

                usuario.Gerente = resultadoGerente.Valor;
            }
            else
            {
                usuario.IdGerente = null;
                usuario.Gerente = null;                
            }

            var resultado = await _servicoUsuarioCa.AtualizarUsuarioAsync(usuario);

            if(!resultado.Sucesso)
                return Resultado.DeErros<UsuarioModel>(resultado.Erros);

            return Resultado.DeValor(resultado.Valor.UsuarioCaParaUsuarioModel());
        }

        public async Task<Resultado<UsuarioModel>> ImportarUsuarioAsync(string email, string nomeCompleto)
        {
            if (!email.Contains(_configuracoesGerais.DominioEmailPermitido))
                return Resultado.DeErros<UsuarioModel>(new Erro("O e-mail informado é de um domínio não permitido pela aplicação.", nameof(email)));

            var resultadoUsuario = _servicoUsuarioCa.ObterUsuarioPorEmail(email);

            if (resultadoUsuario.Sucesso)
                return Resultado.DeErros<UsuarioModel>(new Erro("O usuário informado já existe.", nameof(email)));

            var usuario = new UsuarioCA
            {
                Email = email,
                NomeCompleto = nomeCompleto
            };

            var resultadoUsuarioValido = usuario.Validar();

            if (!resultadoUsuarioValido.Sucesso)
                return Resultado.DeErros<UsuarioModel>(resultadoUsuarioValido.Erros);

            var resultadoUsuarioTfs = await ObterUsuarioTfsPorEmailAsync(email);
            var resultadoFuncionario = await ObterFuncionarioPontoPorNomeAsync(nomeCompleto);
            var resultadoUsuarioChannel = ObterUsuarioChannelPorEmailENome(email, nomeCompleto);

            if (!resultadoUsuarioTfs.Sucesso && !resultadoUsuarioChannel.Sucesso)
                return Resultado.DeErros<UsuarioModel>(new Erro("O usuário informado não possuí uma conta no Tfs e no Channel.", nameof(email)));

            usuario.ParametrosIntegracoes = new ParametrosIntegracao
            {
                DominioTfs = resultadoUsuarioTfs.Sucesso ? resultadoUsuarioTfs.Valor?.Dominio : null,
                IdUsuarioTfs = resultadoUsuarioTfs.Sucesso ? resultadoUsuarioTfs.Valor?.Identidade?.Id : null,
                TipoIdUsuarioTfs = resultadoUsuarioTfs.Sucesso ? resultadoUsuarioTfs.Valor?.Identidade?.Tipo : null,
                NomeUsuarioTfs = resultadoUsuarioTfs.Sucesso ? resultadoUsuarioTfs.Valor?.NomeUsuario : null,
                IdFuncionarioPonto = resultadoFuncionario.Sucesso ? resultadoFuncionario.Valor?.Id : null,
                PisFuncionarioPonto = resultadoFuncionario.Sucesso ? resultadoFuncionario.Valor?.NumeroPis : null,
                IdUsuarioChannel = resultadoUsuarioChannel.Sucesso ? resultadoUsuarioChannel.Valor?.Id : null,
                NomeUsuarioChannel = resultadoUsuarioChannel.Sucesso ? resultadoUsuarioChannel.Valor?.NomeUsuario : null
            };

            var resultado = await _servicoUsuarioCa.AdicionarUsuarioAsync(usuario);

            if (!resultado.Sucesso)
                return Resultado.DeErros<UsuarioModel>(resultado.Erros);

            return Resultado.DeValor(resultado.Valor.UsuarioCaParaUsuarioModel());
        }

        public async Task<IEnumerable<UsuarioModel>> ObterTodosUsuariosAsync()
        {
            var usuarios = await _servicoUsuarioCa.ObterTodosUsuariosAsync();

            return usuarios.UsuariosCaParaUsuariosModel().OrderBy(c => c.NomeCompleto).ToList();
        }

        public async Task<IEnumerable<UsuarioModel>> ObterTodosGerentesAsync()
        {
            var usuarios = await _servicoUsuarioCa.ObterTodosGerentesAsync();

            return usuarios.UsuariosCaParaUsuariosModel().OrderBy(c => c.NomeCompleto).ToList();
        }

        public Resultado<UsuarioModel?> ObterUsuarioPorId(Guid id)
        {
            var resultado = _servicoUsuarioCa.ObterUsuarioPorId(id.ToString());

            if(!resultado.Sucesso)
                return Resultado.DeErros<UsuarioModel?>(resultado.Erros);

            return Resultado.DeValor<UsuarioModel?>(resultado.Valor.UsuarioCaParaUsuarioModel());
        }

        public Task<Resultado> ExcluirUsuarioPorIdAsync(Guid id)
        {
            return _servicoUsuarioCa.ExcluirUsuarioPorIdAsync(id.ToString());
        }

        public async Task<IEnumerable<UsuarioModel>> ObterUsuariosPorUnidadeAsync(string idUnidade)
        {
            var usuarios = await _servicoUsuarioCa.ObterUsuariosPorUnidadeAsync(idUnidade);

            return usuarios.UsuariosCaParaUsuariosModel();
        }

        private async Task<Resultado<UsuarioTfs?>> ObterUsuarioTfsPorIdAsync(string id)
        {
            var colecoesTfs = await _servicoTfs.ObterTodasColecoesAsync();

            var usuarioTfs = default(UsuarioTfs);

            foreach (var colecao in colecoesTfs)
            {
                var resultado = await _servicoTfs.ObterUsuarioPorIdAsync(id, colecao);

                if (resultado.Sucesso)
                {
                    usuarioTfs = resultado.Valor;

                    break;
                }                    
            }

            if (usuarioTfs is null)
                return Resultado.DeErros<UsuarioTfs?>(new Erro("O usuário do TFS informado não foi encontrado.", nameof(id)));

            return Resultado.DeValor<UsuarioTfs?>(usuarioTfs);
        }

        private async Task<Resultado<UsuarioTfs?>> ObterUsuarioTfsPorEmailAsync(string email)
        {
            var colecoesTfs = await _servicoTfs.ObterTodasColecoesAsync();

            var usuarioTfs = default(UsuarioTfs);

            foreach (var colecao in colecoesTfs)
            {
                var resultado = await _servicoTfs.ObterUsuarioPorNomeAsync(email.Split("@")[0], colecao);

                if (resultado.Sucesso)
                {
                    usuarioTfs = resultado.Valor;

                    break;
                }
            }

            if (usuarioTfs is null)
                return Resultado.DeErros<UsuarioTfs?>(new Erro("O usuário do TFS informado não foi encontrado.", nameof(email)));

            return Resultado.DeValor<UsuarioTfs?>(usuarioTfs);
        }

        private async Task<Resultado<Funcionario?>> ObterFuncionarioPontoPorIdAsync(int id)
        {
            var resultado = await _servicoPonto.ObterFuncionarioPorIdAsync(id);

            if (resultado.Sucesso)
                return resultado;

            if (resultado.Valor is null)
                return Resultado.DeErros<Funcionario?>(new Erro("O funcionário informado não foi encontrado.", nameof(id)));

            return resultado;
        }

        private async Task<Resultado<Funcionario?>> ObterFuncionarioPontoPorNomeAsync(string nomeCompleto)
        {
            var resultado = await _servicoPonto.ObterFuncionarioPorNomeAsync(nomeCompleto.RemoverEspacosDuplicados());

            if (!resultado.Sucesso)
                resultado = await _servicoPonto.ObterFuncionarioPorNomeAsync(nomeCompleto.RemoverAcentos().RemoverEspacosDuplicados());

            if (resultado.Valor is null)
                return Resultado.DeErros<Funcionario?>(new Erro("O funcionário informado não foi encontrado.", nameof(nomeCompleto)));

            return resultado;
        }

        private Resultado<UsuarioChannel?> ObterUsuarioChannelPorId(int id)
        {
            return _servicoChannel.ObterUsuarioPorId(id);
        }

        private Resultado<UsuarioChannel?> ObterUsuarioChannelPorEmailENome(string email, string nomeCompleto)
        {
            var resultado = _servicoChannel.ObterUsuarioPorEmail(email);

            if (!resultado.Sucesso)
            {
                resultado = _servicoChannel.ObterUsuarioPorNomeCompleto(nomeCompleto.RemoverEspacosDuplicados());

                if (!resultado.Sucesso)
                {
                    resultado = _servicoChannel.ObterUsuarioPorNomeCompleto(nomeCompleto.RemoverAcentos().RemoverEspacosDuplicados());
                }
            }

            return resultado;
        }
    }
}
