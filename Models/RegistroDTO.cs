using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsientosContrablesApi.Models
{
  public class RegistroDTO
  {
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime LastUpdate { get; set; }
    public int NumberSap1 { get; set; }
    public int NumberSap2 { get; set; }
    public Guid ProcessId { get; set; }
    public Proceso Proceso { get; set; }
    public string Status { get; set; }
    public string? Error { get; set; }
    public Guid AsientoContableReference { get; set; }
  }
}