namespace store_management_mono_api.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string? message) : base(message)
    {
    }
}