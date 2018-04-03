using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Gemonitordeitems
{
  public class GemonitordItemDetail
  {
    [Key]
    public int DetailId { get; set; }

    public List<string> Plaatsnamen { get; set; }
    public List<string> VerbondenWoorden { get; set; }
    public List<string> Urls { get; set; }
    public List<string> Hashtags { get; set; }

    public GemonitordItemDetail()
    {
      Plaatsnamen = new List<string>();
      VerbondenWoorden = new List<string>();
      Urls = new List<string>();
      Hashtags = new List<string>();
    }
  }
}
