using AsientosContrablesApi.Validations;

namespace AsientosContrablesApi.Models
{
  public class JournalEntryLinesDTO
  {
    private static int _nextLineId = 0;

    public JournalEntryLinesDTO()
    {
      LineId = _nextLineId++;
    }
    public int LineId { get; }
    public string AccountCode { get; set; }
    public double Debit { get; set; }
    public double Credit { get; set; }
    public string LineMemo { get; set; }
    public string Reference1 { get; set; }

  }
}