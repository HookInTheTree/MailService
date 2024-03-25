
using MailKit.Net.Smtp;
using MailService.Infrastructure.Smtp;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MailService.Infrastructure.RabbitMq
{
    public class MailQueueConsumer : BackgroundService
    {
        private IConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        public MailQueueConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private void Initialize()
        {
            var settings = _serviceProvider.GetService<IOptions<MailQueueSettings>>().Value;
            var mailService = _serviceProvider.GetService<ISmtpClient>();

            var factory = new ConnectionFactory()
            {
                HostName = settings.HostName,
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password,
            };
            
            _connection = factory.CreateConnection();
            
            var model = _connection.CreateModel();

            model.ExchangeDeclare(settings.ExchangeName, "direct", true, false);

            var queue = model.QueueDeclare(settings.QueueName, true, false, false);
            model.QueueBind(queue.QueueName, settings.ExchangeName, queue.QueueName);

            var consumer = new EventingBasicConsumer(model);

            consumer.Received += async (data, args) =>
            {
                try
                {
                    var s = Encoding.UTF8.GetString(args.Body.ToArray());
                    var jData = JObject.Parse(s);

                   var message = new Message(
                        jData["To"].ToObject<List<string>>(),
                        jData["Subject"].ToString(),
                        jData["Body"].ToString(),
                        jData["From"].ToString(),
                        jData["CC"].ToObject<List<string>>(),
                        jData["BCC"].ToObject<List<string>>()
                    );
                 
                    await mailService.SendAsync(message);
                }
                catch (Exception ex)
                {
                    model.BasicAck(args.DeliveryTag, false);
                }
            };

            model.BasicConsume(settings.QueueName, false, consumer);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Initialize();
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            _connection.Dispose();
        }
    }
}
