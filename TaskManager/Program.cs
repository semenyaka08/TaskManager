using Microsoft.EntityFrameworkCore;
using TaskManager.DataBase;
using TaskManager.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<TaskNoteDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5, // количество повторов
            maxRetryDelay: TimeSpan.FromSeconds(10), // задержка между повторами
            errorNumbersToAdd: null // коды ошибок
        )
        ));

builder.Services.AddScoped<ITaskNoteRepository, TaskNoteRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    var dbContext = scope.ServiceProvider.GetRequiredService<TaskNoteDbContext>();
    await dbContext.Database.MigrateAsync();  // Apply any pending migrations
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();