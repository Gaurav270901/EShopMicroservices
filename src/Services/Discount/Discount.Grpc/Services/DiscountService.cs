using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService (DiscountContext dbContext , ILogger<DiscountService> logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons
                .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if (coupon == null)
            {
                    var emptyCoupon = new CouponModel
                    { 
                        ProductName ="No Discount",
                        Description = "Coupon is not valid",
                        Amount = 0
                    };
            }
            logger.LogInformation("Discount with ProductName={ProductName} retrieved successfully.", request.ProductName);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if(coupon is null)
            {
                throw new RpcException( new Status (StatusCode.InvalidArgument, "Coupon data is null"));
            }
            dbContext.Coupons.Add(coupon);
            await dbContext.SaveChangesAsync();

           logger.LogInformation("Discount with ProductName={ProductName} created successfully.", coupon.ProductName);
           var couponModel = coupon.Adapt<CouponModel>();
           return couponModel;

        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            // check if coupon exists
            var coupon = dbContext.Coupons
                .FirstOrDefault(c => c.Id == request.Coupon.Id);
            if(coupon is null)
            {
                throw new RpcException( new Status (StatusCode.NotFound, $"Coupon with Id={request.Coupon.Id} not found"));
            }
            // update coupon
            coupon.ProductName = request.Coupon.ProductName;
            coupon.Description = request.Coupon.Description;
            coupon.Amount = request.Coupon.Amount;
            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Discount with Id={Id} updated successfully.", coupon.Id);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;

        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons
                .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if(coupon is null)
                {
                throw new RpcException( new Status (StatusCode.NotFound, $"Coupon with ProductName={request.ProductName} not found"));
            }
            dbContext.Coupons.Remove(coupon);
            var deleted = await dbContext.SaveChangesAsync();
            var response = new DeleteDiscountResponse
            {
                Success = deleted > 0
            };
            logger.LogInformation("Discount with ProductName={ProductName} deleted successfully.", request.ProductName);
            return response;
        }
    }
}
