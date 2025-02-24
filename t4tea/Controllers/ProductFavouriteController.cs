using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using t4tea.service.advertise.Dtos;
using t4tea.service.advertise;
using t4tea.service.favourite;
using t4tea.service.favourite.Dtos;

namespace t4tea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductFavouriteController : ControllerBase
    {
        private readonly IFavouriteServices _favouriteServices;

        public ProductFavouriteController(IFavouriteServices favouriteServices)
        {
            _favouriteServices = favouriteServices;
        }

        //[HttpGet("GetAllAdvertise")]
        //public async Task<ActionResult<IReadOnlyList<AdvertiseDto>>> GetAllAdvertise()
        //{
        //    var adverts = await _advertiseServices.GetAllAdvertises();
        //    return Ok(adverts);
        //}

        [HttpGet("GetProductFavouriteByUserId/{id}")]
        public async Task<ActionResult<IReadOnlyList<FavouriteDto>>> GetProductFavouriteByUserId(string id)
        {
            var favProduct = await _favouriteServices.GetProductFavouriteByUserId(id);
            if (favProduct == null)
            {
                return NotFound($"favourite proucts with ID {id} not found.");
            }
            return Ok(favProduct);
        }

        [HttpPost("AddProductFavourite")]
        public async Task<ActionResult> AddProductFavourite(AddFavouriteDto favouriteDto)
        {
            if (favouriteDto == null)
            {
                return BadRequest("favourite Product is required.");
            }

            var addedFav = await _favouriteServices.AddFavouriteProduct(favouriteDto);
            if (addedFav == null)
            {
                return StatusCode(500, "Failed to add favourite.");
            }

            var favoriteProduct = new FavouriteDto
            {
                Id = addedFav.Id,
                ProductId = favouriteDto.ProductId,
                UserId = favouriteDto.UserId
            };

            return Ok(favoriteProduct);
        }

        //[HttpPut("UpdateAdvertise/{id}")]
        //public async Task<ActionResult> UpdateAdvertise(int id, [FromForm] addAdvertise advertiseDto)
        //{
        //    var advert = await _advertiseServices.GetAdvertiseById(id);
        //    if (advert == null)
        //    {
        //        return NotFound($"Advertise with ID {id} not found.");
        //    }

        //    var updatedAdvert = await _advertiseServices.UpdateAdvertise(id, advertiseDto);
        //    if (updatedAdvert == null)
        //    {
        //        return StatusCode(500, "Failed to update advertise.");
        //    }

        //    return Ok(updatedAdvert);
        //}

        [HttpDelete("DeleteProductFavourite/{id}")]
        public async Task<ActionResult<IReadOnlyList<FavouriteDto>>> DeleteProductFavourite(int id)
        {
            var fav = await _favouriteServices.GetProductFavouriteById(id);
            if (fav == null)
            {
                return NotFound($"Favourite with ID {id} not found.");
            }
            var userId = fav.UserId ;

            var result = await _favouriteServices.DeleteProductFavourite(id);
            if (result == 0)
            {
                return StatusCode(500, "Failed to delete favourite product.");
            }

            var favouritesOfUser = await _favouriteServices.GetProductFavouriteByUserId(userId);
            if (favouritesOfUser == null)
            {
                return NotFound($"there are no Advertises");

            }
            return Ok(favouritesOfUser);
            // حذف الصورة بعد التأكد من حذف الإعلان
            //_advertiseServices.DeleteImage(advert.ImagePath);

        }
    }

}

