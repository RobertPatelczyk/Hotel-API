using HotelReview.Entities;
using HotelReview.Models;
using HotelReview.Models.Parking;
using HotelReview.Models.ParkingPlace;
using HotelReview.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HotelReview.Controllers
{
    [Route("api/hotel/parking/parkingplace")]
    [ApiController]
    public class ParkingPlaceController : ControllerBase
    {
        private readonly IParkingPlaceService parkingPlaceService;
        public ParkingPlaceController(IParkingPlaceService parkingPlaceService)
        {
            this.parkingPlaceService = parkingPlaceService;
        }
        [HttpPut]
        public ActionResult Put([FromBody]ParkingPlaceDto dto)
        {
            parkingPlaceService.Put(dto);
            return Ok(dto);
        }
        [HttpPost]
        public ActionResult Post([FromBody]CreateParkingPlaceDto dto)
        {
            parkingPlaceService.Post(dto);
            return Ok(dto);
        }
        [HttpGet("GetSingleParking/{parkingPlaceId}")]
        public ActionResult GetSingleParkingById([FromRoute] int parkingPlaceId)
        {
           var SingleParkingPlace =  parkingPlaceService.GetSingleParkingById(parkingPlaceId);
            return Ok(SingleParkingPlace);
        }
        [HttpGet("GetAllParkings")]
        public ActionResult<List<ParkingPlaceDto>> GetAllParkingPlaces([FromQuery] ParkingPlaceQuery query)
        {
            PageResult<ParkingPlaceDto> ListOfParkingPlaces =  parkingPlaceService.GetAllParkingPlaces(query);
            return Ok(ListOfParkingPlaces);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteById([FromRoute]int id)
        {
            parkingPlaceService.Delete(id);
            return Ok();
        }
    }
}
