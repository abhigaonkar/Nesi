namespace Nesi.Application.DTOs.Report;

public class ArAgingReportDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerNumber { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    
    // Aging Buckets
    public decimal Current { get; set; } // 0-30 days
    public decimal Days31To60 { get; set; }
    public decimal Days61To90 { get; set; }
    public decimal Days91To120 { get; set; }
    public decimal Over120Days { get; set; }
    
    // Total
    public decimal TotalOutstanding { get; set; }
    
    // Additional Info
    public int TotalInvoices { get; set; }
    public DateTime? OldestInvoiceDate { get; set; }
}

public class ArAgingSummaryDto
{
    public DateTime AsOfDate { get; set; }
    
    // Summary by aging bucket
    public decimal TotalCurrent { get; set; }
    public decimal TotalDays31To60 { get; set; }
    public decimal TotalDays61To90 { get; set; }
    public decimal TotalDays91To120 { get; set; }
    public decimal TotalOver120Days { get; set; }
    
    // Grand Total
    public decimal GrandTotal { get; set; }
    
    // Metrics
    public int TotalCustomersWithBalance { get; set; }
    public int TotalOutstandingInvoices { get; set; }
    public decimal AverageDaysOutstanding { get; set; }
    
    // Detail
    public List<ArAgingReportDto> CustomerDetails { get; set; } = new();
}
