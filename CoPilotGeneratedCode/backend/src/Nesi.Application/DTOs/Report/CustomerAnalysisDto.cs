namespace Nesi.Application.DTOs.Report;

public class CustomerAnalysisDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerNumber { get; set; } = string.Empty;
    
    // Activity Metrics
    public int TotalQuotes { get; set; }
    public int TotalWorkOrders { get; set; }
    public decimal QuoteWinRate { get; set; } // Percentage
    
    // Financial Metrics
    public decimal TotalRevenue { get; set; }
    public decimal AverageWorkOrderValue { get; set; }
    public decimal TotalProfit { get; set; }
    public decimal ProfitMargin { get; set; } // Percentage
    
    // Pricing Analysis
    public decimal AverageLaborRate { get; set; }
    public decimal AverageMaterialMarkup { get; set; } // Percentage
    
    // Dates
    public DateTime? FirstQuoteDate { get; set; }
    public DateTime? LastWorkOrderDate { get; set; }
    public int DaysSinceLastOrder { get; set; }
    
    // Payment
    public decimal OutstandingBalance { get; set; }
    public decimal AverageDaysToPayment { get; set; }
}

public class CustomerAnalysisSummaryDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public int TotalActiveCustomers { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageRevenuePerCustomer { get; set; }
    
    // Top Performers
    public List<CustomerAnalysisDto> TopCustomersByRevenue { get; set; } = new();
    public List<CustomerAnalysisDto> TopCustomersByProfitMargin { get; set; } = new();
    
    // All Customers
    public List<CustomerAnalysisDto> AllCustomers { get; set; } = new();
}
