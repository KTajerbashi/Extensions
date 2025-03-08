using Extensions.Serializers.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Serializers.WebApi.Controllers;

public class ExcelController : BaseController
{
    private readonly IExcelSerializer _excelSerializer;

    public ExcelController(IExcelSerializer excelSerializer)
    {
        _excelSerializer = excelSerializer;
    }

    [HttpGet("Generate")]
    public IActionResult Generate()
    {
        string filePath = string.Empty;
        return Ok(filePath);
    }

}