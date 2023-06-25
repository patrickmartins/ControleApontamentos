using CA.Aplicacao.Extensions;
using CA.Core.Configuracoes;
using CA.Core.Entidades.CA;
using CA.Core.Extensions;
using CA.Core.Interfaces.CA;
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
using System.Text;

namespace CA.Identity.Servicos
{
    public class ServicoIdentidade : IServicoIdentidade
    {
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly ConfiguracaoJwt _configuracaoJwt;

        private readonly IServicoUsuariosCa _servicoUsuarioCa;
        private readonly IServicoTfs _servicoTfs;

        public ServicoIdentidade(UserManager<UsuarioIdentity> userManager, ConfiguracaoJwt configuracaoJwt, IServicoUsuariosCa servicoUsuarioCa, IServicoTfs servicoTfs)
        {
            _userManager = userManager;
            _configuracaoJwt = configuracaoJwt;
            _servicoUsuarioCa = servicoUsuarioCa;
            _servicoTfs = servicoTfs;
        }

        public async Task<IEnumerable<UsuarioIdentityModel>> ObterTodasContaUsuarioAsync()
        {
            var administrador = await _userManager.GetUsersInRoleAsync("administrador");

            var usuarios = await ObterTodosUsuariosIdentityAsync();

            return usuarios.Select(c => c.UsuarioIdentityParaUsuarioIdentityModel(new string[0], new string[0], administrador.Any(x => x.Id == c.Id))).OrderBy(c => c.NomeCompleto).ToList();
        }

        public async Task<Resultado<UsuarioIdentityModel>> ObterContaUsuarioPorIdAsync(Guid id)
        {
            var usuarioIdentity = await ObterUsuarioIdentityPorIdAsync(id.ToString());

            if (usuarioIdentity is null)
                return Resultado.DeErros<UsuarioIdentityModel>(new Erro("Não foi encontrado uma conta associada ao usuário informado.", nameof(id)));

            var colecoes = await ObterColecoesPorUsuarioAsync(usuarioIdentity.Usuario);
            var roles = await _userManager.GetRolesAsync(usuarioIdentity);
            var ehAdministrador = roles.Contains("administrador");

            return Resultado.DeValor(usuarioIdentity.UsuarioIdentityParaUsuarioIdentityModel(colecoes, roles.ToArray(), ehAdministrador));
        }

        public async Task<Resultado<UsuarioIdentityModel>> ObterContaUsuarioPorEmailAsync(string email)
        {
            var usuarioIdentity = await ObterUsuarioIdentityPorEmailAsync(email);

            if (usuarioIdentity is null)
                return Resultado.DeErros<UsuarioIdentityModel>(new Erro("Não foi encontrado uma conta associada ao usuário informado.", nameof(email)));

            var colecoes = await ObterColecoesPorUsuarioAsync(usuarioIdentity.Usuario);
            var roles = await _userManager.GetRolesAsync(usuarioIdentity);
            var ehAdministrador = roles.Contains("administrador");

            return Resultado.DeValor(usuarioIdentity.UsuarioIdentityParaUsuarioIdentityModel(colecoes, roles.ToArray(), ehAdministrador));
        }

        public async Task<Resultado<CaJwt>> LoginAsync(string email)
        {
            var usuario = await ObterUsuarioIdentityPorEmailAsync(email);

            if (usuario is null)
                return Resultado.DeErros<CaJwt>(new Erro("Não foi encontrado uma conta associada ao usuário informado.", nameof(email)));

            return Resultado.DeValor(await GerarTokenJwtPorUsuarioAsync(usuario));
        }

        public async Task<Resultado> ExcluirContaUsuarioAsync(string email)
        {
            var usuario = await ObterUsuarioIdentityPorEmailAsync(email);

            if (usuario is null)
                return Resultado.DeErros<CaJwt>(new Erro("Não foi encontrado uma conta associada ao usuário informado.", nameof(email)));

            var resultado = await _userManager.DeleteAsync(usuario);

            if(!resultado.Succeeded) 
                return Resultado.DeErros(resultado.Errors.Select(c => new Erro(c.Description, c.Code)).ToArray());

            return Resultado.DeSucesso();
        }

        public async Task<Resultado<UsuarioIdentityModel>> CriarContaUsuarioAsync(string email)
        {
            var usuarioIdentity = await ObterUsuarioIdentityPorEmailAsync(email);

            if (usuarioIdentity is not null)
                return Resultado.DeErros<UsuarioIdentityModel>(new Erro("O usuário informado já possuí uma conta associada.", nameof(email)));

            var resultadoUsuarioCa = _servicoUsuarioCa.ObterUsuarioPorEmail(email);

            if (!resultadoUsuarioCa.Sucesso)
                return Resultado.DeErros<UsuarioIdentityModel>(resultadoUsuarioCa.Erros);

            var usuarioCa = resultadoUsuarioCa.Valor;

            usuarioIdentity = new UsuarioIdentity
            {
                Id = usuarioCa.Id,
                Email = email,
                UserName = email
            };

            var resultado = await _userManager.CreateAsync(usuarioIdentity);

            if(!resultado.Succeeded)
                return Resultado.DeErros<UsuarioIdentityModel>(resultado.Errors.Select(c => new Erro(c.Description, c.Code)).ToArray());

            var colecoes = await ObterColecoesPorUsuarioAsync(usuarioIdentity.Usuario);
            var roles = await _userManager.GetRolesAsync(usuarioIdentity);
            var ehAdministrador = roles.Contains("administrador");

            return Resultado.DeValor(usuarioIdentity.UsuarioIdentityParaUsuarioIdentityModel(colecoes, roles.ToArray(), ehAdministrador));
        }

