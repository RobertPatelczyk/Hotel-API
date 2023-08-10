using FluentValidation;
using HotelReview.Models.Hotels;
using HotelReview.Models.ParkingPlace;
using System.Linq;

namespace HotelReview.Models.Validators
{
    public class ParkingPlaceValidator :AbstractValidator<ParkingPlaceQuery>
    {
        private string[] allowedSortByColumnNames =
        {
            nameof(ParkingPlaceDto.Size),
            nameof(ParkingPlaceDto.Price)
        };
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        public ParkingPlaceValidator()
        {
            RuleFor(x => x.sample.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.sample.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must in[{string.Join(",", allowedPageSizes)}]");
                }
            });
            RuleFor(x => x.sample.SortBy).Must(
                value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]"
                );
        }
    }
}
