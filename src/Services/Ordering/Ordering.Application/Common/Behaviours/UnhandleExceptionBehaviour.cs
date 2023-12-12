using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Ordering.Application.Common.Behaviours
{
    public class UnhandleExceptionBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {

        private readonly ILogger _logger;

        public UnhandleExceptionBehaviour(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.Error(ex, "Application Request: Unhandle Exception for Request {Name} {@Request}",
                    requestName, request);
                throw;
            }
        }
    }
}
