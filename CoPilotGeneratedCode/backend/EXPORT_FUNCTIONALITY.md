# Export Functionality Documentation

This document describes the PDF and Excel export functionality that has been implemented in the Nesi application.

## Overview

The export functionality allows users to export data from various modules to PDF and Excel formats. This is useful for:
- Generating reports for stakeholders
- Creating offline backups of data
- Sharing data with external systems
- Archiving records

## Technologies Used

- **QuestPDF** (v2024.10.3): Modern, open-source PDF generation library
- **EPPlus** (v7.0.5): Excel file generation library for .NET

Both libraries are configured for non-commercial use with Community licenses.

## Architecture

### Service Layer

The export functionality is implemented through the `IExportService` interface:

```csharp
public interface IExportService
{
    byte[] ExportToPdf<T>(IEnumerable<T> data, string title, Dictionary<string, string> columns);
    byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName, Dictionary<string, string> columns);
    byte[] ExportReportToPdf(string reportTitle, Dictionary<string, object> reportData, string? dateRange = null);
}
```

The service is registered in the DI container as a scoped service in `Program.cs`:

```csharp
builder.Services.AddScoped<IExportService, ExportService>();
```

### Available Export Endpoints

#### 1. Customer Exports

**Export Customers to PDF**
```
GET /api/customer/export/pdf?activeOnly=true&businessUnitId=1
```

**Export Customers to Excel**
```
GET /api/customer/export/excel?activeOnly=true
```

Exported columns:
- Customer Number
- Name
- Email
- Phone
- Status
- Credit Limit
- Current Balance
- Payment Terms (Excel only)
- Tax Exempt (Excel only)

#### 2. Work Order Exports

**Export Work Orders to PDF**
```
GET /api/workorder/export/pdf
```

**Export Work Orders to Excel**
```
GET /api/workorder/export/excel
```

Exported columns:
- WO #
- Customer
- Description
- Status
- Priority (Excel only)
- Start Date
- End Date
- Est. Cost
- Actual Cost
- % Complete (Excel only)

#### 3. Quote Exports

**Export Quotes to PDF**
```
GET /api/quote/export/pdf
```

**Export Quotes to Excel**
```
GET /api/quote/export/excel
```

Exported columns:
- Quote #
- Customer
- Description
- Status
- Total
- Valid Until
- Created
- Approved (Excel only)

#### 4. Purchase Order Exports

**Export Purchase Orders to PDF**
```
GET /api/purchaseorder/export/pdf?status=Approved
```

**Export Purchase Orders to Excel**
```
GET /api/purchaseorder/export/excel?vendorId=5
```

Exported columns:
- PO #
- Vendor
- Description
- Status
- Total
- Order Date
- Delivery Date
- Received Date (Excel only)

#### 5. Report Exports

**Job Cost Report**
```
GET /api/report/job-cost/export/pdf?startDate=2024-01-01&endDate=2024-12-31
GET /api/report/job-cost/export/excel?customerId=10
```

**AR Aging Report**
```
GET /api/report/ar-aging/export/pdf?asOfDate=2024-12-31
GET /api/report/ar-aging/export/excel
```

**Customer Analysis Report**
```
GET /api/report/customer-analysis/export/pdf?startDate=2024-01-01
GET /api/report/customer-analysis/export/excel?customerId=5
```

## Features

### PDF Export Features

1. **Professional Layout**: 
   - Landscape orientation for wider tables
   - Clear headers with title and generation timestamp
   - Page numbers in footer
   - Color-coded headers with borders

2. **Data Formatting**:
   - Automatic date formatting (YYYY-MM-DD)
   - Decimal/currency formatting (2 decimal places)
   - Boolean values displayed as Yes/No
   - PascalCase property names converted to Title Case with spaces

3. **Pagination**: Automatic page breaks with page numbers

### Excel Export Features

1. **Professional Spreadsheet**:
   - Bold headers with light blue background
   - Cell borders for all data
   - Frozen header row for scrolling
   - Auto-fitted column widths

2. **Data Formatting**:
   - Native data types (numbers, dates, text)
   - Proper date/time formatting
   - Decimal precision preserved
   - Formulas supported (can be extended)

## Usage Examples

### From Frontend (JavaScript/TypeScript)

```javascript
// Download Customer List as PDF
async function downloadCustomersPdf() {
    const response = await fetch('/api/customer/export/pdf?activeOnly=true', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    
    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `Customers_${new Date().toISOString().split('T')[0]}.pdf`;
    document.body.appendChild(a);
    a.click();
    a.remove();
}

// Download Work Orders as Excel
async function downloadWorkOrdersExcel() {
    const response = await fetch('/api/workorder/export/excel', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    
    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `WorkOrders_${new Date().toISOString().split('T')[0]}.xlsx`;
    document.body.appendChild(a);
    a.click();
    a.remove();
}
```

