using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Products
{
	public class ProductRepository:IProductRepository
	{
		DAL.DALProducts _DALproducts = new DAL.DALProducts();
		public List<ProductEntity> GetProductList()
		{
			try
			{

				return _DALproducts.GetProductList();
			}
			catch(Exception ex) 
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public ProductEntity GetProductDetail(Int64 ProductID)
		{
			try
			{

				return _DALproducts.GetProductDetail(ProductID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public int SaveProductInformation(ProductEntity obj)
		{
			try
			{

				return _DALproducts.SaveProductInformation(obj);
			}
			catch( Exception ex) 
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public int DeleteProductInformation(Int64 ID)
		{
			try
			{

				return _DALproducts.DeleteProductInformation(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public List<ProductEntity> GetProductByEAN(string EAN)
		{
			try
			{

				return _DALproducts.GetProductByEAN(EAN);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
	}
}
