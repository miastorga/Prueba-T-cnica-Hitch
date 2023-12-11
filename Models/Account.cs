using System.ComponentModel.DataAnnotations;

namespace AsientosContrablesApi.Models
{
  public class Account
  {
    [Key]
    public string Code { get; set; }
    public string Name { get; set; }
  }
}