using System;

// A custom error class. Nothing fancy - just useful for information and debugging later.
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