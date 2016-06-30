using System;

public static partial class Expressions
{
    public static bool ReturnSuccess<In>(this In value)
        where In : class
    {
        return value == null ? false : true;
    }

}

