using System.Globalization;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracaoGlobalizacao
    {
        public static void UsarCulturaBr(this IApplicationBuilder app)
        {
            var cultura = new RequestLocalizationOptions().SetDefaultCulture("pt-BR")
                                                            .AddSupportedCultures("pt-BR")
                                                            .AddSupportedUICultures("pt-BR");

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pt-BR");

            app.UseRequestLocalization(cultura);
        }
    }
}
