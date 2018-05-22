using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Gemonitordeitems;

namespace Domain.Dashboards
{
 public class Statistiek
  {
    public int StatistiekId { get; set; }

    public GemonitordItem GemonitordItem { get; set; }

    //getal, getalTrend, top5, top10
    public string SoortStatistiek { get; set; }
  }
}
