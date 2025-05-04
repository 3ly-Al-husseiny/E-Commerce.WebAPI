using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }

        public Order(string userEmail, Address shippingAddress, ICollection<OrderItem> orderItems, DeliveryMethod deliveryMethod, decimal subTotal, string paymentIntentId)
        {
            Id = Guid.NewGuid();
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            OrderItems = orderItems;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        // Id
        // User Email

        public string UserEmail { get; set; }

        // Shipping Address 
        public Address ShippingAddress { get; set; }
        
        // Order Items
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();    // Navigational Property 


        // Delivery Method
        public DeliveryMethod DeliveryMethod { get; set; }

        public int? DeliveryMethodId { get; set; }



        // Payment Status 
        public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;

        // Sub Total --> Total Price of Products. 
        public decimal SubTotal { get; set; }

        // Order Date
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        // Payment
        public string PaymentIntentId { get; set; }
    }
}
