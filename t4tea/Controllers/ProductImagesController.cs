using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using t4tea.service.advertise.Dtos;
using t4tea.service.advertise;
using t4tea.service.images;
using t4tea.service.images.Dtos;

namespace t4tea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IImagesServices _imagesServices;

        public ProductImagesController(IImagesServices imageServices)
        {
            _imagesServices = imageServices;
        }

        [HttpGet("GetAllProductsImages")]
        public async Task<ActionResult<IReadOnlyList<ImageDto>>> GetAllProductsImages()
        {
            var imgs = await _imagesServices.GetAllProductsImages();
            return Ok(imgs);
        }

        [HttpGet("GetProductImagesById/{id}")]
        public async Task<ActionResult<ImageDto>> GetProductImagesById(int id)
        {
            var img = await _imagesServices.GetProductImageById(id);
            if (img == null)
            {
                return NotFound($"Product image with ID {id} not found.");
            }
            return Ok(img);
        }




        [HttpPost("AddProductImage")]
        public async Task<ActionResult> AddAdvertise([FromForm] AddImage imageDto)
        {
            if (imageDto.ImagePath == null || imageDto.ImagePath.Length == 0)
            {
                return BadRequest("Image is required.");
            }

            var addedImg = await _imagesServices.AddProductImage(imageDto);
            if (addedImg == null)
            {
                return StatusCode(500, "Failed to add advertise.");
            }

            return CreatedAtAction(nameof(GetProductImagesById), new { id = addedImg.Id }, addedImg);
        }

        [HttpPut("UpdateProductImage/{id}")]
        public async Task<ActionResult> UpdateProductImage(int id, [FromForm] AddImage imageDto)
        {
            var img = await _imagesServices.GetProductImageById(id);
            if (img == null)
            {
                return NotFound($"image with ID {id} not found.");
            }

            var updatedImage = await _imagesServices.UpdateImage(id, imageDto);
            if (updatedImage == null)
            {
                return StatusCode(500, "Failed to update image.");
            }

            return Ok(updatedImage );
        }

        [HttpDelete("DeleteImage/{id}")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            var image = await _imagesServices.GetProductImageById(id);
            if (image == null)
            {
                return NotFound($"image with ID {id} not found.");
            }

            var result = await _imagesServices.DeleteImage(id);
            if (result == 0)
            {
                return StatusCode(500, "Failed to delete image.");
            }

            var images = await _imagesServices.GetAllProductsImages();
            if (images == null)
            {
                return NotFound($"there are no Advertises");

            }
            return Ok(images);
            // حذف الصورة بعد التأكد من حذف الإعلان
            //_advertiseServices.DeleteImage(advert.ImagePath);

        }
    
}
}
