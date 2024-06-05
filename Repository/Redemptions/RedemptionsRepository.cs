using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Redemptions
{
	public class RedemptionsRepository:IRedemptionsRepository
	{
		DAL.DALRedemptions _DALredemptions = new DAL.DALRedemptions();
		public List<RedemptionsEntity> GetRedemptionsList()
		{
			try
			{

				return _DALredemptions.GetRedemptionsList();
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public RedemptionsEntity GetRedemptionsDetail(Int64 ID)
		{
			try
			{

				return _DALredemptions.GetRedemptionsDetail(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public int SaveRedemptionsInformation(RedemptionsEntity obj)
		{
			try
			{

				return _DALredemptions.SaveRedemptionsInformation(obj);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public int DeleteRedemptionsInformation(Int64 ID)
		{
			try
			{

				return _DALredemptions.DeleteRedemptionsInformation(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public  RedemptionsEntity GetredeemDataByID(string ID)
		{
			try
			{

				return _DALredemptions.GetredeemDataByID(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public int TransformRedeemData()
		{
			try
			{

				return _DALredemptions.TransformRedeemData();
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
	}
}
