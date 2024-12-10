using CsvHelper.Configuration;
using CsvHelper;
using Entity;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using Repository.Products;
using Repository.Redemptions;
using Repository.Sales;
using System;
using System.Drawing;
using System.Globalization;
using System.Transactions;
using Repository.StoresRedeem;
using Repository.SalesStore;
using Repository.VATRates;
using System.Text.Json.Serialization;
using NPOI.SS.Formula.Functions;
using SixLabors.ImageSharp.ColorSpaces;
using Repository.Retailer;
using OfficeOpenXml;
using System.Reflection;

namespace EPAYGOLF.Controllers
{
	[ConditionalAuthorize(requireAuthorization: true)]
	public class DataController : Controller
	{
		private ISalesRepository _salesRepository = new SalesRepository();
		private IRedemptionsRepository _redemptionsRepository = new RedemptionsRepository();
		private IProductRepository _productRepository = new ProductRepository();
		private IStoresRedeemRepository _storeredeemRepository = new StoresRedeemRepository();
		private ISalesStoreRepository _salesStoreRepository = new SalesStoreRepository();
		private IVATRatesRepository _vATRatesRepository = new VATRatesRepository();
		private IRetailerRepository _retailerRepository = new RetailerRepository();
		public IActionResult Index()
		{
			return View();
		}
		#region Sales
		public IActionResult SalesIndex()
		{
			Helpers.ApplicationExceptions.SaveActivityLog("SalesIndex action method called.");

			return View();
		}
		public IActionResult SalesList()
		{
			var _list = _salesRepository.GetSalesList();

			return View(_list);
		}
		public IActionResult ExportSalesList()
		{
			var _list = _salesRepository.GetSalesList();

			return View(_list);
		}
		public IActionResult DeleteSales(Int64 ID)
		{
			var result = _salesRepository.DeleteSalesInformation(ID);
			return Json(result);
		}

		#endregion
		#region Redemptions
		public IActionResult RedemptionsIndex()
		{
			Helpers.ApplicationExceptions.SaveActivityLog("RedemptionsIndex action method called.");

			return View();
		}
		public IActionResult RedemptionsList()
		{
			var _list = _redemptionsRepository.GetRedemptionsList();

			return View(_list);
		}
		public IActionResult DeleteRedemptions(Int64 ID)
		{
			var result = _redemptionsRepository.DeleteRedemptionsInformation(ID);
			return Json(result);
		}
		public IActionResult ExportRedemptionsList()
		{
			var _list = _redemptionsRepository.GetRedemptionsList();

			return View(_list);
		}

		#endregion

		#region ImportData

