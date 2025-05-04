using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Identity;
using Domain.Models.Orders;
using Services.Specifications;
using Services_Abstraction;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService(
        IMapper mapper ,
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork
        ) : IOrderService
    {

        public async Task<IEnumerable<OrderResultDto>> GetAllOrdersByUserEmailAsync(string email)
        {
            var spec = new OrderSpecifications(email);
            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
            if (orders is null) throw new OrderNotFoundException(Guid.Empty);
            var result = mapper.Map<IEnumerable<OrderResultDto>>(orders);
            return result;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecifications(id);

            var order = await unitOfWork.GetRepository<Order, Guid>().GetAsync(id);
            if (order is null) throw new OrderNotFoundException(id);
            var result = mapper.Map<OrderResultDto>(order);
            return result;
        }

        public async Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest, string userEmail)
        {
            // 1. Address
            var address = mapper.Map<Domain.Models.Orders.Address>(orderRequest.ShipToAddress);

            // 2. Order Items => Basket
            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId);
            if (basket is null) throw new BasketNotFoundException(orderRequest.BasketId);

            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>().GetAsync(item.Id);
                if(product is null) throw new ProductNotFoundExceptions(item.Id);
                var orderItem = new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl), item.Quantity, product.Price);
                orderItems.Add(orderItem);
            }


            // 3. Get Delivery Method
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(orderRequest.DeliveryMethodId);
            if (deliveryMethod is null) throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);

            var SubTotal = orderItems.Sum(x => x.Price * x.Quantity);


            // TODO --> PaymentIntentId

            var Order = new Order(userEmail, address, orderItems, deliveryMethod , SubTotal , "");

            await unitOfWork.GetRepository<Order, Guid>().AddAsync(Order);
            var count = await unitOfWork.SaveChangesAsync();
            if (count <= 0) throw new Exception("Failed to create order");
            return mapper.Map<OrderResultDto>(Order);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
            return result;
        }
    }
}
