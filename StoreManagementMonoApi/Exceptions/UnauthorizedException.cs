namespace store_management_mono_api.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string? message) : base(message)
    {
    }
}