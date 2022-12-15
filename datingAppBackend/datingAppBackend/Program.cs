using datingAppBackend.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<datingContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnections"));
    
    //opt.UseNpgsql(builder.Configuration.GetConnectionString("postgress"));
});
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

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler(exceptionHandler =>
    {
        exceptionHandler.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = Text.Plain;
            await context.Response.WriteAsync("An exception was thrown.");
            var exeptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            await context.Response.WriteAsync(exeptionHandlerPathFeature.Error.Message);
            if (exeptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                await context.Response.WriteAsync("No se ha encontrado este archivo");
            }

            if (exeptionHandlerPathFeature.Path == "/")
            {
                await context.Response.WriteAsync(" Page: Home");
            }
    } );
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
