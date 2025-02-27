using PrisonersStrategy.Dto;
using PrisonersStrategy.Interfaces;
using PrisonersStrategy.Models;
using PrisonersStrategy.Validators;

namespace PrisonersStrategy.Classes
{
    internal class EnvInitDataReader : IInitDataReader
    {
        const string ITERATIONS_COUNT_ENV_VAR = "ITERATIONS_COUNT";
        const string PRISONERS_EVEN_COUNT_ENV_VAR = "PRISONERS_EVEN_COUNT";
        const string SHOW_BENCHMARK_ENV_VAR = "IS_BENCHMARK_MODE";

        private readonly InitDataDtoValidator _validator = new();

        public InitData Read()
        {
            var envVars = Environment.GetEnvironmentVariables();

            InitDataDto initDataDto = new()
            {
                IterationsCount_Int = envVars.Contains(ITERATIONS_COUNT_ENV_VAR) 
                    ? envVars[ITERATIONS_COUNT_ENV_VAR]!.ToString()!
                    : throw new ArgumentException($"An {ITERATIONS_COUNT_ENV_VAR} environment variable isn't set!"),
                PrisonersCount_Even_Int = envVars.Contains(PRISONERS_EVEN_COUNT_ENV_VAR) 
                    ? envVars[PRISONERS_EVEN_COUNT_ENV_VAR]!.ToString()!
                    : throw new ArgumentException($"An {PRISONERS_EVEN_COUNT_ENV_VAR} environment variable isn't set!"),
                ShowBenchmark_Bool = envVars.Contains(SHOW_BENCHMARK_ENV_VAR) 
                    ? envVars[SHOW_BENCHMARK_ENV_VAR]!.ToString()!
                    : throw new ArgumentException($"An {SHOW_BENCHMARK_ENV_VAR} environment variable isn't set!")
            };

            var validationResult = _validator.Validate(initDataDto);
            if (!validationResult.IsValid)
            {
                string errorMsg = "ValidationErrors: " + string.Join(", ", validationResult.Errors);
                throw new ArgumentOutOfRangeException("", errorMsg);
            }

            return new InitData()
            {
                IterationsCount = Convert.ToInt32(initDataDto.IterationsCount_Int),
                PrisonersCount = Convert.ToInt32(initDataDto.PrisonersCount_Even_Int),
                ShowBenchmark = Convert.ToBoolean(initDataDto.ShowBenchmark_Bool)
            };
        }
    }
}
