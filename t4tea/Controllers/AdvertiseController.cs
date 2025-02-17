using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using t4tea.service.category.Dtos;
using t4tea.service.category;
using t4tea.service.advertise;
using t4tea.service.advertise.Dtos;
using AutoMapper;
using t4tea.data.Entities;

namespace t4tea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertiseController : ControllerBase
    {

            private readonly IAdvertiseServices _advertiseServices;

            public AdvertiseController(IAdvertiseServices advertiseServices)
            {
                _advertiseServices = advertiseServices;
            }

            [HttpGet("GetAllAdvertise")]
            public async Task<ActionResult<IReadOnlyList<AdvertiseDto>>> GetAllAdvertise()
            {
                var adverts = await _advertiseServices.GetAllAdvertises();
                return Ok(adverts);
            }

            [HttpGet("GetAdvertiseById/{id}")]
            public async Task<ActionResult<AdvertiseDto>> GetAdvertiseById(int id)
            {
                var advert = await _advertiseServices.GetAdvertiseById(id);
                if (advert == null)
                {
                    return NotFound($"Advertise with ID {id} not found.");
                }
                return Ok(advert);
            }

            [HttpPost("AddAdvertise")]
            public async Task<ActionResult> AddAdvertise([FromForm] addAdvertise advertiseDto)
            {
                if (advertiseDto.ImagePath == null || advertiseDto.ImagePath.Length == 0)
                {
                    return BadRequest("Image is required.");
                }

                var addedAdvert = await _advertiseServices.AddAdvertise(advertiseDto);
                if (addedAdvert == null)
                {
                    return StatusCode(500, "Failed to add advertise.");
                }

                return CreatedAtAction(nameof(GetAdvertiseById), new { id = addedAdvert.Id }, addedAdvert);
            }

            [HttpPut("UpdateAdvertise/{id}")]
            public async Task<ActionResult> UpdateAdvertise(int id, [FromForm] addAdvertise advertiseDto)
            {
                var advert = await _advertiseServices.GetAdvertiseById(id);
                if (advert == null)
                {
                    return NotFound($"Advertise with ID {id} not found.");
                }

                var updatedAdvert = await _advertiseServices.UpdateAdvertise(id, advertiseDto);
                if (updatedAdvert == null)
                {
                    return StatusCode(500, "Failed to update advertise.");
                }

                return Ok(updatedAdvert);
            }

            [HttpDelete("DeleteAdvertise/{id}")]
            public async Task<ActionResult> DeleteAdvertise(int id)
            {
                var advert = await _advertiseServices.GetAdvertiseById(id);
                if (advert == null)
                {
                    return NotFound($"Advertise with ID {id} not found.");
                }

                var result = await _advertiseServices.DeleteAdvertise(id);
                if (result == 0)
                {
                    return StatusCode(500, "Failed to delete advertise.");
                }

                var advertises = await _advertiseServices.GetAllAdvertises();
            if (advertises == null)
            {
                return NotFound($"there are no Advertises");

            }
            return Ok(advertises);
                // حذف الصورة بعد التأكد من حذف الإعلان
                //_advertiseServices.DeleteImage(advert.ImagePath);

            }
        }


    }
