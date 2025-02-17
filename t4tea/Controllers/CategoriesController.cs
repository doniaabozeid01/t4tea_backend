using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using t4tea.data.Entities;
using t4tea.service.category;
using t4tea.service.category.Dtos;

namespace t4tea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;

        public CategoriesController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }


        //[HttpPost("AddCategory")]
        //public async Task<ActionResult<categoryDto>> AddCategory(AddCategoryDto categoryDto)
        //{
        //    try
        //    {
        //        if (categoryDto == null)
        //        {
        //            // إرسال استجابة فارغة مع حالة 404 (Not Found)
        //            return BadRequest("tea product shouldn't be empty .");
        //        }
        //        var tea = await _categoryServices.AddCategory(categoryDto);

        //        if (tea == null && categoryDto == null)
        //        {
        //            return BadRequest("category shouldn't be empty .");
        //        }
        //        else if (tea == null && categoryDto != null)
        //        {
        //            return BadRequest("Failed to save Tea to the database.");

        //        }
        //        // إرسال الاستجابة الناجحة مع البيانات
        //        return Ok(tea);
        //    }
        //    catch (Exception ex)
        //    {
        //        // في حالة حدوث استثناء أثناء تنفيذ الكود
        //        // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}





        [HttpPost("AddCategory")]
        public async Task<ActionResult<categoryDto>> AddCategory([FromForm] AddCategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    return BadRequest("Category shouldn't be empty.");
                }

                string imagePath = null;

                if (categoryDto.Image != null)
                {
                    // حفظ الصورة واسترجاع المسار
                    imagePath = await _categoryServices.SaveImage(categoryDto.Image);
                }

                var category = await _categoryServices.AddCategory(categoryDto, imagePath);

                if (category == null)
                {
                    return BadRequest("Failed to save category.");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }









        //[HttpPut("UpdateCategory")]
        //public async Task<ActionResult<categoryDto>> UpdateCategory(int id, AddCategoryDto catDto)
        //{
        //    try
        //    {

        //        if (id <= 0)
        //        {
        //            return BadRequest("Invalid ID");
        //        }

        //        if (catDto == null)
        //        {
        //            // إرسال استجابة فارغة مع حالة 404 (Not Found)
        //            return BadRequest("category shouldn't be empty .");
        //        }

        //        var tea = await _categoryServices.UpdateCategory(id, catDto);
        //        if (tea == null)
        //        {
        //            return BadRequest("there is no category with that id .");
        //        }

        //        if (tea == null && catDto == null)
        //        {
        //            return BadRequest("category shouldn't be empty .");
        //        }
        //        else if (tea == null && catDto != null)
        //        {
        //            return BadRequest("Failed to save updated catogory to the database.");

        //        }


        //        // إرسال الاستجابة الناجحة مع البيانات
        //        return Ok(tea);
        //    }
        //    catch (Exception ex)
        //    {
        //        // في حالة حدوث استثناء أثناء تنفيذ الكود
        //        // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}










        [HttpPut("UpdateCategory/{id}")]
        public async Task<ActionResult<categoryDto>> UpdateCategory(int id, [FromForm] AddCategoryDto categoryDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                if (categoryDto == null)
                {
                    return BadRequest("Category shouldn't be empty.");
                }

                var existingCategory = await _categoryServices.GetCategoryById(id);
                if (existingCategory == null)
                {
                    return NotFound("No category found.");
                }

                string imagePath = existingCategory.ImageUrl; // الاحتفاظ بالصورة القديمة

                if (categoryDto.Image != null) // إذا تم رفع صورة جديدة
                {
                    // حذف الصورة القديمة إذا كانت موجودة
                    if (!string.IsNullOrEmpty(existingCategory.ImageUrl))
                    {
                        _categoryServices.DeleteImage(existingCategory.ImageUrl);
                    }

                    // حفظ الصورة الجديدة
                    imagePath = await _categoryServices.SaveImage(categoryDto.Image);
                }

                var updatedCategory = await _categoryServices.UpdateCategory(id, categoryDto, imagePath);

                if (updatedCategory == null)
                {
                    return BadRequest("Failed to update category.");
                }

                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }








        [HttpGet("GetCategoryById/{id}")]
        public async Task<ActionResult<IReadOnlyList<categoryDto>>> GetCategoryById(int id)
        {
            try
            {


                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                var category = await _categoryServices.GetCategoryById(id);

                if (category == null)
                {
                    return NotFound("No Category found.");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetCategoryWithProductsById/{id}")]
        public async Task<ActionResult<IReadOnlyList<categoryDto>>> GetCategoryWithProductsById(int id)
        {
            try
            {


                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                var category = await _categoryServices.GetCategoryByIdWithInclude(id);

                if (category == null)
                {
                    return NotFound("No Category found.");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpGet("GetAllCategories")]
        //public async Task<ActionResult<IReadOnlyList<categoryDto>>> GetAllCategories()
        //{
        //    try
        //    {
        //        // الحصول على جميع تفاصيل الشاي
        //        var categories = await _categoryServices.GetAllCategories();

        //        // التحقق إذا كانت البيانات فارغة
        //        if (categories == null || categories.Count == 0)
        //        {
        //            // إرسال استجابة فارغة مع حالة 404 (Not Found)
        //            return NotFound("No categories found.");
        //        }

        //        // إرسال الاستجابة الناجحة مع البيانات
        //        return Ok(categories);
        //    }
        //    catch (Exception ex)
        //    {
        //        // في حالة حدوث استثناء أثناء تنفيذ الكود
        //        // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<categoryDto>>> GetAllCategories()
        {
            var categories = await _categoryServices.GetAllCategories();

            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found.");
            }

            return Ok(categories);
        }


        [HttpGet("GetAllCategoriesWithProducts")]
        public async Task<ActionResult<IEnumerable<categoryDto>>> GetAllCategoriesWithProducts()
        {
            var categories = await _categoryServices.GetAllCategoriesWithInclude();

            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found.");
            }

            return Ok(categories);
        }





        //[HttpDelete("DeleteCategory/{id}")]
        //public async Task<ActionResult<IReadOnlyList<categoryDto>>> DeleteCategory(int id)
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

        //        var result = await _categoryServices.DeleteCategory(id);
        //        if (result == 0 && category == null)
        //        {
        //            return NotFound("No tea product found.");
        //        }
        //        else if (result == 0 && category != null)
        //        {
        //            return NotFound("Failed to delete category from the database .");
        //        }

        //        var allCategories = await _categoryServices.GetAllCategories();
        //        return Ok(allCategories);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}



























        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }

                var category = await _categoryServices.GetCategoryById(id);

                if (category == null)
                {
                    return NotFound("No category found.");
                }

                // حذف الصورة من السيرفر إذا كانت موجودة
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    _categoryServices.DeleteImage(category.ImageUrl);
                }

                var result = await _categoryServices.DeleteCategory(id);

                if (result == 0)
                {
                    return NotFound("Failed to delete category.");
                }

                return Ok("Category deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }





    }
}
