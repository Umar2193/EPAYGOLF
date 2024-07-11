using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public class DALStoresRedeem
	{
		DapperManager dapper;
		public DALStoresRedeem()
		{
			dapper = new DapperManager();
		}
		public List<StoresRedeemEntity> GetStoresRedeemList(Int64 ID = 0)
		{

			string Query = string.Format($"SELECT  [StoresRedeemID],[StoreNo] ,[StoreName],[Email] " +
	  $" , [Address1], [Address2], [CityTown], [PostCode], [UserID] " +
	  $" , [Title], [FirstName], [LastName], [UserEmail], [StoreNameUser] " +
	  $" , [IsActive], [IsDeleted], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy] " +
	  $" ,[SortCode],[AccountNumber],[NameBankAccountHolder] " +
	  $" FROM [dbo].[StoresRedeem] " +
	  $" where IsActive = 1 and IsDeleted = 0 " +
	  $" order by CreatedAt desc");
			var Data = dapper.Query<StoresRedeemEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public StoresRedeemEntity GetStoresRedeemDetail(Int64 ID = 0)
		{

			string Query = string.Format($"SELECT  [StoresRedeemID],[StoreNo] ,[StoreName],[Email] " +
	  $" , [Address1], [Address2], [CityTown], [PostCode], [UserID] " +
	  $" , [Title], [FirstName], [LastName], [UserEmail], [StoreNameUser] " +
	  $" , [IsActive], [IsDeleted], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy] " +
	  $" ,[SortCode],[AccountNumber],[NameBankAccountHolder] " +
	  $" FROM [dbo].[StoresRedeem] " +
	  $" where IsActive = 1 and IsDeleted = 0 " +
				$" and StoresRedeemID = " + ID);
			var Data = dapper.Query<StoresRedeemEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList().FirstOrDefault();
		}
		public int SaveStoresRedeemInformation(StoresRedeemEntity obj)
		{
			string Query = "";
			if (obj.StoresRedeemID > 0)
			{
				Query = string.Format($"UPDATE [dbo].[StoresRedeem] " +
					$"SET [StoreNo] = '" + obj.StoreNo + "'" +
					$",[StoreName] = '" + obj.StoreName + "' " +
					$",[Email] = '" + obj.Email + "'" +
					$",[Address1] ='" + obj.Address1 + "'" +
					$",[Address2] ='" + obj.Address2 + "'" +
					$",[CityTown] ='" + obj.CityTown + "'" +
					$",[PostCode] ='" + obj.PostCode + "'" +
					$",[UserID] ='" + obj.UserID + "'" +
					$",[Title] ='" + obj.Title + "'" +
					$",[FirstName] ='" + obj.FirstName + "'" +
					$",[LastName] ='" + obj.LastName + "'" +
					$",[UserEmail] ='" + obj.UserEmail + "'" +
					$",[StoreNameUser] ='" + obj.StoreNameUser + "'" +
					$",[SortCode] ='" + obj.SortCode + "'" +
					$",[AccountNumber] ='" + obj.AccountNumber + "'" +
					$",[NameBankAccountHolder] ='" + obj.NameBankAccountHolder + "'" +

					$",[UpdatedAt] = GetDate() " +
					$",[UpdatedBy] = 100 " +
					$" WHERE [StoresRedeemID] = '" + obj.StoresRedeemID + "'");
			}
			else
			{
				Query = string.Format($"INSERT INTO [dbo].[StoresRedeem] ([StoreNo] ,[StoreName],[Email],[Address1],[Address2],[CityTown],[PostCode],[UserID],[Title],[FirstName],[LastName],[UserEmail],[StoreNameUser],[SortCode],[AccountNumber],[NameBankAccountHolder],[IsActive],[IsDeleted],[UpdatedAt],[CreatedAt],[CreatedBy],[UpdatedBy])" +
					$" VALUES('" + obj.StoreNo + "','" + obj.StoreName + "'" +
					$",'" + obj.Email + "' ,'" + obj.Address1 + "'" +
					$",'" + obj.Address2 + "' ,'" + obj.CityTown + "'" +
					$",'" + obj.PostCode + "' ,'" + obj.UserID + "'" +
					$",'" + obj.Title + "' ,'" + obj.FirstName + "'" +
					$",'" + obj.LastName + "' ,'" + obj.UserEmail + "'" +
					$",'" + obj.StoreNameUser + "' "+
				    $",'" + obj.SortCode + "' ,'" + obj.AccountNumber + "'" +
					$",'" + obj.NameBankAccountHolder + "' " +
					$",'1' ,'0'" +
					$", GetDate() ,GetDate()" +
					$",'100'" +
					$",'100' )");

			}
			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public int DeleteStoresRedeemInformation(Int64 ID)
		{
			string Query = "";

			Query = string.Format($"UPDATE [dbo].[StoresRedeem] SET [IsActive] = '0'" +
				$",[IsDeleted] = '1' " +
				$",[UpdatedAt] = GetDate() " +
				$",[UpdatedBy] = 100 " +
				$" WHERE StoresRedeemID = '" + ID + "'");

			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public List<StoresRedeemEntity> GetStoreRedeemByStoreNo(string storeno)
		{

			var result = GetStoresRedeemList().ToList().Where(x => x.StoreNo.ToString() == storeno).ToList();
			return result;
		}
	}
}
