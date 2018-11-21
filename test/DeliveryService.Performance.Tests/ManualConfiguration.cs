using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;
using System;
using System.IO;

namespace DeliveryService.Performance.Tests
{
    public class ManualConfiguration : ManualConfig
    {
        private const string PATH_RESULT = "path";

        public ManualConfiguration()
        {
            Add(Job.Default
                .With(Runtime.Core)
                .With(CsProjCoreToolchain.NetCoreApp21)
                .With(RunStrategy.Monitoring));
            ArtifactsPath = GetPath();
            Add(MemoryDiagnoser.Default);
            Add(JsonExporter.Default);
            Add(JsonExporter.Custom("-custom", indentJson: true));
        }

        private string GetPath()
        {
            string pathArgument = ConfigurationHelper.Instance.GetArguments(PATH_RESULT, "performance");

            Console.WriteLine($"Seting the default artifacts path in  : '{pathArgument}'");
            if (!Directory.Exists(pathArgument))
            {
                Directory.CreateDirectory(pathArgument);
                Console.WriteLine($"Default artifacts path '{pathArgument}' Created!");
            }
            return pathArgument;
        }
    }
}
