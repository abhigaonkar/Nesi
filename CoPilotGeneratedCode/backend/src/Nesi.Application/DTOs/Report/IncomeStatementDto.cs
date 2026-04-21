namespace Nesi.Application.DTOs.Report;

public class IncomeStatementDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    // Revenue
    public decimal TotalRevenue { get; set; }
    public decimal ServiceRevenue { get; set; }
    public decimal MaterialRevenue { get; set; }
    
    // Cost of Goods Sold
    public decimal TotalCOGS { get; set; }
    public decimal LaborCost { get; set; }
    public decimal MaterialCost { get; set; }
    
    // Gross Profit
    public decimal GrossProfit { get; set; }
    public decimal GrossProfitMargin { get; set; } // Percentage
    
    // Operating Expenses (placeholder - would come from accounting system)
    public decimal OperatingExpenses { get; set; }
    public decimal AdminExpenses { get; set; }
    public decimal SellingExpenses { get; set; }
    
    // Net Income
    public decimal NetIncome { get; set; }
    public decimal NetProfitMargin { get; set; } // Percentage
    
    // Additional Metrics
    public int TotalWorkOrdersCompleted { get; set; }
    public int TotalQuotesGenerated { get; set; }
    public decimal AverageWorkOrderValue { get; set; }
}
