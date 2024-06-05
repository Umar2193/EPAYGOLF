using Entity;
using Microsoft.AspNetCore.Mvc;
using Repository.Products;
using Repository.SalesStore;
using Repository.StoresRedeem;
using Repository.VATRates;
using System.Text.Json.Serialization;

namespace EPAYGOLF.Controllers
{
    public class MetaDataController : Controller
    {
		private IProductRepository _productRepository = new ProductRepository();

		private IVATRatesRepository _vatratesRepository = new VATRatesRepository();
		private IStoresRedeemRepository _storesredeemRepository = new StoresRedeemRepository();
		private ISalesStoreRepository _salesstoreRepository = new SalesStoreRepository();

		public IActionResult Index()
        {
            return View();
        }
		#region Products
		public IActionResult ProductIndex()
        {
            Helpers.ApplicationExceptions.SaveActivityLog("ProductIndex action method called.");
         
            return View();
        }
        public IActionResult ProductList()
        {
            var _product= _productRepository.GetProductList();

			return View(_product);
        }
        public IActionResult AddUpdateProduct(Int64 ProductID) {
            var model = _productRepository.GetProductDetail(ProductID);
            if(model == null)
            {
                model= new ProductEntity();
            }
			return View(model);
        }
        [HttpPost]
		public IActionResult SaveProducts(ProductEntity obj)
		{
			if (obj != null)
			{
				if (obj.Commission > 0)
				{
					obj.Commission = obj.Commission / 100;
				}
				
				
			}
			var result= _productRepository.SaveProductInformation(obj);
			return Json(result);
		}
		public IActionResult DeletePorduct(Int64 ID)
		{
			var result = _productRepository.DeleteProductInformation(ID);
			return Json(result);
		}
		#endregion
		#region VATRates
		public IActionResult VATRatesIndex()
		{
			Helpers.ApplicationExceptions.SaveActivityLog("VATRatesIndex action method called.");

			return View();
		}
		public IActionResult VATRatesList()
		{
			var _list = _vatratesRepository.GetVATRatesList();

			return View(_list);
		}
		public IActionResult AddUpdateVATRates(Int64 ID)
		{
			var model = _vatratesRepository.GetVATRatesDetail(ID);
			if (model == null)
			{
				model = new VATRatesEntity();
				//model.VATRateDate = DateTime.Now;
			}
			return View(model);
		}
		[HttpPost]
		public IActionResult SaveVATRates(VATRatesEntity obj)
		{
			var result = _vatratesRepository.SaveVATRatesInformation(obj);
			return Json(result);
		}
		public IActionResult DeleteVATRates(Int64 ID)
		{
			var result = _vatratesRepository.DeleteVATRatesInformation(ID);
			return Json(result);
		}
		#endregion
		#region StoresRedeem
		public IActionResult StoresRedeemIndex()
		{
			Helpers.ApplicationExceptions.SaveActivityLog("StoresRedeemIndex action method called.");

			return View();
		}
		public IActionResult StoresRedeemList()
		{
			var _list = _storesredeemRepository.GetStoresRedeemList();

			return View(_list);
		}
		public IActionResult AddUpdateStoresRedeem(Int64 ID)
		{
			var model = _storesredeemRepository.GetStoresRedeemDetail(ID);
			if (model == null)
			{
				model = new StoresRedeemEntity();
			}
			return View(model);
		}
		[HttpPost]
		public IActionResult SaveStoresRedeem(StoresRedeemEntity obj)
		{
			if (obj != null)
			{
				if(obj.StoresRedeemID == 0)
				{
					var ifexist = _storesredeemRepository.GetStoresRedeemList().Where(x => x.StoreNo == obj.StoreNo).FirstOrDefault();
					if(ifexist != null)
					{
						return Json(-2);
					}
				}
			}
			var result = _storesredeemRepository.SaveStoresRedeemInformation(obj);
			return Json(result);
		}
		public IActionResult DeleteStoresRedeem(Int64 ID)
		{
			var result = _storesredeemRepository.DeleteStoresRedeemInformation(ID);
			return Json(result);
		}
		#endregion
		#region SalesStore
		public IActionResult SalesStoreIndex()
		{
			Helpers.ApplicationExceptions.SaveActivityLog("SalesStore action method called.");

			return View();
		}
		public IActionResult SalesStoreList()
		{
			var _list = _salesstoreRepository.GetSalesStoreList();

			return View(_list);
		}
		public IActionResult AddUpdateSalesStore(Int64 ID)
		{

			var model = _salesstoreRepository.GetSalesStoreDetail(ID);
			if (model == null)
			{
				model = new SalesStoreEntity();
			}
			return View(model);
		}
		[HttpPost]
		public IActionResult SaveSalesStore(SalesStoreEntity obj)
		{
			if(obj != null)
			{
				
				if(obj.CommisionID == 0)
				{
					var ifexist = _salesstoreRepository.GetSalesStoreList().Where(x => x.RetailerID == obj.RetailerID).FirstOrDefault();
					if(ifexist != null)
					{
						return Json(-2);
					}
				}
			
				if(obj.Commission > 0)
				{
					obj.Commission = obj.Commission / 100;
				}
				if (obj.StripeFee > 0)
				{
					obj.StripeFee = obj.StripeFee / 100;
				}
				if (obj.ProcessingFee > 0)
				{
					obj.ProcessingFee = obj.ProcessingFee / 100;
				}
				if (obj.GGCRedemption > 0)
				{
					obj.GGCRedemption = obj.GGCRedemption / 100;
				}
				if (obj.TransactionFee > 0)
				{
					obj.TransactionFee = obj.TransactionFee / 100;
				}
			}
			var result = _salesstoreRepository.SaveSalesStoreInformation(obj);
			return Json(result);
		}
		public IActionResult DeleteSalesStore(Int64 ID)
		{
			var result = _salesstoreRepository.DeleteSalesStoreInformation(ID);
			return Json(result);
		}
		#endregion

	}
}
