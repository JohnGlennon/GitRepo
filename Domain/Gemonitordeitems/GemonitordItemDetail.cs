using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Gemonitordeitems
{
  public class GemonitordItemDetail
  {
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
