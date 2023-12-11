using AsientosContrablesApi.Models;
using Hangfire.Server;

public interface IContabilidadRepository
{
  Task<CrearAsientosSuccessResponse> CrearAsientosContables(List<AsientoContableDTO> asientoContables, List<DataModel> numeroSAP);
  Task<Guid> CrearProceso(List<AsientoContableDTO> asientoContables, PerformContext perform, Guid proccesId);
  Task<ProcesoDTO> ConsultarProceso(Guid id);
  Task<RegistroDTO> ConsultarRegistro(Guid id);
}