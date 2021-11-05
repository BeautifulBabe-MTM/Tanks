using System;
using System.Threading.Tasks;
using TanksLib;

namespace Server_Tank
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(8000, "127.0.0.1");
            server.Start();
            Task task = new Task(() => server.Conection());
            task.Start();
            while (true)
            {
                if (server.clients.Count > 0)
                    Console.WriteLine(server.GetMsg(server.clients[0]));
            }
        }
    }
}