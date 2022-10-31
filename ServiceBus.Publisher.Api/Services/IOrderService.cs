using ServiceBus.Publisher.Api.Models;
using System.Threading.Tasks;

namespace ServiceBus.Publisher.Api.Services
{
    public interface IOrderService
    {
        Task<ServiceResponse> CreateOrder(OrderModel order);
    }
}
