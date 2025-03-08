using Extensions.Serializers.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Serializers.WebApi.EPPlusGenerator;

namespace Serializers.WebApi.Controllers;

public class ExcelController : BaseController
{
    private readonly IExcelSerializer _excelSerializer;
    private readonly IWebHostEnvironment _env;
    public ExcelController(IExcelSerializer excelSerializer, IWebHostEnvironment env)
    {
        _excelSerializer = excelSerializer;
        _env = env;
    }
  
    [HttpGet("Generate")]
    public IActionResult Generate()
    {
        var filePath = Datasource.GetMainDatasource().Generate(GetFolderPath("Exports"));
        return Ok(filePath);
    }

    private string GetFolderPath(string foldername) => Path.Combine(_env.WebRootPath, foldername);

    [HttpGet("ExportData")]
    public IActionResult ExportData()
    {
        // Get the path to the wwwroot/Export folder
        string exportFolderPath = Path.Combine(_env.WebRootPath, "Export");

        // Ensure the Export folder exists
        if (!Directory.Exists(exportFolderPath))
        {
            Directory.CreateDirectory(exportFolderPath);
        }

        // Example: Create a file in the Export folder
        string filePath = Path.Combine(exportFolderPath, $"{Guid.NewGuid().ToString()}.txt");
        System.IO.File.WriteAllText(filePath, "This is an example file.");

        return Content($"File created at: {filePath}");
    }


}