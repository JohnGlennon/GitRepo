using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
  public enum GrafiekType
  {
    //Namen lijken vreemd, maar dit is om hard-coded strings in Chart.js te vermijden
    horizontalBar,
    bar,
    pie,
    line,
    node
  }
}
