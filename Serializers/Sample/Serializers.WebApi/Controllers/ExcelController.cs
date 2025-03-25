using Extensions.Serializers.Abstractions;
using Extensions.Serializers.EPPlus.EPPlusGenerator;
using Microsoft.AspNetCore.Mvc;
using Serializers.WebApi.Models;

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

    [HttpGet("GenerateEPPlus")]
    public IActionResult GenerateEPPlus()
    {
        var filePath = Datasource.GetSheetParameter<Person>().GenerateAndSaveExcelEPPlus(GetFolderPath("Exports"));
        return Ok(filePath);
    }

    [HttpGet("GenerateXMLClosed")]
    public IActionResult GenerateXMLClosed()
    {
        var filePath = Datasource.GetSheetParameter<Person>().GenerateAndSaveExcelClosedXML(GetFolderPath("Exports"));
        return Ok(filePath);
    }

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

    private string GetFolderPath(string foldername) => Path.Combine(_env.WebRootPath, foldername);

}