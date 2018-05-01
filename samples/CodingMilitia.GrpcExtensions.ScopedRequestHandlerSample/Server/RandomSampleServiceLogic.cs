using System;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.GrpcExtensions.ScopedRequestHandlerSample.Server.Messages;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.GrpcExtensions.ScopedRequestHandlerSample.Server
{
    public class RandomSampleServiceLogic : ISampleServiceLogic
    {
        private static readonly Random RandomSource = new Random();
        private readonly ILogger<RandomSampleServiceLogic> _logger;

        public RandomSampleServiceLogic(ILogger<RandomSampleServiceLogic> logger)
        {
            _logger = logger;
        }
        public async Task<SampleResponse> SendAsync(SampleRequest request, CancellationToken ct)
        {
            _logger.LogInformation("Received request with value {requestValue}", request.Value);
            
            _logger.LogInformation("Simulating slow operation with a delay for request value {requestValue}", request.Value);
            await Task.Delay(1000, ct);

            var response = new SampleResponse
            {
                Value = request.Value + RandomSource.Next()
            };

            _logger.LogInformation(
                "Random response to request with value {requestValue} will be {responseValue}",
                request.Value,
                response.Value
            );

            return response;
        }
    }
}