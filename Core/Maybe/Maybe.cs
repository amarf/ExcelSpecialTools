using System;

public struct Maybe<T>
{
    private T _value;

    private Maybe(T value)
    {
        System.Diagnostics.Contracts.Contract.Requires(HasValue);
        _value = value;
    }

    public T Value
    {
        get
        {
            if (HasValue)
                return _value;
            throw new NullReferenceException();
        }
    }

    public bool HasValue
    {
        get
        {
            return _value != null;
        }
    }

    public bool HasNoValue
    {
        get
        {
            return _value == null;
        }
    }

    public static implicit operator Maybe<T>(T value)
    {
        return new Maybe<T>(value);
    }
}

