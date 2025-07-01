using System.Reflection;
using System.Text.Json;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Interfaces;
using PaymentService.Infrastructure.Messaging;
using PaymentService.Infrastructure.PaymentProcessing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddJsonOptions(options =>
                                                 {
                                                     options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                                                 });
builder.Services.AddMediatR(config =>
                            {
                                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                            });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPaymentProcessor, FakePaymentProcessor>();
builder.Services.AddSingleton<IEventPublisher, RabbitMqPublisher>();
builder.Services.AddHostedService<RabbitMqConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();