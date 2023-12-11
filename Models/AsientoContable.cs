using System.ComponentModel.DataAnnotations;

namespace AsientosContrablesApi.Models
{
  public class AsientoContable
  {
    [Key]
    public Guid Reference { get; set; }
    public DateTime ReferenceDate { get; set; }
    public string Memo { get; set; }
    public DateTime TaxDate { get; set; }
    public DateTime DueDate { get; set; }
    public List<JournalEntryLines> JournalEntryLines { get; set; }
    public List<Registro> Registros { get; set; }

  }
}