		#region SalesImport
		[HttpPost]
		public IActionResult uploadfilesalesdata(IFormFile filesalesdata)
		{
			Helpers.ApplicationExceptions.SaveActivityLog("uploadfilesalesdata action method called.");
			try
			{
				if (filesalesdata == null)
				{
					return Json(-5);
				}

				else
				{
					var list = new List<SalesImportDataEntity>();
					using (var ms = new MemoryStream())
					{
						filesalesdata.CopyTo(ms);
						var fileBytes = ms.ToArray();
						list = ReadSalesCsvFile(fileBytes);
						if (list == null || (list != null && list.Count == 0))
						{
							return Json(-3);
						}
						var findLoadtypeData = list.Where(x => x.TransactionType.ToLower() == "load").ToList();
						if (findLoadtypeData == null || (findLoadtypeData != null && findLoadtypeData.Count == 0))
						{
							return Json(-4); //Transaction type load not found
						}
						var findtrantypenotload = list.Where(x => x.TransactionType.ToLower() != "load" && x.TransactionType.ToLower() != "refund" && x.TransactionType.ToLower() != "void").ToList();
						if (findtrantypenotload != null && findtrantypenotload.Count > 0)
						{
							return Json(-7); //All record should have transaction type Load.
						}
						var findEANEmptyNUll = list.Where(x => x.EAN < 0).ToList();
						if (findEANEmptyNUll != null && findEANEmptyNUll.Count > 0)
						{
							return Json(-6); // Product EAN is empty
						}
					}

					var errormessage = ProcessSalesImportData(list);
					if (!string.IsNullOrEmpty(errormessage))
					{
						return Json(errormessage);
					}
					else
					{
						return Json(1);
					}

				}
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return Json(-1);
			}
		}
		private string ProcessSalesImportData(List<SalesImportDataEntity> list)
		{
			var errorMessage = string.Empty;
			foreach (var item in list)
			{
				#region CheckEAN
				var _checkEAN = _productRepository.GetProductByEAN(item.EAN.ToString());
				if (_checkEAN != null && _checkEAN.Count > 0)//It means Product EAN exist no need to insert.
				{

				}
				else
				{
					ProductEntity productEntity = new ProductEntity();
					productEntity.ProductName = item.Product;
					productEntity.ProductEAN = Convert.ToInt64(item.EAN);
					_productRepository.SaveProductInformation(productEntity);
				}
				#endregion
				#region CheckSalesStore
				var _checkStoreNo = _salesStoreRepository.GetSalesStoreByStoreNo(item.StoreNo.ToString());
				if (_checkStoreNo != null && _checkStoreNo.Count > 0)//It means StoreNo exist no need to insert.
				{

				}
				else
				{
					SalesStoreEntity salesStoreEntity = new SalesStoreEntity();
					salesStoreEntity.RetailerID = Convert.ToInt64(item.StoreNo);
					salesStoreEntity.RetailerName = item.StoreName;
					salesStoreEntity.GGCRedemption = 13 / 100;
					_salesStoreRepository.SaveSalesStoreInformation(salesStoreEntity);
				}
				#endregion
				#region SaveSalesData 
				#region CheckSalesData
				var _checkSalesData = _salesRepository.GetSalesDataByID(item.ID.ToString());
				Helpers.ApplicationExceptions.SaveActivityLog("GetSalesDataByID " + item.ID);
				if (_checkSalesData == null)//It means SalesID not exist need to insert.
				{
					Helpers.ApplicationExceptions.SaveActivityLog("GetSalesDataByID == null" + item.ID);
					SalesEntity salesEntity = new SalesEntity();
					salesEntity.ID = Convert.ToInt32(item.ID);
					salesEntity.AccountName = item.AccountName;
					salesEntity.TransactionID = item.TransactionID;
					salesEntity.TransactionType = item.TransactionType;
					salesEntity.CardID = item.CardID;
					salesEntity.PAN = item.PAN;
					DateTime _trandate;
					if (DateTime.TryParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						salesEntity.TransactionDateTime = _trandate;


					}
					else if (DateTime.TryParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						salesEntity.TransactionDateTime = _trandate;


					}
					else if (DateTime.TryParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						salesEntity.TransactionDateTime = _trandate;


					}
					else if (DateTime.TryParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						salesEntity.TransactionDateTime = _trandate;


					}
					else if (DateTime.TryParseExact(item.TransactionDateTime, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						salesEntity.TransactionDateTime = _trandate;


					}
					salesEntity.TransactionDateTime = _trandate;//DateTime.ParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture);

					salesEntity.Value = Convert.ToDecimal(item.Value);
					salesEntity.BINNumber = item.BINNumber;
					salesEntity.StoreNo = Convert.ToInt64(item.StoreNo);
					salesEntity.StoreName = item.StoreName;
					salesEntity.Product = item.Product;
					salesEntity.EAN = Convert.ToInt64(item.EAN);
					salesEntity.Date = _trandate;//DateTime.ParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					var _vatrate = _vATRatesRepository.GetVATRatesList().Where(x => x.VATRateDate.Value.Year == salesEntity.Date.Value.Year).FirstOrDefault();
					if (_vatrate == null)
					{
						errorMessage = "VAT rate not found in system";
						break;
					}
					salesEntity.VATRate = _vatrate.VATRate / 100;
					salesEntity.StoreCommission = 0;
					salesEntity.StoreAmount = 0;
					salesEntity.GGCCommission = 0;
					salesEntity.StripeCommission = 0;
					salesEntity.StoreAmount = 0;
					salesEntity.TransactionAmount = 0;
					salesEntity.NetAmount = 0;
					var savesaleresult = _salesRepository.SaveSalesInformation(salesEntity);
					if (savesaleresult < 0)
					{
						Helpers.ApplicationExceptions.SaveActivityLog("Unable to save sales ID record " + item.ID);
						errorMessage = "Unable to save sales data with record ID " + item.ID;
						break;
					}
					else
					{
						Helpers.ApplicationExceptions.SaveActivityLog("saved sales ID record " + item.ID);
					}

				}

				#endregion

				#endregion
			}
			if (string.IsNullOrEmpty(errorMessage))
			{
				var result = TransformSalesData();
				if ((int)result.Value == -10)
				{
					errorMessage = "Please update the salesstore with the commission percentage!";
				}
				if ((int)result.Value == -11)
				{
					errorMessage = "Please update the product with the commission percentage!";
				}
				if ((int)result.Value == -12)
				{
					errorMessage = "No sales data found.";
				}
				if ((int)result.Value == -13)
				{
					errorMessage = "No product data found.";
				}
			}



			return errorMessage;
		}
		public JsonResult TransformSalesData()
		{
			var checkifcommisionnull = _salesRepository.GetSalesList().Where(x => x.Value == 0).ToList();
			if (checkifcommisionnull != null && checkifcommisionnull.Count > 0)
			{
				return Json(-10);
			}
			checkifcommisionnull = _salesRepository.GetSalesList();
			if (checkifcommisionnull == null || (checkifcommisionnull != null && checkifcommisionnull.Count == 0))
			{
				return Json(-12);
			}
			var checkifproductvaluenull = _productRepository.GetProductList().Where(x => x.Commission < 1 && x.ProductEAN != 2 && x.ProductEAN != 3 && x.ProductEAN != 4).ToList();
			if (checkifproductvaluenull != null && checkifproductvaluenull.Count > 0)
			{
				return Json(-11);
			}
			checkifproductvaluenull = _productRepository.GetProductList();
			if (checkifproductvaluenull == null || (checkifproductvaluenull != null && checkifproductvaluenull.Count == 0))
			{
				return Json(-13);
			}
			var result = _salesRepository.TransformSalesData();
			return Json(result);
		}
		public static List<SalesImportDataEntity> ReadSalesCsvFile(byte[] filePath)
		{
			var list = new List<SalesImportDataEntity>();

			using (var reader = new StreamReader(new ByteArrayInputStream(filePath)))
			using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true, // Adjust this if your CSV has no headers
			}))
			{
				list = csv.GetRecords<SalesImportDataEntity>().ToList();
			}

