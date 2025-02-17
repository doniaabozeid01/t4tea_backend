using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using t4tea.service.category.Dtos;
using t4tea.service.category;
using t4tea.service.product;
using t4tea.service.product.Dtos;
using t4tea.data.Entities;

namespace t4tea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {



        private readonly IProductServices _productServices;

        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }


        [HttpPost("AddProduct")]
        public async Task<ActionResult<ProductDto>> AddProduct(addProductDto prodDto)
        {
            try
            {
                if (prodDto == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return BadRequest("product shouldn't be empty .");
                }
                var product = await _productServices.AddProduct(prodDto);

                if (product == null && prodDto == null)
                {
                    return BadRequest("product shouldn't be empty .");
                }
                else if (product == null && prodDto != null)
                {
                    return BadRequest("Failed to save Tea to the database.");

                }
                
                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(product);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }





        //[HttpPost("AddCategory")]
        //public async Task<ActionResult<categoryDto>> AddCategory([FromForm] AddCategoryDto categoryDto)
        //{
        //    try
        //    {
        //        if (categoryDto == null)
        //        {
        //            return BadRequest("Category shouldn't be empty.");
        //        }

        //        string imagePath = null;

        //        if (categoryDto.Image != null)
        //        {
        //            // حفظ الصورة واسترجاع المسار
        //            imagePath = await _categoryServices.SaveImage(categoryDto.Image);
        //        }

        //        var category = await _categoryServices.AddCategory(categoryDto, imagePath);

        //        if (category == null)
        //        {
        //            return BadRequest("Failed to save category.");
        //        }

        //        return Ok(category);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}









        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, addProductDto prodDto)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                if (prodDto == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return BadRequest("product shouldn't be empty .");
                }

                var product = await _productServices.UpdateProduct(id, prodDto);
                if (product == null)
                {
                    return BadRequest("there is no product with that id .");
                }

                if (product == null && prodDto == null)
                {
                    return BadRequest("product shouldn't be empty .");
                }
                else if (product == null && prodDto != null)
                {
                    return BadRequest("Failed to save updated product to the database.");

                }


                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(product);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }














        [HttpPut("changeDiscount")]
        public async Task<ActionResult<ProductDto>> changeDiscount(int id, decimal discount)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                var product = await _productServices.changeDiscount(id, discount);
                if (product == null)
                {
                    return BadRequest("there is no product with that id .");
                }

                if (product == null)
                {
                    return BadRequest("product shouldn't be empty .");
                }
                else if (product == null)
                {
                    return BadRequest("Failed to save updated product to the database.");

                }


                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(product);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        //[HttpPut("UpdateCategory/{id}")]
        //public async Task<ActionResult<categoryDto>> UpdateCategory(int id, [FromForm] AddCategoryDto categoryDto)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            return BadRequest("Invalid ID");
        //        }

        //        if (categoryDto == null)
        //        {
        //            return BadRequest("Category shouldn't be empty.");
        //        }

        //        var existingCategory = await _categoryServices.GetCategoryById(id);
        //        if (existingCategory == null)
        //        {
        //            return NotFound("No category found.");
        //        }

        //        string imagePath = existingCategory.ImageUrl; // الاحتفاظ بالصورة القديمة

        //        if (categoryDto.Image != null) // إذا تم رفع صورة جديدة
        //        {
        //            // حذف الصورة القديمة إذا كانت موجودة
        //            if (!string.IsNullOrEmpty(existingCategory.ImageUrl))
        //            {
        //                _categoryServices.DeleteImage(existingCategory.ImageUrl);
        //            }

        //            // حفظ الصورة الجديدة
        //            imagePath = await _categoryServices.SaveImage(categoryDto.Image);
        //        }

        //        var updatedCategory = await _categoryServices.UpdateCategory(id, categoryDto, imagePath);

        //        if (updatedCategory == null)
        //        {
        //            return BadRequest("Failed to update category.");
        //        }

        //        return Ok(updatedCategory);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}








        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProductById(int id)
        {
            try
            {


                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                // الحصول على جميع تفاصيل الشاي
                var product = await _productServices.GetProductById(id);
                //product.Rate = product.reviews.Average(r => r.Rating);
                // التحقق إذا كانت البيانات فارغة
                if (product == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return NotFound("No Product found.");
                }

                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(product);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProducts()
        {
            try
            {
                // الحصول على جميع تفاصيل الشاي
                var products = await _productServices.GetAllProducts();

                // التحقق إذا كانت البيانات فارغة
                if (products == null )
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return NotFound("No products found.");
                }

                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(products);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        //[HttpGet("GetAllCategories")]
        //public async Task<ActionResult<IEnumerable<categoryDto>>> GetAllCategories()
        //{
        //    var categories = await _categoryServices.GetAllCategories();

        //    if (categories == null || !categories.Any())
        //    {
        //        return NotFound("No categories found.");
        //    }

        //    return Ok(categories);
        //}






        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                var product = await _productServices.GetProductById(id);

                if (product == null)
                {
                    return NotFound("No product found.");
                }

                var result = await _productServices.DeleteProduct(id);
                if (result == 0 && product == null)
                {
                    return NotFound("No product found.");
                }
                else if (result == 0 && product != null)
                {
                    return NotFound("Failed to delete product from the database .");
                }

                var allProducts = await _productServices.GetAllProducts();
                return Ok(allProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }










        [HttpGet("GetAllProductWithOriginalOffer")]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProductWithOriginalOffer()
        {
            try
            {
                // الحصول على جميع تفاصيل الشاي
                var offers = await _productServices.GetAllProductWithOriginalOffer();

                // التحقق إذا كانت البيانات فارغة
                if (offers == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return NotFound("No offers found.");
                }

                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(offers);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        [HttpGet("GetAllProductWithVIPOffer")]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProductWithVIPOffer()
        {
            try
            {
                // الحصول على جميع تفاصيل الشاي
                var offers = await _productServices.GetAllProductWithVIPOffer();

                // التحقق إذا كانت البيانات فارغة
                if (offers == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return NotFound("No offers found.");
                }

                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(offers);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




































        //[HttpDelete("DeleteCategory/{id}")]
        //public async Task<ActionResult> DeleteCategory(int id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            return BadRequest("Invalid Id");
        //        }

        //        var category = await _categoryServices.GetCategoryById(id);

        //        if (category == null)
        //        {
        //            return NotFound("No category found.");
        //        }

        //        // حذف الصورة من السيرفر إذا كانت موجودة
        //        if (!string.IsNullOrEmpty(category.ImageUrl))
        //        {
        //            _categoryServices.DeleteImage(category.ImageUrl);
        //        }

        //        var result = await _categoryServices.DeleteCategory(id);

        //        if (result == 0)
        //        {
        //            return NotFound("Failed to delete category.");
        //        }

        //        return Ok("Category deleted successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}


        /////////////////////////////////// -- Offers -- ////////////////////////////////////
        //[HttpGet("GetAllDailyOffers")]
        //public async Task<ActionResult<IReadOnlyList<getOffers>>> GetAllDailyOffers()
        //{
        //    try
        //    {
        //        // الحصول على جميع تفاصيل الشاي
        //        var offers = await _productServices.GetAllOffers(0);

        //        // التحقق إذا كانت البيانات فارغة
        //        if (offers == null )
        //        {
        //            // إرسال استجابة فارغة مع حالة 404 (Not Found)
        //            return NotFound("No offers found.");
        //        }

        //        // إرسال الاستجابة الناجحة مع البيانات
        //        return Ok(offers);
        //    }
        //    catch (Exception ex)
        //    {
        //        // في حالة حدوث استثناء أثناء تنفيذ الكود
        //        // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}



        //[HttpGet("GetAllWeeklyOffers")]
        //public async Task<ActionResult<IReadOnlyList<getOffers>>> GetAllWeeklyOffers()
        //{
        //    try
        //    {
        //        // الحصول على جميع تفاصيل الشاي
        //        var offers = await _productServices.GetAllOffers(1);

        //        // التحقق إذا كانت البيانات فارغة
        //        if (offers == null)
        //        {
        //            // إرسال استجابة فارغة مع حالة 404 (Not Found)
        //            return NotFound("No offers found.");
        //        }

        //        // إرسال الاستجابة الناجحة مع البيانات
        //        return Ok(offers);
        //    }
        //    catch (Exception ex)
        //    {
        //        // في حالة حدوث استثناء أثناء تنفيذ الكود
        //        // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}


        //[HttpGet("GetAllVIPOffers")]
        //public async Task<ActionResult<IReadOnlyList<getOffers>>> GetAllVIPOffers()
        //{
        //    try
        //    {
        //        // الحصول على جميع تفاصيل الشاي
        //        var offers = await _productServices.GetAllOffers(2);

        //        // التحقق إذا كانت البيانات فارغة
        //        if (offers == null)
        //        {
        //            // إرسال استجابة فارغة مع حالة 404 (Not Found)
        //            return NotFound("No offers found.");
        //        }

        //        // إرسال الاستجابة الناجحة مع البيانات
        //        return Ok(offers);
        //    }
        //    catch (Exception ex)
        //    {
        //        // في حالة حدوث استثناء أثناء تنفيذ الكود
        //        // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

    }
}
