using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Azure.ServiceBus;

namespace Serialization
{
    class Program
    {
        static void Main(string[] args)
        {
            var order = new Order() { Id = 1 };

            var serviceBusMessage = InitMessage(order);

            // Do something with it.

        }

        private static Message InitMessage<T>(T message)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            var xml = "";
            using(var stringWriter = new StringWriter())
            {
                using(XmlWriter writer = XmlWriter.Create(stringWriter))
                {
                    serializer.Serialize(writer, message);
                    xml = stringWriter.ToString(); // Your XML
                }
            }

            var base64Encode = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
            var xmlInBytesInBase64 = Encoding.UTF8.GetBytes(base64Encode);
            return new Message(xmlInBytesInBase64);
        }
    }

    public class Order 
    {
        public int Id { get; set; }
    }
}
