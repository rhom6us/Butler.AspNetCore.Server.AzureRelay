using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Butler.AspNetCore.Server.AzureRelay.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseAzureRelay("https://{your namespace}.servicebus.windows.net/", "RootManageSharedAccessKey", "{your key}")
                .UseContentRoot(Directory.GetCurrentDirectory())
                //.UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
 
            host.Run();
        }
    }
}
