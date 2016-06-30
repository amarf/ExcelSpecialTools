using System;

public static partial class Expressions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="In"></typeparam>
    /// <typeparam name="Out"></typeparam>
    /// <param name="value"></param>
    /// <param name="func"></param>
    /// <param name="failValue"></param>
    /// <returns></returns>
    public static Out Return<In, Out>(this In value, Func<In, Out> func, Out failValue)
        where In : class where Out : class
    {
        return value == null ? failValue : func(value);
    }

}

