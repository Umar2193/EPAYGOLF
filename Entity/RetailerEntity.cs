using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Entity
{
	public class RetailerEntity
	{
		public Int64 RetailerID { get; set; }
		public string RetailerName { get; set; }
		public string RetailerCode { get; set; }
		public decimal Commission { get; set; }
	}
	public class BlackHawkEntityImportDataEntity
	{
		[Name("COMPANY NAME")]
		public string CompanyName { get; set; }
		[Name("MERCHANT NAME")]
		public string MerchantName { get; set; }
		[Name("STORE ID")]
		public string StoreID { get; set; }
		[Name("STORE NAME")]
		public string StoreName { get; set; }
        [Name("ACQUIRED TRANSACTION DATE")]
        public string ACQUIREDTRANSACTIONDATE { get; set; }
        [Name("GIFT CARD NUMBER")]
        public string GIFTCARDNUMBER{ get; set; }
        [Name("PRODUCT DESCRIPTION")]
        public string PRODUCTDESCRIPTION { get; set; }
		[Name("POS TRANSACTION DATE")]
		public string POSTRANSACTIONDATE { get; set;}
		[Name("MERCHANT TRANSACTION ID/ REFERENCE NUMBER")]
		public string REFERENCENUMBER { get; set; }
        [Name("TRANSACTION AMOUNT")]
        public string TRANSACTIONAMOUNT { get; set; }
		[Name("SOURCE TRANSACTION ID")]
		public string SOURCETRANSACTIONID { get; set; }
       


    }
	public class BlackHawkSalesEntity
	{
        public Int64 BlackHawkSalesID { get; set; }
        public string CompanyName { get; set; }
		public string MerchantName { get; set; }
		public string StoreID { get; set; }
		public string StoreName { get; set; }
		
		public string ACQUIREDTRANSACTIONDATE { get; set; }
	
		public string GIFTCARDNUMBER { get; set; }
	
		public string PRODUCTDESCRIPTION { get; set; }
		
		public string POSTRANSACTIONDATE { get; set; }
	
		public string REFERENCENUMBER { get; set; }
		
		public decimal TRANSACTIONAMOUNT { get; set; }
		
		public string SOURCETRANSACTIONID { get; set; }
		public DateTime TransactionDate { get; set; }
        public string EPAYCardID { get; set; }
    }

}
