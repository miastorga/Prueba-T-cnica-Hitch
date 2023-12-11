using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsientosContrablesApi.Models
{
  public class JournalEntryLines
  {
    public int LineId { get; set; }
    public string AccountCode { get; set; }
    public double Debit { get; set; }
    public double Credit { get; set; }
    public string LineMemo { get; set; }
    [Key]
    public Guid Reference1 { get; set; }

    [ForeignKey("AsientoContable")]
    public Guid AsientoContableId { get; set; }
    public AsientoContable AsientoContable { get; set; }

    public double Total => Debit - Credit;
  }
}