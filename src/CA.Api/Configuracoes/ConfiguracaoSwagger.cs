using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracaoSwagger
    {
        public static void AdicionarSwagger(this IServiceCollection servicos)
        {
            servicos.AddEndpointsApiExplorer();
            servicos.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API do Controle de Apontamento",
                    Description = string.Empty,
                    Contact = new OpenApiContact { 
                        Name = "Patrick Souza Martins", 
                        Email = "patrick.martins@vixteam.com.br", 
                        Url = new Uri("https://teams.microsoft.com/l/chat/0/0?users=patrick.martins@vixteam.com.br")
                    }
                });
                
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    BearerFormat = "JWT",
                    Description = "Token de acesso a API",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        public static void UsarSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.DefaultModelsExpandDepth(-1);
                config.DocumentTitle = "API do Controle de Apontamento";
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "ApontamentoTFS API V1.0");
            });
        }
    }
}
