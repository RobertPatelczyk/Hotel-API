using FluentValidation;
using HotelReview.Models.Hotels;
using System.Linq;

namespace HotelReview.Models.Validators
{
    public class HotelQueryValidator : AbstractValidator<SampleQuery>
    {
        private string[] allowedSortByColumnNames =
        {
            nameof(HotelDto.Name), nameof(HotelDto.EmailContact)
        };
        private int[] allowedPageSizes = new[] { 5,10,15};
        public HotelQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).Custom((value, context) =>
                {
                    if(!allowedPageSizes.Contains(value))
                    {
                        context.AddFailure("PageSize", $"PageSize must in[{string.Join(",", allowedPageSizes)}]");
                    }
                });
            RuleFor(x => x.SortBy).Must(
                value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]"
                );
        }
    }
}
