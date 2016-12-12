using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ChameleonForms.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .Build();

            host.Run();
        }
    }
}
