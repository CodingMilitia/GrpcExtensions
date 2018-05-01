using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.GrpcExtensions.ScopedRequestHandlerClientSample.Generated;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static CodingMilitia.GrpcExtensions.ScopedRequestHandlerClientSample.Generated.SampleService;

namespace CodingMilitia.GrpcExtensions.ScopedRequestHandlerClientSample.Controllers
{
    [Produces("application/json")]
    [Route("api/sample")]
    public class SampleController : Controller
    {
        private readonly SampleServiceClient _client;
        private readonly ILogger<SampleController> _logger;
        public SampleController(SampleServiceClient client, ILogger<SampleController> logger)
        {
            _client = client;
            _logger = logger;
        }

        //to check out -> http://localhost:5000/api/sample/onerequest/5
        [Route("onerequest/{value}")]
        public async Task<IActionResult> MakeOneRequestAsync(int value, CancellationToken ct)
        {
            var response = await _client.SendAsync(new SampleRequest { Value = value }, cancellationToken: ct);
            return Ok(new { value = response.Value });
        }

        //to check out -> http://localhost:5000/api/sample/tenrequests/5
        [Route("tenrequests/{startValue}")]
        public async Task<IActionResult> MakeTenRequestsAsync(int startValue, CancellationToken ct)
        {
            var responses = Enumerable
                .Range(startValue, 10)
                .Select(value => _client.SendAsync(new SampleRequest { Value = value }, cancellationToken: ct))
                .ToList(); //without this, the requests won't go in parallel for obvious reasons =P

            var result = new
            {
                results = (await WhenAllAsync(responses)).Select(response => new { value = response.Value })
            };

            return Ok(result);
        }

        //can't use the usual Task.WhenAll because the gRPC lib doesn't return tasks, uses duck typing...
        private async Task<List<SampleResponse>> WhenAllAsync(List<AsyncUnaryCall<SampleResponse>> responses)
        {
            var results = new List<SampleResponse>(responses.Count);
            foreach (var response in responses)
            {
                results.Add(await response);
            }
            return results;
        }
    }
}