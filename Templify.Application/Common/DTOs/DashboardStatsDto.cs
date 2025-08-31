namespace Templify.Application.Common.DTOs;

public class DashboardStatsDto
{
    public int TotalUsers { get; set; }
    public int TotalAuthors { get; set; }
    public int TotalProducts { get; set; }
    public int TotalSales { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<WeeklySalesDto> WeeklySales { get; set; } = new();
    public List<CategoryStatsDto> TopCategories { get; set; } = new();
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
}

public class WeeklySalesDto
{
    public string Day { get; set; } = string.Empty;
    public int Sales { get; set; }
    public decimal Revenue { get; set; }
}

public class CategoryStatsDto
{
    public string Category { get; set; } = string.Empty;
    public int ProductCount { get; set; }
    public decimal Percentage { get; set; }
}

public class RecentActivityDto
{
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string IconClass { get; set; } = string.Empty;
}
