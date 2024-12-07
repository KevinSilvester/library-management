using DotNetEnv;
using library_management.Data;
using library_management.Repositories;
using Microsoft.EntityFrameworkCore;

namespace library_management;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Env.Load();
        var connectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION_STRING");

        builder.Services.AddDbContext<LibraryDbContext>(options =>
        options.UseSqlServer(connectionString));
        // Add Repositories and Services
        builder.Services.AddScoped<IBookRepository, BookRepository>();
        builder.Services.AddScoped<IMemberRepository, MemberRepository>();
        builder.Services.AddScoped<IBorrowingRepository, BorrowingRepository>();

        builder.Services.AddSingleton<MongoDbContext>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllers();

        app.Run();
    }
}
