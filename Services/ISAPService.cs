using AsientosContrablesApi.Models;

namespace AsientosContrablesApi.Services
{
  public interface ISAPService
  {
    Task<List<DataModel>> CrearAsientos(List<AsientoContableDTO> asientos);
    // Task<List<DataModel>> CrearAsientosJSON(List<AsientoContableDTO> asientos);
    // Task<List<DataModel>> CrearAsientosCSV(string asientos);

  }
}