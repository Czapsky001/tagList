using Microsoft.EntityFrameworkCore;
using TagList.Convert;
using TagList.DatabaseConnector;
using TagList.Repositories.TagRepo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IConvertJson, ConvertJson>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = builder.Configuration;
var connectionString = config["ConnectionString"];
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

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
