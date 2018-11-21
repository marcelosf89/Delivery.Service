using System.Collections.Generic;
using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Domain.Model;

namespace DeliveryService.Application.Query
{
    public interface IQuery<TRequest, TResponse>
    {
        TResponse Execute(TRequest request);
    }
}