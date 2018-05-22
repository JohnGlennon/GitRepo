using Domain.Deelplatformen;

namespace MVC.Controllers.Api
{
    public interface IDataController
    {
        void HaalBerichtenOp(Deelplatform deelplatform);
    }
}