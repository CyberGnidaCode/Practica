namespace Engine.Application.Features.AI;

using System.Collections.ObjectModel;
using System.Text;

using Engine.Application.Constants;
using Engine.Application.Enums;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Application.Interfaces.VectorStorage;
using Engine.Application.Messaging;
using Engine.Application.Models;
using Engine.Application.Utils;
using Engine.Domain.Common;

using FluentValidation;

/// <summary>
/// Команда отправки сообщения в чат ИИ
/// </summary>
public static class Complete
{
    /// <summary>
    /// Команда отправки сообщения в чат ИИ
    /// </summary>
    public sealed class Command : IRequest<Result<string>>
    {
        /// <summary>
        /// Сообщения
        /// </summary>
        public IReadOnlyCollection<Message> Messages { get; set; }
    }

    /// <summary>
    /// Обработчик команды <see cref="Command"/>.
    /// </summary>
    public sealed class Handler : IRequestHandler<Command, Result<string>>
    {
        /// <summary>
        /// Фабрика клиентов AI
        /// </summary>
        private readonly IAiClientFactory aiClientFactory;

        /// <summary>
        /// Сервис работы внутренней ии.
        /// </summary>
        private readonly IExtractorService extractorService;

        /// <summary>
        /// Сервис для работы с векторной бд
        /// </summary>
        private readonly IVectorStorageService vectorStorageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Handler"/> class.
        /// </summary>
        /// <param name="aiClientFactory">
        /// Фабрика клиентов AI
        /// </param>
        /// <param name="extractorService">
        /// Сервис работы внутренней ии
        /// </param>
        /// <param name="vectorStorageService">
        /// Сервис для работы с векторной бд
        /// </param>
        public Handler(
            IAiClientFactory aiClientFactory,
            IExtractorService extractorService,
            IVectorStorageService vectorStorageService)
        {
            this.aiClientFactory = aiClientFactory;
            this.extractorService = extractorService;
            this.vectorStorageService = vectorStorageService;
        }

        /// <inheritdoc />
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            // client
            var client = this.aiClientFactory.Create(AiProvider.OpenRouter);

            var similar = await this.vectorStorageService.SearchAsync(request.Messages.First().Content, Guid.NewGuid());

            // todo Вынести куда то и создавать через StringBuidler
            var memory = string.Join(". ", similar);

            var promt = new StringBuilder(PromtConstants.MasterAISystemPromt)
                .JoinPromtsBuilder(PromtConstants.MasterAIVectorPromt)
                .JoinPromtsBuilder(memory)
                .ToString();

            var promtContext = new StringBuilder(PromtConstants.MasterAIRecentTurnsPrompt)
                .JoinPromtsBuilder(Letter.Text)
                .ToString();

            var messages = new List<Message>
            {
                new Message { Role = "System", Content = promt },
                new Message { Role = "Assistant", Content = promtContext },
            };

            messages.AddRange(request.Messages);

            var answer = await client.CompleteAsync(
                             new AiRequest { Messages = messages.ToArray(), },
                             cancellationToken);

            var summary = await this.extractorService.SummarizeAsync(answer, cancellationToken);

            await this.vectorStorageService.AddPointAsync(summary, Guid.NewGuid());

            Letter.Text = answer;

            // return
            return Result.Success(answer);
        }
    }

    /// <summary>
    /// Валидатор команды <see cref="Command"/>.
    /// </summary>
    public sealed class Validator : AbstractValidator<Command>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class.
        /// </summary>
        public Validator()
        {
        }
    }
}

/// <summary>
/// sadsad
/// </summary>
public static class Letter
{
    /// <summary>
    /// ntrcn
    /// </summary>
    public static string Text { get; set; } = string.Empty;
}