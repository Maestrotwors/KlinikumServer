using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;

namespace MyKlinikumCore
{
    public class Program
    {
        public SqlConnection DBConnection = new SqlConnection("Server=Roman\\SqlExpress;Database=Klinikum;Trusted_Connection=True;");
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
				.UseUrls("http://*:80")
                .Build();

            host.Run();
        }
    }
}
