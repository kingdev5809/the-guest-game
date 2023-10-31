using Microsoft.EntityFrameworkCore;
using TheGame.Data;
using TheGame.Repository;
using TheGame.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GameRepository>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<LiderboardRepository>();



builder.Services.AddDbContext<InMemoryDbContext>(options =>
{
    options.UseInMemoryDatabase("MyInMemoryDatabase");
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(policyName =>
{
    policyName
    .SetIsOriginAllowed(_ => true)
    .AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod();
});
app.UseAuthorization();

app.MapControllers();

app.Run();
