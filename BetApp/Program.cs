using BetApp.Application.Interfaces;
using BetApp.Application.Services;
using BetApp.Infrastructure.Extensions;
using BetApp.Infrastructure.Persistence;
using BetApp.Infrastructure.Persistence.Mappings;
using BetApp.Infrastructure.Repositories;
using BetApp.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BetApp.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// register repositories
builder.Services.AddInfrastructure(builder.Configuration);

//register services
builder.Services.AddApplication();


// add controllers, swagger etc.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c => {
        c.SwaggerDoc
                   (
                       "v1",
                       new Microsoft.OpenApi.Models.OpenApiInfo
                       {
                           Title = "BetApp API",
                           Version = "v1"
                       }
                   );
        //c.AddServer(new Microsoft.OpenApi.Models.OpenApiServer { Url = "https://localhost:7126/" });
    });

var app = builder.Build();

// apply migrations and seed DB on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<AppDbContext>();
        // create database and tables
        await db.Database.EnsureCreatedAsync();
        // seed data async
        if (!await db.Wallets.AnyAsync())
        {
            SeedData.EnsureSeedDataAsync(db).GetAwaiter().GetResult();
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        //var logger = services.GetRequiredService<ILogger>();
        //logger.LogError(ex, "Error while migrating or seeding the database."); //fix this 
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "BetApp API v1"); });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