### From cURL

```bash
# Export customers to PDF
curl -X GET "http://localhost:5000/api/customer/export/pdf?activeOnly=true" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  --output customers.pdf

# Export work orders to Excel
curl -X GET "http://localhost:5000/api/workorder/export/excel" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  --output workorders.xlsx

# Export job cost report to PDF with date range
curl -X GET "http://localhost:5000/api/report/job-cost/export/pdf?startDate=2024-01-01&endDate=2024-12-31" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  --output job-cost-report.pdf
```

## Customization

### Adding Export to a New Controller

1. Inject `IExportService` in the controller constructor:

```csharp
private readonly IExportService _exportService;

public YourController(IExportService exportService)
{
    _exportService = exportService;
}
```

2. Add export endpoints:

```csharp
[HttpGet("export/pdf")]
public async Task<IActionResult> ExportToPdf()
{
    var data = await GetYourData();
    
    var columns = new Dictionary<string, string>
    {
        { "PropertyName1", "Display Name 1" },
        { "PropertyName2", "Display Name 2" }
    };
    
    var pdfBytes = _exportService.ExportToPdf(data, "Your Title", columns);
    return File(pdfBytes, "application/pdf", $"YourFile_{DateTime.Now:yyyyMMdd}.pdf");
}

[HttpGet("export/excel")]
public async Task<IActionResult> ExportToExcel()
{
    var data = await GetYourData();
    
    var columns = new Dictionary<string, string>
    {
        { "PropertyName1", "Display Name 1" },
        { "PropertyName2", "Display Name 2" }
    };
    
    var excelBytes = _exportService.ExportToExcel(data, "Sheet Name", columns);
    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
        $"YourFile_{DateTime.Now:yyyyMMdd}.xlsx");
}
```

### Custom Formatting

To add custom formatting for specific data types, modify the `FormatValue` method in `ExportService.cs`:

```csharp
private static string FormatValue(object? value)
{
    if (value == null) return string.Empty;

    return value switch
    {
        DateTime dt => dt.ToString("yyyy-MM-dd"),
        decimal d => d.ToString("N2"),
        // Add your custom formatting here
        YourCustomType custom => custom.FormatForExport(),
        _ => value.ToString() ?? string.Empty
    };
}
```

## Performance Considerations

1. **Large Datasets**: 
   - The current implementation loads all data into memory
   - For very large datasets (>10,000 records), consider implementing pagination in export endpoints
   - Limit exports to 1000 records by default (already implemented in most controllers)

2. **Memory Usage**:
   - PDF generation is more memory-intensive than Excel
   - Consider implementing background jobs for very large exports using a job queue (e.g., Hangfire)

3. **Caching**:
   - Export results are not cached by default
   - Consider adding response caching for frequently exported static data

## Troubleshooting

### Common Issues

1. **"License not set" error**:
   - Ensure `ExcelPackage.LicenseContext = LicenseContext.NonCommercial;` is set in ExportService constructor
   - Ensure `QuestPDF.Settings.License = LicenseType.Community;` is set

2. **Empty/Missing columns**:
   - Check that property names in the columns dictionary match the actual DTO property names (case-sensitive)
   - Verify the data is actually present in the source objects

3. **Formatting issues**:
   - Check the `FormatValue` method for custom type handling
   - Ensure dates and numbers are in the expected format

4. **File not downloading in browser**:
   - Verify the correct MIME type is being returned
   - Check CORS settings if calling from a different origin
   - Ensure the response headers allow file downloads

## License Information

### QuestPDF
- License: Community (free for non-commercial use)
- For commercial use, requires a paid license
- Website: https://www.questpdf.com/license.html

### EPPlus
- License: Polyform Noncommercial 1.0.0
- For commercial use, requires a paid license
- Website: https://epplussoftware.com/Developers/LicenseException

## Future Enhancements

Possible improvements for the export functionality:

1. **Custom Styling**: Allow passing custom styles/themes for PDFs and Excel
2. **Charts and Graphs**: Add support for embedding charts in exports
3. **Multiple Sheets**: Support exporting to Excel with multiple worksheets
4. **Email Integration**: Automatically email exports to users
5. **Scheduled Exports**: Implement background jobs for scheduled report generation
6. **Format Options**: Support additional formats (CSV, JSON, XML)
7. **Compression**: ZIP multiple exports together
8. **Templates**: Support custom PDF/Excel templates
9. **Watermarks**: Add watermarks to PDF exports
10. **Digital Signatures**: Support signing PDF documents

## Support

For issues or questions about the export functionality, please:
1. Check this documentation
2. Review the source code in `Nesi.Application/Services/ExportService.cs`
3. Create an issue in the project repository
