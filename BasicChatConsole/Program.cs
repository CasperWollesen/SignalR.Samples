using System;
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
            var active = true;

            var host = "http://localhost:8081/"; // "http://localhost:44914/";

            Console.WriteLine("Please enter client name: ");
            var clientName = Console.ReadLine();

            var thread = new System.Threading.Thread(() =>
                                                         {
                                                             var connectionOn = new HubConnection(host);
                                                             IHubProxy chatOn = connectionOn.CreateHubProxy("Chat");

                                                             chatOn.On<string>("send", s => Console.WriteLine("Console: " + s));

                                                             connectionOn.Start();

                                                             while (active)
                                                             {
                                                                 System.Threading.Thread.Sleep(10);
                                                             }
                                                         }) { IsBackground = true };
            thread.Start();

            var connection = new HubConnection(host);
            IHubProxy chat = connection.CreateHubProxy("Chat");
            await connection.Start();

            Console.Write("Enter new line: ");
            string line = null;
            while ((line = Console.ReadLine()) != null)
            {
                await chat.Invoke("send", clientName + ": " + line);
                Console.Write("Enter new line: ");
            }

            active = false;
            thread.Join();
        }
    }
}
