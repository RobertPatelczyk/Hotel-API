using HotelReview.Models;
using HotelReview.Models.Parking;
using HotelReview.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReview.Controllers
{
    [Route("api/hotel/parking")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingService parkingService;

        public ParkingController(IParkingService parkingService)
        {
            this.parkingService = parkingService;
        }
        [HttpPost]
        public ActionResult Post(CreateParkingDto dto)
        {
            parkingService.Post(dto);
            return Ok();
        }

        [HttpPut]
        public ActionResult Put([FromBody] PutParkingDto dto)
        {
            parkingService.Put(dto);
            return Ok();
        }

        [HttpGet()]
        public ActionResult<List<ParkingDto>> GetAddress([FromQuery]ParkingQuery query)
        {
            PageResult<ParkingDto> parking = parkingService.GetHotelsParking(query);
            return Ok(parking);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            parkingService.Delete(id);
            return Ok();
        }
    }
}
