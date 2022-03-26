using Microsoft.EntityFrameworkCore;
using ReactShope.DTOs;
using ReactShope.Entity.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactShope.Extension
{
    public static class OrderExtension
    {

        public static IQueryable<OrderDto> ProjectOrderToOrderDto(this IQueryable<Order> query)
        {

            return query
                    .Select(order => new OrderDto
                    {
                        Id = order.Id,
                        BuyerId = order.BuyerId,
                        OrderDate = order.OrderDate,
                        DeliveryFee = order.DeliveryFee,
                        ShippingAddress = order.ShippingAddress,
                        Subtotal = order.Subtotal,
                        OrderStatus = order.OrderStatus.ToString(),
                        Total = order.GetTotal(),
                        OrderItems = order.OrderItems.Select(i => new OrderItemDto
                        {
                            ProductId = i.ItemOrderd.ProductId,
                            Name = i.ItemOrderd.Name,
                            PictureUrl = i.ItemOrderd.PictureUrl,
                            Price = i.Price,
                            Quantity = i.Quantity
                        }).ToList()
                    }).AsNoTracking();
        }
    }
}
