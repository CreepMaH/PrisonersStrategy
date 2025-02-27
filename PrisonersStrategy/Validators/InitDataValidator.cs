using FluentValidation;
using PrisonersStrategy.Dto;

namespace PrisonersStrategy.Validators
{
    internal class InitDataDtoValidator : AbstractValidator<InitDataDto>
    {
        public InitDataDtoValidator()
        {
            RuleFor(d => d.IterationsCount_Int)
                .NotEmpty()
                .Must((x) => int.TryParse(x, out _));

            RuleFor(d => d.PrisonersCount_Even_Int)
                .NotEmpty()
                .Must((x) => int.TryParse(x, out _))
                .Must((x) => int.Parse(x) % 2 == 0);

            RuleFor(d => d.ShowBenchmark_Bool)
                .NotEmpty()
                .Must((x) => bool.TryParse(x, out _));
        }
    }
}
