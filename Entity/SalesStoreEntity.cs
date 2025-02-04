using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
	public class SalesStoreEntity
	{
		public Int64 CommisionID { get; set; }
		public string RetailerID { get; set; }
		public string RetailerName { get; set; }
		public string StoreName { get; set; }
		public decimal Commission { get; set; }
		public decimal StripeFee { get; set; }
		public decimal ProcessingFee { get; set; }
		public decimal GGCRedemption { get; set; }
		public decimal TransactionFee { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? CreatedAt { get; set; }
		public Int64 CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Int64 UpdatedBy { get; set; }
	}
}
