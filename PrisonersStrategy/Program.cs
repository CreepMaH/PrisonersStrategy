using PrisonersStrategy.Classes;
using PrisonersStrategy.Enums;
using PrisonersStrategy.Interfaces;
using PrisonersStrategy.Models;
using System.Diagnostics;

namespace PrisonersStrategy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InitData initData;
            try
            {
                IInitDataReader initDataReader = new EnvInitDataReader();
                initData = initDataReader.Read();

                Console.WriteLine($"Initial data: {initData.PrisonersCount} prisoners, {initData.IterationsCount} iterations. " +
                    $"Is benchmark mode: {initData.ShowBenchmark}.");
            }
            catch (ArgumentException exc)
            {
                Console.WriteLine(exc.Message);
                Environment.Exit(-1);
                return;
            }

            List<ISuccessRateCalc> methodsToRun = 
            [
                new SuccessRateCalc_Multithreaded(initData.PrisonersCount, initData.IterationsCount)
            ];
            if (initData.ShowBenchmark)
            {
                methodsToRun.Add(new SuccessRateCalc(initData.PrisonersCount, initData.IterationsCount));
            }
            RunCalc(methodsToRun);
        }

        private static void RunCalc(List<ISuccessRateCalc> methodsToRun)
        {
            foreach (var calcMethod in methodsToRun)
            {
                Stopwatch stopwatch = new();

                stopwatch.Start();
                CalcRates(calcMethod);
                stopwatch.Stop();
                Console.WriteLine($"Time elapsed: {stopwatch.ElapsedMilliseconds} ms.");
            }
        }

        private static void CalcRates(ISuccessRateCalc rateCalc)
        {
            Console.WriteLine($"Calculating with type {rateCalc.GetType()}...");
            var successRates = rateCalc.CalcRates();
            Console.WriteLine($"Random method: {successRates[Case.Random] * 100:N10}%, " +
                $"strategy method: {successRates[Case.Strategy] * 100:N10}%");
        }

    }
}
