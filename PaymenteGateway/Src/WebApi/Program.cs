using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using CoherentSolutions.Extensions.Configuration.AnyWhere;
using CoherentSolutions.Extensions.Configuration.AnyWhere.AdapterList;
using CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables;
//using CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile;
//using CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((ctx, config) =>
                 {
                     config.AddEnvironmentVariables();
                     config.AddAnyWhereConfigurationAdapterList()
                     .AddAnyWhereConfiguration();
                 });
    }
}
