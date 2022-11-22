using CA.Core.Configuracoes;
using CA.Core.Entidades.Tfs;
using CA.Core.Interfaces.Ponto;
using CA.Core.Interfaces.Tfs;
using CA.Core.Valores;
using CA.Seguranca.Entidades;
using CA.Seguranca.Extensions;
using CA.Seguranca.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace CA.Seguranca.Servicos
{
    public class ServicoIdentidade : IServicoIdentidade
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ConfiguracoesJwt _configuracaoJwt;
        private readonly ConfiguracoesGerais _configuracoesGerais;
        private readonly IRepositorioUsuariosTfs _repositorioTfs;
        private readonly IRepositorioPonto _repositorioPonto;
        private readonly IRepositorioColecoes _repositorioColecoes;

        public ServicoIdentidade(UserManager<IdentityUser> userManager, ConfiguracoesJwt configuracaoJwt, ConfiguracoesGerais configuracoesGerais, IRepositorioUsuariosTfs repositorioTfs, IRepositorioPonto repositorioPonto, IRepositorioColecoes repositorioColecoes)
        {
            _userManager = userManager;
            _configuracaoJwt = configuracaoJwt;
            _configuracoesGerais = configuracoesGerais;
            _repositorioTfs = repositorioTfs;
            _repositorioPonto = repositorioPonto;
            _repositorioColecoes = repositorioColecoes;            
        }

        public async Task<Resultado<UsuarioApp>> ImportarUsuarioAsync(IPrincipal usuarioLogado)
        {
            var nomeCompletoUsuarioLogado = usuarioLogado.ObterNomeCompleto();
            var nomeUsuarioLogado = usuarioLogado.ObterNomeUsuario();
            var emailUsuarioLogado = usuarioLogado.ObterEmailUsuario();

            if(emailUsuarioLogado is null || nomeUsuarioLogado is null || nomeCompletoUsuarioLogado is null)
                return Resultado.DeErros<UsuarioApp>(new Erro("Não foi possível identificar o usuário logado.", "usuarioLogado"));

            if (!emailUsuarioLogado.Contains(_configuracoesGerais.DominioEmailPermitido))
                return Resultado.DeErros<UsuarioApp>(new Erro("O usuário logado é de um domínio não permitido pela aplicação.", "email"));

            var usuarioIdentity = await _userManager.FindByEmailAsync(emailUsuarioLogado);
            var colecoes = await _repositorioColecoes.ObterTodasColecoesAsync();

            if (usuarioIdentity != null)
                return Resultado.DeErros<UsuarioApp>(new Erro("O usuário logado já possuí uma conta local.", "email"));

            var usuarioTfs = default(UsuarioTfs);

            foreach (var colecao in colecoes)
            {
                usuarioTfs = await _repositorioTfs.ObterUsuarioAsync(colecao, nomeUsuarioLogado);

                if (usuarioTfs is not null)
                    break;
            }

            var usuarioPonto = await _repositorioPonto.ObterFuncionarioPorNomeAsync(nomeCompletoUsuarioLogado);

            if(usuarioPonto is null)
                usuarioPonto = await _repositorioPonto.ObterFuncionarioPorNomeAsync(nomeCompletoUsuarioLogado.RemoverAcentos());

            var claims = new List<Claim>();

            usuarioIdentity = new IdentityUser
            {
                Email = emailUsuarioLogado,
                UserName = emailUsuarioLogado
            };

            var result = await _userManager.CreateAsync(usuarioIdentity);

            if (result.Succeeded)
            {
                claims.Add(new Claim(TiposClaims.Email, emailUsuarioLogado));
                claims.Add(new Claim(TiposClaims.NomeCompleto, nomeCompletoUsuarioLogado));

                if (usuarioTfs is not null)
                {
                    claims.Add(new Claim(TiposClaims.NomeUsuarioTfs, usuarioTfs.NomeUsuario));
                    claims.Add(new Claim(TiposClaims.DominioTfs, usuarioTfs.Dominio));
                    claims.Add(new Claim(TiposClaims.IdentidadeTfs, usuarioTfs.Identidade.Id));
                    claims.Add(new Claim(TiposClaims.TipoIdentidadeTfs, usuarioTfs.Identidade.Tipo));
                    claims.Add(new Claim(TiposClaims.ColecoesTfs, string.Join(";", usuarioTfs.Colecoes)));
                }

                if (usuarioPonto is not null)
                    claims.Add(new Claim(TiposClaims.PisFuncionario, usuarioPonto.NumeroPis));

                result = await _userManager.AddClaimsAsync(usuarioIdentity, claims);

                if (!result.Succeeded)
                {
                    await _userManager.DeleteAsync(usuarioIdentity);

                    return Resultado.DeErros<UsuarioApp>(result.Errors.Select(c => new Erro(c.Description, c.Code)).ToArray());
                }
            }
            else
            {
                return Resultado.DeErros<UsuarioApp>(result.Errors.Select(c => new Erro(c.Description, c.Code)).ToArray());
            }

            return Resultado.DeValor(new UsuarioApp
            {
                Email = usuarioIdentity.Email,
                NomeUsuario = usuarioIdentity.UserName,
                NomeCompleto = nomeCompletoUsuarioLogado,
                Colecoes = claims.ObterColecoesTfs(),
                PossuiContaPonto = usuarioPonto is not null,
                PossuiContaTfs = usuarioTfs is not null,
                Claims = claims,
                Roles = new string[0]
            });
        }

        public async Task<Resultado<CaJwt>> LoginAsync(string email)
        {
            var usuario = await ObterUsuarioPorEmailAsync(email);

            if (!usuario.Sucesso)
                return Resultado.DeErros<CaJwt>(usuario.Erros);

            return Resultado.DeValor(GerarTokenJwtPorUsuario(usuario.Valor));
        }

        public Resultado<CaJwt> Login(UsuarioApp usuario)
        {
            return Resultado.DeValor(GerarTokenJwtPorUsuario(usuario));
        }

        public async Task<Resultado<UsuarioApp>> ObterUsuarioPorEmailAsync(string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);
            
            if (usuario == null)
                return Resultado.DeErros<UsuarioApp>(new Erro("Não foi encontrado um usuário com o e-mail informado.", "email"));

            var claims = await _userManager.GetClaimsAsync(usuario);
            var roles = await _userManager.GetRolesAsync(usuario);

            var usuarioTfs = claims.ObterUsuarioTfs();

            var colecoes = usuarioTfs is not null ? await _repositorioColecoes.ObterColecoesPorUsuarioAsync(usuarioTfs) : new string[0];

            return Resultado.DeValor(new UsuarioApp
            {
                Email = usuario.Email,
                NomeUsuario = usuario.UserName,
                NomeCompleto = claims.ObterNomeCompleto(),
                Colecoes = colecoes,                
                Claims = claims,
                Roles = roles,
                PossuiContaTfs = usuarioTfs is not null,
                PossuiContaPonto = claims.ObterPisFuncionario() is not null                
            });
        }

        private CaJwt GerarTokenJwtPorUsuario(UsuarioApp usuario)
        {
            var handler = new JwtSecurityTokenHandler();

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _configuracaoJwt.Issuer,
                Audience = _configuracaoJwt.Audience,
                Subject = new ClaimsIdentity(usuario.Claims),
                Expires = DateTime.UtcNow.AddMinutes(_configuracaoJwt.TempoVidaToken),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuracaoJwt.Chave)), SecurityAlgorithms.HmacSha256)
            });

            var token = handler.WriteToken(securityToken);

            return new CaJwt() 
            { 
                TokenAcesso = token, 
                Validade = _configuracaoJwt.TempoVidaToken
            };
        }
    }
}
