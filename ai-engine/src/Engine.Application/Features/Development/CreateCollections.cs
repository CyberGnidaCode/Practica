namespace Engine.Application.Features.Development;

using Engine.Application.Interfaces.VectorStorage;
using Engine.Application.Messaging;
using Engine.Domain.Common;

using FluentValidation;

/// <summary>
/// Команда создания первоначальных коллекций в Qdrant
/// </summary>
public static class CreateCollections
{
    /// <summary>
    /// Команда создания первоначальных коллекций в Qdrant.
    /// </summary>
    public record Command : IRequest<Result<string>>;

    /// <summary>
    /// Обработчик команды <see cref="Command"/>.
    /// </summary>
    public sealed class Handler : IRequestHandler<Command, Result<string>>
    {
        /// <summary>
        /// Сервис для работы с векторной бд
        /// </summary>
        private readonly IVectorStorageService vectorStorageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Handler"/> class.
        /// </summary>
        /// <param name="vectorStorageService">
        /// Сервис для работы с векторной бд
        /// </param>
        public Handler(IVectorStorageService vectorStorageService)
        {
            this.vectorStorageService = vectorStorageService;
        }

        /// <inheritdoc />
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            await this.vectorStorageService.SetColletionsAsync();

            return Result.Success("success");
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
