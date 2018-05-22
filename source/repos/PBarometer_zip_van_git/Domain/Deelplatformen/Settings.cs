namespace Domain.Deelplatformen
{
  public class Settings
  {
    public bool OverzichtAdded { get; set; }
    public bool WeeklyReviewAdded { get; set; }
    public bool AlertsAdded { get; set; }

    public Settings(bool OverzichtAdded, bool WeeklyReviewAdded, bool AlertsAdded)
    {
      this.OverzichtAdded = OverzichtAdded;
      this.WeeklyReviewAdded = WeeklyReviewAdded;
      this.AlertsAdded = AlertsAdded;
    }
  }
}
