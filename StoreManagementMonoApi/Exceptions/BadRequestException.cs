namespace store_management_mono_api.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string? message) : base(message)
    {
    }
}