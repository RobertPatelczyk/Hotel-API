using AutoMapper;
using HotelReview.Entities;
using HotelReview.Exceptions;
using HotelReview.Models.HotelRooms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace HotelReview.Services
{
    public interface IHotelRoomService
    {
        void Put(HotelRoomsDto dto);
        public void Post(CreateHotelRoomsDto dto);
        public List<HotelRoomsDto> GetAll(int hotelId);
        public HotelRoomsDto GetById(int hotelId, int hotelRoomsId);
        public void Delete(int id);
    }

    public class HotelRoomService : IHotelRoomService
    {
        public IMapper mapper { get; }
        private readonly HotelDbContext context;
        public HotelRoomService(HotelDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void Post(CreateHotelRoomsDto dto)
        {
            Hotel hotel =
                context
                .Hotels
                .FirstOrDefault(x => x.Id == dto.HotelId);

            if (hotel == null)
                throw new NotFoundException("Hotelroom not found");

            HotelRooms mappedHotelRoom = mapper.Map<HotelRooms>(dto);
            context.HotelRooms.Add(mappedHotelRoom);
            context.SaveChanges();
        }

        public void Put(HotelRoomsDto dto)
        {
            HotelRooms rooms = context
                .HotelRooms
                .Include(x => x.Hotel)
                .FirstOrDefault(x => x.Id == dto.Id);

            if (rooms == null)
                throw new NotFoundException("Hotelroom not found");

            rooms.Id = dto.Id;
            rooms.Price = dto.Price;
            rooms.HowManyBeds = dto.HowManyBeds;
            rooms.IsAvailable = dto.IsAvailable;

            context.SaveChanges();
        }
        public List<HotelRoomsDto> GetAll(int hotelId)
        {
            Hotel hotel =
                context
                .Hotels
                .Include(x=>x.Rooms)
                .FirstOrDefault(x=>x.Id==hotelId);

            if (hotel == null)
                throw new NotFoundException("Hotelroom not found");

            List<HotelRoomsDto> mapHotelRooms = mapper.Map<List<HotelRoomsDto>>(hotel.Rooms);

            return mapHotelRooms;
        }
        public HotelRoomsDto GetById(int hotelId, int hotelRoomsId)
        {
            Hotel hotel =
                context
                .Hotels
                .FirstOrDefault(x=>x.Id == hotelId);

            if(hotel is null)
                throw new NotFoundException("Hotel has not found");

            HotelRooms hotelRoom =
                context
                .HotelRooms
                .FirstOrDefault(x => x.Id == hotelRoomsId);

            if (hotelRoom == null || hotelRoom.Id != hotelRoomsId)
                throw new NotFoundException("Hotelroom has not found");

            HotelRoomsDto mapperHotelRooms = mapper.Map<HotelRoomsDto>(hotelRoom);
            return mapperHotelRooms;
        }
        public void Delete(int id)
        {
            HotelRooms hotelRoom =
                context
                .HotelRooms
                .FirstOrDefault(x => x.Id == id);

            if (hotelRoom == null)
                throw new NotFoundException("Hotelroom not found");

            context.HotelRooms.Remove(hotelRoom);
            context.SaveChanges();
        }
    }
}
