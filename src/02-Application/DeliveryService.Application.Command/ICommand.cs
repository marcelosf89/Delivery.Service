using DeliveryService.Crosscutting.Request.PointManagement;

namespace DeliveryService.Application.Command
{
    public interface ICommand<T> where T : class
    {
        void Execute(T request);
    }
}