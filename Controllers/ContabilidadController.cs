namespace AsientosContrablesApi.Controllers;

using AsientosContrablesApi.Models;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using AsientosContrablesApi.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

[Route("api/[controller]")]
[ApiController]
public class ContabilidadController : ControllerBase
{
  private readonly ISAPService _SAPService;
  private readonly IContabilidadRepository _contabilidad;
  private readonly ILogger _logger;
  public ContabilidadController(ISAPService SAPService, IContabilidadRepository contabilidad, ILogger<ContabilidadController> logger)
  {
    _SAPService = SAPService;
    _contabilidad = contabilidad;
    _logger = logger;
  }

  [HttpPost]
  public async Task<ActionResult<CrearAsientosSuccessResponse>> CrearAsientosJSON([FromBody] List<AsientoContableDTO> asientos)
  {
    var result = await _SAPService.CrearAsientos(asientos);
    _logger.LogInformation("Resultado SAP API: {Response}", result);
    var agregarAsientos = await _contabilidad.CrearAsientosContables(asientos, result);
    _logger.LogInformation("Resultado agregar asientos DB: {Response}", agregarAsientos);
    return Ok(agregarAsientos);
  }

  [HttpGet("Proceso/{id}")]
  public async Task<ActionResult<ProcesoDTO>> ConsultarProceso(Guid id)
  {
    return await _contabilidad.ConsultarProceso(id);
  }

  [HttpGet("Registro/{id}")]
  public async Task<ActionResult<RegistroDTO>> ConsultarRegistro(Guid id)

  {
    return await _contabilidad.ConsultarRegistro(id);
  }

  // NO FUNCIONA
  [HttpPost("ProcesarCSV")]
  public async Task<ActionResult<List<AsientoContableDTO>>> ProcesarCSV([FromForm] IFormFile file)
  {
    try
    {
      if (file == null || file.Length == 0)
      {
        return BadRequest("Archivo no proporcionado");
      }

      var asientosContablesDTO = await LeerCSV(file);
      await _SAPService.CrearAsientos(asientosContablesDTO);
      return Ok(asientosContablesDTO);
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Error al procesar el CSV: {ex.Message}");
    }
  }

  private async Task<List<AsientoContableDTO>> LeerCSV(IFormFile file)
  {
    try
    {
      using (var reader = new StreamReader(file.OpenReadStream()))
      {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
          Delimiter = ";"
        };

        using (var csvReader = new CsvReader(reader, csvConfig))
        {
          await csvReader.ReadAsync();
          csvReader.ReadHeader();

          var asientosContablesDTO = new List<AsientoContableDTO>();
          while (await csvReader.ReadAsync())
          {
            var asientoContableDTO = csvReader.GetRecord<AsientoContableDTO>();

            // Crear la lista de JournalEntryLinesDTO a partir de las columnas del CSV
            asientoContableDTO.JournalEntryLines = new List<JournalEntryLinesDTO>
                    {
                        new JournalEntryLinesDTO
                        {
                            AccountCode = csvReader.GetField<string>("AccountCode"),
                            Debit = csvReader.GetField<double>("Debit"),
                            Credit = csvReader.GetField<double>("Credit"),
                            LineMemo = csvReader.GetField<string>("LineMemo"),
                            Reference1 = csvReader.GetField<string>("Reference1")
                        }
                    };

            asientosContablesDTO.Add(asientoContableDTO);
          }

          return asientosContablesDTO;
        }
      }
    }
    catch (Exception ex)
    {
      throw new Exception($"Error al procesar el CSV: {ex.Message}");
    }
  }

}