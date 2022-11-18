using CA.Core.Configuracoes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracoesAutenticacao
    {
        public static void AdicionarAutenticacao(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var config = services.BuildServiceProvider().GetService<ConfiguracoesJwt>();

                    options.SaveToken = true;
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = config.Audience,
                        ValidIssuer = config.Issuer,

                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.Chave))
                    };
                })
                .AddMicrosoftIdentityWebApi(configuration, jwtBearerScheme: OpenIdConnectDefaults.AuthenticationScheme)
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddInMemoryTokenCaches();
        }
    }
}
