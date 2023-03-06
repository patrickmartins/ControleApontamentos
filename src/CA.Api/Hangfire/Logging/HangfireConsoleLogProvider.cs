using System.Collections.Concurrent;

namespace CA.Api.Hangfire.Logging
{
    [ProviderAlias("HangfireConsole")]
    public class HangfireConsoleLogProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly ConcurrentDictionary<string, HangfireConsoleLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

        private IExternalScopeProvider _scopeProvider;

        public HangfireConsoleLogProvider() { }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new HangfireConsoleLogger(_scopeProvider));
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;

            foreach (var logger in _loggers)
            {
                logger.Value.ScopeProvider = _scopeProvider;
            }
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}