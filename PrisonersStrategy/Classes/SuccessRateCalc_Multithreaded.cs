namespace PrisonersStrategy.Classes
{
    internal class SuccessRateCalc_Multithreaded(int prisonersCount, int iterationsCount) 
        : SuccessRateCalc(prisonersCount, iterationsCount)
    {
        protected override double CalcSuccessRate(Func<bool> calcMethod)
        {
            object lockObject = new();
            int successCount = 0;

            Parallel.For(0, _iterationsCount, i =>
            {
                bool isSuccess = calcMethod.Invoke();
                if (isSuccess)
                {
                    lock (lockObject)
                    {
                        successCount++;
                    }
                }
            });

            return (double)successCount / _iterationsCount;
        }
    }
}
