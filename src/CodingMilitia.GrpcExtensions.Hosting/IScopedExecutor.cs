using System;
using System.Threading.Tasks;

namespace CodingMilitia.GrpcExtensions.Hosting
{
    public interface IScopedExecutor
    {
        void Execute<TService>(Action<TService> handler);

        TResult Execute<TService, TResult>(Func<TService, TResult> handler);

        Task ExecuteAsync<TService>(Func<TService, Task> handler);

        Task<TResult> ExecuteAsync<TService, TResult>(Func<TService, Task<TResult>> handler);
    }
}