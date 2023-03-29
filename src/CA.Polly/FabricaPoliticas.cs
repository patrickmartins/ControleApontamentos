using Polly;

namespace CA.Polly
{
    public class FabricaPoliticas
    {
        public static IAsyncPolicy CriarPoliticasTfs()
        {
            var circuitBreaker = Policy
                          .Handle<Exception>()
                          .CircuitBreakerAsync(3, TimeSpan.FromMinutes(10));

            var retry = Policy
                            .Handle<Exception>()
                            .WaitAndRetryAsync(
                                retryCount: 3,
                                sleepDurationProvider: (count) =>
                                {
                                    return TimeSpan.FromSeconds(count);
                                }
                            );

            return Policy.WrapAsync(circuitBreaker, retry);
        }

        public static IAsyncPolicy CriarPoliticasPonto()
        {
            var circuitBreaker = Policy
                          .Handle<Exception>()
                          .CircuitBreakerAsync(3, TimeSpan.FromMinutes(10));

            var retry = Policy
                            .Handle<Exception>()
                            .WaitAndRetryAsync(
                                retryCount: 3,
                                sleepDurationProvider: (count) =>
                                {
                                    return TimeSpan.FromSeconds(count);
                                }
                            );

            return Policy.WrapAsync(circuitBreaker, retry);
        }

        public static IAsyncPolicy CriarPoliticasServicoChannelHttp()
        {
            var circuitBreaker = Policy
                          .Handle<Exception>()
                          .CircuitBreakerAsync(3, TimeSpan.FromMinutes(45));

            var retry = Policy
                            .Handle<Exception>()
                            .WaitAndRetryAsync(
                                retryCount: 3,
                                sleepDurationProvider: (count) =>
                                {
                                    return TimeSpan.FromSeconds(count);
                                }
                            );

            return Policy.WrapAsync(circuitBreaker, retry);
        }
    }
}
