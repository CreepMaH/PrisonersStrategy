using PrisonersStrategy.Enums;
using PrisonersStrategy.Interfaces;

namespace PrisonersStrategy.Classes
{
    internal class SuccessRateCalc (int prisonersCount, int iterationsCount)
        : ISuccessRateCalc
    {
        protected readonly int _prisonersCount = prisonersCount;
        protected readonly int _iterationsCount = iterationsCount;

        public Dictionary<Case,double> CalcRates()
        {
            double successRateRandom = CalcSuccessRate(CalcRandomCase);
            double successRateStrategy = CalcSuccessRate(CalcStrategyCase);

            return new Dictionary<Case, double>
            {
                {Case.Random, successRateRandom },
                {Case.Strategy, successRateStrategy }
            };
        }

        protected virtual double CalcSuccessRate(Func<bool> calcMethod)
        {
            int successCount = 0;
            for (int i = 0; i < _iterationsCount; i++)
            {
                bool isSuccess = calcMethod.Invoke();
                if (isSuccess)
                {
                    successCount++;
                }
            }

            return (double)successCount / _iterationsCount;
        }

        private bool CalcRandomCase()
        {
            var boxes = GenerateBoxes(_prisonersCount);

            for (int i = 1; i <= _prisonersCount; i++)
            {
                List<int> freeNums = [.. boxes.Select(b => b.Key)];
                int attemptsRest = _prisonersCount / 2;
                bool hasFindTheNumber = false;

                while (attemptsRest > 0)
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

        private bool CalcStrategyCase()
        {
            var boxes = GenerateBoxes(_prisonersCount);

            for (int i = 1; i <= _prisonersCount; i++)
            {
                bool hasFindTheNumber = false;
                int attemptsCount = _prisonersCount / 2;
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
