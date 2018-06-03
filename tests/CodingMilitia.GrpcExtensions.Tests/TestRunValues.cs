using Grpc.Core;
using System.Threading;

namespace CodingMilitia.GrpcExtensions.Tests
{
    public class TestRunValues
    {
        private static int CurrentPort = 7999;

        public string Host { get; }
        public int Port { get; }
        public string HostAddress { get; }
        public ServerCredentials ServerCredentials { get; }
        public ChannelCredentials ClientCredentials { get; }

        public TestRunValues()
        {
            Host = "localhost";
            Port = Interlocked.Increment(ref CurrentPort);
            HostAddress = $"{Host}:{Port}";
            ServerCredentials = ServerCredentials.Insecure;
            ClientCredentials = ChannelCredentials.Insecure;
        }
    }
}
