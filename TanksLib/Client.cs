using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TanksLib
{
    public class Client
    {
        public string ipAddr { get; set; }
        public int port { get; set; }
        public Socket socket { get; set; }
        public IPEndPoint iPEndPoint { get; set; }
        public Client(string ipadres, int port)
        {
            this.ipAddr = ipadres;
            this.port = port;
            CreateSocket();
        }
        public Client(string ipadres, int port, Socket socketClient)
        {
            this.ipAddr = ipadres;
            this.port = port;
            this.socket = socketClient;
        }
        public void CreateSocket()
        {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void CreateIPEndPoint()
        {
            this.iPEndPoint = new IPEndPoint(IPAddress.Parse(this.ipAddr), this.port);
        }
        public void Connect()
        {
            this.socket.Connect(this.iPEndPoint);
        }
        public string TakeMSGFromServ()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int bytes = 0;
            byte[] data = new byte[250];
            do
            {
                bytes = this.socket.Receive(data);
                stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (this.socket.Available > 0);
            return (stringBuilder.ToString());
        }
        public void SengMsg(string msg)
        {
            this.socket.Send(Encoding.Unicode.GetBytes(msg));
        }
    }
}