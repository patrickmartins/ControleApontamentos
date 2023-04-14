using CA.Api.Configuracoes;
using CA.Jobs.Configuracoes;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AdicionarHangfireConsoleLogger();

builder.Services.AdicionarContextoDeDados(builder.Configuration);
builder.Services.AdicionarServicosCA(builder.Configuration);
builder.Services.AdicionarIdentity(builder.Configuration);
builder.Services.AdicionarHangfire(builder.Configuration);
builder.Services.AdicionarJobHangfire(builder.Configuration);
builder.Services.AdicionarMicrosoftGraph();
builder.Services.AdicionarPoliticasPolly();
builder.Services.AdicionarSwagger();
builder.Services.AdicionarJobs();

builder.Services
        .AddControllers()
        .ConfigurarSerializacaoJson();

builder.Services.AdicionarAutenticacao(builder.Configuration);
builder.Services.AdicionarCors();

var app = builder.Build();

app.UsarCulturaBr();
app.ConfigurarFlurl();

app.UseRouting();
app.UseCors("default");

app.UseAuthentication();
app.UseAuthorization();

app.UsarDashboardHanfire();
app.UsarHangfire();
app.UsarSwaggerUI();

app.MapControllers();
app.MapHangfireDashboard();

app.UseHttpsRedirection();

app.Run();