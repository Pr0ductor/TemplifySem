using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Dashboard.Queries;

public record GetDashboardStatsQuery : IRequest<DashboardStatsDto>;
