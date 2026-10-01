using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountDbContext dbContext, ILogger<DiscountService> logger):
    DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName)
            ?? new Coupon{ProductName = "No Discount", Amount = 0, Description = "No Discount"};
        logger.LogInformation("Getting coupon {CouponId}",  coupon.Id);
        
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>() 
                     ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid input"));
        await dbContext.AddAsync(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Created coupon {CouponId}", coupon.Id);
        return coupon.Adapt<CouponModel>(); 
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>() 
                     ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid input"));
        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();
        
        
        logger.LogInformation("Updated coupon {CouponId}", coupon.Id);
        return coupon.Adapt<CouponModel>(); 
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName)
                     ?? throw new RpcException(new Status(StatusCode.NotFound,
                         $"Discount With Product Name - {request.ProductName} Couldn't found"));
        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();
        return new DeleteDiscountResponse{Success = true};
    }
}