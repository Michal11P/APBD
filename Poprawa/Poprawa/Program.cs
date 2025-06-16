using Microsoft.EntityFrameworkCore;
using Poprawa.Data;
using Poprawa.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<ICharactersService, CharacterService>();

var app = builder.Build();


app.UseAuthorization();
app.MapControllers();
app.Run();
