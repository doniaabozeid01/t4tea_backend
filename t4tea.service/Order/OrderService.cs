using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;
using t4tea.service.Cart;
using t4tea.service.Order.Dtos;

namespace t4tea.service.Order
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, ICartService cartService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _mapper = mapper;
        }

        public async Task<OrderRequest> CreateOrderAsync(addOrder orderDto)
        {



            if (orderDto.cartItems == null || orderDto.cartItems.Count == 0)
                return null;

            // إنشاء الكائن OrderRequest باستخدام البيانات من الـ DTO
            var order = new OrderRequest
            {
                Id = Guid.NewGuid().ToString(), // إذا كنت تستخدم GUIDs
                UserId = orderDto.UserId,
                PaymentMethod = orderDto.PaymentMethod,
                TotalAmount = orderDto.TotalAmount,
                Country = orderDto.Country,
                city = orderDto.City,
                Address = orderDto.Addrress,
                PhoneNumber = orderDto.PhoneNumber,
                CreatedAt = DateTime.Now,
                Status = orderDto.Status,
            };

            // إضافة الطلب إلى قاعدة البيانات
            await _unitOfWork.Repository<OrderRequest>().AddAsync(order);

            foreach (var item in orderDto.cartItems)
            {
                item.OrderRequestId = order.Id; // ربط كل عنصر في السلة بالطلب الجديد

                // تأكد من أن الكائن لم يتم تتبعه مسبقًا
                var existingItem = await _unitOfWork.Repository<CartItems>().GetByIdAsync(item.Id);
                if (existingItem != null)
                {
                    // إذا كان الكائن موجودًا بالفعل، قم بتحديثه
                    existingItem.OrderRequestId = order.Id;
                    _unitOfWork.Repository<CartItems>().Update(existingItem);
                }
                else
                {
                    // إذا كان الكائن غير موجود، قم بإضافته
                    _unitOfWork.Repository<CartItems>().AddAsync(item);
                }
            }

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                return null;
            }

            return order;
        }






        // add to order item table
        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            await _unitOfWork.Repository<OrderItem>().AddAsync(orderItem);
            await _unitOfWork.CompleteAsync();
        }




        public async Task<IReadOnlyList<GetOrderRequest>> GetAllOrderRequests()
        {
            var orderRequests = await _unitOfWork.Repository<OrderRequest>().GetAllAsync();
            var mappedOrder = _mapper.Map<IReadOnlyList<GetOrderRequest>>(orderRequests);
            return mappedOrder;
        }


        public async Task<GetOrderRequest> GetOrderRequestById(string id)
        {
            var orderRequests = await _unitOfWork.Repository<OrderRequest>().GetByIdAsync(id);
            var mappedOrder = _mapper.Map<GetOrderRequest>(orderRequests);
            return mappedOrder;
        }





        public async Task<int> DeleteOrderRequest(string id)
        {
            var orderRequests = await _unitOfWork.Repository<OrderRequest>().GetByIdAsync(id);

            if (orderRequests != null)
            {
                _unitOfWork.Repository<OrderRequest>().Delete(orderRequests);
                var status = await _unitOfWork.CompleteAsync(); //

                if (status == 0)
                {
                    return 0;
                }
                return status;
            }
            return 0;

        }






        public async Task<IReadOnlyList<GetOrderRequest>> GetAllOrdersByUserId(string id)
        {
            var orderRequests = await _unitOfWork.Repository<OrderRequest>().GetOrdersByUserId(id);
            var mappedOrder = _mapper.Map<IReadOnlyList<GetOrderRequest>>(orderRequests);
            return mappedOrder;
        }













        public async Task<IReadOnlyList<GetOrderItems>> GetOrderItemsByOrderRequestId(string id)
        {
            var orderItems = await _unitOfWork.Repository<OrderItem>().GetOrderItemsByOrderRequestId(id);
            var mappedOrder = _mapper.Map<IReadOnlyList<GetOrderItems>>(orderItems);
            return mappedOrder;
        }


        public async Task<bool> UpdateOrderAsync(OrderRequest order)
        {
            _unitOfWork.Repository<OrderRequest>().Update(order); // ✅ تحديث الطلب
            return await _unitOfWork.CompleteAsync() > 0;
        }




    }
}
