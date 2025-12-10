namespace FarmSync.Application.Interfaces;

public interface IExportService
{
    Task<byte[]> ExportToPdfAsync<T>(T data, string reportTitle);
    Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName);
}
