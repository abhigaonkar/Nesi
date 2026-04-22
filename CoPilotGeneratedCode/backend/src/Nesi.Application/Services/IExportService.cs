namespace Nesi.Application.Services;

/// <summary>
/// Service for exporting data to various formats (PDF, Excel)
/// </summary>
public interface IExportService
{
    /// <summary>
    /// Exports data to PDF format
    /// </summary>
    /// <typeparam name="T">Type of data to export</typeparam>
    /// <param name="data">Data to export</param>
    /// <param name="title">Document title</param>
    /// <param name="columns">Column definitions (property name -> display name)</param>
    /// <returns>PDF file as byte array</returns>
    byte[] ExportToPdf<T>(IEnumerable<T> data, string title, Dictionary<string, string> columns);

    /// <summary>
    /// Exports data to Excel format
    /// </summary>
    /// <typeparam name="T">Type of data to export</typeparam>
    /// <param name="data">Data to export</param>
    /// <param name="sheetName">Worksheet name</param>
    /// <param name="columns">Column definitions (property name -> display name)</param>
    /// <returns>Excel file as byte array</returns>
    byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName, Dictionary<string, string> columns);

    /// <summary>
    /// Exports report data to PDF with custom formatting
    /// </summary>
    /// <param name="reportTitle">Title of the report</param>
    /// <param name="reportData">Report data as dictionary</param>
    /// <param name="dateRange">Optional date range for the report</param>
    /// <returns>PDF file as byte array</returns>
    byte[] ExportReportToPdf(string reportTitle, Dictionary<string, object> reportData, string? dateRange = null);
}
