using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
	public class SettingsEntity
	{
		public Int64 SettingID { get; set; }
        public DateTime? YearStartDate { get; set; }
        public DateTime? MonthStartDate { get; set; }
        public string WeekStartDay { get; set; }
        public decimal liabilitypct { get; set; }
        public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? CreatedAt { get; set; }
		public Int64 CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Int64 UpdatedBy { get; set; }
	}
}
