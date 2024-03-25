using MailService.Infrastructure.RabbitMq;
using MailService.Infrastructure.Smtp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MailQueueSettings>(builder.Configuration.GetSection("RabbitMq:Mails"));
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

builder.Services.AddSingleton<ISmtpClient, MailKitClient>();
builder.Services.AddHostedService<MailQueueConsumer>();

var app = builder.Build();


app.Run();
