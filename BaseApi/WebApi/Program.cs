using Core.Library.Configures;
using Core.Library.DependencyInjections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
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

services.AddControllers();
//services.AddSwaggerServices();
#region API Versioning
services.AddEndpointsApiExplorer();

services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

//var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
services.ConfigureOptions<ConfigureSwaggerOptions>();
services.AddSwaggerGen(options =>
{
    //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //   {
    //       Scheme = "Bearer",
    //       BearerFormat = "JWT",
    //       In = ParameterLocation.Header,
    //       Type = SecuritySchemeType.Http, //ApiKey,
    //       Name = "Authorization",
    //       Description = "Bearer Authentication with JWT Token",
    //   });

    //   options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //   {
    //       {
    //           new OpenApiSecurityScheme
    //           {
    //               Reference = new OpenApiReference
    //               {
    //                   Id = "Bearer",
    //                   Type = ReferenceType.SecurityScheme
    //               }
    //           },
    //           new List<string>()
    //       }
    //   });

    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Cấu hình để hiển thị group name trong Swagger UI
    //options.DocInclusionPredicate((docName, apiDesc) =>
    //{
    //	if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;
    //	var groupName = methodInfo.DeclaringType?.GetCustomAttribute<ApiExplorerSettingsAttribute>()?.GroupName;
    //	return string.IsNullOrEmpty(groupName) || groupName == docName;
    //});
});
#endregion

#region Cors
services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
#endregion

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

//app.AddSwaggerApp();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwagger();
app.UseSwaggerUI(
    c =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToLowerInvariant());
        }
    }
);

app.UseCors("corsapp");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
