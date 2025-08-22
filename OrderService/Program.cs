using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderService.Application.Interfaces;
using OrderService.Domain.Interfaces;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Messaging;
using OrderService.Infrastructure.Repositories;
using OrderService.Infrastructure.ServiceClients;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddMediatR(config =>
                            {
                                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                            });
builder.Services.AddSingleton<IEventPublisher, RabbitMqPublisher>();
builder.Services.AddHostedService<RabbitMqConsumer>();
builder.Services.AddControllers().AddJsonOptions(options =>
                                                 {
                                                     options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                                                 });
builder.Services.AddDbContext<OrderDbContext>(options =>
                                              {
                                                  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
                                              });
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICartClient, HttpCartClient>();
builder.Services.AddScoped<IProductClient, HttpProductClient>();
builder.Services.AddHttpClient<IProductClient, HttpProductClient>(client =>
                                                                  {
                                                                      client.BaseAddress = new Uri(builder.Configuration["Services:ProductService"]);
                                                                  });
builder.Services.AddHttpClient<ICartClient, HttpCartClient>(client =>
                                                            {
                                                                client.BaseAddress = new Uri(builder.Configuration["Services:CartService"]);
                                                            });
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
                        {
                            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
                        });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
                               {
                                   options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                                                                           {
                                                                               Name = "Authorization",
                                                                               Type = SecuritySchemeType.Http,
                                                                               Scheme = "Bearer",
                                                                               BearerFormat = "JWT",
                                                                               In = ParameterLocation.Header,
                                                                               Description = "JWT Authorization header using the Bearer scheme."
                                                                           });

                                   options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                                                                  {
                                                                      {
                                                                          new OpenApiSecurityScheme()
                                                                          {
                                                                              Reference = new OpenApiReference()
                                                                                          {
                                                                                              Type = ReferenceType.SecurityScheme,
                                                                                              Id = "Bearer"
                                                                                          }
                                                                          },
                                                                          new string[] { }
                                                                      }
                                                                  });
                               });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                                                                                        {
                                                                                            options.TokenValidationParameters = new TokenValidationParameters
                                                                                                {
                                                                                                    ValidateIssuer = true,
                                                                                                    ValidateAudience = false,
                                                                                                    ValidateLifetime = true,
                                                                                                    ValidateIssuerSigningKey = true,
                                                                                                    IssuerSigningKey =
                                                                                                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                                                                                                    ValidIssuer = "Mehrdad",
                                                                                                    ClockSkew = TimeSpan.Zero
                                                                                                };
                                                                                        });
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<OrderDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();