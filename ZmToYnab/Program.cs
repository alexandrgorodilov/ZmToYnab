using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZmToYnab.Services;
using ZmToYnab.Models;
using System;
using System.IO;
using Newtonsoft.Json;

namespace ZmToYnab
{
    class Program
    {
        private static IConfiguration _config;
        static void Main(string[] args)
        {            
            // Create service collection and configure our services
            var services = ConfigureServices();
            // Generate a provider
            var serviceProvider = services.BuildServiceProvider();
            Helper.ClientUnixTimeStamp = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            DeserializeTimeStamp();
            var console = serviceProvider.GetService<ConsoleApp>();
            console.Run();
            SerializeTimeStamp();
        }

        private static IServiceCollection ConfigureServices()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(_config);
            services.AddSingleton<ConsoleApp>();
            services.AddSingleton<IZenMoneyService, ZenMoneyService>();
            services.AddSingleton<IYnabService, YnabService>();
            return services;
        }

        public static void SerializeTimeStamp()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter("time.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, Helper.ClientUnixTimeStamp);
            }
        }

        public static void DeserializeTimeStamp()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sw = new StreamReader("time.json"))
            using (JsonTextReader writer = new JsonTextReader(sw))
            {
                 Helper.LastClientUnixTimeStamp = Convert.ToInt32(serializer.Deserialize(writer));
            }
        }
    }
}
