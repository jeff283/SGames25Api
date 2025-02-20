using SGames25Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.DefaultIgnoreCondition
            = JsonIgnoreCondition.WhenWritingDefault;
    });

//To give access to IHttpContextAccessor for Audit Data with IAuditable
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<SummerGamesContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SummerGamesConnection")));




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

//To prepare the database and seed data. 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SummerGamesInitializer.Initialize(serviceProvider: services, DeleteDatabase: true,
        UseMigrations: false, SeedRandomSampleData: false);
}

app.Run();
