using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chatcraft.Common
{
    public static class StaticUtils
    {
        public static ILoggerFactory LoggerFactory;

        public static ILogger Logger;
        static StaticUtils()
        {
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            LoggerFactory.AddConsole();
            Logger= StaticUtils.LoggerFactory.CreateLogger<Player>();
        }
         
    }
}
