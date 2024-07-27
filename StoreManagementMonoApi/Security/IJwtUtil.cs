using store_management_mono_api.Entities;

namespace store_management_mono_api.Security;

public interface IJwtUtil
{
    string GenerateToken(Account account);
}