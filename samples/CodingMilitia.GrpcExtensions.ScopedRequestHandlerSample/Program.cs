using System.Threading.Tasks;
using CodingMilitia.GrpcExtensions.ScopedRequestHandlerSample.Generated;
using CodingMilitia.GrpcExtensions.ScopedRequestHandlerSample.Server;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.GrpcExtensions.ScopedRequestHandlerSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serverHostBuilder = new HostBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                config
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureLogging((context, logging) =>
            {
                logging
                    .AddConfiguration(context.Configuration.GetSection("Logging"))
                    .AddConsole();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services
                .AddScoped<ISampleServiceLogic, RandomSampleServiceLogic>()
                .AddScopedExecutor()
                .AddGrpcServer<AnotherSampleServiceImplementation>(
                    new[] { new ServerPort("127.0.0.1", 5051, ServerCredentials.Insecure) }
                )
                .AddGrpcServer<SampleServiceImplementation>(
                    new[] { new ServerPort("127.0.0.1", 5050, ServerCredentials.Insecure) }
                );
            });

            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
