using Grpc.Core;

namespace CodingMilitia.GrpcExtensions.Hosting.Internal
{
    internal class TypedServerContainer<TService>
    {
        public Server Server { get; }

        public TypedServerContainer(Server server)
        {
            Server = server;
        }        
    }
}