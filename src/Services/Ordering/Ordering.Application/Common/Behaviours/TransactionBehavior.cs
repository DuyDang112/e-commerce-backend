using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Ordering.Application.Common.Behaviours
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public TransactionBehavior(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var response = await next();
                    await _orderRepository.SaveChangesAsync();
                    transaction.Complete();
                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw;
                }
            }
        }
    }

}
