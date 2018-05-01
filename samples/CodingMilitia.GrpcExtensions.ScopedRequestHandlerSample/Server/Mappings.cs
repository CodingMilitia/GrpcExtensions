namespace CodingMilitia.GrpcExtensions.ScopedRequestHandlerSample.Server
{
    internal static class Mappings
    {
        public static Messages.SampleRequest ToInternalRequest(this Generated.SampleRequest request)
        {
            return new Messages.SampleRequest
            {
                Value = request.Value
            };
        }

        public static Generated.SampleResponse ToExternalResponse(this Messages.SampleResponse response)
        {
            return new Generated.SampleResponse
            {
                Value = response.Value
            };
        }
    }
}