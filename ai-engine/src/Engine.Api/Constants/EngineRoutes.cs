namespace Engine.Api.Constants;

/// <summary>
/// Маршруты.
/// </summary>
internal static class EngineRoutes
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
        /// Чаты
        /// </summary>
        public const string Engine = Api + "/engine";
    }
}