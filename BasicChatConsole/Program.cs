using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace BasicChatConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            Console.WriteLine("Press anter to start client: ");
            Console.ReadLine();
            
            var connection = new HubConnection("http://localhost:8081/");
            IHubProxy chat = connection.CreateHubProxy("Chat");

            chat.On<string>("send", s => Console.WriteLine("Console: " + s));

            await connection.Start();

            Console.WriteLine("Enter new line: ");
            string line = null;
            while ((line = Console.ReadLine()) != null)
            {
                await chat.Invoke("send", line);
                Console.Write("Enter new line: ");
            }
        }
    }
}
