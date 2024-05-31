using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
	public class StoresRedeemEntity
	{
		public Int64 StoresRedeemID { get; set; }
		public Int64 StoreNo { get; set; }
		public string StoreName { get; set; }
		public string Email { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string CityTown { get; set; }
		public string PostCode { get; set; }
		public Int64 UserID { get; set; }
		public string Title { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string UserEmail { get; set; }
		public string StoreNameUser { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? CreatedAt { get; set; }
		public Int64 CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Int64 UpdatedBy { get; set; }
	}
}
