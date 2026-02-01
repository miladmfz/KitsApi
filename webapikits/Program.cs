using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using webapikits.Service;

internal class Program
{
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Starting web host");

            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();

            builder.Host.UseWindowsService();
            builder.Services.AddWindowsService();

            // -----------------------------
            //   Controller & Swagger
            // -----------------------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // -----------------------------
            //  Core Services
            // -----------------------------
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IDbService, DbService>();
            builder.Services.AddScoped<IJsonFormatter, JsonFormatter>();

            // -----------------------------
            //  Auth Services
            // -----------------------------
            builder.Services.AddScoped<PasswordHasher>();
            builder.Services.AddScoped<JwtMaker>();

            // -----------------------------
            //  JWT Authentication
            // -----------------------------
            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            // -----------------------------
            //    CORS
            // -----------------------------
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .WithExposedHeaders("PIC", "Content-Disposition", "SessionId", "UserName");
                });
            });

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            var app = builder.Build();

            // -----------------------------
            //  Middleware Pipeline
            // -----------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();   // 🔥 مهم
            app.UseAuthorization();

            app.MapControllers();

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
