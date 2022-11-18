namespace CA.Api.Configuracoes
{
    public static class ConfiguracaoCors
    {
        public static void AdicionarCors(this IServiceCollection services)
        {
            services.AddCors(config =>
            {
                config.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                });
            });
        }
    }
}
