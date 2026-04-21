namespace Nesi.Application.DTOs.Report;

public class InventoryUsageDto
{
    public int MaterialId { get; set; }
    public string MaterialName { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    
    // Usage Data
    public decimal QuantityUsed { get; set; }
    public decimal TotalCost { get; set; }
    public decimal AverageCostPerUnit { get; set; }
    
    // Work Order Count
    public int TimesUsed { get; set; }
    public List<string> WorkOrderNumbers { get; set; } = new();
    
    // Dates
    public DateTime? FirstUsedDate { get; set; }
    public DateTime? LastUsedDate { get; set; }
}

public class InventoryUsageSummaryDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public int TotalMaterialsUsed { get; set; }
    public decimal TotalMaterialCost { get; set; }
    public decimal AverageCostPerWorkOrder { get; set; }
    
    // Top Usage
    public List<InventoryUsageDto> TopMaterialsByQuantity { get; set; } = new();
    public List<InventoryUsageDto> TopMaterialsByCost { get; set; } = new();
    
    // All Materials
    public List<InventoryUsageDto> AllMaterials { get; set; } = new();
}
