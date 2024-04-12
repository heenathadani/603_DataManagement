using System;

public class CombatRuntimeException : Exception
{
    public CombatRuntimeException()
    {
    }

    public CombatRuntimeException(string message)
        : base(message)
    {
    }

    public CombatRuntimeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}