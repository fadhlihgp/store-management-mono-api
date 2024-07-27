namespace store_management_mono_api.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string? message) : base(message)
    {
    }
}