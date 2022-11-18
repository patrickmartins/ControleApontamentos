using Microsoft.AspNetCore.Localization;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracoesGlobalizacao
    {
        public static void UsarCulturaBr(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-br")
            });
        }
    }
}
