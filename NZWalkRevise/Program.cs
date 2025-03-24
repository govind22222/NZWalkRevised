using Microsoft.EntityFrameworkCore;
using NZWalkRevise.Automapper;
using NZWalkRevise.Database;
using NZWalkRevise.Repositories.Interface;
using NZWalkRevise.Repositories.ServiceClass;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Database service added by Raghvendra
builder.Services.AddDbContext<NZWalkDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionStr")));

builder.Services.AddScoped<IRegion, RegionService>();
//-----Added By Raghvendra to use Automapper
builder.Services.AddAutoMapper(typeof(AutomapperClass));
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
