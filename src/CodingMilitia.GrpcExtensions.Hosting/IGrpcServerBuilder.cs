namespace CodingMilitia.GrpcExtensions.Hosting
{
    /// <summary>
    /// Allows for the registration of one or multiple services to be hosted in the same <see cref="Grpc.CoreServer"/>.
    /// </summary>
    public interface IGrpcServerBuilder
    {
        /// <summary>
        /// Adds a service to be registered in a dependency injection container and hosted in a <see cref="Grpc.Core.Server"/>.
        /// </summary>
        /// <typeparam name="TService">The service to host.</typeparam>
        /// <returns>The <see cref="IGrpcServerBuilder"/> instance to which the service was added.</returns>
        IGrpcServerBuilder AddService<TService>() where TService : class;
    }
}
