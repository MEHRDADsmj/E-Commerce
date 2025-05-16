using System.Reflection;
using System.Text;
using System.Text.Json;
using CartService.Domain.Interfaces;
using CartService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
                                                      {
                                                          var redisPort = builder.Configuration["Redis:Port"];
                                                          var redisHost = builder.Configuration["Redis:Host"];
                                                          var configOptions = new ConfigurationOptions()
                                                                              {
                                                                                  EndPoints = { redisHost, redisPort }
                                                                              };
                                                          return ConnectionMultiplexer.Connect(configOptions);
                                                      });
builder.Services.AddScoped<ICartRepository, RedisCartRepository>();
builder.Services.AddMediatR(config =>
                            {
                                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                            });
builder.Services.AddControllers().AddJsonOptions(options =>
                                                 {
                                                     options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                                                 });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
                                        {
                                            ValidateLifetime = true,
                                            ValidateAudience = false,
                                            ValidateIssuer = true,
                                            ValidIssuer = "Mehrdad",
                                            ValidateIssuerSigningKey = true,
                                            IssuerSigningKey =
                                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                                            ClockSkew = TimeSpan.Zero,
                                        };
});
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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