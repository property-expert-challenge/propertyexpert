using FluentValidation;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;
using System.Reflection;
using WebApi.Models;
using WebApi.Models.Options;
using WebApi.Models.Validators;
using WebApi.Services;
using WebApi.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);
ConfigureOptions(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Property Expert API v1");
        c.RoutePrefix = "swagger";
    });
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseHttpLogging();
app.Run();

static void ConfigureServices(IServiceCollection services)
{
    // Endpoints
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Property Expert API",
            Version = "v1",
            Description = "An API for evaluating property repair and maintenance invoices.",
            Contact = new OpenApiContact
            {
                Name = "Property Expert Team",
                Email = "support@propertyexpert.ai"
            }
        });

        // Include XML comments in Swagger documentation
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    services.AddControllers();

    // Services
    services.AddScoped<IEvaluationService, EvaluationService>();
    services.AddScoped<IClassificationHttpClient, ClassificationHttpClient>();

    // Validators
    services.AddScoped<IValidator<EvaluationRequest>, EvaluationRequestValidator>();

    // Logging
    services.AddHttpLogging(logging => { logging.LoggingFields = HttpLoggingFields.All; });
}

static void ConfigureOptions(IServiceCollection services)
{
    services
        .AddOptions<ClassificationApiOptions>()
        .Configure<IConfiguration>((options, configuration) =>
        {
            var configurationSection = configuration.GetSection(nameof(ClassificationApiOptions));
            configurationSection.Bind(options);
        })
        .ValidateDataAnnotations();
}

public partial class Program
{
}