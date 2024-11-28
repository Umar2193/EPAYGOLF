using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
	public class BreakageEntity
	{
	}
	public class BreakageEntityImportDataEntity
	{
		[ExcelHeader("Activation TXID")]
		public string Activation_TXID { get; set; }
		[ExcelHeader("gift_card_number")]
		public string gift_card_number { get; set; }
		[ExcelHeader("gift_card_id")]
		public string gift_card_id { get; set; }
		[ExcelHeader("category")]
		public string category { get; set; }
		[ExcelHeader("initial_face_value")]
		public string initial_face_value { get; set; }
		[ExcelHeader("redemption_amount")]
		public string redemption_amount { get; set; }
		[ExcelHeader("current_value")]
		public string current_value { get; set; }
		[ExcelHeader("activation_month")]
		public string activation_month { get; set; }
		[ExcelHeader("activation_day")]
		public string activation_day { get; set; }
		[ExcelHeader("activation_year")]
		public string activation_year { get; set; }
		[ExcelHeader("expiration_month")]
		public string expiration_month{ get; set; }
		[ExcelHeader("expiration_day")]
		public string expiration_day{ get; set; }
		[ExcelHeader("expiration_year")]
		public string expiration_year{ get; set; }
		[ExcelHeader("gift_card_status")]
		public string gift_card_status{ get; set; }
		[ExcelHeader("redemption_status")]
		public string redemption_status{ get; set; }
		[ExcelHeader("redemption_month")]
		public string redemption_month{ get; set; }
		[ExcelHeader("redemption_year")]
		public string redemption_year{ get; set; }
		[ExcelHeader("Totally Direct")]
		public string Totally_Direct { get; set; }



	}
	public class BreakageRedeemEntity
	{
		public Int64 BreakageID { get; set; }
	
		public string StoreID { get; set; }

		public string Activation_TXID { get; set; }
		public string gift_card_number { get; set; }
		public string gift_card_id { get; set; }
		public string category { get; set; }
		public decimal initial_face_value { get; set; }
		public decimal redemption_amount { get; set; }
		public decimal current_value { get; set; }
		public DateTime? TransactionDate { get; set; }
		public DateTime? ExpiryDate { get; set; }
		
		public string gift_card_status { get; set; }
	
		public string redemption_status { get; set; }
		
		public string redemption_month { get; set; }
	
		public string redemption_year { get; set; }
	
		public string Totally_Direct { get; set; }


		
	}
}
