namespace Engine.Infrastructure.Services;

using Engine.Application.Constants;
using Engine.Application.Enums;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Application.Models;

/// <summary>
/// сервис работы внутренней ИИ.
/// </summary>
public sealed class ExtractorService : IExtractorService
{
    private readonly IAiClientFactory aiClientFactory;

    private readonly IAiClient client;

    /// <summary>
    /// сервис работы внутренней ИИ.
    /// </summary>
    /// <param name="aiClientFactory">фабрика клиентов ии</param>
    public ExtractorService(IAiClientFactory aiClientFactory)
    {
        this.aiClientFactory = aiClientFactory;
        this.client = this.aiClientFactory.Create(AiProvider.OpenRouter);
    }

    /// <summary>
    /// Получение самммари из текста.
    /// </summary>
    /// <param name="text">Исходный текст</param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>Саммари</returns>
    public async Task<string> SummarizeAsync(string text, CancellationToken cancellationToken)
    {
        var prompt = PromtConstants.ExtractorAiSummaryPrompt;

        var request = new AiRequest
        {
            Messages = new List<Message>
            {
                new Message { Role = "System", Content = prompt },
                new Message { Role = "User", Content = text },
            },
        };

        var response = await this.client.CompleteAsync(request, cancellationToken);

        return response;
    }

    /// <summary>
    /// Получение embedding из текста.
    /// </summary>
    /// <param name="text">Исходный текст</param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>Саммари</returns>
    public async Task<string> EmbeddingAsync(string text, CancellationToken cancellationToken)
    {
        var prompt = PromtConstants.ExtractorAiEmbeddingPrompt;

        var request = new AiRequest
        {
            Messages = new List<Message>
            {
                new Message { Role = "System", Content = prompt },
                new Message { Role = "User", Content = text },
            },
        };

        var response = await this.client.CompleteAsync(request, cancellationToken);

        return response;
    }
}