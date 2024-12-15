using Grpc.Core;
using Grpc.Core.Interceptors;

namespace UserManagementService.Src.Services
{
    public class ServerCallContextInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var httpContext = context.GetHttpContext();

            foreach (var header in context.RequestHeaders)
            {
                httpContext.Request.Headers[header.Key] = header.Value;
                Console.WriteLine($"Interceptor Metadata: {header.Key} - {header.Value}");
            }

            return await continuation(request, context);
        }
    }
}