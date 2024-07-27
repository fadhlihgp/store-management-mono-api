using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using store_management_mono_api.Modules.OtherModule._Repositories._Interfaces;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.OtherModule._Controllers;

[ApiController]
[Authorize]
[Route("api/v1/other")]
public class OtherController : ControllerBase
{
    private readonly IOtherRepository _otherRepository;
    private readonly IDashboardRepository _dashboardRepository;
    public OtherController(IOtherRepository otherRepository, IDashboardRepository dashboardRepository)
    {
        _otherRepository = otherRepository;
        _dashboardRepository = dashboardRepository;
    }

    [HttpGet("parameterize/{type}")]
    public IActionResult GetParameterize([FromRoute] string type)
    {
        var data = _otherRepository.GetOtherParameterize(type);
        return Ok(new MultipleDataResponse
        {
            Message = "Berhasil mengambil data",
            TotalData = data.Count(),
            Data = data
        });
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard([FromQuery] int? year)
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        var data = await _dashboardRepository.GetDashboard(storeId, year);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mengambil data",
            Data = data
        });
    }
}