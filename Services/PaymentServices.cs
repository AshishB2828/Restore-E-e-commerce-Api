using Microsoft.Extensions.Configuration;
using ReactShope.Entity;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactShope.Services
{
    public class PaymentServices
    {
        private readonly IConfiguration config;

        public PaymentServices(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<PaymentIntent> createOrUpdatePaymentIntent(Basket basket)
        {

            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];

            var service = new PaymentIntentService();

            var intent = new PaymentIntent();

            var subTotal = basket.Items.Sum(i => i.Quantity * i.Product.Price);

            var deliveryFee = subTotal > 10000 ? 0 : 500;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = subTotal + deliveryFee,
                    Currency = "INR",
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    }

                };
                intent = await service.CreateAsync(options);
                
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = subTotal + deliveryFee,

                };

                await service.UpdateAsync(basket.PaymentIntentId ,options);
            }

            return intent;
        }
    }
}
