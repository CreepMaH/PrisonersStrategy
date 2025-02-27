using PrisonersStrategy.Enums;

namespace PrisonersStrategy.Interfaces
{
    interface ISuccessRateCalc
    {
        Dictionary<Case, double> CalcRates();
    }
}
