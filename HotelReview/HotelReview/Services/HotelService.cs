using AutoMapper;
using HotelReview.Authorization.ResourceOperation;
using HotelReview.Entities;
using HotelReview.Exceptions;
using HotelReview.Models;
using HotelReview.Models.Hotels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HotelReview.Services
{
    public interface IHotelService
    {
        PageResult<HotelDto> GetAllHotels(SampleQuery query);
        HotelDto GetHotelById(int id);
        int PostHotel(CreateHotelDto dto);
        void Delete(int id);
        void Update(UpdateHotelDto dto, int id);
    }

    public class HotelService : IHotelService
    {
        readonly HotelDbContext dbContext;
        private readonly ILogger<HotelService> logger;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserContextService userContextService;

        public IMapper mapper { get; }

        public HotelService(HotelDbContext dbContext, IMapper mapper, ILogger<HotelService> logger,
            IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
            this.authorizationService = authorizationService;
            this.userContextService = userContextService;
        }
        public PageResult<HotelDto> GetAllHotels(SampleQuery query)
        {
            var baseQuery = dbContext
                .Hotels
              .Include(x => x.Address)
              .Include(x => x.Parking)
              .Include(x => x.Parking.ParkingPlace)
              .Include(x => x.Rooms)
              .Where(x => query.SearchPhrase == null ||
              (x.Name.ToLower().Contains(query.SearchPhrase.ToLower())
              || x.EmailContact.ToLower().Contains(query.SearchPhrase.ToLower())));

            if(!string.IsNullOrEmpty(query.SortBy))
            {
                Dictionary<string, Expression<Func<Hotel, object>>> columnSelectors = new Dictionary<string, Expression<Func<Hotel, object>>>
                {
                    {nameof(Hotel.Name), x=>x.Name },
                    {nameof(Hotel.EmailContact), x=>x.EmailContact }
                };

                Expression<Func<Hotel, object>> selectColumn = columnSelectors[query.SortBy];
                baseQuery = query.SortDirection == SortDirection.ASC ? baseQuery.OrderBy(selectColumn) : baseQuery.OrderByDescending(selectColumn);
            }
            
            List<Hotel> hotels =
               baseQuery
                .Skip(query.PageSize * (query.PageNumber-1))
                .Take(query.PageSize)
              .ToList();

            var totalItemsCount = baseQuery.Count();

            List<HotelDto> mapHotels = mapper.Map<List<HotelDto>>(hotels);
            PageResult<HotelDto> result = new PageResult<HotelDto>(mapHotels, totalItemsCount, query.PageSize, query.PageNumber);
            return result;
        }
        public HotelDto GetHotelById(int id)
        {
            Hotel hotel =
                 dbContext
                .Hotels
                .Include(x => x.Address)
                .Include(x => x.Parking)
                .Include(x => x.Parking.ParkingPlace)
                .Include(x => x.Rooms)
                .FirstOrDefault(x => x.Id == id);

            if (hotel is null) throw new NotFoundException("Restaurant not found");

            HotelDto mapHotel = mapper.Map<HotelDto>(hotel);
            return mapHotel;
        }

        public int PostHotel(CreateHotelDto dto)
        {

            Hotel hotel = mapper.Map<Hotel>(dto);
            hotel.CreatedById = userContextService.GetUserId;
            dbContext.Hotels.Add(hotel);
            dbContext.SaveChanges();
            return hotel.Id;

        }
        public void Delete(int id)
        {
            logger.LogError($"Tried delete: {id} hotel");
            Hotel hotel = dbContext.Hotels.FirstOrDefault(x => x.Id == id);
            if (hotel is null) throw new NotFoundException("Restaurant not found");

            AuthorizationResult authorizationResult =
                authorizationService.AuthorizeAsync
                (userContextService.User, hotel, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            dbContext.Hotels.Remove(hotel);
            dbContext.SaveChanges();
        }
        public void Update(UpdateHotelDto dto, int id)
        {
            Hotel hotel = dbContext.Hotels.FirstOrDefault(x => x.Id == id);
            if (hotel is null)
                throw new NotFoundException("Restaurant not found");


            var authorizationResult =
                  authorizationService.AuthorizeAsync
                  (userContextService.User, hotel, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            hotel.ReceptionNumber = dto.ReceptionNumber;
            hotel.EmailContact = dto.EmailContact;
            hotel.HasGym = dto.HasGym;

            dbContext.SaveChanges();
        }
    }
}
