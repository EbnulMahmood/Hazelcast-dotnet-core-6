using Cache.Extensions;
using Cache.Services;
using Hazelcast;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var hazelcastOptions = builder.Configuration.GetSection("hazelcast").Get<HazelcastOptions>();

builder.Services.AddCacheService(hazelcastOptions);

builder.Services.AddSingleton<IHazelcastService<string, int>, HazelcastService<string, int>>(service =>
    new HazelcastService<string, int>(hazelcastOptions, "login_attempts")
);

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
