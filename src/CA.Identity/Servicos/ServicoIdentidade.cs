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

            if (!resultado.Sucesso)
                return resultado;

            var usuarioIdentity = await _userManager.FindByEmailAsync(email);
            var colecoes = await _repositorioColecoes.ObterTodasColecoesAsync();

            if (usuarioIdentity != null)
                return Resultado.DeErros<UsuarioApp>(new Erro("O usuário informado já possuí uma conta local.", "email"));

            var usuarioTfs = default(UsuarioTfs);

            foreach (var colecao in colecoes)
            {
                usuarioTfs = await _repositorioTfs.ObterUsuarioPorNomeAsync(colecao, nomeUsuario);

                if (usuarioTfs is not null)
                    break;
            }

            var funcionario = await ObterFuncionarioPontoAsync(nomeCompleto);
            var usuarioChannel = ObterUsuarioChannel(email, nomeCompleto);

            if (usuarioTfs is null && funcionario is null && usuarioChannel is null)
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
                var resultIntegracao = await AdicionarClaimsDadosUsuario(usuarioIdentity, email, nomeCompleto);

                if (resultIntegracao.Sucesso)
                    resultIntegracao = await IntegrarContas(usuarioIdentity, usuarioTfs, usuarioChannel, funcionario);

                if (!resultIntegracao.Sucesso)
                {
                    await _userManager.DeleteAsync(usuarioIdentity);

                    return Resultado.DeErros<UsuarioApp>(result.Errors.Select(c => new Erro(c.Description, c.Code)).ToArray());
                }
            }
            else
            {
                return Resultado.DeErros<UsuarioApp>(result.Errors.Select(c => new Erro(c.Description, c.Code)).ToArray());
            }

            if (usuarioTfs is not null)
                claims.Add(new Claim(TiposClaims.ColecoesTfs, string.Join(";", usuarioTfs.Colecoes)));

            return Resultado.DeValor(new UsuarioApp
            {
                Id = new Guid(usuarioIdentity.Id),
                IdUsuarioTfs = claims.ObterIdentidadeTfs(),
                IdFuncionarioPonto = claims.ObterIdFuncionarioPonto(),
                IdUsuarioChannel = claims.ObterIdUsuarioChannel(),
                Email = usuarioIdentity.Email,
                NomeUsuario = usuarioIdentity.UserName,
                NomeCompleto = nomeCompleto,
                Colecoes = usuarioTfs is not null ? usuarioTfs.Colecoes : new string[0],
                PossuiContaPonto = funcionario is not null && !string.IsNullOrEmpty(funcionario.NumeroPis),
                PossuiContaTfs = usuarioTfs is not null,
                PossuiContaChannel = usuarioChannel is not null,
                Roles = new string[0],
                Claims = claims
            });
        }

        public async Task<Resultado> AtualizarUsuario(AtualizarUsuarioApp model)
        {
            var usuarioIdentity = await _userManager.FindByIdAsync(model.IdUsuario.ToString());
            var ehAdministrador = await _userManager.IsInRoleAsync(usuarioIdentity, "administrador");

            if(usuarioIdentity is null) 
                return Resultado.DeErros(new Erro("O usuário informado não foi encontrado.", nameof(model.IdUsuario)));

            var usuarioTfs = default(UsuarioTfs);

            if (model.EhAdministrador && !ehAdministrador)
            {
                await _userManager.AddToRoleAsync(usuarioIdentity, "administrador");
            }
            else if (ehAdministrador)
            {
                await _userManager.RemoveFromRoleAsync(usuarioIdentity, "administrador");
            }

            if (model.IdUsuarioTfs is not null)
            {
                var colecoes = await _repositorioColecoes.ObterTodasColecoesAsync();

                foreach (var colecao in colecoes)
                {
                    usuarioTfs = await _repositorioTfs.ObterUsuarioPorIdAsync(colecao, model.IdUsuarioTfs);

                    if (usuarioTfs is not null)
                        break;
                }

                if (usuarioTfs is null)
                    return Resultado.DeErros(new Erro("O usuário informado não foi encontrado no Tfs.", nameof(model.IdUsuarioTfs)));
            }

            var usuarioChannel = default(UsuarioChannel);

            if (model.IdUsuarioChannel is not null)
            {
                usuarioChannel = _repositorioChannel.ObterUsuarioPorId(model.IdUsuarioChannel.Value);

                if (usuarioChannel is null)
                    return Resultado.DeErros(new Erro("O usuário informado não foi encontrado no Channel.", nameof(model.IdUsuarioChannel)));
            }

            var funcionario = default(Funcionario);

            if (model.IdFuncionarioPonto is not null)
            {
                funcionario = await _repositorioPonto.ObterFuncionarioPorIdAsync(model.IdFuncionarioPonto.Value);

                if (funcionario is null)
                    return Resultado.DeErros(new Erro("O funcionário informado não foi encontrado no sistema de ponto.", nameof(model.IdFuncionarioPonto)));
            }

            return await IntegrarContas(usuarioIdentity, usuarioTfs, usuarioChannel, funcionario);
        }

        public async Task<IEnumerable<UsuarioApp>> ObterTodosUsuariosAsync()
        {
            var usuario = await _userManager.Users.Include(c => c.UserClaims).ToListAsync();
            var administradores = await _userManager.GetUsersInRoleAsync("administrador");

            return usuario.Select(usuario => {

                var claims = usuario.UserClaims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();

                return new UsuarioApp
                {
                    Id = new Guid(usuario.Id),
                    IdUsuarioTfs = claims.ObterIdentidadeTfs(),
                    IdFuncionarioPonto = claims.ObterIdFuncionarioPonto(),
                    IdUsuarioChannel = claims.ObterIdUsuarioChannel(),
                    Email = usuario.Email,
                    NomeUsuario = usuario.UserName,
                    NomeCompleto = claims.ObterNomeCompleto(),
                    Colecoes = claims.ObterColecoesTfs(),
                    EhAdministrador = administradores.Any(c => c.Id == usuario.Id),
                    PossuiContaPonto = claims.ObterPisFuncionario() is not null,                    
                    PossuiContaChannel = claims.ObterUsuarioChannel() is not null,
                    PossuiContaTfs = claims.ObterUsuarioTfs() is not null,
                    Roles = new string[0]
                };
            })
            .OrderBy(c => c.NomeCompleto)
            .ToList();
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
                IdUsuarioTfs = claims.ObterIdentidadeTfs(),
                IdFuncionarioPonto = claims.ObterIdFuncionarioPonto(),
                IdUsuarioChannel = claims.ObterIdUsuarioChannel(),
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

        private async Task<Resultado> AdicionarClaimsDadosUsuario(Usuario usuario, string email, string nomeCompleto)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(TiposClaims.Email, email));
            claims.Add(new Claim(TiposClaims.NomeCompleto, nomeCompleto));

            var result = await _userManager.AddClaimsAsync(usuario, claims);

            if (!result.Succeeded)
                return Resultado.DeErros<IEnumerable<Claim>>(result.Errors.Select(c => new Erro(c.Description, c.Code)).ToArray());

            return Resultado.DeSucesso();
        }

        private async Task<Resultado> IntegrarContas(Usuario usuarioIdentity, UsuarioTfs? usuarioTfs, UsuarioChannel? usuarioChannel, Funcionario? funcionarioPonto)
        {
            var claims = (await _userManager.GetClaimsAsync(usuarioIdentity)).Where(c => c.Type != TiposClaims.Email && c.Type != TiposClaims.NomeCompleto).ToList();

            var result = new IdentityResult();

            if (claims.Any())
            {
                result = await _userManager.RemoveClaimsAsync(usuarioIdentity, claims);

                if (!result.Succeeded)
                    return Resultado.DeErros(result.Errors.Select(c => new Erro(c.Description, c.Code)).ToArray());

                claims = new List<Claim>();
            }

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
            {
                claims.Add(new Claim(TiposClaims.IdFuncionario, funcionarioPonto.Id.ToString()));
                claims.Add(new Claim(TiposClaims.PisFuncionario, funcionarioPonto.NumeroPis));
            }

            result = await _userManager.AddClaimsAsync(usuarioIdentity, claims);

            if (!result.Succeeded)
                return Resultado.DeErros(result.Errors.Select(c => new Erro(c.Description, c.Code)).ToArray());

            return Resultado.DeSucesso();
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
