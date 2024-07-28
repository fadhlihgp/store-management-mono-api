using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Context;
using store_management_mono_api.Entities;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Helpers;
using store_management_mono_api.Modules.AuthModule._Repositories._Interfaces;
using store_management_mono_api.Modules.AuthModule._ViewModels;
using store_management_mono_api.Services.Interfaces;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.AuthModule._Repositories;

public class ForgetPasswordRepository : IForgetPasswordRepository
{
    private readonly AppDbContext _context;
    private readonly IApiUtilityService _apiUtilityService;
    private readonly IConfiguration _configuration;

    public ForgetPasswordRepository(AppDbContext context, IConfiguration configuration, IApiUtilityService apiUtilityService)
    {
        _context = context;
        _configuration = configuration;
        _apiUtilityService = apiUtilityService;
    }

    public async Task SendLinkOtpToEmail(SendLinkOtpVm sendLinkOtp)
    {
        // Find user by email
        var account = await _context.Accounts.Where(a => a.Email.ToLower().Equals(sendLinkOtp.Email.ToLower())).FirstOrDefaultAsync();
        if (account == null) throw new NotFoundException("Email tidak terdaftar!");
        
        // Find otp token by email
        var findOtpByEmail = await _context.Otps.Where(o => o.Email.Equals(sendLinkOtp.Email)).FirstOrDefaultAsync();
        try
        {
            await _context.Database.BeginTransactionAsync();
            // If find otp token by email is not null, delete it
            if (findOtpByEmail != null)
            {
                _context.Remove(findOtpByEmail);
            }

            // Get random token
            string randomToken = TextHelper.GetRandomToken(38);
            
            // Create otp token object
            var otp = new Otp
            {
                Id = Guid.NewGuid().ToString(),
                Email = sendLinkOtp.Email,
                Value = randomToken,
                CreatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddMinutes(10),
                IsVerified = false
            };
            
            _context.Otps.Add(otp);
            await _context.SaveChangesAsync();

            string link = _configuration["Otp:WebFrontend"];
            string bodyValue = "Klik link berikut untuk me-reset password anda: "+link+"reset-password/"+randomToken;
            var sendEmail = new EmailVm
            {
                To = sendLinkOtp.Email,
                Subject = "Reset Password - KelolaWarung",
                Body = bodyValue
            };
            await _apiUtilityService.SendEmail(sendEmail);
            await _context.Database.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<GetVerificationResponseVm> VerifyOtp(string token)
    {
        var findOtp = await _context.Otps.Where(o => o.Value.Equals(token)).FirstOrDefaultAsync();
        if (findOtp == null) throw new NotFoundException("Link tidak valid!");
        if (findOtp is not null && findOtp.IsVerified) throw new BadRequestException("Link sudah digunakan!");
        if (findOtp is not null && findOtp.ExpiredAt < DateTime.Now) throw new BadRequestException("Link sudah kadaluwarsa!");
        findOtp.IsVerified = true;
        _context.Otps.Update(findOtp);
        await _context.SaveChangesAsync();
        return new GetVerificationResponseVm
        {
            Email = findOtp?.Email,
            IsSuccess = true
        };
    }

    public async Task ResetPassword(ResetPasswordRequestVm resetPasswordVm)
    {
        var findOtp = await _context.Otps.Where(o => o.Value.Equals(resetPasswordVm.Token)).FirstOrDefaultAsync();
        if (findOtp == null) throw new NotFoundException("Link tidak valid!");
        var account = await _context.Accounts.Where(a => a.Email.Equals(findOtp.Email)).FirstOrDefaultAsync();
        if (account == null) throw new NotFoundException("Email tidak terdaftar!");
        try
        {
            await _context.Database.BeginTransactionAsync();
            account.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordVm.NewPassword);
            _context.Accounts.Update(account);
            _context.Otps.Remove(findOtp);
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            Console.WriteLine(e);
            throw;
        }
    }
}