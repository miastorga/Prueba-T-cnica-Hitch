using AsientosContrablesApi.Validations;

namespace AsientosContrablesApi.Models
{
  public class AsientoContableDTO
  {
    public string Reference { get; set; }

    public DateTime ReferenceDate { get; set; }
    public string Memo { get; set; }
    public DateTime TaxDate { get; set; }
    public DateTime DueDate { get; set; }
    public List<JournalEntryLinesDTO> JournalEntryLines { get; set; }

  }
}
