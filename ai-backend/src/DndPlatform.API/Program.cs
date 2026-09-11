using DndPlatform.Api.Extensions;
using DndPlatform.Api.Hubs;

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
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "DndPlatform API", Description = "API for DndPlatform", });
    });

// Регистрируем все IEndpoint из сборки Api в DI, чтобы MapEndpoints их нашёл.
builder.Services.AddEndpoints(typeof(Program).Assembly);

// tune application
builder.TuneDndPlatformApplication();

var app = builder.Build();

// Логирование HTTP-запросов одной сводной строкой: метод, путь, статус, длительность.
app.UseSerilogRequestLogging();

// Глобальный обработчик исключений → ответ в формате ProblemDetails.
app.UseExceptionHandler();

app.UseCors("client");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.MapEndpoints();

app.MapHub<ChatHub>("/hubs/chat");

app.Run();