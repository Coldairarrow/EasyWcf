using Coldairarrow.Util.Wcf;
using System;
using WcfServer;

namespace WcfClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = WcfClientFactory.CreateClientByUrl<IMyService>("http://localhost:14725/MyService");
            var data = client.Hello();
            Console.WriteLine(data);

            Console.ReadKey();
        }
    }
}
