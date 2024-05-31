using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public class DALProducts
	{
		DapperManager dapper;
		public DALProducts()
		{
			dapper = new DapperManager();
		}
		public List<ProductEntity> GetProductList(Int64 productid=0)
		{

			string Query = string.Format($"SELECT p.[ProductID]\r\n ,p.[ProductName]\r\n " +
				$" ,p.[ProductEAN]\r\n   " +
				$" ,p.[ProductType]\r\n  " +
				$",isnull(p.[Commission],0) * 100 as Commission\r\n " +
				$" FROM [Products] p \r\n" +
				$" where p.IsActive=1 and p.IsDeleted=0 " +
				$" order by ProductID desc");
			var Data = dapper.Query<ProductEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public ProductEntity GetProductDetail(Int64 productid = 0)
		{

			string Query = string.Format($"SELECT p.[ProductID]\r\n ,p.[ProductName]\r\n " +
				$" ,p.[ProductEAN]\r\n   " +
				$" ,p.[ProductType]\r\n  " +
				$",isnull(p.[Commission],0) * 100 as Commission\r\n " +
				$" FROM [Products] p \r\n" +
				$" where p.IsActive=1 and p.IsDeleted=0 " +
				$" and p.ProductID = " + productid);
			var Data = dapper.Query<ProductEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList().FirstOrDefault();
		}
		public int SaveProductInformation(ProductEntity obj)
		{
			string Query = "";
			if (obj.ProductID > 0 )
			{
				Query = string.Format($"UPDATE [dbo].[Products] SET [ProductEAN] = '" + obj.ProductEAN + "'" +
					$",[ProductName] = '" + obj.ProductName + "' " +
					$",[ProductType] = '" + obj.ProductType + "'" +
					$",[Commission] ='" + obj.Commission + "'" +
					$",[UpdatedAt] = GetDate() " +
					$",[UpdatedBy] = 100 " +
					$" WHERE [ProductID] = '" + obj.ProductID + "'");
			}
			else
			{
				Query = string.Format($"INSERT INTO [dbo].[Products] ([ProductEAN] ,[ProductName],[ProductType],[Commission] ,[IsActive],[IsDeleted],[CreatedAt],[UpdatedAt],[CreatedBy] ,[UpdatedBy] )" +
					$" VALUES('" + obj.ProductEAN + "','" + obj.ProductName + "'" +
					$",'" + obj.ProductType + "' ,'" + obj.Commission + "'" +
					$",'1' ,'0'" +
					$", GetDate() ,GetDate()" +
					$",'100'" +
					$",'100' )");

			}
			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public int DeleteProductInformation(Int64 ID)
		{
			string Query = "";

			Query = string.Format($"UPDATE [dbo].[Products] SET [IsActive] = '0'" +
				$",[IsDeleted] = '1' " +
				$",[UpdatedAt] = GetDate() " +
				$",[UpdatedBy] = 100 " +
				$" WHERE [ProductID] = '" + ID + "'");

			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
	}
}
