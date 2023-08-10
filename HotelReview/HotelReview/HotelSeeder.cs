using HotelReview.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HotelReview
{
    public class HotelSeeder
    {
        private readonly HotelDbContext dbContext;

        public HotelSeeder(HotelDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Seed()
        {
            if (dbContext.Database.CanConnect())
            {
                if (!dbContext.Hotels.Any())
                {
                    Hotel hotel = SeedHotel();
                    dbContext.Hotels.AddRange(hotel);
                    dbContext.SaveChanges();
                }
                if (!dbContext.Roles.Any())
                {
                    List<Role> roles = SeedRole();
                    dbContext.Roles.AddRange(roles);
                    dbContext.SaveChanges();
                }
            }
        }
        public List<Role> SeedRole()
        {
            List<Role> roles = new List<Role>()
            {
              new Role()
              {
                  Name="User"
              },
              new Role()
              {
                  Name="Manager"
              },
              new Role()
              {
                  Name="Admin"
              },
            };
            return roles;
        }
        public IEnumerable<ParkingPlace> SeedParkingPlaces(int price, string size, bool camera)
        {
            var places = new List<ParkingPlace>()
            {
                new ParkingPlace()
                {
                        Price = price,
                        Size =size,
                        Camera = camera
                }
            };
            return places;
        }

        public Parking SeedParking(int price, string size, bool camera, bool anyPlaces)
        {

            Parking parking = new Parking();
            parking.AnyPlaces = anyPlaces;

            if (parking.AnyPlaces != false)
            {
                SeedParkingPlaces(price, size, camera);
            }

            Parking parking2 = new Parking()
            {
                AnyPlaces = anyPlaces,
            };
            return parking;
        }

        public Address SeedAddress(string postalCode, string city, string street)
        {
            Address address = new Address()
            {
                PostalCode = postalCode,
                City = city,
                Street = street,
            };
            return address;
        }

        public List<HotelRooms> SeedHotelRooms(int howManyBeds, bool isAvailable, int price)
        {
            List<HotelRooms> rooms = new List<HotelRooms>()
            {
                new HotelRooms()
                {
                      HowManyBeds =howManyBeds,
                      IsAvailable = isAvailable,
                      Price =price
                }
            };
            return rooms;
        }

        public Hotel SeedHotel()
        {
            Hotel hotel = new Hotel()
            {
                Name = "Chuj",
                HasGym = true,
                ReceptionNumber = 518264757,
                EmailContact = "FirstHotelContact",
                Address = SeedAddress("84-200","Wejrowo","kaszebszcziUlica"),
                Rooms = SeedHotelRooms(2, true, 50),
                Parking = SeedParking(20, "5m", true, true),
            };
            return hotel;
        }

    }
}
