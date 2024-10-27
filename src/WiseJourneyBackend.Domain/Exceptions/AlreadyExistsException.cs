namespace WiseJourneyBackend.Domain.Exceptions;

public class AlreadyExistsException : Exception
{
    public string? ExistsValue { get; }

    public AlreadyExistsException(string message)
        : base(message)
    {
    }

    public AlreadyExistsException(string message, string existsValue)
        : base(message)
    {
        ExistsValue = existsValue;
    }

    public AlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}