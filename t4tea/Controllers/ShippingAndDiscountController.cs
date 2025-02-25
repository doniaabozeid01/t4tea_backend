using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using t4tea.data.Entities;
using t4tea.service.product;
using t4tea.service.Review.Dtos;
using t4tea.service.Review;
using t4tea.service.shipAndDis;
using t4tea.service.shipAndDis.Dtos;

namespace t4tea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingAndDiscountController : ControllerBase
    {
        public IShippingAndDiscountServices _shippingAndDiscountServices;

        //private readonly IReviewsServices _reviewServices;
        //private readonly IProductServices _productServices;
        //private readonly UserManager<ApplicationUser> _userManager;

        public ShippingAndDiscountController(IShippingAndDiscountServices shippingAndDiscountServices)
        {

            _shippingAndDiscountServices = shippingAndDiscountServices;
        }



        [HttpGet("GetAllShippingAndDiscount")]
        public async Task<ActionResult<IReadOnlyList<GetShippingAndDiscountDto>>> GetAllShippingAndDiscount()
        {
            try
            {
                // الحصول على جميع تفاصيل الشاي
                var ships = await _shippingAndDiscountServices.GetAllShippingAndDiscount();

                //// التحقق إذا كانت البيانات فارغة
                //if (reviews == null )
                //{
                //    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                //    return NotFound("No reviews found.");
                //}

                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(ships);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }









        [HttpGet("GetShippingAndDiscountById/{id}")]
        public async Task<ActionResult<GetShippingAndDiscountDto>> GetShippingAndDiscountById(int id)
        {
            try
            {


                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                // الحصول على جميع تفاصيل الشاي
                var ship = await _shippingAndDiscountServices.GetShippingAndDiscountById(id);

                // التحقق إذا كانت البيانات فارغة
                if (ship == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return NotFound("No shipping and discount found.");
                }

                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(ship);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }






















        [HttpPost("AddShippingAndDiscount")]
        public async Task<ActionResult<GetShippingAndDiscountDto>> AddShippingAndDiscount(AddShippingAndDiscountDto shippingDto)
        {
            try
            {
                if (shippingDto == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return BadRequest("Shipping and Discount shouldn't be empty .");
                }


                var ship = await _shippingAndDiscountServices.AddShippingAndDiscount(shippingDto);

                if (ship == null && shippingDto == null)
                {
                    return BadRequest("Shipping and Discount shouldn't be empty .");
                }
                else if (ship == null && shippingDto != null)
                {
                    return BadRequest("Failed to save Shipping and Discount to the database.");

                }
                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(ship);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }











        [HttpPut("UpdateShippingAndDiscount")]
        public async Task<ActionResult<GetShippingAndDiscountDto>> UpdateShippingAndDiscount(int id, AddShippingAndDiscountDto shippingDto)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                if (shippingDto == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return BadRequest("Shipping and Discount shouldn't be empty .");
                }

                
                var ship = await _shippingAndDiscountServices.UpdateShippingAndDiscount(id, shippingDto);
                if (ship == null)
                {
                    return BadRequest("there is no Shipping and Discount with that id .");
                }

                if (ship == null && shippingDto == null)
                {
                    return BadRequest("Shipping and Discount shouldn't be empty .");
                }
                else if (ship == null && shippingDto != null)
                {
                    return BadRequest("Failed to save updated Shipping and Discount to the database.");

                }


                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(ship);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }







        [HttpDelete("DeleteShippingAndDiscount/{id}")]
        public async Task<ActionResult> DeleteShippingAndDiscount(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                var ship = await _shippingAndDiscountServices.GetShippingAndDiscountById(id);

                if (ship == null)
                {
                    return NotFound("No Shipping and Discount found.");
                }

                var result = await _shippingAndDiscountServices.DeleteShippingAndDiscount(id);
                if (result == 0 && ship == null)
                {
                    return NotFound("No review found.");
                }
                else if (result == 0 && ship != null)
                {
                    return NotFound("Failed to delete Shipping and Discount from the database .");

                }
                var reviews = await _shippingAndDiscountServices.GetAllShippingAndDiscount();
                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
