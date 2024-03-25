using MailService.Infrastructure.RabbitMq;
using MailService.Infrastructure.Smtp;
using MailService.Services.Emails;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MailQueueSettings>(builder.Configuration.GetSection("RabbitMq:Mails"));
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

builder.Services.AddSingleton<ISmtpClient, MailKitClient>();
builder.Services.AddSingleton<IMailService, MailService.Services.Emails.MailService>();
builder.Services.AddHostedService<MailQueueConsumer>();

var app = builder.Build();


app.Run();
