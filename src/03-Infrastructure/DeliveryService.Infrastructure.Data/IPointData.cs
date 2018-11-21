using System.Collections.Generic;
using DeliveryService.Domain.Model;

namespace DeliveryService.Infrastructure.Data
{
    public interface IPointData
    {
        Point GetPointByCode(string code);

        void Save(Point point);

        void Update(Point point);

        void Delete(string code);

        IEnumerable<string> GetAllPointCode();
    }
}
