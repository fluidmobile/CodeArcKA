using System;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventSender
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventObject = new JObject();
            foreach(var arg in args)
            {
                var keyValue = arg.Split('=');
                eventObject.Add(keyValue[0], keyValue[1]);
            }
            var json = JsonConvert.SerializeObject(eventObject);

            Console.WriteLine(json);

            var client = new UdpClient();
            client.Connect("192.168.2.255", 22022);

            var data = Encoding.UTF8.GetBytes(json);
            client.Send(data, data.Length);
        }
    }
}
