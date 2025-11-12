
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {

        Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug() // سطح لاگ
                    .WriteTo.Console()    // نمایش در کنسول
                    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // لاگ به فایل (هر روز یک فایل جدید)
                    .CreateLogger();

        try
        {
            Log.Information("Starting web host");




            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();

            builder.Host.UseWindowsService();
            builder.Services.AddWindowsService();

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddHttpClient();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IDbService, DbService>();
            builder.Services.AddScoped<IJsonFormatter, JsonFormatter>();


            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod()
            .WithExposedHeaders("PersonInfoRef", "Content-Disposition");

                });
            });

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }




            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();

        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}



