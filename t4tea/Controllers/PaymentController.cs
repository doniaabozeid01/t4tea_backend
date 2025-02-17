using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;
using t4tea.service.Cart;
using t4tea.service.Order.Dtos;
using t4tea.service.Order;

namespace t4tea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {



        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(IOrderService orderService, ICartService cartService, IMapper mapper, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _cartService = cartService;
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        // إنشاء الطلب
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<OrderRequest>> CreateOrder([FromBody] CreateOrderRequest model)
        {

            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound($"there is no users with id {model.UserId}");
                }

                // الحصول على السلة الخاصة بالمستخدم
                var cartItems = await _cartService.GetCartByUserId(model.UserId);
                if (cartItems == null || cartItems.Count == 0)
                    return BadRequest("Cart is empty!");

                var mappedCartItems = _mapper.Map<IReadOnlyList<CartItems>>(cartItems).ToList();
                var totalPrice = cartItems.Sum(item => (item.Product.OldPrice - (item.Product.OldPrice * (item.Product.Discount / 100))) * item.Quantity);
                // إنشاء DTO للطلب
                var orderDto = new addOrder
                {
                    UserId = model.UserId,
                    PaymentMethod = model.PaymentMethod,
                    Country = model.Country,
                    City = model.City,
                    Addrress = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    cartItems = mappedCartItems, // تمرير المنتجات من السلة
                    TotalAmount = totalPrice, // حساب الإجمالي
                    //TotalAmount = 20, // حساب الإجمالي
                    CreatedAt = DateTime.Now,
                    Status = data.Enum.OrderStatus.Pending
                };

                // إنشاء الطلب باستخدام الـ DTO
                var order = await _orderService.CreateOrderAsync(orderDto);

                if (order == null)
                {
                    return BadRequest("there is an error occured");
                }

                // معالجة العناصر: الإضافة إلى OrderItem والحذف من السلة
                foreach (var item in cartItems)
                {
                    // إضافة إلى OrderItem
                    var orderItem = new OrderItem
                    {
                        OrderRequestId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.OldPrice - (item.Product.OldPrice * (item.Product.Discount / 100)), // السعر عند الشراء
                        //Price = 3, // السعر عند الشراء
                        UserId = model.UserId
                    };

                    await _orderService.AddOrderItemAsync(orderItem);

                    // حذف العنصر من السلة
                    await _cartService.DeleteCart(item.Id);
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating order: {ex.Message}");
            }

        }



        [HttpPut("UpdateOrderStatus/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(string orderId, [FromBody] UpdateOrderStatusRequest request)
        {
            try
            {
                // ✅ جلب الطلب كـ DTO فقط
                var orderDto = await _orderService.GetOrderRequestById(orderId);
                if (orderDto == null)
                {
                    return NotFound($"لم يتم العثور على الطلب برقم {orderId}");
                }

                // ✅ جلب الكائن الفعلي من قاعدة البيانات ليصبح متتبعًا
                var order = await _unitOfWork.Repository<OrderRequest>().GetByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound($"لم يتم العثور على الطلب برقم {orderId}");
                }

                // ✅ تحديث الحالة فقط
                order.Status = request.Status;

                // ✅ تحديث الطلب في قاعدة البيانات
                await _orderService.UpdateOrderAsync(order);

                return Ok(new { message = "تم تحديث حالة الطلب بنجاح", updatedStatus = order.Status });
            }
            catch (Exception ex)
            {
                return BadRequest($"حدث خطأ أثناء تحديث الطلب: {ex.Message}");
            }
        }




        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<GetOrderRequest>> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrderRequests();
                if (orders != null)
                {
                    return Ok(orders);
                }
                return NotFound("there are no orders");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }



        [HttpGet("GetOrderById")]
        public async Task<ActionResult<GetOrderRequest>> GetOrderById(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("invalid id");
                }
                var order = await _orderService.GetOrderRequestById(id);
                if (order != null)
                {
                    return Ok(order);
                }
                return NotFound($"there is no order with id {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }



        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(string id)
        {

            try
            {
                if (id == null)
                {
                    return BadRequest("invalid id");
                }
                var order = await _orderService.GetOrderRequestById(id);
                if (order != null)
                {
                    var result = await _orderService.DeleteOrderRequest(id);

                    if (result == 0 && order == null)
                    {
                        return NotFound("No order found.");
                    }
                    else if (result == 0 && order != null)
                    {
                        return NotFound("Failed to delete order from the database .");

                    }
                    var orders = await _orderService.GetAllOrderRequests();
                    return Ok(orders);

                }

                return NotFound($"there is no order with id {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }


        }




        // بترجع كل ال orders بتاعت ال user بس ال orders اللي هو طريقه الدفع و total amount , العنوان و الكلام دا ------> Order Request
        [HttpGet("GetOrdersByUserId/{id}")]
        public async Task<ActionResult<GetOrderRequest>> GetOrdersByUserId(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("Invalid id");
                }
                var orders = await _orderService.GetAllOrdersByUserId(id);
                if (orders != null)
                {
                    return Ok(orders);
                }
                return NotFound("there are no orders");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }








        // بترجع كل ال orders بتاعت ال user بس ال orders اللي هو طريقه الدفع و total amount , العنوان و الكلام دا ------> Order Request
        [HttpGet("GetOrderItemsByOrderRequestId/{id}")]
        public async Task<ActionResult<GetOrderRequest>> GetOrderItemsByOrderRequestId(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("Invalid id");
                }
                var orders = await _orderService.GetOrderItemsByOrderRequestId(id);
                if (orders != null)
                {
                    return Ok(orders);
                }
                return NotFound("there are no orders");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }










    }
}
