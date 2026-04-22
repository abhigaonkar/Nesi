using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Nesi.Application.Services;

public class ExportService : IExportService
{
    public ExportService()
    {
        // Set EPPlus license context (required for v5+)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        // Set QuestPDF license (Community license for non-commercial use)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] ExportToPdf<T>(IEnumerable<T> data, string title, Dictionary<string, string> columns)
    {
        var dataList = data.ToList();
        
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(40);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(header =>
                {
                    header.Column(column =>
                    {
                        column.Item().Text(title)
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);
                        
                        column.Item().Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken1);
                        
                        column.Item().PaddingTop(10).LineHorizontal(1)
                            .LineColor(Colors.Grey.Lighten1);
                    });
                });

                page.Content().Element(content =>
                {
                    content.PaddingVertical(10).Table(table =>
                    {
                        // Define columns
                        table.ColumnsDefinition(columnsDefinition =>
                        {
                            foreach (var _ in columns)
                            {
                                columnsDefinition.RelativeColumn();
                            }
                        });

                        // Header row
                        table.Header(header =>
                        {
                            foreach (var column in columns.Values)
                            {
                                header.Cell().Element(CellStyle).Text(column).Bold();
                            }

                            static IContainer CellStyle(IContainer container)
                            {
                                return container
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Background(Colors.Grey.Lighten3)
                                    .Padding(5)
                                    .AlignCenter()
                                    .AlignMiddle();
                            }
                        });

                        // Data rows
                        foreach (var item in dataList)
                        {
                            foreach (var column in columns.Keys)
                            {
                                var value = GetPropertyValue(item, column);
                                table.Cell().Element(CellStyle).Text(value ?? string.Empty);
                            }

                            static IContainer CellStyle(IContainer container)
                            {
                                return container
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(5);
                            }
                        }
                    });
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                    text.Span(" of ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    public byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName, Dictionary<string, string> columns)
    {
        var dataList = data.ToList();
        
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add(sheetName);

        // Add headers
        var col = 1;
        foreach (var column in columns.Values)
        {
            worksheet.Cells[1, col].Value = column;
            worksheet.Cells[1, col].Style.Font.Bold = true;
            worksheet.Cells[1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
            worksheet.Cells[1, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            col++;
        }

        // Add data
        var row = 2;
        foreach (var item in dataList)
        {
            col = 1;
            foreach (var column in columns.Keys)
            {
                var value = GetPropertyValue(item, column);
                worksheet.Cells[row, col].Value = value;
                worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                col++;
            }
            row++;
        }

        // Auto-fit columns
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        // Add freeze pane
        worksheet.View.FreezePanes(2, 1);

        return package.GetAsByteArray();
    }

    public byte[] ExportReportToPdf(string reportTitle, Dictionary<string, object> reportData, string? dateRange = null)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Element(header =>
                {
                    header.Column(column =>
                    {
                        column.Item().Text(reportTitle)
                            .FontSize(24)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);
                        
                        if (!string.IsNullOrEmpty(dateRange))
                        {
                            column.Item().Text(dateRange)
                                .FontSize(11)
                                .FontColor(Colors.Grey.Darken1);
                        }
                        
                        column.Item().Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken1);
                        
                        column.Item().PaddingTop(10).LineHorizontal(1)
                            .LineColor(Colors.Grey.Lighten1);
                    });
                });

                page.Content().Element(content =>
                {
                    content.PaddingVertical(20).Column(column =>
                    {
                        foreach (var section in reportData)
                        {
                            column.Item().PaddingBottom(15).Column(sectionColumn =>
                            {
                                sectionColumn.Item().Text(section.Key)
                                    .FontSize(14)
                                    .Bold()
                                    .FontColor(Colors.Blue.Darken1);
                                
                                sectionColumn.Item().PaddingTop(5).Element(sectionContent =>
                                {
                                    if (section.Value is IEnumerable<object> list && list.Any())
                                    {
                                        var firstItem = list.First();
                                        var properties = firstItem.GetType().GetProperties();
                                        
                                        sectionContent.Table(table =>
                                        {
                                            table.ColumnsDefinition(columnsDefinition =>
                                            {
                                                foreach (var _ in properties)
                                                {
                                                    columnsDefinition.RelativeColumn();
                                                }
                                            });

                                            // Header
                                            table.Header(header =>
                                            {
                                                foreach (var prop in properties)
                                                {
                                                    header.Cell().Element(CellStyle)
                                                        .Text(FormatPropertyName(prop.Name)).Bold();
                                                }
                                            });

                                            // Data
                                            foreach (var item in list)
                                            {
                                                foreach (var prop in properties)
                                                {
                                                    var value = prop.GetValue(item);
                                                    table.Cell().Element(CellStyle).Text(FormatValue(value));
                                                }
                                            }
                                        });
                                    }
                                    else
                                    {
                                        sectionContent.Text(FormatValue(section.Value));
                                    }
                                });
                            });
                        }
                    });
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                    text.Span(" of ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();

        static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(5);
        }
    }

    private static string? GetPropertyValue<T>(T obj, string propertyName)
    {
        if (obj == null) return null;

        var property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (property == null) return null;

        var value = property.GetValue(obj);
        return FormatValue(value);
    }

    private static string FormatValue(object? value)
    {
        if (value == null) return string.Empty;

        return value switch
        {
            DateTime dt => dt.ToString("yyyy-MM-dd"),
            decimal d => d.ToString("N2"),
            double dbl => dbl.ToString("N2"),
            float f => f.ToString("N2"),
            bool b => b ? "Yes" : "No",
            _ => value.ToString() ?? string.Empty
        };
    }

    private static string FormatPropertyName(string propertyName)
    {
        // Convert PascalCase to Title Case with spaces
        if (string.IsNullOrEmpty(propertyName)) return propertyName;
        
        var result = new System.Text.StringBuilder();
        result.Append(char.ToUpper(propertyName[0]));
        
        for (int i = 1; i < propertyName.Length; i++)
        {
            if (char.IsUpper(propertyName[i]) && i > 0 && !char.IsUpper(propertyName[i - 1]))
            {
                result.Append(' ');
            }
            result.Append(propertyName[i]);
        }
        
        return result.ToString();
    }
}
