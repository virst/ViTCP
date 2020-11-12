using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViTCP;

namespace TestClient
{
    class Program
    {
        const int MyPort = 8888;
        const string szIPstr = "localhost";
        static void Main(string[] args)
        {           

            var client = new Client();
            client.OnDisconnected += OnDisconnect;
            client.OnReceiveData += OnDataReceive;
            IPAddress ipAdd = IPAddress.Parse(Utils.GetAddress(szIPstr));
            client.Connect(ipAdd, MyPort);
            Console.WriteLine("Connected...");

            while(true)
            {
                string s = Console.ReadLine();
                var bb = Encoding.UTF8.GetBytes(s);
                client.SendMessage(bb);
            }

        }

        private static void OnDataReceive(byte[] message, int messageSize)
        {
            string converted = Encoding.UTF8.GetString(message, 0, messageSize);
            Console.WriteLine(converted);
        }

        private static void OnDisconnect()
        {
            Console.WriteLine("Disconnect");
        }
    }
}
