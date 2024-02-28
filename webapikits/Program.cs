


internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseWindowsService();
        builder.Services.AddWindowsService();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();



        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin() // Replace with your actual client origin.
                       .AllowAnyHeader()
                       .AllowAnyMethod();

            });
        });


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
}



