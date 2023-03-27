using CA.Core.Valores;
using Microsoft.Extensions.Logging;

namespace CA.Jobs.Interfaces
{
    public abstract class Job<TEntidade> : IJob<TEntidade>
    {
        private readonly ILogger _logger;

        public Job(ILogger<TEntidade> logger)
        {
            _logger = logger;
        }

        public abstract Task ExecutarAsync();

        protected void LogarInformacao(string informacao, params object[] args)
        {
            _logger.LogInformation(informacao, args);
        }

        protected void LogarAlerta(string alerta, params object[] args)
        {
            _logger.LogWarning(alerta, args);
        }

        protected void LogarErros(string erro, params object[] args)
        {
            _logger.LogError(erro, args);
        }

        protected void LogarErros(params Erro[] erros)
        {
            foreach (var erro in erros)
                _logger.LogError($"\t[{erro.Origem}] {erro.Descricao}");
        }
    }
}
