using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Context;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Modules.OtherModule._Repositories._Interfaces;
using store_management_mono_api.Modules.OtherModule._ViewModels;

namespace store_management_mono_api.Modules.OtherModule._Repositories;

public class OtherRepository : IOtherRepository
{
    private readonly AppDbContext _context;

    public OtherRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<OtherResponseVM> GetOtherParameterize(string type)
    {
        IEnumerable<OtherResponseVM> results;
        switch (type.ToLower())
        {
            case "role":
                results = _context.Roles.AsNoTracking().ToList().Select(r => new OtherResponseVM
                {
                    Id = r.Id,
                    Name = r.Name
                });
                break;
            case "unit-product":
                results = _context.UnitProducts.AsNoTracking().ToList().Select(u => new OtherResponseVM
                {
                    Id = u.Id,
                    Name = u.Name
                });
                break;
            case "purchase-type":
                results = _context.PurchaseTypes.AsNoTracking().ToList().Select(p => new OtherResponseVM
                {
                    Id = p.Id,
                    Name = p.Name
                });
                break;
            default:
                throw new NotFoundException("Parameter tidak ditemukan");
        }

        return results;
    }
}