using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using DeliveryService.Performance.Tests.Controllers;

namespace DeliveryService.Performance.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigurationHelper.GetInstance(args);
            Summary summary = null;
            summary = BenchmarkRunner.Run(typeof(PointsControllerTests));
            summary = BenchmarkRunner.Run(typeof(RoutesControllerTests));
            summary = BenchmarkRunner.Run(typeof(DeliveryControllerTests));
            
        }
    }
}
