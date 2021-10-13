using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace WorkerQueue
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

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "empQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );
                    channel.BasicQos(0, 1, false);

                    var consumer = new EventingBasicConsumer(channel);
                    Console.WriteLine(" Message Count {0}", channel.MessageCount("empQueue").ToString());
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        
                        var message = Encoding.UTF8.GetString(body);
                        channel.BasicAck(ea.DeliveryTag, false);    //Acknowledging the message
                        Console.WriteLine(" [x] Received {0}", message);
                    };

                    channel.BasicConsume(queue: "empQueue",
                        autoAck: false,
                        consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();

                }
            }
        }

        
    }
}
