using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SQL.NoSQL.BLL.Common.DTO;
using SQL.NoSQL.BLL.MixedAcces.Repository;
using SQL.NoSQL.BLL.NoSQL.Repository;
using SQL.NoSQL.BLL.SQL.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Routing;

namespace SQL.NoSQL.WEB.Controllers
{

    public class MeasuredMethod : IDisposable
    {
        private string name;
        private Stopwatch timer;
        public MeasuredMethod(string name)
        {
            this.name = name;
            timer = new Stopwatch();
            timer.Start();
        }
        public void Dispose()
        {
            timer.Stop();
            ExportPerformanceLog.Send(name,timer.ElapsedMilliseconds+ " ms");
        }
    }

    [RoutePrefix("api/Excel")]
    public class ExcelReportController : ApiController
    {

        [HttpGet]
        [Route("SQLReport")]
        public HttpResponseMessage SQLReport()
        {
            HttpResponseMessage data;
            using (MeasuredMethod m = new MeasuredMethod("SQLReport"))
            {

                XSSFWorkbook workbook = new XSSFWorkbook();
                SQLAppRepository appRepo = new SQLAppRepository();
                SQLLogRepository logRepo = new SQLLogRepository();
                List<AppDto> apps = appRepo.GetAll();
                foreach (AppDto app in apps)
                {
                    ISheet worksheet = workbook.CreateSheet(app.Name);
                    List<LogDto> logs = logRepo.GetLogsByAppId(app.Id);
                    BuildExcel(ref worksheet, logs);
                }

                data= SendExcel(workbook, "SQLReport.xlsx");
            }
            return data;
        }

        [HttpGet]
        [Route("NoSQLReport")]
        public HttpResponseMessage NoSQLReport()
        {
            HttpResponseMessage data;
            using (MeasuredMethod m = new MeasuredMethod("NoSQLReport"))
            {

                XSSFWorkbook workbook = new XSSFWorkbook();
                NoSQLAppRepository appRepo = new NoSQLAppRepository();
                NoSQLLogRepository logRepo = new NoSQLLogRepository();
                List<AppDto> apps = appRepo.GetAll();
                foreach (AppDto app in apps)
                {
                    ISheet worksheet = workbook.CreateSheet(app.Name);
                    List<LogDto> logs = logRepo.GetLogsByAppId(app.Id);
                    BuildExcel(ref worksheet, logs);
                }



                 data = SendExcel(workbook, "NoSQLReport.xlsx");

            }
            return data;
        }

        [HttpGet]
        [Route("MixedReport")]
        public HttpResponseMessage MixedReport()
        {
            HttpResponseMessage data;
            using (MeasuredMethod m = new MeasuredMethod("MixedReport"))
            {
                XSSFWorkbook workbook = new XSSFWorkbook();
                AppRepository appRepo = new AppRepository();
                LogRepository logRepo = new LogRepository();
                List<AppDto> apps = appRepo.GetAll();
                foreach (AppDto app in apps)
                {
                    ISheet worksheet = workbook.CreateSheet(app.Name);
                    List<LogDto> logs = logRepo.GetLogsByAppId(app.Id);
                    BuildExcel(ref worksheet, logs);
                }


                data= SendExcel(workbook, "MixedReport.xlsx");
            }
            return data;
        }

        private static void BuildExcel(ref ISheet worksheet, List<LogDto> logs)
        {
            //Header
            IRow headRow = worksheet.CreateRow(0);
            ICellStyle cellStyle;

            BuildHeaderStyle(headRow, out cellStyle);

            ICell headerCell = headRow.CreateCell(0);
            headerCell.SetCellValue("Data Log");
            headerCell.SetCellType(CellType.String);
            headerCell.CellStyle = cellStyle;

            ICell headerCell1 = headRow.CreateCell(1);
            headerCell1.SetCellValue("Livello");
            headerCell1.SetCellType(CellType.String);
            headerCell1.CellStyle = cellStyle;

            ICell headerCell2 = headRow.CreateCell(2);
            headerCell2.SetCellValue("Messaggio");
            headerCell2.SetCellType(CellType.String);
            headerCell2.CellStyle = cellStyle;

            //row
            ICellStyle bodyStyle;
            BuildBodyStyle(headRow, out bodyStyle);

            for (int rownum = 0; rownum < logs.Count; rownum++)
            {
                IRow row = worksheet.CreateRow(rownum + 1);
                LogDto log = logs[rownum];
                ICell cell = row.CreateCell(0);
                cell.SetCellValue(log.LogDate.ToString("dd/MM/yyyy HH:mm:ss"));
                cell.SetCellType(CellType.String);
                cell.CellStyle = bodyStyle;

                ICell cell1 = row.CreateCell(1);
                cell1.SetCellValue(log.Level);
                cell1.SetCellType(CellType.String);
                cell1.CellStyle = bodyStyle;

                ICell cell2 = row.CreateCell(2);
                cell2.SetCellValue(log.Message);
                cell2.SetCellType(CellType.String);
                cell2.CellStyle = bodyStyle;
            }

            worksheet.SetColumnWidth(0, 20 * 256);
            worksheet.SetColumnWidth(1, 10 * 256);
            worksheet.SetColumnWidth(2, 160 * 256);
        }

        private static void BuildBodyStyle(IRow headRow, out ICellStyle bodyStyle)
        {
            bodyStyle = headRow.Sheet.Workbook.CreateCellStyle();
            IFont bodyfont = headRow.Sheet.Workbook.CreateFont();
            bodyfont.Boldweight = 0;
            bodyfont.FontHeightInPoints = ((short)10);
            bodyfont.IsItalic = false;
            bodyStyle.Alignment = HorizontalAlignment.Left;
            bodyStyle.WrapText = true;
            bodyStyle.BorderBottom = BorderStyle.Medium;
            bodyStyle.BorderTop = BorderStyle.Medium;
            bodyStyle.BorderLeft = BorderStyle.Medium;
            bodyStyle.BorderRight = BorderStyle.Medium;
            bodyStyle.SetFont(bodyfont);
        }

        private static void BuildHeaderStyle(IRow headRow, out ICellStyle cellStyle)
        {
            cellStyle = headRow.Sheet.Workbook.CreateCellStyle();
            IFont font = headRow.Sheet.Workbook.CreateFont();
            font.Boldweight = (short)((true) ? 2 : 0);
            font.FontHeightInPoints = ((short)14);
            font.IsItalic = true;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.WrapText = true;
            cellStyle.BorderBottom = BorderStyle.Medium;
            cellStyle.BorderTop = BorderStyle.Medium;
            cellStyle.BorderLeft = BorderStyle.Medium;
            cellStyle.BorderRight = BorderStyle.Medium;
            cellStyle.FillForegroundColor = IndexedColors.Green.Index;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.SetFont(font);
        }

        private HttpResponseMessage SendExcel(IWorkbook workbook,string excelName)
        {
            MemoryStream reader = new MemoryStream();

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                byte[] bytes = ms.ToArray();
                reader.Write(bytes, 0, bytes.Length);
                reader.Flush();
            }
            reader.Seek(0, SeekOrigin.Begin);

            return SendFile(reader, "application/vnd.ms-excel", excelName, true);
        }

        public HttpResponseMessage SendFile(Stream stream, string filetype, string filename, bool forceDownload)
        {


            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(filetype ?? "application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(((forceDownload) ? "attachment" : "inline"))
            {
                FileName = filename.Replace("+", "%20")
            };


            return result;
        }
    }
}
