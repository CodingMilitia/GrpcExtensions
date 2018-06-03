using Grpc.Core;

namespace CodingMilitia.GrpcExtensions.Tests
{
    public static class TestHelpers
    {
        public static readonly string Host = "localhost";
        public static readonly int Port = 1234;
        public static readonly string HostAddress = $"{Host}:{Port}";
        public static readonly ServerCredentials Credentials = ServerCredentials.Insecure;
        public static readonly ChannelCredentials ClientCredentials = ChannelCredentials.Insecure;
    }
}
