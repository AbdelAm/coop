using ClosedXML.Excel;
using coop2._0.Entities;
using coop2._0.Model;
using coop2._0.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coop2._0.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const string SpreadsheetId = "1Br7-Uapf_0UKB5UrUk1vHko55KKBwfxeztnVydJ7eh8";
        private const string SheetName = "CoopHalal";
        SpreadsheetsResource.ValuesResource _googleSheetValues;

        public TransactionsController(ITransactionService transactionService, IWebHostEnvironment webHostEnvironment,
            GoogleSheetsHelper googleSheetsHelper)
        {
            this._transactionService = transactionService;
            _webHostEnvironment = webHostEnvironment;
            _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<object> GetTransactions([FromQuery] PaginationFilter filter)
        {
            return await _transactionService.GetAllTransactions(filter);
        }

        [HttpGet("filter/{status}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<object> GetAllTransactionsByStatus([FromQuery] PaginationFilter filter, Status status)

        {
            return await _transactionService.GetAllTransactionsByStatus(status, filter);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransaction(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpGet("user/{userBankAccountId:int}")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<object> GetTransactionsByUser([FromQuery] PaginationFilter filter
            , int userBankAccountId)
        {
            return await _transactionService.GetTransactionsByUser(userBankAccountId, filter);
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> RemoveTransaction(int id)
        {
            return await _transactionService.RemoveTransaction(id);
            ;
        }

        [HttpGet("reject/{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Transaction>> RejectTransaction(int id)
        {
            return await _transactionService.RejectTransaction(id);
        }

        [HttpGet("validate/{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Transaction>> ValidateTransaction(int id)
        {
            return await _transactionService.ValidateTransaction(id);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<ActionResult<Transaction>> AddTransaction(TransactionModel transactionModel)
        {
            return await _transactionService.AddTransaction(transactionModel);
        }


        [HttpPost]
        [Route("validate-all")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> ValidateAllTransactions(List<int> transactionsIds)
        {
            if (!transactionsIds.Any()) return BadRequest();

            foreach (var transactionId in transactionsIds)
            {
                await _transactionService.ValidateTransaction(transactionId);
            }

            return Ok();
        }

        [HttpPost]
        [Route("reject-all")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> RejectAllTransactions(List<int> transactionsIds)
        {
            if (!transactionsIds.Any()) return BadRequest();

            foreach (var transactionId in transactionsIds)
            {
                await _transactionService.RejectTransaction(transactionId);
            }

            return Ok();
        }

        [HttpPost]
        [Route("remove-all")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> RemoveAllTransactions(List<int> transactionsIds)
        {
            if (!transactionsIds.Any()) return BadRequest();

            foreach (var transactionId in transactionsIds)
            {
                await _transactionService.RemoveTransaction(transactionId);
            }

            return Ok();
        }

        [HttpGet("search/{keyword}")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<object> SearchForTransactions([FromQuery] PaginationFilter filter, string keyword)
        {
            return await _transactionService.SearchForTransactions(keyword, filter);
        }

        [HttpGet("csv/{bankAccountId:int}")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> ExportToCsv(int bankAccountId)
        {
            StringBuilder sb = new StringBuilder();
            var transactions = await GetUserTransactionsAsDataTable(bankAccountId);
            IEnumerable<string> columnNames =
                transactions.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in transactions.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                    string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Transaction.csv");
        }

        [HttpGet("excel/{bankAccountId:int}")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> ExportToExcel(int bankAccountId)
        {
            var transactions = await GetUserTransactionsAsDataTable(bankAccountId);

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Transactions");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Monto";
            worksheet.Cell(currentRow, 2).Value = "Fetcha";
            worksheet.Cell(currentRow, 3).Value = "Concepto";
            worksheet.Cell(currentRow, 4).Value = "Cuenta original";
            worksheet.Cell(currentRow, 5).Value = "Cuenta de destino";
            worksheet.Cell(currentRow, 6).Value = "Estado";


            foreach (DataRow transaction in transactions.Rows)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = transaction["Monto"].ToString();
                worksheet.Cell(currentRow, 2).Value = transaction["Fetcha"].ToString();
                worksheet.Cell(currentRow, 3).Value = transaction["Concepto"].ToString();
                worksheet.Cell(currentRow, 4).Value = transaction["Cuenta original"].ToString();
                worksheet.Cell(currentRow, 5).Value = transaction["Cuenta de destino"].ToString();
                worksheet.Cell(currentRow, 6).Value = TranslateStatus(transaction["Estado"].ToString());
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Transactions.xlsx");
        }

        [HttpGet("pdf/{bankAccountId:int}")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<FileStream> ExportToPdf(int bankAccountId)
        {
            var transactions = await GetUserTransactionsAsDataTable(bankAccountId);

            if (transactions.Rows.Count <= 0) return null;

            int pdfRowIndex = 1;
            string filename = "transactions-" + DateTime.Now.ToString("dd-MM-yyyy hh_mm_s_tt");
            string filepath = _webHostEnvironment.WebRootPath + "" + filename + ".pdf";
            Document document = new Document(PageSize.A4, 5f, 5f, 10f, 10f);
            FileStream fs = new FileStream(filepath, FileMode.Create);
            document.AddAuthor("CoopHalal");
            document.AddCreationDate();
            document.AddTitle("Lista de transacciones de los últimos 3 meses");
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            document.Add(new Phrase("Lista de transacciones de los últimos 3 meses",
                FontFactory.GetFont(FontFactory.TIMES_BOLD, 12)));
            Font font1 = FontFactory.GetFont(FontFactory.COURIER_BOLD, 10);
            Font font2 = FontFactory.GetFont(FontFactory.COURIER, 8);

            float[] columnDefinitionSize = { 2F, 3F, 2F, 2F, 2F, 2F };

            var table = new PdfPTable(columnDefinitionSize)
            {
                WidthPercentage = 100
            };

            var cell = new PdfPCell
            {
                BackgroundColor = new BaseColor(0xC0, 0xC0, 0xC0)
            };

            table.AddCell(new Phrase("Monto", font1));
            table.AddCell(new Phrase("Fetcha", font1));
            table.AddCell(new Phrase("Concepto", font1));
            table.AddCell(new Phrase("Cuenta original", font1));
            table.AddCell(new Phrase("Cuenta de destino", font1));
            table.AddCell(new Phrase("Estado", font1));
            table.HeaderRows = 1;

            foreach (DataRow data in transactions.Rows)
            {
                table.AddCell(new Phrase(data["Monto"].ToString(), font2));
                table.AddCell(new Phrase(data["Fetcha"].ToString(), font2));
                table.AddCell(new Phrase(data["Concepto"].ToString(), font2));
                table.AddCell(new Phrase(data["Cuenta original"].ToString(), font2));
                table.AddCell(new Phrase(data["Cuenta de destino"].ToString(), font2));
                table.AddCell(new Phrase(TranslateStatus(data["Estado"].ToString()), font2));

                pdfRowIndex++;
            }

            document.Add(table);
            document.Close();
            document.CloseDocument();
            document.Dispose();
            writer.Close();
            writer.Dispose();
            fs.Close();
            fs.Dispose();

            FileStream sourceFile = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.None, 4096,
                FileOptions.DeleteOnClose);

            return sourceFile;
        }

        [HttpGet("admin/csv")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ExportToCsv()
        {
            StringBuilder sb = new StringBuilder();
            var transactions = await GetAllTransactionsAsDataTable();
            IEnumerable<string> columnNames =
                transactions.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in transactions.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                    string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Transaction.csv");
        }

        [HttpGet("admin/excel")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ExportToExcel()
        {
            var transactions = await GetAllTransactionsAsDataTable();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Transactions");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Monto";
            worksheet.Cell(currentRow, 2).Value = "Fetcha";
            worksheet.Cell(currentRow, 3).Value = "Concepto";
            worksheet.Cell(currentRow, 4).Value = "Cuenta original";
            worksheet.Cell(currentRow, 5).Value = "Cuenta de destino";
            worksheet.Cell(currentRow, 6).Value = "Estado";


            foreach (DataRow transaction in transactions.Rows)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = transaction["Monto"].ToString();
                worksheet.Cell(currentRow, 2).Value = transaction["Fetcha"].ToString();
                worksheet.Cell(currentRow, 3).Value = transaction["Concepto"].ToString();
                worksheet.Cell(currentRow, 4).Value = transaction["Cuenta original"].ToString();
                worksheet.Cell(currentRow, 5).Value = transaction["Cuenta de destino"].ToString();
                worksheet.Cell(currentRow, 6).Value = TranslateStatus(transaction["Estado"].ToString());
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Transactions.xlsx");
        }

        [HttpGet("admin/pdf")]
        [Authorize(Roles = "ADMIN")]
        public async Task<FileStream> ExportToPdf()
        {
            var transactions = await GetAllTransactionsAsDataTable();

            if (transactions.Rows.Count <= 0) return null;

            int pdfRowIndex = 1;
            string filename = "transactions-" + DateTime.Now.ToString("dd-MM-yyyy hh_mm_s_tt");
            string filepath = _webHostEnvironment.WebRootPath + "" + filename + ".pdf";
            Document document = new Document(PageSize.A4, 5f, 5f, 10f, 10f);
            FileStream fs = new FileStream(filepath, FileMode.Create);
            document.AddAuthor("CoopHalal");
            document.AddCreationDate();
            document.AddTitle("Lista de transacciones de los últimos 3 meses");
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            document.Add(new Phrase("Lista de transacciones de los últimos 3 meses",
                FontFactory.GetFont(FontFactory.TIMES_BOLD, 12)));
            Font font1 = FontFactory.GetFont(FontFactory.COURIER_BOLD, 10);
            Font font2 = FontFactory.GetFont(FontFactory.COURIER, 8);


            float[] columnDefinitionSize = { 2F, 3F, 2F, 2F, 2F, 2F };
            PdfPTable table;
            PdfPCell cell;

            table = new PdfPTable(columnDefinitionSize)
            {
                WidthPercentage = 100
            };

            cell = new PdfPCell
            {
                BackgroundColor = new BaseColor(0xC0, 0xC0, 0xC0)
            };

            table.AddCell(new Phrase("Monto", font1));
            table.AddCell(new Phrase("Fetcha", font1));
            table.AddCell(new Phrase("Concepto", font1));
            table.AddCell(new Phrase("Cuenta original", font1));
            table.AddCell(new Phrase("Cuenta de destino", font1));
            table.AddCell(new Phrase("Estado", font1));
            table.HeaderRows = 1;

            foreach (DataRow data in transactions.Rows)
            {
                table.AddCell(new Phrase(data["Monto"].ToString(), font2));
                table.AddCell(new Phrase(data["Fetcha"].ToString(), font2));
                table.AddCell(new Phrase(data["Concepto"].ToString(), font2));
                table.AddCell(new Phrase(data["Cuenta original"].ToString(), font2));
                table.AddCell(new Phrase(data["Cuenta de destino"].ToString(), font2));
                table.AddCell(new Phrase(TranslateStatus(data["Estado"].ToString()), font2));

                pdfRowIndex++;
            }

            document.Add(table);
            document.Close();
            document.CloseDocument();
            document.Dispose();
            writer.Close();
            writer.Dispose();
            fs.Close();
            fs.Dispose();

            FileStream sourceFile = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.None, 4096,
                FileOptions.DeleteOnClose);

            return sourceFile;
        }

        [HttpGet("sheet")]
        public async Task ExportToGoogleSheets()
        {
            var range = "A:H";
            IList<TransactionResponse> transactions = (IList<TransactionResponse>)await _transactionService.GetAllTransactions();
            var rangeData = new List<IList<object>> { transactions as List<object> };
            var valueRange = new ValueRange
            {
                Values = rangeData
            };

            var appendRequest = _googleSheetValues.Append( valueRange, SpreadsheetId, range);
            appendRequest.ValueInputOption =
                SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            appendRequest.Execute();
        }


        private async Task<DataTable> GetUserTransactionsAsDataTable(int bankAccountId)
        {
            var transactions = await _transactionService.GetTransactionsByUser(bankAccountId);

            DataTable dtTransactions = new DataTable("Transactions");

            dtTransactions.Columns.AddRange(new DataColumn[6]
            {
                new DataColumn("Monto"), new DataColumn("Fetcha"), new DataColumn("Concepto"),
                new DataColumn("Cuenta original"), new DataColumn("Cuenta de destino"), new DataColumn("Estado")
            });

            foreach (var transaction in transactions)
            {
                dtTransactions.Rows.Add(transaction.Amount, transaction.DateTransaction, transaction.Motif,
                    transaction.SenderBankAccount.User.Name, transaction.ReceiverBankAccount.User.Name,
                    transaction.Status);
            }

            return dtTransactions;
        }

        private async Task<DataTable> GetAllTransactionsAsDataTable()
        {
            var transactions = await _transactionService.GetAllTransactions();

            DataTable dtTransactions = new DataTable("Transactions");

            dtTransactions.Columns.AddRange(new DataColumn[6]
            {
                new DataColumn("Monto"), new DataColumn("Fetcha"), new DataColumn("Concepto"),
                new DataColumn("Cuenta original"), new DataColumn("Cuenta de destino"), new DataColumn("Estado")
            });

            foreach (var transaction in transactions)
            {
                dtTransactions.Rows.Add(transaction.Amount, transaction.DateTransaction, transaction.Motif,
                    transaction.SenderName, transaction.ReceiverName,
                    transaction.Status);
            }

            return dtTransactions;
        }

        private string TranslateStatus(string status)
        {
            return status switch
            {
                "Progress" => "En progreso",
                "Approuved" => "Aprobado",
                "Rejected" => "Rechazado",
                _ => null
            };
        }
    }
}