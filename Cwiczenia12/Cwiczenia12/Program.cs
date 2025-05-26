using Cwiczenia12.Data;
using Cwiczenia12.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApbdContext>(options=> options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IClientService, ClientService>();
var app = builder.Build();



app.UseAuthorization();
app.MapControllers();
app.Run();
