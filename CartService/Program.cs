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
builder.Services.AddControllers();
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