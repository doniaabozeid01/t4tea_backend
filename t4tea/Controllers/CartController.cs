using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using t4tea.data.Entities;
using t4tea.service.Cart.Dtos;
using t4tea.service.Cart;
using t4tea.service.product;
using t4tea.service.product.Dtos;

namespace t4tea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartServices;
        private readonly IProductServices _productServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ICartService cartServices, IProductServices productServices, UserManager<ApplicationUser> userManager)
        {
            _cartServices = cartServices;
            _productServices = productServices;
            _userManager = userManager;
        }



        [HttpGet("GetAllCarts")]
        public async Task<ActionResult<IReadOnlyList<GetCart>>> GetAllCarts()
        {
            try
            {
                // الحصول على جميع تفاصيل الشاي
                var carts = await _cartServices.GetAllCarts();

                // التحقق إذا كانت البيانات فارغة
                if (carts == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return NotFound("No carts found.");
                }

                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(carts);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }









        [HttpGet("GetCartById/{id}")]
        public async Task<ActionResult<ProductDto>> GetCartById(int id)
        {
            try
            {


                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                // الحصول على جميع تفاصيل الشاي
                var cart = await _cartServices.GetCartById(id);

                // التحقق إذا كانت البيانات فارغة
                if (cart == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return NotFound("No cart found.");
                }

                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(cart);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }










        [HttpGet("GetCartByUserId/{id}")]
        public async Task<ActionResult<ProductDto>> GetCartByUserId(string id)
        {
            try
            {


                if (id == null)
                {
                    return BadRequest("Invalid User Id");
                }
                // الحصول على جميع تفاصيل الشاي
                var cart = await _cartServices.GetCartByUserId(id);

                // التحقق إذا كانت البيانات فارغة
                if (cart == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return NotFound("No cart found.");
                }

                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(cart);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }













        [HttpPost("AddCart")]
        public async Task<ActionResult<GetCart>> AddCart(AddCart cartDto)
        {
            try
            {
                if (cartDto == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return BadRequest("Cart shouldn't be empty .");
                }
                var product = await _productServices.GetProductById(cartDto.ProductId);
                if (product == null)
                {
                    return NotFound($"there is no product exist with id {cartDto.ProductId}");
                }

                var User = await _userManager.FindByIdAsync(cartDto.UserId);
                if (User == null)
                {
                    return NotFound($"there is no user exist with id {cartDto.UserId}");
                }

                if (cartDto.Quantity <= 0)
                {
                    return BadRequest("Cart quantity should be greater than 0 .");

                }

                var cart = await _cartServices.AddCart(cartDto);

                if (cart == null && cartDto == null)
                {
                    return BadRequest("Cart shouldn't be empty .");
                }
                else if (cart == null && cartDto != null)
                {
                    return BadRequest("Failed to save Cart to the database.");

                }
                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(cart);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }











        [HttpPut("UpdateCart/{id}")]
        public async Task<ActionResult<GetCart>> UpdateCart(int id, AddCart cartDto)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                if (cartDto == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return BadRequest("Cart shouldn't be empty .");
                }

                var product = await _productServices.GetProductById(cartDto.ProductId);
                if (product == null)
                {
                    return NotFound($"there is no product exist with id {cartDto.ProductId}");
                }

                var User = await _userManager.FindByIdAsync(cartDto.UserId);
                if (User == null)
                {
                    return NotFound($"there is no user exist with id {cartDto.UserId}");
                }


                var cart = await _cartServices.UpdateCart(id, cartDto);
                if (cart == null)
                {
                    return BadRequest("there is no cart with that id .");
                }

                if (cart == null && cartDto == null)
                {
                    return BadRequest("Cart shouldn't be empty .");
                }
                else if (cart == null && cartDto != null)
                {
                    return BadRequest("Failed to save updated cart to the database.");

                }


                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(cart);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }








        [HttpDelete("DeleteCart/{id}")]
        public async Task<ActionResult<IReadOnlyList<GetCart>>> DeleteCart(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                var cart = await _cartServices.GetCartById(id);

                if (cart == null)
                {
                    return NotFound("No cart found.");
                }

                var result = await _cartServices.DeleteCart(id);
                if (result == 0 && cart == null)
                {
                    return NotFound("No cart found.");
                }
                else if (result == 0 && cart != null)
                {
                    return NotFound("Failed to delete cart from the database .");

                }

                var userCarts = await _cartServices.GetCartByUserId(cart.UserId);
                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(userCarts);
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
