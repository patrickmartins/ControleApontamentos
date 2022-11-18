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
        private readonly IRepositorioUsuariosTfs _repositorioTfs;
        private readonly IRepositorioPonto _repositorioPonto;
        private readonly IRepositorioColecoes _repositorioColecoes;

        public ServicoIdentidade(UserManager<IdentityUser> userManager, ConfiguracoesJwt configuracaoJwt, IRepositorioUsuariosTfs repositorioTfs, IRepositorioPonto repositorioPonto, IRepositorioColecoes repositorioColecoes)
        {
            _userManager = userManager;
            _configuracaoJwt = configuracaoJwt;
            _repositorioTfs = repositorioTfs;
            _repositorioPonto = repositorioPonto;
            _repositorioColecoes = repositorioColecoes;
        }

        public async Task<Resultado<UsuarioApp>> ImportarUsuarioTfsAsync(IPrincipal usuarioLogado)
        {
            var usuarioIdentity = await _userManager.FindByEmailAsync(usuarioLogado.ObterEmailUsuario());
            var colecoes = await _repositorioColecoes.ObterTodasColecoesAsync();

            if (usuarioIdentity != null)
                return Resultado.DeErros<UsuarioApp>(new Erro("O usuário logado já possuí uma conta local.", "email"));

            var usuarioTfs = default(UsuarioTfs);

            foreach (var colecao in colecoes)
            {
                usuarioTfs = await _repositorioTfs.ObterUsuarioAsync(colecao, usuarioLogado.ObterNomeUsuario());

                if (usuarioTfs is not null)
                    break;
            }

            if (usuarioTfs is null)
                return Resultado.DeErros<UsuarioApp>(new Erro("O usuário logado não possui uma conta no TFS.", "email"));

            if (!usuarioTfs.Email.ToLower().Equals(usuarioLogado.ObterEmailUsuario()))
                return Resultado.DeErros<UsuarioApp>(new Erro("O e-mail do usuário logado é diferente do usuário do TFS.", "email"));

            var usuarioPonto = await _repositorioPonto.ObterFuncionarioPorNomeAsync(usuarioLogado.ObterNomeCompleto());

            if(usuarioPonto is not null)
                usuarioPonto = await _repositorioPonto.ObterFuncionarioPorNomeAsync(usuarioLogado.ObterNomeCompleto().RemoverAcentos());

            var claims = new List<Claim>();

            usuarioIdentity = new IdentityUser
            {
                Email = usuarioLogado.ObterEmailUsuario(),
                UserName = usuarioLogado.ObterEmailUsuario()
            };

            var result = await _userManager.CreateAsync(usuarioIdentity);

            if (result.Succeeded)
            {
                claims.Add(new Claim(TiposClaims.Email, usuarioLogado.ObterEmailUsuario()));
                claims.Add(new Claim(TiposClaims.NomeCompleto, usuarioLogado.ObterNomeCompleto()));
                claims.Add(new Claim(TiposClaims.DominioTfs, usuarioTfs.Dominio));
                claims.Add(new Claim(TiposClaims.IdentidadeTfs, usuarioTfs.Identidade.Id));
                claims.Add(new Claim(TiposClaims.TipoIdentidadeTfs, usuarioTfs.Identidade.Tipo));
                claims.Add(new Claim(TiposClaims.ColecoesTfs, string.Join(";", usuarioTfs.Colecoes)));

                if (usuarioPonto != null)
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
                NomeCompleto = claims.Any(c => c.Type.Equals(TiposClaims.NomeCompleto)) ? claims.First(c => c.Type.Equals(TiposClaims.NomeCompleto)).Value : string.Empty,
                Colecoes = claims.Where(c => c.Type.Equals(TiposClaims.ColecoesTfs)).SelectMany(c => c.Value.Split(';')).ToList(),
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

        public async Task<Resultado<UsuarioApp>> ObterUsuarioPorEmailAsync(string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);
            
            if (usuario == null)
                return Resultado.DeErros<UsuarioApp>(new Erro("Não foi encontrado um usuário com o e-mail informado.", "email"));

            var claims = await _userManager.GetClaimsAsync(usuario);
            var roles = await _userManager.GetRolesAsync(usuario);

            return Resultado.DeValor(new UsuarioApp
            {
                Email = usuario.Email,
                NomeUsuario = usuario.UserName,
                NomeCompleto = claims.Any(c => c.Type.Equals(TiposClaims.NomeCompleto)) ? claims.First(c => c.Type.Equals(TiposClaims.NomeCompleto)).Value : string.Empty,
                Colecoes = claims.Where(c => c.Type.Equals(TiposClaims.ColecoesTfs)).SelectMany(c => c.Value.Split(';')).ToList(),
                Claims = claims,
                Roles = roles
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
