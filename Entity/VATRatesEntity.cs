using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
	public class VATRatesEntity
	{
        public Int64 VATRatesID { get; set; }
		public DateTime? VATRateDate { get; set; }
        public decimal VATRate { get; set; }
    }
}
