// <copyright file="Program.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Benchmarks
{
    using System.IO;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Running;
    using global::Benchmarks;

    /// <summary>
    /// Program entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            IConfig config = new DebugInProcessConfig();

            if (args.Length > 0)
            {
                string artifactsPath = args[0];
                Directory.CreateDirectory(artifactsPath);
                config = config.WithArtifactsPath(artifactsPath);
            }

            if (args.Length > 1)
            {
                string version = args[1];
                config.AddJob(Job.Default.WithArguments(new Argument[] { new MsBuildArgument($"/p:Version={version}") }));
            }

            BenchmarkRunner.Run<ManagementApiBenchmarks>(config);
            BenchmarkRunner.Run<ApiDeliveryChannelBenchmarks>(config);
        }
    }
}