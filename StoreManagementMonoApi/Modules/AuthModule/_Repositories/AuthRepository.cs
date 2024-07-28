using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Context;
using store_management_mono_api.Entities;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Modules.AuthModule._Repositories._Interfaces;
using store_management_mono_api.Modules.AuthModule._ViewModels;
using store_management_mono_api.Security;

namespace store_management_mono_api.Modules.AuthModule._Repositories;

public class AuthRepository : IAuthRepository
{
    private AppDbContext _context;
    private IJwtUtil _jwtUtil;

    public AuthRepository(AppDbContext context, IJwtUtil jwtUtil)
    {
        _context = context;
        _jwtUtil = jwtUtil;
    }

    // Login
    public async Task<LoginResponseVm> Login(LoginRequestVm loginRequestVm)
    {
        Account? findByUsernameOrEmail = await _context.Accounts.Where(a =>
                ((String.Equals(a.Username, loginRequestVm.Email, StringComparison.OrdinalIgnoreCase) ||
                  String.Equals(a.Email, loginRequestVm.Email, StringComparison.OrdinalIgnoreCase))) && !a.IsDeleted)
            .Include(account => account.Role)
            .FirstOrDefaultAsync();
        
        if (findByUsernameOrEmail == null) throw new UnauthorizedException("Username/Email atau Password salah");

        if ((bool)(!findByUsernameOrEmail.IsActive)!) throw new UnauthorizedException("Akun anda tidak aktif");
        
        bool isValid = BCrypt.Net.BCrypt.Verify(loginRequestVm.Password, findByUsernameOrEmail.Password);
        if (!isValid) throw new UnauthorizedException("Username/Email atau Password salah");

        try
        {
            var userLogin = new UserLogin
            {
                FullName = findByUsernameOrEmail.FullName,
                Email = findByUsernameOrEmail.Email,
                RoleId = findByUsernameOrEmail.RoleId,
                Role = findByUsernameOrEmail.Role.Name
            };
            // responseVm.User = userLogin;
            var token = _jwtUtil.GenerateToken(findByUsernameOrEmail);
            findByUsernameOrEmail.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();
            return new LoginResponseVm
            {
                User = userLogin,
                Token = token
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}