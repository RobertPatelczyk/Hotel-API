using AutoMapper;
using HotelReview.Authorization.Age;
using HotelReview.Entities;
using HotelReview.Models;
using HotelReview.Models.Hotels;
using HotelReview.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReview.Controllers
{
    [Route("api/hotel")]
    [ApiController]
    [Authorize]
    public class HotelController :ControllerBase
    {
        private readonly IHotelService hotelService;

        public HotelController(IHotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        [HttpGet]
        public ActionResult<List<HotelDto>> GetHotelsDto([FromQuery] SampleQuery query)
        {
            PageResult<HotelDto> result = hotelService.GetAllHotels(query);
            return Ok(result);
        }

        [HttpGet("GetHotelById/{id}")]
        [Authorize(Policy = "AtLeast18")]
        public ActionResult<IEnumerable<HotelDto>> GetByIdDto([FromRoute]int id)
        {
            HotelDto result = hotelService.GetHotelById(id);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = ("Admin, Manager"))]
        [Authorize(Policy = "DateOfBirth")]
        public ActionResult PostHotel([FromBody] CreateHotelDto createHotel)
        {
            int result = hotelService.PostHotel(createHotel);
            return Created($"/api/{result}", null);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute]int id)
        {
            hotelService.Delete(id);
            return Ok();
        }
        [HttpPut("put/{id}")]
        [AllowAnonymous]
        public ActionResult Put([FromBody]UpdateHotelDto dto, [FromRoute]int id)
        {
           hotelService.Update(dto, id);
           return Ok();  
        }
    }
}