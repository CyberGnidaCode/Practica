namespace Engine.Application.Utils;

using System.Globalization;
using System.Text;

/// <summary>
/// Класс для работы с промтом
/// </summary>
public static class PromtGenerator
{
    /// <summary>
    /// Склеить заголовок + тело промта
    /// </summary>
    /// <param name="header">
    /// Заголовок
    /// </param>
    /// <param name="bodyText">
    /// Тело промта
    /// </param>
    /// <returns>
    /// промт
    /// </returns>
    public static StringBuilder JoinHeaderBodyBuilder(string header, string bodyText)
    {
        var builder = new StringBuilder();

        builder.AppendFormat(CultureInfo.InvariantCulture, "[{0}]\n{1}\n", header, bodyText);

        return builder;
    }

    /// <summary>
    /// Склеить два промта в один
    /// </summary>
    /// <param name="lastPromt">
    /// Прошлый промт
    /// </param>
    /// <param name="nextPromt">
    /// Новый промт
    /// </param>
    /// <returns>
    /// промт
    /// </returns>
    public static StringBuilder JoinPromtsBuilder(this StringBuilder lastPromt, string nextPromt)
    {
        var builder = new StringBuilder();

        builder.Append(lastPromt);
        builder.Append('\n');
        builder.Append(nextPromt);
        builder.Append('\n');

        return builder;
    }
}