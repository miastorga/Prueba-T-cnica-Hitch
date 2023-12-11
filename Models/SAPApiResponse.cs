namespace AsientosContrablesApi.Models
{
  public class SAPApiResponse
  {
    public DataModel Data { get; set; }
    public string Message { get; set; }
  }

  public class DataModel
  {
    public int DocEntry { get; set; }
    public int DocNum { get; set; }
  }
}