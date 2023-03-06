using Hangfire.Console;
using Hangfire.Server;

namespace CA.Api.Hangfire.Logging
{
    public class HangfireConsoleLogger : ILogger
    {
        public IExternalScopeProvider? ScopeProvider { get; set; }

        private Dictionary<LogLevel, ConsoleTextColor> _colors = new Dictionary<LogLevel, ConsoleTextColor>
        {            
            { LogLevel.Warning, ConsoleTextColor.Yellow },
            { LogLevel.Information, ConsoleTextColor.White },
            { LogLevel.Debug, ConsoleTextColor.White },
            { LogLevel.Error, ConsoleTextColor.Red },
            { LogLevel.Trace, ConsoleTextColor.White },
            { LogLevel.Critical, ConsoleTextColor.DarkRed },
            { LogLevel.None, ConsoleTextColor.White },
        };

        public HangfireConsoleLogger(IExternalScopeProvider? scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            ScopeProvider?.ForEachScope((sc, st) =>
            {
                var context = sc as PerformContext;

                if (context != null)
                {
                    context.SetTextColor(_colors[logLevel]);
                    context.WriteLine(st);
                    context.ResetTextColor();
                }
            },
            state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => ScopeProvider?.Push(state) ?? default!;
    }
}
