using Application.Interface.Databases;
using Core.Library.DependencyInjections;
using Infrastructure.SqlContext;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddHttpContextAccessor();
services.AddAuthorization();
services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    // Use PascalCase in Json serialization
    //options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

#region DbContext
services.AddDbContext<BaseWriteContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnectionWrite");
    options.UseSqlServer(connectionString);
}).AddScoped<ICasperWriteContext, BaseWriteContext>();

services.AddDbContext<BaseReadContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnectionRead");
    options.UseSqlServer(connectionString);
}).AddScoped<ICasperReadContext, BaseReadContext>();
#endregion

services.AddControllers();
services.AddSwaggerServices();

#region Cors
services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.AddSwaggerApp();

app.UseCors("corsapp");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
