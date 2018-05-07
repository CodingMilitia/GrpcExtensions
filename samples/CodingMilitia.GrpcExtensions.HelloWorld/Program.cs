using System;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.GrpcExtensions.HelloWorld.Generated;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static CodingMilitia.GrpcExtensions.HelloWorld.Generated.SampleService;

namespace CodingMilitia.GrpcExtensions.HelloWorld
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var serverHostBuilder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddGrpcServer<SampleServiceImplementation>(
                        new []{ new ServerPort("127.0.0.1", 5000, ServerCredentials.Insecure)}
                    );
                });

            var cts = new CancellationTokenSource();
            
            var t = Task.Run(async () =>
            {
                Channel channel = null;
                try
                {
                    await Task.Delay(1000);
                    channel = new Channel("127.0.0.1:5000", ChannelCredentials.Insecure);
                    var clientServices = new ServiceCollection()
                        .AddSingleton(new SampleServiceClient(channel))
                        .BuildServiceProvider();
                    var client = clientServices.GetRequiredService<SampleServiceClient>();
                    var request = new SampleRequest { Value = 1 };
                    var response = await client.SendAsync(request);
                    Console.WriteLine("{0} -> {1}", request.Value, response.Value);
                    cts.Cancel();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    if (channel != null)
                    {
                        await channel.ShutdownAsync();
                    }
                }
            });
            
            await serverHostBuilder.RunConsoleAsync(cts.Token);
            await t;
        }
    }

    public class SampleServiceImplementation : SampleServiceBase
    {
        public override Task<Generated.SampleResponse> Send(Generated.SampleRequest request, ServerCallContext context)
        {
            return Task.FromResult(new Generated.SampleResponse { Value = request.Value + 1 });
        }
    }
}