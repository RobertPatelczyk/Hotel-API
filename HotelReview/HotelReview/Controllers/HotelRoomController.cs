using HotelReview.Entities;
using HotelReview.Models.HotelRooms;
using HotelReview.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HotelReview.Controllers
{
    [Route("api/hotel/rooms")]
    [ApiController]
    public class HotelRoomController : ControllerBase
    {
        private readonly IHotelRoomService service;

        public HotelRoomController(IHotelRoomService service)
        {
            this.service = service;
        }
        [HttpPut]
        public ActionResult Put([FromBody] HotelRoomsDto dto)
        {
            service.Put(dto);
            return Ok();
        }
        [HttpPost]
        public ActionResult Post([FromBody] CreateHotelRoomsDto dto)
        {
            service.Post(dto);
            return Ok();
        }
        [HttpGet("{hotelId}")]
        public ActionResult<List<HotelRoomsDto>> GetAll([FromRoute] int hotelId)
        {
            List<HotelRoomsDto> result = service.GetAll(hotelId);
            return Ok(result);
        }
        [HttpGet("{hotelId}/{hotelRoomsId}")]
        public ActionResult GetById([FromRoute]int hotelId, [FromRoute] int hotelRoomsId)
        {
            HotelRoomsDto result = service.GetById(hotelId, hotelRoomsId);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            service.Delete(id);
            return Ok();
        }
    }
}
