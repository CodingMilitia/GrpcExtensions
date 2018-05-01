using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.GrpcExtensions.ScopedRequestHandlerSample.Server
{
    public interface ISampleServiceLogic
    {
         Task<Messages.SampleResponse> SendAsync(Messages.SampleRequest request, CancellationToken ct);
    }
}