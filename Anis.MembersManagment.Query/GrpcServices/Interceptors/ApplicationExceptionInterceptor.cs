﻿using Anis.MembersManagment.Query.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Anis.MembersManagment.Query.GrpcServices.Interceptors
{
    public class ApplicationExceptionInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
			try
			{
				return await continuation(request, context);
			}
			catch (NotFoundException exception)
			{
				throw new RpcException(new Status(StatusCode.NotFound,exception.Message));
			}
        }
    }
}
