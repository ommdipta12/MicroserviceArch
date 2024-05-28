using City.Helper;
using City.Services;
using Microsoft.EntityFrameworkCore;
using State.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MainDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DbCon"));
});
builder.Services.AddConsulConfig(builder.Configuration);
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IConsulHttpClient, ConsulHttpClient>();
builder.Services.AddSingleton<IRabbitMQPublisherService, RabbitMQPublisherService>();
builder.Services.AddHostedService<RabbitMQConsumeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
