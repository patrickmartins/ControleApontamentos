using CA.Core.Configuracoes;
using CA.Core.Entidades.Channel;
using CA.Core.Entidades.Ponto;
using CA.Core.Entidades.Tfs;
using CA.Core.Interfaces.Channel;
using CA.Core.Interfaces.Ponto;
using CA.Core.Interfaces.Tfs;
using CA.Core.Valores;
using CA.Identity.Entidades;
using CA.Identity.Extensions;
using CA.Identity.Interfaces;
using CA.Identity.Models;
using CA.Util.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace CA.Identity.Servicos
{
    public class ServicoIdentidade : IServicoIdentidade
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly ConfiguracaoJwt _configuracaoJwt;
        private readonly ConfiguracaoGerais _configuracoesGerais;

        private readonly IRepositorioUsuariosTfs _repositorioTfs;
        private readonly IRepositorioPonto _repositorioPonto;
        private readonly IRepositorioUsuariosChannel _repositorioChannel;
        private readonly IRepositorioColecoes _repositorioColecoes;

        public ServicoIdentidade(UserManager<Usuario> userManager, ConfiguracaoJwt configuracaoJwt, ConfiguracaoGerais configuracoesGerais,
                                    IRepositorioUsuariosTfs repositorioTfs, IRepositorioPonto repositorioPonto, IRepositorioUsuariosChannel repositorioChannel, IRepositorioColecoes repositorioColecoes)
        {
            _userManager = userManager;
            _configuracaoJwt = configuracaoJwt;
            _configuracoesGerais = configuracoesGerais;
            _repositorioTfs = repositorioTfs;
            _repositorioPonto = repositorioPonto;
            _repositorioChannel = repositorioChannel;
            _repositorioColecoes = repositorioColecoes;
        }

        public async Task<IEnumerable<UsuarioApp>> ObterTodosUsuariosAsync()
        {
            var usuario = await _userManager.Users.Include(c => c.Claims).ToListAsync();

            return usuario.Select(usuario => {

                var claims = usuario.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();

                return new UsuarioApp
                {
                    Id = new Guid(usuario.Id),
                    Email = usuario.Email,
                    NomeUsuario = usuario.UserName,
                    NomeCompleto = claims.ObterNomeCompleto(),
                    Colecoes = claims.ObterColecoesTfs(),
                    PossuiContaPonto = claims.ObterPisFuncionario() is not null,                    
                    PossuiContaChannel = claims.ObterUsuarioChannel() is not null,
                    PossuiContaTfs = claims.ObterUsuarioTfs() is not null,
                    Roles = new string[0]
                };
            })
            .OrderBy(c => c.NomeCompleto)
            .ToList();
        }

        public async Task<Resultado<UsuarioApp>> ImportarUsuarioAsync(string email, string nomeUsuario, string nomeCompleto)
        {
            var resultado = new Resultado<UsuarioApp>();

            if (!email.Contains(_configuracoesGerais.DominioEmailPermitido))
                resultado.AddError(new Erro("O e-mail informado é de um domínio não permitido pela aplicação.", "email"));

            if (string.IsNullOrEmpty(email))
                resultado.AddError(new Erro("O e-mail do usuário não foi informado.", "email"));

            if (string.IsNullOrEmpty(nomeUsuario))
                resultado.AddError(new Erro("O nome de usuário não foi informado.", "nomeUsuario"));

            if (string.IsNullOrEmpty(nomeCompleto))
                resultado.AddError(new Erro("O nome completo do usuário não foi informado.", "nomeCompleto"));

            if(!resultado.Sucesso)
                return resultado;

            var usuarioIdentity = await _userManager.FindByEmailAsync(email);
            var colecoes = await _repositorioColecoes.ObterTodasColecoesAsync();

            if (usuarioIdentity != null)
                return Resultado.DeErros<UsuarioApp>(new Erro("O usuário informado já possuí uma conta local.", "email"));

            var usuarioTfs = default(UsuarioTfs);

            foreach (var colecao in colecoes)
            {
                usuarioTfs = await _repositorioTfs.ObterUsuarioAsync(colecao, nomeUsuario);

                if (usuarioTfs is not null)
                    break;
            }

            var funcionarioPonto = await ObterFuncionarioPontoAsync(nomeCompleto);
            var usuarioChannel = ObterUsuarioChannel(email, nomeCompleto);

            if(usuarioTfs is null && funcionarioPonto is null && usuarioChannel is null)
                return Resultado.DeErros<UsuarioApp>(new Erro("O usuário informado não possuí uma conta no Tfs, no Channel e no Secullum.", "email"));

            var claims = new List<Claim>();

            usuarioIdentity = new Usuario
            {
                Email = email,
                UserName = email
            };

            var result = await _userManager.CreateAsync(usuarioIdentity);

            if (result.Succeeded)
            {
                claims.Add(new Claim(TiposClaims.Email, email));
                claims.Add(new Claim(TiposClaims.NomeCompleto, nomeCompleto));

                if (usuarioTfs is not null)
                {
                    claims.Add(new Claim(TiposClaims.NomeUsuarioTfs, usuarioTfs.NomeUsuario));
                    claims.Add(new Claim(TiposClaims.DominioTfs, usuarioTfs.Dominio));
                    claims.Add(new Claim(TiposClaims.IdentidadeTfs, usuarioTfs.Identidade.Id));
                    claims.Add(new Claim(TiposClaims.TipoIdentidadeTfs, usuarioTfs.Identidade.Tipo));
                }

                if (usuarioChannel is not null)
                {
                    claims.Add(new Claim(TiposClaims.IdUsuarioChannel, usuarioChannel.Id.ToString()));
                    claims.Add(new Claim(TiposClaims.EmailUsuarioChannel, usuarioChannel.Email));
                }

                if (funcionarioPonto is not null && !string.IsNullOrEmpty(funcionarioPonto.NumeroPis))
                    claims.Add(new Claim(TiposClaims.PisFuncionario, funcionarioPonto.NumeroPis));

                result = await _userManager.AddClaimsAsync(usuarioIdentity, claims);

                if (usuarioTfs is not null)
                    claims.Add(new Claim(TiposClaims.ColecoesTfs, string.Join(";", usuarioTfs.Colecoes)));

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
                Id = new Guid(usuarioIdentity.Id),
                Email = usuarioIdentity.Email,
                NomeUsuario = usuarioIdentity.UserName,
                NomeCompleto = nomeCompleto,
                Colecoes = usuarioTfs is not null ? usuarioTfs.Colecoes : new string[0],
                PossuiContaPonto = funcionarioPonto is not null && !string.IsNullOrEmpty(funcionarioPonto.NumeroPis),
                PossuiContaTfs = usuarioTfs is not null,
                PossuiContaChannel = usuarioChannel is not null,
                Roles = new string[0],
                Claims = claims
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

        public async Task<Resultado<UsuarioApp>> ObterUsuarioPorIdAsync(Guid id)
        {
            var usuario = await _userManager.FindByIdAsync(id.ToString());

            if (usuario == null)
                return Resultado.DeErros<UsuarioApp>(new Erro("Não foi encontrado um usuário com o id informado.", nameof(id)));

            return await ObterUsuarioAppAsync(usuario);
        }

        public async Task<Resultado<UsuarioApp>> ExcluirUsuarioPorIdAsync(Guid id)
        {
            var usuario = _userManager.Users.FirstOrDefault(c => c.Id == id.ToString());

            if (usuario is null)
                return Resultado.DeErros<UsuarioApp>(new Erro("O usuário informado não existe", nameof(id)));

            var resultado = await _userManager.DeleteAsync(usuario);

            if(!resultado.Succeeded) 
                return Resultado.DeErros<UsuarioApp>(resultado.Errors.Select(c => new Erro(c.Description, c.Code)).ToArray());

            return Resultado.DeSucesso<UsuarioApp>();
        }

        public async Task<Resultado<UsuarioApp>> ObterUsuarioPorEmailAsync(string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);

            if (usuario == null)
                return Resultado.DeErros<UsuarioApp>(new Erro("Não foi encontrado um usuário com o e-mail informado.", nameof(email)));

            return await ObterUsuarioAppAsync(usuario);
        }

        private async Task<Resultado<UsuarioApp>> ObterUsuarioAppAsync(Usuario usuario)
        {
            var claims = await _userManager.GetClaimsAsync(usuario);
            var roles = await _userManager.GetRolesAsync(usuario);

            var usuarioTfs = claims.ObterUsuarioTfs();

            var colecoes = usuarioTfs is not null ? await _repositorioColecoes.ObterColecoesPorUsuarioAsync(usuarioTfs) : new string[0];

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.Add(new Claim(TiposClaims.ColecoesTfs, string.Join(";", colecoes)));

            return Resultado.DeValor(new UsuarioApp
            {
                Id = new Guid(usuario.Id),
                Email = usuario.Email,
                NomeUsuario = usuario.UserName,
                NomeCompleto = claims.ObterNomeCompleto(),                
                PossuiContaPonto = claims.ObterPisFuncionario() is not null,
                PossuiContaChannel = claims.ObterUsuarioChannel() is not null,
                PossuiContaTfs = usuarioTfs is not null,
                Colecoes = colecoes,
                Roles = roles,
                Claims = claims
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

        private async Task<Funcionario?> ObterFuncionarioPontoAsync(string nomeCompleto)
        {
            var funcionarioPonto = await _repositorioPonto.ObterFuncionarioPorNomeAsync(nomeCompleto.RemoverEspacosDuplicados());

            if (funcionarioPonto is null)
                funcionarioPonto = await _repositorioPonto.ObterFuncionarioPorNomeAsync(nomeCompleto.RemoverAcentos().RemoverEspacosDuplicados());

            return funcionarioPonto;
        }

        private UsuarioChannel? ObterUsuarioChannel(string email, string nomeCompleto)
        {
            var usuarioChannel = _repositorioChannel.ObterUsuarioPorEmail(email);

            if (usuarioChannel is null)
            {
                usuarioChannel = _repositorioChannel.ObterUsuarioPorNomeCompleto(nomeCompleto.RemoverEspacosDuplicados());

                if (usuarioChannel is null)
                {
                    usuarioChannel = _repositorioChannel.ObterUsuarioPorNomeCompleto(nomeCompleto.RemoverAcentos().RemoverEspacosDuplicados());
                }
            }

            return usuarioChannel;
        }

    }
}
