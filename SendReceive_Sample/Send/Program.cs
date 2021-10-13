using System;
using RabbitMQ.Client;
using System.Text;
using EmployeeModel;

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

            Employee emp1 = new Employee() { Name = "Rachit", Designation = "Sr. Software Engineer", City = "Rochester, MN", Salary = 1000 };
            Employee emp2 = new Employee() { Name = "Mike", Designation = "Advance Software Engineer", City = "New York", Salary = 2000 };
            Employee emp3 = new Employee() { Name = "Peter", Designation = "Software Engineer", City = "Los Angeles", Salary = 3000 };
            Employee emp4 = new Employee() { Name = "Ronald", Designation = "Principal Software Engineer", City = "Boston", Salary = 4000 };
            Employee emp5 = new Employee() { Name = "Lynda", Designation = "Sr. QA Engineer", City = "Wilmington", Salary = 5000 };
            Employee emp6 = new Employee() { Name = "Jack", Designation = "Manager", City = "Austin", Salary = 6000 };
            SendMessage(factory, emp1);
            SendMessage(factory, emp2);
            SendMessage(factory, emp3);
            SendMessage(factory, emp4);
            SendMessage(factory, emp5);
            SendMessage(factory, emp6);

            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();
        }

        private static void SendMessage(ConnectionFactory cf, Employee e)
        {
            using (var connection = cf.CreateConnection()) //Establish connection to Local RabbitMQ node
            {
                using (var channel = connection.CreateModel()) //Create channel
                {
                    //To send, need to declare a queue, then message can be published to the queue
                    channel.QueueDeclare(queue: "empQueue",    //this will create a queue, if not present
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    //string message = "My first message on Queue Hello";
                    var body = Encoding.UTF8.GetBytes(e.Serialize());

                    channel.BasicPublish(exchange: "",
                        routingKey: "empQueue",
                        basicProperties: null,
                        body: body);

                    Console.WriteLine(" [x] Sent {0}", e.Name);
                }
            }

            
        }
    }
}
