using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using t4tea.service.Flavour;
using t4tea.service.Flavour.Dtos;

namespace t4tea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlavoursController : ControllerBase
    {
        private readonly IFlavourServices _flavourServices;

        public FlavoursController (IFlavourServices flavourServices)
        {

            _flavourServices = flavourServices;
        }



        [HttpGet("GetAllFlavours")]
        public async Task<ActionResult<IReadOnlyList<GetFlavourDto>>> GetAllFlavours()
        {
            try
            {
                // الحصول على جميع تفاصيل الشاي
                var flavours = await _flavourServices.GetAllFlavours();

                //// التحقق إذا كانت البيانات فارغة
                //if (reviews == null )
                //{
                //    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                //    return NotFound("No reviews found.");
                //}

                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(flavours);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }









        [HttpGet("GetFlavourById/{id}")]
        public async Task<ActionResult<GetFlavourDto>> GetFlavourById (int id)
        {
            try
            {


                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                // الحصول على جميع تفاصيل الشاي
                var flavour = await _flavourServices.GetFlavourById(id);

                // التحقق إذا كانت البيانات فارغة
                if (flavour == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return NotFound("No flavour found.");
                }

                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(flavour);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }






















        [HttpPost("AddFlavour")]
        public async Task<ActionResult<addFlavourDto>> AddFlavour (addFlavourDto flavourDto)
        {
            try
            {
                if (flavourDto == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return BadRequest("Flavour shouldn't be empty .");
                }

                var flavour = await _flavourServices.AddFlavour(flavourDto);

                if (flavour == null && flavourDto == null)
                {
                    return BadRequest("Flavour shouldn't be empty .");
                }
                else if (flavour == null && flavourDto != null)
                {
                    return BadRequest("Failed to save Flavour to the database.");

                }
                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(flavour);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }











        [HttpPut("UpdateFlavour")]
        public async Task<ActionResult<GetFlavourDto>> UpdateFlavour (int id, addFlavourDto flavourDto)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                if (flavourDto == null)
                {
                    // إرسال استجابة فارغة مع حالة 404 (Not Found)
                    return BadRequest("Flavour shouldn't be empty .");
                }

                var flavour = await _flavourServices.UpdateFlavour(id, flavourDto);
                if (flavour == null)
                {
                    return BadRequest("there is no flavour with that id .");
                }

                if (flavour == null && flavourDto == null)
                {
                    return BadRequest("flavour shouldn't be empty .");
                }
                else if (flavour == null && flavourDto != null)
                {
                    return BadRequest("Failed to save updated flavour to the database.");

                }


                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(flavour);
            }
            catch (Exception ex)
            {
                // في حالة حدوث استثناء أثناء تنفيذ الكود
                // يمكنك تسجيل الخطأ أو إضافة تفاصيل إضافية هنا
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }







        [HttpDelete("DeleteFlavour/{id}")]
        public async Task<ActionResult> DeleteFlavour (int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                var flavour = await _flavourServices.GetFlavourById(id);

                if (flavour == null)
                {
                    return NotFound("No flavour found.");
                }

                var result = await _flavourServices.DeleteFlavour(id);
                if (result == 0 && flavour == null)
                {
                    return NotFound("No flavour found.");
                }
                else if (result == 0 && flavour != null)
                {
                    return NotFound("Failed to delete flavour from the database .");

                }
                var flavours = await _flavourServices.GetAllFlavours();
                // إرسال الاستجابة الناجحة مع البيانات
                return Ok(flavours);
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
