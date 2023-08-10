using AutoMapper;
using HotelReview.Entities;
using HotelReview.Exceptions;
using HotelReview.Models;
using HotelReview.Models.Parking;
using HotelReview.Models.ParkingPlace;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HotelReview.Services
{
    public interface IParkingPlaceService
    {
        public void Put(ParkingPlaceDto dto);
        public void Post(CreateParkingPlaceDto dto);
        public PageResult<ParkingPlaceDto> GetAllParkingPlaces(ParkingPlaceQuery query);
        public ParkingPlaceDto GetSingleParkingById(int parkingPlaceId);
        public void Delete(int id);
    }
    public class ParkingPlaceService : IParkingPlaceService
    {
        private readonly HotelDbContext dbContext;
        private readonly IMapper mapper;

        public ParkingPlaceService(HotelDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public void Post(CreateParkingPlaceDto dto)
        {
            Parking parking =
                dbContext
                .Parkings
                .Include(x => x.ParkingPlace)
                .FirstOrDefault(x => x.Id == dto.ParkingId);

            if (parking is null)
                throw new NotFoundException("Parking was not found.");

            var mappedParking = mapper.Map<ParkingPlace>(dto);

            dbContext.ParkingPlace.Add(mappedParking);
            dbContext.SaveChanges();
        }
        public void Put(ParkingPlaceDto dto)
        {
            ParkingPlace parkingPlace =
              dbContext
             .ParkingPlace
             .FirstOrDefault(ParkingPlace => ParkingPlace.Id == dto.Id);

            if (parkingPlace is null)
                throw new NotFoundException("Parking place was not found");

            parkingPlace.Id = dto.Id;
            parkingPlace.Price = dto.Price;
            parkingPlace.Size = dto.Size;
            parkingPlace.Camera = dto.Camera;

            dbContext.SaveChanges();
        }
        public ParkingPlaceDto GetSingleParkingById(int parkingPlaceId)
        {
            ParkingPlace parkingPlace =
            dbContext
            .ParkingPlace
            .FirstOrDefault(x => x.Id == parkingPlaceId);

            if (parkingPlace is null)
                throw new NotFoundException("Parking was not found");

            var mappedParkingPlace = mapper.Map<ParkingPlaceDto>(parkingPlace);
            return mappedParkingPlace;
        }
        public PageResult<ParkingPlaceDto> GetAllParkingPlaces(ParkingPlaceQuery query)
        {
            IQueryable<ParkingPlace> baseQuery =
                dbContext
                .ParkingPlace
                .Where(x => x.Parking.Id == query.ParkingId);

            if (!string.IsNullOrEmpty(query.sample.SortBy))
            {
                Dictionary<string, Expression<Func<ParkingPlace, object>>> columnSelectors = new Dictionary<string, Expression<Func<ParkingPlace, object>>>
                    {
                        {nameof(ParkingPlace.Price), x=>x.Price },
                        {nameof(ParkingPlace.Size), x=>x.Size }
                    };

                Expression<Func<ParkingPlace, object>> selectColumn = columnSelectors[query.sample.SortBy];
                baseQuery = query.sample.SortDirection == SortDirection.ASC ? baseQuery.OrderBy(selectColumn) : baseQuery.OrderByDescending(selectColumn);
            }

            if (baseQuery is null)
                throw new NotFoundException("Parking place was not found");

            List < ParkingPlace> parking =
                baseQuery
                  .Skip(query.sample.PageSize * (query.sample.PageNumber - 1))
                 .Take(query.sample.PageSize)
                 .ToList();

            if (parking.Count == 0)
                throw new NotFoundException("Parking doesn't exist");

            int totalItemsCount = baseQuery.Count();

            List<ParkingPlaceDto> mappedParking = mapper.Map<List<ParkingPlaceDto>>(parking);

            PageResult<ParkingPlaceDto> result = new PageResult<ParkingPlaceDto>(mappedParking, totalItemsCount, query.sample.PageSize, query.sample.PageNumber);
            return result;

        }

        //public PageResult<ParkingDto> GetAllParkingPlaces(ParkingQuery query)
        //{
        //    IQueryable<Parking> baseQuery =
        //        dbContext
        //        .Parkings
        //        .Include(x => x.ParkingPlace)
        //        .Where(x => x.Id == query.ParkingId);
        //    // .Where(x => query.sample.SearchPhrase == null);


        //    if (!string.IsNullOrEmpty(query.sample.SortBy))
        //    {
        //        Dictionary<string, Expression<Func<Parking, object>>> columnSelectors = new Dictionary<string, Expression<Func<Parking, object>>>
        //        {
        //            {nameof(Parking.ParkingPlace), x=>x.ParkingPlace },
        //            {nameof(Parking.Hotel), x=>x.Hotel }
        //        };

        //        Expression<Func<Parking, object>> selectColumn = columnSelectors[query.sample.SortBy];
        //        baseQuery = query.sample.SortDirection == SortDirection.ASC ? baseQuery.OrderBy(selectColumn) : baseQuery.OrderByDescending(selectColumn);
        //    }


        //    if (baseQuery is null)
        //        throw new NotFoundException("Parking place was not found");

        //    List<Parking> parking =
        //        baseQuery
        //          .Skip(query.sample.PageSize * (query.sample.PageNumber - 1))
        //         .Take(query.sample.PageSize)
        //         .ToList();

        //    if (parking.Count == 0)
        //        throw new NotFoundException("Parking doesn't exist");

        //    int totalItemsCount = baseQuery.Count();

        //    List<ParkingDto> mappedParking = mapper.Map<List<ParkingDto>>(parking);

        //    PageResult<ParkingDto> result = new PageResult<ParkingDto>(mappedParking, totalItemsCount, query.sample.PageSize, query.sample.PageNumber);
        //    return result;
        //}
        public void Delete(int id)
        {
            ParkingPlace parkingPlace =
                dbContext
                .ParkingPlace
                .FirstOrDefault(x => x.Id == id);

            if (parkingPlace is null)
                throw new NotFoundException("Parking was not found");

            dbContext.ParkingPlace.Remove(parkingPlace);
            dbContext.SaveChanges();
        }
    }
}
