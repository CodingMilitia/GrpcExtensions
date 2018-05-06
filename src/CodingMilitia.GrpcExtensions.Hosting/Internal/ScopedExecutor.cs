using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CodingMilitia.GrpcExtensions.Hosting.Internal
{
    internal class ScopedExecutor<TService> : IScopedExecutor<TService>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ScopedExecutor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(paramName: nameof(scopeFactory));
        }

        public void Execute(Action<TService> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                handler(service);
            }
        }

        public TResult Execute<TResult>(Func<TService, TResult> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                return handler(service);
            }
        }

        public async Task ExecuteAsync(Func<TService, Task> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                await handler(service).ConfigureAwait(false);
            }
        }

        public async Task<TResult> ExecuteAsync<TResult>(Func<TService, Task<TResult>> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                return await handler(service).ConfigureAwait(false);
            }
        }
    }
}