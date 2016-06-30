using System;

public static partial class Expressions
{
    /// <summary>
    /// Выполняет действия над объектом
    /// </summary>
    /// <typeparam name="In">Тип объекта над которым выполняются действия</typeparam>
    /// <param name="value">Исходных объект</param>
    /// <param name="action">Делегат выполняющий действия над объектом</param>
    /// <returns>Преобразованный объект</returns>
    public static In Do<In>(this In value, Action<In> action)
        where In : class
    {
        if (value == null)
            return null;
        action(value);
        return value;
    }

}