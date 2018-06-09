# GrpcExtensions
Some extensions to ease development of gRPC services in .NET

Trying to simplify dependency injection configuration and stuff like that, without going as crazy as in the other project ([this one here](https://github.com/CodingMilitia/Grpc)).


## Build status

||master|develop|
|---|---|---|
|AppVeyor|[![Build status](https://ci.appveyor.com/api/projects/status/x6h46pdlok12duwk/branch/master?svg=true)](https://ci.appveyor.com/project/joaofbantunes/grpcextensions)|[![Build status](https://ci.appveyor.com/api/projects/status/x6h46pdlok12duwk/branch/develop?svg=true)](https://ci.appveyor.com/project/joaofbantunes/grpcextensions)|

## Sample
Just a quick sample, for a bit more detail check the samples folder.
~~~~
class Program
{
    static async Task Main(string[] args)
    {
        var serverHostBuilder = new HostBuilder()
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            //...
        })
        .ConfigureLogging((context, logging) =>
        {
            //...
        })
        .ConfigureServices((hostContext, services) =>
        {
            services
            .AddScoped<ISampleServiceLogic, RandomSampleServiceLogic>()
            .AddGrpcServer<SampleServiceImplementation>(
                new[] { 
                    new ServerPort("127.0.0.1", 5050, ServerCredentials.Insecure) 
                }
            );
        });

        await serverHostBuilder.RunConsoleAsync();
    }
}
~~~~
