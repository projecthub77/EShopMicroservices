
using Ordering.Domain.Events;
using Ordering.Domain.ValueObjects;
using System.ComponentModel;
using System.Diagnostics;

namespace Ordering.Domain.Models
{
    public class Order : Aggregate<OrderId>
    {
        public List<OrderItem> _orderItems = new();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
        public CustomerId CustormId { get; private set; } = default!;
        public OrderName OrderName { get; private set; } = default!;  //private set perchè non deveo esser possibile settarla al di fuori dell'aggregato
        public Address ShippingAddress {  get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public decimal TotalPrice
        {
            get => OrderItems.Sum(x => x.Price * x.Quantity);
            private set { }
        }

        public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, OrderStatus orderStatus)
        {
            //ArgumentException.ThrowIfNullOrWhiteSpace(name);
            //ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var order = new Order
            {
                Id = id,
                CustormId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment,
                Status = orderStatus                
            };

            order.AddDomaniEvent(new OrderCreatedEvent(order));

            return order;
        }

        public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, OrderStatus status)
        {
            OrderName = orderName;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Payment = payment;
            Status = status;

            AddDomaniEvent(new OrderUpdatedEvent(this));
        }

        public void Add(ProductId productId, int quantity, decimal price)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var orderItem = new OrderItem(Id, productId, quantity, price);
            _orderItems.Add(orderItem);
        }

        public void Remove(ProductId productId)
        {
            var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
            if (orderItem != null)
            {
                _orderItems.Remove(orderItem);
            }
        }
    }
}


//l'evento del dominio rappresenta qualcosa che è accaduto in passato e le altre parti dello stesso confine di servizio
//dello stesso dominio devono reagire a questi cambiamenti

//l'evento del dominio è un evento aziendale che si verifica all'interno del modello di dominio.
//spesso rappresenta un effetto collaterale del funzionamento del dominio

//raggiungere la coerenza tra aggregatori nello stesso dominio
//cioè quando nello stesso aggregato parte un evento ad es un ordine, l'order item deve essere triggerato(notificato). (o tutte le entità dell'aggregato)