using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;
using UserService.Infrastructure;
using UserService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<UserDbContext>(options =>
                                             {
                                                 options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
                                             });
builder.Services.AddMediatR(config =>
                            {
                                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                            });
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddControllers().AddJsonOptions(options =>
                                                 {
                                                     options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                                                 });
builder.Services.AddEndpointsApiExplorer();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<UserDbContext>();
    context.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();