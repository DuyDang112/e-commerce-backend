using MassTransit;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Enums.Inventory;
using Shared.Enums.Payment;
using Shared.Enums.Shipment;
using System.Text;

namespace Infrastructures.Sagas
{
    public class OrderSagaState : SagaStateMachineInstance, ISagaVersion 
    {
        [BsonId]
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public int Version { get; set; }
        public string UserName { get; set; }
        public string PaymentCardNumber { get; set; }
        public double TotalAmount { get; set; }

        public string FaultReason { get; set; }
        public string ShipmentAddress { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime? SubmitDate { get; set; }
        public DateTime? Updated { get; set; }

        public List<OrdersItem> Items { get; set; } = new List<OrdersItem> { };

        public class OrdersItem
        {
            public string ProductNo { get; set; }
            public int Quantity { get; set; }
        }

    }
    
    
}
