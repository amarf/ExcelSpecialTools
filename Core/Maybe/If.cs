using System;

public static partial class Expressions
{
    /// <summary>
    /// Выполняет проверку условия
    /// </summary>
    /// <typeparam name="In">Тип проверяемого объекта</typeparam>
    /// <param name="value">Значение</param>
    /// <param name="predicate">Делегат</param>
    /// <returns>Если true возращает In, в противном случаем null</returns>
    public static In If<In>(this In value, Predicate<In> predicate)
        where In : class
    {
        return value == null ? null : predicate(value) ? value : null;
    }

}