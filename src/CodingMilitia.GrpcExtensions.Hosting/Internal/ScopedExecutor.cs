using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CodingMilitia.GrpcExtensions.Hosting.Internal
{
    internal class ScopedExecutor : IScopedExecutor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ScopedExecutor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(paramName: nameof(scopeFactory));
        }

        public void Execute<TService>(Action<TService> handler)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                handler(service);
            }
        }

        public TResult Execute<TService, TResult>(Func<TService, TResult> handler)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                return handler(service);
            }
        }

        public async Task ExecuteAsync<TService>(Func<TService, Task> handler)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                await handler(service).ConfigureAwait(false);
            }
        }

        public async Task<TResult> ExecuteAsync<TService, TResult>(Func<TService, Task<TResult>> handler)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                return await handler(service).ConfigureAwait(false);
            }
        }
    }
}