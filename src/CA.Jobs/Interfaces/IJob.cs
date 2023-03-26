using CA.Core.Valores;

namespace CA.Jobs.Interfaces
{
    public interface IJob<TEntidade> : IJobChannel { }

    public interface IJobChannel
    {
        Task ExecutarAsync();
    }
}