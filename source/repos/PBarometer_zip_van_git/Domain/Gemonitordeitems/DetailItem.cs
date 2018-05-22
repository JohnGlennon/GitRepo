using Domain.Deelplatformen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Gemonitordeitems
{
  public class DetailItem
  {
    public DetailItem()
    {

    }
    //BerichtDatum: Datum van het bericht waarop het DetailItem gebsaseerd is.
    public DateTime BerichtDatum { get; set; }
    //DetailItemId: Het Id van het DetailItem.
    public int DetailItemId { get; set; }
    public double Polariteit { get; set; }
    public double Objectiviteit { get; set; }
    public int DeelplatformId { get; set; }
    public Deelplatform Deelplatform { get; set; }
    public List<GemonitordItem> GemonitordeItems { get; set; }
    [NotMapped]
    public List<string> Themas
    {
      get
      {
        return ThemasJSON == null ? null :
        JsonConvert.DeserializeObject<List<string>>(ThemasJSON);
      }
      set
      {
        ThemasJSON = JsonConvert.SerializeObject(value);
      }
    }

    [NotMapped]
    public Dictionary<string, List<string>> AndereEigenschappen
    {
      get
      {
        return AndereEigenschappenJSON == null ? null :
        JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(AndereEigenschappenJSON);
      }
      set
      {
        AndereEigenschappenJSON = JsonConvert.SerializeObject(value);
      }
    }
    [NotMapped]
    public Dictionary<string, string> ProfielEigenschappen
    {
      get
      {
        return ProfielEigenschappenJSON == null ? null :
        JsonConvert.DeserializeObject<Dictionary<string, string>>(ProfielEigenschappenJSON);
      }
      set
      {
        ProfielEigenschappenJSON = JsonConvert.SerializeObject(value);
      }
    }
    //Dictionary wordt als JSON string opgeslagen in DB
    public string AndereEigenschappenJSON { get; set; }
    public string ProfielEigenschappenJSON { get; set; }
    public string ThemasJSON { get; set; }
  }
}
