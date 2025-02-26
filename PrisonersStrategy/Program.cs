namespace PrisonersStrategy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int iterationsCount, prisonersCount;
            try
            {
                ReadInitDataFromEnv(out iterationsCount, out prisonersCount);
            }
            catch (ArgumentException exc)
            {
                Console.WriteLine(exc.Message);
                Environment.Exit(-1);
                return;
            }

            Dictionary<string, Func<int, bool>> caseCalcs = new()
            {
                {"Random",  CalcRandomCase},
                {"Strategic", CalcStrategyCase }
            };

            foreach (var caseCalc in caseCalcs)
            {
                Console.WriteLine($"\r\nCalculating {caseCalc.Key} case for {iterationsCount} iterations...");
                double successRate = CalcSuccessRate(caseCalc.Value, prisonersCount, iterationsCount);
                Console.WriteLine($"{caseCalc.Key} case success rate is {successRate * 100:N10}%.");
            }
        }

        private static void ReadInitDataFromEnv(out int iterationsCount, out int prisonersCount)
        {
            const string ITERATIONS_COUNT_ENV_VAR = "ITERATIONS_COUNT";
            const string PRISONERS_EVEN_COUNT_ENV_VAR = "PRISONERS_EVEN_COUNT";

            bool isIterationsCountValid = int.TryParse(Environment.GetEnvironmentVariable(ITERATIONS_COUNT_ENV_VAR),
                out iterationsCount);
            if (!isIterationsCountValid)
            {
                throw new ArgumentException("Iterations count environment variable isn't set!");
            }

            bool isPrisonersCountValid = int.TryParse(Environment.GetEnvironmentVariable(PRISONERS_EVEN_COUNT_ENV_VAR),
                out prisonersCount);
            if (!isPrisonersCountValid)
            {
                throw new ArgumentException("Prisoners count environment variable isn't set!");
            }
        }

        private static double CalcSuccessRate(Func<int, bool> calcMethod, int prisonersCount, int iterationsCount)
        {
            int successCount = 0;
            for (int i = 0; i < iterationsCount; i++)
            {
                bool isSuccess = calcMethod.Invoke(prisonersCount);
                if (isSuccess)
                {
                    successCount++;
                }
            }

            return (double)successCount / iterationsCount;
        }

        private static bool CalcRandomCase(int prisonersCount)
        {
            var boxes = GenerateBoxes(prisonersCount);

            for (int i = 1; i <= prisonersCount; i++)
            {
                List<int> freeNums = [.. boxes.Select(b => b.Key)];
                int attemptsRest = prisonersCount / 2;
                bool hasFindTheNumber = false;

                while(attemptsRest > 0)
                {
                    int randomIndex = Random.Shared.Next(freeNums.Count);
                    bool matches = boxes[randomIndex] == i;
                    if (matches)
                    {
                        hasFindTheNumber = true;
                        break;
                    }
                    else
                    {
                        freeNums.RemoveAt(randomIndex);
                    }

                    attemptsRest--;
                }

                if (!hasFindTheNumber)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CalcStrategyCase(int prisonersCount)
        {
            var boxes = GenerateBoxes(prisonersCount);

            for (int i = 1; i <= prisonersCount; i++)
            {
                bool hasFindTheNumber = false;
                int attemptsCount = prisonersCount / 2;
                int currentValue = boxes[i - 1];

                while (attemptsCount > 0)
                {
                    if (currentValue == i)
                    {
                        hasFindTheNumber = true;
                        break;
                    }
                    else
                    {
                        currentValue = boxes[currentValue - 1];
                    }

                    attemptsCount--;
                }

                if (!hasFindTheNumber)
                {
                    return false;
                }
            }

            return true;
        }

        private static Dictionary<int, int> GenerateBoxes(int count)
        {
            List<int> numbers = [.. Enumerable.Range(1, count)];
            Dictionary<int, int> boxes = [];

            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Shared.Next(numbers.Count);
                boxes.Add(i, numbers[randomIndex]);
                numbers.RemoveAt(randomIndex);
            }

            return boxes;
        }
    }
}
