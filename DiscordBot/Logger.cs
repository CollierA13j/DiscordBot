using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    internal class Logger
    {
        // Logs messages to bot console
        internal static Task Log(LogMessage logMessage)
        {
            Console.ForegroundColor = SeverityToConsoleColor(logMessage.Severity);
            string message = String.Concat(DateTime.Now.ToShortDateString(), " [", logMessage.Source,
                                           "] ", logMessage.Message);
            Console.WriteLine(message);
            Console.ResetColor();
            return Task.CompletedTask;
        }

        // Selects color for console text based on context/severity
        private static ConsoleColor SeverityToConsoleColor(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical:
                    return ConsoleColor.Red;
                case LogSeverity.Error:
                    return ConsoleColor.Yellow;
                case LogSeverity.Verbose:
                    return ConsoleColor.Green;
                case LogSeverity.Warning:
                    return ConsoleColor.Magenta;
                case LogSeverity.Debug:
                case LogSeverity.Info:
                    return ConsoleColor.Blue;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}
