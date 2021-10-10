using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using (var connection = factory.CreateConnection()) //Establish connection to Local RabbitMQ node
            {
                using (var channel = connection.CreateModel()) //Create channel
                {
                    //To send, need to declare a queue, then message can be published to the queue
                    channel.QueueDeclare(queue: "hello",    //this will create a queue, if not present
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    string message = "My first message on Queue Hello";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                        routingKey: "hello",
                        basicProperties: null,
                        body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();

        }
    }
}
