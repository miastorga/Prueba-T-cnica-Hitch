using System.Runtime.InteropServices;
using AsientosContrablesApi.Models;
using Hangfire;
using Hangfire.Server;
using Hangfire.States;
using Microsoft.EntityFrameworkCore;

namespace AsientosContrablesApi.Repository
{
  public class ContabilidadRepository : IContabilidadRepository
  {
    private readonly Context _context;
    private readonly ILogger _logger;
    public ContabilidadRepository(Context context, ILogger<ContabilidadRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task<ProcesoDTO> ConsultarProceso(Guid id)
    {
      var proceso = await _context.Procesos
          .Where(p => p.ProcessId == id)
          .FirstOrDefaultAsync();

      var procesoDTO = new ProcesoDTO
      {
        ProcessId = proceso.ProcessId,
        StartDate = proceso.StartDate,
        EndDate = proceso.EndDate,
        LastUpdate = proceso.LastUpdate,
        Items = proceso.Items,
        Status = proceso.Status,
        Failed = proceso.Failed,
        Success = proceso.Success
      };
      return procesoDTO;
    }

    public async Task<RegistroDTO> ConsultarRegistro(Guid id)
    {
      var registro = await _context.Registros
          .Where(p => p.AsientoContableReference == id)
          .FirstOrDefaultAsync();

      var regsitroDTO = new RegistroDTO
      {
        ProcessId = registro.ProcessId,
        StartDate = registro.StartDate,
        EndDate = registro.EndDate,
        LastUpdate = registro.LastUpdate,
        AsientoContableReference = registro.AsientoContableReference,
        Error = registro.Error,
        NumberSap1 = registro.NumberSap1,
        NumberSap2 = registro.NumberSap2,
        Status = registro.Status,
        Proceso = registro.Proceso
      };
      return regsitroDTO;
    }

    public async Task<CrearAsientosSuccessResponse> CrearAsientosContables(List<AsientoContableDTO> asientoContablesDTO, List<DataModel> numerosSAP)
    {
      try
      {

        var proccesId = Guid.NewGuid();
        var job = BackgroundJob.Enqueue(() => CrearProceso(asientoContablesDTO, null, proccesId));

        foreach (var asientoContableDTO in asientoContablesDTO)
        {
          var referenceId = Guid.NewGuid();
          var asientoContable = new AsientoContable
          {
            Reference = referenceId,
            ReferenceDate = asientoContableDTO.ReferenceDate,
            Memo = asientoContableDTO.Memo,
            TaxDate = asientoContableDTO.TaxDate,
            DueDate = asientoContableDTO.DueDate,
            JournalEntryLines = asientoContableDTO.JournalEntryLines.Select(line => new JournalEntryLines
            {
              LineId = line.LineId,
              AccountCode = line.AccountCode,
              Debit = line.Debit,
              Credit = line.Credit,
              LineMemo = line.LineMemo,
              Reference1 = Guid.NewGuid()
            }).ToList()
          };
          _logger.LogInformation("asiento contable: {a}", asientoContable);
          _logger.LogInformation("asiento contable id: {a}", asientoContable.Reference);

          await _context.AddAsync(asientoContable);
          await _context.SaveChangesAsync();
          BackgroundJob.ContinueJobWith(job, () => CrearRegistro(numerosSAP, null, proccesId, referenceId));

        }

        var response = new CrearAsientosSuccessResponse
        {
          ProcessId = proccesId.ToString(),
          Success = true,
          Error = null
        };
        return response;
      }
      catch (Exception ex)
      {

        var response = new CrearAsientosSuccessResponse
        {
          ProcessId = "",
          Success = false,
          Error = ex.Message
        };
        return response;
      }
    }

    public async Task<Guid> CrearProceso(List<AsientoContableDTO> asientoContableDTO, PerformContext perform, Guid proccesId)
    {
      string jobId = perform.BackgroundJob.Id;
      var createdAt = perform.BackgroundJob.CreatedAt;
      var jobData = JobStorage.Current.GetConnection().GetJobData(jobId);
      var isSuccess = jobData.State == SucceededState.StateName;
      var isFailed = jobData.State == FailedState.StateName;
      var completionDate = isSuccess || isFailed ? DateTime.Now : (DateTime?)null;

      var nuevoProceso = new Proceso
      {
        ProcessId = proccesId,
        Items = asientoContableDTO.Count,
        Status = isSuccess ? "Completado" : (isFailed ? "Fallido" : "En proceso"),
        Failed = isFailed ? 1 : 0,//cambiar
        Success = asientoContableDTO.Count,//cambiar
        LastUpdate = DateTime.Now,
        EndDate = completionDate ?? DateTime.Now,
        StartDate = createdAt
      };
      await _context.AddAsync(nuevoProceso);
      await _context.SaveChangesAsync();
      return nuevoProceso.ProcessId;
    }

    public async Task<bool> CrearRegistro(List<DataModel> numerosSAP, PerformContext perform, Guid proccesId, Guid reference)
    {
      string jobId = perform.BackgroundJob.Id;
      var createdAt = perform.BackgroundJob.CreatedAt;
      var jobData = JobStorage.Current.GetConnection().GetJobData(jobId);
      var isSuccess = jobData.State == SucceededState.StateName;
      var isFailed = jobData.State == FailedState.StateName;
      var completionDate = isSuccess || isFailed ? DateTime.Now : (DateTime?)null;

      try
      {
        foreach (var numeroSAP in numerosSAP)
        {
          var nuevoRegistro = new Registro
          {
            RegistroId = Guid.NewGuid(),
            StartDate = createdAt,
            EndDate = completionDate ?? DateTime.Now,
            LastUpdate = DateTime.Now,
            NumberSap1 = numeroSAP.DocEntry,
            NumberSap2 = numeroSAP.DocNum,
            ProcessId = proccesId,
            Error = "",
            Status = isSuccess ? "Completado" : (isFailed ? "Fallido" : "En proceso"),
            AsientoContableReference = reference,
          };

          await _context.AddAsync(nuevoRegistro);
        }
        await _context.SaveChangesAsync();
        _logger.LogInformation("insertado registro con exito");
        return true;
      }
      catch (Exception ex)
      {
        _logger.LogInformation("ocurrio un error al insertar registro {ex}", ex);
        return false;
      }
    }
  }
}