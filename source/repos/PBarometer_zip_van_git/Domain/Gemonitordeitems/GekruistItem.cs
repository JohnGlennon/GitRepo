using System.Linq;

namespace Domain.Gemonitordeitems
{
  public class GekruistItem : GemonitordItem
  {
    public GemonitordItem Item1 { get; set; }
    public GemonitordItem Item2 { get; set; }


    public override void BerekenEigenschappen()
    {
      DetailItems = Item1.DetailItems.Intersect(Item2.DetailItems).ToList();
      base.BerekenEigenschappen();
    }
  }
}