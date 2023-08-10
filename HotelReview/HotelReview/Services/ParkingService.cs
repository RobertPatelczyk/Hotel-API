using AutoMapper;
using HotelReview.Entities;
using HotelReview.Exceptions;
using HotelReview.Models;
using HotelReview.Models.Hotels;
using HotelReview.Models.Parking;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HotelReview.Services
{
    public interface IParkingService
    {
        public void Post(CreateParkingDto dto);
        public void Put(PutParkingDto dto);
        public PageResult<ParkingDto> GetHotelsParking(ParkingQuery query);
        public void Delete(int id);
    }
    public class ParkingService : IParkingService
    {
        public ParkingService(HotelDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public HotelDbContext Context { get; }
        public IMapper Mapper { get; }

        public void Post(CreateParkingDto dto)
        {
            Hotel hotel = Context.Hotels
                .Include(x => x.Parking)
                .FirstOrDefault(x => x.Id == dto.HotelId);
            if (hotel is null )
                throw new NotFoundException("Hotel was not found.");

            Parking mappedParking = Mapper.Map<Parking>(dto);
            Context.Parkings.Add(mappedParking);
            Context.SaveChanges();
        }
        public void Put(PutParkingDto dto)
        {
            Parking parking =
                Context
                .Parkings
                .FirstOrDefault(Parking => Parking.Id == dto.Id);

            if (parking is null)
                throw new NotFoundException("Parking was not found");

            if (parking.ParkingPlace == null)
                parking.AnyPlaces = false;

            else if (parking.ParkingPlace != null && parking.AnyPlaces == false)
                parking.AnyPlaces = true;

            else if (parking.ParkingPlace == null)
                parking.AnyPlaces = false;

            parking.Id = dto.Id;
            parking.AnyPlaces = dto.AnyPlaces;
            Context.SaveChanges();
        }
        public PageResult<ParkingDto> GetHotelsParking(ParkingQuery query)
        {
            IQueryable<Parking> baseQuery = Context
                .Parkings
                .Include(x => x.ParkingPlace)
                .Where(x => x.HotelId == query.hotelId)
                .Where(x => query.sample.SearchPhrase == null || x.AnyPlaces == true);

            List<Parking> parking = baseQuery
                .Skip(query.sample.PageSize * (query.sample.PageNumber - 1))
                .Take(query.sample.PageSize)
                .ToList();

            if (parking.Count == 0)
                throw new NotFoundException("Parking doesn't exists");

            int totalItemsCount = baseQuery.Count();
            
            List<ParkingDto> mapParking = Mapper.Map<List<ParkingDto>>(parking);

            PageResult<ParkingDto> result = new PageResult<ParkingDto>(mapParking, totalItemsCount, query.sample.PageSize, query.sample.PageNumber);
            return result;
        }
        public void Delete(int id)
        {
            Parking parking =
                Context
                .Parkings
                .FirstOrDefault(x => x.Id == id);

            if (parking is null)
                throw new NotFoundException("Hotel was not found");

            Context.Parkings.Remove(parking);
            Context.SaveChanges();
        }
    }
}
