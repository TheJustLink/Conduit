using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;

using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace Conduit.Net.Benchmark
{
    static class Program
    {
        private static void Main()
        {
            Initialize();
            
            BenchmarkRunner.Run<TestBenchmarks>();

            Console.ReadKey(true);
        }
        private static void Initialize()
        {
            var process = Process.GetCurrentProcess();
            process.PriorityBoostEnabled = true;
            process.PriorityClass = ProcessPriorityClass.High;

            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }

        private static void RunBenchmarks()
        {
            BenchmarkRunner.Run(typeof(Program).Assembly);
        }
        private static void RunBenchmarksInShortFormat()
        {
            var config = new ManualConfig();
            config.AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
            config.AddExporter(DefaultConfig.Instance.GetExporters().ToArray());
            config.AddDiagnoser(DefaultConfig.Instance.GetDiagnosers().ToArray());
            config.AddAnalyser(DefaultConfig.Instance.GetAnalysers().ToArray());
            config.AddJob(DefaultConfig.Instance.GetJobs().ToArray());
            config.AddValidator(DefaultConfig.Instance.GetValidators().ToArray());
            config.UnionRule = ConfigUnionRule.AlwaysUseGlobal;

            var summaries = BenchmarkRunner.Run(typeof(Program).Assembly, config);

            var logger = ConsoleLogger.Default;
            foreach (var summary in summaries)
            {
                MarkdownExporter.Console.ExportToLog(summary, logger);
                var conclusions = config.GetAnalysers().SelectMany(analyser => analyser.Analyse(summary)).ToList();
                ConclusionHelper.Print(logger, conclusions);
            }
        }
    }
}