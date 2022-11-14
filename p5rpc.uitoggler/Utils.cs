using p5rpc.uitoggler.Configuration;
using Reloaded.Mod.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p5rpc.uitoggler
{
    internal static class Utils
    {
        private static ILogger _logger;
        private static Config _config;
        internal static nint BaseAddress { get; private set; }


        internal static void Initialise(ILogger logger, Config config)
        {
            _logger = logger;
            _config = config;
            using var thisProcess = Process.GetCurrentProcess();
            BaseAddress = thisProcess.MainModule!.BaseAddress;
        }

        internal static void LogDebug(string message)
        {
            if (_config.DebugEnabled)
                _logger.WriteLine($"[UI Toggler] {message}");
        }

        internal static void Log(string message)
        {
            _logger.WriteLine($"[UI Toggler] {message}");
        }

        internal static void LogError(string message, Exception e)
        {
            _logger.WriteLine($"[UI Toggler] {message}: {e.Message}", System.Drawing.Color.Red);
        }

        internal static void LogError(string message)
        {
            _logger.WriteLine($"[UI Toggler] {message}", System.Drawing.Color.Red);

        }
    }
}