			return list;
		}
		#endregion

		#region RedemptionImport
		[HttpPost]
		public IActionResult uploadfileredemptiondata(IFormFile fileredemptiondata)
		{
			Helpers.ApplicationExceptions.SaveActivityLog("uploadfileredemptiondata action method called.");
			try
			{
				if (fileredemptiondata == null)
				{
					return Json(-5);
				}

				else
				{
					var list = new List<RedeptionImportDataEntity>();
					using (var ms = new MemoryStream())
					{
						fileredemptiondata.CopyTo(ms);
						var fileBytes = ms.ToArray();
						list = ReadRedemptionsCsvFile(fileBytes);
						if (list == null || (list != null && list.Count == 0))
						{
							return Json(-3);
						}
						var findLoadtypeData = list.Where(x => x.TransactionType.ToLower() == "redeem").ToList();
						if (findLoadtypeData == null || (findLoadtypeData != null && findLoadtypeData.Count == 0))
						{
							return Json(-4); //Transaction type redeem not found
						}
						var findtrantypenotload = list.Where(x => x.TransactionType.ToLower() != "redeem").ToList();
						if (findtrantypenotload != null && findtrantypenotload.Count > 0)
						{
							return Json(-7); //All record should have transaction type redeem.
						}
						var findStoreNoEmptyNUll = list.Where(x => x.StoreNo < 1).ToList();
						if (findStoreNoEmptyNUll != null && findStoreNoEmptyNUll.Count > 0)
						{
							return Json(-6); // StoreNo is empty
						}
					}

					var errormessage = ProcessRedemptionImportData(list);
					if (!string.IsNullOrEmpty(errormessage))
					{
						return Json(errormessage);
					}
					else
					{
						return Json(1);
					}

				}
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return Json(-1);
			}
		}
		private string ProcessRedemptionImportData(List<RedeptionImportDataEntity> list)
		{
			var errorMessage = string.Empty;
			foreach (var item in list)
			{
				//#region CheckEAN
				//var _checkEAN = _productRepository.GetProductByEAN(item.EAN);
				//if (_checkEAN != null && _checkEAN.Count > 0)//It means Product EAN exist no need to insert.
				//{

				//}
				//else
				//{
				//	ProductEntity productEntity = new ProductEntity();
				//	productEntity.ProductName = item.Product;
				//	productEntity.ProductEAN = Convert.ToInt64(item.EAN);
				//	_productRepository.SaveProductInformation(productEntity);
				//}
				//#endregion
				#region CheckStoreRedeem
				var _checkStoreNo = _storeredeemRepository.GetStoreRedeemByStoreNo(item.StoreNo.ToString());
				if (_checkStoreNo != null && _checkStoreNo.Count > 0)//It means StoreNo exist no need to insert.
				{

				}
				else
				{
					StoresRedeemEntity storesRedeemEntity = new StoresRedeemEntity();
					storesRedeemEntity.StoreNo = Convert.ToInt64(item.StoreNo);
					storesRedeemEntity.StoreName = item.StoreName;
					_storeredeemRepository.SaveStoresRedeemInformation(storesRedeemEntity);
				}
				#endregion
				#region SaveRedeemData 
				#region CheckRedeemData
				var _checkredeemData = _redemptionsRepository.GetredeemDataByID(item.ID.ToString());
				Helpers.ApplicationExceptions.SaveActivityLog("GetredeemDataByID" + item.ID);
				if (_checkredeemData == null)//It means SalesID not exist need to insert.
				{
					Helpers.ApplicationExceptions.SaveActivityLog("GetredeemDataByID == null" + item.ID);
					RedemptionsEntity redeemEntity = new RedemptionsEntity();
					redeemEntity.ID = Convert.ToInt32(item.ID);
					redeemEntity.AccountName = item.AccountName;
					redeemEntity.TransactionID = item.TransactionID;
					redeemEntity.TransactionType = item.TransactionType;
					redeemEntity.CardID = item.CardID;
					redeemEntity.PAN = item.PAN;
					DateTime _trandate;
					if (DateTime.TryParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						redeemEntity.TransactionDateTime = _trandate;


					}
					else if (DateTime.TryParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						redeemEntity.TransactionDateTime = _trandate;


					}
					else if (DateTime.TryParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						redeemEntity.TransactionDateTime = _trandate;


					}
					else if (DateTime.TryParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						redeemEntity.TransactionDateTime = _trandate;


					}
					else if (DateTime.TryParseExact(item.TransactionDateTime, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						redeemEntity.TransactionDateTime = _trandate;


					}
					//redeemEntity.TransactionDateTime = //DateTime.ParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
					redeemEntity.Value = Convert.ToDecimal(item.Value);
					redeemEntity.BINNumber = item.BINNumber;
					redeemEntity.StoreNo = Convert.ToInt64(item.StoreNo);
					redeemEntity.StoreName = item.StoreName;
					redeemEntity.Product = item.Product;
					redeemEntity.EAN = 5060380635181;//Convert.ToInt64(item.EAN);
					redeemEntity.Date = redeemEntity.TransactionDateTime;//DateTime.ParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					//redeemEntity.Postcode = _storeredeemRepository.GetStoreRedeemByStoreNo(Convert.ToInt64(item.StoreNo).ToString()).FirstOrDefault().PostCode;

					var _vatrate = _vATRatesRepository.GetVATRatesList().Where(x => x.VATRateDate.Value.Year == redeemEntity.Date.Value.Year).FirstOrDefault();
					if (_vatrate == null)
					{
						errorMessage = "VAT rate not found in system";
						break;
					}
					redeemEntity.VATRate = _vatrate.VATRate / 100;
					redeemEntity.ProductCommission = 0;
					redeemEntity.ProductAmount = 0;
					redeemEntity.VATDueOnCommission = 0;
					redeemEntity.AmountPayableToStore = 0;


					var saveredemresult = _redemptionsRepository.SaveRedemptionsInformation(redeemEntity);
					if (saveredemresult < 0)
					{
						Helpers.ApplicationExceptions.SaveActivityLog("Unable to save redeem ID record " + item.ID);
						errorMessage = "Unable to save redemption data with record ID " + item.ID;
						break;
					}
					else
					{
						Helpers.ApplicationExceptions.SaveActivityLog("saved redeem ID record " + item.ID);
					}

				}

				#endregion

				#endregion
			}
			if (string.IsNullOrEmpty(errorMessage))
			{
				var result = TransformRedeemData();
				if ((int)result.Value == -10)
				{
					errorMessage = "Please update the Database with the Store addresses!";
				}
				if ((int)result.Value == -11)
				{
					errorMessage = "Please update the product with the commission percentage!";
				}
				if ((int)result.Value == -12)
				{
					errorMessage = "No redemption data found.";
				}
				if ((int)result.Value == -13)
				{
					errorMessage = "No product data found.";
				}
			}
			return errorMessage;
		}
		public JsonResult TransformRedeemData()
		{
			//var checkifStoresRedeemnull = _storeredeemRepository.GetStoresRedeemList().Where(x => x.PostCode== string.Empty).ToList();
			//if (checkifStoresRedeemnull != null && checkifStoresRedeemnull.Count > 0)
			//{
			//	return Json(-10);
			//}
			var checkifStoresRedeemnull = _storeredeemRepository.GetStoresRedeemList();
			if (checkifStoresRedeemnull == null || (checkifStoresRedeemnull != null && checkifStoresRedeemnull.Count == 0))
			{
				return Json(-12);
			}
			var checkifredemtionnull = _redemptionsRepository.GetRedemptionsList();
			if (checkifredemtionnull == null || (checkifredemtionnull != null && checkifredemtionnull.Count == 0))
			{
				return Json(-12);
			}
			//var checkifproductvaluenull = _productRepository.GetProductList().Where(x => x.Commission < 1).ToList();
			//if (checkifproductvaluenull != null && checkifproductvaluenull.Count > 0)
			//{
			//	return Json(-11);
			//}
			//checkifproductvaluenull = _productRepository.GetProductList();
			//if (checkifproductvaluenull == null || (checkifproductvaluenull != null && checkifproductvaluenull.Count == 0))
			//{
			//	return Json(-13);
			//}
			var result = _redemptionsRepository.TransformRedeemData();
			return Json(result);
		}

		public static List<RedeptionImportDataEntity> ReadRedemptionsCsvFile(byte[] filePath)
		{
			var list = new List<RedeptionImportDataEntity>();

			using (var reader = new StreamReader(new ByteArrayInputStream(filePath)))
			using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true, // Adjust this if your CSV has no headers
			}))
			{
				list = csv.GetRecords<RedeptionImportDataEntity>().ToList();
			}

			return list;
		}
		#endregion

		#region commentedcode
		//public static List<SalesImportDataEntity> ReadSalesExcelFile(byte[] filePath)
		//{
		//	var list = new List<SalesImportDataEntity>();


		//	IWorkbook workbook = new XSSFWorkbook(new ByteArrayInputStream(filePath));
		//	ISheet sheet = workbook.GetSheetAt(0); // Assuming the first sheet

		//	for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++) // Skipping the header row
		//	{
		//		IRow row = sheet.GetRow(rowIndex);

		//		if (row != null)
		//		{
		//			var _salesimport = new SalesImportDataEntity
		//			{
		//				ID = (Int64)row.GetCell(0).NumericCellValue,
		//				RetailerNumber = row.GetCell(1).StringCellValue,
		//				AccountName = row.GetCell(2).StringCellValue,
		//				TransactionID = row.GetCell(3).StringCellValue,
		//				TransactionType = row.GetCell(4).StringCellValue,
		//				CardID = row.GetCell(5).StringCellValue,
		//				PAN = row.GetCell(6).StringCellValue,
		//				TransactionDateTime = row.GetCell(7).StringCellValue,
		//				Value = (Int64)row.GetCell(8).NumericCellValue,
		//				BINNumber = row.GetCell(9).StringCellValue,
		//				StoreNo = (Int64)row.GetCell(10).NumericCellValue,
		//				StoreName = row.GetCell(11).StringCellValue,
		//				Product = row.GetCell(12).StringCellValue,
		//				EAN = (Int64)row.GetCell(13).NumericCellValue
		//			};

		//			list.Add(_salesimport);
		//		}
		//	}


		//	return list;
		//}
		#endregion
		public ActionResult TransformSalesRedemptionsData()
		{
			string errorMessage = string.Empty;
			var result = TransformSalesData();
			if ((int)result.Value == -10)
			{
				errorMessage = "Please update the salesstore with the commission percentage!";
			}
			if ((int)result.Value == -11)
			{
				errorMessage = "Please update the product with the commission percentage!";
			}
			if ((int)result.Value == -12)
			{
				errorMessage = "No sales data found.";
			}
			if ((int)result.Value == -13)
			{
				errorMessage = "No product data found.";
			}

			var redemresult = TransformRedeemData();
			//if ((int)redemresult.Value == -10)
			//{
			//	errorMessage = "Please update the Database with the Store addresses!";
			//}
			if ((int)redemresult.Value == -11)
			{
				errorMessage = "Please update the product with the commission percentage!";
			}
			if ((int)redemresult.Value == -12)
			{
				errorMessage = "No redemption data found.";
			}
			if ((int)redemresult.Value == -13)
			{
				errorMessage = "No product data found.";
			}

			return Json(new { salesstatus = (int)result.Value, redemstatus = (int)redemresult.Value, message = errorMessage, type = "redeem" });
		}
		public string TransformSalesRedemptionsDataString()
		{
			string errorMessage = string.Empty;
			var result = TransformSalesData();
			if ((int)result.Value == -10)
			{
				errorMessage = "Please update the salesstore with the commission percentage!";
			}
			if ((int)result.Value == -11)
			{
				errorMessage = "Please update the product with the commission percentage!";
			}
			if ((int)result.Value == -12)
			{
				errorMessage = "No sales data found.";
			}
			if ((int)result.Value == -13)
			{
				errorMessage = "No product data found.";
			}

			var redemresult = TransformRedeemData();
			if ((int)redemresult.Value == -10)
			{
				errorMessage += Environment.NewLine + "Please update the redemptions with the Store addresses!";
			}
			if ((int)redemresult.Value == -11)
			{
				errorMessage += Environment.NewLine + "Please update the product with the commission percentage!";
			}
			if ((int)redemresult.Value == -12)
			{
				errorMessage += Environment.NewLine + "No redemption data found.";
			}
			if ((int)redemresult.Value == -13)
			{
				errorMessage += Environment.NewLine + "No product data found.";
			}

			return errorMessage;
		}
		#endregion

		#region BlackHawk
		[HttpPost]
		public IActionResult uploadfileblackhawkdata(IFormFile fileblackhawkdata)
		{
			Helpers.ApplicationExceptions.SaveActivityLog("uploadfileblackhawkdata action method called.");
			try
			{
				if (fileblackhawkdata == null)
				{
					return Json(-5);
				}

				else
				{
					var list = new List<BlackHawkEntityImportDataEntity>();
					using (var ms = new MemoryStream())
					{
						fileblackhawkdata.CopyTo(ms);
						var fileBytes = ms.ToArray();
						list = ReadBlackHawkCsvFile(fileBytes);
						if (list == null || (list != null && list.Count == 0))
						{
							return Json(-3);
						}
						//var findLoadtypeData = list.Where(x => x.TransactionType.ToLower() == "redeem").ToList();
						//if (findLoadtypeData == null || (findLoadtypeData != null && findLoadtypeData.Count == 0))
						//{
						//	return Json(-4); //Transaction type redeem not found
						//}
						//var findtrantypenotload = list.Where(x => x.TransactionType.ToLower() != "redeem").ToList();
						//if (findtrantypenotload != null && findtrantypenotload.Count > 0)
						//{
						//	return Json(-7); //All record should have transaction type redeem.
						//}
						//var findStoreNoEmptyNUll = list.Where(x => x.StoreNo < 1).ToList();
						//if (findStoreNoEmptyNUll != null && findStoreNoEmptyNUll.Count > 0)
						//{
						//	return Json(-6); // StoreNo is empty
						//}
					}

					var errormessage = ProcessBlackHawkImportData(list);
					if (!string.IsNullOrEmpty(errormessage))
					{
						return Json(errormessage);
					}
					else
					{
						return Json(1);
					}

				}
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return Json(-1);
			}
			return Json(1);
		}
		public static List<BlackHawkEntityImportDataEntity> ReadBlackHawkCsvFile(byte[] filePath)
		{
			var list = new List<BlackHawkEntityImportDataEntity>();
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null, // Ignore missing fields
			};

			using (var reader = new StreamReader(new ByteArrayInputStream(filePath)))
			using (var csv = new CsvReader(reader, config))
			{
				// Skip the first row

				csv.Read();

				// Read the second row as the header
				csv.Read();
				csv.ReadHeader();

				// Read all records into a list
				list = csv.GetRecords<BlackHawkEntityImportDataEntity>().ToList();

				if (list != null && list.Count > 0)
				{
					// Skip the last row by taking all but the last record
					list = list.Take(list.Count - 1).ToList();

					
				}
			}


			return list;
		}
		private string ProcessBlackHawkImportData(List<BlackHawkEntityImportDataEntity> list)
		{
			var errorMessage = string.Empty;
			foreach (var item in list)
			{
				
				
				#region SaveBlackHawkSalesData 
				#region CheckBlackHawkSalesData
				var _checkSalesData = _retailerRepository.GetBlackHawkDataByID(item.REFERENCENUMBER);
				Helpers.ApplicationExceptions.SaveActivityLog("GetBlackHawkDataByID " + item.REFERENCENUMBER);
				if (_checkSalesData == null)//It means SalesID not exist need to insert.
				{
					Helpers.ApplicationExceptions.SaveActivityLog("GetBlackHawkDataByID == null" + item.REFERENCENUMBER);
					BlackHawkSalesEntity salesEntity = new BlackHawkSalesEntity();
					salesEntity.CompanyName = item.CompanyName;
					salesEntity.MerchantName = item.MerchantName;
					salesEntity.REFERENCENUMBER = item.REFERENCENUMBER;
					salesEntity.StoreID = item.StoreID;
					salesEntity.StoreName = item.StoreName;
					salesEntity.GIFTCARDNUMBER = item.GIFTCARDNUMBER;
					DateTime _trandate;
					if (DateTime.TryParseExact(item.POSTRANSACTIONDATE, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						salesEntity.TransactionDate = _trandate;


					}
					else if (DateTime.TryParseExact(item.POSTRANSACTIONDATE, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						salesEntity.TransactionDate = _trandate;


					}
					else if (DateTime.TryParseExact(item.POSTRANSACTIONDATE, "dd/MM/yyyy HH:mm tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						salesEntity.TransactionDate = _trandate;


					}
					else if (DateTime.TryParseExact(item.POSTRANSACTIONDATE, "dd/MM/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						salesEntity.TransactionDate = _trandate;


					}
					else if (DateTime.TryParseExact(item.POSTRANSACTIONDATE, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						salesEntity.TransactionDate = _trandate;


					}
					else if (DateTime.TryParseExact(item.POSTRANSACTIONDATE, "ddMMyyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
					{
						salesEntity.TransactionDate = _trandate;


					}

					salesEntity.TRANSACTIONAMOUNT = Convert.ToDecimal(item.TRANSACTIONAMOUNT);
					salesEntity.PRODUCTDESCRIPTION = item.PRODUCTDESCRIPTION;
					salesEntity.SOURCETRANSACTIONID = item.SOURCETRANSACTIONID;
					salesEntity.TransactionDate = _trandate;//DateTime.ParseExact(item.TransactionDateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					
				
					var savesaleresult = _retailerRepository.SaveBlackHawkSalesInformation(salesEntity);
					if (savesaleresult < 0)
					{
						Helpers.ApplicationExceptions.SaveActivityLog("Unable to save sales ID record " + item.REFERENCENUMBER);
						errorMessage = "Unable to save sales data with record ID " + item.REFERENCENUMBER;
						break;
					}
					else
					{
						Helpers.ApplicationExceptions.SaveActivityLog("saved sales ID record " + item.REFERENCENUMBER);
					}

				}

				#endregion

				#endregion
			}
			if (string.IsNullOrEmpty(errorMessage))
			{
				var result = TransformBlackHawkData(); 
				if ((int)result.Value == -10)
				{
					errorMessage = "Please update the salesstore with the commission percentage!";
				}
				if ((int)result.Value == -11)
				{
					errorMessage = "Please update the product with the commission percentage!";
				}
				if ((int)result.Value == -12)
				{
					errorMessage = "No sales data found.";
				}
				if ((int)result.Value == -13)
				{
					errorMessage = "No product data found.";
				}
			}



			return errorMessage;
		}
		public JsonResult TransformBlackHawkData()
		{
		
			var result = _retailerRepository.TransformBlackHawkSalesData();
			return Json(result);
		}
		#endregion
		public IActionResult BlackHawkIndex()
		{
			Helpers.ApplicationExceptions.SaveActivityLog("BlackHawkIndex action method called.");

			return View();
		}
		public IActionResult BlackHawkList()
		{
			var _list = _retailerRepository.GetBlackHawkSalesList();

			return View(_list);
		}
		public IActionResult ExportBlackHawkList()
		{
			var _list = _retailerRepository.GetBlackHawkSalesList();

			return View(_list);
		}
		public IActionResult DeleteBlackHawk(Int64 ID)
		{
			var result = _retailerRepository.DeleteBlackHawkInformation(ID);
			return Json(result);
		}
		#region BreakageDataImport
		[HttpPost]
		public IActionResult uploadfilebreakagedata(IFormFile filebreakagedata)
		{
			Helpers.ApplicationExceptions.SaveActivityLog("uploadfilebreakagedata action method called.");
			try
			{
				if (filebreakagedata == null)
				{
					return Json(-5);
				}

				else
				{
					var list = new List<BreakageEntityImportDataEntity>();
					using (var ms = new MemoryStream())
					{
						filebreakagedata.CopyTo(ms);
						var fileBytes = ms.ToArray();
						list = ReadExcelFile<BreakageEntityImportDataEntity>(fileBytes);
						if (list == null || (list != null && list.Count == 0))
						{
							return Json(-3);
						}
						//var findLoadtypeData = list.Where(x => x.TransactionType.ToLower() == "redeem").ToList();
						//if (findLoadtypeData == null || (findLoadtypeData != null && findLoadtypeData.Count == 0))
						//{
						//	return Json(-4); //Transaction type redeem not found
						//}
						//var findtrantypenotload = list.Where(x => x.TransactionType.ToLower() != "redeem").ToList();
						//if (findtrantypenotload != null && findtrantypenotload.Count > 0)
						//{
						//	return Json(-7); //All record should have transaction type redeem.
						//}
						//var findStoreNoEmptyNUll = list.Where(x => x.StoreNo < 1).ToList();
						//if (findStoreNoEmptyNUll != null && findStoreNoEmptyNUll.Count > 0)
						//{
						//	return Json(-6); // StoreNo is empty
						//}
					}

					var errormessage = ProcessBreakageImportData(list);
					if (!string.IsNullOrEmpty(errormessage))
					{
						return Json(errormessage);
					}
					else
					{
						return Json(1);
					}

				}
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return Json(-1);
			}
			return Json(1);
		}
		public List<T> ReadExcelFile<T>(byte[] filePath) where T : new()
		{
			var entities = new List<T>();

			// Enable EPPlus license
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			using var package = new ExcelPackage(new ByteArrayInputStream(filePath));
			var worksheet = package.Workbook.Worksheets[0]; // Assume the first sheet

			// Read headers from the first row
			var headers = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]
						  .Select(cell => cell.Text.Trim())
						  .ToList();
			// Get properties with ExcelHeader attributes
			var propertyMap = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
									   .Where(prop => Attribute.IsDefined(prop, typeof(ExcelHeaderAttribute)))
									   .ToDictionary(
										   prop => prop.GetCustomAttribute<ExcelHeaderAttribute>().HeaderName,
										   prop => prop
									   );

			// Map rows to the entity
			for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
			{
				var entity = new T();

				foreach (var header in headers)
				{
					if (propertyMap.TryGetValue(header, out var property))
					{
						var cellValue = worksheet.Cells[row, headers.IndexOf(header) + 1].Text;

						if (!string.IsNullOrWhiteSpace(cellValue))
						{
							var convertedValue = Convert.ChangeType(cellValue, property.PropertyType);
							property.SetValue(entity, convertedValue);
						}
					}
				}

				entities.Add(entity);
			}

			return entities;
		}

		private string ProcessBreakageImportData(List<BreakageEntityImportDataEntity> list)
		{
			var errorMessage = string.Empty;
			foreach (var item in list)
			{


				#region SaveBreakageData 
				#region CheckBreakageData
				var _checkSalesData = _redemptionsRepository.GetBreakageDataByID(item.Activation_TXID);
				Helpers.ApplicationExceptions.SaveActivityLog("GetBreakageDataByID " + item.Activation_TXID);
				if (_checkSalesData == null)//It means SalesID not exist need to insert.
				{
					Helpers.ApplicationExceptions.SaveActivityLog("GetBreakageDataByID == null" + item.Activation_TXID);
					BreakageRedeemEntity salesEntity = new BreakageRedeemEntity();
					salesEntity.Totally_Direct = item.Totally_Direct;
					salesEntity.Activation_TXID = item.Activation_TXID;
					salesEntity.gift_card_number = item.gift_card_number;
					salesEntity.gift_card_id = item.gift_card_id;
					//salesEntity.StoreID = item.StoreID;
					salesEntity.category = item.category;
					salesEntity.gift_card_status = item.gift_card_status;
					salesEntity.redemption_status = item.redemption_status;
					salesEntity.gift_card_status = item.gift_card_status;
					salesEntity.redemption_month = item.redemption_month;
					salesEntity.redemption_year = item.redemption_year;

					DateTime _trandate;
					if(!string.IsNullOrEmpty(item.activation_day) && !string.IsNullOrEmpty(item.activation_month) && !string.IsNullOrEmpty(item.activation_year)
						&& item.activation_day !="00" && item.activation_month != "00" && item.activation_year != "00"
						&& item.activation_day != "0" && item.activation_month != "0" && item.activation_year != "0"
						)
					{
						var _crtrndatestr = item.activation_day +"/" +item.activation_month + "/" +item.activation_year;
						if (DateTime.TryParseExact(_crtrndatestr, "dd/MM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
						{
							salesEntity.TransactionDate = _trandate;


						}
						else if (DateTime.TryParseExact(_crtrndatestr, "dd/MMM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
						{
							salesEntity.TransactionDate = _trandate;


						}
						else if (DateTime.TryParseExact(_crtrndatestr, "d/MMM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
						{
							salesEntity.TransactionDate = _trandate;


						}
						else if (DateTime.TryParseExact(_crtrndatestr, "dd/MMMM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
						{
							salesEntity.TransactionDate = _trandate;


						}
						else if (DateTime.TryParseExact(_crtrndatestr, "d/MMMM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _trandate))
						{
							salesEntity.TransactionDate = _trandate;


						}
					}
					DateTime _expirydate;
					if (!string.IsNullOrEmpty(item.expiration_day) && !string.IsNullOrEmpty(item.expiration_month) && !string.IsNullOrEmpty(item.activation_year)
						&& item.expiration_day != "00" && item.expiration_month != "00" && item.activation_year != "00"
						&& item.expiration_day != "0" && item.expiration_month != "0" && item.activation_year != "0"
						)
					{
						var _crtrndatestr = item.expiration_day + "/" + item.expiration_month + "/" + item.activation_year;
						if (DateTime.TryParseExact(_crtrndatestr, "dd/MM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _expirydate))
						{
							salesEntity.ExpiryDate = _expirydate;


						}
						else if (DateTime.TryParseExact(_crtrndatestr, "dd/MMM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _expirydate))
						{
							salesEntity.ExpiryDate = _expirydate;


						}
						else if (DateTime.TryParseExact(_crtrndatestr, "d/MMM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _expirydate))
						{
							salesEntity.ExpiryDate = _expirydate;


						}
						else if (DateTime.TryParseExact(_crtrndatestr, "dd/MMMM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _expirydate))
						{
							salesEntity.ExpiryDate = _expirydate;


						}
						else if (DateTime.TryParseExact(_crtrndatestr, "d/MMMM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _expirydate))
						{
							salesEntity.ExpiryDate = _expirydate;


						}
					}
					if(salesEntity.TransactionDate != null)
					{
						salesEntity.ExpiryDate = Convert.ToDateTime(salesEntity.TransactionDate).AddYears(1) ;
					}
					if(!string.IsNullOrEmpty(item.initial_face_value))
					salesEntity.initial_face_value = Convert.ToDecimal(item.initial_face_value);
					if (!string.IsNullOrEmpty(item.redemption_amount))
						salesEntity.redemption_amount = Convert.ToDecimal(item.redemption_amount);
					if (!string.IsNullOrEmpty(item.current_value))
						salesEntity.current_value = Convert.ToDecimal(item.current_value);




					var savesaleresult = _redemptionsRepository.SaveBreakageInformation(salesEntity);
					if (savesaleresult < 0)
					{
						Helpers.ApplicationExceptions.SaveActivityLog("Unable to save Activation_TXID ID record " + item.Activation_TXID);
						errorMessage = "Unable to save ProcessBreakageImportData data with record ID " + item.Activation_TXID;
						break;
					}
					else
					{
						Helpers.ApplicationExceptions.SaveActivityLog("saved Activation_TXID ID record " + item.Activation_TXID);
					}

				}

				#endregion

				#endregion
			}
			//if (string.IsNullOrEmpty(errorMessage))
			//{
			//	var result = TransformBlackHawkData();
			//	if ((int)result.Value == -10)
			//	{
			//		errorMessage = "Please update the salesstore with the commission percentage!";
			//	}
			//	if ((int)result.Value == -11)
			//	{
			//		errorMessage = "Please update the product with the commission percentage!";
			//	}
			//	if ((int)result.Value == -12)
			//	{
			//		errorMessage = "No sales data found.";
			//	}
			//	if ((int)result.Value == -13)
			//	{
			//		errorMessage = "No product data found.";
			//	}
			//}



			return errorMessage;
		}
		public JsonResult TransformBreakageData()
		{

			var result = _retailerRepository.TransformBlackHawkSalesData();
			return Json(result);
		}
		#endregion

	}
}
