using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;
using ServiceBus.Publisher.Api.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Publisher.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IConfiguration _config;
        private readonly MongoClient clientDb;
        public readonly IQueueClient queueClient;

        public OrderService(IConfiguration config)
        {
            _config = config;
            clientDb = new MongoClient(_config["MongoDb"]);
            queueClient = new QueueClient(_config["Azure:serviceBusConn"], _config["Azure:queueName"]);
        }

        public async Task<ServiceResponse> CreateOrder(OrderModel order)
        {
            try
            {

                order.ProductId = Guid.NewGuid().ToString();
                order.OrderId = Guid.NewGuid().ToString();

                var ordersCollection = clientDb.GetDatabase("ServiceBusTest").GetCollection<OrderModel>("Orders");
                ordersCollection.InsertOne(order);

                ///Publish Message on ServiceBus
                var messageBody = JsonConvert.SerializeObject(order);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                await queueClient.SendAsync(message);
                ///Publish Message on ServiceBus

                return new ServiceResponse()
                {
                    Success = true,
                    Message = "Success"
                };

            }catch(Exception ex)
            {
                return new ServiceResponse()
                {
                    Success =  false,
                    Message = ex.Message
                };
            }
        }
    }
}
