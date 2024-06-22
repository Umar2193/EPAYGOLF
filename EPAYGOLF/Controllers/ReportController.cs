using Aspose.Html.Saving;
using Aspose.Html;
using DinkToPdf;
using DinkToPdf.Contracts;
using Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using Repository.Products;
using Repository.Redemptions;
using Repository.Sales;
using Repository.SalesStore;
using Repository.Settings;
using Repository.StoresRedeem;
using Repository.VATRates;
using System.Globalization;

namespace EPAYGOLF.Controllers
{
	public class ReportController : Controller
	{
		private ISalesRepository _salesRepository = new SalesRepository();
		private IRedemptionsRepository _redemptionsRepository = new RedemptionsRepository();
		private IProductRepository _productRepository = new ProductRepository();

		private IVATRatesRepository _vatratesRepository = new VATRatesRepository();
		private IStoresRedeemRepository _storesredeemRepository = new StoresRedeemRepository();
		private ISalesStoreRepository _salesstoreRepository = new SalesStoreRepository();
		private ISettingsRepository _settingsRepository = new SettingsRepository();
		private readonly ViewRenderingService _viewRenderingService;
		private readonly IWebHostEnvironment _env;
		public ReportController(ViewRenderingService viewRenderingService, IWebHostEnvironment env)
		{
			_viewRenderingService = viewRenderingService;
			_env = env;
		}
		public IActionResult Index()
		{
			ViewBag.AllProducts = _productRepository.GetProductList().OrderBy(x => x.ProductEAN).ToList();
			ViewBag.AllSalesStore = _salesstoreRepository.GetSalesStoreList().OrderBy(x => x.RetailerID).ToList();
			ViewBag.AllRedemStore = _storesredeemRepository.GetStoresRedeemList().OrderBy(x => x.StoreNo).ToList();
			var getsettinglist = _settingsRepository.GetSettingsList().FirstOrDefault();
			var _liabilitypct = (decimal)0.0;
			var _yearStatDate = "";//"01/01" + DateTime.Now.AddYears(-1).Year;
			if (getsettinglist != null)
			{
				_liabilitypct = getsettinglist.liabilitypct;
				if (getsettinglist.YearStartDate != null)
				{
					var _dtYearStartDate = getsettinglist.YearStartDate.Value.ToString("dd/MM/yyyy");
					_yearStatDate = DateTime.ParseExact(_dtYearStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd/MM");
				}
			}

			ViewBag.configliabilitypct = _liabilitypct;
			ViewBag.configyearStatDate = _yearStatDate;

			return View();
		}
		[HttpPost]
		public async Task<ActionResult> GenerateDetailedReport(ReportSearchParam _request)
		{
			//return Json(new { issalesReportGenerated = false, isredempReportGenerated = false });
			var result = 0;
			string renderedSalesReportView = "";
			string errormessage = "";
			try
			{
				DataController dataController = new DataController();
				var salestran = dataController.TransformSalesData();
				var redemtran = dataController.TransformRedeemData();
				var salesreportresult = _salesRepository.GetSalesListReport(_request.productid, _request.salesstoreno, _request.startDate.Value, _request.endDate.Value).ToList();
				var redempreportresult = _redemptionsRepository.GetRedemptionsListReport(_request.productid, _request.redemstoreno, _request.startDate.Value, _request.endDate.Value).ToList();
				//if (salesreportresult != null && salesreportresult.Count > 0)
				//{

				//	ViewBag.TotalValue = salesreportresult.Sum(x => x.Value);
				//	ViewBag.StoreAmount = salesreportresult.Sum(x => x.StoreAmount);
				//	ViewBag.ProcessAmount = salesreportresult.Sum(x => x.ProcessAmount);
				//	ViewBag.StripeAmount = salesreportresult.Sum(x => x.StripeAmount);
				//	ViewBag.TransactionAmount = salesreportresult.Sum(x => x.TransactionAmount);
				//	ViewBag.NetAmount = salesreportresult.Sum(x => x.NetAmount);
				//	ViewBag.RedeemedAmount = salesreportresult.Sum(x => x.RedeemedAmount);
				//	ViewBag.UnRedeemedAmount = salesreportresult.Sum(x => x.UnRedeemedAmount);
				//	ViewBag.TotalCount = salesreportresult.Count;
				//}
				//else
				//{
				//	ViewBag.TotalValue = 0;
				//	ViewBag.StoreAmount = 0;
				//	ViewBag.ProcessAmount = 0;
				//	ViewBag.StripeAmount = 0;
				//	ViewBag.TransactionAmount = 0;
				//	ViewBag.NetAmount = 0;
				//	ViewBag.RedeemedAmount = 0;
				//	ViewBag.UnRedeemedAmount = 0;
				//	ViewBag.TotalCount = 0;
				//}
				//ViewBag.SalesStoreName = _request.salesstorename;
				//ViewBag.ProductName = _request.productName;
				//ViewBag.StartDate = _request.startDate.ToString("dd/MMM/yyyy");
				//ViewBag.EndDate = _request.endDate.ToString("dd/MMM/yyyy");
				//renderedSalesReportView = await _viewRenderingService.RenderToStringAsync("GenerateDetailedSalesReport", salesreportresult);
				//string path2 = "";
				//string webRootPath2 = _env.WebRootPath;
				//string dirPath2 = Path.Combine(webRootPath2, "Document");

				//if (!System.IO.Directory.Exists(dirPath2))
				//{
				//	System.IO.Directory.CreateDirectory(dirPath2);
				//}
				//string outputPdfPath = dirPath2 + "\\output.pdf";
				//// Convert the HTML string to a stream
				//using (var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(renderedSalesReportView)))
				//{
				//	// Initialize the HTML document from the stream
				//	using (HTMLDocument htmlDocument = new HTMLDocument(stream, ""))
				//	{
				//		// Initialize the PDF save options
				//		PdfSaveOptions options = new PdfSaveOptions();

				//		// Convert HTML to PDF
				//		Aspose.Html.Converters.Converter.ConvertHTML(htmlDocument, options, outputPdfPath);
				//	}
				//}
				//	var doc = new HtmlToPdfDocument()
				//	{
				//		GlobalSettings = {
				//	PaperSize = PaperKind.A4,
				//	Orientation = Orientation.Portrait,
				//},
				//		Objects = {
				//	new ObjectSettings() {
				//		HtmlContent = renderedSalesReportView,
				//	},
				//}
				//	};
				//	var _converter = new SynchronizedConverter(new PdfTools());

				//	byte[] pdf = _converter.Convert(doc);

				//	string webRootPath = _env.WebRootPath;
				//	string contentRootPath = _env.ContentRootPath;

				//	string path = "";
				//	string dirPath = Path.Combine(webRootPath, "Document");

				//	if (!System.IO.Directory.Exists(dirPath))
				//	{
				//		System.IO.Directory.CreateDirectory(dirPath);
				//	}
				//	string pdfFileName = "SalesReporting.pdf";
				//	if (!System.IO.File.Exists(dirPath + "\\" + pdfFileName))
				//		System.IO.File.WriteAllBytes(dirPath + "\\" + pdfFileName, pdf);
				//	if (!Directory.Exists("~/Document/"))
				//	{
				//		Directory.CreateDirectory("~/Document/");
				//	}

				//	return File(pdf, "application/pdf", pdfFileName);

				// Ensure EPPlus license context is set
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				string webRootPath = _env.WebRootPath;
				string contentRootPath = _env.ContentRootPath;

				string path = "";
				string dirPath = Path.Combine(webRootPath, "Document");

				if (!System.IO.Directory.Exists(dirPath))
				{
					System.IO.Directory.CreateDirectory(dirPath);
				}
				string filePath = dirPath + "\\SalesReporting.xlsx";

				#region SalesReport
				// Create a new Excel package
				using (var package = new ExcelPackage())
				{
					// Add a worksheet to the package
					var worksheet = package.Workbook.Worksheets.Add("Sheet1");

					// Add custom information (e.g., title)
					worksheet.Cells["B1"].Value = "Sales Report";
					worksheet.Cells["B1"].Style.Font.Bold = true;
					worksheet.Cells["B1"].Style.Font.UnderLine = true;
					worksheet.Cells["B1"].Style.Font.Size = 16;

					worksheet.Cells["B2"].Value = "STORE NAME: ";
					worksheet.Cells["B2"].Style.Font.Bold = true;
					worksheet.Cells["B2"].Style.Font.Size = 12;
					worksheet.Cells["C2"].Value = _request.salesstorename;
					worksheet.Cells["C2"].Style.Font.Bold = true;
					worksheet.Cells["C2"].Style.Font.Size = 12;

					worksheet.Cells["B3"].Value = "PRODUCT NAME: ";
					worksheet.Cells["B3"].Style.Font.Bold = true;
					worksheet.Cells["B3"].Style.Font.Size = 12;
					worksheet.Cells["C3"].Value = _request.productName;
					worksheet.Cells["C3"].Style.Font.Bold = true;
					worksheet.Cells["C3"].Style.Font.Size = 12;


					worksheet.Cells["B4"].Value = "REPORTING PERIOD: ";
					worksheet.Cells["B4"].Style.Font.Bold = true;
					worksheet.Cells["B4"].Style.Font.Size = 12;
					worksheet.Cells["C4"].Value = _request.startDate.Value.ToString("dd-MMM-yyyy") + " To " + _request.endDate.Value.ToString("dd-MMM-yyyy");
					worksheet.Cells["C4"].Style.Font.Bold = true;
					worksheet.Cells["C4"].Style.Font.Size = 12;

					worksheet.Cells["B5"].Value = "GGC TOTAL NET AMOUNT: ";
					worksheet.Cells["B5"].Style.Font.Bold = true;
					worksheet.Cells["B5"].Style.Font.Size = 12;
					worksheet.Cells["C5"].Value = salesreportresult != null ? salesreportresult.Sum(x => x.NetAmount) : "-";
					worksheet.Cells["C5"].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
					worksheet.Cells["C5"].Style.Font.Bold = true;
					worksheet.Cells["C5"].Style.Font.Size = 12;

					worksheet.Cells["B6"].Value = "COUNT: ";
					worksheet.Cells["B6"].Style.Font.Bold = true;
					worksheet.Cells["B6"].Style.Font.Size = 12;
					worksheet.Cells["C6"].Value = salesreportresult != null ? salesreportresult.Count : "";
					worksheet.Cells["C6"].Style.Font.Bold = true;
					worksheet.Cells["C6"].Style.Font.Size = 12;




					// Define headers for the table
					string[] headers = { "TransactionDateTime", "Product", "Transaction ID"
							, "Card ID", "Value"
							,"Count","StoreNo","StoreName","StoreComm"
							,"GGC Redemption","Processing Fee","Stripe Comm"
							,"Merchant","Net Amount","Expiry Date","Redeemed Amount"
							,"UnRedeemed Amt","Breakage"
							};

					// Add headers to the worksheet
					for (int i = 0; i < headers.Length; i++)
					{
						worksheet.Cells[7, i + 1].Value = headers[i];
						worksheet.Cells[7, i + 1].Style.Font.Bold = true;
					}

					if (salesreportresult != null && salesreportresult.Count > 0)
					{

						// Add data to the worksheet
						int startRow = 8;
						for (int i = 0; i < salesreportresult.Count; i++)
						{
							worksheet.Cells[startRow + i, 1].Value = salesreportresult[i].TransactionDateTime;
							worksheet.Cells[startRow + i, 1].Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";

							worksheet.Cells[startRow + i, 2].Value = salesreportresult[i].Product;

							worksheet.Cells[startRow + i, 3].Value = salesreportresult[i].TransactionID;

							worksheet.Cells[startRow + i, 4].Value = salesreportresult[i].CardID;

							worksheet.Cells[startRow + i, 5].Value = salesreportresult[i].Value;
							worksheet.Cells[startRow + i, 5].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";

							worksheet.Cells[startRow + i, 6].Value = 1;

							worksheet.Cells[startRow + i, 7].Value = salesreportresult[i].StoreNo;

							worksheet.Cells[startRow + i, 8].Value = salesreportresult[i].StoreName;

							worksheet.Cells[startRow + i, 9].Value = salesreportresult[i].StoreAmount;
							worksheet.Cells[startRow + i, 9].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";

							worksheet.Cells[startRow + i, 10].Value = salesreportresult[i].GGCAmount;
							worksheet.Cells[startRow + i, 10].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";

							worksheet.Cells[startRow + i, 11].Value = salesreportresult[i].ProcessAmount;

							worksheet.Cells[startRow + i, 12].Value = salesreportresult[i].StripeAmount;
							worksheet.Cells[startRow + i, 12].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";

							worksheet.Cells[startRow + i, 13].Value = salesreportresult[i].TransactionAmount;
							worksheet.Cells[startRow + i, 13].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";

							worksheet.Cells[startRow + i, 14].Value = salesreportresult[i].NetAmount;
							worksheet.Cells[startRow + i, 14].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";

							worksheet.Cells[startRow + i, 15].Value = salesreportresult[i].Date;
							worksheet.Cells[startRow + i, 15].Style.Numberformat.Format = "dd/mm/yyyy";

							worksheet.Cells[startRow + i, 16].Value = salesreportresult[i].RedeemedAmount;
							worksheet.Cells[startRow + i, 16].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";

							worksheet.Cells[startRow + i, 17].Value = salesreportresult[i].UnRedeemedAmount;
							worksheet.Cells[startRow + i, 17].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";

							worksheet.Cells[startRow + i, 18].Value = salesreportresult[i].Breakage;
							worksheet.Cells[startRow + i, 18].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						}

						var lastrow = salesreportresult.Count + 8;
						worksheet.Cells[lastrow, 1].Value = "Totals";
						worksheet.Cells[lastrow, 1].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 5].Value = salesreportresult.Sum(x => x.Value);
						worksheet.Cells[lastrow, 5].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 5].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 6].Value = salesreportresult.Count;
						worksheet.Cells[lastrow, 6].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 9].Value = salesreportresult.Sum(x => x.StoreAmount);
						worksheet.Cells[lastrow, 9].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 9].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 10].Value = salesreportresult.Sum(x => x.GGCAmount);
						worksheet.Cells[lastrow, 10].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 10].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 11].Value = salesreportresult.Sum(x => x.ProcessAmount);
						worksheet.Cells[lastrow, 11].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 11].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 12].Value = salesreportresult.Sum(x => x.StripeAmount);
						worksheet.Cells[lastrow, 12].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 12].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 13].Value = salesreportresult.Sum(x => x.TransactionAmount);
						worksheet.Cells[lastrow, 13].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 13].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 14].Value = salesreportresult.Sum(x => x.NetAmount);
						worksheet.Cells[lastrow, 14].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 14].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 16].Value = salesreportresult.Sum(x => x.RedeemedAmount);
						worksheet.Cells[lastrow, 16].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 16].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 17].Value = salesreportresult.Sum(x => x.UnRedeemedAmount);
						worksheet.Cells[lastrow, 17].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 17].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 18].Value = salesreportresult.Sum(x => x.Breakage);
						worksheet.Cells[lastrow, 18].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 18].Style.Font.Bold = true;

						var _ct = salesreportresult != null ? salesreportresult.Count + 1 : 0;
						// Define the range for the table
						var tableRange = worksheet.Cells[7, 1, startRow + _ct, headers.Length];

						// Create a table
						var table = worksheet.Tables.Add(tableRange, "Table1");

						// Apply a table style
						table.TableStyle = TableStyles.Medium2;


					}
					// Freeze the top 3 rows
					worksheet.View.FreezePanes(8, 1);

					worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
					// Save the package to the file
					FileInfo fileInfo = new FileInfo(filePath);
					package.SaveAs(fileInfo);

				}
				#endregion

				string redempfilePath = dirPath + "\\RedemptionsReporting.xlsx";

				#region RedemptionsReport
				// Create a new Excel package
				using (var package = new ExcelPackage())
				{
					// Add a worksheet to the package
					var worksheet = package.Workbook.Worksheets.Add("Sheet1");

					// Add custom information (e.g., title)
					worksheet.Cells["B1"].Value = "REDEMPTIONS REPORT";
					worksheet.Cells["B1"].Style.Font.Bold = true;
					worksheet.Cells["B1"].Style.Font.UnderLine = true;
					worksheet.Cells["B1"].Style.Font.Size = 16;

					worksheet.Cells["B2"].Value = "STORE NAME: ";
					worksheet.Cells["B2"].Style.Font.Bold = true;
					worksheet.Cells["B2"].Style.Font.Size = 12;
					worksheet.Cells["C2"].Value = _request.redemstorename;
					worksheet.Cells["C2"].Style.Font.Bold = true;
					worksheet.Cells["C2"].Style.Font.Size = 12;

					worksheet.Cells["B3"].Value = "PRODUCT NAME: ";
					worksheet.Cells["B3"].Style.Font.Bold = true;
					worksheet.Cells["B3"].Style.Font.Size = 12;
					worksheet.Cells["C3"].Value = _request.productName;
					worksheet.Cells["C3"].Style.Font.Bold = true;
					worksheet.Cells["C3"].Style.Font.Size = 12;


					worksheet.Cells["B4"].Value = "REPORTING PERIOD: ";
					worksheet.Cells["B4"].Style.Font.Bold = true;
					worksheet.Cells["B4"].Style.Font.Size = 12;
					worksheet.Cells["C4"].Value = _request.startDate.Value.ToString("dd-MMM-yyyy") + " To " + _request.endDate.Value.ToString("dd-MMM-yyyy");
					worksheet.Cells["C4"].Style.Font.Bold = true;
					worksheet.Cells["C4"].Style.Font.Size = 12;

					worksheet.Cells["B5"].Value = "TOTAL FACE VALUE: ";
					worksheet.Cells["B5"].Style.Font.Bold = true;
					worksheet.Cells["B5"].Style.Font.Size = 12;
					worksheet.Cells["C5"].Value = redempreportresult != null ? redempreportresult.Sum(x => x.Value) : "-";
					worksheet.Cells["C5"].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
					worksheet.Cells["C5"].Style.Font.Bold = true;
					worksheet.Cells["C5"].Style.Font.Size = 12;

					worksheet.Cells["B6"].Value = "TOTAL AMOUNT PAYABLE: ";
					worksheet.Cells["B6"].Style.Font.Bold = true;
					worksheet.Cells["B6"].Style.Font.Size = 12;
					worksheet.Cells["C6"].Value = redempreportresult != null ? redempreportresult.Sum(x => x.AmountPayableToStore) : "";
					worksheet.Cells["C6"].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
					worksheet.Cells["C6"].Style.Font.Bold = true;
					worksheet.Cells["C6"].Style.Font.Size = 12;




					// Define headers for the table
					string[] headers = { "TransactionDateTime","Value"
							,"Count", "Product Comm", "VAT Due","Amount Payable"
							,"Store No","Store Name"
							,"Postcode", "Transaction ID"
							, "Card ID","Statement Date","Statement No."
							,"Statement Amt"
							};

					// Add headers to the worksheet
					for (int i = 0; i < headers.Length; i++)
					{
						worksheet.Cells[7, i + 1].Value = headers[i];
						worksheet.Cells[7, i + 1].Style.Font.Bold = true;
					}

					if (redempreportresult != null && redempreportresult.Count > 0)
					{

						// Add data to the worksheet
						int startRow = 8;
						for (int i = 0; i < redempreportresult.Count; i++)
						{
							worksheet.Cells[startRow + i, 1].Value = redempreportresult[i].TransactionDateTime;
							worksheet.Cells[startRow + i, 1].Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";

							worksheet.Cells[startRow + i, 2].Value = redempreportresult[i].Value;
							worksheet.Cells[startRow + i, 2].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";

							worksheet.Cells[startRow + i, 3].Value = 1;


							worksheet.Cells[startRow + i, 4].Value = redempreportresult[i].ProductAmount;
							worksheet.Cells[startRow + i, 4].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";

							worksheet.Cells[startRow + i, 5].Value = redempreportresult[i].VATDueOnCommission;
							worksheet.Cells[startRow + i, 5].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";

							worksheet.Cells[startRow + i, 6].Value = redempreportresult[i].AmountPayableToStore;
							worksheet.Cells[startRow + i, 6].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";

							worksheet.Cells[startRow + i, 7].Value = redempreportresult[i].StoreNo;

							worksheet.Cells[startRow + i, 8].Value = redempreportresult[i].StoreName;

							worksheet.Cells[startRow + i, 9].Value = redempreportresult[i].Postcode;

							worksheet.Cells[startRow + i, 10].Value = redempreportresult[i].TransactionID;

							worksheet.Cells[startRow + i, 11].Value = redempreportresult[i].CardID;

							worksheet.Cells[startRow + i, 12].Value = redempreportresult[i].StatementCreated;
							worksheet.Cells[startRow + i, 12].Style.Numberformat.Format = "dd/mm/yyyy";
							worksheet.Cells[startRow + i, 13].Value = redempreportresult[i].StatementNumber;
							worksheet.Cells[startRow + i, 14].Value = redempreportresult[i].StatementAmount;
							worksheet.Cells[startRow + i, 14].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";



						}

						var lastrow = redempreportresult.Count + 8;
						worksheet.Cells[lastrow, 1].Value = "Totals";
						worksheet.Cells[lastrow, 1].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 2].Value = redempreportresult.Sum(x => x.Value);
						worksheet.Cells[lastrow, 2].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 2].Style.Font.Bold = true;
						//worksheet.Cells[lastrow, 2].Formula = "=SUM(109:[Value]])";

						worksheet.Cells[lastrow, 3].Value = redempreportresult.Count;
						worksheet.Cells[lastrow, 3].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 4].Value = redempreportresult.Sum(x => x.ProductAmount);
						worksheet.Cells[lastrow, 4].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 4].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 5].Value = redempreportresult.Sum(x => x.VATDueOnCommission);
						worksheet.Cells[lastrow, 5].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 5].Style.Font.Bold = true;

						worksheet.Cells[lastrow, 6].Value = redempreportresult.Sum(x => x.AmountPayableToStore);
						worksheet.Cells[lastrow, 6].Style.Numberformat.Format = "£#,##0.00;[Red](£#,##0.00)";
						worksheet.Cells[lastrow, 6].Style.Font.Bold = true;

						//package.Workbook.Calculate();

						var _ct = salesreportresult != null ? redempreportresult.Count + 1 : 0;
						// Define the range for the table
						var tableRange = worksheet.Cells[7, 1, startRow + _ct, headers.Length];

						// Create a table
						var table = worksheet.Tables.Add(tableRange, "Table1");

						// Apply a table style
						table.TableStyle = TableStyles.Medium2;


					}
					// Freeze the top 3 rows
					worksheet.View.FreezePanes(8, 1);

					worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
					// Save the package to the file
					FileInfo fileInfo = new FileInfo(redempfilePath);
					package.SaveAs(fileInfo);

				}
				#endregion
				return Json(new { issalesReportGenerated = true, isredempReportGenerated = true });
			}
			
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				errormessage = ex.InnerException + Environment.NewLine + ex.StackTrace;
			}
			return Json(new { issalesReportGenerated = false, isredempReportGenerated = false, errormessage= errormessage });
		}

		public IActionResult DownloadSalesReportExcel()
		{
			// Define the path to the Excel file on the server
			string filename = "SalesReporting.xlsx";
			string webRootPath = _env.WebRootPath;
			string contentRootPath = _env.ContentRootPath;

			string dirPath = Path.Combine(webRootPath, "Document");

			if (!System.IO.Directory.Exists(dirPath))
			{
				System.IO.Directory.CreateDirectory(dirPath);
			}
			string filePath = dirPath + "\\" + filename;


			if (!System.IO.File.Exists(filePath))
			{
				return NotFound();
			}

			byte[] fileBytes;
			try
			{
				// Read the Excel file into a byte array
				fileBytes = System.IO.File.ReadAllBytes(filePath);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

			// Return the file as a response
			return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
		}
		public IActionResult DownloadRedempReportExcel()
		{
			// Define the path to the Excel file on the server
			string filename = "RedemptionsReporting.xlsx";
			string webRootPath = _env.WebRootPath;
			string contentRootPath = _env.ContentRootPath;

			string dirPath = Path.Combine(webRootPath, "Document");

			if (!System.IO.Directory.Exists(dirPath))
			{
				System.IO.Directory.CreateDirectory(dirPath);
			}
			string filePath = dirPath + "\\" + filename;


			if (!System.IO.File.Exists(filePath))
			{
				return NotFound();
			}

			byte[] fileBytes;
			try
			{
				// Read the Excel file into a byte array
				fileBytes = System.IO.File.ReadAllBytes(filePath);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

			// Return the file as a response
			return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
		}
		public ActionResult ReportSummaryView(ReportSearchParam _request)
		{
			ReportSummaryModel reportSummaryModel = new ReportSummaryModel();
			try
			{
				var salesreportresult = _salesRepository.GetSalesListReport(_request.productid, _request.salesstoreno, _request.startDate.Value, _request.endDate.Value).ToList();
				var redempreportresult = _redemptionsRepository.GetRedemptionsListReport(_request.productid, _request.redemstoreno, _request.startDate.Value, _request.endDate.Value).ToList();
				var getsettinglist = _settingsRepository.GetSettingsList().FirstOrDefault();
				var _liabilitypct = (decimal)0.0;
				if (getsettinglist != null)
				{
					_liabilitypct = getsettinglist.liabilitypct;
				}
				if (salesreportresult == null)
				{
					salesreportresult = new List<SalesEntity>();
				}
				if (redempreportresult == null)
				{
					redempreportresult = new List<RedemptionsEntity>();
				}
				reportSummaryModel.TotalValue = salesreportresult.Sum(x => x.Value).ToString("N2");
				reportSummaryModel.StoreAmount = salesreportresult.Sum(x => x.StoreAmount).ToString("N2");
				reportSummaryModel.ProcessAmount = salesreportresult.Sum(x => x.ProcessAmount).ToString("N2");
				reportSummaryModel.StripeAmount = salesreportresult.Sum(x => x.StripeAmount).ToString("N2");
				reportSummaryModel.TransactionAmount = salesreportresult.Sum(x => x.TransactionAmount).ToString("N2");
				reportSummaryModel.NetAmount = salesreportresult.Sum(x => x.NetAmount).ToString("N2");
				reportSummaryModel.RedeemedAmount = salesreportresult.Sum(x => x.RedeemedAmount).ToString("N2");
				reportSummaryModel.UnRedeemedAmount = salesreportresult.Sum(x => x.UnRedeemedAmount).ToString("N2");
				reportSummaryModel.SalesTotalCount = salesreportresult.Count;
				reportSummaryModel.ggcBreakage = salesreportresult.Sum(x => x.Breakage).ToString("N2");
				reportSummaryModel.RedeemTotalCount = redempreportresult.Count;
				reportSummaryModel.TotalValueRedemp = redempreportresult.Sum(x => x.Value).ToString("N2");
				decimal liabilitypct = _liabilitypct;
				reportSummaryModel.ggliability = (Convert.ToDecimal(reportSummaryModel.UnRedeemedAmount) * liabilitypct).ToString("N2"); ;

			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
			}
			return Json(reportSummaryModel);
		}
		public ActionResult RemittanceReport([FromQuery] ReportSearchParam _request)
		{

			RedemptionsRemittance redemptionsRemittance = new RedemptionsRemittance();
			InvoiceEntity invoiceEntity = new InvoiceEntity();
			Int64 invoiceNumber = 0;
			string longinvoicenumber = "0";
			try
			{
				var redempreportresult = _redemptionsRepository.GetRedemptionsListReport(_request.productid, _request.redemstoreno, _request.startDate.Value, _request.endDate.Value).ToList();
				var invoiceresult = _redemptionsRepository.GetInvoiceList().ToList().FirstOrDefault();
				if (redempreportresult == null)
				{
					return Content("No Redemption record found in selected time period");
				}
				if (invoiceresult != null)
				{
					invoiceNumber = invoiceresult.InvoiceNumber + 1;
				}
				else
				{
					invoiceNumber = invoiceNumber + 1;
				}
				longinvoicenumber = ConvertTo7Digits(invoiceNumber);
				invoiceEntity.InvoiceNumber = invoiceNumber;
				invoiceEntity.StatementNumber = "GCP" + longinvoicenumber.ToString();
				invoiceEntity.StatementCreated = DateTime.Now;
				invoiceEntity.GrossAmount = redempreportresult != null ? redempreportresult.Sum(x => x.Value) * -1 : 0;
				invoiceEntity.ProductCommission = redempreportresult != null ? redempreportresult.Sum(x => x.ProductAmount) * -1 : 0;
				invoiceEntity.VATDue = redempreportresult != null ? redempreportresult.Sum(x => x.VATDueOnCommission) * -1 : 0;
				invoiceEntity.AmountPayable = redempreportresult != null ? redempreportresult.Sum(x => x.AmountPayableToStore) * -1 : 0;

				int invoicesaveresult = _redemptionsRepository.SaveInvoiceInformation(invoiceEntity);

				if (invoicesaveresult > 0)
				{
					foreach(var item in redempreportresult)
					{
						RedemptionsEntity redemptionsEntity = new RedemptionsEntity();
						redemptionsEntity.StatementCreated = DateTime.Now;
						redemptionsEntity.StatementNumber = invoiceEntity.StatementNumber;
						redemptionsEntity.StatementAmount = invoiceEntity.AmountPayable;
						redemptionsEntity.RedemptionsID = item.RedemptionsID;
						_redemptionsRepository.UpdateRedemptionsInvoiceInformation(redemptionsEntity);
					}

					redemptionsRemittance.StoreID = ConvertTo7Digits(_request.redemstoreno);
					redemptionsRemittance.StoreName = _request.redemstorename.Split("| ")[1].ToString();
					redemptionsRemittance.DatePeriod = _request.startDate.Value.ToString("dd-MMM-yyyy") + " To " + _request.endDate.Value.ToString("dd-MMM-yyyy");
					redemptionsRemittance.InvoiceNumber = invoiceEntity.StatementNumber;
					redemptionsRemittance.DocumentDate = DateTime.Now.ToString("dd/MM/yyyy");
					redemptionsRemittance.GrossAmount = invoiceEntity.GrossAmount;
					redemptionsRemittance.ProductCommission = invoiceEntity.ProductCommission;
					redemptionsRemittance.VATDue = invoiceEntity.VATDue;
					redemptionsRemittance.AmountPayable = invoiceEntity.AmountPayable;
					redemptionsRemittance.InvoiceTotal = invoiceEntity.ProductCommission + invoiceEntity.VATDue;
					if (redempreportresult != null && redempreportresult.Count > 0)
					{
						redemptionsRemittance.lstRedemptions = redempreportresult;
					}
				}
				else
				{
					return Content("Unable to save invoice");
				}
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return Content("Exception occur please contact admin");
			}
			return View(redemptionsRemittance);
		}
		public static string ConvertTo7Digits(Int64 number)
		{
			string numberStr = number.ToString();
			if (numberStr.Length < 7)
			{
				// Pad with leading zeros
				return numberStr.PadLeft(7, '0');
			}
			else if (numberStr.Length > 7)
			{
				// Truncate to the first 7 digits
				return numberStr.Substring(0, 7);
			}
			else
			{
				// Already 7 digits
				return numberStr;
			}
		}



	}
}
