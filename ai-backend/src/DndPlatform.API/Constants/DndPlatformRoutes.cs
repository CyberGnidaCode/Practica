namespace DndPlatform.Api.Constants;

/// <summary>
/// Маршруты.
/// </summary>
internal static class DndPlatformRoutes
{
    /// <summary>
    /// Базовый маршрут.
    /// </summary>
    public const string Api = "/api";

    /// <summary>
    /// Часть маршурта.
    /// </summary>
    internal static class Parts
    {
        /// <summary>
        /// Разработка.
        /// </summary>
        public const string Dev = Api + "/development";

        /// <summary>
        /// Чат
        /// </summary>
        public const string Chat = Api + "/chat";
    }
}