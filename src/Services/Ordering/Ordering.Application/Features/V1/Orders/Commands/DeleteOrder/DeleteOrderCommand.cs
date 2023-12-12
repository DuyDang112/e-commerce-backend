using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders
{
    public class DeleteOrderCommand : IRequest<Unit>
    {
        public long Id { get; set; }

        public DeleteOrderCommand(long id)
        {
            Id = id;
        }
    }
}
