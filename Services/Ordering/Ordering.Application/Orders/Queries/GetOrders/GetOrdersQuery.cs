using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Queries.GetOrders;
public record GetOrdersResult(PaginatedResult<OrderDto> Orders);
public record GetOrdersQuery(PaginationRequest PaginationRequest): IQuery<GetOrdersResult>;