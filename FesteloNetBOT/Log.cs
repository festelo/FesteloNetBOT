using Microsoft.Extensions.Logging;
using System.IO;

namespace FesteloNetBOT
{
    static class Log
    {
        public static ILoggerFactory LoggerFactory { get; } = new LoggerFactory()
            .AddFile("Logs/{Date}.log");
        public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
    }
}
