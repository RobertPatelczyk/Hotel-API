using AutoMapper;
using HotelReview.Entities;
using HotelReview.Models;
using HotelReview.Models.Address;
using HotelReview.Models.HotelRooms;
using HotelReview.Models.Hotels;
using HotelReview.Models.Parking;
using HotelReview.Models.ParkingPlace;

namespace HotelReview
{
    public class HotelMappingProfile : Profile
    {
        public HotelMappingProfile()
        {
            CreateMap<Hotel, HotelDto>();
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();

            CreateMap<Parking, ParkingDto>()
                .ForMember(x => x.parkingPlaces, x => x.MapFrom(x => x.ParkingPlace))
                .ForMember(x => x.AnyPlaces, x => x.MapFrom(x => x.AnyPlaces))
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id));

            //CreateMap<SampleQuery, ParkingQuery>()
            //    .ForMember(x => x.sample.PageNumber, x => x.MapFrom(x => x.PageNumber))
            //    .ForMember(x => x.sample.PageSize, x => x.MapFrom(x => x.PageSize))
            //    .ForMember(x => x.sample.SearchPhrase, x => x.MapFrom(x => x.SearchPhrase))
            //    .ForMember(x => x.sample.SortBy, x => x.MapFrom(x => x.SortBy));

            CreateMap<HotelRooms, HotelRoomsDto>();

            CreateMap<CreateHotelDto, Hotel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            CreateMap<CreateAddressDto, Address>();
            CreateMap<HotelDto, HotelRoomsDto>();
            CreateMap<HotelRooms, HotelRoomsDto>();
            CreateMap<ParkingPlace, ParkingPlaceDto>();

            CreateMap<CreateHotelRoomsDto, HotelRoomsDto>();
            CreateMap<CreateHotelRoomsDto, HotelRooms>();
            CreateMap<HotelRoomsDto,CreateHotelRoomsDto>();

            CreateMap<Parking, ParkingPlace>()
                .ForMember(x => x.Parking, x => x.MapFrom(x => x.ParkingPlace));

            CreateMap<CreateParkingDto, Parking>()
                      .ForMember(x => x.AnyPlaces, x => x.MapFrom(x => x.AnyPlaces))
                      .ForMember(x => x.HotelId, x => x.MapFrom(x => x.HotelId));

            CreateMap<CreateParkingPlaceDto, ParkingPlace>()
                .ForMember(x => x.Price, x => x.MapFrom(x => x.Price))
                .ForMember(x => x.Size, x => x.MapFrom(x => x.Size))
                .ForMember(x => x.Camera, x => x.MapFrom(x => x.Camera));

            CreateMap<HotelDto, HotelRoomsDto>();

            CreateMap<Hotel, HotelDto>()
    .ForMember(dest => dest.HotelRooms, opt => opt.MapFrom(src => src.Rooms))
    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
    .ForMember(dest => dest.Parking, opt => opt.MapFrom(src => src.Parking))
    .ForPath(dest => dest.Parking.parkingPlaces, opt => opt.MapFrom(src => src.Parking.ParkingPlace));

            CreateMap<HotelRooms, HotelRoomsDto>();

        }
    }
}