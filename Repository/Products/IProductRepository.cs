using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Products
{
	public interface IProductRepository
	{
		List<ProductEntity> GetProductList();
		ProductEntity GetProductDetail(Int64 ProductID);
		int SaveProductInformation(ProductEntity obj);
		int DeleteProductInformation(Int64 ID);
		List<ProductEntity> GetProductByEAN(string EAN);
	}
}
