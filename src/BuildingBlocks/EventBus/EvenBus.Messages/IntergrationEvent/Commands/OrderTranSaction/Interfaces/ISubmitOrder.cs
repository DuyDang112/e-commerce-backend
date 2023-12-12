using Contract.Common.Events;
using MassTransit;
using Shared.Enums.Payment;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces
{
    public interface ISubmitOrder : IIntergrationBaseEvent
    {
        DateTime Timestamp { get; }

        string UserName { get; }
        string PaymentCardNumber { get; }
        decimal TotalAmount { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string EmailAddress { get; set; }
        string PhoneNumber { get; set; }
        string ShipmentAddress { get; set; }

        List<SubmitOrder.OrderItem> Items { get; set; }
        //EPaymentMethod PaymentMethod { get; set; }
    }
}
