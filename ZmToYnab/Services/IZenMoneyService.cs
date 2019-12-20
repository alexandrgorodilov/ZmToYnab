using ZmToYnab.Models.ZM;

namespace ZmToYnab.Services
{
    public interface IZenMoneyService
    {
        ZmDiff GetDiff();
    }
}