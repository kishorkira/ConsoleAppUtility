using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Models;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing.Chart;

namespace Excel
{
    public class ExcelUtil
    {

        public static string Create(IEnumerable<Todo> todos, string filename, DirectoryInfo dir)
        {
            try
            {
                using (var package = new ExcelPackage())
                {

                    CreateTodoXls(package, todos);
                    CreateTodosXlsxChart(package, todos);
                    CreateTodoXlsxFromCollection(package, todos);
                    package.Workbook.Properties.Title = "Todos";
                    var xlFile = Utils.GetFileInfo(dir, $"{filename}.xlsx");
                    package.SaveAs(xlFile);
                    return xlFile.FullName;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Somthing went wrong";
            }


        }

        private static void CreateTodoXlsxFromCollection(ExcelPackage package, IEnumerable<Todo> todos)
        {
            var worksheet = package.Workbook.Worksheets.Add("From Collection");
            worksheet.Cells["B2"].Value = "User";
            worksheet.Cells["C2"].Value = "Id";
            worksheet.Cells["D2"].Value = "Title";
            worksheet.Cells["E2"].Value = "Completed";
            worksheet.Cells["B3"].LoadFromCollection<Todo>(todos);
            worksheet.Cells.AutoFitColumns();
        }

        static void CreateTodoXls(ExcelPackage package, IEnumerable<Todo> todos)
        {
            var worksheet = package.Workbook.Worksheets.Add("Todos");

            worksheet.Cells["B2"].Value = "Id";
            worksheet.Cells["C2"].Value = "Title";
            worksheet.Cells["D2"].Value = "User";
            worksheet.Cells["E2"].Value = "Completed";
            int row = 3;
            foreach (var todo in todos)
            {
                worksheet.Cells[$"B{row}"].Value = todo.Id;
                worksheet.Cells[$"C{row}"].Value = todo.Title;
                worksheet.Cells[$"D{row}"].Value = todo.UserId;
                worksheet.Cells[$"E{row}"].Value = todo.Completed;

                worksheet.Cells[$"C{row}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"C{row}"].Style.Fill.BackgroundColor.SetColor(todo.Completed ? Color.Green : Color.Red);

                row++;
            }
            worksheet.Cells.AutoFitColumns();
            
            worksheet.View.PageLayoutView = false;
        }

        static void CreateTodosXlsxChart(ExcelPackage package, IEnumerable<Todo> todos)
        {
            var worksheet = package.Workbook.Worksheets.Add("TodosPi");
            var todosGroup = todos
                .GroupBy(t => t.UserId)
                .Select(g => new
                {
                    User = g.Key,
                    Todos = g,
                    Total = g.Count()
                });



            worksheet.Cells["B2"].Value = "User";
            worksheet.Cells["C2"].Value = "Completed";
            worksheet.Cells["D2"].Value = "Pending";
            worksheet.Cells["E2"].Value = "Total";
            worksheet.Cells["E3"].Formula = "=Sum(C3:D3)";

            int row = 3;
            foreach (var group in todosGroup)
            {
                worksheet.Cells[$"B{row}"].Value = group.User;
                worksheet.Cells[$"C{row}"].Value = group.Todos.Where(t => t.Completed).Count();
                worksheet.Cells[$"D{row}"].Value = group.Todos.Where(t => !t.Completed).Count();
                worksheet.Cells[$"E{row}"].Value = group.Total;

                row++;
            }
            worksheet.Cells[$"C{row}"].Formula = $"=Sum(C3:C{row - 1})";
            worksheet.Cells[$"D{row}"].Formula = $"=Sum(D3:D{row - 1})";
            worksheet.Cells[$"E{row}"].Formula = $"=Sum(E3:E{row - 1})";


            var headerRange = new CellsAddressRange(3, 2, 12, 2);
            var completedvalueRange = new CellsAddressRange(3, 3, 12, 3);
            var pendingvalueRange = new CellsAddressRange(3, 4, 12, 4);


            var completedChart = GenerateChart(worksheet, eChartType.Line, "CompletedChart", "Completed", completedvalueRange, headerRange);
            var pendingChart = GenerateChart(worksheet, eChartType.CylinderBarClustered, "PendingChart", "Pending", pendingvalueRange, headerRange);

            //size of the chart
            completedChart.SetSize(500, 400);
            pendingChart.SetSize(500, 400);

            //add the chart at cell 
            completedChart.SetPosition(14, 0, 1, 0);
            pendingChart.SetPosition(14, 0, 10, 0);

        }

        static ExcelChart GenerateChart(ExcelWorksheet worksheet, eChartType charType, string name, string title, CellsAddressRange valueRange, CellsAddressRange headerRange)
        {
            try
            {
                var Chart = worksheet.Drawings.AddChart(name, charType);
                Chart.Title.Text = title;

                //select the ranges for the pie. First the values, then the header range
                Chart.Series.Add(ExcelRange.GetAddress(valueRange.FromRow, valueRange.FromCol, valueRange.ToRow, valueRange.ToCol),
                                ExcelRange.GetAddress(headerRange.FromRow, headerRange.FromCol, headerRange.ToRow, headerRange.ToCol));

                //position of the legend
                Chart.Legend.Position = eLegendPosition.Bottom;

                return Chart;
            }
            catch (Exception )
            {

                return null;
            }

        }
    }
}
