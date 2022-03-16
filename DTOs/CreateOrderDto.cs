using ReactShope.Entity.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactShope.DTOs
{
    public class CreateOrderDto
    {
        public bool SaveAddress { get; set; }

        public ShippingAddress ShippingAddress { get; set; }
    }
}
