using Microsoft.OpenApi.Models;
using WebAPI.Dapper.Mysql.Helpers;
using WebAPI.Dapper.Mysql.Helpers.Interfaces;
using WebAPI.Dapper.Mysql.Repositories;

var APP_NAME = "Dapper with MySQL";


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = APP_NAME, Version = "v1" });
});

builder.Services.AddScoped<IDBContext, DBContext>(); // To have the same connection per request
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // To have the same connection per request

builder.Services.AddTransient<CountriesRepository>();
builder.Services.AddTransient<PersonsRepository>();
builder.Services.AddTransient<ProductsRepository>();
builder.Services.AddTransient<EventSourcingRepository>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", APP_NAME);
    c.RoutePrefix = string.Empty;
});


app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();