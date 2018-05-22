using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models.Android
{
  public class GrafiekDTO
  {
    public int GrafiekId { get; set; }

    public string Titel { get; set; }

    public bool ToonLegende { get; set; }
    public bool ToonXAs { get; set; }
    public bool ToonYAs { get; set; }

    public bool XOorsprongNul { get; set; }
    public bool YOorsprongNul { get; set; }

    public string XTitel { get; set; }
    public string YTitel { get; set; }
    public bool XOnder { get; set; }

    public List<string> LegendeLijst { get; set; }
    public List<string> XLabels { get; set; }
    public List<List<double>> Data { get; set; }
    public int Periode { get; set; }
    public string Type { get; set; }
    public string Keuze { get; set; }

    public GrafiekDTO()
    {
      LegendeLijst = new List<string>();
      XLabels = new List<string>();
      Data = new List<List<double>>();
    }
  }
}