        public async Task<Resultado> ConcederAcessoDeAdministradorAsync(string email)
        {
            var usuarioIdentity = await ObterUsuarioIdentityPorEmailAsync(email);

            if (usuarioIdentity is null)
                return Resultado.DeErros<CaJwt>(new Erro("Não foi encontrado uma conta associada ao usuário informado.", nameof(email)));

            var ehAdminstrador = await _userManager.IsInRoleAsync(usuarioIdentity, "administrador");

            if (ehAdminstrador)
                return Resultado.DeSucesso();

            var resultado = await _userManager.AddToRoleAsync(usuarioIdentity, "administrador");

            if (!resultado.Succeeded)
                return Resultado.DeErros(resultado.Errors.Select(c => new Erro(c.Description, c.Code)).ToArray());

            return Resultado.DeSucesso();
        }

        public async Task<Resultado> RemoverAcessoDeAdministradorAsync(string email)
        {
            var usuarioIdentity = await ObterUsuarioIdentityPorEmailAsync(email);

            if (usuarioIdentity is null)
                return Resultado.DeErros<CaJwt>(new Erro("Não foi encontrado uma conta associada ao usuário informado.", nameof(email)));

            var ehAdminstrador = await _userManager.IsInRoleAsync(usuarioIdentity, "administrador");

            if (!ehAdminstrador)
                return Resultado.DeSucesso();

            var resultado = await _userManager.RemoveFromRoleAsync(usuarioIdentity, "administrador");

            if (!resultado.Succeeded)
                return Resultado.DeErros(resultado.Errors.Select(c => new Erro(c.Description, c.Code)).ToArray());

            return Resultado.DeSucesso();
        }

        private async Task<CaJwt> GerarTokenJwtPorUsuarioAsync(UsuarioIdentity usuario)
        {
            var handler = new JwtSecurityTokenHandler();

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _configuracaoJwt.Issuer,
                Audience = _configuracaoJwt.Audience,
                Subject = new ClaimsIdentity(await GerarClaimsPorUsuarioAsync(usuario)),
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

        private async Task<IEnumerable<Claim>> GerarClaimsPorUsuarioAsync(UsuarioIdentity usuarioIdentity)
        {
            var claims = new List<Claim>();

            var usuarioCa = usuarioIdentity.Usuario;
            var usuarioTfs = usuarioCa.ExtrairUsuarioTfs();
            var usuarioChannel = usuarioCa.ExtrairUsuarioChannel();
            var funcionarioPonto = usuarioCa.ExtrairFuncionarioPonto();
                        
            var colecoes = usuarioTfs is not null ? await _servicoTfs.ObterColecoesPorUsuarioAsync(usuarioTfs) : new string[0];
            var roles = await _userManager.GetRolesAsync(usuarioIdentity);

            claims.Add(new Claim(TiposClaims.ColecoesTfs, string.Join(";", colecoes)));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (usuarioTfs is not null)
            {
                claims.Add(new Claim(TiposClaims.NomeUsuarioTfs, usuarioTfs.NomeUsuario));
                claims.Add(new Claim(TiposClaims.DominioTfs, usuarioTfs.Dominio));
                claims.Add(new Claim(TiposClaims.IdentidadeTfs, usuarioTfs.Identidade?.Id));
                claims.Add(new Claim(TiposClaims.TipoIdentidadeTfs, usuarioTfs.Identidade?.Tipo));
            }

            if (usuarioChannel is not null)
            {
                claims.Add(new Claim(TiposClaims.IdUsuarioChannel, usuarioChannel.Id.ToString()));
                claims.Add(new Claim(TiposClaims.EmailUsuarioChannel, usuarioChannel.NomeUsuario));
            }

            if (funcionarioPonto is not null && !string.IsNullOrEmpty(funcionarioPonto.NumeroPis))
            {
                claims.Add(new Claim(TiposClaims.IdFuncionario, funcionarioPonto.Id.ToString()));
                claims.Add(new Claim(TiposClaims.PisFuncionario, funcionarioPonto.NumeroPis));
            }

            claims.AddRange(usuarioIdentity.UserClaims.Select(c => new Claim(c.ClaimType, c.ClaimValue)));
            
            return claims;
        }

        private Task<IEnumerable<UsuarioIdentity>> ObterTodosUsuariosIdentityAsync()
        {
            return _userManager.Users
                            .Include(c => c.UserClaims)
                            .Include(c => c.Usuario.ParametrosIntegracoes)
                            .ToIListAsync();
        }

        private Task<UsuarioIdentity?> ObterUsuarioIdentityPorIdAsync(string id)
        {
            return _userManager.Users
                            .Include(c => c.UserClaims)
                            .Include(c => c.Usuario)
                            .ThenInclude(c => c.ParametrosIntegracoes)
                            .FirstOrDefaultAsync(c => c.Id == id.ToString());
        }

        private Task<UsuarioIdentity?> ObterUsuarioIdentityPorEmailAsync(string email)
        {
            return _userManager.Users
                            .Include(c => c.UserClaims)
                            .Include(c => c.Usuario)
                            .ThenInclude(c => c.ParametrosIntegracoes)
                            .FirstOrDefaultAsync(c => c.Email == email);
        }

        private async Task<string[]> ObterColecoesPorUsuarioAsync(UsuarioCA usuarioCa) 
        {
            var usuarioTfs = usuarioCa.ExtrairUsuarioTfs();

            if (usuarioTfs is null || usuarioTfs.Identidade is null)
                return new string[0];

            return await _servicoTfs.ObterColecoesPorUsuarioAsync(usuarioTfs);
        }
    }
}
