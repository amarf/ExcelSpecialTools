using System;

public static partial class Expressions
{
    public static Out With<In, Out>(this In value, Func<In, Out> func)
        where In : class where Out : class
    {
        return value == null ? null : func(value);
    }

}

