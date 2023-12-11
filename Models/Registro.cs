using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsientosContrablesApi.Models
{
  public class Registro
  {
    [Key]
    public Guid RegistroId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime LastUpdate { get; set; }
    public int NumberSap1 { get; set; }
    public int NumberSap2 { get; set; }
    public Guid ProcessId { get; set; }
    [ForeignKey("ProcessId")]
    public Proceso Proceso { get; set; }

    [MaxLength(15)]
    public string Status { get; set; }
    [MaxLength(30)]
    public string? Error { get; set; }
    [ForeignKey("AsientoContable")]
    public Guid AsientoContableReference { get; set; }
    public AsientoContable AsientoContable { get; set; }
  }
}