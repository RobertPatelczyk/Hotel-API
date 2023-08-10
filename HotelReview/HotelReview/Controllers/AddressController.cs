using HotelReview.Entities;
using HotelReview.Models.Address;
using HotelReview.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelReview.Controllers
{
    [Route("api/hotel/address")]
    [ApiController]
    public class AddressController :ControllerBase
    {
        private readonly IAddressService service;

        public AddressController(IAddressService service)
        {
            this.service = service;
        }
        [HttpPost]
        public ActionResult Post([FromBody] CreateAddressDto dto)
        {
            service.Post(dto);
            return Ok();
        }

        [HttpPut]
        public ActionResult Put([FromBody] AddressDto dto)
        {
            service.Put(dto);
            return Ok();
        }
        [HttpGet("{hotelId}")]
        public ActionResult GetAddress(int hotelId)
        {
           var hotelAddress= service.GetAddress(hotelId);
            return Ok(hotelAddress);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            service.Delete(id); 
            return Ok();
        }
    }
}
