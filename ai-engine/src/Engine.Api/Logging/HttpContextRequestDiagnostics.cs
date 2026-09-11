namespace Engine.Api.Logging;

using Engine.Application.Diagnostics;

using Serilog;

/// <summary>
/// Реализация <see cref="IRequestDiagnostics"/> поверх текущего HTTP-запроса. Прикладной слой
/// складывает сведения в <see cref="HttpContext.Items"/>, а <see cref="Enrich"/> переносит их
/// в Serilog-контекст, чтобы они попали в единую итоговую строку UseSerilogRequestLogging.
/// </summary>
internal sealed class HttpContextRequestDiagnostics : IRequestDiagnostics
{
    /// <summary>
    /// Ключ для хранения названия запроса в <see cref="HttpContext.Items"/>.
    /// </summary>
    private const string RequestNameItem = "Diagnostics.RequestName";

    /// <summary>
    /// Ключ для хранения кода ошибки в <see cref="HttpContext.Items"/>.
    /// </summary>
    private const string ErrorCodeItem = "Diagnostics.ErrorCode";

    /// <summary>
    /// Ключ для хранения описания ошибки в <see cref="HttpContext.Items"/>.
    /// </summary>
    private const string ErrorDescriptionItem = "Diagnostics.ErrorDescription";

    /// <summary>
    /// Ключ для хранения категории ошибки в <see cref="HttpContext.Items"/>.
    /// </summary>
    private const string ErrorTypeItem = "Diagnostics.ErrorType";

    /// <summary>
    /// Ключ для хранения значения по умолчанию в <see cref="HttpContext.Items"/>.
    /// </summary>
    private const string Missing = "-";

    /// <summary>
    /// Акцесс к текущему HTTP-контексту для чтения и записи диагностических данных запроса.
    /// </summary>
    private readonly IHttpContextAccessor accessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpContextRequestDiagnostics"/> class.
    /// </summary>
    /// <param name="accessor">Доступ к текущему HTTP-контексту.</param>
    public HttpContextRequestDiagnostics(IHttpContextAccessor accessor) => this.accessor = accessor;

    /// <summary>
    /// Переносит накопленные сведения запроса в Serilog-контекст итоговой строки.
    /// Подключается через <c>UseSerilogRequestLogging(options =&gt; options.EnrichDiagnosticContext = Enrich)</c>.
    /// </summary>
    /// <param name="diagnosticContext">Serilog-контекст итоговой строки запроса.</param>
    /// <param name="httpContext">Текущий HTTP-контекст.</param>
    public static void Enrich(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(diagnosticContext);
        ArgumentNullException.ThrowIfNull(httpContext);

        diagnosticContext.Set("RequestName", Read(httpContext, RequestNameItem));
        diagnosticContext.Set("ErrorCode", Read(httpContext, ErrorCodeItem));
        diagnosticContext.Set("ErrorDescription", Read(httpContext, ErrorDescriptionItem));
        diagnosticContext.Set("ErrorType", Read(httpContext, ErrorTypeItem));
    }

    /// <inheritdoc />
    public void SetRequestName(string requestName) => this.Set(RequestNameItem, requestName);

    /// <inheritdoc />
    public void SetError(string code, string description, string type)
    {
        this.Set(ErrorCodeItem, code);
        this.Set(ErrorDescriptionItem, description);
        this.Set(ErrorTypeItem, type);
    }

    /// <summary>
    /// Чтение значения из <see cref="HttpContext.Items"/> с защитой от отсутствия ключа или значения.
    /// </summary>
    /// <param name="httpContext">Текущий HTTP-контекст.</param>
    /// <param name="key">Ключ для чтения значения.</param>
    /// <returns>Значение из HttpContext.Items или значение по умолчанию.</returns>
    private static object Read(HttpContext httpContext, string key) =>
        httpContext.Items.TryGetValue(key, out var value) && value is not null ? value : Missing;

    /// <summary>
    /// Запись значения в <see cref="HttpContext.Items"/> для последующего переноса в Serilog-контекст.
    /// </summary>
    /// <param name="key">Ключ для записи значения.</param>
    /// <param name="value">Значение для записи.</param>
    private void Set(string key, object value)
    {
        var context = this.accessor.HttpContext;

        if (context is not null)
        {
            context.Items[key] = value;
        }
    }
}
