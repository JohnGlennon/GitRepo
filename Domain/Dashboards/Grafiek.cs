using Domain.Gemonitordeitems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
  public class Grafiek
  {
    public int GrafiekId { get; set; }

    //Foreign keys
    public Dashboard Dashboard { get; set; }
    public int DashboardId { get; set; }
    public List<GemonitordItem> Items { get; set; }

    public Grafiek()
    {
      Items = new List<GemonitordItem>();
    }
  }
}
