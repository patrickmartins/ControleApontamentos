using CA.Core.Valores;

namespace CA.Jobs.Channel.Interfaces
{
    public interface IJobChannel<TEntidade> : IJobChannel { }

    public interface IJobChannel
    {
        Task ExecutarAsync();
    }
}