using CA.Api.Configuracoes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AdicionarServicosCA(builder.Configuration);
builder.Services.AdicionarIdentity(builder.Configuration);
builder.Services.AdicionarPoliticasPolly();
builder.Services.AdicionarSwagger();

builder.Services
        .AddControllers()
        .ConfigurarSerializacaoJson();

builder.Services.AdicionarAutenticacao(builder.Configuration);
builder.Services.AdicionarCors();

var app = builder.Build();

app.UsarCulturaBr();

app.UseRouting();
app.UseCors("default");

app.UseAuthentication();
app.UseAuthorization();

app.UsarSwaggerUI();

app.MapControllers();

app.UseHttpsRedirection();

app.Run();