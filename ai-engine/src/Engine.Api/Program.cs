using Engine.Api.Extensions;
using Engine.Api.Logging;

using Microsoft.OpenApi;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog как провайдер логирования. Уровни, обогащение и приёмники (sinks) задаются
// декларативно в appsettings.json (секция "Serilog"); ReadFrom.Services подхватывает
// обогатители/приёмники, зарегистрированные в DI.
builder.Services.AddSerilog(
    (services, configuration) =>
        configuration.ReadFrom.Configuration(builder.Configuration).ReadFrom.Services(services));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Engine API", Description = "API for Engine", });
    });

// Регистрируем все IEndpoint из сборки Api в DI, чтобы MapEndpoints их нашёл.
builder.Services.AddEndpoints(typeof(Program).Assembly);

// tune application
builder.TuneEngineApplication();

var app = builder.Build();

// Логирование HTTP-запросов одной сводной строкой: метод, путь, статус, длительность,
// а для CQRS-запросов ещё имя запроса и код ошибки (через EnrichDiagnosticContext).
app.UseSerilogRequestLogging(
    options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms [{RequestName} {ErrorCode}]";
        options.EnrichDiagnosticContext = HttpContextRequestDiagnostics.Enrich;
    });

// Глобальный обработчик исключений → ответ в формате ProblemDetails.
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();