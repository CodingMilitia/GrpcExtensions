using CodingMilitia.GrpcExtensions.Tests.Generated;
using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.GrpcExtensions.Tests.Service
{
    public interface IService
    {
        Task<SampleResponse> SendAsync(SampleRequest request, CancellationToken ct);
    }
}
