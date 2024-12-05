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

        builder.Services.AddDbContext<LibraryDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add Repositories and Services
        builder.Services.AddScoped<IBookRepository, BookRepository>();
        builder.Services.AddScoped<IMemberRepository, MemberRepository>();
        builder.Services.AddScoped<IBorrowingRepository, BorrowingRepository>();

        builder.Services.AddSingleton<MongoDbContext>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();



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
