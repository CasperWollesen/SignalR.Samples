using System;
using Microsoft.Owin.Hosting;

namespace BasicChat.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8081";

            using (WebApplication.Start<Startup>(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
                
                //var c = true;
                //while (c)
                //{
                //    System.Threading.Thread.Sleep(10);    
                //}
            }
        }
    }
}
