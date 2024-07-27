namespace store_management_mono_api.Exceptions;

public class InternalServerError : Exception
{
    public InternalServerError(string? message) : base(message)
    {
    }
}