using System;
using System.Threading.Tasks;

namespace CodingMilitia.GrpcExtensions.Hosting
{
    public interface IScopedExecutor<TService>
    {
        void Execute(Action<TService> handler);

        TResult Execute<TResult>(Func<TService, TResult> handler);

        Task ExecuteAsync(Func<TService, Task> handler);

        Task<TResult> ExecuteAsync<TResult>(Func<TService, Task<TResult>> handler);
    }
}