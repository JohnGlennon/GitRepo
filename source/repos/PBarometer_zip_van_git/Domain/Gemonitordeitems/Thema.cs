using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Gemonitordeitems
{
  public class Thema : GemonitordItem
  {
    public Thema()
    {

    }

    [NotMapped]
    public List<string> KernWoorden
    {
      get
      {
        return KernWoordenJSON == null ? null :
        JsonConvert.DeserializeObject<List<string>>(KernWoordenJSON);
      }
      set
      {
        KernWoordenJSON = JsonConvert.SerializeObject(value);
      }
    }

    public string KernWoordenJSON { get; set; }

  }
}
