
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using api.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using Prometheus;
//Bugs I had to overcome: Deleting the migrations folder and redo'ing the process. Using "Users" instead of Users

//dotnet new webapi -o api   to create a new api
//dotnet watch run          to run it
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

////////////
builder.Services.AddDbContext<LibraryContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<BookRepositoryCache>();
builder.Services.AddMemoryCache();


/////////////////////////////////////77

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();
app.UseMetricServer();

app.MapControllers();
app.MapMetrics();
app.Run();

