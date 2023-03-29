using Microsoft.AspNetCore.Localization;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracaoGlobalizacao
    {
        public static void UsarCulturaBr(this IApplicationBuilder app)
        {
            var cultura = new RequestLocalizationOptions().SetDefaultCulture("pt-BR")
                                                            .AddSupportedCultures("pt-BR")
                                                            .AddSupportedUICultures("pt-BR");

            app.UseRequestLocalization(cultura);
        }
    }
}
