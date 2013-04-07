using System;
using Microsoft.AspNet.SignalR;

namespace BasicChat.SelfHost.Hubs
{
    public class Chat : Hub
    {
        public Chat()
        {
            Console.WriteLine("Server: Constructor called.");
        }

        public void Send(string message)
        {
            Console.WriteLine("Server: " + message);
            Clients.All.send(message);
        }
    }
}
