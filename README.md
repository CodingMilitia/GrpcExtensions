# GrpcExtensions
Some extensions to ease development of gRPC services in .NET

Trying to simplify dependency injection configuration and stuff like that, without going as crazy as in the other project ([this one here](https://github.com/CodingMilitia/Grpc)).


## Build status

||master|develop|
|---|---|---|
|AppVeyor|[![Build status](https://ci.appveyor.com/api/projects/status/x6h46pdlok12duwk/branch/master?svg=true)](https://ci.appveyor.com/project/joaofbantunes/grpcextensions)|[![Build status](https://ci.appveyor.com/api/projects/status/x6h46pdlok12duwk/branch/develop?svg=true)](https://ci.appveyor.com/project/joaofbantunes/grpcextensions)|
|Travis CI|[![Build Status](https://travis-ci.org/CodingMilitia/GrpcExtensions.svg?branch=master)](https://travis-ci.org/CodingMilitia/GrpcExtensions)|[![Build Status](https://travis-ci.org/CodingMilitia/GrpcExtensions.svg?branch=develop)](https://travis-ci.org/CodingMilitia/GrpcExtensions)|

## Coverage status

|master|develop|
|---|---|
|[![Coverage Status](https://coveralls.io/repos/github/CodingMilitia/GrpcExtensions/badge.svg?branch=master)](https://coveralls.io/github/CodingMilitia/GrpcExtensions?branch=develop)|[![Coverage Status](https://coveralls.io/repos/github/CodingMilitia/GrpcExtensions/badge.svg?branch=master)](https://coveralls.io/github/CodingMilitia/GrpcExtensions?branch=develop)|



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
