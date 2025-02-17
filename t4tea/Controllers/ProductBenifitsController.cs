using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using t4tea.service.product.Dtos;
using t4tea.service.product;
using t4tea.service.benifit;
using t4tea.service.benifit.Dtos;

namespace t4tea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBenifitsController : ControllerBase
    {

        private readonly IBenifitServices _benifitServices;

        public ProductBenifitsController(IBenifitServices benifitServices)
        {
            _benifitServices = benifitServices;
        }


        [HttpPost("AddProductBenifits")]
        public async Task<ActionResult<BenifitDto>> AddProductBenifits(AddBenifits benifitDto)
        {
            try
            {
                if (benifitDto == null)
                {
                    return BadRequest("product benifits shouldn't be empty .");
                }
                var product = await _benifitServices.AddProductBenifits(benifitDto);

                if (product == null && benifitDto == null)
                {
                    return BadRequest("product benifits shouldn't be empty .");
                }
                else if (product == null && benifitDto != null)
                {
                    return BadRequest("Failed to save Tea to the database.");

                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPut("UpdateProductBenifits")]
        public async Task<ActionResult<BenifitDto>> UpdateProductBenifits(int id, AddBenifits benifitDto)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                if (benifitDto == null)
                {
                    return BadRequest("product benifits shouldn't be empty .");
                }

                var benifit = await _benifitServices.UpdateProductBenifits(id, benifitDto);
                if (benifit == null)
                {
                    return BadRequest("there is no product benifits with that id .");
                }

                if (benifit == null && benifitDto == null)
                {
                    return BadRequest("product benifit shouldn't be empty .");
                }
                else if (benifit == null && benifitDto != null)
                {
                    return BadRequest("Failed to save updated product benifit to the database.");

                }

                return Ok(benifit);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("GetProductBenifitsById/{id}")]
        public async Task<ActionResult<IReadOnlyList<BenifitDto>>> GetProductBenifitsById(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                var benifit = await _benifitServices.GetProductBenifitsById(id);

                if (benifit == null)
                {
                    return NotFound("No Product benifit found.");
                }

                return Ok(benifit);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("GetAllProductBenifits")]
        public async Task<ActionResult<IReadOnlyList<BenifitDto>>> GetAllProductBenifits()
        {
            try
            {
                var productBenifits = await _benifitServices.GetAllProductBenifits();

                if (productBenifits == null )
                {
                    return NotFound("No product benifits found.");
                }

                return Ok(productBenifits);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpDelete("DeleteProductBenifits/{id}")]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> DeleteProductBenifits(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                var benifit = await _benifitServices.GetProductBenifitsById(id);

                if (benifit == null)
                {
                    return NotFound("No product benifit found.");
                }

                var result = await _benifitServices.DeleteProductBenifits(id);
                if (result == 0 && benifit == null)
                {
                    return NotFound("No product benifit found.");
                }
                else if (result == 0 && benifit != null)
                {
                    return NotFound("Failed to delete product benifit from the database .");
                }

                var allProductbenifits = await _benifitServices.GetAllProductBenifits();
                return Ok(allProductbenifits);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
