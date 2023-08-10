using AutoMapper;
using HotelReview.Entities;
using HotelReview.Exceptions;
using HotelReview.Models.Address;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Security.Principal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HotelReview.Services
{
    public interface IAddressService
    {
        void Post(CreateAddressDto dto);
        void Put(AddressDto dto);
        public AddressDto GetAddress(int hotelId);
        public void Delete(int id);
    }

    public class AddressService : IAddressService
    {
        private readonly HotelDbContext context;
        private readonly IMapper mapper;

        public AddressService(HotelDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void Post(CreateAddressDto dto)
        {
            Hotel hotel =
                 context
                .Hotels
                .Include(HOTEL => HOTEL.Address)
                .FirstOrDefault(HOTEL => HOTEL.Id == dto.HotelId);

            if (hotel == null)
                throw new NotFoundException("Hotel not found");

            var xx = mapper.Map<Address>(dto);
            context.Addresses.Add(xx);
            context.SaveChanges();
        }
        public void Put(AddressDto dto)
        {
            Address address =
                context
                .Addresses
                .FirstOrDefault(x => x.Id == dto.Id);

            if (address is null)
                throw new NotFoundException("Address was not found");

            address.Id = dto.Id;
            address.Street = dto.Street;
            address.City = dto.City;
            address.PostalCode = dto.PostalCode;

            context.SaveChanges();
        }

        public AddressDto GetAddress(int hotelId)
        {
            Hotel hotel = context
                .Hotels
                .Include(x => x.Address)
                .FirstOrDefault(x => x.Id == hotelId);

            if (hotel is null)
                throw new NotFoundException("Hotel was not found");

            AddressDto mappedAddress = mapper.Map<AddressDto>(hotel.Address);

            return mappedAddress;
        }
        public void Delete(int id)
        {
            Address address =
                context
            .Addresses
                .FirstOrDefault(x => x.Id == id);

            if (address is null)
                throw new NotFoundException("Address was not found");

            context.Addresses.Remove(address);
            context.SaveChanges();
        }
    }
}
