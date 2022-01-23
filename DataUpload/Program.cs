using DataUpload.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DataUpload
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateDefaultBuilder().Build();
            
            //Set up DI for the service
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            var uploadPropertyInstance = provider.GetRequiredService<UploadPropertyService>();

            await uploadPropertyInstance.UploadProperties();

            host.Run();

            Console.WriteLine("Done.......");
        }

        /// <summary>
        /// Set up configuration for appsettings and DI
        /// </summary>
        /// <returns></returns>
        static IHostBuilder CreateDefaultBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<UploadPropertyService>();
                });
        }
    }
}