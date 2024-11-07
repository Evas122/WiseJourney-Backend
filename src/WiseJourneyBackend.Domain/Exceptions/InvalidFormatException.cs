namespace WiseJourneyBackend.Domain.Exceptions;

public class InvalidFormatException : Exception
{
    public string? InvalidValue { get; }

    public InvalidFormatException(string message)
        : base(message)
    {
    }

    public InvalidFormatException(string message, string invalidValue)
        : base(message)
    {
        InvalidValue = invalidValue;
    }

    public InvalidFormatException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}