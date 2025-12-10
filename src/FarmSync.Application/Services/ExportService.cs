using ClosedXML.Excel;
using FarmSync.Application.DTOs.Reports;
using FarmSync.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection;

namespace FarmSync.Application.Services;

public class ExportService : IExportService
{
    public ExportService()
    {
        // Set QuestPDF license for development
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> ExportToPdfAsync<T>(T data, string reportTitle)
    {
        return await Task.Run(() =>
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .AlignCenter()
                        .Text(reportTitle)
                        .SemiBold()
                        .FontSize(20)
                        .FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            // Add report generation info
                            column.Item().Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                            column.Item().PaddingTop(10);

                            // Render content based on type
                            if (data is PurchaseOrderSummaryDto poSummary)
                            {
                                RenderPurchaseOrderReport(column, poSummary);
                            }
                            else if (data is InventoryValuationDto invValuation)
                            {
                                RenderInventoryValuationReport(column, invValuation);
                            }
                            else if (data is SupplierTransactionSummaryDto suppTransactions)
                            {
                                RenderSupplierTransactionReport(column, suppTransactions);
                            }
                            else if (data is ExpenseSummaryDto expenseSummary)
                            {
                                RenderExpenseReport(column, expenseSummary);
                            }
                            else if (data is GoodsReceivedSummaryDto grnSummary)
                            {
                                RenderGoodsReceivedReport(column, grnSummary);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                        });
                });
            });

            return document.GeneratePdf();
        });
    }

    public async Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName)
    {
        return await Task.Run(() =>
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            if (!data.Any())
            {
                worksheet.Cell(1, 1).Value = "No data available";
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }

            // Get properties
            var properties = typeof(T).GetProperties()
                .Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string))
                .ToList();

            // Add headers
            for (int i = 0; i < properties.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
            }

            // Add data
            int row = 2;
            foreach (var item in data)
            {
                for (int col = 0; col < properties.Count; col++)
                {
                    var value = properties[col].GetValue(item);
                    if (value != null)
                    {
                        if (value is DateTime dateTime)
                        {
                            worksheet.Cell(row, col + 1).Value = dateTime;
                            worksheet.Cell(row, col + 1).Style.DateFormat.Format = "yyyy-mm-dd";
                        }
                        else if (value is decimal || value is double || value is float)
                        {
                            worksheet.Cell(row, col + 1).Value = Convert.ToDouble(value);
                            worksheet.Cell(row, col + 1).Style.NumberFormat.Format = "#,##0.00";
                        }
                        else
                        {
                            worksheet.Cell(row, col + 1).Value = value.ToString();
                        }
                    }
                }
                row++;
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            using var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            return memoryStream.ToArray();
        });
    }

    private void RenderPurchaseOrderReport(ColumnDescriptor column, PurchaseOrderSummaryDto summary)
    {
        // Summary section
        column.Item().Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text($"Total Orders: {summary.TotalOrders}").Bold();
                col.Item().Text($"Total Amount: R {summary.TotalAmount:N2}").Bold();
            });
            row.RelativeItem().Column(col =>
            {
                col.Item().Text($"Pending: {summary.PendingOrders}");
                col.Item().Text($"Approved: {summary.ApprovedOrders}");
                col.Item().Text($"Completed: {summary.CompletedOrders}");
            });
        });

        column.Item().PaddingTop(20);

        // Table
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(80);  // PO Number
                columns.ConstantColumn(70);  // Date
                columns.RelativeColumn();    // Supplier
                columns.ConstantColumn(60);  // Status
                columns.ConstantColumn(80);  // Amount
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("PO Number").Bold();
                header.Cell().Element(CellStyle).Text("Date").Bold();
                header.Cell().Element(CellStyle).Text("Supplier").Bold();
                header.Cell().Element(CellStyle).Text("Status").Bold();
                header.Cell().Element(CellStyle).Text("Amount").Bold();
            });

            foreach (var order in summary.Orders)
            {
                table.Cell().Element(CellStyle).Text(order.PoNumber);
                table.Cell().Element(CellStyle).Text(order.OrderDate.ToString("yyyy-MM-dd"));
                table.Cell().Element(CellStyle).Text(order.SupplierName);
                table.Cell().Element(CellStyle).Text(order.Status);
                table.Cell().Element(CellStyle).Text($"R {order.TotalAmount:N2}");
            }
        });
    }

    private void RenderInventoryValuationReport(ColumnDescriptor column, InventoryValuationDto valuation)
    {
        // Summary section
        column.Item().Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text($"Total Value: R {valuation.TotalInventoryValue:N2}").Bold().FontSize(14);
                col.Item().Text($"Total Items: {valuation.TotalItems}");
            });
            row.RelativeItem().Column(col =>
            {
                col.Item().Text($"Low Stock: {valuation.LowStockItems}").FontColor(Colors.Orange.Medium);
                col.Item().Text($"Out of Stock: {valuation.OutOfStockItems}").FontColor(Colors.Red.Medium);
            });
        });

        column.Item().PaddingTop(20);
        column.Item().Text("Category Breakdown").Bold().FontSize(12);
        
        // Category table
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();    // Category
                columns.ConstantColumn(80);  // Items
                columns.ConstantColumn(100); // Value
                columns.ConstantColumn(80);  // Percentage
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Category").Bold();
                header.Cell().Element(CellStyle).Text("Items").Bold();
                header.Cell().Element(CellStyle).Text("Value").Bold();
                header.Cell().Element(CellStyle).Text("Percentage").Bold();
            });

            foreach (var cat in valuation.CategoryBreakdown)
            {
                table.Cell().Element(CellStyle).Text(cat.CategoryName);
                table.Cell().Element(CellStyle).Text(cat.ItemCount.ToString());
                table.Cell().Element(CellStyle).Text($"R {cat.TotalValue:N2}");
                table.Cell().Element(CellStyle).Text($"{cat.Percentage:N1}%");
            }
        });
    }

    private void RenderSupplierTransactionReport(ColumnDescriptor column, SupplierTransactionSummaryDto summary)
    {
        column.Item().Text($"Supplier: {summary.SupplierName}").Bold().FontSize(14);
        
        column.Item().PaddingTop(10).Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text($"Total Purchases: R {summary.TotalPurchases:N2}");
                col.Item().Text($"Total Payments: R {summary.TotalPayments:N2}");
            });
            row.RelativeItem().Column(col =>
            {
                col.Item().Text($"Current Balance: R {summary.CurrentBalance:N2}").Bold().FontSize(12);
                col.Item().Text($"Transactions: {summary.TransactionCount}");
            });
        });

        column.Item().PaddingTop(20);

        column.Item().Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(70);  // Date
                columns.ConstantColumn(80);  // Type
                columns.ConstantColumn(90);  // Reference
                columns.ConstantColumn(80);  // Amount
                columns.ConstantColumn(80);  // Balance
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Date").Bold();
                header.Cell().Element(CellStyle).Text("Type").Bold();
                header.Cell().Element(CellStyle).Text("Reference").Bold();
                header.Cell().Element(CellStyle).Text("Amount").Bold();
                header.Cell().Element(CellStyle).Text("Balance").Bold();
            });

            foreach (var trans in summary.Transactions)
            {
                table.Cell().Element(CellStyle).Text(trans.TransactionDate.ToString("yyyy-MM-dd"));
                table.Cell().Element(CellStyle).Text(trans.TransactionType);
                table.Cell().Element(CellStyle).Text(trans.ReferenceNumber);
                table.Cell().Element(CellStyle).Text($"R {trans.Amount:N2}");
                table.Cell().Element(CellStyle).Text($"R {trans.Balance:N2}");
            }
        });
    }

    private void RenderExpenseReport(ColumnDescriptor column, ExpenseSummaryDto summary)
    {
        column.Item().Text($"Total Expenses: R {summary.TotalExpenses:N2}").Bold().FontSize(14);
        column.Item().Text($"Period: {summary.StartDate:yyyy-MM-dd} to {summary.EndDate:yyyy-MM-dd}");

        column.Item().PaddingTop(20);
        column.Item().Text("Expenses by Category").Bold().FontSize(12);
        
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();    // Category
                columns.ConstantColumn(100); // Amount
                columns.ConstantColumn(80);  // Count
                columns.ConstantColumn(80);  // Percentage
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Category").Bold();
                header.Cell().Element(CellStyle).Text("Amount").Bold();
                header.Cell().Element(CellStyle).Text("Count").Bold();
                header.Cell().Element(CellStyle).Text("Percentage").Bold();
            });

            foreach (var cat in summary.CategoryBreakdown)
            {
                table.Cell().Element(CellStyle).Text(cat.CategoryName);
                table.Cell().Element(CellStyle).Text($"R {cat.TotalAmount:N2}");
                table.Cell().Element(CellStyle).Text(cat.TransactionCount.ToString());
                table.Cell().Element(CellStyle).Text($"{cat.Percentage:N1}%");
            }
        });
    }

    private void RenderGoodsReceivedReport(ColumnDescriptor column, GoodsReceivedSummaryDto summary)
    {
        column.Item().Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text($"Total GRNs: {summary.TotalGrns}").Bold();
                col.Item().Text($"Total Value: R {summary.TotalValue:N2}").Bold();
            });
            row.RelativeItem().Column(col =>
            {
                col.Item().Text($"Pending: {summary.PendingInspections}");
                col.Item().Text($"Approved: {summary.Approved}");
            });
        });

        column.Item().PaddingTop(20);

        column.Item().Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(80);  // GRN Number
                columns.ConstantColumn(70);  // Date
                columns.ConstantColumn(80);  // PO Number
                columns.RelativeColumn();    // Supplier
                columns.ConstantColumn(80);  // Value
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("GRN Number").Bold();
                header.Cell().Element(CellStyle).Text("Date").Bold();
                header.Cell().Element(CellStyle).Text("PO Number").Bold();
                header.Cell().Element(CellStyle).Text("Supplier").Bold();
                header.Cell().Element(CellStyle).Text("Value").Bold();
            });

            foreach (var grn in summary.GoodsReceived)
            {
                table.Cell().Element(CellStyle).Text(grn.GrnNumber);
                table.Cell().Element(CellStyle).Text(grn.ReceivedDate.ToString("yyyy-MM-dd"));
                table.Cell().Element(CellStyle).Text(grn.PoNumber);
                table.Cell().Element(CellStyle).Text(grn.SupplierName);
                table.Cell().Element(CellStyle).Text($"R {grn.TotalValue:N2}");
            }
        });
    }

    private static IContainer CellStyle(IContainer container)
    {
        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5);
    }
}
