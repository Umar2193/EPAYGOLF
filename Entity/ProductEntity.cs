using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
	public class ProductEntity
	{
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public Int64 ProductEAN { get; set; }
        public string ProductType { get; set; }
        public decimal Commission { get; set; }
    }
}
