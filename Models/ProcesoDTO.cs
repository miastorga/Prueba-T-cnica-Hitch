
using System.ComponentModel.DataAnnotations;

namespace AsientosContrablesApi.Models
{
  public class ProcesoDTO
  {
    public Guid ProcessId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime LastUpdate { get; set; }
    public int Items { get; set; }

    [MaxLength(15)]
    public string Status { get; set; }

    [Range(1, 10)]
    public int Failed { get; set; }
    public int Success { get; set; }
  }
}