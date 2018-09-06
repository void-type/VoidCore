using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;
using VoidCore.AspNet.Logging;

namespace VoidCore.AspNet.Configuration
{
    /// <summary>
    /// Methods for setting up an AspNetCore web server.
    /// </summary>
    public static class WebServer
    {
        /// <summary>
        /// Build a web host using serilog and log all exceptions. Use this in Program.Main().
        /// </summary>
        /// <typeparam name="TStartup">The type containing startup methods for the application.</typeparam>
        public static int BuildAndRun<TStartup>(string[] args) where TStartup : class
        {
            Log.Logger = SerilogFileLoggerFactory.Create<TStartup>(false);

            try
            {
                var host = WebHost
                    .CreateDefaultBuilder(args)
                    .UseStartup<TStartup>()
                    .UseSerilog()
                    .Build();

                Log.Information("Starting web host.");

                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
