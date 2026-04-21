namespace Nesi.Application.DTOs.Report;

public class JobCostReportDto
{
    public int WorkOrderId { get; set; }
    public string WorkOrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    
    // Cost Data
    public decimal ActualLaborCost { get; set; }
    public decimal ActualMaterialCost { get; set; }
    public decimal TotalActualCost { get; set; }
    
    // Revenue Data
    public decimal QuotedAmount { get; set; }
    public decimal InvoicedAmount { get; set; }
    
    // Profitability
    public decimal GrossProfit { get; set; }
    public decimal GrossProfitMargin { get; set; } // Percentage
    
    // Hour Data
    public decimal TotalHours { get; set; }
    public decimal CostPerHour { get; set; }
}

public class JobCostSummaryDto
{
    public int TotalWorkOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalProfit { get; set; }
    public decimal AverageProfitMargin { get; set; }
    public decimal TotalHours { get; set; }
    public List<JobCostReportDto> WorkOrders { get; set; } = new();
}
